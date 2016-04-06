$(document).ready(function () {
    "use strict";

    var menu = $('#Menu');

    $(".doctran-menu").doctranMenu({
        "toggleDuration": 150,
        "hideDuration": 300,
        "showHide": {
            "toggleDuration": 300,
            "appendTo": "#show-hide",
            "onShow": function() {
                $("#Page>.mainContainer").animate({left: '-=21em'}, 300)
            },
            "onHide": function() {
                $("#Page>.mainContainer").animate({left: '+=21em'}, 300)
            }
        },
        "search": {
            "inputAttr": {
                "id": "tipue_search_input",
                "name": "q"
            },
            "formAttr": {
                "method": "GET",
                "action": globals.prefix + "html/Navigation/Search.html"
            },
            "resultInfo": function (a) {
                var typeString = a.data("type"),
                    containerA;

                if (typeString) {
                    return "<br/>Type: " + typeString
                        + "<br/>Parent: " + a.data("parent");
                } else {
                    containerA = a.parent().parent().closest("li:not(.sublist)").children("a, .title");
                    return containerA.length ? " (" + containerA.text() + ")" : "";
                }
            }
        }
    });
});