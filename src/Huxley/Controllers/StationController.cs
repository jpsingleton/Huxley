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

using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Huxley.Models;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class StationController : LdbController {

        public StationController(ILdbClient client)
            : base(client) {
        }

        // GET /{board}/CRS?accessToken=[your token]
        public async Task<BaseStationBoard> Get([FromUri] StationBoardRequest request) {

            // Lookup CRS codes from station names
            var crs = MakeCrsCode(request.Crs);
            var filterList = new[] { "" };
            if (!string.IsNullOrWhiteSpace(request.FilterCrs)) {
                filterList = request.FilterCrs.Split(',').Select(MakeCrsCode).ToArray();
            }

            var token = MakeAccessToken(request.AccessToken);

            if (Board.Departures == request.Board) {
                if (request.Expand) {
                    var departuresWithDetails = await Client.GetDepBoardWithDetailsAsync(token, request.NumRows, crs, filterList[0], request.FilterType, 0, 0);
                    return departuresWithDetails.GetStationBoardResult;
                }
                var departures = await Client.GetDepartureBoardAsync(token, request.NumRows, crs, filterList[0], request.FilterType, 0, 0);
                return departures.GetStationBoardResult;
            }

            if (Board.Arrivals == request.Board) {
                if (request.Expand) {
                    var arrivalsWithDetails = await Client.GetArrBoardWithDetailsAsync(token, request.NumRows, crs, filterList[0], request.FilterType, 0, 0);
                    return arrivalsWithDetails.GetStationBoardResult;
                }
                var arrivals = await Client.GetArrivalBoardAsync(token, request.NumRows, crs, filterList[0], request.FilterType, 0, 0);
                return arrivals.GetStationBoardResult;
            }

            if (Board.Next == request.Board) {
                if (request.Expand) {
                    var nextWithDetails = await Client.GetNextDeparturesWithDetailsAsync(token, crs, filterList, 0, 0);
                    return nextWithDetails.DeparturesBoard;
                }
                var next = await Client.GetNextDeparturesAsync(token, crs, filterList, 0, 0);
                return next.DeparturesBoard;
            }

            if (Board.Fastest == request.Board) {
                if (request.Expand) {
                    var nextWithDetails = await Client.GetFastestDeparturesWithDetailsAsync(token, crs, filterList, 0, 0);
                    return nextWithDetails.DeparturesBoard;
                }
                var next = await Client.GetFastestDeparturesAsync(token, crs, filterList, 0, 0);
                return next.DeparturesBoard;
            }

            // Default all (departures and arrivals board)
            if (request.Expand) {
                var boardWithDetails = await Client.GetArrDepBoardWithDetailsAsync(token, request.NumRows, crs, filterList[0], request.FilterType, 0, 0);
                return boardWithDetails.GetStationBoardResult;
            }
            var board = await Client.GetArrivalDepartureBoardAsync(token, request.NumRows, crs, filterList[0], request.FilterType, 0, 0);
            return board.GetStationBoardResult;
        }
    }
}