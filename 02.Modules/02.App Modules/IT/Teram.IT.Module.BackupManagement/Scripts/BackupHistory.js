var editButton = '';
var removeButton = '';

function changeDatatableOptions() {
    return {
        "columnDefs": [
            {
                "targets": -1,
                "responsivePriority": 1,
                "data": null,
                "width": "0px",
                "responsivePriority": 1,
                "render": function (data, type, row) {
                    return editButton + removeButton;
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}