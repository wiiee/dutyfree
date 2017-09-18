$(function () {

});

(function () {
    angular.module("item", ['ui.bootstrap'])
    .controller("itemCtrl", function ($scope, $http, $timeout, $q) {
        $scope.item = {};
        $scope.oriItem = {};
        $scope.itemId = _itemId;
        $scope.statuses = _statuses;
        $scope.products = [];

        $scope.initItem = function () {
            if($scope.itemId){
                var url = _basePath + "/api/orderInfo/" + $scope.itemId;
                $http.get(url).then(function (response) {
                    $scope.item = response.data;
                    $scope.oriItem = angular.copy($scope.item);

                    var url = _basePath + "/api/product/getProducts";
                    var productIds = _.keys($scope.item.ProductInfos);
                    $http.post(url, productIds).then(function (response) {
                        $scope.products = response.data;
                    });
                });
            }
        };

        $scope.getProduct = function(productId){
            return _.findWhere($scope.products, {Id: productId});
        };

        $scope.initItem();

        $scope.reset = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.item = angular.copy($scope.oriItem);
        };

        $scope.submit = function () {
            if ($("form").isValid()) {
                if ($scope.itemId) {
                    var url = _basePath + "/api/OrderInfo";
                    $http.post(url, $scope.item).then(function (response) {
                        location = _basePath + "/Order/Index";
                    });
                }
                else {
                    var url = _basePath + "/api/OrderInfo";
                    $http.put(url, $scope.product).then(function (response) {
                        location = _basePath + "/Order/Index";
                    });
                }
            }
        };
    });
}());