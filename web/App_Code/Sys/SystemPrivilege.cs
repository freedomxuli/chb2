using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///SystemPrivilege 的摘要说明
/// </summary>
namespace Smart.SystemPrivilege
{
    public class 平价数据审核_平价商店数据
    {
        public static PrivilegeDescription 平价公示审核 = new PrivilegeDescription("平价数据审核_平价商店数据-平价公示审核", "查看", 1);
        public static PrivilegeDescription 平价执行审核 = new PrivilegeDescription("平价数据审核_平价商店数据-平价执行审核", "查看", 2);
    }

    public class 平价数据审核_数据审核情况
    {
        public static PrivilegeDescription 上报情况汇总 = new PrivilegeDescription("平价数据审核_数据审核情况-上报情况汇总", "查看", 1);
        public static PrivilegeDescription 审核情况汇总 = new PrivilegeDescription("平价数据审核_数据审核情况-审核情况汇总", "查看", 2);
        public static PrivilegeDescription 公示情况汇总 = new PrivilegeDescription("平价数据审核_数据审核情况-公示情况汇总", "查看", 3);
        public static PrivilegeDescription 执行情况汇总 = new PrivilegeDescription("平价数据审核_数据审核情况-执行情况汇总", "查看", 4);
        public static PrivilegeDescription 价格公示展示 = new PrivilegeDescription("平价数据审核_数据审核情况-价格公示展示", "查看", 5);
        public static PrivilegeDescription 网上考核查询 = new PrivilegeDescription("平价数据审核_数据审核情况-网上考核查询", "查看", 6);
    }

    public class 数据汇总_平价商店
    {
        public static PrivilegeDescription 省表 = new PrivilegeDescription("数据汇总_平价商店-省表", "查看", 1);
        public static PrivilegeDescription 市表 = new PrivilegeDescription("数据汇总_平价商店-市表", "查看", 2);
        public static PrivilegeDescription 平价商店销售价格统计表 = new PrivilegeDescription("数据汇总_平价商店-平价商店销售价格统计表", "查看", 3);
        public static PrivilegeDescription 平价商店销售情况统计表 = new PrivilegeDescription("数据汇总_平价商店-平价商店销售情况统计表", "查看", 4);
        public static PrivilegeDescription 平价商店惠民统计表 = new PrivilegeDescription("数据汇总_平价商店-平价商店惠民统计表", "查看", 5);
    }

    public class 价格数据采集_农贸市场
    {
        public static PrivilegeDescription 数据上报 = new PrivilegeDescription("价格数据采集_农贸市场-数据上报", "查看", 1);
        public static PrivilegeDescription 任务审核 = new PrivilegeDescription("价格数据采集_农贸市场-任务审核", "查看", 2);
        public static PrivilegeDescription 汇总审核 = new PrivilegeDescription("价格数据采集_农贸市场-汇总审核", "查看", 3);
    }

    public class 价格数据采集_批发市场
    {
        public static PrivilegeDescription 数据上报 = new PrivilegeDescription("价格数据采集_批发市场-数据上报", "查看", 1);
        public static PrivilegeDescription 任务审核 = new PrivilegeDescription("价格数据采集_批发市场-任务审核", "查看", 2);
        public static PrivilegeDescription 汇总审核 = new PrivilegeDescription("价格数据采集_批发市场-汇总审核", "查看", 3);
    }

    public class 价格数据采集_生产基地
    {
        public static PrivilegeDescription 数据上报 = new PrivilegeDescription("价格数据采集_生产基地-数据上报", "查看", 1);
        public static PrivilegeDescription 任务审核 = new PrivilegeDescription("价格数据采集_生产基地-任务审核", "查看", 2);
        public static PrivilegeDescription 汇总审核 = new PrivilegeDescription("价格数据采集_生产基地-汇总审核", "查看", 3);
    }


