

$(document).ready(function () { 
    InitialComponent();
});


afterSuccess = function () {
    clearForm();
}
 

$(window).on('load', function () {
    $(".nav-item").each(function () {
        if ($(this).children().hasClass("active")) {
            var sourceClass = $(this).children().children().attr("class");
            $(".card-header").children().children().addClass(sourceClass);
        }
    });
}); 