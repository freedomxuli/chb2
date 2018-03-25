Ext.onReady(function () {
    dataBind();
});

function dataBind() {
    CS('CZCLZ.Handler.SelectModelByUser', function (retVal) {
        if (retVal) {
            window.location.href = "approot/r/page/zhidan/AddYunDanNew.html";
        } else {
            window.location.href = "approot/r/page/zhidan/AddYunDan.html";
        }
    },CS.onError);
}