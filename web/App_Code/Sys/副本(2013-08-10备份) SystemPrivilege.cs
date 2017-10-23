//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

///// <summary>
/////SystemPrivilege 的摘要说明
///// </summary>
//namespace Smart.SystemPrivilege
//{
//    public class 数据审核_平价商店数据
//    {
//        public static PrivilegeDescription 公示价格审核 = new PrivilegeDescription("数据审核_平价商店数据-公示价格审核", "查看", 1);
//        public static PrivilegeDescription 销售价格审核 = new PrivilegeDescription("数据审核_平价商店数据-销售价格审核", "查看", 2);
        
//    }

//    public class 数据汇总_平价直销店
//    {
//        public static PrivilegeDescription 公示价数据 = new PrivilegeDescription("数据汇总_平价直销店-公示价数据", "查看", 1);
//        public static PrivilegeDescription 销售价数据 = new PrivilegeDescription("数据汇总_平价直销店-销售价数据", "查看", 2);
//    }

//    public class 数据汇总_农贸市场数据
//    {
//        public static PrivilegeDescription 农贸市场数据 = new PrivilegeDescription("数据汇总_农贸市场数据-农贸市场数据", "查看", 1);
//    }

//    public class 数据汇总_批发市场数据
//    {
//        public static PrivilegeDescription 批发市场数据 = new PrivilegeDescription("数据汇总_批发市场数据-批发市场数据", "查看", 1);
//    }

//    public class 数据汇总_生产基地数据
//    {
//        public static PrivilegeDescription 生产基地数据 = new PrivilegeDescription("数据汇总_生产基地数据-生产基地数据", "查看", 1);
//    }


//    public class 数据分析_统计报表分析
//    {
//        public static PrivilegeDescription 省表 = new PrivilegeDescription("数据分析_统计报表分析-省表", "查看", 1);
//        public static PrivilegeDescription 市表 = new PrivilegeDescription("数据分析_统计报表分析-市表", "查看", 2);
//        public static PrivilegeDescription 销售情况统计 = new PrivilegeDescription("数据分析_统计报表分析-销售情况统计", "查看", 3);
//        public static PrivilegeDescription 蔬菜日销售情况统计 = new PrivilegeDescription("数据分析_统计报表分析-蔬菜日销售情况统计", "查看", 4);
//        public static PrivilegeDescription 未上报单位统计表 = new PrivilegeDescription("数据分析_统计报表分析-未上报单位统计表", "查看", 5);
//        public static PrivilegeDescription 未上报情况统计 = new PrivilegeDescription("数据分析_统计报表分析-未上报情况统计", "查看", 6);
//    }
//    public class 数据分析_惠民情况分析
//    {
//        public static PrivilegeDescription 全市惠民区域分析 = new PrivilegeDescription("数据分析_惠民情况分析-全市惠民区域分析", "查看", 1);
//        public static PrivilegeDescription 平价直销店惠民情况分析 = new PrivilegeDescription("数据分析_惠民情况分析-平价直销店惠民情况分析", "查看", 2);        
//    }
//    public class 数据分析_排行榜
//    {
//        public static PrivilegeDescription 平价直销店惠民排行榜 = new PrivilegeDescription("数据分析_排行榜-平价直销店惠民排行榜", "查看", 1);
//        public static PrivilegeDescription 平价商品销量排行榜 = new PrivilegeDescription("数据分析_排行榜-平价商品销量排行榜", "查看", 2);
//    }
//    public class 数据分析_成本毛利分析
//    {
//        public static PrivilegeDescription 成本毛利分析 = new PrivilegeDescription("数据分析_成本毛利分析-成本毛利分析", "查看", 1);
//    }
//    public class 数据分析_流通差价分析
//    {
//        public static PrivilegeDescription 流通差价分析 = new PrivilegeDescription("数据分析_流通差价分析-流通差价分析", "查看", 1);
//    }



//    public class 考核评分_评分汇总
//    {
//        public static PrivilegeDescription 考核评分汇总 = new PrivilegeDescription("考核评分_评分汇总-考核评分汇总", "查看", 1);
//    }
//    public class 考核评分_每日量化考核
//    {
//        public static PrivilegeDescription 每日量化考核 = new PrivilegeDescription("考核评分_每日量化考核-每日量化考核", "查看", 1);

