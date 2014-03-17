var LMApp = angular.module('LMApp', ['ngTable', 'ui.bootstrap']);
LMApp.directive('lmtable', function() {
    return {
        restrict: 'E',
        templateUrl: '/Templates/table.html',
    };
});
LMApp.service('dashboardService', function ($http, $q) {
    this.getCustomers = function () {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: 'api/customer'
        }).
         success(function (data, status, headers, config) {
             deferred.resolve(data)
         }).
         error(function (data, status) {
             deferred.reject(data);
         });

        return deferred;
    }
});