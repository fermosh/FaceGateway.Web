using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

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

        public void SetPartitionKey(Guid tenantId) => PartitionKey = tenantId.ToString();
        public void SetRowKey() => RowKey = Guid.NewGuid().ToString();
        public Guid FaceId { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> TrainingImageFiles { get; set; }
    }
}
