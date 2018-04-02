using FluentScheduler;
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
using System.Diagnostics;
using SmartFramework4v2.Data;
using System.Collections;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// PlanTime 的摘要说明
/// </summary>
public class PlanTime : Registry
{
    public PlanTime()
    {
        // Schedule an IJob to run at an interval
        // 立即执行每两秒一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）
        Schedule<MyJob>().ToRunNow().AndEvery(10).Minutes();
        Schedule<MyGpsDistanceJob>().ToRunNow().AndEvery(10).Minutes();
        
        using (var db = new DBConnection())
        {
            string sql = "select * from GpsDeviceTable";
            DataTable dt = db.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["DeviceCode"].ToString() == "1919")
                {
                    Schedule<MyJobByGps1>().ToRunNow().AndEvery(Convert.ToInt32(dt.Rows[i]["DeviceTime"].ToString())).Minutes();
                }
                else if (dt.Rows[i]["DeviceCode"].ToString() == "8630")
                {
                    
                }
            }
        }

        // Schedule an IJob to run once, delayed by a specific time interval
        // 延迟一个指定时间间隔执行一次计划任务。（当然，这个间隔依然可以是秒、分、时、天、月、年等。）
        //Schedule<MyJob>().ToRunOnceIn(5).Seconds();

        // Schedule a simple job to run at a specific time
        // 在一个指定时间执行计划任务（最常用。这里是在每天的下午 1:10 分执行）
        //Schedule(() => Trace.WriteLine("It‘s 1:10 PM now.")).ToRunEvery(1).Days().At(13, 10);

        //Schedule(() =>
        //{
        //    // 做你想做的事儿。
        //    Trace.WriteLine("It‘s 1:10 PM now.");

        //}).ToRunEvery(1).Days().At(13, 10);

        // Schedule a more complex action to run immediately and on an monthly interval
        // 立即执行一个在每月的星期一 3:00 的计划任务（可以看出来这个一个比较复杂点的时间，它意思是它也能做到！）
        //Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

        // Schedule multiple jobs to be run in a single schedule
        // 在同一个计划中执行两个（多个）任务
        //Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();

    }
}

public class MyJobByGps1 : IJob
{
    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string gpsvid = ""; 
                string gpsvkey = "";

                string sql = "select GpsDevicevid,GpsDevicevKey,GpsDeviceID,UserDenno,GpsDeviceID,UserID,YunDanDenno from YunDan where IsBangding = 1";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                sql = @"select a.GpsDeviceID,a.Gps_time from
                            (
	                            select GpsDeviceID,Gps_time,row_number() over (partition by GpsDeviceID order by Gps_time desc) rn from GpsLocation a 
	                            where exists(select 1 from YunDan where IsBangding = 1 and GpsDeviceID = a.GpsDeviceID)
                            ) a where a.rn = 1";
                DataTable dt_gps_time = db.ExecuteDataTable(sql);

                DataTable dt_location = db.GetEmptyDataTable("GpsLocation");
                DataTable dt_yun_up = db.GetEmptyDataTable("YunDan");
                DataTableTracker dtt_yun_up = new DataTableTracker(dt_yun_up);

