function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/ControlPlan/GetControlPlanData",
            "data": function (d) {
                return $.extend({}, d, {
                    "controlPlanCategoryId": $("#FilterControlPlanCategoryId").val()                   
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