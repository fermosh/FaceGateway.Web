using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGateway.Services.ConcreteServices
{
    public class ImagesService : IImagesService
    {
        public string GetFullImageUrl(string imageName)
        {
            return imageName;
        }
    }
}
