## Meteorological
### An example of Api calls against the SMHI metrological data API
### Code tested and run localy through the swagger interface.

### Design decisions
##### The API endpoints can be found in the Meterological_API project
##### For API discovery i chose to use Swagger rather then Hateoas because I was more familiar with it and it is more widely used.
##### There are two API endpoints one for getting the Wind speeds and one for current temperatures.
##### &nbsp;&nbsp;&nbsp; I created a custom class called WeatherReport to collect the data to present what the user needs and nothing more.
##### &nbsp;&nbsp;&nbsp; The apis are distinquished by a parameter set by an enum so I don't have to use magic numbers.
##### &nbsp;&nbsp;&nbsp; They have two Parameters
##### &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; StationKey (long) that sets what station key you are requesting and defaults to show all when omitted.
##### &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; latestDay (bool) that when true sets the scope of the date to the latestDay and when false or ommited sets to latestHour.
##### &nbsp;&nbsp;&nbsp; They hare both powered by the same service and helper classes to reduce code duplication.
##### Rudamentery UnitTests have been created in the Meterological_Test project to verify that the service is working as intended.

<br/>

### Things I would approve upon if I had more time
##### Add more UnitTests to cover more edge cases and possible failure points.
##### I would unify the Api Endpoints error handling to return more consistent error messages and reduce code duplication.
##### I would convert the long numbers in the WeatherReport class to accual DatTime for better usability.