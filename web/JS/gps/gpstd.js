var pageSize = 15;

var tdStore = createSFW4Store({
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
                        store: tdStore,
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

                            //            }
                            //        },
                            //        {
                            //            xtype: 'button',
                            //            iconCls: 'add',
                            //            text: '生成退单',
                            //            handler: function () {
                            //                FrameStack.pushFrame({
                            //                    url: "AddGpsTuidan.html",
                            //                    onClose: function (ret) {
                            //                        DataBind();
                            //                    }
                            //                });
                            //            }
                            //        }
                            //    ]
                            //},
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
