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

/// <summary>
/// PlanTime 的摘要说明
/// </summary>
public class PlanTime : Registry
{
    public PlanTime()
    {
        // Schedule an IJob to run at an interval
        // 立即执行每两秒一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）
        Schedule<MyJob>().ToRunNow().AndEvery(5).Minutes();

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

public class MyJob : IJob
{

    void IJob.Execute()
    {
        using (var db = new DBConnection())
        {
            try
            {
                db.BeginTransaction();
                string sql = "select * from YunDan where SuoShuGongSi in (select Name from CompanyModel) and IsBangding = 1";
                DataTable dt = db.ExecuteDataTable(sql);

                sql = "select YunDanDenno from YunDanIsArrive";
                DataTable dt_dan = db.ExecuteDataTable(sql);

                DataTable dt_list = db.GetEmptyDataTable("YunDanIsArrive");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] drs = dt_dan.Select("YunDanDenno = '" + dt.Rows[i]["YunDanDenno"].ToString() + "'");
                    if (drs.Length == 0)
                    {
                        string DaoDaZhan = dt.Rows[i]["DaoDaZhan"].ToString().Replace(" ", "");
                        string[] LastZhanArray = dt.Rows[i]["Gps_lastinfo"].ToString().Split(' ');
                        string LastZhan = "";
                        if (LastZhanArray.Length >= 2)
                        {
                            LastZhan = LastZhanArray[0] + LastZhanArray[1];
                        }
                        if (DaoDaZhan == LastZhan)
                        {
                            DataRow dr = dt_list.NewRow();
                            dr["ID"] = Guid.NewGuid();
                            dr["YunDanDenno"] = dt.Rows[i]["YunDanDenno"];
                            dr["Addtime"] = DateTime.Now;
                            dr["Gps_lastlat"] = dt.Rows[i]["Gps_lastlat"];
                            dr["Gps_lastlng"] = dt.Rows[i]["Gps_lastlng"];
                            dr["Gps_lastinfo"] = dt.Rows[i]["Gps_lastinfo"];
                            dr["Company"] = dt.Rows[i]["SuoShuGongSi"];
                            dt_list.Rows.Add(dr);
                        }
                    }
                }
                if (dt_list.Rows.Count > 0)
                    db.InsertTable(dt_list);
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