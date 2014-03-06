LMApp.controller("LicenseController", function ($scope, bootstrappedData) {
    $scope.customers = bootstrappedData.customers;
    $scope.products = bootstrappedData.products;
    $scope.selectedProduct = "";
    $scope.selectedCustomer = "";
    $scope.selectedProductId = function () {
        for (var x = 0, len = $scope.products.length; x < len; x += 1) {
            if ($scope.products[x].Name === $scope.selectedProduct) {
                console.info($scope.products[x]);
                return $scope.products[x].Id;
            }
        }
        return "null";
    };
    $scope.selectedCustomerId = function () {
        if ($scope.selectedCustomer == null) {
            return null;
        }
        return $scope.selectedCustomer.Id;
    };
    $scope.GetFilteredCustomers = function (viewValue) {
        var returnlist = [];
        $scope.customers.forEach(function (cust) {
            var hasLastName = (cust.LastName.toLowerCase().indexOf(viewValue.toLowerCase()) !== -1);
            var hasFirstName = (cust.FirstName.toLowerCase().indexOf(viewValue.toLowerCase()) !== -1);
            if (hasFirstName || hasLastName) {
                returnlist.push(cust);
            }
        });
        return returnlist;
    };
});