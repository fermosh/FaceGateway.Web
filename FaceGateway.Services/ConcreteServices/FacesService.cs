using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services.ConcreteServices
{
    public class FacesService : IFacesService
    {
        public IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds)
        {
            
            return faceIds.Select(f=>new Face());
        }
    }
}
