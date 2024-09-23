
var globalParentId;
var globalPositionId;
var globalUserId;

$(document).ready(function () {
    createTree();
    loadParents();
    loadPositions();
    loadUsers();
});
function createTree() {

    $.ajax({
        url: '/OrganizationChart/GetChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Initialize jsTree
            $('#jstree').jstree({
                'core': {
                    'data': data,
                },
                'plugins': ['contextmenu'],
                'contextmenu': {
                    'items': function (node) {
                        return {
                            'deleteItem': {
                                'label': 'حذف',

                                'icon': 'delIcon',
                                'action': function () {
                                    // Handle custom action here                                   
                                    deleteNode(node.id);
                                }
                            },
                            'editItem': {
                                'label': 'ویرایش',

                                'icon': 'delIcon',
                                'action': function () {
                                    // Handle custom action here                                   
                                    editFunction(node.id);
                                }
                            }
                        };
                    }
                }
            });
            $('#jstree').on('loaded.jstree', function () {
                $(this).jstree('open_all');
            });

            $('#jstree').on('select_node.jstree', function (e, data) {
                var nodeId = data.node.id;
                $("#ParentOrganizationChartId").val(nodeId).trigger("change");
            });
        }
    });
}




function deleteNode(id) {
    teram().showConfirm('حذف اطلاعات', 'آیا قصد دارید این آیتم را حذف نمایید؟',
        function () {
            removeNode(id)
        },
        function () {
        });
}

function OnAddSuccess(data) {
    onSuccess(data);
    $('#jstree').jstree('destroy');
    createTree();
    loadParents();
}


function removeNode(id) {

    $.post("/OrganizationChart/RemoveNode", { id: id }, function (updateResult) {

        if (updateResult.result == "ok") {
            teram().showSuccessfullMessage(updateResult.message.value);            
        }
        else {
            teram().showErrorMessage(updateResult.message.value);
        }
    }).done(function () {
        $('#jstree').jstree('destroy');
        createTree();
        loadParents();
    });
};

function loadParents() {



    $.post("/OrganizationChart/FillParents",
        function (content) {

            $("#ParentOrganizationChartId").empty();
            $("#ParentOrganizationChartId").append(
                '<option value="">-- یک فرد انتخاب کنید --</option>'
            );
            $.each(content,
                function (index, item) {
                    $("#ParentOrganizationChartId").append(
                        '<option value="' + item.value + '">' + item.text + '</option>'
                    );
                });

            if (globalParentId) {
                $("#ParentOrganizationChartId").val(globalParentId).trigger('change');
            }
        });
}
$(document).on("change", "#ParentOrganizationChartId", function () {

    globalParentId = $(this).parent().data().id;
});

$(document).on("change", "#PositionId", function () {

    globalPositionId = $(this).parent().data().id;
});

$(document).on("change", "#UserId", function () {

    globalUserId = $(this).parent().data().id;
});


function loadPositions() {

    $.post("/OrganizationChart/FillPositions",
        function (content) {

            $("#PositionId").empty();
            $("#PositionId").append(
                '<option value="">-- یک سمت انتخاب کنید --</option>'
            );
            $.each(content,
                function (index, item) {
                    $("#PositionId").append(
                        '<option value="' + item.value + '">' + item.text + '</option>'
                    );
                });

            if (globalPositionId) {
                $("#PositionId").val(globalPositionId).trigger('change');
            }


        });
}

function loadUsers() {

    $.post("/OrganizationChart/FillUsers",
        function (content) {

            $("#UserId").empty();
            $("#UserId").append(
                '<option value="">-- یک فرد انتخاب کنید --</option>'
            );
            $.each(content,
                function (index, item) {
                    $("#UserId").append(
                        '<option value="' + item.value + '">' + item.text + '</option>'
                    );
                });

            if (globalUserId) {
                $("#UserId").val(globalUserId).trigger('change');
            }
        });
}

editFunction = function (id) {
    Edit(id);
}

function Edit(id) {
    $.post("EditPartial/" + id, function (content) {
        $(".formInfo").html(content);
        initialValidation();
        $("#ParentOrganizationChartId").trigger("change");
        loadParents();
        $("#PositionId").trigger("change");
        loadPositions();
        $("#UserId").trigger("change");
        loadUsers();
    }).done(function () {


        createTree();
    });
}