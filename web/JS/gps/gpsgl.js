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
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDeviceID',
                                flex: 2,
                                sortable: false,
                                menuDisabled: true,
                                text: '设备标识'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'IsBangding',
                                flex: 2,
                                sortable: false,
                                menuDisabled: true,
                                text: '绑定状态'
                            }
                        ],
                        dockedItems: [
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
