angular.module('app').controller('CurrencyController', ['$scope', '$compile', function ($scope, $compile) {

    var TableName = "Currencies";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            //$scope.Search.IncludingDeleted = $scope.SearchReplica.IncludingDeleted = localdata.IncludingDeleted;
        }
    }

    $scope.PrepareDataForLocalStorage = function () {
        //$scope.SearchReplica.Name = $scope.Search.Name;
    }

    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindCurrencyGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindCurrencyGrid();
    }
    $scope.ActiveAndInactive = function (index, flag) {

        var msg = 'Are you sure you want to archive the Currency?';

        if (flag) {
            msg = 'Are you sure you want to restore the Currency?';
        }
        $.confirm({
            title: "Please confirm",
            content: msg,
            buttons: {
                confirm: function () {
                    
                    $scope.ActiveAndInactiveSwitch = angular.copy($("#Currencies").dataTable()._fnGetDataMaster()[parseInt(index)]);
                    $scope.ActiveAndInactiveSwitch.IsActive = flag;

                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/Currency/ActiveAndInactiveSwitchUpdate',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token,
                            currencyMaster: $scope.ActiveAndInactiveSwitch
                        },
                        success: function (result) {
                            SaveOnArchive({}, TableName, false, true);
                            var localdata = GettLocalStorageData(TableName);
                            BindCurrencyGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);

                            toastr["success"]("Currency status has been updated!", "Success")

                        }, error: function (ex) {

                            toastr["error"]("Somthing went wrong!", "Error")
                        }
                    });
                },
                cancel: function () { return; }
            }
        });

    }

    $scope.UpdateLatestCurrencyRate = function () {

        var msg = 'Are you sure you want to update the Currency?';
        $.confirm({
            title: "Please confirm",
            content: msg,
            buttons: {
                confirm: function () {
                    
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/Currency/UpdateLatestCurrencyRate',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token
                        },
                        success: function (result) {
                            SaveOnArchive({}, TableName, false, true);
                            var localdata = GettLocalStorageData(TableName);
                            BindCurrencyGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);

                            toastr["success"]("Currency has been updated!", "Success")

                        }, error: function (ex) {

                            toastr["error"]("Somthing went wrong!", "Error")
                        }
                    });
                },
                cancel: function () { return; }
            }
        });

    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        SaveOnArchive({}, TableName, false, false);
        window.location = url;
    }

}]);