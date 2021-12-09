angular.module('FrontApp').controller('CustomerController', ['$scope', '$compile', 'CustomerFactory', '$filter', '$timeout',
    function ($scope, $compile, CustomerFactory, $filter, $timeout) {

        $scope.Search = {};
        $scope.Search.ActiveTab = 1;        


        // ----- Hotel Booking ----- //
        $scope.GetHotelBookings = function () {
            var response = CustomerFactory.GetHotelBookings();
            response.then(function (successdata) {
                
                $scope.HotelBookingsList = successdata.data.Data;
                $scope.Search.ActiveTab = 2;
                if ($scope.HotelBookingsList != null) {
                    $('#HotelBookings').DataTable({
                        data: $scope.HotelBookingsList,
                        destroy: true,
                        bSort: false,
                        searching: false,
                        paging: false,
                        
                        columns: [
                            //{
                            //    title: "Id", data: "Id"
                            //},
                            {
                                title: "Date", data: "CreatedDate"
                            },
                            {
                                title: "Hotel",
                                mRender: function (data, type, row, full) {
                                    return "<a onclick=\"angular.element(this).scope().ShowHotelDetail('" + full.row + "')\" title='View' class=\"cursorPointer\">" + row.HotelBookingMaster.HotelDetails.HotelName + "</a>";
                                }
                            },
                            {
                                title: "No of Rooms",
                                mRender: function (data, type, row, full) {
                                    return row.HotelBookingMaster.NoOfRooms;
                                }
                            },
                            {
                                title: "Nights", data: "Amount",
                                mRender: function (data, type, row, full) {
                                    return row.HotelBookingMaster.Nights;
                                }                                
                            },
                            {
                                title: "Amount", //data: "Amount"
                                mRender: function (data, type, row, full) {
                                    return row.Currency + ' ' + row.Amount
                                }
                            }                           
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
        //$scope.GetHotelBookings();
        // ----- Hotel Booking End ----- //


        // ----- Other Functions ----- //
        $scope.ChangeTab = function (activeTab) {
            switch (activeTab) {
              
                case 1:
                    $scope.GetHotelBookings();
                    break;
                default:
                    $scope.GetHotelBookings();
                    break;
            }
        }


        $scope.ShowHotelDetail = function (data) {
            $timeout(function () {
                $scope.HotelDetailData = angular.copy($("#HotelBookings").dataTable()._fnGetDataMaster()[parseInt(data)])
            });
            $("#divHotelDetail").modal('show');
        }


        $scope.CloseHotelDetailPopup = function () {
            $("#divHotelDetail").modal('hide');
        }

        $scope.TimeDifferenceByHours = function (fromDate, toDate) {
            if (fromDate && toDate) {
                var totalmin = Math.round(Math.abs((new Date(fromDate).getTime() - new Date(toDate).getTime()) / (60 * 1000)));

                var hours = Math.trunc(totalmin / 60);
                var minutes = totalmin % 60;
                return ('0' + hours).slice(-2) + "h " + ('0' + minutes).slice(-2) + "m";
            }
        }

        $scope.FindAirlineName = function (code) {

            if ($scope.AirlinesData != undefined) {
                try {
                    return $filter('filter')($scope.AirlinesData, { Value: code })[0].Text;
                } catch (e) {
                    return code;
                }
            }

        };

        GetAllAirportsFunction();

        $scope.DateToGetTimeOnly = function (date) {
            if (date) {
                var dt = new Date(date);
                return ('0' + dt.getHours()).slice(-2) + ":" + ('0' + dt.getMinutes()).slice(-2);
            }
        }

        $scope.FindAirportNameWithoutCountry = function (code) {
            if ($scope.AirportsData != undefined) {
                try {
                    return $filter('filter')($scope.AirportsData, { Value: code })[0].Text.split("(")[0];
                }
                catch (err) { return $filter('filter')($scope.AirportsData, { Value: code })[0].Text; }
            }
        }

        $scope.range = function (count) {
            return Array.apply(0, Array(+count));
        }

        // ----- Other Functions End ----- //

    }]);



