var yanzhengma = "";

var StartMessage;

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
                                id: 'UserName',
                                readOnly:true,
                                fieldLabel: '手机号码'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: 20,
                                id: 'UserPassword',
                                fieldLabel: '登录密码'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 1,
                                padding: 20,
                                id: 'UserPassword_confirm',
                                fieldLabel: '确认密码'
                            },
                            {
                                xtype: 'textfield',
                                columnWidth: 0.8,
                                padding: 20,
                                id: 'yanzhengma',
                                fieldLabel: '验证码'
                            },
                            {
                                xtype: 'button',
                                margin: '20 0 20 10',
                                iconCls: 'add',
                                text: '发送',
                                id: 'addMessages',
                                handler: function () {
                                    yanzhengma = "";
                                    CS('CZCLZ.Handler.SendMessage', function (retVal) {
                                        if (retVal) {
                                            if (retVal.sign = "true") {
                                                yanzhengma = retVal.yanzhengma;
                                                Ext.Msg.alert("提示", "已成功发送短信！");
                                                StartSendMessage();
                                                Ext.getCmp("addMessages").hide();
                                                Ext.getCmp("sj").show();
                                                return;
                                            } else {
                                                Ext.Msg.alert("提示", retVal.msg);
                                                return false;
                                            }
                                        }
                                    }, CS.onError, 'chongzhimima');
                                }
                            },
                            {
                                xtype: 'label',
                                id: 'sj',
                                margin: '20 0 20 10',
                                hidden: true,
                                html: '60s'
                            },
                            {
                                xtype: 'container',
                                columnWidth: 1,
                                items: [
                                    {
                                        xtype: 'button',
                                        margin: '50 0 20 130',
                                        iconCls: 'enable',
                                        text: '重置密码',
                                        style: 'padding:15px;',
                                        handler: function () {
                                            if (Ext.getCmp("UserName").getValue() == "" || Ext.getCmp("UserName").getValue() == null)
                                            {
                                                Ext.Msg.alert("提示", "手机号码不能为空");
                                                return false;
                                            }
                                            if (Ext.getCmp("UserPassword").getValue() == "" || Ext.getCmp("UserPassword").getValue() == null) {
                                                Ext.Msg.alert("提示", "登录密码不能为空");
                                                return false;
                                            }
                                            if (Ext.getCmp("UserPassword_confirm").getValue() == "" || Ext.getCmp("UserPassword_confirm").getValue() == null) {
                                                Ext.Msg.alert("提示", "确认密码不能为空");
                                                return false;
                                            }
                                            if (Ext.getCmp("UserPassword").getValue() != Ext.getCmp("UserPassword_confirm").getValue())
                                            {
                                                Ext.Msg.alert("提示", "确认密码和登录密码必须一致");
                                                return false;
                                            }
                                            if (Ext.getCmp("yanzhengma").getValue() == "" || Ext.getCmp("yanzhengma").getValue() == null)
                                            {
                                                Ext.Msg.alert("提示", "验证码不能为空");
                                                return false;
                                            }
                                            CS('CZCLZ.Handler.CZMM', function (retVal) {
                                                if (retVal) {
                                                    if (retVal.sign == "true") {
                                                        Ext.Msg.alert("提示", "修改成功！", function () {
                                                            CS('CZCLZ.UserClass.Logout', function (retVal) {
                                                                if (retVal)
                                                                    top.location.href = 'approot/r/login.aspx';
                                                            }, CS.onError);
                                                        });
                                                    } else {
                                                        Ext.Msg.alert("提示", retVal.Msg);
                                                        return false;
                                                    }
                                                }
                                            }, CS.onError, Ext.getCmp("UserName").getValue(), Ext.getCmp("UserPassword").getValue(), "APP");
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

    dataBind();
});

function dataBind()
{
    CS('CZCLZ.Handler.GetUserName', function (retVal) {
        if (retVal)
        {
            Ext.getCmp("UserName").setValue(retVal);
        }
    },CS.onError)
}

var messageTime = 60;

function StartSendMessage() {
    StartMessage = setInterval(function () {
        if (messageTime > 0) {
            --messageTime;
            Ext.getCmp("sj").update("<span>" + messageTime + "s</span>");
        } else {
            Ext.getCmp("addMessages").show();
            Ext.getCmp("sj").hide();
            messageTime = 60;
            clearInterval(StartMessage);
        }
    }, 1000);
}