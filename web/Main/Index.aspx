<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Main_Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .x-grid-record-red table{  
             color: #FF0000;  
         } 
         .x-grid-record-green table{  
             color: #0C0;  
         } 
         .x-grid-record-magenta table{  
             color: #FF00FF;  
         } 
         .x-grid-record-orange table{  
             color: #FFA500;
             text-decoration:line-through;  
         } 
    </style>
    <link rel="Stylesheet" href="../js/extjs/resources/css/ext-all.css" />

    <link rel="Stylesheet" href="../CSS/ext-patch.css" />
    
    <link rel="Stylesheet" href="../CSS/Main.css" />
    <link href="../CSS/icon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../js/extjs/ext-all.js"></script>

    <script type="text/javascript" src="../js/extjs/ext-lang-zh_CN.js"></script>
    
    <script type="text/javascript" src="../js/json.js"></script>
    <script type="text/javascript" src="../js/fun.js"></script>
    <script type="text/javascript" src="../js/cb.js"></script>
    <script type="text/javascript">
        function modifyPwd() {
            var winMP = new Ext.window.Window({
                height: 169,
                width: 318,
                modal: true,
                draggable: false,
                layout: {
                    type: 'fit'
                },
                title: '修改密码',
                items: [
                {
                    xtype: 'form',
                    id: "frmPWD",
                    border: false,
                    bodyPadding: 10,
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: '原密码',
                            labelWidth: 60,
                            anchor: '100%',
                            allowBlank: false,
                            name: 'oldpwd',
                            inputType: 'password'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '新密码',
                            labelWidth: 60,
                            anchor: '100%',
                            allowBlank: false,
                            inputType: 'password',
                            name: 'newpwd',
                            id: 'firstPWD'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '确认密码',
                            labelWidth: 60,
                            anchor: '100%',
                            allowBlank: false,
                            inputType: 'password',
                            id: 'secondPWD',
                            vtype: 'repwd'
                        }
                    ]
                }
                ],
                buttons: [
                    {
                        text: '确定',
                        handler: function() {
                            var form = Ext.getCmp("frmPWD");
                            if (form.form.isValid()) {
                                var values = form.form.getValues(false);
                                CS("CZCLZ.SystemUser.ModifyPassword", function() {            
                                    Ext.MessageBox.alert("确认", "修改成功");
                                }, CS.onError, values["oldpwd"], values["newpwd"]);
                            }
                        }
                    },
                    {
                        text: '取消',
                        handler: function() { this.up("window").close(); }
                    }
                ]
            });
            Ext.apply(Ext.form.field.VTypes, {
                repwd: function(val, field) {
                    return Ext.getCmp('secondPWD').getValue() == Ext.getCmp('firstPWD').getValue();
                },
                repwdText: '两次输入的密码必须相同'
            });
            winMP.show();
        }
        var strHelp = '';
        
        
    </script>
    
    <script src="../js/Main/Index.js" type="text/javascript" defer=defer></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
</html>
