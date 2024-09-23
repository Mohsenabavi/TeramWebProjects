var removeButton = '<button type="button" class="btn fa fa-close defaultButton removeRecord gridBtn"><img style="width:24px;height:24px" src="/icons/delete.png"></i ></button >';
var editButton = '<button   type="button" class="btn btn-xs btn-flat defaultButton editRecord gridBtn"><img style="width:24px;height:24px" src="/icons/edit.png"></i ></a >';
var customButton = '';

var defaultDataTableOptions = function () {
    return {
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "stateSave": true,
        "stateDuration": -1,
        "searching": false,
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
            "info": "نمایش _START_ تا _END_ از _TOTAL_ ردیف",
            "infoEmpty": "هیچ ردیفی برای نمایش وجود ندارد",
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

        dom: '<"float-left"B><"float-right"f>rt<"row"<"col-sm-4"l><"col-sm-4"i><"col-sm-4"p>>'
    };
}

reloadDataTable = function () {
    if (table !== undefined) {
        table.ajax.reload();
    }
}
var columnOperation = {
    "targets": -1,
    "data": null,
    "width": "70px",
    "responsivePriority": 2,
    "defaultContent": removeButton + editButton + customButton
};
initializeDatatable = function (datatableSelector = ".datatableGrid", otherOptions = null) {

    var datatableOptions = defaultDataTableOptions();

    if (otherOptions) {
        datatableOptions = $.extend(datatableOptions, otherOptions())
    }
    else if (changeDatatableOptions) {

        datatableOptions = $.extend(datatableOptions, changeDatatableOptions())
    }

    if ($(datatableSelector).data('has-operations-column')) {
        if (datatableOptions.columnDefs) {
            datatableOptions.columnDefs.push(columnOperation)
        } else {
            datatableOptions.columnDefs = [];
            datatableOptions.columnDefs.push(columnOperation);
        }
    }

    var element = document.querySelector(datatableSelector);
    if (element) {
        table = $(datatableSelector).DataTable(datatableOptions);
    }

}



