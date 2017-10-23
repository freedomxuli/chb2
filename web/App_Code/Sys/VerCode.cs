using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

/// <summary>
///VerCode 的摘要说明
/// </summary>
public static  class VerCode
{
    private struct CodeInf
    {
        public string Code;
        public DateTime Time;
    }
    private static SortedList<Guid, CodeInf> CodeCache = new SortedList<Guid, CodeInf>();
    private static System.Threading.Timer _CheckTimer;
    static VerCode()
    {
        _CheckTimer = new System.Threading.Timer(delegate(object o)
            {
                lock (CodeCache)
                {
                    var RemoveList = from kp in CodeCache
                                     where
                                         (DateTime.Now - kp.Value.Time).TotalMinutes > 2
                                     select kp.Key;
                    foreach (var id in RemoveList.ToArray())
                    {
                        CodeCache.Remove(id);
                    }
                }
            }, null, 120000, 120000);
    }

    public static bool CheckCode(string vctype, string code)
    {
        var Request = System.Web.HttpContext.Current.Request;
        if (string.IsNullOrEmpty(Request.Cookies["vc_" + vctype].Value))
            return false;
        Guid id;
        try
        {
            id = new Guid(Request.Cookies["vc_" + vctype].Value);
        }
        catch
        {
            return false;
        }
        lock (CodeCache)
        {
            if (CodeCache.ContainsKey(id))
            {
                bool bcheck;
                if (((DateTime.Now - CodeCache[id].Time).TotalMinutes <= 2) && code.ToLower() == CodeCache[id].Code.ToLower())
                    bcheck = true;                
                else
                    bcheck = false;
                CodeCache.Remove(id);
                return bcheck;
            }
        }
        return false;
    }

    public static void NewCode(string vctype)
    {
        CaptchaImage validate = new CaptchaImage(12, 50);
        validate.strFont = "宋体";
        validate.nFontSize = 13;
        MemoryStream stream = validate.GetCode(1, 4, 58, 22);
        Guid id;
        lock (CodeCache)
        {

            id = Guid.NewGuid();
            CodeCache[id] = new CodeInf { Code = validate.strValidate, Time = DateTime.Now };
        }
        var Response = System.Web.HttpContext.Current.Response;
        Response.Cookies.Add(new HttpCookie("vc_" + vctype, id.ToString()) { HttpOnly = true });
        Response.ContentType = "image/jpeg";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Clear();
        Response.BinaryWrite(stream.ToArray());
        Response.Flush();
        Response.End();
    }
}
