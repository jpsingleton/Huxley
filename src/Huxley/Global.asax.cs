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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Huxley {
    public class HuxleyApi : HttpApplication {

        // Singleton to store the station name to CRS lookup
        public static IList<CrsRecord> CrsCodes { get; private set; }

        protected void Application_Start() {
            // Makes the JSON easier to read in a browser without installing an extension like JSONview
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            // Stops the backing field names being used instead of the public property names (*Field & PropertyChanged etc.)
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Returns JSON to the browser without needing to add application/json to the accept request header - remove to use XML (becomes the default)
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            // Pass Register into Configure to support attribute routing in the future
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Set the CRS dictionary passing in embedded CRS path
            CrsCodes = GetCrsCodes(Server.MapPath("~/RailReferences.csv")).Result;
        }

        protected void Application_BeginRequest(object sender, EventArgs e) {
            var application = sender as HttpApplication;
            if (application != null && application.Context != null) {
                application.Context.Response.Headers.Remove("Server");
            }
        }

        private static async Task<IList<CrsRecord>> GetCrsCodes(string embeddedCrsPath) {
            List<CrsRecord> codes;

            // NRE
            const string crsUrl = "http://www.nationalrail.co.uk/static/documents/content/station_codes.csv";
            try {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(crsUrl);
                using (var csvReader = new CsvReader(new StreamReader(stream))) {
                    csvReader.Configuration.RegisterClassMap<NreCrsRecordMap>();
                    // Get results as reader can only be enumerated once
                    codes = csvReader.GetRecords<CrsRecord>().ToList();
                }
            } catch {
                codes = new List<CrsRecord>();
            }

            // NaPTAN
            // Part of http://www.dft.gov.uk/NaPTAN/snapshot/NaPTANcsv.zip
            // Contains public sector information licensed under the Open Government Licence v3.0.
            try {
                using (var stream = File.OpenRead(embeddedCrsPath)) {
                    using (var csvReader = new CsvReader(new StreamReader(stream))) {
                        // If no codes yet (error from NRE) then all are selected
                        // Otherwise only missing entries are added to the list
                        codes.AddRange(csvReader.GetRecords<CrsRecord>().Where(c =>
                            codes.All(code => code.CrsCode != c.CrsCode)).Select(c =>
                                new CrsRecord {
                                    StationName = c.StationName.Replace("Rail Station", "").Trim(),
                                    CrsCode = c.CrsCode,
                                }));
                    }
                }
                // ReSharper disable EmptyGeneralCatchClause
            } catch {
                // If this doesn't work continue to start up
                // ReSharper restore EmptyGeneralCatchClause
            }

            return codes;
        }
    }
}