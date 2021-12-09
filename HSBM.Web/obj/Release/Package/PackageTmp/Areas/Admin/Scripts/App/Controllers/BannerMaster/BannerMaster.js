angular.module('app').controller('BannerMasterController', ['$scope', '$compile', 'Upload', '$timeout', function ($scope, $compile, Upload, $timeout) {

    var TableName = "BannerMaster";

    $scope.ListofBanner = [];

    ////$scope.UserAccess = JSON.parse($("#hdnUserAccess").val());

    //$scope.SetSearchFromLocalStorage = function (localdata) {
    //    if (typeof (localdata) != 'undefined') {
    //        //$scope.Search.IncludingDeleted = $scope.SearchReplica.IncludingDeleted = localdata.IncludingDeleted;
    //    }
    //}

    //$scope.PrepareDataForLocalStorage = function () {
    //    //$scope.SearchReplica.Name = $scope.Search.Name;
    //}

    //function BindBannerMasterGrid(initialStart, initialLength, initialSortCol, initialSortType) {
    //    var Columns = [
    //                { data: 'Id', bVisible: false },
    //                //{
    //                //    bSortable: false,
    //                //    sClass: "action-cell",
    //                //    mRender: function (data, type, row, full) {
    //                //        return "<img src='/Uploaded/"+row.ImageName+"' alt='"+row.Alt+"' style='width:100px;' />";
    //                //    }
    //                //} ,
    //                { data: 'Title' },
    //                //{ data: 'Alt' },
    //                //{ data: 'OrderIndex' },
    //                //{
    //                //    bSortable: false,
    //                //    sClass: "action-cell",
    //                //    mRender: function (data, type, row, full) {
    //                //        if (!row.IsActive) {
    //                //            return "<div class='btn-group' id='toggle_event_editing_" + full.row + "'><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-info locked_active'>INACTIVE</button><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-default unlocked_inactive'>ACTIVE</button></div>";
    //                //        }
    //                //        else {
    //                //            return "<div class='btn-group' id='toggle_event_editing_" + full.row + "'><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-default unlocked_inactive'>INACTIVE</button><button type='button' onclick=\"angular.element(this).scope().ActiveAndInactive('" + full.row + "')\" class='btn btn-info locked_active'>ACTIVE</button></div>";
    //                //        }
    //                //    }
    //                //},
    //                {
    //                    bSortable: false,
    //                    sClass: "action-cell",
    //                    mRender: function (data, type, row, full) {
    //                        //var str = "";
    //                        //if ($scope.UserAccess.CanUpdate) {
    //                        //    str += "<a onclick=\"angular.element(this).scope().SetLocalStorage('/Admin/Banner/UpdateBanner/" + row.Id + "')\" href='javascript:void(0);' class='glyphicon glyphicon-edit' title='View'> </a>";
    //                        //}
    //                        //if ($scope.UserAccess.CanDelete) {
    //                        //    str += (str != "" ? " | " : "") + "<a ng-show=\"UserAccess.CanUpdate\" onclick=\"angular.element(this).scope().Delete('/Admin/Banner/DeleteBanner/" + row.Id + "')\" href='javascript:void(0);' title='Delete' class='glyphicon glyphicon-trash'></a>";
    //                        //}
    //                        //return str;
    //                        return "<a ng-show=\"UserAccess.CanUpdate\" onclick=\"angular.element(this).scope().SetLocalStorage('/Admin/Banner/UpdateBanner/" + row.Id + "')\" href='javascript:void(0);' class='glyphicon glyphicon-edit' title='View'> </a>" +
    //                             "| <a ng-show=\"UserAccess.CanUpdate\" onclick=\"angular.element(this).scope().DeleteBanner('" + row.Id + "','" + row.ImageName + "')\" href='javascript:void(0);' title='Delete' class='glyphicon glyphicon-trash'></a>";
    //                    }
    //                }
    //    ];
    //    BindSearchGridData('BannerMaster', Columns, '/Admin/Banner/GetAllBannerBySearchRequest', $scope.Search, initialStart, initialLength, initialSortCol, initialSortType, function () { });



    //}

    //var localdata = GettLocalStorageData(TableName);
    //if (localdata != null) {
    //    BindBannerMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    //} else {
    //    BindBannerMasterGrid();
    //}

    //$scope.SetLocalStorage = function (url) {
    //    SetLocalStorageData({}, TableName);
    //    window.location = url;
    //}

    //$scope.Delete = function (url) {
    //    SaveOnArchive({}, TableName, false, false);
    //    window.location = url;
    //}

    //$scope.ActiveAndInactive = function (index) {

    //    $scope.ActiveAndInactiveSwitch = angular.copy($("#BannerMaster").dataTable()._fnGetDataMaster()[parseInt(index)]);

    //    if ($('#toggle_event_editing_' + index + ' button').eq(0).hasClass('locked_active')) {
    //        $scope.ActiveAndInactiveSwitch.IsActive = true;
    //    } else {
    //        $scope.ActiveAndInactiveSwitch.IsActive = false;
    //    }

    //    var form = $('#__AjaxAntiForgeryForm');
    //    var token = $('input[name="__RequestVerificationToken"]', form).val();

    //    $.ajax({
    //        url: '/Admin/Banner/ActiveAndInactiveSwitchUpdate',
    //        type: 'POST',
    //        data: {
    //            '__RequestVerificationToken': token,
    //            bannerMaster: $scope.ActiveAndInactiveSwitch
    //        },
    //        success: function (result) {

    //            SaveOnArchive({}, TableName, false, true);
    //            var localdata = GettLocalStorageData(TableName);
    //            BindBannerMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
    //        }
    //    });

    //    $('#toggle_event_editing_' + index + ' button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
    //    $('#toggle_event_editing_' + index + ' button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');
    //}

    $scope.GetAllBanner = function () {
        $.ajax({
            url: '/Admin/Banner/GetAllBannerBySearchRequest',
            type: 'POST',
            data: {
                //'__RequestVerificationToken': token,
                p_SearchRequest: $scope.Search
            },
            success: function (result) {

                $scope.ListofBanner = (result);
                $scope.$digest();


            }
        });
    }

    $scope.GetAllBanner();

    $scope.UploadFile = function (file, url, callback, errorCallBack) {

        file.upload = Upload.upload({
            url: url,
            method: 'POST',
            headers: {
                'my-header': 'my-header-value'
            },
            file: file

        });

        file.upload.then(function (response) {
            $timeout(function () {
              
                callback(response);
            });
        }, function (response) {
            if (response.status > 0)
                errorCallBack(response);
        });
    }

    $scope.UploadPicImage = function (file) {

        value = file[file.length - 1].name.split(".").pop();
        var ext = value.toLowerCase();
        var lstExt = ["jpg", "jpeg", "png", "gif"];
        if (lstExt.indexOf(ext) == -1) {
            toastr["warning"]("Only jpg, jpeg, png, gif file allowed", "Warning")
        }
        else {
            var uploadUrl = '/Admin/Banner/AddUpdateBanner'
        }
        $scope.UploadFile(file, uploadUrl,
            function (response) {
                toastr["success"]("Banner has been added successfully", "Success")
                $scope.GetAllBanner();


            },
            function (error) {
                toastr["error"](error.Message, "Error");
            });
    }

    $scope.ViewImage = function (image, index) {
        $scope.Images = image;
        $scope.ImageIndex = index;
        $('#ImagePopup').modal('show');
    }

    $scope.DeleteBanner = function (id, name) {
        if ($scope.ListofBanner.length > 1) {
            $.confirm({
                title: 'Please confirm!',
                content: 'Are you sure you want to delete the Banner?',

                buttons: {
                    confirm: function () {
                        $.ajax({
                            url: '/Admin/Banner/DeleteBanner',
                            type: 'Get',
                            data: {
                                //'__RequestVerificationToken': token,
                                Id: id,
                                ImageName: name
                            },
                            success: function (result) {
                                toastr["success"]("Banner has been deleted successfully", "Success")
                                $scope.GetAllBanner();
                            },
                            error: function (ex) {
                                toastr["error"](ex.Message, "Error")
                            }
                        });
                    },
                    cancel: function () {

                    }
                }
            });
        }
        else {
            toastr["error"]("Minimum one Banner is required.", "Error")
        }
    }

}]);