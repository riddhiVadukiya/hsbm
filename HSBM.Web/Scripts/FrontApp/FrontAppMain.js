(function () {
    'use strict';
    //angular.module('FrontApp', ['ngFileUpload', 'ngSanitize', 'toastr', 'ui.bootstrap', 'rzModule', 'ui.select'])
    angular.module('FrontApp', ['ngFileUpload', 'ngSanitize', 'toastr', 'ui.bootstrap', 'ui.select', 'ngCookies','angular.filter'])
})();

angular.module('FrontApp').filter('rangeStar', function () {
    return function (input, total) {
        total = parseInt(total);
        if (total > 0) {
            // If total = 5

            // Will return 0,1,2,3,4
            for (var i = 0; i < total; i++) {
                input.push(i);
            }
        }
        else {
            // If total = -5
            // Will return -5,-4,-3,-2,-1
            for (var i = total; i < 0; i = i + 1) {
                input.push((-1 * i) - 1);
            }
        }

        return input;
    };
});


angular.module('FrontApp').directive('numbersOnly', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {

                if (angular.isUndefined(val)) {
                    var val = '';
                }
                var clean = val.replace(/[^-?\d*\.?\d*$]/g, '');
                var decimalCheck = clean.split('.');

                if (!angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = decimalCheck[1].slice(0, 5);
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }

                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
}).directive('validateEmail', function () {
    var EMAIL_REGEXP = /^[_A-Za-z0-9]+(\.[_A-Za-z0-9]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$/;

    return {
        require: 'ngModel',
        restrict: '',
        link: function (scope, elm, attrs, ctrl) {
            if (ctrl && ctrl.$validators.email) {
                ctrl.$validators.email = function (modelValue) {
                    return ctrl.$isEmpty(modelValue) || EMAIL_REGEXP.test(modelValue);
                };
            }
        }
    };
}).directive('alphanumericcharactersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^a-zA-Z0-9 ]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return '';
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
}).directive('maxNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            var maxlength = attrs.maxNumber;
            function fromUser(text) {

                if (text > maxlength) {
                    var transformedInput = maxlength;
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                    return transformedInput;
                }
                return text;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

//(function () {
//    'use strict';
//    angular.module('FrontApp')
//        .factory('CommonFactory', function ($http, toastr) {
//            var _CommonFrontUrl = '/CommonFront';
//            var mem = {};

//            toastr.options = {
//                "closeButton": true,
//                "positionClass": "toast-top-right",
//                "timeOut": "3000"
//            };

//            var showSimpleToast = function (message, type) {
//                return toastr[type](message);
//            };

//            return {
//                GetAllCountry: function () {
//                    return $http({
//                        url: _CommonFrontUrl + "/GetCountryList",
//                        method: "GET",
//                    });
//                },
//                GetAllCityByCountryId: function (CountryID) {
//                    return $http({
//                        url: _CommonFrontUrl + "/GetAllCityByCountryId",
//                        method: "POST",
//                        data: { p_CountryId: CountryID }
//                    });
//                },
//                SuccessToast: function (text) {
//                    showSimpleToast(text, 'success');
//                },
//                ErrorToast: function (text) {
//                    showSimpleToast(text, 'error');
//                },
//                WarnToast: function (text) {
//                    showSimpleToast(text, 'warning');
//                },
//            }
//        });
//})();