$(function () {
    $('#txtSelectionInfo').on("keypress", function (e) {
        if (e.keyCode == 13) {
            $('#search').click();
        }
    });
});
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
            SearchRecord();
        }
    });
});

$(document).ready(function () {
    macDrug.fn_BindListOnLoad();
});


var macDrug = {
    "fn_BindListOnLoad": function () {
        var obj = {};
        $("#SearchForm").serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });
        $.post("/MacDrug/FilteredMacDrug", { searchModelEntity: obj, sortBy: "", pageIndex: 1 },
            function (data) {
                $("#dvClaimPartial").html(data);
            });
    },
    "fn_GetMacDrugListLoad": function () {
        var _Obj = new Object();
        _Obj.strValue = commanAjax.fn_GetUrlVars()["strValue"];

        $.ajax({
            type: "GET"
            , url: '/MacDrug/FilteredMacDrug'
            , data: _Obj
            , success: function (result) {
                $("#dvMacDrugListPartial").html("");
                $("#dvMacDrugListPartial").html(result);
            }
        });
    },
    "fn_filterMacDrug": function () {
        var _filterObj = new Object();
        _filterObj.strValue = $("#txtSelectionInfo").val();

        $.ajax({
            type: "POST"
            , url: '/MacDrug/FilteredMacDrug'
            , data: _filterObj
            , success: function (result) {
                $("#dvMacDrugListPartial").html("");
                $("#dvMacDrugListPartial").html(result);

            }
        });
    },
    "fn_BidDetails": function (id, forName) {
        $("#dMacDrugListName").html("MAC Drug List Details (" + forName + ")");

        var _filterObj = new Object();
        _filterObj.macDrugListId = id;
        $("#divClaimList").addClass('ajaxLoading');
        $.ajax({
            type: "POST"
            , url: '/MacDrug/MacDrugListDetails'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                }
                else {
                    $('#MacDrugListId').val(id);
                    $("#dvMacDrugDetailsHeading").show();
                    $("#dvMacDrugListDetailsPartial").html(result);
                    $("#dvMacDrugDetailsHeading").find("#SearchRecord").val('');
                    commanAjax.fn_SetActiveFirstRow("#tblMacDrugDetailList");
                    $("#THSMACDrugListDetailModel").modal("show");
                    $('#divClaimList').removeClass('ajaxLoading');
                }
               
            }
        });
    },
    "fn_SearchMacDrug": function () {
        var _filterObj = new Object();
        _filterObj.macDrugListId = $('#hdnSearchRecordMacDrugListId').val();
        _filterObj.searchRecord = $('#txtSearchRecord').val().replace("Search NDC", "");

        $.ajax({
            type: "POST"
            , url: '/MacDrug/MacDrugListDetailsSearchRecords'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                }
                else {
                    $("#dvMacDrugListDetailsPartial").html(result);
                    var RecordsFound = $("#dvMacDrugListDetailsPartial  #claimSearch_info").text();
                    if (typeof (RecordsFound) != "undefined" && RecordsFound.trim() != "") {
                        RecordsFound = RecordsFound.split(" ")[5] + " record(s) found";
                        //$("#spnSearchRecords").text(RecordsFound);
                    }
                    else {
                        //$("#spnSearchRecords").text("No record(s) found");
                    }
                }
               
            }
        });
    },
    "fn_GetUrl": function () {
        return "/MacDrug/FilteredMacDrug";
    },
    "fn_GetInnerFormName": function () {
        return "SearchFormInner";
    },
    "fn_GetUrlInnerList": function () {
        return "/MacDrug/MacDrugListDetails";
    },
    "fn_GetLoadingDiv": function () {
        return "#divDrugListDetail";
    },
    "fn_CloseCurrentModel": function () {
        setTimeout(function () {
            if ($("#tblMacDrugList").find("tbody tr:first") != undefined && $("#tblMacDrugList tbody tr:first").find('td').length > 1) {
                $('#tblMacDrugList tr.selected').click();
            }

        }, 100);
    },
    "fn_BidDetailsOnReset": function () {
        //$("#dMacDrugListName").html("Mac Drug List Details (" + forName + ")");
        var id = $("#hdnMacDrugListId").val();
        var _filterObj = new Object();
        _filterObj.macDrugListId = id;

        $.ajax({
            type: "POST"
            , url: '/MacDrug/MacDrugListDetails'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $('#MacDrugListId').val(id);
                    $("#dvMacDrugDetailsHeading").show();
                    $("#dvMacDrugListDetailsPartial").html(result);
                    commanAjax.fn_SetActiveFirstRow("#tblMacDrugDetailList");
                    $("#THSMACDrugListDetailModel").modal("show");
                }
              
            }
        });
    } 
};



