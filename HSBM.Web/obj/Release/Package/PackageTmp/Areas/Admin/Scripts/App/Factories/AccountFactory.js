angular.module('app').factory('AccountFactory', function ($http) {
    var _AccountFactoryUrl = '/Admin/Account';

    return {
        Signup: function (SystemUser) {
            return $http({
                url: _AccountFactoryUrl + "/Signup",
                method: "POST",
                data: { p_SystemUser: SystemUser }
            });
        }
    }
});