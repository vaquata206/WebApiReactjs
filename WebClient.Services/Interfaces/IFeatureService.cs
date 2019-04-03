using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    /// <summary>
    /// Feature service
    /// </summary>
    public interface IFeatureService
    {
        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        Task<IEnumerable<Feature>> GetAllAsync();

        /// <summary>
        /// Save a feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A task</returns>
        Task SaveFeature(FeatureVM feature);

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

        Task<IEnumerable<TreeNode>> GetTreeNodeFeaturesOfAccount(int accountId);

        /// <summary>
        /// Get all features of a employee
        /// </summary>
        /// <param name="accountId">Employee id</param>
        /// <returns>A feature list</returns>
        Task<IEnumerable<Feature>> GetFeaturesUser(int accountId);

        /// <summary>
        /// Checks the user is access to feature whose link like the "path"
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="path">The path</param>
        /// <param name="isModeUri">Is mode uri</param>
        /// <returns>true if the user is accessed</returns>
        Task<bool> IsAccessedToTheFeature(int userId, string path, bool isModeUri);

        /// <summary>
        /// Convert a tree features to array menus
        /// </summary>
        /// <param name="features">The features</param>
        /// <returns>A menu list</returns>
        IEnumerable<Menu> TreeFeaturesToMenu(IEnumerable<Feature> features);
    }   
}
