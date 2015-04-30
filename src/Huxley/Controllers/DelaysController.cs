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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;
using Huxley.Models;
using Huxley.ldbServiceReference;

namespace Huxley.Controllers {
    public class DelaysController : BaseController {
        // GET /delays/{crs}/{filtertype}/{filtercrs}/{numrows}/{stds}?accessToken=[your token]
        public async Task<DelaysResponse> Get([FromUri] StationBoardRequest request) {

            // Parse the list of comma separated STDs if provided (e.g. /btn/to/lon/50/0729,0744,0748)
            var stds = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.Std)) {
                var potentialStds = request.Std.Split(',');
                var ukNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                var dontRequest = 0;
                foreach (var potentialStd in potentialStds) {
                    DateTime requestStd;
                    // Parse the STD in 24-hour format (with no colon)
                    if (!DateTime.TryParseExact(potentialStd, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out requestStd)) {
                        continue;
                    }
                    stds.Add(potentialStd);
                    var diff = requestStd.Subtract(ukNow);
                    if (diff.TotalHours > 2 || diff.TotalHours < -1) {
                        dontRequest++;
                    }
                }
                // Don't make a request if all trains are more than 2 hours in the future or more than 1 hour in the past
                if (stds.Count > 0 && stds.Count == dontRequest) {
                    return new DelaysResponse();
                }
            }

            var londonTerminals = new List<string> { "BFR", "LBG", "CST", "CHX", "EUS", "FST", "KGX", "LST", "MYB", "PAD", "STP", "SPX", "VIC", "WAT", "WAE", };

            var client = new LDBServiceSoapClient();

            // Avoiding Problems with the Using Statement in WCF clients
            // https://msdn.microsoft.com/en-us/library/aa355056.aspx
            try {
                var totalDelayMinutes = 0;
                var totalTrainsDelayed = 0;

                dynamic config = new Formo.Configuration();
                int delayMinutesThreshold = config.DelayMinutesThreshold<int>(5);

                var token = MakeAccessToken(request.AccessToken);

                var filterCrs = request.FilterCrs;
                if (request.FilterCrs.Equals("LON", StringComparison.InvariantCultureIgnoreCase) ||
                    request.FilterCrs.Equals("London", StringComparison.InvariantCultureIgnoreCase)) {
                    filterCrs = null;
                }

                var board = await client.GetDepartureBoardAsync(token, request.NumRows, request.Crs, filterCrs, request.FilterType, 0, 0);

                var response = board.GetStationBoardResult;
                var filterLocationName = response.filterLocationName;

                var trainServices = response.trainServices;
                if (null == filterCrs) {
                    // This only finds trains terminating at London terminals. BFR/STP etc. won't be picked up if called at en-route.
                    // Could query for every terminal or get service for every train and check calling points. Very chatty either way.
                    switch (request.FilterType) {
                        case FilterType.to:
                            trainServices = trainServices.Where(ts => ts.destination.Any(d => londonTerminals.Contains(d.crs.ToUpperInvariant()))).ToArray();
                            break;
                        case FilterType.from:
                            trainServices = trainServices.Where(ts => ts.origin.Any(d => londonTerminals.Contains(d.crs.ToUpperInvariant()))).ToArray();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    filterCrs = "LON";
                    filterLocationName = "London";
                }

                // If STDs are provided then select only the train(s) matching them
                if (stds.Count > 0) {
                    trainServices = trainServices.Where(ts => stds.Contains(ts.std.Replace(":", ""))).ToArray();
                }

                // Parse the response from the web service.
                foreach (var si in trainServices.Where(si => !si.etd.Equals("On time", StringComparison.InvariantCultureIgnoreCase))) {
                    if (si.etd.Equals("Delayed", StringComparison.InvariantCultureIgnoreCase) ||
                        si.etd.Equals("Cancelled", StringComparison.InvariantCultureIgnoreCase)) {
                        totalTrainsDelayed++;
                    } else {
                        DateTime etd;
                        // Could be "Starts Here", "No Report" or contain a * (report overdue)
                        if (DateTime.TryParse(si.etd.Replace("*", ""), out etd)) {
                            DateTime std;
                            if (DateTime.TryParse(si.std, out std)) {
                                var late = etd.Subtract(std);
                                totalDelayMinutes += (int)late.TotalMinutes;
                                if (late.TotalMinutes > delayMinutesThreshold) {
                                    totalTrainsDelayed++;
                                }
                            }
                        }
                    }
                }

                return new DelaysResponse {
                    GeneratedAt = response.generatedAt,
                    Crs = response.crs,
                    LocationName = response.locationName,
                    Filtercrs = filterCrs,
                    FilterLocationName = filterLocationName,
                    Delays = totalTrainsDelayed > 0,
                    TotalTrainsDelayed = totalTrainsDelayed,
                    TotalDelayMinutes = totalDelayMinutes,
                    TotalTrains = trainServices.Length,
                };

            } catch (CommunicationException) {
                client.Abort();
            } catch (TimeoutException) {
                client.Abort();
            } catch (Exception) {
                client.Abort();
                throw;
            } finally {
                client.Close();
            }
            return new DelaysResponse();
        }
    }
}