angular.module('FrontApp').controller('HotelsController', ['$scope', '$compile', 'HotelsFactory', 'CommonFactory', '$filter', function ($scope, $compile, HotelsFactory, CommonFactory, $filter) {

    // ----- INIT ----- //
    $scope.HotelLoading = false;
    $scope.No_result_found = false;
    $scope.HotelDetailModel = {}
    $scope.ListofSearchRoom = [];
    $scope.Rooms = {};
    $scope.RoomDetail = {};
    $scope.RoomDetail.AdultDetail = [];
    $scope.RoomDetail.ChildDetail = [];
    $scope.HotelWishlist = {};
    
    $scope.TotalPeople = 0;

    var date = new Date();
    cin = ('0' + date.getDate()).slice(-2) + "-" + ('0' + (date.getMonth() + 1)).slice(-2) + "-" + date.getFullYear();
    cout = ('0' + (date.getDate() + 1)).slice(-2) + "-" + ('0' + (date.getMonth() + 1)).slice(-2) + "-" + date.getFullYear();

    $scope.Search = {
        DestinationId: '13920',
        CheckIn: cin,
        CheckOut: cout,
        Language: 'ENG',
        APIProvider: APIProvider
    };

    $scope.AddSearchRoom = function () {
        $scope.ListofSearchRoom.push({ "AdultCount": 1, "ChildCount": 0, "arrayofchildrenages": [] });
    }
    $scope.AddSearchRoom();
    $scope.GetAllDestination = function () {
        var response = CommonFactory.GetAllDestination();
        response.then(function (successdata) {
            $scope.DestinationSource = successdata.data;
            return "success";
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        });
    }
    $scope.GetAllDestination();
    $scope.SetTotalPeople = function () {
        $scope.TotalPeople = 0;
        for (var i = 0; i < $scope.ListofSearchRoom.length; i++) {
            $scope.TotalPeople = $scope.TotalPeople + $scope.ListofSearchRoom[i].AdultCount;
            $scope.TotalPeople = $scope.TotalPeople + $scope.ListofSearchRoom[i].ChildCount;
        }
    }
    $scope.RemoveSearchRoom = function (index) {
        $scope.ListofSearchRoom.splice(index, 1);
    }

    $scope.AddChild = function (obj) {
        obj.ChildCount = obj.ChildCount + 1;
        obj.arrayofchildrenages.push({});
        $scope.SetTotalPeople();
    }
    $scope.MinusChild = function (obj) {
        if (obj.ChildCount > 0) {
            obj.ChildCount = obj.ChildCount - 1;
            obj.arrayofchildrenages.pop();
            $scope.SetTotalPeople();
        }
    }
    $scope.Search.ActiveTab = 1;

    $scope.BookingPassengerDetails = [];
    $scope.RoomcountList = [
         { Text: "1", Value: "1" },
         { Text: "2", Value: "2" },
         { Text: "3", Value: "3" },
         { Text: "4", Value: "4" },
         { Text: "5", Value: "5" },
         { Text: "6", Value: "6" },
         { Text: "7", Value: "7" },
         { Text: "8", Value: "8" },
         { Text: "9", Value: "9" }
    ];
    $scope.AdultcountList =
        [{ Text: "1", Value: "1" },
         { Text: "2", Value: "2" },
         { Text: "3", Value: "3" },
         { Text: "4", Value: "4" },
         { Text: "5", Value: "5" },
         { Text: "6", Value: "6" }
        ];

    $scope.ChildcountList =
       [{ Text: "1", Value: "1" },
        { Text: "2", Value: "2" },
        { Text: "3", Value: "3" },
        { Text: "4", Value: "4" },
        { Text: "5", Value: "5" },
        { Text: "6", Value: "6" }];

    $scope.PriceSource = { MinPrice: 0, MaxPrice: 0 };
    $scope.TimeSource = { StartTime: 0, EndTime: 24 };
    $scope.CheckInTime = { min: 0, max: 24 };
    $scope.CheckOutTime = { min: 0, max: 24 };
    $scope.DestinationSource;

    // ----- INIT END ----- //

    // ----- Get Hotel ----- //



    $scope.SearchHotels = function () {
        //ShowLoadingPannel('HotelData');

        $scope.HotelLoading = true;
        $scope.No_result_found = false;
        $scope.Search.Rooms = [];
        for (var i = 0; i < $scope.ListofSearchRoom.length; i++) {
            $scope.Room = {};
            $scope.Room.RoomId = (i + 1);
            $scope.Room.Adult = $scope.ListofSearchRoom[i].AdultCount;
            $scope.Room.Child = $scope.ListofSearchRoom[i].ChildCount;
            $scope.Room.PaxDetail = [];
            for (var j = 0; j < $scope.ListofSearchRoom[i].arrayofchildrenages.length; j++) {
                $scope.Pax = {};
                $scope.Pax.Age = $scope.ListofSearchRoom[i].arrayofchildrenages[j].age;
                $scope.Room.PaxDetail.push($scope.Pax);
            }
            $scope.Search.Rooms.push($scope.Room);
        }
        var response = HotelsFactory.HotelSearch($scope.Search);
        $scope.HotelSearchResult = null;
        response.then(function (successdata) {

            if (successdata.data == "" || successdata.data == "null") {
                $scope.HotelSearchResult = [];
                $scope.HotelLoading = false;
                $scope.No_result_found = true;
            } else {
                $scope.HotelLoading = false;
                $scope.No_result_found = false;
                $scope.HotelSearchResult = successdata.data;
                $scope.HotelSearchResult.forEach(function (hotel) { hotel._isShown = true; });
            }
            HideLoadingPannel();
            $scope.HotelLoading = false;
        }).catch(function (data, status) {
            HideLoadingPannel();
            $scope.HotelLoading = false;
            $scope.No_result_found = true;
            console.error('Error', response.status, response.data);
        }).finally(function () {
            $scope.HotelLoading = false;
            HideLoadingPannel();
            console.log("done");
        });
    };
    $scope.ChangeTab = function (tab) {
        $scope.Search.ActiveTab = tab;
    }
    $scope.ViewHotelDetail = function (Hotel) {
        
        if (typeof (Hotel.HotelDetailKey) != 'undefined' || Hotel.HotelCode != undefined) {
            var ID;

            var room = '';
            $scope.ListofSearchRoom.forEach(function (rooms) {

                room = room.concat(rooms.AdultCount + "p" + (rooms.ChildCount > 0 ? rooms.ChildCount + "p" : "-"));
                var ages = '';
                if (rooms.arrayofchildrenages.length > 0) {
                    rooms.arrayofchildrenages.forEach(function (age) {
                        ages = ages.concat(age.age + "p");
                    });
                    room = room.concat(ages + "-");
                }
            });
            if (parseInt($scope.Search.APIProvider) != 4) {
                ID = Hotel.HotelCode != undefined && Hotel.HotelCode != '' ? Hotel.HotelCode : Hotel.HotelDetailKey; //Hotel.HotelDetailKey != undefined && Hotel.HotelDetailKey != 0 ? Hotel.HotelDetailKey : Hotel.HotelCode;
            }
            else {
                ID = Hotel.HotelCodeString;
            }
            room = room.slice(0, -1);
            $scope.url = '/Hotel/Detail/' + $scope.Search.APIProvider + '/' + ID + '/' + $scope.Search.DestinationId + '/' + $scope.Search.CheckIn + '/' + $scope.Search.CheckOut + '/' + room;
            window.open($scope.url, '_blank');
        }
    };

    $scope.AddRemoveFromWishList = function (Hotel) {

        var isLoggedIn = CommonFactory.CheckUserIsLoggedIn();

        isLoggedIn.then(function (data) {

            if (!data.data) {
                $("#frmLogin").show();
                $("#myModal").modal({ backdrop: 'static', keyboard: false });
                $("#myModal").modal('show');
            }
            else return;

        }).catch(function () {
            return;
        }).finally(function () { return; });

        $scope.HotelWishlist.HotelType = Hotel.HotelEnumId;
        $scope.HotelWishlist.HotelDetailKey = Hotel.HotelDetailKey;
        $scope.HotelWishlist.IsActive = !Hotel.ExistInWishlist;

        if (Hotel.HotelEnumId == SupplierApi.Galileo || Hotel.HotelEnumId == SupplierApi.Amadeus) {
            $scope.HotelWishlist.HotelMaster = {
                HotelCode: Hotel.HotelCode,
                HotelName: Hotel.HotelName,
                Contact: Hotel.Phone,
                AddressLine1: Hotel.AddressLine1,
                AddressLine2: Hotel.AddressLine2,
                AddressLine3: Hotel.AddressLine3,
                Description: Hotel.Description,
                Email: Hotel.Email,
                HotelEnumId: Hotel.HotelEnumId
            };
        }

        var response = CommonFactory.AddUpdateHotelWishlist($scope.HotelWishlist);
        response.then(function (successdata) {
            Hotel.ExistInWishlist = !Hotel.ExistInWishlist;
        }).catch(function (data, status) {            
            console.log("error");
        }).finally(function () {            
            console.log("done");
        });
    };

}]);


