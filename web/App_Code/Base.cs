using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartFramework4v2.Web.WebExecutor;
using SmartFramework4v2.Data;
using SmartFramework4v2.Data.SqlServer;
using System.Collections;
using System.Data;
using SmartFramework4v2.Web.Common.JSON;
using System.IO;
using Aspose.Cells;
using System.Data.SqlClient;
using System.Text;

/// <summary>
///Base 的摘要说明
/// </summary>
[CSClass("Base")]
public class Base
{
    public Base()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    [CSMethod("GetCatWhere")]
    public DataTable GetCatWhere()
    {
        using (DBConnection dbc = new DBConnection())
        {
            string str = "select cat_name,cat_code from tb_b_ProCat where cat_DelFlag=0 and LEN(cat_code)=3 order by cat_code";
            DataTable dt = dbc.ExecuteDataTable(str);
            DataRow dr = dt.NewRow();
            dr["cat_name"] = "全部";
            dr["cat_code"] = "";

            dt.Rows.InsertAt(dr, 0);

            return dt;
        }
    }
}
