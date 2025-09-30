
$(document).ready(function () {
    $("#dvSearch").focusin(function () {
        $("#dvSearch").addClass("focus");
    });
    $("#dvSearch").focusout(function () {
        $('#dvSearch').removeClass("focus");
    });

    $("#dvSearchInner").focusin(function () {
        $("#dvSearchInner").addClass("focus");
    });
    $("#dvSearchInner").focusout(function () {
        $('#dvSearchInner').removeClass("focus");
    });

    $("#dvSearchAdvance").focusin(function () {
        $("#dvSearchAdvance").addClass("focus");
    });
    $("#dvSearchAdvance").focusout(function () {
        $('#dvSearchAdvance').removeClass("focus");
    });

    $("#dvFormSubmit").focusin(function () {
        $("#dvFormSubmit").addClass("focus");
    });
    $("#dvFormSubmit").focusout(function () {
        // $('#dvFormSubmit').removeClass("focus");
    });
    $("#Utilitysearch").focusin(function () {
        $("#Utilitysearch").addClass("focus");
    });
    $("#Utilitysearch").focusout(function () {
        $('#Utilitysearch').removeClass("focus");
    }); 

});


function onBegin() {
    $("#_divClaimList").addClass('ajaxLoading');
}

function onSuccess(response) {
    $("#_divClaimList").removeClass('ajaxLoading');
    if (response.isSuccess) {
        showToast(response.message, "success");
    }
    showToast(response.message, "error");
    $("#_divClaimList").removeClass('ajaxLoading');
}
function enterFunctinality() {
    //Enter Key Press Functionality for Search Button
    $(document).keydown(function (e) {
        debugger
        let $container = $('.modal.fade:visible').length > 0 ? $('.modal.fade:visible') : $(document);
        var focusElement = $container.find('button').is(':focus') === false ? $container.find('[type="button"]').is(':focus') : $container.find('button').is(':focus');
        if (e.which == 13) {
            e.preventDefault();

            if (!$('#dialogconfirm:visible,#dialogalert:visible').length) {
                if (focusElement) {
                    $container.find('button:focus').click();
                }
                else if ($container.find('#dvSearch').hasClass('focus')) {
                    //modified for THSb 
                    $container.find('#dvSearch').find('.clsGo:visible').trigger("click");
                }
                else if ($container.find('#dvSearchInner').hasClass('focus')) {
                    //modified for THS
                    $container.find('#dvSearchInner').find('.clsGo:visible').trigger("click");
                }
                else if ($container.find('#dvSearchAdvance').hasClass('focus')) {     
                    //modified for THS
                    $container.find('#dvSearchAdvance').find('.clsGo:visible').trigger("click");
                }
                else if ($container.find('#dvFormSubmit').hasClass('focus')) {
                    //modified for THS
                    $container.find('#dvFormSubmit').find('.clsGo:visible').trigger("click");
                }
                else if ($container.find('#Utilitysearch').hasClass('focus')) {
                    //modified for THS
                    $container.find('#Utilitysearch').find('.clsGo:visible').trigger("click");
                }
                else {
                    //debugger;
                    var popped = false;
                    $.each($container.find(".table, .table-hover, .table-striped, .singleSelect, .singleUtilitySelect").find('tbody tr'), function (index, value) {
                        //debugger
                        if (value.className == "selected" && !popped) {
                            //debugger;
                            popped = true;
                            value.ondblclick.call();
                        }
                    });
                }
            }
            else {
                if ($('#dialogconfirm:visible').length > 0) {
                    if (focusElement) {
                        $container.find('button:focus').click();
                    }
                    else /*if ($(".onEnter", $container).is(':focus')) */ {
                        $container.find(".clsGo:visible").trigger("click");
                    }
                    $('.ui-dialog-buttonset button:first').trigger('click');
                }
                else {
                    if ($('#dialogalert:visible').length == 0) {
                        $('.ui-dialog-buttonset button:button').trigger('click');
                    }
                    $('#dialogalert').next().find('.ui-dialog-buttonset button').trigger('click');
                }
            }
        }
    });
}

function showToast(message, type = "success") {
    const container = document.getElementById("toast-container");
    $("#_divClaimList").removeClass('ajaxLoading');
    const toast = document.createElement("div");
    toast.className = `toast toast-${type}`;
    toast.style.cssText = `
            background: ${type === "success" ? "#28a745" : "#dc3545"};
            color: white;
            padding: 10px 20px;
            margin-top: 10px;
            border-radius: 5px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.2);
            opacity: 0;
            transform: translateX(100%);
            transition: all 0.5s ease;
        `;
    toast.innerText = message;

    container.appendChild(toast);

    // animate in
    setTimeout(() => {
        toast.style.opacity = "1";
        toast.style.transform = "translateX(0)";
    }, 100);

    // auto-remove after 3s
    setTimeout(() => {
        toast.style.opacity = "0";
        toast.style.transform = "translateX(100%)";
        setTimeout(() => toast.remove(), 500);
    }, 3000);
}

