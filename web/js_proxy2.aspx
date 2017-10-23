<%@ Page Language="C#" AutoEventWireup="true" CodeFile="js_proxy2.aspx.cs" Inherits="js_proxy" %>
<%@ Import Namespace="SmartFramework4v2.Web.Common.JSON" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Text" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="IE=9" http-equiv="X-UA-Compatible" />
    <script runat="server" type="text/C#">
        string GetVPath(string p)
        {
            return VirtualPathUtility.MakeRelative(Request.Url.AbsolutePath, p);
        }

        bool IsDebugMode
        {
            get
            {
                var asm = this.GetType().Assembly;
                var attrs = asm.GetCustomAttributes(typeof(DebuggableAttribute), true);
                if (attrs.Length > 0)
                {
                    DebuggableAttribute da = attrs[0] as DebuggableAttribute;
                    return da.IsJITTrackingEnabled;
                }
                return false;
            }
        }
    </script>

    <%
        JSONConvert jc = new JSONConvert();
        System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);
        sb.AppendFormat("<link type='text/css' href='{0}' rel='Stylesheet' />", GetVPath("~/js/extjs/resources/css/ext-all.css"));
        sb.AppendFormat("<link type='text/css' href='{0}' rel='Stylesheet' />", GetVPath("~/css/ext-patch.css"));
        sb.AppendFormat("<link type='text/css' href='{0}' rel='Stylesheet' />", GetVPath("~/css/ext-patch2.css"));

        sb.AppendFormat("<link type='text/css' href='{0}' rel='Stylesheet' />", GetVPath("~/css/icon.css"));
        sb.AppendFormat("<link type='text/css' href='{0}' rel='Stylesheet' />", GetVPath("~/css/Main.css"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>",
            GetVPath(IsDebugMode ? "~/js/extjs/ext-all-debug-w-comments.js" : "~/js/extjs/ext-all.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/SFW4Uploader.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/json.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/SFW4Store.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/cb.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/fun.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/extjs/ext-lang-zh_CN.js"));
        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath("~/js/FrameStack.js"));
                    
                                      
        Hashtable ht = new Hashtable();
        foreach (var key in Request.QueryString.AllKeys)
        {
            if (key != "js")
            {
                ht[key] = Request[key];
            }
        }
        JSONConvert convert = new JSONConvert();
        sb.AppendFormat("\r\n<script type='text/javascript'>var queryString = {0};</script>",
            Regex.Replace(jc.CreateJSON(ht), "</script>", "<' + '/script>", RegexOptions.Multiline | RegexOptions.IgnoreCase));
        
        

        sb.AppendFormat("\r\n<script type='text/javascript' src='{0}'></script>", GetVPath(Request["js"]));        
        Response.Write(sb.ToString());
        
    %>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
