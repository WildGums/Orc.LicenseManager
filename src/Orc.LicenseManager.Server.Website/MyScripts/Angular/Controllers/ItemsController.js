LMApp.controller("ItemsController", function ($scope, bootstrappedData, $filter, ngTableParams) {
    $scope.items = [];
    $scope.data = bootstrappedData.items;
    $scope.clipboardText = "";
    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 5,           // count per page
        filter: {
        },
        sorting: {
        }
    }, {
        total: $scope.data.length, // length of data
        getData: function ($defer, params) {
            var orderedData = params.sorting() ? $filter('orderBy')($scope.data, params.orderBy()) : $scope.data;
            orderedData = params.filter() ? $filter('filter')(orderedData, params.filter()) : orderedData;
            $scope.items = orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
            $defer.resolve($scope.items);
        }
    });
});
