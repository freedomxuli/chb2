
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
                                height: document.documentElement.clientHeight / 2,
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
                                                padding: 20,
                                                fieldLabel: '手机号码'
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 1,
                                                padding: 20,
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
                                                        text: '确认'
                                                    }
                                                ]
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'gridpanel',
                                height: document.documentElement.clientHeight / 2,
                                border: 1,
                                columnLines: 1,
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
                                        dataIndex: 'GpsDingDanMingXiTime',
                                        flex: 1,
                                        sortable: false,
                                        menuDisabled: true,
                                        format:'Y-m-d',
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
                                        text: '操作'
                                    }
                                ]
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '支付订单',
                                iconCls:'enable',
                                handler: function () {
                                    var win = new zhifu();
                                    win.show(null, function () {

                                    });
                                }
                            },
                            {
                                text: '返回',
                                iconCls: 'back',
                                handler: function () {

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
});

Ext.define('zhifu', {
    extend: 'Ext.window.Window',

    height: 200,
    width: 300,
    layout: {
        type: 'fit'
    },
    title: '支付',

    modal:true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    items: [
                        {
                            xtype: 'numberfield',
                            fieldLabel: '押金',
                            labelWidth: 40,
                            allowDecimals: false,
                            padding:'50 50 50 50',
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '微信支付',
                            iconCls: 'enable',
                            handler: function () {

                            }
                        },
                        {
                            text: '支付宝支付',
                            iconCls: 'enable',
                            handler: function () {

                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});