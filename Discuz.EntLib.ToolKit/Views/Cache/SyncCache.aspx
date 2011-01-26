<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Discuz.Cache" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Encode(ViewData["Title"])%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>����ͬ��</h2>
    
      <% using (Html.BeginForm("SyncCache", "Cache"))
         { %>
            <div>
             <%=Html.Hidden("cacheKeyArray")%>
             <input type="checkbox" name="checkedAll" id="checkedAll" />ȫѡ/ȡ��ȫѡ
             <input type="submit" value="ͬ��ָ����ֵ�Ļ�����Ϣ" onclick="CheckOption();"/><br/><br/>
             <table width="100%">
                <th style="width:5%">ѡ��</th><th style="width:30%">��ֵ</th><th>����</th><th>˵��</th>
                <!-- UI START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS%></td>
                   <td>����</td>
                   <td>��̳�ײ���������б�</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO%></td>
                   <td>����</td>
                   <td>�����б���Ϣ</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST%></td>
                   <td>����</td>
                   <td>�û��Զ����ǩ[JavaScript��������]</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO%></td>
                   <td>����</td>
                   <td>�û��Զ����ǩ[CustomEditorButtonInfo������ʽ]</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_SMILIES_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_SMILIES_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_SMILIES_LIST%></td>
                   <td>����</td>
                   <td>�������JSON����</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_ICONS_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_ICONS_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_ICONS_LIST%></td>
                   <td>����</td>
                   <td>Icons����ͼ������</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS%></td>
                   <td>����</td>
                   <td>ģ���б��������</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_ONLINE_ICON_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_ONLINE_ICON_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_ONLINE_ICON_LIST%></td>
                   <td>����</td>
                   <td>�����û��б�ͼ��</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_MEDALS_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_MEDALS_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_MEDALS_LIST%></td>
                   <td>����</td>
                   <td>ѫ���б�</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_UI_JAMMER%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_UI_JAMMER) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_UI_JAMMER%></td>
                   <td>����</td>
                   <td>�������ַ���[ͬ��֮�󽫻�ʹ���µĸ�����]</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE%></td>
                   <td>����</td>
                   <td>���������б�[DataTable��ʽ]</td>                  
                </tr>
                <!-- UI END-->
                
                
                <!-- SET START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ADMIN_GROUP_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ADMIN_GROUP_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ADMIN_GROUP_LIST%></td>
                   <td>����</td>
                   <td>��������Ϣ</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_USER_GROUP_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_USER_GROUP_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_USER_GROUP_LIST%></td>
                   <td>����</td>
                   <td>�û���������Ϣ</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_MODERATOR_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_MODERATOR_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_MODERATOR_LIST%></td>
                   <td>����</td>
                   <td>������Ϣ</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ANNOUNCEMENT_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ANNOUNCEMENT_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ANNOUNCEMENT_LIST%></td>
                   <td>����</td>
                   <td>�����б�</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST%></td>
                   <td>����</td>
                   <td>ָ��ʱ����ڵ�ǰn�������б�</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ONLINE_ICON_TABLE%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ONLINE_ICON_TABLE) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ONLINE_ICON_TABLE%></td>
                   <td>����</td>
                   <td>�����û�ͼ��</td>                  
                </tr>  
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_LINK_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_LINK_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_LINK_LIST%></td>
                   <td>����</td>
                   <td>���������б�</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_BAN_WORD_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_BAN_WORD_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_BAN_WORD_LIST%></td>
                   <td>����</td>
                   <td>���ֹ����б�</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_LIST%></td>
                   <td>����</td>
                   <td>ȫ������б�</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_TEMPLATE_ID_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_TEMPLATE_ID_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_TEMPLATE_ID_LIST%></td>
                   <td>����</td>
                   <td>ǰ̨��Ч��ģ��ID�б�</td>                  
                </tr>
              <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_LAST_POST_TABLE_NAME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_LAST_POST_TABLE_NAME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_LAST_POST_TABLE_NAME%></td>
                   <td>����</td>
                   <td></td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_POST_TABLE_NAME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_POST_TABLE_NAME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_POST_TABLE_NAME%></td>
                   <td>����</td>
                   <td></td>                  
                </tr>         --%>        
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_ADVERTISEMENTS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_ADVERTISEMENTS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_ADVERTISEMENTS%></td>
                   <td>����</td>
                   <td>����б���Ϣ</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_STATISTICS_SEARCHTIME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_STATISTICS_SEARCHTIME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_STATISTICS_SEARCHTIME%></td>
                   <td>����</td>
                   <td>��һ��ִ������������ʱ��</td>                  
                </tr>
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_STATISTICS_SEARCHCOUNT%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_STATISTICS_SEARCHCOUNT) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_STATISTICS_SEARCHCOUNT%></td>
                   <td>����</td>
                   <td>�û���һ�����������Ĵ���</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_COMMON_AVATAR_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_COMMON_AVATAR_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_COMMON_AVATAR_LIST%></td>
                   <td>����</td>
                   <td>�Դ�ͷ���б�</td>                  
                </tr>                 
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_MAGIC_LIST%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_MAGIC_LIST) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_MAGIC_LIST%></td>
                   <td>����</td>
                   <td>ħ���б�<!-�ü�ֵĿǰ�����ѱ�ȡ��-></td>                  
                </tr>
              
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_FORUM_LIST_MENU_DIV%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_FORUM_LIST_MENU_DIV) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_FORUM_LIST_MENU_DIV%></td>
                   <td>����</td>
                   <td>����б����˵�</td>                  
                </tr>
                
                
                <!-- SET END-->
                  
                <!-- SCORESET START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORE_PAY_SET%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORE_PAY_SET) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORE_PAY_SET%></td>
                   <td>����</td>
                   <td>���жһ����ʵĿɽ��׻��ֲ���</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_CREDITS_TAX%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_CREDITS_TAX) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_CREDITS_TAX%></td>
                   <td>����</td>
                   <td>���ֽ���˰</td>                  
                </tr>
               <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_CREDITS_TRANS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_CREDITS_TRANS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_CREDITS_TRANS%></td>
                   <td>����</td>
                   <td></td>                  
                </tr>--%>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RATESCORESET%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RATESCORESET) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RATESCORESET%></td>
                   <td>����</td>
                   <td>���ֲ���ר�õĻ��ֲ���</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_VALID_SCORE_NAME%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_VALID_SCORE_NAME) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_VALID_SCORE_NAME%></td>
                   <td>����</td>
                   <td>ǰ̨����ʹ�õ���չ�ֶ�������ʾ����</td>                  
                </tr>
              <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD%></td>
                   <td>����</td>
                   <td></td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN%></td>
                   <td>����</td>
                   <td></td>                  
                </tr>--%>
               <!-- SCORESET END-->
               
               <!-- RSS START-->
                <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RSS%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RSS) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RSS%></td>
                   <td>RSS</td>
                   <td>��̳����RSS��ָ�����RSS</td>                  
                </tr>
                 <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RSS_INDEX%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RSS_INDEX) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RSS_INDEX%></td>
                   <td>RSS</td>
                   <td>��̳����RSS</td>                  
                </tr>
               <%--  <tr>
                   <td><input type="checkbox" name="cacheKey" value="<%=CacheKeys.FORUM_RSS_FORUM%>" <%if(ViewData["CacheKey"]!= null && (ViewData["CacheKey"] as string) == CacheKeys.FORUM_RSS_FORUM) {%>checked<%}%> /></td>
                   <td><%=CacheKeys.FORUM_RSS_FORUM%></td>
                   <td>RSS</td>
                   <td>����forumid</td>                  
                </tr>--%>
                <!-- RSS END-->
             </table>
            </div>
        <%} %>
     
       
        <%if (ViewData["OpInfo"]!= null)
        {%>
           <h3 style="color:red">�������:<%=Html.Encode(ViewData["OpInfo"])%></h3>
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
        <legend>�������ò���</legend>   
        1.Ҫ������أ��뽫memcached.config������ˣ��ļ��нڵ���������:<br/>
          &nbsp;<%=Html.TextArea("memcachedconfig", "<ApplyMemCached>true</ApplyMemCached>", new { rows=1, cols=100 })%><br/><br/>
          &nbsp;&nbsp;&nbsp;ͬʱloadbalance.config������ˣ��ļ��нڵ���������:<br/>
          &nbsp;<%=Html.TextArea("loadbalance", "<AppLoadBalance>true</AppLoadBalance>\r\n<SiteUrl>��д���ؾ��⹤�ߵ����ӵ�ַ(��','�ָ�),����:\r\n    http://10.0.2.137:8088/tools/,\r\n    http://10.0.2.150:8088/tools/\r\n</SiteUrl>\r\n<AuthCode>������֤����Ϣ</AuthCode>", new { rows = 7, cols = 100 })%><br/><br/>

        2.��Discuz.Web/tools/synclocalcache.ashx��Discuz.EntLib.ToolKit.dll�ļ����Ƶ������
    </fieldset>
</asp:Content>
