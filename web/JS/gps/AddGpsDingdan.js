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
                                        text: '操作'
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
                                    var win = new zhifu();
                                    win.show(null, function () {
                                        Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>押金为：" + 100000.000 + "元</span>");
                                    });
                                    //if (OrderDenno != "") {
                                    //    CS('CZCLZ.Handler.TJDD', function (retVal) {
                                    //        if (retVal) {
                                    //            if (retVal.sign == "true") {
                                    //                GpsDingDanJinE = retVal.GpsDingDanJinE;
                                    //                var win = new zhifu();
                                    //                win.show(null, function () {
                                    //                    Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>押金为：" + GpsDingDanJinE + "元</span>");
                                    //                });
                                    //            } else {
                                    //                Ext.Msg.alert("提示", retVal.msg);
                                    //                return false;
                                    //            }
                                    //        }
                                    //    }, CS.onError, OrderDenno)
                                    //}
                                    //else {
                                    //    Ext.Msg.alert("提示", "请先生成支付单！");
                                    //    return false;
                                    //}
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
        }
    },CS.onError)
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
                                CS('CZCLZ.Handler.ZF', function (retVal) {
                                    if (retVal)
                                    {
                                        if (retVal.sign == "true") {
                                            Ext.Msg.alert("提示", "支付成功！");
                                            OrderDenno = "";
                                            GpsDingDanJinE = "";
                                            dataBind();
                                            me.close();
                                        } else {
                                            Ext.Msg.alert("提示", "支付失败，请重试！");
                                            return false;
                                        }
                                    }
                                }, CS.onError, OrderDenno, "wxpay");
                            }
                        },
                        {
                            text: '支付宝支付',
                            iconCls: 'enable',
                            handler: function () {
                                CS('CZCLZ.Handler.ZF', function (retVal) {
                                    if (retVal) {
                                        if (retVal.sign == "true") {
                                            Ext.Msg.alert("提示", "支付成功！");
                                            OrderDenno = "";
                                            GpsDingDanJinE = "";
                                            dataBind();
                                            me.close();
                                        } else {
                                            Ext.Msg.alert("提示", "支付失败，请重试！");
                                            return false;
                                        }
                                    }
                                }, CS.onError, OrderDenno, "alipay");
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});