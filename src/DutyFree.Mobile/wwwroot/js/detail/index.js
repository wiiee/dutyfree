$(function () {

});

(function () {
    var swipe = function () {
        $("#pics").swiperight(function () {
            $(this).carousel('prev');
        });
        $("#pics").swipeleft(function () {
            $(this).carousel('next');
        });
    };

    app.controller("detailCtrl", function ($rootScope, $scope, $http, $timeout) {
        swipe();

        $scope.showModal = function () {
            $('#cartMessage').modal("show");
            $timeout(function () {
                $('#cartMessage').modal("hide");
            }, 1000);
        };

        $scope.addToCart = function () {
            dutyFree.utility.addToCart(_userId, $http, $rootScope, $scope, _productId);
        };
    });

    app.controller("tabCtrl", function ($scope, $http) {
        $scope.tab = 0;

        $scope.select = function (tab) {
            if ($scope.tab !== tab) {
                var url = _basePath + "detail/getTab?tab=" + tab + "&productId=" + _productId;
                $http.post(url).then(function (response) {
                    $("#bodyContainer").html(response.data);
                    if (tab == 0) {
                        swipe();
                    }
                });
            }

            $scope.tab = tab;
        };
    });
}());