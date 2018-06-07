using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public class FacesService : IFacesService
    {
        private FaceServiceClient faceServiceClient = new FaceServiceClient("de34d99901514f93b29181e97059be03", "https://eastus.api.cognitive.microsoft.com/face/v1.0");

        public async Task CreateTenatAsync(string name, string groupId)
        {
            await faceServiceClient.CreatePersonGroupAsync(groupId, name);
        }

        public async Task<Guid> RegisterFaceAsync(string tenatGroupId, string faceName)
        {
            var result = await faceServiceClient.CreatePersonAsync(tenatGroupId, faceName);

            return result.PersonId;
        }

        public IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds)
        {
            throw new NotImplementedException();
        }
    }
}
