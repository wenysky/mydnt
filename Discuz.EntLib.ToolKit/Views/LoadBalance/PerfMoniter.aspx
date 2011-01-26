<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--[if IE]><script language="javascript" type="text/javascript" src="/Scripts/excanvas.pack.js"></script><![endif]-->

    <script language="javascript" type="text/javascript" src="/Scripts/jquery.flot.js"></script>

    <h2>
        ���ؾ��⼯Ⱥ�����¼����CPUʹ���ʺͿ����ڴ�</h2>
        
    <h4>˵�����������ΪCPUʹ����(%)���Ҳ�����Ϊ���ֽڣ�����Ϊʱ����</h4>
 
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
        <legend>�������ò���</legend>    
        1.Ҫ������أ��뽫loadbalance.config������ˣ��ļ��нڵ���������:<br/>
          &nbsp;<%=Html.TextArea("loadbalance", "<AppLoadBalance>true</AppLoadBalance>\r\n<SiteUrl>��д���ؾ��⹤�ߵ����ӵ�ַ(��','�ָ�),����:\r\n    http://10.0.2.137:8088/tools/,\r\n    http://10.0.2.150:8088/tools/\r\n</SiteUrl>\r\n<AuthCode>������֤����Ϣ</AuthCode>", new { rows = 7, cols = 100 })%><br/><br/>

        2.��ServerTool/ComputerPerfMoniter.ashx��Discuz.EntLib.ToolKit.dll�ļ����Ƶ������
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
            $('#url_' + i).attr('innerHTML', '��ַ:<a href=' +_url[i]+ ' target="blank" >'+ _url[i] + '</a>');
            
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
                [{ label: "������(�ٷֱ�\%) ����ʱ��", data: _d1[index] },
                 { label: "�ڴ�\��Ч�ֽ���", data: _d2[index], yaxis: 2}],
                // { label: "����", data: _d3[index]}],
                _chartOptions[index]);
            window.setTimeout(function() {
                GetData(index)
            }, 2000);
        }

    </script>

</asp:Content>
