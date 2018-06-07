using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface IFacesService
    {
        IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds);

        Task CreateTenantAsync(string name, string groupId);

        Task<Guid> RegisterFaceAsync(string tenantGroupId, FaceModel faceModel);
    }
}
