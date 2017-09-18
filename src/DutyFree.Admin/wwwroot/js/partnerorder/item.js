$(function () {

});

(function () {
    angular.module("item", ['ui.bootstrap'])
    .controller("itemCtrl", function ($scope, $http, $timeout, $q) {
        $scope.item = {};
        $scope.itemId = _itemId;
        $scope.statuses = _statuses;

        $scope.initItem = function () {
            if($scope.itemId){
                var url = _basePath + "/api/providerOrder/" + $scope.itemId;
                $http.get(url).then(function (response) {
                    $scope.item = response.data;
                });
            }
        };

        $scope.initItem();

        $scope.reset = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.product = angular.copy($scope.oriProduct);
        };

        $scope.submit = function () {
            if ($("form").isValid()) {
                if ($scope.itemId) {
                    var url = _basePath + "/api/ProviderOrder";
                    $http.post(url, $scope.item).then(function (response) {
                        location = _basePath + "/ProviderOrder/Index";
                    });
                }
                else {
                    var url = _basePath + "/api/ProviderOrder";
                    $http.put(url, $scope.product).then(function (response) {
                        location = _basePath + "/ProviderOrder/Index";
                    });
                }
            }
        };
    });
}());