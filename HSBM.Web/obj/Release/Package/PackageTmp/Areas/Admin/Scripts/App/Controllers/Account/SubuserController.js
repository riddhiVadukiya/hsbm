angular.module('app').controller('SubuserController', ['$scope','$compile','SubuserFactory', function ($scope,$compile,SubuserFactory) {


    $scope.Search = {};

    $scope.SearchData = function () {
        BindSubUsersGrid();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        BindSubUsersGrid();
    };

    var TableName = "Subuser";

    $scope.SetSearchFromLocalStorage = function (localdata) { if (typeof (localdata) != 'undefined') { } }

    $scope.PrepareDataForLocalStorage = function () { }

    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindSubUsersGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindSubUsersGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        $.confirm({
            title: 'Please confirm!',
            content: 'Are you sure you want to delete the System User?',

            buttons: {
                confirm: function () {
                    toastr.success("System User has been deleted successfully", "Success");
                    SaveOnArchive({}, TableName, false, false);
                    window.location = url;
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.ActiveAndInactive = function (index, flag) {
        
        var msg = 'Are you sure you want to delete the System User?';

        if (flag) {
            msg = 'Are you sure you want to restore the System User?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {
                    

        $scope.ActiveAndInactiveSwitch = angular.copy($("#Subuser").dataTable()._fnGetDataMaster()[parseInt(index)]);

        //if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
        //    $scope.ActiveAndInactiveSwitch.IsActive = true;
        //} else {
        //    $scope.ActiveAndInactiveSwitch.IsActive = false;
        //}
        $scope.ActiveAndInactiveSwitch.IsActive = flag;

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
         
        $.ajax({
            url: '/Admin/Account/ActiveAndInactiveSwitchUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                systemUsers: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                if (flag) {
                    toastr.success("System User has been restored successfully", "Success");
                }
                else { toastr.success("System User has been deleted successfully", "Success"); }                
                SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted);
                var localdata = GettLocalStorageData(TableName);
                BindSubUsersGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
                },
                cancel: function () {
                    return;
                }
            }
        });

    }
}]);

angular.module('app').factory('SubuserFactory', function ($http) {
    return {
        ActiveAndInactiveSwitchUpdate: function (systemUsers, token) {
            return $http({
                url: "/Admin/Account/ActiveAndInactiveSwitchUpdate",
                method: "POST",
                data: { systemUsers: systemUsers } //, '__RequestVerificationToken': token }
            });
        },
    }
});