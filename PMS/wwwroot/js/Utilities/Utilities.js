

$(document).ready(function () {

    var location = $("#utilityHeading").text();

    $("#utilityNav li").each(function (index, value) {
        $(value).children().removeClass('active');
    })

    $("#utilityNav li").each(function (index, value) {
        if (value.innerText.trim() == location.trim()) {
            $(value).children().addClass('active');
        }
    })
    $("#SearchMasterform").submit()

    $("#txtSearch").keyup(function (event) {
        CheckSessionIsExists();
        if (event.keyCode == 13) {
            $("#SearchMasterform").submit()
        }
    })

    DeActivateLinks();
    ActivateLink();


});

function DeActivateLinks() {
    $("#headingClaimRejectCode").removeClass('active');
    $("#headingClaimSearchBy").removeClass('active');
    $("#headingClaimStatus").removeClass('active');
    $("#headingClaimType").removeClass('active');
    $("#headingPriceSourceOptions").removeClass('active');
    $("#headingClaimReport").removeClass('active');
    $("#headingDrugType").removeClass("active");
}

function ActivateLink() {
    var location = $("#utilityHeading").text();
    switch (location.replace(/\s/g, '')) {
        case "ClaimRejectCode":
            $("#headingClaimRejectCode").addClass('active');
            break;
        case "ClaimSearchBy":
            $("#headingClaimSearchBy").addClass('active');
            break;
        case "ClaimStatus":
            $("#headingClaimStatus").addClass('active');
            break;
        case "ClaimType":
            $("#headingClaimType").addClass('active');
            break;
        case "PriceSourceOptions":
            $("#headingPriceSourceOptions").addClass('active');
            break;
        case "Reports":
            $("#headingClaimReport").addClass('active');
            break;
        case "DrugType":
            $("#headingDrugType").addClass('active');
            break;

    }
}

function CreateUtility(name) {
    debugger;
    $.get("/Authenticate/CheckSessionPresist", function (data) {
        if (!data) {
            location.reload();
        }
        else {
            var url = "/Utilities/Create?paramName=" + name;
            $("#divClaimList").addClass("ajaxLoading");
            $.get(url, function (data) {
                $("#divPartialData").html(data);
                $("#THSCommanModalPopUp").modal('show');
                $("#modalSize").removeClass("lg").addClass("md")
                $("#modalHeader").text("Add " + name);
            }).done(function () {
                $("#divClaimList").removeClass('ajaxLoading');
            });

        }
    })
}

function Edit(id) {
    var param = $("#utilityHeading").text();
    $("#divClaimList").addClass('ajaxLoading');
    $.get("/Utilities/Create", { id: id, paramName: param }, function (data) {
        if (data.search("sessionModel") > 0) {
            location.reload();
        }
        else {
            $("#divPartialData").html(data);
            $("#THSCommanModalPopUp").modal('show');
            $("#modalSize").removeClass("lg").addClass("md")
            $("#modalHeader").text("Edit " + param);
            $("#btnSubmit").val("Update")
        }
    }).done(function () {
        $("#divClaimList").removeClass('ajaxLoading');
    });
}

function AjaxSuccess(response) {
    if (response.search("sessionModel") > 0) {
        location.reload();
    }
    else {
        $("#THSCommanModalPopUp").modal('hide');
        location.reload();
    }

}

function GetUtilityUrl() {
    return "/Utilities/" + $("#utilityHeading").text().replace(/\s/g, '');
}

$(document).ready(function () {
    var location = $("#utilityHeading").text();

    $("#utilityNav li").each(function (index, value) {
        $(value).children().removeClass('active');
    })

    $("#utilityNav li").each(function (index, value) {
        if (value.innerText.trim() == location.trim()) {
            $(value).children().addClass('active');
        }
    })

    $("#SearchMasterform").submit();
});

function AjaxSuccess(response) {
    if (!CheckSessionIsExists()) {
        location.reload();
    }
    else {
        $("#THSCommanModalPopUp").modal('hide');
        location.reload();
    }
}

function GetUtilityUrl() {
    return "/Utilities/" + $("#utilityHeading").text().replace(/\s/g, '');
}

