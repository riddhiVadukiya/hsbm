angular.module('app').controller('AddUpdateFarmStaysController', ['$scope', '$compile', '$filter', '$q', 'Upload', '$timeout','ModalService',  function ($scope, $compile, $filter, $q, Upload, $timeout,ModalService) {
    $scope.Testing = {};
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $scope.BasicDetailFarmStays = {};
    $scope.Discount = {};
    $scope.Season = {};
    $scope.Status = 'BasicDetail';
    $scope.BasicDetailFarmStays.CategoryIdList = [];
    $scope.SetSearchFromLocalStorage = function (localdata) {
        if (typeof (localdata) != 'undefined') {
            $scope.SearchReplica.IncludingDeleted = $scope.Search.IncludingDeleted = localdata.IncludingDeleted;
        }
    }

    $scope.PrepareDataForLocalStorage = function () { }
    

    $scope.tinymceOptions = {
        height: 300,
        toolbar: 'undo redo | bold italic | alignleft aligncenter alignright | code'
    };

    

    var GetAllCategoryMastersForDropDown = function () {
        return $.ajax({
            url: '/Admin/CategoryMaster/GetAllCategoryMastersForDropDown',
            type: 'Get',
            cache: false,
            success: function (result) {
                $scope.ListofCategories = result;
            },
            error: function (ex) {
                toastr["error"]("Somthing went wrong!", "Error")
            }
        });
    }
    var GetAllAmenityMastersForDropDown = function () {
        return $.ajax({
            url: '/Admin/AmenityMaster/GetAllAmenityMastersForDropDown',
            type: 'Get',
            cache: false,
            success: function (result) {
                $scope.ListofAmenities = result;
            },
            error: function (ex) {
                toastr["error"]("Somthing went wrong!", "Error")
            }
        });
    }
    var GetRoomTypeForDropDown = function () {
        return $.ajax({
            url: '/Admin/FarmStays/GetRoomTypeForDropDown',
            type: 'Get',
            cache: false,
            success: function (result) {
                $scope.ListofRoomType = result;
            },
            error: function (ex) {
                toastr["error"]("Somthing went wrong!", "Error")
            }
        });
        //return $q.all([]);
    }

    //Get

    $scope.GetRoomByFarmStayId = function () {

        $.ajax({
            url: '/Admin/FarmStays/GetRoomByFarmStayId',
            type: 'Get',
            cache: false,
            data: {
                FarmStayId: FarmStayId
            },
            success: function (result) {
                $scope.FarmStaysRooms = result.Data;
                $scope.$digest();
            },
            error: function (ex) {
                toastr["error"]("Somthing went wrong!", "Error")
            }
        });
    }
    $scope.GetSeasonByRoomId = function () {

        $.ajax({
            url: '/Admin/FarmStays/GetSeasonByRoomId',
            type: 'Get',
            cache: false,
            data: {
                RoomId: $scope.Season.RoomId
            },
            success: function (result) {
                $scope.FarmStaysSeasons = result.Data;
                $scope.$digest();
            },
            error: function (ex) {
                toastr["error"]("Somthing went wrong!", "Error")
            }
        });
    }

    $scope.GetDiscountByFarmStayId = function () {

        $.ajax({
            url: '/Admin/FarmStays/GetDiscountByFarmStayId',
            type: 'Get',
            cache: false,
            data: {
                FarmStayId: FarmStayId
            },
            success: function (result) {
                $scope.FarmStaysDiscounts = result.Data;
                $scope.$digest();
            },
            error: function (ex) {
                toastr["error"]("Somthing went wrong!", "Error")
            }
        });
    }

    $.when(GetAllCategoryMastersForDropDown(), GetAllAmenityMastersForDropDown(), GetRoomTypeForDropDown()).done(function (a1, a2, a3) {
        if (FarmStayId != null && FarmStayId > 0) {
            $.ajax({
                url: '/Admin/FarmStays/GetFarmStayBasicDetailById',
                type: 'Get',
                cache :false,
                data: {
                    //'__RequestVerificationToken': token,
                    Id: FarmStayId
                },
                success: function (result) {                    
                    $scope.BasicDetailFarmStays = result;
                    initGMap(result.Latitude,result.Longitude);
                    $scope.FarmStaysAmenities = result.FarmStaysAmenities;
                    $scope.FarmStaysImages = result.FarmStaysImages;
                    $scope.GetRoomByFarmStayId();
                    $scope.GetDiscountByFarmStayId();
                    //$scope.$digest();
                    for (var i = 0; i < $scope.ListofAmenities.length; i++) {

                        var Amenities = $filter('filter')(result.FarmStaysAmenities, { AmenityId: $scope.ListofAmenities[i].Value });

                        if (Amenities != null && Amenities.length > 0) {
                            $scope.ListofAmenities[i].Selected = true;
                        }
                    }

                    $scope.BasicDetailFarmStays.CategoryIdList = [];
                    if ($scope.BasicDetailFarmStays.CategoryIds != null) {
                        var _category = $scope.BasicDetailFarmStays.CategoryIds.split(',');

                        for (var i = 0; i < _category.length; i++) {

                            var category = $filter('filter')($scope.ListofCategories, { Value: _category[i] });

                            if (category != null && category.length > 0) {
                                category[0].Selected = true;
                                $scope.BasicDetailFarmStays.CategoryIdList.push(category[0]);
                            }
                        }
                    }
                },
                error: function (ex) {
                    toastr["error"]("Somthing went wrong!", "Error")
                }
            });
        }
        else {
            initGMap();
        }
    });

    //Basic
    $scope.SaveFarmStayBasicDetail = function () {
        if ($scope.BasicDetailFarmStays.CategoryIdList != "" && $scope.BasicDetailFarmStays.CategoryIdList != null && $scope.BasicDetailFarmStays.CategoryIdList.length > 0) {

            $scope.BasicDetailFarmStays.CategoryIds = $scope.BasicDetailFarmStays.CategoryIdList.map(function (e) { return e.Value }).toString();
            // alert(JSON.stringify( $scope.BasicDetailFarmStays.CategoryIds))
            if (($scope.BasicDetailFarmStays.ExtraBedPrice != null || $scope.BasicDetailFarmStays.ExtraBedPrice != "") && parseFloat($scope.BasicDetailFarmStays.ExtraBedPrice) < 1) {
                toastr["error"]("Extra Bed price must be greater than 0.", "Error");

            }
            else {
                $.ajax({
                    url: '/Admin/FarmStays/SaveFarmStayBasicDetail',
                    type: 'POST',
                    cache: false,
                    data: {
                        //'__RequestVerificationToken': token,
                        FarmStaysBasicDetail: $scope.BasicDetailFarmStays
                    },
                    success: function (result) {
                        toastr["success"](result.Message, "Success")
                        if (FarmStayId == null || FarmStayId <= 0) {
                            window.location.href = "/Admin/FarmStays/UpdateFarmStays/" + result.Data;
                        }
                    },
                    error: function (result) {
                        toastr["error"](result.responseJSON.Message, "Error")
                    }
                });
            }
        }
        else
            toastr["error"]("Please select Categories", "Error")
    }

    //Amenity
    $scope.SaveFarmStayAmenities = function () {

        var ListofFarmStayAmenities = [];

        var Amenities = $filter('filter')($scope.ListofAmenities, { Selected: true });

        for (var i = 0; i < Amenities.length; i++) {
            ListofFarmStayAmenities.push({ "AmenityId": Amenities[i].Value, "FarmStaysId": FarmStayId });
        }

        $.ajax({
            url: '/Admin/FarmStays/SaveFarmStayAmenities',
            type: 'POST',
            cache: false,
            data: {
                //'__RequestVerificationToken': token,
                Amenities: ListofFarmStayAmenities
            },
            success: function (result) {
                toastr["success"](result.Message, "Success")
            },
            error: function (ex) {
                toastr["error"](result.responseJSON.Message, "Error")
            }
        });
    }

    //Season
    $scope.SaveFarmStaySeason = function () {

        if (parseFloat($scope.Season.Price) >= 1) {
            
            if ((typeof ($scope.Season.Id) == "undefined" || $scope.Season.Id <= 0) && stringToDate($scope.Season.StartDate, DefaultDateFormat, '/') <= new Date()) {
                toastr["error"]("Start date must be greater than current date.", "Error");

            }
            else {
                $scope.Season.StartDate = $("#StartDate").val();
                $scope.Season.EndDate = $("#EndDate").val();


                $.ajax({
                    url: '/Admin/FarmStays/SaveFarmStaySeason',
                    type: 'POST',
                    cache: false,
                    data: {
                        FarmStaysSeasons: $scope.Season
                    },
                    success: function (result) {
                        toastr["success"](result.Message, "Success");
                        $scope.GetSeasonByRoomId();
                        $scope.ClearSeason($scope.Season.RoomId, $scope.Season.RoomName);
                    },
                    error: function (result) {
                        toastr["error"](result.responseJSON.Message, "Error")
                    }
                });
            }
        }
        else {
            toastr["error"]("Price must be greater than 1.", "Error")
        }
    }

    $scope.EditSeason = function (season, RoomId, RoomName) {
        $scope.Season = angular.copy(season);
        $scope.Season.RoomId = RoomId;
        $scope.Season.RoomName = RoomName;
    }

    $scope.ClearSeason = function (RoomId, RoomName) {
        $scope.Season = {};
        $scope.Season.RoomId = RoomId;
        $scope.Season.RoomName = RoomName;
        $('#StartDate').datepicker('option', 'maxDate', null);
        $('#EndDate').datepicker('option', 'minDate', null);
        $scope.Form.SeasonForm.$setPristine();
    }

    $scope.DeleteFarmStaySeason = function (GroupId) {
        $scope.ClearSeason($scope.Season.RoomId, $scope.Season.RoomName);
        var msg = 'Are you sure you want to delete the season?';

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {
                    $.ajax({
                        url: '/Admin/FarmStays/DeleteFarmStaySeason',
                        type: 'Get',
                        cache: false,
                        data: {
                            GroupId: GroupId
                        },
                        success: function (result) {
                            toastr["success"](result.Message, "Success");
                            $scope.GetSeasonByRoomId();
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

    $scope.CancelSeason = function () {
        $scope.FarmStaysSeasons = [];
        $scope.Season = {};
        $scope.Status = 'Room';

        $('#StartDate').datepicker('option', 'maxDate', null);
        $('#EndDate').datepicker('option', 'minDate', null);
        $scope.Form.SeasonForm.$setPristine();
    }

    //Room
    $scope.SaveFarmStayRoom = function () {
        if ($scope.Room.MaxPerson <= 0) {
            toastr["error"]("Max Person must be greater than 0.", "Error");

        }
        else {
            $scope.Room.FarmStaysId = FarmStayId;
            $.ajax({
                url: '/Admin/FarmStays/SaveFarmStayRoom',
                type: 'POST',
                cache: false,
                data: {
                    FarmStaysRooms: $scope.Room
                },
                success: function (result) {
                    toastr["success"](result.Message, "Success");
                    if ($scope.Room.Id == null || $scope.Room.Id <= 0) {
                        $scope.Season = {};
                        $scope.Season.RoomId = result.Data;
                        $scope.Season.RoomName = $scope.Room.Name;
                        //$scope.Status = 'Season';
                    }
                    $scope.GetRoomByFarmStayId();
                    $scope.ClearRoom();
                },
                error: function (result) {
                    toastr["error"](result.responseJSON.Message, "Error")
                }
            });
        }
    }


    $scope.EditRoom = function (room) {
        
        $scope.Room = angular.copy(room);
    }

    $scope.ClearRoom = function () {
        $scope.Room = {};
       
        $scope.Form.RoomForm.$setPristine();
    }

    $scope.DeleteFarmStayRoom = function (Id) {
        $scope.ClearRoom();
        var msg = 'Are you sure you want to delete the room?';

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {
                    $.ajax({
                        url: '/Admin/FarmStays/DeleteFarmStayRoom',
                        type: 'Get',
                        cache: false,
                        data: {
                            Id: Id
                        },
                        success: function (result) {
                            toastr["success"](result.Message, "Success");
                            $scope.GetRoomByFarmStayId();
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

    $scope.RoomSeason = function (id, Name,isSolo) {
        $scope.Status = 'Season';
        $scope.Season = {};
        $scope.Season.RoomId = id;
        $scope.Season.RoomName = Name;
        $scope.Season.IsSolo = isSolo;
        $scope.SeasonRequest = {};
        $scope.SeasonRequest.RoomId = id;
        $scope.SeasonRequest.IsSolo = isSolo;
        //$scope.RatePlansArray = {};
        //$scope.RatePlansArray.RoomId = id;
        //$scope.GetSeasonByRoomId();
        GetNextSevenDaysOnClick($scope.Testing.Date);
    }

    //Discount
    $scope.SaveFarmStayDiscount = function () {
        if ((typeof ($scope.Discount.Id) == "undefined" || $scope.Discount.Id <= 0) && stringToDate($scope.Discount.StartDate, DefaultDateFormat, '-') < new Date()) {
            toastr["error"]("Start date must be greater than current date.", "Error");
        }
        else {
            if ($scope.Discount.DiscountValue <= 0)  {
                toastr["error"]("Discount must be greater than 0.", "Error");
            }
            else {
                $scope.Discount.IsPercentage = true;
                $scope.Discount.IsEBO = true;
                $scope.Discount.SelectedFarmStays = FarmStayId;

                $.ajax({
                    url: '/Admin/Discount/AddOrUpdateDiscountMaster',
                    type: 'POST',
                    cache: false,
                    data: {
                        '__RequestVerificationToken': token,
                        discountMaster: $scope.Discount
                    },
                    success: function (result) {
                        if ($scope.Discount.Id != null && $scope.Discount.Id > 0)
                            toastr["success"]("Discount has been updated succefully.", "Success");
                        else
                            toastr["success"]("Discount has been added succefully.", "Success");

                        $scope.GetDiscountByFarmStayId();
                        $scope.ClearDiscount();
                    },
                    error: function (result) {
                        toastr["error"](result.responseJSON.StatusMessage, "Error")
                    }
                });
            }
        }
    }

    $scope.EditDiscount = function (discount) {
        $scope.Discount = angular.copy(discount);
    }

    $scope.ClearDiscount = function () {
        $scope.Discount = {};
        $('#DiscountStartDate').datepicker('option', 'maxDate', null);
        $('#DiscountEndDate').datepicker('option', 'minDate', null);
        //$('#BookbyDate').datepicker('option', 'maxDate', null);
        $scope.Form.DiscountForm.$setPristine();
    }

    $scope.DeleteFarmStayDiscount = function (id) {
        $scope.ClearDiscount();
        var msg = 'Are you sure you want to delete the Discount?';

        $.confirm({
            title: "Please confirm",
            content: msg,

            buttons: {
                confirm: function () {

                    //var form = $('#__AjaxAntiForgeryForm');
                    //var token = $('input[name="__RequestVerificationToken"]', form).val();
                    $.ajax({
                        url: '/Admin/Discount/DeleteDiscount/',
                        type: 'POST',
                        cache: false,
                        data: {
                            '__RequestVerificationToken': token,
                            id: id
                        },
                        success: function (result) {
                            toastr["success"]("Discount has been deleted successfully.", "Success")
                            $scope.GetDiscountByFarmStayId();
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

    //Policy
    $scope.SaveFarmStayPolicy = function () {
        if ($scope.BasicDetailFarmStays.CancellationPolicyIsNonRefundable == true) {
            $scope.BasicDetailFarmStays.RefundablePercentage = 0;
            $scope.BasicDetailFarmStays.RefundableBeforDays = 0;
        }
        $.ajax({
            url: '/Admin/FarmStays/SaveFarmStayPolicy',
            type: 'POST',
            cache: false,
            data: {
                //'__RequestVerificationToken': token,
                FarmStaysPolicyDetail: $scope.BasicDetailFarmStays
            },
            success: function (result) {
                toastr["success"](result.Message, "Success")
            },
            error: function (result) {
                toastr["error"](result.responseJSON.StatusMessage, "Error")
            }
        });
    }

    //Image
    $scope.SaveFarmStayImages = function () {

        for (var i = 0; i < $scope.FarmStaysImages.length; i++) {
            $scope.FarmStaysImages[i].FarmStaysId = FarmStayId;
        }

        $.ajax({
            url: '/Admin/FarmStays/SaveFarmStayImages',
            type: 'POST',
            cache: false,
            data: {
                FarmStaysImages: $scope.FarmStaysImages
            },
            success: function (result) {
                $scope.FarmStaysImages = result.Data;
                toastr["success"](result.Message, "Success")
            },
            error: function (ex) {
                toastr["error"](result.responseJSON.Message, "Error")
            }
        });
    }

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
            toastr["warning"](result.Message, "Warning")
        }
        else {
            var uploadUrl = '/Admin/FarmStays/UploadImages'
        }
        $scope.UploadFile(file, uploadUrl,
            function (response) {

                if (response.data.Message != "") {
                    toastr["error"](response.data.Message, "Error");
                }

                if ($scope.FarmStaysImages == undefined) {
                    $scope.FarmStaysImages = [];
                }

                if (response.data.Data != null) {
                    for (var i = 0; i < response.data.Data.length; i++) {
                        $scope.FarmStaysImages.push(response.data.Data[i]);
                    }
                }

            },
            function (response) {
                toastr["error"](response.data.Message, "Error");
            });
    }

    $scope.ViewImage = function (image, index) {
        $scope.Images = image;
        $scope.ImageIndex = index;
        $('#ImagePopup').modal('show');
    }

    $scope.DeleteImage = function (image) {
        image.IsDeleted = true;
    }

    //Map
    $scope.SetLocation = function (lat, lng) {
        initGMap(lat, lng, 12);
    }

    //Other
    $('#CheckInTime').datetimepicker({
        format: 'HH:mm:ss'
    });

    $('#CheckOutTime').datetimepicker({
        format: 'HH:mm:ss'
    });

    $("#CheckInTime").on("dp.change", function () {
        if ($("#CheckInTime").val() != "") {
            $scope.BasicDetailFarmStays.CheckInTime = $("#CheckInTime").val();
        }
    });


    $("#CheckOutTime").on("dp.change", function () {
        if ($("#CheckOutTime").val() != "") {
            $scope.BasicDetailFarmStays.CheckOutTime = $("#CheckOutTime").val();
        }
    });

    $('#StartDate').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,
        onSelect: function (date) {
            var dt2 = $('#EndDate');
            //  var startDate = $(this).datepicker('getDate');
            var minDate = $(this).datepicker('getDate');
            //dt2.datepicker('setDate', minDate);
            //startDate.setDate(startDate.getDate() + 30);
            //sets dt2 maxDate to the last day of 30 days window
            //dt2.datepicker('option', 'maxDate', startDate);
            dt2.datepicker('option', 'minDate', minDate);
            // $(this).datepicker('option', 'minDate', minDate);
            $scope.Season.StartDate = $("#StartDate").val();
            $scope.Season.EndDate = $("#EndDate").val();
        }
    });
    $('#EndDate').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,
        onSelect: function (date) {
            //var dt2 = $('#StartDate');
            //var maxDate = $(this).datepicker('getDate');
            //dt2.datepicker('option', 'maxDate', maxDate);
            var dt2 = $('#StartDate');
            var maxDate = $(this).datepicker('getDate');
            dt2.datepicker('option', 'maxDate', maxDate);
            $scope.Season.StartDate = $("#StartDate").val();
            $scope.Season.EndDate = $("#EndDate").val();
        }
    });

    $('#DiscountStartDate').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,        
        onSelect: function (date) {            
            var dt2 = $('#DiscountEndDate');
            var Date = $(this).datepicker('getDate');
            //dt2.datepicker('setDate', minDate);
            //startDate.setDate(startDate.getDate() + 30);
            //sets dt2 maxDate to the last day of 30 days window
            //dt2.datepicker('option', 'maxDate', startDate);
            dt2.datepicker('option', 'minDate', Date);
            // $(this).datepicker('option', 'minDate', minDate);

            //  var dt3 = $('#BookbyDate');
            // var maxDate = $(this).datepicker('getDate');
            //dt3.datepicker('setDate', maxDate);
            //startDate.setDate(startDate.getDate() + 30);
            // dt3.datepicker('option', 'maxDate', Date);
            // $(this).datepicker('option', 'maxDate', maxDate);

            $scope.Discount.StartDate = $("#DiscountStartDate").val();
            $scope.Discount.EndDate = $("#DiscountEndDate").val();
            // $scope.Discount.BookbyDate = $("#BookbyDate").val();
        }
    });
    $('#DiscountEndDate').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,
        onSelect: function (date) {
            var dt2 = $('#DiscountStartDate');
            var maxDate = $(this).datepicker('getDate');
            dt2.datepicker('option', 'maxDate', maxDate);
            //$('#DiscountStartDate').datepicker('option', 'maxDate', maxDate);
            $scope.Discount.StartDate = $("#DiscountStartDate").val();
            $scope.Discount.EndDate = $("#DiscountEndDate").val();
            //   $scope.Discount.BookbyDate = $("#BookbyDate").val();
        }        
    });

    //$('#BookbyDate').datepicker({
    //    showOtherMonths: true,
    //    selectOtherMonths: true,
    //    dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
    //    dateFormat: DefaultDateFormatsForDatePicker
    //});

    $scope.OpenCalender = function (id) {
        debugger;
        document.getElementById(id).focus();
    }
 
    function initGMap(lat, lng) {
        $timeout(function () {
            if (lat === undefined || lat == null || lat === '' || lng === undefined || lng == null || lng === '') {
                lat = 34.152588;
                lng = 77.577049;
            }
            $scope.BasicDetailFarmStays.Latitude = lat;
            $scope.BasicDetailFarmStays.Longitude = lng;
            //$("#Latitude").val(lat);
            //$("#Longitude").val(lng);
            var latlng = new google.maps.LatLng(lat, lng);
            /*var lat = $("#Latitude").val();
            var long = $("#Longitude").val(); 
            if (lat != undefined && lat != null && lat !== '' && long != undefined && long != null && long !== '') {
                latlng = new google.maps.LatLng(lat, long);
            }
            else {
                $("#Latitude").val('-25.2744');
                $("#Longitude").val('133.7751');
            }*/

            var map = new google.maps.Map(document.getElementById('map'), {
                center: latlng,
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            var marker = new google.maps.Marker({
                position: latlng,
                map: map,
                title: 'Set lat/lon values for this property',
                draggable: false
            });

            //google.maps.event.addListener(marker, 'dragend', function (a) {
            //    $("#Latitude").val(a.latLng.lat);
            //    $("#Longitude").val(a.latLng.lng);
            //});

        }, 100);
    }
    //////////////////////////// ////////////////////////Room Season(For Channel Manager)///////////////////////////////////////////////
    //$scope.SeasonRequest = {};
    //$scope.SeasonRequest.RoomId = $scope.Season.RoomId;

    $('#TestingDatePicker').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
        dateFormat: DefaultDateFormatsForDatePicker,
        onSelect: function (date) {            
            $scope.Testing.Date = $("#TestingDatePicker").val();
            GetNextSevenDaysOnClick($scope.Testing.Date);
        }
    });
///////////    Get Current Date on load in dd/mm/yyyy format
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    today = dd + '/' + mm + '/' + yyyy;
    $scope.Testing.Date = today;
////////////////////////
    var GetNextSevenDaysOnClick = function (currentDate) {
        return $.ajax({
            url: '/Admin/FarmStays/GetNextSevenDays',
            type: 'POST',
            cache: false,
            data: {
                currentDate: currentDate
            },
            success: function (result) {
                $scope.DateArray = result;
                //console.log(JSON.stringify($scope.DateArray));
                $scope.LastDayOfArrayforNext = result[$scope.DateArray.length - 1].ShortDate;
                $scope.SeasonRequest.SeasonStartDate = result[0].ShortDate;
                $scope.SeasonRequest.SeasonEndDate = result[$scope.DateArray.length - 1].ShortDate;
                GetByDate($scope.SeasonRequest);
                $scope.$apply();
            },
            error: function (ex) {
                toastr["error"]("Something went wrong!", "Error")
            }
        });
    }
    //GetNextSevenDaysOnClick($scope.Testing.Date);

    $scope.PreviousClick = function (selectedDate) {
        return $.ajax({
            url: '/Admin/FarmStays/PreviousClick',
            type: 'POST',
            cache: false,
            data: {
                selectedDate: selectedDate
            },
            success: function (result) {
                $scope.DateArray = result;
                $scope.Testing.Date = result[0].ShortDate;
                $scope.LastDayOfArrayforNext = result[$scope.DateArray.length - 1].ShortDate;
                $scope.SeasonRequest.SeasonStartDate = result[0].ShortDate;
                $scope.SeasonRequest.SeasonEndDate = result[$scope.DateArray.length - 1].ShortDate;
                GetByDate($scope.SeasonRequest);
                $scope.$apply();
            },
            error: function (ex) {
                toastr["error"]("Something went wrong!", "Error")
            }
        });
    }

    $scope.NextClick = function (selectedDate) {
        return $.ajax({
            url: '/Admin/FarmStays/NextClick',
            type: 'POST',
            cache: false,
            data: {
                selectedDate: selectedDate
            },
            success: function (result) {
                $scope.DateArray = result;
                $scope.Testing.Date = result[0].ShortDate;
                $scope.LastDayOfArrayforNext = result[$scope.DateArray.length - 1].ShortDate;
                $scope.SeasonRequest.SeasonStartDate = result[0].ShortDate;
                $scope.SeasonRequest.SeasonEndDate = result[$scope.DateArray.length - 1].ShortDate;
                GetByDate($scope.SeasonRequest);
                $scope.$apply();
            },
            error: function (ex) {
                toastr["error"]("Something went wrong!", "Error")
            }
        });
    }

    var GetByDate = function (p_SeasonRequest) {
        return $.ajax({
            url: '/Admin/FarmStays/GetByDate',
            type: 'POST',
            cache: false,
            data: {
                p_SeasonRequest: p_SeasonRequest
            },
            success: function (result) {
                $scope.Arrayyyyys = result.Data;
                $scope.$apply();
                //console.log(JSON.stringify($scope.Arrayyyyys))
            },
            error: function (ex) {
                toastr["error"]("Something went wrong!", "Error")
            }
        });
    }
     
    var GetAllRatePLansAndPlans = function () {
        return $.ajax({
            url: '/Admin/FarmStays/GetAllRatePlansandPlans',
            type: 'POST',
            cache: false,
            data: {
                p_RoomId: $scope.SeasonRequest.RoomId
            },
            success: function (result) {
                $scope.RatePlansArray = result;
                console.log(JSON.stringify($scope.RatePlansArray))
                ModalService.showModal({
                    templateUrl: 'AddSeason.html',
                    scope: $scope.AdvertisementModel,
                    controller: "AddSeasonController",
                    //container: document.getElementById('exampleModal'),
                    inputs: {
                        RPA: $scope.RatePlansArray,
                        isSolo: $scope.SeasonRequest.IsSolo
                    },
                }).then(function (modal) {
                    modal.element.modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    modal.close.then(function (result) {
                        if (result == true)
                        {
                            $.ajax({
                                url: '/Admin/FarmStays/AddSeason',
                                type: 'POST',
                                cache: false,
                                data: {
                                    p_AddSeasonResponse: $scope.RatePlansArray
                                },
                                success: function (result) {
                                    toastr["success"](result.Message, "Success");
                                    GetByDate($scope.SeasonRequest);
                                    $scope.$apply();

                                },
                                error: function (ex) {
                                    toastr["error"](ex.responseJSON.Message, "Error")
                                }
                            });
                        }
                        
                    });
                });
            },
            error: function (ex) {
                toastr["error"](ex.responseJSON.Message, "Error")
            }
        });
    }

    //$scope.GetAllRatePLansAndPlans();
    $scope.AddSeason = function () {
        GetAllRatePLansAndPlans();
        //debugger;
        
    };

    //$scope.AddSeason = function () {

    //    $.ajax({
    //        url: '/Admin/FarmStays/GetAllRatePlansandPlans',
    //        type: 'GET',
    //        cache: false,
    //        success: function (result) {
    //            $scope.RatePlansArray = result;

    //            $scope.Data = {
    //                "RatePlansArray": $scope.RatePlansArray,
    //            };

    //            var confirmModal = $scope.launch("AddSeasonPopUp", "", $scope.Data);
    //            confirmModal.result.then(function (result) {
    //                ShowLoadingPannel();
    //                $scope.PlanValue = result

    //                if ($scope.PlanValue != null) {
    //                    $.ajax({
    //                        url: '/Admin/FarmStays/AddSeasonData',
    //                        type: 'POST',
    //                        cache: false,
    //                        data: {
    //                            p_PlanValue: $scope.PlanValue
    //                        },
    //                        success: function (result) {
    //                            toastr["success"](result.Message, "Success");
    //                            GetByDate($scope.SeasonRequest);
    //                            $scope.$apply();
    //                        },
    //                        error: function (ex) {
    //                            toastr["error"]("Something went wrong!", "Error")
    //                        }
    //                    });
    //                }
    //            });

    //        },
    //        error: function (ex) {
    //            toastr["error"]("Something went wrong!", "Error")
    //        }
    //    });
    //}
    //$('#exampleModal').on('shown.bs.modal', function () {
    //    $('#StartDatePicker1').focus()
    //})

    $scope.UpdateSeason = function () {
        debugger;
        $scope.Arrayyyyys.RoomId = $scope.SeasonRequest.RoomId;
        $scope.Arrayyyyys.SeasonStartDate =  $scope.SeasonRequest.SeasonStartDate;
        $scope.Arrayyyyys.SeasonEndDate = $scope.SeasonRequest.SeasonEndDate;
        return $.ajax({
            url: '/Admin/FarmStays/UpdateSeason',
            type: 'POST',
            cache: false,
            data: {
                p_SeasonListResponse: $scope.Arrayyyyys
            },
            success: function (result) {
                //$scope.Arrayyyyys = result.Data;
                toastr["success"](result.Message, "Success");
                GetByDate($scope.SeasonRequest);
                $scope.$apply();
                //console.log(JSON.stringify($scope.Arrayyyyys))
            },
            error: function (ex) {
                toastr["error"](ex.responseJSON.Message, "Error")
            }
        });
    }
}]);
angular.module('app').controller('AddSeasonController', ['$scope', 'close', 'RPA','isSolo', '$timeout', function ($scope, close, RPA,isSolo, $timeout) {
    //$scope.PlanValue = {};
    $scope.RatePlansArray = RPA;
    $scope.IsSolo = isSolo;

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    today = dd + '/' + mm + '/' + yyyy;
    $scope.RatePlansArray.SeasonStartDate = today;
    $scope.RatePlansArray.SeasonEndDate = today;

    $timeout(function () {        

        $('#StartDatePicker').datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
            dateFormat: DefaultDateFormatsForDatePicker,
            //onSelect: function (date) {
            //    $scope.RatePlansArray.SeasonStartDate = $("#StartDatePicker").val();
            //},
            onSelect: function (date) {            
            var dt2 = $('#EndDatePicker');
            var Date = $(this).datepicker('getDate');
            dt2.datepicker('option', 'minDate', Date);
            $scope.RatePlansArray.SeasonStartDate = $("#StartDatePicker").val();
            $scope.RatePlansArray.SeasonEndDate = $("#EndDatePicker").val();
        }
        });
        $('#EndDatePicker').datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wen', 'Thu', 'Fri', 'Sat'],
            dateFormat: DefaultDateFormatsForDatePicker,
            //onSelect: function (date) {
            //    $scope.RatePlansArray.SeasonEndDate = $("#EndDatePicker").val();
            //}
            onSelect: function (date) {
                var dt2 = $('#StartDatePicker');
                var maxDate = $(this).datepicker('getDate');
                dt2.datepicker('option', 'maxDate', maxDate);
                $scope.RatePlansArray.SeasonStartDate = $("#StartDatePicker").val();
                $scope.RatePlansArray.SeasonEndDate = $("#EndDatePicker").val();
            }
        });
    });
    
    $scope.OpenCalender = function (id) {
        document.getElementById(id).focus();
    }

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
    //alert(JSON.stringify(RPA));

}]);