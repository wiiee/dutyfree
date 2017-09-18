(function () {
    app.controller("categoryCtrl", function ($rootScope, $scope, $http, $interval) {
        $scope.currentItem = "Recommend";
        $scope.tab = 0;

        $scope.getItem = function (item, tab) {
            $scope.tab = tab;
            if (item !== $scope.currentItem) {
                $scope.currentItem = item;
                var url = _basePath + "/category/getItem?item=" + item;
                $http.post(url).then(function (response) {
                    $("#right").html(response.data);
                });
            }
        };

        $interval(function () {
            $rootScope.$broadcast("initTab", 1)
        }, 100, 10);
    });
}());