$(document).ready(function () {
    loadProcessStatusChart();
});

async function loadProcessStatusChart() {
    let controlPanelDashboardId = $("#ViewRegistredPeopleByProcessStatusChartComponentId").val();
    await $.ajax({
        type: "post",
        url: "/ViewRegistredPeopleByProcessStatusChart/GetChartData",
        data: { controlPanelDashboardId: controlPanelDashboardId },
        success: function (out) {
            if (out.result == "ok") {
                let data = out.chartModel.overalChartModelSeries;

                $('#GetViewRegistredPeopleByProcessStatusChartComponent').highcharts({
                    chart: {
                        type: 'pie'
                    },
                    title: {
                        text: 'افراد به تفکیک وضعیت'
                    },
                    allowPointSelect: true,
                    cursor: 'pointer',
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}%</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) ||
                                'black'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                            '<td class="pointFormat"  style="padding:0;"><b>{point.y}</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [
                        {
                            name: "",
                            colorByPoint: true,
                            data: data
                        }
                    ]

                });
            }
        },
        error: function (error) {
            var err = error;
        }
    });

    return this;
};