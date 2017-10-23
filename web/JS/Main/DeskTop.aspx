<%@ Page Language="C#" %>

<!DOCTYPE html>

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .fixedHeader th{
	        cursor:pointer;
	        color:#0CF;
        }

        .fixedHeader th a{
	        text-decoration:none;
	        color:#0CF;
        }

        .fixedHeader{
	        table-layout:fixed;
	        background-color:#ffffff;
	        width:100%;
        }

        .yd-data{
	        cursor:pointer;
        }
        .yd-data tr td:hover{
	        background:#CFF;
        }
        .confirm {
	        background:#CFF;
        }
    </style>
    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.min.js"></script>
</head>
<body>
<div style="overflow:hidden;height:95%;">
    	<table width="100%" cellpadding="0"  cellspacing="0" border="1" bordercolor="#cccccc">
            <thead class="fixedHeader" width="100%">
                <tr>
                    <th width="15%" height="80"><a href="javascript:void(0);">选择时间</a></th>
                    <th>2017-10-11 <br /> 周三</th>
                    <th>2017-10-12 <br /> 周四</th>
                    <th>2017-10-13 <br /> 周五</th>
                    <th>2017-10-14 <br /> 周六</th>
                    <th>2017-10-15 <br /> 周日</th>
                    <th>2017-10-16 <br /> 周一</th>
                    <th>2017-10-17 <br /> 周二</th>
                </tr>
            </thead>
        </table>
        <div style="overflow-y:auto;height:600px;margin-right:-18px;">
        <table class="yd-data" width="100%" cellpadding="0" cellspacing="0" border="1" bordercolor="#cccccc">
            <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td bgcolor="#0F0">&nbsp;</td>
                <td bgcolor="#0FF">&nbsp;</td>
                <td bgcolor="#FF0">&nbsp;</td>
                <td bgcolor="#C9F">&nbsp;</td>
                <td bgcolor="#999">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
                <tr>
                <td width="15%" height="80">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <div style="border-top:2px solid #cccccc;height:40px;line-height:40px;color:red;font-weight:bold;">
    	<div style="float:left;">待处理：</div>
        <div style="float:left;">2条</div>
        <div style="float:left;">　　</div>
        <div style="float:left;">今日入住：</div>
        <div style="float:left;">2条</div>
        <div style="float:left;">　　</div>
        <div style="float:left;">今天待退房：</div>
        <div style="float:left;">2条</div>
        <div style="float:left;">　　</div>
        <div style="float:left;">今天空房：</div>
        <div style="float:left;">2条</div>
        <div style="float:left;">　　</div>
    </div>
</div>
<script type="text/javascript">
    var $lastTD;
    $(".yd-data").on("click", "td", function () {
        if ($lastTD)
            $lastTD.removeClass("confirm");
        $(this).addClass("confirm");
        $lastTD = $(this);
    });
</script>
</body>
</html>
