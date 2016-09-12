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

using System.Web.Http;

namespace Huxley {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            config.Routes.MapHttpRoute("CrsCodesApi", "crs/{query}", new { controller = "Crs", query = RouteParameter.Optional });
            config.Routes.MapHttpRoute("ServiceDetailsApi", "service/{*serviceid}", new { controller = "Service" });
            config.Routes.MapHttpRoute("StationDelaysApi", "delays/{crs}/{filtertype}/{filtercrs}/{numrows}/{std}",
                new {
                    controller = "Delays",
                    std = RouteParameter.Optional,
                });
            config.Routes.MapHttpRoute("StationBoardApiSimple", "{board}/{crs}/{numrows}", new { controller = "Station" });
            config.Routes.MapHttpRoute("StationBoardApi", "{board}/{crs}/{filtertype}/{filtercrs}/{numrows}",
                new {
                    controller = "Station",
                    filtertype = RouteParameter.Optional,
                    filtercrs = RouteParameter.Optional,
                    numrows = RouteParameter.Optional,
                });
        }
    }
}