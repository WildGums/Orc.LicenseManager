LMApp.controller("DashboardController", function ($scope, dashboardService) {
    $scope.products = {};
    $scope.licenses = {};
    $scope.customers = {};
    dashboardService.getProducts()
        .then(function (data) {
            $scope.products = data;
    }, function (reason) {
        console.info(reason);
    });;
    dashboardService.getCustomers()
    .then(function (data) {
        $scope.customers = data;
    }, function (reason) {
        console.info(reason);
    });;
    dashboardService.getLicenses()
    .then(function (data) {
        $scope.licenses = data;
    }, function (reason) {
        console.info(reason);
    });;
});
