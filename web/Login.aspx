<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>查货宝后台管理系统</title>
    <style type="text/css">

body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
body,td,th {
	font-size: 12px;
}

    </style>
    

    <script type="text/javascript" src="js/extjs/ext-all.js"></script>
    <link rel="Stylesheet" type="text/css" href="js/extjs/resources/css/ext-all.css" />
    <script type="text/javascript" src="js/extjs/ext-lang-zh_CN.js"></script>

    <script type="text/javascript" src="js/json.js"></script>

    <script type="text/javascript" src="js/cb.js"></script>

    <script type="text/javascript" src="js/fun.js"></script>
    
    <script type="text/javascript" src="JS/city.js"></script>

    <script type="text/javascript">
        function code(v) {
            setTimeout(function() { v.src = "captcha.aspx?vctype=log&r=" + Math.random().toString() }, 1);
        }
    </script>

</head>
<body onkeydown="Send()">
    <form id="form1" runat="server">
<table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td align="center" valign="middle"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="608" align="center" background="images/dl_r1_c1.jpg"><table border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="1000" height="613" align="center" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td><img src="images/dl22_r1_c1.jpg" width="1000" height="86"></td>
              </tr>
            </table>
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                  <td><img src="images/dl22_r2_c1.jpg" width="1000" height="123"></td>
                </tr>
              </table>
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                  <td width="550" background="images/dl22_r3_c1.jpg"><img id="ewm" src="" style="margin-left:280px;margin-top:30px;" /><div style="color:#ffffff;font-size:13px;font-weight:bold;margin-left:265px;padding-top:10px;">打开查货APP“我的”页面扫码登录</div></td><%--二维码 --%>
                  <td valign="top" background="images/dl22_r3_c2.jpg"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                      <td height="96" align="right">&nbsp;</td>
                      <td>&nbsp;</td>
                    </tr>
                    <tr>
                      <td width="45%" height="35" align="right">&nbsp;</td>
                      <td width="55%" align="left"><INPUT style="BORDER-BOTTOM: #cbd0d4 1px solid; BORDER-LEFT: #cbd0d4 1px solid; WIDTH: 120px; HEIGHT: 20px; COLOR: #333333; FONT-SIZE: 14px; BORDER-TOP: #cbd0d4 1px solid; BORDER-RIGHT: #cbd0d4 1px solid" id="username" name="username"  value="<%=username %>"/></td>
                    </tr>
                    <tr>
                      <td height="35" align="right">&nbsp;</td>
                      <td align="left"><INPUT type="password" style="BORDER-BOTTOM: #cbd0d4 1px solid; BORDER-LEFT: #cbd0d4 1px solid; WIDTH: 120px; HEIGHT: 20px; COLOR: #333333; FONT-SIZE: 14px; BORDER-TOP: #cbd0d4 1px solid; BORDER-RIGHT: #cbd0d4 1px solid" id="password" name="password" /></td>
                    </tr>
                    <tr>
                      <td height="35" align="right">&nbsp;</td>
                      <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                          <tr>
                            <td align="left"><INPUT style="BORDER-BOTTOM: #cbd0d4 1px solid; BORDER-LEFT: #cbd0d4 1px solid; WIDTH: 60px; HEIGHT: 20px; COLOR: #333333; FONT-SIZE: 14px; BORDER-TOP: #cbd0d4 1px solid; BORDER-RIGHT: #cbd0d4 1px solid" id="captcha" name="captcha" />&nbsp;<img id="imgcode" src="captcha.aspx?vctype=log" style="cursor: pointer; vertical-align: top;"onclick='code(this);' /></td>
                          </tr>
                      </table></td>
                    </tr>
                  </table>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="31%" height="36">&nbsp;</td>
                        <td width="20%" align="center"><a href="#" onclick="return Login();"><img src="images/1.jpg" width="60" height="24" border="0"></a></td>
                        <td width="21%" align="center"><a href="#" onclick="javascript:var win = window.open('', '_self');win.close();return false;"><img src="images/2.jpg" width="60" height="24" border="0" onclick="chongzhi();"></a></td>
                        <td width="28%">&nbsp;</td>
                      </tr>
                    </table></td>
                </tr>
             </table>
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                  <td><img src="images/dl22_r4_c1.jpg" width="1000" height="116"></td>
                </tr>
              </table></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
