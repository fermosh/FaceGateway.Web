using FaceGateway.Services.AzureEntities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
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
    }
}
