
//First Row of Table Set Selected Functionality
$(document).ready(function () {
    setTimeout(function () {
        $('table.singleSelect tbody tr:first').addClass("selected").click();
        $('table.multipleSelect tbody tr:first').addClass("selected").click();
    }, 500);


}); 

var jqGrigTable = {
    "fn_GetTablePageIndex": function (pageIndex, url, divHolder = "#divClaimList", formId = "SearchForm") {
        var $container = $(divHolder);
        var pageSize = parseInt($container.find('#pagesizevalue option:selected').text());
        this.fn_GetTableSortingOnEachPage(pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetTablePageSize": function (obj, url, divHolder = "#divClaimList", formId = "SearchForm") {
        var pageSize = parseInt($(obj).find('option:selected').text());
        var pageIndex = 1;
        this.fn_GetTableSortingOnEachPage(pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetTableSortingOnEachPage": function (pageIndex, pageSize, url, divHolder = "#divClaimList", formId = "SearchForm") {
        var sortingName = "";
        var sortingIco = "Ascending";
        var $container = $(divHolder);
        var isSortingFound = false;
        $.each($container.find('table .sortingIco'),
            function (index, value) {
                //debugger;
                if ($(value).attr('class') === 'sortingIco desc') {
                    sortingName = $(value).parent().parent().find('span').text();
                    sortingIco = "ASC";
                    isSortingFound = true;
                    return false;
                } else if ($(value).attr('class') === 'sortingIco ace') {
                    sortingName = $(value).parent().parent().find('span').text();
                    sortingIco = "DESC";
                    isSortingFound = true;
                    return false;
                }
               
            });
        if (!isSortingFound) {
            //debugger;
            sortingName = $container.find('table .th:first').parent().find('span').text();
            sortingIco = "Ascending";
        }
        //debugger;
        this.fn_GetTableSearchList(sortingName.trim(), sortingIco, pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetSortingItem": function (sortBy, sortOrder, url, divHolder = "#divClaimList", formId = "SearchForm") {
        var $container = $(divHolder);
        var pageIndex = parseInt($container.find('.pagination .active').text());
        var pageSize = parseInt($container.find('#pagesizevalue option:selected').text());
        this.fn_GetTableSearchList(sortBy, sortOrder, pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetTableSearchList": function (sortBy, sortOrder, pageindex, pageSize, url, divHolder = "#divClaimList", formId = "SearchForm") { 
        //debugger;
        if (sortOrder === "ASC") {
            sortOrder = "DESC";
        }
        else if (sortOrder === "DESC") {
            sortOrder = "ASC";
        }
        else
             sortOrder = "ASC";
           


        var obj = {};
        $("#" + formId).serializeArray().map(function (x) {
            obj[x.name] = x.value;
        }); 
        parameter = obj;

        $(divHolder).addClass('ajaxLoading');
        $.ajax({
            type: "POST",
            url: url,
            data: { searchModelEntity: parameter, searchBy: "", sortBy: sortBy, sortOrder: sortOrder, pageIndex: pageindex, pageSize: pageSize },
            cache: false,
            success: function (data) {

                if (data.search("sessionModel") > 0) {
                    //debugger;
                    location.reload();
                }

                var lowerLimit = (pageindex - 1) * pageSize + 1;
                var upperLimit = (pageindex - 1) * pageSize + $(data).find('tbody tr:not(".norecord")').length;
                var getTime = new Date().getTime();
                //debugger
                $(divHolder).html(data);
                //code written for set first row selected of table: by Bharat'
                $(divHolder + ' table').find("tbody tr:first").click(); 

                $(divHolder).removeClass('ajaxLoading');
                $("#pagesizevalue option").filter(function (index) { return $(this).text() === String(pageSize); }).attr('selected', 'selected');
                if ($(data).find('tbody tr:not(".norecord")').length == 0) {
                    setTimeout(function () {
                        $(divHolder).find('fieldset [type="text"]:first').focus();
                    }, 200);
                }
            }
        });
    },

    "fn_GetLoadingDiv": function (strLoadindDiv) {
        if (typeof strLoadindDiv === "undefined") {
            return "#divClaimList";
        }
        return strLoadindDiv;
    }
};
 