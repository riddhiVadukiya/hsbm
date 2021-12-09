//angular.module('FrontApp').controller('OrdersController', ['$scope', '$compile', '$http', function ($scope, $compile, $http) {

//    var TableName = "Orders";

//    $scope.SetSearchFromLocalStorage = function (localdata) {
//        if (typeof (localdata) != 'undefined') {            
//        }
//    }
//    $scope.Search = {};
//    $scope.PrepareDataForLocalStorage = function () {        
//    }
//    $scope.SearchData = function () {
//        $scope.Search = {            
//            CreatedDateFrom: $('#CreatedDateFrom').val(),
//            CreatedDateTo: $('#CreatedDateTo').val(),
//            OrderNo: $('#OrderNo').val(),
//            CustomerName: $('#CustomerName').val(),
//            Farmstaysid: $('#ddlFrontFarmStays').val(),
//            OrderStatusId: $('#ddlOrderStatus').val(),
//        };
        
//        BindOrdersGrid();
//    };
//    $scope.ResetData = function () {
//        $scope.Search = {};
//        $('#CreatedDateFrom').datepicker('option', 'maxDate', null);
//        $('#CreatedDateTo').datepicker('option', 'minDate', null);
//        $('#OrderNo').val('');
//        $('#CustomerName').val('');
//        $('#ddlFrontFarmStays').val('');
//        $('#ddlOrderStatus').val('');

//        $('#Subscription_NoRecordFoundButton').show();
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

//    var localdata = GettLocalStorageData(TableName);
//    if (localdata != null) {
//        BindOrdersGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
//    } else {
//        BindOrdersGrid();
//    }   

//    $scope.SetLocalStorage = function (url) {
//        SetLocalStorageData({}, TableName);
//        window.location = url;
//    }
    
//}]);
