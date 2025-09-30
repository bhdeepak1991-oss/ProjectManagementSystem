var errorMSG = 'We are unable to process your request.\nWe are sorry for the inconvenience caused to you.';
var pleaseWaitHTML = '<span style="color:Red">Please wait while loading....</span>';
var yMax = 100;

$(document).ready(function () {
    monitorClass.LoadFirstNodeCPUUsage();
    monitorClass.LoadSecondNodeBizCPUUsage();
    monitorClass.LoadSecondNodeBizMemoryUsage();
    monitorClass.LoadFirstNodeMemeoryUsage();

});

var monitorClass = {
    "LoadFirstNodeCPUUsage": function () {
        $.get("/Monitor/LoadBizTalkFirstNodeUsage", function (data) {
            if (data.length > 0) {
                var dataArray = new Array();
                var categories = new Array();
                var usage = new Array();
                var str, year, month, day, hour, minutes, seconds, milliseconds, d;
                data.forEach(item => {
                    //str = item.timeStamp.replace(/\D/g, "");
                    //d = new Date(parseInt(str));
                    d = new Date(item.timeStamp);
                    year = parseInt(d.getFullYear());
                    month = parseInt(d.getMonth());
                    day = parseInt(d.getDate());
                    hour = parseInt(d.getHours());
                    minutes = parseInt(d.getMinutes());
                    seconds = parseInt(d.getSeconds());
                    milliseconds = parseInt(d.getMilliseconds());
                    categories.push(Date.UTC(year, month, day, hour, minutes, seconds, milliseconds));
                    usage.push(item.processorUses);
                    dataArray.push([Date.UTC(year, month, day, hour, minutes, seconds, milliseconds), item.processorUses]);

                });
                yMax = 100;
                monitorClass.LoadChart(usage, categories, dataArray, '#FirstNodeCPUUsage', 'CPU Usage', '', 'CPU usage (%)', 'CPU usage (%)');
            }
        });
    },

    "LoadSecondNodeBizCPUUsage": function () {
        $("#SecondNodeCPUUsage").html(pleaseWaitHTML);
        $.get("/Monitor/LoadBizTalkSecondNodeCPUUsage",
            function (data) {
                $("#SecondNodeCPUUsage").html("");
                if (data.length > 0) {
                    var dataArray = new Array();
                    var categories = new Array();
                    var usage = new Array();
                    var str, year, month, day, hour, minutes, seconds, milliseconds, d;
                    data.forEach(item => {
                        //str = item.timeStamp.replace(/\D/g, "");
                        //d = new Date(parseInt(str));
                        d = new Date(item.timeStamp);
                        year = parseInt(d.getFullYear());
                        month = parseInt(d.getMonth());
                        day = parseInt(d.getDate());
                        hour = parseInt(d.getHours());
                        minutes = parseInt(d.getMinutes());
                        seconds = parseInt(d.getSeconds());
                        milliseconds = parseInt(d.getMilliseconds());

                        categories.push(Date.UTC(year, month, day, hour, minutes, seconds, milliseconds));
                        usage.push(item.processorUses);
                        dataArray.push([
                            Date.UTC(year, month, day, hour, minutes, seconds, milliseconds), item.processorUses
                        ]);
                    });
                    yMax = 100;
                    monitorClass.LoadChart(usage,
                        categories,
                        dataArray,
                        "#SecondNodeCPUUsage",
                        "CPU Usage",
                        "",
                        "CPU usage (%)",
                        "CPU usage (%)");
                }
            });
    },

    "LoadSecondNodeBizMemoryUsage": function () {
        $("#SecondNodeMemoryUsage").html(pleaseWaitHTML);
        $.get("/Monitor/LoadBizTalkSecondNodeMemoryUsage", function (data) {
            $("#SecondNodeMemoryUsage").html("");
            if (data.length > 0) {
                var dataArray = new Array();
                var categories = new Array();
                var usage = new Array();
                var str, year, month, day, hour, minutes, seconds, milliseconds, d;
                var TotalMemory = data[0].totalMemory;
                data.forEach(item => {
                    //str = item.timeStamp.replace(/\D/g, "");
                    //d = new Date(parseInt(str));
                    d = new Date(item.timeStamp);
                    year = parseInt(d.getFullYear());
                    month = parseInt(d.getMonth());
                    day = parseInt(d.getFullYear());
                    hour = parseInt(d.getHours());
                    minutes = parseInt(d.getMinutes());
                    seconds = parseInt(d.getSeconds());
                    milliseconds = parseInt(d.getMilliseconds());

                    categories.push(Date.UTC(year, month, day, hour, minutes, seconds, milliseconds));
                    usage.push(item.freeMemory);
                    dataArray.push([Date.UTC(year, month, day, hour, minutes, seconds, milliseconds), item.freeMemory]);

                    //if (i === 0) {
                    //    TotalMemory = item.totalMemory;
                    //}
                });
                yMax = 16000;
                monitorClass.LoadChart(usage, categories, dataArray, "#SecondNodeMemoryUsage", "Memory Usage", "Total Physical Memory (MB): " + TotalMemory, "Memory usage (MB)", "Memory usage (MB)");

            }
        });
    },

    "LoadFirstNodeMemeoryUsage": function () {
        $('#FirstNodeMemoryUsage').html(pleaseWaitHTML);
        $.get("/Monitor/GetMemeoryUsageDetails", function (data) {
            if (data.length > 0) {
                $('#FirstNodeMemoryUsage').html("");
                var dataArray = new Array();
                var categories = new Array();
                var usage = new Array();
                var str, year, month, day, hour, minutes, seconds, milliseconds, d;
                var TotalMemory = data[0].totalMemory;
                data.forEach(item => {
                    //str = item.timeStamp.replace(/\D/g, "");
                    //d = new Date(parseInt(str));
                    d = new Date(item.timeStamp);
                    year = parseInt(d.getFullYear());
                    month = parseInt(d.getMonth());
                    day = parseInt(d.getDay());
                    hour = parseInt(d.getHours());
                    minutes = parseInt(d.getMinutes());
                    seconds = parseInt(d.getSeconds());
                    milliseconds = parseInt(d.getMilliseconds());

                    categories.push(Date.UTC(year, month, day, hour, minutes, seconds, milliseconds));
                    usage.push(item.freeMemory);
                    dataArray.push([Date.UTC(year, month, day, hour, minutes, seconds, milliseconds), item.freeMemory]);

                    //if (i === 0) {
                    //    TotalMemory = item.totalMemory;
                    //}
                });
                yMax = 16000;
                monitorClass.LoadChart(usage, categories, dataArray, "#FirstNodeMemoryUsage", "Memory Usage", "Total Physical Memory (MB): " + TotalMemory, "Memory usage (MB)", "Memory usage (MB)");


            }
        });
    },

    "LoadChart": function (CPUUsage, ChartCategories, ChartData, containerID, ChartTitle, ChartSubTitle, AreaTitle, yAxisTitle) {
        $(containerID).highcharts({
            chart: {
                zoomType: 'x'
            },
            title: {
                text: ChartTitle
            },
            subtitle: {
                text: ChartSubTitle
            },
            xAxis: {
                type: 'datetime'
            },
            yAxis: {
                min: 0,
                max: yMax,
                title: {
                    text: yAxisTitle
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                area: {
                    fillColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, Highcharts.getOptions().colors[0]],
                            [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                        ]
                    },
                    marker: {
                        radius: 2
                    },
                    lineWidth: 1,
                    states: {
                        hover: {
                            lineWidth: 1
                        }
                    },
                    threshold: null
                }
            },

            series: [{
                type: 'area',
                name: AreaTitle,
                pointInterval: 5 * 60 * 1000,
                pointStart: ChartCategories[0],
                data: ChartData
            }]
        });
    }, 
};