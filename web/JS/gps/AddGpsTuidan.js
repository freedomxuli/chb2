
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
                                        format: 'Y-m-d',
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
                                text: '申请退单',
                                iconCls: 'enable',
                                handler: function () {
                                    var win = new tuidan();
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

Ext.define('tuidan', {
    extend: 'Ext.window.Window',

    height: 500,
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
                            xtype: 'numberfield',
                            fieldLabel: '退款金额',
                            labelWidth: 70,
                            allowDecimals: false,
                            columnWidth:1,
                            padding: '50 10 50 10',
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '退款卡号',
                            labelWidth: 70,
                            columnWidth: 0.8,
                            padding: '0 10 50 10',
                        },
                        {
                            xtype: 'button',
                            iconCls: 'add',
                            columnWidth: 0.2,
                            margin: '0 10 50 0',
                            text: '选择'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '验证码',
                            labelWidth: 70,
                            columnWidth: 0.8,
                            padding: '0 10 50 10',
                        },
                        {
                            xtype: 'button',
                            iconCls: 'add',
                            columnWidth: 0.2,
                            margin: '0 10 50 0',
                            text: '发送'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认申请',
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