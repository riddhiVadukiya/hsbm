angular.module('app').controller('EmailTemplatesController', ['$scope', '$compile', function ($scope, $compile) {
    
    var TableName = "EmailTemplates";

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
        BindEmailTemplatesGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindEmailTemplatesGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        $.confirm({
            title: 'Please confirm!',
            content: 'Are you sure you want to delete the Email Template?',

            buttons: {
                confirm: function () {
                    SaveOnArchive({}, TableName, false, false);
                    window.location = url;
                },
                cancel: function () {

                }
            }
        });
    }
      $scope.SearchData = function () {
          BindEmailTemplatesGrid();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        BindEmailTemplatesGrid();
    };

    $scope.ActiveAndInactive = function (index, flag) {
        var msg = 'Are you sure you want to delete the Email Template?';

        if (flag) {
            msg = 'Are you sure you want to restore the Email Template?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

        $scope.ActiveAndInactiveSwitch = angular.copy($("#EmailTemplates").dataTable()._fnGetDataMaster()[parseInt(index)]);

        //if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
        //    $scope.ActiveAndInactiveSwitch.IsActive = true;
        //} else {
        //    $scope.ActiveAndInactiveSwitch.IsActive = false;
        //}
        $scope.ActiveAndInactiveSwitch.IsActive = !$scope.ActiveAndInactiveSwitch.IsActive;

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        
        $.ajax({
            url: '/EmailTemplates/ActiveAndInactiveSwitchUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                emailTemplates: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                
                SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted);
                var localdata = GettLocalStorageData(TableName);
                BindEmailTemplatesGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
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