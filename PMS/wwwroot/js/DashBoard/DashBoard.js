$(function () {

    $('#chartContainer').highcharts({

        chart: {
            type: 'gauge',
            plotBackgroundColor: null,
            plotBackgroundImage: null,
            plotBorderWidth: 0,
            plotShadow: false
        },

        title: {
            text: ''
        },

        pane: {
            startAngle: -150,
            endAngle: 150,
            background: [{
                backgroundColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                    stops: [
                        [0, '#FFF'],
                        [1, '#333']
                    ]
                },
                borderWidth: 0,
                outerRadius: '109%'
            }, {
                backgroundColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                    stops: [
                        [0, '#333'],
                        [1, '#FFF']
                    ]
                },
                borderWidth: 1,
                outerRadius: '107%'
            }, {
                // default background
            }, {
                backgroundColor: '#DDD',
                borderWidth: 0,
                outerRadius: '105%',
                innerRadius: '103%'
            }]
        },

        // the value axis
        yAxis: {
            min: 0,
            max: 1000,

            minorTickInterval: 'auto',
            minorTickWidth: 1,
            minorTickLength: 10,
            minorTickPosition: 'inside',
            minorTickColor: '#666',

            tickPixelInterval: 30,
            tickWidth: 2,
            tickPosition: 'inside',
            tickLength: 10,
            tickColor: '#666',
            labels: {
                step: 2,
                rotation: 'auto'
            },
            title: {
                text: 'ms'
            },
            plotBands: [{
                from: 0,
                to: 600,
                color: '#52ACC1'//'#55BF3B' // green
            }, {
                from: 600,
                to: 800,
                color: 'rgba(221,153,18,1)' // yellow
            }, {
                from: 800,
                to: 1000,
                color: '#DF5353' // red
            }]
        },

        series: [{
            name: 'Average Time',
            data: [1],
            tooltip: {
                valueSuffix: ' ms'
            }
        }],
        exporting: {
            enabled: false
        }

    },
        // Add some life
        function (chart) {
            if (!chart.renderer.forExport) {
                setInterval(function () {
                    var point = chart.series[0].points[0],
                        newVal,
                        inc = Math.round((Math.random() - 0.5) * 20);

                    newVal = point.y + inc;
                    $.ajax({
                        url: '/DashBoard/GetExecutionStatics',
                        type: "Post",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) { 
                            for (var i = 0; i < data.length; i++) {
                                newVal = eval(data[i].avgTransactionTime);
                                $("#dvAvgTransactionTime").html(data[i].avgTransactionTime);
                                $("#dvClaimsProcessedToday").html(data[i].claimsReceived.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                                $("#dvClaimsProcessedThisMonth").html(data[i].incorrectClaimsReceived.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                                $("#dvClaimsDiscounted").html(data[i].claimsPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                                $("#dvNoOfAlerts").html(data[i].claimsRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                                point.update(newVal);
                            }
            
                        },
                        error: function (data) {
                        }
                    });

                }, 3000);
            }
        });
});

$(function () {
    $('.date-picker').datepicker({
        dateFormat: "mm/dd/yy",
        constrainInput: true
    }).on("change",
        function (e, obj) {
            if ($(this).val() == '' || $(this).val() == 'mm/dd/yyyy') {
                $('#msgServiceDate').text("");
                return true;
            } else {
                var valueEntered = Date.parse(dashBoard.fn_checkDate($(this).val()));

                if (!/Invalid|NaN/.test(valueEntered)) {
                    $('#msgServiceDate').text("");
                    return true;
                } else {
                    $('#msgServiceDate').text("Please enter correct Start date/End date in mm/dd/yyyy format.");
                    return false;
                }
            }
        });
});

var dashBoard = {
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
        var valueStartDate = Date.parse(dashBoard.fn_checkDate($('#StartDate').val()));
        var valueEndDate = Date.parse(dashBoard.fn_checkDate($('#EndDate').val()));

        if ((!/Invalid|NaN/.test(valueStartDate)) && (!/Invalid/.test(valueEndDate))) {
            $('#msgServiceDate').text("");
            return true;
        }
        else {
            $('#msgServiceDate').text("Please enter correct Start date/End date in mm/dd/yyyy format.")
            return false;
        }
    }

};
