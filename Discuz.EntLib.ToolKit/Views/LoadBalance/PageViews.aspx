<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Discuz.EntLib.ToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>���ؼ�Ⱥ�����¸�������PV</h2>
    <% using (Html.BeginForm("PageViews", "LoadBalance")) { %>
    <div>
        <%if (ViewData["StatisticUrl"] != null) { %>
            <%=Html.Hidden("moniterUrl")%>
            <%=Html.Hidden("opName")%>
            <input type="submit" value="��ȡָ��������PV" onclick="javascript:CheckOption();$('#opName').attr('value', 'GetPageView');"  />
            <input type="submit" value="����ָ��������PV" title="��������PV������,������������" onclick="$('#opName').attr('value', 'ClearPV');" />
            
            <%Html.RenderPartial("~/Views/Controls/SiteUrlListView.ascx");%>
        <%}%>
      </div>
    <%} %>
        
         <%if (ViewData["pageViewList"] != null) {%>
          <%foreach (WebSitePageViewInfo webSitePageViewInfo in ViewData["pageViewList"] as List<WebSitePageViewInfo>) {%>
               <fieldset>
               <legend>վ������:<font color="red"><%=Html.Encode((new Uri(webSitePageViewInfo.WebSiteName)).Host)%></font>, ��ҳ�������: <font color="red"><%=Html.Encode(webSitePageViewInfo.ViewSum)%></font></legend>
               <%if (webSitePageViewInfo.PageViewInfoList.Count >= 2)
                 { //������������%>
                  ͳ��ʱ��: <font color="red"><%=Html.Encode(webSitePageViewInfo.PageViewInfoList[0].PageName)%></font>----<font color="red"><%=Html.Encode(webSitePageViewInfo.PageViewInfoList[webSitePageViewInfo.PageViewInfoList.Count - 1].PageName)%></font>
                   <table width="100%">
                      <th>ҳ������</th><th>�����</th><th>��ͼ</th>
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
        <legend>�������ò���</legend>
        1.Ҫ������أ��뽫loadbalance.config������ˣ��ļ��нڵ���������:<br/>
          &nbsp;<%=Html.TextArea("loadbalance", "<AppLoadBalance>true</AppLoadBalance>\r\n<SiteUrl>��д���ؾ��⹤�ߵ����ӵ�ַ(��','�ָ�),����:\r\n    http://10.0.2.137:8088/tools/,\r\n    http://10.0.2.150:8088/tools/\r\n</SiteUrl>\r\n<AuthCode>������֤����Ϣ</AuthCode>", new { rows = 7, cols = 100 })%><br/><br/>

        2.��ServerTool/LBStatistic.ashx��Discuz.EntLib.ToolKit.dll�ļ����Ƶ������

    </fieldset>
    </div>
   

</asp:Content>