angular.module('FrontApp').controller('HotelDetailController', ['$scope', '$compile', 'HotelsFactory', 'CommonFactory', '$filter', function ($scope, $compile, HotelsFactory, CommonFactory, $filter) {
    $scope.HotelLoading = false;
    $scope.No_result_found = false;
    $scope.HotelDetail = {};
    $scope.hotelImages = [];
    
    $scope.Search = {
        DestinationId: DestinationId,
        StrCheckIn: CheckIn,
        StrCheckOut: CheckOut,
        Language: 'ENG',
        APIProvider: APIProvider,
        HotelCode: HotelCode,
        HotelCodeString: HotelCodeString,
        Rooms: Rooms,
    };

    $scope.GetHotelDetail = function () {
        
        ShowLoadingPannel();
        var responsePromise = HotelsFactory.GetHotelDetail($scope.Search);
        responsePromise.then(function (successData) {
            console.log(successData.data);
            $scope.HotelDetail = successData.data;

            $scope.HotelDetail.StrCheckIn = $scope.HotelDetail.CheckInDate;
            $scope.HotelDetail.StrCheckOut = $scope.HotelDetail.CheckOutDate;
            
            if ($scope.HotelDetail.Images.length > 0) {
                for (var i = 0; i < $scope.HotelDetail.Images.length; i++) {
                    var imagedetail = {
                        img: $scope.HotelDetail.Images[i].Url,
                        thumb: $scope.HotelDetail.Images[i].ThumbnailUrl
                    }
                    $scope.hotelImages.push(imagedetail);
                }
                $('.fotorama').fotorama({
                    data: $scope.hotelImages
                });
            }

            HideLoadingPannel();
        }).catch(function (response) {
            HideLoadingPannel();
            alert(response.data.Message)
        });
    }
    $scope.GetHotelDetail();

    $scope.BookHotel = function (room) {

        $scope.HotelBookingInfo = {};
        $scope.HotelBookingInfo.HotelInfo = {};
        $scope.HotelBookingInfo.HotelInfo.HotelDetailKey= $scope.HotelDetail.HotelDetailKey;
        $scope.HotelBookingInfo.HotelInfo.HotelCode = $scope.HotelDetail.HotelCode;

        $scope.HotelBookingInfo.HotelInfo.CheckInDate = $scope.HotelDetail.StrCheckIn;
        $scope.HotelBookingInfo.HotelInfo.CheckOutDate = $scope.HotelDetail.StrCheckOut;
        $scope.HotelBookingInfo.HotelInfo.StrCheckIn = $scope.HotelDetail.StrCheckIn;
        $scope.HotelBookingInfo.HotelInfo.StrCheckOut = $scope.HotelDetail.StrCheckOut;

        $scope.HotelBookingInfo.HotelInfo.HotelId= $scope.HotelDetail.HotelId;
        $scope.HotelBookingInfo.HotelInfo.HotelEnumId = $scope.HotelDetail.HotelType != undefined && $scope.HotelDetail.HotelType != null && $scope.HotelDetail.HotelType != 0 && $scope.HotelDetail.HotelType != '' ? $scope.HotelDetail.HotelType : $scope.HotelDetail.HotelEnumId;
        // $scope.HotelBookingInfo.HotelInfo = $scope.HotelDetail;
        $scope.HotelBookingInfo.HotelInfo.HotelName = $scope.HotelDetail.HotelName;
        $scope.HotelBookingInfo.HotelInfo.Stars = $scope.HotelDetail.Stars;
        $scope.HotelBookingInfo.HotelInfo.AddressLine1 = $scope.HotelDetail.AddressLine1;
        $scope.HotelBookingInfo.HotelInfo.AddressLine2 = $scope.HotelDetail.AddressLine2;
        $scope.HotelBookingInfo.HotelInfo.AddressLine3 = $scope.HotelDetail.AddressLine3;
        $scope.HotelBookingInfo.HotelInfo.City = $scope.HotelDetail.City;
        $scope.HotelBookingInfo.HotelInfo.Phone = $scope.HotelDetail.Phone;
        $scope.HotelBookingInfo.HotelInfo.Fax = $scope.HotelDetail.Fax;

        $scope.HotelBookingInfo.HotelDetailKey = $scope.HotelDetail.HotelDetailKey;
        $scope.HotelBookingInfo.HotelCode = $scope.HotelDetail.HotelCode;
        $scope.HotelBookingInfo.Rooms = [];
        $scope.HotelBookingInfo.HotelInfo.CheckInDate = $scope.Search.CheckIn;
        $scope.HotelBookingInfo.HotelInfo.CheckOutDate = $scope.Search.CheckOut;
        $scope.HotelBookingInfo.TotalPrice = 0;
        $scope.HotelBookingInfo.Token = room.Token;

        if ($scope.HotelDetail.HotelEnumId == SupplierApi.Galileo || $scope.HotelDetail.HotelEnumId == SupplierApi.Amadeus) {
            $scope.HotelBookingInfo.HotelBookingDetail = {
                HotelCode: $scope.HotelDetail.HotelCode,
                HotelName: $scope.HotelDetail.HotelName,
                Contact: $scope.HotelDetail.Phone,
                Address: $scope.HotelDetail.AddressLine1 + ", " + $scope.HotelDetail.City + " - " + $scope.HotelDetail.PostalCode,
                Description: $scope.HotelDetail.Description,
                Email: $scope.HotelDetail.Email,
                HotelType: $scope.HotelDetail.HotelEnumId
            };
        }
        
        $scope.Rooms = [];
        $scope.Search.Rooms.forEach(function (rm, index) {            
            
            rm.Amount = room.Amount;
            rm.RoomType = room.RoomType;
            rm.RoomClass = room.RoomClass;
            rm.RoomName = room.RoomName;
            rm.RoomId = room.Id != undefined && room.Id != null && room.Id != 0 ? room.Id : index + 1;
            rm.PassengerDetail = [];
            for (var i = 0; i < rm.Adult; i++) {
                if (index == 0 && i == 0) {
                    rm.PassengerDetail.push({ "FirstName": "", "LastName": "", "Email": "", "ContactNumber": "", "Type": "A", "RoomNo": index + 1, "IsLeadTraveler": true });
                }
                else {
                    rm.PassengerDetail.push({ "FirstName": "", "LastName": "", "Email": "", "ContactNumber": "", "Type": "A", "RoomNo": index + 1 });
                }

            }
            for (var i = 0; i < rm.Child; i++) {
                rm.PassengerDetail.push({ "FirstName": "", "LastName": "", "Email": "", "ContactNumber": "", "Type": "C", "RoomNo": index + 1, "Age": rm.PaxDetail[i].Age });
            }
            rm.Price = rm.Amount;            
            $scope.Rooms.push(rm);
        });
        $scope.HotelBookingInfo.TotalPrice = room.Amount;
        $scope.HotelBookingInfo.Rooms = $scope.Rooms;
        var response = HotelsFactory.HotelBooking($scope.HotelBookingInfo);
        response.then(function (data) {
            location.href = "/Hotel/Booking/" + data.data;
            //location.href = "/Booking/" + data.data;

        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            return;
        });
    }
}]);

