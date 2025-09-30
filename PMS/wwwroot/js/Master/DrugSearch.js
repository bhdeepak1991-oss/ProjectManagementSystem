var tblRowCount = -1;
var lastCheckedEndDateElmId;

$(document).ready(function () {
    SearchDrugList(true);
    //fn_ValidatePrice();
    bindControlStartDate();
    bindControlEndDate();
});

function SearchDrugList(isDefault) {
    $("#divDrugNDCSearch").addClass("ajaxLoading");
    if (isDefault) {
        $("#NDC").val(GetParameterValues("strValue"));
    }
    var obj = {};
    $("#SearchForm").serializeArray().map(function (x) {
        obj[x.name] = x.value;
    });
    $.post("/DrugSearch/DrugSearchList",
        { searchModelEntity: obj, sortBy: "", pageIndex: 1 },
        function (data) {
            $("#divClaimList").html(data);
            commanAjax.fn_SetActiveFirstRow("#tblDrugSearchList");
        }).done(function () {
            $("#divDrugNDCSearch").removeClass("ajaxLoading");
        });
}

function fn_AddDrug() {
    //tblRowCount = -1; 
    $("#modalTitle").text("Add Drug Detail");
    $.get("/DrugSearch/CreateDrug", function (data) {
        if (data.search("sessionModel") > 0) {
            location.reload();
        }
        else {
            $("#divPartialData").html(data);
            fn_AddPriceDetail();
        }

    }).done(function () {
        $("#THSCommanModalPopUp").modal('show');

    });
}

function fn_AddPriceDetail() {

    $("#DrugPriceDetails_0_BGCode").removeClass("input-validation-error");
    $("#DrugPriceDetails_0_Price").removeClass("input-validation-error");
    $("#DrugPriceDetails_0_StartDate").removeClass("input-validation-error");

    var rowCount = $('#tblDrugPrice tr').length;
    if (checkValidationOnAddRow()) {
        tblRowCount = rowCount - 1;

        tblRowCount++;
        var html = "";
        html = "<tr>";
        html += "<td> <input type='text' id='DrugPriceDetails_" + tblRowCount + "_BGCode' name='DrugPriceDetails[" + tblRowCount + "].BGCode' class='form-control bgreq validateBGCode' autocomplete='off'/><span class='field-validation-valid' data-valmsg-for='DrugPriceDetails[" + tblRowCount + "].BGCode' id='DrugPriceDetails_" + tblRowCount + "_BGCode' data-valmsg-replace='true'></span></td>";
        html += "<td> <input type='text' id='DrugPriceDetails_" + tblRowCount + "_Price' name='DrugPriceDetails[" + tblRowCount + "].Price' class='form-control pricereq rtl number selectAll' autocomplete='off' /><span class='field-validation-valid' data-valmsg-for='DrugPriceDetails[" + tblRowCount + "].Price' data-valmsg-replace='true'></span></td>";
        html += "<td> <input type='text' id='DrugPriceDetails_" + tblRowCount + "_StartDate' name='DrugPriceDetails[" + tblRowCount + "].StartDate' readonly='readonly' data-picker-position='top-right' class='form-control date validateStartDate date-picker-StartDate' autocomplete='off' onchange='StartDateHandler(this);' /><span class='field-validation-valid' data-valmsg-for='DrugPriceDetails[" + tblRowCount + "].StartDate' id='DrugPriceDetails_" + tblRowCount + "_StartDate' data-valmsg-replace='true'></span></td>";
        html += "<td> <input type='text' id='DrugPriceDetails_" + tblRowCount + "_EndDate' name='DrugPriceDetails[" + tblRowCount + "].EndDate' readonly='readonly' data-picker-position='top-right' class='form-control date validateEndDate' autocomplete='off' onchange='EndDateHandler(this);' /><span class='field-validation-valid' data-valmsg-for='DrugPriceDetails[" + tblRowCount + "].EndDate' id='DrugPriceDetails_" + tblRowCount + "_EndDate' data-valmsg-replace='true'></span></td>";
        html += "</tr>";
        $("#tblDrugPrice").append(html);
        bindControlStartDate();
        bindControlEndDate();
    }
    else {
        $("#DrugPriceDetails_" + tblRowCount+"_BGCode").addClass("input-validation-error");
        $("#DrugPriceDetails_" + tblRowCount+"_Price").addClass("input-validation-error");
        $("#DrugPriceDetails_" + tblRowCount+"_StartDate").addClass("input-validation-error")
    }
}

function fn_EditDrugInfo(ndc) {
    $("#divClaimList").addClass('ajaxLoading');
    $("#modalTitle").text("Edit Drug Details");
    $.get("/DrugSearch/CreateDrug",
        { drugId: ndc }, function (data) {
            if (data.search("sessionModel") > 0) {
                location.reload();
            }
            else {
                $("#divPartialData").html(data);
            }

        }).done(function () {
            $("#THSCommanModalPopUp").modal('show');
            $("#btnClose").bind("click",
                function () {
                });
            $("#divClaimList").removeClass('ajaxLoading');
        });
}

