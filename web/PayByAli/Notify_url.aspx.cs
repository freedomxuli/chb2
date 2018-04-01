using Aop.Api.Util;
using SmartFramework4v2.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Notify_url : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /* 实际验证过程建议商户添加以下校验。
        1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
        2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
        3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
        4、验证app_id是否为该商户本身。
        */

        Dictionary<string, string> sArray = GetRequestPost();
        if (sArray.Count != 0)
        {
            //交易状态
            //判断该笔订单是否在商户网站中已经做过处理
            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
            //如果有做过处理，不执行商户的业务程序

            //注意：
            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
            string trade_status = Request.Form["trade_status"];
            if (trade_status == "TRADE_SUCCESS")
            {
                using (var db = new DBConnection())
                {
                    string OrderDenno = Request.Form["out_trade_no"].ToString();
                    if (OrderDenno.Substring(0, 2) == "01")
                    {
                        string sql = "update ChongZhi set ZhiFuZhuangTai = 1,ZhiFuTime = '" + DateTime.Now + "' where OrderDenno = '" + OrderDenno + "'";
                        db.ExecuteNonQuery(sql);

                        sql = "select * from ChongZhi where OrderDenno = '" + OrderDenno + "'";
                        DataTable dt_ChongZhi = db.ExecuteDataTable(sql);

                        if (dt_ChongZhi.Rows.Count > 0)
                        {
                            sql = "select * from [dbo].[User] where UserID = '" + dt_ChongZhi.Rows[0]["UserID"].ToString() + "'";
                            DataTable dt_user = db.ExecuteDataTable(sql);

                            int num = Convert.ToInt32(dt_user.Rows[0]["UserRemainder"].ToString()) + Convert.ToInt32(dt_ChongZhi.Rows[0]["ChongZhiCiShu"].ToString());
                            sql = "update [dbo].[User] set UserRemainder = '" + num + "' where UserID = '" + dt_ChongZhi.Rows[0]["UserID"].ToString() + "'";
                            db.ExecuteNonQuery(sql);

                            DataTable dt_caozuo = db.GetEmptyDataTable("CaoZuoJiLu");
                            DataRow dr_caozuo = dt_caozuo.NewRow();
                            dr_caozuo["UserID"] = dt_ChongZhi.Rows[0]["UserID"];
                            dr_caozuo["CaoZuoLeiXing"] = "充值";
                            dr_caozuo["CaoZuoNeiRong"] = "web内用户充值，充值方式：支付宝；充值单号：" + OrderDenno + "；充值金额：" + Request.Form["total_amount"].ToString() + "。";
                            dr_caozuo["CaoZuoTime"] = DateTime.Now;
                            dr_caozuo["CaoZuoRemark"] = "";
                            dt_caozuo.Rows.Add(dr_caozuo);
                            db.InsertTable(dt_caozuo);
                        }
                    }
                    else if (OrderDenno.Substring(0, 2) == "03")
                    {
                        string sql = "select * from GpsDingDanSale where OrderDenno = '" + OrderDenno + "'";
                        DataTable dt_dingdan = db.ExecuteDataTable(sql);

                        string sql_dingdan = "update GpsDingDanSale set GpsDingDanZhiFuZhuangTai = 1,GpsDingDanZhiFuShiJian = '" + DateTime.Now + "' where OrderDenno = '" + OrderDenno + "'";
                        db.ExecuteNonQuery(sql_dingdan);

                        if (dt_dingdan.Rows.Count > 0)
                        {
                            string sql_mx = "select * from GpsDingDanSaleMingXi where GpsDingDanDenno = '" + dt_dingdan.Rows[0]["GpsDingDanDenno"].ToString() + "'";
                            DataTable dt_mx = db.ExecuteDataTable(sql_mx);

                            DataTable dt_device = db.GetEmptyDataTable("GpsDevice");
                            for (int i = 0; i < dt_mx.Rows.Count; i++)
                            {
                                DataRow dr_device = dt_device.NewRow();
                                dr_device["GpsDeviceID"] = dt_mx.Rows[i]["GpsDeviceID"].ToString();
                                dr_device["UserID"] = dt_dingdan.Rows[0]["UserID"].ToString();
                                dt_device.Rows.Add(dr_device);
                            }
                            db.InsertTable(dt_device);

                            DataTable dt_devicesale = db.GetEmptyDataTable("GpsDeviceSale");
                            for (int i = 0; i < dt_mx.Rows.Count; i++)
                            {
                                DataRow dr_device = dt_devicesale.NewRow();
                                dr_device["GpsDeviceID"] = dt_mx.Rows[i]["GpsDeviceID"].ToString();
                                dr_device["UserID"] = dt_dingdan.Rows[0]["UserID"].ToString();
                                dt_devicesale.Rows.Add(dr_device);
                            }
                            db.InsertTable(dt_devicesale);
                        }
                    }
                    else
                    {
                        string sql = "select * from GpsDingDan where OrderDenno = '" + OrderDenno + "'";
                        DataTable dt_dingdan = db.ExecuteDataTable(sql);

                        string sql_dingdan = "update GpsDingDan set GpsDingDanZhiFuZhuangTai = 1,GpsDingDanZhiFuShiJian = '" + DateTime.Now + "' where OrderDenno = '" + OrderDenno + "'";
                        db.ExecuteNonQuery(sql_dingdan);

                        if (dt_dingdan.Rows.Count > 0)
                        {
                            string sql_mx = "select * from GpsDingDanMingXi where GpsDingDanDenno = '" + dt_dingdan.Rows[0]["GpsDingDanDenno"].ToString() + "'";
                            DataTable dt_mx = db.ExecuteDataTable(sql_mx);

                            DataTable dt_device = db.GetEmptyDataTable("GpsDevice");
                            for (int i = 0; i < dt_mx.Rows.Count; i++)
                            {
                                DataRow dr_device = dt_device.NewRow();
                                dr_device["GpsDeviceID"] = dt_mx.Rows[i]["GpsDeviceID"].ToString();
                                dr_device["UserID"] = dt_dingdan.Rows[0]["UserID"].ToString();
                                dt_device.Rows.Add(dr_device);
                            }
                            db.InsertTable(dt_device);
                        }
                    }
                    DataTable dt = db.GetEmptyDataTable("ZhiFuOrder");
                    DataRow dr = dt.NewRow();
                    dr["Guid"] = Guid.NewGuid();
                    dr["OrderDenno"] = OrderDenno;
                    dr["Lx"] = 0;
                    dt.Rows.Add(dr);
                    db.InsertTable(dt);
                }
            }

            Response.Write("success");
        }
    }

    public Dictionary<string, string> GetRequestPost()
    {
        int i = 0;
        Dictionary<string, string> sArray = new Dictionary<string, string>();
        NameValueCollection coll;
        //coll = Request.Form;
        coll = Request.Form;
        String[] requestItem = coll.AllKeys;
        for (i = 0; i < requestItem.Length; i++)
        {
            WriteLog("DEBUG", requestItem[i], Request.Form[requestItem[i]]);
            sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
        }
        return sArray;

    }
    public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "logs";
    protected static void WriteLog(string type, string className, string content)
    {
        if (!Directory.Exists(path))//如果日志目录不存在就创建
        {
            Directory.CreateDirectory(path);
        }

        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
        string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

        //创建或打开日志文件，向日志文件末尾追加记录
        StreamWriter mySw = File.AppendText(filename);

        //向日志文件写入内容
        string write_content = time + " " + type + " " + className + ": " + content;
        mySw.WriteLine(write_content);

        //关闭日志文件
        mySw.Close();
    }
}