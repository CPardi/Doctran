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

    var $Menu = $("#Menu");
    var $Menu_Heading = $Menu.children("#Heading");

    $Menu_Heading.find("h2").css("display", "inline-block");

    var $HiddenMenu = $("<div><span>▶<span></div>")
        .prop("id", "HiddenMenu")
        .css("margin-top", $Menu.css("margin-top"))
        .css("display", "none")
        .click(function () {
            $Menu.show();
            $HiddenMenu.hide();
            $("#ObjectContent").css("margin-left", "18em");
        });

    $HiddenMenu.insertAfter($Menu);

    var $hideSwitch = $("<a><span>◀</span></a>")
        .prop("id", "HideSwitch")
        .click(function () {
            $Menu.hide();
            $HiddenMenu.show();
            $("#ObjectContent").css("margin-left", "3.0em");
        });

    $Menu_Heading.append($hideSwitch);

    $("#Menu #Holder ul > .objectEntry")
        .prepend(function () {
            if ($(this).find("ul").text() !== "") {
                return $("<span class='objectExpand arrow'>▼</span>")
                    .css("cursor", "pointer").css("cursor", "hand")
                    .click(objectEntry_onclick)
                    .mousedown(function (e) {
                        e.preventDefault();
                    });
            }
        });

    $("#Menu #Holder ul > .subtitle")
        .prepend(function () {
            return $("<span class='subExpand arrow'>▼</span>")
                .css("cursor", "pointer").css("cursor", "hand")
                .click(subtitle_onclick)
                .mousedown(function (e) {
                    e.preventDefault();
                });
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