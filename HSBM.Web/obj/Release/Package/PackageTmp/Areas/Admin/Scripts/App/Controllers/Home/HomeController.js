angular.module('app').controller('HomeController', ['$scope', '$compile',
    function ($scope, $compile) {
        
        
        var TableName = "Homes";
        $scope.Search = {};
        $scope.SearchReplica = {};

        //$scope.SearchData = function () {
        //    BindRecentMasterGrid();
        //};
        //$scope.ResetData = function () {
        //    $scope.Search = {};
        //    BindRecentMasterGrid();
        //};
        //$scope.SetSearchFromLocalStorage = function (localdata) {
        //    
        //    if (typeof (localdata) != 'undefined') {
        //        $scope.SearchReplica.IncludeIsDeleted = $scope.Search.IncludeIsDeleted = localdata.IncludeIsDeleted;
        //        $scope.SearchReplica.Popular = $scope.Search.Popular = localdata.Popular;
        //    }

        //}
        //var localdata = GettLocalStorageData(TableName);
        //if (localdata != null) {
        //    
        //    $scope.SetSearchFromLocalStorage(localdata);
        //    // alert(JSON.stringify(localdata))
        //    BindRecentMasterGrid(localdata.InitialStart, localdata.InitialLen, localdata.InitialOrderIndex, localdata.InitialOrderType);
        //    SetLocalStorageData({}, TableName);
        //    ResetLocalStorage(TableName);
        //} else {
        //    
        //    BindRecentMasterGrid();
        //}
        
    }]);