var dateValidation = {
    "ValidateStartEndDate": function (startDateId, endDateId) {
        if ($(endDateId).val() != "" && $(startDateId).val() != "") {

            var endDate = Date.parse(checkdate($(endDateId).val()));
            var startDate = Date.parse(checkdate($(startDateId).val()));

            if (/Invalid|NaN|-/.test(endDate)) {
                $(endDateId).removeClass('valid').addClass('input-validation-error');
                returnbool = false;
            }
            if (/Invalid|NaN|-/.test(startDate)) {
                $(startDateId).removeClass('valid').addClass('input-validation-error');
                returnbool = false;
            }

            if (!/Invalid|NaN|-/.test(startDate) && !/Invalid|NaN|-/.test(endDate)) {

                var D1 = $(startDateId).val().split("/");
                D1 = new Date(D1[2], D1[0] - 1, D1[1]).getTime();

                var D2 = $(endDateId).val().split("/");
                D2 = new Date(D2[2], D2[0] - 1, D2[1]).getTime();

                if (D2 >= D1) {
                    $(endDateId).removeClass('input-validation-error').addClass('valid');
                } else {
                    $(endDateId).removeClass('valid').addClass('input-validation-error');
                    returnbool = false;
                }
            }
        }
    }
};
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

function CheckTermDate() {
    var returnbool = true;

    if ($('#TerminationDate').val() != "" && $('#EffectiveDate').val() != "") {
        var valueEntered = Date.parse(checkdate($('#TerminationDate').val()));

        var valueEffectiveDate = Date.parse(checkdate($('#EffectiveDate').val()));

        if (/Invalid|NaN|-/.test(valueEffectiveDate)) {
            $('Span[data-valmsg-for="EffectiveDate"]').html("<span for='EffectiveDate' generated='true'>Please enter date in mm/dd/yyyy format.</span>");
            returnbool = false;
        }
        if (/Invalid|NaN|-/.test(valueEntered)) {
            $('Span[data-valmsg-for="TerminationDate"]').html("<span for='TerminationDate' generated='true'>Please enter date in mm/dd/yyyy format.</span>");
            returnbool = false;
        }
        if (!/Invalid|NaN|-/.test(valueEntered) && !/Invalid|NaN|-/.test(valueEffectiveDate)) {

            var D1 = $('#EffectiveDate').val().split("/");
            D1 = new Date(D1[2], D1[0] - 1, D1[1]).getTime();

            var D2 = $('#TerminationDate').val().split("/");
            D2 = new Date(D2[2], D2[0] - 1, D2[1]).getTime();

            if (D2 >= D1) {
                $('Span[data-valmsg-for="TerminationDate"]').html("");
            } else {
                $('Span[data-valmsg-for="TerminationDate"]').html("<span for='TerminationDate' generated='true'>Termination Date must be equal to or greater than Effective Date.</span>").removeClass('field-validation-valid').addClass('field-validation-error');
                returnbool = false;
            }
        }

    }
    return returnbool;
}

function ValidateStartEndDate(startDateId, endDateId) { 
    if ($(endDateId).val() != "" && $(startDateId).val() != "") {

        var endDate = Date.parse(checkdate($(endDateId).val()));
        var startDate = Date.parse(checkdate($(startDateId).val()));

        if (/Invalid|NaN|-/.test(endDate)) {
            $(endDateId).removeClass('valid').addClass('input-validation-error');
            returnbool = false;
        }
        if (/Invalid|NaN|-/.test(startDate)) {
            $(startDateId).removeClass('valid').addClass('input-validation-error');
            returnbool = false;
        }

        if (!/Invalid|NaN|-/.test(startDate) && !/Invalid|NaN|-/.test(endDate)) {

            var D1 = $(startDateId).val().split("/");
            D1 = new Date(D1[2], D1[0] - 1, D1[1]).getTime();

            var D2 = $(endDateId).val().split("/");
            D2 = new Date(D2[2], D2[0] - 1, D2[1]).getTime();

            if (D2 >= D1) {
                $(endDateId).removeClass('input-validation-error').addClass('valid');
            } else {
                $(endDateId).removeClass('valid').addClass('input-validation-error');
                returnbool = false;
            }
        }
    }
}
