
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(), //day 
        "h+": this.getHours(), //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
        "S": this.getMilliseconds() //millisecond 
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}



Date.prototype.toCHString = function (showTime) {
    //var s = this.getFullYear() + '年' + (this.getMonth() + 1) + '月' + this.getDate() + '日';
    //if (showTime !== false)
    //    s += ' ' + this.getHours() + ':' + this.getMinutes() + ':' + this.getSeconds();
    //return s;
    if (showTime !== false)
        return this.format('yyyy年MM月dd日 hh:mm:ss');
    else
        return this.format('yyyy年MM月dd日');
}
Date.prototype.toStdString = function (showTime) {
    //var s = this.getFullYear() + '-' + (this.getMonth() + 1) + '-' + this.getDate();
    //if (showTime !== false)
    //    s += ' ' + this.getHours() + ':' + this.getMinutes() + ':' + this.getSeconds();
    if (showTime !== false)
        return this.format('yyyy-MM-dd hh:mm:ss');
    else
        return this.format('yyyy-MM-dd');
}



function getEvent() {
    var p = getEvent.caller;
    while (p) {
        if (p.arguments[0] && p.arguments[0].stopPropagation)
            return p.arguments[0];
        p = p.caller;
    }
    return {};
}

if (!Ext.isIE) {
    window.__defineGetter__("event",
        function() {
            return getEvent();
        }
   );
}

function getXmlHttp() {
    try { return new XMLHttpRequest(); } catch (e) { }
    try { return new ActiveXObject("Msxml2.XMLHTTP.7.0"); } catch (e) { }
    try { return new ActiveXObject("Msxml2.XMLHTTP.6.0"); } catch (e) { }
    try { return new ActiveXObject("Msxml2.XMLHTTP.5.0"); } catch (e) { }
    try { return new ActiveXObject("Msxml2.XMLHTTP.4.0"); } catch (e) { }
    try { return new ActiveXObject("Msxml2.XMLHTTP.3.0"); } catch (e) { }
    try { return new ActiveXObject("Msxml2.XMLHTTP"); } catch (e) { }
    try { return new ActiveXObject("Microsoft.XMLHTTP"); } catch (e) { }
    return null;
}


function stopEvent() {
    var evt = window.event;
    if(evt){
        if (evt.cancelBubble != undefined)
            evt.cancelBubble = true;
        else if (evt.stopPropagation) {
            evt.stopPropagation();
        }
    }
}

var fTmpId = 0;
function createTempFun(fun) {
    window["tempFunSerId" + fTmpId++] = fun;
}



function loadScript(path, nocache, loaded) {
    var flag = false;
    var cb = function (evt) {
        evt = evt || window.event;
        me = evt.srcElement;
        if (loaded) {
            if (!me.readyState ||
                me.readyState == "loaded" || me.readyState == "complete") {
                if (!flag) {
                    flag = true;
                    loaded(path);
                }
            }
        }
    }
    try
    {
        if (window.isDebug && nocache !== false)
            path += (path.lastIndexOf('?') >= 0 ? '': '?') +  '&t=' + new Date().toJSON();    
        var script = document.createElement("script");
        script.setAttribute("src", path);
        script.setAttribute('type', 'text/javascript');
        script.onreadystatechange = script.onload = cb; 
        document.body.appendChild(script);
    }
    catch(ex)
    {
        var strLoaded = '';
        if (loaded) {
            var tfcb = createTempFun(cb);
            strLoaded = 'onreadystatechange="' + tfcb + '(window.event);" onload="' + tfcb + '(window.event);"';
        }
        document.write('<script type="text/javascript" src="' + path + '" ></' + 'script>');
    }
      
}

function inline_include(path) {
    if (!window.basePath)
        loadScript(path);
    else {
        var args = [];
        if (arguments.length > 1)
            for (var i = 1; i < arguments.length; i++)
                args[i - 1] = arguments[i];
        var args = JSON.stringify(args);
        var ts = new Date().toJSON();
        var initPath = basePath + path + ".Init?Init=" + encodeURIComponent(args) + '&t=' + ts;
        var http = getXmlHttp();
        http.open("GET", initPath, false);
        http.send();
        var initScript = http.responseText;
        if (initScript)
            eval(http.responseText);
        var jsPath = basePath + path;
        http.open("POST", jsPath, false);
        http.send(window.isDebug ? null: ts);
        var r = http.responseText;
        window.eval(r);
    }
}

function include(path, loaded) {
    
    if (!window.basePath)
        loadScript(path, true, loaded);
    else {
        var args = [];
        if (arguments.length > 1)
            for (var i = 1; i < arguments.length; i++)
                args[i - 1] = arguments[i];
        var args = JSON.stringify(args);
        var ts = new Date().toJSON();
        var initPath = basePath + path + ".Init?Init=" + encodeURIComponent(args) + '&t=' + ts;
        var http = getXmlHttp();
        http.onreadystatechange = function () {
            if (http.readyState == 4 && http.status == 200) {
                var initScript = http.responseText;
                if (initScript)
                    window.eval(http.responseText);
                var jsPath = basePath + path;
                loadScript(jsPath, true, loaded);
            }
        }
        http.open("GET", initPath, true);
        http.send();
    }
    //if(path.search(/^[a-z]+:\/\//) != -1)
    //    loadScript(path, true);
    //else{
    //    if (basePath)
    //        path = basePath + path;
    //    loadScript(path, true);
    //}
}

function appendQuery(path){
    if(path == null)
        return path;
    if(path.lastIndexOf('?') >= 0)
        if("".substr("".length, 1) == '&')
            return path
}


function SmartApp(conf) {
    if (conf.preLoad) {
        if (conf.preLoad.length) {
            var iLoad = 0;
            var fnLoad = function () {
                include(conf.preLoad[iLoad], function () {
                    if (iLoad == conf.preLoad.length - 1) {
                        if (conf.onReady)
                            conf.onReady(this);
                    }
                    else {
                        fnLoad();
                    }
                })
            }
            fnLoad();
        }
        else {
            if (conf.onReady)
                conf.onReady(this);
        }
    }
}

Ext.override(Ext.data.Store, {
    loadDataTable: function (dtb) {
        if (dtb.sfwtype!='datatable')
            throw "无效的DataTable类型";
        var cols = [];
        for (var i = 0; i < dtb.columns.length; i++)
            cols[i] = dtb.columns[i].ColumnName;
        this.model.setFields(cols);
        this.loadData(dtb.data);
    }
});