                for (int i = 0; i < dt_yun.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt_yun.Rows[i]["GpsDevicevid"].ToString()))
                    {
                        Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + dt_yun.Rows[i]["GpsDeviceID"] + "&pwd=123456");
                        if (gpsinfo["success"].ToString().ToUpper() != "True".ToUpper())
                        {
                            continue;
                        }
                        gpsvid = gpsinfo["vid"].ToString();
                        gpsvkey = gpsinfo["vKey"].ToString();
                    }
                    else
                    {
                        gpsvid = dt_yun.Rows[i]["GpsDevicevid"].ToString();
                        gpsvkey = dt_yun.Rows[i]["GpsDevicevKey"].ToString();
                    }

                    Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");

                    if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                    {
                        continue;
                    }

                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());

                    string newgpstime = ja.First()["gpstime"].ToString();
                    //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                    string newlng = ja.First()["lng"].ToString();
                    //newlng = newlng.Substring(0, newlng.Length - 2);
                    string newlat = ja.First()["lat"].ToString();
                    //newlat = newlat.Substring(0, newlat.Length - 2);
                    string newinfo = ja.First()["info"].ToString();
                    //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                    //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                    long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                    DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                    long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                    DateTime gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime

                    DataRow[] drs = dt_gps_time.Select("Gps_time = '" + gpstm + "' and GpsDeviceID = '" + dt_yun.Rows[i]["GpsDeviceID"] + "'");
                    if (drs.Count() == 0)
                    {
                        DataRow dr_yun_up = dt_yun_up.NewRow();
                        dr_yun_up["GpsDeviceID"] = dt_yun.Rows[i]["GpsDeviceID"];
                        dr_yun_up["YunDanDenno"] = dt_yun.Rows[i]["YunDanDenno"];
                        dr_yun_up["UserID"] = dt_yun.Rows[i]["UserID"];
                        dr_yun_up["IsBangding"] = 1;
                        dr_yun_up["Gps_lasttime"] = gpstm;
                        dr_yun_up["Gps_lastlng"] = newlng;
                        dr_yun_up["Gps_lastlat"] = newlat;
                        dr_yun_up["Gps_lastinfo"] = newinfo;
                        if (gpsvid != "")
                        {
                            dr_yun_up["GpsDevicevid"] = gpsvid;
                            dr_yun_up["GpsDevicevKey"] = gpsvkey;
                        }
                        dt_yun_up.Rows.Add(dr_yun_up);

                        DataRow dr_location = dt_location.NewRow();
                        dr_location["GpsDeviceID"] = dt_yun.Rows[i]["GpsDeviceID"];
                        dr_location["Gps_lat"] = newlat;
                        dr_location["Gps_lng"] = newlng;
                        dr_location["Gps_time"] = gpstm;
                        dr_location["Gps_info"] = newinfo;
                        dr_location["GpsRemark"] = "自动定位";
                        dt_location.Rows.Add(dr_location);
                    }
                }
                if (dt_location.Rows.Count > 0)
                    db.InsertTable(dt_location);
                if (dt_yun_up.Rows.Count > 0)
                    db.UpdateTable(dt_yun_up, dtt_yun_up);

                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
            }
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
}

public class MyJob : IJob
{

    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                DataTable dt_message = db.GetEmptyDataTable("GpsMessage");

                #region 出发提醒
                string sql = "select * from YunDan where IsBangding = 1 and YunDanDenno not in (select YunDanDenno from GpsMessage)";
                DataTable dt_start = db.ExecuteDataTable(sql);

                string sql_user_client = "select a.UserID,a.clientId,b.UserName from User_Client a left join [dbo].[User] b on a.UserID = b.UserID";
                DataTable dt_user_client = db.ExecuteDataTable(sql_user_client);

                for (int i = 0; i < dt_start.Rows.Count; i++)
                {
                    string QiShiZhan = dt_start.Rows[i]["QiShiZhan"].ToString().Replace(" ", "");
                    string[] LastZhanArray = dt_start.Rows[i]["Gps_lastinfo"].ToString().Split(' ');
                    string LastZhan = "";
                    if (LastZhanArray.Length >= 2)
                    {
                        LastZhan = LastZhanArray[0] + LastZhanArray[1];
                    }
                    if (!string.IsNullOrEmpty(LastZhan))
                    {
                        if (QiShiZhan != LastZhan)
                        {
                            DataRow[] drs = dt_user_client.Select("UserID = '" + dt_start.Rows[i]["UserID"].ToString() + "'");
                            if (drs.Length > 0)
                            {
                                DataRow dr = dt_message.NewRow();
                                dr["GpsMessageId"] = Guid.NewGuid();
                                dr["YunDanDenno"] = dt_start.Rows[i]["YunDanDenno"].ToString();
                                dr["UserID"] = dt_start.Rows[i]["UserID"].ToString();
                                dr["Addtime"] = DateTime.Now;
                                dr["Gps_lastlat"] = dt_start.Rows[i]["Gps_lastlat"].ToString();
                                dr["Gps_lastlng"] = dt_start.Rows[i]["Gps_lastlng"].ToString();
                                dr["Gps_lastinfo"] = dt_start.Rows[i]["Gps_lastinfo"].ToString();
                                dr["LX"] = 1;
                                dt_message.Rows.Add(dr);
                                GetuiServer.SendMessage(drs, 1, dt_start.Rows[i]["YunDanDenno"].ToString(), QiShiZhan);
                            }
                        }
                    }
                }
                db.InsertTable(dt_message);
                #endregion

