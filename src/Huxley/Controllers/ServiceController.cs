using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class ServiceController : ApiController {
        // GET /service/ID?accessToken=[your token]
        public async Task<ServiceDetails> Get(string id, Guid accessToken) {

            Guid sid;
            if (Guid.TryParse(id, out sid)) {
                id = Convert.ToBase64String(sid.ToByteArray());
            }

            var client = new LDBServiceSoapClient();
            var darwinAccessToken = ConfigurationManager.AppSettings["DarwinAccessToken"];
            var clientAccessToken = ConfigurationManager.AppSettings["ClientAccessToken"];
            Guid dat;
            Guid cat;
            if (Guid.TryParse(darwinAccessToken, out dat) &&
                Guid.TryParse(clientAccessToken, out cat) &&
                cat == accessToken) {
                accessToken = dat;
            }
            var token = new AccessToken { TokenValue = accessToken.ToString() };

            var service = await client.GetServiceDetailsAsync(token, id);
            return service.GetServiceDetailsResult;

        }
    }
}