<%@ WebHandler Language="C#" Class="InterFaceHandler" %>

using System;
using System.Web;
using System.Collections;

public class InterFaceHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string str = "";
        switch (context.Request["action"])
        { 
            case "TuiDanDeciceIsClose":
                str = TuiDanDeciceIsClose(context);
                break;
        }
        context.Response.Write(str);
        context.Response.End();
    }

    //退单前判断是否能解绑该设备
    public string TuiDanDeciceIsClose(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "请先解绑该设备，再退单！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.TuiDanDeciceIsCloseByApp(context.Request["GpsDeviceID"], context.Request["UserName"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "可解绑！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}