//    }
//    public class 考核评分_网络监督考核
//    {
//        public static PrivilegeDescription 数据上报情况 = new PrivilegeDescription("考核评分_网络监督考核-数据上报情况", "查看", 1);
//        public static PrivilegeDescription 达标情况统计 = new PrivilegeDescription("考核评分_网络监督考核-达标情况统计", "查看", 2);
//        public static PrivilegeDescription 销售情况统计 = new PrivilegeDescription("考核评分_网络监督考核-销售情况统计", "查看", 3);
//    }
//    public class 考核评分_现场检查考核
//    {
//        public static PrivilegeDescription 巡查考核统计 = new PrivilegeDescription("考核评分_现场检查考核-巡查考核统计", "查看", 1);
//        public static PrivilegeDescription 考核情况填报 = new PrivilegeDescription("考核评分_现场检查考核-考核情况填报", "查看", 2);
//    }
//    public class 考核评分_专项检查考核
//    {
//        public static PrivilegeDescription 考核情况填报 = new PrivilegeDescription("考核评分_专项检查考核-考核情况填报", "查看", 1);
//    }
//    public class 考核评分_问卷调查考核
//    {
//        public static PrivilegeDescription 问卷调查考核 = new PrivilegeDescription("考核评分_问卷调查考核-问卷调查考核", "查看", 1);
//    }
//    public class 考核评分_奖励加分填报
//    {
//        public static PrivilegeDescription 奖励加分填报 = new PrivilegeDescription("考核评分_奖励加分填报-奖励加分填报", "查看", 1);
//    }
//    public class 考核评分_考核项目管理
//    {
//        public static PrivilegeDescription 考核项目管理 = new PrivilegeDescription("考核评分_考核项目管理-考核项目管理", "查看", 1);
//    }


//    public class 预警预报_价格波动预警
//    {
//        public static PrivilegeDescription 价格波动预警 = new PrivilegeDescription("预警预报_价格波动预警-价格波动预警", "查看", 2);
//    }

//    public class 预警预报_供应增减预警
//    {
//        public static PrivilegeDescription 供应增减预警 = new PrivilegeDescription("预警预报_供应增减预警-供应增减预警", "查看", 3);
//    }
//    public class 预警预报_预警值设置
//    {
//        public static PrivilegeDescription 预警值设置 = new PrivilegeDescription("预警预报_预警值设置-预警值设置", "查看", 4);
//    }


//    public class 系统管理_用户管理
//    {
//        public static PrivilegeDescription 人员权限管理 = new PrivilegeDescription("系统管理_用户管理-人员权限管理", "查看", 1);
//    }
//    public class 系统管理_商品管理
//    {
//        public static PrivilegeDescription 商品类别 = new PrivilegeDescription("系统管理_商品管理-商品类别", "查看", 1);
//        public static PrivilegeDescription 商品维护 = new PrivilegeDescription("系统管理_商品管理-商品维护", "查看", 2);
//    }
//    public class 系统管理_角色管理
//    {
//        public static PrivilegeDescription 角色管理 = new PrivilegeDescription("系统管理_角色管理-角色管理", "查看", 1);
//    }
//    public class 系统管理_平价直销店管理
//    {
//        public static PrivilegeDescription 平价直销店管理 = new PrivilegeDescription("系统管理_平价直销店管理-平价直销店管理", "查看", 1);
//    }
//    public class 系统管理_参数设置
//    {
//        public static PrivilegeDescription 时间设定 = new PrivilegeDescription("系统管理_参数设置-时间设定", "查看", 1);
//        public static PrivilegeDescription 上报类型设定 = new PrivilegeDescription("系统管理_参数设置-上报类型设定", "查看", 2);
//    }
//    public class 系统管理_客户端联网管理
//    {
//        public static PrivilegeDescription 客户端联网情况 = new PrivilegeDescription("系统管理_客户端联网管理-客户端联网情况", "查看", 1);
//    }


//    public class 信息平台_用户组管理
//    {
//        public static PrivilegeDescription 用户组管理 = new PrivilegeDescription("信息平台_用户组管理", "查看", 1);
//    }

