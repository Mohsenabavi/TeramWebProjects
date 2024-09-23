$(document).ready(function () {

    window.unCheckedNodes = [];
    InitialComponent();
});
function InitialComponent() {
    $('.select2').select2({
        width: '80%',
        dir: 'rtl'
    });
}


$(document).on('change', '#Role',
    function () {
        if ($("#Role").prop('selectedIndex') !== 0) {
            var roleId = $("#Role").val();
            LoadTree(roleId);
        }
    });



$(document).on('click', '#submit', function () { Save(); });

function LoadTree(roleId) {

    var url = $(".ComponetTree").attr("data-ajax-url");

    $.ajax({
        data: { id: roleId },
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            createJSTree(json);
        }
    });

}

function createJSTree(jsondata) {
    var id = $(".ComponetTree").attr("id");
    var plugins = $(".ComponetTree").attr("data-plugins");
    var checkcallback = $(".ComponetTree").attr("data-checkcallback") === "true";
    var multiple = $(".ComponetTree").attr("data-multiple") === "true";
    var arrayplugin = plugins.split(',');

    var plugin = [];
    for (var i in arrayplugin) {
        plugin.push(eval(arrayplugin[i]));
    }


    $("#" + id).jstree('destroy');

    $("#" + id).jstree({
        'core': {
            "animation": 0,
            "check_callback": checkcallback,
            "themes": { "stripes": true },
            "multiple": multiple,
            "loaded_state": true,
            'data': jsondata
        },
        'plugins': plugin,
        checkbox: { tie_selection: false, whole_node: true, three_state: false, keep_selected_style: false, cascade: "down" }
    }).on("check_node.jstree uncheck_node.jstree",
        function (e, data) {

            nodeselected = $("#permissions").jstree('get_checked');

        }).on('loaded.jstree',
            function (event, data) {
                $("#" + id).jstree().deselect_all(true);
                $("#" + id).jstree(true).refresh(true, true);
            });
}
$("#permissions").on("check_node.jstree",
    function (event, data) {

        console.log($("#permissions").jstree(true).get_checked());
        //if (window.unCheckedNodes.includes(data.node))

        var itemToRemoveIndex = window.unCheckedNodes.findIndex(function (item) {
            return item.a_attr["path"] === data.node.a_attr["path"];
        });

        // proceed to remove an item only if it exists.
        if (itemToRemoveIndex !== -1) {
            window.unCheckedNodes.splice(itemToRemoveIndex, 1);
        }

    });
$("#permissions").on("uncheck_node.jstree", function (event, data) {
    console.log($("#permissions").jstree(true).get_checked());

    window.unCheckedNodes.push(data.node);
});

function Save() {



    teram().showConfirm('ذخیره اطلاعات',
        'آیا نسبت به تغییرات دسترسی ها اطمینان دارید؟',
        function () {

            var selectedIds = [];

            var selectedElms = $('#permissions').jstree("get_checked", true);
            $.each(selectedElms,
                function (e, item) {
                    var modelData = {
                        Path: item.a_attr["path"],
                        HasPermission: true
                    }
                    selectedIds.push(modelData);
                });

            var unCheckedList = [];
            $.each(window.unCheckedNodes,
                function (e, item) {
                    var modelData = {
                        Path: item.a_attr["path"],
                        HasPermission: false
                    }
                    unCheckedList.push(modelData);
                });
            var roleId = $("#Role").val();

            $.ajax({
                url: "/PermissionControlPanel/SavePermission",
                data: { roleId: roleId, permissionsList: JSON.stringify(selectedIds), removedPermission: JSON.stringify(unCheckedList) },
                type: 'POST',
                success: function (data) {

                    if (data.result === "ok") {
                        window.unCheckedNodes = [];
                        teram().showSuccessfullMessage(data.message.value);
                    } else {

                        teram().showErrorMessage(data.message.value);
                    }
                }

            });
        },
        function () {
        });

}

$(window).on('load',
    function () {
        $(".nav-item").each(function () {
            if ($(this).children().hasClass("active")) {
                var sourceClass = $(this).children().children().attr("class");
                $(".card-header").children().children().addClass(sourceClass);
            }
        });
    });