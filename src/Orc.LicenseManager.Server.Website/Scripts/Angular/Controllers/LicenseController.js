licenseModule.controller("LicenseController", function ($scope, bootstrappedData) {
    $scope.customers = bootstrappedData.customers;
    $scope.products = bootstrappedData.products;
    $scope.selectedProduct = "";
    $scope.selectedCustomer = "";
    $scope.selectedProductId = function() {
        for (var x = 0, len = $scope.products.length; x < len; x += 1) {
            if ($scope.products[x].Name === $scope.selectedProduct) {
                console.info($scope.products[x]);
                return $scope.products[x].Id;
            }
        }
        return "null";
    };
    $scope.selectedCustomerId = function () {
        for (var x = 0, len = $scope.customers.length; x < len; x += 1) {
            if ($scope.customers[x].FirstName === $scope.selectedCustomer) {
                console.info($scope.customers[x]);
                return $scope.customers[x].Id;
            }
        }
        return "null";
    };
});