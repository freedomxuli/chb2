var mxStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsTuiDanMingXiTime', 'GpsDeviceID'
    ]
});

var bankStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsTuiDanZhangHao'
    ]
});

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
                                height: document.documentElement.clientHeight / 4,
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
                                                padding: 20,
                                                id: 'GpsDeviceID',
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
                                                        handler: function () {
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
                                handler: function () {
                                    if (OrderDenno != "") {
                                        CS('CZCLZ.Handler.TJTD', function (retVal) {
                                            if (retVal) {
                                                if (retVal.sign == "true") {
                                                    GpsTuiDanJinE = retVal.GpsTuiDanJinE;
                                                    var win = new tuidan();
                                                    win.show(null, function () {
                                                        Ext.getCmp("GpsTuiDanJinE").setValue(GpsTuiDanJinE);
                                                    });
                                                } else {
                                                    Ext.Msg.alert("提示", retVal.msg);
                                                    return false;
                                                }
                                            }
                                        }, CS.onError, OrderDenno)
                                    }
                                    else {
                                        Ext.Msg.alert("提示", "请先生成退单！");
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

    height: 500,
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
                            editable: false,
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
                            handler: function () {
                                yanzhengma = "";
                                CS('CZCLZ.Handler.SendMessage', function (retVal) {
                                    if(retVal)
                                    {
                                        if (retVal.sign = "true") {
                                            yanzhengma = retVal.yanzhengma;
                                            Ext.Msg.alert("提示", "已成功发送短信！");
                                            return;
                                        } else {
                                            Ext.Msg.alert("提示", retVal.msg);
                                            return false;
                                        }
                                    }
                                }, CS.onError, 'tuidanshenqing');
                            }
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认申请',
                            iconCls: 'enable',
                            handler: function () {
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
                                }, CS.onError, OrderDenno, Ext.getCmp("GpsTuiDanZhangHao").getValue());
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

function Pick(zh) {
    Ext.getCmp("GpsTuiDanZhangHao").setValue(zh);
    Ext.getCmp("pcWin").close();
}