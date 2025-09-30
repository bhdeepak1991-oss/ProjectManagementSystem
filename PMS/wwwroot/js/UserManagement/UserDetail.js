var _userDetailListLink = "/UserDetail/UserDetails";
var _createUserLink = "/UserDetail/CreateUser";

$(document).ready(function () {
    userDetail.fn_GetUserDetailList();
});
var userDetail = {
    "fn_GetUserDetailList": function () {
        $.get(_userDetailListLink, function (data) {
            $("#divUserList").html(data);
            commanAjax.fn_SetActiveFirstRow("#tblUsers");
        });
    },
    "fn_AddUser": function () {
        $("#divUserList").addClass('ajaxLoading');
        $("#modalTitle").text("Add User Details");
        $.get(_createUserLink, function (data) {
            if (data.search("sessionModel") > 0) {
                location.reload();
            }
            $("#divPartialData").html(data);
        }).done(function () {
            $("#THSCommanModalPopUp").modal('show');
            $("#modalSize").removeClass("modal-dialog lg").addClass("modal-dialog md");
            $("#btnClose").bind("click",
                function () {
                    userDetail.fn_GetUserDetailList();
                });
            $("#divUserList").removeClass('ajaxLoading');
        });
    },
    "fn_UpdateUser": function (id) {
        commanAjax.Fn_AjaxBegin('divUserList');
        $("#divUserList").addClass('ajaxLoading');
        $("#modalTitle").text("Edit User Details");
        $.get(_createUserLink, { id: id }, function (data) {
            $("#divPartialData").html(data);
        }).done(function () {
            $("#THSCommanModalPopUp").modal('show');
            $("#modalSize").removeClass("modal-dialog lg").addClass("modal-dialog md");
            $("#btnClose").bind("click",
                function () {
                    userDetail.fn_GetUserDetailList();
                });
            $("#divUserList").removeClass('ajaxLoading');
        });
    },
    "fn_closePopUp": function () {
        userDetail.fn_GetUserDetailList();
        $("#THSCommanModalPopUp").modal('hide');
    },
    "fn_Success": function (response) {
        //debugger;
        if (response.messageId == -1) {
            commanAjax.Fn_CommanAlert(response.message, 'Error');
        }
        else {
            commanAjax.Fn_CommanAlert(response.message, 'Alert!');
            if (response.messageId > 0) {
                $("#THSCommanModalPopUp").modal('hide');
                userDetail.fn_GetUserDetailList();
            } else {
                $("#UserName").val('');
            }
        }

    },
    "fn_ValidateEmail": function (userId) {
        var isPresist = true;
        $.get("/Authenticate/CheckSessionPresist", function (data) {
            if (!data) {
                isPresist = false;
                location.reload();
            }
        });
        if (isPresist) {
            var emailId = $("#EmailId").val();
            $.get("/UserDetail/ValidateEmailId", { emailId: emailId, userId: userId }, function (response) {
                if (response.length > 0) {
                    commanAjax.Fn_CommanAlert(response, 'Alert!');
                    $("#EmailId").val('');
                    $("#EmailId").removeClass().addClass("form-control input-validation-error");
                }
            });
        }
    },
    "fn_SearchBegin": function () {
        $("#divUserSearch").addClass('ajaxLoading');
    },
    "fn_searchComplete": function () {
        commanAjax.fn_SetActiveFirstRow("#tblUsers");
        $("#divUserSearch").removeClass('ajaxLoading');
    },
};
var masking = {
    "fn_phonemasking": function () {
        $(".phone").mask("(999) 999-9999");


        $(".phone").on("blur", function () {
            var last = $(this).val().substr($(this).val().indexOf("-") + 1);

            if (last.length == 5) {
                var move = $(this).val().substr($(this).val().indexOf("-") + 1, 1);

                var lastfour = last.substr(1, 4);

                var first = $(this).val().substr(0, 9);

                $(this).val(first + move + '-' + lastfour);
            }
        });
    }
};