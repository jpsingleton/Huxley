/*
Huxley - a JSON proxy for the UK National Rail Live Departure Board SOAP API
Copyright (C) 2016 James Singleton
 * https://huxley.unop.uk
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
using Huxley.ldbServiceReference;

namespace Huxley {
    public interface ILdbClient {
        Task<GetDepartureBoardResponse> GetDepartureBoardAsync(AccessToken accessToken, ushort numRows, string crs,
                                                               string filterCrs, FilterType filterType, int timeOffset,
                                                               int timeWindow);

        Task<GetDepBoardWithDetailsResponse> GetDepBoardWithDetailsAsync(AccessToken accessToken, ushort numRows,
                                                                         string crs, string filterCrs,
                                                                         FilterType filterType, int timeOffset,
                                                                         int timeWindow);

        Task<GetArrivalBoardResponse> GetArrivalBoardAsync(AccessToken accessToken, ushort numRows, string crs,
                                                           string filterCrs, FilterType filterType, int timeOffset,
                                                           int timeWindow);

        Task<GetArrBoardWithDetailsResponse> GetArrBoardWithDetailsAsync(AccessToken accessToken, ushort numRows,
                                                                         string crs, string filterCrs,
                                                                         FilterType filterType, int timeOffset,
                                                                         int timeWindow);

        Task<GetArrivalDepartureBoardResponse> GetArrivalDepartureBoardAsync(AccessToken accessToken, ushort numRows,
                                                                             string crs, string filterCrs,
                                                                             FilterType filterType, int timeOffset,
                                                                             int timeWindow);

        Task<GetArrDepBoardWithDetailsResponse> GetArrDepBoardWithDetailsAsync(AccessToken accessToken, ushort numRows,
                                                                               string crs, string filterCrs,
                                                                               FilterType filterType, int timeOffset,
                                                                               int timeWindow);

        Task<GetNextDeparturesResponse> GetNextDeparturesAsync(AccessToken accessToken, string crs, string[] filterList,
                                                               int timeOffset, int timeWindow);

        Task<GetNextDeparturesWithDetailsResponse> GetNextDeparturesWithDetailsAsync(AccessToken accessToken, string crs,
                                                                                     string[] filterList, int timeOffset,
                                                                                     int timeWindow);

        Task<GetFastestDeparturesResponse> GetFastestDeparturesAsync(AccessToken accessToken, string crs,
                                                                                  string[] filterList, int timeOffset,
                                                                                  int timeWindow);

        Task<GetFastestDeparturesWithDetailsResponse> GetFastestDeparturesWithDetailsAsync(AccessToken accessToken,
                                                                                           string crs, string[] filterList,
                                                                                           int timeOffset,
                                                                                           int timeWindow);


        Task<GetServiceDetailsResponse> GetServiceDetailsAsync(AccessToken accessToken, string serviceId);

        Task<ldbStaffServiceReference.GetDepartureBoardByCRSResponse> GetStaffDepartureBoardAsync(ldbStaffServiceReference.AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType);

        Task<ldbStaffServiceReference.GetDepBoardWithDetailsResponse> GetStaffDepartureBoardWithDetailsAsync(ldbStaffServiceReference.AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType);

        Task<ldbStaffServiceReference.GetArrivalBoardByCRSResponse> GetStaffArrivalBoardAsync(ldbStaffServiceReference.AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType);

        Task<ldbStaffServiceReference.GetArrBoardWithDetailsResponse> GetStaffArrivalBoardWithDetailsAsync(ldbStaffServiceReference.AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType);

        Task<ldbStaffServiceReference.GetArrivalDepartureBoardByCRSResponse> GetStaffArrivalDepartureBoardAsync(ldbStaffServiceReference.AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType);

        Task<ldbStaffServiceReference.GetArrDepBoardWithDetailsResponse> GetStaffArrivalDepartureBoardWithDetailsAsync(ldbStaffServiceReference.AccessToken accessToken, ushort numRows, string crs, string filterCrs, FilterType filterType);

        Task<ldbStaffServiceReference.GetNextDeparturesResponse> GetStaffNextDeparturesAsync(ldbStaffServiceReference.AccessToken accessToken, string crs, string[] filterList);

        Task<ldbStaffServiceReference.GetNextDeparturesWithDetailsResponse> GetStaffNextDeparturesWithDetailsAsync(ldbStaffServiceReference.AccessToken accessToken, string crs, string[] filterList);

        Task<ldbStaffServiceReference.GetFastestDeparturesResponse> GetStaffFastestDeparturesAsync(ldbStaffServiceReference.AccessToken accessToken, string crs, string[] filterList);

        Task<ldbStaffServiceReference.GetFastestDeparturesWithDetailsResponse> GetStaffFastestDeparturesWithDetailsAsync(ldbStaffServiceReference.AccessToken accessToken, string crs, string[] filterList);

        Task<ldbStaffServiceReference.GetServiceDetailsByRIDResponse> GetStaffServiceDetailsAsync(ldbStaffServiceReference.AccessToken accessToken, string rid);

    }
}
