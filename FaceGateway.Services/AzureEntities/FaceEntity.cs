using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FaceGateway.Services.AzureEntities
{

    public class FaceEntity : TableEntity
    {
        public FaceEntity() { }

        public FaceEntity(string tenantGroupId)
        {
            PartitionKey = tenantGroupId;
            RowKey = Guid.NewGuid().ToString();
        }

        public Guid FaceId { get; set; }

        public string Name { get; set; }

        public string TrainingImageFile { get; set; }
    }
}
