angular.module('app').controller('CountryController', ['$q', '$scope', '$compile', 'CountryFactory', function ($q, $scope, $compile, CountryFactory) {

    var TableName = "CountryMaster";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            $scope.Search.IncludingDeleted = $scope.SearchReplica.IncludingDeleted = localdata.IncludingDeleted;
        }
    }

    $scope.PrepareDataForLocalStorage = function () {
        $scope.SearchReplica.CountryName = $scope.Search.CountryName;
    }
    
    
    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindCountryMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindCountryMasterGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.CloneCountry = function (index) {
        if (typeof ($scope.ClonedCountry.Id) == "undefined") {
            $scope.ClonedCountry = angular.copy($("#CountryMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);
            $scope.ClonedCountry.Id = 0;
            $scope.ClonedCountry.RowIndex = parseInt(index);
            $("#CountryMaster tbody tr").eq($scope.ClonedCountry.RowIndex).after($("#CloneTemp")[0].innerHTML);
            $compile($("#CountryMaster tbody tr"))($scope);
            $scope.$apply();
        }
    }

    $scope.Delete = function (url) {
        SaveOnArchive({}, TableName, false, false);
        window.location = url;
    }

    $scope.SaveClonedCountry = function () {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: '/Admin/Locations/AddUpdateCountry',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                countryMaster: $scope.ClonedCountry
            },
            success: function (result) {
                BindCountryMasterGrid();
            }
        });
    }

    $scope.CancleClonedCountry = function () {
        var index = $scope.ClonedCountry.RowIndex + 1;
        $("#CountryMaster tbody tr:eq(" + index + ")").remove();
        $scope.ClonedCountry = { RowIndex: 0 };
    }

    $scope.ActiveAndInactive = function (index) {
        $scope.ActiveAndInactiveSwitch = angular.copy($("#CountryMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.ActiveAndInactiveSwitch.IsActive = true;
        } else {
            $scope.ActiveAndInactiveSwitch.IsActive = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
         
        $.ajax({
            url: '/Admin/Locations/ActiveAndInactiveSwitchForCountryUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                countryMaster: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                 
                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindCountryMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }

}]);