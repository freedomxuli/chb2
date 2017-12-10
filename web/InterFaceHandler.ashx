<%@ WebHandler Language="C#" Class="InterFaceHandler" %>

using System;
using System.Web;
using System.Collections;

public class InterFaceHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string str = "";
        switch (context.Request["action"])
        { 
            case "TuiDanDeciceIsClose":
                str = TuiDanDeciceIsClose(context);
                break;
            case "GDGByGPS":
                str = GDGByGPS(context);
                break;
            case "GDGByCZ":
                str = GDGByCZ(context);
                break;
            case "GDGChongZhi":
                str = GDGChongZhi(context);
                break;
            case "GetInvoiceList":
                str = GetInvoiceList(context);
                break;
            case "GetChongZhiListByInvoice":
                str = GetChongZhiListByInvoice(context);
                break;
            case "AddInvoice":
                str = AddInvoice(context);
                break;
            case "OnDelInvoice":
                str = OnDelInvoice(context);
                break;
            case "InsertClient":
                str = InsertClient(context);
                break;
            case "GetOrderDenno":
                str = GetOrderDenno(context);
                break;
            case "AddWayBill":
                str = AddWayBill(context);
                break;
            case "GetWayBillMemoByUserDenno":
                str = GetWayBillMemoByUserDenno(context);
                break;
            case "RelieveWayBill":
                str = RelieveWayBill(context);
                break;
            case "GetLocationList":
                str = GetLocationList(context);
                break;
        }
        context.Response.Write(str);
        context.Response.End();
    }

    //退单前判断是否能解绑该设备
    public string TuiDanDeciceIsClose(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "请先解绑该设备，再退单！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.TuiDanDeciceIsCloseByApp(context.Request["GpsDeviceID"], context.Request["UserName"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "可解绑！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }
    
    //gps公对公支付
    public string GDGByGPS(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "支付失败！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.GDGPay(context.Request["OrderDenno"], context.Request["DGZZCompany"], context.Request["DGZH"], context.Request["DKPZH"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "支付成功！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "支付失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    //充值公对公支付
    public string GDGByCZ(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "支付失败！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.GDGByMobileCZ(context.Request["UserName"], context.Request["OrderDenno"], Convert.ToDecimal(context.Request["ChongZhiJinE"]), Convert.ToInt32(context.Request["ChongZhiCiShu"]), context.Request["ChongZhiRemark"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "支付成功！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "支付失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }
    
    //充值公对公提交
    public string GDGChongZhi(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "提交失败！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.GDGChongZhi(context.Request["OrderDenno"], context.Request["DGZZCompany"], context.Request["DGZH"], context.Request["DKPZH"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "提交成功！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "提交失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    //获取发票列表
    public string GetInvoiceList(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "获取发票列表失败！";
        try
        {
            Handler App_Handler = new Handler();
            System.Data.DataTable dt = App_Handler.GetInvoiceListByMobile(context.Request["UserName"]);
            Newtonsoft.Json.Linq.JArray jary = new Newtonsoft.Json.Linq.JArray();
            jary = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(dt)) as Newtonsoft.Json.Linq.JArray;
            
            hash["sign"] = "1";
            hash["msg"] = "获取发票列表成功！";
            hash["info"] = jary;
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "获取失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }
    
    //获取可选充值订单
    public string GetChongZhiListByInvoice(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "可选充值订单失败！";
        try
        {
            Handler App_Handler = new Handler();
            System.Data.DataTable dt = App_Handler.GetChongZhiListByInvoiceM(context.Request["UserName"]);
            Newtonsoft.Json.Linq.JArray jary = new Newtonsoft.Json.Linq.JArray();
            jary = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(dt)) as Newtonsoft.Json.Linq.JArray;

            hash["sign"] = "1";
            hash["msg"] = "可选充值订单成功！";
            hash["info"] = jary;
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "获取失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }
    
    //新增发票
    public string AddInvoice(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "新增发票失败！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.AddInvoiceByMobile(context.Request["UserName"], context.Request["InvoiceTitle"], context.Request["InvoiceZZJGDM"], context.Request["InvoicePerson"], context.Request["InvoiceMobile"], context.Request["InvoiceAddress"], context.Request["je"], context.Request["ChongZhiIDs"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "提交成功！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "新增失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    //删除发票
    public string OnDelInvoice(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "删除发票失败！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.OnDelInvoice(context.Request["InvoiceId"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "删除成功！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "删除失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    //插入客户端id
    public string InsertClient(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "插入失败！";
        try
        {
            Handler App_Handler = new Handler();
            bool flag = App_Handler.InsertClient(context.Request["UserName"], context.Request["clientId"]);

            if (flag)
            {
                hash["sign"] = "1";
                hash["msg"] = "插入成功！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "插入失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    public string GetOrderDenno(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "获取失败！";
        try
        {
            Handler App_Handler = new Handler();
            string OrderDenno = App_Handler.GetOrderDenno("01");

            hash["sign"] = "1";
            hash["msg"] = "获取成功！";
            hash["OrderDenno"] = OrderDenno;
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "获取失败，内部错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    public string AddWayBill(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "制单失败！";
        try
        {
            context.Response.ContentType = "text/plain";
            //用户名
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            string UserID = context.Request["UserID"];
            string QiShiZhan = context.Request["Departure"];
            string DaoDaZhan = context.Request["Destination"];
            string SuoShuGongSi = context.Request["Company"];
            string UserDenno = context.Request["UserDenno"];
            string GpsDeviceID = context.Request["GpsDeviceID"];
            string YunDanRemark = context.Request["Memo"];
            
            Handler App_Handler = new Handler();
            int sign = App_Handler.AddWayBill(UserID, QiShiZhan, DaoDaZhan, SuoShuGongSi, UserDenno, GpsDeviceID, YunDanRemark);
            if (sign == 1)
            {
                hash["sign"] = "1";
                hash["msg"] = "制单成功！";
            }
            else if (sign == 2)
            {
                hash["sign"] = "2";
                hash["msg"] = "制单失败，用户标示或设备码错误！";
            }
            else if (sign == 3)
            {
                hash["sign"] = "3";
                hash["msg"] = "制单失败，起始站或到达站错误！";
            }
            else if (sign == 100)
            {
                hash["sign"] = "100";
                hash["msg"] = "制单失败，内部错误！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "制单失败，错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    public string GetWayBillMemoByUserDenno(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "查单失败！";
        try
        {
            if (!string.IsNullOrEmpty(context.Request["UserDenno"]) && !string.IsNullOrEmpty(context.Request["UserID"]))
            {
                Handler App_Handler = new Handler();
                System.Data.DataTable dt = App_Handler.GetWayBillMemoByUserDenno(context.Request["UserID"], context.Request["UserDenno"]);
                Newtonsoft.Json.Linq.JArray jary = new Newtonsoft.Json.Linq.JArray();
                jary = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(dt)) as Newtonsoft.Json.Linq.JArray;

                hash["sign"] = "1";
                hash["msg"] = "查单成功！";
                hash["info"] = jary;
            }
            else
            {
                hash["sign"] = "2";
                hash["msg"] = "查单失败，用户标示或单号不能为空";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "查单失败，错误:" + ex.Message;
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    public string RelieveWayBill(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "解绑失败！";

        try
        {
            Handler App_Handler = new Handler();
            context.Response.ContentType = "text/plain";
            //用户名
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            string UserID = context.Request["UserID"];
            string UserDenno = context.Request["UserDenno"];
            int sign = App_Handler.RelieveWayBill(UserID, UserDenno);
            if (sign == 1)
            {
                hash["sign"] = "1";
                hash["msg"] = "解绑成功！";
            }
            else if (sign == 2)
            {
                hash["sign"] = "2";
                hash["msg"] = "解绑失败，该运单已解绑！";
            }
            else
            {
                hash["sign"] = "100";
                hash["msg"] = "解绑失败，内部错误！";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "解绑失败，错误:" + ex.Message;
        }
        
        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }

    public string GetLocationList(HttpContext context)
    {
        Newtonsoft.Json.Linq.JObject hash = new Newtonsoft.Json.Linq.JObject();
        hash["sign"] = "0";
        hash["msg"] = "获取定位数据失败！";
        
        try
        {
            if (!string.IsNullOrEmpty(context.Request["UserDenno"]) && !string.IsNullOrEmpty(context.Request["UserID"]))
            {
                Handler App_Handler = new Handler();
                System.Data.DataTable dt = App_Handler.GetLocationList(context.Request["UserID"], context.Request["UserDenno"]);
                Newtonsoft.Json.Linq.JArray jary = new Newtonsoft.Json.Linq.JArray();
                jary = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(dt)) as Newtonsoft.Json.Linq.JArray;

                hash["sign"] = "1";
                hash["msg"] = "获取定位数据成功！";
                hash["info"] = jary;
            }
            else
            {
                hash["sign"] = "2";
                hash["msg"] = "获取定位数据失败，用户标示或单号不能为空";
            }
        }
        catch (Exception ex)
        {
            hash["sign"] = "0";
            hash["msg"] = "获取定位数据失败，错误:" + ex.Message;
        }
        

        return Newtonsoft.Json.JsonConvert.SerializeObject(hash);
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}