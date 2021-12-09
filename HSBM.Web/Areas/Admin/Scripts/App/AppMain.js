(function () {
    'use strict';
    angular.module('app', ['ngFileUpload', 'ngSanitize', 'toastr', 'angular-input-stars', 'fileModel-directive', 'ngTagsInput', 'isteven-multi-select', 'angular.filter', 'ui.tinymce', 'angularModalService'])

    angular.module('app')     
    .directive('numbersOnly', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    
                    if (text) {
                        var transformedInput = text.replace(/[^0-9]/g, '');

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
    })
    .directive('priceOnly', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                function fromUser(text) {
                    if (text) {
                        var transformedInput = text.replace(/[^0-9.]/g, '');

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
    })
        .directive('validateEmail', function () {
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
        }).directive('percentageOnly', function () {
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
                        var clean = val.replace(/[^0-9\.]/g, '');
                        var decimalCheck = clean.split('.');

                        if (!angular.isUndefined(decimalCheck[1])) {
                            decimalCheck[1] = decimalCheck[1].slice(0, 2);
                            clean = decimalCheck[0] + '.' + decimalCheck[1];
                        }



                        if (val !== clean) {
                            ngModelCtrl.$setViewValue(clean);
                            ngModelCtrl.$render();
                        }
                        return clean;
                    });

                    element.bind('keypress', function (event) {
                        var old = $(this).val() + event.key;
                        try {
                            console.log(parseFloat(old));
                            if (parseFloat(old) > 100 || parseFloat(old)<1) {
                                event.preventDefault();
                            }
                        } catch (e) { }

                        if (event.keyCode === 32) {
                            event.preventDefault();
                        }
                    });
                }
            };
        });

})();


(function () {
    'use strict';
    angular.module('app')
        .controller('CommonController', ['$scope', '$timeout', 'dialogs', 'CommonFactory',
            function ($scope, $timeout, dialogs, CommonFactory) {

                $scope.Search = {};
                //$scope.launch = function (which, text, datasource) {
                //    var dlg = null;
                //    switch (which) {

                //        // Error Dialog
                //        case 'error':
                //            dlg = dialogs.error('This is my error message');
                //            break;

                //            // Wait / Progress Dialog
                //        case 'wait':
                //            dlg = dialogs.wait(msgs[i++], progress);
                //            fakeProgress();
                //            break;

                //            // Notify Dialog
                //        case 'notify':
                //            //dlg = dialogs.notify('Something Happened!', 'Something happened that I need to tell you.');
                //            dlg = dialogs.notify('Please Note!', text);
                //            break;

                //            // Add Block Dialog
                //        case 'AddSeasonPopUp':
                //            dlg = dialogs.create('/Admin/FarmStays/AddSeason', 'AddSeasonController', { data: datasource }, { key: false, back: 'static' });
                //            break;

                //            // Confirm Dialog
                //        case 'confirm':
                //            dlg = dialogs.confirm('Please Confirm', text);
                //            break;
                //    }; // end switch
                //    return dlg;
                //}; // end launch

                //var response = CommonFactory.GetAllLanguages();
                //response.then(function (successdata) {

                //    $scope.LanguageSource = successdata.data.Data;
                //    $scope.Search.Language = $.map($scope.LanguageSource, function (l) { if (l.DefaultLanguage) return l.LanguageFullName; })[0];

                //}).catch(function (data, status) {
                //    console.log("Error : " + data);
                //}).finally(function () {
                //    console.log("Finally finished");
                //});

                //$scope.ChangeLanguage = function (lang) {

                //    var response = CommonFactory.ChangeLanguage(lang);
                //    response.then(function (successdata) {

                //        $scope.LanguageSource = successdata.data.Data;
                //        window.location.reload();

                //    }).catch(function (data, status) {
                //        console.log("Error : " + data);
                //    }).finally(function () {
                //        console.log("Finally finished");
                //    });
                //}
            }])
        .factory('CommonFactory', function ($http, toastr) {
            var _CommonFrontUrl = '/Admin/CommonFront';
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
                GetAllCountry: function () {
                    return $http({
                        url: _CommonFrontUrl + "/GetCountryList",
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
                GetAllBlogCategory: function () {
                    return $http({
                        url: "/Admin/Blogs/GetAllBlogCategory",
                        method: "GET",
                    });
                },
                SuccessToast: function (text) {
                    showSimpleToast(text, 'success');
                },
                ErrorToast: function (text) {
                    showSimpleToast(text, 'error');
                },
                WarnToast: function (text) {
                    showSimpleToast(text, 'warning');
                },
                GetAllLanguages: function () {
                    return $http({
                        url: "/Admin/Common/GetAllLanguages",
                        method: "GET"
                    })
                },
                ChangeLanguage: function (lang) {
                    return $http({
                        url: "/Admin/Common/ChangeLanguage",
                        method: "GET",
                        params: { lang: lang }
                    })
                }
            }
        });
})();

//(function () {
//    angular.module('app').controller('AddSeasonController', ['$injector', '$scope', '$modalInstance', 'data', '$controller', '$timeout', '$filter', function ($injector, $scope, $modalInstance, data, $controller, $timeout, $filter) {
//        $timeout(function () {
//            $(".modal-dialog").addClass("modal-sm");
//        })
//        $scope.OpenCalender = function (id) {
//            document.getElementById(id).focus();
//        }

//        $scope.PlanValue = {};
//        if (data) {
//            $scope.RatePlansArray = data.data.RatePlansArray;
//        }

//        $scope.AddSeason = function () {
//            $modalInstance.close($scope.PlanValue);
//        }
//        $scope.Cancel = function () {
//            $modalInstance.dismiss();
//        }

//        $scope.ClosePopup = function () {
//            $modalInstance.dismiss();
//        }

//    }]);
//})();