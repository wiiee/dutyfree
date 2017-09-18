(function () {
    app.controller("providerCtrl", function ($rootScope, $scope, $http, $timeout, $interval, $q) {
        $interval(function () {
            $rootScope.$broadcast("initTab", 0)
        }, 100, 10);

        $scope.isValid = false;
        $scope.isTrustUser = _isTrustUser;
        $scope.airports = [
            { Id: "MEL", Name: "墨尔本机场" },
            { Id: "SYD", Name: "悉尼机场" },
            { Id: "SEL", Name: "仁川国际机场" },
            { Id: "DXB", Name: "迪拜国际机场" },
            { Id: "TYO", Name: "东京机场" },
            { Id: "SEA", Name: "西雅图机场" },
            { Id: "BKK", Name: "曼谷机场" },
            { Id: "AKL", Name: "奥克兰机场" },
            { Id: "NYC", Name: "纽约机场" }
        ];

        $scope.flightNos = [];
        $scope.isError = false;
        $scope.isEmpty = false;
        $scope.errorMsg = "";

        $scope.init = function () {
            if ($scope.isTrustUser) {
                $scope.isValid = true;
            }
            else {
                $.validate();
                var url1 = _basePath + "/api/session/AirportId";
                var url2 = _basePath + "/api/session/FlightNo";
                $q.all([
                    $http.get(url1),
                    $http.get(url2)
                ]).then(function (responses) {
                    if (responses[0].data && responses[0].data !== "null") {
                        //机场和航班都有
                        if (responses[1].data && responses[1].data !== "null") {
                            $scope.airport = _.findWhere($scope.airports, { Id: responses[0].data });
                            $scope.flightNo = responses[1].data;

                            var url = _basePath + "/api/flight/getFlightNos?airportId=" + $scope.airport.Id;

                            $http.post(url).then(function (response) {
                                if (response.data && response.data.length > 0) {
                                    $scope.flightNos = response.data;

                                    url = _basePath + "/api/flight/IsFlightValid?" + "&flightNo=" + $scope.flightNo;

                                    $http.post(url).then(function (response) {
                                        if (response.data.ErrorMsg) {
                                            $scope.isError = true;
                                            $scope.errorMsg = response.data.ErrorMsg;
                                            $scope.isValid = false;
                                        }
                                        else {
                                            $scope.isValid = true;
                                        }
                                    });
                                }
                                else {
                                    $scope.flightNos = [];
                                    $scope.flightNo = "";
                                    $scope.isError = true;
                                    $scope.errorMsg = "该机场没有今天出发的航班";
                                    $scope.isValid = false;
                                }
                            });
                        }
                        //只有机场
                        else {
                            $scope.airport = _.findWhere($scope.airports, { Id: responses[0].data });
                            var url = _basePath + "/api/flight/getFlightNos?airportId=" + $scope.airport.Id;

                            $http.post(url).then(function (response) {
                                if (response.data && response.data.length > 0) {
                                    $scope.flightNos = response.data;
                                    $scope.isError = false;
                                    $scope.errorMsg = "";
                                    $scope.isValid = false;
                                }
                                else {
                                    $scope.flightNos = [];
                                    $scope.flightNo = "";
                                    $scope.isError = true;
                                    $scope.errorMsg = "该机场没有今天出发的航班";
                                    $scope.isValid = false;
                                }
                            });
                        }
                    }
                });
            }
        };

        $scope.init();

        $scope.acceptOrder = function (providerOrderId) {
            if ($scope.isValid) {
                if (_userId) {
                    window.location = _basePath + "/Cart/AcceptOrder?providerOrderId=" + providerOrderId + "&flightNo=" + $scope.flightNo;
                }
                else {
                    var url = _basePath + "/api/session/ProviderOrderId?value=" + providerOrderId;
                    $http.put(url).then(function (response) {
                        window.location = "/Account/Login";
                    });
                }
            }
            else {
                if ($scope.airport) {
                    $("#flightNo").focus();
                    $('#flightNo').validate(function (valid, elem) {
                    });
                }
                else {
                    $("#airport").focus();
                    $('#airport').validate(function (valid, elem) {
                    });
                }
            }
        };

        $scope.changeAirport = function () {
            $('#airport').validate(function (valid, elem) {});

            var url = _basePath + "/api/flight/getFlightNos?airportId=" + $scope.airport.Id;

            $http.post(url).then(function (response) {
                if (response.data && response.data.length > 0) {
                    $scope.flightNos = response.data;
                    $scope.isError = false;
                    $scope.errorMsg = "";
                    $scope.isValid = false;
                }
                else {
                    $scope.flightNos = [];
                    $scope.flightNo = "";
                    $scope.isError = true;
                    $scope.errorMsg = "该机场没有今天出发的航班";
                    $scope.isValid = false;
                }
            });

            //更新Session-AirportId
            url = _basePath + "/api/session/AirportId?value=" + $scope.airport.Id;
            $http.put(url);

            //删除Session-FlightNo
            url = _basePath + "/api/session/FlightNo";
            $http.delete(url);
        };

        $scope.changeFlightNo = function () {
            $('#flightNo').validate(function (valid, elem) { });

            $scope.isError = false;
            $scope.errorMsg = "";

            if ($scope.flightNo) {
                var url = _basePath + "/api/flight/IsFlightValid?" + "&flightNo=" + $scope.flightNo;

                $http.post(url).then(function (response) {
                    if (response.data.ErrorMsg) {
                        $scope.isError = true;
                        $scope.errorMsg = response.data.ErrorMsg;
                    }
                    else {
                        $scope.isValid = true;
                        $scope.isError = false;
                        $scope.errorMsg = "";
                    }
                });
            }

            //更新Session-FlightNo
            url = _basePath + "/api/session/FlightNo?value=" + $scope.flightNo;
            $http.put(url);
        };
    });
}());