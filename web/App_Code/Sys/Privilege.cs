using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartFramework4v2.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

/// <summary>
///Privilege 的摘要说明
/// </summary>
public class PrivilegeDescription
{
    public readonly string PrivilegeName;
    public readonly string ModuleName;
    /// <summary> 
    /// 申明方法，初始化权限 
    /// </summary> 
    /// <remarks></remarks> 
    static PrivilegeDescription()
    {
        DBConnection conn = new DBConnection();
        using (conn)
        {
            conn.ExecuteNonQuery("truncate table tb_b_YH_QX");
            var asm = typeof(PrivilegeDescription).Assembly;
            var pns = "Smart.SystemPrivilege";
            foreach (var tp in asm.GetTypes())
            {
                if (tp.FullName.IndexOf(pns) >= 0)
                {
                    Activator.CreateInstance(tp);
                }
            }
        }

    }
    /// <summary> 
    /// 申明权限方法 
    /// </summary> 
    /// <param name="szModuleName">权限模块名称</param> 
    /// <param name="szPrivilegeName">权限名称</param> 
    /// <remarks></remarks> 
    public PrivilegeDescription(string szModuleName, string szPrivilegeName, int orderno)
    {
        PrivilegeName = szPrivilegeName;
        ModuleName = szModuleName;
        DBConnection conn = new DBConnection();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO tb_b_YH_QX VALUES(@PrivilegeCode,@PrivilegeName,@ModuleName,@OrderNO)";
        cmd.Parameters.Add("@PrivilegeCode", SqlDbType.UniqueIdentifier).Value = (Guid)this;
        cmd.Parameters.Add("@PrivilegeName", SqlDbType.VarChar, 100).Value = PrivilegeName;
        cmd.Parameters.Add("@ModuleName", SqlDbType.VarChar, 100).Value = ModuleName;
        cmd.Parameters.Add("@OrderNO", SqlDbType.Int).Value = orderno;
        using (conn)
        {
            conn.ExecuteNonQuery(cmd);
        }
    }

    /// <summary> 
    /// 对模块权限进行转换 
    /// </summary> 
    /// <param name="pd">权限描述</param> 
    /// <returns>Guid</returns> 
    /// <remarks></remarks> 
    public static implicit operator Guid(PrivilegeDescription pd)
    {
        string sM = SmartFramework4v2.Web.Security.SecurityFunction.MD5Hash(pd.ModuleName);
        string sP = SmartFramework4v2.Web.Security.SecurityFunction.MD5Hash(pd.PrivilegeName);
        return new Guid(SmartFramework4v2.Web.Security.SecurityFunction.RawMD5Hash(System.Text.Encoding.UTF8.GetBytes(sM + sP)));
    }
    /// <summary> 
    /// 将Guid转换成String 
    /// </summary> 
    /// <returns>String</returns> 
    /// <remarks></remarks> 
    public override string ToString()
    {
        return ((Guid)this).ToString();
    }

    public static DSQX.tb_b_YHQXDataTable GetAllPrivlieges()
    {
        DBConnection conn = new DBConnection();
        using (conn)
        {
            return conn.GetDataTable<DSQX.tb_b_YHQXDataTable>();
        }
    }
    public static DataTable PrivilegeType()
    {
        DBConnection dbcon = new DBConnection();
        using (dbcon)
        {
            string sql = "select distinct(substring(ModuleName,1,CHARINDEX('-',ModuleName))) as ModuleName from tb_b_YH_QX";
            return dbcon.ExecuteDataTable(sql);
        }
    }

    public static DataTable GetPrivilegeByModuleName(string strModuleName)
    {
        DBConnection dbcon = new DBConnection();
        using (dbcon)
        {
            string sql = "select PrivilegeCode,PrivilegeName, substring(ModuleName,CHARINDEX('-',ModuleName)+1,(len(ModuleName)-CHARINDEX('-',ModuleName))) as  ModuleName "
                + " from tb_b_YH_QX where ModuleName like '" + strModuleName + "%' order by ModuleName,OrderNO";
            return dbcon.ExecuteDataTable(sql);
        }
    }

    /// <summary>
    /// 判断该栏目是否需要
    /// </summary>
    /// <param name="lmmc"></param>
    /// <returns></returns>
    public static bool IsNeedAreaPrivilege(string lmmc)
    {
        using(DBConnection dbc=new DBConnection())
        {
            string sql = "select MK_ISQY from tb_b_MK where MK_MC like '%"+lmmc.Trim()+"%'";
            return Convert.ToBoolean(dbc.ExecuteScalar(sql));
        }
    }
}
