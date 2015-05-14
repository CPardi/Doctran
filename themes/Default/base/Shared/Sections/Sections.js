$(document).ready(function () {

    $(".section > h3").each(function () {

        var $this = $(this);

        $this.css("padding-left", 0);

        var switcher = $("<span>▼</span>")
				.css("margin-right", "0.5em")
				.css("font-size", (0.6 * parseFloat($this.css("font-size"))).toString() + "px")
                .css("cursor", "pointer").css("cursor", "hand")
                .mousedown(function (e) { e.preventDefault(); })
				.click(function () {
				    var $switcher = $(this);
				    var $content = $this.parent().find(".content")
				    $content.hasClass("folded") ? showSection($switcher, $content) : hideSection($switcher, $content);
				});
        $this.prepend(switcher);
    });

})

function showSection(switcher, content) {
    $(switcher).text("▼");
    $(content).removeClass("folded").addClass("unfolded")
}

function hideSection(switcher, content) {
    $(switcher).text("▶");
    $(content).removeClass("unfolded").addClass("folded");
}