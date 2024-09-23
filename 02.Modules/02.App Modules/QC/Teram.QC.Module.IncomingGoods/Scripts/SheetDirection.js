$(document).on("click", "#btnView", function () {

    var firstDimension = parseFloat($("#FirstDimension").val());
    var sendondDimension = parseFloat($("#SecondDimension").val());


    if (firstDimension < sendondDimension) {

        $("#ImgHelp").attr("src", "/Images/Sheets/LetfAndRightArrow.png");
        $("#ImgHelp").removeClass("d-none");
    }
    else {

        $("#ImgHelp").attr("src", "/Images/Sheets/UpAndDownArrow.png");
        $("#ImgHelp").removeClass("d-none");

    }
});