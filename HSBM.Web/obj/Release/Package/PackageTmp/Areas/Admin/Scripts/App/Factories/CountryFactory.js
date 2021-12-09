angular.module('app').factory('CountryFactory', function ($http) {
    var _Url = '/Admin/Locations';
    return {
        SaveCountry: function (Country, token) {
            return $http({
                url: _Url + "/AddUpdateCountry",
                method: "POST",
                data: { '__RequestVerificationToken': token , countryMaster: Country}
            });
        },
    }
});