editButton = '';

$(document).on("click", "#btnFetchData", function () {

    $.post("/AssetSelfExpression/GetAssetInfoByPlaqueNumber",
        { plaqueNumber: $("#PlaqueNumber").val() },
        function (content) {

            if (content.result == "ok") {
                var data = content.data;
                $("#AssetID").val(data.assetID);
                $("#Title").val(data.title);
                $("#Code").val(data.code);
            }
            else {
                teram().showErrorMessage("اموالی با شماره پلاک وارد شده یافت نشد"); 
            }            
        });         
});

function changeDatatableOptions() {
    return {
        'ajax': {
            "url": "/AssetSelfExpression/GetAssetsData",          
        },
        "columnDefs": [
            {
                "targets": -1,
                "responsivePriority": 2,
                "data": null,
                "width": "150px",
                "responsivePriority": 2,
                "render": function (data, type, row) {
                    return removeButton + editButton;
                }
            }
        ]
    };
}
