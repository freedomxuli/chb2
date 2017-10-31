var dg = {};
var sl = {};
var fin_je = 10;
var fin_num = 1;
var memo = "";

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
                                columnWidth: 1,
                                id: 'UserRemainder',
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
                                id:'je',
                                html: '<div style="color:red;font-size:30px;">10元</div>'
                            },
                            {
                                xtype: 'container',
                                columnWidth: 1,
                                padding:'50 0 0 0',
                                items: [
                                    {
                                        xtype: 'button',
                                        iconCls: 'enable',
                                        text: '微信支付',
                                        handler: function () {
                                            CS('CZCLZ.Handler.CZZH', function (retVal) {
                                                if (retVal) {
                                                    Ext.Msg.alert("提示", "支付成功！");
                                                    return false;
                                                }
                                            }, CS.onError, fin_num, fin_je, "wx");
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'enable',
                                        margin:'0 0 0 20',
                                        text: '支付宝支付',
                                        handler: function () {
                                            CS('CZCLZ.Handler.CZZH', function (retVal) {
                                                if (retVal) {
                                                    Ext.Msg.alert("提示", "支付成功！");
                                                    return false;
                                                }
                                            }, CS.onError, fin_num, fin_je, "zfb");
                                        }
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
    CS('CZCLZ.Handler.GetJGCL', function (retVal) {
        if(retVal)
        {
            Ext.getCmp("UserRemainder").update('<div style="font-size:25px;text-align-center;">剩余单量：' + retVal.UserRemainder + '单</div>');
            if (retVal.dt_jg.length > 0)
            {
                var radio1 = Ext.create('Ext.form.field.Radio', {
                    columnWidth: 0.3,
                    padding: '0 0 5 0',
                    id: 'radio1',
                    checked:true,
                    boxLabel: "购买单量（单价" + retVal.dt_jg[0]["JiaGeCeLveJinE"] + "元）",
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            if (newValue == true)
                            {
                                Ext.getCmp('je').update('<div style="color:red;font-size:30px;">' + parseInt(dg[field.id]) * parseInt(Ext.getCmp('numfield1').getValue()) + '元</div>');
                                fin_je = parseInt(dg[field.id]) * parseInt(Ext.getCmp('numfield1').getValue());
                                fin_num = parseInt(Ext.getCmp('numfield1').getValue());
                                memo = "单次充值，充值：" + fin_num + "单。共计：" + fin_je + ".000元";
                                if (newValue == true) {
                                    var n2 = 0;
                                    for (var i = 1; i < retVal.dt_jg.length; i++) {
                                        n2 = i + 1;
                                        Ext.getCmp("radio" + n2).setValue(false);
                                    }
                                }
                            }
                        }
                    }
                });
                var numfield1 = Ext.create('Ext.form.field.Number', {
                    columnWidth: 0.2,
                    minValue: 0,
                    id: 'numfield1',
                    value: 1,
                    minValue: 1,
                    allowDecimals: false,
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            if (Ext.getCmp("radio1").checked == true) {
                                Ext.getCmp('je').update('<div style="color:red;font-size:30px;">' + parseInt(dg["radio1"]) * parseInt(Ext.getCmp('numfield1').getValue()) + '元</div>');
                                fin_je = parseInt(dg["radio1"]) * parseInt(Ext.getCmp('numfield1').getValue());
                                fin_num = parseInt(Ext.getCmp('numfield1').getValue());
                                memo = "单次充值，充值：" + fin_num + "单。共计：" + fin_je + ".000元";
                            }
                        }
                    }
                });
                Ext.getCmp("taocan").add(radio1);
                Ext.getCmp("taocan").add(numfield1);
                dg["radio1"] = retVal.dt_jg[0]["JiaGeCeLveJinE"];
                sl["radio1"] = 1;
                var num = 2;
                for (var i = 1; i < retVal.dt_jg.length; i++)
                {
                    dg["radio" + num] = retVal.dt_jg[i]["JiaGeCeLveJinE"];
                    sl["radio" + num] = retVal.dt_jg[i]["JiaGeCeLveCiShu"];

                    var radio2 = Ext.create('Ext.form.field.Radio', {
                        columnWidth: 1,
                        padding: '0 0 5 0',
                        id: "radio" + num,
                        boxLabel: retVal.dt_jg[i]["JiaGeCeLveName"],
                        inputValue: retVal.dt_jg[i]["JiaGeCeLveCiShu"],
                        listeners:{  
                            change: function (field, newValue, oldValue) {
                                if (newValue == true)
                                {
                                    Ext.getCmp('je').update('<div style="color:red;font-size:30px;">' + parseInt(dg[field.id]) * parseInt(sl[field.id]) + '元</div>');
                                    fin_je = parseInt(dg[field.id]) * parseInt(sl[field.id]);
                                    fin_num = parseInt(sl[field.id]);
                                    memo = "套餐充值，充值：" + fin_num + "单。共计：" + fin_je + ".000元";

                                    Ext.getCmp("radio1").setValue(false);
                                    var n2 = 0;
                                    for (var i = 1; i < retVal.dt_jg.length; i++)
                                    {
                                        n2 = i + 1;
                                        if ("radio" + n2 != field.id)
                                        {
                                            Ext.getCmp("radio" + n2).setValue(false);
                                        }
                                    }
                                }
                            }
                        } 
                    });
                    Ext.getCmp("taocan").add(radio2);
                    num++;
                }
            }
        }
    },CS.onError);
}