//    public class 信息平台_用户管理
//    {
//        public static PrivilegeDescription 用户管理 = new PrivilegeDescription("信息平台_用户管理", "查看", 1);
//    }
//    public class 信息平台_发送短信
//    {
//        public static PrivilegeDescription 发送短信 = new PrivilegeDescription("信息平台_发送短信", "查看", 1);
//    }
//    public class 信息平台_发信箱
//    {
//        public static PrivilegeDescription 发信箱 = new PrivilegeDescription("信息平台_发信箱", "查看", 1);
//    }
//    public class 信息平台_收信箱
//    {
//        public static PrivilegeDescription 收信箱 = new PrivilegeDescription("信息平台_收信箱", "查看", 1);
//    }
//    public class 信息平台_信息公告
//    {
//        public static PrivilegeDescription 信息类别维护 = new PrivilegeDescription("信息平台_信息公告-信息类别维护", "查看", 1);
//        public static PrivilegeDescription 信息内容管理 = new PrivilegeDescription("信息平台_信息公告-信息内容管理", "查看", 2);
//    }
//    public class 信息平台_咨询答复
//    {
//        public static PrivilegeDescription 咨询答复 = new PrivilegeDescription("信息平台_咨询答复-咨询答复", "查看", 1);
//    }
//    public class 信息平台_平价直销店分布图
//    {
//        public static PrivilegeDescription 平价直销店分布图 = new PrivilegeDescription("信息平台_平价直销店分布图-平价直销店分布图", "查看", 1);
//    }



//    public class 平价商店权限_信息采集_公示价格
//    {
//        public static PrivilegeDescription 公示价格 = new PrivilegeDescription("平价商店权限_信息采集-公示价格", "查看", 1);
//    }

//    public class 平价商店权限_信息采集_销售价格
//    {
//        public static PrivilegeDescription 销售价格 = new PrivilegeDescription("平价商店权限_信息采集-销售价格", "查看", 1);
//    }

//    public class 平价商店权限_量化考核_网上监测
//    {
//        public static PrivilegeDescription 网上监测 = new PrivilegeDescription("平价商店权限_量化考核_网上监测", "查看", 1);
//    }

//    public class 平价商店权限_量化考核_巡查记录
//    {
//        public static PrivilegeDescription 巡查记录 = new PrivilegeDescription("平价商店权限_量化考核_巡查记录", "查看", 1);
//    }

//    public class 平价商店权限_量化考核_专项检查
//    {
//        public static PrivilegeDescription 专项检查 = new PrivilegeDescription("平价商店权限_量化考核_专项检查", "查看", 1);
//    }

//    public class 平价商店权限_量化考核_季度考评
//    {
//        public static PrivilegeDescription 季度考评 = new PrivilegeDescription("平价商店权限_量化考核_季度考评", "查看", 1);
//    }

//    public class 平价商店权限_价格中心_平价商品价格
//    {
//        public static PrivilegeDescription 平价商品价格 = new PrivilegeDescription("平价商店权限_价格中心_平价商品价格", "查看", 1);
//    }

//    public class 平价商店权限_价格中心_平价商店比较
//    {
//        public static PrivilegeDescription 平价商店比较 = new PrivilegeDescription("平价商店权限_价格中心_平价商店比较", "查看", 1);
//    }

//    public class 平价商店权限_价格中心_批发市场价格
//    {
//        public static PrivilegeDescription 批发市场价格 = new PrivilegeDescription("平价商店权限_价格中心_批发市场价格", "查看", 1);
//    }

//    public class 平价商店权限_宣传推荐_公示价格查询
//    {
//        public static PrivilegeDescription 公示价格查询 = new PrivilegeDescription("平价商店权限_宣传推荐_公示价格查询", "查看", 1);
//    }

   

//    public class 平价商店权限_信息服务_信息公告
//    {
//        public static PrivilegeDescription 信息公告 = new PrivilegeDescription("平价商店权限_信息服务_信息公告", "查看", 1);
//    }



//    public class 平价商店权限_信息服务_咨询答复
//    {
//        public static PrivilegeDescription 咨询答复 = new PrivilegeDescription("平价商店权限_信息服务_咨询答复", "查看", 1);
//    }


//    public class 平价商店权限_供需对接_供需对接
//    {
//        public static PrivilegeDescription 供需对接 = new PrivilegeDescription("平价商店权限_供需对接_供需对接", "查看", 1);
//    }

   

//}

