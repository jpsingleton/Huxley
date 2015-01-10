# Huxley

![Build Status](https://ci.appveyor.com/api/projects/status/github/jpsingleton/huxley?retina=true "Build Status")

## A restful JSON proxy for the UK National Rail Live Departure Board [SOAP](http://harmful.cat-v.org/software/xml/soap/simple) [API](http://www.nationalrail.co.uk/46391.aspx) ([Darwin](https://lite.realtime.nationalrail.co.uk/OpenLDBWS/))

You will need to add your access token to the URL for this to work. You can register to obtain one [here](https://realtime.nationalrail.co.uk/OpenLDBWSRegistration/Registration).

Append the accessToken={your token} parameter to the query string for every request.

### Departures

/departures/[Three letter CRS station code]?accessToken=[Your GUID token]

### Arrivals

/arrivals/[Three letter CRS station code]?accessToken=[Your GUID token]

### Service

/service/[Service ID]?accessToken=[Your GUID token]

The service ID can be found for each service inside the departures and arrivals response.

This endpoint also accepts the GUID representation of the ID as /, + and case sensitivity can cause trouble if you're not careful.
