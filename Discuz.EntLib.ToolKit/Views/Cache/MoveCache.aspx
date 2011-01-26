<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Discuz.EntLib.ToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Encode(ViewData["Title"])%>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("MoveCache", "Cache"))
  { %>
    <div>
        <fieldset class="search">
            <legend><b>该功能用于将指定memcache服务器的缓存数据迁移到另外一台memcache服务器上</b></legend>
            <%=Html.Hidden("opName")%>
            <%=Html.Hidden("selectedSourceCacheKey")%>
            <table style="width:100%">
                <tr>
                    <td class="style2">
                        源memcached服务器: <input type="text" id="SourceTb" name="SourceTb" value="10.0.4.107:11211" />                  
                    </td>
                    <td>
                        目标memcached服务器: <input type="text" id="TargetTb" name="TargetTb" value="10.0.4.107:11212" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top;width:50%">
                        <%if (ViewData["sourceCache"]!=null) {%>
                            <%foreach (Object obj in ViewData["sourceCache"] as ArrayList){%>
                            <input type="checkbox" name="sourceCacheKey" value="<%=obj%>" /><%=obj%><br/>
                            <%}%>
                        <%}%>
                    </td>
                    <td style="vertical-align:top;width:50%">
                        <%if (ViewData["sourceCache"]!=null) {%>
                            <%foreach (Object obj in ViewData["targetCache"] as ArrayList){%>
                            <%=obj%><br/>
                            <%}%>
                        <%}%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                         <input type="checkbox" name="checkedAll" id="checkedAll" />全选/取消全选
                        <input type="submit" value="获取键值" onclick="$('#opName').attr('value', 'GetCacheKey');" />
                        <input type="submit" value="迁移键值" onclick="CheckOption();$('#opName').attr('value', 'MoveCache');" />
                    </td>
                  
                </tr>
              
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
                $('#selectedSourceCacheKey').attr('value', intvalue);
            }
            
            $("#checkedAll").click(
            function() {
                if (this.checked) {
                    $("input[name='sourceCacheKey']").each(function() {
                        this.checked = true;
                    });
                } else {
                $("input[name='sourceCacheKey']").each(function() {
                        this.checked = false;
                    });
                }
            }
         );   
        </script>

    </div>
    <%} %>
</asp:Content>
