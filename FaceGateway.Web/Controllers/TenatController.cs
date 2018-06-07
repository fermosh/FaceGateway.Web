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

        [HttpPost]
        public async Task CreateTenatAsync([FromUri] string name)
        {
            var groupId = GenerateTenatGroupId(name);

            await facesService.CreateTenatAsync(name, groupId);
        }

        private string GenerateTenatGroupId(string value)
        {
            var sha1 = SHA1.Create();
            var data = sha1.ComputeHash(Encoding.Default.GetBytes(value));
            var id = new Guid(data);

            return id.ToString();
        }
    }
}