// Hook into unobtrusive ajax lifecycle
function onSuccess(response) {
    console.log("AJAX success", response);
    showToast(response.Message, "success");
}

function onError(xhr, status, error) {
    console.error("AJAX error", error);
    showToast("Something went wrong. Please try again.", "error");
}

/* detect and return Internet Explorer version */
function detectIE() {
    var ua = window.navigator.userAgent;

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    var trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
        // Edge (IE 12+) => return version number
        return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
    }

    // other browser
    return false;
}

if (detectIE()) { $('body').addClass('browserIE browserVersion' + detectIE()); }

//Search for keywords within the user agent string.  When a match is found, add a corresponding class to the html element and return.  (Inspired by http://stackoverflow.com/a/10137912/114558)
function addUserAgentClass(keywords) {
    for (var i = 0; i < keywords.length; i++) {
        if (navigator.userAgent.indexOf(keywords[i]) != -1) {
            $('html').addClass(keywords[i].toLowerCase());
            return; //Once we find and process a matching keyword, return to prevent less "specific" classes from being added
        }
    }
}
addUserAgentClass(['Chrome', 'Firefox', 'MSIE', 'Safari', 'Opera', 'Mozilla']); //Browsers listed generally from most-specific to least-specific
addUserAgentClass(['Android', 'iPhone', 'iPad', 'Linux', 'Mac', 'Windows']); //Platforms, also in order of specificity



function setMinHeight() {
    if (($('.inner-body').length) && ($(window).width() < 1280) && ($(window).width() > 767)) {
        if ($('.inner-body').hasClass('noButton')) {
            var minH = $(window).height() - 255;
        } else {
            var minH = $(window).height() - 300;
        }
        $('.inner-body').css('min-height', minH);
    } else {
        $('.inner-body').removeAttr("style");
    }
}

function setThWidth() {
    if ($('.table-fixed-header thead .text-right').length || $('.table-fixed-header thead .text-center').length) {
        $('.table-fixed-header thead .th, .table-fixed-header tfoot .th').each(function (index, element) {
            var thWidth = $(element).parent('th').innerWidth();
            $(element).css('width', thWidth + 'px');
        });
    }
}

function setTable() {
    /*if ($('.has-v-scroll > div').length) {
        var offsetL;
        var tbloffsetL = $('.has-v-scroll > div').offset().left;
        $('.has-v-scroll thead .th').each(function (index, element) {
            offsetL = $(element).parent('th').offset().left;
            offsetL = (offsetL - tbloffsetL) + 10;           
            $(element).css('left', offsetL + 'px');           
        });
        if (offsetL > 50) {
            $('.has-v-scroll').addClass('_done');
        }
    }*/


    if ($('.has-v-scroll > div').length) {
        var offsetL, tbloffsetL, scrollId;
        $('.has-v-scroll > div').each(function (index, patentElement) {
            scrollId = '#' + $(patentElement).attr('id');
            if (scrollId != "#undefined") {
                tbloffsetL = $(scrollId).offset().left;

                $('' + scrollId + ' thead .th').each(function (index, element) {
                    offsetL = $(element).parent('th').offset().left;
                    offsetL = (offsetL - tbloffsetL) + 10;
                    $(element).css('left', offsetL + 'px');
                });
                if (offsetL > 50) {
                    $(scrollId).parent('.has-v-scroll').addClass('_done');
                }
            }

        });
    }
}

function setSubNavWidth() {
    if ($('.top-right .nav-h').length) {
        //$('.top-right').width($(window).width() - ($('.top-left').width() + 110));
        $('.top-right').css('width', $(window).width() - ($('.top-left').width() + $('.top-right').width() + 50));
    }
}

function headerTransition() {
    $('.search-fields').addClass('animation');
    setTimeout(function () {
        $('.search-fields').removeClass('animation');
    }, 700);
}

function setTooltip() {
    if ($('.custom-tooltip[data-tooltip]').length) {
        $('.custom-tooltip[data-tooltip]').mousemove(function (event) {
            if (!$('.tooltipDiv').length) {
                $("<div class='tooltipDiv'></div>").appendTo('body');
            }
            $('.tooltipDiv').html($(this).attr('data-tooltip')).css({
                'left': event.pageX,
                'top': event.pageY
            }).show();
        }).mouseleave(function () {
            $('.tooltipDiv').remove();
        });
    }
}

