function open($subtitle, openSymbol) {
    $subtitle.text(openSymbol);
    var $subList = $subtitle.next().next();
    $subList.show();
}

function close($subtitle, closeSymbol) {
    $subtitle.text(closeSymbol);
    var $subList = $subtitle.next().next();
    $subList.hide();
}

function objectEntry_onclick() {
    var $subtitle = $(this);
    var $subList = $subtitle.next().next();
    if ($subList.is(":visible")) close($subtitle, "▶");
    else {
        close($subtitle.parent().parent().parent().parent().find(".objectExpand"), "▶");
        open($subtitle, "▼");
    }
}

function subtitle_onclick() {
    var $subtitle = $(this);
    var $subList = $subtitle.next().next();
    if ($subList.is(":visible")) close($subtitle, "▶");
    else {
        close($subtitle.parent().parent().parent().parent().find(".subExpand"), "▶");
        open($subtitle, "▼");
        }
}

$(document).ready(function () {
    $("#Menu #Holder ul > .objectEntry")
		.prepend(function () {
		    if ($(this).find("ul").text() !== "") {
		        return $("<span class='objectExpand arrow'>▼</span>").click(objectEntry_onclick);
		    }
		});

    $("#Menu #Holder ul > .subtitle")
		.prepend(function () {
		    return $("<span class='subExpand arrow'>▼</span>").click(subtitle_onclick);
		});

    $(".objectExpand").each(function () {
        close($(this), "▶")
    });

    $(".subExpand:not(:first)").each(function () {
        close($(this), "▶")
    });

    $(".breadcrumbs > a").each(function () {
        open($(this).prev(), "▼");
        var $subtitle = $(this).parent().parent().parent().children(".subExpand");
        open($subtitle, "▼");
    });

    $("#Menu #Holder").scrollTop($(".active").offset().top - $("#Holder").offset().top);
})