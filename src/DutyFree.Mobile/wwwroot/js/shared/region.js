(function () {
    app.controller("regionCtrl", function ($rootScope, $scope, $http, $timeout) {
        $scope.regions = {};
        $scope.size = 0;
        $scope.areas = {};

        $scope.init = function (regions) {
            $scope.regions = regions;
            $scope.size = _.keys(regions).length;
            $scope.refreshAreas();
        };

        $scope.show = function () {
            $('#region').modal("show");
        };

        $scope.hide = function () {
            $('#region').modal("hide");
        };

        $scope.refreshAreas = function () {
            if (_.isEmpty($scope.regions)) {
                var url = _basePath + "/api/region/getNextNodes";
                $http.post(url).then(function (response) {
                    $scope.areas[0] = response.data;
                    $scope.showLastTab();
                });
            }
            else {
                $.each($scope.regions, function (index, value) {
                    var url = _basePath + "/api/region/GetParallelNodes?regionId=" + value.Id;
                    $http.post(url).then(function (response) {
                        $scope.areas[index] = response.data;

                        if (index == ($scope.size - 1)) {
                            $scope.showLastTab();
                        }
                    });
                });
            }
        };

        $scope.showLastTab = function () {
            $timeout(function () {
                $scope.show();
                $('#region-tab a:last').tab('show');
            }, 100);
        };

        $scope.hasNext = function () {
            return _.keys($scope.areas).length > _.keys($scope.regions).length;
        };

        $scope.selectRegion = function (region, level) {
            level = parseInt(level);
            $scope.regions[level] = region;

            var url = _basePath + "/api/region/getNextNodes?regionId=" + region.Id;
            $http.post(url).then(function (response) {
                for (var i = level + 1; i < $scope.size; i++) {
                    delete $scope.regions[i];
                    delete $scope.areas[i];
                }

                $scope.size = level + 1;

                //有下一个
                if (response.data.length > 0) {
                    $scope.areas[level + 1] = response.data;
                    $scope.showLastTab();
                }
                else {
                    $rootScope.$broadcast("updateRegions", $scope.regions);
                    $timeout($scope.hide, 800);
                }
            });
        };

        $scope.$on('initRegion', function (event, data) {
            $scope.init(data);
        });

        $scope.$on('showRegion', function (event, data) {
            $scope.show();
        });
    });
}());