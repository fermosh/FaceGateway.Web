﻿using FaceGateway.Services;
using FaceGateway.Web.Hubs;
using FaceGateway.Web.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace FaceGateway.Web.Controllers
{
    public class FaceRecognitionController : ApiController
    {
        private IFacesService facesService;
        private IImagesService imagesService;
        private ICamerasService camerasService;
        private string tenatGroupId;

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

        [HttpPost]
        public async Task<IHttpActionResult> RegisterFaceAsync([FromBody]ImageModel model)
        {
            var faceId = await facesService.RegisterFaceAsync(tenatGroupId, model.Name);
            var stream = new MemoryStream(model.Content);
            await facesService.AddFaceAsync(tenatGroupId, faceId, stream);

            return Ok();
        }
    }
}
