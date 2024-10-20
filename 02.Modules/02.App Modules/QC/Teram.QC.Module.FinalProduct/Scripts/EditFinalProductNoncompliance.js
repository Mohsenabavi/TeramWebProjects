﻿
showDetailsBtn = '<button  title="مشاهده جزییات" type="button" class="btn gridBtn showDetailsBtn ml-0 mr-0"><img style="width:24px;height:24px" src="/icons/History.png"></button>';
var nonComplianceId;
var dateOption = {
    "altFormat": 'YYYY/MM/DD',
    "calendarType": "persian",
    "initialValue": false,
    "minDate": null,
    "maxDate": null
}

function onSuccessAfterEditControlPlanDefectId(data) {
    onSuccess(data);
    bootbox.hideAll();
    reloadDataTable();
}

function onSuccessAfterCausation(data) {
    onSuccess(data);
    bootbox.hideAll();
    reloadDataTable();
}

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/EditFinalProductNoncompliance/GetNonComplianceData",
            "data": function (d) {
                return $.extend({}, d, {
                    "orderNo": $("#OrderNo").val(),
                    "productCode": $("#ProductCode").val(),
                    "finalProductNoncomplianceNumber": $("#FinalProductNoncomplianceNumber").val(),
                    "formStatus": $("#FormStatus").val(),
                    "referralStatus": $("#ReferralStatus").val(),
                    "ViewAll": $("#ViewAll").is(":checked")
                });
            }
        },
        "columnDefs": [
            {
                "targets": -1,
                "responsivePriority": 2,
                "data": null,
                "width": "150px",
                "width": "150px",
                "responsivePriority": 2,
                "render": function (data, type, row) {
                    return showDetailsBtn;
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}

$(document).on('click', '#exportexel', function () {
    changeDatatableOptions();
    var data = "?orderNo=" + $("#OrderNo").val()
        + "&productCode=" + $("#ProductCode").val()
        + "&finalProductNoncomplianceNumber=" + $("#FinalProductNoncomplianceNumber").val()
        + "&formStatus=" + $("#FormStatus").val()
        + "&referralStatus=" + $("#ReferralStatus").val()
        + "&viewAll=" + $("#ViewAll").is(":checked")
    window.location.href = "/EditFinalProductNoncompliance/PrintExcel" + data;
});


$(document).on('change', '#IsCaseError', function () {

    var isChecked = $("#IsCaseError").is(":checked");
    if (isChecked) {
        $("#IsLackOfFit").prop('checked', false);
        $("#IsLackOfFit").trigger("change");
        $(".lackOfFitSection :checkbox").prop('checked', false);
    }
});

$(document).on('change', '#IsLackOfFit', function () {

    var isChecked = $("#IsLackOfFit").is(":checked");
    if (isChecked) {
        $("#IsCaseError").prop('checked', false);
        $("#IsCaseError").trigger("change");
    }
    else {
        $("#HasLackOfFitWorkerAndJob").prop('checked', false);
        $("#HasLackOfEducation").prop('checked', false);
        $("#HasFailureOfDefineJob").prop('checked', false);
    }
});


function validateForm() {

    var isValidate = true;
    var hrCauseIsChecked = $("#HasHRCause").is(":checked");
    var methodCauseIsChecked = $("#HasMethodCause").is(":checked");
    var rawMaterialCauseIsChecked = $("#HasRawMaterialCause").is(":checked");
    var essentialCauseIsChecked = $("#HasEssentialCause").is(":checked");
    var equipmentCauseIsChecked = $("#HasEquipmentCause").is(":checked");

    if (!hrCauseIsChecked && !methodCauseIsChecked && !rawMaterialCauseIsChecked && !essentialCauseIsChecked && !equipmentCauseIsChecked) {

        isValidate = false;
        teram().showErrorMessage("هیچ یک از موارد نیروی انسانی ، روش کار ، مواد اولیه ، ملزومات و دستگاهها و تجهیزات انتخاب نشده است ");
    }

    if (hrCauseIsChecked) {

        var wrongDoerId = $("#WrongdoerId").val();
        var IsCaseError = $("#IsCaseError").is(":checked");
        var IsLackOfFit = $("#IsLackOfFit").is(":checked");

        if (wrongDoerId == "") {
            teram().showErrorMessage("فرد خاطی را مشخص نمایید");
            isValidate = false;
        }
        if (!IsCaseError && !IsLackOfFit) {
            teram().showErrorMessage("یکی از موارد خطای موردی یا عدم تناسب شعل با شاغل را انتخاب نمایید");
            isValidate = false;
        }

        if (IsLackOfFit) {

            var hasLackOfFitWorkerAndJobValue = $("#HasLackOfFitWorkerAndJob").is(":checked");
            var hasLackOfEducationValue = $("#HasLackOfEducation").is(":checked");
            var hasFailureOfDefineJobValue = $("#HasFailureOfDefineJob").is(":checked");

            if (!hasLackOfFitWorkerAndJobValue && !hasLackOfEducationValue && !hasFailureOfDefineJobValue) {
                isValidate = false;
                teram().showErrorMessage("یکی از موارد عدم تناسب شغل با شاغل ، نقصان آموزش و یا عدم تعریف صحیح شغل را وارد نمایید");
            }
        }
    }

    if (methodCauseIsChecked) {

        var rootCauseId = $("#RootCauseId").val();
        if (rootCauseId == "") {
            teram().showErrorMessage("ریشه عدم انطباق را مشخص نمایید");
            isValidate = false;
        }
        if (rootCauseId == "2") {

            var InstructionDescription = $("#InstructionDescription").val();
            if (InstructionDescription == "" || InstructionDescription == null) {
                isValidate = false;
                teram().showErrorMessage("دستور العمل مربوطه را وارد نمایید");
            }
            var unitId = $("#UnitId").val();
            if (unitId == "") {
                isValidate = false;
                teram().showErrorMessage("واحد خاطی را مشخص نمایید");
            }
        }
    }

    if (essentialCauseIsChecked) {

        var canBeIdentifiedAtEntrancevalue = $("#CanBeIdentifiedAtEntrance").is(":checked")
        if (canBeIdentifiedAtEntrancevalue) {
            var hasEntitlementLicense = $("#HasEntitlementLicense").is(":checked")
            if (!hasEntitlementLicense) {

                var operatorId = $("#OperatorId").val();

                if (operatorId == "") {
                    isValidate = false;
                    teram().showErrorMessage("فرد خاطی را وارد نمایید");
                }
            }
        }
    }

    if (equipmentCauseIsChecked) {

        var hasNotification = $("#HasNotification").is(":checked");

        if (hasNotification) {

            if ($("#NotificationNumber").val() == "" || $("#NotificationNumber").val() == null) {

                isValidate = false;
                teram().showErrorMessage("شماره اعلان را وارد نمایید");
            }
        }
    }
    return isValidate;
}

$(document).on('click', '#btnSubmitCausation', function (event) {

    var valdidateResult = validateForm();
    if (valdidateResult) {
        $("#frmCausation").submit();
    }

});

$(document).on('change', '#HasHRCause', function () {

    var isChecked = $("#HasHRCause").is(":checked");
    if (isChecked) {
        $(".hrSection").removeClass("d-none");
    }
    else {
        $(".hrSection").addClass("d-none");
        $(".hrFiledSet :checkbox").prop('checked', false);
    }
});
$(document).on('change', '#HasMethodCause', function () {

    var isChecked = $("#HasMethodCause").is(":checked");
    if (isChecked) {
        $(".methodSection").removeClass("d-none");
    }
    else {
        $(".methodSection").addClass("d-none");
        $(".mthodFieldSet .select2").val("").trigger("change")
    }
});
$(document).on('change', '#HasRawMaterialCause', function () {

    var isChecked = $("#HasRawMaterialCause").is(":checked");
    if (isChecked) {
        $(".rawMaterialSection").removeClass("d-none");
    }
    else {
        $(".rawMaterialSection").addClass("d-none");
    }
});

$(document).on('change', '#HasEssentialCause', function () {

    var isChecked = $("#HasEssentialCause").is(":checked");
    if (isChecked) {
        $(".essentialSection").removeClass("d-none");
    }
    else {
        $(".essentialSection").addClass("d-none");
        $(".essentialFieldSet .select2").val("").trigger("change")
        $(".essentialFieldSet :checkbox").prop('checked', false);
        $(".HasEntitlementLicenseSection").addClass("d-none");
        $(".OperatorSection").addClass("d-none");
    }
});

$(document).on('change', '#HasEquipmentCause', function () {

    var isChecked = $("#HasEquipmentCause").is(":checked");
    if (isChecked) {
        $(".equipmentSection").removeClass("d-none");
    }
    else {
        $(".equipmentSection").addClass("d-none");
        $(".NotificationNumberSection").addClass("d-none");
        $("#NotificationNumber").val("");
        $(".equipmentFieldSet :checkbox").prop('checked', false);
    }
});

$(document).on('change', '#CanBeIdentifiedAtEntrance', function () {

    var isChecked = $("#CanBeIdentifiedAtEntrance").is(":checked");
    if (isChecked) {
        $(".HasEntitlementLicenseSection").removeClass("d-none");
        $(".OperatorSection").removeClass("d-none");
    }
    else {
        $(".HasEntitlementLicenseSection").addClass("d-none");
        $(".OperatorSection").addClass("d-none");
        $("#HasEntitlementLicense").prop("checked", false);
        $("#OperatorId").val("").trigger("change");
    }
});

$(document).on('change', '#HasEntitlementLicense', function () {

    var isChecked = $("#HasEntitlementLicense").is(":checked");
    if (isChecked) {
        $(".OperatorSection").addClass("d-none");
        $("#OperatorId").val("").trigger("change");
    }
    else {
        $(".OperatorSection").removeClass("d-none");
    }
});

$(document).on('change', '#HasNotification', function () {

    var isChecked = $("#HasNotification").is(":checked");
    if (isChecked) {
        $(".NotificationNumberSection").removeClass("d-none");
    }
    else {
        $(".NotificationNumberSection").addClass("d-none");
        $("#NotificationNumber").val("");
    }
});

$(document).on('change', '#RootCauseId', function () {

    if ($(this).val() == "2") {

        $(".instructionSection").removeClass("d-none");
        $(".unitSection").removeClass("d-none");
    }
    else {
        $(".instructionSection").addClass("d-none");
        $(".unitSection").addClass("d-none");
        /*$("#InstructionId").val("").trigger("change");*/
        $("#UnitId").val("").trigger("change");
    }
});


$(document).on('change', '#hasNeedToRefferToOthers', function () {

    var selectedValue = $("#hasNeedToRefferToOthers").val();
    if (selectedValue == "1") {
        getOthersListForAdvisoryOpinion();
        $(".refferFromProductionManagerSection").removeClass("d-none");
        $(".causationSection").addClass("d-none");
    }
    else {
        $(".causationSection").removeClass("d-none");
        $(".refferFromProductionManagerSection").addClass("d-none");
        showHideCreateRowButton();
    }
});

$(document).on('change', '#IsLackOfFit', function () {

    var isChecked = $("#IsLackOfFit").is(":checked");
    if (isChecked) {
        $(".lackOfFitSection").removeClass("d-none");
    }
    else {
        $(".lackOfFitSection").addClass("d-none");
    }
});
function getNeedToAdvisoryOpinionRefferralList() {

    $.post("/EditFinalProductNoncompliance/GetNeedToAdvisoryOpinionRefferralList",
        function (content) {
            var selectBox = $('#NeedToAdvisoryOpinionRefferralList');
            selectBox.empty();
            selectBox.append($('<option>').text("-  انتخاب کنید -").val(""));
            $.each(content, function (index, item) {
                selectBox.append($('<option>').text(item.text).val(item.value));
            });
        }).done(function () {


        });
}

function getOthersListForAdvisoryOpinion() {

    $.post("/EditFinalProductNoncompliance/GetOthersListForAdvisoryOpinion",
        function (content) {
            var selectBox = $('#OthersList');
            selectBox.empty();
            selectBox.append($('<option>').text("-  انتخاب کنید -").val(""));
            $.each(content, function (index, item) {
                selectBox.append($('<option>').text(item.text).val(item.value));
            });
        }).done(function () {

        });
}


$(document).on("change", "#ApproveStatus", function () {


    var selectedValue = $(this).val();
    var hideArea = $(".hideArea");
    var needToAdvisoryOpinionSection = $(".NeedToAdvisoryOpinionSection");
    switch (selectedValue) {
        case "1":
            needToAdvisoryOpinionSection.removeClass('d-none');
            hideArea.addClass('d-none');
            break;

        case "2":
            hideArea.removeClass('d-none');
            needToAdvisoryOpinionSection.addClass('d-none');
            $(".dynamicContent").addClass("d-none");
            break;

        case "3":
            hideArea.removeClass('d-none');
            needToAdvisoryOpinionSection.addClass('d-none');
            $(".dynamicContent").addClass("d-none");
            break;
    }
});

$(document).on("change", "#NeedToAdvisoryOpinion", function () {

    var selectedValue = $(this).val();
    var hideArea = $(".needToAdvisoryOpinionRefferralListSection");

    if (selectedValue == "1") {
        hideArea.removeClass('d-none');
        getNeedToAdvisoryOpinionRefferralList();
        $(".QcManagerComments").removeClass("d-none");
        $(".dynamicContent").addClass("d-none");
    }
    else {
        hideArea.addClass('d-none');
        $(".QcManagerComments").addClass("d-none");
    }
});

$(document).on("change", "#NeedToAdvisoryOpinion", function () {

    var needToAdvisoryOpinionValue = $(this).val();
    if (needToAdvisoryOpinionValue == "0") {
        $(".dynamicContent").removeClass("d-none");
    }
});


$(document).on('click', '#btnRefferalFromSeparation', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var firstSampleSeparatedCount = $("#FirstSampleSeparatedCount").val() ?? 0;
    var secondSampleSeparatedCount = $("#SecondSampleSeparatedCount").val() ?? 0;
    var thirdSampleSeparatedCount = $("#ThirdSampleSeparatedCount").val() ?? 0;
    var forthSampleSeparatedCount = $("#ForthSampleSeparatedCount").val() ?? 0;

    $.post("/FinalProductNoncompliance/TriggerSaveSeparationInfo",
        {
            finalProductNonComplianceId: nonComplianeId,
            firstSampleSeparatedCount: firstSampleSeparatedCount,
            secondSampleSeparatedCount: secondSampleSeparatedCount,
            thirdSampleSeparatedCount: thirdSampleSeparatedCount,
            forthSampleSeparatedCount: forthSampleSeparatedCount
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});


$(document).on('change', '#NeedToReviseByCEO', function () {

    var isChecked = $("#NeedToReviseByCEO").is(":checked");
    if (isChecked) {
        $(".commentSection").removeClass("d-none");
    }
    else {
        $(".commentSection").addClass("d-none");
        $("#Comments").val("");
    }
});


$(document).on('click', '#btnRequestForReviewByCEO', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var comments = $("#Comments").val() ?? 0;
    var needToReviseByCEO = $("#NeedToReviseByCEO").is(":checked");

    if (needToReviseByCEO) {
        $.post("/FinalProductNoncompliance/TriggerRequestForReviewByCEO",
            {
                finalProductNonComplianceId: nonComplianeId,
                comment: comments,
            }, function (content) {
                if (content.result == "ok") {
                    teram().showSuccessfullMessage(content.message.value);
                    bootbox.hideAll();
                    reloadDataTable();
                }
                else {
                    teram().showErrorMessage(content.message.value);
                }
            });
    }
    else {

        $.post("/FinalProductNoncompliance/FinalApproveByQCManager",
            {
                finalProductNonComplianceId: nonComplianeId,
                comment: comments,
            }, function (content) {
                if (content.result == "ok") {
                    teram().showSuccessfullMessage(content.message.value);
                    bootbox.hideAll();
                    reloadDataTable();
                }
                else {
                    teram().showErrorMessage(content.message.value);
                }
            });
    }
});

$(document).on('click', '#btnSaveWasteDocumentNumber', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();

    var wasteDocumentNumber = $("#WasteDocumentNumber").val() ?? 0;

    $.post("/FinalProductNoncompliance/TriggerSaveWasteDocument",
        {
            finalProductNonComplianceId: nonComplianeId,
            wasteDocumentNumber: wasteDocumentNumber,
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value)
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});

$(document).on('click', '#FinalApproveOfNonCompliance', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();

    $.post("/FinalProductNoncompliance/TriggerFinalApprove",
        {
            finalProductNonComplianceId: nonComplianeId,
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value)
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});




$(document).on('click', '#btnRefferalFromSeparationAfterCEOOpinion', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var firstSampleSeparatedCount = $("#FirstSampleSeparatedCount").val() ?? 0;
    var secondSampleSeparatedCount = $("#SecondSampleSeparatedCount").val() ?? 0;
    var thirdSampleSeparatedCount = $("#ThirdSampleSeparatedCount").val() ?? 0;
    var forthSampleSeparatedCount = $("#ForthSampleSeparatedCount").val() ?? 0;

    $.post("/FinalProductNoncompliance/TriggerSaveSeparationInfoAfterCEOOpinion",
        {
            finalProductNonComplianceId: nonComplianeId,
            firstSampleSeparatedCount: firstSampleSeparatedCount,
            secondSampleSeparatedCount: secondSampleSeparatedCount,
            thirdSampleSeparatedCount: thirdSampleSeparatedCount,
            forthSampleSeparatedCount: forthSampleSeparatedCount
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});



$(document).on('click', '#btnRefferalFromCEO', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var firstSampleOpinion = $("#FirstSampleOpinionCEO").val() ?? 0;
    var secondSampleOpinion = $("#SecondSampleOpinionCEO").val() ?? 0;
    var thirdSampleOpinion = $("#ThirdSampleOpinionCEO").val() ?? 0;
    var forthSampleOpinion = $("#ForthSampleOpinionCEO").val() ?? 0;

    var validityStatus = opiniosIsValid();

    if (!validityStatus) {
        teram().showErrorMessage("لطفا همه موارد را تعیین تکلیف نمایید");
        return;
    }


    $.post("/FinalProductNoncompliance/TriggerSaveCEOInfo",
        {
            finalProductNonComplianceId: nonComplianeId,
            firstSampleOpinion: firstSampleOpinion,
            secondSampleOpinion: secondSampleOpinion,
            thirdSampleOpinion: thirdSampleOpinion,
            forthSampleOpinion: forthSampleOpinion,
            comments: $("#RefferalComments").val()
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});


$(document).on('click', '#btnRefferalFromCEOAfterFirstRevise', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var firstSampleOpinion = $("#FirstSampleOpinionCEO").val() ?? 0;
    var secondSampleOpinion = $("#SecondSampleOpinionCEO").val() ?? 0;
    var thirdSampleOpinion = $("#ThirdSampleOpinionCEO").val() ?? 0;
    var forthSampleOpinion = $("#ForthSampleOpinionCEO").val() ?? 0;


    var validityStatus = opiniosIsValid();

    if (!validityStatus) {

        teram().showErrorMessage("لطفا همه موارد را تعیین تکلیف نمایید");
        return;
    }

    $.post("/FinalProductNoncompliance/TriggerSaveCEOInfoAfterFirstRevise",
        {
            finalProductNonComplianceId: nonComplianeId,
            firstSampleOpinion: firstSampleOpinion,
            secondSampleOpinion: secondSampleOpinion,
            thirdSampleOpinion: thirdSampleOpinion,
            forthSampleOpinion: forthSampleOpinion,
            comments: $("#RefferalComments").val()
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message);
            }
        });
});

$(document).on('click', '#btnRefferalFromCEOAfterSeparation', function () {




    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var firstSampleOpinion = $("#FirstSampleOpinionCEOFinal").val() ?? 0;
    var secondSampleOpinion = $("#SecondSampleOpinionCEOFinal").val() ?? 0;
    var thirdSampleOpinion = $("#ThirdSampleOpinionCEOFinal").val() ?? 0;
    var forthSampleOpinion = $("#ForthSampleOpinionCEOFinal").val() ?? 0;


    var validityStatus = opiniosIsValid();

    if (!validityStatus) {

        teram().showErrorMessage("لطفا همه موارد را تعیین تکلیف نمایید");
        return;
    }

    $.post("/FinalProductNoncompliance/TriggerSaveCEOInfoAfterSeparation",
        {
            finalProductNonComplianceId: nonComplianeId,
            firstSampleOpinion: firstSampleOpinion,
            secondSampleOpinion: secondSampleOpinion,
            thirdSampleOpinion: thirdSampleOpinion,
            forthSampleOpinion: forthSampleOpinion,
            comments: $("#RefferalComments").val()
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});

function opiniosIsValid() {

    var selected = true;
    $(".opinionList").each(function () {

        if ($(this).val() == "" || $(this).val() == null || $(this).val() == 'undefined') {

            selected = false;
        }
    });
    return selected;
}

$(document).on('click', '#btnRefferal', function () {

    var approveStatus = $("#ApproveStatus").val();
    var comments = $("#Comments").val();


    if (approveStatus == "" || approveStatus == null || approveStatus == 'undefined') {
        teram().showErrorMessage("لطفاً یک وضعیت انتخاب نمایید");
        return;
    }

    nonComplianceId = $("#FinalProductNoncomplianceId").val();

    if (comments == "" && (approveStatus == "2" || approveStatus == "3")) {
        teram().showErrorMessage("وارد کردن توضیحات اجباری است")
        return;
    }
    switch (approveStatus) {

        case "1":

            var needToAdvisoryOpinionValue = $("#NeedToAdvisoryOpinion").val();

            if (needToAdvisoryOpinionValue == "1") {

                var destinationUserId = $("#NeedToAdvisoryOpinionRefferralList").val();

                var comments = $("#QcManagerComments").val();

                $.post("/FinalProductNoncompliance/TriggerQCManagerSendToSalesUnit",
                    {
                        destinationUser: destinationUserId,
                        finalProductNonComplianceId: nonComplianceId,
                        comments: comments
                    }, function (content) {
                        if (content.result == "ok") {
                            teram().showSuccessfullMessage(content.message.value);
                            bootbox.hideAll();
                            reloadDataTable();
                        }
                        else {

                            teram().showErrorMessage(content.message.value);
                        }
                    });
            }
            if (needToAdvisoryOpinionValue == "0") {

                var validity = opiniosIsValid();

                if (!validity) {
                    teram().showErrorMessage("لطفاً همه موارد را تعیین تکلیف نمایید");
                    return;
                }

                $.post("/FinalProductNoncompliance/TriggerSaveOpinionByQCManager",
                    {
                        finalProductNonComplianceId: $("#FinalProductNoncomplianceId").val(),
                        firstSampleOpinion: $("#FirstSampleOpinion").val() ?? 0,
                        secondSampleOpinion: $("#SecondSampleOpinion").val() ?? 0,
                        thirdSampleOpinion: $("#ThirdSampleOpinion").val() ?? 0,
                        forthSampleOpinion: $("#ForthSampleOpinion").val() ?? 0,
                        comments: $("#RefferalComments").val()

                    }, function (content) {
                        if (content.result == "ok") {
                            teram().showSuccessfullMessage(content.message.value);
                            bootbox.hideAll();
                            reloadDataTable();
                        }
                        else {
                            teram().showErrorMessage(content.message);
                        }
                    });
            }
            break;
        case "2":
            $.post("/FinalProductNoncompliance/TriggerQCManagerModifyOrder",
                {
                    finalProductNonComplianceId: nonComplianceId,
                    comment: comments,
                    approveStatus: approveStatus
                }, function (content) {

                    if (content.result == "ok") {
                        teram().showSuccessfullMessage(content.message.value);
                        bootbox.hideAll();
                        reloadDataTable();
                    }
                    else {
                        teram().showErrorMessage(content.message.value);
                    }
                });
            break;
        case "3":
            $.post("/FinalProductNoncompliance/TriggerQcManagerVoided",
                {
                    finalProductNonComplianceId: nonComplianceId,
                    comment: comments,
                    approveStatus: approveStatus
                }, function (content) {

                    if (content.result == "ok") {
                        teram().showSuccessfullMessage(content.message.value);
                        bootbox.hideAll();
                        reloadDataTable();
                    }
                    else {
                        teram().showErrorMessage(content.message.value);
                    }
                });
            break;
        default:
    }
});


$(document).on('click', '#btnRefferToOtherActioner ', function () {

    nonComplianceId = $("#FinalProductNoncomplianceId").val();
    var destinationUser = $("#ActionerId").val();

    $.post("/FinalProductNoncompliance/TriggerRefferFromProuctionManagerToTherActioner",
        {
            finalProductNonComplianceId: nonComplianceId,
            destinationUser: destinationUser
        }, function (content) {

            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});

$(document).on('click', '.btnRefferFromProuctionManager ', function () {

    var comments = $("#productionManagerComment").val();
    nonComplianceId = $("#FinalProductNoncomplianceId").val();
    var destinationUser = $("#OthersList").val();
    var needToAdvisoryOpinion = $("#hasNeedToRefferToOthers").val();

    $.post("/FinalProductNoncompliance/TriggerRefferFromProuctionManager",
        {
            finalProductNonComplianceId: nonComplianceId,
            comment: comments,
            destinationUser: destinationUser,
            needToAdvisoryOpinion: needToAdvisoryOpinion
        }, function (content) {

            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});


$(document).on('click', '#btnSendBackToProductionManager', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var comments = $("#Comments").val() ?? 0;

    $.post("/FinalProductNoncompliance/TriggerSendBackToProductionManager",
        {
            finalProductNonComplianceId: nonComplianeId,
            comment: comments,
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});


$(document).on('click', '#btnSendBackToProductionManagerFromQCManager', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var comments = $("#Comments").val() ?? 0;

    $.post("/FinalProductNoncompliance/TriggerSendBackToProductionManagerFromProuctionManager",
        {
            finalProductNonComplianceId: nonComplianeId,
            comment: comments,
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});



$(document).on('click', '#btnSendBackToQCManager', function () {

    var nonComplianeId = $("#FinalProductNonComplianceId").val();
    var comments = $("#Comments").val() ?? 0;

    $.post("/FinalProductNoncompliance/TriggerSendBackToQCManager",
        {
            finalProductNonComplianceId: nonComplianeId,
            comment: comments,
        }, function (content) {
            if (content.result == "ok") {
                teram().showSuccessfullMessage(content.message.value);
                bootbox.hideAll();
                reloadDataTable();
            }
            else {
                teram().showErrorMessage(content.message.value);
            }
        });
});

$(document).on("change", "#NotRelatedToProduction", function () {

    var isChecked = $("#NotRelatedToProduction").is(":checked");
    if (isChecked) {
        $('.chooseActioners').attr('style', 'display: flex !important');
        $("#btnRefferToOtherActioner").removeClass("d-none");
        $(".InputForm").addClass("d-none");
        $(".InputForm .select2").val("").trigger("change")
        $(".InputForm :checkbox").prop('checked', false);
        $('.InputForm :input[type=text]').val('');
    }
    else {
        $('.chooseActioners').attr('style', 'display: none !important');
        $("#btnRefferToOtherActioner").addClass("d-none");
        $(".InputForm").removeClass("d-none");
    }
});

$(document).on('click', '.showDetailsBtn', function () {

    var tr = $(this).closest('tr');
    $("#EditFinalProductNoncomplianceGrid tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/EditFinalProductNoncompliance/GetNonComplianceDetails", { finalProductNoncomplianceId: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-large"
        });
    }).done(function () {
        var section = $(".correctiveActions");
        indexCorrection($(section));
        var isEditMode = $("#IsEditMode").val();
        var hasPermissionForSave = $("#HasPermissionForSave").val();
        if (isEditMode != undefined && isEditMode.toLowerCase() == "true") {
            showHideElementsBasedOnData(hasPermissionForSave);
        }
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');
        var altFieldvar = "#CorrectiveActions\\[" + length + "\\]\\.ActionDate";
        var customoption = { "altField": altFieldvar };
        var options = $.extend(dateOption, customoption);
        datePickerInitilize(".actionDate", options);
        $('.chooseActioners').attr('style', 'display: none !important');
        $("#btnRefferToOtherActioner").addClass("d-none");
    });
});

function showHideElementsBasedOnData(hasPermissionForSave) {

    $(".hasNeedToRefferToOthersSection").addClass("d-none");
    $(".causationSection").removeClass("d-none");
    $(".causationSection div").removeClass("d-none");
    $("input[type=checkbox]").trigger("change");
    $(".select2").trigger("change");

    if (hasPermissionForSave != undefined && hasPermissionForSave.toLowerCase() == "true") {
        $(".lockedSection input[type=checkbox]").prop('disabled', true);
        $(".lockedSection input[type=text]").prop('disabled', true);
        $(".lockedSection").find(".select2").prop('disabled', true);
        $(".lockedSection #Comments").prop('disabled', false);
    }
    showHideCreateRowButton();
}

function indexCorrection(parent) {
    $(parent).find(".correctiveActionRowContainer").each(function (index) {
        $(this).find("[name*='[']").each(function () {
            var newName = $(this).attr("name").replace(new RegExp("(-)*[0-9]+", "gm"), index);
            $(this).attr("name", newName);
            $(this).attr("id", newName);
            if ($(this).hasClass('pDatePicker')) {
                $(this).attr("data-altfield", newName.replace("Persian", ""))
            }
            $(this).prev(".field-validation-valid").attr("data-valmsg-for", newName);
        });
    });

    var $select = $('.bootboxParent .select2').select2();
    $select.each(function (i, item) {
        $(item).select2({
            width: '100%',
            cache: true,
            dir: 'rtl',
            language: "fa",
            closeOnSelect: true,
            dropdownParent: '.bootboxParent'
        });
    });

    $('form').removeData('validator');
    $('form').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('form');
    showHideCreateRowButton();
}

$(document).on("change", ".actioners", function () {
    var section = $(".correctiveActions");
    indexCorrection($(section));
});

$(document).on("click", ".btnCreateRow", function () {

    var newRow = ".correctiveActionRow";
    var section = $(".correctiveActions");
    var length = $(".correctiveActionRowContainer").length - 1;
    // Find and destroy Select2 instances in the first row to be cloned
    var originalSelect = $(newRow).first().find('select.select2');
    originalSelect.select2('destroy');

    // Clone the row
    var clone = $(newRow).first().clone().removeClass($(this).data().newrow).removeClass("d-none");

    // Reset Select2 instances in the clone
    clone.find('select').each(function () {
        $(this).removeAttr('data-select2-id');
        $(this).removeClass('select2-hidden-accessible');
        $(this).next('.select2').remove();
    });

    // Append the cloned row to the section
    $(section).append(clone);

    // Re-initialize Select2 on the new row
    clone.find('select.select2').select2();

    // Reset inputs in the cloned row
    var lastRowContainer = section.find('.correctiveActionRowContainer:last');
    var lastRowInputs = lastRowContainer.find('input')
    lastRowInputs.val("");

    var altFieldvar = "#CorrectiveActions\\[" + length + "\\]\\.ActionDate";

    var customoption = { "altField": altFieldvar };

    var options = $.extend(dateOption, customoption);
    datePickerInitilize(".actionDate", options);

    // Adjust the indices for the new row
    indexCorrection($(section));
});

$(document).on("click", ".addButton", function () {

    var newRow = ".correctiveActionRow";
    var section = $(".correctiveActions");
    var length = $(".correctiveActionRowContainer").length - 1;
    // Find and destroy Select2 instances in the first row to be cloned
    var originalSelect = $(newRow).first().find('select.select2');
    originalSelect.select2('destroy');

    // Clone the row
    var clone = $(newRow).first().clone().removeClass($(this).data().newrow).removeClass("d-none");

    // Reset Select2 instances in the clone
    clone.find('select').each(function () {
        var $this = $(this);
        $this.removeAttr('data-select2-id');
        $this.removeClass('select2-hidden-accessible');
        $this.next('.select2').remove();
        // Remove any existing values
        $this.val(null);
    });

    // Append the cloned row to the section
    $(section).append(clone);

    // Re-initialize Select2 on the new row
    clone.find('select.select2').select2();

    // Reset inputs in the cloned row
    clone.find('input').val("");

    var altFieldvar = "#CorrectiveActions\\[" + length + "\\]\\.ActionDate";

    var customoption = { "altField": altFieldvar };

    var options = $.extend(dateOption, customoption);
    datePickerInitilize(".actionDate", options);
    // Adjust the indices for the new row
    indexCorrection($(section));
});

$(document).on("click", ".removeButton", function () {

    var section = $(".correctiveActions");
    $(this).parent().parent().parent().remove();
    indexCorrection(section);
});

function showHideCreateRowButton() {
    var count = $('.correctiveActionSection').length
    if (count == 1) {
        $('.btnCreateRow').removeClass("d-none");
    } else {
        $('.btnCreateRow').addClass("d-none");
    }
}
initFunction = function () {

    initialValidation();
    showHideCreateRowButton();
    var section = $(".correctiveActions");
    indexCorrection($(section));
}




