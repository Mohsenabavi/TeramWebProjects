$(document).on('click', '#exportexel', function () {
    changeDatatableOptions();
    var data = "?wrongDoerId=" + $("#WrongDoerId").val()
        + "&statInputDate=" + $("#StartDate").val()
        + "&endInputDate=" + $("#EndDate").val()
    window.location.href = "/WronDoersReport/PrintExcel" + data;
});

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/WronDoersReport/GetWrongDoersData",
            "data": function (d) {
                return $.extend({}, d, {         
                    "wrongDoerId": $("#WrongDoerId").val(),
                    "statInputDate": $("#StartDate").val(),
                    "endInputDate": $("#EndDate").val(),
                });
            }
        },
        "columnDefs": [
            {
                "targets": -1,
                "responsivePriority": 2,
                "data": null,
                "width": "150px",
                "responsivePriority": 2,
                "render": function (data, type, row) {
                    return ''
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}