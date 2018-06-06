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
    <style type="text/css">
        .map_input {
            -web-kit-appearance:none;
            -moz-appearance: none;
            font-size:1em;
            height:1.8em;
            border-radius:4px;
            border:1px solid #c8cccf;
            color:#6a6f77;
            outline:0;
            width:140px;
        }
    </style>
    <script type="text/javascript" src="../extjs/ext-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../json.js"></script>
    <script type="text/javascript" src="../cb.js"></script>
    <script type="text/javascript" src="../fun.js"></script>
    <script type="text/javascript" src="../city.js"></script>

</head>
<body>
<div id="container"></div>
<div class="button-group">
    <div id="startTime" style="float:left;padding:5px 0px 0px 5px;"></div>
    <div style="float:left;padding:5px 0px 0px 5px;">~</div>
    <div id="endTime" style="float:left;padding:5px 5px 0px 5px;"></div>
    <input type="text" class="map_input" value="500" id="speed" placeholder="速度控制500-10000之间" />
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
    var startTime = "";
    var endTime = "";

    Ext.onReady(function () {
        Ext.create('Ext.form.field.Date', {
            renderTo: 'startTime',
            name: 'startTime',
            id: 'time1',
            format: 'Y-m-d',
            editable: false,
            value: new Date('2017-09-01')
        });
        Ext.create('Ext.form.field.Date', {
            renderTo: 'endTime',
            name: 'endTime',
            id: 'time2',
            format: 'Y-m-d',
            editable: false,
            value: new Date()
        });

        InitFun(Ext.getCmp("time1").getValue(), Ext.getCmp("time2").getValue());

        function InitFun(st,et) {
            CS('CZCLZ.Handler.GJMap', function (retVal) {
                if (retVal) {
                    var marker, lineArr = [];

                    var map = new AMap.Map("container", {
                        resizeEnable: true,
                        center: [116.397428, 39.90923],
                        zoom: 17
                    });

                    if (retVal.length > 0) {
                        var centerdot = Math.floor(retVal.length / 2);
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

                        marker.moveAlong(lineArr, document.getElementsByClassName('map_input')[0].value);

                        AMap.event.addDomListener(document.getElementById('start'), 'click', function () {
                            if (document.getElementsByClassName('map_input')[0].value) {
                                InitFun(Ext.getCmp("time1").getValue(), Ext.getCmp("time2").getValue());
                            }
                            else {
                                alert("请填入速度！");
                            }
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
            }, CS.onError, GetRequest().UserID, GetRequest().YunDanDenno, st,et);
        }
    });
    
</script>
</body>
</html>
