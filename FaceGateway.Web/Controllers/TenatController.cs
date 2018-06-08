using FaceGateway.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FaceGateway.Web.Controllers
{
    public class TenatController : ApiController
    {
        private readonly IFacesService facesService;

        public TenatController(IFacesService facesService)
        {
            this.facesService = facesService;
        }

        [HttpPost]
        [Route("api/Tenat/{name}")]
        public async Task<IHttpActionResult> CreateTenantAsync([FromUri] string name)
        {
            var groupId = CreateGroupId(name);

            await facesService.CreateTenantAsync(name, groupId);

            return Ok(groupId);
        }

        private string CreateGroupId(string value)
        {
            var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(value));
            var guid = new Guid(data);

            return guid.ToString();
        }
    }
}
