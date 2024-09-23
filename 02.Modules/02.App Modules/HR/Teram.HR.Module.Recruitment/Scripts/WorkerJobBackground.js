$(document).on("change", "#JobApplicantId", function () {

    if ($(this).val() > 0) {
        $.post("/JobApplicant/GetJobApplicantById",
            { jobApplicantId: $(this).val() },
            function (content) {
                if (content.result == "ok") {
                    $("#JobTitle").val(content.data.jobPositionTitle);
                }
                else {
                    $("#JobTitle").val("");
                }
            });
    }
    else {
        $("#JobTitle").val("");
    }
});

$(document).ready(function () {

    var jobApplicantId = $("#JobApplicantHidden").val();
    if (jobApplicantId > 0) {
        $('#JobApplicantId').val(jobApplicantId).trigger('change');
        $("#JobApplicantId").prop('readonly', true);
    }
});

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

function changeDatatableOptions() {
    return {
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

$(document).ready(function () {
    editFunction = function (id) {
        Edit(id);
    }
});

deleteFileUrl = "/JobApplicant/RemoveUploadedFiles/";
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
    $.post("/WorkerJobBackground/GetWorkerJobBackgroundAttachmentTypes/" + id, function (content) {
        content.data.forEach(x => {
            $.post("/JobApplicant/ShowUploadedFile/", { jobApplicantId: content.jobApplicantId, attachmentTypeId: x }, function (result) {
                var uploaderName = result.uploaderName;
                initialFileUploader('.' + uploaderName, result, "/JobApplicant/RemoveUploadedFiles/");
                $(".fileinput-remove-button").addClass("d-none");
                $(".fileinput-remove").addClass("d-none");
            });
        });
    });
}