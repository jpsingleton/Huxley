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
using System.Threading.Tasks;
using System.Web.Http;
using Huxley.Models;
using System.Web;

namespace Huxley.Controllers
{
    public class ServiceController : LdbController
    {

        public ServiceController(ILdbClient client)
            : base(client)
        {
        }

        // GET /service/ID?accessToken=[your token]
        public async Task<object> Get([FromUri] ServiceRequest request)
        {
            var token = MakeAccessToken(request.AccessToken);

            // The Darwin API requires service ID to be a standard base 64 string
            // As it's simply a GUID there will always be maximum padding (==)
            // By this point it doesn't matter if it was percent-encoded
            // It will have been decoded and we get the raw characters
            if (request.ServiceId.Length == 24)
            {
                try
                {
                    if (Convert.FromBase64String(request.ServiceId).Length == 16)
                    {
                        var s = await Client.GetServiceDetailsAsync(token, request.ServiceId);
                        return s.GetServiceDetailsResult;
                    }
                }
                catch
                {
                    // Not a base 64 encoded GUID or the API rejected it
                }
            }

            // If ID looks like a RID (15 decimal digit long base 10 integer) then use the staff API
            long rid;
            if (long.TryParse(request.ServiceId, out rid))
            {
                var staffToken = MakeStaffAccessToken(request.AccessToken);
                var staffService = await Client.GetStaffServiceDetailsAsync(staffToken, request.ServiceId);
                return staffService.GetServiceDetailsResult;
            }

            // We also accept the standard hexadecimal (base 16) GUID representation
            Guid sid;
            if (Guid.TryParse(request.ServiceId, out sid))
            {
                request.ServiceId = Convert.ToBase64String(sid.ToByteArray());
            }

            // We support URL safe base 64 encoding as it's more suitable for this situation
            // https://en.wikipedia.org/wiki/Base64#URL_applications
            // https://tools.ietf.org/html/rfc4648#section-5
            // This decoder uses a number suffix for padding so the URLs will also be shorter
            // Other encoders are available as part of ASP.NET Core
            //  - Microsoft.AspNet.WebUtilities.WebEncoders in RC1
            //  - Microsoft.Extensions.WebEncoders for RC2 / 1.0
            // For more info read ASP.NET Core 1.0 High Performance (https://unop.uk/book) :)
            var id = request.ServiceId;
            if (id.Length == 22)
            {
                // IDs always have 2 characters of padding
                id += "2";
            }
            if (id.Length == 23)
            {
                try
                {
                    var sidBytes = HttpServerUtility.UrlTokenDecode(id);
                    if (sidBytes != null && sidBytes.Length == 16)
                    {
                        request.ServiceId = Convert.ToBase64String(sidBytes);
                    }
                }
                catch
                {
                    // Not Base64 URL encoded
                }
            }

            // If ID wasn't percent-encoded then it may be missing / + =
            // We try to fix it up if it isn't the correct length
            while (!request.ServiceId.EndsWith("=="))
            {
                request.ServiceId += "=";
            }
            while (request.ServiceId.Length < 24)
            {
                request.ServiceId = "/" + request.ServiceId;
            }

            var service = await Client.GetServiceDetailsAsync(token, request.ServiceId);
            return service.GetServiceDetailsResult;
        }
    }
}