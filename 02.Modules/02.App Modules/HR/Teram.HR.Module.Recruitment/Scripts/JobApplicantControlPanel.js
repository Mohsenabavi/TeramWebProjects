IssuanceLettersButton = '<button title="صدور معرفی نامه ها " type="button" class="btn gridBtn issuanceLetters ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Accept.png"></button>';
StepsButton = '<button  title="خلاصه وضعیت مراحل" type="button" class="btn gridBtn viewSteps ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/History.png"></button>';
AttachemntsButton = '<button  title="پیوست ها" type="button" class="btn gridBtn detailInfo ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Document.png"></button>';
finalApproveButton = '<button title="تایید " type="button" class="btn gridBtn approveInfo ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Accept.png"></button>';
employeeApproveButton = '<button title="تایید " type="button" class="btn gridBtn employeeAproveInfo ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Accept.png"></button>';
DisApproveButton = '<button  title="رد کردن" type="button" class="btn gridBtn disapproveInfo ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Reject.png"></button>';
introductionFiles = '<button title="معرفی نامه ها" type="button" class="btn gridBtn showIntroductions"><img style="width:24px;height:24px" src="/icons/Introductions.png"></button>';
previewButton = '<a title="مشاهده اطلاعات" href="/PreviewBaseInformation/{jobApplicantId}" target="_blank" class="btn gridBtn" style="margin: 1px;"><img style="width:24px;height:24px" src="/icons/ViewInfo.png"></a>';
historyButton = '<a  title="تاریخچه تاییدات" href="/JobApplicantApproveHistory/{jobApplicantId}" target="_blank" class="btn gridBtn" style="margin: 1px;"><img style="width:24px;height:24px" src="/icons/ViewHistory.png"></a>';
smsHistoryButton = '<button title="پیام ها" type="button" class="btn gridBtn showSMS"><img style="width:24px;height:24px" src="/icons/Introductions.png"></button>';
InviteButton = '<button title="دعوت به کار" type="button" class="btn gridBtn showInvitation"><img style="width:24px;height:24px" src="/icons/Accept.png"></button>';

deleteFileUrl = "/JobApplicant/RemoveUploadedFiles/";

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
                "width": "1200px",
                "responsivePriority": 1,
                "render": function (data, type, row) {                    

                    return editButton + removeButton + 
                        AttachemntsButton.replace("{jobApplicantId}", row.jobApplicantId) +
                        (row.flowType == "0" ? IssuanceLettersButton.replace("{jobApplicantId}", row.jobApplicantId) : "") +                                             
                        (row.flowType == "0" ? finalApproveButton.replace("{jobApplicantId}", row.jobApplicantId) : "") +                                                                     
                        (row.flowType == "1" ? employeeApproveButton.replace("{jobApplicantId}", row.jobApplicantId) : "") +                                                                     
                        DisApproveButton.replace("{jobApplicantId}", row.jobApplicantId) +
                        (row.flowType == "0" ? introductionFiles.replace("{jobApplicantId}", row.jobApplicantId) : "") +                         
                        previewButton.replace("{jobApplicantId}", row.jobApplicantId) +
                        (row.flowType == "0" ? historyButton.replace("{jobApplicantId}", row.jobApplicantId) : "") +                           
                        StepsButton.replace("{jobApplicantId}", row.jobApplicantId) +
                        smsHistoryButton.replace("{mobileNumber}", row.mobileNumber) +
                        (row.flowType == "0" ? InviteButton.replace("{jobApplicantId}", row.jobApplicantId) : "")                     
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}

