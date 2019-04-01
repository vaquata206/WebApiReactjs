﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class FeatureService: IFeatureService
    {
        /// <summary>
        /// Feature repository
        /// </summary>
        private IFeatureRepository featureRepository;

        public FeatureService(IFeatureRepository featureRepository)
        {
            this.featureRepository = featureRepository;
        }

        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        public async Task<IEnumerable<Feature>> GetAllAsync()
        {
            var list = await this.featureRepository.GetAllAsync();
            list = this.ConvertToTree(list);
            return list;
        }

        /// <summary>
        /// Save a feature
        /// </summary>
        /// <param name="featureVM">The feature</param>
        /// <returns>A task</returns>
        public async Task SaveFeature(FeatureVM featureVM)
        {
            if (featureVM.ID_CN.HasValue && featureVM.ID_CN.Value != 0)
            {
                // Edit
                var entity = await this.featureRepository.GetFeatureByIdAsync(featureVM.ID_CN.Value);
                if (entity == null)
                {
                    throw new Exception("Chức năng này không tồn tại");
                }

                entity.Action_Name = featureVM.Action_Name;
                entity.Controller_Name = featureVM.Controller_Name;
                entity.Id_ChucNang_Cha = featureVM.ID_CN_PR ?? 0;
                entity.Ten_ChucNang = featureVM.Ten_CN;
                entity.Tooltip = featureVM.ToolTip_CN;
                entity.MoTa_ChucNang = featureVM.Mota_CN;
                entity.Action_Name = featureVM.Action_Name;
                entity.HienThi_Menu = featureVM.HienThi_Menu ? 1 : 0;

                await this.featureRepository.UpdateFeatureAsync(entity);
            }
            else
            {
                await this.featureRepository.AddFeatureAsync(new Feature {
                    Ten_ChucNang = featureVM.Ten_CN,
                    Ma_ChucNang = "CN" + string.Format("{0:yyyyMMddhhmmss}", DateTime.Now),
                    Tooltip = featureVM.ToolTip_CN,
                    MoTa_ChucNang = featureVM.Mota_CN,
                    Controller_Name = featureVM.Controller_Name,
                    Action_Name = featureVM.Action_Name,
                    Id_ChucNang_Cha = featureVM.ID_CN_PR ?? 0,
                    HienThi_Menu = featureVM.HienThi_Menu ? 1 : 0
                });
            }

        }

        /// <summary>
        /// Changes a feature's position
        /// </summary>
        /// <param name="parentId">ParentId of the feature</param>
        /// <param name="order">Numerical order of the feature into its parent</param>
        /// <param name="featureId">The feature ids</param>
        /// <returns>A Task</returns>
        public async Task ChangePosition(int parentId, int order, int featureId)
        {
            var feature = await this.featureRepository.GetFeatureByIdAsync(featureId);

            if (feature == null)
            {
                throw new Exception("Chức năng này không tồn tại");
            }

            if (parentId != 0)
            {
                var parent = await this.featureRepository.GetFeatureByIdAsync(parentId);
                if (parent == null)
                {
                    throw new Exception("Chức năng cha không tồn tại");
                }
            }

            await this.featureRepository.ChangePosition(parentId, order, featureId);
        }

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        public async Task DeleteFeatureAsync(int featureId)
        {
            var feature = await this.featureRepository.GetFeatureByIdAsync(featureId);
            if (feature == null)
            {
                throw new Exception("Chức năng này không tồn tại");
            }

            await this.featureRepository.DeleteFeatureAsync(featureId);
        }

        public async Task<IEnumerable<TreeNode>> GetTreeNodeFeaturesOfAccount(int accountId)
        {
            var result = Enumerable.Empty<TreeNode>();
            var fp = await this.featureRepository.GetFeaturesOfPermissionsByAccountId(accountId);
            var fe = await this.featureRepository.GetFeaturesNotBelongPermissionsByAccountId(accountId);

            // feature list belong the employee's permissions
            result = fp.Select(x => new TreeNode
            {
                Id = x.Id_ChucNang.ToString(),
                TypeNode = "P"
            });

            // feature list expand to the employee
            result = result.Concat(fe.Select(x => new TreeNode
            {
                Id = x.Id_ChucNang.ToString(),
                TypeNode = "E"
            }));

            return result;
        }

        /// <summary>
        /// Get all features of a employee
        /// </summary>
        /// <param name="userId">Employee id</param>
        /// <returns>A feature list</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesUser(int userId)
        {
            var list = await this.featureRepository.GetFeaturesUser(userId);
            return this.ConvertToTree(list);
        }

        /// <summary>
        /// Checks the user is access to feature whose link like the "path"
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="path">The path</param>
        /// <param name="isModeUri">Is mode uri</param>
        /// <returns>true if the user is accessed</returns>
        public async Task<bool> IsAccessedToTheFeature(int userId, string path, bool isModeUri)
        {
            
            string controllerName = string.Empty;
            string actionName = string.Empty;

            var ctPath = path.Split("/");
            controllerName = ctPath[0];
            if (ctPath.Length >=2 && !ctPath[1].Equals("index"))
            {
                actionName = ctPath[1];
            }

            return await this.featureRepository.IsAccessedToTheFeature(userId, controllerName, actionName, isModeUri);
        }

        /// <summary>
        /// Convert a list to tree
        /// </summary>
        /// <param name="list">features</param>
        /// <returns>A tree list</returns>
        private IEnumerable<Feature> ConvertToTree(IEnumerable<Feature> list)
        {
            if (list != null && list.Count() > 0)
            {
                // groups features by Id feature parent
                var groupFeatures = list.GroupBy(x => x.Id_ChucNang_Cha);

                foreach (var gr in groupFeatures)
                {
                    var fr = list.FirstOrDefault(y => y.Id_ChucNang == gr.Key);
                    if (fr != null)
                    {
                        fr.Children = gr.OrderBy(y => y.Thu_Tu);
                    }
                };

                // Gets root features that has ID_CN_PR = 0
                list = groupFeatures.Where(x => x.Key == 0).SelectMany(x => x).OrderBy(x => x.Thu_Tu);
            }

            return list;
        }
    }
}