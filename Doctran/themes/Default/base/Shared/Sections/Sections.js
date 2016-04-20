$(document).ready(function () {

    $(".section > h2").each(function () {

        var heading = $(this),
            content = heading.parent().find(".content"),
            hide = "▼",
            show = "▶";

        heading.css("padding-left", 0);

        var switcher = $("<span class='expander'>" + hide + "</span>")
                .mousedown(function (e) { e.preventDefault(); })
				.click(function () {
                    $(content).toggleClass("folded");
                    $(this).text(content.hasClass("folded") ? show : hide);
				});
        heading.prepend(switcher);
    });
});