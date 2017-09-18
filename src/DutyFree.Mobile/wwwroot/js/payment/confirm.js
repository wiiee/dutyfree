$(function () {
    setTimeout(function () {
        window.location = _basePath + "/Home";
    }, 6000);

    setInterval(function () {
        var second = $("#second").text();
        $("#second").text(second - 1);
    }, 1000);
});