
var fnZoom = function (dw) {
    if (document.body) {
        var iw = $(window).width(); // document.documentElement.clientWidth;
        var r = iw / dw;
        if (document.body.style.zoom != r)
            document.body.style.zoom = r;
    }
};

fnZoom(480);
setInterval(function () {
    fnZoom(480);
}, 50);