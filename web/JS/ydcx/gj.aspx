<%@ Page Language="C#" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no, width=device-width">
    <title>轨迹回放</title>
    <link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css"/>
    <script src="http://webapi.amap.com/maps?v=1.4.6&key=您申请的key值"></script>
    <script type="text/javascript" src="http://cache.amap.com/lbs/static/addToolbar.js"></script>

    <script type="text/javascript" src="../extjs/ext-all.js"></script>
    <link rel="Stylesheet" type="text/css" href="../extjs/resources/css/ext-all.css" />
    <script type="text/javascript" src="../extjs/ext-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../json.js"></script>
    <script type="text/javascript" src="../cb.js"></script>
    <script type="text/javascript" src="../fun.js"></script>
    <script type="text/javascript" src="../city.js"></script>

</head>
<body>
<div id="container"></div>
<div class="button-group">
    <input type="button" class="button" value="开始动画" id="start"/>
     <input type="button" class="button" value="暂停动画" id="pause"/>
      <input type="button" class="button" value="继续动画" id="resume"/>
    <input type="button" class="button" value="停止动画" id="stop"/>
   

</div>
<script>
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串  
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    CS('CZCLZ.Handler.GJMap', function (retVal) {
        if(retVal)
        {
            var marker, lineArr = [];

            var map = new AMap.Map("container", {
                resizeEnable: true,
                center: [116.397428, 39.90923],
                zoom: 17
            });

            if (retVal.length > 0)
            {
                var centerdot = Math.floor(retVal.length/2);
                marker = new AMap.Marker({
                    map: map,
                    position: [retVal[0]["Gps_lng"], retVal[0]["Gps_lat"]],
                    icon: "http://webapi.amap.com/images/car.png",
                    offset: new AMap.Pixel(-26, -13),
                    zoom: 12,
                    autoRotation: true
                });
                var lineArr = [];
                for (var i = 0; i < retVal.length; i++) {
                    lineArr.push([retVal[i]["Gps_lng"], retVal[i]["Gps_lat"]]);
                }

                // 绘制轨迹
                var polyline = new AMap.Polyline({
                    map: map,
                    path: lineArr,
                    strokeColor: "#00A",  //线颜色
                    // strokeOpacity: 1,     //线透明度
                    strokeWeight: 3,      //线宽
                    // strokeStyle: "solid"  //线样式
                });
                var passedPolyline = new AMap.Polyline({
                    map: map,
                    // path: lineArr,
                    strokeColor: "#F00",  //线颜色
                    // strokeOpacity: 1,     //线透明度
                    strokeWeight: 3,      //线宽
                    // strokeStyle: "solid"  //线样式
                });


                marker.on('moving', function (e) {
                    passedPolyline.setPath(e.passedPath);
                })
                map.setFitView();

                AMap.event.addDomListener(document.getElementById('start'), 'click', function () {
                    marker.moveAlong(lineArr, 500);
                }, false);
                AMap.event.addDomListener(document.getElementById('pause'), 'click', function () {
                    marker.pauseMove();
                }, false);
                AMap.event.addDomListener(document.getElementById('resume'), 'click', function () {
                    marker.resumeMove();
                }, false);
                AMap.event.addDomListener(document.getElementById('stop'), 'click', function () {
                    marker.stopMove();
                }, false);
            }
        }
    }, CS.onError, GetRequest().UserID, GetRequest().YunDanDenno)
    
</script>
</body>
</html>
