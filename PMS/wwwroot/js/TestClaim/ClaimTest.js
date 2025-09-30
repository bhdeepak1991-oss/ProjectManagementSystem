var fileExtension = ["txt", "csv", "xlsx"];

function checkBatchFile() {
    if ($("#fileUpload").val().trim() == "") { 
        commanAjax.Fn_CommanAlert("Please select the Batch file", 'Alert!');
        $("#fileUpload").addClass("input-validation-error");
        return false;
    }
    else if ($.inArray($("#FileUpload").val().split('.').pop().toLowerCase(), fileExtension) == -1) { 
        commanAjax.Fn_CommanAlert("Please change the file. Only formats are allowed : " + fileExtension.join(', '), 'Alert!');
        return false;

    } else {
        $("#fileUpload").removeClass("input-validation-error");
        return true;
    }
}

$("#fileUpload").change(function() {
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) { 
        commanAjax.Fn_CommanAlert("Only formats are allowed : " + fileExtension.join(', '), 'Alert!');
        $("#fileUpload").val('');
    }

});

function uploadClaimSuccess(response) { 
    commanAjax.Fn_CommanAlert(response, 'Alert!');
    $("#uploadClaimForm")[0].reset();
    $("#fileUpload").removeClass("input-validation-error");
}

function ValidateInputData() {
    var data = $("#DOTest").val();
    if (data.length == 0) {
        return false;
        $("#DOTest").addClass("input-validation-error");
    } else {
        $("#DOTest").removeClass("input-validation-error");
        return true;
    }
    
}

function submitClaimSuccess(response) {
    commanAjax.Fn_CommanAlert(response, 'Alert!'); 
    $("#submitClaimForm")[0].reset();
    $("#dvPartialMsg").html('');
    $("#DOTest").removeClass("input-validation-error");
} 