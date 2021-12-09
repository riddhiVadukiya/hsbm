angular.module('app').controller('SiteSettingController', ['$scope', '$compile', function ($scope, $compile) {
    
    var TableName = "SiteSetting";

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
        BindSiteSettingGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindSiteSettingGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        SaveOnArchive({}, TableName, false, false);
        window.location = url;
    }
    $scope.ActiveAndInactive = function (index) {
        $scope.ActiveAndInactiveSwitch = angular.copy($("#SiteSetting").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.ActiveAndInactiveSwitch.IsActive = true;
        } else {
            $scope.ActiveAndInactiveSwitch.IsActive = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        
        $.ajax({
            url: '/Admin/SiteSettings/ActiveAndInactiveSwitchUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                siteSettings: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                
                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindSiteSettingGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }


}]);