function Edit(id) {
    $.post("PartialEdit/" + id, function (content) {
        $(".formInfo").html(content);
    }).done(function () {
        initializeUploaders(id);
        datePickerInitilize();
        $(".select2").select2({
            width: '100%',
            cache: true,
            dir: 'rtl',
            language: "fa",
            closeOnSelect: true,
            focusInputOnOpen: true
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


$(document).on('click', '#btnInvite', function () {


    let jobApplicantId = $("#CurrrentJobApplicantId").val();
    let invitationDate = $("#InvitationToWorkDate").val();

    $.post("/JobApplicantControlPanel/SaveInvitationDate", { jobApplicantId: jobApplicantId, invitationDate: invitationDate }, function (updateResult) {

        if (updateResult.result == "ok") {
            teram().showSuccessfullMessage(updateResult.message.value);
            bootbox.hideAll();
            reloadDataTable();
        }
        else {
            teram().showErrorMessage(updateResult.message.value);
            bootbox.hideAll();
        }
    });
});

$(document).on('click', '.btnApprove', function () {
    var rowId = $(this).data("id");
    $.post("/JobApplicant/ApproveFile", { jobApplicantFileId: rowId }, function (updateResult) {
        if (updateResult.data == true) {
            teram().showSuccessfullMessage("به روز رسانی وضعیت تایید با موفقیت انجام شد");
            var jobApplicantId = $("#jobApplicantId").val();
            $.post("/JobApplicant/ShowImagesInfo", { id: jobApplicantId }, function (content) {
                $('.bootbox-body').html(content);
            });
        }
        else {
            teram().showErrorMessage(updateResult.message.value);
        }
    });
});

$(document).on('click', '.issuanceLetters', function () {
    var tr = $(this).closest('tr');
    $("#JobApplicantGrid tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/JobApplicantControlPanel/IssuanceIntroductionLetters", { id: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    });
});

$(document).on('click', '.showSMS', function () {
    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();

    $.post("/JobApplicantControlPanel/GetSMSHistory", { mobileNumber: selectedRowData.mobileNumber }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
        reloadDataTable();
    });
});

$(document).on('click', '.showInvitation', function () {
    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();

    $.post("/JobApplicantControlPanel/ShowInvitation", { id: selectedRowData.jobApplicantId }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    }).done(function () {
        datePickerInitilize();
    });
});

$(document).on('click', '#exportexel', function () {
    changeDatatableOptions();
    var data = "?personnelCode=" + $("#PersonnelCodeFilter").val()
        + "&nationalCode=" + $("#NationalCodeFilter").val()
        + "&firstName=" + $("#FirstNameFilter").val()
        + "&lastName=" + $("#LastNameFilter").val()
    window.location.href = "/JobApplicantControlPanel/PrintExcel" + data;
});

$(document).on('click', '.print', function () {

    var tr = $(this).closest('tr');
    $("#JobApplicantGrid tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/Reports/ViewReport", function (content) {
    });
});

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


$(document).on('click', '.viewSteps', function () {

    var tr = $(this).closest('tr');
    $("#JobApplicantGrid tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/JobApplicantControlPanel/ViewStepsStatus", { jobApplicantId: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    });
});

$(document).on('click', '.approveInfo', function () {

    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();

    $.post("/JobApplicantControlPanel/ApproveOperation", { id: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
        reloadDataTable();
    });
});

$(document).on('click', '.employeeAproveInfo', function () {

    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/JobApplicantControlPanel/EmployeeApproveOperation", { id: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
        reloadDataTable();
    });
});

$(document).on('click', '.disapproveInfo', function () {

    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/JobApplicantControlPanel/ShowDisApproveModal", { id: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    });
});

$(document).on('click', '.btnDisApprove', function () {

    var jobApplicantId = $(".JobApplicantId").val();
    var reason = $(".reason").val();

    if (reason == "" || reason == null) {
        teram().showErrorMessage("لطفاً دلیل رد اطلاعات را وارد نمایید");
    }
    else {
        $.post("/JobApplicantControlPanel/DisApproveInfo", { id: jobApplicantId, reason: reason }, function (content) {

            if (content.result == "ok") {

                teram().showSuccessfullMessage(content.message.value);
                reloadDataTable();
            }
            else {

                teram().showErrorMessage(content.message.value);
            }
        });
    }
});

$(document).on('click', '.showIntroductions', function () {

    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/JobApplicantControlPanel/GetIntroductionLetters", { id: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    });
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

$(document).ready(function () {
    editFunction = function (id) {
        Edit(id);

    }
});

function formatNumber() {
    // Get the input value
    var inputValue = $('#PromissoryNoteAmount').val();

    // Remove existing commas
    inputValue = inputValue.replace(/,/g, '');

    // Add commas as thousand separators
    inputValue = inputValue.replace(/\B(?=(\d{3})+(?!\d))/g, ',');

    // Update the input value
    $('#PromissoryNoteAmount').val(inputValue);
}