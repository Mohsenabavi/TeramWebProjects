$("#steps li").each(function (i, c) {
    if (i < step)
        $(this).addClass("passed");
    else if (i == step)
        $(this).addClass("active");
    else $(this).addClass("disabled");
});

if (window.url) {
    $.get("/" + url, r => {
        $("#container").html(r);
    });
}