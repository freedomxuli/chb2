/// <reference path="DeskTop.js" />
/// <reference path="DeskTop.js" />

var now;
function gettime() {
    var date = new Date();
    now = date.getFullYear() + "-";
    now = now + (date.getMonth() + 1) + "-"; //取月的时候取的是当前月-1如果想取当前月+1就可以了
    now = now + date.getDate() + " ";
    now = now + date.getHours() + ":";
    now = now + date.getMinutes() + ":";
    now = now + date.getSeconds() + "";
    document.getElementById("CurrentTime").innerText = now;
    setTimeout("gettime()", 1000)
}
function suojin() {
    Ext.getCmp("MainHeader").setVisible(false);
}

inline_include("approot/r/js/UserMag/SetLP.js");

function showSZ() {
    var win = new SBTypeSet();
    win.show(null, function () {
        getType();
    });
}

//****************************************************************************************************************************
Ext.define('Sbxq', {
    extend: 'Ext.window.Window',

    height: 235,
    modal: true,
    width: 450,
    layout: {
        type: 'fit'
    },
    title: '系统服务中心',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                    {
                        xtype: 'panel',
                        autoScroll: true,
                        layout: {
                            type: 'border'
                        },
                        items: [
                            {
                                xtype: 'panel',
                                height: 200,
                                width: 444,
                                html: "<iframe frameborder='0' src='approot/r/js/Main/index.html' width='100%' height='100%'></iframe>"
                            }
                        ]
                    }
            ]
        });

        me.callParent(arguments);
    }
});

function tc() {
    var win = new Sbxq();
    win.show();
}

//***************************************************************************************************************************************************************
Ext.onReady(function () {
    Ext.define('MainViewport', {
        extend: 'Ext.container.Viewport',
        padding: '0 5 5 5',
        layout: {
            type: 'border'
        },

        initComponent: function () {
            var me = this;
            me.items = [
            {
                height: 85,
                xtype: 'component',
                region: 'north',
                id: 'MainHeader',
                collapsible: true,

                html: '<div class="LoginHeader"><div class=leftHeader><a href="" target="mainframe"><img alt="平价菜篮子服务平台" src="../images/logo/' + logo + '"></a></div><div class="rightHeader"><a href="" target="mainframe"><img align="absmiddle" src="approot/d/images/home.png">首页</a><a href="approot/r/help.aspx"><img align="absmiddle" src="approot/d/images/help.png">帮助</a><a onclick="logout()" href="javascript:void(0);"><img align="absmiddle" src="approot/d/images/exit.png">退出</a></div></div>'
                + '<div class="LoginInfo" style="height:24px;line-height:24px;">' + strHelp + '　　欢迎您：' + UserName + '　　　　当前时间：<span id="CurrentTime"></span></div>'
            },
            {
                xtype: 'panel',
                minWidth: 150,
                width: 225,
                layout: {
                    type: 'accordion'
                },
                id: 'MainMenu',
                collapsible: true,
                title: '菜单',
                region: 'west',
                padding: 2,
                items: itemmsg.evalJSON()
            },
            {
                xtype: 'panel',
                minWidth: 300,
                region: 'center',
                border: false,
                padding: 2,
                id: 'MainPanel',
                html: '<iframe name="mainframe" id="mainframe" frameborder="0" src="page/Main/desktop.html" width="100%" height="100%">'
            }
            ];
            me.callParent(arguments);//../JS/Main/DeskTop.aspx
        }

    });
    new MainViewport();
    gettime();
    Ext.getCmp('MainMenu').getEl().select('.MenuItem').on('click', function () {
        var iframe = document.getElementById("mainframe");
        if (iframe.attachEvent) {
            iframe.attachEvent("onload", function () {
                if (window.parent.myMask) {
                    window.parent.myMask.hide();
                }
            });
        } else {
            iframe.onload = function () {
                if (window.parent.myMask) {
                    window.parent.myMask.hide();
                }
            };
        }
        if (this.href != 'javascript:void(0)') {
            if (!window.myMask) {
                window.myMask = new Ext.LoadMask(Ext.getCmp('MainPanel'), 'loading...');
            }
            window.myMask.show();
        }
    });
});
function logout() {
    CS('CZCLZ.UserClass.Logout', function (retVal) {
        if (retVal)
            window.location.href = 'approot/r/login.aspx';
    }, CS.onError);
}