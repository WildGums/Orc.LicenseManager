var LMApp = angular.module('LMApp', ['ngTable', 'ui.bootstrap']);
LMApp.directive('lmtable', function () {
    return {
        restrict: 'E',
        templateUrl: '/Templates/table.html',
    };
});