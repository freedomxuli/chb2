﻿var StartSearch;

var pageSize = 15;

var ddStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'GpsDingDanTime', 'OrderDenno', 'GpsDingDanShuLiang', 'GpsDingDanJinE', 'GpsDingDanZhiFuZhuangTai', 'GpsDingDanSH', 'GpsDingDanZhiFuLeiXing'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

Ext.onReady(function () {

    Ext.define('mainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        store: ddStore,
                        columns: [
                            Ext.create('Ext.grid.RowNumberer'),
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'GpsDingDanTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                format: 'Y-m-d',
                                text: '日期'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'OrderDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDingDanShuLiang',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '数量'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDingDanJinE',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '金额'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDingDanZhiFuZhuangTai',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '状态',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (value == "1") {
                                        if (record.data.GpsDingDanZhiFuLeiXing != "公对公")
                                            return "已支付";
                                        else {
                                            if (record.data.GpsDingDanSH == "0")
                                                return "公对公支付待审核";
                                            else
                                                return "已支付";
                                        }
                                    } else
                                        return "未支付";
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDingDanZhiFuZhuangTai',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.GpsDingDanZhiFuZhuangTai == "1")
                                        return "<a href='javascript:void(0);'></a>";
                                    else
                                        return "<a href='javascript:void(0);' onClick='Del(\"" + record.data.OrderDenno + "\");'>删除</a>　<a href='javascript:void(0);' onClick='zhifu(\"" + record.data.OrderDenno + "\",\"" + record.data.GpsDingDanJinE + "\");'>支付</a>";
                                }
                            }
                        ],
                        dockedItems: [
                            //{
                            //    xtype: 'toolbar',
                            //    dock: 'top',
                            //    items: [
                            //        {
                            //            xtype: 'textfield',
                            //            width: 130,
                            //            labelWidth: 30,
                            //            id: 'UserDenno',
                            //            fieldLabel: '单号'
                            //        },
                            //        {
                            //            xtype: 'button',
                            //            iconCls: 'search',
                            //            text: '查询',
                            //            handler: function () {
                            //                DataBind(1);
                            //            }
                            //        }
                            //    ]
                            //},
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: ddStore,
                                displayInfo: true
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new mainView();

    DataBind(1);
});

Ext.define('zhifuWin', {
    extend: 'Ext.window.Window',

    height: 200,
    width: 400,
    layout: {
        type: 'fit'
    },
    title: '支付',

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
                                            getSuccess(me.OrderDenno);
                                            me.close();
                                        }
                                    }, CS.onError, me.OrderDenno, me.GpsDingDanJinE, "web内用户付款，付款方式：微信；付款单号：" + me.OrderDenno + "；付款金额：" + me.GpsDingDanJinE + "。");//GpsDingDanJinE
                                });
                            }
                        },
                        {
                            xtype: 'button',
                            iconCls: 'enable',
                            text: '支付宝支付',
                            handler: function () {
                                var win = new Ali();
                                win.show(null, function () {
                                    Ext.getCmp("AliFrame").update("<iframe frameborder='0' src='../../PayByAli/Ali_url.aspx?orderdenno=" + me.OrderDenno + "&fin_je=0.01&lx=2' width='100%' height='100%'></iframe>");
                                    setTimeout(function () {
                                        Ext.getCmp("AliFrame").show();
                                        getSuccess2(me.OrderDenno);
                                    }, 1500);
                                });
                            }
                        },
                        {
                            text: '公对公支付',
                            iconCls: 'enable',
                            handler: function () {
                                var win = new GDG({ "OrderDenno": me.OrderDenno });
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
                        //                    DataBind(1);
                        //                    me.close();
                        //                } else {
                        //                    Ext.Msg.alert("提示", "支付失败，请重试！");
                        //                    return false;
                        //                }
                        //            }
                        //        }, CS.onError, me.OrderDenno, "alipay");
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
                                DataBind(1);
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
                                DataBind(1);
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
                                            DataBind(1);
                                        });
                                    }
                                }, CS.onError, me.OrderDenno, Ext.getCmp("DGZZCompany").getValue(), Ext.getCmp("DGZH").getValue(), Ext.getCmp("DKPZH").getValue());
                            }
                        },
                        {
                            text: '关闭',
                            iconCls: 'close',
                            handler: function () {
                                DataBind(1);
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

function DataBind(cp) {
    CS('CZCLZ.Handler.GPSDDSale', function (retVal) {
        if (retVal) {
            ddStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize);
}

function Del(OrderDenno) {
    CS('CZCLZ.Handler.DelDDSale', function (retVal) {
        if (retVal) {
            if (retVal.sign == "true") {
                Ext.Msg.alert("提示", "删除成功！", function () {
                    DataBind(1);
                });
            }
            else {
                Ext.Msg.alert("提示", msg, function () {
                    return false;
                });
            }
        }
    }, CS.onError, OrderDenno);
}

function zhifu(OrderDenno, GpsDingDanJinE) {
    var win = new zhifuWin({ OrderDenno: OrderDenno, GpsDingDanJinE: GpsDingDanJinE });
    win.show(null, function () {
        Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>金额为：" + GpsDingDanJinE + "元</span>");
    });
}

function getSuccess(OrderDenno) {
    StartSearch = setInterval(function () {
        ACS('CZCLZ.Handler.StartSearch', function (retVal) {
            if (retVal) {
                Ext.getCmp("WXEWM").close();
                DataBind(1);
                Ext.Msg.alert("提示", "支付成功！", function () {
                    window.clearInterval(StartSearch);
                });
            }
        }, CS.onError, OrderDenno)
    }, 3000);
}

function getSuccess2(OrderDenno) {
    StartSearch = setInterval(function () {
        ACS('CZCLZ.Handler.StartSearch', function (retVal) {
            if (retVal) {
                Ext.getCmp("AliYun").close();
                DataBind(1);
                Ext.Msg.alert("提示", "支付成功！", function () {
                    window.clearInterval(StartSearch);
                });
            }
        }, CS.onError, OrderDenno);
    }, 3000);
}