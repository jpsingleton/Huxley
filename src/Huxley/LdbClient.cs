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
using System.ServiceModel;
using System.Threading.Tasks;
using Huxley.ldbServiceReference;

namespace Huxley {
    //TODO inject client and extract try block to common method

    public class LdbClient : ILdbClient {

        private readonly LDBServiceSoapClient client;

        public LdbClient() {
            client = new LDBServiceSoapClient();
        }

        public async Task<GetDepartureBoardResponse> GetDepartureBoardAsync(AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType, int timeOffset, int timeWindow) {
            // Avoiding Problems with the Using Statement in WCF clients
            // https://msdn.microsoft.com/en-us/library/aa355056.aspx
            try {
                return await client.GetDepartureBoardAsync(accessToken, numRows, crs, filterCrs, filterType, timeOffset, timeWindow);
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
            return new GetDepartureBoardResponse();
        }

        public async Task<GetArrivalBoardResponse> GetArrivalBoardAsync(AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType, int timeOffset, int timeWindow) {
            // Avoiding Problems with the Using Statement in WCF clients
            // https://msdn.microsoft.com/en-us/library/aa355056.aspx
            try {
                return await client.GetArrivalBoardAsync(accessToken, numRows, crs, filterCrs, filterType, timeOffset, timeWindow);
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
            return new GetArrivalBoardResponse();
        }

        public async Task<GetArrivalDepartureBoardResponse> GetArrivalDepartureBoardAsync(AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType, int timeOffset, int timeWindow) {
            // Avoiding Problems with the Using Statement in WCF clients
            // https://msdn.microsoft.com/en-us/library/aa355056.aspx
            try {
                return await client.GetArrivalDepartureBoardAsync(accessToken, numRows, crs, filterCrs, filterType, timeOffset, timeWindow);
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
            return new GetArrivalDepartureBoardResponse();
        }

        public async Task<GetServiceDetailsResponse> GetServiceDetailsAsync(AccessToken accessToken, string serviceId) {
            // Avoiding Problems with the Using Statement in WCF clients
            // https://msdn.microsoft.com/en-us/library/aa355056.aspx
            try {
                return await client.GetServiceDetailsAsync(accessToken, serviceId);
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
            return new GetServiceDetailsResponse();
        }

    }
}