using FaceGateway.Services.AzureEntities;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AZURE-TABLES-CONN-STR"]);

        public async Task CreateTenantAsync(string name, string groupId)
        {
            await faceServiceClient.CreatePersonGroupAsync(groupId, name);
            RegisterTenant(name, groupId);
        }

        private async Task<string> UploadBlobAsync(Stream stream)
        {
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("face-training-tray");
            await cloudBlobContainer.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            await cloudBlobContainer.SetPermissionsAsync(permissions);

            var blobName = $"{Guid.NewGuid()}.jpg";
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            cloudBlockBlob.Properties.ContentType = @"image\jpeg";
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return blobName;
        }

        public async Task<Guid> RegisterFaceAsync(string tenantGroupId, FaceModel faceModel)
        {
            var result = await faceServiceClient.CreatePersonAsync(tenantGroupId, faceModel.Name);
            var imageNames = new ConcurrentBag<string>();

            var tasks = faceModel.Base64Images.Select(async (image) =>
            {
                var faceData = Convert.FromBase64String(image);
                await AddFaceAsync(tenantGroupId, result.PersonId, GetStream(faceData));
                var blobName = await UploadBlobAsync(GetStream(faceData));

                imageNames.Add(blobName);
            });

            await Task.WhenAll(tasks);

            var associateFaceTask = AssociateFaceToTenant(tenantGroupId, result.PersonId, faceModel.Name, imageNames.ToArray());

            await TrainAsync(tenantGroupId);

            return result.PersonId;
        } 

        private async Task AddFaceAsync(string tenantGroupId, Guid faceId, Stream stream)
        {
            await faceServiceClient.AddPersonFaceAsync(tenantGroupId, faceId, stream);
        }

        private async Task AssociateFaceToTenant(string tenantGroupId, Guid faceId, string name, IEnumerable<string> imageNames)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Faces");
            var faceEntity = new FaceEntity(tenantGroupId)
            {
                Name = name,
                FaceId = faceId,
                TrainingImageFiles = JsonConvert.SerializeObject(imageNames)
            };
            var insertOperation = TableOperation.Insert(faceEntity);

           await Task.Run(() => table.Execute(insertOperation));
        }

        private void RegisterTenant(string name, string id)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Tenants");
            var tenantEntity = new TenantEntity(id)
            {
                Tenant = name
            };
            var insertOperation = TableOperation.Insert(tenantEntity);

            table.Execute(insertOperation);
        }

        public async Task<IEnumerable<Face>> GetFacesAsync(IEnumerable<Guid> faceIds)
        {
            if (!faceIds.Any()) return new List<Face>();
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Faces");
            var query = new TableQuery<FaceEntity>();
            var wheres = faceIds.Select( fid => TableQuery.GenerateFilterCondition("FaceId",QueryComparisons.Equal,fid.ToString()) );
            var filter = query.Where(Combine(wheres));
            return await Task.Run(() =>
           {
               return table.ExecuteQuery(filter).Select(f => new Face { Name = f.Name, TrainingImageFiles = JsonConvert.DeserializeObject<IEnumerable<string>>(f.TrainingImageFiles)});
           });
        }

        private async Task TrainAsync(string tenantGroupId)
        {
            await faceServiceClient.TrainPersonGroupAsync(tenantGroupId);

            var trainingStatus = new TrainingStatus { Status = Status.Running };

            while (trainingStatus.Status == Status.Running)
            {
                trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(tenantGroupId);

                await Task.Delay(1000);
            }
        }

        private static Stream GetStream(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);

            return stream;
        }
    }
}
