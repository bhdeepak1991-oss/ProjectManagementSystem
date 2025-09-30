$(function () {
    //$('.date-picker').datepicker({
    //    dateFormat: "mm/dd/yy",
    //    constrainInput: true
    //}).on("change",
    //    function(e, obj) {
    //        if ($(this).val() == '' || $(this).val() == 'mm/dd/yyyy') {
    //            $('#msgServiceDate').text("");
    //            return true;
    //        } else {
    //            var valueEntered = Date.parse(report.fn_checkDate($(this).val()));

    //            if (!/Invalid|NaN/.test(valueEntered)) {
    //                $('#msgServiceDate').text("");
    //                return true;
    //            } else {
    //                $(this).text("");
    //                return false;
    //            }
    //        }
    //    });

});

var report = {
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
        if ($("#ReportType").val() == "2") {
            return true;
        }
        var valueStartDate = Date.parse(report.fn_checkDate($('#StartDate').val()));
        var valueEndDate = Date.parse(report.fn_checkDate($('#EndDate').val()));

        if ((!/Invalid|NaN/.test(valueStartDate)) && (!/Invalid|NaN/.test(valueEndDate))) {
            $('#msgServiceDate').text("");
            $("#StartDate").removeClass().addClass("form-control date");
            $("#EndDate").removeClass().addClass("form-control date");
            return true;
        }
        else {
            var status = true;
            if ((/Invalid|NaN/.test(valueStartDate))) {
                //$('#msgServiceDate').text("Please enter correct Start Date/End Date in mm/dd/yyyy format.")
                $('#msgServiceDate').text("");
                $("#StartDate").removeClass().addClass("form-control date  input-validation-error");
                status = false;
            }
            else {
                $('#msgServiceDate').text("");
                $("#StartDate").removeClass().addClass("form-control date");
            }
            if ((/Invalid|NaN/.test(valueEndDate))) {
                //$('#msgServiceDate').text("Please enter correct Start Date/End Date in mm/dd/yyyy format.")
                $('#msgServiceDate').text("");
                $("#EndDate").removeClass().addClass("form-control date  input-validation-error");
                status = false;
            }
            else {
                $('#msgServiceDate').text("");
                $("#EndDate").removeClass().addClass("form-control date");
            }

            return status;
        }
    },
    "fn_ValidateEndDate": function () {
        var startDate = new Date($('#StartDate').val());
        var endDate = new Date($('#EndDate').val());
        if (startDate > endDate) {
            $('#EndDate').val('');
            $("#EndDate").removeClass('valid').addClass('input-validation-error')
            $('#msgServiceDate').text('End Date should be greater or equal to Start Date.');
        }
        else {
            $("#EndDate").removeClass('input-validation-error').addClass('valid')
            $('#msgServiceDate').text('');
        }
    },
    "fn_ValidateStartDate": function () {
        var startDate = new Date($('#StartDate').val());
        var endDate = new Date($('#EndDate').val());
        if (startDate > endDate) {
            $('#EndDate').val('');
            $("#EndDate").removeClass('valid').addClass('input-validation-error')
            $('#msgServiceDate').text('End Date should be greater or equal to Start Date.');
        }
        else {
            $("#EndDate").removeClass('input-validation-error').addClass('valid')
            $('#msgServiceDate').text('');
        }
    }
};