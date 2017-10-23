function _CallServer(context, onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var args = createParams(arguments);
    $.blockUI({
        centerY: 0,
        css: { top: '10px', left: '', right: '10px', border: 'none', width: 'auto', backgroundColor: '#FFFFAA', padding: '2px' },
        overlayCSS: { opacity: 0.01, filter: 'alpha(opacity=0.1)', backgroundColor: '#FFFFFF' },
        baseZ: 100000,
        message: '<img src="approot/d/images/ajax-loader.gif" align="middle"/>　服务器正在处理...'
    });
    $.ajax({
        url: 'cse/' + context,
        data: { args: args },
        success: function(retValue) {
            try {
                var rv = retValue.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal, context);
                }
                else {
                    onError(rv.retVal, context);
                }
                $.unblockUI();
            }
            catch (e) {
                onError(e.description, context);
                $.unblockUI();
            }
        },
        error: function(err) {
            onError("发生未知错误，错误号:" + err.status, context);
            $.unblockUI();
        }
    });
}
function CallServer(context, onReceiveData, onError) {
    var args = arguments;
    setTimeout(function() { _CallServer.apply(this, args); }, 1);
}

function _AsyncCallServer(context, onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var args = createParams(arguments);
    $.ajax({
        url: 'cse/' + context,
        data: { args: args },
        success: function(retValue) {
            try {
                var rv = retValue.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal, context);
                }
                else {
                    onError(rv.retVal, context);
                }
            }
            catch (e) {
                onError(e.description, context);
            }
        },
        error: function(err) {
            onError("发生未知错误，错误号:" + err.status, context);
        }
    });
}
function AsyncCallServer(context, onReceiveData, onError) {
    var args = arguments;
    setTimeout(function() { _AsyncCallServer.apply(this, args); }, 1);
}
function createParams(args) {
    var paramRet = [];
    if (args.length > 3) {
        var arr = [];
        for (var i = 3; i < args.length; i++) {
            arr[i - 3] = args[i];
        }
        paramRet = JSON.stringify(arr);
    }
    return paramRet;
}
function createParamsForUpload(args) {
    var paramRet = [];
    if (args.length > 0) {
        var arr = args;
        paramRet = JSON.stringify(arr);
    }
    return paramRet;
}

function createParamsForDownload(args) {
    var paramRet = [];
    if (args.length > 1) {
        var arr = [];
        for (var i = 2; i < args.length; i++) {
            arr[i - 2] = args[i];
        }
        paramRet = JSON.stringify(arr);
    }
    return paramRet;
}


window.CS = CallServer;
window.ACS = AsyncCallServer;

CS.onError = function(err, context) {
    setTimeout(function() {
        alert(err);
    }, 1);
};