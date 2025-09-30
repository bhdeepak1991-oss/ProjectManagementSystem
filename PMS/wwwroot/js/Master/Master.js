function fn_DrugSearch() {
    var drugSearch = $("#txtDrugList").val();
    location.href = "/DrugSearch/Index?ndc=" + drugSearch;
}

var master = {
    
    "fn_SerachMaster": function (masterType) { 
        "use strict";
        switch (masterType) {
            case 1: 
                window.location.href = "/Client/ClientList?strValue=" + $("#txtNABPSearchMaster").val() + "&id=" + new Date().getMilliseconds();
                break;
            case 2:
                window.location.href = "/Plan/PlanList?strValue=" + $("#txtPlanSearchMaster").val() + "&id=" + new Date().getMilliseconds();
                break;
            case 3:
                window.location.href = "/Formulary/FormularyIndex?strValue=" + $("#txtFormulaurySearchMaster").val();
                break;
            case 4:
                window.location.href = "/DrugSearch/Index?strValue=" + $("#txtDrugNdcSearchMaster").val();
                break;
            case 5:
                window.location.href = "/MacDrug/Index?strValue=" + $("#txtMacDrugSearchMaster").val();
                break;
            case 6:
                window.location.href = "/DrugList/Index?strValue=" + $("#txtDrugListSearchMaster").val();
                break;
        }
    },

    "fn_ClearTextbox": function (id) {
        $("#" + id).val('');
    }
};

$(document).ready(function () {
    $("#txtNABPSearchMaster").keyup(function (event) { 
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            master.fn_SerachMaster(1);
        }
    });

    $("#txtPlanSearchMaster").keyup(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            master.fn_SerachMaster(2);
        }
    });

    $("#txtFormulaurySearchMaster").keyup(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            master.fn_SerachMaster(3);
        }
    });

    $("#txtMacDrugSearchMaster").keyup(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            master.fn_SerachMaster(5);
        }
    });

    $("#txtDrugListSearchMaster").keyup(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            master.fn_SerachMaster(6);
        }
    });


    $("#txtDrugNdcSearchMaster").keyup(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            master.fn_SerachMaster(4);
        }
    });

});