    public class 数据汇总分析_价格数据汇总
    {
        public static PrivilegeDescription 平价数据汇总 = new PrivilegeDescription("数据汇总分析_价格数据汇总-平价数据汇总", "查看", 1);
        public static PrivilegeDescription 平价价格比对 = new PrivilegeDescription("数据汇总分析_价格数据汇总-平价价格比对", "查看", 2);
        public static PrivilegeDescription 农贸市场价格 = new PrivilegeDescription("数据汇总分析_价格数据汇总-农贸市场价格", "查看", 2);
        public static PrivilegeDescription 批发市场价格 = new PrivilegeDescription("数据汇总分析_价格数据汇总-批发市场价格", "查看", 3);
        public static PrivilegeDescription 生产基地价格 = new PrivilegeDescription("数据汇总分析_价格数据汇总-生产基地价格", "查看", 4);
    }

    public class 数据汇总分析_销量数据分析
    {
        public static PrivilegeDescription 每日销量分析 = new PrivilegeDescription("数据汇总分析_销量数据分析-每日销量分析", "查看", 1);
        public static PrivilegeDescription 销量对比分析 = new PrivilegeDescription("数据汇总分析_销量数据分析-销量对比分析", "查看", 2);
        public static PrivilegeDescription 平均销量对比 = new PrivilegeDescription("数据汇总分析_销量数据分析-平均销量对比", "查看", 3);
        public static PrivilegeDescription 品种销量分析 = new PrivilegeDescription("数据汇总分析_销量数据分析-品种销量分析", "查看", 4);
    }

    public class 数据汇总分析_惠民情况分析
    {
        public static PrivilegeDescription 每日惠民情况 = new PrivilegeDescription("数据汇总分析_惠民情况分析-每日惠民情况", "查看", 1);
        public static PrivilegeDescription 惠民情况统计 = new PrivilegeDescription("数据汇总分析_惠民情况分析-惠民情况统计", "查看", 2);
    }

    public class 统计报表分析_总体情况统计
    {
        public static PrivilegeDescription 总体情况统计 = new PrivilegeDescription("统计报表分析_总体情况统计-总体情况统计", "查看", 1);
    }
    public class 统计报表分析_分类情况统计
    {
        public static PrivilegeDescription 分类情况统计 = new PrivilegeDescription("统计报表分析_分类情况统计-分类情况统计", "查看", 1);
    }
    public class 统计报表分析_时段售价分析
    {
        public static PrivilegeDescription 时段售价分析 = new PrivilegeDescription("统计报表分析_时段售价分析-时段售价分析", "查看", 1);
    }
    public class 统计报表分析_销售价格统计
    {
        public static PrivilegeDescription 销售价格统计 = new PrivilegeDescription("统计报表分析_销售价格统计-销售价格统计", "查看", 1);
    }

    public class 运营分析_成本毛利分析
    {
        public static PrivilegeDescription 成本毛利分析 = new PrivilegeDescription("运营分析_成本毛利分析-成本毛利分析", "查看", 1);
    }
    public class 运营分析_流通差价分析
    {
        public static PrivilegeDescription 流通差价分析 = new PrivilegeDescription("运营分析_流通差价分析-流通差价分析", "查看", 1);
    }

    public class 监督管理指导_平价商店管理
    {
        public static PrivilegeDescription 平价商店管理 = new PrivilegeDescription("监督管理指导_平价商店管理-平价商店管理", "查看", 1);
    }
    public class 监督管理指导_平价店分布图
    {
        public static PrivilegeDescription 平价店分布图 = new PrivilegeDescription("监督管理指导_平价店分布图-平价店分布图", "查看", 1);
    }
    public class 监督管理指导_联网情况管理
    {
        public static PrivilegeDescription 客户端联网情况 = new PrivilegeDescription("监督管理指导_联网情况管理-客户端联网情况", "查看", 1);
        public static PrivilegeDescription 显示屏联网情况 = new PrivilegeDescription("监督管理指导_联网情况管理-显示屏联网情况", "查看", 2);
    }
    public class 监督管理指导_每日量化考核
    {
        public static PrivilegeDescription 每日量化考核 = new PrivilegeDescription("监督管理指导_每日量化考核-每日量化考核", "查看", 1);
    }
    public class 监督管理指导_品种销量排行
    {
        public static PrivilegeDescription 平价商品销量排行 = new PrivilegeDescription("监督管理指导_品种销量排行-平价商品销量排行", "查看", 1);
    }
    public class 监督管理指导_惠民情况排行
    {
        public static PrivilegeDescription 平价商店惠民排行 = new PrivilegeDescription("监督管理指导_惠民情况排行-平价商店惠民排行", "查看", 1);
    }
    public class 监督管理指导_信息发布中心
    {
        public static PrivilegeDescription 信息公告 = new PrivilegeDescription("监督管理指导_信息发布中心-信息公告", "查看", 1);
    }
    public class 监督管理指导_咨询答复中心
    {
        public static PrivilegeDescription 咨询答复 = new PrivilegeDescription("监督管理指导_咨询答复中心-咨询答复", "查看", 1);
    }

