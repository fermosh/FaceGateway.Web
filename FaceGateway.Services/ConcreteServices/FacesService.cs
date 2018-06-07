using FaceGateway.Services.AzureEntities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceGateway.Services.ConcreteServices
{
    public class FacesService : IFacesService
    {
        private string Combine(IEnumerable<string> filters) {
            if (filters.Count() == 1) {
                return filters.First();
            }
            return TableQuery.CombineFilters(filters.First(),TableOperators.Or,Combine(filters.Skip(1)));
        }
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
            if (!faceIds.Any()) return new List<Face>();
            var storageAcct = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AZURE-TABLES-CONN-STR"]);
            var tableClient = storageAcct.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Faces");
            var query = new TableQuery<FaceEntity>();
            var wheres = faceIds.Select( fid => TableQuery.GenerateFilterCondition("FaceId",QueryComparisons.Equal,fid.ToString()) );
            var filter = query.Where(Combine(wheres));

//            return faces.Select(f => new Face { Name = f.Name, TrainingImageFile = f.TrainingImageName });
            return table.ExecuteQuery(filter).Select(f => new Face { Name = f.Name, TrainingImageFile = f.TrainingImageFile });
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
