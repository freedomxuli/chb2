using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///LogManager 的摘要说明
/// </summary>
public class LogManager
{
    public static void Error(Exception ex)
    {
        var log = log4net.LogManager.GetLogger("CZCLZ");
        log.Error(ex.Message, ex);
    }
    public static void Info(string msg)
    {
        var log = log4net.LogManager.GetLogger("CZCLZ");
        log.Info(msg);
    }
    public static void Debug(string msg)
    {
        var log = log4net.LogManager.GetLogger("CZCLZ");
        log.Debug(msg);
    }
}
