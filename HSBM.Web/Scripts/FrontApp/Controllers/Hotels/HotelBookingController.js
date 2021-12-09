angular.module('FrontApp').controller('HotelBookingController', ['$scope', '$compile', 'HotelsFactory', 'CommonFactory', '$filter',
    function ($scope, $compile, HotelsFactory, CommonFactory, $filter) {

        var isLoggedIn = CommonFactory.CheckUserIsLoggedIn();

        isLoggedIn.then(function (data) {

            if (!data.data) {
                $("#frmLogin").show();
                $("#myModal").modal({ backdrop: 'static', keyboard: false });
                $("#myModal").modal('show');
            }
            else return;

        }).catch(function () {
            return;
        }).finally(function () { return; });

        $scope.PassengerTypeSource = [
            { Key: "Male", Value: "Male" },
            { Key: "Female", Value: "Female" }
        ];
        $scope.TitleSource = [
            { Key: "MR", Value: "MR" },
            { Key: "MS", Value: "MS" }
        ];

        $scope.HotelBookingInfo = {};
        $scope.SetHotelBookingInfo = function (model) {
            $scope.HotelBookingInfo = model;
        }

        $scope.ConfirmBooking = function () {
            ShowLoadingPannel();

            var isLoggedIn = CommonFactory.CheckUserIsLoggedIn();

            isLoggedIn.then(function (data) {

                if (!data.data) {
                    $("#frmLogin").show();
                    $("#myModal").modal({ backdrop: 'static', keyboard: false });
                    $("#myModal").modal('show');
                }
                else return;

            }).catch(function () {
                return;
            }).finally(function () { return; });

            var responsePromise = HotelsFactory.ConfirmBooking($scope.HotelBookingInfo);
            responsePromise.then(function (successData) {
                HideLoadingPannel();

                location.href = successData.data.Data;
            }).catch(function (data, status, response) {
                HideLoadingPannel();
                console.error('Error', response.status, response.data);
            });
        }

    }]);