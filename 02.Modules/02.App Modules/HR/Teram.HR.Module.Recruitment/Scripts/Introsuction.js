$(document).ready(function () {

    var jobApplicantId = $("#JobApplicantId").val();

    if (jobApplicantId > 0)
        Edit(jobApplicantId);
});

function Edit(id) {
    initializeUploaders(id);
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
    $.post("/JobApplicant/GetJobApplicantIntroductionAttachmentTypes/" + id, function (content) {
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