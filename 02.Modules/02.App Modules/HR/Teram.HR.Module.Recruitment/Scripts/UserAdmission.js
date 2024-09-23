removeButton = '';
function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/UserAdmission/GetGridData",
            "data": function (d) {
                return $.extend({}, d, {
                    "viewInprogressStatus": $("#ViewInprogressStatus").val(),
                    "personnelCode": $("#PersonnelCodeFilter").val(),
                    "nationalCode": $("#NationalCodeFilter").val(),
                    "firstName": $("#FirstNameFilter").val(),
                    "lastName": $("#LastNameFilter").val(),
                    "processStatus": $("#ProcessStatusFilter").val(),
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
                    return removeButton + editButton;
                }
            }
        ]
    };
}

$(document).on('click', '#ViewAll', function () {
    $("#ViewInprogressStatus").val(false);
    $(".searchStatus").html("در همه پرونده ها");
    reloadDataTable();
});
$(document).on('click', '#ViewInprogress', function () {
    $("#ViewInprogressStatus").val(true);
    $(".searchStatus").html("در پرونده های در گردش");
    reloadDataTable();
});

