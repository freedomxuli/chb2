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
                                flex: 2,
                                sortable: false,
                                menuDisabled: true,
                                text: '充值描述'
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'datefield',
                                        editable: false,
                                        labelWidth: 60,
                                        width: 200,
                                        fieldLabel: '日期'
                                    },
                                    {
                                        xtype: 'label',
                                        text: '至'
                                    },
                                    {
                                        xtype: 'datefield',
                                        editable: false,
                                        width: 140
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询'
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
