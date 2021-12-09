angular.module('app').controller('RoleMasterController',
         ['$scope', 'RoleManagementFactory',
function ($scope, RoleManagementFactory) {


    $scope.Search = {};

    $scope.SearchData = function () {
        BindRoleMasterGrid();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        BindRoleMasterGrid();
    };


    var TableName = "RoleMaster";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
        }
    }

    $scope.PrepareDataForLocalStorage = function () {
    }
    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindRoleMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindRoleMasterGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        SaveOnArchive({}, TableName, false, false);
        window.location = url;
    }


    $scope.ActiveAndInactive = function (index, flag) {

        var msg = 'Are you sure you want to delete the Role ?';

        if (flag) {
            msg = 'Are you sure you want to restore the Role ?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

                    $scope.ActiveAndInactiveSwitch = angular.copy($("#RoleMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);
                    $scope.ActiveAndInactiveSwitch.IsActive = flag;
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/RoleManagement/ActiveAndInactiveSwitchUpdate',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token,
                            roleMaster: $scope.ActiveAndInactiveSwitch
                        },
                        success: function (result) {
                            
                            SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted);
                            var localdata = GettLocalStorageData(TableName);
                            BindRoleMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);

                            if (!$scope.ActiveAndInactiveSwitch.IsActive)
                                toastr["success"]("Role has been deleted successfully.", "Success");
                            else
                                toastr["success"]("Role has been restored successfully.", "Success");
                        },
                        error: function (ex) {

                            toastr["error"]("Somthing went wrong!", "Error")
                        }
                    });

                },
                cancel: function () {
                    return;
                }
            }
        });

        //$scope.ActiveAndInactiveSwitch = angular.copy($("#RoleMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);

        //if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
        //    $scope.ActiveAndInactiveSwitch.IsActive = true;
        //} else {
        //    $scope.ActiveAndInactiveSwitch.IsActive = false;
        //}

        //var form = $('#__AjaxAntiForgeryForm');
        //var token = $('input[name="__RequestVerificationToken"]', form).val();

        //$.ajax({
        //    url: '/Admin/RoleManagement/ActiveAndInactiveSwitchUpdate',
        //    type: 'POST',
        //    data: {
        //        '__RequestVerificationToken': token,
        //        roleMaster: $scope.ActiveAndInactiveSwitch
        //    },
        //    success: function (result) {

        //        SaveOnArchive({}, TableName, false, true);
        //        var localdata = GettLocalStorageData(TableName);
        //        BindRoleMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
        //    }
        //});

        //$('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        //$('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }

}]);