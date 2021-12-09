angular.module('app').controller('CustomerController', ['$scope', '$http', function ($scope, $http) {

    var TableName = "Customer";

    $scope.SetSearchFromLocalStorage = function (localdata) { if (typeof (localdata) != 'undefined') { } }

    $scope.PrepareDataForLocalStorage = function () { }

      $scope.SearchData = function () {
          BindCustomerGrid();
          $('#CustomerName').val($scope.Search.UserName);
    };
    $scope.ResetData = function () {
        $scope.Search = {};
        
        BindCustomerGrid();
        $('#CustomerName').val('');
    };

    function BindCustomerGrid(initialStart, initialLength, initialSortCol, initialSortType) {
        var Columns = [
                    { data: 'Id', bVisible: false },
                    //{ data: 'FirstName' },
                    //{ data: 'LastName' },
                     {
                         bSortable: false,
                         mRender: function (data, type, row, full) {
                             return row.FirstName + " " + row.LastName
                         }
                     },
                    { data: 'Email' },
                    { data: 'Mobile' },
                    //{ data: 'Gender' },
                    //{
                    //    bSortable: false,
                    //    sClass: "action-cell",
                    //    mRender: function (data, type, row, full) {
                    //        if (!row.IsActive) {
                    //            return "<div class='btn-group' id='toggle_event_editing_" + full.row + "'><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-info locked_active'>INACTIVE</button><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-default unlocked_inactive'>ACTIVE</button></div>";
                    //        }
                    //        else {
                    //            return "<div class='btn-group' id='toggle_event_editing_" + full.row + "'><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-default unlocked_inactive'>INACTIVE</button><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-info locked_active'>ACTIVE</button></div>";
                    //        }
                    //    }
                    //},
                    {
                        bSortable: false,
                        sClass: "action-cell",
                        mRender: function (data, type, row, full) {
                            return "<a onclick=\"angular.element(this).scope().SetLocalStorage('/Admin/Customer/ViewCustomerDetail/" + row.Id + "')\" href='javascript:void(0);' title='View' class='glyphicon glyphicon-eye-open'></a>";
                        }
                    }
        ];
        BindSearchGridData('Customer', Columns, '/Admin/Customer/GetAllCustomerBySearchRequest', $scope.Search, initialStart, initialLength, initialSortCol, initialSortType, function () {
            if ($("#Customer").dataTable().fnGetData().length == 0) {
                $('#' + TableName + "_NoRecordFoundButton").hide();
            } else {
                $('#' + TableName + "_NoRecordFoundButton").show();
            }
        });
    }

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindCustomerGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindCustomerGrid();
    }

    $scope.ExportData = function () {
        BindSubscriptionGridExport();
    };
    function BindSubscriptionGridExport() {
        //$scope.Search.UserName = $("#username").val();
        console.log("UserName = " + $("#username").val())
        $http({
            method: 'POST',
            url: '/Admin/Customer/Export',
            params: { UserName: $("#username").val() }
        })
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
        $scope.ActiveAndInactiveSwitch = angular.copy($("#Customer").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.ActiveAndInactiveSwitch.IsActive = true;
        } else {
            $scope.ActiveAndInactiveSwitch.IsActive = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        
        $.ajax({
            url: '/Admin/Customer/ActiveAndInactiveSwitchUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                systemUsers: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {
                
                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindCustomerGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }

}]);