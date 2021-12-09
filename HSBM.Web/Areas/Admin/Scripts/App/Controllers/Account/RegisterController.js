angular.module('app').controller('RegisterController',
         ['$scope', 'AccountFactory',
function ($scope, AccountFactory) {

    $scope.model = {};
    
    $scope.singup = function () {
        AccountFactory.Signup($scope.model).then(function (data) {
            window.location = "/";
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });
    };

}]);