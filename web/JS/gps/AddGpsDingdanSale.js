﻿var mxStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsDingDanMingXiTime', 'GpsDeviceID', 'GpsDingDanMingXiID'
    ]
});

var OrderDenno = "";

var GpsDingDanJinE = "";

var StartSearch;

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
                                                                var n = Ext.getCmp("GpsDeviceID").getValue().indexOf("2020");
                                                                if (n != 0) {
                                                                    n = Ext.getCmp("GpsDeviceID").getValue().indexOf("8630");
                                                                }
                                                                //if (n != 0) {
                                                                //    n = Ext.getCmp("GpsDeviceID").getValue().indexOf("2020");
                                                                //}
                                                                if (n == 0) {
                                                                    CS('CZCLZ.Handler.AddGPSSale', function (retVal) {
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
                                                        margin: '50 0 20 130',
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
                                        dataIndex: 'GpsDingDanMingXiTime',
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
                                        dataIndex: 'GpsDingDanMingXiID',
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
                                text: '支付订单',
                                iconCls: 'enable',
                                style: 'padding:15px;',
                                handler: function () {
                                    //var win = new zhifu();
                                    //win.show(null, function () {
                                    //    Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>押金为：" + 100000.000 + "元</span>");
                                    //});
                                    if (OrderDenno != "") {
                                        CS('CZCLZ.Handler.TJDDSale', function (retVal) {
                                            if (retVal) {
                                                if (retVal.sign == "true") {
                                                    GpsDingDanJinE = retVal.GpsDingDanJinE;
                                                    var win = new zhifu();
                                                    win.show(null, function () {
                                                        Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>金额为：" + GpsDingDanJinE + "元</span>");
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
                                        Ext.Msg.alert("提示", "请先生成支付单！");
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

function dataBind() {
    CS('CZCLZ.Handler.GetZhiFuGPSSale', function (retVal) {
        if (retVal) {
            mxStore.loadData(retVal.dt);
            OrderDenno = retVal.OrderDenno;
            Ext.getCmp("HiddenID").setValue("");
            Ext.getCmp('HiddenID').focus(true, true);
        }
    }, CS.onError)
}

function del(GpsDingDanMingXiID) {
    CS('CZCLZ.Handler.DeleteDDItemSale', function (retVal) {
        if (retVal) {
            if (retVal.sign == "true") {
                Ext.Msg.alert("提示", "删除成功！", function () {
                    dataBind();
                });
            } else {
                Ext.Msg.alert("提示", retVal.msg);
                dataBind();
                return false;
            }
        }
    }, CS.onError, GpsDingDanMingXiID)
}

function getSuccess() {
    StartSearch = setInterval(function () {
        ACS('CZCLZ.Handler.StartSearch', function (retVal) {
            if (retVal) {
                Ext.getCmp("WXEWM").close();
                dataBind();
                Ext.Msg.alert("提示", "支付成功！", function () {
                    window.clearInterval(StartSearch);
                });
            }
        }, CS.onError, OrderDenno)
    }, 3000);
}

function getSuccess2() {
    StartSearch = setInterval(function () {
        ACS('CZCLZ.Handler.StartSearch', function (retVal) {
            if (retVal) {
                Ext.getCmp("AliYun").close();
                dataBind();
                Ext.Msg.alert("提示", "支付成功！", function () {
                    window.clearInterval(StartSearch);
                });
            }
        }, CS.onError, OrderDenno);
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
    id: 'zhifuShow',
    modal: true,
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
                            margin: 50,
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
                                            me.close();
                                        }
                                    }, CS.onError, OrderDenno, GpsDingDanJinE, "web内用户付款，付款方式：微信；付款单号：" + OrderDenno + "；付款金额：" + GpsDingDanJinE + "。");//GpsDingDanJinE
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
                        },
                        {
                            xtype: 'button',
                            iconCls: 'enable',
                            text: '支付宝支付',
                            handler: function () {
                                var win = new Ali();
                                win.show(null, function () {
                                    Ext.getCmp("AliFrame").update("<iframe frameborder='0' src='../../PayByAli/Ali_url.aspx?orderdenno=" + OrderDenno + "&fin_je=" + GpsDingDanJinE + "&lx=3' width='100%' height='100%'></iframe>");
                                    setTimeout(function () {
                                        Ext.getCmp("AliFrame").show();
                                        getSuccess2();
                                    }, 1500);
                                });
                            }
                        },
                        {
                            text: '公对公支付',
                            iconCls: 'enable',
                            handler: function () {
                                var win = new GDG();
                                win.show(null, function () {
                                    me.close();
                                });
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
                            margin: '80 140 80 140'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '关闭',
                            iconCls: 'close',
                            handler: function () {
                                dataBind();
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

Ext.define('Ali', {
    extend: 'Ext.window.Window',

    height: 350,
    width: 700,
    layout: {
        type: 'fit'
    },
    title: '支付宝支付',
    modal: true,
    id: 'AliYun',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'fit'
                    },
                    id: 'AliFrame',
                    hidden: true,
                    html: "",
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '关闭',
                            iconCls: 'close',
                            handler: function () {
                                dataBind();
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

Ext.define('GDG', {
    extend: 'Ext.window.Window',

    height: document.documentElement.clientHeight - 200,
    width: document.documentElement.clientWidth / 3,
    layout: {
        type: 'fit'
    },
    title: '公对公支付',
    id: 'GDGWin',
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
                            fieldLabel: '对公转账公司名称',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'DGZZCompany',
                            padding: '20 10 20 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '对公账户',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'DGZH',
                            padding: '20 10 20 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '打款凭证号',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'DKPZH',
                            padding: '20 10 20 10'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认',
                            iconCls: 'enable',
                            handler: function () {
                                if (Ext.getCmp("DGZZCompany").getValue() == "" || Ext.getCmp("DGZZCompany").getValue() == null) {
                                    Ext.Msg.alert("提示", "对公转账公司名称不能为空！");
                                    return;
                                }
                                if (Ext.getCmp("DGZH").getValue() == "" || Ext.getCmp("DGZH").getValue() == null) {
                                    Ext.Msg.alert("提示", "对公账户不能为空！");
                                    return;
                                }
                                if (Ext.getCmp("DKPZH").getValue() == "" || Ext.getCmp("DKPZH").getValue() == null) {
                                    Ext.Msg.alert("提示", "打款凭证号不能为空！");
                                    return;
                                }
                                CS('CZCLZ.Handler.GDGPaySale', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "支付成功！", function () {
                                            Ext.getCmp("GDGWin").close();
                                            dataBind();
                                        });
                                    }
                                }, CS.onError, OrderDenno, Ext.getCmp("DGZZCompany").getValue(), Ext.getCmp("DGZH").getValue(), Ext.getCmp("DKPZH").getValue());
                            }
                        },
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