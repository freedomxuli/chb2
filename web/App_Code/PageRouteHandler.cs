using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartFramework4v2.Web.Common;
using System.Text.RegularExpressions;

/// <summary>
///PageRouteHandler 的摘要说明
/// </summary>
public sealed class JSRouteExecutor :
    IRouteExecutor
{

    public void Execute(MatchCollection ms, string QueryString, HttpContext context)
    {
         var Request = context.Request;
        var Server = context.Server;
        var Response = context.Response;
        if (ms[0].Groups.Count == 2)
        {
            var JSUrl = "~/js/" +ms[0].Groups[1].Value+".js";
            if (System.IO.File.Exists(Server.MapPath(JSUrl)))
            {
                var PageUrl = "~/js_proxy.aspx";
                Server.Execute(PageUrl + "?js=" + JSUrl + (QueryString == "" ? "" : "&" + QueryString));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            JSUrl = "~/js2/" + ms[0].Groups[1].Value + ".js";
            if (System.IO.File.Exists(Server.MapPath(JSUrl)))
            {
                var PageUrl = "~/js_proxy2.aspx";
                Server.Execute(PageUrl + "?js=" + JSUrl + (QueryString == "" ? "" : "&" + QueryString));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
    #region IRouteExecutor 成员
    private Regex m_Regex = new Regex(@"page/(.*).html", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public Regex GetRegex()
    {
        return m_Regex;
    }
    #endregion
}


public sealed class AppRoot_RouteExecutor :
    IRouteExecutor
{

    #region IRouteExecutor 成员

    public void Execute(MatchCollection ms, string QueryString, HttpContext context)
    {
        var Request = context.Request;
        var Server = context.Server;
        var Response = context.Response;
        if (ms[0].Groups[1].Value == "r")
            Response.Redirect("~/" + ms[0].Groups[2].Value + '?' + QueryString, false);
        else
        {
            //DefaultHttpHandler handler = new DefaultHttpHandler();
            //HttpContext.Current.RewritePath("~/" + ms[0].Groups[2].Value);
            //System.Threading.ManualResetEvent evt = new System.Threading.ManualResetEvent(false);
            //handler.BeginProcessRequest(HttpContext.Current, delegate(IAsyncResult ar)
            //{
            //    handler.EndProcessRequest(ar);
            //    evt.Set();
            //}, null);
            //evt.WaitOne();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            //}
            //HttpContext.Current.RewritePath("~/" + ms[0].Groups[2].Value + QueryString);
           
            Server.Execute("~/" + ms[0].Groups[2].Value + '?' + QueryString);

        }
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion

    #region IRouteExecutor 成员

    private Regex m_Regex = new Regex(@"approot/([rd])/((?!approot).+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public Regex GetRegex()
    {
        return m_Regex;
    }

    #endregion
}