    public class 量化评分考核_评分汇总统计
    {
        public static PrivilegeDescription 评分汇总统计 = new PrivilegeDescription("量化评分考核_评分汇总统计-评分汇总统计", "查看", 1);
        public static PrivilegeDescription 历史评分汇总统计 = new PrivilegeDescription("量化评分考核_评分汇总统计-历史评分汇总统计", "查看", 2);
    }
    public class 量化评分考核_网上监测考核
    {
        public static PrivilegeDescription 数据上报情况 = new PrivilegeDescription("量化评分考核_网上监测考核-数据上报情况", "查看", 1);
        public static PrivilegeDescription 达标情况统计 = new PrivilegeDescription("量化评分考核_网上监测考核-达标情况统计", "查看", 2);
        public static PrivilegeDescription 销售情况统计 = new PrivilegeDescription("量化评分考核_网上监测考核-销售情况统计", "查看", 3);
        public static PrivilegeDescription 优惠金额情况统计 = new PrivilegeDescription("量化评分考核_网上监测考核-优惠金额情况统计", "查看", 4);
    }
    public class 量化评分考核_巡查考核
    {
        public static PrivilegeDescription 巡查考核统计 = new PrivilegeDescription("量化评分考核_巡查考核-巡查考核统计", "查看", 1);
        public static PrivilegeDescription 考核情况填报 = new PrivilegeDescription("量化评分考核_巡查考核-考核情况填报", "查看", 2);
    }
    public class 量化评分考核_专项考核
    {
        public static PrivilegeDescription 考核情况填报 = new PrivilegeDescription("量化评分考核_专项考核-考核情况填报", "查看", 1);
    }
    public class 量化评分考核_问卷调查考核
    {
        public static PrivilegeDescription 问卷调查考核 = new PrivilegeDescription("量化评分考核_问卷调查考核-问卷调查考核", "查看", 1);
    }
    public class 量化评分考核_奖励加分填报
    {
        public static PrivilegeDescription 奖励加分填报 = new PrivilegeDescription("量化评分考核_奖励加分填报-奖励加分填报", "查看", 1);
    }
    public class 量化评分考核_考核项目管理
    {
        public static PrivilegeDescription 考核项目管理 = new PrivilegeDescription("量化评分考核_考核项目管理-考核项目管理", "查看", 1);
    }

    public class 巡查采价考核_采价考核评分
    {
        public static PrivilegeDescription 采价考核评分 = new PrivilegeDescription("巡查采价考核_采价考核评分-采价考核评分", "查看", 1);
    }

    public class 巡查采价考核_巡查考核评分
    {
        public static PrivilegeDescription 巡查考核评分 = new PrivilegeDescription("巡查采价考核_巡查考核评分-巡查考核评分", "查看", 1);
    }

    public class 巡查采价考核_巡查采价评分统计
    {
        public static PrivilegeDescription 巡查采价评分统计 = new PrivilegeDescription("巡查采价考核_巡查采价评分统计-巡查采价评分统计", "查看", 1);
    }

    public class 巡查采价考核_巡查农贸市场
    {
        public static PrivilegeDescription 巡查农贸市场 = new PrivilegeDescription("巡查采价考核_巡查农贸市场-巡查农贸市场", "查看", 1);
    }

    public class 价格指数系统_平价菜篮子价格指数
    {
        public static PrivilegeDescription 平价菜篮子价格指数 = new PrivilegeDescription("价格指数系统_平价菜篮子价格指数-平价菜篮子价格指数", "查看", 1);
    }


    public class 预警预报系统_价格波动预警
    {
        public static PrivilegeDescription 价格波动预警 = new PrivilegeDescription("预警预报系统_价格波动预警-价格波动预警", "查看", 2);
    }

