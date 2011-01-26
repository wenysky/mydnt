<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Discuz.Cache" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Encode(ViewData["Title"])%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>缓存同步</h2>
    
      <% using (Html.BeginForm("SyncCache", "Cache"))
         { %>
            <div>
             <%=Html.Hidden("cacheKeyArray")%>
             <input type="checkbox" name="checkedAll" id="checkedAll" />全选/取消全选
             <input type="submit" value="同步指定键值的缓存信息" onclick="CheckOption();"/><br/><br/>
             <table width="100%">
                <th style="width:5%">选中</th><th style="width:30%">键值</th><th>类型</th><th>说明</th>
                <!-- UI START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS%></td>
                   <td>界面</td>
                   <td>论坛底部版块下拉列表</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO%></td>
                   <td>界面</td>
                   <td>表情列表信息</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST%></td>
                   <td>界面</td>
                   <td>用户自定义标签[JavaScript数组类型]</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO%></td>
                   <td>界面</td>
                   <td>用户自定义标签[CustomEditorButtonInfo数组形式]</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_SMILIES_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_SMILIES_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_SMILIES_LIST%></td>
                   <td>界面</td>
                   <td>表情符的JSON数据</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_ICONS_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_ICONS_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_ICONS_LIST%></td>
                   <td>界面</td>
                   <td>Icons主题图标数据</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS%></td>
                   <td>界面</td>
                   <td>模板列表的下拉框</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_ONLINE_ICON_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_ONLINE_ICON_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_ONLINE_ICON_LIST%></td>
                   <td>界面</td>
                   <td>在线用户列表图例</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_MEDALS_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_MEDALS_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_MEDALS_LIST%></td>
                   <td>界面</td>
                   <td>勋章列表</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_JAMMER%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_JAMMER) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_JAMMER%></td>
                   <td>界面</td>
                   <td>干扰码字符串[同步之后将会使用新的干扰码]</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE%></td>
                   <td>界面</td>
                   <td>附件类型列表[DataTable方式]</td>                  
                </tr>
                <!-- UI END-->
                
                
                <!-- SET START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ADMIN_GROUP_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ADMIN_GROUP_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ADMIN_GROUP_LIST%></td>
                   <td>设置</td>
                   <td>管理组信息</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_USER_GROUP_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_USER_GROUP_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_USER_GROUP_LIST%></td>
                   <td>设置</td>
                   <td>用户组数据信息</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_MODERATOR_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_MODERATOR_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_MODERATOR_LIST%></td>
                   <td>设置</td>
                   <td>版主信息</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ANNOUNCEMENT_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ANNOUNCEMENT_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ANNOUNCEMENT_LIST%></td>
                   <td>设置</td>
                   <td>公告列表</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST%></td>
                   <td>设置</td>
                   <td>指定时间段内的前n条公告列表</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ONLINE_ICON_TABLE%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ONLINE_ICON_TABLE) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ONLINE_ICON_TABLE%></td>
                   <td>设置</td>
                   <td>在线用户图例</td>                  
                </tr>  
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_LINK_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_LINK_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_LINK_LIST%></td>
                   <td>设置</td>
                   <td>友情链接列表</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_BAN_WORD_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_BAN_WORD_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_BAN_WORD_LIST%></td>
                   <td>设置</td>
                   <td>脏字过滤列表</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_LIST%></td>
                   <td>设置</td>
                   <td>全部版块列表</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_TEMPLATE_ID_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_TEMPLATE_ID_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_TEMPLATE_ID_LIST%></td>
                   <td>设置</td>
                   <td>前台有效的模板ID列表</td>                  
                </tr>
              <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_LAST_POST_TABLE_NAME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_LAST_POST_TABLE_NAME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_LAST_POST_TABLE_NAME%></td>
                   <td>设置</td>
                   <td></td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_POST_TABLE_NAME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_POST_TABLE_NAME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_POST_TABLE_NAME%></td>
                   <td>设置</td>
                   <td></td>                  
                </tr>         --%>        
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ADVERTISEMENTS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ADVERTISEMENTS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ADVERTISEMENTS%></td>
                   <td>设置</td>
                   <td>广告列表信息</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_STATISTICS_SEARCHTIME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_STATISTICS_SEARCHTIME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_STATISTICS_SEARCHTIME%></td>
                   <td>设置</td>
                   <td>上一次执行搜索操作的时间</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_STATISTICS_SEARCHCOUNT%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_STATISTICS_SEARCHCOUNT) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_STATISTICS_SEARCHCOUNT%></td>
                   <td>设置</td>
                   <td>用户在一分钟内搜索的次数</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_COMMON_AVATAR_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_COMMON_AVATAR_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_COMMON_AVATAR_LIST%></td>
                   <td>设置</td>
                   <td>自带头像列表</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_MAGIC_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_MAGIC_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_MAGIC_LIST%></td>
                   <td>设置</td>
                   <td>魔力列表<!-该键值目前可能已被取消-></td>                  
                </tr>
              
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_LIST_MENU_DIV%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_LIST_MENU_DIV) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_LIST_MENU_DIV%></td>
                   <td>设置</td>
                   <td>版块列表弹出菜单</td>                  
                </tr>
                
                
                <!-- SET END-->
                  
                <!-- SCORESET START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORE_PAY_SET%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORE_PAY_SET) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORE_PAY_SET%></td>
                   <td>积分</td>
                   <td>具有兑换比率的可交易积分策略</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_CREDITS_TAX%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_CREDITS_TAX) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_CREDITS_TAX%></td>
                   <td>积分</td>
                   <td>积分交易税</td>                  
                </tr>
               <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_CREDITS_TRANS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_CREDITS_TRANS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_CREDITS_TRANS%></td>
                   <td>积分</td>
                   <td></td>                  
                </tr>--%>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RATESCORESET%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RATESCORESET) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RATESCORESET%></td>
                   <td>积分</td>
                   <td>评分操作专用的积分策略</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_VALID_SCORE_NAME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_VALID_SCORE_NAME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_VALID_SCORE_NAME%></td>
                   <td>积分</td>
                   <td>前台可以使用的扩展字段名和显示名称</td>                  
                </tr>
              <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD%></td>
                   <td>积分</td>
                   <td></td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN%></td>
                   <td>积分</td>
                   <td></td>                  
                </tr>--%>
               <!-- SCORESET END-->
               
               <!-- RSS START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RSS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RSS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RSS%></td>
                   <td>RSS</td>
                   <td>论坛整体RSS及指定版块RSS</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RSS_INDEX%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RSS_INDEX) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RSS_INDEX%></td>
                   <td>RSS</td>
                   <td>论坛整体RSS</td>                  
                </tr>
               <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RSS_FORUM%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RSS_FORUM) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RSS_FORUM%></td>
                   <td>RSS</td>
                   <td>参数forumid</td>                  
                </tr>--%>
                <!-- RSS END-->
             </table>
            </div>
        <%} %>
     
       
        <%if (ViewData["OpInfo"]!= null)
        {%>
           <h3 style="color:red">操作结果:<%=Html.Encode(ViewData["OpInfo"])%></h3>
        <%} %>

    <script language="javascript">
         function CheckOption() {
             var elementarr = $("input[@type=checkbox]:checked");
             var intvalue = '';
             $.each(elementarr, function(n, element) {
                 if (element.name == 'cacheKey') {
                     if (intvalue == '')
                         intvalue = element.value;
                     else
                         intvalue = intvalue + ',' + element.value;
                 }
             });
             //alert(intvalue);
             $('#cacheKeyArray').attr('value', intvalue);
         }


         $("#checkedAll").click( 
            function(){ 
                 if(this.checked){
                     $("input[name='cacheKey']").each(function() {
                         this.checked = true; 
                     }); 
                 }else{
                     $("input[name='cacheKey']").each(function() {
                         this.checked = false; 
                     }); 
                 } 
             } 
         );  
    </script>
    

   <fieldset>
        <legend>环境配置步骤</legend>   
        1.要开启监控，请将memcached.config（服务端）文件中节点设置如下:<br/>
          &nbsp;<%=Html.TextArea("memcachedconfig", "<ApplyMemCached>true</ApplyMemCached>", new { rows=1, cols=100 })%><br/><br/>
          &nbsp;&nbsp;&nbsp;同时loadbalance.config（服务端）文件中节点设置如下:<br/>
          &nbsp;<%=Html.TextArea("loadbalance", "<AppLoadBalance>true</AppLoadBalance>\r\n<SiteUrl>填写负载均衡工具的链接地址(以','分割),形如:\r\n    http://10.0.2.137:8088/tools/,\r\n    http://10.0.2.150:8088/tools/\r\n</SiteUrl>\r\n<AuthCode>您的认证码信息</AuthCode>", new { rows = 7, cols = 100 })%><br/><br/>

        2.将Discuz.Web/tools/synclocalcache.ashx和Discuz.EntLib.ToolKit.dll文件复制到服务端
    </fieldset>
</asp:Content>
