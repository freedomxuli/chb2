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
                  <td width="550"><img src="images/dl22_r3_c1.jpg" width="550" height="288"></td>
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
                        <td width="21%" align="center"><a href="#" onclick="javascript:var win = window.open('', '_self');win.close();return false;"><img src="images/2.jpg" width="60" height="24" border="0"></a></td>
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
        function Close() {
            var win = window.open('', '_self');
            win.close();
            return false;
        }
    </script>

    </form>
</body>
</html>
