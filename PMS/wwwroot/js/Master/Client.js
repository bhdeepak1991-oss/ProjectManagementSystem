
$(document).ready(function () {
    client.fn_ClientListOnLoad();
    $("#PlanRuleSetID").change(function () {
        if ($(this).val() > 0) {
            $("#spnMsgPlanRuleSetID").text("");
        }
    });
  
});
var client = {
    "fn_ClientListOnLoad": function () {
        var _filterObj = new Object();
        _filterObj.ClientName = "";
        _filterObj.NABP = commanAjax.fn_GetUrlVars()["strValue"];
        _filterObj.NPI = "";
        _filterObj.id = null;

        $.ajax({
            type: "POST"
            , url: '/Client/GetClientList'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvClaimPartial").html("");
                    $("#dvClaimPartial").html(result);
                }
            }
        });
    },
    "fn_ClientListFilter": function () {
        var _filterObj = new Object();
        _filterObj.ClientName = $("#txtClientName").val();
        _filterObj.NABP = $("#txtPharmacyID").val();
        _filterObj.NPI = $("#txtNPI").val();
        _filterObj.id = new Date().getMilliseconds();

        $.ajax({
            type: "POST"
            , url: '/Client/GetClientList'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvPharmacyPartial").html("");
                    $("#dvPharmacyPartial").html(result);
                }
            }
        });
    },
    "fn_UpsertMemberPlan": function (facilityId, forName, planId) {
        $("#dvClientFacilityPartial").addClass('ajaxLoading');
        $.get('/Client/UpsertPharmacyRatePlan', { PharmacyId: facilityId, PlanId: planId }, function (data) {
            if (data.search("sessionModel") > 0) {
                location.reload();
            } else {
                $("#divPartialEditPlan").html(data);
            }
        }).done(function () {
            if (planId > 0) {
                $("#modalTitle").text("Edit Facility Plan");//(" + forName + ")
            } else {
                $("#modalTitle").text("Add Facility Plan");
            }
            $("#clientEditPlanModal").modal('show');
            initDatePicker();
            $("#dvClientFacilityPartial").removeClass('ajaxLoading');
        });
    },
    "fn_BindFacilityPlan": function (facilityId, forName) {
        var _Obj = new Object();
        _Obj.FacilityID = facilityId;
        $("#dvClientFacilityPartial").addClass('ajaxLoading');
        $.ajax({
            type: "POST"
            , url: '/Client/GetFacilityPlans'
            , data: _Obj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#divPartialViewPlan").html(result);
                    commanAjax.fn_SetActiveFirstRow("#tblFacilityPlan");
                    $("#clientViewPlanModal").modal('show');
                    $("#modalSize").removeClass().addClass("modal-dialog lg");
                    $("#dvClientFacilityPartial").removeClass('ajaxLoading');
                }
            }
        });

    },
    "fn_BindPlanPricingRule": function (facilityPlanId, clientBasicRuleSetID, elm) {
        $.each($('#tblFacilityPlan a.minus'), function (index, val) {
            if ($(val).attr('data-ref') !== $(elm).attr('data-ref')) {
                $("#trMemberPlanPricingRule_" + $(val).attr('data-ref')).slideUp();
                $(val).removeClass("minus");
            }
        });
        if ($(elm).hasClass("minus")) {
            debugger
             $("#dvMemberPlanPricingRule_" + facilityPlanId).html('');
            $("#trMemberPlanPricingRule_" + facilityPlanId).slideUp("slow", "linear", function () {
                $(elm).removeClass("minus");
            });
           
        } else {
            var _Obj = new Object();
            _Obj.ClientBasicRuleSetID = clientBasicRuleSetID;
            $("#divPartialViewPlan").addClass('ajaxLoading');
            $.ajax({
                type: "POST"
                , url: '/Client/GetPlanPricingRuleForPharmacy'
                , data: _Obj
                , success: function (result) {
                    if (result.search("sessionModel") > 0) {
                        location.reload();
                    } else {
                        $("#dvMemberPlanPricingRule_" + facilityPlanId).html(result);
                        $("#trMemberPlanPricingRule_" + facilityPlanId).slideDown();
                        $(elm).addClass("minus");
                        $("#divPartialViewPlan").removeClass('ajaxLoading');
                    }
                }
            });
        }

    },
    "fn_RatePlanIdValidation": function () {
        if ($("#PlanRuleSetID").val() == 0) {
            $("#spnMsgPlanRuleSetID").text("Please Select Plan");
            return false;
        }
        else {
            $("#spnMsgPlanRuleSetID").text("");
        }

        if (!CheckTermDate()) {
            return false;
        }
        return true;

    },
    "fn_DateValidation": function () {
        if (!CheckTermDate()) {
            $('#TerminationDate').removeClass('valid').addClass('input-validation-error');
            return false;
        } else {
            $('#TerminationDate').removeClass('input-validation-error').addClass('valid');
            return true;
        }
    },
    "fn_PharmacyRatePlanValidation": function () {
        var _ratValue = false;
        if ($("#PlanRuleSetID").val() == 0) {
            $("#spnMsgPlanRuleSetID").text("Please Select Plan");
            _ratValue = false;
        }
        else {
            _ratValue = true;
            $("#spnMsgPlanRuleSetID").text("");
        }
        if (_ratValue == true) {
            if ($("#EffectiveDate").val() == "") {
                $("#spnMsgEffectiveDate").text("Please Enter Effective Date");
                _ratValue = false;
            }
            else {
                _ratValue = true;
                $("#spnMsgEffectiveDate").text("");
            }
        }
        if (_ratValue == true) {
            if ($("#TerminationDate").val() == "") {
                $("#spnMsgTerminationDate").text("Please Enter Termination Date");
                _ratValue = false;
            }
            else {
                _ratValue = true;
                $("#spnMsgTerminationDate").text("");
            }
        }
        if (!CheckTermDate()) {
            _ratValue = false;
        }

        return _ratValue;
    },
    "fn_UpsertPharmacyRatePlan": function () {
        var _ratValue = false;
        if (client.fn_PharmacyRatePlanValidation()) {
            _ratValue = true;

            var _Obj = new Object();
            _Obj.PharmacyID = $("#PharmacyID").val();
            _Obj.PharmacyPlanID = $("#PharmacyPlanID").val();
            _Obj.PlanRuleSetID = $("#PlanRuleSetID").val();
            _Obj.EffectiveDate = $("#EffectiveDate").val();
            _Obj.TerminationDate = $("#TerminationDate").val();


            $.ajax({
                type: "POST"
                , url: '/Client/UpsertPharmacyRatePlan'
                , data: _Obj
                , success: function (result) {
                    if (result.search("sessionModel") > 0) {
                        location.reload();
                    } else {
                        if (result.messageId > 0) {
                            commanAjax.Fn_CommanAlert(result.message, 'Alert!');
                            $("#THSCommanModalPopUp").modal('hide');
                            location.reload();
                        }
                    }
                }
            });
        }
        return _ratValue;
    },
    "fn_GetUrl": function () {
        return "/Client/GetClientList";
    },
    "fn_GetInnerUrl": function () {
        return "/Client/PharmacyFacility";
    },
    "fn_GetClientDetails": function (id) {
        $.get("/Client/PharmacyDetails", { pharmacyId: id }, function (data) {
            if (data.search("sessionModel") > 0) {
                location.reload();
            } else {
                $("#modalSize").removeClass().addClass("modal-dialog lg");
                $("#divPartialData").html(data);
                $("#THSCommanModalPopUp").modal("show");
            }
        });
    },
    "fn_UpsertPharmacyRatePlanBegin": function () {
        return client.fn_DateValidation();
        $("#divPartialEditPlan").addClass("ajaxLoading");
    },
    "fn_UpsertPharmacyRatePlanSuccess": function (response) {
        $("#divPartialEditPlan").removeClass("ajaxLoading");
        commanAjax.Fn_CommanAlert(response.message, 'Alert!');
        if (response.returnId == 1) {
            $("#clientEditPlanModal").modal('hide');
            client.fn_BindFacilityPlan(response.id);
        }
    },
    "fn_UpsertClaimBegin": function () {
        $("#divPartialEditPlan").addClass("ajaxLoading");
    },
    "fn_UpsertClaimSuccess": function (response) {
        if (response.search("sessionModel") > 0) {
            location.reload();
        } else {
            $("#divPartialEditPlan").removeClass("ajaxLoading");
            commanAjax.Fn_CommanAlert(response.message, 'Alert!');
            $("#clientEditPlanModal").modal('hide');
            client.fn_BindFacilityPlan(response.id);
        }
    },
    "fn_GetLoadingDiv": function () {
        return "#divClaimFacilityList";
    },
    "fn_BidFacility": function (id, forName) {
        $("#dvClaimPartial").addClass('ajaxLoading');
        $("#modalHeader").html("Pharmacy Facilities (" + forName + ")");
        var _filterObj = new Object();
        _filterObj.PharmacyID = id;

        $.ajax({
            type: "POST"
            , url: '/Client/PharmacyFacility'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvClientFacilityHeading").show();
                    $("#dvClientFacilityPartial").html(result);
                    commanAjax.fn_SetActiveFirstRow("#tblPharmacyFacility");
                    $("#THSClientFacilityModel").modal("show");
                    $("#dvClaimPartial").removeClass('ajaxLoading');
                }
            }
        });
    },
    "fn_CloseCurrentModel": function (divId) {
        setTimeout(function () {
            if ($(divId).find("tbody tr:first") != undefined && $(divId + " tbody tr:first").find('td').length > 1) {
                $(divId + ' tr.selected').click();
            }
        }, 100);
    }
};



