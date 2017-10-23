using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SmartFramework4v2.Data.SqlServer;
using System.Text;
using SmartFramework4v2.Web.WebExecutor;

/// <summary>
///UserClass 的摘要说明
/// </summary>
[CSClass("SystemUser")]
public class SystemUser
{


    private DataRow m_data;

    public string UserID
    {
        get { return m_data["YH_ID"].ToString(); }
    }
    public string LoginName
    {
        get { return m_data["YH_DLM"].ToString(); }
    }
    public string UserName
    {
        get { return m_data["YH_XM"].ToString(); }
    }
    public string UserType
    {
        get
        {
            return m_data["YH_TP"].ToString();
        }
    }



    public static string GetDWIDByUserID(string uid)
    {
        using (var dbc = new DBConnection())
        {
            string sqlStr = "select DW_ID from tb_b_User_Dw_Gl where delflag=0 and User_ID='" + uid + "'";
            return dbc.ExecuteScalar(sqlStr).ToString();
        }
    }

    public enum UserTypes
    {
        检测人员 = 0,
        采价员 = 1,
        配供中心 = 2
    }

    public static SystemUser Login(string username, string password)
    {
        using (DBConnection dbc = new DBConnection())
        {
            string sqlStr = "select UserID YH_ID, UserName YH_DLM,UserName YH_XM from [chahuobao].[dbo].[User] where UserName=@UserName and UserPassword=@UserPassword";
            SqlCommand cmd = new SqlCommand(sqlStr);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@UserPassword", password);
            var dtUser = dbc.ExecuteDataTable(cmd);
            SystemUser su = new SystemUser();
            if (dtUser.Rows.Count > 0)
            {
                su.m_data = dtUser.Rows[0];

                HttpContext.Current.Response.Cookies.Add(new HttpCookie("userid", dtUser.Rows[0]["YH_ID"].ToString()) { HttpOnly = true });
                return su;
            }
            return null;
        }
    }
    public static void Logout()
    {
        if (SystemUser.CurrentUser != null)
        {
            var u = SystemUser.CurrentUser;
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("userid", "") { Expires = DateTime.MinValue });
        }
    }
    public static SystemUser CurrentUser
    {
        get
        {
            try
            {
                var cookf = from string k in HttpContext.Current.Response.Cookies.Keys where k == "userid" select k;
                if (cookf.Count() > 0)
                    return GetUserByID(HttpContext.Current.Response.Cookies["userid"].Value);
                if (HttpContext.Current.Request.Cookies["userid"] != null)
                    return GetUserByID(HttpContext.Current.Request.Cookies["userid"].Value);
                else if (HttpContext.Current.Request["userid"] != null)
                    return GetUserByID(HttpContext.Current.Request["userid"]);
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }

    [CSMethod("ModifyPassword")]
    public void ModifyPassword(string oldPassword, string newPassword)
    {
        var u = SystemUser.CurrentUser;
        string UserID = u.UserID;
        if (string.IsNullOrEmpty(newPassword))
            throw new Exception("密码不能为空");
        using (DBConnection dbc = new DBConnection())
        {
            try
            {
                SqlCommand cmd = new SqlCommand("update tb_b_Users set Password = @newpwd, updatetime=getdate() where User_ID=@yh_id and Password=@oldpwd");
                cmd.Parameters.AddWithValue("@newpwd", newPassword);
                cmd.Parameters.AddWithValue("@yh_id", UserID);
                cmd.Parameters.AddWithValue("@oldpwd", oldPassword);

                if (dbc.ExecuteNonQuery(cmd) == 0)
                {
                    throw new Exception("原密码错误");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public static SystemUser GetUserByID(string userid)
    {
        string sqlStr = "select UserID YH_ID, UserName YH_DLM,UserName YH_XM from [chahuobao].[dbo].[User] where  UserID = @yh_id";
        SqlCommand cmd = new SqlCommand(sqlStr);
        cmd.Parameters.AddWithValue("@yh_id", userid);
        using (DBConnection dbc = new DBConnection())
        {
            var dtb = dbc.ExecuteDataTable(cmd);
            if (dtb.Rows.Count == 0)
            {
                throw new Exception("无效的用户ID");
            }
            SystemUser su = new SystemUser();
            su.m_data = dtb.Rows[0];
            return su;
        }
    }
    public static SystemUser GetUserByUserName(string username)
    {
        string sqlStr = "select User_ID YH_ID, LoginName YH_DLM,User_XM YH_XM,QY_ID,User_DM,case when '7E53492E-CF66-411F-83C4-7923467F59B4' in (select JS_ID from tb_b_User_JS_Gl where User_ID = tb_b_Users.User_ID and delflag=0)  then '1' else '0' end YH_TP from tb_b_Users where User_DelFlag=0  and LoginName = @yh_dlm";
        SqlCommand cmd = new SqlCommand(sqlStr);
        cmd.Parameters.AddWithValue("@yh_dlm", username);
        using (DBConnection dbc = new DBConnection())
        {
            var dtb = dbc.ExecuteDataTable(cmd);
            if (dtb.Rows.Count == 0)
            {
                throw new Exception("无效的用户ID");
            }
            SystemUser su = new SystemUser();
            su.m_data = dtb.Rows[0];
            return su;
        }
    }
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="loginname">登录名</param>
    /// <param name="password">密码</param>
    /// <param name="dwid">所属单位ID</param>
    /// <returns>是否创建成功</returns>
    public static bool CreateUser(string 登陆名, string 姓名, string 密码, string 部门ID, string 电话, string 职务, string 手机, string 电子邮件, string 地址)
    {
        string sqlStr = "insert into TZJGJC_T_YH (User_ID,LoginName,Password,User_XM,User_ZW,User_DH,User_SJ,User_Email,User_DZ,User_Enable,User_DelFlag,addtime,updatetime,updateuser) " +
                "values (@User_ID,@LoginName,@Password,@User_XM,@User_ZW,@User_DH,@User_SJ,@User_Email,@User_DZ,@User_Enable,@User_DelFlag,@addtime,@updatetime,@updateuser)";
        var YHID = Guid.NewGuid().ToString();
        var EditUser = SystemUser.CurrentUser;
        SqlCommand cmd = new SqlCommand(sqlStr);
        cmd.Parameters.AddWithValue("@User_ID", YHID);
        cmd.Parameters.AddWithValue("@LoginName", 登陆名);
        cmd.Parameters.AddWithValue("@Password", 密码);
        cmd.Parameters.AddWithValue("@User_XM", 姓名);
        cmd.Parameters.AddWithValue("@User_ZW", 职务);
        cmd.Parameters.AddWithValue("@User_DH", 电话);
        cmd.Parameters.AddWithValue("@User_SJ", 手机);
        cmd.Parameters.AddWithValue("@User_Email", 电子邮件);
        cmd.Parameters.AddWithValue("@User_DZ", 地址);
        cmd.Parameters.AddWithValue("@User_Enable", 0);
        cmd.Parameters.AddWithValue("@User_DelFlag", 0);
        cmd.Parameters.AddWithValue("@addtime", DateTime.Now);
        cmd.Parameters.AddWithValue("@updatetime", DateTime.Now);
        cmd.Parameters.AddWithValue("@updateuser", EditUser.UserID);
        using (DBConnection dbc = new DBConnection())
        {
            var retInt = dbc.ExecuteNonQuery(cmd);
            if (retInt > 0)
                return true;
            return false;
        }
    }
    #region 权限部分
    /// <summary> 
    /// 检查多种权限 
    /// </summary> 
    /// <param name="pids">权限项拼接的字符串</param> 
    /// <returns>Boolean</returns> 
    /// <remarks></remarks> 
    public bool CheckPrivilege(params Guid[] pids)
    {
        StringBuilder sqlCmd = new StringBuilder();
        SqlCommand cmd = new SqlCommand();
        sqlCmd.Append("SELECT count(*) FROM tb_b_YH_YHQX WHERE UserID=@UserID ");
        cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = new Guid(UserID);
        if (pids != null)
        {
            sqlCmd.Append(" AND (");
            for (int i = 0; i <= pids.Length - 1; i++)
            {
                sqlCmd.Append("PrivilegeCode=@PrivilegeCode");
                sqlCmd.Append(i);
                cmd.Parameters.Add("@PrivilegeCode" + i, SqlDbType.UniqueIdentifier).Value = pids[i];
                if (i < pids.Length - 1) sqlCmd.Append(" OR ");
            }
            sqlCmd.Append(" )");
        }
        cmd.CommandText = sqlCmd.ToString();
        DBConnection conn = new DBConnection();
        int iRet = 0;
        using (conn)
        {
            iRet = Convert.ToInt32(conn.ExecuteScalar(cmd));
        }
        if (pids == null)
        {
            return iRet == 1;
        }
        else
        {
            return iRet > 0;
        }
        //Return iRet = pids.Length 
    }

    /// <summary> 
    /// 检测权限字符串 
    /// </summary> 
    /// <param name="str">权限字符串</param> 
    /// <returns>Boolean</returns> 
    /// <remarks></remarks> 
    public bool CheckPrivilegeString(string str)
    {
        if (str == null) return true;
        if (str.Length == 0) return true;
        string[] pstrs = str.Split(',');
        List<Guid> lst = new List<Guid>();
        try
        {
            foreach (string pstr in pstrs)
            {
                string clsName = null;
                int iLastDot = pstr.LastIndexOf(".");
                if (iLastDot == -1) return false;
                clsName = pstr.Substring(0, iLastDot);
                Type tp = Type.GetType(clsName);
                System.Reflection.FieldInfo fld = default(System.Reflection.FieldInfo);
                fld = tp.GetField(pstr.Substring(iLastDot + 1, pstr.Length - iLastDot - 1), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (fld == null) return false;
                PrivilegeDescription pd = (PrivilegeDescription)fld.GetValue(null);
                lst.Add(pd);
            }
            return CheckPrivilege(lst.ToArray());
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary> 
    /// 新增权限 
    /// </summary> 
    /// <param name="Privilege">权限编号</param> 
    /// <remarks></remarks> 
    public void AddPriviledge(Guid Privilege)
    {
        DBConnection conn = new DBConnection();
        SqlCommand cmd = new SqlCommand();
        try
        {
            using (conn)
            {
                conn.ExecuteNonQuery(string.Format("INSERT INTO tb_b_YH_YHQX (UserID,PrivilegeCode) VALUES('{0}','{1}')", UserID.ToString(), Privilege.ToString()));
            }
        }
        catch (Exception ex)
        {
            if (!DBConnection.IsUniquePKConflict(ex))
            {
                if (DBConnection.FKRestrictConflict(ex))
                {
                    throw new Exception("无效的用户");
                }
                else
                {
                    throw ex;
                }
            }
        }
    }

    /// <summary>
    /// 移除所有权限
    /// </summary>
    /// <param name="UserID">用户编号</param>
    public void RemoveAllPriviledge()
    {
        DBConnection conn = new DBConnection();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "delete from tb_b_YH_YHQX where  UserID=@UserID";
        cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = new Guid(UserID);
        try
        {
            using (conn)
            {
                conn.ExecuteNonQuery(cmd);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary> 
    /// 移除权限 
    /// </summary> 
    /// <param name="Priviledge">权限编号</param> 
    /// <remarks></remarks> 
    public void RemovePriviledge(Guid Priviledge)
    {
        DBConnection conn = new DBConnection();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "delete from tb_b_YH_YHQX where PrivilegeCode = @PrivilegeCode and UserID=@UserID";
        cmd.Parameters.Add("@PrivilegeCode", SqlDbType.UniqueIdentifier).Value = Priviledge;
        cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = new Guid(UserID);
        try
        {
            using (conn)
            {
                conn.ExecuteNonQuery(cmd);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetUserPriviledgeInfo()
    {
        DBConnection conn = new DBConnection();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select PrivilegeCode from tb_b_YH_YHQX where UserID = @UserID";
        cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = new Guid(UserID);
        try
        {
            using (conn)
            {
                return conn.ExecuteDataTable(cmd);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}