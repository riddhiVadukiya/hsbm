angular.module('app').controller('BlogCategoryController', ['$scope', '$compile', function ($scope, $compile) {

    var TableName = "BlogCategory";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {

        }
    }

    $scope.PrepareDataForLocalStorage = function () {

    }

    function BindBlogCategoryGrid(initialStart, initialLength, initialSortCol, initialSortType) {
        var Columns = [
                    { data: 'Id', bVisible: false },
                    { data: 'Category' },
                    {
                        bSortable: false,
                        sClass: "action-cell",
                        mRender: function (data, type, row, full) {
                            if (!row.IsActive) {
                                return "<div class='btn-group' id='toggle_event_editing_" + full.row + "'><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-info locked_active'>INACTIVE</button><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-default unlocked_inactive'>ACTIVE</button></div>";
                            }
                            else {
                                return "<div class='btn-group' id='toggle_event_editing_" + full.row + "'><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-default unlocked_inactive'>INACTIVE</button><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-info locked_active'>ACTIVE</button></div>";
                            }
                        }
                    },
                    {
                        bSortable: false,
                        sClass: "action-cell",
                        mRender: function (data, type, row, full) {
                            return "<a onclick=\"angular.element(this).scope().SetLocalStorage('/Admin/BlogCategory/UpdateBlogCategory/" + row.Id + "')\" href='javascript:void(0);' title='Edit' class='glyphicon glyphicon-edit'> </a> | <a onclick=\"angular.element(this).scope().Delete('/Admin/BlogCategory/DeleteBlogCategory/" + row.Id + "')\" href='javascript:void(0);' title='Delete' class='glyphicon glyphicon-trash'></a>";
                        }
                    }
        ];
        BindSearchGridData('BlogCategory', Columns, '/Admin/BlogCategory/GetAllBlogCategoryBySearchRequest', $scope.Search, initialStart, initialLength, initialSortCol, initialSortType, function () { });
    }

    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindBlogCategoryGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindBlogCategoryGrid();
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
        $scope.ActiveAndInactiveSwitch = angular.copy($("#BlogCategory").dataTable()._fnGetDataMaster()[parseInt(index)]);

        if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
            $scope.ActiveAndInactiveSwitch.IsActive = true;
        } else {
            $scope.ActiveAndInactiveSwitch.IsActive = false;
        }

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: '/BlogCategory/ActiveAndInactiveSwitchUpdate',
            type: 'POST',
            data: {
                '__RequestVerificationToken': token,
                blogCategory: $scope.ActiveAndInactiveSwitch
            },
            success: function (result) {

                SaveOnArchive({}, TableName, false, true);
                var localdata = GettLocalStorageData(TableName);
                BindBlogCategoryGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
            }
        });

        $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
        $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    }

}]);