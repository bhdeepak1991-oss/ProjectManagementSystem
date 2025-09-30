var searchBy = "pat";
$(document).ready(function () {
    DisplayLoadingTillPageLoad();

    var url = (window.location.pathname).split("/");
    if (url[1] === "MacDrug") {
        GetSelectedSearchParam("mac");
    }
    else if (url[1] === "DrugList") {
        GetSelectedSearchParam("drug");
    }
    else if (url[1] === "DrugSearch") {
        GetSelectedSearchParam("drugNDC");
    } else {
        GetSelectedSearchParam(url[1].toLowerCase());
    }
    GetUrlPathName();
    $("#mainWrap").removeClass('ajaxLoading');
});
var userCommon = {
    "fn_CheckPassword": function () {
        $.get("/Authenticate/CheckSessionPresist", function (data) {
            if (!data) {
                location.reload();
            }
            else {
                if ($('#Password').val() != "") {
                    $.get("/Authenticate/CheckUserPassword", { password: $('#Password').val() }, function (response) {
                        if (response.messageId == 0) {
                            //alert(response.message);
                            commanAjax.Fn_CommanAlert(response.message, 'Alert!');
                            $('#Password').val('').focus();
                            $('#Password').removeClass('valid').addClass('input-validation-error');
                        } else {
                            $('#Password').removeClass('input-validation-error').addClass('valid');
                        }
                    });
                }
            }
        });
    },
    "fn_ChangePasswordComplete": function (response) {
        if (response.messageId == 1) {
            commanAjax.Fn_CommanAlert(response.message, 'Alert!');
            setTimeout(function () {
                RedirectToLogin();
            },
                5000);
        } else {
            $('#Password').val('');
            $('#NewPassword').val('');
            $('#ConfirmPassword').val('');
        }
    }
   
};
function RedirectToLogin() {
    location.href = "/Authenticate/Index";
}

function OmnieANotification(SuccessFail, Message, Header) {
    if (SuccessFail == 1) { //1 for Info
        $('#AlertHeader').html(Header);
        $('#lblEMessage').html(Message);
    }
    else if (SuccessFail == 2) { //2 for Success
        $('#AlertHeader').html(Header);
        $('#lblEMessage').html(Message);
    }
    else if (SuccessFail == 0) { //0 for Fail
        $('#AlertHeader').html(Header);
        $('#lblEMessage').html(Message);
    }

    $('#AlertModal').modal('show');
}

function OkFunction() {
    window.location.reload();

}

function CancelFunction() {

}

function OpenChangePasswordPartial() {
    $.get("/Authenticate/ChangePassword", function (response) {
        if (response.search("sessionModel") > 0) {
            location.reload();
        }
        else {
            $("#modalTitle").html("Change Password");
            $("#_divChangePassword").html(response);
            $("#ChangePasswordModal").modal('show');
        }

    }).catch(function (error) {
        console.log(error);
    });
}

function ExpandCollpseLi() {
    $("#_ulStoreList").toggle();
}

function MasterAdvanceSearch() {
    var searchBy = $("#lblSearchParams").text();
    switch (searchBy) {
        case "Claim":
            var nabp = $("#txtPharmacyID").val();
            var rxNumber = $("#txtRxNumber").val();
            var dateOfService = $("#txtDateOfService").val();
            var ndc = $("#txtNDC").val();
            var npi = $("#txtNPI").val();
            var group = $("#txtGroup").val();
            var cardHolderId = $("#txtCardHolderId").val();
            var gpi = $("#txtGPI").val();
            window.location.href = "/Claim/Index?NPI=" +
                npi +
                "&RxNumber=" +
                rxNumber +
                "&ProductServiceId=" +
                ndc +
                "&DateOfService=" +
                dateOfService +
                "&PharmacyId=" +
                nabp +
                "&Group=" +
                group +
                "&CardHolderId=" +
                cardHolderId +
                "&GPI=" +
                gpi;
            break;
        case "Client":
            $("#clilentSearch").removeAttr("style");
            var clientName = $("#txtClientName").val();
            window.location.href = "/Client/ClientList?strValue=" + clientName + "&id=95";
            break;
        case "Plan":
            $("#planSearch").removeAttr("style");
            var plan = $("#txtPlan").val();
            window.location.href = "/Plan/PlanList?strValue=" + plan + "&id=851";
            break;
        case "Formulary":
            $("#formularySearch").removeAttr("style");
            var formualry = $("#txtFormulaury").val();
            window.location.href = "/Formulary/FormularyIndex?strValue=" + formualry + "";
            break;
        case "MAC Drug List":
            $("#maxDrugList").removeAttr("style");
            var maxDrugList = $("#txtMacDrug").val();
            window.location.href = "/MacDrug/Index?strValue=" + maxDrugList + "";
            break;
        case "Drug List":
            $("#drugList").removeAttr("style");
            var drugList = $("#txtDrug").val();
            window.location.href = "/DrugList/Index?strValue=" + drugList;

            break;
        case "Drug NDC":
            $("#drugNDC").removeAttr("style");
            var drugNDC = $("#txtDrugNdc").val();
            window.location.href = "/DrugSearch/Index?strValue=" + drugNDC;
            break;

    }
}

