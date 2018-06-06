using FaceGateway.Web.Models;
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
        [HttpPost]
        public async Task<IHttpActionResult> Alert(ImageRecognitionMessage message) {

            return Ok(message);
        }
    }
}
