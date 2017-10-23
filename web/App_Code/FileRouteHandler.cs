using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SmartFramework4v2.Data.SqlServer;
using System.Data.SqlClient;
using SmartFramework4v2.Web.Common;
using System.Text.RegularExpressions;

public sealed class FileRouteExecutor :
    IRouteExecutor
{
    #region IRouteExecutor 成员
    public void Execute(MatchCollection ms, string QueryString, HttpContext context)
    {
        var Request = context.Request;
        var Server = context.Server;
        var Response = context.Response;
        string FileGuid = ms[0].Groups[1].Value as string;
        string FileName = ms[0].Groups[2].Value as string;
        if (!Directory.Exists(Server.MapPath("~/files")))
            Directory.CreateDirectory(Server.MapPath("~/files"));
        string newfilepath = "";
        string fid = string.Empty ;
        fid = FileGuid;
        if (fid != string.Empty)
        {
            string truepath = "~/files/" + FileGuid + "." + FileName;
            try
            {
                if (!File.Exists(Server.MapPath(truepath)))
                {
                    using (DBConnection dbc = new DBConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "select FJ_NR from tb_b_FJ where fj_id=@fj_id";
                        cmd.Parameters.AddWithValue("@fj_id", fid);
                        //cmd.Parameters.AddWithValue("@file_name", FileName);
                        object o = dbc.ExecuteScalar(cmd);
                        if (o != null)
                        {
                            byte[] bytes = (byte[])o;
                            using (Stream iStream = new FileStream(Server.MapPath(truepath),
                                FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                iStream.Write(bytes, 0, bytes.Length);
                            }
                            newfilepath = truepath;
                        }
                    }
                }
                else
                    newfilepath = truepath;
            }
            catch
            {

            }
        }
        if (newfilepath == "")
            newfilepath = "~/files/" + FileGuid + '/' + FileName;
        Response.AddHeader("Content-Disposition", "attachment");
        HttpContext.Current.RewritePath(newfilepath);
    }

    #endregion

    #region IRouteExecutor 成员

    private Regex m_Regex = new Regex(@"files/(\w{8}-\w{4}-\w{4}-\w{4}-\w{12})/([^/\:*?""<>]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public Regex GetRegex()
    {
        return m_Regex;
    }

    #endregion
}

