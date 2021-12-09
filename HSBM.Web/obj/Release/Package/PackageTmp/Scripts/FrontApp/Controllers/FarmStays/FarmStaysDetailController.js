angular.module('FrontApp').controller('FarmStaysDetailController', ['$scope', '$compile', '$filter', '$timeout', 'CommonFactory',
function ($scope, $compile, $filter, $timeout, CommonFactory) {

    var DetailURL = '/FarmStaysDetail/';
    $scope.Search = {};
    $scope.IsFirstTime = true;
    //if (FarmStayId != '') {
    //    $scope.Search.FarmStayId = FarmStayId;
    //}
    //if (CheckIn != '') {
    //    $scope.Search.CheckIn = CheckIn;
    //}
    //if (CheckOut != '') {
    //    $scope.Search.CheckOut = CheckOut;
    //}
    //if (Guests != '') {
    //    $scope.Search.Guests = Guests;
    //}
    //if (IsSolo != '') {
    //    $scope.Search.IsSolo = IsSolo;
    //}
    //alert(JSON.stringify($scope.Search))

    $scope.SearchFarmstays = function () {
        $scope.Search = {
            FarmStayId: $('#FarmStayId').val(),
            CheckIn: $('#CheckIn').val(),
            CheckOut: $('#CheckOut').val(),
            Guests: $('#Guests').val(),
            Child: $('#Child').val(),
            IsSolo: $('#IsSolo').val()
        };
        $.ajax({

            url: DetailURL + 'GetAvailableRoom',
            type: 'Post',
            data: {
                SearchFarmStaysRequest: $scope.Search
            },
            success: function (result) {
                if (result != null) {
                    $scope.ListofRoom = result.ListofRoom;
                    $scope.FarmStayDiscount = result.FarmStayDiscount;

                    if ($scope.ListofRoom != null && $scope.ListofRoom.length > 0) {
                        for (var i = 0; i < $scope.ListofRoom.length; i++) {

                            $scope.ListofRoom[i].DiscountPrice = convertToPrice(result.ListofRoom[i].DiscountPrice);
                            $scope.ListofRoom[i].Price = convertToPrice(result.ListofRoom[i].Price);
                        }
                        if ($scope.IsFirstTime) {
                            $scope.Price = $scope.ListofRoom[0].Price;
                            $scope.DiscountPrice = $scope.ListofRoom[0].DiscountPrice;
                            $scope.IsFirstTime = false;
                        }
                    }
                }
                $scope.$digest();
            },
            error: function (ex) {
            }
        });
        
    }

    $scope.SearchFarmstays();
}]);

