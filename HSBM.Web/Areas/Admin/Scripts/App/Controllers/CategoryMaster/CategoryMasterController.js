angular.module('app').controller('CategoryMasterController', ['$scope', '$compile', 'CategoryMasterFactory', function ($scope, $compile, CategoryMasterFactory) {

    var TableName = "CategoryMaster";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {

        }
    }

    $scope.PrepareDataForLocalStorage = function () {

    }

    $scope.Search = {};

    $scope.SearchData = function () {
        BindCategoryMasterGrid();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        BindCategoryMasterGrid();
    };

    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindCategoryMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindCategoryMasterGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {

        $.confirm({
            title: 'Please confirm!',
            content: 'Are you sure you want to delete the Category?',

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
        

        var msg = 'Are you sure you want to delete the Category?';

        if (flag) {
            msg = 'Are you sure you want to restore the Category?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

                    $scope.ActiveAndInactiveSwitch = angular.copy($("#CategoryMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);
                    $scope.ActiveAndInactiveSwitch.IsActive = flag;
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/CategoryMaster/ActiveAndInactiveSwitchUpdate',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token,
                            CategoryMaster: $scope.ActiveAndInactiveSwitch
                        },
                        success: function (result) {
                            SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted);
                            var localdata = GettLocalStorageData(TableName);
                            BindCategoryMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                            
                            if (flag) {
                                toastr["success"]("Category has been restored successfully.", "Success")
                            }
                            else {
                                toastr["success"]("Category has been deleted successfully.", "Success")
                            }
                            
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

angular.module('app').factory('CategoryMasterFactory', function ($http) {
    return {
        ActiveAndInactiveSwitchUpdate: function (CategoryMaster, token) {
            return $http({
                url: "/Admin/CategoryMaster/ActiveAndInactiveSwitchUpdate",
                method: "POST",
                data: { CategoryMaster: CategoryMaster } //, '__RequestVerificationToken': token }
            });
        },
    }
});
