$(function () {

});

(function () {
    app.controller("cartCtrl", function ($rootScope, $scope, $http, $interval) {
        $scope.isReady = false;
        $scope.preference = {};
        $scope.products = {};
        $scope.productIds = {};
        $scope.orderQuantity = 0;
        $scope.totalPrice = 0;
        $scope.isSelectAll = true;

        $interval(function () {
            $rootScope.$broadcast("initTab", 2)
        }, 100, 10);

        $scope.minusCount = function (productId, value) {
            if (value > 1) {
                $scope.preference.Carts[productId].Quantity--;
                $scope.updatePreference();

                $scope.updateOrder();
                $rootScope.$broadcast("minusCount", 1);
            }
        };

        $scope.plusCount = function (productId) {
            $scope.preference.Carts[productId].Quantity++;
            $scope.updatePreference();

            $scope.updateOrder();
            $rootScope.$broadcast("plusCount", 1);
        };

        $scope.getImgUrl = function (productId) {
            if (!_.isEmpty($scope.products)) {
                var imgId = $scope.products[productId].Logo;
                return dutyFree.utility.getImgUrl(imgId);
            }
        };

        $scope.init = function () {
            if (_userId) {
                var url = _basePath + "/api/preference/" + encodeURIComponent(_userId);
                $http.get(url).then(function (response) {
                    if (response.data) {
                        $scope.preference = response.data;
                    }
                    else {
                        $scope.preference = {
                            Carts: {}
                        };
                    }

                    $scope.getProducts();
                });
            }
        };

        $scope.calOrder = function () {
            $scope.orderQuantity = 0;

            $.each($scope.preference.Carts, function (key, value) {
                if (!value.IsSelected) {
                    $scope.productIds[key] = false;
                } else {
                    $scope.productIds[key] = true;
                    $scope.orderQuantity += value.Quantity;
                }
            });
        };

        $scope.calOrderQuantity = function () {
            $scope.orderQuantity = 0;

            $.each($scope.preference.Carts, function (key, value) {
                if (!_.isEmpty($scope.products) && $scope.productIds[key]) {
                    $scope.orderQuantity += value.Quantity;
                }
            });
        };

        $scope.calTotalPrice = function () {
            $scope.totalPrice = 0;

            $.each($scope.preference.Carts, function (key, value) {
                if (!_.isEmpty($scope.products) && $scope.productIds[key]) {
                    $scope.totalPrice += $scope.products[key].Price * value;;
                }
            });
        };

        $scope.calSelectAll = function () {
            $.each($scope.productIds, function (key, value) {
                if (!value) {
                    $scope.isSelectAll = false;
                    return false;
                }
            });
        };

        $scope.getProducts = function () {
            $scope.calOrder();
            $scope.calSelectAll();

            var url = _basePath + "/api/product/getProducts";

            $http.post(url, _.keys($scope.productIds)).then(function (response) {
                $scope.products = response.data;
                $scope.calTotalPrice();

                $scope.isReady = true;
            });
        };

        $scope.updatePreference = function () {
            var url = _basePath + "/api/preference";
            $http.post(url, $scope.preference);
        };

        $scope.selectAll = function () {
            $.each($scope.productIds, function (key, value) {
                $scope.productIds[key] = $scope.isSelectAll;
            });

            $scope.unselectProductIds();
        };

        $scope.selectProduct = function (productId) {
            $scope.unselectProductIds();
        };

        $scope.unselectProductIds = function () {
            var productIds = [];
            $.each($scope.productIds, function (key, value) {
                if (!value) {
                    productIds.push(key);
                }
            });

            var url = _basePath + "/api/preference/unselectProducts";
            $http.post(url, productIds);

            $scope.updateOrder();
        };

        $scope.removeCart = function (productId) {
            bootbox.confirm("确定要从购物车中移除?", function (result) {
                if (result) {
                    $rootScope.$broadcast("minusCount", $scope.preference.Carts[productId]);
                    $scope.preference.Carts = _.omit($scope.preference.Carts, productId);
                    $scope.updatePreference();
                    $scope.updateOrder();
                }
            });
        };

        $scope.updateOrder = function () {
            $scope.calOrderQuantity();
            $scope.calTotalPrice();
        };

        $scope.confirmOrder = function () {
            if ($scope.orderQuantity !== 0) {
                location = _basePath + "/ConfirmOrder";
            }
        };

        $scope.init();
    });
}());