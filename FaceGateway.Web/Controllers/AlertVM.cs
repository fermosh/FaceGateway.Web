using System.Collections.Generic;
using FaceGateway.Services;

namespace FaceGateway.Web.Controllers
{
    internal class AlertVM
    {
        public IEnumerable<Face> Faces { get; set; }
        public string AlertImage { get; set; }
        public Camera Camera { get; set; }
    }
}