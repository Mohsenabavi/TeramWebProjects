$(document).on('click', '.areaBtn', function () {


    var areaId = $(this).attr("id");  
    var divParent = $("#ParentDiv");

    $.ajax({
        url: "/Reservation/GetAreaSeats",
        data: { id: areaId },
        type: 'POST',
        success: function (data) {     
            divParent.html("");
            divParent.html(data);
            $(".draggable").draggable({
                revert: "invalid", // Snap back to the original position if not dropped on a droppable
                start: function (event, ui) {
                    // Enable draggable when it starts
                    $(this).draggable("enable");
                }
            });

            $(".droppable").droppable({
                drop: function (event, ui) {
                    var droppedElement = ui.helper;

                    // Check if the element is not already dropped
                    if (!droppedElement.hasClass("ui-state-highlight")) {
                        $(this)
                            .addClass("ui-state-highlight")
                            .find("p")
                            .html(droppedElement.find("p").html());

                        // Disable draggable once dropped
                        droppedElement.draggable("disable");
                        droppedElement.hide();
                    }
                }
            });
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
});