function fn_DrugDetail(ndc) {
    $("#divClaimList").addClass('ajaxLoading');
    $("#modalTitle").text("Drug Details");
    $.get("/DrugSearch/GetDrugDetail",
        { drugId: ndc }, function (data) {
            if (data.search("sessionModel") > 0) {
                location.reload();
            } else {
                $("#divPartialData").html(data);
                commanAjax.fn_SetActiveFirstRow("#tblDrugPrice");
            }

        }).done(function () {
            $("#THSCommanModalPopUp").modal('show');
            $("#btnClose").bind("click",
                function () {
                });
            $("#divClaimList").removeClass('ajaxLoading');
        });
}

function fn_CloseCurrentModel() {
    setTimeout(function () {
        if ($("#tblDrugSearchList").find("tbody tr:first") != undefined && $("#tblDrugSearchList tbody tr:first").find('td').length > 1) {
            $('#tblDrugSearchList tr.selected').click();
        }
    }, 50);
}

function fn_GetDrugSearchUrl() {
    return "/DrugSearch/DrugSearchList";
}

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split("=");
        if (urlparam[0] === param) {
            return urlparam[1];
        }
    }
}

function SearchBegin() {
    $("#divClaimList").addClass("ajaxLoading");
}

function ValidateEndDateOnEdit(startDate, eData) {
    var chkDate = checkDate(startDate);
    if (chkDate != 'NaN' && chkDate != 'Invalid') {

    }
}

function fn_ClearFormDrugNDC() {
    $("#DrugType").val('0');
    $("#NDC").val('');
    $("#DrugName").val('');
    $("#GPI").val('');
    SearchDrugList(false);
}

function DrugAddSuccess(response) {
    commanAjax.Fn_CommanAlert(response.message, 'Alert!');
    if (response.messageId == 1) {
        $("#THSCommanModalPopUp").modal("hide");
        SearchDrugList(false);
    }

}

