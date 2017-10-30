var pageSize = 15;

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
                                    if (value == "0")
                                        return "已支付";
                                    else
                                        return "未支付";
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    return "<a href='javascript:void(0);' onClick='Del(\"" + record.data.OrderDenno + "\");'>删除</a>";
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

function DataBind() {
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
