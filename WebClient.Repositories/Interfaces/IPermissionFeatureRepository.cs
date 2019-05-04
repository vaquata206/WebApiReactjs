using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IPermissionFeatureRepository
    {
        Task AddAsync(Permission_Feature permission_Feature);

        Task DeleteAsync(Permission_Feature permission_Feature);

        Task<IEnumerable<Permission_Feature>> GetListsByPermissionIdAsync(int permissionId);
    }
}
