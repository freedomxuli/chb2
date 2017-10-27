
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
                                xtype: 'label',
                                padding: 20,
                                columnWidth:1,
                                html: '<div style="font-size:25px;text-align-center;">剩余单量：0单</div>'
                            },
                            {
                                xtype: 'label',
                                columnWidth: 1,
                                text: '选择套餐:'
                            },
                            {
                                xtype: 'container',
                                id: 'taocan',
                                columnWidth: 1,
                                layout: {
                                    type: 'column'
                                },
                                padding:'5 0 0 0',
                                items: [
                                    
                                ]
                            },
                            {
                                xtype: 'label',
                                columnWidth: 0.2,
                                padding:'15 0 0 0',
                                text: '支付金额:'
                            },
                            {
                                xtype: 'label',
                                columnWidth: 0.8,
                                html: '<div style="color:red;font-size:30px;">100元</div>'
                            },
                            {
                                xtype: 'container',
                                columnWidth: 1,
                                padding:'50 0 0 0',
                                items: [
                                    {
                                        xtype: 'button',
                                        iconCls: 'enable',
                                        text: '微信支付'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'enable',
                                        margin:'0 0 0 20',
                                        text: '支付宝支付'
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

    radioBind();

});

function radioBind() {
    var radio1 = Ext.create('Ext.form.field.Radio', {
        columnWidth: 0.3,
        padding:'0 0 5 0',
        boxLabel: "购买单量（单价10元）"
    });
    var numfield1 = Ext.create('Ext.form.field.Number', {
        columnWidth: 0.2,
        minValue: 0,
        allowDecimals: false
    });
    var radio2 = Ext.create('Ext.form.field.Radio', {
        columnWidth: 1,
        padding: '0 0 5 0',
        boxLabel: "套餐一：10单（58元）"
    });
    var radio3 = Ext.create('Ext.form.field.Radio', {
        columnWidth: 1,
        padding: '0 0 5 0',
        boxLabel: "套餐二：50单（188元）"
    });
    var radio4 = Ext.create('Ext.form.field.Radio', {
        columnWidth: 1,
        padding: '0 0 5 0',
        boxLabel: "套餐三：100单（288元）"
    });
    var label1 = Ext.create('Ext.form.Label', {
        columnWidth: 1,
        padding: '0 0 50 0',
        text:'备注：每制单一次，即消耗一次单量'
    });
    Ext.getCmp("taocan").add(radio1);
    Ext.getCmp("taocan").add(numfield1);
    Ext.getCmp("taocan").add(radio2);
    Ext.getCmp("taocan").add(radio3);
    Ext.getCmp("taocan").add(radio4);
    Ext.getCmp("taocan").add(label1);
}