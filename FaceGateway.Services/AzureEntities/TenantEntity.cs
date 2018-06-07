using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FaceGateway.Services.AzureEntities
{
    public class TenantEntity : TableEntity
    {
        public TenantEntity() { }

        public TenantEntity(string tenantGroupId)
        {
            PartitionKey = tenantGroupId;
            RowKey = Guid.NewGuid().ToString();
        }

        public string Tenant { get; set; }

        public Guid BranchId { get; set; }
    }
}
