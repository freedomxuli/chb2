using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Ali_url : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var lx = Request["lx"].ToString();
        var orderdenno = Request["orderdenno"].ToString();
        var fin_je = Request["fin_je"].ToString();
        IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", config.app_id, config.private_key, "json", "1.0", "RSA2", config.alipay_public_key, "utf-8", false);
        AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
        //request.BizContent = "{" +
        //"    \"body\":\"Iphone6 16G\"," +
        //"    \"subject\":\"Iphone6 16G\"," +
        //"    \"out_trade_no\":\"20150320010101001\"," +
        //"    \"total_amount\":88.88," +
        //"    \"product_code\":\"FAST_INSTANT_TRADE_PAY\"" +
        //"  }";
        SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
        if (lx == "1")
        {
            m_values["body"] = "查货宝充值";
            m_values["subject"] = "查货宝充值";
        }
        else if (lx == "2")
        {
            m_values["body"] = "查货宝购买";
            m_values["subject"] = "查货宝购买";
        }
        else
        {
            m_values["body"] = "查货宝押金";
            m_values["subject"] = "查货宝押金";
        }
        m_values["out_trade_no"] = orderdenno;
        m_values["total_amount"] = Convert.ToDecimal(fin_je);
        m_values["product_code"] = "FAST_INSTANT_TRADE_PAY";
        m_values["qr_pay_mode"] = 0;
        string json1 = JsonHelper.SerializeObject(m_values);
        request.BizContent = json1;
        request.SetNotifyUrl("http://client.chahuobao.net/PayByAli/Notify_url.aspx");

        AlipayTradePagePayResponse response = client.pageExecute(request);
        string form = response.Body;
        Response.Write(form);
    }

    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}