// check if BGCode already exists; if exists then check if termination date is exists or not; 
$("body").delegate(".bgreq", "blur", function (value, element) {
    if ($(this).is('[readonly]')) {
        element.stopImmediatePropagation();
        element.preventDefault();
    } else {
        var curElmId = $(this).attr('ID');
        var curElmName = $(this).attr('Name');
        var curPriceCode = $('#' + curElmId).val();
        var curRowindex = $("#" + curElmId).closest("tr").index();
        var curStartDate = $("#" + curElmId.replace("_BGCode", "_EndDate")).val();
        var endDateElmId, nxtStartDateElmId;

        var rowCount = $('#tblDrugPrice tr').length;
        $(".pricing .bgreq").each(function (indexP, obj) {
            //debugger
            var priceCodeElmId = $(obj).attr('ID');
            var priceCode = $('#' + priceCodeElmId).val();
            endDateElmId = priceCodeElmId.replace("_BGCode", "_EndDate");
            //lastCheckedEndDateElmId = "";
            if (curPriceCode == priceCode) {

                if (indexP > curRowindex) {
                    //To set empty all StartDate and EndDate after change in current StartDate element
                    nxtStartDateElmId = $(obj).attr('ID');
                    $('#' + nxtStartDateElmId).val('');
                    $('#' + nxtStartDateElmId).addClass('input-validation-error');
                    $("#" + nxtStartDateElmId).css('border-color', 'red');
                    $('#' + endDateElmId).val('');
                    $('#' + endDateElmId).addClass('input-validation-error');
                    return 1;
                }


                var vEndDate = $('#' + endDateElmId).val();
                if (vEndDate != '') {

                    var valueEntered = Date.parse(checkdate(vEndDate));
                    if (!/Invalid|NaN/.test(valueEntered)) {
                        if (curStartDate != '') {
                            var D1 = new Date(vEndDate).getTime();
                            var D2 = new Date(curStartDate).getTime();
                            if (D1 < D2) {
                                $("#" + curElmId).removeAttr('style');
                                $("Span[data-valmsg-for='" + $("#" + curElmId).attr('asp-for') + "']").html("");
                                // if (indexP < rowCount && curRowindex != rowCount) { return; }

                                return 1;
                            }
                            else {

                                $('#' + curElmId).val('');
                                $('#' + curElmId).addClass('input-validation-error');
                                $("#" + curElmId).css('border-color', 'red');
                                $("span#" + curElmId).html("<span for='" + curElmId + "' generated='true'>Start Date must be greater than End Date of same Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                                //$("Span[data-valmsg-for='" + $("#" + curElmId).attr('asp-for') + "']").html("<span for='" + curElmId + "' generated='true'>Start Date must be greater than End Date of same Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                                return false;
                            }
                        }
                    }
                    else {
                        $('#' + endDateElmId).val('');
                        $('#' + endDateElmId).addClass('input-validation-error');
                        $("#" + endDateElmId).css('border-color', 'red');
                        return false;
                    }
                }
                else {
                    if (curRowindex > 0 && indexP < curRowindex) {
                        $("Span[data-valmsg-for='" + curElmName + "']").html("<span for='" + curElmId + "' generated='true'>This Price rule is already added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        //$("span#" + curElmId).html("<span for='" + curElmId + "' generated='true'>This Price rule is already added.</span>");
                        $("#" + endDateElmId).css('border-color', 'red');
                        $('#' + endDateElmId).addClass('input-validation-error');
                        $("span#" + endDateElmId).html("<span for='" + endDateElmId + "' generated='true'>Please enter End Date as same Price rule is being added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        //lastCheckedEndDateElmId = endDateElmId;
                        return 1;
                    }
                    else {
                        //$("#" + curElmId).removeClass('input-validation-error').addClass('input-validation-valid');
                        return true;
                    }
                }
            }
            else {

                $("#" + endDateElmId).removeAttr('style');
                $("#" + endDateElmId).removeClass('input-validation-error').addClass('input-validation-valid');
                $("span#" + endDateElmId).html("");
                return true;

            }
            //lastCheckedEndDateElmId = "";
        });
    }
});

function StartDateHandler(e) {
    var curElmId = $(e).attr('ID');
    var curRowindex = $("#" + curElmId).closest("tr").index();
    var curStartDate = $(e).val();
    var elmStartVal = Date.parse(checkdate(curStartDate));
    var endDateElmId, nxtStartDateElmId;
    var sEndDate;
    if (!/Invalid|NaN/.test(elmStartVal)) {

        var VctrID = $('#' + curElmId.replace("StartDate", "EndDate"));
        $(VctrID).val('');
        $(VctrID).removeAttr('style');
        //$("Span[data-valmsg-for='" + $(VctrID).attr('asp-for') + "']").html("");
        $("span#" + $(VctrID).attr('id')).html("").removeClass('field-validation-error').addClass('field-validation-valid');
        //return true;
        var rowCount = $('#tblDrugPrice tr').length;
        $(".pricing .validateStartDate").each(function (indexP, obj) {
            endDateElmId = $(obj).attr('ID').replace("StartDate", "EndDate");
            if (endDateElmId != curElmId) {
                var curPriceCode = $('#' + endDateElmId.replace("_EndDate", "_BGCode")).val();
                var prevPriceCode = $('#' + curElmId.replace("_StartDate", "_BGCode")).val();
                sEndDate = $('#' + endDateElmId.replace("_StartDate", "_EndDate"));
                if (curPriceCode == prevPriceCode) {
                    if (indexP > curRowindex) {
                        //To set empty all StartDate and EndDate after change in current StartDate element
                        nxtStartDateElmId = $(obj).attr('ID');
                        $('#' + nxtStartDateElmId).val('');
                        $("#" + nxtStartDateElmId).css('border-color', 'red');
                        $('#' + endDateElmId).val('');
                        return 1;
                    }


                    var vEndDate = sEndDate.val();
                    if (vEndDate != '') {

                        var valueEntered = Date.parse(checkdate(vEndDate));
                        if (!/Invalid|NaN/.test(valueEntered)) {
                            var D1 = new Date(vEndDate).getTime();

                            var D2 = new Date(curStartDate).getTime();
                            if (D1 < D2) {
                                $("#" + curElmId).removeAttr('style');
                                $("Span[data-valmsg-for='" + $("#" + curElmId).attr('asp-for') + "']").html("");
                                // if (indexP < rowCount && curRowindex != rowCount) { return; }

                                return 1;
                            } else {

                                $('#' + curElmId).val('');
                                $("#" + curElmId).css('border-color', 'red');
                                $("#" + curElmId).removeClass('input-validation-valid').addClass('input-validation-error');
                                $("span#" + curElmId).html("<span for='" + curElmId + "' generated='true'>Start Date must be greater than End Date of same Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                                //$("Span[data-valmsg-for='" + $("#" + curElmId).attr('asp-for') + "']").html("<span for='" + curElmId + "' generated='true'>Start Date must be greater than End Date of same Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                                return false;
                            }

                        }
                        else {
                            $('#' + curElmId).val('');
                            return false;
                        }
                    }
                    else {
                        //if (curRowindex > 0 && indexP < curRowindex) {
                        //    $('#' + curElmId).val('');
                        //    $("#" + curElmId).css('border-color', 'red');
                        //    $("#" + endDateElmId).css('border-color', 'red');
                        //    $("span#" + endDateElmId).html("<span for='" + endDateElmId + "' generated='true'>Please enter End Date as same Price rule is being added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        //    return false;
                        //} else {
                        //    $("#" + curElmId).removeClass('input-validation-error').addClass('input-validation-valid');
                        //    return true;
                        //}
                        $("#" + curElmId).removeClass('input-validation-error').addClass('input-validation-valid');
                        $("#" + curElmId).removeAttr('style');
                        return true;
                    }
                } else {

                    return true;
                }
            }
        });
    }
    else {
        $("#" + curElmId).css('border-color', 'red');
        $("Span[data-valmsg-for='" + $("#" + curElmId).attr('asp-for') + "']").html("<span for='" + curElmId + "' generated='true'>Please enter date in mm/dd/yyyy format.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
        return false;
    }

}

function EndDateHandler(e) {
    var ctrID = $(e).attr('ID');
    var curRowindex = $("#" + ctrID).closest("tr").index();
    var vEndDate = $(e).val();
    var elmEndVal = Date.parse(checkdate(vEndDate));
    var nxtEndDateElmId, nxtStartDateElmId;
    if (!/Invalid|NaN/.test(elmEndVal)) {

        $(".pricing .validateEndDate").each(function (indexP, obj) {
            nxtEndDateElmId = $(obj).attr('ID');
            nxtStartDateElmId = $('#' + nxtEndDateElmId.replace("EndDate", "StartDate"));
            //debugger
            var curPriceCode = $('#' + nxtEndDateElmId.replace("_EndDate", "_BGCode")).val();
            var checkPriceCode = $('#' + ctrID.replace("_EndDate", "_BGCode")).val();
            if (curPriceCode == checkPriceCode) {
                if (indexP > curRowindex) {
                    //To set empty all StartDate and EndDate after change in current EndDate element 
                    //Check Element readonly or not
                    if ($(nxtStartDateElmId).hasClass("editMode")) {
                        return false;
                    } else {
                        $(nxtStartDateElmId).val('');
                        $(nxtStartDateElmId).css('border-color', 'red');
                        $('#' + nxtEndDateElmId).val('');
                        return 1;
                    }

                } else {
                    var elmStartVal = $('#' + ctrID.replace("EndDate", "StartDate")).val();
                    var D1 = new Date(elmStartVal).getTime();

                    var D2 = new Date(elmEndVal).getTime();

                    if (D2 >= D1) {
                        $("#" + ctrID).removeClass('input-validation-error').addClass('input-validation-valid');
                        $("#" + ctrID).removeAttr('style');

                        //$("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("");
                        return true;
                    } else {
                        $("#" + ctrID).val('');
                        $("#" + ctrID).css('border-color', 'red');
                        $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("<span for='" + ctrID + "' generated='true'>End Date must be equal to or greater than Start Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        return false;
                    }
                }
            }
            else {
                var elmStartVal = $('#' + ctrID.replace("EndDate", "StartDate")).val();
                var D1 = new Date(elmStartVal).getTime();

                var D2 = new Date(elmEndVal).getTime();

                if (D2 >= D1) {
                    $("#" + ctrID).removeAttr('style');
                    $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("");
                    return true;
                } else {
                    $("#" + ctrID).val('');
                    $("#" + ctrID).css('border-color', 'red');
                    $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("<span for='" + ctrID + "' generated='true'>End Date must be equal to or greater than Start Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                    return false;
                }

            }
        });
    }
    else {
        $("#" + ctrID).css('border-color', 'red');
        $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("<span for='" + ctrID + "' generated='true'>Please enter date in mm/dd/yyyy format.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
        return false;
    }
}

function checkdate(strval) {
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
}

function bindControlStartDate() {

    $('.date').datetimepicker({
        language: 'fr',
        format: 'mm/dd/yyyy',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0
        , constrainInput: true
        , beforeShow: function () {
        }
        , onClose: function (value, object) {
        }
    }).on("change", function (e, obj) {
        var ctrID = $(this).attr('ID');
        var ctrName = $(this).attr('name');


        if ($(this).val() == '' || $(this).val() == 'mm/dd/yyyy') {
            $("Span[data-valmsg-for='" + ctrName + "']").html("");
            return true;
        }
        else {
            var valueEntered = Date.parse(checkdate($(this).val()));
            if (!/Invalid|NaN/.test(valueEntered)) {
                $("Span[data-valmsg-for='" + ctrName + "']").html("");


                checkStartDate(this);  //check if start date is greater than the  prvousily enter end date

                //check if end date is entered if yes then verify start date is less then end date
                var elmEndDate = $('#' + ctrID.replace("StartDate", "EndDate"));
                var vEndDate = $(elmEndDate).val();
                if (vEndDate != '') {
                    var D1 = new Date(valueEntered).getTime();
                    var D2 = new Date(vEndDate).getTime();

                    if (D2 >= D1) {
                        $("Span[data-valmsg-for='" + ctrName + "']").html("");
                    } else {
                        $("Span[data-valmsg-for='" + ctrName + "']").html("<span for='" + ctrID + "' generated='true'>Start Date must be equal to or less than End Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        return false;
                    }
                }


                return true;
            }
            else {
                $("Span[data-valmsg-for='" + ctrName + "']").html("<span for='" + ctrID + "' generated='true'>Please enter date in mm/dd/yyyy format.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                return false;
            }
        }
    });

    $.validator.addMethod("date-picker-StartDate", function (value, element) {
        if (value != '' && value != 'mm/dd/yyyy') {
            var valueEntered = Date.parse(checkdate(value));
            if (/Invalid|NaN/.test(valueEntered)) {
                return false;
            }
            else {
                $("Span[data-valmsg-for='" + $(this).attr('name') + "']").html('');
                return true;
            }
        }
        else {
            return false;
        }
    }, "Please enter date in mm/dd/yyyy format.");


    //required check for Unit Price
    $.validator.addMethod("pricereq", function (value, element) {
        if (value != "") {
            var regex = /^\d+(\.\d{1,2})?$/;
            if (regex.test(value)) {
                $(element).removeClass('input-validation-error').addClass('input-validation-valid');
                $(element).removeAttr('style');
                $("Span[data-valmsg-for='" + $(this).attr('name') + "']").html('');
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }, "Please enter valid unit price");

    // check if Price indicator already exists if exists then check if termination date is exists or not; 
    $('.bgreq').on("blur", function (value, element) {
        var elmId = $(this).attr('ID');
        var elmName = $(this).attr('Name');
        var elmVal = $(this).val();
        var selmId;
        var sEndDate;

        $(".pricing .bgreq").each(function (indexP, obj) {
            selmId = $(obj).attr('ID');
            if (selmId != elmId) {

                if ($('#' + selmId).val() == elmVal) {

                    var vTranCode = $('#' + selmId.replace("BG", "TransactionCode")).val();
                    sEndDate = $('#' + selmId.replace("BG", "EndDate"));
                    var vEndDate = sEndDate.val();
                    if (vTranCode == "D") {
                        if (vEndDate != '') {
                            var valueEntered = Date.parse(checkdate(vEndDate));
                            if (!/Invalid|NaN/.test(valueEntered)) {
                                $("Span[data-valmsg-for='" + elmName + "']").html('');
                                return true;
                            }
                            else {
                                $("Span[data-valmsg-for='" + $(sEndDate).attr('Name') + "']").html("<span for='" + $(sEndDate).attr('ID') + "' generated='true'>Please enter correct End Date in already added Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                                return false;
                            }
                        }
                        else {
                            $("Span[data-valmsg-for='" + $(sEndDate).attr('Name') + "']").html("<span for='" + $(sEndDate).attr('ID') + "' generated='true'>Please enter End Date as same Price rule is being added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                            //$("Span[data-valmsg-for='AWPPricingSummary']").html("<span for='AWPPricingSummary' generated='true'>Please enter End Date in current active Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');

                            //$(sEndDate).focus();
                            return false;
                        }
                    }
                    else {
                        $("Span[data-valmsg-for='" + elmName + "']").html("<span for='" + elmId + "' generated='true'>This Price rule is already added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        return false;
                    }

                }
            }
        });

    });

    // required validation for Price indicator 
    $.validator.addMethod("bgreq", function (value, element) {
        value = $.trim(value);
        if (value != "") {
            var reg = /^[a-zA-Z]*$/;
            if (reg.test(value) && (value == "AWP" || value == "WAC" || value == "DP")) {
                $(element).removeClass('input-validation-error').addClass('input-validation-valid');
                $(element).removeAttr('style');
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }, "Please enter valid price indicator AWP, WAC or DP.");

}

function bindControlEndDate() {

    $('.date').datetimepicker({
        language: 'fr',
        format: 'mm/dd/yyyy',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0
        , constrainInput: true
        , beforeShow: function () {
        }
        , onClose: function (value, object) {
        }
    }).on("change", function (e, obj) {
        var ctrID = $(this).attr('ID');
        var ctrName = $(this).attr('name');

        if ($(this).val() == '' || $(this).val() == 'mm/dd/yyyy') {
            $("Span[data-valmsg-for='" + ctrName + "']").html("");
            return true;
        }
        else {
            var valueEntered = Date.parse(checkdate($(this).val()));
            if (!/Invalid|NaN/.test(valueEntered)) {

                $("Span[data-valmsg-for='" + ctrName + "']").html("");

                var elmStartVal = $('#' + ctrID.replace("EndDate", "StartDate")).val();

                var D1 = new Date(elmStartVal).getTime(); //Date(D1[2], D1[0] - 1, D1[1]).getTime();

                var D2 = new Date(valueEntered).getTime();

                if (D2 >= D1) {
                    $("Span[data-valmsg-for='" + ctrName + "']").html("");
                } else {
                    $("Span[data-valmsg-for='" + ctrName + "']").html("<span for='" + ctrID + "' generated='true'>End Date must be equal to or greater than Start Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                    return false;
                }

                return true;
            }
            else {
                $("Span[data-valmsg-for='" + ctrName + "']").html("<span for='" + ctrID + "' generated='true'>Please enter date in mm/dd/yyyy format.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                return false;
            }
        }
    });

    $.validator.addMethod("date-picker-EndDate", function (value, element) {
        if (value != '' && value != 'mm/dd/yyyy') {
            var valueEntered = Date.parse(checkdate(value));
            if (/Invalid|NaN/.test(valueEntered)) {
                return false;
            }
            else {
                $("Span[data-valmsg-for='" + element + "']").html('');
                return true;
            }
        }
        else {
            return true;
        }
    }, "Please enter date in mm/dd/yyyy format.");
}

function checkStartDate(objStartDate) {
    var elmBG = $('#' + $(objStartDate).attr('ID').replace("StartDate", "BG"));

    var elmId = $(elmBG).attr('ID');
    var elmName = $(elmBG).attr('Name');
    var elmVal = $(elmBG).val();
    var selmId;
    var sEndDate;


    //$(".pricing .bgreq").each(function (indexP, obj) { 
    //    selmId = $(obj).attr('ID');
    //    if (selmId != elmId) {
    //        if ($('#' + selmId).val() == elmVal) {
    //            sEndDate = $('#' + selmId.replace("BG", "EndDate"));
    //            var vEndDateOldPrice = sEndDate.val();

    //            if (vEndDateOldPrice != '') {
    //                vEndDateOldPrice = Date.parse(checkdate(vEndDateOldPrice));

    //                var vStarDateNewPrice = Date.parse(checkdate($(objStartDate).val()));

    //                if (!/Invalid|NaN/.test(vEndDateOldPrice)) {
    //                    var D1 = new Date(vStarDateNewPrice).getTime();
    //                    var D2 = new Date(vEndDateOldPrice).getTime();

    //                    if (D1 > D2) {
    //                        $("Span[data-valmsg-for='" + $(objStartDate).attr('Name') + "']").html("");
    //                    } else {
    //                        $("Span[data-valmsg-for='" + $(objStartDate).attr('Name') + "']").html("<span for='" + $(objStartDate).attr('ID') + "' generated='true'>Start Date must be greater than Previous Pricing rule End Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
    //                        return false;
    //                    }
    //                    return true;
    //                }
    //            }
    //            else {
    //                $("Span[data-valmsg-for='" + $(sEndDate).attr('Name') + "']").html("<span for='" + $(sEndDate).attr('ID') + "' generated='true'>Please enter End Date as same Price rule is being added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
    //                //$("Span[data-valmsg-for='AWPPricingSummary']").html("<span for='AWPPricingSummary' generated='true'>Please enter End Date in current active Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
    //                //$(sEndDate).focus();
    //                return false;
    //            }

    //        }
    //    }
    //});
}

function checkValidationOnAddRow() {
    var returnFlag = true;

    $(".pricing .bgreq").each(function (index, obj) {
        var curBGCodeElm;
        var curPriceElm;
        var curStartDateElm;
        var curEndDateElm;
        curBGCodeElm = $(obj).attr('ID');
        if ($("#" + curBGCodeElm).hasClass('input-validation-error')) {
            returnFlag = false;
        }
        curPriceElm = curBGCodeElm.replace("_BGCode", "_Price");
        if ($("#" + curPriceElm).hasClass('input-validation-error')) {
            returnFlag = false;
        }
        curStartDateElm = curBGCodeElm.replace("_BGCode", "_StartDate");
        if ($("#" + curStartDateElm).hasClass('input-validation-error')) {
            returnFlag = false;
        }
        curEndDateElm = curBGCodeElm.replace("_BGCode", "_EndDate");
        if ($("#" + curEndDateElm).hasClass('input-validation-error')) {
            returnFlag = false;
        }
        if ($("#" + curBGCodeElm).val() == "" || $("#" + curPriceElm).val() == "" || $("#" + curStartDateElm).val() == "") {
            returnFlag = false;
        }

    });
    return returnFlag;
}

function checkValidation() {
    var returnFlag = true;
    if (isFormValid()) {

        $(".pricing .bgreq").each(function (index, obj) {
            var curBGCodeElm;
            var curPriceElm;
            var curStartDateElm;
            var curEndDateElm;
            curBGCodeElm = $(obj).attr('ID');
            if ($("#" + curBGCodeElm).hasClass('input-validation-error') || $("#" + curBGCodeElm).val() === '') {
                $("#" + curBGCodeElm).removeClass('input-validation-valid').addClass('input-validation-error');
                $("#" + curBGCodeElm).css('border-color', 'red');
            } else {
                $("#" + curBGCodeElm).removeClass('input-validation-error').addClass('input-validation-valid');
                $("#" + curBGCodeElm).removeAttr('style');
            }
            curPriceElm = curBGCodeElm.replace("_BGCode", "_Price");
            if ($("#" + curPriceElm).hasClass('input-validation-error') || $("#" + curPriceElm).val() === '') {
                $("#" + curPriceElm).removeClass('input-validation-valid').addClass('input-validation-error');
                $("#" + curPriceElm).css('border-color', 'red');
            } else {
                $("#" + curPriceElm).removeClass('input-validation-error').addClass('input-validation-valid');
                $("#" + curPriceElm).removeAttr('style');
            }
            curStartDateElm = curBGCodeElm.replace("_BGCode", "_StartDate");
            if ($("#" + curStartDateElm).hasClass('input-validation-error') || $("#" + curStartDateElm).val() === '') {
                $("#" + curStartDateElm).removeClass('input-validation-valid').addClass('input-validation-error');
                $("#" + curStartDateElm).css('border-color', 'red');
            } else {
                $("#" + curStartDateElm).removeClass('input-validation-error').addClass('input-validation-valid');
                $("#" + curStartDateElm).removeAttr('style');
            }
            curEndDateElm = curBGCodeElm.replace("_BGCode", "_EndDate");
            if ($("#" + curEndDateElm).hasClass('input-validation-error') || $("#" + curEndDateElm).val() === '') {
                //$("#" + curEndDateElm).removeClass('input-validation-valid').addClass('input-validation-error');
                //$("#" + curEndDateElm).css('border-color', 'red');
                //return returnFlag;
            } else {
                //$("#" + curEndDateElm).removeClass('input-validation-error').addClass('input-validation-valid');
                //$("#" + curEndDateElm).removeAttr('style');
            }

        });
        $(".pricing .bgreq").each(function (index, obj) {
            var curBGCodeElm;
            var curPriceElm;
            var curStartDateElm;
            var curEndDateElm;
            curBGCodeElm = $(obj).attr('ID');
            if ($("#" + curBGCodeElm).hasClass('input-validation-error')) {
                returnFlag = false;
            }
            curPriceElm = curBGCodeElm.replace("_BGCode", "_Price");
            if ($("#" + curPriceElm).hasClass('input-validation-error')) {
                returnFlag = false;
            }
            curStartDateElm = curBGCodeElm.replace("_BGCode", "_StartDate");
            if ($("#" + curStartDateElm).hasClass('input-validation-error')) {
                returnFlag = false;
            }
            curEndDateElm = curBGCodeElm.replace("_BGCode", "_EndDate");
            if ($("#" + curEndDateElm).hasClass('input-validation-error')) {
                returnFlag = false;
            }

        });
        return returnFlag;
    } else {
        return false;
    }
}
function isFormValid() {
    //debugger
    var returnFlag = true;

    if ($("#divCreateDrug").find("#DrugName").val() === '') {
        $("#divCreateDrug").find("#DrugName").removeClass('input-validation-valid').addClass('input-validation-error');
        returnFlag = false;
    }
    if ($("#NDCNumber").val() === '') {
        $("#NDCNumber").removeClass('input-validation-valid').addClass('input-validation-error');
        returnFlag = false;
    }
    if ($("#GenericName").val() === '') {
        $("#GenericName").removeClass('input-validation-valid').addClass('input-validation-error');
        returnFlag = false;
    }
    if ($("#MultiSourceCode").val() === '') {
        $("#MultiSourceCode").removeClass('input-validation-valid').addClass('input-validation-error');
        returnFlag = false;
    }
    return returnFlag;
}

function fn_SubmitDrugSearch(searchForm) {
    debugger;
}


/////////////////// Obsolated  /////////////////

function fn_ValidatePrice() {
    $(".valildatePrice").keydown(function (event) {


        if (event.shiftKey == true) {
            event.preventDefault();
        }

        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

        } else {
            event.preventDefault();
        }

        if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
            event.preventDefault();

    });
}
function reformatDate(dateStr) {
    if (dateStr.includes('-')) {
        dArr = dateStr.split("-");  // ex input "2010-01-18"
        return dArr[1] + "/" + dArr[2] + "/" + dArr[0]; //ex out: "18/01/2010"
    }
}
function fn_ValidateRow() {
    var isValidate = true;
    $(".valildatePrice").each(function (i, value) {
        if ($(value).val().length === 0) {
            $(value).css('border-color', 'red');
            isValidate = false;
        } else {
            $(value).removeAttr('style');
            //$(value).removeAttr('border-color');
        }
    });

    $(".validateStartDate").each(function (i, value) {
        if ($(value).val().length === 0) {
            $(value).css('border-color', 'red');
            isValidate = false;
        } else {
            $(value).removeAttr('style');
            // $(value).removeAttr('border-color');
        }
    });

    $("#tblDrugPrice tr td input").each(function () {
        if ($(this).is("[style]")) {
            if ($(this).attr('style') != "") {
                isValidate = false;
            }
        }

    });

    return isValidate;
}
function fn_ValidatePrice() {
    var isValidate = true;
    $(".valildatePrice").each(function (i, value) {
        if ($(value).val().length === 0) {
            $(value).css('border-color', 'red');
            isValidate = false;
        } else {
            $(value).removeAttr('border-color');
        }
    });

    $(".validateStartDate").each(function (i, value) {
        if ($(value).val().length === 0) {
            $(value).css('border-color', 'red');
            isValidate = false;
        } else {
            $(value).removeAttr('border-color');
        }
    });

    return isValidate;
}
function fn_ValidatePriceCode(event) {
}

///////////////Not in Use///////////////

function checkValidationOld() {
    var curPriceCode;
    var curEndDateElm;
    var curEndDate;
    var newPriceCode;
    var newStartDateElm;
    var newStartDate;
    var returnFlag = true;

    $(".pricing .bgreq").each(function (indexOuter, obj) {
        curPriceCode = $(obj).val();

        $(".pricing .bgreq").each(function (indexInner, innerObj) {
            if (indexInner > indexOuter) {
                newPriceCode = $(innerObj).val();

                if (curPriceCode == newPriceCode) {

                    curEndDateElm = $('#' + $(obj).attr('ID').replace("BG", "EndDate"));
                    curEndDate = curEndDateElm.val();

                    if (curEndDate != '') {
                        curEndDate = Date.parse(checkdate(curEndDate));

                        newStartDateElm = $('#' + $(innerObj).attr('ID').replace("BG", "StartDate"));
                        newStartDate = Date.parse(checkdate($(newStartDateElm).val()));

                        if (!/Invalid|NaN/.test(curEndDate)) {
                            var D1 = new Date(newStartDate).getTime();
                            var D2 = new Date(curEndDate).getTime();

                            if (D1 > D2) {
                                $("Span[data-valmsg-for='" + $(curEndDate).attr('Name') + "']").html("");
                                $("Span[data-valmsg-for='" + $(newStartDateElm).attr('Name') + "']").html("");
                            } else {
                                $("Span[data-valmsg-for='" + $(newStartDateElm).attr('Name') + "']").html("<span for='" + $(newStartDateElm).attr('ID') + "' generated='true'>Start Date must be greater than Previous Pricing rule End Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                                //return false;
                                returnFlag = false;
                            }
                            //return true;
                            //returnFlag = true;
                        }
                    }
                    else {
                        $("Span[data-valmsg-for='" + $(curEndDate).attr('Name') + "']").html("<span for='" + $(curEndDate).attr('ID') + "' generated='true'>Please enter End Date as same Price rule is being added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                        returnFlag = false;
                        // return false;
                    }

                }


            }
        }); //innner for loop
    }); //outer for loop

    return returnFlag;
}
function EndDateHandlerOld(e) {
    var ctrID = $(e).attr('ID');
    var vEndDate = $(e).val();
    var elmEndVal = Date.parse(checkdate(vEndDate));
    if (!/Invalid|NaN/.test(elmEndVal)) {

        var elmStartVal = $('#' + ctrID.replace("EndDate", "StartDate")).val();

        var D1 = new Date(elmStartVal).getTime();

        var D2 = new Date(elmEndVal).getTime();

        if (D2 >= D1) {
            $("#" + ctrID).removeAttr('style');
            $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("");
        } else {
            $("#" + ctrID).val('');
            $("#" + ctrID).css('border-color', 'red');
            $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("<span for='" + ctrID + "' generated='true'>End Date must be equal to or greater than Start Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
            return false;
        }
        return true;
    }
    else {
        $("#" + ctrID).css('border-color', 'red');
        $("Span[data-valmsg-for='" + $("#" + ctrID).attr('asp-for') + "']").html("<span for='" + ctrID + "' generated='true'>Please enter date in mm/dd/yyyy format.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
        return false;
    }
}
function GetDetailOnDrugType() {
    $("#SearchForm").submit();
}

// check if Price indicator already exists if exists then check if termination date is exists or not; 
//$("body").delegate(".valildatePrice", "blur", function (value, element) {
//    var elmId = $(this).attr('ID');
//    //var elmName = $(this).attr('Name');
//    //var elmVal = $(this).val();
//    var selmId;
//    var sEndDate;

//    $(".pricing .valildatePrice").each(function (indexP, obj) {
//        selmId = $(obj).attr('ID');
//        if (selmId != elmId) {
//            var sPriceCode = $('#' + selmId.replace("_Price", "_BGCode")).val();
//            var vPriceCode = $('#' + elmId.replace("_Price", "_BGCode")).val();
//            sEndDate = $('#' + selmId.replace("_Price", "_EndDate"));
//            if (sPriceCode == vPriceCode) {


//                var vEndDate = sEndDate.val();
//                if (vEndDate != '') {
//                    var valueEntered = Date.parse(checkdate(vEndDate));
//                    if (!/Invalid|NaN/.test(valueEntered)) {
//                        $(sEndDate).removeAttr('style');
//                        $("Span[data-valmsg-for='" + $(sEndDate).attr('asp-for') + "']").html('');
//                        //$("Span[data-valmsg-for='" + elmName + "']").html('');
//                        return true;
//                    }
//                    else {
//                        $(sEndDate).css('border-color', 'red');
//                        $("Span[data-valmsg-for='" + $(sEndDate).attr('asp-for') + "']").html("<span for='" + $(sEndDate).attr('ID') + "' generated='true'>Please enter correct End Date in already added Price rule.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
//                        return false;
//                    }
//                }
//                else {
//                    $(sEndDate).css('border-color', 'red');
//                    $("Span[data-valmsg-for='" + $(sEndDate).attr('asp-for') + "']").html("<span for='" + $(sEndDate).attr('ID') + "' generated='true'>Please enter End Date as same Price rule is being added.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
//                    return false;
//                }
//            } else {
//                $(sEndDate).removeAttr('style');
//                $("Span[data-valmsg-for='" + $(sEndDate).attr('asp-for') + "']").html('');
//                return true;
//            }
//        }
//    });

//});


