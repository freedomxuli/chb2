
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
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'xuhao',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '序号'
                            },
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
                                text: '状态'
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'textfield',
                                        width: 130,
                                        labelWidth: 30,
                                        id: 'UserDenno',
                                        fieldLabel: '单号'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'add',
                                        text: '生成订单',
                                        handler: function () {
                                            FrameStack.pushFrame({
                                                url: "AddGpsDingdan.html",
                                                onClose: function (ret) {
                                                    DataBind();
                                                }
                                            });
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
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

    DataBind();
});

function DataBind() {

}