function onlyNumericAllow(obj, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode !== 46 || $(obj).val().indexOf(".") !== -1)) {
        return false;
    }
    $(obj).on('paste', function (event) {
        if (event.originalEvent.clipboardData.getData('Text').match(/[^\d]/)) {
            event.preventDefault();
            return false;
        }
    });
    return true;
}

//Restrict to Enter only Number
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

$(document).ready(function () {
    //Remove tab from readonly fields
    $('form').find('input[readonly]').prop('tabindex', '-1');
    setMinHeight();
    //setSubNavWidth();
    //setThWidth();

    if ((!$('.body').hasClass('browserIE')) && detectIE()) {
        $('body').addClass('browserIE browserVersion' + detectIE());;
    }

    /*Script for new theme implementation start*/
    if ($('.top-right .nav-h').length) {
        setTimeout(function () {
            $('.top-right .nav-h').flexMenu({
                showOnHover: false
            });
            $('.top-right').addClass('_visible');
        }, 200);
    }

    $('.header-right .dropdown-menu li a.store').on('click', function (e) {
        e.stopPropagation();
        $(this).toggleClass('open').next('ul').slideToggle();
    });

    $('.filterBtn').on('click', function (e) {
        if ($(this).parents('.graphFilter').hasClass('active')) {
            $(this).parents('.graphFilter').removeClass('active');
        } else {
            $(this).parents('.graphFilter').addClass('active');
        }
    })


    $('.advSearch').click(function (e) {
        e.stopPropagation();
        //headerTransition();
        $('body').removeClass('slideLeft');
        $('.search-type ul').hide();
        $('.user-action').removeClass('open');

        if ($(this).hasClass('active')) {
            $(this).removeClass('active').parents('.header-search').removeClass('open').find('.header-search-wrap').slideUp();
        } else {
            $(this).addClass('active').parents('.header-search').addClass('open').find('.header-search-wrap').slideDown();
            $('.header-search.open').find('.header-search-wrap:visible .search-inner .search-fields>div:visible input:first').focus();
        }
    });


    $('.dispensSearch .advSearchBtn').click(function (e) {
        if ($(this).parents('.dispensSearch').hasClass('active')) {
            $(this).parents('.dispensSearch').removeClass('active');
        } else {
            $(this).parents('.dispensSearch').addClass('active');
        }
    });

    $("body").delegate("fieldset.toggleSearch > legend", "click", function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active').next().slideUp();
        } else {
            $(this).addClass('active').next().slideDown();
        }
    });

    $("body").delegate(".form-block.toggleSearch > .head", "click", function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active').next().slideUp();
        } else {
            $(this).addClass('active').next().slideDown();
        }
    });


    $('.search-type > .label').on('click', function (e) {
        e.stopPropagation();
        //headerTransition();
        $('.search-type > ul').toggle();
        $('.user-action').removeClass('open');
        //$('.advSearch').removeClass('active');
        //$('.search-fields').removeClass('active');
    });

    $('.search-type ul li a').click(function () {
        $('.search-type > .label').text($(this).text());
        $('.search-type ul').hide();
    });

    $('.user-action .btn').click(function (e) {
        e.stopPropagation();
        //headerTransition();
        $(this).parent().toggleClass('open').siblings().removeClass('open');
        $('.search-type ul').hide();
        $('.advSearch').removeClass('active');
        $('.search-fields').removeClass('active');
        $('.header-search-wrap').slideUp();
    });

    $('.search-fields, .header-search-wrap').click(function (e) {
        e.stopPropagation();
    });


    $("body").delegate(".search-fieldset .advSearchLink", "click", function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active').find('span').text('+').parents('.search-fieldset').find('.advSearchEle').slideUp();
        } else {
            $(this).addClass('active').find('span').text('-').parents('.search-fieldset').find('.advSearchEle').slideDown();
        }
    });


    $('.chartType li a').click(function (e) {
        $(this).parents('.chartType').find('a').removeClass('active');
        $(this).addClass('active');
    });

    //Add active class on current modal Region  --Gagandeep Singh
    $('.modal.fade').on("show.bs.modal", function (e) {
        var highestActiveValue = 1;
        if (e.namespace === "bs.modal") {
            if ($('.modal.fade:visible').length) {
                var num = $(".modal.fade:visible").map(function () {
                    return $(this).attr('data-active');
                }).get();//get all data values in an array
                if (num.length) {
                    highestActiveValue = Math.max.apply(Math, num) + 1;//find the highest value from them
                }
                else {
                    highestActiveValue = 1;
                }
                $('.modal.fade:visible').removeClass("active");
            }
            $(this).addClass("active").attr('data-active', highestActiveValue);
        }
    });
    $('.modal.fade').on("hidden.bs.modal", function (e) {
        var highestActiveValue = 0;
        if (e.namespace === "bs.modal") {
            $(this).removeClass('active').attr('data-active', 0);
            if ($('.modal.fade:visible').length) {
                var num = $(".modal.fade:visible").map(function () {
                    return $(this).attr('data-active');
                }).get();//get all data values in an array
                if (num.length) {
                    highestActiveValue = Math.max.apply(Math, num);//find the highest value from them
                }
                else {
                    highestActiveValue = 0;
                }
            }
            $('.modal.fade:visible[data-active=' + highestActiveValue + ']').addClass('active');
        }
    });
    // Region End


    $(document).click(function () {
        //headerTransition();
        $('.search-type ul').hide();
        $('.user-action').removeClass('open');
        $('.advSearch').removeClass('active');
        //$('.search-fields').removeClass('active');
        $('.header-search-wrap').slideUp();
    });

    /*Script for new theme implementation end*/

    /*Script for slide Navigation start*/
    $('.slideNav').on('click', function () {
        var direction = $(this).attr('data-direction');
        var className = (direction == 'left') ? 'slideLeft' : 'slideRight';

        //console.log('==>' + className);

        if ($('body').hasClass(className)) {
            $('body').removeClass(className);
        } else {
            $('body').addClass(className);
        }

        if ($('.address-slider').length) {
            for (var i = 0; i < 40; i++) {
                setTimeout(function () {
                    $('.address-slider').slick('setPosition', 0);
                }, i * 10);
            }
        }

        if ($('.addrSlider').length) {
            for (var i = 0; i < 40; i++) {
                setTimeout(function () {
                    $('.addrSlider').slick('setPosition', 0);
                }, i * 10);
            }
        }

        for (var i = 0; i < 40; i++) {
            setTimeout(function () {
                setTable();
            }, i * 10);
        }

        //=================================================================
        if ($('#RxProcessedGraphView').hasClass("active")) {
            $('#RxProcessedColIcon').addClass("active");
            $('#RxProcessedBarIcon').removeClass("active");
            $('#RxProcessedPieIcon').removeClass("active");
            GetRxProcessedDetails('column', false);
        }
        //-----------------------------------------------------------        
        if ($('#ProcessQueueGraphView').hasClass("active")) {
            $('#processQueueColId').removeClass("active");
            $('#processQueueBarId').removeClass("active");
            $('#processQueuePieId').addClass("active");
            GetProcessQueueDetails('pie', true);
        }
        //-----------------------------------------------------------
        if ($('#DrugUtilizationGraphView').hasClass("active")) {
            $('#drugUtilizedColId').removeClass("active");
            $('#drugUtilizedBarId').addClass("active");
            $('#drugUtilizedPieId').removeClass("active");
            GetDrugUtilizationDetails('bar', false);
        }
        //-----------------------------------------------------------
        if ($('#LowInventoryGraphView').hasClass("active")) {
            $('#lowInventoryColId').addClass("active");
            $('#lowInventoryBarId').removeClass("active");
            $('#lowInventoryPieId').removeClass("active");
            GetLowInventoryDetails('column', false);
        }
        //=================================================================
    });
    /*Script for slide Navigation end*/


    $('.search-btn').click(function () {
        $('.header-search').addClass('show');
    });

    $('.cancelBtn').click(function () {
        $('.header-search').removeClass('show');
    });

    $('.date').datetimepicker({
        language: 'fr',
        format: 'mm/dd/yyyy',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0
    }).on('show', function (e) {
        $(this).addClass('prevent');
    }).on('hide', function (e) {
        $(this).removeClass('prevent');
    });
    //$('.date').not('.maxFilter').datetimepicker({
    //    language: 'fr',
    //    format: 'mm/dd/yyyy',
    //    weekStart: 1,
    //    todayBtn: 1,
    //    autoclose: 1,
    //    todayHighlight: 1,
    //    startView: 2,
    //    minView: 2,
    //    forceParse: 0
    //}).on('show', function (e) {
    //    $(this).addClass('prevent');
    //}).on('hide', function (e) {
    //    $(this).removeClass('prevent');
    //});


    //$('.maxFilter').datetimepicker({
    //    language: 'fr',
    //    format: 'mm/dd/yyyy',
    //    weekStart: 1,
    //    todayBtn: 1,
    //    autoclose: 1,
    //    todayHighlight: 1,
    //    startView: 2,
    //    minView: 2,
    //    forceParse: 0,
    //    endDate: new Date()
    //}).on('show', function (e) {
    //    $(this).addClass('prevent');
    //}).on('hide', function (e) {
    //    $(this).removeClass('prevent');
    //});


    //$('.dateTime').datetimepicker({
    //    language: 'fr',
    //    format: 'mm/dd/yyyy - HH:ii P',
    //    showMeridian: true,
    //    autoclose: true,
    //    todayBtn: true,
    //    todayHighlight: 1
    //}).on('show', function (e) {
    //    $(this).addClass('prevent');
    //}).on('hide', function (e) {
    //    $(this).removeClass('prevent');
    //});

    // $.fn.modal.prototype.constructor.Constructor.DEFAULTS.keyboard = true;
    //$.fn.modal.prototype.constructor.Constructor.DEFAULTS.backdrop = 'static';

    //document.addEventListener('keydown', function (e) {
    //    if ($('.modal.active.in').length) {
    //        if (e.keyCode == 27) // The Esc key 
    //        {
    //            $('.modal.active.in:not("#modalAdjudication")').modal('hide');
    //        }
    //    }
    //});

    $(window).on('show.bs.modal', function (e) {
        setTimeout(function () {
            $('.date').datetimepicker({
                language: 'fr',
                format: 'mm/dd/yyyy',
                weekStart: 1,
                todayBtn: 1,
                autoclose: 1,
                todayHighlight: 1,
                startView: 2,
                minView: 2,
                forceParse: 0
            }).on('show', function (e) {
                $(this).addClass('prevent');
            }).on('hide', function (e) {
                $(this).removeClass('prevent');
            });
        }, 250);
    });

    $(window).on('hide.bs.modal', function (e) {
        if ($('.modal-backdrop').length > 1) {
            setTimeout(function () {
                $('body').addClass('modal-open');
            }, 600);
        }
    });

    if ($('.address-navbar').length) {
        var totalInx = $('.address-navbar ul li').length - 1;
        var totalWidth = $('.address-navbar ul li').length * $('.address-navbar ul li').outerWidth();
        var totalScroll = totalWidth - ($('.address-navbar ul li').outerWidth() * 3)
        $('.address-navbar ul').css('width', totalWidth);
        //console.log('==>' + totalWidth + '=>' + totalScroll);        

        $('.address-navbar ul li a').click(function () {
            $('.address-navbar ul li a').removeClass('active');
            $(this).addClass('active');
        });

        $('.address-left-btn').click(function () {

            $(this).attr('disabled', 'disabled');
            var curActive = $('.address-navbar ul li a.active').parent().index();
            var leftPos = $('.address-navbar ul').css('left').replace('px', '');

            //console.log('==>' + leftPos);

            if (leftPos < 0) {
                leftPos = parseInt(leftPos) + parseInt($('.address-navbar ul li').outerWidth());
                $('.address-navbar ul').animate({ left: leftPos });
            }
            if (curActive > 0) {
                $('.address-navbar ul li a.active').removeClass('active').parent().prev().find('a').addClass('active');
            }
            setTimeout(function () {
                $('.address-nav button').removeAttr('disabled');
            }, 500);

        });

        $('.address-right-btn').click(function () {
            $(this).attr('disabled', 'disabled');

            var curActive = $('.address-navbar ul li a.active').parent().index();
            var leftPos = $('.address-navbar ul').css('left').replace('px', '');

            leftPos = parseInt(leftPos) - parseInt($('.address-navbar ul li').outerWidth());

            //console.log('==>' + leftPos);

            if (leftPos + totalScroll >= 0) {
                $('.address-navbar ul').animate({ left: leftPos });
            }

            if (curActive < totalInx) {
                $('.address-navbar ul li a.active').removeClass('active').parent().next().find('a').addClass('active');
            }
            setTimeout(function () {
                $('.address-nav button').removeAttr('disabled');
            }, 500);

        });
    }



    //Modified by Nikesh Checked in By Manish on  26 Mar 2019
    $("body").delegate("table.singleSelect tbody tr", "click", function () {
        $(this).parents('table.singleSelect').find('tr').removeClass('selected');
        $(this).addClass('selected');
        // console.log($(this).find('.selectedRow').prop('checked'))
        if ($(this).find('.selectedRow').is(":not(:checked)")) {
            $(this).find('.selectedRow').prop('checked', true);
        } else {
            $(this).find('.selectedRow').prop('checked', false);
        }

    });
    //Add By Shobhnath
    $('body').on('click', 'table.multipleSelect tbody tr', function (e) {
        if (e.ctrlKey) {
            $(this).addClass('selected');
        }
        else {
            $(this).parents('table.multipleSelect').find('tr').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    //End Multiple Select

    if ($('aside.sidebar > nav').length) {
        var windowHeight = $(window).height() - 50;
        var navigationTop = 0;
        var subNavigationTop = 0;
        if ($('.sidebar > nav > ul > li > .active').length) {
            navigationTop = $('.sidebar > nav > ul > li > .active').offset().top;
        }

        if ($('.sidebar > nav > ul > li > .active + ul > li .active').length) {
            subNavigationTop = $('.sidebar > nav > ul > li li .active').offset().top;
        }

        if (subNavigationTop > windowHeight) {
            $('aside.sidebar > nav').scrollTop(subNavigationTop - windowHeight);
        } else if (navigationTop > windowHeight) {
            $('aside.sidebar > nav').scrollTop(navigationTop - windowHeight);
        }
    }

    if ($('.toggleWrap').length) {
        $('.toggleWrap > h3').on('click', function (e) {

            if ($(this).hasClass('active')) {
                $(this).removeClass('active').next().slideUp();
            } else {
                $('.toggleWrap > h3').removeClass('active');
                $('.tpggleTxt').slideUp();
                $(this).addClass('active').next().slideDown();
            }
        })
    }

});


function tooltipManage() {
    /*$('.tooltip-msg').each(function (index, element) {        
        var offsetL = $(element).offset().left;
        var windowSize = $(window).width();
        var textSize = $(this).find('.tooltip-txt').outerWidth() + 50;
        
        $(this).find('.tooltip-txt').addClass(textSize);

        if (textSize > 279) {
            $(this).find('.tooltip-txt').addClass('text-wrap');
        }

        //$(this).parent().find('.form-msg > span').css('width', textSize);
        if (windowSize < (offsetL + textSize)) {
            $(this).addClass('left');
        }
    });*/

    var toolTipOffsetL, toolTipOffsetT, toolTipWindowSize, toolTipTextSize;
    $('.tooltip-msg').hover(function (e) {
        toolTipOffsetL = $(this).offset().left;
        toolTipOffsetT = $(this).offset().top;
        toolTipWindowSize = $(window).width();
        toolTipTextSize = $(this).find('.tooltip-txt').outerWidth() + 50;

        if (toolTipTextSize > 279) {
            $(this).find('.tooltip-txt').addClass('text-wrap');
        }

        if (toolTipWindowSize < (toolTipOffsetL + toolTipTextSize)) {
            $(this).addClass('left');
            toolTipOffsetL = toolTipOffsetL - toolTipTextSize + 50;
            $(this).find('.tooltip-txt').css({
                'left': toolTipOffsetL,
                'top': toolTipOffsetT
            });
        } else {
            $(this).find('.tooltip-txt').css({
                'left': toolTipOffsetL,
                'top': toolTipOffsetT
            });
        }

    }, function () {
        $(this).find('.tooltip-txt').removeAttr("style");
    });
}


//Added by Nikesh for row selection by arrow keys 03 Apr 2019
function tableRowSelection() {
    if ($('table.singleSelect').length) {
        var selectedId, rowCount, rowElement, containerH, keyEvent, scrollAmt, scrollHeight, isImplemented, autoHeight;
        selectedId = false;

        $("body").delegate("table.singleSelect tbody tr", "click", function () {
            //debugger
            selectedId = $(this).parents('.scrollDiv').attr('id');
            rowCount = $(this).parents("table.singleSelect tbody").find("tr").index($(this));
            isImplemented = $(this).parents().hasClass('scrollDiv');
            autoHeight = $(this).parents().hasClass('heightAuto');
            scrollHeight = 0;
            $('#_prescriberSearchResult table tbody tr').each(function (index, element) {
                scrollHeight += $(this).outerHeight();
                if (index == rowCount)
                    return false;
            });
        });


        //rowCount = -1;

        document.addEventListener('keydown', function (e) {
            //debugger
            if (isImplemented) {

                if ($('.date').is(':focus')) {
                    return false;
                }

                keyEvent = false;

                if (selectedId)
                    containerH = $('#' + selectedId).height();


                if (e.keyCode == 38) // The Up arrow key 
                {
                    if (--rowCount < 0) {
                        rowCount = 0;
                    }

                    if (rowCount == 0) {
                        scrollHeight = $("#" + selectedId + " table tbody tr.selected").outerHeight();
                    } else {
                        scrollHeight -= $("#" + selectedId + " table tbody tr.selected").outerHeight();
                    }
                    //console.log('up arrow =' + scrollHeight);

                    keyEvent = true;
                }
                else if (e.keyCode == 40) {
                    if (++rowCount > $("#" + selectedId + " table tbody tr").length - 1) {
                        rowCount = $("#" + selectedId + " table tbody tr").length - 1;
                    } else {
                        scrollHeight += $("#" + selectedId + " table tbody tr.selected").outerHeight();
                    }
                    //console.log('down arrow =' + scrollHeight);
                    keyEvent = true;
                } else if (e.keyCode == 13) {
                    //console.log('enter key pressed');                   
                }

                if (keyEvent) {

                    $("#" + selectedId + " table tbody tr").removeClass('selected');
                    rowElement = $("#" + selectedId + " table tbody tr").eq(rowCount).addClass('selected');

                    if (autoHeight != true) {
                        e.preventDefault();
                        containerH = containerH - rowElement.height();
                        scrollAmt = (rowCount) * (rowElement.height());
                        //console.clear();
                        //console.log('final Height = ' + scrollHeight);
                        //$('#' + selectedId ).scrollTop(scrollAmt);
                        //$('#' + selectedId).scrollTop(scrollAmt - containerH);
                        $('#' + selectedId).scrollTop(scrollHeight - containerH);
                    }


                    //console.log('Container Height=' + containerH + ',==> scrollAmt' + scrollAmt + 'Row H=' + rowElement.height());
                    //$('#' + selectedId ).animate({scrollTop:scrollAmt - containerH},300);
                    //return false;
                }
            }
        });
    }
}

function rightNavDraggable() {
    if ($('.draggableNav').length) {
        $('.draggableNav').wrapInner("<span><div class='content'></div></span>").draggable({ containment: "#mainWrap" });

        $(".right-nav .content").mCustomScrollbar({
            scrollButtons: {
                enable: true
            },
            callbacks: {
                onInit: function () {
                    $('.right-nav .mCSB_buttonUp').addClass('disabled');
                },
                onScrollStart: function () {
                    $('.right-nav .mCSB_buttonDown, .right-nav .mCSB_buttonUp').removeClass('disabled');
                },
                onTotalScroll: function () {
                    // console.log("onTotalScroll of content.");
                    $('.right-nav .mCSB_buttonDown').addClass('disabled');
                },
                onTotalScrollBack: function () {
                    // console.log("onTotalScrollBack of content.");
                    $('.right-nav .mCSB_buttonUp').addClass('disabled');
                }
            }
        });
    }
}
function tableRowSelectionUtility() {
    if ($('table.singleUtilitySelect').length) {
        var selectedId, rowCount, rowElement, containerH, keyEvent, scrollAmt, scrollHeight, isImplemented, autoHeight;
        selectedId = false;

        $("body").delegate("table.singleUtilitySelect tbody tr", "click", function () {
            selectedId = $(this).parents('.scrollDiv').attr('id');
            rowCount = $(this).parents("table.singleUtilitySelect tbody").find("tr").index($(this));
            isImplemented = $(this).parents().hasClass('scrollDiv');
            autoHeight = $(this).parents().hasClass('heightAuto');
            scrollHeight = 0;
            $("#" + selectedId + " table tbody tr").removeClass('selected');
            $("#" + selectedId + " table tbody tr").eq(rowCount).addClass('selected');

            var createdByName = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-createdByName');
            var createdDate = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-createdDate');
            var updatedByName = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-updatedByName');
            var updatedDate = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-updatedDate');
            updateCreatedDateName(createdByName, createdDate, updatedByName, updatedDate);

        });

        document.addEventListener('keydown', function (e) {
            if (isImplemented) {

                if ($('.date').is(':focus')) {
                    return false;
                }

                keyEvent = false;

                if (selectedId)
                    containerH = $('#' + selectedId).height();


                if (e.keyCode == 38) // The Up arrow key 
                {
                    if (--rowCount < 0) {
                        rowCount = 0;
                    }

                    if (rowCount == 0) {
                        scrollHeight = $("#" + selectedId + " table tbody tr.selected").outerHeight();
                    } else {
                        scrollHeight -= $("#" + selectedId + " table tbody tr.selected").outerHeight();
                    }
                    //console.log('up arrow =' + scrollHeight);

                    keyEvent = true;
                }
                else if (e.keyCode == 40) {
                    if (++rowCount > $("#" + selectedId + " table tbody tr").length - 1) {
                        rowCount = $("#" + selectedId + " table tbody tr").length - 1;
                    } else {
                        scrollHeight += $("#" + selectedId + " table tbody tr.selected").outerHeight();
                    }
                    //console.log('down arrow =' + scrollHeight);
                    keyEvent = true;
                } else if (e.keyCode == 13) {
                    //console.log('enter key pressed');                   
                }

                if (keyEvent) {

                    $("#" + selectedId + " table tbody tr").removeClass('selected');
                    rowElement = $("#" + selectedId + " table tbody tr").eq(rowCount).addClass('selected');

                    var createdByName = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-createdByName');
                    var createdDate = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-createdDate');
                    var updatedByName = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-updatedByName');
                    var updatedDate = $("#" + selectedId + " table tbody tr").eq(rowCount).attr('data-updatedDate');
                    updateCreatedDateName(createdByName, createdDate, updatedByName, updatedDate);

                    if (autoHeight != true) {
                        e.preventDefault();
                        containerH = containerH - rowElement.height();
                        scrollAmt = (rowCount) * (rowElement.height());
                        $('#' + selectedId).scrollTop(scrollHeight - containerH);
                    }
                }
            }
        });
    }
}

$(document).ready(function () {
    tableRowSelection();  
    enterFunctinality();
    tooltipManage();
    rightNavDraggable()

    setTable();

    $('.has-v-scroll > div').bind("scroll", function () {
        setTable();
    });

    $(window).on('show.bs.modal', function (e) {
        setTable()
        setTimeout(function () {
            setTable();
        }, 300);
    }); 

    $('.left-nav ul li a').on('mouseover', function (e) {
        if ($('.nav-h li.flexMenu-viewMore').hasClass('active')) {
            $('.nav-h li.flexMenu-viewMore').removeClass('active').find('.flexMenu-popup').hide();
        }
    });

    $('section.main .row').click(function (e) {
        $('.nav-h li.flexMenu-viewMore').removeClass('active').find('.flexMenu-popup').hide();
    });

});

$(document).ajaxComplete(function () {
    tableRowSelection();
    tableRowSelectionUtility();
    setTable();
    setTooltip();
    $('.has-v-scroll > div').bind("scroll", function () {
        setTable();
    });

    setTimeout(function () {
        tooltipManage();
        $("div.modal.fade").each(function (index) {
            $(this).prop('tabindex', '-1');
        });
    }, 600);
});



$(window).bind('resize orientationchange', function () {
    setMinHeight();
    setTable();
    setThWidth();
    //setSubNavWidth();

    if ($('.address-slider').length) {
        for (var i = 0; i < 40; i++) {
            setTimeout(function () {
                $('.address-slider').slick('setPosition', 0);
            }, i * 10);
        }
    }
});

//Modified By Bharat Singh
$(window).on('load', function () { setTooltip(); });
//$(window).load(function () { 
//    setTooltip();
//});

function alertMessage(message, header, size) {
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
};
function confirmMessage(message, yesCallBackFnct, noCallBackFnct) {

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

};

function confirmMessageWithParam(message, yesCallBackFnct, noCallBackFnct, param1, param2) {

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
        yesCallBackFnct(param1, param2);
    });
    $("#btnNo").click(function () {
        noCallBackFnct(param1, param2);
    });

};
//Added By Manish Pandey for Custom alert on 19 Jan 2019
function alertPopup(msgTitle, msgColor, msgText, oktext, cancelText) {
    var modelTitle = msgTitle;
    var modelTxtColor = msgColor;

    if (modelTxtColor == 'success') {
        var modelBrief = '<div class="text-success">' + msgText + '</div>';
    } else if (modelTxtColor == 'error') {
        var modelBrief = '<div class="text-danger">' + msgText + '</div>';
    } else {
        var modelBrief = msgText;
    }

    var alertString = '<div class="modal fade" id="alertModel" role="dialog"><div class="modal-dialog sm">';
    alertString += '<div class="modal-content">';
    alertString += '<div class="modal-header">';
    alertString += '<h4 class="modal-title">' + msgTitle + '</h4>';
    alertString += '</div>';

    alertString += '<div class="modal-body">';
    alertString += '<div class="pad5">' + modelBrief + '</div>';
    alertString += '</div>';

    alertString += '<div class="modal-footer">';
    alertString += '<div class="pull-right">';
    alertString += '<button type="button" class="btn btn-default btnAlertClick" data-dismiss="modal" data-attr="true">' + oktext + '</button>';

    if (cancelText != undefined && cancelText != null && cancelText != "")
        alertString += '<button type="button" class="btn btn-default btnAlertClick" data-dismiss="modal" data-attr="false">' + cancelText + '</button>';

    alertString += '</div>';
    alertString += '</div>';
    alertString += '</div>';
    alertString += '</div></div>';

    if ($('#alertModel').length) {
        $('#alertModel').remove();
    }

    $('section.main').append(alertString);
    $('#alertModel').modal('show');
}



