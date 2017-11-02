var mxStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsDingDanMingXiTime', 'GpsDeviceID'
    ]
});

var OrderDenno = "";

var GpsDingDanJinE = "";

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
                                                id: 'HiddenID',
                                                fieldLabel: '隐藏设备号',
                                                margin:'-25 0 0 0',
                                                listeners: {
                                                    'change': function (field, newValue, oldValue) {
                                                        Ext.getCmp("GpsDeviceID").setValue(newValue);
                                                    }
                                                }
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
                                                        handler: function () {
                                                            CS('CZCLZ.Handler.AddGPS', function (retVal) {
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
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        margin: '50 0 20 130',
                                                        iconCls: 'close',
                                                        text: '重置设备号',
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
                                        dataIndex: 'GpsDingDanMingXiTime',
                                        flex: 1,
                                        sortable: false,
                                        menuDisabled: true,
                                        format:'Y-m-d',
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
                                        dataIndex: 'GpsDingDanMingXiID',
                                        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store)
                                        {
                                            return "<a href='javascript:void(0);' onclick='del(\"" + value + "\");'>删除</a>";
                                        }
                                    }
                                ]
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '支付订单',
                                iconCls:'enable',
                                handler: function () {
                                    //var win = new zhifu();
                                    //win.show(null, function () {
                                    //    Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>押金为：" + 100000.000 + "元</span>");
                                    //});
                                    if (OrderDenno != "") {
                                        CS('CZCLZ.Handler.TJDD', function (retVal) {
                                            if (retVal) {
                                                if (retVal.sign == "true") {
                                                    GpsDingDanJinE = retVal.GpsDingDanJinE;
                                                    var win = new zhifu();
                                                    win.show(null, function () {
                                                        Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>押金为：" + GpsDingDanJinE + "元</span>");
                                                    });
                                                } else {
                                                    Ext.Msg.alert("提示", retVal.msg);
                                                    return false;
                                                }
                                            }
                                        }, CS.onError, OrderDenno)
                                    }
                                    else {
                                        Ext.Msg.alert("提示", "请先生成支付单！");
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

function dataBind() {
    CS('CZCLZ.Handler.GetZhiFuGPS', function (retVal) {
        if (retVal)
        {
            mxStore.loadData(retVal.dt);
            OrderDenno = retVal.OrderDenno;
            Ext.getCmp("HiddenID").setValue("");
            Ext.getCmp('HiddenID').focus(true, true);
        }
    },CS.onError)
}

function del(GpsDingDanMingXiID) {
    CS('CZCLZ.Handler.DeleteDDItem', function (retVal) {
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
    }, CS.onError, GpsDingDanMingXiID)
}

function getSuccess() {
    var StartSearch = setInterval(function () {
        CS('CZCLZ.Handler.StartSearch', function (retVal) {
            if (retVal) {
                Ext.getCmp("WXEWM").close();
                dataBind();
                Ext.Msg.alert("提示", "充值成功！", function () {
                    window.clearInterval(StartSearch);
                });
            }
        }, CS.onError, OrderDenno)
    }, 3000);
}

Ext.define('zhifu', {
    extend: 'Ext.window.Window',

    height: 200,
    width: 400,
    layout: {
        type: 'fit'
    },
    title: '支付',

    modal:true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    items: [
                        {
                            xtype: 'label',
                            html: '',
                            margin:50,
                            id: 'GpsDingDanJinE'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '微信支付',
                            iconCls: 'enable',
                            handler: function () {
                                var win = new EWM();
                                win.show(null, function () {
                                    CS('CZCLZ.Handler.ShowEWMByYJ', function (retVal) {
                                        if (retVal) {
                                            Ext.getCmp("ShowEWM").setSrc("../../Pay/" + retVal);
                                            getSuccess();
                                        }
                                    }, CS.onError, OrderDenno, "web内用户押金，押金方式：微信；押金单号：" + orderdenno + "；押金金额：" + GpsDingDanJinE + "。");
                                });
                                //CS('CZCLZ.Handler.ZF', function (retVal) {
                                //    if (retVal)
                                //    {
                                //        if (retVal.sign == "true") {
                                //            Ext.Msg.alert("提示", "支付成功！");
                                //            OrderDenno = "";
                                //            GpsDingDanJinE = "";
                                //            dataBind();
                                //            me.close();
                                //        } else {
                                //            Ext.Msg.alert("提示", "支付失败，请重试！");
                                //            return false;
                                //        }
                                //    }
                                //}, CS.onError, OrderDenno, "wxpay");
                            }
                        }
                        //{
                        //    text: '支付宝支付',
                        //    iconCls: 'enable',
                        //    handler: function () {
                        //        CS('CZCLZ.Handler.ZF', function (retVal) {
                        //            if (retVal) {
                        //                if (retVal.sign == "true") {
                        //                    Ext.Msg.alert("提示", "支付成功！");
                        //                    OrderDenno = "";
                        //                    GpsDingDanJinE = "";
                        //                    dataBind();
                        //                    me.close();
                        //                } else {
                        //                    Ext.Msg.alert("提示", "支付失败，请重试！");
                        //                    return false;
                        //                }
                        //            }
                        //        }, CS.onError, OrderDenno, "alipay");
                        //    }
                        //}
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('EWM', {
    extend: 'Ext.window.Window',

    height: 385,
    width: 509,
    layout: {
        type: 'fit'
    },
    title: '支付二维码',
    modal: true,
    id: 'WXEWM',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'fit'
                    },
                    items: [
                        {
                            xtype: 'image',
                            id: 'ShowEWM',
                            margin: 30
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '关闭',
                            iconCls: 'close',
                            handler: function () {
                                window.clearInterval(StartSearch);
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