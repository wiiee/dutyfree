(function () {
    app.controller("searchCtrl", function ($rootScope, $scope, $http, $timeout, $interval) {
        $scope.addToCart = function (productId) {
            dutyFree.utility.addToCart(_userId, $http, $rootScope, $scope, productId);
        };

        $scope.showModal = function () {
            $('#cartMessage').modal("show");
            $timeout(function () {
                $('#cartMessage').modal("hide");
            }, 1000);
        };

        $interval(function () {
            $rootScope.$broadcast("initTab", 0)
        }, 100, 10);
    });
}());