using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SmartFramework4v2.Data.SqlServer;
using System.Text;
using SmartFramework4v2.Web.WebExecutor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// DeskTop 的摘要说明
/// </summary>
[CSClass("DeskTop")]
public class DeskTop
{
	public DeskTop()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    private string[] Day = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

    [CSMethod("Tshow")]
    public string Tshow(string now_date)
    {
        using (var db = new DBConnection())
        {
            try
            {
//                List<string> days = new List<string>();
//                for (int i = 0; i < 7; i++)
//                    days.Add(Convert.ToDateTime(now_date).AddDays(i).ToString("yyyy-MM-dd"));

//                List<string> week = new List<string>();
//                for (int i = 0; i < 7; i++)
//                {
//                    string xq = Day[Convert.ToInt32(Convert.ToDateTime(now_date).AddDays(i).DayOfWeek.ToString("d"))].ToString();
//                    week.Add(xq);
//                }

//                string sql = "SELECT ID,ParentRoomId,RoomNo,RoomGuidNumber,IsClose FROM Lock_Room WHERE UserId = 1701 ORDER BY ID";
//                DataTable dt_room = db.ExecuteDataTable(sql);
//                dt_room.Columns.Add("DAY1");
//                dt_room.Columns.Add("DAY2");
//                dt_room.Columns.Add("DAY3");
//                dt_room.Columns.Add("DAY4");
//                dt_room.Columns.Add("DAY5");
//                dt_room.Columns.Add("DAY6");
//                dt_room.Columns.Add("DAY7");
//                dt_room.Columns.Add("DL");//电量
//                dt_room.Columns.Add("IsCanClose");//是否可关房

//                for (int i = 0; i < dt_room.Rows.Count; i++)
//                {
//                    dt_room.Rows[i]["IsCanClose"] = 0;
//                    //根据房间获取设备
//                    StringBuilder builder = new StringBuilder("select top 1 * from Lock_Device where 1=1 ");
//                    builder.AppendFormat(" and RoomId={0}", dt_room.Rows[i]["ID"].ToString());
//                    builder.Append(" and (CHARINDEX('电池电量',EquipmentInfo1Type)>0 or CHARINDEX('电池电量',EquipmentInfo2Type)>0 ");
//                    builder.Append(" or CHARINDEX('电池电量',EquipmentInfo3Type)>0 or CHARINDEX('电池电量',EquipmentInfo4Type)>0");
//                    builder.Append(" or CHARINDEX('电池电量',EquipmentInfo5Type)>0) ");
//                    builder.Append(" order by ID asc");
//                    DataTable dt_device = db.ExecuteDataTable(builder.ToString());//EquipmentInfo1
//                    if (dt_device.Rows.Count > 0)
//                    {
//                        string backdata = DeviceAPI.DeviceAPI.GetCollectDeviceInfo(dt_room.Rows[i]["RoomGuidNumber"].ToString(), dt_device.Rows[0]["DeviceNo"].ToString(), dt_device.Rows[0]["DeviceSN"].ToString());
//                        JObject backjobj = JsonConvert.DeserializeObject(backdata) as JObject;
//                        if (backjobj["status"] != null && backjobj["status"].ToString() == "ok")
//                        {
//                            JArray jaryshowinfo = backjobj["equipmentpara_table"] as JArray;
//                            string dcdl = jaryshowinfo[0]["EquipmentInfo1"].ToString();
//                            dt_room.Rows[i]["DL"] = dcdl;
//                        }
//                    }

//                    dt_room.Rows[i]["DAY1"] = days[0];
//                    dt_room.Rows[i]["DAY2"] = days[1];
//                    dt_room.Rows[i]["DAY3"] = days[2];
//                    dt_room.Rows[i]["DAY4"] = days[3];
//                    dt_room.Rows[i]["DAY5"] = days[4];
//                    dt_room.Rows[i]["DAY6"] = days[5];
//                    dt_room.Rows[i]["DAY7"] = days[6];
//                }

//                string sql_order = @"SELECT a.AuthorizeNo,a.AuthorLiveStatus,a.RoomId,a.HotelId,a.OrderUserState,CONVERT(varchar(10), a.LiveStartDate, 120) LiveStartDate,CONVERT(varchar(10), a.LiveEndDate, 120) LiveEndDate,
//                                 a.UserId,a.RealName,a.CellPhone,case when a.PlatType = 0 then '其他平台' when a.PlatType = 1 then 'E家智宿' when a.PlatType = 2 then '美团' when a.PlatType = 3 then '线下' end PlatType,
//                                 b.RoomGuidNumber,case when a.AuthorRoomStyle = 1 then '全天' when a.AuthorRoomStyle = 2 then '钟点' when a.AuthorRoomStyle = 3 then '月租' when a.AuthorRoomStyle = 4 then '看房' end AuthorRoomStyle
//                                 FROM Lock_AuthorizeOrder a 
//                                 LEFT JOIN Lock_Room b on a.RoomId = b.ID
//                                 WHERE a.FDUsreId = @FDUsreId AND a.AuthorLiveStatus in (0,1,2,3,4,6,16)";
//                SqlCommand cmd = db.CreateCommand(sql_order);
//                cmd.Parameters.AddWithValue("@FDUsreId", 1701);
//                DataTable dt_order = db.ExecuteDataTable(cmd);
//                dt_order.Columns.Add("ISRZ");//是否入住
//                dt_order.Columns.Add("ISSERVICE");//是否服务

//                string sql_service = @"SELECT ID,AuthorizeNo,ServiceStatus FROM Lock_ServiceApply WHERE AuthorizeNo IN (SELECT AuthorizeNo FROM Lock_AuthorizeOrder WHERE FDUsreId = @FDUsreId AND AuthorLiveStatus in (4,6))";
//                cmd = db.CreateCommand(sql_service);
//                cmd.Parameters.AddWithValue("@FDUsreId", 1701);
//                DataTable dt_service = db.ExecuteDataTable(cmd);

//                for (int i = 0; i < dt_room.Rows.Count; i++)
//                {
//                    DataRow[] drs = dt_order.Select("RoomId = '" + dt_room.Rows[i]["ID"].ToString() + "'");
//                    if (drs.Length > 0)
//                        dt_room.Rows[i]["IsCanClose"] = 1;
//                }

//                for (int i = 0; i < dt_order.Rows.Count; i++)
//                {
//                    dt_order.Rows[i]["ISRZ"] = 0;//默认未入住
//                    dt_order.Rows[i]["ISSERVICE"] = 1;//默认已服务
//                    if (dt_order.Rows[i]["AuthorLiveStatus"].ToString() == "1" || dt_order.Rows[i]["AuthorLiveStatus"].ToString() == "2" || dt_order.Rows[i]["AuthorLiveStatus"].ToString() == "3")
//                    {
//                        string str = DeviceAPI.DeviceAPI.CheckLiveStatus(dt_order.Rows[i]["RoomGuidNumber"].ToString(), dt_order.Rows[i]["AuthorizeNo"].ToString());
//                        JObject retjobj = JsonConvert.DeserializeObject(str) as JObject;
//                        if (retjobj["status"] != null && retjobj["status"].ToString().ToLower() == "ok")
//                        {
//                            JArray jary = retjobj["homelog_table"] as JArray;
//                            dt_order.Rows[i]["ISRZ"] = jary[0]["AuthoriGatewayOrderState"].ToString();
//                        }
//                    }
//                    else if (dt_order.Rows[i]["AuthorLiveStatus"].ToString() == "4" || dt_order.Rows[i]["AuthorLiveStatus"].ToString() == "6")
//                    {
//                        DataRow[] drs = dt_service.Select("AuthorizeNo = '" + dt_order.Rows[i]["AuthorizeNo"].ToString() + "'");
//                        if (drs.Length == 0)
//                            dt_order.Rows[i]["ISSERVICE"] = 0;
//                    }
//                }

//                int dcldd = 0;//待处理订单
//                int jrrz = 0;//今日入住
//                int jrdtf = 0;//今日待退房
//                int jrkf = 0;//今日空房
//                string sql_hotel = "SELECT HandlerKind,ID FROM Lock_Hotel WHERE ID IN (SELECT HotelId FROM Lock_Room WHERE UserId = 1701)";
//                DataTable dt_hotel = db.ExecuteDataTable(sql_hotel);
//                for (int i = 0; i < dt_hotel.Rows.Count; i++)
//                {
//                    if (dt_hotel.Rows[i]["HandlerKind"].ToString() == "0")
//                    {
//                        DataRow[] drs = dt_order.Select("HotelId = '" + dt_hotel.Rows[i]["ID"].ToString() + "' and AuthorLiveStatus = 0");
//                        dcldd += drs.Length;
//                    }
//                    else
//                    {
//                        DataRow[] drs = dt_order.Select("HotelId = '" + dt_hotel.Rows[i]["ID"].ToString() + "' and (AuthorLiveStatus = 0 or (AuthorLiveStatus = 1 and OrderUserState = 0))");
//                        dcldd += drs.Length;
//                    }
//                }

//                DataRow[] drs_jrrz = dt_order.Select("LiveStartDate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
//                jrrz = drs_jrrz.Length;

//                DataRow[] drs_jrdtf = dt_order.Select("LiveEndDate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
//                jrdtf = drs_jrdtf.Length;

//                for (int i = 0; i < dt_room.Rows.Count; i++)
//                {
//                    DataRow[] drs = dt_order.Select("LiveStartDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and LiveEndDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and RoomId = '" + dt_room.Rows[i]["ID"] + "'");
//                    if(drs.Length==0)
//                        jrkf++;
//                }

//                string html = SmartFramework4v2.Web.Common.SmartTemplate.RenderTemplate(HttpContext.Current.Server.MapPath("~/JS/Main/Tshow.cshtml"), new { days = days, week = week, dt_room = dt_room, dt_order = dt_order, dt_service = dt_service, dcldd = dcldd, jrrz = jrrz, jrdtf = jrdtf, jrkf = jrkf });

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("SetCloseRoom")]
    public bool SetCloseRoom(string RoomId)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "UPDATE Lock_Room SET IsClose = 1 WHERE ID = @ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@ID",RoomId);
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [CSMethod("SetOpenRoom")]
    public bool SetOpenRoom(string RoomId)
    {
        using (var db = new DBConnection())
        {
            try
            {
                string sql = "UPDATE Lock_Room SET IsClose = 0 WHERE ID = @ID";
                SqlCommand cmd = db.CreateCommand(sql);
                cmd.Parameters.AddWithValue("@ID", RoomId);
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}