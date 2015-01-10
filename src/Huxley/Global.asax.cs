using System;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace Huxley {
    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }

        protected void Application_BeginRequest(object sender, EventArgs e) {
            var application = sender as HttpApplication;
            if (application != null && application.Context != null) {
                application.Context.Response.Headers.Remove("Server");
            }
        }
    }
}