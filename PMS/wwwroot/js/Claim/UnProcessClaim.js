var UnProcessClaim= {
    "fn_GetUnProcessClaimUrl": function() {
        return "/UnProcessedClaim/Search";
    },
    "fn_GetUnProcessedClaimDetails": function() {
        var obj = {};
        $("#SearchForm").serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });
        $.post("/UnProcessedClaim/Search",
            { searchModelEntity: obj},
            function(data) {
                $("#dvClaimPartial").html(data);
            });
    }
};

$(document).ready(function() {
    UnProcessClaim.fn_GetUnProcessedClaimDetails();
    $("#divUnProcessClaimDiv").removeClass("ajaxLoading");
})