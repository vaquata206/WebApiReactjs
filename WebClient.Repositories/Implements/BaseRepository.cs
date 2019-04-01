using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core;
using WebClient.Core.Helper;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : class
    {

        private IDbConnection dbConnection;

        public IDbConnection DbConnection { get
            {
                if (dbConnection == null)
                {
                    this.dbConnection = new OracleConnection(WebConfig.ConnectionString);
                }

                return dbConnection;
            }
        }

        public void Dispose()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Dispose();
            }
        }

        public async Task<TEntity> GetByIdAsync(int id, bool checkTinhTrang = true)
        {
            try
            {
                var tableName = this.GetTableName();

                var propertyId = typeof(TEntity).GetProperties().FirstOrDefault(x => Attribute.IsDefined(x, typeof(ExplicitKeyAttribute)));

                var query = string.Format("select * from {0} where {1} = :id", tableName, propertyId.Name);
                if (checkTinhTrang)
                {
                    query += " AND tinh_trang = 1";
                }

                return await this.DbConnection.QueryFirstOrDefaultAsync<TEntity>(query, param: new
                {
                    id = id
                }, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity, IDbTransaction transaction = null)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                var properties = typeof(TEntity).GetProperties();
                var pars = new List<string>();
                var returns = new List<string>();
                foreach(var p in properties)
                {
                    if (Attribute.IsDefined(p, typeof(ComputedAttribute)))
                    {
                        continue;
                    }

                    if (Attribute.IsDefined(p, typeof(ExplicitKeyAttribute)) || Attribute.IsDefined(p, typeof(ReturningAttribute)))
                    {
                        returns.Add(p.Name);

                        var oracleType = this.GenerateOracleType(p);
                        if (oracleType == OracleDbType.Varchar2)
                        {
                            dyParam.Add(p.Name, oracleType, ParameterDirection.Output, size: 20);
                        }
                        else
                        {
                            dyParam.Add(p.Name, oracleType, ParameterDirection.Output);
                        }

                        continue;
                    }

                    var val = GetPropValue(entity, p.Name);
                    if (val != null)
                    {
                        pars.Add(p.Name);

                        dyParam.Add(p.Name, this.GenerateOracleType(p), ParameterDirection.Input, val);
                    }
                }

                var sql = @"insert into {0}({1})values({2}){3}";
                sql = string.Format(sql,
                    this.GetTableName(),
                    string.Join(',', pars),
                    string.Join(',', pars.Select(x => ":" + x)),
                    returns.Count == 0 ? "" : " returning " + string.Join(", ", returns) + " into " + string.Join(", ", returns.Select(x => ":" + x)));

                if (transaction == null)
                {
                    await this.DbConnection.ExecuteAsync(sql, param: dyParam, commandType: CommandType.Text);
                }
                else
                {
                    await transaction.Connection.ExecuteAsync(sql, param: dyParam, transaction: transaction, commandType: CommandType.Text);
                }

                foreach (var i in returns)
                {
                    var oracleParam = dyParam.GetByName(i);
                    if (oracleParam.DbType == DbType.Int64 || oracleParam.DbType == DbType.Int32 || oracleParam.DbType == DbType.Int16)
                    {
                        typeof(TEntity).GetProperty(i).SetValue(entity, int.Parse(oracleParam.Value.ToString()));
                    }
                    else
                    {
                        typeof(TEntity).GetProperty(i).SetValue(entity, oracleParam.Value.ToString());
                    }
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                await DbConnection.DeleteAsync(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(TEntity entity, IDbTransaction transaction = null)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                var properties = typeof(TEntity).GetProperties();
                var pars = new List<string>();
                var idName = "";
                foreach (var p in properties)
                {
                    if (Attribute.IsDefined(p, typeof(ComputedAttribute)))
                    {
                        continue;
                    }

                    var val = GetPropValue(entity, p.Name);
                    if (val != null)
                    {
                        if (!Attribute.IsDefined(p, typeof(ExplicitKeyAttribute)))
                        {
                            pars.Add(p.Name + "=:" + p.Name);
                        }
                        else
                        {
                            idName = p.Name;
                        }
                        dyParam.Add(p.Name, this.GenerateOracleType(p), ParameterDirection.Input, val);
                    }
                    else
                    {
                        pars.Add(p.Name + "=null");
                    }
                }

                var sql = "update {0} set {1} where {2}";
                sql = string.Format(sql, this.GetTableName(), string.Join(',', pars), idName + "=:" + idName);

                if (transaction == null)
                {
                    await this.DbConnection.ExecuteAsync(sql, param: dyParam, commandType: CommandType.Text);
                }
                else
                {
                    await this.DbConnection.ExecuteAsync(sql, param: dyParam, commandType: CommandType.Text, transaction: transaction);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static object DictionaryToObject(IDictionary<String, object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            return (object)expandoObj;
        }

        private string GetTableName()
        {
            var table = (TableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(TableAttribute));
            return (table == null) ? typeof(TEntity).Name : table.Name;
        }

        private object GetPropValue(TEntity src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private OracleDbType GenerateOracleType(PropertyInfo p)
        {
            OracleDbType dbType = OracleDbType.Varchar2;
            if (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?) || p.PropertyType == typeof(long) || p.PropertyType == typeof(long?))
            {
                dbType = OracleDbType.Int64;
            }
            else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
            {
                dbType = OracleDbType.Date;
            }

            return dbType;
        }
    }
}
