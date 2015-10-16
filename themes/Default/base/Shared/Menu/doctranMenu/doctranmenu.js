(function ($) {
    "use strict";
    $.fn.extend({
        doctranMenu: function (user_options) {

            var
                defaults = {
                    "toggleDuration": 0,
                    "recursiveClose": true,
                    "uniqueBranching": true,
                    "expanderOpen": "▼",
                    "expanderClosed": "▶",
                    "markTargeted": true,
                    "openActive": true,
                    "search": {
                        "formAttr": {},
                        "inputAttr": {
                            "type": "text",
                            "value": "Search...",
                            "autocomplete": "off"
                        },
                        filter: function (a, searchString) {
                            // Used for search, a regex with special characters escaped.
                            var r = new RegExp(searchString.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&"), 'i');

                            // Remember a.text() is all that is returned by "resultInfo".
                            return a.text().match(r);
                        },
                        "resultInfo": function (a) {
                            var containerA = a.parent().parent().parent().children("a, .title");
                            return containerA.length ? " (" + containerA.text() + ")" : "";
                        }
                    }
                },
            // Merge the users setting with the defaults.
                options = $.extend(true, defaults, user_options),
            // Methods
                createExpander = function () {
                    return $("<span>", {
                        "class": "expander"
                    }).mousedown(function (e) {
                        // Stop the expander's text being selected upon double clicking.
                        e.preventDefault();
                    }).hover(function () {
                        $(this).parent().toggleClass("hover");
                    }).append($("<a>", {
                        "text": options.expanderClosed
                    }));
                },
                scrollTo = function (plugin, item) {
                    plugin.scrollTop(item.offset().top - plugin.offset().top + plugin.scrollTop() - (plugin.height() / 2));
                },
                addExpanders = function (parent_ul) {

                    // Go though each list item which itself contains a list.
                    parent_ul.children("li.sublist").each(function () {
                        addExpanders($(this).children("ul"));
                    });

                    parent_ul.children("li:not(.sublist):has(ul)").each(function () {
                        // Store the current node.
                        var li_i = $(this),
                            expanderNode,
                            child_ul;

                        // Stop the user selecting text within the menu.
                        li_i.mousedown(function (e) {
                            e.preventDefault();
                        });

                        // Expanders are added when needed. So if needed, add one.
                        if (li_i.has(".expander").length === 0) {

                            // Get the child list and add expanders to its list items.
                            child_ul = li_i.children("ul");
                            addExpanders(child_ul);

                            // Creates an expander node and prepends it to the list element.
                            expanderNode = createExpander()
                                .click(function () {
                                    var expander = $(this),
                                        li = expander.parent();

                                    // Close others at same level.
                                    if (options.uniqueBranching) {
                                        li.siblings().children(".open.expander").click();
                                    }

                                    // When a parent is closed, close all its sub-items.
                                    if (options.recursiveClose && expander.is(".open")) {
                                        li.find("ul").find(".open.expander").click();
                                    }

                                    // Toggle class to add css effects.
                                    expander.toggleClass("open");
                                    if (options.markTargeted) {
                                        li.toggleClass("targeted");
                                        li.parent().closest("li:not(.sublist)").toggleClass("targeted");
                                    }

                                    // Do the opposite of what is expected as the is called before the
                                    // toggle. Must be called beforehand because of possible animation.
                                    expander.children("a").text(!child_ul.is(":visible") ? options.expanderOpen : options.expanderClosed);
                                    child_ul.slideToggle(options.toggleDuration);
                                });
                            li_i.prepend(expanderNode);
                        }
                        // Hide the next level's list ready for viewing.
                        li_i.children("ul").toggle();
                    });
                },
                openActive = function (plugin, activeLi) {
                    var toggleDuration = options.toggleDuration;

                    // If an active element is not specified then do nothing.
                    if (!activeLi.length) {
                        return;
                    }

                    // We don't want the animation to be run when the menu is first loaded.
                    options.toggleDuration = 0;

                    // Click the active list item's expander, as well as its parents.
                    activeLi.parents("li").andSelf().children(".expander").click();

                    // Set the animation to its previous value.
                    options.toggleDuration = toggleDuration;

                    // Scroll so that the active list item appears in the center of the menu.
                    scrollTo(plugin, activeLi);
                },
                createSearchResults = function (plugin, menuUl) {
                    var searchUl = $("<ul>", {
                            "class": "searchResults"
                        }),
                        listItems = {};

                    // Flatten the menu list and add it to the plugin.
                    menuUl.find("li>a").each(function () {
                        var a = $(this),
                            info,
                            newLink;

                        info = options.search.resultInfo(a);

                        newLink = a.clone().append($("<span>", {
                            "class": "resultInfo"
                        }).append(info));

                        listItems[a.prop("href")] = ($("<li>").hover(function () {
                            searchUl.children(".focused").removeClass("focused");
                        }).append(newLink));

                    });

                    searchUl.append($.map(listItems, function (li) {
                        return li;
                    }));

                    return searchUl.hide();
                },
                addSearchInput = function (plugin) {
                    var menuUl = plugin.children(".menu"),
                        searchUl = createSearchResults(plugin, menuUl),
                        searchForm = $("<form>", options.search.formAttr).submit(function (e) { // Prevent form submission is an item is in focus.
                            if (searchUl.children(".focused").length) {
                                e.preventDefault();
                                return false;
                            }
                        }).append($("<input>", options.search.inputAttr)
                            .focusin(function () {
                                var input = $(this);
                                input.toggleClass("active");
                                if (input.prop("value") === options.search.inputAttr.value) {
                                    input.prop("value", "");
                                }
                            }).focusout(function () {
                                var input = $(this);
                                input.toggleClass("active");
                                if (input.prop("value") === "") {
                                    input.prop("value", options.search.inputAttr.value);
                                }
                                searchUl.children(".focused").removeClass("focused");
                            })
                            // Check for arrow key up or down to move the focus up or down.
                            .keydown(function (e) {
                                var keyCode = e.keyCode || e.which,
                                    focused,
                                    move;

                                if (keyCode === 38 || keyCode === 40) {
                                    focused = searchUl.children(".focused");
                                    if (focused.length) {
                                        move = keyCode === 38 ? focused.prevAll("li:visible").first() : focused.nextAll("li:visible").first();
                                        focused.removeClass("focused");
                                        move.addClass("focused");
                                    } else {
                                        move = keyCode === 38 ? searchUl.children("li:visible:last") : searchUl.children("li:visible:first");
                                        move.addClass("focused");
                                    }
                                    scrollTo(plugin, move);
                                    e.preventDefault();
                                    return false;
                                }
                            })
                            // If:
                            // * Escape is pressed - then set the input's text to empty and show the normal menu.
                            // * Enter is pressed on a focused item - then go to the url of the focused item.
                            // * Any other key - Filter or re-filter the search results.
                            .keyup(function (e) {
                                var input = $(this),
                                    keyCode = e.keyCode || e.which,
                                    searchString = keyCode === 27 ? "" : input.prop("value"),
                                    focused;

                                if (keyCode === 13 && (focused = searchUl.children(".focused")).length) {
                                    window.location.replace(focused.children("a").prop("href"));
                                    return;
                                }

                                input.prop("value", searchString);
                                if (searchString === "") {
                                    menuUl.show();
                                    searchUl.hide();
                                } else {
                                    searchUl
                                        .children("li")
                                        .hide()
                                        .filter(function () {
                                            return options.search.filter($(this).children("a"), searchString);
                                        }).show();
                                    menuUl.hide();
                                    searchUl.show();
                                }
                            }));
                    plugin.prepend(searchForm);
                    plugin.append(searchUl);
                };

            return this.each(function () {

                var plugin = $(this),
                    menuUl = plugin.children("ul"),
                    activeLi = plugin.find(".active").first();

                // Tell the CSS to apply the javascript styling.
                plugin.addClass("jsEnabled");

                menuUl.addClass("menu");

                // Begin main plugin body
                addExpanders(menuUl);

                // If requested, open active and scroll to it.
                if (options.openActive) {
                    openActive(plugin, activeLi);
                }

                // If requested, make the menu searchable.
                if (options.search) {
                    addSearchInput(plugin);
                }

            });
        }
    });
}(jQuery));