angular.module('app').controller('AmenityMasterController', ['$scope', '$compile', 'AmenityMasterFactory', function ($scope, $compile, AmenityMasterFactory) {

    var TableName = "AmenityMaster";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {

        }
    }

    $scope.PrepareDataForLocalStorage = function () {

    }

    $scope.Search = {};

    $scope.SearchData = function () {
        BindAmenityMasterGrid();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        BindAmenityMasterGrid();
    };

    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindAmenityMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindAmenityMasterGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {

        $.confirm({
            title: 'Please confirm!',
            content: 'Are you sure you want to delete the Amenity?',

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

    $scope.ActiveAndInactive = function (index, flag) {
        

        var msg = 'Are you sure you want to delete the Amenity?';

        if (flag) {
            msg = 'Are you sure you want to restore the Amenity?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

                    $scope.ActiveAndInactiveSwitch = angular.copy($("#AmenityMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);
                    $scope.ActiveAndInactiveSwitch.IsActive = flag;
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/AmenityMaster/ActiveAndInactiveSwitchUpdate',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token,
                            amenityMaster: $scope.ActiveAndInactiveSwitch
                        },
                        success: function (result) {
                            SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted);
                            var localdata = GettLocalStorageData(TableName);
                            BindAmenityMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                            
                            if (!$scope.ActiveAndInactiveSwitch.IsActive)
                                toastr["success"]("Amenity has been deleted successfully.", "Success");
                            else
                                toastr["success"]("Amenity has been restored successfully.", "Success");
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


        

        //$('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        //$('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }

}]);

angular.module('app').factory('AmenityMasterFactory', function ($http) {
    return {
        ActiveAndInactiveSwitchUpdate: function (amenityMaster, token) {
            return $http({
                url: "/Admin/AmenityMaster/ActiveAndInactiveSwitchUpdate",
                method: "POST",
                data: { amenityMaster: amenityMaster } //, '__RequestVerificationToken': token }
            });
        },
    }
});
