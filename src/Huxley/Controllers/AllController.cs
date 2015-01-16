using System;
using System.Threading.Tasks;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class AllController : BaseController {
        // GET /all/CRS?accessToken=[your token]
        public async Task<StationBoard> Get(string id, Guid accessToken) {

            var client = new LDBServiceSoapClient();
            var token = MakeAccessToken(accessToken);

            var board = await client.GetArrivalDepartureBoardAsync(token, 42, id.ToUpperInvariant(), null, FilterType.to, 0, 0);
            return board.GetStationBoardResult;

        }
    }
}