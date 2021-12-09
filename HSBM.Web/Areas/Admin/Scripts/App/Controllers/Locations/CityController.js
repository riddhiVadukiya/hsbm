angular.module('app').controller('CityController', ['$q', '$scope', '$compile', 'CityFactory', function ($q, $scope, $compile, CityFactory) {

    var TableName ="CityMaster";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            $scope.Search.IncludingDeleted = $scope.SearchReplica.IncludingDeleted = localdata.IncludingDeleted;
        }
    }

    $scope.PrepareDataForLocalStorage = function () {
        $scope.SearchReplica.CityName = $scope.Search.CityName;
    }

    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindCityMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindCityMasterGrid();
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
        var response = CityFactory.GetAllCountry();
        response.then(function (successdata) {
            $scope.Country = successdata.data;
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });
    }

    $scope.ChangeCountry = function (CountryID) {
        $scope.GetAllRegionByDropDown(CountryID);
    }

    $scope.GetAllRegionByDropDown = function (CountryID) {
        var response = CityFactory.GetAllRegion(CountryID);
        response.then(function (successdata) {
            $scope.Region = successdata.data;
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });
    }


    $scope.CloneCity = function (index) {
        if (typeof ($scope.ClonedCity.Id) == "undefined") {
            $scope.ClonedCity = angular.copy($("#CityMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);
            $scope.ClonedCity.Id = 0;
            $scope.ClonedCity.RowIndex = parseInt(index);
            $("#CityMaster tbody tr").eq($scope.ClonedCity.RowIndex).after($("#CloneTemp")[0].innerHTML);
            $compile($("#CityMaster tbody tr"))($scope);
            $scope.$apply();
            $scope.GetAllCountryByDropDown();
        }
    }

    $scope.CancleClonedRegion = function () {
        var index = $scope.ClonedCity.RowIndex + 1;
        $("#CityMaster tbody tr:eq(" + index + ")").remove();
        $scope.ClonedCity = { RowIndex: 0 };
    }


    $scope.SaveClonedRegion = function () {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: '/Admin/Locations/AddUpdateCity',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                cityMaster: $scope.ClonedCity
            },
            success: function (result) {
                BindCityMasterGrid();
            }
        });
    }

    $scope.ActiveAndInactive = function (index) {
        $scope.ActiveAndInactiveSwitch = angular.copy($("#CityMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.ActiveAndInactiveSwitch.IsActive = true;
        } else {
            $scope.ActiveAndInactiveSwitch.IsActive = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
         
        $.ajax({
            url: '/Admin/Locations/ActiveAndInactiveSwitchForCityMasterUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                cityMaster: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                 
                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindCityMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });
    }
    $scope.SetTopDestination = function (index) {
        $scope.TopDestinationSwitch = angular.copy($("#CityMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_2_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.TopDestinationSwitch.IsTopDestination = true;
        } else {
            $scope.TopDestinationSwitch.IsTopDestination = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: '/Admin/Locations/TopDestinationSwitchForCityMasterUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                cityMaster: $scope.TopDestinationSwitch
            },
            success: function (result) {

                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindCityMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }

}]);