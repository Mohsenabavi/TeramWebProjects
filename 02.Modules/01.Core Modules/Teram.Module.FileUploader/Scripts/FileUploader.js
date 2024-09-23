$(document).ready(function () {

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
});


