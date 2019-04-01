using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IFeatureRepository : IBaseRepository<Feature>
    {
        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        Task<IEnumerable<Feature>> GetAllAsync();

        /// <summary>
        /// Gets a feature by id
        /// </summary>
        /// <param name="idFeature">Id of feature</param>
        /// <returns>A feature</returns>
        Task<Feature> GetFeatureByIdAsync(int idFeature);

        /// <summary>
        /// Updates a feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns></returns>
        Task UpdateFeatureAsync(Feature feature);

        /// <summary>
        /// Adds a feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A Task</returns>
        Task AddFeatureAsync(Feature feature);

        /// <summary>
        /// Changes a feature's position
        /// </summary>
        /// <param name="parentId">ParentId of the feature</param>
        /// <param name="order">Numerical order of the feature into its parent</param>
        /// <param name="featureId">The feature ids</param>
        /// <returns>A Task</returns>
        Task ChangePosition(int parentId, int order, int featureId);

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        Task DeleteFeatureAsync(int featureId);

        /// <summary>
        /// Id of the permission
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A list feature</returns>
        Task<IEnumerable<Feature>> GetFeaturesByPermissionId(int permissionId);

        Task<IEnumerable<Feature>> GetFeaturesOfPermissionsByAccountId(int accountId);

        Task<IEnumerable<Feature>> GetFeaturesNotBelongPermissionsByAccountId(int accountId);

        Task<IEnumerable<Feature>> GetFeaturesUser(int employeeId);

        /// <summary>
        /// Checks the user is access to feature whose link like the "path"
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="controlerName">The controller name</param>
        /// <param name="actionName">The action name</param>
        /// <param name="isModeUri">Is mode uri</param>
        /// <returns>true if the user is accessed</returns>
        Task<bool> IsAccessedToTheFeature(int userId, string controlerName, string actionName, bool isModeUri);
    }
}
