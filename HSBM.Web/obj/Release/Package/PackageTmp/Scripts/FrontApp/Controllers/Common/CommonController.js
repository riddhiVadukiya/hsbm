angular.module('FrontApp').controller('CommonController', ['$scope', '$compile', 'CommonFactory', '$filter', '$timeout', '$q',
    function ($scope, $compile, CommonFactory, $filter, $timeout, $q) {

        Array.prototype.where = Array.prototype.where || function (predicate) {
            var results = [],
              len = this.length,
              i = 0;

            for (; i < len; i++) {
                var item = this[i];
                if (predicate(item)) {
                    results.push(item);
                }
            }

            return results;
        };
        $scope.Search = {};        



        $scope.CheckUserIsLoggedIn = function () {
            var response = CommonFactory.CheckUserIsLoggedIn();
            response.then(function (data) {
                return data.data;
            }).catch(function (data, status) {
                return;
            }).finally(function () { return; });
            
        }

        $scope.Login_User = function (model) {
            var response = CommonFactory.Login_User(model);
            response.then(function (data) {
                location.reload();
            }).catch(function (data, status) { return;
            }).finally(function () { return; });
        }

    
        // Get All Currencies
        //var response = CommonFactory.GetAllCurrencyForDropDown();
        //response.then(function (successdata) {
            
        //    $scope.CurrencySource = successdata.data;
        //    $scope.Search.Currency = $filter('filter')($scope.CurrencySource, { Selected: true }).map(function (elem) { return elem.Value; })[0];
        //}).catch(function (data, status) {

        //    console.error('Error', response.status, response.data);
        //}).finally(function () { return; });

        //// Get Region By Country ID //
        //$scope.ChangeCountry = function (CountryID) {
        //    var response = CommonFactory.GetAllCityByCountryId(CountryID);
        //    response.then(function (successdata) {
        //        $scope.RegionSource = successdata.data;
        //    }).catch(function (data, status) {
        //        console.error('Error', response.status, response.data);
        //    }).finally(function () {
        //        console.log("finally finished");
        //    });
        //}


       

        $scope.ChangeBaseCurrency = function () {
            var response = CommonFactory.ChangeBaseCurrency($scope.Search.Currency);
            response.then(function (successdata) {
                window.location.reload();
            }).catch(function (data, status) {

                console.error('Error', response.status, response.data);
            }).finally(function () { return; });
        }

        //$scope.GetCMSPages = function () {
        //    var response = CommonFactory.GetCMSPages();
        //    response.then(function (successData) {
        //        $scope.CMSPages = successData.data;
        //        $scope.CMSPages.forEach(function (e) {
        //            e.Link = "/Page/"+e.Value;
        //        });
        //    }).catch(function (data, status) {
        //        console.error('Error', response.status, response.data);
        //    }).finally(function () { return; });
        //}

        $scope.GetCMSPageDescription = function (cmsPage) {
            $scope.CMSPageDescription = cmsPage["Description"];
        }

        $scope.SendSubscribeRequest = function () {


            $.ajax({
                url: '/CommonFront/SendSubscribeRequest',
                type: 'Post',
                data: { email: $scope.SubscribeEmail },
                success: function (result) {
                    if (result != null) {
                        $scope.SubscribeEmail = null;
                        $scope.SubscribeMessage = result.Message;
                        $scope.$digest();
                        $timeout(function () { $scope.SubscribeMessage = ""; }, 10000);
                        $scope.$digest();
                    }                    
                },
                error: function (ex) {
                    console.error('Error', response.status, response.data);
                }
            });



            //var response = CommonFactory.SendSubscribeRequest($scope.SubscribeEmail);
            //response.then(function (successData) {
            //    $scope.SubscribeEmail = null;
            //$scope.SubscribeMessage = successData.data.Message;
            //$timeout(function () { $scope.SubscribeMessage = ""; }, 10000);
            //}).catch(function (data, status) {
            //    console.error('Error', response.status, response.data);
            //}).finally(function () { return; });
        }

    }]);



