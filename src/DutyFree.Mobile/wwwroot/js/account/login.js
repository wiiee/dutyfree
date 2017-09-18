$(function () {
    $.validate({
        onSuccess: function ($form) {
            var rawPassword = $("#rawPassword").val();
            $("#password").val(dutyFree.utility.getMd5(rawPassword));
        }
    });
});