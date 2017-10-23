using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : SmartFramework4v2.Web.UI.SmartPage
{
    public string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["login_Username"] != null)
        {
            username = Request.Cookies["login_Username"].Value;
        }
    }
}
