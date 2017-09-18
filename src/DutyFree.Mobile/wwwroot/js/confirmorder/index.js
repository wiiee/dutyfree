$(function () {

});

(function () {
    app.controller("confirmOrderCtrl", function ($rootScope, $scope, $http) {
        $scope.preference = {};
        $scope.products = {};
        $scope.totalCount = 0;

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

        $scope.removeCart = function (productId) {
            _.omit($scope.preference.Carts, productId);

            this.updatePreference();
        };

        $scope.getProducts = function () {
            var url = _basePath + "/api/product/getProducts";
            var productIds = [];

            $.each($scope.preference.Carts, function (key, value) {
                productIds.push(key);
                $scope.totalCount += value;
            });

            $http.post(url, productIds).then(function (response) {
                $scope.products = response.data;

                $rootScope.$broadcast("totalPrice", $scope.getTotalCartPrice());
            });
        };

        $scope.updatePreference = function () {
            var url = _basePath + "/api/preference";
            $http.post(url, $scope.preference);
        };

        $scope.getTotalCartPrice = function () {
            var total = 0;

            $.each($scope.preference.Carts, function (key, value) {
                if (!_.isEmpty($scope.products)) {
                    total += $scope.products[key].Price * value;
                }
            });

            return total;
        };

        $scope.init();
    });
}());