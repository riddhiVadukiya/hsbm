
(function () {
    'use strict';
    angular.module('FrontApp')
        .factory('CustomerFactory', function ($http, toastr) {
            var _CustomerUrl = '/Customer';

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

                GetHotelBookings: function () {
                    return $http({
                        url: _CustomerUrl + "/GetHotelBookings",
                        method: "POST"
                    });
                },
                GetWishlist: function () {
                    return $http({
                        url: _CustomerUrl + "/GetAllHotelWishlistByUserId",
                        method: "GET"
                    });
                },
            }
        });
})();