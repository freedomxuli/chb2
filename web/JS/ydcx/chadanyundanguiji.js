var UserID = queryString.UserID;
var YunDanDenno = queryString.YunDanDenno;
var type = queryString.type;

Ext.onReady(function () {
    Ext.define('MainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'panel',
                        title: '运单轨迹',
                        id:'YDGJ',
                        html:'<iframe width=100% height=100% frameborder=0 scrolling=auto src="http://www.baidu.com"></iframe>',
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '解除绑定',
                                iconCls: 'close',
                                id:'jcbd',
                                handler: function () {
                                    Ext.Msg.confirm("提示", "是否解绑该设备?", function (btn) {
                                        if (btn == "yes") {
                                            CS('CZCLZ.Handler.CloseBD', function (retVal) {
                                                if (retVal) {
                                                    Ext.Msg.alert("提示", "解除绑定成功！", function () {
                                                        FrameStack.popFrame();
                                                    });
                                                }
                                            }, CS.onError, UserID, YunDanDenno);
                                        }
                                    });
                                }
                            },
                            {
                                text: '返回',
                                iconCls: 'back',
                                handler: function () {
                                    FrameStack.popFrame();
                                }
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new MainView();

    DataBind();

});

function DataBind()
{
    var src = "http://chb.yk56.net/Map?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno;

    Ext.getCmp('YDGJ').update("<iframe width=100% height=100% frameborder=0 scrolling=auto src='" + src + "'></iframe>");

    if (type == "ziyouchadan") {
        Ext.getCmp("jcbd").hide();
    } else {
        CS('CZCLZ.Handler.SearchBD', function (retVal) {
            if(!retVal)
            {
                Ext.getCmp("jcbd").hide();
            }
        }, CS.onError, UserID, YunDanDenno)
    }
}