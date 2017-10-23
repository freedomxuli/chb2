using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartFramework4v2.Web.UI;

/// <summary>
///BasePage 的摘要说明
/// </summary>
public class BasePage : SmartPage
{
    private SystemUser m_UserCache;
    public BasePage()
    {
        this.PreInit += BasePage_PreInit;
    }
    ///<summary> 
    ///获得当前用户对象 
    ///</summary> 
    ///<returns>SystemUser对象</returns> 
    protected new SystemUser User
    {
        get
        {
            if (m_UserCache == null) m_UserCache = SystemUser.CurrentUser;
            return m_UserCache;
        }
    }
    private void BasePage_PreInit(Object sender, System.EventArgs e)
    {
        try
        {
            Type tp = this.GetType();
            object[] objs = null;
            objs = tp.GetCustomAttributes(typeof(UserCheck), true);
            if (objs.Length > 0)
            {
                if (User == null)
                {
                    Response.Redirect(VirtualPathUtility.MakeRelative(Request.Url.AbsolutePath, "~/login.aspx"));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
/// <summary> 
/// 该页面需要用户登录并获得相应的权限 
/// </summary> 
/// <remarks></remarks> 
public sealed class UserCheck : Attribute
{

    private string _PrivilegeObjects = "";
    public UserCheck()
    {

    }

    /// <summary> 
    /// 
    /// </summary> 
    /// <param name="PrivilegeObjects">由 逗号 分割的权限对象列表</param> 
    /// <remarks></remarks> 
    public UserCheck(string PrivilegeObjects)
    {
        if (PrivilegeObjects != null)
        {
            _PrivilegeObjects = PrivilegeObjects;
        }
    }

    public string GetPrivilegeObjects()
    {
        return _PrivilegeObjects;
    }
}
