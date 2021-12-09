angular.module('app').controller('AccountSummaryController', ['$scope', '$compile', '$filter', '$timeout', 'AccountSummaryFactory',
    function ($scope, $compile, $filter, $timeout, AccountSummaryFactory) {

        $scope.GetOutstandingSummary = function () {
            var response = AccountSummaryFactory.GetOutstandingSummary();
            response.then(function (successdata) {
                $scope.OutstandingSummaryList = successdata.data.Data;

                if ($scope.AccountSummaryList != null) {
                    $('#Outstanding').DataTable({
                        data: $scope.OutstandingSummaryList,
                        destroy: true,
                        bSort: false,
                        searching: false,
                        paging: false,
                        //pageLength: (typeof (initialLen) != 'undefined') ? initialLen : 10,
                        //displayStart: (typeof (initialStart) != 'undefined') ? initialStart : 0,
                        //order: (typeof (initialSortCol) != 'undefined' && parseInt(initialSortCol) != NaN) ? [[initialSortCol, initialSortType]] : [1, 'desc'],
                        columns: [
                            {
                                data: "CompanyName"
                            },
                            {
                                data: "TotalBookings"
                            },
                            {
                                mRender: function (data, type, row, full) {
                                    return row.Currency + ' ' + row.Outstanding;
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
        $scope.GetOutstandingSummary();

        $scope.GetAccountSummary = function () {
            var response = AccountSummaryFactory.GetAccountSummary();
            response.then(function (successdata) {
                $scope.AccountSummaryList = successdata.data.Data;
                
                if ($scope.AccountSummaryList != null) {
                    $('#Summary').DataTable({
                        data: $scope.AccountSummaryList,
                        destroy: true,
                        bSort: false,
                        searching: false,
                        paging: false,
                        //pageLength: (typeof (initialLen) != 'undefined') ? initialLen : 10,
                        //displayStart: (typeof (initialStart) != 'undefined') ? initialStart : 0,
                        //order: (typeof (initialSortCol) != 'undefined' && parseInt(initialSortCol) != NaN) ? [[initialSortCol, initialSortType]] : [1, 'desc'],
                        columns: [
                            {
                                data: "Id"
                            },
                            {
                                data: "UserName"
                            },
                            {
                                data: "Description"
                            },
                            {
                                bVisible: false, data: "TransactionNumber"
                            },
                            {
                                data: "ReferenceNumber"
                            },
                            {
                                mRender: function (data, type, row, full) {
                                    return row.Currency + ' ' + row.Amount;
                                }
                            },
                            {
                                mRender: function (data, type, row, full) {
                                    return row.Currency + ' ' + row.MarkupAmount;
                                }
                            },
                            {
                                bVisible: false, data: "SupplierType"
                            },
                            {
                                data: "CompanyName"
                            },
                            {
                                data: "CreatedDate"
                            },
                            {
                                data: "OrderType"
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
        $scope.GetAccountSummary();


    }]);