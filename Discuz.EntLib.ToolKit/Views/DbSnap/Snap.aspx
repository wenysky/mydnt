<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Discuz.EntLib.ToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        ���չ���</h2>
    <fieldset>
        <legend>"������־�б�" �������ò���</legend>    
        1.Ҫ������أ��뽫dbsnap.config������ˣ��ļ��нڵ���������:<br/>  
          &nbsp;<%=Html.TextArea("dbsnapconfig", "<AppDbSnap>true</AppDbSnap>\r\n<RecordeLog>true</RecordeLog>", new { rows = 2, cols = 60 })%><br/><br/>  
        2.��ServerTool/SnapTools.ashx��Discuz.EntLib.ToolKit.dll�ļ����Ƶ������
    </fieldset>
    
    <% using (Html.BeginForm("Snap", "DbSnap"))
       { %>
    <div>
        <%=Html.Hidden("moniterUrl")%>
        <%=Html.Hidden("opName")%>
       
        <table width="100%">
           <th style="width:20%">����</th><th style="width:60%">˵��</th>
           <tr>
              <td><input type="submit" value="��Ͽ���״̬" onclick="if (confirm('�ò����ȽϺ�ʱ,��ȷ����?')) {$('#opName').attr('value', 'GetSnapState'); return true;} return false;"/></td>
              <td>���ڲ鿴��д���뷽����<font color="red">Slave</font>���ݿ�(����)״̬, ע��ò����ȽϺ�ʱ!</td>
           </tr>
           <tr>
              <td><input type="submit" value="��ȡ������־�б�" onclick="CheckOption();$('#opName').attr('value', 'GetSnapLogList');"/><br /><br />
                  <input type="submit" value="���ÿ�����־�б�" onclick="CheckOption();$('#opName').attr('value', 'ClearSnapLogList');" title="�ò�����������ָ��վ��Ŀ�����־�б�"/></td>
              <td><%Html.RenderPartial("~/Views/Controls/SiteUrlListView.ascx");%></td>
           <tr>      
           <tr>
              <td colspan=2>
     <%if (ViewData["SnapLogList"] != null)
      {%>
          <%foreach (WebSiteSnapLogInfo webSiteSnapLogInfo in ViewData["SnapLogList"] as List<WebSiteSnapLogInfo>)
            {%>
               <fieldset>
               <legend>վ������:<font color="red"><%=Html.Encode((new Uri(webSiteSnapLogInfo.WebSiteName)).Host)%></font>, �ܿ��շ�����: <font color="red"><%=Html.Encode(webSiteSnapLogInfo.SnapLogInfoList.Count)%></font></legend>
             <!--������־����ͳ�ƿ�ʼ-->
                   <fieldset>
                       <legend>���շ���ͳ��</legend>
                        <table class="1">
                              <th>����ID</th><th>��������</th><th>���ʴ���</th>
                           <% webSiteSnapLogInfo.SnapSourceSumList.ForEach(delegate(SnapSourceSum snapSourceSum)
                              {%> 
                                   <tr>
                                      <td width="6%"><%=Html.Encode(snapSourceSum.SouceID)%></td>
                                      <td width="75%"><%=Html.Encode(snapSourceSum.DbconnectString)%></td>
                                      <td><%=Html.Encode(snapSourceSum.Sum)%></td>
                                   </tr>
                            <%});%>
                           </table>       
                   </fieldset>          
             <!--������־����ͳ�ƽ���-->
        
             <!--������־���ݿ�ʼ-->
                   <fieldset>
                       <legend>���շ�����־�б�</legend> 
                             <div style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                               <%if (webSiteSnapLogInfo.SnapLogInfoList.Count >0) { //������������%>
                                   <table >
                                      <th>����ID</th><th>��������</th><th>SQL���</th><th>�ύʱ��</th>
                                   <%foreach (SnapLogInfo snapLogInfo in webSiteSnapLogInfo.SnapLogInfoList){ %>
                                           <tr>
                                              <td width="6%"><%=Html.Encode(snapLogInfo.SouceID)%></td>
                                              <td><%=Html.Encode(snapLogInfo.DbconnectString)%></td>
                                              <td width="40%"><%=Html.TextArea("commandText", snapLogInfo.CommandText, new { cols = 70, rows = 3 })%></td>
                                              <td width="20%"><%=Html.Encode(snapLogInfo.PostDateTime)%></td>
                                           </tr>
                                    <%} %>
                                   </table>                    
                               <%}%>    
                              </div>
                    </fieldset>          
              <!--������־���ݽ���-->
              
               </fieldset>   
          <%} %>
        <%}%>
              </td>
           </tr>
        <table>
    </div>
    <% } %>
    
      <%if (ViewData["DbSnapState"] != null) {%>
          <table width ="100%">
                <th>����������Ϣ</th> <th>�Ƿ���Ч</th>  <th>������Ϣ</th>
                <%foreach (DbSnapState dbSnapState in ViewData["DbSnapState"] as List<DbSnapState>)
                  { %>
                       <tr>
                          <td width="30%"><%=Html.TextArea("connectstring", dbSnapState.ConnectString)%></td>
                          <td width="10%"><img src='/Images/<%=dbSnapState.SnapState?"true.jpg":"false.jpg"%>' width="16px" /></td>
                          <td><%=Html.Encode(dbSnapState.Message)%></td>
                       </tr>
                <%} %>
          </table>
      <%} %>
    

     <%=ViewData["OpInfo"].OutputOpInfo()%>
   
</asp:Content>