</table>
    <script type="text/javascript">
        var newcity = {};

        var province = Ext.create('Ext.data.Store', {
            fields: [
                'ID', 'MC'
            ]
        });

        var city = Ext.create('Ext.data.Store', {
            fields: [
                'ID', 'MC'
            ]
        });

        Ext.define('zhuceWin', {
            extend: 'Ext.window.Window',

            height: document.documentElement.clientHeight/1.5,
            width: document.documentElement.clientWidth / 2,
            layout: {
                type: 'fit'
            },
            title: '注册',

            modal:true,

            initComponent: function () {
                var me = this;

                Ext.applyIf(me, {
                    items: [
                        {
                            xtype: 'panel',
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
                                            xtype: 'combobox',
                                            columnWidth: 0.65,
                                            padding: 20,
                                            valueField: 'ID',
                                            displayField: 'MC',
                                            queryMode: 'local',
                                            store: province,
                                            id: 'User_Province',
                                            fieldLabel: '所在城市',
                                            listeners: {
                                                change: function (data, newValue, oldValue, eOpts) {
                                                    city.loadData(newcity[newValue]);
                                                }
                                            }
                                        },
                                        {
                                            xtype: 'combobox',
                                            columnWidth: 0.35,
                                            padding: 20,
                                            valueField: 'ID',
                                            displayField: 'MC',
                                            queryMode: 'local',
                                            store: city,
                                            id: 'User_City'
                                        },
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
                                            text: '发送'
                                        }
                                    ],
                                    buttonAlign: 'center',
                                    buttons: [
                                        {
                                            text: '注册',
                                            handler: function () {
                                                alert(1);
                                            }
                                        },
                                        {
                                            text: '关闭',
                                            handler: function () {
                                                me.close();
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
    </script>
    <script type="text/javascript">
        window.onload = function () {
            GetEWM();
        };

        function Send() {
            if (window.event.keyCode == 13) {
                Login();
            }
        }
        function Login() {
            CS("CZCLZ.UserClass.Login", function(retVal) {
                if (!retVal) {
                    Ext.MessageBox.show({
                        title: "错误",
                        msg: "登陆失败",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                    code(document.getElementById('imgcode'));
                }
                else {
                    window.location.href = 'Main/Index.aspx';
                }
            }, function(retValue) {
                Ext.MessageBox.show({
                    title: "错误",
                    msg: retValue,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING
                });
                code(document.getElementById('imgcode'));
            },
            document.getElementById('username').value,
            document.getElementById('password').value,
            document.getElementById('captcha').value);
        }
        function GetEWM()
        {
            CS('CZCLZ.Handler.GetEWM', function (retVal) {
                if (retVal) {
                    document.getElementById("ewm").src = "erweima/" + retVal + ".png";
                    EWMLogin(retVal);
                }
            }, CS.onError)
        }
        function Close() {
            var win = window.open('', '_self');
            win.close();
            return false;
        }
        function zhuce() {

            var win = new zhuceWin();
            win.show(null, function () {
                
                var provincesData = [];
                for (var i = 0; i < cityList.provinces.length; i++) {
                    var obj = {};
                    obj.ID = cityList.provinces[i].name;
                    obj.MC = cityList.provinces[i].name;
                    provincesData.push(obj);

                    var cityData = [];
                    for (var j = 0; j < cityList.provinces[i].citys.length; j++) {
                        var obj2 = {};
                        obj2.ID = cityList.provinces[i].citys[j];
                        obj2.MC = cityList.provinces[i].citys[j];
                        cityData.push(obj2);
                    }
                    newcity[cityList.provinces[i].name] = cityData;
                }
                province.loadData(provincesData);

            });
        }
        function chongzhi() {
            document.getElementById('username').value = "";
            document.getElementById('password').value = "";
            document.getElementById('captcha').value = "";
        }
        function EWMLogin(id)
        {
            setInterval(function () {
                ACS('CZCLZ.Handler.LoginByEWM', function (retVal) {
                    if (retVal) {
                        window.location.href = 'Main/Index.aspx';
                    }
                }, CS.onError, id)
            }, 2000);
        }
    </script>

    </form>
</body>
</html>
