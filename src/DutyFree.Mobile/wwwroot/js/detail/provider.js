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
    });
}());