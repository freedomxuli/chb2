var mxStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsTuiDanMingXiTime', 'GpsDeviceID', 'GpsTuiDanMingXiID'
    ]
});

var bankStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsTuiDanZhangHao'
    ]
});

var StartMessage;

var OrderDenno = "";

var GpsTuiDanJinE = "";

var yanzhengma = "";

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
                            type: 'anchor'
                        },

                        items: [
                            {
                                xtype: 'panel',
                                height: document.documentElement.clientHeight / 4 + 100,
                                layout: {
                                    align: 'center',
                                    type: 'vbox'
                                },
                                items: [
                                    {
                                        xtype: 'panel',
                                        flex: 1,
                                        width: 471,
                                        layout: {
                                            type: 'column'
                                        },
                                        border: 0,
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 1,
                                                id: 'HiddenID',
                                                fieldLabel: '隐藏设备号',
                                                margin: '-25 0 0 0',
                                                listeners: {
                                                    'change': function (field, newValue, oldValue) {
                                                        Ext.getCmp("GpsDeviceID").setValue(newValue);
                                                    }
                                                }
                                            },
                                            {
                                                xtype: 'label',
                                                columnWidth: 1,
                                                html: "<div style='color:red;'>打开页面即可扫描枪扫码，如果获取扫描枪获取不到数据，请点击重置设备号再次尝试！</div>",
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 1,
                                                padding: 20,
                                                id: 'GpsDeviceID',
                                                readOnly: true,
                                                fieldLabel: 'gps设备号'
                                            },
                                            {
                                                xtype: 'container',
                                                columnWidth: 1,
                                                items: [
                                                    {
                                                        xtype: 'button',
                                                        margin: '50 0 20 130',
                                                        iconCls: 'enable',
                                                        text: '确认',
                                                        style: 'padding:15px;',
                                                        handler: function () {
                                                            if (Ext.getCmp("GpsDeviceID").getValue() != "" && Ext.getCmp("GpsDeviceID").getValue() != null) {
                                                                var n = Ext.getCmp("GpsDeviceID").getValue().indexOf("1919");
                                                                if (n == 0) {
                                                                    CS('CZCLZ.Handler.TuiDanDeciceIsClose', function (ret) {
                                                                        if (ret) {
                                                                            CS('CZCLZ.Handler.AddTuiDanGPS', function (retVal) {
                                                                                if (retVal.sign == "true") {
                                                                                    OrderDenno = retVal.OrderDenno;
                                                                                    Ext.getCmp("GpsDeviceID").setValue("");
                                                                                    Ext.Msg.alert("提示", "添加成功！", function () {
                                                                                        dataBind();
                                                                                    });
                                                                                } else {
                                                                                    Ext.getCmp("GpsDeviceID").setValue("");
                                                                                    Ext.Msg.alert("提示", retVal.msg, function () {
                                                                                        dataBind();
                                                                                    });
                                                                                }
                                                                            }, CS.onError, Ext.getCmp("GpsDeviceID").getValue());
                                                                        } else {
                                                                            Ext.Msg.alert("提示", "请先解绑该设备，再退单！", function () {
                                                                                dataBind();
                                                                            });
                                                                        }
                                                                    }, CS.onError, Ext.getCmp("GpsDeviceID").getValue());
                                                                }
                                                                else {
                                                                    Ext.Msg.alert("提示", "请扫描指定二维码！");
                                                                    return false;
                                                                }
                                                            }
                                                            else {
                                                                Ext.Msg.alert("提示", "请先添加设备号！");
                                                                return false;
                                                            }
                                                        }
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        margin: '50 0 20 90',
                                                        iconCls: 'close',
                                                        text: '重置设备号',
                                                        style: 'padding:15px;',
                                                        handler: function () {
                                                            Ext.getCmp("GpsDeviceID").setValue("");
                                                            Ext.getCmp("HiddenID").setValue("");
                                                            Ext.getCmp('HiddenID').focus(true, true);
                                                        }
                                                    }
                                                ]
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'gridpanel',
                                height: (document.documentElement.clientHeight / 4) * 3,
                                border: 1,
                                columnLines: 1,
                                store: mxStore,
                                columns: [
                                    Ext.create('Ext.grid.RowNumberer'),
                                    {
                                        xtype: 'datecolumn',
                                        dataIndex: 'GpsTuiDanMingXiTime',
                                        flex: 1,
                                        sortable: false,
                                        menuDisabled: true,
                                        format: 'Y-m-d',
                                        text: '日期'
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'GpsDeviceID',
                                        flex: 1,
                                        sortable: false,
                                        menuDisabled: true,
                                        text: '设备标识'
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 1,
                                        sortable: false,
                                        menuDisabled: true,
                                        text: '操作',
                                        dataIndex: 'GpsTuiDanMingXiID',
                                        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                            return "<a href='javascript:void(0);' onclick='del(\"" + value + "\");'>删除</a>";
                                        }
                                    }
                                ]
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '申请退单',
                                iconCls: 'enable',
                                style: 'padding:15px;',
                                handler: function () {
                                    if (OrderDenno != "") {
                                        CS('CZCLZ.Handler.TJTD', function (retVal) {
                                            if (retVal) {
                                                if (retVal.sign == "true") {
                                                    GpsTuiDanJinE = retVal.GpsTuiDanJinE;
                                                    var win = new tuidan({ OrderDenno: OrderDenno });
                                                    win.show(null, function () {
                                                        Ext.getCmp("GpsTuiDanJinE").setValue(GpsTuiDanJinE);
                                                    });
                                                } else {
                                                    Ext.Msg.alert("提示", retVal.msg);
                                                    dataBind();
                                                    return false;
                                                }
                                            }
                                        }, CS.onError, OrderDenno)
                                    }
                                    else {
                                        Ext.Msg.alert("提示", "请先生成退单！");
                                        dataBind();
                                        return false;
                                    }
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

    dataBind();
});

function dataBind()
{
    CS('CZCLZ.Handler.GetTuiDanGPS', function (retVal) {
        if (retVal) {
            mxStore.loadData(retVal.dt);
            OrderDenno = retVal.OrderDenno;
            Ext.getCmp("HiddenID").setValue("");
            Ext.getCmp('HiddenID').focus(true, true);
        }
    }, CS.onError)
}

function del(GpsTuiDanMingXiID) {
    CS('CZCLZ.Handler.DeleteTDItem', function (retVal) {
        if (retVal) {
            if (retVal.sign == "true") {
                Ext.Msg.alert("提示", "删除成功！", function () {
                    dataBind();
                });
            } else {
                Ext.Msg.alert("提示", retVal.msg);
                return false;
            }
        }
    }, CS.onError, GpsTuiDanMingXiID)
}

Ext.define('tuidan', {
    extend: 'Ext.window.Window',

    height: 400,
    width: 350,
    layout: {
        type: 'fit'
    },
    title: '退单',

    modal: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'column'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: '退款金额',
                            labelWidth: 70,
                            allowDecimals: false,
                            columnWidth: 1,
                            readOnly: true,
                            id: 'GpsTuiDanJinE',
                            padding: '50 10 50 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '退款卡号',
                            labelWidth: 70,
                            columnWidth: 0.8,
                            id: 'GpsTuiDanZhangHao',
                            padding: '0 10 50 10'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'add',
                            columnWidth: 0.2,
                            margin: '0 10 50 0',
                            text: '选择',
                            handler: function () {
                                var win = new PickBank();
                                win.show(null, function () {
                                    CS('CZCLZ.Handler.PickBank', function (retVal) {
                                        if (retVal)
                                        {
                                            bankStore.loadData(retVal);
                                        }
                                    },CS.onError)
                                });
                            }
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '验证码',
                            labelWidth: 70,
                            columnWidth: 0.8,
                            id: 'yanzhengma',
                            padding: '0 10 50 10'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'add',
                            columnWidth: 0.2,
                            margin: '0 10 50 0',
                            text: '发送',
                            id: 'addMessages',
                            handler: function () {
                                yanzhengma = "";
                                CS('CZCLZ.Handler.SendMessage', function (retVal) {
                                    if(retVal)
                                    {
                                        if (retVal.sign = "true") {
                                            yanzhengma = retVal.yanzhengma;
                                            Ext.Msg.alert("提示", "已成功发送短信！");
                                            StartSendMessage();
                                            Ext.getCmp("addMessages").hide();
                                            Ext.getCmp("sj").show();
                                            return;
                                        } else {
                                            Ext.Msg.alert("提示", retVal.msg);
                                            return false;
                                        }
                                    }
                                }, CS.onError, 'tuidanshenqing');
                            }
                        },
                        {
                            xtype: 'label',
                            id: 'sj',
                            columnWidth: 0.2,
                            margin: '0 10 50 0',
                            hidden: true,
                            html: '60s'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认申请',
                            iconCls: 'enable',
                            handler: function () {
                                if (me.OrderDenno!="")
                                    OrderDenno = me.OrderDenno;
                                if (yanzhengma != Ext.getCmp("yanzhengma").getValue())
                                {
                                    Ext.Msg.alert("提示", "验证码错误！");
                                    return false;
                                }
                                if(Ext.getCmp("GpsTuiDanZhangHao").getValue()==""||Ext.getCmp("GpsTuiDanZhangHao").getValue()==null)
                                {
                                    Ext.Msg.alert("提示", "退单账号不能为空！");
                                    return false;
                                }
                                CS('CZCLZ.Handler.QRSQ', function (retVal) {
                                    if (retVal) {
                                        if (retVal.sign == "true") {
                                            Ext.Msg.alert("提示", "申请成功！");
                                            OrderDenno = "";
                                            GpsTuiDanJinE = "";
                                            yanzhengma = "";
                                            dataBind();
                                            me.close();
                                        } else {
                                            Ext.Msg.alert("提示", "申请失败，请重试！");
                                            return false;
                                        }
                                    }
                                }, CS.onError, me.OrderDenno, Ext.getCmp("GpsTuiDanZhangHao").getValue());
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('PickBank', {
    extend: 'Ext.window.Window',

    height: document.documentElement.clientHeight - 200,
    width: document.documentElement.clientWidth / 4,
    layout: {
        type: 'fit'
    },
    title: '银行账号选择',
    id: 'pcWin',
    modal: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    id: 'xy',
                    layout: {
                        type: 'fit'
                    },
                    items: [
                      {
                          xtype: 'gridpanel',
                          columnLines: 1,
                          border: 1,
                          store: bankStore,
                          autoScroll: true,
                          columns: [
                              {
                                  xtype: 'gridcolumn',
                                  dataIndex: 'GpsTuiDanZhangHao',
                                  flex: 3,
                                  sortable: false,
                                  menuDisabled: true,
                                  text: '账号'
                              },
                              {
                                  xtype: 'gridcolumn',
                                  flex: 1,
                                  sortable: false,
                                  menuDisabled: true,
                                  text: '操作',
                                  renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                      return "<a href = 'javascript:void(0);' onClick='Pick(\"" + record.data.GpsTuiDanZhangHao + "\");'>选择</a>";
                                  }
                              }
                          ]
                      }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '关闭',
                            iconCls: 'close',
                            handler: function () {
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

var messageTime = 60;

function StartSendMessage() {
    StartMessage = setInterval(function () {
        if (messageTime > 0) {
            --messageTime;
            Ext.getCmp("sj").update("<span>" + messageTime + "s</span>");
        } else {
            Ext.getCmp("addMessages").show();
            Ext.getCmp("sj").hide();
            messageTime = 60;
            clearInterval(StartMessage);
        }
    }, 1000);
}

function Pick(zh) {
    dataBind();
    Ext.getCmp("GpsTuiDanZhangHao").setValue(zh);
    Ext.getCmp("pcWin").close();
}