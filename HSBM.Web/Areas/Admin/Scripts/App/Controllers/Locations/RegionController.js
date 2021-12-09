angular.module('app').controller('RegionController', ['$scope', '$compile', 'RegionFactory', function ($scope, $compile, RegionFactory) {

    var TableName = "RegionMaster";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            $scope.Search.IncludingDeleted = $scope.SearchReplica.IncludingDeleted = localdata.IncludingDeleted;
        }
    }

    $scope.PrepareDataForLocalStorage = function () {
        $scope.SearchReplica.CountryName = $scope.Search.CountryName;
        $scope.SearchReplica.RegionName = $scope.Search.RegionName;
    }
       

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindRegionMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindRegionMasterGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        SaveOnArchive({}, TableName, false, false);
        window.location = url;
    }

    $scope.GetAllCountryByDropDown = function () {
        var response = RegionFactory.GetAllCountry();
        response.then(function (successdata) {
            $scope.Country = successdata.data;
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });
    }

    $scope.CloneRegion = function (index) {
        if (typeof ($scope.ClonedRegion.Id) == "undefined") {
            $scope.ClonedRegion = angular.copy($("#RegionMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);
            $scope.ClonedRegion.Id = 0;
            $scope.ClonedRegion.RowIndex = parseInt(index);
            $("#RegionMaster tbody tr").eq($scope.ClonedRegion.RowIndex).after($("#CloneTemp")[0].innerHTML);
            $compile($("#RegionMaster tbody tr"))($scope);
            $scope.$apply();
            $scope.GetAllCountryByDropDown();
        }
    }

    $scope.CancleClonedRegion = function () {
        var index = $scope.ClonedRegion.RowIndex + 1;
        $("#RegionMaster tbody tr:eq(" + index + ")").remove();
        $scope.ClonedRegion = { RowIndex: 0 };
    }


    $scope.SaveClonedRegion = function () {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: '/Admin/Locations/AddUpdateRegion',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                regionMaster: $scope.ClonedRegion
            },
            success: function (result) {
                BindRegionMasterGrid();
            }
        });
    }

    $scope.ActiveAndInactive = function (index) {
        $scope.ActiveAndInactiveSwitch = angular.copy($("#RegionMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.ActiveAndInactiveSwitch.IsActive = true;
        } else {
            $scope.ActiveAndInactiveSwitch.IsActive = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
         
        $.ajax({
            url: '/Admin/Locations/ActiveAndInactiveSwitchForRegionMasterUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                regionMaster: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                 
                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindRegionMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }
}]);