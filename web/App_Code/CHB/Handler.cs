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
using System.Drawing;
using Aspose.BarCode;
using WxPayAPI;

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
                    DaoDaZhan += DaoDaZhan_Province;
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
            parameters.Add("QiShiZhan", QiShiZhan_Province + " " + QiShiZhan_City);
            parameters.Add("DaoDaZhan", DaoDaZhan_Province + " " + DaoDaZhan_City);
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
    public object SearchMyYunDan(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string DaoDaZhan_Province, string DaoDaZhan_City, string SuoShuGongSi, string UserDenno)
    {
        using(var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string conn = "";

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
                    conn += " and QiShiZhan like @QiShiZhan";
                }
                if (!string.IsNullOrEmpty(DaoDaZhan_Province))
                {
                    DaoDaZhan += DaoDaZhan_Province;
                }
                if (!string.IsNullOrEmpty(DaoDaZhan_City))
                {
                    DaoDaZhan += " " + DaoDaZhan_City;
                }
                if (!string.IsNullOrEmpty(DaoDaZhan))
                {
                    conn += " and DaoDaZhan like @DaoDaZhan";
                }
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                {
                    conn += " and SuoShuGongSi like @SuoShuGongSi";
                }

                if (!string.IsNullOrEmpty(UserDenno))
                    conn += " and UserDenno like @UserDenno";
                string sql = "select * from YunDan where UserID = @UserID" + conn + " order by BangDingTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID",SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(conn))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi + "%");
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
                string sql = "select * from YunDan where 1=1 " + conn;
                SqlCommand cmd = db.CreateCommand(sql);
                //cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
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

    [CSMethod("SearchBD")]
    public bool SearchBD(string UserID, string YunDanDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_JieChuBangDingLoad.ashx";
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

    [CSMethod("GDGPay")]
    public bool GDGPay(string OrderDenno, string DGZZCompany, string DGZH, string DKPZH)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from GpsDingDan where OrderDenno = '" + OrderDenno + "'";
                DataTable dt_dingdan = db.ExecuteDataTable(sql);

                string sql_dingdan = "update GpsDingDan set GpsDingDanZhiFuZhuangTai = 1,GpsDingDanSH = 0,GpsDingDanZhiFuLeiXing = '公对公',GpsDingDanZhiFuShiJian = '" + DateTime.Now + "' where OrderDenno = '" + OrderDenno + "'";
                db.ExecuteNonQuery(sql_dingdan);

                if (dt_dingdan.Rows.Count > 0)
                {
                    string sql_mx = "select * from GpsDingDanMingXi where GpsDingDanDenno = '" + dt_dingdan.Rows[0]["GpsDingDanDenno"].ToString() + "'";
                    DataTable dt_mx = db.ExecuteDataTable(sql_mx);

                    //DataTable dt_device = db.GetEmptyDataTable("GpsDevice");
                    //for (int i = 0; i < dt_mx.Rows.Count; i++)
                    //{
                    //    DataRow dr_device = dt_device.NewRow();
                    //    dr_device["GpsDeviceID"] = dt_mx.Rows[i]["GpsDeviceID"].ToString();
                    //    dr_device["UserID"] = dt_dingdan.Rows[0]["UserID"].ToString();
                    //    dt_device.Rows.Add(dr_device);
                    //}
                    //db.InsertTable(dt_device);
                }

                DataTable dt = db.GetEmptyDataTable("ZhiFuOrder");
                DataRow dr = dt.NewRow();
                dr["Guid"] = Guid.NewGuid();
                dr["OrderDenno"] = OrderDenno;
                dr["Lx"] = 0;
                dt.Rows.Add(dr);
                db.InsertTable(dt);

                DataTable dt_gdg = db.GetEmptyDataTable("GpsDingDanGDG");
                DataRow dr_gdg = dt_gdg.NewRow();
                dr_gdg["GDGZhiFu"] = Guid.NewGuid();
                dr_gdg["OrderDenno"] = OrderDenno;
                dr_gdg["DGZZCompany"] = DGZZCompany;
                dr_gdg["DGZH"] = DGZH;
                dr_gdg["DKPZH"] = DKPZH;
                dr_gdg["AddTime"] = DateTime.Now;
                dt_gdg.Rows.Add(dr_gdg);
                db.InsertTable(dt_gdg);

                db.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("GDGChongZhi")]
    public bool GDGChongZhi(string OrderDenno, string DGZZCompany, string DGZH, string DKPZH)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "update ChongZhi set ZhiFuZhuangTai = 1,ChongZhiDescribe = '公对公',ChongZhiSH = 0,ZhiFuTime = '" + DateTime.Now + "' where OrderDenno = '" + OrderDenno + "'";
                db.ExecuteNonQuery(sql);

                sql = "select * from ChongZhi where OrderDenno = '" + OrderDenno + "'";
                DataTable dt_ChongZhi = db.ExecuteDataTable(sql);

                if (dt_ChongZhi.Rows.Count > 0)
                {
                    //sql = "select * from [dbo].[User] where UserID = '" + dt_ChongZhi.Rows[0]["UserID"].ToString() + "'";
                    //DataTable dt_user = db.ExecuteDataTable(sql);

                    //int num = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) + Convert.ToInt32(dt_ChongZhi.Rows[0]["ChongZhiCiShu"].ToString());
                    //sql = "update [dbo].[User] set UserRemainder = '" + num + "' where UserID = '" + dt_ChongZhi.Rows[0]["UserID"].ToString() + "'";
                    //db.ExecuteNonQuery(sql);

                    DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                    DataRow dr_caozuo = dt_caozuo.NewRow();
                    dr_caozuo["UserID"] = dt_ChongZhi.Rows[0]["UserID"];
                    dr_caozuo["CaoZuoLeiXing"] = "充值";
                    dr_caozuo["CaoZuoNeiRong"] = "web内用户充值，充值方式：微信；充值单号：" + OrderDenno + "；充值金额：" + Convert.ToDecimal(dt_ChongZhi.Rows[0]["ChongZhiJinE"].ToString()) + "。";
                    dr_caozuo["CaoZuoTime"] = DateTime.Now;
                    dr_caozuo["CaoZuoRemark"] = "";
                    dt_caozuo.Rows.Add(dr_caozuo);
                    db.InsertTable(dt_caozuo);
                }

                DataTable dt = db.GetEmptyDataTable("ZhiFuOrder");
                DataRow dr = dt.NewRow();
                dr["Guid"] = Guid.NewGuid();
                dr["OrderDenno"] = OrderDenno;
                dr["Lx"] = 0;
                dt.Rows.Add(dr);
                db.InsertTable(dt);

                DataTable dt_gdg = db.GetEmptyDataTable("ChongZhiGDG");
                DataRow dr_gdg = dt_gdg.NewRow();
                dr_gdg["GDGChongZhi"] = Guid.NewGuid();
                dr_gdg["OrderDenno"] = OrderDenno;
                dr_gdg["DGZZCompany"] = DGZZCompany;
                dr_gdg["DGZH"] = DGZH;
                dr_gdg["DKPZH"] = DKPZH;
                dr_gdg["AddTime"] = DateTime.Now;
                dt_gdg.Rows.Add(dr_gdg);
                db.InsertTable(dt_gdg);

                db.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
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
                return new { sign = "true", msg = "添加成功！", GpsTuiDanJinE = obj["GpsTuiDanJinE"].ToString() };
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
                return new { sign = "true", msg = "获取验证码成功！", yanzhengma = obj["yanzhengma"].ToString() };
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

    [CSMethod("GetJGCL")]
    public object GetJGCL()
    {
        using (var db = new DBConnection())
        {
            string sql = "SELECT * FROM [dbo].[User] WHERE UserID = '" + SystemUser.CurrentUser.UserID + "'";
            DataTable dt_user = db.ExecuteDataTable(sql);

            sql = "select * from JiaGeCeLve where JiaGeCeLveLeiXing = 'ChongZhi' order by JiaGeCeLveCiShu";
            DataTable dt_jg = db.ExecuteDataTable(sql);

            return new { UserRemainder = dt_user.Rows[0]["UserRemainder"], dt_jg = dt_jg };
        }
    }

    [CSMethod("CZZH")]
    public bool CZZH(int num, decimal money, string memo, string lx)
    {
        using (var db = new DBConnection())
        {
            string OrderDenno = "01" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return true;
        }
    }

    [CSMethod("XFJL")]
    public object XFJL(int CurrentPage,int PageSize,string startTime,string endTime)
    {
        using (var db = new DBConnection())
        {
            int cp = CurrentPage;
            int ac = 0;

            string conn = "";
            if (!string.IsNullOrEmpty(startTime))
                conn += " and ChongZhiTime > '" + Convert.ToDateTime(startTime) + "'";
            if (!string.IsNullOrEmpty(endTime))
                conn += " and ChongZhiTime < '" + Convert.ToDateTime(endTime).AddDays(1) + "'";
            string sql = "select * from ChongZhi where UserID = @UserID and ZhiFuZhuangTai = 1" + conn;
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
            DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

            #region  插入操作表
            DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
            DataRow dr = dt_caozuo.NewRow();
            dr["UserID"] = SystemUser.CurrentUser.UserID;
            dr["CaoZuoLeiXing"] = "消费记录";
            dr["CaoZuoNeiRong"] = "web内用户查询消费记录";
            dr["CaoZuoTime"] = DateTime.Now;
            dr["CaoZuoRemark"] = "";
            dt_caozuo.Rows.Add(dr);
            db.InsertTable(dt_caozuo);
            #endregion

            return new { dt = dt, cp = cp, ac = ac };
        }
    }

    [CSMethod("CZMM")]
    public object CZMM(string UserName, string UserPassword, string UserLeiXing)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ChongZhiMiMa.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", UserName);
            parameters.Add("UserPassword", UserPassword);
            parameters.Add("UserLeiXing", UserLeiXing);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
                return new { sign = "true", msg = "修改成功！" };
            else
                return new { sign = "false", msg = obj["msg"].ToString() };

        }
        catch (Exception ex)
        {
            throw ex;
        }  
    }

    [CSMethod("GetEWM")]
    public string GetEWM()
    {
        string id = Guid.NewGuid().ToString();
        SetEWM(id);
        return id;
    }

    private void SetEWM(string id)
    {
        //二维码
        //System.IO.MemoryStream bms = new MemoryStream();
        //var url = System.Configuration.ConfigurationSettings.AppSettings["erweima"];

        var url = id;
        Aspose.BarCode.BarCodeBuilder b = new Aspose.BarCode.BarCodeBuilder(url);
        b.AutoSize = false;
        b.ImageWidth = 80;
        b.ImageHeight = 80;
        b.SymbologyType = Aspose.BarCode.Symbology.QR;
        b.BorderVisible = false;
        b.CodeLocation = Aspose.BarCode.CodeLocation.None;
        b.QRErrorLevel = Aspose.BarCode.QRErrorLevel.LevelH;
        b.GetOnlyBarCodeImage();
        //b.Save(bms, Aspose.BarCode.BarCodeImageFormat.Bmp);
        //var imgBack = System.Drawing.Image.FromStream(bms);
        var imgBack = GetBarcodeImage(url, 165, 165);
        //二维码中加logo
        //System.Drawing.Image img = System.Drawing.Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "main\\images\\logo.jpg");//照片图片    
        //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgBack);
        //g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);//g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   
        //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  
        //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);  
        //g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Height / 2 - img.Height / 2, img.Width, img.Height);
        MemoryStream ms = new MemoryStream();
        imgBack.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        ms.Close();
        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        string fullPath = path + "\\erweima\\" + id + ".png";
        imgBack.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);  
    }

    private static Bitmap GetBarcodeImage(string code, int width, int height)
    {
        BarCodeBuilder builder = new BarCodeBuilder(code, Symbology.QR);
        builder.QRErrorLevel = Aspose.BarCode.QRErrorLevel.LevelH;
        builder.BorderVisible = false;
        builder.CodeLocation = CodeLocation.None;
        builder.AspectRatio = 2;
        builder.Margins.Set(0);
        builder.GraphicsUnit = GraphicsUnit.Pixel;
        return builder.GetCustomSizeBarCodeImage(new Size(width, height), false);
    }

    [CSMethod("LoginByEWM")]
    public bool LoginByEWM(string id)
    {
        using (var db = new DBConnection())
        {
            string sql = "select * from EWMLogin where Guid = @Guid";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@Guid", id);
            DataTable dt = db.ExecuteDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                sql = "select * from [dbo].[User] where UserName = @UserName";
                SqlCommand cmd2 = db.CreateCommand(sql);
                cmd2.Parameters.AddWithValue("@UserName", dt.Rows[0]["UserName"].ToString());
                DataTable dt_login = db.ExecuteDataTable(cmd2);

                if (dt_login.Rows.Count > 0)
                {
                    var su = SystemUser.Login(dt.Rows[0]["UserName"].ToString(), dt_login.Rows[0]["UserPassword"].ToString());
                    if (su != null)
                    {
                        HttpCookie cookie = new HttpCookie("login_Username", dt.Rows[0]["UserName"].ToString())
                        {
                            Expires = DateTime.Now.AddYears(1)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public bool InsertGuid(string guid,string username)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();
                DataTable dt = db.GetEmptyDataTable("EWMLogin");
                DataRow dr = dt.NewRow();
                dr["Guid"] = guid;
                dr["UserName"] = username;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                db.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("ShowEWMByCZ")]
    public string ShowEWMByCZ(string OrderDenno, decimal ChongZhiJinE, int ChongZhiCiShu, string ChongZhiRemark)
    {
        using (var db = new DBConnection())
        {
            try
            {
                DataTable dt = db.GetEmptyDataTable("ChongZhi");
                DataRow dr = dt.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["ChongZhiJinE"] = ChongZhiJinE;
                dr["ChongZhiCiShu"] = ChongZhiCiShu;
                dr["ChongZhiTime"] = DateTime.Now;
                dr["ZhiFuZhuangTai"] = 0;
                dr["ChongZhiRemark"] = ChongZhiRemark;
                dr["OrderDenno"] = OrderDenno;
                dt.Rows.Add(dr);
                db.InsertTable(dt);

                NativePay nativePay = new NativePay();

                //生成扫码支付模式二url
                string url = nativePay.GetPayUrl(OrderDenno, ChongZhiJinE.ToString(), ChongZhiRemark);

                //将url生成二维码图片
                return "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GDGByCZ")]
    public bool GDGByCZ(string OrderDenno, decimal ChongZhiJinE, int ChongZhiCiShu, string ChongZhiRemark)
    {
        using (var db = new DBConnection())
        {
            try
            {
                DataTable dt = db.GetEmptyDataTable("ChongZhi");
                DataRow dr = dt.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["ChongZhiJinE"] = ChongZhiJinE;
                dr["ChongZhiCiShu"] = ChongZhiCiShu;
                dr["ChongZhiTime"] = DateTime.Now;
                dr["ZhiFuZhuangTai"] = 0;
                dr["ChongZhiRemark"] = ChongZhiRemark;
                dr["OrderDenno"] = OrderDenno;
                dt.Rows.Add(dr);
                db.InsertTable(dt);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GDGByMobileCZ")]
    public bool GDGByMobileCZ(string UserName,string OrderDenno, decimal ChongZhiJinE, int ChongZhiCiShu, string ChongZhiRemark)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select UserID from [dbo].[User] where UserName = '" + UserName + "'";
                DataTable dt_user = db.ExecuteDataTable(sql);

                if (dt_user.Rows.Count > 0)
                {
                    DataTable dt = db.GetEmptyDataTable("ChongZhi");
                    DataRow dr = dt.NewRow();
                    dr["UserID"] = dt_user.Rows[0]["UserID"].ToString();
                    dr["ChongZhiJinE"] = ChongZhiJinE;
                    dr["ChongZhiCiShu"] = ChongZhiCiShu;
                    dr["ChongZhiTime"] = DateTime.Now;
                    dr["ZhiFuZhuangTai"] = 0;
                    dr["ChongZhiRemark"] = ChongZhiRemark;
                    dr["OrderDenno"] = OrderDenno;
                    dr["ChongZhiSH"] = 0;
                    dt.Rows.Add(dr);
                    db.InsertTable(dt);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("ShowEWMByYJ")]
    public string ShowEWMByYJ(string OrderDenno, string GpsDingDanJinE, string memo)
    {
        NativePay nativePay = new NativePay();

        //生成扫码支付模式二url
        string url = nativePay.GetPayUrl(OrderDenno, GpsDingDanJinE, memo);

        //将url生成二维码图片
        return "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(url);
    }

    [CSMethod("GetOrderDenno")]
    public string GetOrderDenno(string lb)
    {
        return lb + getdenno();
    }

    [CSMethod("StartSearch")]
    public bool StartSearch(string OrderDenno)
    {
        using (var db = new DBConnection())
        {
            string sql = "select count(*) num from ZhiFuOrder where OrderDenno = @OrderDenno";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@OrderDenno", OrderDenno);
            DataTable dt = db.ExecuteDataTable(cmd);
            if (Convert.ToInt32(dt.Rows[0]["num"].ToString()) > 0)
                return true;
            else
                return false;
        }
    }

    [CSMethod("GetUserName")]
    public string GetUserName()
    {
        return SystemUser.CurrentUser.UserName;
    }

    public string getdenno()
    {
        DateTime dttime = DateTime.Now;
        string TableID_str = dttime.ToString("yyyyMMddHHmmssfff");
        return TableID_str;
    }

    [CSMethod("TuiDanDeciceIsClose")]
    public bool TuiDanDeciceIsClose(string GpsDeviceID)
    {
        using (var db = new DBConnection())
        {
            string sql = "select * from GpsDevice a where UserID = @UserID and GpsDeviceID = @GpsDeviceID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
            cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
            DataTable dt = db.ExecuteDataTable(cmd);

            sql = "select YunDanDenno,GpsDeviceID from YunDan where IsBangding = 1";
            DataTable dt_yun = db.ExecuteDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow[] drs = dt_yun.Select("GpsDeviceID = '" + dt.Rows[0]["GpsDeviceID"].ToString() + "'");
                if (drs.Length > 0)
                    return false;//不可解绑
                else
                    return true;//可解绑
            }
            else
            {
                return false;//不可解绑
            }
        }
    }

    public bool TuiDanDeciceIsCloseByApp(string GpsDeviceID, string UserName)
    {
        using (var db = new DBConnection())
        {
            string UserID = GetUserIdByName(UserName);

            string sql = "select * from GpsDevice a where UserID = @UserID and GpsDeviceID = @GpsDeviceID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
            DataTable dt = db.ExecuteDataTable(cmd);

            sql = "select YunDanDenno,GpsDeviceID from YunDan where IsBangding = 1";
            DataTable dt_yun = db.ExecuteDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow[] drs = dt_yun.Select("GpsDeviceID = '" + dt.Rows[0]["GpsDeviceID"].ToString() + "'");
                if (drs.Length > 0)
                    return false;//不可解绑
                else
                    return true;//可解绑
            }
            else
            {
                return false;//不可解绑
            }
        }
    }

    public string GetUserIdByName(string UserName)
    {
        using(var db = new DBConnection())
        {
            string sql = "select * from [dbo].[User] where UserName = @UserName";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            DataTable dt = db.ExecuteDataTable(cmd);
            string UserID = "";
            if (dt.Rows.Count>0)
                UserID = dt.Rows[0]["UserID"].ToString();
            return UserID;
        }
    }

    [CSMethod("IsBangBind")]
    public bool IsBangBind(string GpsDeviceID)
    {
        using (var db = new DBConnection())
        {
            string sql = "select * from GpsDevice a where UserID = @UserID and GpsDeviceID = @GpsDeviceID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
            cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
            DataTable dt = db.ExecuteDataTable(cmd);

            sql = "select YunDanDenno,GpsDeviceID from YunDan where IsBangding = 1";
            DataTable dt_yun = db.ExecuteDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow[] drs = dt_yun.Select("GpsDeviceID = '" + dt.Rows[0]["GpsDeviceID"].ToString() + "'");
                if (drs.Length > 0)
                    return false;//不可解绑
                else
                    return true;//可解绑
            }
            else
            {
                return false;//不可解绑
            }
        }
    }

    [CSMethod("GetInvoiceList")]
    public object GetInvoiceList(int CurrentPage, int PageSize, string StartTime, string EndTime)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string where = "";

                if (!string.IsNullOrEmpty(StartTime)&&!string.IsNullOrEmpty(EndTime))
                {
                    where = " and AddTime >= '" + StartTime + "' and AddTime < '" + Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + "'";
                }

                string sql = "select * from InvoiceModel where UserID = @UserID" + where + " order by AddTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "我的发票--发票页面";
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

    [CSMethod("GetInvoiceListByMobile")]
    public DataTable GetInvoiceListByMobile(string UserName)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                string sql = "select UserID from [dbo].[User] where UserName = '" + UserName + "'";
                DataTable dt_user = db.ExecuteDataTable(sql);

                sql = "select * from InvoiceModel where UserID = @UserID order by AddTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@UserID", dt_user.Rows[0]["UserID"].ToString());
                DataTable dt = db.ExecuteDataTable(cmd);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = dt_user.Rows[0]["UserID"].ToString();
                dr["CaoZuoLeiXing"] = "我的发票--发票页面";
                dr["CaoZuoNeiRong"] = "app内登陆";
                dr["CaoZuoTime"] = DateTime.Now;
                dr["CaoZuoRemark"] = "";
                dt_caozuo.Rows.Add(dr);
                db.InsertTable(dt_caozuo);
                #endregion

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetChongZhiListByInvoice")]
    public DataTable GetChongZhiListByInvoice()
    {
        using (var db = new DBConnection())
        {
            string sql = "select * from ChongZhi where UserID = @UserID and ZhiFuZhuangTai = 1 and ChongZhiID not in (select a.ChongZhiID from InvoiceMxModel a left join InvoiceModel b on a.InvoiceId = b.InvoiceId where b.UserId = @UserID)";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@UserID", SystemUser.CurrentUser.UserID);
            DataTable dt = db.ExecuteDataTable(cmd);
            return dt;
        }
    }

    [CSMethod("GetChongZhiListByInvoiceM")]
    public DataTable GetChongZhiListByInvoiceM(string UserName)
    {
        using (var db = new DBConnection())
        {
            string sql = "select UserID from [dbo].[User] where UserName = '" + UserName + "'";
            DataTable dt_user = db.ExecuteDataTable(sql);

            sql = "select * from ChongZhi where UserID = @UserID and ZhiFuZhuangTai = 1 and ChongZhiID not in (select a.ChongZhiID from InvoiceMxModel a left join InvoiceModel b on a.InvoiceId = b.InvoiceId where b.UserId = @UserID)";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@UserID", dt_user.Rows[0]["UserID"].ToString());
            DataTable dt = db.ExecuteDataTable(cmd);
            return dt;
        }
    }

    [CSMethod("AddInvoice")]
    public bool AddInvoice(string InvoiceTitle, string InvoiceZZJGDM, string InvoicePerson, string InvoiceMobile, string InvoiceAddress, string je, string ChongZhiIDs)
    {
        using (var db = new DBConnection())
        {
            DataTable dt = db.GetEmptyDataTable("InvoiceModel");
            DataRow dr = dt.NewRow();
            dr["InvoiceId"] = Guid.NewGuid();
            dr["InvoiceTitle"] = InvoiceTitle;
            dr["InvoiceZZJGDM"] = InvoiceZZJGDM;
            dr["InvoicePerson"] = InvoicePerson;
            dr["InvoiceMobile"] = InvoiceMobile;
            dr["InvoiceAddress"] = InvoiceAddress;
            dr["UserId"] = SystemUser.CurrentUser.UserID;
            dr["AddTime"] = DateTime.Now;
            dr["IsOut"] = 0;
            dr["InvoiceJe"] = je;
            dt.Rows.Add(dr);
            db.InsertTable(dt);
            DataTable dt_mx = db.GetEmptyDataTable("InvoiceMxModel");
            string[] ids = ChongZhiIDs.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                if (!string.IsNullOrEmpty(ids[i]))
                {
                    DataRow dr_mx = dt_mx.NewRow();
                    dr_mx["InvoiceMxId"] = Guid.NewGuid();
                    dr_mx["InvoiceId"] = dr["InvoiceId"];
                    dr_mx["ChongZhiID"] = ids[i];
                    dt_mx.Rows.Add(dr_mx);
                }
            }
            db.InsertTable(dt_mx);
            return true;
        }
    }

    [CSMethod("AddInvoiceByMobile")]
    public bool AddInvoiceByMobile(string UserName, string InvoiceTitle, string InvoiceZZJGDM, string InvoicePerson, string InvoiceMobile, string InvoiceAddress, string je, string ChongZhiIDs)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();
                string sql = "select UserID from [dbo].[User] where UserName = '" + UserName + "'";
                DataTable dt_user = db.ExecuteDataTable(sql);

                DataTable dt = db.GetEmptyDataTable("InvoiceModel");
                DataRow dr = dt.NewRow();
                dr["InvoiceId"] = Guid.NewGuid();
                dr["InvoiceTitle"] = InvoiceTitle;
                dr["InvoiceZZJGDM"] = InvoiceZZJGDM;
                dr["InvoicePerson"] = InvoicePerson;
                dr["InvoiceMobile"] = InvoiceMobile;
                dr["InvoiceAddress"] = InvoiceAddress;
                dr["UserId"] = dt_user.Rows[0]["UserID"].ToString();
                dr["AddTime"] = DateTime.Now;
                dr["IsOut"] = 0;
                dr["InvoiceJe"] = je;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                DataTable dt_mx = db.GetEmptyDataTable("InvoiceMxModel");
                string[] ids = ChongZhiIDs.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ids[i]))
                    {
                        DataRow dr_mx = dt_mx.NewRow();
                        dr_mx["InvoiceMxId"] = Guid.NewGuid();
                        dr_mx["InvoiceId"] = dr["InvoiceId"];
                        dr_mx["ChongZhiID"] = ids[i];
                        dt_mx.Rows.Add(dr_mx);
                    }
                }
                db.InsertTable(dt_mx);
                return true;
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    [CSMethod("OnDelInvoice")]
    public bool OnDelInvoice(string InvoiceId)
    {
        using (var db = new DBConnection())
        {
            string sql = "delete from InvoiceModel where InvoiceId = @InvoiceId";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@InvoiceId", InvoiceId);
            db.ExecuteNonQuery(cmd);

            sql = "delete from InvoiceMxModel where InvoiceId = @InvoiceId";
            cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@InvoiceId", InvoiceId);
            db.ExecuteNonQuery(cmd);

            return true;
        }
    }

    [CSMethod("InsertClient")]
    public bool InsertClient(string UserName, string clientId)
    {
        using (var db = new DBConnection())
        {
            string sql = "select UserID from [dbo].[User] where UserName = '" + UserName + "'";
            DataTable dt_user = db.ExecuteDataTable(sql);

            sql = "select * from User_Client where clientId = '" + clientId + "'";
            DataTable dt_client = db.ExecuteDataTable(sql);

            if (dt_client.Rows.Count == 0)
            {
                DataTable dt = db.GetEmptyDataTable("User_Client");
                DataRow dr = dt.NewRow();
                dr["ID"] = Guid.NewGuid();
                dr["UserID"] = dt_user.Rows[0]["UserID"].ToString();
                dr["clientId"] = clientId;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
                return true;
            }
            else
            {
                return true;
            }
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