    public class 预警预报系统_供应增减预警
    {
        public static PrivilegeDescription 供应增减预警 = new PrivilegeDescription("预警预报系统_供应增减预警-供应增减预警", "查看", 3);
    }
    public class 预警预报系统_预警值设置
    {
        public static PrivilegeDescription 预警值设置 = new PrivilegeDescription("预警预报系统_预警值设置-预警值设置", "查看", 4);
    }
    public class 预警预报系统_预警预报
    {
        public static PrivilegeDescription 预警预报 = new PrivilegeDescription("预警预报系统_预警预报-预警预报", "查看", 1);
    }
    public class 预警预报系统_异常一览表
    {
        public static PrivilegeDescription 异常一览表 = new PrivilegeDescription("预警预报系统_异常一览表-异常一览表", "查看", 1);
    }



    public class 系统管理_商品管理
    {
        public static PrivilegeDescription 商品类别 = new PrivilegeDescription("系统管理_商品管理-商品类别", "查看", 1);
        public static PrivilegeDescription 商品维护 = new PrivilegeDescription("系统管理_商品管理-商品维护", "查看", 2);
    }
    public class 系统管理_角色管理
    {
        public static PrivilegeDescription 角色管理 = new PrivilegeDescription("系统管理_角色管理-角色管理", "查看", 1);
    }
    public class 系统管理_人员管理
    {
        public static PrivilegeDescription 人员管理 = new PrivilegeDescription("系统管理_人员管理-人员管理", "查看", 1);
    }
    public class 系统管理_参数设置
    {
        public static PrivilegeDescription 时间设定 = new PrivilegeDescription("系统管理_参数设置-时间设定", "查看", 1);
        public static PrivilegeDescription 上报类型设定 = new PrivilegeDescription("系统管理_参数设置-上报类型设定", "查看", 2);
        public static PrivilegeDescription 商品幅度设定 = new PrivilegeDescription("系统管理_参数设置-商品幅度设定", "查看", 3);
        public static PrivilegeDescription 上报商品品种设定 = new PrivilegeDescription("系统管理_参数设置-上报商品品种设定", "查看", 4);
    }
    public class 系统管理_采价任务设置
    {
        public static PrivilegeDescription 采价任务类别 = new PrivilegeDescription("系统管理_采价任务设置-采价任务类别", "查看", 1);
        public static PrivilegeDescription 采价任务设置 = new PrivilegeDescription("系统管理_采价任务设置-采价任务设置", "查看", 2);
    }
    public class 系统管理_采价单位管理
    {
        public static PrivilegeDescription 采价单位管理 = new PrivilegeDescription("系统管理_采价单位管理-采价单位管理", "查看", 1);
    }
    public class 系统管理_数据关联
    {
        public static PrivilegeDescription 数据关联 = new PrivilegeDescription("系统管理_数据关联-数据关联", "查看", 1);
    }


    public class 短信平台_用户组管理
    {
        public static PrivilegeDescription 用户组管理 = new PrivilegeDescription("短信平台_用户组管理", "查看", 1);
    }

    public class 短信平台_用户管理
    {
        public static PrivilegeDescription 用户管理 = new PrivilegeDescription("短信平台_用户管理", "查看", 1);
    }
    public class 短信平台_发送短信
    {
        public static PrivilegeDescription 发送短信 = new PrivilegeDescription("短信平台_发送短信", "查看", 1);
    }
    public class 短信平台_发信箱
    {
        public static PrivilegeDescription 发信箱 = new PrivilegeDescription("短信平台_发信箱", "查看", 1);
    }
    public class 短信平台_收信箱
    {
        public static PrivilegeDescription 收信箱 = new PrivilegeDescription("短信平台_收信箱", "查看", 1);
    }



    public class 平价商店权限_信息采集_公示价格
    {
        public static PrivilegeDescription 公示价格 = new PrivilegeDescription("平价商店权限_信息采集-公示价格", "查看", 1);
    }

    public class 平价商店权限_信息采集_销售价格
    {
        public static PrivilegeDescription 销售价格 = new PrivilegeDescription("平价商店权限_信息采集-销售价格", "查看", 1);
    }

    public class 平价商店权限_信息采集_农贸市场价格
    {
        public static PrivilegeDescription 农贸市场价格 = new PrivilegeDescription("平价商店权限_信息采集-农贸市场价格", "查看", 1);
    }

