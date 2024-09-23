var inspectionResults = [];
var selectedId = "";
editButton = '';
removeButton = '';

previewButton = '<button  title="نمایش" type="button" class="btn btn-success gridBtn detailInfo ml-0 mr-0 w-100">نمایش</button>';

$(document).on('click', '.detailInfo', function () {

    var tr = $(this).closest('tr');
    $("#IncomingGoodsInspectionGrid tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/IncomingGoodsInspection/GetIncomingGoodsInspectionItems", { incomingGoodsInspectionId: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'extra-large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-extra-large detailsbootbox"
        });

    });
});

function isNoneSelected() {

    var radioButtons = document.getElementsByName("FinalApproveStatus");

    for (var i = 0; i < radioButtons.length; i++) {
        if (radioButtons[i].checked) {
            return false;
        }
    }
    return true;
}

$(document).on('click', '.btnSaveFinalApprove', function () {


    var incomingGoodsInspectionId = $(".IncomingGoodsInspectionId").val();

    var selectedValue = $('input[name="FinalApproveStatus"]:checked').val();

    if (isNoneSelected()) {
        teram().showErrorMessage("لطفا وضعیت تایید نهایی را مشخص نمایید");
        return;
    }
    $.post("/IncomingGoodsInspection/FinalApproveForm", { incomingGoodsInspectionId: incomingGoodsInspectionId, finalApprove: selectedValue }, function (content) {

        if (content.result == "ok") {

            teram().showSuccessfullMessage(content.message.value);
            reloadDataTable();
            bootbox.hideAll();
        }
        else {

            teram().showErrorMessage(content.message.value);
        }
    });
});




$(document).on('change', '#NeedToRefferal', function () {

    var isChecked = $("#NeedToRefferal").is(":checked");
    if (isChecked) {

        $(".productionSupervisorsList").removeClass("d-none");

        let userId = $("#ReferralUserId");

        $.post("/IncomingGoodsInspection/GetProductionSupervisors",
            function (content) {

                $(userId).empty();
                $(userId).append(
                    '<option value="">-- یک فرد انتخاب کنید --</option>'
                );
                $.each(content.data,
                    function (index, item) {
                        $(userId).append(
                            '<option value="' + item.userId + '">' + item.username + '</option>'
                        );
                    });
            });
    }
    else {
        $(".productionSupervisorsList").addClass("d-none");
        $("#ReferralUserId").val("");
    }
});

$(document).on('change', '#IsSampleGoods', function () {

    var isSampleGoods = $(this).is(":checked");
    var hasFunctionalTest = $("#HasFunctionalTest").is(":checked");

    if (isSampleGoods || hasFunctionalTest) {

        $(".finalApproveSection").addClass("d-none");
    }
    else {
        $(".finalApproveSection").removeClass("d-none");
    }
});

$(document).on('change', '#HasFunctionalTest', function () {

    var isSampleGoods = $("#IsSampleGoods").is(":checked");
    var hasFunctionalTest = $(this).is(":checked");

    if (isSampleGoods || hasFunctionalTest) {

        $(".finalApproveSection").addClass("d-none");
    }
    else {
        $(".finalApproveSection").removeClass("d-none");
    }

});



$(document).on('click', '.btnAddComment', function () {

    var incomingGoodsInspectionId = $(".IncomingGoodsInspectionId").val();
    var commnets = $(".commnets").val();

    if (commnets == "" || commnets == null) {
        teram().showErrorMessage("لطفاً توضیحات را وارد نمایید");
    }
    else {
        $.post("/IncomingGoodsInspection/AddComment", { id: incomingGoodsInspectionId, comments: commnets }, function (content) {

            if (content.result == "ok") {

                teram().showSuccessfullMessage(content.message.value);
                reloadDataTable();
                bootbox.hideAll();
            }
            else {

                teram().showErrorMessage(content.message.value);
            }
        });
    }
});

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/IncomingGoodsInspection/GetIncomingGoodsInspectionGridData",
            "data": function (d) {
                return $.extend({}, d, {
                    "qualityInspectionNumber": $("#QualityInspectionNumberFilter").val(),
                    "goodsTitle": $("#GoodsTitleFilter").val(),
                    "vendorName": $("#VendorNameFilter").val(),
                    "inspectionFormStatus": $("#InspectionFormStatus").val(),
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
                        + previewButton;
                }
            }
        ],
        'order': [[1, 'asc']]
    };
}
$(function () {

    initialValidation();
    $('#GoodsInfoIsApproved').change(function () {
        if ($(this).is(':checked')) {
            $(".hiddenSection").removeClass("d-none");
        } else {
            $(".hiddenSection").addClass("d-none");
        }
    });


    $(document).on("click", "#btnFetchData", function () {

        $.post("/IncomingGoodsInspection/FetchRahkaranData",
            { number: $("#QualityInspectionNumber").val() },
            function (content) {
                inspectionResults = content.results;
                var selectOption = $("#GoodsList");
                selectOption.empty();
                selectOption.append(
                    '<option value="">-- یک محصول انتخاب کنید --</option>'
                );
                $.each(content.results,
                    function (index, item) {
                        selectOption.append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );
                    });
            });
        $("#QualityInspectionNumber").attr("readonly", "readonly");
    });
    $(document).on("change", "#GoodsList", function () {
        var selectedIndex = $(this).val();

        if (selectedIndex!=null && selectedIndex.length > 0) {
            const result = inspectionResults.find(x => x.code == selectedIndex);
            $("#GoodsCode").val(result.code);
            $("#Name").val(result.name);
            $("#Quantity").val(result.quantity);
            $("#VendorName").val(result.title);
            $("#Refkind").val(result.refkind);
            $("#Counterkind").val(result.counterkind);
            $("#PersianTestDate").val(result.persianTestDate);
            $("#PersianDocumentDate").val(result.persianDocumentDate);
        }
    });
    function getPartial(selectedId) {

        var divParent = $("#ControlPalnsDiv");


        $.ajax({
            url: "/IncomingGoodsInspection/GetCategoryPartial",
            data: { categoryId: selectedId },
            type: 'POST',
            success: function (data) {
                $(".partialSection").remove();
                divParent.before(data);
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    }
    $(document).on("change", "#Category", function () {

        if ($("#GoodsInfoIsApproved").is(':checked')) {
            $(".hiddenSection").removeClass("d-none");
        }
        else {
            $(".hiddenSection").addClass("d-none");
        }

        selectedId = $(this).val();
        if (selectedId > 0) {
            getPartial(selectedId);
            $(".submitArea").removeClass("d-none");

        }
        else {
            $(".submitArea").addClass("d-none");
        }
    });
});
