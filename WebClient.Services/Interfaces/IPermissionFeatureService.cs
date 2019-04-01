using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Services.Interfaces
{
    public interface IPermissionFeatureService
    {
        Task SetFeaturesForPermissionAsync(IEnumerable<int> featureIds, int permissionId, int idNhanVien);

        Task<IEnumerable<Permission_Feature>> GetPermissionFeaturesByPermissionId(int permissionId);
    }
}
