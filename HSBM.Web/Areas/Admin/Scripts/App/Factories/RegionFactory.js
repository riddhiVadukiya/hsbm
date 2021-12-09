angular.module('app').factory('RegionFactory', function ($http) {
    var _Url = '/Admin/Locations';
    return {
        GetAllCountry: function () {
            return $http({
                url: _Url + "/GetCountryList",
                method: "GET",
            });
        },
    }
});