$(document).delegate("#btnCancel", "click", function (event) {
    $("#THSCommanModalPopUp").modal('hide');
});

function initDatePicker() {
       $('.date-picker4').datepicker({
        dateFormat: "mm/dd/yy"
        //, autoclose: true
        , constrainInput: true
        , beforeShow: function () {
        }
        , onClose: function (value, object) {
        }
    }).on("change", function (e, obj) {
        var ctrID = $(this).attr('ID');

        if ($(this).val() == '' || $(this).val() == 'mm/dd/yyyy') {
            if (ctrID == 'EffectiveDate') {
                $('Span[data-valmsg-for="EffectiveDate"]').html("");
            }
            else {
                $('Span[data-valmsg-for="TerminationDate"]').html("");
            }
            return true;
        }
        else {

            var valueEntered = Date.parse(checkdate($(this).val()));
            if (!/Invalid|NaN/.test(valueEntered)) {
                if (ctrID == 'EffectiveDate') {
                    $('Span[data-valmsg-for="EffectiveDate"]').html("");
                }
                else {
                    $('Span[data-valmsg-for="TerminationDate"]').html("");
                }
                return true;
            }
            else {
                if (ctrID == 'EffectiveDate') {
                    $('Span[data-valmsg-for="EffectiveDate"]').html("<span for='EffectiveDate' generated='true'>Please enter date in mm/dd/yyyy format.</span>");
                }
                else {
                    $('Span[data-valmsg-for="TerminationDate"]').html("<span for='TerminationDate' generated='true'>Please enter date in mm/dd/yyyy format.</span>");
                }
                return false;
            }
        }
    });
    $.validator.addMethod("date-picker4", function (value, element) {
        if (value != '' && value != 'mm/dd/yyyy') {
            var valueEntered = Date.parse(checkdate(value));
            if (/Invalid|NaN/.test(valueEntered)) {
                return false;
            }
            else {
                if (element.id == "EffectiveDate") { $('Span[data-valmsg-for="EffectiveDate"]').html(''); }
                else { $('Span[data-valmsg-for="TerminationDate"]').html(''); }
                return true;
            }
        }
        else {
            return true;
        }
    }, "Please enter date in mm/dd/yyyy format.");

}
