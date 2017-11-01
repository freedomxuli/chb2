<%@ WebHandler Language="C#" Class="App_Login" %>

using System;
using System.Web;

public class App_Login : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        Handler login = new Handler();
        login.InsertGuid(context.Request["guid"].ToString(), context.Request["username"].ToString());
        context.Response.Write("true");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}