var pageSize = 15;

var ddStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'GpsDingDanTime', 'OrderDenno', 'GpsDingDanShuLiang', 'GpsDingDanJinE', 'GpsDingDanZhiFuZhuangTai'
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
                        store:ddStore,
                        columns: [
                            Ext.create('Ext.grid.RowNumberer'),
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'GpsDingDanTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                format:'Y-m-d',
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
                                text: '押金'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDingDanZhiFuZhuangTai',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '状态',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (value == "0")
                                        return "已支付";
                                    else
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
                                    if (record.data.GpsDingDanZhiFuZhuangTai=="0")
                                        return "<a href='javascript:void(0);' onClick='Del(\"" + record.data.OrderDenno + "\");'>删除</a>";
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
                                CS('CZCLZ.Handler.ZF', function (retVal) {
                                    if (retVal) {
                                        if (retVal.sign == "true") {
                                            Ext.Msg.alert("提示", "支付成功！");
                                            DataBind(1);
                                            me.close();
                                        } else {
                                            Ext.Msg.alert("提示", "支付失败，请重试！");
                                            return false;
                                        }
                                    }
                                }, CS.onError, me.OrderDenno, "wxpay");
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
                                            DataBind(1);
                                            me.close();
                                        } else {
                                            Ext.Msg.alert("提示", "支付失败，请重试！");
                                            return false;
                                        }
                                    }
                                }, CS.onError, me.OrderDenno, "alipay");
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
    CS('CZCLZ.Handler.GPSDD', function (retVal) {
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
    CS('CZCLZ.Handler.DelDD', function (retVal) {
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
        Ext.getCmp("GpsDingDanJinE").update("<span style='font-size:25px;color:red;font-weight:bold;'>押金为：" + GpsDingDanJinE + "元</span>");
    });
}