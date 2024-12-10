$(document).on('click', '#exportexel', function () {
    changeDatatableOptions();
    var data = "?orderNo=" + $("#OrderNoFilter").val()
        + "&number=" + $("#NumberFilter").val()
        + "&productCode=" + $("#ProductCodeFilter").val()
        + "&tracingCode=" + $("#TracingCodeFilter").val()
        + "&orderTitle=" + $("#OrderTitleFilter").val()
        + "&productName=" + $("#ProductNameFilter").val()
    window.location.href = "/FinalProductInspection/PrintExcel" + data;
});

$(document).on("click", "#btnFetchData", function () {

    $.post("/FinalProductInspection/FetchPalletInfo",
        { palletNo: $("#Number").val() },
        function (content) {

            if (content.result == "ok") {
                inspectionResults = content.content;
                $("#OrderNo").val(inspectionResults.orderNo);
                $("#OrderTitle").val(inspectionResults.orderTitle);
                $("#ProductCode").val(inspectionResults.productCode);
                $("#ProductName").val(inspectionResults.productName);
                $("#ControlPlan").val(inspectionResults.controlPlan);
                $("#StartInterval").val(inspectionResults.startInterval);
                $("#EndInterval").val(inspectionResults.endInterval);
                $("#SampleCount").val(inspectionResults.sampleCount);
                $("#TotalCount").val(inspectionResults.quantity);
                $("#Number").attr("readonly", "readonly");
            }
            else {

                teram().showErrorMessage(content.message);
            }
        });
});

$(document).ready(function () {


    var section = $(".finalProductInspectionDefects");
    indexCorrection($(section));
});

function finalProductInspectionOnSuccess(data) {

    onSuccess(data);
    if (data.hasNoDefect != null && data.hasNoDefect.toLowerCase() == "true") {
        $("#OrderNo").val("");
        $("#OrderTitle").val("");
        $("#ProductCode").val("");
        $("#ProductName").val("");
        $("#ControlPlan").val("");
        $("#StartInterval").val("");
        $("#EndInterval").val("");
        $("#SampleCount").val("");
        $("#TotalCount").val("");
        $("#Number").val("");
        $("#TracingCode").val("");
        $("#Number").removeAttr("readonly", "readonly");
        return;
    }
    if (data.id != null) {
        editAfterSave(data.id);
        bootbox.hideAll();
    }
}

function editAfterSave(id) {

    $("#operationMessage").html('');
    $("#operationMessage").removeClass();

    if (editFunction == null) {
        if (editInSamePage == true) {

            var editUrl = (overrideEditUrl != "") ? overrideEditUrl : "PartialEdit";

            $.post(editUrl, { id: id }, function (result) {
                $(".manageForm").html(result);
                datePickerInitilize();
                initialValidation();
                teram().pageInit();

                if (initFunction != null) {
                    initFunction();
                }
            })
        }
        else {
            if (overrideEditUrl != "") {
                window.location.href = overrideEditUrl + "/" + id;
                return;
            }
            window.location.href = "Edit/" + id;
        }
    }
    else {
        editFunction(id);
    }

}

function indexCorrection(parent) {
    $(parent).find(".finalProductInspectionDefectRowContainer").each(function (index) {
        $(this).find("[name*='[']").each(function () {
            var newName = $(this).attr("name").replace(new RegExp("(-)*[0-9]+", "gm"), index);

            $(this).attr("name", newName);
            $(this).attr("id", newName);
            if ($(this).hasClass('pDatePicker')) {
                $(this).attr("data-altfield", newName.replace("Persian", ""))
            }
            $(this).prev(".field-validation-valid").attr("data-valmsg-for", newName)

            if ($(this).hasClass('select2')) {
                $(this).select2({
                    width: '100%',
                    cache: true,
                    dir: 'rtl',
                    language: "fa",
                    closeOnSelect: true,
                });
            }
        })
    });
    showHideCreateRowButton();
}

initFunction = function () {

    initialValidation();
    showHideCreateRowButton();
    var section = $(".finalProductInspectionDefects");
    indexCorrection($(section));
}

