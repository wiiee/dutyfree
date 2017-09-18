(function () {
    app.controller("personalCtrl", function ($rootScope, $scope, $http, $interval) {
        $interval(function () {
            $rootScope.$broadcast("initTab", 3)
        }, 100, 10);
    });
}());