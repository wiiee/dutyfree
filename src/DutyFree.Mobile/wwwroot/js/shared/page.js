app.factory("pageService", function ($http, $compile) {
    var instance = {
        pageIndex: 0,
        pageSize: 10,
        container: null,
        hasNext: true,
        isBottom: false,
        url: "",
        scope: null
    };

    instance.nextPage = function () {
        if (instance.hasNext) {
            var url = instance.url + "&pageIndex=" + instance.pageIndex + "&pageSize=" + instance.pageSize;

            $http.post(url).then(function (response) {
                instance.isBottom = false;
                if (response.data && response.data.trim()) {
                    var template = $compile(angular.element(response.data))(instance.scope);
                    instance.container.append(template);
                    instance.pageIndex++;
                }
                else {
                    instance.hasNext = false;
                    instance.isBottom = false;
                    $(window).unbind("scroll", instance.scroll);
                }
            });
        }
    };

    instance.scroll = function () {
        if ($(window).scrollTop() + $(window).height() == $(document).height()) {
            instance.isBottom = true;
            instance.nextPage();
        }
    };

    instance.init = function (scope, url, container, pageSize) {
        instance.scope = scope;
        instance.url = url;
        instance.container = container;

        if (pageSize) {
            instance.pageSize = pageSize;
        }

        instance.nextPage();
        $(window).bind("scroll", instance.scroll);
    };

    return instance;
});