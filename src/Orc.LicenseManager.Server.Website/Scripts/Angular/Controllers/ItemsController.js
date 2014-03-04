LMApp.controller("ItemsController", function ($scope, bootstrappedData, $filter, ngTableParams) {
    $scope.items = [];
    $scope.data = bootstrappedData.items;
    $scope.SetClipBoardText = function (obj, text) {
        //obj.zclip({
        //    path: "//cdnjs.cloudflare.com/ajax/libs/zclip/1.1.2/ZeroClipboard.swf",
        //    copy: text
        //});
        $scope.clipboardText = text;

    };
    $scope.clipboardText = "okzor";
    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 5,           // count per page
        filter: {
            Name: ''       // initial filter
        },
        sorting: {
            Name: 'asc'     // initial sorting
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
