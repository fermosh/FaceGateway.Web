using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services.ConcreteServices
{
    public class CamerasService : ICamerasService
    {
        public Camera GetCamera(Guid camId)
        {
            return new Camera();
        }
    }
}
