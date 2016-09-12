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
using Huxley.ldbServiceReference;

namespace Huxley.Models {
    public class DelaysResponse {
        public DateTime GeneratedAt { get; set; }
        public string LocationName { get; set; }
        public string Crs { get; set; }
        public string FilterLocationName { get; set; }
        // Yes this is a typo but it matches StationBoard
        public string Filtercrs { get; set; }
        public bool Delays { get; set; }
        public int TotalTrainsDelayed { get; set; }
        public int TotalDelayMinutes { get; set; }
        public int TotalTrains { get; set; }
        public IEnumerable<ServiceItem> DelayedTrains { get; set; }
    }
}