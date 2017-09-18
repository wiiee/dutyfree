$(function () {

});

(function () {
    app.controller("addressCtrl", function ($rootScope, $scope, $http) {
        $scope.regions = {};
        $scope.address = _address;
        $scope.isRegionInited = false;

        $scope.selectAddress = function (index) {
            var url = _basePath + "/api/user/defaultAddress?index=" + index;
            $http.post(url).then(function (response) {
                dutyFree.utility.back();
            });
        };

        $scope.addAddress = function () {
            if ($scope.isValid()) {
                var url = _basePath + "/api/user/addAddress?isDefault=true";
                $http.post(url, $scope.address).then(function (response) {
                    location = _basePath + "/ConfirmOrder";
                });
            }
        };

        $scope.saveAddress = function () {
            if ($scope.isValid()) {
                var url = _basePath + "/api/user/saveAddress?index=" + _index;
                $http.post(url, $scope.address).then(function (response) {
                    location = _basePath + "/ConfirmOrder";
                });
            }
        };

        $scope.getRegions = function () {
            $.each($scope.address.Regions, function (index, value) {
                $scope.regions[index] = {
                    Id: value.Key,
                    Name: value.Value
                };
            });
        };

        $scope.getRegions();

        $scope.editRegion = function () {
            if (!$scope.isRegionInited) {
                $rootScope.$broadcast("initRegion", $scope.regions);
                $scope.isRegionInited = true;
            }
            else {
                $rootScope.$broadcast("showRegion", $scope.regions);
            }
        };

        $scope.updateRegions = function (regions) {
            $scope.address.Regions = [];
            $.each(regions, function (index, value) {
                $scope.address.Regions.push({
                    Key: value.Id,
                    Value: value.Name
                });
            });
        };

        $scope.$on('updateRegions', function (event, data) {
            $scope.updateRegions(data);
        });

        $scope.getRegion = function () {
            var result = "";

            $.each($scope.address.Regions, function (index, value) {
                result += value.Value + " ";
            });

            return result;
        };

        $scope.isValid = function () {
            return $scope.address.Name && $scope.address.Phone && $scope.address.Text && $scope.address.Regions.length > 0;
        };
    });
}());