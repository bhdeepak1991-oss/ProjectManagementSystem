
$(function () {
    // For table sorting and expanding - start //
    if ($("[data-expand-collapse]").length) {
        $("[data-expand-collapse]").each(function () {
            var container = $(this), targetLink = $(this).data("expand-collapse");
            $(targetLink).on('click', function () {
                if (container.find('tr').length <= 2) {
                    return;
                }
                $(this).parents('tr').siblings('tr').find('td').slideUp(200);
                if (container.find('div.expIcon').length == 0) {
                    container.css('position', 'relative')
                    $('<div class="expIcon curPointer posAbs f22" title="Expand">[+]</div>').css({
                        'bottom': '8px',
                        'right': '-30px',
                    }).prependTo(container)
                        .on('click', function () {
                            container.find('td').slideDown(200);
                            $(this).remove();
                        });
                }
            });
        });
    }

    $('#txtSearchRecord').on("keypress", function (e) {

        if (e.keyCode == 13) {
            // SearchRecord();
        }
    });
    $('#txtSelectionInfo').on("keypress", function (e) {
        if (e.keyCode == 13) {
            $('#search').click();
        }
    });
});
$(document).ready(function () {
    formulary.fn_GetFormularyListLoad();

});


var formulary = {
    "fn_GetFormularyListLoad": function () {
        var obj = {};
        $("#SearchForm").serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });
        $.post("/Formulary/FilteredFormulary", { searchModelEntity: obj, sortBy: "", pageIndex: 1 },
            function (data) {
                if (data.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvClaimPartial").html(data);
                }
            });
    },
    "fn_FilterFormulary": function () {
        var _filterObj = new Object();
        _filterObj.strValue = $("#txtSelectionInfo").val();

        $.ajax({
            type: "POST"
            , url: '/Formulary/FilteredFormulary'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvFormularyPartial").html("");
                    $("#dvFormularyPartial").html(result);
                }
            }
        });
    },
    "fn_BidDetails": function (id, bViewRestricted, forName) {
        $("#modalHeader").html("Formulary Details (" + forName + ")");
        document.getElementById("chkShowAll").checked = bViewRestricted;
        var _isShowAll = false;
        if (bViewRestricted) {
            showRestrictedNDC = 1;
            _isShowAll = true;
        }
        $('[name="ShowAll"]').val(_isShowAll);

        var _filterObj = new Object();
        _filterObj.formularyId = id;
        _filterObj.showAll = _isShowAll;
        $("#divClaimList").addClass('ajaxLoading');
        $.ajax({
            type: "POST"
            , url: '/Formulary/FormularyDetails'
            , data: _filterObj
            , success: function (result) {
                //debugger
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $('#FormularyId').val(id);
                    $("#dvFormularyDetailsHeading").show();
                    $("#dvFormularyDetailsPartial").html(result);
                    $("#dvFormularyDetailsHeading").find("#SearchRecord").val('');
                    commanAjax.fn_SetActiveFirstRow("#tblFormularyDetails");
                    $("#THSFormularyDetailModel").modal("show");
                    $("#divClaimList").removeClass('ajaxLoading');
                }
            }
        });
    },
    "fn_SearchFormulary": function () {
        $("#dvFormularyDetailsPartial").addClass('ajaxLoading');
        var _isShowAll = false;
        if (document.getElementById("chkShowAll").checked) {
            _isShowAll = true;
        }
        $('[name="ShowAll"]').val(_isShowAll);
        var _formularyId = $('#FormularyId').val();

        var _filterObj = new Object();
        _filterObj.formularyId = _formularyId;
        _filterObj.searchRecord = $('#SearchRecord').val().replace("Search NDC", "");
        _filterObj.showAll = _isShowAll;

        $.ajax({
            type: "POST"
            , url: '/Formulary/FormularyDetails'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $('#FormularyId').val(_formularyId);
                    $("#dvFormularyDetailsHeading").show();
                    $("#dvFormularyDetailsPartial").html(result);
                    commanAjax.fn_SetActiveFirstRow("#tblFormularyDetails");
                    $("#dvFormularyDetailsPartial").removeClass('ajaxLoading');
                } 
            }
        });
    },
    "fn_GetUrl": function () {
        return "/Formulary/FilteredFormulary";
    },
    "fn_GetUrlInnerList": function () {
        return "/Formulary/FormularyDetails";
    },
    "fn_GetLoadingDiv": function () {
        return "#dvClaimPartial";
    },
    "fn_GetLoadingInnerDiv": function () {
        return "#divFormualryDetail";
    },
    "fn_GetInnerFormName": function () {
        return "formularyDetailForm";
    },
    "fn_GetFormularyDetailForm": function () {
        return "formularyDetailForm";
    },
    "fn_CloseCurrentModel": function () {
        setTimeout(function () {
            if ($("#tblFormulary").find("tbody tr:first") != undefined && $("#tblFormulary tbody tr:first").find('td').length > 1) {
                $('#tblFormulary tr.selected').click();
            }

        }, 100);
    },
    "fn_BidDetailsOnReset": function () {
        var id = $("#hdnFormularyId").val();
        var bViewRestricted = false;
        var forName = id;
        //$("#modalHeader").html("Formulary Details (" + forName + ")");
        document.getElementById("chkShowAll").checked = bViewRestricted;
        var _isShowAll = false;
        if (bViewRestricted) {
            showRestrictedNDC = 1;
            _isShowAll = true;
        }
        $('[name="ShowAll"]').val(_isShowAll);

        var _filterObj = new Object();
        _filterObj.formularyId = id;
        _filterObj.showAll = _isShowAll;

        $.ajax({
            type: "POST"
            , url: '/Formulary/FormularyDetails'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $('#FormularyId').val(id);
                    $("#dvFormularyDetailsHeading").show();
                    $("#dvFormularyDetailsPartial").html(result);
                    commanAjax.fn_SetActiveFirstRow("#tblFormularyDetails");
                    $("#THSFormularyDetailModel").modal("show");
                }
            }
        });
    },

};

