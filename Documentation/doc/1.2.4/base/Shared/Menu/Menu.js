$(document).ready(function () {
    "use strict";

    var menu = $('#Menu');

    menu.children(".doctranMenu").doctranMenu({
        "toggleDuration": 150,
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