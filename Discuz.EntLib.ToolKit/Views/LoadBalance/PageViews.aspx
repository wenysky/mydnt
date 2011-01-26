<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Discuz.EntLib.ToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>负载集群环境下各服务器PV</h2>
    <% using (Html.BeginForm("PageViews", "LoadBalance")) { %>
    <div>
        <%if (ViewData["StatisticUrl"] != null) { %>
            <%=Html.Hidden("moniterUrl")%>
            <%=Html.Hidden("opName")%>
            <input type="submit" value="获取指定服务器PV" onclick="javascript:CheckOption();$('#opName').attr('value', 'GetPageView');"  />
            <input type="submit" value="重置指定服务器PV" title="将会重置PV计数器,所有数据清零" onclick="$('#opName').attr('value', 'ClearPV');" />
            
            <%Html.RenderPartial("~/Views/Controls/SiteUrlListView.ascx");%>
        <%}%>
      </div>
    <%} %>
        
         <%if (ViewData["pageViewList"] != null) {%>
          <%foreach (WebSitePageViewInfo webSitePageViewInfo in ViewData["pageViewList"] as List<WebSitePageViewInfo>) {%>
               <fieldset>
               <legend>站点名称:<font color="red"><%=Html.Encode((new Uri(webSitePageViewInfo.WebSiteName)).Host)%></font>, 总页面访问量: <font color="red"><%=Html.Encode(webSitePageViewInfo.ViewSum)%></font></legend>
               <%if (webSitePageViewInfo.PageViewInfoList.Count >= 2)
                 { //正常返回数据%>
                  统计时间: <font color="red"><%=Html.Encode(webSitePageViewInfo.PageViewInfoList[0].PageName)%></font>----<font color="red"><%=Html.Encode(webSitePageViewInfo.PageViewInfoList[webSitePageViewInfo.PageViewInfoList.Count - 1].PageName)%></font>
                   <table width="100%">
                      <th>页面名称</th><th>浏览数</th><th>柱图</th>
                   <%foreach (PageViewInfo pageViewInfo in webSitePageViewInfo.PageViewInfoList)
                     {
                         if (pageViewInfo.Views > 0)
                         {%>
                           <tr>
                              <td width="20%"><%=Html.Encode(pageViewInfo.PageName)%></td>
                              <td width="10%"><%=Html.Encode(pageViewInfo.Views)%></td>
                              <td width="70%"><%if (webSitePageViewInfo.ViewSum > 0)
                                                {%>
                                                   <img src="/Images/Bar.gif" width="<%=Html.Encode(pageViewInfo.Views * 100/webSitePageViewInfo.ViewSum)%>%" height="12px"/>
                                              <%}%></td>
                           </tr>
                           <%} %>
                    <%} %>
                   </table>                    
               <%}%>    
              </fieldset>      
          <%} %>
        <%}%>
        
    <%=ViewData["OpInfo"].OutputOpInfo()%>
                       
    <div>
    <fieldset>
        <legend>环境配置步骤</legend>
        1.要开启监控，请将loadbalance.config（服务端）文件中节点设置如下:<br/>
          &nbsp;<%=Html.TextArea("loadbalance", "<AppLoadBalance>true</AppLoadBalance>\r\n<SiteUrl>填写负载均衡工具的链接地址(以','分割),形如:\r\n    http://10.0.2.137:8088/tools/,\r\n    http://10.0.2.150:8088/tools/\r\n</SiteUrl>\r\n<AuthCode>您的认证码信息</AuthCode>", new { rows = 7, cols = 100 })%><br/><br/>

        2.将ServerTool/LBStatistic.ashx和Discuz.EntLib.ToolKit.dll文件复制到服务端

    </fieldset>
    </div>
   

</asp:Content>
