$(document).on("click", "#btnFetchData", function () {

    $.post("/WithoutBasisNonCompliance/FetchRahkaranData",
        { orderNo: $("#OrderNo").val() },
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
    $("#OrderNo").attr("readonly", "readonly");
});