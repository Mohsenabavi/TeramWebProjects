

$(document).ready(function () {

    $(window).scroll(function () {

        if ($(this).scrollTop() > 130) {
            $("#pageNavigation").addClass("shrink");
            $("#navBarExtraItemsShrink").addClass("navBarExtraItemsShrink");

        }
        else {
            $("#pageNavigation").removeClass("shrink");
            $("#navBarExtraItemsShrink").removeClass("navBarExtraItemsShrink");
        }

        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });

    $('.back-to-top').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 1500, 'easeInOutExpo');
        return false;
    });


    // Initi AOS
    AOS.init({
        duration: 2000,
        easing: "ease-in-out",
        mirror: false
    });

});