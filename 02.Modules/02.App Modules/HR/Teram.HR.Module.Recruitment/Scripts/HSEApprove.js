removeButton = "";
historyButton = '<a  title="تاریخچه تاییدات" href="/HSEApproveHistory/{jobApplicantId}" target="_blank" class="btn gridBtn" style="margin: 1px;"><img style="width:24px;height:24px" src="/icons/History.png"></a>';

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/HSEApprove/GetJobAllicants",
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
                    return removeButton + editButton + historyButton.replace("{jobApplicantId}", row.jobApplicantId);
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

deleteFileUrl = "/JobApplicant/RemoveUploadedFiles/";
function Edit(id) {
    $.post("PartialEdit/" + id, function (content) {
        $(".formInfo").html(content);
    }).done(function () {
        initializeUploaders(id);
        teram().pageInit();
        $(".select2").select2({
            width: '100%',
            cache: true,
            dir: 'rtl',
            language: "fa",
            closeOnSelect: true,
        });
    });
}

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

$(document).ready(function () {

    editFunction = function (id) {
        Edit(id);
    }

});


