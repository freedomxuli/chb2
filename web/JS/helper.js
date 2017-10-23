//为Firefox的window提供event属性

function getEvent() {
    var p = getEvent.caller;
    while (p) {
        if (p.arguments[0] && p.arguments[0].stopPropagation)
            return p.arguments[0];
        p = p.caller;
    }
    return {};
}

if (!$.browser.msie) {
    window.__defineGetter__("event",
        function() {
            return getEvent();
        }
   );
}
//停止时间向上传递
function stopEvent() {
    var evt = window.event;
    if (evt) {
        if (evt.cancelBubble != undefined)
            evt.cancelBubble = true;
        else if (evt.stopPropagation) {
            evt.stopPropagation();
        }
    }
}
//扩展字符串方法
var regexHtml = /[<> &\\"'\n]/g;
regexHtml.compile = true;
$.extend(String.prototype, {
    isNumeric: function() {
        return !!this.match("^[0-9]+$");
    },

    left: function(n) {
        if (this.length <= n) return this;
        return this.substr(0, n);
    },

    right: function(n) {
        if (this.length <= n) return this;
        return this.substr(this.length - n - 1, n);
    },

    trim: function() {
        //return   this.replace(/(^\s*)|(\s*$)/g,   "");
        return $.trim(this);
    },

    htmlEncode: function() {
        if (this.length == 0) return "";
        return this.replace(regexHtml, function(c) {
            switch (c) {
                case "<": return "&lt;";
                case ">": return "&gt;";
                case "&": return "&amp;";
                case " ": return "&nbsp;";
                case "\n": return "<br>";
                case "\"": return "&quot;";
                case "'": return "&#39;";
            }
        })
    },
    attrEncode: function() {
        return this.htmlEncode();
        //        var s = "";
        //        if (this.length == 0) return "";
        //        for (var i = 0; i < this.length; i++) {
        //            switch (this.substr(i, 1)) {
        //                case "\"": s += "&quot;"; break;
        //                case "'": s += "&#39;"; break;
        //                default: s += this.substr(i, 1); break;
        //            }
        //        }
        //        return s;
    },

    htmlDecode: function() {
        var dv = document.createElement("div");
        dv.innerHTML = "<input value='" + this + "'>";
        return dv.childNodes[0].value;
    },


    blank: function() {
        return /^\s*$/.test(this);
    },

    evalJSON: function() {
        try {
            return eval('(' + this + ')');
        } catch (e) { }
        throw '无效的 JSON 字符串: ' + this;
    }
});


