using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services.AzureEntities
{
   
    public class FaceEntity : TableEntity
    {
        public void SetPartitionKey() {
            this.PartitionKey = Guid.NewGuid().ToString();
        }
        public void SetRowKey() => RowKey = Guid.NewGuid().ToString();
        public Guid FaceId { get; set; }
        public string Name { get; set; }
        public string TrainingImageFile { get; set; }
    }
}
