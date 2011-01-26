<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Discuz.EntLib.ToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        快照工具</h2>
    <fieldset>
        <legend>"快照日志列表" 环境配置步骤</legend>    
        1.要开启监控，请将dbsnap.config（服务端）文件中节点设置如下:<br/>  
          &nbsp;<%=Html.TextArea("dbsnapconfig", "<AppDbSnap>true</AppDbSnap>\r\n<RecordeLog>true</RecordeLog>", new { rows = 2, cols = 60 })%><br/><br/>  
        2.将ServerTool/SnapTools.ashx和Discuz.EntLib.ToolKit.dll文件复制到服务端
    </fieldset>
    
    <% using (Html.BeginForm("Snap", "DbSnap"))
       { %>
    <div>
        <%=Html.Hidden("moniterUrl")%>
        <%=Html.Hidden("opName")%>
       
        <table width="100%">
           <th style="width:20%">操作</th><th style="width:60%">说明</th>
           <tr>
              <td><input type="submit" value="诊断快照状态" onclick="if (confirm('该操作比较耗时,您确定吗?')) {$('#opName').attr('value', 'GetSnapState'); return true;} return false;"/></td>
              <td>用于查看读写分离方案中<font color="red">Slave</font>数据库(快照)状态, 注意该操作比较耗时!</td>
           </tr>
           <tr>
              <td><input type="submit" value="获取快照日志列表" onclick="CheckOption();$('#opName').attr('value', 'GetSnapLogList');"/><br /><br />
                  <input type="submit" value="重置快照日志列表" onclick="CheckOption();$('#opName').attr('value', 'ClearSnapLogList');" title="该操作将会重置指定站点的快照日志列表"/></td>
              <td><%Html.RenderPartial("~/Views/Controls/SiteUrlListView.ascx");%></td>
           <tr>      
           <tr>
              <td colspan=2>
     <%if (ViewData["SnapLogList"] != null)
      {%>
          <%foreach (WebSiteSnapLogInfo webSiteSnapLogInfo in ViewData["SnapLogList"] as List<WebSiteSnapLogInfo>)
            {%>
               <fieldset>
               <legend>站点名称:<font color="red"><%=Html.Encode((new Uri(webSiteSnapLogInfo.WebSiteName)).Host)%></font>, 总快照访问量: <font color="red"><%=Html.Encode(webSiteSnapLogInfo.SnapLogInfoList.Count)%></font></legend>
             <!--快照日志访问统计开始-->
                   <fieldset>
                       <legend>快照访问统计</legend>
                        <table class="1">
                              <th>快照ID</th><th>快照链接</th><th>访问次数</th>
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
             <!--快照日志访问统计结束-->
        
             <!--快照日志数据开始-->
                   <fieldset>
                       <legend>快照访问日志列表</legend> 
                             <div style="height:250px; overflow-y:scroll; overflow-x:hidden;">
                               <%if (webSiteSnapLogInfo.SnapLogInfoList.Count >0) { //正常返回数据%>
                                   <table >
                                      <th>快照ID</th><th>快照链接</th><th>SQL语句</th><th>提交时间</th>
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
              <!--快照日志数据结事-->
              
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
                <th>快照链接信息</th> <th>是否有效</th>  <th>错误信息</th>
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
