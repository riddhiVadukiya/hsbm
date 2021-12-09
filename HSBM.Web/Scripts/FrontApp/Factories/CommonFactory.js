
(function () {
    'use strict';
    angular.module('FrontApp')
        .factory('CommonFactory', function ($http, toastr) {
            var _CommonFrontUrl = '/CommonFront';
            var _CustomerUrl = '/Customer';
            var mem = {};

            toastr.options = {
                "closeButton": true,
                "positionClass": "toast-top-right",
                "timeOut": "3000"
            };

            var showSimpleToast = function (message, type) {
                return toastr[type](message);
            };

            return {
                SuccessToast: function (text) {
                    showSimpleToast(text, 'success');
                },
                ErrorToast: function (text) {
                    showSimpleToast(text, 'error');
                },
                WarnToast: function (text) {
                    showSimpleToast(text, 'warning');
                },

                CheckUserIsLoggedIn: function () {
                    return $http({
                        url: _CommonFrontUrl + "/CheckUserIsLoggedIn",
                        method: "GET",
                    });
                },
                Login_User: function (model) {
                    return $http({
                        url: _CustomerUrl + "/Login",
                        method: "POST",
                        data: { p_SystemUsers: model }
                    });
                },

                GetAllCountry: function () {
                    return $http({
                        url: _CommonFrontUrl + "/GetCountryList",
                        method: "GET",
                    });
                },
                GetAllDestination: function(){
                    return $http({
                        url: _CommonFrontUrl + "/GetAllDestination",
                        method: "GET",
                    });
                },
                GetAllCityByCountryId: function (CountryID) {
                    return $http({
                        url: _CommonFrontUrl + "/GetAllCityByCountryId",
                        method: "POST",
                        data: { p_CountryId: CountryID }
                    });
                },
                GetAllTopDestinationCity: function () {
                    return $http({
                        url: _CommonFrontUrl + "/GetAllTopDestinationCity",
                        method: "GET",
                    });
                },
                GetAllBanners: function () {
                    return $http({
                        url: _CommonFrontUrl + "/GetAllBanners",
                        method: "GET",
                    });
                },
                GetAllAirPortsFromCode: function (p_AirportCodes) {
                    return $http({
                        url: _CommonFrontUrl + "/GetAllAirPortsFromCode",
                        method: "GET",
                        params: { p_AirportCodes: p_AirportCodes }
                    });
                },
                GetAllAirlinesFromCode: function (p_AirlineCodes) {
                    return $http({
                        url: _CommonFrontUrl + "/GetAllAirlinesFromCode",
                        method: "GET",
                        params: { p_AirlineCodes: p_AirlineCodes }
                    });
                },
                AddUpdateHotelWishlist: function (hotelWishlist) {
                    return $http({
                        url: _CustomerUrl + "/AddUpdateHotelWishlist",
                        method: "POST",
                        data: { p_HotelWishlist: hotelWishlist }
                    });
                },
                GetAllCurrencyForDropDown: function () {
                    return $http({
                        url: _CommonFrontUrl + "/GetAllCurrencyForDropDown",
                        method: "GET"
                    });
                },
                ChangeBaseCurrency: function (id) {
                    return $http({
                        url: _CommonFrontUrl + "/ChangeBaseCurrency",
                        method: "POST",
                        data: { id: id }
                    });
                },
                GetCMSPages: function () {
                    return $http({
                        url: _CommonFrontUrl + "/GetCMSPages",
                        method: "GET"
                    });
                },
                SendSubscribeRequest: function (email) {
                    return $http({
                        url: _CommonFrontUrl + "/SendSubscribeRequest",
                        method: "POST",
                        data: { email: email }
                    });
                },

            }
        });
})();