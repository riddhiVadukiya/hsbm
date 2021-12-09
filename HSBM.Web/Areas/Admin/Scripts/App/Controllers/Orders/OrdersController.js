//angular.module('app').controller('OrdersController', ['$scope', '$compile', '$http', '$rootScope', function ($scope, $compile, $http, $rootScope) {

//    var TableName = "Orders";

    
//    $scope.Search = {};
//    $scope.SearchReplica = {};

//    $scope.SetSearchFromLocalStorage = function (localdata) {
//        if (typeof (localdata) != 'undefined') {
//            $scope.SearchReplica.CreatedDateFrom = $scope.Search.CreatedDateFrom = localdata.CreatedDateFrom;
//            $scope.SearchReplica.CreatedDateTo = $scope.Search.CreatedDateTo = localdata.CreatedDateTo;
//            $scope.SearchReplica.OrderNo = $scope.Search.OrderNo = localdata.OrderNo;
//            $scope.SearchReplica.CustomerName = $scope.Search.CustomerName = localdata.CustomerName;
//            $scope.SearchReplica.Farmstaysid = $scope.Search.Farmstaysid = localdata.Farmstaysid;
//            $scope.SearchReplica.OrderStatusId = $scope.Search.OrderStatusId = localdata.OrderStatusId;

//            $('#CreatedDateFrom').val(localdata.CreatedDateFrom);
//            $('#CreatedDateTo').val(localdata.CreatedDateTo);
//            $('#OrderNo').val(localdata.OrderNo);
//            $('#CustomerName').val(localdata.CustomerName);
//            $('#ddlFrontFarmStays').val(localdata.Farmstaysid);
//            $('#ddlOrderStatus').val(localdata.OrderStatusId);

//        }
//    }

//    $scope.PrepareDataForLocalStorage = function () {
//        $scope.SearchReplica.CreatedDateFrom = $scope.Search.CreatedDateFrom = $('#CreatedDateFrom').val();
//        $scope.SearchReplica.CreatedDateTo = $scope.Search.CreatedDateTo = $('#CreatedDateTo').val();
//        $scope.SearchReplica.OrderNo = $scope.Search.OrderNo = $('#OrderNo').val();
//        $scope.SearchReplica.CustomerName = $scope.Search.CustomerName = $('#CustomerName').val();
//        $scope.SearchReplica.Farmstaysid = $scope.Search.Farmstaysid = $('#ddlFrontFarmStays').val();
//        $scope.SearchReplica.OrderStatusId = $scope.Search.OrderStatusId = $('#ddlOrderStatus').val();

//    }
//    $scope.SearchData = function () {        
//        $scope.PrepareDataForLocalStorage();
//        BindOrdersGrid();
//    };    

//    $scope.ResetData = function () {
//        $scope.Search = {};
//        $('#CreatedDateFrom').val('');
//        $('#CreatedDateTo').val('');
//        $('#OrderNo').val('');
//        $('#CustomerName').val('');
//        $('#ddlFrontFarmStays').val('');
//        $('#ddlOrderStatus').val('');
//        $('#Subscription_NoRecordFoundButton').show();
//        $scope.PrepareDataForLocalStorage();
//        BindOrdersGrid();
//    };
//    $('#CreatedDateFrom').datepicker({
//        showOtherMonths: true,
//        selectOtherMonths: true,
//        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
//        dateFormat: DefaultDateFormatsForDatePicker,
//        onSelect: function (date) {
//            var dt2 = $('#CreatedDateTo');
//            var Date = $(this).datepicker('getDate');
//            dt2.datepicker('option', 'minDate', Date);
//            $scope.Search.CreatedDateFrom = $("#CreatedDateFrom").val();                  
//        }
//    });
//    $('#CreatedDateTo').datepicker({
//        showOtherMonths: true,
//        selectOtherMonths: true,
//        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
//        dateFormat: DefaultDateFormatsForDatePicker,
//        onSelect: function (date) {
//            var dt2 = $('#CreatedDateFrom');
//            var maxDate = $(this).datepicker('getDate');
//            dt2.datepicker('option', 'maxDate', maxDate);            
//            $scope.Search.CreatedDateTo = $("#CreatedDateTo").val();
//        }
//    });
//    $scope.OpenCalender = function (id) {
//        document.getElementById(id).focus();
//    }

//    $(document).ready(function () {
        
//        var localdata = GettLocalStorageData(TableName);
//        if (localdata != null) {
            
//            $scope.SetSearchFromLocalStorage(localdata);
//            BindOrdersGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
//            ResetLocalStorage(TableName);
//        } else {
//            $scope.PrepareDataForLocalStorage();
//            BindOrdersGrid();
            
            
//        }
//    });

//    $scope.SetLocalStorage = function (url) {
//        SetLocalStorageData($scope.SearchReplica, TableName);
//        window.location = url;
//    }
//}]);
