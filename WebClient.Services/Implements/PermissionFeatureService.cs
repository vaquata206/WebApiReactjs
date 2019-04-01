using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class PermissionFeatureService : IPermissionFeatureService
    {
        private IPermissionFeatureRepository permissionFeatureRepository;

        public PermissionFeatureService(IPermissionFeatureRepository permissionFeatureRepository)
        {
            this.permissionFeatureRepository = permissionFeatureRepository;
        }

        public async Task SetFeaturesForPermissionAsync(IEnumerable<int> featureIds, int permissionId, int idNhanVien)
        {
            var oldFeatures = await this.permissionFeatureRepository.GetListsByPermissionIdAsync(permissionId);

            var currentDate = DateTime.Now;
            var newFeatures = featureIds.Where(x => oldFeatures.All(y => y.Id_ChucNang != x)).Select(x => new Permission_Feature {
                Id_ChucNang = x,
                Id_Quyen = permissionId,
                Ngay_KhoiTao = currentDate,
                Id_NV_KhoiTao = idNhanVien
            });
            var removedFeatures = oldFeatures.Where(x => featureIds.All(y => y != x.Id_ChucNang));

            foreach(var i in removedFeatures)
            {
                await this.permissionFeatureRepository.DeleteAsync(i);
            }

            foreach(var i in newFeatures)
            {
                await this.permissionFeatureRepository.AddAsync(i);
            }
        }

        public async Task<IEnumerable<Permission_Feature>> GetPermissionFeaturesByPermissionId(int permissionId)
        {
            return await this.permissionFeatureRepository.GetListsByPermissionIdAsync(permissionId);
        }
    }
}
