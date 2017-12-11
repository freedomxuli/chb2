using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;

/// <summary>
/// Route 的摘要说明
/// </summary>
public class Route
{
    public static Hashtable getMapRoute(string origin, string destination)
    {
        Hashtable hashTable = new Hashtable();

        try
        {
            string url = "";
            url = "http://restapi.amap.com/v3/direction/driving?key=03d07f4db9627fbee898da02c692aded&origin=" + origin + "&destination=" + destination + "&originid=&destinationid=&extensions=base&strategy=0&waypoints=&avoidpolygons=&avoidroad=";

            WebRequest request = WebRequest.Create(url);
            Encoding encode = Encoding.GetEncoding("utf-8");

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, encode);
            string responseFromServer = reader.ReadToEnd();
            string outStr = responseFromServer;
            reader.Close();
            dataStream.Close();
            response.Close();

            hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(outStr);
            if (hashTable["status"].ToString() == "1")
            {
                string route = hashTable["route"].ToString().Trim();
                hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(route);
                string paths = hashTable["paths"].ToString().Trim();
                paths = paths.Substring(1, paths.Length - 3);
                hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(paths);
                hashTable["sign"] = "1";
                hashTable["msg"] = "success";

            }
            return hashTable;
        }
        catch (Exception ex)
        {
            hashTable["sign"] = "0";
            hashTable["msg"] = "请求失败，可能的原因是：" + ex.Message;
            return hashTable;
        }
    }
}