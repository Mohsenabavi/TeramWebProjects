removeButton = '';

var markers = [];
var map;
deleteFileUrl = "/JobApplicant/RemoveUploadedFiles/";
function Edit(id) {
    $.post("PartialEdit/" + id, function (content) {
        $(".formInfo").html(content);
    }).done(function () {
        initializeUploaders(id);
        $(".select2").select2({
            width: '100%',
            cache: true,
            dir: 'rtl',
            language: "fa",
            closeOnSelect: true,
        });
    });
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

function initializeUploaders(id) {
    var fileuploaders = $('.fileuploader').filter(function () {
        return this.className.match(/\bfileuploader_.+\b/);
    });
    fileuploaders.each(function () {
        var uploaderClass = $(this).attr('class').split(' ').find(function (className) {
            return className.startsWith('fileuploader_');
        });
        initialFileUploader('.' + uploaderClass, null);
        $(".fileinput-remove").addClass("d-none");
    });
    $.post("/JobApplicant/GetJobApplicantAttachmentTypes/" + id, function (content) {
        content.data.forEach(x => {
            $.post("/JobApplicant/ShowUploadedFile/", { jobApplicantId: id, attachmentTypeId: x }, function (result) {
                var uploaderName = result.uploaderName;
                var attachmentType = "div_" + uploaderName.split('_')[1];
                var statusSelector = "." + attachmentType + " .file-input .attachmentLabel .approveStatusIcon"
                var headerSelector = "." + attachmentType + " .file-input .attachmentLabel";
                initialFileUploader('.' + uploaderName, result, "/JobApplicant/RemoveUploadedFiles/");
                if (result.initialPreviewConfig[0].isApproved) {
                    $(statusSelector).html("تایید شده");
                    $(headerSelector).addClass("bg-success");
                }
                else {
                    $(statusSelector).html("تایید نشده");
                    $(headerSelector).addClass("bg-danger");
                }
                $(".fileinput-remove-button").addClass("d-none");
                $(".fileinput-remove").addClass("d-none");
            });
        });
    });
}
function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/JobApplicant/GetGridData",
            "data": function (d) {
                return $.extend({}, d, {
                    "viewInprogressStatus": $("#ViewInprogressStatus").val(),
                    "personnelCode": $("#PersonnelCodeFilter").val(),
                    "nationalCode": $("#NationalCodeFilter").val(),
                    "firstName": $("#FirstNameFilter").val(),
                    "lastName": $("#LastNameFilter").val(),
                    "flowType": $("#FlowTypeFilter").val(),
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
                    return editButton + removeButton;
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}

$(document).ready(function () {

    var jobApplicantId = $("#JobApplicantId").val();

    if (jobApplicantId > 0)
        Edit(jobApplicantId);

    $(document).on('click', '.detailInfo', function () {

        var tr = $(this).closest('tr');
        $("#JobApplicantGrid tr").each(function () {
            $(this).removeClass("selected");
        });

        tr.addClass("selected");
        var row = table.row(tr);
        var selectedRowData = row.data();
        $.post("/JobApplicant/ShowImagesInfo", { id: selectedRowData.key }, function (content) {
            bootbox.setDefaults({ size: 'large' });
            bootbox.dialog({
                message: content,
                className: "bootbox-large"
            });
        });
    });
    editFunction = function (id) {
        Edit(id);
    }
});



