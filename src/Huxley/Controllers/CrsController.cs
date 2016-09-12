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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Huxley.Controllers {
    public class CrsController : ApiController {
        // GET /crs
        public IEnumerable<CrsRecord> Get() {
            return HuxleyApi.CrsCodes;
        }

        // GET /crs/{query}
        public IEnumerable<CrsRecord> Get(string query) {
            if (query.Equals("London Terminals", StringComparison.InvariantCultureIgnoreCase)) {
                return HuxleyApi.LondonTerminals;
            }
            // Could use a RegEx here but putting user input into a RegEx can be dangerous
            var results = HuxleyApi.CrsCodes.Where(c => c.StationName.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0);
            return results;
        }
    }
}
