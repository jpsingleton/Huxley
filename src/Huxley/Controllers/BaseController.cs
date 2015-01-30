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
