$(document).ready(function () {
    $("#dvFormSubmit").addClass("focus");
});
//window.addEventListener('load', function () {
//    debugger
//    $("#dvFormSubmit").addClass("focus");
//})
var authenticate = {
    "fn_BackToLogin": function () {
        $("#forgotPassword").addClass("hide");
        $("#loginFrame").removeClass("hide");
    },
    "fn_ForgetPassword": function () {

        $("#forgotPassword").removeClass("hide");
        $("#loginFrame").addClass("hide");
        $.get("/Authenticate/ForgetPasswordPartial", function (response) {
            $("#_divForgetPassword").html(response);
            setTimeout(function () {
                $('#EmailAddress').focus();
            },100)
            $("#_divForgetPassword").removeAttr("style");
            $("#_divLoginBody").hide();
            $("#_divFooter").hide();
        });
    },   
    "fn_ForgetPasswordStart": function () {
        $("#divForgetPassword").addClass('ajaxLoading');
    },
    "fn_ForgetPasswordComplete": function () {
        $("#divForgetPassword").removeClass('ajaxLoading');
    },
    "fn_Success": function (response) {
        commanAjax.Fn_CommanAlert(response, 'Alert!');
    }
}; 