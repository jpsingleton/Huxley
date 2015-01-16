using System;
using System.Configuration;
using System.Web.Http;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class BaseController : ApiController {
        protected static AccessToken MakeAccessToken(Guid accessToken) {
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
            return token;
        }

    }
}
