! function (t) {
    "use strict";
    t("#sidebarToggle, #sidebarToggleTop").on("click", function (o) {
        t("html").removeClass("nav-open"), t("#close-layer visible").removeClass("close-layer visible")
    })
}(jQuery);