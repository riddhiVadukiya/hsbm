angular.module('app').controller('BlogsController', ['$scope', '$compile', 'BlogsFactory', function ($scope, $compile, BlogsFactory) {

    var TableName = "Blogs";
    $scope.Search = {};
    $scope.SearchReplica = {};

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            $scope.SearchReplica.IncludeIsDeleted = $scope.Search.IncludeIsDeleted = localdata.IncludeIsDeleted;
            $scope.SearchReplica.Popular = $scope.Search.Popular = localdata.Popular;
        }
      
    }

    $scope.PrepareDataForLocalStorage = function () {
        $scope.SearchReplica.IncludeIsDeleted = $scope.Search.IncludeIsDeleted;
        $scope.SearchReplica.Popular = $scope.Search.Popular;
    }


    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        $scope.SetSearchFromLocalStorage(localdata);
       // alert(JSON.stringify(localdata))
        BindBlogsMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
        SetLocalStorageData({}, TableName);
        ResetLocalStorage(TableName);
    } else {
        BindBlogsMasterGrid();
    }



    $scope.SearchData = function () {
        $scope.PrepareDataForLocalStorage();
        BindBlogsMasterGrid();
    }
    $scope.ResetData = function () {
        $scope.Search = {};
        $scope.PrepareDataForLocalStorage();
        BindBlogsMasterGrid();
    }
    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData($scope.SearchReplica, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        $.confirm({
            title: 'Please confirm!',
            content: 'Are you sure you want to delete the Blog?',

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
        var msg = 'Are you sure you want to delete the Blog?';

        if (flag) {
            msg = 'Are you sure you want to restore the Blog?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

                    $scope.ActiveAndInactiveSwitch = angular.copy($("#Blogs").dataTable()._fnGetDataMaster()[parseInt(index)]);

                    //if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
                    //    $scope.ActiveAndInactiveSwitch.IsActive = true;
                    //} else {
                    //    $scope.ActiveAndInactiveSwitch.IsActive = false;
                    //}

                    $scope.ActiveAndInactiveSwitch.IsActive = !$scope.ActiveAndInactiveSwitch.IsActive;

                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();

                    var response = BlogsFactory.ActiveAndInactiveSwitchUpdate($scope.ActiveAndInactiveSwitch);
                    response.then(function (successdata) {

                        if (flag)
                            toastr["success"]("Blog has been restored successfully.", "Success");
                        else
                            toastr["success"]("Blog has been deleted successfully.", "Success");


                        //if (flag) {
                        //    $.alert('Blog has been restored successfully.');
                        //}
                        //else {
                        //    $.alert('Blog has been deleted successfully.');
                        //}

                        SaveOnArchive({}, TableName, flag, $scope.Search.IncludeIsDeleted, $scope.Search.Popular);
                        var localdata = GettLocalStorageData(TableName);
                        BindBlogsMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                    }).catch(function (data, status) {
                        console.error('Error', response.status, response.data);
                    }).finally(function () {
                        console.log("finally finished");
                    });

                    //$.ajax({
                    //    url: '/Blogs/ActiveAndInactiveSwitchUpdate',
                    //    type: 'POST',
                    //    data: {
                    //        '__RequestVerificationToken': token,
                    //        blog: $scope.ActiveAndInactiveSwitch
                    //    },
                    //    success: function (result) {

                    //        SaveOnArchive({}, TableName, false, true);
                    //        var localdata = GettLocalStorageData(TableName);
                    //        BindBlogsMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                    //    }
                    //});

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

angular.module('app').factory('BlogsFactory', function ($http) {
    return {
        ActiveAndInactiveSwitchUpdate: function (blog, token) {
            return $http({
                url: "/Admin/Blogs/ActiveAndInactiveSwitchUpdate",
                method: "POST",
                data: { blog: blog }
            });
        },
    }
});