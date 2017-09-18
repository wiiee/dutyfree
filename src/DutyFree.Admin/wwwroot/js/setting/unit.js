(function () {
    angular.module("unit", [])
    .controller("unitCtrl", function ($scope, $http) {
        $scope.items = [];
        $scope.oriItems = [];
        $scope.size = 0;

        var url = _basePath + "/api/setting/getUnits";

        $scope.validate = function () {
            setTimeout(function () {
                $.validate();
            }, 300);
        };

        $scope.addItem = function () {
            $scope.items.push({ Index: $scope.size});
            $scope.validate();
            $scope.size++;
        };

        $http.post(url).then(function (response) {
            $scope.items = response.data;
            $scope.size = $scope.items.length;
            $scope.oriItems = angular.copy($scope.items);

            if ($scope.size === 0) {
                $scope.addItem();

                $scope.validate();
            }
            else {
                for (var i = 0 ; i < $scope.size; i++) {
                    $scope.items[i].Index = i;
                }

                $scope.validate();
            }
        });

        $scope.removeItem = function (index) {
            $scope.items = _.reject($scope.items, { Index: index });
        };

        $scope.reset = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.items = angular.copy($scope.oriItems);
        };

        $scope.submit = function () {
            if ($("form").isValid()) {
                var url = _basePath + "/api/setting/postUnits";
                $http.post(url, $scope.items).then(function (response) {
                    location.reload();
                });
            }
        };
    });
}());