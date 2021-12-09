angular.module('app').factory('HomeFactory', function ($http) {
    var _HomeFactoryUrl = '/Admin/Home';
    var _FarmStaysUrl = '/Admin/FarmStays';

    return {
        NewCustomers: function () {
            return $http({
                url: _HomeFactoryUrl + "/NewCustomers",
                method: "GET"                
            });
        },
        GetAllRatePlansandPlans: function () {
            return $http({
                url: _FarmStaysUrl + "/GetAllRatePlansandPlans",
                method: "GET"
            });
        },
        AddSeason: function (planValue) {
            return $http({
                url: _FarmStaysUrl + "/AddSeason",
                method: "POST",
                data: { p_PlanValue: planValue }
            });
        },
    }
});