    public class 平价商店权限_量化考核_网上监测
    {
        public static PrivilegeDescription 网上监测 = new PrivilegeDescription("平价商店权限_量化考核_网上监测", "查看", 1);
    }

    public class 平价商店权限_量化考核_巡查记录
    {
        public static PrivilegeDescription 巡查记录 = new PrivilegeDescription("平价商店权限_量化考核_巡查记录", "查看", 1);
    }

    public class 平价商店权限_量化考核_专项检查
    {
        public static PrivilegeDescription 专项检查 = new PrivilegeDescription("平价商店权限_量化考核_专项检查", "查看", 1);
    }

    public class 平价商店权限_量化考核_季度考评
    {
        public static PrivilegeDescription 季度考评 = new PrivilegeDescription("平价商店权限_量化考核_季度考评", "查看", 1);
        public static PrivilegeDescription 历史季度考评 = new PrivilegeDescription("平价商店权限_量化考核_历史季度考评", "查看", 2);
    }

    public class 平价商店权限_价格中心_平价商品价格
    {
        public static PrivilegeDescription 平价商品价格 = new PrivilegeDescription("平价商店权限_价格中心_平价商品价格", "查看", 1);
    }

    public class 平价商店权限_价格中心_平价商店比较
    {
        public static PrivilegeDescription 平价商店比较 = new PrivilegeDescription("平价商店权限_价格中心_平价商店比较", "查看", 1);
    }

    public class 平价商店权限_价格中心_批发市场价格
    {
        public static PrivilegeDescription 批发市场价格 = new PrivilegeDescription("平价商店权限_价格中心_批发市场价格", "查看", 1);
    }

    public class 平价商店权限_宣传推荐_公示价格查询
    {
        public static PrivilegeDescription 公示价格查询 = new PrivilegeDescription("平价商店权限_宣传推荐_公示价格查询", "查看", 1);
    }



    public class 平价商店权限_信息服务_信息公告
    {
        public static PrivilegeDescription 信息公告 = new PrivilegeDescription("平价商店权限_信息服务_信息公告", "查看", 1);
    }



    public class 平价商店权限_信息服务_咨询答复
    {
        public static PrivilegeDescription 咨询答复 = new PrivilegeDescription("平价商店权限_信息服务_咨询答复", "查看", 1);
    }


    public class 平价商店权限_台账查询_平价商店管理
    {
        public static PrivilegeDescription 平价商店管理 = new PrivilegeDescription("平价商店权限_台账查询_平价商店管理", "查看", 1);
    }
    public class 平价商店权限_台账查询_平价惠民
    {
        public static PrivilegeDescription 平价公示表 = new PrivilegeDescription("平价商店权限_台账查询_平价惠民_平价公示表", "查看", 1);
        public static PrivilegeDescription 平价销售表 = new PrivilegeDescription("平价商店权限_台账查询_平价惠民_平价销售表", "查看", 1);
    }
    public class 平价商店权限_台账查询_省月报表
    {
        public static PrivilegeDescription 省月报表 = new PrivilegeDescription("平价商店权限_台账查询_省月报表", "查看", 1);
    }

    public class 平价商店权限_订货平台
    {
        public static PrivilegeDescription 在线订货 = new PrivilegeDescription("平价商店权限_订货平台_在线订货", "查看", 1);
        public static PrivilegeDescription 订单查询 = new PrivilegeDescription("平价商店权限_订货平台_订单查询", "查看", 1);
    }

    public class 供货平台
    {
        public static PrivilegeDescription 供货发布 = new PrivilegeDescription("供货平台_供货发布", "查看", 1);

        public static PrivilegeDescription 订单处理 = new PrivilegeDescription("供货平台_订单处理", "查看", 1);

        public static PrivilegeDescription 订单查询 = new PrivilegeDescription("供货平台_订单查询", "查看", 1);

    }

    public class 农超对接
    {
        public static PrivilegeDescription 订单查询 = new PrivilegeDescription("农超对接_订单查询", "查看", 1);
    }

    public class 台账查询_省月报表
    {
        public static PrivilegeDescription 省月报表 = new PrivilegeDescription("台账查询_省月报表-省月报表", "查看", 1);
    }

}

