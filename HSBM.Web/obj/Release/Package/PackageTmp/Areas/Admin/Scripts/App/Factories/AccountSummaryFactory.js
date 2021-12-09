
(function () {
    'use strict';
    angular.module('app')
        .factory('AccountSummaryFactory', function ($http, toastr) {
            var _CustomerUrl = '/AccountSummary';

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

                GetAccountSummary: function () {
                    return $http({
                        url: _CustomerUrl + "/GetAccountSummary",
                        method: "POST"
                    });
                },
                GetOutstandingSummary: function () {
                    return $http({
                        url: _CustomerUrl + "/GetOutstandingSummary",
                        method: "POST"
                    });
                }

            }
        });
})();