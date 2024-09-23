$(document).ready(function () {
    loadData();
});

async function loadData() {
    let controlPanelDashboardId = $("#ViewRegisteredPeopleByDayChartComponentId").val();
    await $.ajax({
        type: "post",
        url: "/ViewRegisteredPeopleByDayChart/GetChartData",
        data: { controlPanelDashboardId: controlPanelDashboardId },
        success: function (out) {
            if (out.result == "ok") {
                let data = out.chartModel.overalChartModelSeries;
                let categories = out.chartModel.categories;
                let yAxisTitle = out.chartModel.yAxisTitle;
                let chartTitle = out.chartModel.chartTitle;
                $('#GetRegisteredPeopleByDayChartComponent').highcharts({
                    chart: {
                        type: 'column'
                    },
                    title: {
                        align: 'left',
                        text: ''
                    },

                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category'
                    },
                    yAxis: {
                        title: {
                            text: ''
                        }
                    },
                    legend: {
                        enabled: false
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