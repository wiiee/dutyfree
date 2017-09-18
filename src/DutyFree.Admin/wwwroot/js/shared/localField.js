angular
    .module('localField', [])
    .controller('localFieldCtrl', function () {

    }).directive('localField', function () {
        return {
            restrict: 'E',
            templateUrl: '../template/localField.html',
            controller: 'localFieldCtrl'
        };
    });