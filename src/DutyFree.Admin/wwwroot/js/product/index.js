$(function () {

});

(function () {
    angular.module('products', ['smart-table', 'ui.bootstrap'])
        .controller('productsCtrl', function ($rootScope, $scope, $http, $modal) {
            $scope.rowCollection = [];
            $scope.itemsByPage = 100;

            $scope.isCollapsed = true;
            $scope.searchTexts = ["Show Condition", "Hide Condition"];
            $scope.searchText = $scope.searchTexts[0];
            $scope.currentTextIndex = 0;

            var url = _basePath + "/api/Product";

            $http.get(url).then(function (response) {
                $scope.rowCollection = response.data;
            });

            $scope.switchSearch = function () {
                $scope.isCollapsed = !$scope.isCollapsed;
                $scope.currentTextIndex = ($scope.currentTextIndex + 1) % 2;
                $scope.searchText = $scope.searchTexts[$scope.currentTextIndex];
            };

            $scope.deleteProject = function (projectId) {
                bootbox.confirm("Are you sure to delete this project?", function (result) {
                    if (result) {
                        url = _basePath + "/api/Project/" + projectId;
                        $http.delete(url).then(function (response) {
                            if (response.data.successMsg) {
                                var msg = $("#successMsg").html();
                                msg = msg.replace("replaceText", response.data.successMsg);
                                $("#messageContainer").append(msg);

                                var index = _.findIndex($scope.rowCollection, { "Id": projectId });
                                $scope.rowCollection.splice(index, 1);

                                //index = _.findIndex($scope.displayCollection, { "Id": projectId });
                                //$scope.displayCollection.splice(index, 1);
                            }
                            else if (response.data.errorMsg) {
                                var msg = $("#errorMsg").html();
                                msg = msg.replace("replaceText", response.data.errorMsg);
                                $("#messageContainer").append(msg);
                            }
                        });
                    }
                });
            };
        })
        .directive('pageSelect', function () {
            return {
                restrict: 'E',
                template: '<input type="text" class="form-control" ng-model="inputPage" ng-change="selectPage(inputPage)">',
                link: function (scope, element, attrs) {
                    scope.$watch('currentPage', function (c) {
                        scope.inputPage = c;
                    });
                }
            }
        })
        .directive("stResetSearch", function () {
            return {
                restrict: 'EA',
                require: '^stTable',
                link: function (scope, element, attrs, ctrl) {
                    return element.bind('click', function () {
                        return scope.$apply(function () {
                            var tableState;
                            tableState = ctrl.tableState();
                            tableState.search.predicateObject = {};
                            tableState.pagination.start = 0;
                            return ctrl.pipe();
                        });
                    });
                }
            };
        })
        .filter('customFilter', function ($filter) {
            var filterFilter = $filter('filter');
            var standardComparator = function standardComparator(obj, text) {
                text = ('' + text).toLowerCase();
                return ('' + obj).toLowerCase().indexOf(text) > -1;
            };

            return function customFilter(array, expression) {
                function customComparator(actual, expected) {
                    var isBeforeActivated = expected.before;
                    var isAfterActivated = expected.after;
                    var isLower = expected.lower;
                    var isHigher = expected.higher;
                    var higherLimit;
                    var lowerLimit;
                    var itemDate;
                    var queryDate;

                    if (angular.isObject(expected)) {

                        //date range
                        if (expected.before || expected.after) {
                            try {
                                if (isBeforeActivated) {
                                    higherLimit = expected.before;

                                    itemDate = new Date(actual);
                                    queryDate = new Date(higherLimit);

                                    if (itemDate > queryDate) {
                                        return false;
                                    }
                                }

                                if (isAfterActivated) {
                                    lowerLimit = expected.after;

                                    itemDate = new Date(actual);
                                    queryDate = new Date(lowerLimit);

                                    if (itemDate < queryDate) {
                                        return false;
                                    }
                                }

                                return true;
                            } catch (e) {
                                return false;
                            }

                        } else if (isLower || isHigher) {
                            //number range
                            if (isLower) {
                                higherLimit = expected.lower;

                                if (actual > higherLimit) {
                                    return false;
                                }
                            }

                            if (isHigher) {
                                lowerLimit = expected.higher;
                                if (actual < lowerLimit) {
                                    return false;
                                }
                            }

                            return true;
                        }
                        //etc
                        return true;

                    }
                    return standardComparator(actual, expected);
                }

                var output = filterFilter(array, expression, customComparator);
                return output;
            };
        });
}());