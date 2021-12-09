angular.module('FrontApp').controller('CustomerWishlistController', ['$scope', '$compile', 'CustomerFactory', '$filter', '$timeout', 'CommonFactory',
function ($scope, $compile, CustomerFactory, $filter, $timeout, CommonFactory) {
        
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

        $scope.GetWishlist = function () {
            var response = CustomerFactory.GetWishlist();
            response.then(function (successdata) {
                $scope.Wishlist = successdata.data.Data;

                if ($scope.Wishlist != null) {
                    $('#Wishlist').DataTable({
                        data: $scope.Wishlist,
                        destroy: true,
                        bSort: false,
                        searching: false,
                        paging: false,                        
                        columns: [                            
                            {
                                title: "Hotel Name",
                                mRender: function (data, type, row, full) {
                                    var url = "javascript:;";

                                    var d = new Date();
                                    var strDate1 = (d.getDate() < 10 ? '0' + d.getDate() : d.getDate()) + "-" + ((d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1)) + "-" + d.getFullYear();
                                    d.setDate(d.getDate() + 1);
                                    var strDate2 = (d.getDate() < 10 ? '0' + d.getDate() : d.getDate()) + "-" + ((d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1)) + "-" + d.getFullYear();

                                    if (row.HotelType == SupplierApi.RoomXML || row.HotelType == SupplierApi.HotelBeds)
                                    {
                                        url = "/Hotel/Detail/" + row.HotelType + "/" + row.HotelId + "/" + row.CityId + "/" + strDate1 + "/" + strDate2 + "/1p";
                                    }
                                    if (row.HotelType == SupplierApi.Galileo || row.HotelType == SupplierApi.Amadeus) {
                                        url = "/Hotel/Detail/" + row.HotelType + "/" + row.HotelCode + "/" + "13920" + "/" + strDate1 + "/" + strDate2 + "/1p";
                                    }
                                    return "<a title='View' href='" + url + "' class=\"cursorPointer\" target='_blank'>" + row.HotelName + "</a>";
                                }
                            },                           
                        ],                        
                        drawCallback: function () { }
                    });
                }
            }).catch(function (data, status) {
                console.error('Error', response.status, response.data);
            }).finally(function () {
                console.log("finally finished");
            });
        }
        $scope.GetWishlist();

    }]);



