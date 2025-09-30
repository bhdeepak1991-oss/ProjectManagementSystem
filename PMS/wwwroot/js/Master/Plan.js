
$(function () {
    // will trigger when the document is ready
    $('.date-picker3').datepicker({
        dateFormat: "mm/dd/yy"
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

    $.validator.addMethod("date-picker3", function (value, element) {
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

    $.validator.addMethod("OnlyChar", function (value, element) {
        if (value != "") {
            var reg = /^[a-zA-Z]*$/;
            if (reg.test(value)) {
                return true;
            }
            else {
                return false;
            }
        }
    }, "Number and space are not allowed.");


    $.validator.addMethod("OnlyNum", function (value, element) {
        if (value != "") {
            var regex = /^[0-9\b]+$/;
            if (regex.test(value)) {
                return true;
            }
            else {
                return false;
            }
        }
    }, "Character and space are not allowed.");

    $.validator.addMethod("OnlyDec", function (value, element) {
        if (value != "") {
            var regex = /^\d+(\.\d{1,2})?$/;
            if (regex.test(value)) {
                return true;
            }
            else {
                return false;
            }
        }
    }, "Character and space are not allowed.");
});

$(document).ready(function () {
    $("#BGindicator").change(function () {
        if ($(this).val() != -1) {
            $("#spnMsgBGindicator").text("");
        }
    });
    $("#PriceComparisonID").change(function () {
        if ($(this).val() != -1) {
            $("#spnMsgPriceComparisonID").text("");
        }
    });
    $('#txtRate').on("keypress", function (e) {
        if (e.keyCode == 13) {
            $('#search').click();
        }
    });
    planMaster.fn_BindListOnLoad();
});

function ClientIdValidation() {
    if ($("#PriceSourceOptionID option:selected").text() == "DRUGLIST" || $("#PriceSourceOptionID option:selected").text() == "MAC") {
        if ($("#PriceBasicDrugListID").val() == 0) {
            $("#spnMsgPriceBasicDrugListID").text("Please Select Drug List");
            return false;
        }
        else {
            $("#spnMsgPriceBasicDrugListID").text("");
        }
    }
    else {
        $("#spnMsgPriceBasicDrugListID").text("");
    }
    return true;
}

var planMaster = {
    "fn_BindListOnLoad": function () {
        var obj = {};
        $("#SearchForm").serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });

        $.post("/Plan/FilteredPlanList", { searchModelEntity: obj, sortBy: "", pageIndex: 1 },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvClaimPartial").html("");
                    $("#dvClaimPartial").html(data);
                    commanAjax.fn_SetActiveFirstRow("#tblRatePlan"); 
                }
            });
    },
    "fn_BindPlanPricingRule": function (clientBasicRuleSetID) {
        $("#divClaimList").addClass('ajaxLoading');
        $.get("/Plan/GetPlanPricingRule",
            { ClientBasicRuleSetID: clientBasicRuleSetID },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#modalSize").removeClass().addClass("modal-dialog lg");
                    $("#divPartialData").html(data);
                    commanAjax.fn_SetActiveFirstRow("#tblPlanPricingRule");
                    $("#THSCommanModalPopUp").modal("show");
                }
            }).done(function () {
                $("#divClaimList").removeClass('ajaxLoading');
            });
    },
    "fn_filterPlan": function () {
        var _filterObj = new Object();
        _filterObj.strValue = $("#txtRatePlan").val();
        _filterObj.id = new Date().getMilliseconds();

        $.ajax({
            type: "POST"
            , url: '/Plan/FilteredPlanList'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvRatePartial").html("");
                    $("#dvRatePartial").html(result);
                }
            }
        });
    },
    "fn_DeleteRatePlanPriceRule": function (ruleSetId, rulePriceId) {
        $.confirm({
            title: 'Confirm',
            content: 'Are you sure, You want to delete the selected price rule?',
            typeAnimated: true,
            draggable: false,
            buttons: {
                ok: {
                    text: 'Yes',
                    btnClass: 'btn-blue',
                    keys: ['enter'],
                    action: function () {
                        $('#hdnPlanId').val(ruleSetId);
                        var _Obj = new Object();
                        _Obj.ClientBasicRuleSetID = ruleSetId;
                        _Obj.ClientBasicRulePriceID = rulePriceId;
                        $("#divPartialData").addClass('ajaxLoading');
                        $.ajax({
                            type: "POST"
                            , url: '/Plan/DeleteRatePlanPriceRule'
                            , data: _Obj
                            , success: function (result) {
                                if (result.messageId > 0) {
                                    commanAjax.Fn_CommanAlert(result.message, 'Alert!');
                                    planMaster.fn_BindPlanPricingRule(ruleSetId);
                                    $("#divPartialData").removeClass('ajaxLoading');
                                }
                            }
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
        //if (confirm("Are you sure to delete the selected price rule?")) {
        //    $('#hdnPlanId').val(ruleSetId);
        //    var _Obj = new Object();
        //    _Obj.ClientBasicRuleSetID = ruleSetId;
        //    _Obj.ClientBasicRulePriceID = rulePriceId;

        //    $.ajax({
        //        type: "POST"
        //        , url: '/Plan/DeleteRatePlanPriceRule'
        //        , data: _Obj
        //        , success: function (result) {
        //            if (result.messageId > 0) {
        //                //alert(result.message);
        //                commanAjax.Fn_CommanAlert(result.message, 'Success!');
        //                planMaster.fn_BindPlanPricingRule(ruleSetId);
        //            }
        //        }
        //    });
        //} else {
        //    return false;
        //}

    },
    "fn_RatePlanValidation": function () {
        var bflag = true;
        var _isFormValid = $("#frmUpsertRatePlan").valid();
        if (!_isFormValid) {
            bflag = false;
        }
        return bflag;


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
    "fn_UpsertRatePlanBegin": function () {
        return planMaster.fn_DateValidation();
    },
    "fn_UpsertRatePlan": function () {
        var _ratValue = false;
        if (planMaster.fn_RatePlanValidation()) {
            _ratValue = true;

            var _Obj = new Object();
            _Obj.ClientBasicRuleSetID = $("#ClientBasicRuleSetID").val();
            _Obj.ClientMemberPlanName = $("#ClientMemberPlanName").val();
            _Obj.PriceBasicDrugListID = $("#PriceBasicDrugListID").val();
            _Obj.BGindicator = $("#BGindicator").val();
            _Obj.PriceComparisonID = $("#PriceComparisonID").val();
            _Obj.DaySupplyEdit = $("#DaySupplyEdit").val();
            _Obj.QuantityEdit = $("#QuantityEdit").val();
            _Obj.EarlyRefillEdit = $("#EarlyRefillEdit").val();
            _Obj.MaximumDollarEdit = $("#MaximumDollarEdit").val();
            _Obj.EffectiveDate = $("#EffectiveDate").val();
            _Obj.TerminationDate = $("#TerminationDate").val();
            _Obj.PlanNumber = $("#PlanNumber").val();
            _Obj.MinimumDollarEdit = $("#MinimumDollarEdit").val();

            $.ajax({
                type: "POST"
                , url: '/Plan/UpsertRatePlan'
                , data: _Obj
                , success: function (result) {
                    if (result.search("sessionModel") > 0) {
                        location.reload();
                    } else {
                        if (result.messageId > 0) {
                            commanAjax.Fn_CommanAlert(result.message, 'Alert!');
                            window.location.href = "/Plan/PlanList?strValue=";//location.reload();
                        }
                    }
                }
            });
        }
        return _ratValue;
    },
    "fn_ClientIdValidation": function () {

        if ($("#PriceSourceOptionID option:selected").text() == "DRUGLIST" || $("#PriceSourceOptionID option:selected").text() == "MAC") {
            if ($("#PriceBasicDrugListID").val() == 0) {
                $("#spnMsgPriceBasicDrugListID").text("Please Select Drug List");
                return false;
            }
            else {
                $("#spnMsgPriceBasicDrugListID").text("");
            }
        }
        else {
            $("#spnMsgPriceBasicDrugListID").text("");
        }
        return true;
    },
    "fn_UpsertRatePlanPriceRuleBegin": function () {
        var retVal = true;
        if ($("#PriceSourceOptionID").val() == 0) {
            $('#PriceSourceOptionID').removeClass('valid').addClass('input-validation-error');
            retVal = false;
        } else {
            $('#PriceSourceOptionID').removeClass('input-validation-error').addClass('valid');
        }
        if ($("#PriceSourceOptionID option:selected").text() == "DRUGLIST" || $("#PriceSourceOptionID option:selected").text() == "MAC") {
            if ($("#PriceBasicDrugListID").val() == 0) {
                $('#PriceBasicDrugListID').removeClass('valid').addClass('input-validation-error');
                retVal = false;
            } else {
                $('#PriceBasicDrugListID').removeClass('input-validation-error').addClass('valid');
            }
        }
        return retVal;
    },
    "fn_PriceSourceOptionIdOnChange": function () {
        if ($("#PriceSourceOptionID").val() == 0) {
            $('#PriceSourceOptionID').removeClass('valid').addClass('input-validation-error');
        } else {
            $('#PriceSourceOptionID').removeClass('input-validation-error').addClass('valid');
        }
        if ($("#PriceSourceOptionID option:selected").text() == "DRUGLIST" || $("#PriceSourceOptionID option:selected").text() == "MAC") {
            $("#fldDrugList").show();
        }
        else {
            $("#fldDrugList").hide();
            $("#PriceBasicDrugListID").val("0");
        }
    },
    "fn_PriceBasicDrugListIdOnChange": function () {
        if ($("#PriceBasicDrugListID").val() == 0) {
            $('#PriceBasicDrugListID').removeClass('valid').addClass('input-validation-error');
        } else {
            $('#PriceBasicDrugListID').removeClass('input-validation-error').addClass('valid');
        }
    },
    "fn_UpsertRatePlanPriceRule": function () {
        var _ratValue = false;
        var _isFormValid = $("#frmUpsertRatePlanPrice").valid();
        if (planMaster.fn_ClientIdValidation() && _isFormValid) {
            _ratValue = true;

            var _Obj = new Object();
            _Obj.ClientBasicRulePriceID = $("#ClientBasicRulePriceID").val();
            _Obj.ClientBasicRuleSetID = $("#ClientBasicRuleSetID").val();
            _Obj.PriceSourceOptionID = $("#PriceSourceOptionID").val();
            _Obj.PriceBasicDrugListID = $("#PriceBasicDrugListID").val();
            _Obj.PercentAdjutment = $("#PercentAdjutment").val();
            _Obj.FlatPrice = $("#FlatPrice").val();
            _Obj.DispenseFee = $("#DispenseFee").val();
            _Obj.AdditionalFee = $("#AdditionalFee").val();
            //_Obj.EffectiveDate = $("#EffectiveDate").val();
            //_Obj.TerminationDate = $("#TerminationDate").val();

            $.ajax({
                type: "POST"
                , url: '/Plan/UpsertRatePlanPriceRule'
                , data: _Obj
                , success: function (result) {
                    if (result.search("sessionModel") > 0) {
                        location.reload();
                    } else {
                        if (result.messageId > 0) {
                            if (result.messageId == 1) {
                                //alert(result.message);
                                commanAjax.Fn_CommanAlert(result.message, 'Alert!');
                                window.location.href = "/Plan/PlanList?strValue=";//location.reload();
                            }
                        }
                    }
                }
            });
        }
        return _ratValue;
    },
    "fn_GetUrl": function () {
        return "/Plan/FilteredPlanList";
    },
    "fn_AddPlan": function () {
        $.get("/Plan/UpsertRatePlan",
            { PlanId: -1 },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#divPartialData").html(data);
                    $("#modalSize").removeClass().addClass("modal-dialog md");
                    $("#THSCommanModalPopUp").modal("show");
                }
            });
    },
    "fn_EditPlan": function (id) {
        $("#dvClaimPartial").addClass('ajaxLoading');
        $.get("/Plan/UpsertRatePlan",
            { PlanId: id },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#modalSize").removeClass().addClass("modal-dialog md");
                    $("#divPartialData").html(data);
                    $("#THSCommanModalPopUp").modal("show");
                }
            }).done(function () {
                $("#dvClaimPartial").removeClass('ajaxLoading');
            });
    },
    "fn_PlanAddSuccess": function (response) {
        commanAjax.Fn_CommanAlert(response.message, 'Alert!');
        if (response.messageId == 1) {
            $("#THSCommanModalPopUp").modal("hide");
            planMaster.fn_BindListOnLoad();
        }
    },
    "fn_AddPriceRule": function (id) {
        $("#dvClaimPartial").addClass('ajaxLoading');
        $.get("/Plan/UpsertRatePlanPriceRule",
            { PlanId: id, PlanRuleId: -1 },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#modalSize").removeClass().addClass("modal-dialog sm");
                    $("#divPartialData").html(data);
                    $("#THSCommanModalPopUp").modal("show");
                }
            }).done(function () {
                $("#dvClaimPartial").removeClass('ajaxLoading');
            });
    },
    "fn_PlanPriceAddSuccess": function (response) {
        commanAjax.Fn_CommanAlert(response.message, 'Alert!');
        if (response.messageId == 1) {
            $("#AddPlanPriceRuleModalPopUp").modal('hide');
            planMaster.fn_BindPlanPricingRule(response.clientBasicRuleSetID);
        }
    },
    "fn_EditPlanPrice": function (id, ruleId) {
        $("#divPartialData").addClass('ajaxLoading');
        $.get("/Plan/UpsertRatePlanPriceRule",
            { PlanId: id, PlanRuleId: ruleId },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#divAddPlanPriceRule").html(data);
                    $("#AddPlanPriceRuleModalPopUp").modal("show");
                }
            }).done(function () {
                $("#divPartialData").removeClass('ajaxLoading');
            });
    },
    "fn_CloseCurrentModel": function () {
        setTimeout(function () {
            if ($("#tblRatePlan").find("tbody tr:first") != undefined && $("#tblRatePlan tbody tr:first").find('td').length > 1) {
                $('#tblRatePlan tr.selected').click();
            }
        }, 100);
    }

};



