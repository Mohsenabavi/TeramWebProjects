
function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/FlowInstruction/GetFlowInstructions",
            "data": function (d) {
                return $.extend({}, d, {
                    "fromStatus": $("#FromStatusFilter").val(),
                    "toStatus": $("#ToStatusFilter").val(),
                    "formStatus": $("#FormStatusFilter").val(),   
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


function indexCorrection(parent) {
    $(parent).find(".flowInstructionConditionRowContainer").each(function (index) {
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

$(document).ready(function () {

    var section = $(".flowInstructionConditions");
    indexCorrection($(section));
});

$(document).on("click", ".btnCreateRow", function () {

    var newRow = ".flowInstructionConditionRow";
    var clone = $(newRow).clone().removeClass($(this).data().newrow).removeClass("d-none");
    var section = $(".flowInstructionConditions");
    $(section).append(clone);
    $('form').removeData('validator');
    $('form').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('form');
    $(".fieldsSelectList").find(".select2").removeClass("select2-hidden-accessible");
    $(".fieldsSelectList").find("span").hide();
    indexCorrection($(section));
    showHideCreateRowButton();
});

$(document).on("click", ".addButton", function () {


    var newRow = ".flowInstructionConditionRow";

    var clone = $(newRow).first().clone().removeClass($(this).data().newrow).removeClass("d-none");
    var section = $(".flowInstructionConditions");
    $(section).append(clone);
    var lastRowContainer = section.find('.flowInstructionConditionRowContainer:last');
    lastRowContainer.find('input').val("");    
    $(".fieldsSelectList").find(".select2").removeClass("select2-hidden-accessible");
    $(".fieldsSelectList").find("span").hide();
    indexCorrection($(section));
});

$(document).on("click", ".removeButton", function () {

    var section = $(".flowInstructionConditions");
    $(this).parent().parent().parent().remove();
    indexCorrection(section);

});

function showHideCreateRowButton() {
    var count = $('.flowInstructionConditionSection').length
    if (count == 1) {
        $('.btnCreateRow').removeClass("d-none");
    } else {
        $('.btnCreateRow').addClass("d-none");

    }
}


initFunction = function () {

    initialValidation();
    showHideCreateRowButton();
    var section = $(".flowInstructionConditions");
    indexCorrection($(section));
}

