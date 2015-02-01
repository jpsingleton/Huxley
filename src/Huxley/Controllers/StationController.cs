/*
Huxley - a JSON proxy for the UK National Rail Live Departure Board SOAP API
Copyright (C) 2015 James Singleton
 * http://huxley.unop.uk
 * https://github.com/jpsingleton/Huxley

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Threading.Tasks;
using System.Web.Http;
using Huxley.Models;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class StationController : BaseController {
        // GET /all/CRS?accessToken=[your token]
        public async Task<StationBoard> Get([FromUri] StationBoardRequest request) {

            var client = new LDBServiceSoapClient();
            var token = MakeAccessToken(request.AccessToken);

            if ("departures" == request.Board) {
                var departures = await client.GetDepartureBoardAsync(token, request.NumRows, request.Crs, request.FilterCrs, request.FilterType, 0, 0);
                return departures.GetStationBoardResult;
            }
            if ("arrivals" == request.Board) {
                var arrivals = await client.GetArrivalBoardAsync(token, request.NumRows, request.Crs, request.FilterCrs, request.FilterType, 0, 0);
                return arrivals.GetStationBoardResult;
            }

            var board = await client.GetArrivalDepartureBoardAsync(token, request.NumRows, request.Crs, request.FilterCrs, request.FilterType, 0, 0);
            return board.GetStationBoardResult;

        }
    }
}