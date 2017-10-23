var now_date = new Date();

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
                        layout: {
                            type: 'fit'
                        },
                        id: 't_show',
                        height:document.documentElement.clientHeight,
                        html:""
                    }
                ]
            });
            me.callParent(arguments);
        }
    });
    new MainView();

    dataBind();
});

function dataBind()
{
    CS("CZCLZ.DeskTop.Tshow", function (retVal) {
        if (retVal)
        {
            Ext.getCmp("t_show").update(retVal);
            addListen();
        }
    }, CS.onError, now_date)
}

function selecttime() {
    var win = new TimeWin();
    win.show(null, function () {
        Ext.getCmp("CheckTime").setValue(now_date);
    });
}

function addListen() {
    var ydata = Ext.query("#yd-data td");
    for (var i = 0; i < ydata.length; i++) {
        Ext.EventManager.addListener(ydata[i], "click", function () {
            if ($(this).children("input")[1].value == 0)
            {
                var isCanClose = $(this).children("input")[2].value;
                var win = new Buttons({ roomid: $(this).children("input")[0].value });
                win.show(null, function () {
                    Ext.getCmp("AddOrder").show();
                    Ext.getCmp("ShowRoom").show();
                    if (isCanClose == 0)
                        Ext.getCmp("CloseRoom").show();
                });
            } else if ($(this).children("input")[1].value == 1)
            {
                var win = new Buttons();
                win.show(null, function () {
                    Ext.getCmp("ShowRoom").show();
                    Ext.getCmp("ShowOrder").show();
                });
            } else
            {
                var win = new Buttons({ roomid: $(this).children("input")[0].value });
                win.show(null, function () {
                    Ext.getCmp("ShowRoom").show();
                    Ext.getCmp("OpenRoom").show();
                });
            }
        });
    }
}

//弹出框
Ext.define('TimeWin', {
    extend: 'Ext.window.Window',

    height: 244,
    width: 285,
    layout: {
        type: 'fit'
    },
    title: '选择时间',
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    items: [
                        {
                            xtype: 'datefield',
                            id:"CheckTime",
                            format: 'Y-m-d',
                            editable: false,
                            padding: 10,
                            labelWidth:40,
                            fieldLabel: '时间'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确定',
                            handler: function () {
                                now_date = Ext.getCmp("CheckTime").getValue();
                                dataBind();
                                me.close();
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('Buttons', {
    extend: 'Ext.window.Window',

    width: 167,
    layout: {
        type: 'anchor'
    },
    title: '操作',

    modal: true,
    id: 'cz',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'panel',
                    dock: 'top',
                    layout: {
                        align: 'center',
                        pack: 'center',
                        type: 'vbox'
                    },
                    items: [
                        {
                            xtype: 'button',
                            flex: 1,
                            margin: 10,
                            id: 'AddOrder',
                            hidden:true,
                            text: '新增授权'
                        },
                        {
                            xtype: 'button',
                            flex: 1,
                            margin: 10,
                            id: 'ShowRoom',
                            hidden: true,
                            text: '房间信息'
                        },
                        {
                            xtype: 'button',
                            flex: 1,
                            margin: 10,
                            id: 'ShowOrder',
                            hidden: true,
                            text: '订单信息'
                        },
                        {
                            xtype: 'button',
                            flex: 1,
                            margin: 10,
                            id: 'CloseRoom',
                            hidden: true,
                            text: '设置关房',
                            handler: function () {
                                CS('CZCLZ.DeskTop.SetCloseRoom', function (retVal) {
                                    if (retVal)
                                    {
                                        Ext.Msg.alert("提示", "关房成功！", function () {
                                            dataBind();
                                            me.close();
                                        });
                                    }
                                }, CS.onError, me.roomid);
                            }
                        },
                        {
                            xtype: 'button',
                            flex: 1,
                            margin: 10,
                            id: 'OpenRoom',
                            hidden: true,
                            text: '设置开房',
                            handler: function () {
                                CS('CZCLZ.DeskTop.SetOpenRoom', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "开房成功！", function () {
                                            dataBind();
                                            me.close();
                                        });
                                    }
                                }, CS.onError, me.roomid);
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});



