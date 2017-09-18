(function () {
    angular.module("category", [])
    .controller("categoryCtrl", function ($scope, $http) {
        $scope.items = [];
        $scope.properties = [];
        $scope.oriItems = [];
        $scope.size = 0;

        var url = _basePath + "/api/setting/getProperties";
        $http.post(url).then(function(response){
            $scope.properties = response.data;
        });

        $scope.validate = function () {
            setTimeout(function () {
                $.validate();
                $(".propertyIds").multiselect({
                    buttonWidth: "100%",
                    onChange: function (option, checked) {
                        option.parent().blur();
                        //$("form").isValid();
                    }
                });
            }, 300);
        };

        $scope.addItem = function () {
            $scope.items.push({
                Index: $scope.size,
                Properties: {}
            });
            $scope.validate();
            $scope.size++;
        };

        url = _basePath + "/api/setting/getCategories";
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
                    $scope.items[i].PropertyIds = _.keys($scope.items[i].Properties);

                    var element = $scope.items[i];
                }

                $scope.validate();
            }
        });

        $scope.getCategoryIds = function (id) {
            var ids = _.pluck($scope.items, "Id");
            return _.without(ids, id);
        }

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
                var url = _basePath + "/api/setting/postCategories";

                $scope.items = _.map($scope.items, function (item) {
                    item.Properties = _.pick(item.Properties, item.PropertyIds);
                    return item;
                });

                $http.post(url, $scope.items).then(function (response) {
                    location.reload();
                });
            }
        };
    });
}());