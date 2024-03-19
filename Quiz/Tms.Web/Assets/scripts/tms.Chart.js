
var chartModule = {
    init: function () {
        chartModule.initCheckboPhanUser();
    },
    initCheckboPhanUser: function () {
        // Load google charts
        google.charts.load('current', { 'packages': ['bar'] });
        google.charts.setOnLoadCallback(chartModule.drawBarChart);

        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(chartModule.drawChart3);
    },
    drawBarChart: function () {
        $.ajax({
            url: "/PKHKeHoachSXNKXHXuongLapRap/GetReportXuongLapRap",
            type: "GET",
            dataType: "json",
            global: false,
            success: function (response) {
                //// Create our data table out of JSON data loaded from server.
                chartModule.drawVisualizationBarChart(JSON.parse(response.data), response.title, response.totalColumns);
            }
        });
    },
    drawVisualizationBarChart: function (dataValues, subTitle, totalColumns) {
        var data = new google.visualization.DataTable();
        //data.ad
        data.addColumn('string', 'Tháng');
        data.addColumn('number', 'K40');
        data.addColumn('number', 'K50');
        data.addColumn('number', 'K60');

        for (var i = 0; i < dataValues.length; i++) {
            var k40 = parseInt(dataValues[i].K40);
            var k50 = parseInt(dataValues[i].K50);
            var k60 = parseInt(dataValues[i].K60);
            data.addRow([dataValues[i].Month, k40, k50, k60]);
        }

        var options = {
            chart: {
                title: 'Kế hoạch sản xuất',
                subtitle: subTitle,
            }
        };
        var chart = new google.charts.Bar(document.getElementById('columnchart_material'));
        chart.draw(data, google.charts.Bar.convertOptions(options));
        $(window).resize(function () {
            chart.draw(data, google.charts.Bar.convertOptions(options));
        });
    },

    drawChart3: function () {
        $.ajax({
            url: "/PhieuXuatKho/GetReportXuatKho",
            type: "GET",
            dataType: "json",
            global: false,
            success: function (response) {
                //// Create our data table out of JSON data loaded from server.
                chartModule.drawVisualizationXK(JSON.parse(response.data), response.title, response.totalColumns);
            }
        });
    },
    drawVisualizationXK: function (dataValues, subTitle, totalColumns) {
        var data = new google.visualization.DataTable();
        //data.ad
        data.addColumn('string', 'Tháng');
        data.addColumn('number', 'Nhập kho');
        data.addColumn('number', 'Xuất kho');

        for (var i = 0; i < dataValues.length; i++) {
            var XuatKho = parseInt(dataValues[i].XK);
            var NhapKho = parseInt(dataValues[i].NK);
            data.addRow([dataValues[i].Month, NhapKho, XuatKho]);
        }

        var options = {
            title: 'Xuất - Nhập kho',
            titleTextStyle: {
                color: 'rgb(117, 117, 117)',
                fontName: 'Roboto',
                fontSize: 16,
                bold: false
            },
            legend: { position: 'top', alignment: 'end' },
            hAxis: { title: 'Tháng', titleTextStyle: { color: '#333', italic: false } },
            vAxis: { minValue: 0 }
        };
        var chart = new google.visualization.AreaChart(document.getElementById('piechartXuatKho'));
        chart.draw(data, google.charts.Bar.convertOptions(options));

        $(window).resize(function () {
            chart.draw(data, google.charts.Bar.convertOptions(options));
        });
    }

};

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}