                #region 到达提醒
                DataTable dt_message_new = db.GetEmptyDataTable("GpsMessage");
                DataTableTracker dtt_message_new = new DataTableTracker(dt_message_new);

                sql = "select a.GpsMessageId,a.YunDanDenno,a.UserID,b.Gps_lastlat,b.Gps_lastlng,b.Gps_lastinfo,b.QiShiZhan,b.DaoDaZhan from GpsMessage a left join YunDan b on a.YunDanDenno = b.YunDanDenno where a.LX = 1 and a.YunDanDenno in (select YunDanDenno from GpsMessage where IsBangding = 1)";
                DataTable dt_end = db.ExecuteDataTable(sql);

                for (int i = 0; i < dt_end.Rows.Count; i++)
                {
                    string DaoDaZhan = dt_end.Rows[i]["DaoDaZhan"].ToString().Replace(" ", "");
                    string[] LastZhanArray = dt_end.Rows[i]["Gps_lastinfo"].ToString().Split(' ');
                    string LastZhan = "";
                    if (LastZhanArray.Length >= 2)
                    {
                        LastZhan = LastZhanArray[0] + LastZhanArray[1];
                    }
                    if (!string.IsNullOrEmpty(LastZhan))
                    {
                        if (DaoDaZhan == LastZhan)
                        {
                            DataRow[] drs = dt_user_client.Select("UserID = '" + dt_end.Rows[i]["UserID"].ToString() + "'");
                            if (drs.Length > 0)
                            {
                                DataRow dr = dt_message_new.NewRow();
                                dr["GpsMessageId"] = dt_end.Rows[i]["GpsMessageId"].ToString();
                                dr["Gps_lastlat"] = dt_end.Rows[i]["Gps_lastlat"].ToString();
                                dr["Gps_lastlng"] = dt_end.Rows[i]["Gps_lastlng"].ToString();
                                dr["Gps_lastinfo"] = dt_end.Rows[i]["Gps_lastinfo"].ToString();
                                dr["LX"] = 2;
                                dt_message_new.Rows.Add(dr);
                                GetuiServer.SendMessage(drs, 2, dt_end.Rows[i]["YunDanDenno"].ToString(), DaoDaZhan);
                            }
                        }
                    }
                }

                if(dt_message_new.Rows.Count > 0)
                    db.UpdateTable(dt_message_new, dtt_message_new);
                #endregion

                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }

    }
}

public class MyGpsDistanceJob : IJob
{

    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                DataTable dt_new = db.GetEmptyDataTable("YunDanDistance");
                //DataTable dt_up = db.GetEmptyDataTable("YunDanDistance");
                //dt_up.PrimaryKey = new DataColumn[] { dt_up.Columns["ID"] };
                //DataTableTracker dtt_up = new DataTableTracker(dt_up);

                string sql = "select * from YunDan where IsBangding = 1";
                DataTable dt_yundan = db.ExecuteDataTable(sql);

                sql = "select * from YunDanDistance where YunDanDenno in (select YunDanDenno from YunDan where IsBangding = 1)";
                DataTable dt_distance = db.ExecuteDataTable(sql);

