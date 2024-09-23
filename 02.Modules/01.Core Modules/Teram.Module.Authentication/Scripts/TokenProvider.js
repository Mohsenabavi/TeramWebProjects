var removeButton = '<button type="button" class="btn icon-red-color fa fa-close defaultButton removeRecord gridBtn"><i class="fas fa-times-circle" ></i ></button >';
var customButton = '<button type="button" class="btn btn-info btn-flat btn-xs copyContent gridBtn"><i class="fas fa-copy" ></i ></a >';

var dateOption = {
    "altField": '#ExpireDate',
    "altFormat": 'YYYY/MM/DD',
    "calendarType": "persian",
    "initialValue": false,
    "minDate": new persianDate().valueOf(),

}

$(document).ready(function () {
    InitialTable();
    datePickerInitilize(".pDatePickerr", dateOption);
});
function InitialTable() {
    table = $("#TokenProviderGrid").DataTable({
        "retrieve": true,
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "select": true,
        "rowId": 'key',
        "sServerMethod": "POST",
        "bSort": false,
        "bPaginate": true,
        "oLanguage": {
            "sSearch": "جستجو"
        }, "language": {
            "paginate": {
                "previous": "قبلی",
                "first": "صفحه اول",
                "next": "بعدی",
                "last": "آخرین صفحه"
            },
            "decimal": "",
            "emptyTable": "داده ای درجدول موجود نمی باشد",
            "info": "نمایش _START_  _END_ از _TOTAL_ رکورد",
            "infoEmpty": "نمایش 0  0 از 0 رکوردها",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "نمایش _MENU_ رکورد",
            "loadingRecords": "...درحال بارگذاری",
            "processing": "درحال پردازش...",
            "search": "جستجو:",
            "zeroRecords": "هیچ رکوردی یافت نشد",
            "sLengthMenu": "نمایش _MENU_ رکورد",
        },
        "select": {
            style: 'multi'
        },
        'columnDefs': [{
            "targets": -1,
            "data": null,
            "width": "150px",
            "responsivePriority": 1,
            "defaultContent": removeButton  + customButton

        },

        { "responsivePriority": 1, targets: 0 },
        { "responsivePriority": 1, targets: 1 },
        { "responsivePriority": 1, targets: 2 },
        { "responsivePriority": 1, targets: 3 },
        { "responsivePriority": 1, targets: 4 },
        { "responsivePriority": 1, targets: -1 }

        ],

    });

}
$(document).on('click', 'button[type="submit"]', function () { Save() });
$(document).on('click', 'button[type="reset"]', function () { Edit() });
$(document).on('click', '#ExpireDateIcon', function () { ShowDatePicker() });

$(document).on('change', '.claim', function () {
    // اسم کلیم را به عنوان دیتااتریبیوت به تکست باکس داده شده
    var tokenParameterName = $(this).attr('data-class-name')
    // تکست باکس مربوط به چک باکس را با اساس کلاس (اسم کلیم) پیدا میکنیم
    var valueTextbox = $('input[type=text].' + tokenParameterName);

    valueTextbox.attr("disabled", !$(this).is(':checked'));
});


$(document).on('click', '.copyContent', function () {
    var tr = $(this).closest('tr');
    var row = table.row(tr);
    var selectedRowData = row.data();
    /* Get the text field */
    var copyText = selectedRowData.content;
    copyToClipboard(copyText);
});

function ShowDatePicker(text) {
    pd.show();
}
function copyToClipboard(text) {
    var success = true,
        range = document.createRange(),
        selection;

    // For IE.
    if (window.clipboardData) {
        window.clipboardData.setData("Text", text);
    } else {
        // Create a temporary element off screen.
        var tmpElem = $('<div>');
        tmpElem.css({
            position: "absolute",
            left: "-1000px",
            top: "-1000px",
        });
        // Add the input value to the temp element.
        tmpElem.text(text);
        $("body").append(tmpElem);
        // Select temp element.
        range.selectNodeContents(tmpElem.get(0));
        selection = window.getSelection();
        selection.removeAllRanges();
        selection.addRange(range);
        // Lets copy.
        try {
            success = document.execCommand("copy", false, null);
        }
        catch (e) {
            copyToClipboardFF(text);
        }
        if (success) {
            teram().showSuccessfullMessage('مقدار توکن در کلیپ بورد کپی شد');

            tmpElem.remove();
        }
    }
}

editFunction = function (id) {
    Edit(id);
}


function Edit(id) {
    $.post("EditPartial/" + id, function (content) {

        if (content.result === "fail") {
            teram().showErrorMessage(content.message);
        } else

            $("#formInfo").html(content);
    }).done(function () {
        dateOption.initialValue = false;
        datePickerInitilize(".pDatePickerr", dateOption);
        initialValidation();
    });
}

function Save() {
    var data = {
        TokenId: $("#TokenId").val(),
        UserId: $("#UserId").val(),
        IssuedFor: $("#IssuedFor").val(),
        ExpireDate: $("#ExpireDate").val(),
        Policy: $("#Policy").val(),
        Description: $("#Description").val(),
        Content: $("#Content").val(),
        IsActive: $("#IsActive").val()
    }
    // لیست کلیم هایی که برای ذخیره ارسال میشود
    var claimsList = [];
    // چک باکس های تیک خورده
    var chk = $('input[type="checkbox"]:checked ');

    chk.each(function () {
        // اسم کلیم را به عنوان دیتااتریبیوت به تکست باکس داده شده
        var tokenParameterName = $(this).attr('data-class-name')
        if (tokenParameterName) { // برای اینکه چک باکس وضعیت را شامل نشود

            // تکست باکس مربوط به چک باکس را با اساس کلاس (اسم کلیم) پیدا میکنیم
            var valueTextboxHasValue = $('input[type=text]').hasClass(tokenParameterName);
            if (valueTextboxHasValue) {

                var valueTextbox = $('input[type=text].' + tokenParameterName);
                // به دست اوردن آیدی توکن پارامتر از روی آیدی تکست باکس
                var tokenId = valueTextbox.attr('id');
                var value = valueTextbox.val();
                // اگر مقدار دارد به لیست اضافه میشود
                if (value && value !== '') {
                    claimsList.push({
                        Id: tokenId,
                        Name: tokenParameterName,
                        Value: value
                    });
                }
            }
        }
    });

    $.ajax({
        url: "/TokenProvider/SaveToken",
        data: { model: data, claimsList: claimsList },
        type: 'POST',
        success: function (data) {

            onSuccess(data);
        }
    });
}



