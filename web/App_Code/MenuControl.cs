using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;

/// <summary>
///MenuControl 的摘要说明
/// </summary>
public class MenuControl
{
    //<Tab p='Smart.SystemPrivilege.信息采集_平价商店数据.不符合要求的数据统计表' Name='不符合要求的数据统计表'>approot/r/page/jkpt/BFHYQTable.html</Tab>

    //<Tab p='Smart.SystemPrivilege.考核评分_网络监督考核.显示屏联网考核' Name='显示屏联网考核'>approot/r/page/LhKh/XspShow.html</Tab>

    public static String xmlMenu = @"
        <MainMenu>
        
            <Menu Name='制单'>            
                <Item Name='制单'>
                   <Tab p='' Name='制单'>approot/r/page/zhidan/zhidan.html</Tab>
                </Item>
            </Menu>
            <Menu Name='运单查询'>            
                <Item Name='我的运单'>
                   <Tab p='' Name='我的运单'>approot/r/page/ydcx/wdyd.html</Tab>
                </Item>
               <Item Name='自由查单'>
                   <Tab p='' Name='自由查单'>approot/r/page/ydcx/zycd.html</Tab>
                </Item>
            </Menu>
            <Menu Name='我的GPS'>            
                <Item Name='GPS管理'>
                   <Tab p='' Name='GPS管理'>approot/r/page/gps/gpsgl.html</Tab>
                </Item>
               <Item Name='GPS订单新增'>
                   <Tab p='' Name='GPS订单新增'>approot/r/page/gps/AddGpsDingdan.html</Tab>
                </Item>
               <Item Name='GPS订单'>
                   <Tab p='' Name='GPS订单'>approot/r/page/gps/gpsdd.html</Tab>
                </Item>
               <Item Name='GPS退单新增'>
                   <Tab p='' Name='GPS退单新增'>approot/r/page/gps/AddGpsTuidan.html</Tab>
                </Item>
               <Item Name='GPS退单'>
                   <Tab p='' Name='GPS退单'>approot/r/page/gps/gpstd.html</Tab>
                </Item>
            </Menu>
            <Menu Name='我的发票'>            
                <Item Name='发票管理'>
                   <Tab p='' Name='发票管理'>approot/r/page/wdfp/fpgl.html</Tab>
                </Item>
            </Menu>
            <Menu Name='我的账户'>            
                <Item Name='账户管理'>
                   <Tab p='' Name='账户管理'>approot/r/page/wdzh/zhgl.html</Tab>
                </Item>
               <Item Name='消费记录'>
                   <Tab p='' Name='消费记录'>approot/r/page/wdzh/xfjl.html</Tab>
                </Item>
               <Item Name='重置密码'>
                   <Tab p='' Name='重置密码'>approot/r/page/wdzh/czmm.html</Tab>
                </Item>
            </Menu>
          
        </MainMenu>
    ";




    public static string GenerateMenuByPrivilege()
    {
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        doc.LoadXml(xmlMenu);
        StringBuilder sb = new StringBuilder();
        int num = 0;

        var cu = SystemUser.CurrentUser;

        sb.Append("[");
        foreach (System.Xml.XmlElement MenuEL in doc.SelectNodes("/MainMenu/Menu"))
        {
            if (num > 0)
            {
                sb.Append(",");
            }
            num++;

            string title = MenuEL.GetAttribute("Name").ToString().Trim();



            string lis = "";
            foreach (System.Xml.XmlElement ItemEl in MenuEL.SelectNodes("Item"))
            {
                string secname = ItemEl.GetAttribute("Name");
                string msg = "";
                foreach (XmlElement TabEl in ItemEl.SelectNodes("Tab"))
                {
                    string p = TabEl.GetAttribute("p").ToString().Trim();
                    string pantitle = TabEl.GetAttribute("Name").ToString().Trim();
                    string src = TabEl.InnerText;
                    if (msg == "")
                    {
                        msg += pantitle + "," + src;
                    }
                    else
                    {
                        msg += "|" + pantitle + "," + src;
                    }
                }
                if (msg != "")
                {
                    lis += "+ '<li class=\"fore\"><a class=\"MenuItem\" href=\"page/TabMenu.html?msg=" + msg + "\" target=\"mainframe\"><img height=16 width=16 align=\"absmiddle\" style=\"border:0\" src=\"../CSS/images/application.png\" />　" + secname + "</a></li>'";

                }
            }

            if (lis != "")
            {
                sb.Append("{");
                sb.Append("xtype: 'panel',");
                sb.Append("collapsed: false,");
                sb.Append("iconCls: 'cf',");
                sb.Append("title: '" + title + "',");
                sb.Append("html: '<ul class=\"MenuHolder\">'");
                sb.Append(lis);
                sb.Append("+ '</ul>'");
                sb.Append("}");
            }
        }
        sb.Append("]");
        return sb.ToString();
    }
}