                for (int i = 0; i < dt_yundan.Rows.Count; i++)
                {
                    DataRow[] drs = dt_distance.Select("YunDanDenno = '" + dt_yundan.Rows[i]["YunDanDenno"].ToString() + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["Gps_lastlat"].ToString() != dt_yundan.Rows[i]["Gps_lastlat"].ToString() && drs[0]["Gps_lastlng"].ToString() != dt_yundan.Rows[i]["Gps_lastlng"].ToString())
                        {
                            Hashtable ht = Route.getMapRoute(dt_yundan.Rows[i]["Gps_lastlng"].ToString() + "," + dt_yundan.Rows[i]["Gps_lastlat"].ToString(), dt_yundan.Rows[i]["DaoDaZhan_lng"].ToString() + "," + dt_yundan.Rows[i]["DaoDaZhan_lat"].ToString());
                            if (ht["distance"] != null && !string.IsNullOrEmpty(ht["distance"].ToString()) && ht["duration"] != null && !string.IsNullOrEmpty(ht["duration"].ToString()))
                            {
                                sql = @"update YunDanDistance set Gps_lastlat = '" + dt_yundan.Rows[i]["Gps_lastlat"].ToString() + @"',Gps_lastlng = '" + dt_yundan.Rows[i]["Gps_lastlng"].ToString() + @"',
                                    Gps_lasttime = '" + dt_yundan.Rows[i]["Gps_lasttime"].ToString() + @"',Gps_distance = '" + (Convert.ToDecimal(ht["distance"]) / 1000).ToString("F2") + @"',
                                    Gps_duration = '" + (Convert.ToDecimal(ht["duration"]) / 60).ToString("F2") + @"' where ID = '" + drs[0]["ID"].ToString() + @"'";
                                db.ExecuteNonQuery(sql);
                            }
                            //DataRow dr = dt_up.NewRow();
                            //dr["ID"] = drs[0]["ID"].ToString();
                            //dr["Gps_lastlat"] = dt_yundan.Rows[i]["Gps_lastlat"].ToString();
                            //dr["Gps_lastlng"] = dt_yundan.Rows[i]["Gps_lastlng"].ToString();
                            //dr["Gps_lasttime"] = dt_yundan.Rows[i]["Gps_lasttime"].ToString();
                            //if (ht["distance"] != null && !string.IsNullOrEmpty(ht["distance"].ToString()))
                            //    dr["Gps_distance"] = (Convert.ToDecimal(ht["distance"]) / 1000).ToString("F2");
                            //else
                            //    dr["Gps_distance"] = ht["distance"];
                            //if (ht["duration"] != null && !string.IsNullOrEmpty(ht["duration"].ToString()))
                            //    dr["Gps_duration"] = (Convert.ToDecimal(ht["duration"]) / 60).ToString("F2");
                            //else
                            //    dr["Gps_duration"] = ht["duration"];
                            //dt_up.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dt_yundan.Rows[i]["Gps_lastlat"].ToString()) && !string.IsNullOrEmpty(dt_yundan.Rows[i]["Gps_lastlng"].ToString()))
                        {
                            Hashtable ht = Route.getMapRoute(dt_yundan.Rows[i]["Gps_lastlng"].ToString() + "," + dt_yundan.Rows[i]["Gps_lastlat"].ToString(), dt_yundan.Rows[i]["DaoDaZhan_lng"].ToString() + "," + dt_yundan.Rows[i]["DaoDaZhan_lat"].ToString());
                            DataRow dr = dt_new.NewRow();
                            dr["YunDanDenno"] = dt_yundan.Rows[i]["YunDanDenno"];
                            dr["UserDenno"] = dt_yundan.Rows[i]["UserDenno"];
                            dr["UserID"] = dt_yundan.Rows[i]["UserID"];
                            dr["GpsDeviceID"] = dt_yundan.Rows[i]["GpsDeviceID"];
                            dr["Gps_lastlat"] = dt_yundan.Rows[i]["Gps_lastlat"];
                            dr["Gps_lastlng"] = dt_yundan.Rows[i]["Gps_lastlng"];
                            dr["Gps_lasttime"] = dt_yundan.Rows[i]["Gps_lasttime"];
                            if (ht["distance"] != null && !string.IsNullOrEmpty(ht["distance"].ToString()))
                                dr["Gps_distance"] = (Convert.ToDecimal(ht["distance"]) / 1000).ToString("F2");
                            else
                                dr["Gps_distance"] = ht["distance"];
                            if (ht["duration"] != null && !string.IsNullOrEmpty(ht["duration"].ToString()))
                                dr["Gps_duration"] = (Convert.ToDecimal(ht["duration"]) / 60).ToString("F2");
                            else
                                dr["Gps_duration"] = ht["duration"];
                            dt_new.Rows.Add(dr);
                        }
                    }
                }

                if (dt_new.Rows.Count > 0)
                    db.InsertTable(dt_new);

                //if (dt_up.Rows.Count > 0)
                //    db.UpdateTable(dt_up, dtt_up);

                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.RoolbackTransaction();
                throw ex;
            }
        }

    }
}



public class MyOtherJob : IJob
{

    void IJob.Execute()
    {
        Trace.WriteLine("这是另一个 Job ，现在时间是：" + DateTime.Now);
    }
}

public class MyComplexJob : IJob
{

    void IJob.Execute()
    {
        Trace.WriteLine("这是比较复杂的 Job ，现在时间是：" + DateTime.Now);
    }
}