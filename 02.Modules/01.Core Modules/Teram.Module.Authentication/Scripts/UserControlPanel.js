
var removeButton = '<button type="button" class="btn icon-red-color fa fa-close removeRecord gridBtn"><i class="fas fa-times" ></i ></button >';
var editButton = '<button   type="button" class="btn icon-green-color btn-xs btn-flat editRecord gridBtn"><i class="fas fa-edit" ></i ></button >';
var table;

$(document).ready(function () {
    initialValidation();
    table = $("#UserControlPanelGrid").DataTable({
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "searching": false,
        "rowId": 'key',
        "sServerMethod": "POST",
        "bSort": false,
        "bPaginate": true,
        "ajax": {
            "url": "/UserControlPanel/GetData",
            "data": function (d) {
                d.name = $("#Name").val();
                d.userName = $("#UserName").val();
                d.email = $("#Email").val();
                d.phoneNumber = $("#PhoneNumber").val();
            }
        },
        "language": {
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
        "columnDefs": [{
            "targets": -1,
            "data": null,
            "width": "150px",
            "responsivePriority": 1,
            "defaultContent": removeButton + editButton
        }],

    });
});

$(document).on('click', '#ChangePassword', function () { OpenChangePassword(); });
$(document).on('click', '#ResetPassword', function () { ResetPassword(); });
$(document).on('click', '#reset', function () { Edit(null) });



editFunction = function (id) {
    Edit(id);
}


function Edit(id) {
    $.post("EditPartial/" + id, function (content) {
        $("#formInfo").html(content);
    }).done(function () {
        initialValidation();
    });
}

// دکمه تغییر رمز گذاشته بودم که صفحه جدا باز میکرد
function OpenChangePassword(id) {

    $.ajax({
        url: "/UserControlPanel/ChangePassword",
        data: { userId: $("#Id").val() },
        type: 'POST',
        success: function (content) {
            $("#formInfo").html(content);

        }
    });
}

// فعلا ریست پسورد را برداشتیم
function ResetPassword() {

    var data = {
        UserId: $("#UserId").val(),
    }
    $.ajax({
        url: "/UserControlPanel/ResetPassword",
        data: { model: data },
        type: 'POST',
        success: function (data) {
            onsuccess(data);
        }
    });
}

function SaveUser() {

    if ($("#Email").val() == "") {
        if ($("#EmailConfirmed").is(':checked')) {
            teram().showErrorMessage('مقدار پست الکترونیکی وارد نشده امکان تایید پست الکترونیکی وجود ندارد');
            return
        }
    }

    if ($("#PhoneNumber").val() == "") {
        if ($("#PhoneNumberConfirmed").is(':checked')) {
            teram().showErrorMessage('شماره تماس وارد نشده امکان تایید شماره تماس وجود ندارد');
            return
        }
    }

    var data = {
        PhoneNumber: $("#PhoneNumber").val(),
        EmailConfirmed: $("#EmailConfirmed").val(),
        PhoneNumberConfirmed: $("#PhoneNumberConfirmed").val(),
        Email: $("#Email").val(),
        UserName: $("#UserName").val(),
        Id: $("#Id").val(),
        ConcurrencyStamp: $("#ConcurrencyStamp").val(),
        SecurityStamp: $("#SecurityStamp").val()
    }

    var passwordData = {
        UserId: $("#UserId").val(),
        OldPassword: $("#OldPassword").val(),
        NewPassword: $("#NewPassword").val(),
        ConfirmPassword: $("#ConfirmPassword").val(),
    }

    $.ajax({
        url: "/UserControlPanel/Save",
        data: { model: data, passwordModel: passwordData },
        type: 'POST',
        success: function (data) {

            onsuccess(data);
        }
    });
}



afterSuccess = function () {
    Edit(null)
}

$(document).on("click", "#btnSearch", function (e) {
    table.ajax.reload();
});
