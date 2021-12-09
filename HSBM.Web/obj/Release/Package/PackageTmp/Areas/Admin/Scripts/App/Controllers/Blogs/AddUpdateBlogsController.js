angular.module('app').controller('AddUpdateBlogsController', ['$scope', '$compile', '$filter', 'CommonFactory', function ($scope, $compile, $filter, CommonFactory) {

    $scope.vm = {};
    $scope.vm.categories = [];
        
    $scope.GetAllBlogCategory = function () {
        var response = CommonFactory.GetAllBlogCategory();
        response.then(function (successdata) {
            $scope.vm.blogCategoryList = successdata.data.Data;

            //if ($("#Id").val() > 0) {
            //    $scope.vm.categories = JSON.parse($("#CategoriesWithName").val());                
            //}

        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });
    }

   // $scope.GetAllBlogCategory();



    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#blogImage').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#file").change(function () {
        readURL(this);
    });

    //$scope.SetCategories = function ()
    //{
    //    var str = "";
    //    angular.forEach($scope.vm.categories, function (cat) {
    //        if(str != "")
    //        {
    //            str += ","
    //        }
    //        str += cat.Id;
    //    });
    //    $("#Categories").val(str);
    //}   

}]);