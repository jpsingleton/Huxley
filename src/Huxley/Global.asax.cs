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
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Huxley {
    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {
            // Makes the JSON easier to read in a browser without installing an extension like JSONview
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            // Stops the backing field names being used instead of the public property names (*Field & PropertyChanged etc.)
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Returns JSON to the browser without needing to add application/json to the accept request header - remove to use XML (becomes the default)
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            // Pass Register into Configure to support attribute routing in the future
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest(object sender, EventArgs e) {
            var application = sender as HttpApplication;
            if (application != null && application.Context != null) {
                application.Context.Response.Headers.Remove("Server");
            }
        }
    }
}