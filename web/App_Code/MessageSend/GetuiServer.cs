using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.ProtocolBuffers;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.payload;
using System.Net;
using System.Data;

/// <summary>
/// GetuiServer 的摘要说明
/// </summary>
public class GetuiServer
{
    //参数设置 <-----参数需要重新设置----->
    //http的域名
    private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";

    //https的域名
    //private static String HOST = "https://api.getui.com/apiex.htm";

    //定义常量, appId、appKey、masterSecret 采用本文档 "第二步 获取访问凭证 "中获得的应用配置
    private static String APPID = "nUgQuBJdKw8mnvutSB5UnA";
    private static String APPKEY = "p2EC48zqBo5jctu670Tio8";
    private static String MASTERSECRET = "GBiThQA64mAIEgobHcoAb3";
    //private static String APPID = "sW0SEJ2IOj9xCXEtPe8Fu1";
    //private static String APPKEY = "4cEleyfzWZ7xFtCHotexH4";
    //private static String MASTERSECRET = "dg5s7hh57R6g883ZmeQjN8";

    public static void SendMessage(DataRow[] drs,int lx,string YunDanDenno,string address)
    {
        IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
        IBatch batch = new BatchImpl(APPKEY, push);

        if (lx == 1)
        {
            for (int i = 0; i < drs.Length; i++)
            {
                string message = "尊敬的" + drs[i]["UserName"].ToString() + "，您的运单号为：" + YunDanDenno + "的运单已离开出发地" + address+"。";
                NotificationTemplate templateNoti = NotificationTemplateDemo(lx, message);
                // 单推消息模型
                SingleMessage messageNoti = new SingleMessage();
                messageNoti.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
                messageNoti.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
                messageNoti.Data = templateNoti;
                //判断是否客户端是否wifi环境下推送，2为4G/3G/2G，1为在WIFI环境下，0为不限制环境
                //messageNoti.PushNetWorkType = 1;  

                com.igetui.api.openservice.igetui.Target targetNoti = new com.igetui.api.openservice.igetui.Target();
                targetNoti.appId = APPID;
                targetNoti.clientId = drs[i]["clientId"].ToString();
                batch.add(messageNoti, targetNoti);
            }
        }
        else if (lx == 2)
        {
            for (int i = 0; i < drs.Length; i++)
            {
                string message = "尊敬的" + drs[i]["UserName"].ToString() + "，您的运单号为：" + YunDanDenno + "的运单已到达目的地" + address + "。";
                NotificationTemplate templateNoti = NotificationTemplateDemo(lx, message);
                // 单推消息模型
                SingleMessage messageNoti = new SingleMessage();
                messageNoti.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
                messageNoti.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
                messageNoti.Data = templateNoti;
                //判断是否客户端是否wifi环境下推送，2为4G/3G/2G，1为在WIFI环境下，0为不限制环境
                //messageNoti.PushNetWorkType = 1;  

                com.igetui.api.openservice.igetui.Target targetNoti = new com.igetui.api.openservice.igetui.Target();
                targetNoti.appId = APPID;
                targetNoti.clientId = drs[i]["clientId"].ToString();
                batch.add(messageNoti, targetNoti);
            }
        }

        try
        {
            batch.submit();
        }
        catch (Exception e)
        {
            batch.retry();
        }
    }

    /*
     * 
     * 所有推送接口均支持四个消息模板，依次为透传模板，通知透传模板，通知链接模板，通知弹框下载模板
     * 注：IOS离线推送需通过APN进行转发，需填写pushInfo字段，目前仅不支持通知弹框下载功能
     *
     */
    //透传模板动作内容
    public static TransmissionTemplate TransmissionTemplateDemo()
    {
        TransmissionTemplate template = new TransmissionTemplate();
        template.AppId = APPID;
        template.AppKey = APPKEY;
        template.TransmissionType = "1";            //应用启动类型，1：强制应用启动 2：等待应用启动
        template.TransmissionContent = "这是一个测试";  //透传内容

        //设置客户端展示时间
        String begin = "2015-03-06 14:28:10";
        String end = "2015-03-06 14:38:20";
        template.setDuration(begin, end);

        return template;
    }

    //通知透传模板动作内容
    public static NotificationTemplate NotificationTemplateDemo(int lx,string message)
    {
        NotificationTemplate template = new NotificationTemplate();
        template.AppId = APPID;
        template.AppKey = APPKEY;
        //通知栏标题
        if (lx==1)
            template.Title = "货物出发提醒";
        else
            template.Title = "货物到达提醒";
        //通知栏内容     
        template.Text = message;
        //通知栏显示本地图片
        template.Logo = "";
        //通知栏显示网络图标
        template.LogoURL = "";
        //应用启动类型，1：强制应用启动  2：等待应用启动
        template.TransmissionType = "1";
        //透传内容  
        template.TransmissionContent = "";
        //接收到消息是否响铃，true：响铃 false：不响铃   
        template.IsRing = true;
        //接收到消息是否震动，true：震动 false：不震动   
        template.IsVibrate = true;
        //接收到消息是否可清除，true：可清除 false：不可清除    
        template.IsClearable = true;
        //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
        String begin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String end = DateTime.Now.AddHours(12).ToString("yyyy-MM-dd HH:mm:ss");
        template.setDuration(begin, end);

        return template;
    }
}