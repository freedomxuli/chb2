using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main_Index : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetJSVariant("UserName", SystemUser.CurrentUser.UserName);
        //this.SetJSVariant("UserName", "18912303195");
        this.SetJSVariant("itemmsg", MenuControl.GenerateMenuByPrivilege());
        //if (SystemUser.CurrentUser.User_DM != "")
        //{
        //    this.SetJSVariant("UserDW", "(" + SystemUser.CurrentUser.User_DM + ")");
        //}
        //else
        //{
        //    this.SetJSVariant("UserDW", "");
        //}

        this.SetJSVariant("logo", "logokh.gif");
        this.SetJSVariant("help", "help.htm");
        //this.SetJSVariant("DeptName", SystemUser.CurrentUser.OrgName);
        var cu = SystemUser.CurrentUser;
        this.SetJSVariant("CZCLZUser", new { UserID = cu.UserID, UserName = cu.UserName });
    }
}
