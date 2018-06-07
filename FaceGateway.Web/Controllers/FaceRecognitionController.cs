using FaceGateway.Services;
using FaceGateway.Web.Hubs;
using FaceGateway.Web.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FaceGateway.Web.Controllers
{
    public class FaceRecognitionController : ApiController
    {
        private IFacesService facesService;
        private IImagesService imagesService;
        private ICamerasService camerasService;

        public FaceRecognitionController(IFacesService facesService, IImagesService imagesService, ICamerasService camerasService) {
            this.facesService = facesService;
            this.imagesService = imagesService;
            this.camerasService = camerasService;
        }
        [HttpPost]
        public async Task<IHttpActionResult> Alert(ImageRecognitionMessage message) {
           var hub  = GlobalHost.ConnectionManager.GetHubContext<AlertHub>();
            var faces = facesService.GetFaces(message.FaceIds);
            var alertImage = imagesService.GetFullImageUrl(message.FileName);
            var camera = camerasService.GetCamera(message.CamId);
            var alertVM = new AlertVM { Faces = faces , AlertImage = alertImage, Camera = camera };
            hub.Clients.All.handleAlert(JsonConvert.SerializeObject(alertVM));
            return Ok(message);
        }
    }
}
