angular.module('app').controller('BlogCommentController', ['$scope', '$compile', 'BlogCommentFactory', '$sce', function ($scope, $compile, BlogCommentFactory, $sce) {

    var TableName = "BlogComment";

    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {

        }
    }    
    $scope.PrepareDataForLocalStorage = function () {

    }

    $scope.GetShortString = function (string, len) {
        
        var shortText = string;
        if (jQuery.trim(shortText).length > len) {
            shortText = jQuery.trim(shortText).substring(0, len).split("")
        .slice(0, -1).join("") + "...";
        }
        return $sce.trustAsHtml(shortText);
    };


    $scope.Search = {};
    var BlogId = location.pathname.substring(1, location.pathname.length).split('/')[3];
    $scope.Search.Id = BlogId;


    var localdata = GettLocalStorageData(TableName);
    if (localdata != null) {
        BindBlogCommentGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    } else {
        BindBlogCommentGrid();
    }

    $scope.SetLocalStorage = function (url) {
        SetLocalStorageData({}, TableName);
        window.location = url;
    }

    $scope.Delete = function (url) {
        
        $.confirm({
            title: 'Please confirm!',
            content: 'Are you sure you want to delete the Blog Comment?',

            buttons: {
                confirm: function () {
                    SaveOnArchive({}, TableName, false, false);
                    toastr["success"]("Blog Comment has been deleted successfully.", "Success");
                    window.location = url;
                },
                cancel: function () {

                }
            }
        });        
    }

    $scope.ActiveAndInactive = function (index, flag) {
        
        var msg = 'Are you sure you want to approve the Blog Comment?';
        if (!flag) {
            msg = 'Are you sure you want to unapprove the Blog Comment?';
        }

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {
                    $scope.ActiveAndInactiveSwitch = angular.copy($("#BlogComment").dataTable()._fnGetDataMaster()[parseInt(index)]);

                    //if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
                    //    $scope.ActiveAndInactiveSwitch.IsApproved = true;
                    //} else {
                    //    $scope.ActiveAndInactiveSwitch.IsApproved = false;
                    //}
                    $scope.ActiveAndInactiveSwitch.IsApproved = !$scope.ActiveAndInactiveSwitch.IsApproved
                    var form = $('#__AjaxAntiForgeryForm');
                    var token = $('input[name="__RequestVerificationToken"]', form).val();

                    var response = BlogCommentFactory.ActiveAndInactiveSwitchUpdateForComment($scope.ActiveAndInactiveSwitch);
                    response.then(function (successdata) {                        
                        if (!flag) {
                            toastr.success('Blog comment has been unapproved successfully.', "Success");
                        }
                        else {
                            toastr.success('Blog comment has been approved successfully.', "Success");
                        }
                        SaveOnArchive({}, TableName, false, true);
                        var localdata = GettLocalStorageData(TableName);
                        BindBlogCommentGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
                    }).catch(function (data, status) {
                        console.error('Error', response.status, response.data);
                    }).finally(function () {
                        console.log("finally finished");
                    });

                    //$.ajax({
                    //    url: '/Blogs/ActiveAndInactiveSwitchUpdateForComment',
                    //    type: 'POST',
                    //    data: {
                    //        '__RequestVerificationToken': token,
                    //        blogComment: $scope.ActiveAndInactiveSwitch
                    //    },
                    //    success: function (result) {

                    //        SaveOnArchive({}, TableName, false, true);
                    //        var localdata = GettLocalStorageData(TableName);
                    //        BindBlogCommentGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
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

angular.module('app').factory('BlogCommentFactory', function ($http) {
    return {
        ActiveAndInactiveSwitchUpdateForComment: function (blogComment, token) {
            return $http({
                url: "/Admin/Blogs/ActiveAndInactiveSwitchUpdateForComment",
                method: "POST",
                data: { blogComment: blogComment }
            });
        },
    }
});