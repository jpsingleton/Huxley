using System;

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
    }
}