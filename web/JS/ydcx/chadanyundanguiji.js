﻿var UserID = queryString.UserID;
var YunDanDenno = queryString.YunDanDenno;

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
                                handler: function () {

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
}