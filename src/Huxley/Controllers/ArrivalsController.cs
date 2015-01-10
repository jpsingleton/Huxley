using System;
using System.Threading.Tasks;
using System.Web.Http;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class ArrivalsController : ApiController {
        // GET /arrivals/CRS?accessToken=[your token]
        public async Task<StationBoard> Get(string id, Guid accessToken) {

            var client = new LDBServiceSoapClient();
            var token = new AccessToken { TokenValue = accessToken.ToString() };

            var board = await client.GetArrivalBoardAsync(token, 42, id.ToUpperInvariant(), null, FilterType.to, 0, 0);
            return board.GetStationBoardResult;

        }
    }
}