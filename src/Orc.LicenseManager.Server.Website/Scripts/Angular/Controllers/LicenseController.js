licenseModule.controller("LicenseController", function ($scope, bootstrappedData) {
    $scope.customers = bootstrappedData.customers;
    $scope.products = bootstrappedData.products;
});