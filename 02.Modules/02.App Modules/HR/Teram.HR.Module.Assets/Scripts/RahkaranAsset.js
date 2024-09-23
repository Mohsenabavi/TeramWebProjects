removeButton = '';
editButton = '';
approveButton = '<button  title="تایید" type="button" class="btn btn-success gridBtn detailInfo ml-0 mr-0 w-100">ثبت تایید یا عدم تایید</button>';

$(document).on('click', '.detailInfo', function () {

    var tr = $(this).closest('tr');
    $("#AssetsGrid tr").each(function () {
        $(this).removeClass("selected");
    });
    tr.addClass("selected");
    var row = table.row(tr);
    var selectedRowData = row.data();
    $.post("/RahkaranAsset/ShowAssetApproveInfo", { rahkaranAssetId: selectedRowData.key }, function (content) {
        bootbox.setDefaults({ size: 'extra-large' });
        bootbox.dialog({
            message: content,
            className: "bootbox-extra-large detailsbootbox"
        });
    });
});


function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/RahkaranAsset/GetAssetsData",
            "data": function (d) {
                return $.extend({}, d, {
                    "personnelCode": $("#PersonnelCode").val(),
                    "title": $("#Title").val(),
                    "fullName": $("#FullName").val(),
                    "assetID": $("#AssetID").val(),
                    "code": $("#Code").val(),
                    "plaqueNumber": $("#PlaqueNumber").val(),
                    "groupTitle": $("#GroupTitle").val(),
                    "depreciatedMethodTitle": $("#DepreciatedMethodTitle").val(),
                    "costCenter": $("#CostCenter").val(),
                    "settlementPlace": $("#SettlementPlace").val(),
                    "collector": $("#Collector").val(),
                    "cost": $("#Cost").val(),                    
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
                    return removeButton + editButton + approveButton;
                }
            }
        ]
    };
}

$(document).on('click', '#exportexel', function () {
    changeDatatableOptions();
    var data = "?personnelCode=" + $("#PersonnelCode").val()
        + "&title=" + $("#Title").val()
        + "&fullName=" + $("#FullName").val()
        + "&assetID=" + $("#AssetID").val()
        + "&code=" + $("#Code").val()
        + "&plaqueNumber=" + $("#PlaqueNumber").val()
        + "&groupTitle=" + $("#GroupTitle").val()
        + "&depreciatedMethodTitle=" + $("#DepreciatedMethodTitle").val()
        + "&costCenter=" + $("#CostCenter").val()
        + "&settlementPlace=" + $("#SettlementPlace").val()
        + "&collector=" + $("#Collector").val()
        + "&cost=" + $("#Cost").val()
    window.location.href = "/RahkaranAsset/PrintExcel" + data;
});