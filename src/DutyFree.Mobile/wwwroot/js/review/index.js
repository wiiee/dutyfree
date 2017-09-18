$(function () {

});

(function () {
    app.controller("reviewCtrl", function ($scope, $http) {
        $scope.star = 0;

        $scope.select = function (i) {
            $scope.star = i;
        }

        $scope.isValid = function () {
            return $scope.star > 0 && $scope.text;
        };

        $scope.addReview = function () {
            if ($scope.isValid()) {
                var url = _basePath + "/api/review/addReview?productId=" + _productId + "&orderInfoId=" + _orderInfoId;
                var comment = {
                    Star: $scope.star,
                    Content: $scope.text
                };
                $http.post(url, comment).then(function (response) {
                    dutyFree.utility.back();
                });
            };
        };
    });
}());