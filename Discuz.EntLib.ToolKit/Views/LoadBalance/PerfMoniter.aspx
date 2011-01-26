<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--[if IE]><script language="javascript" type="text/javascript" src="/Scripts/excanvas.pack.js"></script><![endif]-->

    <script language="javascript" type="text/javascript" src="/Scripts/jquery.flot.js"></script>

    <h2>
        负载均衡集群环境下计算机CPU使用率和可用内存</h2>
        
    <h4>说明：左侧纵轴为CPU使用率(%)，右侧纵轴为兆字节，横轴为时间轴</h4>
 
    <%if (ViewData["MoniterUrl"] != null) {%>
        <%for(int i =0; i< ((string[])ViewData["MoniterUrl"]).Length; i++) {%>
            <div class="monitor">
            <fieldset>
                <legend id="url_<%=Html.Encode(i)%>"></legend>    
                <div id="placeholder_<%=Html.Encode(i)%>" style="width: 450px; height: 300px;">
                </div>
                <div id="legend_<%=Html.Encode(i)%>" style="width: 400px; height: 50px;">
                </div>              
                <div id="trace_<%=Html.Encode(i)%>">
                </div>
                </fieldset>
            </div>
        <%} %>
    <%} %>

    <fieldset>
        <legend>环境配置步骤</legend>    
        1.要开启监控，请将loadbalance.config（服务端）文件中节点设置如下:<br/>
          &nbsp;<%=Html.TextArea("loadbalance", "<AppLoadBalance>true</AppLoadBalance>\r\n<SiteUrl>填写负载均衡工具的链接地址(以','分割),形如:\r\n    http://10.0.2.137:8088/tools/,\r\n    http://10.0.2.150:8088/tools/\r\n</SiteUrl>\r\n<AuthCode>您的认证码信息</AuthCode>", new { rows = 7, cols = 100 })%><br/><br/>

        2.将ServerTool/ComputerPerfMoniter.ashx和Discuz.EntLib.ToolKit.dll文件复制到服务端
    </fieldset>


    <script id="source" language="javascript" type="text/javascript">
        //var _url = ['/ToolPage/ComputerPerfMoniter.ashx', '/ToolPage/ComputerPerfMoniter.ashx'];
        var _url = [<%=Html.Encode(string.Join(",", ((string[])ViewData["MoniterUrl"])))%>];
        var _d1 = new Array(_url.length);
        var _d2 = new Array(_url.length);
        //var _d3 = new Array(_url.length);
       
        var _chartOptions = new Array(_url.length);
        for (var i = 0; i < _url.length; i++) {
            _chartOptions[i] = {
                xaxis: { mode: "time" },
                series: {
                    lines: { show: true },
                    points: { show: true }
                },
                legend: {
                    show: true,
                    container: $("#legend_" + i)
                },
                yaxis: { min: 0 },
                y2axis: { tickFormatter: suffixFormatter }
            };
            $('#url_' + i).attr('innerHTML', '地址:<a href=' +_url[i]+ ' target="blank" >'+ _url[i] + '</a>');
            
            _d1[i] = [];
            _d2[i] = [];
            //_d3[i] = [];
        }

        function suffixFormatter(val, axis) {
            if (val > 1000000)
                return (val / 1000000).toFixed(axis.tickDecimals) + " MB";
            else if (val > 1000)
                return (val / 1000).toFixed(axis.tickDecimals) + " kB";
            else
                return val.toFixed(axis.tickDecimals) + " B";
        }

        $(function() {
            window.setTimeout(function() {
                for (var i = 0; i < _url.length; i++) {
                    GetData(i);
                }
            }, 2000);
        });

        function GetData(index) {        
            $.getJSON("/ClientTool/PerfMoniterProxy.ashx?url="+ _url[index] + "&math=" + Math.random() + "&index=" + index, showPercounter)
        }
        function showPercounter(data) {
            var index = data[2];
            //var index = data[3];
         //   $("#trace_"+index).html($("#trace").html()+"<br>"+data);            
            _d1[index].push(data[0]);            
            if (_d1[index].length >= 100) _d1[index].shift();
            
            _d2[index].push(data[1]);
            if (_d2.length >= 100) _d2[index].shift();
            
//            _d3[index].push(data[2]);
//            if (_d3.length >= 100) _d3[index].shift();

            $.plot($("#placeholder_" + index),
                [{ label: "处理器(百分比\%) 处理时间", data: _d1[index] },
                 { label: "内存\有效字节数", data: _d2[index], yaxis: 2}],
                // { label: "网络", data: _d3[index]}],
                _chartOptions[index]);
            window.setTimeout(function() {
                GetData(index)
            }, 2000);
        }

    </script>

</asp:Content>
