$(document).ready(function () {

    $(".section > h2").each(function () {

        var $this = $(this);

        var leave = "▼";
        var over = "▽";

        $this.css("padding-left", 0);

        var switcher = $("<span>" + leave + "</span>")
				.css("display", "inline-block")
                .css("width", "2em")
				.css("font-size", (0.6 * parseFloat($this.css("font-size"))).toString() + "px")
                .css("cursor", "pointer").css("cursor", "hand")
                .mouseover(function() { $(this).html(over); })
                .mouseleave(function() { $(this).html(leave); })
                .mousedown(function (e) { e.preventDefault(); })
				.click(function () {
				    var $switcher = $(this);
				    var $content = $this.parent().find(".content");
				    $content.hasClass("folded") ? showSection($switcher, $content) : hideSection($switcher, $content);
				});
        $this.prepend(switcher);

        function showSection(switcher, content) {
            over = "▽";
            leave = "▼";

            $(switcher).text(over);
            $(content).removeClass("folded").addClass("unfolded")
        }

        function hideSection(switcher, content) {
            over = "▷";
            leave = "▶";

            $(switcher).text(over);
            $(content).removeClass("unfolded").addClass("folded");
        }
    });
});