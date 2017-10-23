<%@ Page Language="C#" %>
<%@ Import Namespace="SmartFramework4v2.Web.Common" %>
<!DOCTYPE html>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["session_id"]))
        {
            var retReader = MethodExcutor.CallServer("SOASERVER.SOAPortal.LoginClass.loginVerifyWithAdapter",
                Request["session_id"], "CZPJSD");
            if (!retReader.IsEmpty)
            {
                var userId = retReader.ToString();
                if (SystemUser.GetUserByID(userId) == null)
                {
                    Response.ContentType = "text/plain";
                    Response.Write("未找到关联用户，无法进行验证！");
                    Response.End();
                }
                else
                {
                    HttpCookie hc = new HttpCookie("login_Username");
                    hc.HttpOnly = true;
                    hc.Value = SystemUser.GetUserByID(userId).LoginName;
                    Response.Cookies.Add(hc);
                    HttpCookie hcUserId = new HttpCookie("userid");
                    hcUserId.HttpOnly = true;
                    hcUserId.Value = userId;
                    Response.Cookies.Add(hcUserId);
                    Response.Redirect("main/index.aspx");
                }
            }
            else
            {
                Response.ContentType = "text/plain";
                Response.Write("未找到关联用户，无法进行验证！");
                Response.End();
            }
        }
        else
        {
            Response.ContentType = "text/plain";
            Response.Write("未找到关联用户，无法进行验证！");
            Response.End();
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
