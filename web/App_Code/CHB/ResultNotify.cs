using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartFramework4v2.Web.WebExecutor;
using SmartFramework4v2.Data.SqlServer;
using System.Data;

namespace WxPayAPI
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        public ResultNotify(Page page):base(page)
        {
        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());

                using (var db = new DBConnection())
                {
                    string OrderDenno = notifyData.GetValue("out_trade_no").ToString();
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
                            dr_caozuo["CaoZuoNeiRong"] = "web内用户充值，充值方式：微信；充值单号：" + OrderDenno + "；充值金额：" + Convert.ToInt32(notifyData.GetValue("total_fee").ToString()) / 100 + "。";
                            dr_caozuo["CaoZuoTime"] = DateTime.Now;
                            dr_caozuo["CaoZuoRemark"] = "";
                            dt_caozuo.Rows.Add(dr_caozuo);
                            db.InsertTable(dt_caozuo);
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
                    db.InsertTable(dr);
                }
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}