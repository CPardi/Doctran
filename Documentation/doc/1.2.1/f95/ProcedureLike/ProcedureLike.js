
$(document).ready(function () {

    // Move the argument/result list below the call syntax headings.
    $("#CallSyntax .content .syntax").each(function () {
        $(this).find(".content").attr("id", $(this).attr("id")).insertAfter("#CallSyntax .content .syntax:last");
    });

    // Add the active class to the first call syntax heading.
    $("#CallSyntax .content h3:first").addClass("active");

    // Hide all of but the first of the syntax contents.
    $("#CallSyntax .content .content:not(:last)").hide();

    // Add some extra margin between the call syntax lines and the argument/result list.
    $("#CallSyntax .content .syntax:last").css("margin-bottom", "1em");

    // Add an event when a syntax title is clicked.
    $("#CallSyntax .content .syntax").each(
	function () {
	    $(this).find("h3").attr("onclick", "syntaxTitle_onclick('" + $(this).attr("id") + "')");
	})
})

function syntaxTitle_onclick(active_idname) {

    // Hide all syntax content.
    $("#CallSyntax .content .content").hide();

    // Store the active object.
    var active_obj = $("#CallSyntax .content #" + active_idname);

    // Show the syntax content corresponding to the clicked title.
    active_obj.show();

    // Remove the active class from what had it previously.
    $("#CallSyntax .content h3").removeClass("active");
    // Mark the clicked syntax heading as active.
    active_obj.find("h3").addClass("active");
}