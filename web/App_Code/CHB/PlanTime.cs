﻿using FluentScheduler;
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
using System.Net.Sockets;
using System.Web.Script.Serialization;

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
        Schedule<MyJobByGps3>().ToRunNow().AndEvery(30).Minutes();
        Schedule<MyJobByGps4>().ToRunNow().AndEvery(30).Minutes();
        
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
                    Schedule<MyJobByGps2>().ToRunNow().AndEvery(Convert.ToInt32(dt.Rows[i]["DeviceTime"].ToString())).Minutes();
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

                string sql = "select GpsDevicevid,GpsDevicevKey,GpsDeviceID,UserDenno,GpsDeviceID,UserID,YunDanDenno from YunDan where IsBangding = 1 order by BangDingTime desc";
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
                    string gpsvid = "";
                    string gpsvkey = "";

                    bool isbsj = true;

                    Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + dt_yun.Rows[i]["GpsDeviceID"] + "");
                    //Hashtable gpslocation = Gethttpresult("http://47.98.58.55:8998/gpsonline/GPSAPI", "method=loadLocation&DeviceID=" + dt_yun.Rows[i]["GpsDeviceID"] + "");
                    if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
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

                        gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");

                        isbsj = false;
                        if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                        {
                            continue;
                        }
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
                        if (isbsj)
                        {
                            dr_yun_up["speed"] = ja.First()["speed"].ToString();
                            dr_yun_up["direct"] = ja.First()["direct"].ToString();
                            dr_yun_up["temp"] = ja.First()["temp"].ToString();
                            dr_yun_up["oil"] = ja.First()["oil"].ToString();
                            dr_yun_up["battery"] = ja.First()["battery"].ToString();
                            dr_yun_up["totalDis"] = ja.First()["totalDis"].ToString();
                            dr_yun_up["vhcofflinemin"] = ja.First()["vhcofflinemin"].ToString();
                            dr_yun_up["parkingmin"] = ja.First()["parkingmin"].ToString();
                        }
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
        try
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
        catch (Exception ex)
        {
            throw ex;
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
            sr.Close();
            stream.Close();
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
        request.Timeout = -1;
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

public class MyJobByGps2 : IJob
{
    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();

                string sql = "select GpsDevicevid,GpsDevicevKey,GpsDeviceID,UserDenno,GpsDeviceID,UserID,YunDanDenno from YunDan where IsBangding = 1 and GpsDeviceID like '8630%' order by BangDingTime desc";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                sql = @"select a.GpsDeviceID,a.Gps_time from
                        (
	                        select GpsDeviceID,Gps_time,row_number() over (partition by GpsDeviceID order by Gps_time desc) rn from GpsLocation2 a 
	                        where exists(select 1 from YunDan where IsBangding = 1 and GpsDeviceID like '8630%' and GpsDeviceID = a.GpsDeviceID)
                        ) a where a.rn = 1";
                DataTable dt_gps_time = db.ExecuteDataTable(sql);

                DataTable dt_location = db.GetEmptyDataTable("GpsLocation2");

                for (int i = 0; i < dt_yun.Rows.Count; i++)
                {
                    string gpsvid = "";
                    string gpsvkey = "";

                    Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + dt_yun.Rows[i]["GpsDeviceID"] + "");
                    //Hashtable gpslocation = Gethttpresult("http://47.98.58.55:8998/gpsonline/GPSAPI", "method=loadLocation&DeviceID=" + dt_yun.Rows[i]["GpsDeviceID"] + "");
                    if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
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

                        gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");

                        if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                        {
                            continue;
                        }
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
        try
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
        catch (Exception ex)
        {
            throw ex;
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
            sr.Close();
            stream.Close();
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
        request.Timeout = -1;
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

public class MyJobByGps3 : IJob
{
    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select GpsDeviceID,CarNumber,SuoShuGongSi from YunDan where IsBangding = 1 and UserID = '4ddd6496-f031-4f4a-a50b-10742ff70462' order by BangDingTime desc";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                if (dt_yun.Rows.Count > 0)
                {
                    InsertData(dt_yun,0);
                }
            }
            catch (Exception ex)
            {
                
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
        request.Timeout = -1;
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
            sr.Close();
            stream.Close();
            string outStr = html;

            Hashtable hashTable = JsonConvert.DeserializeObject<Hashtable>(outStr);
            return hashTable;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void InsertData(DataTable dt,int num)
    {
        if (dt.Rows.Count - 1 > num)
        {
            Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + dt.Rows[num]["GpsDeviceID"] + "");

            if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
            {
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
                        ht_data.Add("deptName", dt.Rows[num]["SuoShuGongSi"].ToString().TrimEnd(' ').TrimStart(' '));//所属单位
                        ht_data.Add("longitude", newlng);
                        ht_data.Add("latitude", newlat);
                        ht_data.Add("positionDes", newinfo);
                        ht_data.Add("plateNumber", dt.Rows[num]["CarNumber"].ToString());
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
            
            InsertData(dt, num + 1);
        }
    }
}

public class MyJobByGps4 : IJob
{
    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "select GpsDeviceID,CarNumber,SuoShuGongSi,BangDingTime,Expect_Hour from YunDan where IsBangding = 1 and UserID = '4ddd6496-f031-4f4a-a50b-10742ff70462' order by BangDingTime desc";
                DataTable dt_yun = db.ExecuteDataTable(sql);

                if (dt_yun.Rows.Count > 0)
                {
                    InsertData(dt_yun, 0);
                }
            }
            catch (Exception ex)
            {

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
        request.Timeout = -1;
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
            sr.Close();
            stream.Close();
            string outStr = html;

            Hashtable hashTable = JsonConvert.DeserializeObject<Hashtable>(outStr);
            return hashTable;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void InsertData(DataTable dt, int num)
    {
        if (dt.Rows.Count - 1 > num)
        {
            if (!string.IsNullOrEmpty(dt.Rows[num]["Expect_Hour"].ToString()))
            {
                if (DateTime.Now > Convert.ToDateTime(dt.Rows[num]["BangDingTime"].ToString()).AddHours(Convert.ToDouble(dt.Rows[num]["Expect_Hour"].ToString())))
                {
                    Hashtable gpslocation = GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + dt.Rows[num]["GpsDeviceID"] + "");

                    if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                    {
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
                                ht_data.Add("positionDes", newinfo);
                                ht_data.Add("alarmInfoNo", "1");
                                ht_data.Add("deptName", dt.Rows[num]["SuoShuGongSi"].ToString().TrimEnd(' ').TrimStart(' '));//所属单位
                                ht_data.Add("alarmType", "到达超时");
                                ht_data.Add("startTime", ja.First()["gpstime"].ToString());
                                ht_data.Add("endTime", ja.First()["gpstime"].ToString());
                                ht_data.Add("alarmSpeed", ja.First()["speed"].ToString());
                                ht_data.Add("plateNumber", dt.Rows[num]["CarNumber"].ToString());
                                ht_data.Add("alarmInfo", "到达超时");
                                ht_data.Add("latitude", newlat);
                                ht_data.Add("longitude", newlng);
                                ht_data.Add("address", newinfo);
                                ht_data.Add("positioningTime", timeStamp);
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

                    InsertData(dt, num + 1);
                }
            }
        }
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
                DataTable dt_up = db.GetEmptyDataTable("YunDanDistance");
                DataTableTracker dtt_up = new DataTableTracker(dt_up);

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
//                                sql = @"update YunDanDistance set Gps_lastlat = '" + dt_yundan.Rows[i]["Gps_lastlat"].ToString() + @"',Gps_lastlng = '" + dt_yundan.Rows[i]["Gps_lastlng"].ToString() + @"',
//                                    Gps_lasttime = '" + dt_yundan.Rows[i]["Gps_lasttime"].ToString() + @"',Gps_distance = '" + (Convert.ToDecimal(ht["distance"]) / 1000).ToString("F2") + @"',
//                                    Gps_duration = '" + (Convert.ToDecimal(ht["duration"]) / 60).ToString("F2") + @"' where ID = '" + drs[0]["ID"].ToString() + @"'";
//                                db.ExecuteNonQuery(sql);
                                DataRow dr = dt_up.NewRow();
                                dr["ID"] = drs[0]["ID"].ToString();
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
                                dt_up.Rows.Add(dr);
                            }
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

                if (dt_up.Rows.Count > 0)
                    db.UpdateTable2(dt_up, dtt_up);

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