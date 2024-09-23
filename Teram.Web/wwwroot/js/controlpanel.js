
function activeSidebarMenu() {
    var url = window.location.pathname;
    var item = $(".nav-link[href='" + url + "']");
    $(item).addClass("active");
    $(item).parents("li.nav-item.has-treeview").addClass("menu-open");

}

 
$(document).ready(function () {
    activeSidebarMenu();

    $(document).on('click', '.editRecord', function () {
        $("#operationMessage").html('');
        $("#operationMessage").removeClass();

        $("tr").each(function () {
            $(this).removeClass("selected");
        });

        var tr = $(this).closest('tr');       
        tr.addClass("selected");

        var id = $(this).closest('tr').attr("id");
        if (editFunction == null) {
            if (editInSamePage == true) {

                var editUrl = (overrideEditUrl != "") ? overrideEditUrl : "PartialEdit";

                $.post(editUrl, { id: id }, function (result) {
                    $(".manageForm").html(result);
                    datePickerInitilize();
                    initialValidation();
                    teram().pageInit();

                    if (initFunction != null) {
                        initFunction();
                    }

                })
            }
            else {
                if (overrideEditUrl != "") {
                    window.location.href = overrideEditUrl + "/" + id;
                    return;
                }
                window.location.href = "Edit/" + id;
            }
        }
        else {
            editFunction(id);
        }
    });
    $(document).on('click', '.detailRecord', function () {
        var id = $(this).closest('tr').attr("id");
        if (overrideDetailUrl !== "") {
            window.location.href = overrideDetailUrl + "/" + id;
            return;
        }
        window.location.href = "Detail/" + id;
    });
    $(document).on('click', '.removeRecord', function () {
        var id = $(this).closest('tr').attr("id");
        teram().showConfirm('حذف اطلاعات', 'آیا قصد دارید این آیتم را حذف نمایید؟',
            function () {
                var url = 'remove/'
                if (overrideDeleteUrl !== "") {
                    url = overrideDeleteUrl;
                }
                $.ajax({
                    url: url,
                    data: { "key": id },
                    type: 'DELETE',
                    success: function (data) {
                        onSuccess(data);
                    }
                });
            },
            function () {
            });
    });
    $(document).on('click', '.shouldReloadGrid', function () {
        reloadDataTable();
    });

    teram().pageInit();


});

