var commanAjax = {
    "AjaxSuccess": function (response) {
        commanAjax.Fn_CommanAlert(response, 'Alert!');
    },
    "fn_GetUrlVars": function () {
        // Read a page's GET URL variables and return them as an associative array.
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    "Fn_AjaxBegin": function (divId) {
        $("#" + divId).addClass('ajaxLoading');
        $.get("/Authenticate/CheckSessionPresist", function (data) {
            if (!data) {
                location.reload();
            }
        });
    },
    "Fn_AjaxComplete": function (divId) { 
        $("#" + divId).removeClass('ajaxLoading');
        //code written by bharat'
        $("#" + divId + ' table').find("tbody tr:first").click();

    },
    "Fn_AlertMessage": function (message, header, size) {
        var alertString = '<div class="modal-dialog ' + size + '">';
        alertString += '<div class="modal-content">';
        alertString += '<div class="modal-header">';
        alertString += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        alertString += '<h4 class="modal-title">' + header + '</h4>';
        alertString += '</div><!--modal-header end-->';

        alertString += '<div class="modal-body"> ' + message + '';
        alertString += '</div><!--modal-body end-->';

        alertString += '<div class="modal-footer">';
        alertString += '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>';
        alertString += '</div>';
        alertString += '</div >';

        alertString += '</div >';
        $('#myModelAlert').modal('show').html(alertString);
    },
    "Fn_ConfirmMessage": function (message, yesCallBackFnct, noCallBackFnct) {

        var alertString = '<div class="modal-dialog sm">';
        alertString += '<div class="modal-content">';
        alertString += '<div class="modal-header">';
        alertString += '<h4 class="modal-title"> Confirm </h4>';
        alertString += '</div><!--modal-header end-->';

        alertString += '<div class="modal-body"> ' + message + '';
        alertString += '</div>';

        alertString += '<div class="modal-footer">';
        alertString += '<input type="button" id="btnYes" value="Ok" class="btn btn-default" />';
        alertString += '<button type="button" id="btnNo" class="btn btn-secondary" data-dismiss="modal">Cancel</button>';
        alertString += '</div>';
        alertString += '</div >';

        alertString += '</div >';
        $('#myModelAlert').modal('show').html(alertString);
        $("#btnYes").click(function () {
            $('#myModelAlert').modal('hide');
            yesCallBackFnct();
        });
        $("#btnNo").click(function () {
            noCallBackFnct();
        });
    },
    "Fn_ClearForm": function (form, divId) {
        $.get("Authenticate/CheckSessionPresist", function (data) {
            if (!data) {
                location.reload();
            }
        });
        $("#" + form).clearForm();
        //code written by bharat'
        commanAjax.fn_SetActiveFirstRow(divId);
    },
    "fn_SetActiveFirstRow": function (divId) {
        setTimeout(function () {
            if ($(divId).find("tbody tr:first") != undefined && $(divId + " tbody tr:first").find('td').length > 1) {
                $(divId + " tbody tr:first").addClass("selected").click();
            }

        }, 100);
        $(divId).removeClass('ajaxLoading');
    },
    "Fn_CommanAlert": function (response, title) {
        jQuery.alert({
            title: title,
            buttons: {
                ok: {
                    text: 'Ok',
                    btnClass: 'btn-blue',
                    keys: ['enter'],
                }
            },
            content: response,
            draggable: false
        });
    },
    "Fn_CheckSessionExists": function () {

    }
};

$(document).ready(function () {
    $(".date").prop("readonly", true);
    $("body").delegate(".number", "keypress", function (event) {
        //Allow only two digit after decimal-- added by Bharat 
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') == -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });
    $("body").delegate(".numberOnly", "keydown", function (event) {
        if (event.shiftKey == true) {
            event.preventDefault();
        }

        if ((event.keyCode >= 48 && event.keyCode <= 57) ||
            (event.keyCode >= 96 && event.keyCode <= 105) ||
            event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
            event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

        } else {
            event.preventDefault();
        }

        if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
            event.preventDefault();
        //if a decimal has been added, disable the "."-button

    });
    $("body").delegate(".txtOnly", "keypress", function (event) {
        var key = event.keyCode;
        if (key >= 48 && key <= 57) {
            event.preventDefault();
        }
    });
    $("body").delegate(".selectAll", "click", function (event) {
        $(this).select();
    });
    $("body").delegate(".noSpaceOnStart", "keypress", function (event) {
        if (event.which === 32 && !this.value.length)
            event.preventDefault();
    });

    $("body").delegate(".allowMinus", "keypress", function (event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (
            (charCode != 45 || $(this).val().indexOf('-') != -1) &&      // Check minus and only once.
            (charCode != 46 || $(this).val().indexOf('.') != -1) &&      // Check dot and only once.
            (charCode < 48 || charCode > 57))
            event.preventDefault();

        var text = $(this).val();
        // Check and allow only 2 digit after decimal point.
        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });

});

$.fn.clearForm = function () {
    return this.each(function () {
        var type = this.type, tag = this.tagName.toLowerCase();
        if (tag == 'form')
            return $(':input', this).clearForm();
        if (type == 'text' || type == 'password' || tag == 'textarea' || type == 'search') {
            this.value = '';
        }

        else if (type == 'checkbox' || type == 'radio')
            this.checked = false;
        else if (tag == 'select') {
            $(this.options[0]).attr('selected', 'selected')
        }

    });
};



