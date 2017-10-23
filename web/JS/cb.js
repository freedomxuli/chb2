String.prototype.evalJSON = function () {
    try {
        return eval('(' + this + ')');
    } catch (e) { }
    throw '无效的 JSON 字符串: ' + this;
}



function showMask() {
    if (!window.smCount) {
        window.smCount = 1;
        Ext.getBody().mask('请稍候...');
    }
    else
        window.smCount++;
}


function hideMask() {
    if (window.smCount)
        if(!--window.smCount)
            Ext.getBody().unmask();
}

function _MultiCallServer(onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var reqs = [];
    for (var i = 2; i < arguments.length; i++) {
        var req = arguments[i];
        reqs[reqs.length] = { ctx: req.ctx, args: req.args || []};
    }
    showMask();
    Ext.Ajax.request({
        url: 'cse/sfwmcse',
        params: { args: JSON.stringify(reqs) },
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal);
                }
                else {
                    onError(rv.retVal);
                }
                hideMask();
            }
            catch (e) {
                hideMask();
                onError(e.description || e);
            }
        },
        failure: function (err) {
            hideMask();
            onError("发生未知错误，错误号:" + err.status);
        }
    });
}


function Inline_MultiCallServer(onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var reqs = [];
    for (var i = 2; i < arguments.length; i++) {
        var req = arguments[i];
        reqs[reqs.length] = { ctx: req.ctx, args: req.args || [] };
    }
    Ext.Ajax.request({
        url: 'cse/sfwmcse',
        async: false,
        params: { args: JSON.stringify(reqs) },
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal);
                }
                else {
                    onError(rv.retVal);
                }
            }
            catch (e) {
                onError(e.description || e);
            }
        },
        failure: function (err) {
            onError("发生未知错误，错误号:" + err.status);
        }
    });
}

function Inline_CallServer(context, onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var args = createParams(arguments);
    Ext.Ajax.request({
        url: 'cse/' + encodeURI(context),
        async: false,
        params: { args: args },
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal, context);
                }
                else {
                    onError(rv.retVal, context);
                }
            }
            catch (e) {
                onError(e.description || e, context);
            }
        },
        failure: function (err) {
            onError("发生未知错误，错误号:" + err.status, context);
        }
    });
}


function _CallServer(context, onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var args = createParams(arguments);
    //var myMask = new Ext.LoadMask(Ext.getBody(), {
    //    msg: "请稍候..."
    //});
    Ext.getBody().mask('请稍候...');
    Ext.Ajax.request({
        url: 'cse/' + encodeURI(context),
        params: { args: args },
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal, context);
                }
                else {
                    onError(rv.retVal, context);
                }
                Ext.getBody().unmask();
            }
            catch (e) {
                Ext.getBody().unmask();
                onError(e.description || e, context);
            }
        },
        failure: function (err) {
            Ext.getBody().unmask();
            onError("发生未知错误，错误号:" + err.status, context);
        }
    });
}


function CallServer(context, onReceiveData, onError) {
    var args = arguments;
    _CallServer.apply(this, args);
}





function _MultiAsyncCallServer(onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var reqs = [];
    for (var i = 2; i < arguments.length; i++) {
        var req = arguments[i];
        reqs[reqs.length] = { ctx: req.ctx, args: req.args || [] };
    }
    Ext.Ajax.request({
        url: 'cse/sfwmcse',
        params: { args: JSON.stringify(reqs) },
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal);
                }
                else {
                    onError(rv.retVal);
                }
            }
            catch (e) {
                onError(e.description || e);
            }
        },
        failure: function (err) {
            onError("发生未知错误，错误号:" + err.status);
        }
    });
}


function _AsyncCallServer(context, onReceiveData, onError) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var args = createParams(arguments);
    Ext.Ajax.request({
        url: '~/cse/' + encodeURI(context),
        params: { args: args },
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    onReceiveData(rv.retVal, context);
                }
                else {
                    onError(rv.retVal, context);
                }
            }
            catch (e) {
                onError(e.description || e, context);
            }
        },
        failure: function (err) {
            onError("发生未知错误，错误号:" + err.status, context);
        }
    });
}
function AsyncCallServer(context, onReceiveData, onError) {
    var args = arguments;
    _AsyncCallServer.apply(this, args);
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
    if (args.length > 3) {
        var arr = [];
        for (var i = 3; i < args.length; i++) {
            arr[i - 3] = args[i];
        }
        paramRet = JSON.stringify(arr);
    }
    return paramRet;
}



window.CS = CallServer;
window.ACS = AsyncCallServer;
window.MCS = function () {
    var args = arguments;
    _MultiCallServer.apply(this, args);
};
window.MACS = function () {
    var args = arguments;
    _MultiAsyncCallServer.apply(this, args);
};

window.InlineCS = Inline_CallServer;
window.InlineMCS = Inline_MultiCallServer;


CS.onError = function (err, context) {
    setTimeout(function () {
        Ext.MessageBox.show({
            title: "错误",
            //msg: "执行" + context + "发生错误\n" + err,
            msg: err.replace("\n", "<br/>"),
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.ERROR
        });
    }, 1);
};

function DownloadFile(context, fileName) {
    if (arguments.length < 2) {
        alert('无效的参数');
        return;
    }
    var args = [];
    args[0] = context;
    args[1] = function (err) { alert(err); };
    if(arguments.length > 2)
        for (var i = 1; i < arguments.length; i++)
            args[args.length] = arguments[i];
    DownloadFile2.apply(this, args);
}

function DownloadFile2(context, onError, fileName) {
    if (arguments.length < 3) {
        alert('无效的参数');
        return;
    }
    var args = createParamsForDownload(arguments);

    var fm = document.getElementById("sfw4_df_frame");
    if (!fm) {
        fm = document.createElement('iframe');
        fm.id = 'sfw4_df_frame';
        fm.src = 'about:blank';
        fm.style.display = 'none';
        document.body.appendChild(fm);
    }
    fm.src = 'about:blank';

    Ext.getBody().mask('请稍候，下载文件生成中...');
    Ext.Ajax.request({
        url: 'cse/' + encodeURI(context) + '/f/' + encodeURI(fileName),
        params: { args: args },
        timeout: 0,
        success: function (retValue) {
            try {
                var rv = retValue.responseText.evalJSON();
                if (rv.succeed == 1) {
                    var FilePath = rv.retVal;
                    setTimeout(function () {
                        fm.src = FilePath
                    }, 500);
                }
                else {
                    if (onError)
                        onError(rv.retVal, context);
                }
                Ext.getBody().unmask();
            }
            catch (e) {
                Ext.getBody().unmask();
                if (onError)
                    onError(e.description, context);
            }
        },
        failure: function (err) {
            Ext.getBody().unmask();
            if (onError)
                onError("发生未知错误，错误号:" + err.status, context);
        }
    });
}