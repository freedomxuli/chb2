﻿using System;
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
using System.Collections;
using SmartFramework4v2.Web.Common.JSON;
using Aspose.Cells;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Web.Script.Serialization;

/// <summary>
/// Handler 的摘要说明
/// </summary>
[CSClass("Handler")]
public class Handler
{
    [CSMethod("GetZhiDanList")]
    public object GetZhiDanList(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string UserDenno)
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
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    where += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }
                
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    where += " and DaoDaZhan_QX like @DaoDaZhan_QX";
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
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
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

    [CSMethod("GetYanzhengma")]
    public object GetYanzhengma(string UserName, string type)
    {
        try
        {
            string sign = "0";
            string msg = "获取验证码失败";
            string yzm = "";
            string url = "http://chb.yk56.net/WebService/APP_GetYanZhengMa.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserName", UserName);
            parameters.Add("type", type);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            if (obj["sign"].ToString() == "1")
            {
                sign = "1";
                yzm = obj["yanzhengma"].ToString();
            }
            else
            {
                msg = obj["msg"].ToString();
            }
            return new { sign = sign, msg = msg, yzm = yzm };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("Zhuce")]
    public object GetYanzhengma(string UserCity, string UserName, string UserPassword, string UserLeiXing)
    {
        try
        {
            string sign = "0";
            string msg = "";
            string url = "http://chb.yk56.net/WebService/APP_ZhuCe.ashx";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("UserCity", UserCity);
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
            {
                sign = "1";
                msg = obj["msg"].ToString();
            }
            else
            {
                msg = obj["msg"].ToString();
            }
            return new { sign = sign, msg = msg};
        }
        catch (Exception ex)
        {
            throw ex;
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
    public bool SaveYunDan(string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string QiShiAddress, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string DaoDaAddress, string SuoShuGongSi, string UserDenno, double Expect_Hour, string SalePerson, string Purchaser, string PurchaserPerson, string PurchaserTel, string CarrierCompany, string CarrierPerson, string CarrierTel, string GpsDeviceID, string YunDanRemark, string IsYJ, string IsSendMessage, string MessageTel, JSReader[] jsr)
    {
        using (var dbc = new DBConnection())
        {
            try
            {
                dbc.BeginTransaction();


                //获取起始站、到达站位置
                string QiShiZhan_lat = "";
                string QiShiZhan_lng = "";
                string DaoDaZhan_lat = "";
                string DaoDaZhan_lng = "";
                string QiShiZhan = "";
                string QiShiZhan_Text = "";
                string DaoDaZhan = "";
                string DaoDaZhan_Text = "";
                bool IsCanSale = (GpsDeviceID.Substring(0, 4) == "2020" || GpsDeviceID.Substring(0, 4) == "8630") ? true : false;

                bool isReturn = true;

                string UserID = SystemUser.CurrentUser.UserID;

                string sql_device = "select count(*) from GpsDeviceSale where GpsDeviceID = @GpsDeviceID";
                SqlCommand cmd_device = dbc.CreateCommand(sql_device);
                cmd_device.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                string device_num = dbc.ExecuteScalar(cmd_device).ToString();

                string sql_danhao = "select * from YunDan where UserID = @UserID and UserDenno = @UserDenno";
                SqlCommand cmd_danhao = dbc.CreateCommand(sql_danhao);
                cmd_danhao.Parameters.AddWithValue("@UserID", UserID);
                cmd_danhao.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt_userdenno = dbc.ExecuteDataTable(cmd_danhao);
                if (dt_userdenno.Rows.Count > 0)
                    throw new Exception("单号为：" + UserDenno + "的单号重复！");

                string sql_user = "select * from [dbo].[User] where UserID = @UserID";
                SqlCommand cmd = dbc.CreateCommand(sql_user);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                DataTable dt_user = dbc.ExecuteDataTable(cmd);

                if (Convert.ToInt32(device_num)==0)
                {
                    if (Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) == 0)
                    {
                        throw new Exception("您的建单次数为0，请充值！");
                    }
                    if (IsCanSale)
                    {
                        throw new Exception("制单失败，请先购买该设备！");
                    }
                }

                #region  更新设备绑定状态
                string sql = "update YunDan set IsBangding = 0 where GpsDeviceID = @GpsDeviceID";
                cmd = dbc.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                dbc.ExecuteNonQuery(cmd);
                #endregion

                //#region  更新设备绑定状态
                //sql = "update YunDan set IsBangding = 0 where GpsDeviceID = @GpsDeviceID";
                //cmd = dbc.CreateCommand(sql);
                //cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                //dbc.ExecuteNonQuery(cmd);
                //#endregion

                #region  插入运单表
                Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + GpsDeviceID + "&pwd=123456");
                if (gpsinfo["success"].ToString().ToUpper() == "True".ToUpper())
                {
                    gpsinfo["sign"] = "1";
                }
                else
                {
                    gpsinfo["sign"] = "0";
                }
                string gpsvid = "";
                string gpsvkey = "";
                if (gpsinfo["sign"].ToString() == "1")
                {
                    gpsvid = gpsinfo["vid"].ToString();
                    gpsvkey = gpsinfo["vKey"].ToString();

                    Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");

                    string newlng = "";
                    string newlat = "";
                    string newinfo = "";
                    DateTime gpstm = DateTime.Now;
                    if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                    {
                        if (Convert.ToInt32(device_num) == 0)
                        {
                            #region  更新用户剩余次数
                            int UserRemainder = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) - 1;
                            sql = "update [dbo].[User] set UserRemainder = @UserRemainder where UserID = @UserID";
                            cmd = dbc.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserID", UserID);
                            cmd.Parameters.AddWithValue("@UserRemainder", UserRemainder);
                            dbc.ExecuteNonQuery(cmd);
                            #endregion
                        }
                        
                        Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                        string newgpstime = ja.First()["gpstime"].ToString();
                        //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                        newlng = ja.First()["lng"].ToString();
                        //newlng = newlng.Substring(0, newlng.Length - 2);
                        newlat = ja.First()["lat"].ToString();
                        //newlat = newlat.Substring(0, newlat.Length - 2);
                        newinfo = ja.First()["info"].ToString();
                        //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                        //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                        long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                        DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                        long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                        gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                        sql = "select * from GpsLocation where Gps_time = @Gps_time and GpsDeviceID = @GpsDeviceID";
                        cmd = dbc.CreateCommand(sql);
                        cmd.Parameters.Add("@Gps_time", gpstm);
                        cmd.Parameters.Add("@GpsDeviceID", GpsDeviceID);
                        DataTable dt_locations = dbc.ExecuteDataTable(cmd);
                        if (dt_locations.Rows.Count == 0)
                        {
                            DataTable dt_location_new = dbc.GetEmptyDataTable("GpsLocation");
                            DataRow dr_location = dt_location_new.NewRow();
                            dr_location["GpsDeviceID"] = GpsDeviceID;
                            dr_location["Gps_lat"] = newlat;
                            dr_location["Gps_lng"] = newlng;
                            dr_location["Gps_time"] = gpstm;
                            dr_location["Gps_info"] = newinfo;
                            dr_location["GpsRemark"] = "自动定位";
                            dt_location_new.Rows.Add(dr_location);
                            dbc.InsertTable(dt_location_new);
                        }

                        if (QiShiZhan_Province == QiShiZhan_City)
                        {
                            QiShiZhan = QiShiZhan_City + QiShiZhan_Qx;
                            QiShiZhan_Text = QiShiZhan_City + " " + QiShiZhan_Qx;
                        }
                        else
                        {
                            if (QiShiZhan_City == QiShiZhan_Qx)
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                            else
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City + QiShiZhan_Qx;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                        }

                        if (DaoDaZhan_Province == DaoDaZhan_City)
                        {
                            DaoDaZhan = DaoDaZhan_City + DaoDaZhan_Qx;
                            DaoDaZhan_Text = DaoDaZhan_City + " " + DaoDaZhan_Qx;
                        }
                        else
                        {
                            if (DaoDaZhan_City == DaoDaZhan_Qx)
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                            else
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City + DaoDaZhan_Qx;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                        }

                        Hashtable addresshash = getmapinfobyaddress(QiShiZhan, "");
                        if (addresshash["sign"] == "1")
                        {
                            QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                            QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                        }
                        Hashtable daozhanaddresshash = getmapinfobyaddress(DaoDaZhan, "");
                        if (daozhanaddresshash["sign"] == "1")
                        {
                            DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                            DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                        }

                        if (!string.IsNullOrEmpty(QiShiZhan_lat) && !string.IsNullOrEmpty(DaoDaZhan_lat))
                        {
                            DataTable dt_yundan = dbc.GetEmptyDataTable("YunDan");
                            DataRow dr_yundan = dt_yundan.NewRow();
                            dr_yundan["YunDanDenno"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            dr_yundan["UserDenno"] = UserDenno.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["UserID"] = UserID;
                            dr_yundan["QiShiZhan"] = QiShiZhan_Text;
                            dr_yundan["DaoDaZhan"] = DaoDaZhan_Text;
                            dr_yundan["SuoShuGongSi"] = SuoShuGongSi.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["BangDingTime"] = DateTime.Now;
                            dr_yundan["GpsDeviceID"] = GpsDeviceID;
                            dr_yundan["Gps_lastlat"] = newlat;
                            dr_yundan["Gps_lastlng"] = newlng;
                            if (newinfo != "")
                            {
                                dr_yundan["Gps_lasttime"] = gpstm;
                            }
                            dr_yundan["Gps_lastinfo"] = newinfo;
                            dr_yundan["IsBangding"] = true;
                            dr_yundan["YunDanRemark"] = YunDanRemark;
                            dr_yundan["QiShiZhan_lat"] = QiShiZhan_lat;
                            dr_yundan["QiShiZhan_lng"] = QiShiZhan_lng;
                            dr_yundan["DaoDaZhan_lat"] = DaoDaZhan_lat;
                            dr_yundan["DaoDaZhan_lng"] = DaoDaZhan_lng;

                            dr_yundan["SalePerson"] = SalePerson;
                            dr_yundan["Purchaser"] = Purchaser;
                            dr_yundan["PurchaserPerson"] = PurchaserPerson;
                            dr_yundan["PurchaserTel"] = PurchaserTel;
                            dr_yundan["CarrierCompany"] = CarrierCompany;
                            dr_yundan["CarrierPerson"] = CarrierPerson;
                            dr_yundan["CarrierTel"] = CarrierTel;
                            dr_yundan["DaoDaAddress"] = DaoDaAddress;
                            dr_yundan["QiShiAddress"] = QiShiAddress;
                            dr_yundan["IsChuFaMessage"] = 1;
                            dr_yundan["IsDaoDaMessage"] = 1;
                            if (IsYJ == "false")
                                dr_yundan["IsYJ"] = 0;
                            else
                                dr_yundan["IsYJ"] = 1;
                            if (IsSendMessage == "false")
                                dr_yundan["IsSendMessage"] = 0;
                            else
                                dr_yundan["IsSendMessage"] = 1;
                            dr_yundan["MessageTel"] = MessageTel;
                            dr_yundan["QiShiZhan_QX"] = QiShiZhan_Qx;
                            dr_yundan["DaoDaZhan_QX"] = DaoDaZhan_Qx;
                            dr_yundan["Expect_Hour"] = Expect_Hour;
                            dt_yundan.Rows.Add(dr_yundan);
                            dbc.InsertTable(dt_yundan);

                            if (jsr.Length > 0)
                            {
                                DataTable dt_detail = dbc.GetEmptyDataTable("YunDanDetails");
                                for (int i = 0; i < jsr.Length; i++)
                                {
                                    DataRow dr = dt_detail.NewRow();
                                    dr["YunDanDenno"] = dr_yundan["YunDanDenno"];
                                    foreach (var item in jsr[i])
                                    {
                                        dr[item] = jsr[i][item];
                                    }
                                    dt_detail.Rows.Add(dr);
                                }
                                dbc.InsertTable(dt_detail);
                            }

                            dbc.CommitTransaction();
                        }
                    }
                }
                else
                {
                    Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + GpsDeviceID + "");

                    string newlng = "";
                    string newlat = "";
                    string newinfo = "";
                    DateTime gpstm = DateTime.Now;
                    if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                    {
                        if (Convert.ToInt32(device_num) == 0)
                        {
                            #region  更新用户剩余次数
                            int UserRemainder = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) - 1;
                            sql = "update [dbo].[User] set UserRemainder = @UserRemainder where UserID = @UserID";
                            cmd = dbc.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserID", UserID);
                            cmd.Parameters.AddWithValue("@UserRemainder", UserRemainder);
                            dbc.ExecuteNonQuery(cmd);
                            #endregion
                        }

                        Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                        string newgpstime = ja.First()["gpstime"].ToString();
                        //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                        newlng = ja.First()["lng"].ToString();
                        //newlng = newlng.Substring(0, newlng.Length - 2);
                        newlat = ja.First()["lat"].ToString();
                        //newlat = newlat.Substring(0, newlat.Length - 2);
                        newinfo = ja.First()["info"].ToString();
                        //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                        //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                        long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                        DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                        long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                        gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                        sql = "select * from GpsLocation where Gps_time = @Gps_time and GpsDeviceID = @GpsDeviceID";
                        cmd = dbc.CreateCommand(sql);
                        cmd.Parameters.Add("@Gps_time", gpstm);
                        cmd.Parameters.Add("@GpsDeviceID", GpsDeviceID);
                        DataTable dt_locations = dbc.ExecuteDataTable(cmd);
                        if (dt_locations.Rows.Count == 0)
                        {
                            DataTable dt_location_new = dbc.GetEmptyDataTable("GpsLocation");
                            DataRow dr_location = dt_location_new.NewRow();
                            dr_location["GpsDeviceID"] = GpsDeviceID;
                            dr_location["Gps_lat"] = newlat;
                            dr_location["Gps_lng"] = newlng;
                            dr_location["Gps_time"] = gpstm;
                            dr_location["Gps_info"] = newinfo;
                            dr_location["GpsRemark"] = "自动定位";
                            dt_location_new.Rows.Add(dr_location);
                            dbc.InsertTable(dt_location_new);
                        }

                        if (QiShiZhan_Province == QiShiZhan_City)
                        {
                            QiShiZhan = QiShiZhan_City + QiShiZhan_Qx;
                            QiShiZhan_Text = QiShiZhan_City + " " + QiShiZhan_Qx;
                        }
                        else
                        {
                            if (QiShiZhan_City == QiShiZhan_Qx)
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                            else
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City + QiShiZhan_Qx;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                        }

                        if (DaoDaZhan_Province == DaoDaZhan_City)
                        {
                            DaoDaZhan = DaoDaZhan_City + DaoDaZhan_Qx;
                            DaoDaZhan_Text = DaoDaZhan_City + " " + DaoDaZhan_Qx;
                        }
                        else
                        {
                            if (DaoDaZhan_City == DaoDaZhan_Qx)
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                            else
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City + DaoDaZhan_Qx;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                        }

                        Hashtable addresshash = getmapinfobyaddress(QiShiZhan, "");
                        if (addresshash["sign"] == "1")
                        {
                            QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                            QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                        }
                        Hashtable daozhanaddresshash = getmapinfobyaddress(DaoDaZhan, "");
                        if (daozhanaddresshash["sign"] == "1")
                        {
                            DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                            DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                        }

                        if (!string.IsNullOrEmpty(QiShiZhan_lat) && !string.IsNullOrEmpty(DaoDaZhan_lat))
                        {
                            DataTable dt_yundan = dbc.GetEmptyDataTable("YunDan");
                            DataRow dr_yundan = dt_yundan.NewRow();
                            dr_yundan["YunDanDenno"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            dr_yundan["UserDenno"] = UserDenno.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["UserID"] = UserID;
                            dr_yundan["QiShiZhan"] = QiShiZhan_Text;
                            dr_yundan["DaoDaZhan"] = DaoDaZhan_Text;
                            dr_yundan["SuoShuGongSi"] = SuoShuGongSi.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["BangDingTime"] = DateTime.Now;
                            dr_yundan["GpsDeviceID"] = GpsDeviceID;
                            dr_yundan["Gps_lastlat"] = newlat;
                            dr_yundan["Gps_lastlng"] = newlng;
                            if (newinfo != "")
                            {
                                dr_yundan["Gps_lasttime"] = gpstm;
                            }
                            dr_yundan["Gps_lastinfo"] = newinfo;
                            dr_yundan["IsBangding"] = true;
                            dr_yundan["YunDanRemark"] = YunDanRemark;
                            dr_yundan["QiShiZhan_lat"] = QiShiZhan_lat;
                            dr_yundan["QiShiZhan_lng"] = QiShiZhan_lng;
                            dr_yundan["DaoDaZhan_lat"] = DaoDaZhan_lat;
                            dr_yundan["DaoDaZhan_lng"] = DaoDaZhan_lng;

                            dr_yundan["SalePerson"] = SalePerson;
                            dr_yundan["Purchaser"] = Purchaser;
                            dr_yundan["PurchaserPerson"] = PurchaserPerson;
                            dr_yundan["PurchaserTel"] = PurchaserTel;
                            dr_yundan["CarrierCompany"] = CarrierCompany;
                            dr_yundan["CarrierPerson"] = CarrierPerson;
                            dr_yundan["CarrierTel"] = CarrierTel;
                            dr_yundan["DaoDaAddress"] = DaoDaAddress;
                            dr_yundan["QiShiAddress"] = QiShiAddress;
                            dr_yundan["IsChuFaMessage"] = 1;
                            dr_yundan["IsDaoDaMessage"] = 1;
                            if (IsYJ == "false")
                                dr_yundan["IsYJ"] = 0;
                            else
                                dr_yundan["IsYJ"] = 1;
                            if (IsSendMessage == "false")
                                dr_yundan["IsSendMessage"] = 0;
                            else
                                dr_yundan["IsSendMessage"] = 1;
                            dr_yundan["MessageTel"] = MessageTel;
                            dr_yundan["QiShiZhan_QX"] = QiShiZhan_Qx;
                            dr_yundan["DaoDaZhan_QX"] = DaoDaZhan_Qx;
                            dr_yundan["Expect_Hour"] = Expect_Hour;
                            dt_yundan.Rows.Add(dr_yundan);
                            dbc.InsertTable(dt_yundan);

                            if (jsr.Length > 0)
                            {
                                DataTable dt_detail = dbc.GetEmptyDataTable("YunDanDetails");
                                for (int i = 0; i < jsr.Length; i++)
                                {
                                    DataRow dr = dt_detail.NewRow();
                                    dr["YunDanDenno"] = dr_yundan["YunDanDenno"];
                                    foreach (var item in jsr[i])
                                    {
                                        dr[item] = jsr[i][item];
                                    }
                                    dt_detail.Rows.Add(dr);
                                }
                                dbc.InsertTable(dt_detail);
                            }

                            dbc.CommitTransaction();

                            if (UserID == "4ddd6496-f031-4f4a-a50b-10742ff70462")//中石油接口处理
                            {
                                sql = "select CarNumber from YunDan where YunDanDenno = @YunDanDenno";
                                SqlCommand cmd_car = dbc.CreateCommand(sql);
                                cmd_car.Parameters.Add("@YunDanDenno", dr_yundan["YunDanDenno"]);
                                DataTable dt_car = dbc.ExecuteDataTable(cmd_car);
                                if (dt_car.Rows.Count > 0)
                                {
                                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                    IPAddress ipa = IPAddress.Parse("210.12.209.156");
                                    try
                                    {
                                        s.Connect(ipa, 9004);

                                    }
                                    catch (Exception ex)
                                    {
                                        s.Close();
                                        throw ex;
                                    }
                                    try
                                    {
                                        if (s.Connected == true)
                                        {
                                            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                            TimeSpan toNow = DateTime.Now.Subtract(dtStart);
                                            long timeStamp = toNow.Ticks;
                                            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
                                            string str = "";
                                            ArrayList eventList = new ArrayList();
                                            Hashtable ht_data = new Hashtable();
                                            ht_data.Add("deptName", SuoShuGongSi.TrimEnd(' ').TrimStart(' '));//所属单位
                                            ht_data.Add("longitude", newlng);
                                            ht_data.Add("latitude", newlat);
                                            ht_data.Add("positionDes", newinfo);
                                            ht_data.Add("plateNumber", dt_car.Rows[0]["CarNumber"].ToString());
                                            ht_data.Add("direction", ja.First()["direct"].ToString());
                                            ht_data.Add("mileage", ja.First()["totalDis"].ToString());
                                            ht_data.Add("speed", ja.First()["speed"].ToString());
                                            ht_data.Add("positioningTime", ja.First()["gpstime"].ToString());
                                            ht_data.Add("status", ja.First()["status"].ToString());
                                            ht_data.Add("addstatus", ja.First()["status"].ToString());
                                            eventList.Add(ht_data);
                                            JavaScriptSerializer ser = new JavaScriptSerializer();
                                            String jsonStr = ser.Serialize(eventList);

                                            Hashtable ht = new Hashtable();
                                            ht.Add("source", "03");//定位信息来源系统
                                            ht.Add("time", DateTime.Now.ToString());
                                            ht.Add("sysId", "10013");
                                            ht.Add("data", jsonStr);
                                            JavaScriptSerializer ser1 = new JavaScriptSerializer();
                                            str = ser.Serialize(ht);

                                            byte[] content = Encoding.UTF8.GetBytes(str);
                                            //BitArray content_array = new BitArray(content);
                                            ByteBuffer buffer = ByteBuffer.Allocate(content.Length + 8);
                                            buffer.WriteShort((short)97);
                                            buffer.WriteShort((short)0);
                                            buffer.WriteInt(content.Length);
                                            buffer.WriteBytes(content);
                                            s.Send(buffer.ToArray());

                                            byte[] result = new byte[1024];
                                            try
                                            {
                                                int receiveLength = s.Receive(result);
                                                string res = Encoding.UTF8.GetString(result, 0, receiveLength);
                                                int a = 0;
                                            }
                                            catch (Exception ex)
                                            {
                                                s.Close();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        s.Close();
                                        //throw ex;
                                    }   
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("该设备链接不上服务器！请联系查货宝！");
                    }
                }

                #endregion

                dbc.CommitTransaction();

                if (IsSendMessage == "true")
                {
                    string url = "http://chb.yk56.net/WebService/APP_ZhiDanMessage.ashx";
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("fileText", MessageTel.TrimEnd(','));
                    parameters.Add("daodadi", DaoDaZhan_Text + DaoDaZhan_Qx);
                    parameters.Add("company", SuoShuGongSi);
                    parameters.Add("code", UserDenno.TrimEnd(' ').TrimStart(' '));
                    HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
                    //打印返回值  
                    Stream stream = response.GetResponseStream();   //获取响应的字符串流  
                    StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
                    string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
                    JObject obj = JsonConvert.DeserializeObject(html) as JObject;
                }

                return isReturn;
            }
            catch (Exception ex)
            {
                dbc.RoolbackTransaction();
                throw ex;
            }
        }

            //string url = "http://chb.yk56.net/WebService/APP_ZhiDan.ashx";
            //Encoding encoding = Encoding.GetEncoding("utf-8");
            //IDictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("UserName", SystemUser.CurrentUser.UserName);
            //parameters.Add("QiShiZhan", QiShiZhan_Province + " " + QiShiZhan_City);
            //parameters.Add("DaoDaZhan", DaoDaZhan_Province + " " + DaoDaZhan_City);
            //parameters.Add("SuoShuGongSi", SuoShuGongSi);
            //parameters.Add("UserDenno", UserDenno);
            //parameters.Add("GpsDeviceID", GpsDeviceID);
            //parameters.Add("YunDanRemark", YunDanRemark);
            //HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            ////打印返回值  
            //Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            //StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            //string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            //JObject obj = JsonConvert.DeserializeObject(html) as JObject;
            //if (obj["sign"].ToString() == "1")
            //    return true;
            //else
            //    return false;
            
    }

    [CSMethod("SaveYunDanNew")]
    public bool SaveYunDanNew(string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string UserDenno, string GpsDeviceID, string IsYJ, string IsSendMessage,string Expect_Hour, string MessageTel, JSReader jsr1, JSReader[] jsr)
    {
        using (var dbc = new DBConnection())
        {
            try
            {
                dbc.BeginTransaction();

                //获取起始站、到达站位置
                string QiShiZhan_lat = "";
                string QiShiZhan_lng = "";
                string DaoDaZhan_lat = "";
                string DaoDaZhan_lng = "";
                string QiShiZhan = "";
                string QiShiZhan_Text = "";
                string DaoDaZhan = "";
                string DaoDaZhan_Text = "";
                bool IsCanSale = (GpsDeviceID.Substring(0, 4) == "2020" || GpsDeviceID.Substring(0, 4) == "8630") ? true : false;

                //var cc = jsr1["Expect_Hour"];
                //foreach (var k in jsr1)
                //{
                //    var aa = jsr1[k];
                //    var bb = 0;
                //}

                bool isReturn = true;

                string UserID = SystemUser.CurrentUser.UserID;

                string sql_device = "select count(*) from GpsDeviceSale where GpsDeviceID = @GpsDeviceID";
                SqlCommand cmd_device = dbc.CreateCommand(sql_device);
                cmd_device.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                string device_num = dbc.ExecuteScalar(cmd_device).ToString();

                string sql_user = "select * from [dbo].[User] where UserID = @UserID";
                SqlCommand cmd = dbc.CreateCommand(sql_user);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                DataTable dt_user = dbc.ExecuteDataTable(cmd);
                if (Convert.ToInt32(device_num) == 0)
                {
                    if (Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) == 0)
                    {
                        throw new Exception("您的建单次数为0，请充值！");
                    }
                    if (IsCanSale)
                    {
                        throw new Exception("制单失败，请先购买该设备！");
                    }
                }

                #region  获取自定义数据
                string sql_sel = "select * from DingDanSetList where UserID = @UserID";
                cmd = dbc.CreateCommand(sql_sel);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                DataTable dt_sel = dbc.ExecuteDataTable(cmd);
                #endregion

                #region  更新设备绑定状态
                string sql = "update YunDan set IsBangding = 0 where GpsDeviceID = @GpsDeviceID";
                cmd = dbc.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                dbc.ExecuteNonQuery(cmd);
                #endregion

                #region  插入运单表
                Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + GpsDeviceID + "&pwd=123456");
                if (gpsinfo["success"].ToString().ToUpper() == "True".ToUpper())
                {
                    gpsinfo["sign"] = "1";
                }
                else
                {
                    gpsinfo["sign"] = "0";
                }
                string gpsvid = "";
                string gpsvkey = "";
                if (gpsinfo["sign"].ToString() == "1")
                {
                    gpsvid = gpsinfo["vid"].ToString();
                    gpsvkey = gpsinfo["vKey"].ToString();

                    Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");

                    string newlng = "";
                    string newlat = "";
                    string newinfo = "";
                    DateTime gpstm = DateTime.Now;
                    if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                    {
                        if (Convert.ToInt32(device_num) == 0)
                        {
                            #region  更新用户剩余次数
                            int UserRemainder = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) - 1;
                            sql = "update [dbo].[User] set UserRemainder = @UserRemainder where UserID = @UserID";
                            cmd = dbc.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserID", UserID);
                            cmd.Parameters.AddWithValue("@UserRemainder", UserRemainder);
                            dbc.ExecuteNonQuery(cmd);
                            #endregion
                        }
                        
                        Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                        string newgpstime = ja.First()["gpstime"].ToString();
                        //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                        newlng = ja.First()["lng"].ToString();
                        //newlng = newlng.Substring(0, newlng.Length - 2);
                        newlat = ja.First()["lat"].ToString();
                        //newlat = newlat.Substring(0, newlat.Length - 2);
                        newinfo = ja.First()["info"].ToString();
                        //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                        //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                        long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                        DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                        long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                        gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                        sql = "select * from GpsLocation where Gps_time = @Gps_time and GpsDeviceID = @GpsDeviceID";
                        cmd = dbc.CreateCommand(sql);
                        cmd.Parameters.Add("@Gps_time", gpstm);
                        cmd.Parameters.Add("@GpsDeviceID", GpsDeviceID);
                        DataTable dt_locations = dbc.ExecuteDataTable(cmd);
                        if (dt_locations.Rows.Count == 0)
                        {
                            DataTable dt_location_new = dbc.GetEmptyDataTable("GpsLocation");
                            DataRow dr_location = dt_location_new.NewRow();
                            dr_location["GpsDeviceID"] = GpsDeviceID;
                            dr_location["Gps_lat"] = newlat;
                            dr_location["Gps_lng"] = newlng;
                            dr_location["Gps_time"] = gpstm;
                            dr_location["Gps_info"] = newinfo;
                            dr_location["GpsRemark"] = "自动定位";
                            dt_location_new.Rows.Add(dr_location);
                            dbc.InsertTable(dt_location_new);
                        }

                        if (QiShiZhan_Province == QiShiZhan_City)
                        {
                            QiShiZhan = QiShiZhan_City + QiShiZhan_Qx;
                            QiShiZhan_Text = QiShiZhan_City + " " + QiShiZhan_Qx;
                        }
                        else
                        {
                            if (QiShiZhan_City == QiShiZhan_Qx)
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                            else
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City + QiShiZhan_Qx;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                        }

                        if (DaoDaZhan_Province == DaoDaZhan_City)
                        {
                            DaoDaZhan = DaoDaZhan_City + DaoDaZhan_Qx;
                            DaoDaZhan_Text = DaoDaZhan_City + " " + DaoDaZhan_Qx;
                        }
                        else
                        {
                            if (DaoDaZhan_City == DaoDaZhan_Qx)
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                            else
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City + DaoDaZhan_Qx;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                        }

                        Hashtable addresshash = getmapinfobyaddress(QiShiZhan, "");
                        if (addresshash["sign"] == "1")
                        {
                            QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                            QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                        }
                        Hashtable daozhanaddresshash = getmapinfobyaddress(DaoDaZhan, "");
                        if (daozhanaddresshash["sign"] == "1")
                        {
                            DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                            DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                        }

                        if (!string.IsNullOrEmpty(QiShiZhan_lat) && !string.IsNullOrEmpty(DaoDaZhan_lat))
                        {
                            DataTable dt_yundan = dbc.GetEmptyDataTable("YunDan");
                            DataRow dr_yundan = dt_yundan.NewRow();
                            dr_yundan["YunDanDenno"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            dr_yundan["UserDenno"] = UserDenno.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["UserID"] = UserID;
                            dr_yundan["QiShiZhan"] = QiShiZhan_Text;
                            dr_yundan["DaoDaZhan"] = DaoDaZhan_Text;
                            dr_yundan["SuoShuGongSi"] = SuoShuGongSi.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["BangDingTime"] = DateTime.Now;
                            dr_yundan["GpsDeviceID"] = GpsDeviceID;
                            dr_yundan["Gps_lastlat"] = newlat;
                            dr_yundan["Gps_lastlng"] = newlng;
                            if (newinfo != "")
                            {
                                dr_yundan["Gps_lasttime"] = gpstm;
                            }
                            dr_yundan["Gps_lastinfo"] = newinfo;
                            dr_yundan["IsBangding"] = true;
                            dr_yundan["QiShiZhan_lat"] = QiShiZhan_lat;
                            dr_yundan["QiShiZhan_lng"] = QiShiZhan_lng;
                            dr_yundan["DaoDaZhan_lat"] = DaoDaZhan_lat;
                            dr_yundan["DaoDaZhan_lng"] = DaoDaZhan_lng;
                            dr_yundan["IsChuFaMessage"] = 1;
                            dr_yundan["IsDaoDaMessage"] = 1;
                            if (IsYJ == "false")
                                dr_yundan["IsYJ"] = 0;
                            else
                                dr_yundan["IsYJ"] = 1;
                            if (IsSendMessage == "false")
                                dr_yundan["IsSendMessage"] = 0;
                            else
                                dr_yundan["IsSendMessage"] = 1;
                            dr_yundan["MessageTel"] = MessageTel;
                            dr_yundan["QiShiZhan_QX"] = QiShiZhan_Qx;
                            dr_yundan["DaoDaZhan_QX"] = DaoDaZhan_Qx;
                            dr_yundan["Expect_Hour"] = Convert.ToDecimal(Expect_Hour) > 999 ? 999 : Convert.ToDecimal(Expect_Hour);

                            DataTable dt_field = dbc.GetEmptyDataTable("YunDanField");

                            for (int i = 0; i < dt_sel.Rows.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(dt_sel.Rows[i]["DingDanSetListBS"].ToString()))
                                {
                                    if (dt_sel.Rows[i]["DingDanSetListBS"].ToString() == "Expect_Hour")
                                        dr_yundan[dt_sel.Rows[i]["DingDanSetListBS"].ToString()] = Convert.ToDecimal(jsr1[dt_sel.Rows[i]["DingDanSetListBS"].ToString()].ToString());
                                    else
                                        dr_yundan[dt_sel.Rows[i]["DingDanSetListBS"].ToString()] = jsr1[dt_sel.Rows[i]["DingDanSetListBS"].ToString()];
                                }
                                else
                                {
                                    DataRow dr_field = dt_field.NewRow();
                                    dr_field["YunDanFieldID"] = Guid.NewGuid();
                                    dr_field["YunDanFieldMC"] = dt_sel.Rows[i]["DingDanSetListMC"].ToString();
                                    dr_field["YunDanFieldLX"] = Convert.ToInt32(dt_sel.Rows[i]["DingDanSetListLX"].ToString());
                                    dr_field["YunDanFieldBS"] = "div" + dt_sel.Rows[i]["DingDanSetListPX"].ToString();
                                    dr_field["YunDanFieldLXID"] = dt_sel.Rows[i]["DingDanSetListID"].ToString();
                                    dr_field["YunDanFieldPX"] = Convert.ToInt32(dt_sel.Rows[i]["DingDanSetListPX"].ToString());
                                    dr_field["YunDanDenno"] = dr_yundan["YunDanDenno"];
                                    dr_field["YunDanFieldValue"] = jsr1["div" + dt_sel.Rows[i]["DingDanSetListPX"].ToString()];
                                    dt_field.Rows.Add(dr_field);
                                }
                            }

                            dt_yundan.Rows.Add(dr_yundan);
                            dbc.InsertTable(dt_yundan);
                            if(dt_field.Rows.Count > 0)
                                dbc.InsertTable(dt_field);

                            if (jsr.Length > 0)
                            {
                                DataTable dt_detail = dbc.GetEmptyDataTable("YunDanDetails");
                                for (int i = 0; i < jsr.Length; i++)
                                {
                                    DataRow dr = dt_detail.NewRow();
                                    dr["YunDanDenno"] = dr_yundan["YunDanDenno"];
                                    foreach (var item in jsr[i])
                                    {
                                        dr[item] = jsr[i][item];
                                    }
                                    dt_detail.Rows.Add(dr);
                                }
                                dbc.InsertTable(dt_detail);
                            }

                            dbc.CommitTransaction();
                        }
                    }
                }
                else
                {
                    Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + GpsDeviceID + "");

                    string newlng = "";
                    string newlat = "";
                    string newinfo = "";
                    DateTime gpstm = DateTime.Now;
                    if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                    {
                        if (Convert.ToInt32(device_num) == 0)
                        {
                            #region  更新用户剩余次数
                            int UserRemainder = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) - 1;
                            sql = "update [dbo].[User] set UserRemainder = @UserRemainder where UserID = @UserID";
                            cmd = dbc.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserID", UserID);
                            cmd.Parameters.AddWithValue("@UserRemainder", UserRemainder);
                            dbc.ExecuteNonQuery(cmd);
                            #endregion
                        }

                        Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                        string newgpstime = ja.First()["gpstime"].ToString();
                        //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                        newlng = ja.First()["lng"].ToString();
                        //newlng = newlng.Substring(0, newlng.Length - 2);
                        newlat = ja.First()["lat"].ToString();
                        //newlat = newlat.Substring(0, newlat.Length - 2);
                        newinfo = ja.First()["info"].ToString();
                        //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                        //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                        long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                        DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                        long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                        gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                        sql = "select * from GpsLocation where Gps_time = @Gps_time and GpsDeviceID = @GpsDeviceID";
                        cmd = dbc.CreateCommand(sql);
                        cmd.Parameters.Add("@Gps_time", gpstm);
                        cmd.Parameters.Add("@GpsDeviceID", GpsDeviceID);
                        DataTable dt_locations = dbc.ExecuteDataTable(cmd);
                        if (dt_locations.Rows.Count == 0)
                        {
                            DataTable dt_location_new = dbc.GetEmptyDataTable("GpsLocation");
                            DataRow dr_location = dt_location_new.NewRow();
                            dr_location["GpsDeviceID"] = GpsDeviceID;
                            dr_location["Gps_lat"] = newlat;
                            dr_location["Gps_lng"] = newlng;
                            dr_location["Gps_time"] = gpstm;
                            dr_location["Gps_info"] = newinfo;
                            dr_location["GpsRemark"] = "自动定位";
                            dt_location_new.Rows.Add(dr_location);
                            dbc.InsertTable(dt_location_new);
                        }

                        if (QiShiZhan_Province == QiShiZhan_City)
                        {
                            QiShiZhan = QiShiZhan_City + QiShiZhan_Qx;
                            QiShiZhan_Text = QiShiZhan_City + " " + QiShiZhan_Qx;
                        }
                        else
                        {
                            if (QiShiZhan_City == QiShiZhan_Qx)
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                            else
                            {
                                QiShiZhan = QiShiZhan_Province + QiShiZhan_City + QiShiZhan_Qx;
                                QiShiZhan_Text = QiShiZhan_Province + " " + QiShiZhan_City;
                            }
                        }

                        if (DaoDaZhan_Province == DaoDaZhan_City)
                        {
                            DaoDaZhan = DaoDaZhan_City + DaoDaZhan_Qx;
                            DaoDaZhan_Text = DaoDaZhan_City + " " + DaoDaZhan_Qx;
                        }
                        else
                        {
                            if (DaoDaZhan_City == DaoDaZhan_Qx)
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                            else
                            {
                                DaoDaZhan = DaoDaZhan_Province + DaoDaZhan_City + DaoDaZhan_Qx;
                                DaoDaZhan_Text = DaoDaZhan_Province + " " + DaoDaZhan_City;
                            }
                        }

                        Hashtable addresshash = getmapinfobyaddress(QiShiZhan, "");
                        if (addresshash["sign"] == "1")
                        {
                            QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                            QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                        }
                        Hashtable daozhanaddresshash = getmapinfobyaddress(DaoDaZhan, "");
                        if (daozhanaddresshash["sign"] == "1")
                        {
                            DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                            DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                        }

                        if (!string.IsNullOrEmpty(QiShiZhan_lat) && !string.IsNullOrEmpty(DaoDaZhan_lat))
                        {
                            DataTable dt_yundan = dbc.GetEmptyDataTable("YunDan");
                            DataRow dr_yundan = dt_yundan.NewRow();
                            dr_yundan["YunDanDenno"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            dr_yundan["UserDenno"] = UserDenno.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["UserID"] = UserID;
                            dr_yundan["QiShiZhan"] = QiShiZhan_Text;
                            dr_yundan["DaoDaZhan"] = DaoDaZhan_Text;
                            dr_yundan["SuoShuGongSi"] = SuoShuGongSi.TrimEnd(' ').TrimStart(' ');
                            dr_yundan["BangDingTime"] = DateTime.Now;
                            dr_yundan["GpsDeviceID"] = GpsDeviceID;
                            dr_yundan["Gps_lastlat"] = newlat;
                            dr_yundan["Gps_lastlng"] = newlng;
                            if (newinfo != "")
                            {
                                dr_yundan["Gps_lasttime"] = gpstm;
                            }
                            dr_yundan["Gps_lastinfo"] = newinfo;
                            dr_yundan["IsBangding"] = true;
                            dr_yundan["QiShiZhan_lat"] = QiShiZhan_lat;
                            dr_yundan["QiShiZhan_lng"] = QiShiZhan_lng;
                            dr_yundan["DaoDaZhan_lat"] = DaoDaZhan_lat;
                            dr_yundan["DaoDaZhan_lng"] = DaoDaZhan_lng;
                            dr_yundan["IsChuFaMessage"] = 1;
                            dr_yundan["IsDaoDaMessage"] = 1;
                            if (IsYJ == "false")
                                dr_yundan["IsYJ"] = 0;
                            else
                                dr_yundan["IsYJ"] = 1;
                            if (IsSendMessage == "false")
                                dr_yundan["IsSendMessage"] = 0;
                            else
                                dr_yundan["IsSendMessage"] = 1;
                            dr_yundan["MessageTel"] = MessageTel;
                            dr_yundan["QiShiZhan_QX"] = QiShiZhan_Qx;
                            dr_yundan["DaoDaZhan_QX"] = DaoDaZhan_Qx;
                            dr_yundan["Expect_Hour"] = Convert.ToDecimal(Expect_Hour) > 999 ? 999 : Convert.ToDecimal(Expect_Hour);

                            DataTable dt_field = dbc.GetEmptyDataTable("YunDanField");

                            for (int i = 0; i < dt_sel.Rows.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(dt_sel.Rows[i]["DingDanSetListBS"].ToString()))
                                {
                                    if (dt_sel.Rows[i]["DingDanSetListBS"].ToString() == "Expect_Hour")
                                        dr_yundan[dt_sel.Rows[i]["DingDanSetListBS"].ToString()] = Convert.ToDecimal(jsr1[dt_sel.Rows[i]["DingDanSetListBS"].ToString()].ToString());
                                    else
                                        dr_yundan[dt_sel.Rows[i]["DingDanSetListBS"].ToString()] = jsr1[dt_sel.Rows[i]["DingDanSetListBS"].ToString()];
                                }
                                else
                                {
                                    DataRow dr_field = dt_field.NewRow();
                                    dr_field["YunDanFieldID"] = Guid.NewGuid();
                                    dr_field["YunDanFieldMC"] = dt_sel.Rows[i]["DingDanSetListMC"].ToString();
                                    dr_field["YunDanFieldLX"] = Convert.ToInt32(dt_sel.Rows[i]["DingDanSetListLX"].ToString());
                                    dr_field["YunDanFieldBS"] = "div" + dt_sel.Rows[i]["DingDanSetListPX"].ToString();
                                    dr_field["YunDanFieldLXID"] = dt_sel.Rows[i]["DingDanSetListID"].ToString();
                                    dr_field["YunDanFieldPX"] = Convert.ToInt32(dt_sel.Rows[i]["DingDanSetListPX"].ToString());
                                    dr_field["YunDanDenno"] = dr_yundan["YunDanDenno"];
                                    dr_field["YunDanFieldValue"] = jsr1["div" + dt_sel.Rows[i]["DingDanSetListPX"].ToString()];
                                    dt_field.Rows.Add(dr_field);
                                }
                            }

                            dt_yundan.Rows.Add(dr_yundan);
                            dbc.InsertTable(dt_yundan);
                            if (dt_field.Rows.Count > 0)
                                dbc.InsertTable(dt_field);

                            if (jsr.Length > 0)
                            {
                                DataTable dt_detail = dbc.GetEmptyDataTable("YunDanDetails");
                                for (int i = 0; i < jsr.Length; i++)
                                {
                                    DataRow dr = dt_detail.NewRow();
                                    dr["YunDanDenno"] = dr_yundan["YunDanDenno"];
                                    foreach (var item in jsr[i])
                                    {
                                        dr[item] = jsr[i][item];
                                    }
                                    dt_detail.Rows.Add(dr);
                                }
                                dbc.InsertTable(dt_detail);
                            }

                            dbc.CommitTransaction();

                            if (UserID == "4ddd6496-f031-4f4a-a50b-10742ff70462")//中石油接口处理
                            {
                                sql = "select CarNumber from YunDan where YunDanDenno = @YunDanDenno";
                                SqlCommand cmd_car = dbc.CreateCommand(sql);
                                cmd_car.Parameters.Add("@YunDanDenno", dr_yundan["YunDanDenno"]);
                                DataTable dt_car = dbc.ExecuteDataTable(cmd_car);
                                if (dt_car.Rows.Count > 0)
                                {
                                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                    IPAddress ipa = IPAddress.Parse("210.12.209.156");
                                    try
                                    {
                                        s.Connect(ipa, 9004);

                                    }
                                    catch (Exception ex)
                                    {
                                        s.Close();
                                        throw ex;
                                    }
                                    try
                                    {
                                        if (s.Connected == true)
                                        {
                                            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                            TimeSpan toNow = DateTime.Now.Subtract(dtStart);
                                            long timeStamp = toNow.Ticks;
                                            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
                                            string str = "";
                                            ArrayList eventList = new ArrayList();
                                            Hashtable ht_data = new Hashtable();
                                            ht_data.Add("deptName", SuoShuGongSi.TrimEnd(' ').TrimStart(' '));//所属单位
                                            ht_data.Add("longitude", newlng);
                                            ht_data.Add("latitude", newlat);
                                            ht_data.Add("positionDes", newinfo);
                                            ht_data.Add("plateNumber", dt_car.Rows[0]["CarNumber"].ToString());
                                            ht_data.Add("direction", ja.First()["direct"].ToString());
                                            ht_data.Add("mileage", ja.First()["totalDis"].ToString());
                                            ht_data.Add("speed", ja.First()["speed"].ToString());
                                            ht_data.Add("positioningTime", ja.First()["gpstime"].ToString());
                                            ht_data.Add("status", ja.First()["state"].ToString());
                                            ht_data.Add("addstatus", ja.First()["state"].ToString());
                                            eventList.Add(ht_data);

                                            Hashtable ht = new Hashtable();
                                            ht.Add("source", "03");//定位信息来源系统
                                            ht.Add("time", DateTime.Now.ToString());
                                            ht.Add("sysId", "10013");
                                            ht.Add("data", eventList);
                                            JavaScriptSerializer ser = new JavaScriptSerializer();
                                            str = ser.Serialize(ht);

                                            byte[] content = Encoding.UTF8.GetBytes(str);
                                            //BitArray content_array = new BitArray(content);
                                            ByteBuffer buffer = ByteBuffer.Allocate(content.Length + 8);
                                            buffer.WriteShort((short)97);
                                            buffer.WriteShort((short)0);
                                            buffer.WriteInt(content.Length);
                                            buffer.WriteBytes(content);
                                            s.Send(buffer.ToArray());

                                            byte[] result = new byte[1024];
                                            try
                                            {
                                                int receiveLength = s.Receive(result);
                                                string res = Encoding.UTF8.GetString(result, 0, receiveLength);
                                                int a = 0;
                                            }
                                            catch (Exception ex)
                                            {
                                                s.Close();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        s.Close();
                                        //throw ex;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("该设备链接不上服务器！请联系查货宝！");
                    }
                }

                #endregion

                dbc.CommitTransaction();

                if (IsSendMessage == "true")
                {
                    string url = "http://chb.yk56.net/WebService/APP_ZhiDanMessage.ashx";
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("fileText", MessageTel.TrimEnd(','));
                    parameters.Add("daodadi", DaoDaZhan_Text + DaoDaZhan_Qx);
                    parameters.Add("company", SuoShuGongSi);
                    parameters.Add("code", UserDenno.TrimEnd(' ').TrimStart(' '));
                    HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
                    //打印返回值  
                    Stream stream = response.GetResponseStream();   //获取响应的字符串流  
                    StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
                    string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
                    JObject obj = JsonConvert.DeserializeObject(html) as JObject;
                }

                return isReturn;
            }
            catch (Exception ex)
            {
                dbc.RoolbackTransaction();
                throw ex;
            }
        }

    }

    [CSMethod("SearchMyYunDan")]
    public object SearchMyYunDan(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string IsBangding, int isyj)
    {
        using (var db = new DBConnection())
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
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
                }
                if (!string.IsNullOrEmpty(DaoDaZhan))
                {
                    conn += " and DaoDaZhan like @DaoDaZhan";
                }
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                {
                    conn += " and SuoShuGongSi like @SuoShuGongSi";
                }
                if (isyj == 1)
                {
                    conn += " and IsBangding = 1";
                }
                else
                {
                    if (!string.IsNullOrEmpty(IsBangding))
                    {
                        conn += " and IsBangding = " + IsBangding;
                    }
                }
                if (!string.IsNullOrEmpty(UserDenno))
                    conn += " and UserDenno like @UserDenno";

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                string sql = "select * from YunDan where UserID = @UserID" + conn + " order by BangDingTime desc";
                if (isyj == 1)
                {
                    sql = @"select a.* from YunDan a 
                          inner join (
	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                          ) b on a.YunDanDenno = b.YunDanDenno
                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                }
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql = "select * from YunDanDistance where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_distance = db.ExecuteDataTable(sql);
                    if (dt_distance.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = dt_distance.Rows[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                }

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

    [CSMethod("SearchMyYunDanByGJ")]
    public object SearchMyYunDanByGJ(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string IsBangding, int isyj)
    {
        using (var db = new DBConnection())
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
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
                }
                if (!string.IsNullOrEmpty(DaoDaZhan))
                {
                    conn += " and DaoDaZhan like @DaoDaZhan";
                }
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                {
                    conn += " and SuoShuGongSi like @SuoShuGongSi";
                }
                if (isyj == 1)
                {
                    conn += " and IsBangding = 1";
                }
                else
                {
                    if (!string.IsNullOrEmpty(IsBangding))
                    {
                        conn += " and IsBangding = " + IsBangding;
                    }
                }
                if (!string.IsNullOrEmpty(UserDenno))
                    conn += " and UserDenno like @UserDenno";

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                string sql = "select * from YunDan where UserID = @UserID and GpsDeviceID like '8630%' " + conn + " order by BangDingTime desc";
                if (isyj == 1)
                {
                    sql = @"select a.* from YunDan a 
                          inner join (
	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                          ) b on a.YunDanDenno = b.YunDanDenno
                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                }
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql = "select * from YunDanDistance where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_distance = db.ExecuteDataTable(sql);
                    if (dt_distance.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = dt_distance.Rows[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                }

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

    [CSMethod("SearchMyYunDanByZT")]
    public object SearchMyYunDanByZT(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string StartTime, string EndTime, string Purchaser, string CarrierCompany,string sysjpx,string sylcpx)
    {
        using(var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string conn = "";
                string where = "";

                string QiShiZhan = "";
                string DaoDaZhan = "";

                if (!string.IsNullOrEmpty(QiShiZhan_Province))
                {
                    QiShiZhan += QiShiZhan_Province;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_City))
                {
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and a.QiShiZhan_QX like @QiShiZhan_QX";
                }
                if (!string.IsNullOrEmpty(QiShiZhan))
                {
                    conn += " and a.QiShiZhan like @QiShiZhan";
                }
                if (!string.IsNullOrEmpty(DaoDaZhan_Province))
                {
                    DaoDaZhan += DaoDaZhan_Province;
                }
                if (!string.IsNullOrEmpty(DaoDaZhan_City))
                {
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and a.DaoDaZhan_QX like @DaoDaZhan_QX";
                }
                if (!string.IsNullOrEmpty(DaoDaZhan))
                {
                    conn += " and a.DaoDaZhan like @DaoDaZhan";
                }
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                {
                    conn += " and a.SuoShuGongSi like @SuoShuGongSi";
                }
                if (!string.IsNullOrEmpty(UserDenno))
                    conn += " and a.UserDenno like @UserDenno";

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and a.GpsDeviceID = @GpsDeviceID";

                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    conn += " and a.BangDingTime >= @StartTime and a.BangDingTime <= @EndTime";

                if(!string.IsNullOrEmpty(Purchaser))
                    conn += " and a.Purchaser like @Purchaser";

                if (!string.IsNullOrEmpty(CarrierCompany))
                    conn += " and a.CarrierCompany like @CarrierCompany";

                if (!string.IsNullOrEmpty(sysjpx))
                {
                    if (sysjpx=="0")
                        where += " a.Gps_duration1 asc,";
                    else
                        where += " a.Gps_duration1 desc,";
                }

                if (!string.IsNullOrEmpty(sylcpx))
                {
                    if (sylcpx == "0")
                        where += " a.Gps_distance1 asc,";
                    else
                        where += " a.Gps_distance1 desc,";
                }

                string sql = @"select a.* from (
                                    select a.*,convert(decimal,b.Gps_duration) Gps_duration1,convert(decimal,b.Gps_distance) Gps_distance1 from YunDan a left join YunDanDistance b on a.YunDanDenno = b.YunDanDenno where a.UserID = @UserID and a.IsBangding = 1" + conn + @"
                                ) a order by a.BangDingTime desc";

                if (!string.IsNullOrEmpty(where))
                {
                    sql = @"select a.* from (
                                select a.*,convert(decimal,b.Gps_duration) Gps_duration1,convert(decimal,b.Gps_distance) Gps_distance1 from YunDan a left join YunDanDistance b on a.YunDanDenno = b.YunDanDenno where a.UserID = @UserID and a.IsBangding = 1" + conn + @"
                            ) a order by " + where.TrimEnd(',');
                }
                
//                if (isyj == 1)
//                {
//                    sql = @"select a.* from YunDan a 
//                          inner join (
//	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
//	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
//	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
//                          ) b on a.YunDanDenno = b.YunDanDenno
//                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
//                }
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID",SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ","") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if(!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");

                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);
                dt.Columns.Add("Expect_ArriveTime");
                dt.Columns.Add("Actual_ArriveTime");

                sql = "select * from YunDanDistance where YunDanDenno in (select a.YunDanDenno from YunDan a where a.UserID = @UserID and a.IsBangding = 1" + conn + ")";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");
                DataTable dt_distance = db.ExecuteDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                    if (drs.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(drs[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(drs[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = drs[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(drs[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Expect_Hour"].ToString()))
                    {
                        dt.Rows[i]["Expect_ArriveTime"] = Convert.ToDateTime(dt.Rows[i]["BangDingTime"]).AddHours(Convert.ToDouble(dt.Rows[i]["Expect_Hour"].ToString()));
                    }
                    sql = "select * from YunDanIsArrive where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_arrive = db.ExecuteDataTable(sql);
                    if (dt_arrive.Rows.Count > 0)
                    {
                        dt.Rows[i]["Actual_ArriveTime"] = dt_arrive.Rows[0]["Addtime"];
                    }
                }

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

    [CSMethod("SearchMyYunDanByYJ")]
    public object SearchMyYunDanByYJ(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string StartTime, string EndTime, string Purchaser, string CarrierCompany)
    {
        using (var db = new DBConnection())
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
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
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

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    conn += " and BangDingTime >= @StartTime and BangDingTime <= @EndTime";

                if (!string.IsNullOrEmpty(Purchaser))
                    conn += " and Purchaser like @Purchaser";

                if (!string.IsNullOrEmpty(CarrierCompany))
                    conn += " and CarrierCompany like @CarrierCompany";

                string sql = @"select a.* from YunDan a 
                                inner join (
                	                select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                	                inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                	                where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                                ) b on a.YunDanDenno = b.YunDanDenno
                                where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");

                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);
                dt.Columns.Add("Expect_ArriveTime");
                dt.Columns.Add("Actual_ArriveTime");

                sql = @"select * from YunDanDistance where YunDanDenno in (select a.YunDanDenno from YunDan a 
                        inner join (
                	        select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                	        inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                	        where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                        ) b on a.YunDanDenno = b.YunDanDenno
                        where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive))";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");
                DataTable dt_distance = db.ExecuteDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                    if (drs.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(drs[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(drs[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = drs[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(drs[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Expect_Hour"].ToString()))
                    {
                        dt.Rows[i]["Expect_ArriveTime"] = Convert.ToDateTime(dt.Rows[i]["BangDingTime"]).AddHours(Convert.ToDouble(dt.Rows[i]["Expect_Hour"].ToString()));
                    }
                    sql = "select * from YunDanIsArrive where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_arrive = db.ExecuteDataTable(sql);
                    if (dt_arrive.Rows.Count > 0)
                    {
                        dt.Rows[i]["Actual_ArriveTime"] = dt_arrive.Rows[0]["Addtime"];
                    }
                }

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

    [CSMethod("SearchMyYunDanByLS")]
    public object SearchMyYunDanByLS(int CurrentPage, int PageSize, string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string StartTime, string EndTime, string Purchaser, string CarrierCompany)
    {
        using (var db = new DBConnection())
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
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
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

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    conn += " and BangDingTime >= @StartTime and BangDingTime <= @EndTime";

                if (!string.IsNullOrEmpty(Purchaser))
                    conn += " and Purchaser like @Purchaser";

                if (!string.IsNullOrEmpty(CarrierCompany))
                    conn += " and CarrierCompany like @CarrierCompany";

                string sql = "select * from YunDan where UserID = @UserID and IsBangding = 0" + conn + " order by BangDingTime desc";
                //                if (isyj == 1)
                //                {
                //                    sql = @"select a.* from YunDan a 
                //                          inner join (
                //	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                //	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                //	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                //                          ) b on a.YunDanDenno = b.YunDanDenno
                //                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                //                }
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");

                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);
                dt.Columns.Add("Expect_ArriveTime");
                dt.Columns.Add("Actual_ArriveTime");


                sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 0" + conn + ")";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");
                DataTable dt_distance = db.ExecuteDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                    if (drs.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(drs[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(drs[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = drs[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(drs[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(drs[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Expect_Hour"].ToString()))
                    {
                        dt.Rows[i]["Expect_ArriveTime"] = Convert.ToDateTime(dt.Rows[i]["BangDingTime"]).AddHours(Convert.ToDouble(dt.Rows[i]["Expect_Hour"].ToString()));
                    }
                    sql = "select * from YunDanIsArrive where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_arrive = db.ExecuteDataTable(sql);
                    if (dt_arrive.Rows.Count > 0)
                    {
                        dt.Rows[i]["Actual_ArriveTime"] = dt_arrive.Rows[0]["Addtime"];
                    }
                }

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

                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(UserDenno) && !string.IsNullOrEmpty(SuoShuGongSi))
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn += " and UserDenno = @UserDenno";
                    if (!string.IsNullOrEmpty(SuoShuGongSi))
                        conn += " and SuoShuGongSi = @SuoShuGongSi";
                    string sql = "select * from YunDan where 1=1 " + conn;
                    SqlCommand cmd = db.CreateCommand(sql);
                    //cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                    cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                    dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                    if (dt.Rows.Count > 0)
                    {
                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                        cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                        cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                        cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                        cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        DataTable dt_ziyou = db.GetEmptyDataTable("ZiYouSearch");
                        DataRow dr_ziyou = dt_ziyou.NewRow();
                        dr_ziyou["UserID"] = SystemUser.CurrentUser.UserID;
                        dr_ziyou["SuoShuGongSi"] = SuoShuGongSi;
                        dr_ziyou["UserDenno"] = UserDenno;
                        dt_ziyou.Rows.Add(dr_ziyou);
                        db.InsertOrUpdateTable(dt_ziyou);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }

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
                    }
                }
                else
                {
                    string sql = "select * from ZiYouSearch where UserID = @UserID";
                    SqlCommand cmd = db.CreateCommand(sql);
                    cmd.Parameters.AddWithValue("@UserID",SystemUser.CurrentUser.UserID);
                    DataTable dt_user = db.ExecuteDataTable(cmd);

                    if (dt_user.Rows.Count > 0)
                    {
                        UserDenno = dt_user.Rows[0]["UserDenno"].ToString();
                        SuoShuGongSi = dt_user.Rows[0]["SuoShuGongSi"].ToString();
                        string conn = "";
                        if (!string.IsNullOrEmpty(UserDenno))
                            conn += " and UserDenno = @UserDenno";
                        if (!string.IsNullOrEmpty(SuoShuGongSi))
                            conn += " and SuoShuGongSi = @SuoShuGongSi";
                        sql = "select * from YunDan where 1=1 " + conn;
                        cmd = db.CreateCommand(sql);
                        //cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                        cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                        cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                        dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                        if (dt.Rows.Count > 0)
                        {
                            sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                            cmd = db.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                            cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                            cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                            DataTable dt_arrive = db.ExecuteDataTable(cmd);

                            sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                            cmd = db.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                            cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                            cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                            DataTable dt_distance = db.ExecuteDataTable(cmd);

                            DataTable dt_ziyou = db.GetEmptyDataTable("ZiYouSearch");
                            DataRow dr_ziyou = dt_ziyou.NewRow();
                            dr_ziyou["UserID"] = SystemUser.CurrentUser.UserID;
                            dr_ziyou["SuoShuGongSi"] = SuoShuGongSi;
                            dr_ziyou["UserDenno"] = UserDenno;
                            dt_ziyou.Rows.Add(dr_ziyou);
                            db.InsertOrUpdateTable(dt_ziyou);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs.Length == 0)
                                {
                                    DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                    if (drs_distance.Length > 0)
                                    {
                                        dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                        dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                    }
                                }
                            }

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
                        }
                    }
                }

                return new { dt = dt, cp = cp, ac = ac };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetOldYunDan")]
    public DataTable GetOldYunDan()
    {
        try
        {
            using(var db = new DBConnection())
            {
                string userid = SystemUser.CurrentUser.UserID;
                string sql = "select top 1 QiShiZhan,QiShiZhan_QX,SuoShuGongSi from YunDan where UserID = @UserID order by BangDingTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", userid);
                DataTable dt = db.ExecuteDataTable(cmd);
                return dt;
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
                dt.Columns.Add("LX");

                sql = "select YunDanDenno,GpsDeviceID from YunDan where IsBangding = 1";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                sql = "select * from GpsDeviceSale a where UserID = @UserID";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt_sale = db.ExecuteDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_yun.Select("GpsDeviceID = '" + dt.Rows[i]["GpsDeviceID"].ToString() + "'");
                    if(drs.Length > 0)
                        dt.Rows[i]["IsBangding"] = "已绑定";
                    else
                        dt.Rows[i]["IsBangding"] = "未绑定";

                    DataRow[] drs_sale = dt_sale.Select("GpsDeviceID = '" + dt.Rows[i]["GpsDeviceID"].ToString() + "'");
                    if(drs_sale.Length > 0)
                        dt.Rows[i]["LX"] = "售买类";
                    else
                        dt.Rows[i]["LX"] = "租赁类";
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


    [CSMethod("GPSGLSale")]
    public object GPSGLSale(int CurrentPage, int PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string sql = "select * from GpsDeviceSale a where UserID = @UserID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);
                dt.Columns.Add("IsBangding");

                sql = "select YunDanDenno,GpsDeviceID from YunDan where IsBangding = 1";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_yun.Select("GpsDeviceID = '" + dt.Rows[i]["GpsDeviceID"].ToString() + "'");
                    if (drs.Length > 0)
                        dt.Rows[i]["IsBangding"] = "已绑定";
                    else
                        dt.Rows[i]["IsBangding"] = "未绑定";
                }

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "GPS销售管理";
                dr["CaoZuoNeiRong"] = "web内用户查询GPS设备销售列表。";
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

    [CSMethod("GPSDDSale")]
    public object GPSDDSale(int CurrentPage, int PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                int cp = CurrentPage;
                int ac = 0;

                string sql = "select * from GpsDingDanSale a where UserID = @UserID and GpsDingDanIsEnd = 1 order by GpsDingDanTime asc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                DataTable dt = db.GetPagedDataTable(cmd, PageSize, ref cp, out ac);

                #region  插入操作表
                DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                DataRow dr = dt_caozuo.NewRow();
                dr["UserID"] = SystemUser.CurrentUser.UserID;
                dr["CaoZuoLeiXing"] = "销售订单列表";
                dr["CaoZuoNeiRong"] = "web内用户查询销售订单列表。";
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

    [CSMethod("AddGPSSale")]
    public object AddGPSSale(string GpsDeviceID)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShengChengDingDanSale.ashx";
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

    [CSMethod("GetZhiFuGPSSale")]
    public object GetZhiFuGPSSale()
    {
        using (var db = new DBConnection())
        {
            try
            {
                string username = SystemUser.CurrentUser.UserName;
                string userid = SystemUser.CurrentUser.UserID;

                string sql = "select * from GpsDingDanSale where UserID = '" + userid + "' and GpsDingDanIsEnd = 0";
                DataTable dt_gpsdd = db.ExecuteDataTable(sql);
                if (dt_gpsdd.Rows.Count > 0)
                {
                    sql = "select * from GpsDingDanSaleMingXi where GpsDingDanDenno = '" + dt_gpsdd.Rows[0]["GpsDingDanDenno"].ToString() + "'";
                    DataTable dt = db.ExecuteDataTable(sql);
                    return new { dt = dt, OrderDenno = dt_gpsdd.Rows[0]["OrderDenno"].ToString() };
                }
                else
                {
                    DataTable dt = db.GetEmptyDataTable("GpsDingDanSaleMingXi");
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

    [CSMethod("GDGPaySale")]
    public bool GDGPaySale(string OrderDenno, string DGZZCompany, string DGZH, string DKPZH)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from GpsDingDanSale where OrderDenno = '" + OrderDenno + "'";
                DataTable dt_dingdan = db.ExecuteDataTable(sql);

                string sql_dingdan = "update GpsDingDanSale set GpsDingDanZhiFuZhuangTai = 1,GpsDingDanSH = 0,GpsDingDanZhiFuLeiXing = '公对公',GpsDingDanZhiFuShiJian = '" + DateTime.Now + "' where OrderDenno = '" + OrderDenno + "'";
                db.ExecuteNonQuery(sql_dingdan);

                if (dt_dingdan.Rows.Count > 0)
                {
                    string sql_mx = "select * from GpsDingDanSaleMingXi where GpsDingDanDenno = '" + dt_dingdan.Rows[0]["GpsDingDanDenno"].ToString() + "'";
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

                DataTable dt_gdg = db.GetEmptyDataTable("GpsDingDanSaleGDG");
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

    [CSMethod("TJDDSale")]
    public object TJDDSale(string OrderDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_TiJiaoDingDanSale.ashx";
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

    [CSMethod("DelDDSale")]
    public object DelDDSale(string OrderDenno)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShanChuDingDanSale.ashx";
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

    [CSMethod("DeleteDDItemSale")]
    public object DeleteDDItemSale(string GpsDingDanMingXiID)
    {
        try
        {
            string url = "http://chb.yk56.net/WebService/APP_ShanChuDingDanOneSale.ashx";
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
                dr["ChongZhiSH"] = 0;
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

    [CSMethod("ShowAliByCZ")]
    public bool ShowAliByCZ(string OrderDenno, decimal ChongZhiJinE, int ChongZhiCiShu, string ChongZhiRemark)
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
                dr["ChongZhiSH"] = 0;
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
                dr["ChongZhiSH"] = 0;
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
            string sql = @"select CAST(ChongZhiID AS varchar) ChongZhiID,ChongZhiJinE,ChongZhiTime,ChongZhiRemark,0 as LX from ChongZhi where UserID = @UserID and ZhiFuZhuangTai = 1 and ChongZhiID not in (select a.ChongZhiID from InvoiceMxModel a left join InvoiceModel b on a.InvoiceId = b.InvoiceId where b.UserId = @UserID and a.ChongZhiID > 0)
                           UNION ALL
                           select GpsDingDanDenno as ChongZhiID,GpsDingDanJinE as ChongZhiJinE,GpsDingDanZhiFuShiJian as ChongZhiTime,'销售订单' as ChongZhiRemark,1 as LX from GpsDingDanSale where UserID = @UserID and GpsDingDanZhiFuZhuangTai = 1 and GpsDingDanDenno not in (select a.SaleID from InvoiceMxModel a left join InvoiceModel b on a.InvoiceId = b.InvoiceId where b.UserId = @UserID and a.SaleID is not null)";
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

            sql = @"select CAST(ChongZhiID AS varchar) ChongZhiID,ChongZhiJinE,ChongZhiTime,ChongZhiRemark,0 as LX from ChongZhi where UserID = @UserID and ZhiFuZhuangTai = 1 and ChongZhiID not in (select a.ChongZhiID from InvoiceMxModel a left join InvoiceModel b on a.InvoiceId = b.InvoiceId where b.UserId = @UserID and a.ChongZhiID > 0)
                    UNION ALL
                    select GpsDingDanDenno as ChongZhiID,GpsDingDanJinE as ChongZhiJinE,GpsDingDanZhiFuShiJian as ChongZhiTime,'销售订单' as ChongZhiRemark,1 as LX from GpsDingDanSale where UserID = @UserID and GpsDingDanZhiFuZhuangTai = 1 and GpsDingDanDenno not in (select a.SaleID from InvoiceMxModel a left join InvoiceModel b on a.InvoiceId = b.InvoiceId where b.UserId = @UserID and a.SaleID is not null)";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.Add("@UserID", dt_user.Rows[0]["UserID"].ToString());
            DataTable dt = db.ExecuteDataTable(cmd);
            return dt;
        }
    }

    [CSMethod("AddInvoice")]
    public bool AddInvoice(string InvoiceTitle, string InvoiceZZJGDM, string InvoicePerson, string InvoiceMobile, string InvoiceAddress, string je, string ChongZhiIDs, string SaleIDs)
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
            string[] ids2 = SaleIDs.Split(',');
            for (int i = 0; i < ids2.Length; i++)
            {
                if (!string.IsNullOrEmpty(ids2[i]))
                {
                    DataRow dr_mx = dt_mx.NewRow();
                    dr_mx["InvoiceMxId"] = Guid.NewGuid();
                    dr_mx["InvoiceId"] = dr["InvoiceId"];
                    dr_mx["SaleID"] = ids2[i];
                    dt_mx.Rows.Add(dr_mx);
                }
            }
            db.InsertTable(dt_mx);
            return true;
        }
    }

    [CSMethod("AddInvoiceByMobile")]
    public bool AddInvoiceByMobile(string UserName, string InvoiceTitle, string InvoiceZZJGDM, string InvoicePerson, string InvoiceMobile, string InvoiceAddress, string je, string ChongZhiIDs,string SaleIDs)
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
                string[] ids2 = Regex.Replace(SaleIDs, "[^a-zA-Z0-9-,]", string.Empty).Split(',');
                for (int i = 0; i < ids2.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ids2[i]))
                    {
                        DataRow dr_mx = dt_mx.NewRow();
                        dr_mx["InvoiceMxId"] = Guid.NewGuid();
                        dr_mx["InvoiceId"] = dr["InvoiceId"];
                        dr_mx["SaleID"] = ids2[i];
                        dt_mx.Rows.Add(dr_mx);
                    }
                }
                db.InsertTable(dt_mx);
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
            if (!string.IsNullOrEmpty(clientId) && clientId != "undefined" && clientId != "null")
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
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    [CSMethod("DownLoadMb", 2)]
    public byte[] DownLoadMb(string id)
    {
        try
        {
            Workbook book = new Workbook();

            book.Open(HttpContext.Current.Server.MapPath("~/Mb/制单模板.xlsx"));

            System.IO.MemoryStream ms = new MemoryStream();

            book.Save(ms, FileFormatType.Excel2007Xlsx);

            return ms.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [CSMethod("UploadSJ", 1)]
    public object UploadSJ(FileData[] fds, string lx)
    {
        var user = SystemUser.CurrentUser;
        string userid = user.UserID;

        using (DBConnection dbc = new DBConnection())
        {
            dbc.BeginTransaction();
            try
            {
                if (fds[0].FileBytes.Length == 0)
                {
                    throw new Exception("你上传的文件可能已被打开，请关闭该文件！");
                }

                System.IO.MemoryStream ms = new System.IO.MemoryStream(fds[0].FileBytes);
                Workbook workbook = new Workbook();
                workbook.Open(ms);
                Worksheet sheet = workbook.Worksheets[0];
                Cells cells = sheet.Cells;

                int firstRow = 0;

                //DataTable dataTable = cells.ExportDataTableAsString(firstRow, 0, cells.MaxRow + 1, 6);
                DataTable dataTable = cells.ExportDataTableAsString(0, 0, 100, 100);
                DataTable dt_new = new DataTable();
                dt_new.Columns.Add("MC");
                dt_new.Columns.Add("VAL");

                List<string> val_list = new List<string>();
                for (var i = 0; i < 100; i++)
                {
                    for (var j = 0; j < 100; j++)
                    {
                        if(!string.IsNullOrEmpty(dataTable.Rows[i][j].ToString().Replace(" ","")) && j < 99)
                        {
                            val_list.Add(dataTable.Rows[i][j].ToString().Replace(" ", ""));
                        }
                    }
                }
                for (var i = 0; i < val_list.Count() - 1; i++)
                {
                    DataRow dr2 = dt_new.NewRow();
                    dr2["MC"] = val_list[i];
                    dr2["VAL"] = val_list[i + 1];
                    dt_new.Rows.Add(dr2);
                }
                string sql_sel = "select count(*) num from DingDanSetModel where UserID = @UserID";
                SqlCommand cmd_sel = dbc.CreateCommand(sql_sel);
                cmd_sel.Parameters.Add("@UserID",userid);
                int count = Convert.ToInt32(dbc.ExecuteScalar(cmd_sel).ToString());

                DataTable dt = new DataTable();

                DataTable dt_column = new DataTable();
                dt_column.Columns.Add("MC");

                if (count == 0)
                {
                    dt.Columns.Add("GpsDeviceID");
                    dt.Columns.Add("QiShiZhan_Province");
                    dt.Columns.Add("QiShiZhan_City");
                    dt.Columns.Add("QiShiZhan_Qx");
                    dt.Columns.Add("DaoDaZhan_Province");
                    dt.Columns.Add("DaoDaZhan_City");
                    dt.Columns.Add("DaoDaZhan_Qx");
                    dt.Columns.Add("SuoShuGongSi");
                    dt.Columns.Add("UserDenno");
                    dt.Columns.Add("Expect_Hour");
                    dt.Columns.Add("MessageTel");
                    dt.Columns.Add("QiShiAddress");
                    dt.Columns.Add("DaoDaAddress");
                    dt.Columns.Add("SalePerson");
                    dt.Columns.Add("YunDanRemark");
                    dt.Columns.Add("CarrierCompany");
                    dt.Columns.Add("CarrierPerson");
                    dt.Columns.Add("CarrierTel");
                    dt.Columns.Add("Purchaser");
                    dt.Columns.Add("PurchaserPerson");
                    dt.Columns.Add("PurchaserTel");

                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < dt_new.Rows.Count; i++)
                    {
                        switch (dt_new.Rows[i]["MC"].ToString())
                        {
                            case "设备码":
                                dr["GpsDeviceID"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "出发地省份":
                                dr["QiShiZhan_Province"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "出发地城市":
                                dr["QiShiZhan_City"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "出发地区县":
                                dr["QiShiZhan_Qx"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "目的地省份":
                                dr["DaoDaZhan_Province"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "目的地城市":
                                dr["DaoDaZhan_City"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "目的地区县":
                                dr["DaoDaZhan_Qx"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "建单公司":
                                dr["SuoShuGongSi"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "建单号":
                                dr["UserDenno"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "预计到达时间（小时）":
                                dr["Expect_Hour"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "推送短信":
                                dr["MessageTel"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "出发地详细地址":
                                dr["QiShiAddress"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "目的地详细地址":
                                dr["DaoDaAddress"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "销售员":
                                dr["SalePerson"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "货物信息备注":
                                dr["YunDanRemark"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "承运公司（专线）":
                                dr["CarrierCompany"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "负责人":
                                dr["CarrierPerson"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "负责人联系方式":
                                dr["CarrierTel"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "收货单位":
                                dr["Purchaser"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "收货人":
                                dr["PurchaserPerson"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                            case "收货人联系方式":
                                dr["PurchaserTel"] = dt_new.Rows[i]["VAL"].ToString();
                                break;
                        }
                    }

                    dt.Rows.Add(dr);
                }
                else
                {
                    dt.Columns.Add("GpsDeviceID");
                    dt.Columns.Add("QiShiZhan_Province");
                    dt.Columns.Add("QiShiZhan_City");
                    dt.Columns.Add("QiShiZhan_Qx");
                    dt.Columns.Add("DaoDaZhan_Province");
                    dt.Columns.Add("DaoDaZhan_City");
                    dt.Columns.Add("DaoDaZhan_Qx");
                    dt.Columns.Add("SuoShuGongSi");
                    dt.Columns.Add("UserDenno");
                    dt.Columns.Add("Expect_Hour");
                    dt.Columns.Add("MessageTel");

                    string sql_user = "select * from DingDanSetList where UserID = @UserID";
                    SqlCommand cmd_user = dbc.CreateCommand(sql_user);
                    cmd_user.Parameters.Add("@UserID", userid);
                    DataTable dt_user = dbc.ExecuteDataTable(cmd_user);

                    for (var i = 0; i < dt_user.Rows.Count; i++)
                    { 
                        if(string.IsNullOrEmpty(dt_user.Rows[i]["DingDanSetListBS"].ToString()))
                            dt.Columns.Add("div" + dt_user.Rows[i]["DingDanSetListPX"].ToString());
                        else
                            dt.Columns.Add(dt_user.Rows[i]["DingDanSetListBS"].ToString());
                    }

                    for (var i = 0; i < dt_user.Rows.Count; i++)
                    {
                        DataRow dr_column = dt_column.NewRow();
                        if (string.IsNullOrEmpty(dt_user.Rows[i]["DingDanSetListBS"].ToString()))
                            dr_column["MC"] = "div" + dt_user.Rows[i]["DingDanSetListPX"].ToString();
                        else
                            dr_column["MC"] = dt_user.Rows[i]["DingDanSetListBS"].ToString();
                        dt_column.Rows.Add(dr_column);
                    }


                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < dt_new.Rows.Count; i++)
                    {
                        DataRow[] drs = dt_user.Select("DingDanSetListMC = '" + dt_new.Rows[i]["MC"].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            if (string.IsNullOrEmpty(drs[0]["DingDanSetListBS"].ToString()))
                                dr["div" + drs[0]["DingDanSetListPX"].ToString()] = dt_new.Rows[i]["VAL"].ToString();
                            else
                                dr[drs[0]["DingDanSetListBS"].ToString()] = dt_new.Rows[i]["VAL"].ToString();
                        }
                        else
                        {
                            switch (dt_new.Rows[i]["MC"].ToString())
                            {
                                case "设备码":
                                    dr["GpsDeviceID"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "出发地省份":
                                    dr["QiShiZhan_Province"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "出发地城市":
                                    dr["QiShiZhan_City"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "出发地区县":
                                    dr["QiShiZhan_Qx"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "目的地省份":
                                    dr["DaoDaZhan_Province"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "目的地城市":
                                    dr["DaoDaZhan_City"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "目的地区县":
                                    dr["DaoDaZhan_Qx"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "建单公司":
                                    dr["SuoShuGongSi"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "建单号":
                                    dr["UserDenno"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "预计到达时间（小时）":
                                    dr["Expect_Hour"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                                case "推送短信":
                                    dr["MessageTel"] = dt_new.Rows[i]["VAL"].ToString();
                                    break;
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }

                dbc.CommitTransaction();
                return new { dt = dt, dt_column = dt_column };
            }
            catch (Exception ex)
            {
                dbc.RoolbackTransaction();
                throw ex;
            }
        }


    }

    public DataTable WoDeYunDanByAll(string UserName,string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByAllByPage(string UserName, string UserDenno,string CurrentPage,string PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                int cp = Convert.ToInt32(CurrentPage);
                int ac = 0;

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.GetPagedDataTable(cmd, Convert.ToInt32(PageSize), ref cp, out ac);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public int WoDeYunDanByAllByNum(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                }

                db.CommitTransaction();

                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByGZ(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and IsBangding = 1 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByGZByPage(string UserName, string UserDenno, string CurrentPage, string PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                int cp = Convert.ToInt32(CurrentPage);
                int ac = 0;

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and IsBangding = 1 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.GetPagedDataTable(cmd, Convert.ToInt32(PageSize), ref cp, out ac);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public int WoDeYunDanByGZByNum(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and IsBangding = 1 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                }

                db.CommitTransaction();

                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByHis(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and IsBangding = 0 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 0 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 0 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByHisByPage(string UserName, string UserDenno, string CurrentPage, string PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                int cp = Convert.ToInt32(CurrentPage);
                int ac = 0;

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and IsBangding = 0 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.GetPagedDataTable(cmd, Convert.ToInt32(PageSize), ref cp, out ac);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 0 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 0 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public int WoDeYunDanByHisByNum(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and IsBangding = 0 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                }

                db.CommitTransaction();

                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByYJ(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = @"select a.* from YunDan a 
                          inner join (
	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                          ) b on a.YunDanDenno = b.YunDanDenno
                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable WoDeYunDanByYJByPage(string UserName, string UserDenno, string CurrentPage, string PageSize)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                int cp = Convert.ToInt32(CurrentPage);
                int ac = 0;

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = @"select a.* from YunDan a 
                          inner join (
	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                          ) b on a.YunDanDenno = b.YunDanDenno
                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.GetPagedDataTable(cmd, Convert.ToInt32(PageSize), ref cp, out ac);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "我的运单";
                        dr["CaoZuoNeiRong"] = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and IsBangding = 1 " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public int WoDeYunDanByYJByNum(string UserName, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno like @UserDenno";
                    sql = @"select a.* from YunDan a 
                          inner join (
	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                          ) b on a.YunDanDenno = b.YunDanDenno
                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                }

                db.CommitTransaction();

                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable ZiYouSearch(string UserName)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    sql = "select * from ZiYouSearch where UserID = @UserID";
                    cmd = db.CreateCommand(sql);
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                }

                db.CommitTransaction();
                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public DataTable ZiYouChaDan(string UserName, string UserDenno, string SuoShuGongSi)
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select * from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                DataTable dt_user = db.ExecuteDataTable(cmd);

                DataTable dt = new DataTable();
                if (dt_user.Rows.Count > 0)
                {
                    string conn = "";
                    if (!string.IsNullOrEmpty(UserDenno))
                        conn = " and UserDenno = @UserDenno";
                    sql = "select * from YunDan where UserID = @UserID and SuoShuGongSi = @SuoShuGongSi and IsBangding = 0 " + conn + " order by BangDingTime desc";
                    cmd = db.CreateCommand(sql);
                    if (!string.IsNullOrEmpty(UserDenno))
                        cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                    cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                    dt = db.ExecuteDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                        DataRow dr = dt_caozuo.NewRow();
                        dr["UserID"] = dt_user.Rows[0]["UserID"];
                        dr["CaoZuoLeiXing"] = "自由查单";
                        dr["CaoZuoNeiRong"] = "APP内用户自由查单查询，搜索单号：" + UserDenno + "；搜索公司：" + SuoShuGongSi + "。";
                        dr["CaoZuoTime"] = DateTime.Now;
                        dr["CaoZuoRemark"] = "";
                        dt_caozuo.Rows.Add(dr);
                        db.InsertTable(dt_caozuo);

                        sql = "select * from SearchHistory where UserID = @UserID and Value = @SuoShuGongSi and Type = '自由查单_公司'";
                        cmd = db.CreateCommand(sql);
                        cmd.Parameters.AddWithValue("@SuoShuGongSi", SuoShuGongSi);
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_search = db.ExecuteDataTable(cmd);

                        if (dt_search.Rows.Count == 0)
                        {
                            DataTable dt_search_new = db.GetEmptyDataTable("SearchHistory");
                            DataRow dr_search_new = dt_search_new.NewRow();
                            dr_search_new["UserID"] = dt_user.Rows[0]["UserID"].ToString();
                            dr_search_new["Type"] = "自由查单_公司";
                            dr_search_new["Value"] = SuoShuGongSi;
                            dt_search_new.Rows.Add(dr_search_new);
                            db.InsertTable(dt_search_new);
                        }

                        sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_arrive = db.ExecuteDataTable(cmd);

                        sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID " + conn + ")";
                        cmd = db.CreateCommand(sql);
                        if (!string.IsNullOrEmpty(UserDenno))
                            cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno + "%");
                        cmd.Parameters.AddWithValue("@UserID", dt_user.Rows[0]["UserID"].ToString());
                        DataTable dt_distance = db.ExecuteDataTable(cmd);

                        DataTable dt_ziyou = db.GetEmptyDataTable("ZiYouSearch");
                        DataRow dr_ziyou = dt_ziyou.NewRow();
                        dr_ziyou["UserID"] = dt_user.Rows[0]["UserID"].ToString();
                        dr_ziyou["SuoShuGongSi"] = SuoShuGongSi;
                        dr_ziyou["UserDenno"] = UserDenno;
                        dt_ziyou.Rows.Add(dr_ziyou);
                        db.InsertOrUpdateTable(dt_ziyou);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] drs = dt_arrive.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                            if (drs.Length == 0)
                            {
                                DataRow[] drs_distance = dt_distance.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                                if (drs_distance.Length > 0)
                                {
                                    dt.Rows[i]["Gps_distance"] = drs_distance[0]["Gps_distance"];
                                    dt.Rows[i]["Gps_duration"] = drs_distance[0]["Gps_duration"];
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();

                return dt;
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }
    }

    public int UpdateYunDanAddress(string UserID, string QiShiZhan, string DaoDaZhan, string UserDenno)
    { 
        using(var db = new DBConnection())
        {
            int sign = 0;//更新失败
            try
            {
                db.BeginTransaction();
                string sql_device = "select count(*) NUM from YunDan where UserID = @UserID and UserDenno = @UserDenno and IsBangding = 1";
                SqlCommand cmd = db.CreateCommand(sql_device);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@GpsDeviceID", UserDenno);
                DataTable dt_device = db.ExecuteDataTable(cmd);
                if (dt_device.Rows[0]["NUM"].ToString() == "0")
                {
                    sign = 2;//未找到该运单号
                }
                else
                {
                    string[] qsz = QiShiZhan.Split(' ');
                    string qsz1 = "";
                    string qsz2 = "";
                    string qsz3 = "";
                    string[] ddz = DaoDaZhan.Split(' ');
                    string ddz1 = "";
                    string ddz2 = "";
                    string ddz3 = "";
                    if (ddz.Length > 0 && qsz.Length > 0 && !string.IsNullOrEmpty(QiShiZhan) && !string.IsNullOrEmpty(DaoDaZhan))
                    {
                        qsz1 = qsz[0];
                        qsz2 = qsz[1];
                        ddz1 = ddz[0];
                        ddz2 = ddz[1];
                        if(qsz.Length == 3)
                            qsz3 = qsz[2];
                        if (ddz.Length == 3)
                            ddz3 = ddz[2];
                        string sql = "update YunDan set QiShiZhan = @QiShiZhan,DaoDaZhan = @DaoDaZhan,QiShiZhan_QX = @QiShiZhan_QX,DaoDaZhan_QX = @DaoDaZhan_QX where UserID = @UserID and UserDenno = @UserDenno and IsBangding = 1";
                        SqlCommand cmd2 = db.CreateCommand(sql);
                        cmd2.Parameters.AddWithValue("@QiShiZhan", qsz1 + " " + qsz2);
                        cmd2.Parameters.AddWithValue("@DaoDaZhan", ddz1 + " " + ddz2);
                        cmd2.Parameters.AddWithValue("@QiShiZhan_QX", string.IsNullOrEmpty(qsz3) ? DBNull.Value : (object)qsz3);
                        cmd2.Parameters.AddWithValue("@DaoDaZhan_QX", string.IsNullOrEmpty(ddz3) ? DBNull.Value : (object)ddz3);
                        cmd2.Parameters.AddWithValue("@UserID", UserID);
                        cmd2.Parameters.AddWithValue("@UserDenno", UserDenno);
                        db.ExecuteNonQuery(cmd2);
                        db.CommitTransaction();
                        sign = 1;
                    }
                    else
                    {
                        sign = 3;//起始站或到达站不能为空
                    }
                }

            }
            catch (Exception ex)
            {
                sign = 100;//内部错误
                db.RoolbackTransaction();
            }
            return sign;
        }
    }

    public int AddWayBill(string UserID, string QiShiZhan, string DaoDaZhan, string SuoShuGongSi, string UserDenno, string GpsDeviceID,string ExpectHour, string YunDanRemark)
    {
        using (var db = new DBConnection())
        {
            db.BeginTransaction();
            int sign = 0;//制单失败
            
            try
            {
                string sql_device = "select count(*) NUM from GpsDevice where UserID = @UserID and GpsDeviceID = @GpsDeviceID";
                SqlCommand cmd = db.CreateCommand(sql_device);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                DataTable dt_device = db.ExecuteDataTable(cmd);

                if (dt_device.Rows[0]["NUM"].ToString() == "0")
                {
                    sign = 2;//用户标示或设备码错误
                }
                else
                {
                    
                    string sql_user = "select * from [dbo].[User] where UserID = @UserID";
                    cmd = db.CreateCommand(sql_user);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    DataTable dt_user = db.ExecuteDataTable(cmd);

                    if (Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) == 0)
                    {
                        sign = 4;//充值次数已用光，请充值后再制单
                    }
                    else
                    {
                        string sql_up = "update YunDan set IsBangding = 0,JieBangTime =@JieBangTime,GpsDevicevid='',GpsDevicevKey='' where GpsDeviceID = @GpsDeviceID and IsBangding = 1";
                        cmd = db.CreateCommand(sql_up);
                        cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                        cmd.Parameters.AddWithValue("@JieBangTime", DateTime.Now);
                        db.ExecuteNonQuery(cmd);

                        string sql_yun = "select * from YunDan where GpsDeviceID = @GpsDeviceID and IsBangding = 1";
                        cmd = db.CreateCommand(sql_yun);
                        cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                        DataTable dt_yun_num = db.ExecuteDataTable(cmd);
                        if (dt_yun_num.Rows.Count > 0)
                        {
                            sign = 5;//已绑定设备，请先解绑已绑定的设备的运单
                        }
                        else
                        {
                            #region  更新设备绑定状态
                            string sql = "update YunDan set IsBangding = 0 where GpsDeviceID = @GpsDeviceID";
                            cmd = db.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID);
                            db.ExecuteNonQuery(cmd);
                            #endregion

                            #region  更新用户剩余次数
                            int UserRemainder = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) - 1;
                            sql = "update [dbo].[User] set UserRemainder = @UserRemainder where UserID = @UserID";
                            cmd = db.CreateCommand(sql);
                            cmd.Parameters.AddWithValue("@UserID", UserID);
                            cmd.Parameters.AddWithValue("@UserRemainder", UserRemainder);
                            db.ExecuteNonQuery(cmd);
                            #endregion

                            #region  插入运单表
                            Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + GpsDeviceID + "&pwd=123456");
                            if (gpsinfo["success"].ToString().ToUpper() == "True".ToUpper())
                            {
                                gpsinfo["sign"] = "1";
                            }
                            else
                            {
                                gpsinfo["sign"] = "0";
                            }
                            string gpsvid = "";
                            string gpsvkey = "";
                            if (gpsinfo["sign"].ToString() == "1")
                            {
                                gpsvid = gpsinfo["vid"].ToString();
                                gpsvkey = gpsinfo["vKey"].ToString();

                                Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");

                                string newlng = "";
                                string newlat = "";
                                string newinfo = "";
                                DateTime gpstm = DateTime.Now;
                                if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                                {
                                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                                    string newgpstime = ja.First()["gpstime"].ToString();
                                    //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                                    newlng = ja.First()["lng"].ToString();
                                    //newlng = newlng.Substring(0, newlng.Length - 2);
                                    newlat = ja.First()["lat"].ToString();
                                    //newlat = newlat.Substring(0, newlat.Length - 2);
                                    newinfo = ja.First()["info"].ToString();
                                    //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                                    //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                                    long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                                    DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                                    long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                                    gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                                    sql = "select * from GpsLocation where Gps_time = @Gps_time and GpsDeviceID = @GpsDeviceID";
                                    cmd = db.CreateCommand(sql);
                                    cmd.Parameters.Add("@Gps_time", gpstm);
                                    cmd.Parameters.Add("@GpsDeviceID", GpsDeviceID);
                                    DataTable dt_locations = db.ExecuteDataTable(cmd);
                                    if (dt_locations.Rows.Count > 0)
                                    {
                                        DataTable dt_location_new = db.GetEmptyDataTable("GpsLocation");
                                        DataRow dr_location = dt_location_new.NewRow();
                                        dr_location["GpsDeviceID"] = GpsDeviceID;
                                        dr_location["Gps_lat"] = newlat;
                                        dr_location["Gps_lng"] = newlng;
                                        dr_location["Gps_time"] = gpstm;
                                        dr_location["Gps_info"] = newinfo;
                                        dr_location["GpsRemark"] = "自动定位";
                                        dt_location_new.Rows.Add(dr_location);
                                        db.InsertTable(dt_location_new);
                                    }
                                    //获取起始站、到达站位置
                                    string QiShiZhan_lat = "";
                                    string QiShiZhan_lng = "";
                                    string DaoDaZhan_lat = "";
                                    string DaoDaZhan_lng = "";

                                    var QiShiZhan_old = QiShiZhan;
                                    var DaoDaZhan_old = DaoDaZhan;

                                    if (!string.IsNullOrEmpty(QiShiZhan))
                                    {
                                        string[] qsz1 = QiShiZhan.Split(' ');
                                        if (qsz1.Length==2)
                                        {
                                            if (qsz1[0] == qsz1[1])
                                            {
                                                QiShiZhan = qsz1[0];
                                            }
                                        }
                                        else if (qsz1.Length == 3)
                                        {
                                            if (qsz1[0] == qsz1[1])
                                            {
                                                QiShiZhan = qsz1[1] + " " + qsz1[2];
                                            }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(DaoDaZhan))
                                    {
                                        string[] ddz1 = DaoDaZhan.Split(' ');
                                        if (ddz1.Length == 2)
                                        {
                                            if (ddz1[0] == ddz1[1])
                                            {
                                                DaoDaZhan = ddz1[0];
                                            }
                                        }
                                        else if (ddz1.Length == 3)
                                        {
                                            if (ddz1[0] == ddz1[1])
                                            {
                                                DaoDaZhan = ddz1[1] + " " + ddz1[2];
                                            }
                                        }
                                    }

                                    Hashtable addresshash = getmapinfobyaddress(QiShiZhan, "");
                                    if (addresshash["sign"] == "1")
                                    {
                                        QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                                        QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                                    }
                                    QiShiZhan = QiShiZhan_old;
                                    Hashtable daozhanaddresshash = getmapinfobyaddress(DaoDaZhan, "");
                                    if (daozhanaddresshash["sign"] == "1")
                                    {
                                        DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                                        DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                                    }
                                    DaoDaZhan = DaoDaZhan_old;
                                    if (!string.IsNullOrEmpty(QiShiZhan_lat) && !string.IsNullOrEmpty(DaoDaZhan_lat))
                                    {
                                        
                                        string[] qsz = QiShiZhan.Split(' ');
                                        string qsz1 = "";
                                        string qsz2 = "";
                                        string qsz3 = "";
                                        string[] ddz = DaoDaZhan.Split(' ');
                                        string ddz1 = "";
                                        string ddz2 = "";
                                        string ddz3 = "";
                                        if (ddz.Length > 0 && qsz.Length > 0 && !string.IsNullOrEmpty(QiShiZhan) && !string.IsNullOrEmpty(DaoDaZhan))
                                        {
                                            qsz1 = qsz[0];
                                            qsz2 = qsz[1];
                                            ddz1 = ddz[0];
                                            ddz2 = ddz[1];
                                            if (qsz.Length == 3)
                                                qsz3 = qsz[2];
                                            if (ddz.Length == 3)
                                                ddz3 = ddz[2];
                                            DataTable dt_yundan = db.GetEmptyDataTable("YunDan");
                                            DataRow dr_yundan = dt_yundan.NewRow();
                                            dr_yundan["YunDanDenno"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                            dr_yundan["UserDenno"] = UserDenno;
                                            dr_yundan["UserID"] = UserID;
                                            if (qsz1 == qsz2)
                                                dr_yundan["QiShiZhan"] = qsz1 + " " + qsz3;
                                            else
                                                dr_yundan["QiShiZhan"] = qsz1 + " " + qsz2;
                                            if (ddz1 == ddz2)
                                                dr_yundan["DaoDaZhan"] = ddz1 + " " + ddz3;
                                            else
                                                dr_yundan["DaoDaZhan"] = ddz1 + " " + ddz2;
                                            dr_yundan["SuoShuGongSi"] = SuoShuGongSi;
                                            dr_yundan["BangDingTime"] = DateTime.Now;
                                            dr_yundan["GpsDeviceID"] = GpsDeviceID;
                                            dr_yundan["Gps_lastlat"] = newlat;
                                            dr_yundan["Gps_lastlng"] = newlng;
                                            if (newinfo != "")
                                            {
                                                dr_yundan["Gps_lasttime"] = gpstm;
                                            }
                                            dr_yundan["Gps_lastinfo"] = newinfo;
                                            dr_yundan["IsBangding"] = true;
                                            dr_yundan["YunDanRemark"] = YunDanRemark;
                                            dr_yundan["Expect_Hour"] = Convert.ToDecimal(ExpectHour);
                                            dr_yundan["QiShiZhan_lat"] = QiShiZhan_lat;
                                            dr_yundan["QiShiZhan_lng"] = QiShiZhan_lng;
                                            dr_yundan["DaoDaZhan_lat"] = DaoDaZhan_lat;
                                            dr_yundan["DaoDaZhan_lng"] = DaoDaZhan_lng;
                                            dr_yundan["QiShiZhan_QX"] = string.IsNullOrEmpty(qsz3) ? DBNull.Value : (object)qsz3;
                                            dr_yundan["DaoDaZhan_QX"] = string.IsNullOrEmpty(ddz3) ? DBNull.Value : (object)ddz3;
                                            dt_yundan.Rows.Add(dr_yundan);
                                            db.InsertTable(dt_yundan);
                                            sign = 1;
                                        }

                                        
                                        db.CommitTransaction();
                                    }
                                    else
                                    {
                                        sign = 3;
                                    }
                                }
                            }
                            else
                            {
                                Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + GpsDeviceID + "");

                                string newlng = "";
                                string newlat = "";
                                string newinfo = "";
                                DateTime gpstm = DateTime.Now;
                                if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                                {
                                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                                    string newgpstime = ja.First()["gpstime"].ToString();
                                    //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                                    newlng = ja.First()["lng"].ToString();
                                    //newlng = newlng.Substring(0, newlng.Length - 2);
                                    newlat = ja.First()["lat"].ToString();
                                    //newlat = newlat.Substring(0, newlat.Length - 2);
                                    newinfo = ja.First()["info"].ToString();
                                    //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                                    //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                                    long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                                    DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                                    long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                                    gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                                    sql = "select * from GpsLocation where Gps_time = @Gps_time and GpsDeviceID = @GpsDeviceID";
                                    cmd = db.CreateCommand(sql);
                                    cmd.Parameters.Add("@Gps_time", gpstm);
                                    cmd.Parameters.Add("@GpsDeviceID", GpsDeviceID);
                                    DataTable dt_locations = db.ExecuteDataTable(cmd);
                                    if (dt_locations.Rows.Count > 0)
                                    {
                                        DataTable dt_location_new = db.GetEmptyDataTable("GpsLocation");
                                        DataRow dr_location = dt_location_new.NewRow();
                                        dr_location["GpsDeviceID"] = GpsDeviceID;
                                        dr_location["Gps_lat"] = newlat;
                                        dr_location["Gps_lng"] = newlng;
                                        dr_location["Gps_time"] = gpstm;
                                        dr_location["Gps_info"] = newinfo;
                                        dr_location["GpsRemark"] = "自动定位";
                                        dt_location_new.Rows.Add(dr_location);
                                        db.InsertTable(dt_location_new);
                                    }
                                    //获取起始站、到达站位置
                                    string QiShiZhan_lat = "";
                                    string QiShiZhan_lng = "";
                                    string DaoDaZhan_lat = "";
                                    string DaoDaZhan_lng = "";
                                    var QiShiZhan_old = QiShiZhan;
                                    var DaoDaZhan_old = DaoDaZhan;
                                    if (!string.IsNullOrEmpty(QiShiZhan))
                                    {
                                        string[] qsz1 = QiShiZhan.Split(' ');
                                        if (qsz1.Length == 2)
                                        {
                                            if (qsz1[0] == qsz1[1])
                                            {
                                                QiShiZhan = qsz1[0];
                                            }
                                        }
                                        else if (qsz1.Length == 3)
                                        {
                                            if (qsz1[0] == qsz1[1])
                                            {
                                                QiShiZhan = qsz1[1] + " " + qsz1[2];
                                            }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(DaoDaZhan))
                                    {
                                        string[] ddz1 = DaoDaZhan.Split(' ');
                                        if (ddz1.Length == 2)
                                        {
                                            if (ddz1[0] == ddz1[1])
                                            {
                                                DaoDaZhan = ddz1[0];
                                            }
                                        }
                                        else if (ddz1.Length == 3)
                                        {
                                            if (ddz1[0] == ddz1[1])
                                            {
                                                DaoDaZhan = ddz1[1] + " " + ddz1[2];
                                            }
                                        }
                                    }

                                    Hashtable addresshash = getmapinfobyaddress(QiShiZhan, "");
                                    if (addresshash["sign"] == "1")
                                    {
                                        QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                                        QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                                    }
                                    QiShiZhan = QiShiZhan_old;
                                    Hashtable daozhanaddresshash = getmapinfobyaddress(DaoDaZhan, "");
                                    if (daozhanaddresshash["sign"] == "1")
                                    {
                                        DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                                        DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                                    }
                                    DaoDaZhan = DaoDaZhan_old;
                                    if (!string.IsNullOrEmpty(QiShiZhan_lat) && !string.IsNullOrEmpty(DaoDaZhan_lat))
                                    {
                                        string[] qsz = QiShiZhan.Split(' ');
                                        string qsz1 = "";
                                        string qsz2 = "";
                                        string qsz3 = "";
                                        string[] ddz = DaoDaZhan.Split(' ');
                                        string ddz1 = "";
                                        string ddz2 = "";
                                        string ddz3 = "";

                                        if (ddz.Length > 0 && qsz.Length > 0 && !string.IsNullOrEmpty(QiShiZhan) && !string.IsNullOrEmpty(DaoDaZhan))
                                        {
                                            qsz1 = qsz[0];
                                            qsz2 = qsz[1];
                                            ddz1 = ddz[0];
                                            ddz2 = ddz[1];
                                            if (qsz.Length == 3)
                                                qsz3 = qsz[2];
                                            if (ddz.Length == 3)
                                                ddz3 = ddz[2];
                                            DataTable dt_yundan = db.GetEmptyDataTable("YunDan");
                                            DataRow dr_yundan = dt_yundan.NewRow();
                                            dr_yundan["YunDanDenno"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                            dr_yundan["UserDenno"] = UserDenno;
                                            dr_yundan["UserID"] = UserID;
                                            if (qsz1 == qsz2)
                                                dr_yundan["QiShiZhan"] = qsz1 + " " + qsz3;
                                            else
                                                dr_yundan["QiShiZhan"] = qsz1 + " " + qsz2;
                                            if (ddz1 == ddz2)
                                                dr_yundan["DaoDaZhan"] = ddz1 + " " + ddz3;
                                            else
                                                dr_yundan["DaoDaZhan"] = ddz1 + " " + ddz2;
                                            dr_yundan["SuoShuGongSi"] = SuoShuGongSi;
                                            dr_yundan["BangDingTime"] = DateTime.Now;
                                            dr_yundan["GpsDeviceID"] = GpsDeviceID;
                                            dr_yundan["Gps_lastlat"] = newlat;
                                            dr_yundan["Gps_lastlng"] = newlng;
                                            if (newinfo != "")
                                            {
                                                dr_yundan["Gps_lasttime"] = gpstm;
                                            }
                                            dr_yundan["Gps_lastinfo"] = newinfo;
                                            dr_yundan["IsBangding"] = true;
                                            dr_yundan["YunDanRemark"] = YunDanRemark;
                                            dr_yundan["Expect_Hour"] = Convert.ToDecimal(ExpectHour);
                                            dr_yundan["QiShiZhan_lat"] = QiShiZhan_lat;
                                            dr_yundan["QiShiZhan_lng"] = QiShiZhan_lng;
                                            dr_yundan["DaoDaZhan_lat"] = DaoDaZhan_lat;
                                            dr_yundan["DaoDaZhan_lng"] = DaoDaZhan_lng;
                                            dr_yundan["QiShiZhan_QX"] = string.IsNullOrEmpty(qsz3) ? DBNull.Value : (object)qsz3;
                                            dr_yundan["DaoDaZhan_QX"] = string.IsNullOrEmpty(ddz3) ? DBNull.Value : (object)ddz3;
                                            dt_yundan.Rows.Add(dr_yundan);
                                            db.InsertTable(dt_yundan);
                                            sign = 1;
                                        }
                                        db.CommitTransaction();
                                    }
                                    else
                                    {
                                        sign = 3;
                                    }
                                }
                                else
                                {
                                    sign = 100;//内部错误
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sign = 100;//内部错误
                db.RoolbackTransaction();
            }
            return sign;
        }
    }

    public int RelieveWayBill(string UserID, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            int sign = 0;
            db.BeginTransaction();
            try
            {
                string sql = "select * from YunDan where UserID = @UserID and UserDenno = @UserDenno and IsBangding = 1";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.Add("@UserID", UserID);
                cmd.Parameters.Add("@UserDenno", UserDenno);
                DataTable dt = db.ExecuteDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    sql = "update YunDan set IsBangding = 0,JieBangTime =@JieBangTime,GpsDevicevid='',GpsDevicevKey='' where UserID = @UserID and UserDenno = @UserDenno";
                    cmd = db.CreateCommand(sql);
                    cmd.Parameters.Add("@UserID", UserID);
                    cmd.Parameters.Add("@UserDenno", UserDenno);
                    cmd.Parameters.Add("@JieBangTime", DateTime.Now);
                    db.ExecuteNonQuery(cmd);
                    sign = 1;
                }
                else
                {
                    sign = 2;
                }
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                sign = 100;
                db.RoolbackTransaction();
            }
            
            return sign;
        }
    }

    public DataTable GetWayBillMemoByUserDenno(string UserID, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select BangDingTime Time,UserDenno,QiShiZhan Departure,DaoDaZhan Destination,SuoShuGongSi Company,GpsDeviceID,YunDanRemark Memo,Gps_lastinfo,Gps_lastlat,Gps_lastlng,Gps_lasttime,QiShiZhan_QX,DaoDaZhan_QX,battery from YunDan where UserID = @UserID and UserDenno = @UserDenno";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt = db.ExecuteDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["Departure"] = dt.Rows[0]["Departure"].ToString() + " " + dt.Rows[0]["QiShiZhan_QX"];
                    dt.Rows[0]["Destination"] = dt.Rows[0]["Destination"].ToString() + " " + dt.Rows[0]["DaoDaZhan_QX"];
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public DataTable GetWayBillMemoByUserDennoZC(string UserID, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select BangDingTime Time,UserDenno,QiShiZhan Departure,DaoDaZhan Destination,SuoShuGongSi Company,GpsDeviceID,YunDanRemark Memo,Gps_lastinfo,Gps_lastlat,Gps_lasttime,Gps_lastlng,QiShiZhan_lat,QiShiZhan_lng,DaoDaZhan_lat,DaoDaZhan_lng,QiShiZhan_QX,DaoDaZhan_QX,battery from YunDan where UserID = @UserID and UserDenno = @UserDenno";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt = db.ExecuteDataTable(cmd);
                dt.Columns.Add("ArriveState");
                dt.Columns.Add("WarnState");

                if (dt.Rows.Count == 0)
                {
                    return dt;
                }
                else
                {
                    dt.Rows[0]["Departure"] = dt.Rows[0]["Departure"].ToString() + " " + dt.Rows[0]["QiShiZhan_QX"];
                    dt.Rows[0]["Destination"] = dt.Rows[0]["Destination"].ToString() + " " + dt.Rows[0]["DaoDaZhan_QX"];
                }

                sql = "select * from YunDanIsArrive where YunDanDenno in (select YunDanDenno from YunDan where UserID = @UserID and UserDenno = @UserDenno)";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt_arrive = db.ExecuteDataTable(cmd);
                if (dt_arrive.Rows.Count > 0)
                {
                    string DaoDaZhan = dt.Rows[0]["Destination"].ToString().Replace(" ", "");
                    string[] LastZhanArray = dt.Rows[0]["Gps_lastinfo"].ToString().Split(' ');
                    string LastZhan = "";
                    if (LastZhanArray.Length >= 2)
                    {
                        LastZhan = LastZhanArray[0] + LastZhanArray[1];
                    }
                    if (DaoDaZhan == LastZhan)
                    {
                        dt.Rows[0]["ArriveState"] = 1;//已达
                    }
                    else
                    {
                        dt.Rows[0]["ArriveState"] = 2;//返回
                    }
                }
                else
                { 
                   dt.Rows[0]["ArriveState"] = 0;//未达
                }

                sql = @"select * from YunDanDistance where YunDanDenno in (select a.YunDanDenno from YunDan a 
                        inner join (
                	        select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                	        inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                	        where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                        ) b on a.YunDanDenno = b.YunDanDenno
                        where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0 and a.UserDenno = @UserDenno and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive))";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt_yj = db.ExecuteDataTable(cmd);

                if (dt_yj.Rows.Count > 0)
                {
                    dt.Rows[0]["WarnState"] = "1";
                }
                else
                {
                    dt.Rows[0]["WarnState"] = "0";
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public DataTable GetLocationList(string UserID, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {

                string sql = "select * from YunDan where UserID = @UserID and UserDenno = @UserDenno";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt = db.ExecuteDataTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    DataTable dt_gps = new DataTable();
                    dt_gps.Columns.Add("Gps_lat");
                    dt_gps.Columns.Add("Gps_lng");
                    dt_gps.Columns.Add("Gps_time");
                    dt_gps.Columns.Add("Gps_info");
                    return dt_gps;
                }
                else
                {
                    string conn = "";
                    if (dt.Rows[0]["IsBangding"].ToString() == "False")
                        conn = " and Gps_time < '" + Convert.ToDateTime(dt.Rows[0]["JieBangTime"].ToString()) + "'";
                    sql = "select Gps_lat,Gps_lng,Gps_time,Gps_info from GpsLocation where GpsDeviceID = '" + dt.Rows[0]["GpsDeviceID"].ToString() + "' and Gps_time > '" + Convert.ToDateTime(dt.Rows[0]["BangDingTime"].ToString()).AddHours(-1) + "'" + conn;
                    DataTable dt_gps = db.ExecuteDataTable(sql);
                    return dt_gps;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }
    }

    public DataTable GetWheelPathList(string UserID, string UserDenno)
    {
        using (var db = new DBConnection())
        {
            try
            {

                string sql = "select * from YunDan where UserID = @UserID and UserDenno = @UserDenno";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UserDenno", UserDenno);
                DataTable dt = db.ExecuteDataTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    DataTable dt_gps = new DataTable();
                    dt_gps.Columns.Add("Gps_lat");
                    dt_gps.Columns.Add("Gps_lng");
                    dt_gps.Columns.Add("Gps_time");
                    dt_gps.Columns.Add("Gps_info");
                    return dt_gps;
                }
                else
                {
                    string conn = "";
                    if (dt.Rows[0]["IsBangding"].ToString() == "False")
                        conn = " and Gps_time < '" + Convert.ToDateTime(dt.Rows[0]["JieBangTime"].ToString()) + "'";
                    sql = "select Gps_lat,Gps_lng,Gps_time,Gps_info from GpsLocation2 where GpsDeviceID = '" + dt.Rows[0]["GpsDeviceID"].ToString() + "' and Gps_time > '" + Convert.ToDateTime(dt.Rows[0]["BangDingTime"].ToString()).AddHours(-1) + "'" + conn;
                    DataTable dt_gps = db.ExecuteDataTable(sql);
                    return dt_gps;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("GetPoint_file", 2)]
    public byte[] GetPoint_file(string UserID, string YunDanDenno,string startTime,string endTime)
    {
        using (DBConnection dbc = new DBConnection())
        {
            try
            {
                DataTable dt_yun = dbc.ExecuteDataTable("select * from YunDan where UserID = '" + UserID + "' and YunDanDenno = '" + YunDanDenno + "'");
                DataTable dt_gps = new DataTable();
                string conn = "";
                string sql_gps = "";
                if (dt_yun.Rows.Count > 0)
                {
                    DateTime BangDingTime_new = Convert.ToDateTime(dt_yun.Rows[0]["BangDingTime"]).AddHours(-1);
                    if (BangDingTime_new < Convert.ToDateTime(startTime))
                        BangDingTime_new = Convert.ToDateTime(startTime);
                    if (dt_yun.Rows[0]["GpsDeviceID"].ToString().Substring(0,4)=="1919")
                        sql_gps = "select * from GpsLocation where GpsDeviceID = '" + dt_yun.Rows[0]["GpsDeviceID"].ToString() + "' and Gps_time > '" + BangDingTime_new + "'";
                    else if(dt_yun.Rows[0]["GpsDeviceID"].ToString().Substring(0,4)=="8630")
                        sql_gps = "select * from GpsLocation2 where GpsDeviceID = '" + dt_yun.Rows[0]["GpsDeviceID"].ToString() + "' and Gps_time > '" + BangDingTime_new + "'";
                    if (dt_yun.Rows[0]["IsBangding"].ToString() == "0")
                    {
                        DateTime JieBangTime_new = Convert.ToDateTime(dt_yun.Rows[0]["JieBangTime"]);
                        if (JieBangTime_new > Convert.ToDateTime(endTime))
                            JieBangTime_new = Convert.ToDateTime(endTime);
                        conn = " and Gps_time < '" + JieBangTime_new + "'";
                        sql_gps = sql_gps + conn + " order by Gps_time desc";
                    }
                    else
                    {
                        DateTime JieBangTime_new = Convert.ToDateTime(endTime);
                        conn = " and Gps_time < '" + JieBangTime_new + "'";
                        sql_gps = sql_gps + conn + " order by Gps_time desc";
                    }
                    dt_gps = dbc.ExecuteDataTable(sql_gps);
                }

                Workbook workbook = new Workbook(); //工作簿
                Worksheet sheet = workbook.Worksheets[0]; //工作表
                Cells cells = sheet.Cells;//单元格

                //为标题设置样式  
                Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
                styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                styleTitle.Font.Name = "宋体";//文字字体
                styleTitle.Font.Size = 16;//文字大小
                styleTitle.Font.IsBold = true;//粗体

                //样式1
                Style style1 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style1.Font.Name = "宋体";//文字字体
                style1.Font.Size = 12;//文字大小
                style1.Font.IsBold = true;//粗体
                //style1.IsTextWrapped = true;//单元格内容自动换行

                //样式1
                Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style2.Font.Name = "宋体";//文字字体
                style2.Font.Size = 12;//文字大小
               // style2.IsTextWrapped = true;//单元格内容自动换行
                style2.Font.IsBold = true;//粗体
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style3.Font.Name = "宋体";//文字字体
                style3.Font.Size = 12;//文字大小
                style3.IsTextWrapped = true;//单元格内容自动换行
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                int maxnum = 5;
                int currentRow = 0;
                cells.Merge(currentRow, 0, 1, maxnum);
                cells.SetRowHeight(currentRow, 28);
                cells[currentRow, 0].PutValue("运单号：" + dt_yun.Rows[0]["UserDenno"].ToString() + "定位表");
                cells[currentRow, 0].SetStyle(styleTitle);

                currentRow++;

                cells[currentRow, 0].PutValue("序号");
                cells[currentRow, 0].SetStyle(style2);
                cells.SetColumnWidth(0, 8);

                cells[currentRow, 1].PutValue("经度");
                cells[currentRow, 1].SetStyle(style2);
                cells.SetColumnWidth(1, 25);

                cells[currentRow, 2].PutValue("纬度");
                cells[currentRow, 2].SetStyle(style2);
                cells.SetColumnWidth(2, 25);

                cells[currentRow, 3].PutValue("定位时间");
                cells[currentRow, 3].SetStyle(style2);
                cells.SetColumnWidth(3, 25);

                cells[currentRow, 4].PutValue("定位地址");
                cells[currentRow, 4].SetStyle(style2);
                cells.SetColumnWidth(4, 65);

                cells.SetRowHeight(currentRow, 20);

                int xh = 0;
                foreach (DataRow dr in dt_gps.Rows)
                {
                    currentRow++;
                    xh++;
                    cells[currentRow, 0].PutValue(xh);
                    cells[currentRow, 0].SetStyle(style3);

                    cells[currentRow, 1].PutValue(dr["Gps_lng"].ToString());
                    cells[currentRow, 1].SetStyle(style3);
                    cells[currentRow, 1].Style.HorizontalAlignment = TextAlignmentType.Left;


                    cells[currentRow, 2].PutValue(dr["Gps_lat"].ToString());
                    cells[currentRow, 2].SetStyle(style3);
                    cells[currentRow, 2].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 3].PutValue(Convert.ToDateTime(dr["Gps_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[currentRow, 3].SetStyle(style3);
                    cells[currentRow, 3].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 4].PutValue(dr["Gps_info"].ToString());
                    cells[currentRow, 4].SetStyle(style3);
                    cells[currentRow, 4].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells.SetRowHeight(currentRow, 48);
                }

                //sheet.AutoFitColumns();

                System.IO.MemoryStream ms = workbook.SaveToStream();
                byte[] bt = ms.ToArray();
                return bt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    [CSMethod("GetPoint_fileByZT", 2)]
    public byte[] GetPoint_fileByZT(string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string StartTime, string EndTime, string Purchaser, string CarrierCompany)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                string conn = "";

                string QiShiZhan = "";
                string DaoDaZhan = "";

                if (!string.IsNullOrEmpty(QiShiZhan_Province))
                {
                    QiShiZhan += QiShiZhan_Province;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_City))
                {
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
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

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    conn += " and BangDingTime >= @StartTime and BangDingTime <= @EndTime";

                if (!string.IsNullOrEmpty(Purchaser))
                    conn += " and Purchaser like @Purchaser";

                if (!string.IsNullOrEmpty(CarrierCompany))
                    conn += " and CarrierCompany like @CarrierCompany";

                string sql = "select * from YunDan where UserID = @UserID and IsBangding = 1" + conn + " order by BangDingTime desc";
                //                if (isyj == 1)
                //                {
                //                    sql = @"select a.* from YunDan a 
                //                          inner join (
                //	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                //	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                //	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                //                          ) b on a.YunDanDenno = b.YunDanDenno
                //                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                //                }
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");

                DataTable dt = db.ExecuteDataTable(cmd);
                dt.Columns.Add("Expect_ArriveTime");
                dt.Columns.Add("Actual_ArriveTime");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql = "select * from YunDanDistance where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_distance = db.ExecuteDataTable(sql);
                    if (dt_distance.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = dt_distance.Rows[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Expect_Hour"].ToString()))
                    {
                        dt.Rows[i]["Expect_ArriveTime"] = Convert.ToDateTime(dt.Rows[i]["BangDingTime"]).AddHours(Convert.ToDouble(dt.Rows[i]["Expect_Hour"].ToString()));
                    }
                    sql = "select * from YunDanIsArrive where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_arrive = db.ExecuteDataTable(sql);
                    if (dt_arrive.Rows.Count > 0)
                    {
                        dt.Rows[i]["Actual_ArriveTime"] = dt_arrive.Rows[0]["Addtime"];
                    }
                }

                Workbook workbook = new Workbook(); //工作簿
                Worksheet sheet = workbook.Worksheets[0]; //工作表
                Cells cells = sheet.Cells;//单元格

                //为标题设置样式  
                Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
                styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                styleTitle.Font.Name = "宋体";//文字字体
                styleTitle.Font.Size = 16;//文字大小
                styleTitle.Font.IsBold = true;//粗体

                //样式1
                Style style1 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style1.Font.Name = "宋体";//文字字体
                style1.Font.Size = 12;//文字大小
                style1.Font.IsBold = true;//粗体
                //style1.IsTextWrapped = true;//单元格内容自动换行

                //样式1
                Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style2.Font.Name = "宋体";//文字字体
                style2.Font.Size = 12;//文字大小
                // style2.IsTextWrapped = true;//单元格内容自动换行
                style2.Font.IsBold = true;//粗体
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style3.Font.Name = "宋体";//文字字体
                style3.Font.Size = 12;//文字大小
                style3.IsTextWrapped = true;//单元格内容自动换行
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                int maxnum = 21;
                int currentRow = 0;
                cells.Merge(currentRow, 0, 1, maxnum);
                cells.SetRowHeight(currentRow, 28);
                cells[currentRow, 0].PutValue("在途列表");
                cells[currentRow, 0].SetStyle(styleTitle);

                currentRow++;

                cells[currentRow, 0].PutValue("建单号");
                cells[currentRow, 0].SetStyle(style2);
                cells.SetColumnWidth(0, 25);

                cells[currentRow, 1].PutValue("建单公司");
                cells[currentRow, 1].SetStyle(style2);
                cells.SetColumnWidth(1, 25);

                cells[currentRow, 2].PutValue("GPS设备号");
                cells[currentRow, 2].SetStyle(style2);
                cells.SetColumnWidth(2, 25);

                cells[currentRow, 3].PutValue("制单日期");
                cells[currentRow, 3].SetStyle(style2);
                cells.SetColumnWidth(3, 25);

                cells[currentRow, 4].PutValue("收货单位");
                cells[currentRow, 4].SetStyle(style2);
                cells.SetColumnWidth(4, 25);

                cells[currentRow, 5].PutValue("收货方联系方式");
                cells[currentRow, 5].SetStyle(style2);
                cells.SetColumnWidth(5, 25);

                cells[currentRow, 6].PutValue("预计到达时间");
                cells[currentRow, 6].SetStyle(style2);
                cells.SetColumnWidth(6, 25);

                cells[currentRow, 7].PutValue("剩余时间");
                cells[currentRow, 7].SetStyle(style2);
                cells.SetColumnWidth(7, 25);

                cells[currentRow, 8].PutValue("剩余路程");
                cells[currentRow, 8].SetStyle(style2);
                cells.SetColumnWidth(8, 25);

                cells[currentRow, 9].PutValue("出发地");
                cells[currentRow, 9].SetStyle(style2);
                cells.SetColumnWidth(9, 25);

                cells[currentRow, 10].PutValue("实际到达时间");
                cells[currentRow, 10].SetStyle(style2);
                cells.SetColumnWidth(10, 25);

                cells[currentRow, 11].PutValue("目的地");
                cells[currentRow, 11].SetStyle(style2);
                cells.SetColumnWidth(11, 25);

                cells[currentRow, 12].PutValue("当前位置");
                cells[currentRow, 12].SetStyle(style2);
                cells.SetColumnWidth(12, 65);

                cells[currentRow, 13].PutValue("承运公司");
                cells[currentRow, 13].SetStyle(style2);
                cells.SetColumnWidth(13, 25);

                cells[currentRow, 14].PutValue("承运公司联系方式");
                cells[currentRow, 14].SetStyle(style2);
                cells.SetColumnWidth(14, 25);

                cells[currentRow, 15].PutValue("承运专线");
                cells[currentRow, 15].SetStyle(style2);
                cells.SetColumnWidth(15, 25);

                cells[currentRow, 16].PutValue("承运专线联系方式");
                cells[currentRow, 16].SetStyle(style2);
                cells.SetColumnWidth(16, 25);

                cells[currentRow, 17].PutValue("承运专线车辆车牌");
                cells[currentRow, 17].SetStyle(style2);
                cells.SetColumnWidth(17, 25);

                cells[currentRow, 18].PutValue("驾驶员联系方式");
                cells[currentRow, 18].SetStyle(style2);
                cells.SetColumnWidth(18, 25);

                cells[currentRow, 19].PutValue("销售员");
                cells[currentRow, 19].SetStyle(style2);
                cells.SetColumnWidth(19, 25);

                cells[currentRow, 20].PutValue("货物信息");
                cells[currentRow, 20].SetStyle(style2);
                cells.SetColumnWidth(20, 25);

                cells.SetRowHeight(currentRow, 20);

                foreach (DataRow dr in dt.Rows)
                {
                    currentRow++;
                    cells[currentRow, 0].PutValue(dr["UserDenno"].ToString());
                    cells[currentRow, 0].SetStyle(style3);

                    cells[currentRow, 1].PutValue(dr["SuoShuGongSi"].ToString());
                    cells[currentRow, 1].SetStyle(style3);
                    cells[currentRow, 1].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 2].PutValue(dr["GpsDeviceID"].ToString());
                    cells[currentRow, 2].SetStyle(style3);
                    cells[currentRow, 2].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 3].PutValue(Convert.ToDateTime(dr["BangDingTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[currentRow, 3].SetStyle(style3);
                    cells[currentRow, 3].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 4].PutValue(dr["Purchaser"].ToString());
                    cells[currentRow, 4].SetStyle(style3);
                    cells[currentRow, 4].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 5].PutValue(dr["PurchaserTel"].ToString());
                    cells[currentRow, 5].SetStyle(style3);
                    cells[currentRow, 5].Style.HorizontalAlignment = TextAlignmentType.Left;

                    if (!string.IsNullOrEmpty(dr["Expect_ArriveTime"].ToString()))
                    {
                        cells[currentRow, 6].PutValue(Convert.ToDateTime(dr["Expect_ArriveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        cells[currentRow, 6].SetStyle(style3);
                        cells[currentRow, 6].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }
                    else
                    {
                        cells[currentRow, 6].PutValue("");
                        cells[currentRow, 6].SetStyle(style3);
                        cells[currentRow, 6].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }

                    cells[currentRow, 7].PutValue(dr["Gps_duration"].ToString());
                    cells[currentRow, 7].SetStyle(style3);
                    cells[currentRow, 7].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 8].PutValue(dr["Gps_distance"].ToString());
                    cells[currentRow, 8].SetStyle(style3);
                    cells[currentRow, 8].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 9].PutValue(dr["QiShiZhan"].ToString());
                    cells[currentRow, 9].SetStyle(style3);
                    cells[currentRow, 9].Style.HorizontalAlignment = TextAlignmentType.Left;

                    if (!string.IsNullOrEmpty(dr["Actual_ArriveTime"].ToString()))
                    {
                        cells[currentRow, 10].PutValue(Convert.ToDateTime(dr["Actual_ArriveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        cells[currentRow, 10].SetStyle(style3);
                        cells[currentRow, 10].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }
                    else
                    {
                        cells[currentRow, 10].PutValue("");
                        cells[currentRow, 10].SetStyle(style3);
                        cells[currentRow, 10].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }

                    cells[currentRow, 11].PutValue(dr["DaoDaZhan"].ToString());
                    cells[currentRow, 11].SetStyle(style3);
                    cells[currentRow, 11].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 12].PutValue(dr["Gps_lastinfo"].ToString());
                    cells[currentRow, 12].SetStyle(style3);
                    cells[currentRow, 12].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 13].PutValue(dr["CarrierCompany"].ToString());
                    cells[currentRow, 13].SetStyle(style3);
                    cells[currentRow, 13].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 14].PutValue(dr["CarrierTel"].ToString());
                    cells[currentRow, 14].SetStyle(style3);
                    cells[currentRow, 14].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 15].PutValue(dr["SpecialLine"].ToString());
                    cells[currentRow, 15].SetStyle(style3);
                    cells[currentRow, 15].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 16].PutValue(dr["SpecialLinePersonTel"].ToString());
                    cells[currentRow, 16].SetStyle(style3);
                    cells[currentRow, 16].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 17].PutValue(dr["CarNumber"].ToString());
                    cells[currentRow, 17].SetStyle(style3);
                    cells[currentRow, 17].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 18].PutValue(dr["CarPersonTel"].ToString());
                    cells[currentRow, 18].SetStyle(style3);
                    cells[currentRow, 18].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 19].PutValue(dr["SalePerson"].ToString());
                    cells[currentRow, 19].SetStyle(style3);
                    cells[currentRow, 19].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 20].PutValue(dr["YunDanRemark"].ToString());
                    cells[currentRow, 20].SetStyle(style3);
                    cells[currentRow, 20].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells.SetRowHeight(currentRow, 48);
                }

                //sheet.AutoFitColumns();

                System.IO.MemoryStream ms = workbook.SaveToStream();
                byte[] bt = ms.ToArray();
                return bt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    [CSMethod("GetPoint_fileByYJ", 2)]
    public byte[] GetPoint_fileByYJ(string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string StartTime, string EndTime, string Purchaser, string CarrierCompany)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                string conn = "";

                string QiShiZhan = "";
                string DaoDaZhan = "";

                if (!string.IsNullOrEmpty(QiShiZhan_Province))
                {
                    QiShiZhan += QiShiZhan_Province;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_City))
                {
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
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

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    conn += " and BangDingTime >= @StartTime and BangDingTime <= @EndTime";

                if (!string.IsNullOrEmpty(Purchaser))
                    conn += " and Purchaser like @Purchaser";

                if (!string.IsNullOrEmpty(CarrierCompany))
                    conn += " and CarrierCompany like @CarrierCompany";

                string sql = @"select a.* from YunDan a 
                                inner join (
                	                select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                	                inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                	                where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                                ) b on a.YunDanDenno = b.YunDanDenno
                                where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");

                DataTable dt = db.ExecuteDataTable(cmd);
                dt.Columns.Add("Expect_ArriveTime");
                dt.Columns.Add("Actual_ArriveTime");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql = "select * from YunDanDistance where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_distance = db.ExecuteDataTable(sql);
                    if (dt_distance.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = dt_distance.Rows[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Expect_Hour"].ToString()))
                    {
                        dt.Rows[i]["Expect_ArriveTime"] = Convert.ToDateTime(dt.Rows[i]["BangDingTime"]).AddHours(Convert.ToDouble(dt.Rows[i]["Expect_Hour"].ToString()));
                    }
                    sql = "select * from YunDanIsArrive where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_arrive = db.ExecuteDataTable(sql);
                    if (dt_arrive.Rows.Count > 0)
                    {
                        dt.Rows[i]["Actual_ArriveTime"] = dt_arrive.Rows[0]["Addtime"];
                    }
                }

                Workbook workbook = new Workbook(); //工作簿
                Worksheet sheet = workbook.Worksheets[0]; //工作表
                Cells cells = sheet.Cells;//单元格

                //为标题设置样式  
                Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
                styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                styleTitle.Font.Name = "宋体";//文字字体
                styleTitle.Font.Size = 16;//文字大小
                styleTitle.Font.IsBold = true;//粗体

                //样式1
                Style style1 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style1.Font.Name = "宋体";//文字字体
                style1.Font.Size = 12;//文字大小
                style1.Font.IsBold = true;//粗体
                //style1.IsTextWrapped = true;//单元格内容自动换行

                //样式1
                Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style2.Font.Name = "宋体";//文字字体
                style2.Font.Size = 12;//文字大小
                // style2.IsTextWrapped = true;//单元格内容自动换行
                style2.Font.IsBold = true;//粗体
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style3.Font.Name = "宋体";//文字字体
                style3.Font.Size = 12;//文字大小
                style3.IsTextWrapped = true;//单元格内容自动换行
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                int maxnum = 21;
                int currentRow = 0;
                cells.Merge(currentRow, 0, 1, maxnum);
                cells.SetRowHeight(currentRow, 28);
                cells[currentRow, 0].PutValue("预警列表");
                cells[currentRow, 0].SetStyle(styleTitle);

                currentRow++;

                cells[currentRow, 0].PutValue("建单号");
                cells[currentRow, 0].SetStyle(style2);
                cells.SetColumnWidth(0, 25);

                cells[currentRow, 1].PutValue("建单公司");
                cells[currentRow, 1].SetStyle(style2);
                cells.SetColumnWidth(1, 25);

                cells[currentRow, 2].PutValue("GPS设备号");
                cells[currentRow, 2].SetStyle(style2);
                cells.SetColumnWidth(2, 25);

                cells[currentRow, 3].PutValue("制单日期");
                cells[currentRow, 3].SetStyle(style2);
                cells.SetColumnWidth(3, 25);

                cells[currentRow, 4].PutValue("收货单位");
                cells[currentRow, 4].SetStyle(style2);
                cells.SetColumnWidth(4, 25);

                cells[currentRow, 5].PutValue("收货方联系方式");
                cells[currentRow, 5].SetStyle(style2);
                cells.SetColumnWidth(5, 25);

                cells[currentRow, 6].PutValue("预计到达时间");
                cells[currentRow, 6].SetStyle(style2);
                cells.SetColumnWidth(6, 25);

                cells[currentRow, 7].PutValue("剩余时间");
                cells[currentRow, 7].SetStyle(style2);
                cells.SetColumnWidth(7, 25);

                cells[currentRow, 8].PutValue("剩余路程");
                cells[currentRow, 8].SetStyle(style2);
                cells.SetColumnWidth(8, 25);

                cells[currentRow, 9].PutValue("出发地");
                cells[currentRow, 9].SetStyle(style2);
                cells.SetColumnWidth(9, 25);

                cells[currentRow, 10].PutValue("实际到达时间");
                cells[currentRow, 10].SetStyle(style2);
                cells.SetColumnWidth(10, 25);

                cells[currentRow, 11].PutValue("目的地");
                cells[currentRow, 11].SetStyle(style2);
                cells.SetColumnWidth(11, 25);

                cells[currentRow, 12].PutValue("当前位置");
                cells[currentRow, 12].SetStyle(style2);
                cells.SetColumnWidth(12, 65);

                cells[currentRow, 13].PutValue("承运公司");
                cells[currentRow, 13].SetStyle(style2);
                cells.SetColumnWidth(13, 25);

                cells[currentRow, 14].PutValue("承运公司联系方式");
                cells[currentRow, 14].SetStyle(style2);
                cells.SetColumnWidth(14, 25);

                cells[currentRow, 15].PutValue("承运专线");
                cells[currentRow, 15].SetStyle(style2);
                cells.SetColumnWidth(15, 25);

                cells[currentRow, 16].PutValue("承运专线联系方式");
                cells[currentRow, 16].SetStyle(style2);
                cells.SetColumnWidth(16, 25);

                cells[currentRow, 17].PutValue("承运专线车辆车牌");
                cells[currentRow, 17].SetStyle(style2);
                cells.SetColumnWidth(17, 25);

                cells[currentRow, 18].PutValue("驾驶员联系方式");
                cells[currentRow, 18].SetStyle(style2);
                cells.SetColumnWidth(18, 25);

                cells[currentRow, 19].PutValue("销售员");
                cells[currentRow, 19].SetStyle(style2);
                cells.SetColumnWidth(19, 25);

                cells[currentRow, 20].PutValue("货物信息");
                cells[currentRow, 20].SetStyle(style2);
                cells.SetColumnWidth(20, 25);

                cells.SetRowHeight(currentRow, 20);

                foreach (DataRow dr in dt.Rows)
                {
                    currentRow++;
                    cells[currentRow, 0].PutValue(dr["UserDenno"].ToString());
                    cells[currentRow, 0].SetStyle(style3);

                    cells[currentRow, 1].PutValue(dr["SuoShuGongSi"].ToString());
                    cells[currentRow, 1].SetStyle(style3);
                    cells[currentRow, 1].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 2].PutValue(dr["GpsDeviceID"].ToString());
                    cells[currentRow, 2].SetStyle(style3);
                    cells[currentRow, 2].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 3].PutValue(Convert.ToDateTime(dr["BangDingTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[currentRow, 3].SetStyle(style3);
                    cells[currentRow, 3].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 4].PutValue(dr["Purchaser"].ToString());
                    cells[currentRow, 4].SetStyle(style3);
                    cells[currentRow, 4].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 5].PutValue(dr["PurchaserTel"].ToString());
                    cells[currentRow, 5].SetStyle(style3);
                    cells[currentRow, 5].Style.HorizontalAlignment = TextAlignmentType.Left;

                    if (!string.IsNullOrEmpty(dr["Expect_ArriveTime"].ToString()))
                    {
                        cells[currentRow, 6].PutValue(Convert.ToDateTime(dr["Expect_ArriveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        cells[currentRow, 6].SetStyle(style3);
                        cells[currentRow, 6].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }
                    else
                    {
                        cells[currentRow, 6].PutValue("");
                        cells[currentRow, 6].SetStyle(style3);
                        cells[currentRow, 6].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }

                    cells[currentRow, 7].PutValue(dr["Gps_duration"].ToString());
                    cells[currentRow, 7].SetStyle(style3);
                    cells[currentRow, 7].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 8].PutValue(dr["Gps_distance"].ToString());
                    cells[currentRow, 8].SetStyle(style3);
                    cells[currentRow, 8].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 9].PutValue(dr["QiShiZhan"].ToString());
                    cells[currentRow, 9].SetStyle(style3);
                    cells[currentRow, 9].Style.HorizontalAlignment = TextAlignmentType.Left;

                    if (!string.IsNullOrEmpty(dr["Actual_ArriveTime"].ToString()))
                    {
                        cells[currentRow, 10].PutValue(Convert.ToDateTime(dr["Actual_ArriveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        cells[currentRow, 10].SetStyle(style3);
                        cells[currentRow, 10].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }
                    else
                    {
                        cells[currentRow, 10].PutValue("");
                        cells[currentRow, 10].SetStyle(style3);
                        cells[currentRow, 10].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }

                    cells[currentRow, 11].PutValue(dr["DaoDaZhan"].ToString());
                    cells[currentRow, 11].SetStyle(style3);
                    cells[currentRow, 11].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 12].PutValue(dr["Gps_lastinfo"].ToString());
                    cells[currentRow, 12].SetStyle(style3);
                    cells[currentRow, 12].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 13].PutValue(dr["CarrierCompany"].ToString());
                    cells[currentRow, 13].SetStyle(style3);
                    cells[currentRow, 13].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 14].PutValue(dr["CarrierTel"].ToString());
                    cells[currentRow, 14].SetStyle(style3);
                    cells[currentRow, 14].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 15].PutValue(dr["SpecialLine"].ToString());
                    cells[currentRow, 15].SetStyle(style3);
                    cells[currentRow, 15].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 16].PutValue(dr["SpecialLinePersonTel"].ToString());
                    cells[currentRow, 16].SetStyle(style3);
                    cells[currentRow, 16].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 17].PutValue(dr["CarNumber"].ToString());
                    cells[currentRow, 17].SetStyle(style3);
                    cells[currentRow, 17].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 18].PutValue(dr["CarPersonTel"].ToString());
                    cells[currentRow, 18].SetStyle(style3);
                    cells[currentRow, 18].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 19].PutValue(dr["SalePerson"].ToString());
                    cells[currentRow, 19].SetStyle(style3);
                    cells[currentRow, 19].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 20].PutValue(dr["YunDanRemark"].ToString());
                    cells[currentRow, 20].SetStyle(style3);
                    cells[currentRow, 20].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells.SetRowHeight(currentRow, 48);
                }

                //sheet.AutoFitColumns();

                System.IO.MemoryStream ms = workbook.SaveToStream();
                byte[] bt = ms.ToArray();
                return bt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    [CSMethod("GetPoint_fileByLS", 2)]
    public byte[] GetPoint_fileByLS(string QiShiZhan_Province, string QiShiZhan_City, string QiShiZhan_Qx, string DaoDaZhan_Province, string DaoDaZhan_City, string DaoDaZhan_Qx, string SuoShuGongSi, string GpsDeviceID, string UserDenno, string StartTime, string EndTime, string Purchaser, string CarrierCompany)
    {
        using (DBConnection db = new DBConnection())
        {
            try
            {
                string conn = "";

                string QiShiZhan = "";
                string DaoDaZhan = "";

                if (!string.IsNullOrEmpty(QiShiZhan_Province))
                {
                    QiShiZhan += QiShiZhan_Province;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_City))
                {
                    if (QiShiZhan_Province != QiShiZhan_City)
                        QiShiZhan += " " + QiShiZhan_City;
                }
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                {
                    conn += " and QiShiZhan_QX like @QiShiZhan_QX";
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
                    if (DaoDaZhan_Province != DaoDaZhan_City)
                        DaoDaZhan += " " + DaoDaZhan_City;
                }

                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                {
                    conn += " and DaoDaZhan_QX like @DaoDaZhan_QX";
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

                if (!string.IsNullOrEmpty(GpsDeviceID))
                    conn += " and GpsDeviceID = @GpsDeviceID";

                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    conn += " and BangDingTime >= @StartTime and BangDingTime <= @EndTime";

                if (!string.IsNullOrEmpty(Purchaser))
                    conn += " and Purchaser like @Purchaser";

                if (!string.IsNullOrEmpty(CarrierCompany))
                    conn += " and CarrierCompany like @CarrierCompany";

                string sql = "select * from YunDan where UserID = @UserID and IsBangding = 0" + conn + " order by BangDingTime desc";
                //                if (isyj == 1)
                //                {
                //                    sql = @"select a.* from YunDan a 
                //                          inner join (
                //	                          select DATEDIFF(mi,dateadd(SS,duration,getdate()),dateadd(HH,a.Expect_Hour,a.BangDingTime)) TimeCZ,a.YunDanDenno from YunDan a
                //	                          inner join (select *,cast(Gps_duration as decimal) duration from YunDanDistance where Gps_duration is not null) b on a.YunDanDenno = b.YunDanDenno
                //	                          where a.Expect_Hour is not null and a.UserID = @UserID and a.IsBangding = 1 
                //                          ) b on a.YunDanDenno = b.YunDanDenno
                //                          where a.UserID = @UserID and a.IsBangding = 1 and TimeCZ < 0" + conn + " and a.YunDanDenno not in (select YunDanDenno from YunDanIsArrive) order by BangDingTime desc";
                //                }
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@UserID", SystemUser.CurrentUser.UserID);
                if (!string.IsNullOrEmpty(UserDenno))
                    cmd.Parameters.AddWithValue("@UserDenno", "%" + UserDenno.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(GpsDeviceID))
                    cmd.Parameters.AddWithValue("@GpsDeviceID", GpsDeviceID.Replace(" ", ""));
                if (!string.IsNullOrEmpty(QiShiZhan))
                    cmd.Parameters.AddWithValue("@QiShiZhan", "%" + QiShiZhan + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan))
                    cmd.Parameters.AddWithValue("@DaoDaZhan", "%" + DaoDaZhan + "%");
                if (!string.IsNullOrEmpty(SuoShuGongSi))
                    cmd.Parameters.AddWithValue("@SuoShuGongSi", "%" + SuoShuGongSi.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(QiShiZhan_Qx))
                    cmd.Parameters.AddWithValue("@QiShiZhan_QX", "%" + QiShiZhan_Qx + "%");
                if (!string.IsNullOrEmpty(DaoDaZhan_Qx))
                    cmd.Parameters.AddWithValue("@DaoDaZhan_QX", "%" + DaoDaZhan_Qx + "%");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                    cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                }
                if (!string.IsNullOrEmpty(Purchaser))
                    cmd.Parameters.AddWithValue("@Purchaser", "%" + Purchaser.Replace(" ", "") + "%");
                if (!string.IsNullOrEmpty(CarrierCompany))
                    cmd.Parameters.AddWithValue("@CarrierCompany", "%" + CarrierCompany.Replace(" ", "") + "%");

                DataTable dt = db.ExecuteDataTable(cmd);
                dt.Columns.Add("Expect_ArriveTime");
                dt.Columns.Add("Actual_ArriveTime");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql = "select * from YunDanDistance where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_distance = db.ExecuteDataTable(sql);
                    if (dt_distance.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_distance"].ToString()) && !string.IsNullOrEmpty(dt_distance.Rows[0]["Gps_duration"].ToString()))
                        {
                            dt.Rows[i]["Gps_distance"] = dt_distance.Rows[0]["Gps_distance"].ToString() + "公里";

                            int hour = 0;
                            int minute = 0;
                            if (Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60) == 0)
                                dt.Rows[i]["Gps_duration"] = Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()).ToString("F0") + "分钟";
                            else
                            {
                                hour = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) / 60);
                                minute = Convert.ToInt32(Convert.ToDouble(dt_distance.Rows[0]["Gps_duration"].ToString()) % 60);
                                dt.Rows[i]["Gps_duration"] = hour + "小时" + minute + "分钟";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Expect_Hour"].ToString()))
                    {
                        dt.Rows[i]["Expect_ArriveTime"] = Convert.ToDateTime(dt.Rows[i]["BangDingTime"]).AddHours(Convert.ToDouble(dt.Rows[i]["Expect_Hour"].ToString()));
                    }
                    sql = "select * from YunDanIsArrive where YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'";
                    DataTable dt_arrive = db.ExecuteDataTable(sql);
                    if (dt_arrive.Rows.Count > 0)
                    {
                        dt.Rows[i]["Actual_ArriveTime"] = dt_arrive.Rows[0]["Addtime"];
                    }
                }

                Workbook workbook = new Workbook(); //工作簿
                Worksheet sheet = workbook.Worksheets[0]; //工作表
                Cells cells = sheet.Cells;//单元格

                //为标题设置样式  
                Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
                styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                styleTitle.Font.Name = "宋体";//文字字体
                styleTitle.Font.Size = 16;//文字大小
                styleTitle.Font.IsBold = true;//粗体

                //样式1
                Style style1 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style1.Font.Name = "宋体";//文字字体
                style1.Font.Size = 12;//文字大小
                style1.Font.IsBold = true;//粗体
                //style1.IsTextWrapped = true;//单元格内容自动换行

                //样式1
                Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style2.Font.Name = "宋体";//文字字体
                style2.Font.Size = 12;//文字大小
                // style2.IsTextWrapped = true;//单元格内容自动换行
                style2.Font.IsBold = true;//粗体
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                style3.Font.Name = "宋体";//文字字体
                style3.Font.Size = 12;//文字大小
                style3.IsTextWrapped = true;//单元格内容自动换行
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                int maxnum = 21;
                int currentRow = 0;
                cells.Merge(currentRow, 0, 1, maxnum);
                cells.SetRowHeight(currentRow, 28);
                cells[currentRow, 0].PutValue("历史列表");
                cells[currentRow, 0].SetStyle(styleTitle);

                currentRow++;

                cells[currentRow, 0].PutValue("建单号");
                cells[currentRow, 0].SetStyle(style2);
                cells.SetColumnWidth(0, 25);

                cells[currentRow, 1].PutValue("建单公司");
                cells[currentRow, 1].SetStyle(style2);
                cells.SetColumnWidth(1, 25);

                cells[currentRow, 2].PutValue("GPS设备号");
                cells[currentRow, 2].SetStyle(style2);
                cells.SetColumnWidth(2, 25);

                cells[currentRow, 3].PutValue("制单日期");
                cells[currentRow, 3].SetStyle(style2);
                cells.SetColumnWidth(3, 25);

                cells[currentRow, 4].PutValue("收货单位");
                cells[currentRow, 4].SetStyle(style2);
                cells.SetColumnWidth(4, 25);

                cells[currentRow, 5].PutValue("收货方联系方式");
                cells[currentRow, 5].SetStyle(style2);
                cells.SetColumnWidth(5, 25);

                cells[currentRow, 6].PutValue("预计到达时间");
                cells[currentRow, 6].SetStyle(style2);
                cells.SetColumnWidth(6, 25);

                cells[currentRow, 7].PutValue("剩余时间");
                cells[currentRow, 7].SetStyle(style2);
                cells.SetColumnWidth(7, 25);

                cells[currentRow, 8].PutValue("剩余路程");
                cells[currentRow, 8].SetStyle(style2);
                cells.SetColumnWidth(8, 25);

                cells[currentRow, 9].PutValue("出发地");
                cells[currentRow, 9].SetStyle(style2);
                cells.SetColumnWidth(9, 25);

                cells[currentRow, 10].PutValue("实际到达时间");
                cells[currentRow, 10].SetStyle(style2);
                cells.SetColumnWidth(10, 25);

                cells[currentRow, 11].PutValue("目的地");
                cells[currentRow, 11].SetStyle(style2);
                cells.SetColumnWidth(11, 25);

                cells[currentRow, 12].PutValue("当前位置");
                cells[currentRow, 12].SetStyle(style2);
                cells.SetColumnWidth(12, 65);

                cells[currentRow, 13].PutValue("承运公司");
                cells[currentRow, 13].SetStyle(style2);
                cells.SetColumnWidth(13, 25);

                cells[currentRow, 14].PutValue("承运公司联系方式");
                cells[currentRow, 14].SetStyle(style2);
                cells.SetColumnWidth(14, 25);

                cells[currentRow, 15].PutValue("承运专线");
                cells[currentRow, 15].SetStyle(style2);
                cells.SetColumnWidth(15, 25);

                cells[currentRow, 16].PutValue("承运专线联系方式");
                cells[currentRow, 16].SetStyle(style2);
                cells.SetColumnWidth(16, 25);

                cells[currentRow, 17].PutValue("承运专线车辆车牌");
                cells[currentRow, 17].SetStyle(style2);
                cells.SetColumnWidth(17, 25);

                cells[currentRow, 18].PutValue("驾驶员联系方式");
                cells[currentRow, 18].SetStyle(style2);
                cells.SetColumnWidth(18, 25);

                cells[currentRow, 19].PutValue("销售员");
                cells[currentRow, 19].SetStyle(style2);
                cells.SetColumnWidth(19, 25);

                cells[currentRow, 20].PutValue("货物信息");
                cells[currentRow, 20].SetStyle(style2);
                cells.SetColumnWidth(20, 25);

                cells.SetRowHeight(currentRow, 20);

                foreach (DataRow dr in dt.Rows)
                {
                    currentRow++;
                    cells[currentRow, 0].PutValue(dr["UserDenno"].ToString());
                    cells[currentRow, 0].SetStyle(style3);

                    cells[currentRow, 1].PutValue(dr["SuoShuGongSi"].ToString());
                    cells[currentRow, 1].SetStyle(style3);
                    cells[currentRow, 1].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 2].PutValue(dr["GpsDeviceID"].ToString());
                    cells[currentRow, 2].SetStyle(style3);
                    cells[currentRow, 2].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 3].PutValue(Convert.ToDateTime(dr["BangDingTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[currentRow, 3].SetStyle(style3);
                    cells[currentRow, 3].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 4].PutValue(dr["Purchaser"].ToString());
                    cells[currentRow, 4].SetStyle(style3);
                    cells[currentRow, 4].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 5].PutValue(dr["PurchaserTel"].ToString());
                    cells[currentRow, 5].SetStyle(style3);
                    cells[currentRow, 5].Style.HorizontalAlignment = TextAlignmentType.Left;

                    if (!string.IsNullOrEmpty(dr["Expect_ArriveTime"].ToString()))
                    {
                        cells[currentRow, 6].PutValue(Convert.ToDateTime(dr["Expect_ArriveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        cells[currentRow, 6].SetStyle(style3);
                        cells[currentRow, 6].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }
                    else
                    {
                        cells[currentRow, 6].PutValue("");
                        cells[currentRow, 6].SetStyle(style3);
                        cells[currentRow, 6].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }

                    cells[currentRow, 7].PutValue(dr["Gps_duration"].ToString());
                    cells[currentRow, 7].SetStyle(style3);
                    cells[currentRow, 7].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 8].PutValue(dr["Gps_distance"].ToString());
                    cells[currentRow, 8].SetStyle(style3);
                    cells[currentRow, 8].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 9].PutValue(dr["QiShiZhan"].ToString());
                    cells[currentRow, 9].SetStyle(style3);
                    cells[currentRow, 9].Style.HorizontalAlignment = TextAlignmentType.Left;

                    if (!string.IsNullOrEmpty(dr["Actual_ArriveTime"].ToString()))
                    {
                        cells[currentRow, 10].PutValue(Convert.ToDateTime(dr["Actual_ArriveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        cells[currentRow, 10].SetStyle(style3);
                        cells[currentRow, 10].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }
                    else
                    {
                        cells[currentRow, 10].PutValue("");
                        cells[currentRow, 10].SetStyle(style3);
                        cells[currentRow, 10].Style.HorizontalAlignment = TextAlignmentType.Left;
                    }

                    cells[currentRow, 11].PutValue(dr["DaoDaZhan"].ToString());
                    cells[currentRow, 11].SetStyle(style3);
                    cells[currentRow, 11].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 12].PutValue(dr["Gps_lastinfo"].ToString());
                    cells[currentRow, 12].SetStyle(style3);
                    cells[currentRow, 12].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 13].PutValue(dr["CarrierCompany"].ToString());
                    cells[currentRow, 13].SetStyle(style3);
                    cells[currentRow, 13].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 14].PutValue(dr["CarrierTel"].ToString());
                    cells[currentRow, 14].SetStyle(style3);
                    cells[currentRow, 14].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 15].PutValue(dr["SpecialLine"].ToString());
                    cells[currentRow, 15].SetStyle(style3);
                    cells[currentRow, 15].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 16].PutValue(dr["SpecialLinePersonTel"].ToString());
                    cells[currentRow, 16].SetStyle(style3);
                    cells[currentRow, 16].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 17].PutValue(dr["CarNumber"].ToString());
                    cells[currentRow, 17].SetStyle(style3);
                    cells[currentRow, 17].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 18].PutValue(dr["CarPersonTel"].ToString());
                    cells[currentRow, 18].SetStyle(style3);
                    cells[currentRow, 18].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 19].PutValue(dr["SalePerson"].ToString());
                    cells[currentRow, 19].SetStyle(style3);
                    cells[currentRow, 19].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells[currentRow, 20].PutValue(dr["YunDanRemark"].ToString());
                    cells[currentRow, 20].SetStyle(style3);
                    cells[currentRow, 20].Style.HorizontalAlignment = TextAlignmentType.Left;

                    cells.SetRowHeight(currentRow, 48);
                }

                //sheet.AutoFitColumns();

                System.IO.MemoryStream ms = workbook.SaveToStream();
                byte[] bt = ms.ToArray();
                return bt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    [CSMethod("ChangeSetModel")]
    public bool ChangeSetModel(bool isSet)
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;
            string sql = "delete from DingDanSetModel where UserID = @UserID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            db.ExecuteNonQuery(cmd);

            if (isSet)
            {
                sql = "insert into DingDanSetModel values(@DingDanSetModelID,@UserID)";
                cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@DingDanSetModelID",Guid.NewGuid());
                cmd.Parameters.AddWithValue("@UserID", userid);
                db.ExecuteNonQuery(cmd);
            }
            return true;
        }
    }

    [CSMethod("GetSetModel")]
    public object GetSetModel()
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;

            string sql = "select * from DingDanSetModel where UserID = @UserID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt_model = db.ExecuteDataTable(cmd);

            sql = @"select a.* from (
                        select '' as DingDanSetListID,SetStoreMc as DingDanSetListMC,'' as DingDanSetListBS,SetStorePx as DingDanSetListPX from DingDanSetStore
                        union all
                        select DingDanSetListID,DingDanSetListMC,DingDanSetListBS,DingDanSetListPX from DingDanSetList where DingDanSetListLX = 0 and UserID = @UserID
                    ) a order by a.DingDanSetListPX";
            cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt_btstore = db.ExecuteDataTable(cmd);

            sql = @"select DingDanSetListID,DingDanSetListMC,DingDanSetListBS,DingDanSetListPX from DingDanSetList where DingDanSetListLX = 1 and UserID = @UserID order by DingDanSetListPX";
            cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt_xtstore = db.ExecuteDataTable(cmd);

            return new { modelnum = dt_model.Rows.Count, dt_btstore = dt_btstore, dt_xtstore = dt_xtstore };
        }
    }

    [CSMethod("GetSelection")]
    public DataTable GetSelection()
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;
            string sql = "select * from DingDanSetSelection where DingDanSetSelectionBS not in (select DingDanSetListBS from DingDanSetList where UserID = @UserID and DingDanSetListBS is not null) order by DingDanSetSelectionPX";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt_selection = db.ExecuteDataTable(cmd);
            return dt_selection;
        }
    }

    [CSMethod("InsertModel")]
    public bool InsertModel(string DingDanSetSelectionID, int lb)
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;

            string sql = "select * from DingDanSetSelection where DingDanSetSelectionID = @DingDanSetSelectionID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@DingDanSetSelectionID", DingDanSetSelectionID);
            DataTable dt_selection = db.ExecuteDataTable(cmd);

            sql = "select max(cast(DingDanSetListPX as int)) DingDanSetListPX from DingDanSetList where UserID = @UserID";
            cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            string px = db.ExecuteScalar(cmd).ToString();

            if (string.IsNullOrEmpty(px))
            {
                DataTable dt = db.GetEmptyDataTable("DingDanSetList");
                DataRow dr = dt.NewRow();
                dr["DingDanSetListID"] = Guid.NewGuid();
                dr["DingDanSetListMC"] = dt_selection.Rows[0]["DingDanSetSelectionMC"];
                dr["DingDanSetListBS"] = dt_selection.Rows[0]["DingDanSetSelectionBS"];
                dr["DingDanSetListPX"] = 6;
                dr["UserID"] = userid;
                dr["DingDanSetListLX"] = lb;
                dt.Rows.Add(dr);
                db.InsertTable(dt);
            }
            else
            {
                DataTable dt = db.GetEmptyDataTable("DingDanSetList");
                DataRow dr = dt.NewRow();
                dr["DingDanSetListID"] = Guid.NewGuid();
                dr["DingDanSetListMC"] = dt_selection.Rows[0]["DingDanSetSelectionMC"];
                dr["DingDanSetListBS"] = dt_selection.Rows[0]["DingDanSetSelectionBS"];
                dr["DingDanSetListPX"] = Convert.ToInt32(px) + 1;
                dr["UserID"] = userid;
                dr["DingDanSetListLX"] = lb;
                dt.Rows.Add(dr);
                db.InsertTable(dt); 
            }

            return true;
        }
    }

    [CSMethod("InsertSelection")]
    public bool InsertSelection(string DingDanSetListMC, int lb)
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;
            string sql = "select max(cast(DingDanSetListPX as int)) DingDanSetListPX from DingDanSetList where UserID = @UserID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            string px = db.ExecuteScalar(cmd).ToString();

            DataTable dt = db.GetEmptyDataTable("DingDanSetList");
            DataRow dr = dt.NewRow();
            dr["DingDanSetListID"] = Guid.NewGuid();
            dr["DingDanSetListMC"] = DingDanSetListMC;
            dr["DingDanSetListBS"] = DBNull.Value;
            dr["DingDanSetListPX"] = string.IsNullOrEmpty(px) ? 6 : Convert.ToInt32(px) + 1;
            dr["UserID"] = userid;
            dr["DingDanSetListLX"] = lb;
            dt.Rows.Add(dr);
            db.InsertTable(dt);

            return true;
        }
    }

    [CSMethod("DeleteDingDanSet")]
    public bool DeleteDingDanSet(string DingDanSetListID)
    {
        using (var db = new DBConnection())
        {
            string sql = "delete from DingDanSetList where DingDanSetListID = @DingDanSetListID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@DingDanSetListID", DingDanSetListID);
            db.ExecuteNonQuery(cmd);
            return true;
        }
    }

    [CSMethod("SelectModelByUser")]
    public bool SelectModelByUser()
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;
            string sql = "select * from DingDanSetModel where UserID = @UserID";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt = db.ExecuteDataTable(cmd);

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }

    [CSMethod("GetSelectionModelByUser")]
    public DataTable GetSelectionModelByUser()
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;
            string sql = "select * from DingDanSetList where UserID = @UserID order by DingDanSetListPX";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt = db.ExecuteDataTable(cmd);
            return dt;
        }
    }

    [CSMethod("GetSelectionByUserBT")]
    public DataTable GetSelectionByUserBT()
    {
        using (var db = new DBConnection())
        {
            string userid = SystemUser.CurrentUser.UserID;
            string sql = "select * from DingDanSetList where UserID = @UserID and DingDanSetListLX = 0 order by DingDanSetListPX";
            SqlCommand cmd = db.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt = db.ExecuteDataTable(cmd);
            return dt;
        }
    }

    [CSMethod("GJMap")]
    public DataTable GJMap(string UserID, string YunDanDenno,string st,string et)
    {
        using (var db = new DBConnection())
        {
            //UserID = "a96f579e-0ae1-4d32-b56a-ecdb794f08de";
            //YunDanDenno = "20180502185448487";
            string sql_yun = "select GpsDeviceID,IsBangding,BangDingTime,JieBangTime from YunDan where YunDanDenno = @YunDanDenno and UserID = @UserID";
            SqlCommand cmd = db.CreateCommand(sql_yun);
            cmd.Parameters.AddWithValue("@YunDanDenno", YunDanDenno);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataTable dt_yun = db.ExecuteDataTable(cmd);

            string conn = "";
            conn += " and Gps_time <= @JieBangTime";

            string sql = "select Gps_lng,Gps_lat from GpsLocation2 where GpsDeviceID = @GpsDeviceID and Gps_time >= @BangDingTime " + conn + " order by GpsLocationID";
            SqlCommand cmd2 = db.CreateCommand(sql);
            cmd2.Parameters.AddWithValue("@GpsDeviceID", dt_yun.Rows[0]["GpsDeviceID"].ToString());
            if (Convert.ToDateTime(dt_yun.Rows[0]["BangDingTime"].ToString()) > Convert.ToDateTime(st))
                cmd2.Parameters.AddWithValue("@BangDingTime", Convert.ToDateTime(dt_yun.Rows[0]["BangDingTime"].ToString()));
            else
                cmd2.Parameters.AddWithValue("@BangDingTime", Convert.ToDateTime(st));
            if (dt_yun.Rows[0]["IsBangding"].ToString() == "False")
            {
                if (Convert.ToDateTime(dt_yun.Rows[0]["JieBangTime"].ToString()) > Convert.ToDateTime(et))
                    cmd2.Parameters.AddWithValue("@JieBangTime", Convert.ToDateTime(et));
                else
                    cmd2.Parameters.AddWithValue("@JieBangTime", Convert.ToDateTime(dt_yun.Rows[0]["JieBangTime"].ToString()));
            }
            else
            {
                cmd2.Parameters.AddWithValue("@JieBangTime", Convert.ToDateTime(et));
            }
            DataTable dt_location = db.ExecuteDataTable(cmd2);
            return dt_location;
        }
    }

    [CSMethod("EditYunDan")]
    public object EditYunDan(string YunDanDenno)
    {
        using (var dbc = new DBConnection())
        {
            string sql = "select * from YunDan where YunDanDenno = @YunDanDenno";
            SqlCommand cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@YunDanDenno", YunDanDenno);
            DataTable dt = dbc.ExecuteDataTable(cmd);

            sql = "select * from YunDanDetails where YunDanDenno = @YunDanDenno";
            cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@YunDanDenno", YunDanDenno);
            DataTable dt_detail = dbc.ExecuteDataTable(cmd);

            return new { dt = dt, dt_detail = dt_detail };
        }
    }

    [CSMethod("EditYunDanNew")]
    public object EditYunDanNew(string YunDanDenno)
    {
        using (var dbc = new DBConnection())
        {
            string sql = "select * from YunDan where YunDanDenno = @YunDanDenno";
            SqlCommand cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@YunDanDenno", YunDanDenno);
            DataTable dt = dbc.ExecuteDataTable(cmd);

            sql = "select * from YunDanDetails where YunDanDenno = @YunDanDenno";
            cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@YunDanDenno", YunDanDenno);
            DataTable dt_detail = dbc.ExecuteDataTable(cmd);

            string userid = SystemUser.CurrentUser.UserID;
            sql = "select * from DingDanSetList where UserID = @UserID and DingDanSetListBS is not null order by DingDanSetListPX";
            cmd = dbc.CreateCommand(sql);
            cmd.Parameters.AddWithValue("@UserID", userid);
            DataTable dt_field = dbc.ExecuteDataTable(cmd);

            sql = "select * from YunDanField where YunDanDenno = @YunDanDenno";
            cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@YunDanDenno", YunDanDenno);
            DataTable dt_field_valule = dbc.ExecuteDataTable(cmd);
            return new { dt = dt, dt_detail = dt_detail, dt_field = dt_field, dt_field_valule = dt_field_valule };
        }
    }

    [CSMethod("XiaoFeiJiLu")]
    public DataTable XiaoFeiJiLu(string UserName)
    {
        using (var dbc = new DBConnection())
        {
            string sql = "select UserID from [dbo].[User] where UserName = @UserName and UserLeiXing = 'APP'";
            SqlCommand cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@UserName", UserName);
            DataTable dt_user = dbc.ExecuteDataTable(cmd);

            sql = "select * from ChongZhi where UserID = @UserID and ZhiFuZhuangTai = 1 order by ChongZhiTime desc";
            cmd = dbc.CreateCommand(sql);
            cmd.Parameters.Add("@UserID", dt_user.Rows[0]["UserID"].ToString());
            DataTable dt_xfjl = dbc.ExecuteDataTable(cmd);
            dt_xfjl.Columns.Add("ISKP");

            sql = "select b.ChongZhiID from InvoiceModel a left join InvoiceMxModel b on a.InvoiceId = b.InvoiceId where a.IsOut = 1";
            DataTable dt_invoice = dbc.ExecuteDataTable(sql);

            for (var i = 0; i < dt_xfjl.Rows.Count; i++)
            { 
                DataRow[] drs = dt_invoice.Select("ChongZhiID = '"+dt_xfjl.Rows[i]["ChongZhiID"].ToString()+"'");
                if(drs.Length>0)
                    dt_xfjl.Rows[i]["ISKP"] = "已审";
                else
                    dt_xfjl.Rows[i]["ISKP"] = "未审";
            }
            return dt_xfjl;
        }
    }

    public System.Collections.Hashtable Gethttpresult(string url, string data)
    {
        WebRequest request = WebRequest.Create(url);
        Encoding encode = Encoding.GetEncoding("utf-8");
        request.Method = "POST";
        Byte[] byteArray = encode.GetBytes(data);
        request.ContentType = "application/x-www-form-urlencoded";

        request.ContentLength = byteArray.Length;
        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        WebResponse response = request.GetResponse();

        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream, encode);
        String responseFromServer = reader.ReadToEnd();
        string outStr = responseFromServer;
        reader.Close();
        dataStream.Close();
        response.Close();

        Hashtable hashTable = JsonConvert.DeserializeObject<Hashtable>(outStr);
        return hashTable;
    }

    public Hashtable getmapinfobyaddress(string address, string city)
    {
        Hashtable hashTable = new Hashtable();

        try
        {
            string url = null;
            url = "http://restapi.amap.com/v3/geocode/geo?key=eeaa068dfa76612008db1232f98ae753&address=" + System.Web.HttpUtility.UrlEncode(address) + "&city=" + System.Web.HttpUtility.UrlEncode(city) + "";

            WebRequest request = WebRequest.Create(url);
            Encoding encode = Encoding.GetEncoding("utf-8");

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, encode);
            string responseFromServer = reader.ReadToEnd();
            string outStr = responseFromServer;
            reader.Close();
            dataStream.Close();
            response.Close();

            hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(outStr);
            if (hashTable["status"].ToString() == "1")
            {
                string geocodes = hashTable["geocodes"].ToString().Trim();
                geocodes = geocodes.Substring(1, geocodes.Length - 3);
                hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(geocodes);
                hashTable["sign"] = "1";
                hashTable["msg"] = "success";

            }
            return hashTable;
        }
        catch (Exception ex)
        {
            //AppLog.Error(ex.Message);
            hashTable["sign"] = "0";
            hashTable["msg"] = "请求失败，可能的原因是：" + ex.Message;
            return hashTable;
        }

    }

    public System.Collections.Hashtable GethttpresultBybsj(string url)
    {
        try
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("method", "loadLocation");
            //parameters.Add("DeviceID", "19190002187");
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            string outStr = html;

            Hashtable hashTable = JsonConvert.DeserializeObject<Hashtable>(outStr);
            return hashTable;
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