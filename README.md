# Huxley

![Build Status](https://ci.appveyor.com/api/projects/status/github/jpsingleton/huxley?retina=true "Build Status")

## Extremely simple restful JSON proxy for the UK National Rail Live Departure Board [SOAP](http://harmful.cat-v.org/software/xml/soap/simple) [API](http://www.nationalrail.co.uk/46391.aspx) ([Darwin](https://lite.realtime.nationalrail.co.uk/OpenLDBWS/))

You will need to add your access token to the URL for this to work. You can register to obtain one [here](https://realtime.nationalrail.co.uk/OpenLDBWSRegistration/Registration).
Append the accessToken={your token} parameter to the query string for every request.

## Demo
There is an example deployment set up [here](https://huxley.apphb.com/).
(**DO NOT USE THIS FOR ANYTHING SERIOUS!**)

If you want to use this proxy then you should deploy the code to your own [App Harbor](https://appharbor.com/) or [Azure](https://azure.microsoft.com/en-gb/) account.
If you deploy to the App Harbor Europe AWS region then this will run very close to the NRE servers.

SDKs in 9 languages (including Java and Ruby) for this endpoint (generated with [Swagger](https://github.com/swagger-api/swagger-codegen)) are available [here](http://restunited.com/releases/430721415517308710/wrappers). If you use these make sure to change the endpoint for production.

### Departures

[/departures/[Three letter CRS station code]?accessToken=[Your GUID token]](https://huxley.apphb.com/departures/crs?accessToken=)

### Arrivals

[/arrivals/[Three letter CRS station code]?accessToken=[Your GUID token]](https://huxley.apphb.com/arrivals/crs?accessToken=)

### Service

[/service/[Service ID]?accessToken=[Your GUID token]](https://huxley.apphb.com/service/Z/zlpIG8jJacKayAnOXODw==?accessToken=)

The service ID can be found for each service inside the departures and arrivals response.

This endpoint also accepts the [GUID representation of the ID](https://huxley.apphb.com/service/8c105350-4235-44f3-b076-87fe829c577e?accessToken=) as /, + and case sensitivity can cause trouble if you're not careful.

---

Made by [James Singleton](https://unop.uk)

![powered by National Rail Enquiries](src/Huxley/NRE_Powered_logo.png "powered by National Rail Enquiries")
