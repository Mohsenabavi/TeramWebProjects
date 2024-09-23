deleteFileUrl = "/JobApplicant/RemoveUploadedFiles/";
AttachemntsButton = '<button  title="پیوست ها" type="button" class="btn gridBtn detailInfo ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Document.png"></button>';
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
                initialFileUploader('.' + uploaderName, result, "/JobApplicant/RemoveUploadedFiles/");
                $(".fileinput-remove-button").addClass("d-none");
                $(".fileinput-remove").addClass("d-none");
            });
        });
    });
}

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
editFunction = function (id) {
    Edit(id);
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


function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/JobApplicantControlPanel/GetJobApplicantsData",
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
                "responsivePriority": 1,
                "data": null,
                "width": "500px",
                "responsivePriority": 1,
                "render": function (data, type, row) {
                    return editButton +
                        AttachemntsButton.replace("{jobApplicantId}", row.jobApplicantId);
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}

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
            className: "bootbox-largest"
        });
    });
});