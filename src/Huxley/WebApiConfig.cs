using System.Web.Http;

namespace Huxley {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{*id}");
        }
    }
}