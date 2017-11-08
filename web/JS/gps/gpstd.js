var pageSize = 15;
var StartMessage;

var tdStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'GpsTuiDanTime', 'OrderDenno', 'GpsTuiDanShuLiang', 'GpsTuiDanJinE', 'GpsTuiDanTuiHuanZhuangTai'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var bankStore = Ext.create('Ext.data.Store', {
    fields: [
        'GpsTuiDanZhangHao'
    ]
});

var yanzhengma = "";

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
                        store: tdStore,
                        columns: [
                            Ext.create('Ext.grid.RowNumberer'),
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'GpsTuiDanTime',
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
                                dataIndex: 'GpsTuiDanShuLiang',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '数量'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsTuiDanJinE',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '押金'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsTuiDanTuiHuanZhuangTai',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '状态',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (value == "1")
                                        return "已退还";
                                    else
                                        return "未退还";
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.GpsDingDanZhiFuZhuangTai == "1")
                                        return "<a href='javascript:void(0);'></a>";
                                    else
                                        return "<a href='javascript:void(0);' onClick='Del(\"" + record.data.OrderDenno + "\");'>删除</a>　<a href='javascript:void(0);' onClick='shenqing(\"" + record.data.OrderDenno + "\",\"" + record.data.GpsTuiDanJinE + "\");'>申请退单</a>";
                                }
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: tdStore,
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
                                        if (retVal) {
                                            bankStore.loadData(retVal);
                                        }
                                    }, CS.onError)
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
                            id:'addMessages',
                            handler: function () {
                                yanzhengma = "";
                                CS('CZCLZ.Handler.SendMessage', function (retVal) {
                                    if (retVal) {
                                        if (retVal.sign = "true") {
                                            yanzhengma = retVal.yanzhengma;
                                            Ext.Msg.alert("提示", "已成功发送短信！");
                                            DataBind(1);
                                            StartSendMessage();
                                            Ext.getCmp("addMessages").hide();
                                            Ext.getCmp("sj").show();
                                            return;
                                        } else {
                                            Ext.Msg.alert("提示", retVal.msg);
                                            DataBind(1);
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
                            html:'60s'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认申请',
                            iconCls: 'enable',
                            handler: function () {
                                if (yanzhengma != Ext.getCmp("yanzhengma").getValue()) {
                                    Ext.Msg.alert("提示", "验证码错误！");
                                    DataBind(1);
                                    return false;
                                }
                                if (Ext.getCmp("GpsTuiDanZhangHao").getValue() == "" || Ext.getCmp("GpsTuiDanZhangHao").getValue() == null) {
                                    Ext.Msg.alert("提示", "退单账号不能为空！");
                                    DataBind(1);
                                    return false;
                                }
                                CS('CZCLZ.Handler.QRSQ', function (retVal) {
                                    if (retVal) {
                                        if (retVal.sign == "true") {
                                            Ext.Msg.alert("提示", "申请成功！");
                                            yanzhengma = "";
                                            DataBind(1);
                                            me.close();
                                        } else {
                                            Ext.Msg.alert("提示", "申请失败，请重试！");
                                            DataBind(1);
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

function DataBind(cp) {
    CS('CZCLZ.Handler.GPSTD', function (retVal) {
        if (retVal)
        {
            tdStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize);
}

function Del(OrderDenno) {
    CS('CZCLZ.Handler.DelTD', function (retVal) {
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

function shenqing(OrderDenno, GpsTuiDanJinE)
{
    var win = new tuidan({ OrderDenno: OrderDenno });
    win.show(null, function () {
        Ext.getCmp("GpsTuiDanJinE").setValue(GpsTuiDanJinE);
    });
}

function Pick(zh) {
    Ext.getCmp("GpsTuiDanZhangHao").setValue(zh);
    Ext.getCmp("pcWin").close();
}
