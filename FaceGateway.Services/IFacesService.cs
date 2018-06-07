using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface IFacesService
    {
        IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds);

        Task CreateTenatAsync(string name, string groupId);

        Task<Guid> RegisterFaceAsync(string tenatGroupId, string faceName);

        Task AddFaceAsync(string tenatGroupId, Guid faceId, Stream stream);
    }
}
