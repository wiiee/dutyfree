(function () {
    angular.module("region", [])
    .controller("regionCtrl", function ($scope, $http) {
        $scope.source = function (id) {
            return {
                type: "GET",
                url: _basePath + "/api/region/" + id,
                dataType: "json",
                contentType: "application/json",
                error: function (XMLHttpRequest) {
                    alert(XMLHttpRequest.status + ': ' + XMLHttpRequest.responseText);
                }
            };
        };

        $scope.onSave = function (node) {
            $.validate();

            var url = _basePath + "/api/region";
            var type = $(node.$input[0]).prop('disabled') ? "POST" : "PUT";

            var data = {
                id: $(node.getId()).val(),
                name: $(node.getName()).val(),
                parentId: node.getParent()
            };

            return {
                type: type,
                url: url,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                error: function (XMLHttpRequest) {
                    alert(XMLHttpRequest.status + ': ' + XMLHttpRequest.responseText);
                }
            };
        };

        $scope.onDelete = function (node) {
            //return {
            //    type: "DELETE",
            //    url: _basePath + "/api/region/" + node.getId(),
            //    dataType: "json",
            //    contentType: "application/json",
            //    error: function (XMLHttpRequest) {
            //        alert(XMLHttpRequest.status + ': ' + XMLHttpRequest.responseText);
            //    }
            //};
        };

        $scope.init = function () {
            $('#region').gtreetable({
                "source": $scope.source,
                "onSave": $scope.onSave,
                "onDelete": $scope.onDelete,
                "cache": 2,
                "manyroots": true
            });
        };

        $scope.init();
    });
}());