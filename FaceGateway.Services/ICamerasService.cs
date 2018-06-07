using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface ICamerasService
    {
        Camera GetCamera(Guid camId);
    }
}
