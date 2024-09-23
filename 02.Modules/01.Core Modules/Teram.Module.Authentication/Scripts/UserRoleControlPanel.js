removeButton = '';

$(document).ready(function () { 
    InitialComponent();
});

$(document).on('change', '#UserId', function () { GetRoles(); });
$(document).on('click', '#submit', function () { Save(); });

 
function GetRoles() {
    if ($("#UserId").prop('selectedIndex') != 0) {
        $.ajax({
            url: "/UserRoleControlPanel/GetUserRoles",
            data: { userId: $("#UserId").val() },
            type: 'POST',
            success: function (data) {
                $("#RoleId").empty();
                $.map(data.results, function (val, i) {
                    if (val.selected)
                        $("#RoleId").append("<option value=\"" + val.value + "\" selected=" + val.selected + ">" + val.text + "</option>");
                    else
                        $("#RoleId").append("<option value=\"" + val.value + "\">" + val.text + "</option>");
                });
            }
        });
    }
}

editFunction = function (id) {
    Edit(id);
} 

function Edit(id) {
    $.post("EditPartial/" + id, function (content) {
        $("#formInfo").html(content);
    }).done(function () {
        initialValidation();
        InitialComponent();
    });
}
function Save() { 
    $.ajax({
        url: "/UserRoleControlPanel/Save",
        data: { userId: $("#UserId").val(), roles: $("#RoleId").select2("val") },
        type: 'POST',
        success: function (data) {
            onSuccess(data); 
        }
    });
}
function InitialComponent() {
    $('.select2').select2({
        width: '100%',
        dir: 'rtl'
    });
}
 

afterSuccess = function () {
    clearForm();
}
