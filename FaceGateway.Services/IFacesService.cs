using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface IFacesService
    {
        IEnumerable<Face> GetFaces(IEnumerable<Guid> faceIds);
    }
}
