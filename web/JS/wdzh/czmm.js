
Ext.onReady(function () {
    Ext.define('MainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            align: 'center',
            type: 'vbox'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
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
                                fieldLabel: '登录密码'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: 20,
                                fieldLabel: '确认密码'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.8,
                                padding: 20,
                                fieldLabel: '验证码'
                            },
                            {
                                xtype: 'button',
                                margin: '20 0 20 10',
                                iconCls: 'add',
                                text: '发送'
                            },
                            {
                                xtype: 'container',
                                columnWidth: 1,
                                items: [
                                    {
                                        xtype: 'button',
                                        margin: '50 0 20 130',
                                        iconCls: 'enable',
                                        text: '重置密码'
                                    }
                                ]
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