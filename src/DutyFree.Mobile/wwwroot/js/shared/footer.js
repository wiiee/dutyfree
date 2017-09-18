(function () {
    app.controller("headerCtrl", function ($scope) {
        $scope.text = "";

        $scope.search = function () {
            if ($scope.text) {
                var url = _basePath + "/search?text=" + $scope.text;
                location = url;
            }
        };
    });

    app.controller("footerCtrl", function ($rootScope, $scope, $http) {
        $scope.currentTab = -1;
        //购物车数量
        $scope.quantity = _cartQuantity;
        //机场购买订单数量
        $scope.providerOrderQuantity = _providerOrderQuantity;

        $scope.plusCount = function (number) {
            $scope.quantity += number;
        };

        $scope.minusCount = function (number) {
            $scope.quantity -= number;
        };

        $scope.$on('plusCount', function (event, data) {
            $scope.plusCount(data);
        });

        $scope.$on('minusCount', function (event, data) {
            $scope.minusCount(data);
        });

        $scope.$on('initTab', function (event, data) {
            $scope.currentTab = data;
        });

        $scope.go = function (path) {
            location = _basePath + "/" + path;
        };
    });
}());