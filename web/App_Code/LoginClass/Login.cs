using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartFramework4v2.Web.WebExecutor;

/// <summary>
///Login 的摘要说明
/// </summary>
[CSClass("UserClass")]
public class UserClass
{
    [CSMethod("Login")]
    public bool Login(string username, string password, string captcha)
    {
        if (username.Trim() == string.Empty)
        {
            throw new Exception("用户名不能为空！");
        }
        if (password.Trim() == string.Empty)
        {
            throw new Exception("密码不能为空！");

        }
        if (captcha.Trim() == string.Empty)
        {
            throw new Exception("验证码不能为空！");
        }
        if (!VerCode.CheckCode("log", captcha))
        {
            throw new Exception("验证码验证错误！");
        }
        var su = SystemUser.Login(username, password);
        if (su != null)
        {
            HttpCookie cookie = new HttpCookie("login_Username", username)
            {
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            return true;
        }
        else
        {
            return false;
        }
    }
    [CSMethod("Logout")]
    public bool QuitSystem()
    {
        try
        {
            SystemUser.Logout();
        }
        catch
        {
        }
        return true;
    }
}
