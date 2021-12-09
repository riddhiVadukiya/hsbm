angular.module('FrontApp').factory('HotelBedsFactory', function ($http) {
    var url = 'Hotelbeds';
    return {
        SearchHotels: function (SearchRequest, p_RoomParams) {
            return $http({
                url: url + "/SearchHotels",
                method: "POST",
                data: { p_SearchRequest: SearchRequest, p_RoomParams: p_RoomParams }
            });
        },
    };
});