function getSelectedRow() {
    var PrescriberId = $('#_prescriberSearchDiv').find('.selected input[type="hidden"]').val();
    window.location.href = "/Prescriber/BasicInformation/Create?prescriberId=" + PrescriberId;
    return true;
}

function GetSelectedSearchParam(id) {
    HideSearchDiv();
    switch (id) {
        case "client":
            $("#clilentSearch").removeAttr("style");
            $("#lblSearchParams").text("Client");
            break;
        case "plan":
            $("#planSearch").removeAttr("style");
            $("#lblSearchParams").text("Plan");
            break;
        case "formulary":
            $("#formularySearch").removeAttr("style");
            $("#lblSearchParams").text("Formulary");
            break;
        case "mac":
            $("#maxDrugList").removeAttr("style");
            $("#lblSearchParams").text("MAC Drug List");
            break;
        case "drug":
            $("#drugList").removeAttr("style");
            $("#lblSearchParams").text("Drug List");
            break;
        case "drugNDC":
            $("#drugNDC").removeAttr("style");
            $("#lblSearchParams").text("Drug NDC");
            break;
        default:
            $("#claimSearch").removeAttr("style");
            $("#lblSearchParams").text("Claim");

    }

    $('.header-search.open').find('.header-search-wrap:visible .search-inner .search-fields>div:visible input:first').focus();
}

function HideSearchDiv() {
    $("#claimSearch").attr("style", "display:none");
    $("#clilentSearch").attr("style", "display:none");
    $("#planSearch").attr("style", "display:none");
    $("#formularySearch").attr("style", "display:none");
    $("#maxDrugList").attr("style", "display:none");
    $("#drugList").attr("style", "display:none");
    $("#drugNDC").attr("style", "display:none");
}

function ClearHeaderData() {
    $('.masterSearch').parent().find('input[type=text]').val('');
    $('.masterSearch').parent().find('select').val('0');
}

function CheckSessionIsAlive() {
    $.get("/Home/CheckSessionIsAlive",
        function (data) {
            $("#divSession").html(data);
            $("#divSessionTimeOut").modal('show');
        });
}

function GetUrlPathName() {
    var pathName = window.location.pathname;
    $("#aCurrentModuleName").text(pathName.split("/")[1]);
    if (pathName.split("/")[1] == "ClaimTest") {
        $("#aCurrentModuleName").text("Test Claim");
    } 
    if (pathName.split("/")[1] == "DrugList") {
        $("#aCurrentModuleName").text("Drug List");
    }
    if (pathName.split("/")[1] == "MacDrug") {
        $("#aCurrentModuleName").text("Mac Drug");
    }
    if (pathName.split("/")[1] == "DrugSearch") {
        $("#aCurrentModuleName").text("Drug Search");
    }
    switch (pathName.split("/")[1].trim().toLowerCase()) {
        case "dashboard":
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco dashboard");
            $("#Dashboard").addClass('active');
            break;
        case "claim":
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco claim");
            $("#Claims").addClass('active');
            $("#aCurrentModuleName").text("Claims");
            break;
        case "master":
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco master");
            $("#Masters").addClass('active');
            $("#aCurrentModuleName").text("Masters")
            break;
        case "monitor":
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco monitor");
            $("#Monitor").addClass('active');
            $("#aCurrentModuleName").text("Monitor");
            break;
        case "report":
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco report");
            $("#Reports").addClass('active');
            $("#aCurrentModuleName").text("Reports")
            break;
        case "userdetail":
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco user");
            $("#aCurrentModuleName").text('Users');
            $("#Users").addClass('active');
            $("#aCurrentModuleName").text("Users");
            break;
        case "utilities":
            $("#Masters").removeClass('active');
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco utilities");
            break;
        default: 
            $("#Masters").addClass('active');
            $("#spanModuleIcon").removeClass().addClass("nav-ico beforeIco master");
    }
}

function DisplayLoadingTillPageLoad() {
    document.onreadystatechange = function () {
        if (document.readyState == "loading") {
            $("#mainWrap").addClass("ajaxLoading");
        }
        if (document.readyState == "complete") {
            $("#mainWrap").removeClass("ajaxLoading");
        }
    }
}





