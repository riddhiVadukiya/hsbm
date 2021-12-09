angular.module('app').controller('FarmStaysController', ['$scope', '$compile',  function ($scope, $compile) {

    var TableName = "FarmStays";

    $scope.Search = {}
    $scope.SearchReplica = {};
    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined' && typeof (localdata.IncludeIsDeleted) != 'undefined') {
            $scope.SearchReplica.IncludeIsDeleted = $scope.Search.IncludeIsDeleted = localdata.IncludeIsDeleted;
        }
    }

    $scope.PrepareDataForLocalStorage = function () {
        $scope.SearchReplica.IncludeIsDeleted = $scope.Search.IncludeIsDeleted;
    }


    $scope.SearchData = function () {
        $scope.PrepareDataForLocalStorage();
        BindFarmStaysGrid();
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        $scope.PrepareDataForLocalStorage();
        BindFarmStaysGrid();
    };

    

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        $scope.SetSearchFromLocalStorage(localdata);
        BindFarmStaysGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
        ResetLocalStorage(TableName);
    } else {
        BindFarmStaysGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData($scope.SearchReplica, TableName);
        window.location = url;
    }

    $scope.ActiveAndInactive = function (index, flag) {
        

        var msg = 'Are you sure you want to delete the FarmStay?';

        if (flag) {
            msg = 'Are you sure you want to restore the FarmStay?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

                    $scope.ActiveAndInactiveSwitch = angular.copy($("#FarmStays").dataTable()._fnGetDataMaster()[parseInt(index)]);
                    $scope.ActiveAndInactiveSwitch.IsActive = flag;
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/FarmStays/ActiveAndInactiveFarmStay',
                        type: 'POST',
                        data: {
                            '__RequestVerificationToken': token,
                            FarmStays: $scope.ActiveAndInactiveSwitch
                        },
                        success: function (result) {
                            SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted);
                            var localdata = GettLocalStorageData(TableName);
                            BindFarmStaysGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                            if (flag) {
                                toastr["success"]("FarmStay has been restored successfully.", "Success")
                            }
                            else {
                                toastr["success"]("FarmStay has been deleted successfully.", "Success")
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
        
    }

}]);

