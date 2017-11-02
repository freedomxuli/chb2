using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pay_wxPayHandler : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WxPayAPI.ResultNotify resultNotify = new WxPayAPI.ResultNotify(this);
        resultNotify.ProcessNotify();
    }
}