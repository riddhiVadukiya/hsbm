angular.module('FrontApp').factory('HotelsFactory', function ($http) {
    var _Url = '/Hotel';
    return {
        HotelSearch: function (HotelSearchRequest) {
            return $http({
                url: _Url + "/Search",
                method: "POST",
                data: { searchRequest: HotelSearchRequest }
            });
        },

        HotelCheckOut: function (hotelCheckOutRequest) {
            return $http({
                url: _Url + "/HotelCheckOut",
                method: "POST",
                data: { hotelCheckOutRequest: hotelCheckOutRequest }
            });
        },
        GetHotelDetail: function (HotelSearchRequest) {
            return $http({
                url: _Url + "/GetHotelDetail",
                method: "POST",
                data: { searchRequest: HotelSearchRequest }
            });
        },
        GetAllDestination: function () {
            return $get({
                url: _Url + "/GetAllDestination",
                method: "GET"
            });
        },
        HotelBooking: function (HotelBookingInfo) {
            return $http({
                url: _Url + "/HotelBooking",
                method: "POST",
                data: { hotelBookingModel: HotelBookingInfo }
            });
        },
        SetHotelBookingInfo: function (Id) {
            return $http({
                url: "/Booking/SetHotelBookingInfo",
                method: "GET",
                params: { id: Id }
            });
        },
        ConfirmBooking: function (HotelBookingInfo) {
            return $http({
                url: "/Booking/ConfirmBooking",
                method: "POST",
                data: { hotelBookingInfo: HotelBookingInfo }
            });
        }        
    }
});