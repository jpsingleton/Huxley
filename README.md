# Huxley

![Huxley](src/Huxley/huxley.png "Huxley")

![Build Status](https://ci.appveyor.com/api/projects/status/github/jpsingleton/huxley?retina=true "Build Status")

## Extremely simple restful JSON proxy for the UK National Rail Live Departure Board [SOAP](http://harmful.cat-v.org/software/xml/soap/simple) [API](http://www.nationalrail.co.uk/46391.aspx) ([Darwin](https://lite.realtime.nationalrail.co.uk/OpenLDBWS/))

You will need to add your access token to the URL for this to work. You can register to obtain one [here](https://realtime.nationalrail.co.uk/OpenLDBWSRegistration/Registration).
Append the `accessToken={Your GUID token}` parameter to the query string for every request.

## Demo
There is an example deployment set up [here](https://huxley.apphb.com/).
(**DO NOT USE THIS FOR ANYTHING SERIOUS!**)

Paste this into your web console <kbd>F12</kbd>:
```javascript
var r = new XMLHttpRequest();
r.open("GET", "https://huxley.apphb.com/all/stp/from/bxs/1?accessToken=DA1C7740-9DA0-11E4-80E6-A920340000B1", true);
r.onreadystatechange = function () {
    if (r.readyState != 4 || r.status != 200) return;
    var resp = JSON.parse(r.response);
    if (resp.trainServices && resp.trainServices.length > 0) {
        alert("The next train to arrive at " + resp.locationName + " from " + resp.filterLocationName + " will get in at " + resp.trainServices[0].sta);
    } else {
        alert("Sorry, no trains from " + resp.filterLocationName + " arriving soon");
    }
};
r.send();
```

If you want to use this proxy then you should deploy the code to your own [App Harbor](https://appharbor.com/) or [Azure](https://azure.microsoft.com/en-gb/) account.
If you deploy to the App Harbor Europe AWS region then this will run very close to the NRE servers.

SDKs in 9 languages (including Java, PHP, Python and Ruby) for this endpoint (generated with [Swagger](https://github.com/swagger-api/swagger-codegen)) are available [here](http://restunited.com/releases/430721415517308710/wrappers). If you use these make sure to change the endpoint for production.

There is an additional Python (v2) [example for a Raspberry Pi and Blinky Tape RGB LED strip](https://github.com/Blinkinlabs/BlinkyTape_Python/blob/master/Huxley_UK_Rail_Station_Delays.py).

### URL Format

The URL format is `{board}/{CRS|StationName}/{filterType}/{filterCRS|StationName}/{numRows}` or `{board}/{CRS|StationName}/{numRows}` where only board and CRS (or a station name) are required. The filter type can be either `to` or `from` (case is not important).

A station name can be used in place of CRS codes if the name matches only one station (or matches one exactly) but case is not important. See the [CRS section](#crs-station-codes) below for more information.

Examples:

* 10 (default value) Arrivals and Departures at Clapham Junction: `/all/clj`
* 15 Arrivals and Departures at Clapham Junction: `/all/clj/15`
* 10 (default value) Departures at Clapham Junction to Waterloo: `/departures/clj/to/wat`
* 15 Arrivals at Clapham Junction from Waterloo: `/arrivals/clj/from/wat/15`
* 10 (default value) Arrivals and Departures at Wandsworth Common to Clapham Junction: `/all/wandsworth common/to/clapham junction`
* 20 Departures at East Croydon to London Victoria: `/departures/east croydon/to/london victoria/20`

### Departures

[`/departures/{Three letter CRS station code}?accessToken={Your GUID token}`](https://huxley.apphb.com/departures/crs?accessToken=)

### Arrivals

[`/arrivals/{Three letter CRS station code}?accessToken={Your GUID token}`](https://huxley.apphb.com/arrivals/crs?accessToken=)

### Departures and Arrivals

[`/all/{Three letter CRS station code}?accessToken={Your GUID token}`](https://huxley.apphb.com/all/crs?accessToken=)

### Service

[`/service/{Service ID}?accessToken={Your GUID token}`](https://huxley.apphb.com/service/Z/zlpIG8jJacKayAnOXODw==?accessToken=)

The service ID can be found for each service inside the departures and arrivals response.

This endpoint also accepts the [GUID representation of the ID](https://huxley.apphb.com/service/8c105350-4235-44f3-b076-87fe829c577e?accessToken=) as `/`, `+` and case sensitivity can cause trouble if you're not careful.
[More information on the wiki](https://github.com/jpsingleton/Huxley/wiki/Train-Service-IDs).

### Delays

The **delays** action performs calculations server side to easily let you know if there are problems on a particular route.

[`/delays/{crs}/{filtertype}/{filtercrs}/{numrows}?accessToken={Your GUID token}`](https://huxley.apphb.com/delays/gtw/to/lon/50?accessToken=)

**Sample Response:**
```javascript
{
  "generatedAt": "2015-04-24T14:59:29.6198809+01:00",
  "locationName": "Gatwick Airport",
  "crs": "GTW",
  "filterLocationName": "London",
  "filtercrs": "LON",
  "delays": true,
  "totalTrainsDelayed": 8,
  "totalDelayMinutes": 4,
  "totalTrains": 20
}
```

This action will accept `lon` or `London` as a filter CRS to find trains going to or coming from any London terminal.

You can also pass in a comma separated list of 24 hour train times to filter on (e.g. `/btn/to/lon/50/0729,0744,0748`).

## CRS Station Codes

CRS (Computer Reservation System) station codes are available from the following endpoint:

[`/crs/{query}`](https://huxley.apphb.com/crs)

If `query` is omitted then all CRS codes are returned along with their respective station names. If `query` is provided then only station names matching it will be returned along with their CRS codes.

Example response for `/crs/oswald`:
```javascript
[
  {
    "stationName": "Church & Oswaldtwistle",
    "crsCode": "CTW"
  },
  {
    "stationName": "Lazonby & Kirkoswald",
    "crsCode": "LZB"
  }
]
```

[More information on the wiki](https://github.com/jpsingleton/Huxley/wiki/CRS-station-codes).

## Hosting Quick Start

To get your own instance of Huxley (on [App Harbor](https://appharbor.com/) or [Azure](https://azure.microsoft.com/en-gb/)) follow the [instructions on the wiki](https://github.com/jpsingleton/Huxley/wiki/Hosting-Quick-Start).

---

Made by [James Singleton](https://unop.uk)

![powered by National Rail Enquiries](src/Huxley/NRE_Powered_logo.png "powered by National Rail Enquiries")

---

© 2015 James Singleton

This program is licensed under the terms of the GNU Affero General Public License. This means that you need to share any changes (even if only running on a public server).

If you would like another license (such as a commercial license with an invoice) then this can be provided. Please get in touch (send an email to jpsingleton at gmail dot com). 

Contains public sector information licensed under the Open Government Licence v3.0.
