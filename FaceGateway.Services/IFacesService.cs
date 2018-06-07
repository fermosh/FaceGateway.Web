using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface IFacesService
    {
        IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds);

        Task CreateTenantAsync(string name, string groupId);

        Task<Guid> RegisterFaceAsync(string tenantGroupId, string faceName);

        Task AddFaceAsync(string tenantGroupId, Guid faceId, Stream stream);

        Task TrainAsync(string tenantGroupId);
    }
}
