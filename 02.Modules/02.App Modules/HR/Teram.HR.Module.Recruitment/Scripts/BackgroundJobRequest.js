editButton = '';
removeButton = '';
customButton = '<button title="پرینت فرم خام" type="button" class="btn printForm"><img style="width:24px;height:24px" src="/icons/Introductions.png"></button>';
var insertDataButton = '<button title="تکمیل فرم"  type="button" class="insertData btn btn-xs btn-flat defaultButton editRecord gridBtn"><img style="width:24px;height:24px" src="/icons/edit.png"></i ></a >';
previewButton = '<a title="مشاهده اطلاعات" href="/PreviewBaseInformation/{jobApplicantId}" target="_blank" class="btn gridBtn" style="margin: 1px;"><img style="width:24px;height:24px" src="/icons/ViewInfo.png"></a>';
AttachemntsButton = '<button  title="پیوست ها" type="button" class="btn gridBtn detailInfo ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/Document.png"></button>';
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
                    "flowType": 0,
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
                    return editButton + removeButton + customButton.replace("{id}", row.jobApplicantId)
                        + previewButton.replace("{jobApplicantId}", row.jobApplicantId)
                        + insertDataButton.replace("{jobApplicantId}", row.jobApplicantId)
                        + AttachemntsButton.replace("{jobApplicantId}", row.jobApplicantId);
                }
            }
        ],
        'order': [[1, 'asc']]
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


$(document).on('click', '.printForm', function () {

    var tr = $(this).closest('tr');
    $("#BackgroundJobRequest tr").each(function () {
        $(this).removeClass("selected");
    });
    var row = table.row(tr);
    var selectedRowData = row.data();
    var jobApplicantId = selectedRowData.jobApplicantId;

    if (selectedRowData.jobCategory == 1) {

        var link = "/ViewWorkerJobBackgroundInfo/WorkerPrint/" + jobApplicantId;
        window.open(link, '_blank');
    }
    else if (selectedRowData.jobCategory == 2) {
        var link = "/ViewWorkerJobBackgroundInfo/EmployeePrint/" + jobApplicantId;
        window.open(link, '_blank');
    }
});

$(document).on('click', '.insertData', function () {

    var tr = $(this).closest('tr');
    $("#BackgroundJobRequest tr").each(function () {
        $(this).removeClass("selected");
    });
    var row = table.row(tr);
    var selectedRowData = row.data();
    var jobApplicantId = selectedRowData.jobApplicantId;

    if (selectedRowData.baseInformationApproveStatus == 1 || selectedRowData.baseInformationApproveStatus == 2) {

        teram().showErrorMessage("اطلاعات این پرونده توسط منایع انسانی قبلا تایید شده است");
        return;
    }
    if (selectedRowData.jobCategory == 1) {
        var link = "/WorkerJobBackground/" + jobApplicantId;
        window.open(link, '_blank');
        return;
    }
    else if (selectedRowData.jobCategory == 2) {
        var link = "/EmployeeJobBackground/" + jobApplicantId;
        window.open(link, '_blank');
        return;
    }
});
$(document).on('click', '.detailInfo', function () {

    var tr = $(this).closest('tr');
    $("#BackgroundJobRequest tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();

    if (selectedRowData.baseInformationApproveStatus == 1 || selectedRowData.baseInformationApproveStatus == 2) {

        teram().showErrorMessage("اطلاعات این پرونده توسط منایع انسانی قبلا تایید شده است");
        return;
    }
    $.post("/JobApplicant/ShowResumeInfo", { id: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    });
});