
$(document).ready(function () {
    claims.fn_GetClaimDetailListOnLoad();
});

$(document).on("dblclick", "#tblClaimList tr", function () {
    claims.fn_GetClaimDetail($(this).attr("data-claimId"), $(this).attr('data-claim'));
});

$(document).on("keypress", "#tblClaimList tr", function (event) {
    if (event == '13') {
        claims.fn_GetClaimDetail($(this).attr("data-claimId"), $(this).attr('data-claim'))
    }

});

var claims = {

    "claimDetailLink": "/Claim/ClaimDetailList",

    "fn_ShowSearchType": function () {

        var searchFilter = $("#ClaimSearchBy").val();
        this.fn_GetClaimDetailListBySearchQuery(searchFilter);

        $("#SearchForm")[0].reset();

        $("#ClaimSearchBy").val(searchFilter);

        $("#searchTypeFilter").find("[data-searchtype=" + searchFilter + "]").removeClass('hide').slideDown().siblings().addClass('hide').slideUp();
    },

    "fn_GetClaimDetailList": function () {
        $.get(this.claimDetailLink
            , function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvClaimPartial").html(data);
                }

            });
    },

    "fn_GetClaimDetailListBySearchQuery": function (searchBy) {
        $.get("/Claim/ClaimDetailList", { searchBy: searchBy }, function (data) {
            if (data.search("sessionModel") > 0) {
                location.reload();
            } else {
                $("#dvClaimPartial").html(data);
                commanAjax.fn_SetActiveFirstRow("#tblClaim");
            }

        });
    },

    "fn_GetClaimDetailListOnLoad": function () {
        $.get("/Authenticate/CheckSessionPresist", function (data) {
            if (!data) {
                location.reload();
            }
        })

        claims.fn_SetClaimSearchBy();

        var searchFilter = $("#ClaimSearchBy").val();
        $.get("/Claim/ClaimDetailList",
            {
                ProductServiceId: $("#ProductServiceId").val(),
                PharmacyId: $("#PharmacyId").val(),
                NPI: $("#NPI").val(), ClaimSearchBy: searchFilter,
                RxNumber: $("#RxNumber").val(),
                DateOfService: $("#DateOfService").val(),
                Group: $("#Group").val()

            },
            function (data) {

                $("#dvClaimPartial").html(data);
                commanAjax.fn_SetActiveFirstRow("#tblClaim");
                $("#searchTypeFilter").find("[data-searchtype=" + searchFilter + "]").removeClass('hide').slideDown().siblings().addClass('hide').slideUp();
            }).done(function () {
                $("#divClaimMainWrapper").removeClass('ajaxLoading');

            });
    },

    "fn_Sorting": function (sortOrder) {

        var obj = {};
        $("#claimForm").serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });

        $.post("/Claim/ClaimDetailSearchList",
            { searchModelEntity: obj, searchBy: $("#ClaimSearchBy").val(), sortBy: "Id", sortOrder: sortOrder },
            function (data) {
                $("#divClaimList").html(data);
            });
    },

    "fn_GetClaimUrl": function () {
        return "/Claim/ClaimDetailSearchList";
    },

    "fn_SetClaimSearchBy": function () {
        var pharmacyId = $("#PharmacyId").val();
        var rxNumber = $("#RxNumber").val();
        var dateOfService = $("#DateOfService").val();
        var productServiceId = $("#ProductServiceId").val();
        var npi = $("#NPI").val();
        var group = $("#Group").val();
        if (pharmacyId.length > 0) {
            $("#ClaimSearchBy").val("Pharmacy");
        }
        if (npi.length > 0) {
            $("#ClaimSearchBy").val("Pharmacy");
        }
        if (rxNumber.length > 0) {
            $("#ClaimSearchBy").val("Pharmacy");
        }
        if (dateOfService.length > 0) {
            $("#ClaimSearchBy").val("Pharmacy");
        }

        if (group.length > 0) {
            $("#ClaimSearchBy").val("Client");
        }
        if (productServiceId.length > 0) {
            $("#ClaimSearchBy").val("Drug");
        }
    },

    "fn_GetClaimDetail": function (claimId, claim) {
        debugger;
    
        $("#divClaimList").addClass('ajaxLoading');
        $.get("/Claim/ClaimDetailInfo", { claimId: claimId }, function (response) {
            if (response.search("sessionModel") > 0) {
                location.reload();
            }
            else {
                $("#divPartialData").html(response);
                $("#THSCommanModalPopUp").modal("show");
                $("#divClaimHeaderId").html("Claim Details (" + claim + ")");
                $("#divClaimList").removeClass('ajaxLoading');
            }

        })
    },

    "fn_ClearPage": function () {
        $(".form-control").each(function (e, data) {
            if (this.type == "text") {
                $(this).val('');
            }
            if (this.type == "select") {
                $(this).val('ALL');
            }

        })
    },

    "fn_ClearForm": function () {
        //check session is Exists or not
        var isPresist = true;
        $.get("/Authenticate/CheckSessionPresist", function (data) {
            isPresist = data;
            if (!data) {
                window.location.reload();
            }
        
        });
        if (isPresist) {
            $("#ClaimSearchBy").val('Pharmacy');
            $("#ClaimType").val('ALL');
            $("#ClaimStatus").val('ALL');
            $("#RejectCode").val('ALL');
            $("#PharmacyId").val('');
            $("#NPI").val('');
            $("#DateOfService").val('');
            $("#RxNumber").val('');
            $("#FillNumber").val('');
            $("#ProcessDate").val('');
            $("#BIN").val('');
            $("#PCN").val('');
            $("#Group").val('');
            $("#PatientId").val('');
            $("#GroupMember").val('');
            $("#ProductServiceId").val('');
            $("#DAWIndicator").val('');
            $("#FormularyId").val('');
            $("#PhysicianCode").val('');
            $("#FacilityCode").val('');
            claims.fn_GetClaimDetailListOnLoad();
        }

     
    },
};


var datePicker = {
    "fn_checkDate": function (strval) {
        var check = 'NaN';
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(strval)) {
            var adata = strval.split('/');
            var gg = parseInt(adata[1], 10);
            var mm = parseInt(adata[0], 10);
            var aaaa = parseInt(adata[2], 10);
            var xdata = new Date(aaaa, mm - 1, gg);
            if ((xdata.getFullYear() == aaaa) && (xdata.getMonth() == mm - 1) && (xdata.getDate() == gg))
                check = strval;
            else
                check = 'NaN';
        } else
            check = 'Invalid';

        return check;
    },

    "fn_ValidateDate": function () {
        var valueStartDate = Date.parse(datePicker.fn_checkDate($('#StartDate').val()));
        var valueEndDate = Date.parse(datePicker.fn_checkDate($('#EndDate').val()));

        if ((!/Invalid|NaN/.test(valueStartDate)) && (!/Invalid/.test(valueEndDate))) {
            $('#msgServiceDate').text("");
            return true;
        }
        else {
            $('#msgServiceDate').text("Please enter correct Start date/End date in mm/dd/yyyy format.")

            return false;
        }
    }
    
};

