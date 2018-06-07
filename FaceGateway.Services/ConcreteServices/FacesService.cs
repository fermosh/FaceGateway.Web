using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceGateway.Services.ConcreteServices
{
    public class FacesService : IFacesService
    {
        private FaceServiceClient faceServiceClient = new FaceServiceClient("de34d99901514f93b29181e97059be03", "https://eastus.api.cognitive.microsoft.com/face/v1.0");

        public async Task CreateTenantAsync(string name, string groupId)
        {
            await faceServiceClient.CreatePersonGroupAsync(groupId, name);
        }

        public async Task<Guid> RegisterFaceAsync(string tenatGroupId, string faceName)
        {
            var result = await faceServiceClient.CreatePersonAsync(tenatGroupId, faceName);

            return result.PersonId;
        }

        public async Task AddFaceAsync(string tenantGroupId, Guid faceId, Stream stream)
        {
            await faceServiceClient.AddPersonFaceAsync(tenantGroupId, faceId, stream);
        }

        public IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds)
        {
            
            return faceIds.Select(f=>new Face());
        }

        public async Task TrainAsync(string tenantGroupId)
        {
            await faceServiceClient.TrainPersonGroupAsync(tenantGroupId);

            TrainingStatus trainingStatus = null;

            while (true)
            {
                trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(tenantGroupId);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(100);
            }
        }
    }
}
