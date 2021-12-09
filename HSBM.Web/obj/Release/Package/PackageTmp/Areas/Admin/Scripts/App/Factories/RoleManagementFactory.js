angular.module('app').factory('RoleManagementFactory', function ($http) {
    var _Url = '/Admin/RoleManagement';

    return {
        GetAllRolesForTable: function (RolesSearchRequest) {
            return $http({
                url: _Url + "/GetAllRolesForGrid",
                method: "POST",
                data: { p_SearchRequest: RolesSearchRequest }
            });
        },
    }
});