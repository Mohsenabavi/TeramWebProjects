

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/PalletsStatus/GetPalletsStatus",
            "data": function (d) {
                return $.extend({}, d, {
                    "orderNo": $("#OrderNoFilter").val(),
                    "number": $("#NumberFilter").val(),
                    "productCode": $("#ProductCodeFilter").val(),
                    "tracingCode": $("#TracingCodeFilter").val(),
                    "orderTitle": $("#OrderTitleFilter").val(),
                    "productName": $("#ProductNameFilter").val()
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
                    return editButton + removeButton
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}