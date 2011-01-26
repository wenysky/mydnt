<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Discuz.EntLib.ToolKit" %>
<fieldset>
     <legend>负载均衡集群列表</legend>
             <table width="100%">
                  <th>选择</th><th>站点链接</th><th>服务端工具链接</th>
                   <%foreach (string url in ViewData["StatisticUrl"] as List<string>)
                     {%>                  
                       <tr>
                          <td width="5%"><input type="checkbox" name="url" value="<%=Html.Encode(url)%>" <%if(ViewData["MoniterUrl"]!= null && (ViewData["MoniterUrl"] as string).IndexOf(url)>=0) {%>checked<%}%> /></td>
                          <td width="10%"><%=Html.Encode((new Uri(url)).Host)%></td>
                          <%if (ViewData["ViewName"] != null && ViewData["ViewName"] == "PageViews")
                            {%>
                               <td ><a href="<%=Utils.CreateRequestUrl(url, "pageViewStatistic")%>" target="_blank" ><%=Utils.CreateRequestUrl(url, "pageViewStatistic")%></a></td>
                          <%}else{%>
                               <td ><a href="<%=Utils.CreateRequestUrl(url, "getSnapLog")%>" target="_blank" ><%=Utils.CreateRequestUrl(url, "getSnapLog").TrimWithElipsis(70)%></a></td>
                           <%} %>
                       </tr>
                   <%} %>
             </table>               
</fieldset>

<script language="javascript">
     function CheckOption() {
         var elementarr = $("input[@type=checkbox]:checked");
         var intvalue = '';
         $.each(elementarr, function(n, element) {
             if (intvalue == '')
                 intvalue = element.value;
             else
                 intvalue = intvalue + ',' + element.value;
         });
         //alert(intvalue);
         $('#moniterUrl').attr('value', intvalue);
     }         
</script>
