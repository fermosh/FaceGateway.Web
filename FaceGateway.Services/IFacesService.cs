using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface IFacesService
    {
        Task<IEnumerable<Face>> GetFacesAsync(IEnumerable<Guid> faceIds);

        Task CreateTenantAsync(string name, string groupId);

        Task<Guid> RegisterFaceAsync(string tenantGroupId, FaceModel faceModel);
    }
}
