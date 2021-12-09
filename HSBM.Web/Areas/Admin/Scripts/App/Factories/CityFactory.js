angular.module('app').factory('CityFactory', function ($http) {
    var _Url = '/Admin/Locations';
    return {
        GetAllCountry: function () {
            return $http({
                url: _Url + "/GetCountryList",
                method: "GET",
            });
        },
        GetAllRegion: function (CountryID) {
            return $http({
                url: _Url + "/GetRegionList",
                method: "POST",
                data: { Id: CountryID }
            });
        },
    }
});