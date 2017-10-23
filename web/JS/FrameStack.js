var FrameStack = {};
FrameStack.pushFrame = function (url) {
    var onClose = null;
    var param = null;
    var title = null;
    var showBackbutton = null;
    if (Ext.typeOf(url) == 'object') {
        var p = url;
        url = p.url;
        param = p.param;
        title = p.title;
        showBackbutton = p.showBackbutton;
        onClose = p.onClose;
    }

    if (arguments.length > 1) {
        if (!Ext.isFunction(arguments[1]))
            param = arguments[1];
        else
            onClose = arguments[1];
    }
    if (!onClose && arguments.length > 2)
        onClose = arguments[2];

    var winDom = window;
    var mw = FrameStack.getMainWindow();
    var orginOverflow = mw.document.body.style.overflow;
    mw.document.body.style.overflow = "hidden";
    var sz = mw.Ext.getBody().getViewSize();
    var win = new mw.Ext.container.Container({
        resizable: false,
        //header: false,
        modal: true,
        floating: true,
        border: false,
        //closable: false,
        //draggable: false,
        width: sz.width,
        height: sz.height,
        shadow: false,
        //maximized: true,
        onWindowResize: function (w, h) {
            win.setSize(w, h);
        },
        listeners: {
            beforedestroy: function () {
                mw.Ext.EventManager.removeResizeListener(this.onWindowResize, this);
            },
            destroy: function () {
                if (onClose)
                    onClose.call(winDom, this.retVal);
                mw.document.body.style.overflow = orginOverflow;
            },
            show: function () {
                mw.Ext.EventManager.onWindowResize(this.onWindowResize, this, { delay: 1 });
                var f = win.getEl().down('iframe');
                f.dom.pw = this;
                this.param = param;
                if (showBackbutton) {
                    mw.Ext.fly(win.el.query('.backbt')[0]).on('click', function () {
                        win.destroy();
                    });
                }

            }
        },
        layout: 'fit',
        items: [
            {
                xtype: 'panel',
                title: (showBackbutton ? '<img src="approot/d/images/bbb.png" width=16 style="vertical-align:bottom;cursor:pointer; margin-right:10px; width:82px; height:24px; float:left;" class="backbt"/>' : ""),
                border: false
            }
        ]

    });

    win.down('panel').update('<iframe frameborder="0" src="' + url + '" style="width:100%;height:100%" />');
    win.show();
}

FrameStack.popFrame = function (retVal) {
    var pw = FrameStack.getSelfPushFrameWindow();
    var mywin = window;
    if (pw) {
        var parentFrame = FrameStack.findParentIFrame(window);
        var parentWindow = parentFrame.parentWindow;
        parentWindow.setTimeout(function () {
            pw.retVal = retVal;
            mywin.location.href = 'about:blank';
            var clearProxy = parentWindow.eval('({clear: function(pw){ return function(){ pw.destroy(); }; } })');
            parentWindow.setTimeout(clearProxy.clear(pw), 1);

        }, 1);
        return true;
    }
    return false;
}

FrameStack.isInFrameStack = function () {
    var pf = FrameStack.findParentIFrame(window);
    return pf && pf.pw;
}

FrameStack.getParam = function () {
    var pw = FrameStack.getSelfPushFrameWindow();
    return pw.param;
}

FrameStack.findParentIFrame = function (win) {
    if (win.parent == win)
        return null;
    var arrFrames = win.parent.document.getElementsByTagName("IFRAME");
    for (var i = 0; i < arrFrames.length; i++) {
        if (arrFrames[i].contentWindow == win) {
            arrFrames[i].parentWindow = win.parent;
            return arrFrames[i];
        }

        //if (arrFrames[i].pw)
        //    return arrFrames[i];
        //else
        //    return null;
    }
}

FrameStack.getSelfPushFrameWindow = function () {
    var pf = FrameStack.findParentIFrame(window);
    if (pf && pf.pw)
        return pf.pw;
};


FrameStack.getMainWindow = function () {
    var curWin = window;
    if (curWin.isFSMainWin)
        return curWin;
    while (true) {
        var parentFrame = FrameStack.findParentIFrame(curWin);
        if (parentFrame)
            if (parentFrame.parentWindow.isFSMainWin)
                return parentFrame.parentWindow;
            else
                curWin = parentFrame.parentWindow;
        else
            return window;
    }
    return window;
}