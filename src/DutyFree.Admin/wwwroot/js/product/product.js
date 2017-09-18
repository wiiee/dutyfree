$(function () {

});

(function () {
    angular.module("product", ['ui.bootstrap'])
    .controller("productCtrl", function ($scope, $http, $timeout, $q) {
        $scope.categories = [];
        $scope.categoryChain = [];
        $scope.brands = [];
        $scope.brandIds = [];
        $scope.properties = [];
        $scope.propertyIds = [];
        $scope.product = {};
        $scope.oriProduct = {};
        $scope.productId = _productId;
        $scope.statuses = _statuses;
        $scope.sendingDatas = {};

        $scope.initFileInput = function () {
            var elements = [];
            elements.push({ id: "Logo", item: $("#logo") });
            elements.push({ id: "Imgs", item: $("#imgs") });

            if ($scope.productId) {
                $.each(elements, function (index, element) {
                    $(element.item).fileinput({
                        initialPreview: $scope.sendingDatas[element.id].initialPreviews,
                        initialPreviewConfig: $scope.sendingDatas[element.id].initialPreviewConfigs,
                        uploadExtraData: { productId: $scope.productId },
                        uploadUrl: _basePath + "/api/img/uploads",
                        overwriteInitial: $scope.productId ? false : true,
                        uploadAsync: false,
                        showUpload: false, // hide upload button
                        showRemove: false, // hide remove button
                        previewFileType: "image",
                        allowedFileExtensions: ["jpg", "png", "gif"],
                        resizePreference: 'height',
                        maxFileCount: 3,
                        maxFileSize: 100,
                        resizeImage: true
                    }).on("filebatchselected", function (event, files) {
                        // trigger upload method immediately after files are selected
                        $(element).fileinput("upload");
                    }).on('filebatchuploadsuccess', function (event, data) {
                        subProduct.ImgIds = [];
                        $.each(data.response.initialPreviewConfig, function (index, element) {
                            subProduct.ImgIds.push(element.key);
                        });
                    }).on("filepredelete", function (event, key) {
                        var abort = true;

                        if (confirm("Are you sure you want to delete this image?")) {
                            abort = false;
                        }

                        return abort; // you can also send any data/object that you can receive on `filecustomerror` event
                    }).on('filedeleted', function (event, key) {
                        //product.Logo = element.key;
                        subProduct.ImgIds = _.without(subProduct.ImgIds, key);
                    });
                });
            }
            else {
                $.each(elements, function (index, element) {
                    $(element.item).fileinput({
                        uploadExtraData: { productId: $scope.productId },
                        uploadUrl: _basePath + "/api/img/uploads",
                        overwriteInitial: $scope.productId ? false : true,
                        uploadAsync: false,
                        showUpload: false, // hide upload button
                        showRemove: false, // hide remove button
                        previewFileType: "image",
                        allowedFileExtensions: ["jpg", "png", "gif"],
                        resizePreference: 'height',
                        maxFileCount: 3,
                        maxFileSize: 100,
                        resizeImage: true
                    }).on("filebatchselected", function (event, files) {
                        // trigger upload method immediately after files are selected
                        $(element).fileinput("upload");
                    }).on('filebatchuploadsuccess', function (event, data) {
                        if(element.id === "Logo"){
                            $scope.product.Logo = data.response;
                        }
                        else{
                            $scope.product.ImgIds = [];
                            $.each(data.response.initialPreviewConfig, function (index, element) {
                                $scope.product.ImgIds.push(element.key);
                            });
                        }
                    }).on("filepredelete", function (event, key) {
                        var abort = true;

                        if (confirm("Are you sure you want to delete this image?")) {
                            abort = false;
                        }

                        return abort; // you can also send any data/object that you can receive on `filecustomerror` event
                    }).on('filedeleted', function (event, key) {
                        //product.Logo = element.key;
                        $scope.product.ImgIds = _.without($scope.product.ImgIds, key);
                    });
                });
            }
        };

        $scope.initCategoryChain = function (categoryId) {
            if (categoryId) {
                var url1 = _basePath + "/api/setting/GetCategoryChain?categoryId=" + categoryId;
                var url2 = _basePath + "/api/setting/GetCategoriesByChild?categoryId=" + categoryId;

                $q.all([
                    $http.post(url1),
                    $http.post(url2)
                ]).then(function (responses) {
                    $scope.categoryChain = responses[0].data;
                    $scope.product.CategoryId = responses[1].data;
                });
            }
            else {
                var url = _basePath + "/api/setting/GetCategoryChain?categoryId=" + categoryId;
                $http.post(url).then(function (response) {
                    $scope.categoryChain = response.data;
                });
            }
        };

        $scope.initProduct = function () {
            if ($scope.productId) {
                var url1 = _basePath + "/api/setting/getCategoryLinkedList";
                var url2 = _basePath + "/api/setting/getProperties";
                var url3 = _basePath + "/api/setting/getBrands";
                var url4 = _basePath + "/api/product/" + $scope.productId;
                var url5 = _basePath + "/api/product/GetSendingData?productId=" + $scope.productId;
                $q.all([
                    $http.post(url1),
                    $http.post(url2),
                    $http.post(url3),
                    $http.get(url4),
                    $http.post(url5)
                ]).then(function (responses) {
                    $scope.categories = responses[0].data;
                    $scope.categoryChain.push(_.pluck($scope.categories, "Id"));

                    $scope.properties = responses[1].data;
                    $scope.propertyIds = _.pluck($scope.properties, "Id");

                    $scope.brands = responses[2].data;
                    $scope.brandIds = _.pluck($scope.brands, "Id");

                    $scope.product = responses[3].data;
                    $scope.oriProduct = angular.copy($scope.product);
                    $scope.initCategoryChain($scope.product.CategoryId);

                    $scope.sendingDatas = responses[4].data;
                    $scope.initFileInput();
                });
            }
            else {
                var url1 = _basePath + "/api/setting/getCategoryLinkedList";
                var url2 = _basePath + "/api/setting/getProperties";
                var url3 = _basePath + "/api/setting/getBrands";
                $q.all([
                    $http.post(url1),
                    $http.post(url2),
                    $http.post(url3)
                ]).then(function (responses) {
                    $scope.categories = responses[0].data;
                    $scope.categoryChain.push(_.pluck($scope.categories, "Id"));

                    $scope.properties = responses[1].data;
                    $scope.propertyIds = _.pluck($scope.properties, "Id");

                    $scope.brands = responses[2].data;
                    $scope.brandIds = _.pluck($scope.brands, "Id");

                    $scope.initFileInput();
                    $scope.initCategoryChain();
                });
            }
        };

        $scope.initProduct();

        $scope.updateCategory = function (index) {
            $scope.categoryChain = _.first($scope.categoryChain, index + 1);
            //build category chain after index
            if ($scope.product.CategoryId[index]) {
                //找到next
                var next = $scope.findNext(index);
                if (next !== null) {
                    $scope.categoryChain.push(_.pluck(next, "Id"))
                }
            }
        };

        $scope.findNext = function (index) {
            var result = _.findWhere($scope.categories, {"Id": $scope.product.CategoryId[0]});
            var start = 1;

            while (start <= index) {
                result = _.findWhere(result.Nexts, {"Id": $scope.product.CategoryId[start]});
                start++;
            }

            return result.Nexts;
        };

        $scope.addProperty = function () {
            if (!$scope.product.Properties) {
                $scope.product.Properties = [];
            }

            $scope.product.Properties.push({});
        };

        $scope.addValue = function (property) {
            if (!property.Values) {
                property.Values = [];
            }

            property.Values.push({});
        };

        $scope.getUnits = function (id) {
            var property = _.findWhere($scope.properties, { Id: id });
            return _.pluck(property.Units, "Key");
        };

        $scope.reset = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.product = angular.copy($scope.oriProduct);
        };

        $scope.submit = function () {
            if ($("form").isValid()) {
                var size = _.keys($scope.product.CategoryId).length;
                $scope.product.CategoryId = $scope.product.CategoryId[size - 1];

                if ($scope.productId) {
                    var url = _basePath + "/api/Product";
                    $http.post(url, $scope.product).then(function (response) {
                        location = _basePath + "/Product/Index";
                    });
                }
                else {
                    var url = _basePath + "/api/Product";
                    $http.put(url, $scope.product).then(function (response) {
                        location = _basePath + "/Product/Index";
                    });
                }
            }
        };
    });
}());