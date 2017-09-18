(function () {
    angular.module("property", [])
    .controller("propertyCtrl", function ($scope, $http) {
        $scope.unitValueTypes = _unitValueTypes;
        $scope.items = [];
        $scope.oriItems = [];
        $scope.size = 0;
        $scope.units = [];

        var url = _basePath + "/api/setting/getUnits";
        $http.post(url).then(function (response) { 
            $scope.units = response.data;
        });

        $scope.validate = function () {
            setTimeout(function () {
                $.validate();
            }, 300);
        };

        $scope.addItem = function () {
            $scope.items.push({ Index: $scope.size, unitsSize: 0});
            $scope.validate();
            $scope.size++;
        };

        url = _basePath + "/api/setting/getProperties";
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

                    var element = $scope.items[i];
                    element.unitsSize = element.Units.length;

                    for (var j = 0; j < element.Units.length; j++) {
                        element.Units[j].Index = j;
                    }
                }

                $scope.validate();
            }
        });

        $scope.removeItem = function (index) {
            $scope.items = _.reject($scope.items, { Index: index });
        };

        $scope.addUnit = function (item) {
            if (!item.Units) {
                item.Units = [];
            }

            item.Units.push({
                Index: item.unitsSize,
                Key: {},
                Value: {
                    Values: []
                }
            });

            item.unitsSize++;
        };

        $scope.removeUnit = function (item, index) {
            item.Units = _.reject(item.Units, { Index: index })
        };

        $scope.addValue = function (unit) {
            unit.Value.Values.push("");
        };

        $scope.reset = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.items = angular.copy($scope.oriItems);
        };

        $scope.submit = function () {
            if ($("form").isValid()) {
                var url = _basePath + "/api/setting/postProperties";
                $http.post(url, $scope.items).then(function (response) {
                    location.reload();
                });
            }
        };
    });
}());