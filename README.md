vbcvb# Huxley

[![Huxley](https://huxley.apphb.com/huxley.png "Huxley")](https://huxley.apphb.com/)

[![Build Status](https://ci.appveyor.com/api/projects/status/github/jpsingleton/huxley?retina=true "Build Status")](https://ci.appveyor.com/project/jpsingleton/huxley)

[![Slack Chat](https://openraildata.herokuapp.com/badge.svg "Slack Chat")](https://openraildata.herokuapp.com)

## UK National Rail Live Departure Boards JSON proxy

Huxley is a [CORS](http://enable-cors.org/) enabled JSON [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) proxy for the UK National Rail Enquires Live Departure Board [SOAP](http://harmful.cat-v.org/software/xml/soap/simple) [API](http://www.nationalrail.co.uk/46391.aspx) (Darwin). 

Huxley is being _actively_ developed. If you want to be informed of updates when they are released then watch the project on GitHub and/or **[follow me on Twitter](https://twitter.com/shutdownscanner)**.

---

[SOAP](http://harmful.cat-v.org/software/xml/soap/simple) is a pain to use (you have to POST specially crafted XML) so this proxy allows you to GET nicely formatted JSON instead ([REST](https://en.wikipedia.org/wiki/Representational_state_transfer)). It also adds [CORS](http://enable-cors.org/) headers so you can access it with JavaScript from a different domain.

Huxley also has a built in CRS code lookup API so you can search for station names from your app. You can also use station names directly in any query. The codes are automatically kept up to date from the official sources.

In addition it has a function for calculating delays which allows you to build useful IoT devices like this [LED strip delay indicator](https://unop.uk/dev/train-disruption-indicator-with-a-blinky-tape-rgb-led-strip-and-raspberry-pi/). You can specify specific trains and it even knows about London Terminals.

[![Train Disruption Indicator](https://unop.uk/wp-content/uploads/2015/05/trains.jpg "Train Disruption Indicator")](https://unop.uk/dev/train-disruption-indicator-with-a-blinky-tape-rgb-led-strip-and-raspberry-pi/)

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

## Access Token

You will need to add your access token to the URL. You can register to obtain one [here](https://realtime.nationalrail.co.uk/OpenLDBWSRegistration/Registration).
Append the `accessToken={Your GUID token}` parameter to the query string for every request.

There is optional support for configuring the access token server side. So you don't need to worry about revealing it.

You can set `DarwinAccessToken` to your NRE access token. If you leave `ClientAccessToken` as an empty GUID then no token is required in the Huxley URL. If you set `ClientAccessToken` to a random GUID and it matches the token in the URL then the `DarwinAccessToken` will be used instead in the SOAP call. Otherwise the URL token is passed straight through. Look in the `Web.config` file for more details.

**N.B.** You should set up these tokens in your deployment platform and not in your source code repository. You'll notice that the values are empty GUIDs by default. The example token used above will only work on the demo server and not directly against the SOAP API.

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

[`/delays/{CRS|StationName}/{filterType}/{filterCRS|StationName}/{numRows}?accessToken={Your GUID token}`](https://huxley.apphb.com/delays/clapham junction/from/london/20?accessToken=)

**Sample Response:**
```javascript
{
  "generatedAt": "2015-05-08T11:28:33.7187169+01:00",
  "locationName": "Clapham Junction",
  "crs": "CLJ",
  "filterLocationName": "London",
  "filtercrs": "LON",
  "delays": true,
  "totalTrainsDelayed": 1,
  "totalDelayMinutes": 16,
  "totalTrains": 12,
  "delayedTrains": [
    {
      "origin": [
        {
          "locationName": "London Waterloo",
          "crs": "WAT",
          "via": null,
          "futureChangeTo": null,
          "assocIsCancelled": false
        }
      ],
      "destination": [
        {
          "locationName": "London Waterloo",
          "crs": "WAT",
          "via": null,
          "futureChangeTo": null,
          "assocIsCancelled": false
        }
      ],
      "currentOrigins": null,
      "currentDestinations": null,
      "sta": null,
      "eta": null,
      "std": "11:20",
      "etd": "11:28",
      "platform": "3",
      "operator": "South West Trains",
      "operatorCode": "SW",
      "isCircularRoute": false,
      "serviceID": "F4GbTDZuLjb4VlXEYDuakg==",
      "adhocAlerts": null
    }
  ]
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
