var LMApp = angular.module('LMApp', ['ngTable', 'ui.bootstrap']);
LMApp.directive('lmtable', function() {
    return {
        restrict: 'E',
        templateUrl: '/Templates/table.html',
    };
});
LMApp.service('dashboardService', function ($http, $q) {
    this.getProducts = function() {
        var deferred = $q.defer();
        $http({
                method: 'GET',
                url: 'api/dashboard/GetLast5Products'
            }).
            success(function(data, status, headers, config) {
                deferred.resolve(data);
            }).
            error(function(data, status) {
                deferred.reject(data);
            });
        return deferred.promise;
    };
    this.getCustomers = function () {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: 'api/dashboard/GetLast5Customers'
        }).
            success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).
            error(function (data, status) {
                deferred.reject(data);
            });
        return deferred.promise;
    };
    this.getLicenses = function () {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: 'api/dashboard/GetLast5Licenses'
        }).
            success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).
            error(function (data, status) {
                deferred.reject(data);
            });
        return deferred.promise;
    };
});