using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SmartFramework4v2.Web.WebExecutor;
using SmartFramework4v2.Data.SqlServer;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// Handler 的摘要说明
/// </summary>
[CSClass("Handler")]
public class Handler
{
    [CSMethod("GetZhiDanList")]
    public object GetZhiDanList(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string DaoDaZhan_Province, string DaoDaZhan_City, string SuoShuGongSi, string UserDenno)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string where = "";
                string QiShiZhan = "";
                string DaoDaZhan = "";

                if (!string.IsNullOrEmpty(QiShiZhan_Province))
                {
                    QiShiZhan += QiShiZhan_Province;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_City))
                {
                    QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan))
                {
                    where += " and QiShiZhan like @QiShiZhan";
                }
                if (!string.IsNullOrEmpty(DaoDaZhan_Province))
                {
                    DaoDaZhan += QiShiZhan_Province;
                }
                if (!string.IsNullOrEmpty(DaoDaZhan_City))
                {
                    DaoDaZhan += " " + DaoDaZhan_City;
                }
                if (!string.IsNullOrEmpty(DaoDaZhan))
                {
                    where += " and DaoDaZhan like @DaoDaZhan";
                }
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                {
                    where += " and SuoShuGongSi like @SuoShuGongSi";
                }
                if (!string.IsNullOrEmpty(UserDenno))
                {
                    where += " and UserDenno like @UserDenno";
                }

                string sql = "select * from YunDan where UserID = @UserID" + where + " order by BangDingTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi + "%");
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "我的运单--制单页面";
                dr["CaoZuoNeiRong"] = "web网页内登陆";
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetCompanyHis")]
    public DataTable GetCompanyHis()
    {
        try
        {
            DataTable dt_gs = new DataTable();
            dt_gs.Columns.Add("SuoShuGongSi");

            string url = "http://chb.yk56.net/WebService/APP_ZhiDanLoad.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
            {
                JArray jary = obj["gongsis"] as JArray;
                for (int i = 0; i < jary.Count; i++)
                {
                    DataRow dr = dt_gs.NewRow();
                    dr["SuoShuGongSi"] = jary[i]["text"].ToString();
                    dt_gs.Rows.Add(dr);
                }
            }
            return dt_gs;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("SaveYunDan")]
    public bool SaveYunDan(string QiShiZhan_Province, string QiShiZhan_City, string DaoDaZhan_Province, string DaoDaZhan_City, string SuoShuGongSi, string UserDenno, string GpsDeviceID, string YunDanRemark)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ZhiDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("QiShiZhan", QiShiZhan_Province + "　" + QiShiZhan_City);
            parameters.Add("DaoDaZhan", DaoDaZhan_Province + "　" + DaoDaZhan_City);
            parameters.Add("SuoShuGongSi", SuoShuGongSi);
            parameters.Add("UserDenno", UserDenno);
            parameters.Add("GpsDeviceID", GpsDeviceID);
            parameters.Add("YunDanRemark", YunDanRemark);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
                return true;
            else
                return false;
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("SearchMyYunDan")]
    public object SearchMyYunDan(int CurrentPage,int PageSize, string UserDenno)
    {
        using(var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string conn = "";
                if (!string.IsNullOrEmpty(UserDenno))
                    conn += " and UserDenno like @UserDenno";
                string sql = "select * from YunDan where UserID = @UserID" + conn;
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID",SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(conn))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "我的运单";
                dr["CaoZuoNeiRong"] = "web登录我的运单查询，搜索单号：" + UserDenno;
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("SearchGSYunDan")]
    public object SearchGSYunDan(int CurrentPage, int PageSize, string SuoShuGongSi, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string conn = "";
                if (!string.IsNullOrEmpty(UserDenno))
                    conn += " and UserDenno like @UserDenno";
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    conn += " and SuoShuGongSi like @SuoShuGongSi";
                string sql = "select * from YunDan where UserID = @UserID" + conn;
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi + "%");
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "自由查单";
                dr["CaoZuoNeiRong"] = "web登录自由查单查询，搜索单号：" + UserDenno + "；搜索公司：" + SuoShuGongSi + "。";
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                #region 插入历史查询表
                string conn2 = "";
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    conn2 += " and Value = @Value";
                sql = "select * from SearchHistory where UserID = @UserID and Type = '自由查单_公司'" + conn2;
                SqlCommand cmd2 = db.CreateCommand(sql);
                cmd2.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                cmd2.Parameters.AddWithValue("@Value", SuoShuGongSi);
                DataTable dt_search = db.ExecuteDataTable(cmd2);

                if (dt_search.Rows.Count == 0)
                {
                    DataTable dt_his = db.GetEmptyDataTable("SearchHistory");
                    DataRow dr_his = dt_his.NewRow();
                    dr_his["UserID"] = SystemUser.CurrentUser.UserID;
                    dr_his["Type"] = "自由查单_公司";
                    dr_his["Value"] = SuoShuGongSi;
                    dt_his.Rows.Add(dr_his);
                    db.InsertTable(dt_his);
                }
                #endregion

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetSearchHis")]
    public DataTable GetSearchHis()
    {
        try
        {
            DataTable dt_gs = new DataTable();
            dt_gs.Columns.Add("SuoShuGongSi");

            string url = "http://chb.yk56.net/WebService/APP_ZiYouChaDanLoad.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
            {
                JArray jary = obj["gongsis"] as JArray;
                for (int i = 0; i < jary.Count; i++)
                {
                    DataRow dr = dt_gs.NewRow();
                    dr["SuoShuGongSi"] = jary[i]["text"].ToString();
                    dt_gs.Rows.Add(dr);
                }
            }
            return dt_gs;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("CloseBD")]
    public bool CloseBD(string UserID, string YunDanDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_JieChuBangDing.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserID", UserID);
            parameters.Add("YunDanDenno", YunDanDenno);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
                return true;
            else
                return false;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("GPSGL")]
    public object GPSGL(int CurrentPage, int PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string sql = "select * from GpsDevice a where UserID = @UserID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);
                dt.Columns.Add("IsBangding");

                sql = "select YunDanDenno,GpsDeviceID from YunDan where IsBangding = 1";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_yun.Select("GpsDeviceID = '" + dt.Rows[i]["GpsDeviceID"].ToString() + "'");
                    if(drs.Length > 0)
                        dt.Rows[i]["IsBangding"] = "已绑定";
                    else
                        dt.Rows[i]["IsBangding"] = "未绑定";
                }

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "GPS管理";
                dr["CaoZuoNeiRong"] = "web内用户查询GPS设备列表。";
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GPSDD")]
    public object GPSDD(int CurrentPage, int PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string sql = "select * from GpsDingDan a where UserID = @UserID and GpsDingDanIsEnd = 1 order by GpsDingDanTime asc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "订单列表";
                dr["CaoZuoNeiRong"] = "web内用户查询订单列表。";
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("AddGPS")]
    public object AddGPS(string GpsDeviceID)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShengChengDingDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("GpsDeviceID", GpsDeviceID);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "添加成功！", OrderDenno = obj["OrderDenno"].ToString() };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("GetZhiFuGPS")]
    public object GetZhiFuGPS()
    {
        using (var db = new DBConnection())
        {
            try
            {
                string username = SystemUser.CurrentUser.UserName;
                string userid = SystemUser.CurrentUser.UserID;

                string sql = "select * from GpsDingDan where UserID = '" + userid + "' and GpsDingDanIsEnd = 0";
                DataTable dt_gpsdd = db.ExecuteDataTable(sql);
                if (dt_gpsdd.Rows.Count > 0)
                {
                    sql = "select * from GpsDingDanMingXi where GpsDingDanDenno = '" + dt_gpsdd.Rows[0]["GpsDingDanDenno"].ToString() + "'";
                    DataTable dt = db.ExecuteDataTable(sql);
                    return new { dt = dt, OrderDenno = dt_gpsdd.Rows[0]["OrderDenno"].ToString() };
                }
                else
                {
                    DataTable dt = db.GetEmptyDataTable("GpsDingDanMingXi");
                    return new { dt = dt, OrderDenno = "" };
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("TJDD")]
    public object TJDD(string OrderDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_TiJiaoDingDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("OrderDenno", OrderDenno);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if(obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "添加成功！", GpsDingDanJinE = obj["GpsDingDanJinE"].ToString() };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("ZF")]
    public object ZF(string OrderDenno,string lx)
    {
        try
        {
            string pay = "";
            if (lx == "wxpay")
                pay = "http://chb.yk56.net/WebService/APP_WeiXinPay.ashx";
            else
                pay = "http://chb.yk56.net/WebService/APP_ALiYunPay.ashx";
            string url = pay;
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("OrderDenno", OrderDenno);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "添加成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("GPSTD")]
    public object GPSTD(int CurrentPage, int PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string sql = "select * from GpsTuiDan a where UserID = @UserID and GpsTuiDanIsEnd = 1 order by GpsTuiDanTime asc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "退单列表";
                dr["CaoZuoNeiRong"] = "web内用户查询退单列表。";
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("DelDD")]
    public object DelDD(string OrderDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShanChuDingDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("OrderDenno", OrderDenno);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "删除成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("DelTD")]
    public object DelTD(string OrderDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShanChuTuiDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("OrderDenno", OrderDenno);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "删除成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("DeleteDDItem")]
    public object DeleteDDItem(string GpsDingDanMingXiID)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShanChuDingDanOne.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("GpsDingDanMingXiID", GpsDingDanMingXiID);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "删除成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("GetTuiDanGPS")]
    public object GetTuiDanGPS()
    {
        using (var db = new DBConnection())
        {
            try
            {
                string username = SystemUser.CurrentUser.UserName;
                string userid = SystemUser.CurrentUser.UserID;

                string sql = "select * from GpsTuiDan where UserID = '" + userid + "' and GpsTuiDanIsEnd = 0";
                DataTable dt_gpstd = db.ExecuteDataTable(sql);
                if (dt_gpstd.Rows.Count > 0)
                {
                    sql = "select * from GpsTuiDanMingXi where GpsTuiDanDenno = '" + dt_gpstd.Rows[0]["GpsTuiDanDenno"].ToString() + "'";
                    DataTable dt = db.ExecuteDataTable(sql);
                    return new { dt = dt, OrderDenno = dt_gpstd.Rows[0]["OrderDenno"].ToString() };
                }
                else
                {
                    DataTable dt = db.GetEmptyDataTable("GpsTuiDanMingXi");
                    return new { dt = dt, OrderDenno = "" };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("AddTuiDanGPS")]
    public object AddTuiDanGPS(string GpsDeviceID)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShengChengTuiDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("GpsDeviceID", GpsDeviceID);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "添加成功！", OrderDenno = obj["OrderDenno"].ToString() };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };

        }
        catch (Exception ex)
        {
            throw ex;
        }      
    }

    [CSMethod("DeleteTDItem")]
    public object DeleteTDItem(string GpsTuiDanMingXiID)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShanChuTuiDanOne.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("GpsTuiDanMingXiID", GpsTuiDanMingXiID);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "删除成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("TJTD")]
    public object TJTD(string OrderDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_TiJiaoTuiDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("OrderDenno", OrderDenno);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "添加成功！", GpsDingDanJinE = obj["GpsTuiDanJinE"].ToString() };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("SendMessage")]
    public object SendMessage(string type)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_GetYanZhengMa.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("type", type);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;

            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "获取验证码成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("PickBank")]
    public DataTable PickBank()
    {
        try
        {
            using(var db = new DBConnection())
            {
                string userid = SystemUser.CurrentUser.UserID;
                string sql = "select distinct GpsTuiDanZhangHao from GpsTuiDan where UserID = '" + userid + "' and GpsTuiDanZhangHao is not null";
                DataTable dt = db.ExecuteDataTable(sql);
                return dt;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("QRSQ")]
    public object QRSQ(string OrderDenno, string GpsTuiDanZhangHao)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_TuiDan.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            parameters.Add("OrderDenno", OrderDenno);
            parameters.Add("GpsTuiDanZhangHao", GpsTuiDanZhangHao);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "申请成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };

        }
        catch (Exception ex)
        {
            throw ex;
        }   
    }

    #region webservice请求方法
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true; //总是接受     
    }

    public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
    {
        HttpWebRequest request = null;
        //HTTPSQ请求  
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        request = WebRequest.Create(url) as HttpWebRequest;
        request.ProtocolVersion = HttpVersion.Version10;
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.UserAgent = DefaultUserAgent;
        //如果需要POST数据     
        if (!(parameters == null || parameters.Count == 0))
        {
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in parameters.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, parameters[key]);
                }
                i++;
            }
            byte[] data = charset.GetBytes(buffer.ToString());
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
        return request.GetResponse() as HttpWebResponse;
    }
    #endregion
}