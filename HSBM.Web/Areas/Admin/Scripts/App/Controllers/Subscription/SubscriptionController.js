angular.module('app').controller('SubscriptionController', ['$scope', '$compile', '$http', function ($scope, $compile, $http) {

    var TableName = "Subscription";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            //$scope.Search.IncludingDeleted = $scope.SearchReplica.IncludingDeleted = localdata.IncludingDeleted;
        }
    }
    $scope.Search = {};
    $scope.PrepareDataForLocalStorage = function () {
        //$scope.SearchReplica.Name = $scope.Search.Name;
    }
    $scope.SearchData = function () {
        BindSubscriptionGrid();
        $('#CreatedFromDate').val($scope.Search.CreatedDateFrom);
        $('#CreatedToDate').val($scope.Search.CreatedDateTo);
    };

    $scope.ExportData = function () {
        BindSubscriptionGridExport();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        $('#CreatedDateFrom').datepicker('option', 'maxDate', null);
        $('#CreatedDateTo').datepicker('option', 'minDate', null);
        
        //$('#Subscription_NoRecordFoundButton').show();
        BindSubscriptionGrid();
        $('#CreatedFromDate').val('');
        $('#CreatedToDate').val('');
    };
    $('#CreatedDateFrom').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,
        onSelect: function (date) {
            var dt2 = $('#CreatedDateTo');
            var Date = $(this).datepicker('getDate');
            dt2.datepicker('option', 'minDate', Date);
            $scope.Search.CreatedDateFrom = $("#CreatedDateFrom").val();
            //$scope.Search.CreatedDateTo = $("#CreatedDateTo").val();            
        }
    });
    $('#CreatedDateTo').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,
        onSelect: function (date) {
            var dt2 = $('#CreatedDateFrom');
            var maxDate = $(this).datepicker('getDate');
            dt2.datepicker('option', 'maxDate', maxDate);
            //$scope.Search.CreatedDateFrom = $("#CreatedDateFrom").val();
            $scope.Search.CreatedDateTo = $("#CreatedDateTo").val();
        }
    });
    $scope.OpenCalender = function (id) {
        document.getElementById(id).focus();
    }


    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindSubscriptionGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindSubscriptionGrid();
    }

    function BindSubscriptionGridExport(initialStart, initialLength, initialSortCol, initialSortType) {
        
        //var Columns = [                    
        //            { data: 'Id', bVisible: false },
        //            { data: 'CreatedDate' },
        //            { data: 'Email' },                    
        //];
        //BindSearchGridData('Subscription', Columns, '/Admin/Subscription/Export', $scope.Search, initialStart, initialLength, initialSortCol, initialSortType, function () { });
        console.log("CreatedDateFrom = " + $scope.Search.CreatedDateFrom + " - " + "CreatedDateTo = " + $scope.Search.CreatedDateTo)
        $http({
            method: 'GET',
            url: '/Admin/Subscription/Export',
            params: { CreatedDateFrom: $scope.Search.CreatedDateFrom, CreatedDateTo: $scope.Search.CreatedDateTo }            
        })
    }    

    //$scope.SetLocalStorage = function (url) {
    //    SetLocalStorageData({}, TableName);
    //    window.location = url;
    //}

    //$scope.Delete = function (url) {
    //    SaveOnArchive({}, TableName, false, false);
    //    window.location = url;
    //}

    $scope.ActiveAndInactive = function (index) {

        $scope.ActiveAndInactiveSwitch = angular.copy($("#Subscription").dataTable()._fnGetDataMaster()[parseInt(index)]);

        //if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
        //    $scope.ActiveAndInactiveSwitch.IsActive = true;
        //} else {
        //    $scope.ActiveAndInactiveSwitch.IsActive = false;
        //}



        $.confirm({
            title: 'Please confirm!',
            content: !$scope.ActiveAndInactiveSwitch.IsActive ? 'Are you sure you want to subscribe the Subscriber?' : 'Are you sure you want to unsubscribe the Subscriber?',

            buttons: {
                confirm: function () {

                    $scope.ActiveAndInactiveSwitch.IsActive = !$scope.ActiveAndInactiveSwitch.IsActive

                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();

                    $.ajax({
                        url: '/Admin/Subscription/ActiveAndInactiveSwitchUpdate',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token,
                            subscriptionMaster: $scope.ActiveAndInactiveSwitch
                        },
                        success: function (result) {

                            if ($scope.ActiveAndInactiveSwitch.IsActive)
                                toastr["success"]("Subscriber has been subscribe successfully.", "Success");
                            else
                                toastr["success"]("Subscriber has been unsubscribe successfully.", "Success");

                            SaveOnArchive({}, TableName, false, true);
                            var localdata = GettLocalStorageData(TableName);
                            BindSubscriptionGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                        }, error: function (ex) {

                            toastr["error"]("Somthing went wrong!", "Error")
                        }
                    });
                },
                cancel: function () {

                }
            }
        });


    }

}]);
