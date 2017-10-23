using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartFramework4v2.Data.SqlServer;
using System.Data;
/// <summary>
/// DBHelper 的摘要说明
/// </summary>
public static class DBHelper
{
    public static void InsertTable(this DBConnection dbc, DataRow dr, bool IgnoreNULL = true)
    {
        var tb = dr.Table.Clone();
        tb.Rows.Add(dr.ItemArray);
        if (IgnoreNULL)
        {
            foreach (DataColumn col in dr.Table.Columns)
            {
                if (dr[col.ColumnName] == DBNull.Value)
                {
                    tb.Columns.Remove(col.ColumnName);
                }
            }
        }
        
        dbc.InsertTable(tb);
    }


    public static int UpdateTable(this DBConnection dbc, DataRow dr, bool IgnoreNULL = true)
    {
        var tb = dr.Table.Clone();
        tb.Rows.Add(dr.ItemArray);
        return dbc.UpdateTable(tb, IgnoreNULL);
    }


}