$(function () {

});

(function () {
    app.controller("providerCtrl", function ($rootScope, $interval) {
        $interval(function () {
            $rootScope.$broadcast("initTab", 2)
        }, 100, 10);
    });
}());