$(document).on("click", ".btnCreateNoncompliance", function () {

    var controlPlanDefectId = $(this).closest('.defectRow').find('.defectsSelectList').find(':selected').val();

    var relatedControlPlanDefectId = $(this).closest('.defectRow').find('.relatedControlPlanDefectId').val();

    var passedControlPlanDefectId;

    if (controlPlanDefectId != undefined) {

        passedControlPlanDefectId = controlPlanDefectId;
    }
    else {
        passedControlPlanDefectId = relatedControlPlanDefectId;
    }


    var orderNo = $("#OrderNo").val();
    var productCode = $("#ProductCode").val();
    var number = $("#Number").val();
    var finalProductInspectionId = $("#FinalProductInspectionId").val();

    $.post("/FinalProductNoncompliance/GetNonCompliances",
        {
            controlPlanDefectId: passedControlPlanDefectId,
            orderNo: orderNo,
            productCode: productCode,
            number: number,
            finalProductInspectionId: finalProductInspectionId
        },
        function (content) {

            bootbox.setDefaults({ size: 'large' });
            bootbox.dialog({
                message: content,
                className: "bootbox-large",
                backdrop: true,
            });

            $('form').removeData('validator');
            $('form').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('form');

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
});

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/FinalProductInspection/GetFinalProductInspectionGridData",
            "data": function (d) {
                return $.extend({}, d, {
                    "orderNo": $("#OrderNoFilter").val(),
                    "number": $("#NumberFilter").val(),
                    "productCode": $("#ProductCodeFilter").val(),
                    "tracingCode": $("#TracingCodeFilter").val(),
                    "orderTitle": $("#OrderTitleFilter").val(),
                    "productName": $("#ProductNameFilter").val()
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
                    return editButton + removeButton
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}

$(document).on("change", "#NoncomplianceSelectList", function () {

    $.post("/FinalProductNoncompliance/GetDefectNoComplianceData",
        {
            finalProductNoncomplianceId: $(this).val(),
            palletNumber: $("#PalletNumber").val(),
            controlPlanDefectId: $("#PassedControlPlanDefectId").val(),
            finalProductInspectionId: $("#PassedFinalProductInspectionId").val()
        },
        function (content) {
            $(".nonComplianceForm").html(content);
        });
});

$(document).on("click", ".btnCreateRow", function () {
    var controlPlan = $("#ControlPlan").val();
    if (controlPlan == "" || controlPlan == "undefined" || controlPlan == null) {
        teram().showErrorMessage("طرح کنترلی نا مشخص است");
    }
    else {
        var newRow = ".finalProductInspectionDefectRow";
        var clone = $(newRow).clone().removeClass($(this).data().newrow).removeClass("d-none");
        var section = $(".finalProductInspectionDefects");
        $(section).append(clone);
        GetDefects();
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');
        $(".defectsSelectList").find(".select2").removeClass("select2-hidden-accessible");
        $(".defectsSelectList").find("span").hide();
    }
});

$(document).on("click", ".addButton", function () {

    var controlPlan = $("#ControlPlan").val();
    if (controlPlan == "" || controlPlan == "undefined" || controlPlan == null) {
        teram().showErrorMessage("طرح کنترلی نا مشخص است");
    }
    else {
        var newRow = ".finalProductInspectionDefectRow";

        var clone = $(newRow).first().clone().removeClass($(this).data().newrow).removeClass("d-none");
        var section = $(".finalProductInspectionDefects");
        $(section).append(clone);
        var lastRowContainer = section.find('.finalProductInspectionDefectRowContainer:last');
        lastRowContainer.find('input').val("");
        GetDefects();
        $(".defectsSelectList").find(".select2").removeClass("select2-hidden-accessible");
        $(".defectsSelectList").find("span").hide();
    }
});


function GetDefects() {

    $.post("/FinalProductInspection/GetDefects",
        {
            controlPlan: $("#ControlPlan").val()
        },


        function (content) {

            var selectBox = $('#FinalProductInspectionDefects\\[-1\\]\\.ControlPlanDefectId');
            selectBox.empty(); // Clear existing options

            selectBox.append($('<option>').text("-  انتخاب کنید -").val(""));
            $.each(content, function (index, item) {
                selectBox.append($('<option>').text(item.text).val(item.value));
            });
        }).done(function () {
            var section = $(".finalProductInspectionDefects");
            indexCorrection($(section));
        });
}



$(document).on("click", ".removeButton", function () {

    var section = $(".finalProductInspectionDefects");
    $(this).parent().parent().parent().remove();
    indexCorrection(section);

});

function showHideCreateRowButton() {
    var count = $('.finalProductInspectionDefectSection').length
    if (count == 1) {
        $('.btnCreateRow').removeClass("d-none");
    } else {
        $('.btnCreateRow').addClass("d-none");

    }
}

$(document).on("click", ".SpecialresetForm", function () {
    window.location.reload();
});
