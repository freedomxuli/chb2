using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
[UserCheck()]
public partial class js_proxy : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var cu = SystemUser.CurrentUser;
        this.SetJSVariant("CZCLZUser", new { UserID = cu.UserID, UserName = cu.UserName });
    }
}
