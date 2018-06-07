var pageSize = 15;

var InvoiceStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'InvoiceId', 'InvoiceTitle', 'InvoiceZZJGDM', 'InvoicePerson', 'InvoiceMobile', 'InvoiceAddress', 'UserId', 'AddTime', 'IsOut'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var CZStore = Ext.create('Ext.data.Store', {
    fields: [
        'ChongZhiID', 'ChongZhiJinE', 'ChongZhiTime', 'ChongZhiRemark', 'LX',
    ]
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
                        store: InvoiceStore,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'InvoiceTitle',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '发票抬头'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'InvoiceZZJGDM',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '纳税人识别号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'InvoicePerson',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '联系人'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'InvoiceMobile',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '联系电话'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'InvoiceAddress',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '寄送地址'
                            },
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'AddTime',
                                flex: 1,
                                format:'Y-m-d',
                                sortable: false,
                                menuDisabled: true,
                                text: '申领时间'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'IsOut',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '是否发出',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                    if(value==false)
                                        return "未发出";
                                    else
                                        return "已发出";
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'IsOut',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '是否审核',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                    if(value==false)
                                        return "未审核";
                                    else
                                        return "已审核";
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'InvoiceId',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.IsOut == false)
                                        return "<a href='javascript:void(0);' onclick='OnDel(\"" + value + "\");'>删除</a>";
                                }
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'datefield',
                                        width: 220,
                                        labelWidth: 60,
                                        format:'Y-m-d',
                                        id: 'StartTime',
                                        editable:false,
                                        fieldLabel: '申领时间'
                                    },
                                    {
                                        xtype: 'label',
                                        text:'至'
                                    },
                                    {
                                        xtype: 'datefield',
                                        width: 160,
                                        format: 'Y-m-d',
                                        id: 'EndTime',
                                        editable: false
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询',
                                        handler: function () {
                                            DataBind(1);
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'add',
                                        text: '新增发票',
                                        handler: function () {
                                            var win = new AddInvoice();
                                            win.show(null, function () {
                                                CS('CZCLZ.Handler.GetChongZhiListByInvoice', function (retVal) {
                                                    if (retVal)
                                                    {
                                                        CZStore.loadData(retVal);
                                                    }
                                                },CS.onError)
                                            });
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: InvoiceStore,
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

function DataBind(cp) {
    CS('CZCLZ.Handler.GetInvoiceList', function (retVal) {
        if (retVal) {
            InvoiceStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('StartTime').getValue(), Ext.getCmp('EndTime').getValue());
}


Ext.define('AddInvoice', {
    extend: 'Ext.window.Window',

    height: document.documentElement.clientHeight - 200,
    width: document.documentElement.clientWidth / 2,
    layout: {
        type: 'fit'
    },
    title: '新增发票',
    id: 'AddWin',
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
                            xtype: 'panel',
                            columnWidth: 0.5,
                            layout: {
                                type: 'column'
                            },
                            height: document.documentElement.clientHeight - 250,
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '发票抬头',
                                    labelWidth: 80,
                                    columnWidth: 1,
                                    id: 'InvoiceTitle',
                                    padding: 10
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '组织结构代码',
                                    labelWidth: 80,
                                    columnWidth: 1,
                                    id: 'InvoiceZZJGDM',
                                    padding: 10
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '联系人',
                                    labelWidth: 80,
                                    columnWidth: 1,
                                    id: 'InvoicePerson',
                                    padding: 10
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '联系电话',
                                    labelWidth: 80,
                                    columnWidth: 1,
                                    id: 'InvoiceMobile',
                                    padding: 10
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '寄送地址',
                                    labelWidth: 80,
                                    columnWidth: 1,
                                    id: 'InvoiceAddress',
                                    padding: 10
                                },
                                {
                                    xtype: 'label',
                                    html: '<div style="color:red;">开票金额满1000元包邮，邮费由查货宝支付！</div>',
                                    columnWidth: 1,
                                    padding: 10
                                }
                            ]
                        },
                        {
                            xtype: 'gridpanel',
                            columnLines: 1,
                            border: 1,
                            columnWidth: 0.5,
                            store: CZStore,
                            selModel: { selType: 'checkboxmodel' },
                            id: 'CZStore',
                            scroll: true,
                            height: document.documentElement.clientHeight - 260,
                            columns: [
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'ChongZhiTime',
                                    flex: 1,
                                    sortable: false,
                                    menuDisabled: true,
                                    format: 'Y-m-d',
                                    text: '日期'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ChongZhiJinE',
                                    flex: 1,
                                    sortable: false,
                                    menuDisabled: true,
                                    text: '金额'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ChongZhiRemark',
                                    flex: 1,
                                    sortable: false,
                                    menuDisabled: true,
                                    text: '充值描述'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ChongZhiID',
                                    flex: 1,
                                    sortable: false,
                                    menuDisabled: true,
                                    hidden:true,
                                    text: '充值ID'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'LX',
                                    flex: 1,
                                    sortable: false,
                                    menuDisabled: true,
                                    hidden: true,
                                    text: '类型'
                                }
                            ]
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认',
                            iconCls: 'enable',
                            handler: function () {
                                if (Ext.getCmp('InvoiceTitle').getValue() == "" || Ext.getCmp('InvoiceTitle').getValue() == null)
                                {
                                    Ext.Msg.alert("提示", "发票抬头不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp('InvoiceZZJGDM').getValue() == "" || Ext.getCmp('InvoiceZZJGDM').getValue() == null) {
                                    Ext.Msg.alert("提示", "组织机构代码不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp('InvoicePerson').getValue() == "" || Ext.getCmp('InvoicePerson').getValue() == null) {
                                    Ext.Msg.alert("提示", "联系人不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp('InvoiceMobile').getValue() == "" || Ext.getCmp('InvoiceMobile').getValue() == null) {
                                    Ext.Msg.alert("提示", "联系电话不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp('InvoiceAddress').getValue() == "" || Ext.getCmp('InvoiceAddress').getValue() == null) {
                                    Ext.Msg.alert("提示", "寄送地址不能为空！");
                                    return false;
                                }
                                var selectedData = Ext.getCmp('CZStore').getSelectionModel().getSelection();
                                var str = "";
                                var str2 = "";
                                var je = 0;
                                for (var i = 0; i < selectedData.length; i++)
                                {
                                    console.log(selectedData[i].data.ChongZhiID);
                                    if (selectedData[i].data.LX == "0")
                                        str += selectedData[i].data.ChongZhiID + ",";
                                    else
                                        str2 += selectedData[i].data.ChongZhiID + ",";
                                    je += parseFloat(selectedData[i].data.ChongZhiJinE);
                                }
                                if (str == ""&&str2 == "")
                                {
                                    Ext.Msg.alert("提示", "请选择充值记录！");
                                    return false;
                                }
                                CS('CZCLZ.Handler.AddInvoice', function (retVal) {
                                    if (retVal) {
                                        me.close();
                                        Ext.Msg.alert("提示", "添加成功，等待审核及邮寄！", function () {
                                            DataBind(1);
                                        });
                                    }
                                }, CS.onError, Ext.getCmp('InvoiceTitle').getValue(), Ext.getCmp('InvoiceZZJGDM').getValue(), Ext.getCmp('InvoicePerson').getValue(), Ext.getCmp('InvoiceMobile').getValue(), Ext.getCmp('InvoiceAddress').getValue(), je, str, str2);
                            }
                        },
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

function OnDel(Invoiceid)
{
    Ext.Msg.confirm("提示", "是否删除该条记录？", function (btn) {
        if (btn=="yes")
        {
            CS('CZCLZ.Handler.OnDelInvoice', function (retVal) {
                if (retVal) {
                    DataBind(1);
                }
            }, CS.onError, Invoiceid);
        }
    })
}