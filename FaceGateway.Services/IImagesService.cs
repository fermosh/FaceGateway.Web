using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services
{
    public interface IImagesService
    {
        string GetFullImageUrl(string imageName);
    }
}