function DeActivate(id) {
    $.confirm({
        title: 'Confirm',
        content: 'Are you sure want to change the status?',
        typeAnimated: true,
        draggable: false,
        buttons: {
            ok: {
                text: 'Yes',
                btnClass: 'btn-blue',
                keys: ['enter'],
                action: function () {
                    $.get("/Utilities/ActivateDeActivate",
                        { id: id, paramName: $("#utilityHeading").text().replace(/\s/g, ''), activate: 0 }, function (data) {
                            if (data.search("sessionModel") > 0) {
                                location.reload();
                            } else {
                                $("#divClaimList").addClass("ajaxLoading");
                            }

                        }).done(function () {
                            $("#divClaimList").removeClass("ajaxLoading");
                            location.reload();
                        });
                }
            },
            cancel: {
                text: 'No',
                btnClass: 'btn-dark',
                action: function () {
                }
            }
        }
    });
}

function Activate(id) {
    $.confirm({
        title: 'Confirm',
        content: 'Are you sure want to change the status?',
        typeAnimated: true,
        draggable: false,
        buttons: {
            ok: {
                text: 'Yes',
                btnClass: 'btn-blue',
                keys: ['enter'],
                action: function () {
                    $.get("/Utilities/ActivateDeActivate", { id: id, paramName: $("#utilityHeading").text().replace(/\s/g, ''), activate: 1 }, function (data) {
                        $("#divClaimList").addClass("ajaxLoading");
                    }).done(function () {
                        $("#divClaimList").removeClass("ajaxLoading");
                        location.reload();
                    });
                }
            },
            cancel: {
                text: 'No',
                btnClass: 'btn-dark',
                action: function () {
                }
            }
        }
    });
}

function CheckIsExists() {
    var value = $("#Name").val();
    var tableName = $("#utilityHeading").text().replace(/\s/g, '');
    $.get("/Utilities/CheckIsExists", { name: value, paramName: tableName }, function (data) {
        if (data.search("sessionModel") > 0) {
            location.reload();
        }
        else {
            if (data) {
                $("#Name").val('')
                jQuery.alert({
                    title: "Alert.",
                    buttons: {
                        ok: {
                            text: 'Ok',
                            btnClass: 'btn-blue',
                            keys: ['enter'],
                        }
                    },
                    content: "Name already exists.",
                    draggable: false
                });
            }
        }

    })
}

function ValidateDefaultValue() {
    if (!CheckSessionIsExists()) {
        location.reload()
    } else {
        if ($("#IsDefault").prop("checked") == true) {
            var defaultValue = $("#_hdDefaultValue").val();
            if (defaultValue == null || defaultValue.length == 0) {

            } else {
                $.confirm({
                    title: 'Confirm',
                    content: 'Another ' + defaultValue + ' record already exists as default. Do you want to proceed with the current as default?',
                    typeAnimated: true,
                    draggable: false,
                    buttons: {
                        ok: {
                            text: 'Yes',
                            btnClass: 'btn-blue',
                            keys: ['enter'],
                            action: function () {

                            }
                        },
                        cancel: {
                            text: 'No',
                            btnClass: 'btn-dark',
                            action: function () {
                                $("#IsDefault").prop("checked", false);
                            }
                        }
                    }
                });
            }
        }
    }
  
}

function updateCreatedDateName(createdBy, createdDate, updatedBy, updatedDate) {
    CheckSessionIsExists();
    $("#createdBy").text(createdBy);
    $("#createdDate").text(createdDate);
    if (updatedBy !== "") {
        $("#updatedBy").text(updatedBy);
        $("#updatedDate").text(updatedDate);
    }
    else {
        $("#updatedBy").text("");
        $("#updatedDate").text("");
    }
}

function AjaxUtilityGetComplete(id) {
    setTimeout(function () {
        if ($("#" + id).find("tbody tr:first") != undefined && $("#" + id + " tbody tr:first").find('td').length > 1) {
            $("#" + id + " tbody tr:first").addClass("selected").click();
        }
    }, 100);

}

function CheckSessionIsExists() {
    $.get("/Authenticate/CheckSessionPresist", function (data) {
        return data;
    })
}