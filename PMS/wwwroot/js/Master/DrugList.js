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
    drugList.fn_BindListOnLoad();
});


var drugList = {
    "fn_BindListOnLoad": function () {
        var obj = {};
        $("#SearchForm").serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });
        $.post("/DrugList/FilteredDrugList", { searchModelEntity: obj, sortBy: "", pageIndex: 1 },
            function (data) {
                $("#dvClaimPartial").html(data);
            });
    },
    "fn_GetDrugListLoad": function () {
        var _filterObj = new Object();
        _filterObj.strValue = commanAjax.fn_GetUrlVars()["strValue"];

        $.ajax({
            type: "GET"
            , url: '/DrugList/FilteredDrugList'
            , data: _filterObj
            , success: function (result) {
                $("#dvDrugListPartial").html("");
                $("#dvDrugListPartial").html(result);
            }
        });
    },
    "fn_filterDrugList": function () {
        var _filterObj = new Object();
        _filterObj.strValue = $("#txtSelectionInfo").val();

        $.ajax({
            type: "POST"
            , url: '/DrugList/FilteredDrugList'
            , data: _filterObj
            , success: function (result) {
                $("#dvDrugListPartial").html("");
                $("#dvDrugListPartial").html(result);

            }
        });
    },
    "fn_BidDetails": function (id, forName) {
        $("#dDrugListName").html("Drug List Details (" + forName + ")");
        var _filterObj = new Object();
        _filterObj.drugListId = id;

        $.ajax({
            type: "POST"
            , url: '/DrugList/DrugListDetails'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                }
                else {
                    $('#DrugListId').val(id);
                    $("#dvDrugDetailsHeading").show();
                    $("#dvDrugListDetailsPartial").html(result);
                    $("#dvDrugDetailsHeading").find("#SearchRecord").val('');
                    commanAjax.fn_SetActiveFirstRow("#tblDrugDetailList");
                    $("#THSDrugListDetailModel").modal("show");
                }
              
            }
        });
    },
    "fn_SearchDrugList": function () {
        var _filterObj = new Object();
        _filterObj.drugListId = $('#hdnSearchRecordDrugListId').val();
        _filterObj.searchRecord = $('#txtSearchRecord').val().replace("Search NDC", "");

        $.ajax({
            type: "POST"
            , url: '/DrugList/DrugListDetailsSearchRecords'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                } else {
                    $("#dvDrugListDetailsPartial").html(result);
                }
            }
        });
    },
    "fn_GetUrl": function () {
        return "/DrugList/FilteredDrugList";
    },
    "fn_GetUrlInnerList": function () {
        return "/DrugList/DrugListDetails";
    },
    "fn_GetLoadingDiv": function () {
        return "#divDrugListDetailPartial";
    },
    "fn_GetInnerFormName": function () {
        return "SearchFormInner";
    },
    "fn_CloseCurrentModel": function () {
        setTimeout(function () {
            if ($("#tblDrugList").find("tbody tr:first") != undefined && $("#tblDrugList tbody tr:first").find('td').length > 1) {
                $('#tblDrugList tr.selected').click();
            }

        }, 100);
    },
    "fn_BidDetailsOnReset": function () {
        //$("#dDrugListName").html("Drug List Details (" + forName + ")");
        var id = $("#hdnDrugListId").val();
        var _filterObj = new Object();
        _filterObj.drugListId = id;

        $.ajax({
            type: "POST"
            , url: '/DrugList/DrugListDetails'
            , data: _filterObj
            , success: function (result) {
                if (result.search("sessionModel") > 0) {
                    location.reload();
                }
                else {
                    $('#DrugListId').val(id);
                    $("#dvDrugDetailsHeading").show();
                    $("#dvDrugListDetailsPartial").html(result);
                    commanAjax.fn_SetActiveFirstRow("#tblDrugDetailList");
                    $("#THSDrugListDetailModel").modal("show");
                }
              
            }
        });
    }
};


