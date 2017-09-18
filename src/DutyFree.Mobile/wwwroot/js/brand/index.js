(function () {
    app.controller("brandCtrl", function ($rootScope, $scope, $http, $timeout, $interval, pageService) {
        $scope.isTop = true;

        $scope.init = function () {
            var url = _basePath + "/brand/getPage?brandId=" + _brandId + "&";
            var container = $("#products");
            pageService.init($scope, url, container);
        };

        $scope.init();
        $interval(function () {
            $rootScope.$broadcast("initTab", 2)
        }, 100, 10);


        $scope.isLoading = function () {
            return pageService.isBottom;
        }

        $interval(function(){
            $scope.isTop = $(window).scrollTop() === 0;
        }, 1000);

        $scope.addToCart = function (productId) {
            dutyFree.utility.addToCart(_userId, $http, $rootScope, $scope, productId);
        };

        $scope.showModal = function () {
            $('#cartMessage').modal("show");
            $timeout(function () {
                $('#cartMessage').modal("hide");
            }, 1000);
        };
    });
}());