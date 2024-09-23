$(document).ready(function () {
    initialValidation();
});

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
 

afterSuccess = function () {
   clearForm();
}
