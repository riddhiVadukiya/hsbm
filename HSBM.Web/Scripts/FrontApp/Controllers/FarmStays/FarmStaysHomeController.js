angular.module('FrontApp').controller('FarmStaysHomeController', ['$scope', '$compile', '$filter', '$timeout',
function ($scope, $compile, $filter, $timeout) {    
    $scope.GetAllBanner = function () {
        $.ajax({
            url: '/FarmStaysHome/GetAllBanner',
            type: 'POST',
            data: {
                //'__RequestVerificationToken': token,
                //p_SearchRequest: $scope.Search
            },
            success: function (result) {                
                $scope.ListofBanner = (result);
                $scope.$digest();
            }
        });
    }
    //$scope.GetAllBanner();
    $scope.formatDate = function (date) {
        
        var dateOut = new Date(date);
        return dateOut;
    };
    $scope.GetAllBlogs = function () {        
        $.ajax({
            url: '/FarmStaysHome/GetAllPopularBlog',
            type: 'POST',
            data: {
                //'__RequestVerificationToken': token,
                //p_SearchRequest: $scope.Search
            },
            success: function (result) {                
                $scope.ListofBlogs = (result.Data);
                $scope.$digest();
            }
        });
    }
    //$scope.GetAllBlogs();
    $scope.GetAllPopularFarmStay = function () {        
        $.ajax({
            url: '/FarmStaysHome/GetAllPopularFarmStay',
            type: 'POST',
            data: {
                //'__RequestVerificationToken': token,
                //p_SearchRequest: $scope.Search
            },
            success: function (result) {
                
                $scope.ListofFarmStay = (result.Data);                
                $scope.$digest();
            }
        });
    }
    //$scope.GetAllPopularFarmStay();

    $scope.GetAllFarmStaysDiscount = function () {        
        $.ajax({
            url: '/FarmStaysHome/GetAllFarmStaysDiscount',
            type: 'POST',
            data: {
                //'__RequestVerificationToken': token,
                //p_SearchRequest: $scope.Search
            },
            success: function (result) {                
                $scope.ListofFarmStayDiscount = (result.Data);
                $scope.$digest();
            }
        });
    }
    $scope.GetAllFarmStaysDiscount();

    $scope.ShowDetail = function (blog) {        
        $scope.blog = JSON.stringify(blog);
        window.localStorage.setItem("blog", $scope.blog);        
        window.location = "/Blogs/Index";
    }

    $scope.convertToPrice =function(str){
       return  convertToPrice(str)
    }
}]);