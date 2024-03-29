<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.postreply" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2010/12/28 17:13:00.
		本页面代码由Discuz!NT模板引擎生成于 2010/12/28 17:13:00. 
	*/

	base.OnInit(e);

	templateBuilder.Capacity = 220000;



	if (infloat!=1)
	{

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
	templateBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
	templateBuilder.Append("<head>\r\n");
	templateBuilder.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n");

	if (pagetitle=="首页")
	{

	templateBuilder.Append("<title>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n");

	}
	else
	{

	templateBuilder.Append("<title>");
	templateBuilder.Append(pagetitle.ToString());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n");

	}	//end if
	templateBuilder.Append(meta.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("<meta name=\"generator\" content=\"Discuz!NT 3.5.0\" />\r\n");
	templateBuilder.Append("<meta name=\"author\" content=\"Discuz!NT Team and Comsenz UI Team\" />\r\n");
	templateBuilder.Append("<meta name=\"copyright\" content=\"2001-2010 Comsenz Inc.\" />\r\n");
	templateBuilder.Append("<meta http-equiv=\"x-ua-compatible\" content=\"ie=7\" />\r\n");
	templateBuilder.Append("<link rel=\"icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n");
	templateBuilder.Append("<link rel=\"shortcut icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n");

	if (pagename!="website.aspx")
	{

	templateBuilder.Append("<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/dnt.css\" type=\"text/css\" media=\"all\" />\r\n");

	}	//end if

	templateBuilder.Append("<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/float.css\" type=\"text/css\" />\r\n");

	if (Utils.GetCookie("widthauto")=="0"&&pagename!="website.aspx")
	{

	templateBuilder.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/widthauto.css\" id=\"css_widthauto\">\r\n");

	}	//end if
	templateBuilder.Append(link.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("var creditnotice='");
	templateBuilder.Append(Scoresets.GetValidScoreNameAndId().ToString().Trim());
	templateBuilder.Append("';	\r\n");
	templateBuilder.Append("var forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/common.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_report.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_utils.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	var aspxrewrite = ");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("	var IMGDIR = '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("    var disallowfloat = '");
	templateBuilder.Append(config.Disallowfloatwin.ToString().Trim());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("	var rooturl=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("\";\r\n");
	templateBuilder.Append("	var imagemaxwidth='");
	templateBuilder.Append(Templates.GetTemplateWidth(templatepath).ToString().Trim());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("	var cssdir='");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append(script.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("</head>\r\n");


	templateBuilder.Append("<body onkeydown=\"if(event.keyCode==27) return false;\">\r\n");
	templateBuilder.Append("<div id=\"append_parent\"></div><div id=\"ajaxwaitid\"></div>\r\n");

	if (headerad!="")
	{

	templateBuilder.Append("	<div id=\"ad_headerbanner\">");
	templateBuilder.Append(headerad.ToString());
	templateBuilder.Append("</div>\r\n");

	}	//end if

	templateBuilder.Append("<div id=\"hd\">\r\n");
	templateBuilder.Append("	<div class=\"wrap\">\r\n");
	templateBuilder.Append("		<div class=\"head cl\">\r\n");
	templateBuilder.Append("			<h2><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\" title=\"Discuz!NT|BBS|论坛\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/logo.png\" alt=\"Discuz!NT|BBS|论坛\"/></a></h2>\r\n");

	if (userid==-1)
	{


	if (pagename!="login.aspx"&&pagename!="register.aspx")
	{

	templateBuilder.Append("			<form onsubmit=\"if ($('ls_username').value == '' || $('ls_username').value == '用户名/Email') showWindow('login', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');return\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?referer=");
	templateBuilder.Append(pagename.ToString());
	templateBuilder.Append("\" id=\"lsform\" autocomplete=\"off\" method=\"post\">\r\n");
	templateBuilder.Append("				<div class=\"fastlg c1\">\r\n");
	templateBuilder.Append("					<div class=\"y pns\">\r\n");
	templateBuilder.Append("						<p>\r\n");
	templateBuilder.Append("							<label for=\"ls_username\">帐号</label> <input type=\"text\" tabindex=\"901\" value=\"用户名/Email\" id=\"ls_username\" name=\"username\" class=\"txt\" onblur=\"if(this.value == '') this.value = '用户名/Email';\" onfocus=\"if(this.value == '用户名/Email') this.value = '';\"/><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"showWindow('register', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx');hideWindow('login');\" style=\"margin-left: 7px;\" class=\"xg2\">注册</a>							\r\n");
	templateBuilder.Append("						</p>\r\n");
	templateBuilder.Append("						<p>\r\n");
	templateBuilder.Append("							<label for=\"ls_password\">密码</label> <input type=\"password\" onfocus=\"lsShowmore();innerVcode();\" tabindex=\"902\" autocomplete=\"off\" id=\"ls_password\" name=\"password\"  class=\"txt\"/>\r\n");
	templateBuilder.Append("							&nbsp;<input type=submit style=\"width:0px;filter:alpha(opacity=0);-moz-opacity:0;opacity:0;display:none;\"/><button class=\"pn\" type=\"submit\"><span>登录</span></button>\r\n");
	templateBuilder.Append("						</p>\r\n");
	templateBuilder.Append("					</div>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("                <div id=\"ls_more\" style=\"position:absolute;display:none;\">\r\n");
	templateBuilder.Append("                <h3 class=\"cl\"><em class=\"y\"><a href=\"###\" class=\"flbc\" title=\"关闭\" onclick=\"closeIsMore();return false;\">关闭</a></em>安全选项</h3>\r\n");

	if (isLoginCode)
	{

	templateBuilder.Append("                <div id=\"vcode_header\"></div>\r\n");
	templateBuilder.Append("                <script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	                if (typeof vcodeimgid == 'undefined'){\r\n");
	templateBuilder.Append("	                    var vcodeimgid = 1;\r\n");
	templateBuilder.Append("	                }\r\n");
	templateBuilder.Append("	                else\r\n");
	templateBuilder.Append("	                    vcodeimgid++;\r\n");
	templateBuilder.Append("	                function innerVcode() {\r\n");
	templateBuilder.Append("	                    if ($('vcodetext1') == null) {\r\n");
	templateBuilder.Append("	                        $('vcode_header').innerHTML = '<input name=\"vcodetext\" tabindex=\"903\" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:50px;\" id=\"vcodetext' + vcodeimgid + '\" value=\"\" autocomplete=\"off\"/>' +\r\n");
	templateBuilder.Append("                                                        '<span><a href=\"###\" onclick=\"vcodeimg' + vcodeimgid + '.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();return false;\" style=\"margin-left: 7px;\">看不清</a></span>' + '<p style=\"margin:6px 0\">输入下图中的字符</p>' +\r\n");
	templateBuilder.Append("	                                                    '<div  style=\"cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n");
	templateBuilder.Append("	                        optionVcode();\r\n");
	templateBuilder.Append("                            }\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("	                 function changevcode(form, value){\r\n");
	templateBuilder.Append("		                if (!$('vcode')){\r\n");
	templateBuilder.Append("			                var vcode = document.createElement('input');\r\n");
	templateBuilder.Append("			                vcode.id = 'vcode';\r\n");
	templateBuilder.Append("			                vcode.name = 'vcode';\r\n");
	templateBuilder.Append("			                vcode.type = 'hidden';\r\n");
	templateBuilder.Append("			                vcode.value = value;\r\n");
	templateBuilder.Append("			                form.appendChild(vcode);\r\n");
	templateBuilder.Append("		                }else{\r\n");
	templateBuilder.Append("			                $('vcode').value = value;\r\n");
	templateBuilder.Append("		                }\r\n");
	templateBuilder.Append("	                }\r\n");
	templateBuilder.Append("                </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("                <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                    var secclick = new Array();\r\n");
	templateBuilder.Append("                    var seccodefocus = 0;\r\n");
	templateBuilder.Append("                    var optionVcode = function (id, type) {\r\n");
	templateBuilder.Append("                        id = vcodeimgid;\r\n");
	templateBuilder.Append("                        if ($('vcode')) {\r\n");
	templateBuilder.Append("                            $('vcode').parentNode.removeChild($('vcode'));\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                        if (!secclick['vcodetext' + id]) {\r\n");
	templateBuilder.Append("                            if ($('vcodetext' + id) != null)\r\n");
	templateBuilder.Append("                                $('vcodetext' + id).value = '';\r\n");
	templateBuilder.Append("                            secclick['vcodetext' + id] = 1;\r\n");
	templateBuilder.Append("                            if (type)\r\n");
	templateBuilder.Append("                                $('vcodetext' + id + '_menu').style.top = parseInt($('vcodetext' + id + '_menu').style.top) - parseInt($('vcodetext' + id + '_menu').style.height) + 'px';\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                        $('vcodetext' + id + '_menu').style.display = '';\r\n");
	templateBuilder.Append("                        $('vcodetext' + id).unselectable = 'off';\r\n");
	templateBuilder.Append("                        $('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n");
	templateBuilder.Append("                    }\r\n");
	templateBuilder.Append("                </");
	templateBuilder.Append("script>\r\n");

	}
	else
	{

	templateBuilder.Append("                    <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                        function innerVcode() {\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                        function optionVcode() {\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                    </");
	templateBuilder.Append("script>\r\n");

	}	//end if


	if (config.Secques==1)
	{

	templateBuilder.Append("			    <div id=\"floatlayout_login\" class=\"pbm\">\r\n");
	templateBuilder.Append("					<select style=\"width:156px;margin-bottom:8px;\" id=\"question\" name=\"question\" selecti=\"5\" name=\"question\" onchange=\"displayAnswer();\" tabindex=\"904\">\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"0\" selected=\"selected\">安全提问(未设置请忽略)</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"1\">母亲的名字</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"2\">爷爷的名字</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"3\">父亲出生的城市</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"4\">您其中一位老师的名字</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"5\">您个人计算机的型号</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"6\">您最喜欢的餐馆名称</option>\r\n");
	templateBuilder.Append("						<option id=\"question\" value=\"7\">驾驶执照的最后四位数字</option>\r\n");
	templateBuilder.Append("					</select>\r\n");
	templateBuilder.Append("					<input type=\"text\" tabindex=\"905\" class=\"txt\" size=\"20\" autocomplete=\"off\" style=\"width:140px;display:none;\"  id=\"answer\" name=\"answer\"/>\r\n");
	templateBuilder.Append("		        </div>\r\n");

	}	//end if

	templateBuilder.Append("                <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                    function closeIsMore() {\r\n");
	templateBuilder.Append("                        $('ls_more').style.display = 'none';\r\n");
	templateBuilder.Append("                    }\r\n");
	templateBuilder.Append("                    function displayAnswer() {\r\n");
	templateBuilder.Append("                        $('answer').style.display = '';\r\n");
	templateBuilder.Append("						$('answer').focus();\r\n");
	templateBuilder.Append("                    }\r\n");
	templateBuilder.Append("                </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("				<div class=\"ptm cl\" style=\"border-top:1px dashed #CDCDCD;\">\r\n");
	templateBuilder.Append("					<a class=\"y xg2\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("getpassword.aspx\" onclick=\"hideWindow('register');hideWindow('login');showWindow('getpassword', this.href);\">找回密码</a>\r\n");
	templateBuilder.Append("					<label class=\"z\" for=\"ls_cookietime\"><input type=\"checkbox\" tabindex=\"906\" value=\"2592000\" id=\"ls_cookietime\" name=\"expires\" checked=\"checked\" tabindex=\"906\">记住密码</label>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("            </div>\r\n");
	templateBuilder.Append("			</form>\r\n");

	}	//end if


	}
	else
	{

	templateBuilder.Append("			<div id=\"um\">\r\n");
	templateBuilder.Append("				<div class=\"avt y\"><a alt=\"用户名称\" target=\"_blank\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\"><img src=\"");
	templateBuilder.Append(useravatar.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\" /></a></div>\r\n");
	templateBuilder.Append("				<p>\r\n");
	templateBuilder.Append("					<strong><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("userinfo.aspx?userid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("\" class=\"vwmy\">");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</a></strong><span class=\"xg1\">在线</span><span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<a id=\"pm_ntc\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\" title=\"\r\n");

	if (oluserinfo.Newpms>0)
	{

	templateBuilder.Append("您有");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("条新短消息\r\n");

	}
	else
	{

	templateBuilder.Append("您没有新短消息\r\n");

	}	//end if

	templateBuilder.Append("\">短消息</a><span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnotice.aspx?filter=all\" title=\"\r\n");

	if (oluserinfo.Newnotices>0)
	{

	templateBuilder.Append("您有");
	templateBuilder.Append(oluserinfo.Newnotices.ToString().Trim());
	templateBuilder.Append("条新通知\r\n");

	}
	else
	{

	templateBuilder.Append("您没有新通知\r\n");

	}	//end if

	templateBuilder.Append("\">通知\r\n");

	if (oluserinfo.Newnotices>0)
	{

	templateBuilder.Append("(");
	templateBuilder.Append(oluserinfo.Newnotices.ToString().Trim());
	templateBuilder.Append(")\r\n");

	}	//end if

	templateBuilder.Append("</a><span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<a id=\"usercenter\" class=\"drop\" onmouseover=\"showMenu(this.id);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户中心</a>\r\n");

	if (config.Regstatus==2||config.Regstatus==3)
	{


	if (userid>0)
	{

	templateBuilder.Append("					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("invite.aspx\">邀请</a>\r\n");

	}	//end if


	}	//end if


	if (useradminid==1)
	{

	templateBuilder.Append("					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("admin/index.aspx\" target=\"_blank\">系统设置</a>\r\n");

	}	//end if

	templateBuilder.Append("					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("logout.aspx?userkey=");
	templateBuilder.Append(userkey.ToString());
	templateBuilder.Append("\">退出</a>\r\n");
	templateBuilder.Append("				</p>\r\n");
	templateBuilder.Append("				");
	templateBuilder.Append(userinfotips.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("			</div> \r\n");

	if (oluserinfo.Newpms>0)
	{

	templateBuilder.Append("			<div id=\"pm_ntc_menu\" class=\"g_up\">\r\n");
	templateBuilder.Append("				<div class=\"mncr\"></div>\r\n");
	templateBuilder.Append("				<div class=\"crly\">\r\n");
	templateBuilder.Append("					<div style=\"clear:both;font-size:0;\"></div>\r\n");
	templateBuilder.Append("					<span class=\"y\"><a onclick=\"javascript:$('pm_ntc_menu').style.display='none';\" href=\"javascript:;\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/delete.gif\" alt=\"关闭\"/></a></span>\r\n");
	templateBuilder.Append("					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\">您有");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("条新消息</a>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("            <script type=\"text/javascript\">setMenuPosition('pm_ntc', 'pm_ntc_menu', '43');</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div id=\"menubar\">\r\n");

	if (userid!=-1)
	{

	templateBuilder.Append("			<a onMouseOver=\"showMenu(this.id, false);\" href=\"javascript:void(0);\" id=\"mymenu\">我的中心</a>\r\n");
	templateBuilder.Append("            <div class=\"popupmenu_popup headermenu_popup\" id=\"mymenu_menu\" style=\"display: none\">\r\n");
	templateBuilder.Append("			<ul class=\"sel_my\">\r\n");
	templateBuilder.Append("				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("mytopics.aspx\">我的主题</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("myposts.aspx\">我的帖子</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("search.aspx?posterid=current&type=digest&searchsubmit=1\">我的精华</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("myattachment.aspx\">我的附件</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpsubscribe.aspx\">我的收藏</a></li>\r\n");

	if (config.Enablespace==1)
	{

	templateBuilder.Append("				<li class=\"myspace\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("space/\">我的空间</a></li>\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("				<li class=\"myalbum\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showalbumlist.aspx?uid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("\">我的相册</a></li>\r\n");

	}	//end if

	templateBuilder.Append("            </ul>\r\n");

	if (pagename!="website.aspx")
	{

	templateBuilder.Append("            <ul class=\"sel_mb\">\r\n");

	if (Utils.GetCookie("widthauto")=="0")
	{

	templateBuilder.Append("				<li><a href=\"javascript:;\" onclick=\"widthauto(this,'");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("')\">切换到宽版</a></li>\r\n");

	}
	else
	{

	templateBuilder.Append("				<li><a href=\"javascript:;\" onclick=\"widthauto(this,'");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("')\">切换到窄版</a></li>\r\n");

	}	//end if

	templateBuilder.Append("			</ul>\r\n");

	}	//end if

	templateBuilder.Append("            </div>\r\n");

	}	//end if

	templateBuilder.Append("			<ul id=\"menu\" class=\"cl\">\r\n");
	templateBuilder.Append("				");
	templateBuilder.Append(mainnavigation.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	}
	else
	{


	Response.Clear();
	Response.ContentType = "Text/XML";
	Response.Expires = 0;
	Response.Cache.SetNoStore();
	
	templateBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><root><![CDATA[\r\n");

	}	//end if



	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/bbcode.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/editor.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_calendar.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<div class=\"wrap cl pageinfo\">\r\n");
	templateBuilder.Append("	<div id=\"nav\">\r\n");

	if (usergroupinfo.Allowsearch>0)
	{


	templateBuilder.Append("<form method=\"post\" action=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("search.aspx\" target=\"_blank\" onsubmit=\"bind_keyword(this);\" class=\"y\">\r\n");
	templateBuilder.Append("	<input type=\"hidden\" name=\"poster\" />\r\n");
	templateBuilder.Append("	<input type=\"hidden\" name=\"keyword\" />\r\n");
	templateBuilder.Append("	<input type=\"hidden\" name=\"type\" value=\"\" />\r\n");
	templateBuilder.Append("	<input id=\"keywordtype\" type=\"hidden\" name=\"keywordtype\" value=\"0\" />\r\n");
	templateBuilder.Append("	<a href=\"javascript:void(0);\" class=\"drop s_type\" id=\"quicksearch\" onclick=\"showMenu(this.id, false);\" onmouseover=\"MouseCursor(this);\">快速搜索</a>\r\n");
	templateBuilder.Append("	<input type=\"text\" name=\"keywordf\" value=\"输入搜索关键字\" onblur=\"if(this.value=='')this.value=defaultValue\" onclick=\"if(this.value==this.defaultValue)this.value = ''\" onkeydown=\"if(this.value==this.defaultValue)this.value = ''\" class=\"txt\"/>\r\n");
	templateBuilder.Append("	<input name=\"searchsubmit\" type=\"submit\" value=\"\" class=\"btnsearch\"/>\r\n");
	templateBuilder.Append("</form>\r\n");
	templateBuilder.Append("<ul id=\"quicksearch_menu\" class=\"p_pop\" style=\"display: none;\">\r\n");
	templateBuilder.Append("	<li><a href=\"###\" onclick=\"$('keywordtype').value='0';$('quicksearch').innerHTML='帖子标题';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">帖子标题</a></li>\r\n");

	if (config.Enablespace==1)
	{

	templateBuilder.Append("	<li><a href=\"###\" onclick=\"$('keywordtype').value='2';$('quicksearch').innerHTML='空间日志';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">空间日志</a></li>\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("	<li><a href=\"###\" onclick=\"$('keywordtype').value='3';$('quicksearch').innerHTML='相册标题';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">相册标题</a></li>\r\n");

	}	//end if

	templateBuilder.Append("	<li><a href=\"###\" onclick=\"$('keywordtype').value='8';$('quicksearch').innerHTML='作者';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">作者</a></li>\r\n");
	templateBuilder.Append("	<li><a href=\"###\" onclick=\"$('keywordtype').value='9';$('quicksearch').innerHTML='版块';$('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">版块</a></li>\r\n");
	templateBuilder.Append("</ul>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("    function bind_keyword(form) {\r\n");
	templateBuilder.Append("        if (form.keywordtype.value == '9') {\r\n");
	templateBuilder.Append("            form.action = '");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("forumsearch.aspx?q=' + escape(form.keywordf.value);\r\n");
	templateBuilder.Append("        } else if (form.keywordtype.value == '8') {\r\n");
	templateBuilder.Append("            form.keyword.value = '';\r\n");
	templateBuilder.Append("            form.poster.value = form.keywordf.value != form.keywordf.defaultValue ? form.keywordf.value : '';\r\n");
	templateBuilder.Append("        } else {\r\n");
	templateBuilder.Append("            form.poster.value = '';\r\n");
	templateBuilder.Append("            form.keyword.value = form.keywordf.value != form.keywordf.defaultValue ? form.keywordf.value : '';\r\n");
	templateBuilder.Append("            if (form.keywordtype.value == '2')\r\n");
	templateBuilder.Append("                form.type.value = 'spacepost';\r\n");
	templateBuilder.Append("            if (form.keywordtype.value == '3')\r\n");
	templateBuilder.Append("                form.type.value = 'album';\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");



	}	//end if

	templateBuilder.Append("		<a href=\"");
	templateBuilder.Append(config.Forumurl.ToString().Trim());
	templateBuilder.Append("\" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a>  \r\n");

	if (forum.Pathlist!="")
	{

	templateBuilder.Append("&raquo;");
	templateBuilder.Append(ShowForumAspxRewrite(forum.Pathlist.Trim(),forumid,forumpageid).ToString().Trim());
	templateBuilder.Append(" &raquo; \r\n");

	}	//end if

	templateBuilder.Append("		<a href=\"");
	templateBuilder.Append(ShowTopicAspxRewrite(topicid,0).ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(topictitle.ToString());
	templateBuilder.Append("</a> &raquo; <strong>回复主题</strong>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" reload=\"1\" >\r\n");
	templateBuilder.Append("    var postminchars = parseInt(");
	templateBuilder.Append(config.Minpostsize.ToString().Trim());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("    var postmaxchars = parseInt(");
	templateBuilder.Append(config.Maxpostsize.ToString().Trim());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("    var disablepostctrl = parseInt(");
	templateBuilder.Append(disablepost.ToString());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("    var forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n");
	templateBuilder.Append("    var posturl=forumpath+'postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("    var postaction='postreply.aspx?infloat=1&topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&';\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");

	if (page_err==0)
	{


	if (ispost)
	{


	templateBuilder.Append("<div class=\"wrap s_clear\" id=\"wrap\">\r\n");
	templateBuilder.Append("<div class=\"main\">\r\n");
	templateBuilder.Append("	<div class=\"msgbox\">\r\n");
	templateBuilder.Append("		<h1>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("　提示信息</h1>\r\n");
	templateBuilder.Append("		<hr class=\"solidline\"/>\r\n");
	templateBuilder.Append("		<div class=\"msg_inner\">\r\n");
	templateBuilder.Append("			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");

	if (msgbox_url!="")
	{

	templateBuilder.Append("			<p><a href=\"");
	templateBuilder.Append(msgbox_url.ToString());
	templateBuilder.Append("\">如果浏览器没有转向, 请点击这里.</a></p>\r\n");

	}	//end if

	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</div>\r\n");



	}
	else
	{

	templateBuilder.Append("<div class=\"wrap cl post\">\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	    function geteditormessage(theform)\r\n");
	templateBuilder.Append("	    {\r\n");
	templateBuilder.Append("	        var message = wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(theform.message.value) : theform.message.value);\r\n");
	templateBuilder.Append("	        theform.message.value = message;\r\n");
	templateBuilder.Append("	    }\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	<form method=\"post\" name=\"postform\" id=\"postform\" action=\"\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this)\">\r\n");

	templateBuilder.Append("<div id=\"editorbox\">\r\n");
	templateBuilder.Append("	<div class=\"edt_main\">\r\n");
	templateBuilder.Append("	<div class=\"edt_content cl\">\r\n");
	string special = DNTRequest.GetString("type").ToLower();;
	

	if (special=="" && topic.Special>0)
	{


	if (topic.Special==1)
	{

	 special = "poll";
	

	}	//end if


	if (topic.Special==2 || topic.Special==3)
	{

	 special = "bonus";
	

	}	//end if


	if (topic.Special==4)
	{

	 special = "debate";
	

	}	//end if


	}	//end if

	bool adveditor = (special!="" || topic.Special>0)&&isfirstpost;
	
	string action = pagename.Replace("post","").Replace(".aspx","").Replace("topic","newthread");
	
	string actiontitle = "";
	

	if (pagename=="posttopic.aspx")
	{


	if (special=="bonus")
	{

	 actiontitle = "发布悬赏";
	

	}
	else if (special=="poll")
	{

	 actiontitle = "发布投票";
	

	}
	else if (special=="debate")
	{

	 actiontitle = "发布辩论";
	

	}
	else
	{

	 actiontitle = "发新主题";
	

	}	//end if


	}
	else if (pagename=="postreply.aspx")
	{

	 actiontitle = "回复主题";
	

	}
	else if (pagename=="editpost.aspx")
	{

	 actiontitle = "编辑帖子";
	

	}	//end if

	char comma = ',';
	
	string editorid = "e";
	
	int thumbwidth = 400;
	
	int thumbheight = 300;
	
	templateBuilder.Append("		<h1 class=\"mt cl\"><em id=\"returnmessage\">");
	templateBuilder.Append(actiontitle.ToString());
	templateBuilder.Append("</em>\r\n");

	if (needaudit)
	{

	templateBuilder.Append("<em class=\"needverify\">需审核</em>\r\n");

	}	//end if

	templateBuilder.Append("</h1>\r\n");
	templateBuilder.Append("		<div id=\"postbox\">\r\n");
	templateBuilder.Append("		<div class=\"pbt cl\">\r\n");
	templateBuilder.Append("			<input type=\"hidden\" name=\"iconid\" id=\"iconid\" value=\"");
	templateBuilder.Append(topic.Iconid.ToString().Trim());
	templateBuilder.Append("\" />\r\n");

	if (special=="" && isfirstpost)
	{

	templateBuilder.Append("		<div class=\"ftid\">\r\n");
	templateBuilder.Append("			<a id=\"icon\" class=\"z\" onmouseover=\"InFloat='floatlayout_");
	templateBuilder.Append(action.ToString());
	templateBuilder.Append("';showMenu(this.id)\"><img id=\"icon_img\" src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(topic.Iconid.ToString().Trim());
	templateBuilder.Append(".gif\" style=\"margin-top:4px;\"/></a>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<ul id=\"icon_menu\" class=\"sltm\" style=\"display:none\">\r\n");
	string icons = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";
	

	int id__loop__id=0;
	foreach(string id in icons.Split(comma))
	{
		id__loop__id++;

	templateBuilder.Append("			<li><a href=\"javascript:;\"><img onclick=\"switchicon(");
	templateBuilder.Append(id.ToString());
	templateBuilder.Append(", this)\" src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(id.ToString());
	templateBuilder.Append(".gif\" alt=\"\" /></a></li>\r\n");

	}	//end loop

	templateBuilder.Append("		</ul>\r\n");

	}	//end if


	if (forum.Applytopictype==1 && topictypeselectoptions!=""&&isfirstpost)
	{

	templateBuilder.Append("			<div class=\"ftid\">\r\n");
	templateBuilder.Append("				<select name=\"typeid\" id=\"typeid\">");
	templateBuilder.Append(topictypeselectoptions.ToString());
	templateBuilder.Append("</select>\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\" reload=\"1\">$('typeid').value = '");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("';</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\">simulateSelect(\"typeid\");</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			</div>\r\n");

	}	//end if


	if (!isfirstpost && (topic.Special==4||special=="debate"))
	{

	templateBuilder.Append("			<div class=\"ftid\">\r\n");
	templateBuilder.Append("				<select id=\"debateopinion\" name=\"debateopinion\">\r\n");
	templateBuilder.Append("					<option value=\"0\">观点</option>\r\n");
	templateBuilder.Append("					<option value=\"1\">正方</option>\r\n");
	templateBuilder.Append("					<option value=\"2\">反方</option>\r\n");
	templateBuilder.Append("				</select>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<script type=\"text/javascript\">simulateSelect(\"debateopinion\");</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			<script type=\"text/javascript\" reload=\"1\">$('debateopinion').selectedIndex = parseInt(getQueryString(\"debate\"));</");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("			<input name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" type=\"text\" id=\"title\" value=\"");
	templateBuilder.Append(postinfo.Title.ToString().Trim());
	templateBuilder.Append("\" class=\"txt postpx\"/>\r\n");

	if (action=="reply" || postinfo.Layer>0)
	{

	templateBuilder.Append("			<cite class=\"tips\">(可选)</cite>\r\n");

	}	//end if

	templateBuilder.Append("标题最多为60个字符\r\n");

	if (canhtmltitle)
	{

	templateBuilder.Append("			<a href=\"###\" id=\"titleEditorButton\" onclick=\"\">高级编辑</a>\r\n");
	templateBuilder.Append("			<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/dnteditor.js\" reload=\"1\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			<div id=\"titleEditorDiv\" style=\"display: none;\">\r\n");
	templateBuilder.Append("				<textarea name=\"htmltitle\" id=\"htmltitle\" cols=\"80\" rows=\"10\"></textarea>\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\" reload=\"1\" >\r\n");
	templateBuilder.Append("				var forumpath = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("				var templatepath = '");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("				var temptitle = $('faketitle');\r\n");
	templateBuilder.Append("				var titleEditor = null;\r\n");
	templateBuilder.Append("				function AdvancedTitleEditor() {\r\n");
	templateBuilder.Append("				$('title').style.display = 'none';\r\n");
	templateBuilder.Append("				$('titleEditorDiv').style.display = '';\r\n");
	templateBuilder.Append("				$('titleEditorButton').style.display = 'none';\r\n");
	templateBuilder.Append("				titleEditor = new DNTeditor('htmltitle', '500', '50', '");
	templateBuilder.Append(htmltitle.ToString());
	templateBuilder.Append("' == '' ? $('title').value : '");
	templateBuilder.Append(htmltitle.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("				titleEditor.OnChange = function() {\r\n");
	templateBuilder.Append("				}\r\n");
	templateBuilder.Append("				titleEditor.Basic = true;\r\n");
	templateBuilder.Append("				titleEditor.IsAutoSave = false;\r\n");
	templateBuilder.Append("				titleEditor.Style = forumpath + 'templates/' + templatepath + '/editor.css';\r\n");
	templateBuilder.Append("				titleEditor.BasePath = forumpath;\r\n");
	templateBuilder.Append("				titleEditor.ReplaceTextarea();\r\n");
	templateBuilder.Append("				}\r\n");
	templateBuilder.Append("				$('titleEditorButton').onclick = function() {\r\n");
	templateBuilder.Append("				AdvancedTitleEditor();\r\n");
	templateBuilder.Append("				};\r\n");
	templateBuilder.Append("				</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			</div>\r\n");

	if (htmltitle!="")
	{

	templateBuilder.Append("			<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("			AdvancedTitleEditor();\r\n");
	templateBuilder.Append("			</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		</div>\r\n");

	if (adveditor)
	{

	templateBuilder.Append("		<div id=\"specialpost\" class=\"pbt cl\"></div>\r\n");
	templateBuilder.Append("		<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("			_attachEvent(window, \"load\", function(){ \r\n");
	templateBuilder.Append("			if($('specialposttable')) {\r\n");
	templateBuilder.Append("				$('specialpost').innerHTML = $('specialposttable').innerHTML;\r\n");
	templateBuilder.Append("				$('specialposttable').innerHTML = '';\r\n");
	templateBuilder.Append("			}\r\n");
	templateBuilder.Append("			});\r\n");
	templateBuilder.Append("		</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\" reload=\"1\" ></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<div class=\"edt cl\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_body\">\r\n");
	templateBuilder.Append("	<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_controls\" class=\"bar\">\r\n");
	templateBuilder.Append("		<div class=\"y\">\r\n");
	templateBuilder.Append("			<div class=\"b2r nbl nbr\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_5\">\r\n");
	templateBuilder.Append("				<p><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_undo\" title=\"撤销\">Undo</a></p>\r\n");
	templateBuilder.Append("				<p><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_redo\" title=\"重做\">Redo</a></p>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"z\">\r\n");
	templateBuilder.Append("				<span class=\"mbn\"><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_simple\"></a><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fullswitcher\"></a></span>\r\n");
	templateBuilder.Append("				<label id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_switcher\" class=\"bar_swch ptn\"><input type=\"checkbox\" class=\"pc\" name=\"checkbox\" value=\"0\" \r\n");

	if (config.Defaulteditormode==0)
	{

	templateBuilder.Append("checked=\"checked\"\r\n");

	}	//end if

	templateBuilder.Append(" onclick=\"switchEditor(this.checked?0:1)\" />代码模式</label>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_button\" class=\"cl\">\r\n");
	templateBuilder.Append("			<div class=\"b1r\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_s0\">\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_paste\" title=\"粘贴\">粘贴</a>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"b2r nbr\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_s2\">\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontname\" class=\"dp\" title=\"设置字体\"><span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_font\">字体</span></a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontsize\" class=\"dp\" title=\"设置文字大小\"><span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_size\">大小</span></a>\r\n");
	templateBuilder.Append("				<br id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_1\" />\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_bold\" title=\"粗体\">B</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_italic\" title=\"文字斜体\">I</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_underline\" title=\"文字加下划线\">U</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_forecolor\" title=\"设置文字颜色\">Color</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_url\" title=\"添加链接\">Url</a>\r\n");
	templateBuilder.Append("				<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_8\">\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_unlink\" title=\"移除链接\">Unlink</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_inserthorizontalrule\" title=\"分隔线\">Hr</a>\r\n");
	templateBuilder.Append("				</span>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"b2r nbl\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_2\">\r\n");
	templateBuilder.Append("				<p id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_3\"><a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_tbl\" title=\"添加表格\">Table</a></p>\r\n");
	templateBuilder.Append("				<p>	<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_removeformat\" title=\"清除文本格式\">Removeformat</a></p>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"b2r\">\r\n");
	templateBuilder.Append("				<p>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_justifyleft\" title=\"居左\">Left</a>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_justifycenter\" title=\"居中\">Center</a>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_justifyright\" title=\"居右\">Right</a>\r\n");
	templateBuilder.Append("				</p>\r\n");
	templateBuilder.Append("				<p id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_4\">\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_autotypeset\" title=\"自动排版\">Autotypeset</a>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_insertorderedlist\" title=\"排序的列表\">Orderedlist</a>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_insertunorderedlist\" title=\"未排序列表\">Unorderedlist</a>\r\n");
	templateBuilder.Append("				</p>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"b1r\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_s1\">\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_sml\" title=\"添加表情\">表情</a>\r\n");
	templateBuilder.Append("				<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_imagen\" style=\"display:none\">!</div>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image\" title=\"添加图片\">图片</a>\r\n");

	if (canpostattach)
	{

	templateBuilder.Append("				<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attachn\" style=\"display:none\">!</div>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attach\" title=\"添加附件\">附件</a>\r\n");

	}	//end if

	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_aud\" title=\"添加音乐\">音乐</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_vid\" title=\"添加视频\">视频</a>\r\n");
	templateBuilder.Append("				<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fls\" title=\"添加 Flash\">Flash</a>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"b2r nbr\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_adv_6\">\r\n");
	templateBuilder.Append("				<p>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_code\" title=\"添加代码文字\">代码</a>	\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_hide\" title=\"隐藏内容\">隐藏内容</a>				\r\n");
	templateBuilder.Append("				</p>\r\n");
	templateBuilder.Append("				<p>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_quote\" title=\"添加引用文字\">引用</a>\r\n");
	templateBuilder.Append("					<a id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_free\" title=\"免费信息\">免费信息</a>\r\n");
	templateBuilder.Append("				</p>\r\n");
	templateBuilder.Append("			</div>			\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<div class=\"area cl\">\r\n");
	templateBuilder.Append("		<textarea name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_textarea\" class=\"pt\" tabindex=\"1\" rows=\"15\">");
	templateBuilder.Append(message.ToString());
	templateBuilder.Append("</textarea>\r\n");
	templateBuilder.Append("	</div>\r\n");

	templateBuilder.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/editor.css\" />\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post_editor.js\" ></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	var infloat = ");
	templateBuilder.Append(infloat.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("	var InFloat_Editor = 'floatlayout_");
	templateBuilder.Append(action.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("	var editoraction = '");
	templateBuilder.Append(action.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("	var lang	= new Array();\r\n");
	templateBuilder.Append("	lang['post_discuzcode_code'] = '请输入要插入的代码';\r\n");
	templateBuilder.Append("	lang['post_discuzcode_quote'] = '请输入要插入的引用';\r\n");
	templateBuilder.Append("	lang['post_discuzcode_free'] = '请输入要插入的免费信息';\r\n");
	templateBuilder.Append("	lang['post_discuzcode_hide'] = '请输入要插入的隐藏内容';\r\n");
	templateBuilder.Append("	lang['board_allowed'] = '系统限制';\r\n");
	templateBuilder.Append("	lang['lento'] = '到';\r\n");
	templateBuilder.Append("	lang['bytes'] = '字节';\r\n");
	templateBuilder.Append("	lang['post_curlength'] = '当前长度';\r\n");
	templateBuilder.Append("	lang['post_title_and_message_isnull'] = '请完成标题或内容栏。';\r\n");
	templateBuilder.Append("	lang['post_title_toolong'] = '您的标题超过 60 个字符的限制。';\r\n");
	templateBuilder.Append("	lang['post_message_length_invalid'] = '您的帖子长度不符合要求。';\r\n");
	templateBuilder.Append("	lang['post_type_isnull'] = '请选择主题对应的分类。';\r\n");
	templateBuilder.Append("	lang['post_reward_credits_null'] = '对不起，您输入悬赏积分。';\r\n");
	templateBuilder.Append("	lang['post_attachment_ext_notallowed']	= '对不起，不支持上传此类扩展名的附件。';\r\n");
	templateBuilder.Append("	lang['post_attachment_img_invalid']		= '无效的图片文件。';\r\n");
	templateBuilder.Append("	lang['post_attachment_deletelink']		= '删除';\r\n");
	templateBuilder.Append("	lang['post_attachment_insert']			= '点击这里将本附件插入帖子内容中当前光标的位置';\r\n");
	templateBuilder.Append("	lang['post_attachment_insertlink']		= '插入';\r\n");
	templateBuilder.Append("	lang['enter_list_item']			= \"输入一个列表项目.\\r\\n留空或者点击取消完成此列表.\";\r\n");
	templateBuilder.Append("	lang['enter_link_url']			= \"请输入链接的地址:\";\r\n");
	templateBuilder.Append("	lang['enter_image_url']			= \"请输入图片链接地址:\";\r\n");
	templateBuilder.Append("	lang['enter_email_link']		= \"请输入此链接的邮箱地址:\";\r\n");
	templateBuilder.Append("	lang['fontname']				= \"字体\";\r\n");
	templateBuilder.Append("	lang['fontsize']				= \"大小\";\r\n");
	templateBuilder.Append("	lang['post_advanceeditor']		= \"全部功能\";\r\n");
	templateBuilder.Append("	lang['post_simpleeditor']		= \"简单功能\";\r\n");
	templateBuilder.Append("	lang['submit']					= \"提交\";\r\n");
	templateBuilder.Append("	lang['cancel']					= \"取消\";\r\n");
	templateBuilder.Append("	lang['post_autosave_none'] = \"没有可以恢复的数据\";\r\n");
	templateBuilder.Append("	lang['post_autosave_confirm'] = \"本操作将覆盖当前帖子内容，确定要恢复数据吗？\";\r\n");
	templateBuilder.Append("	lang['enter_tag_option']		= \"请输入 %1 标签的选项:\";\r\n");
	templateBuilder.Append("	lang['enter_table_rows']		= \"请输入行数，最多 30 行:\";\r\n");
	templateBuilder.Append("	lang['enter_table_columns']		= \"请输入列数，最多 30 列:\";\r\n");
	templateBuilder.Append("var editorid = '");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("	var editorcss = 'templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/editor.css';\r\n");
	templateBuilder.Append("	var textobj = $(editorid + '_textarea');\r\n");
	templateBuilder.Append("	var typerequired = parseInt('0');\r\n");
	templateBuilder.Append("	var seccodecheck = parseInt('0');\r\n");
	templateBuilder.Append("	var secqaacheck = parseInt('0');\r\n");
	templateBuilder.Append("	var special = 1;\r\n");

	if (special=="")
	{

	templateBuilder.Append("	special = 0;\r\n");

	}	//end if

	templateBuilder.Append("	var isfirstpost = 0;\r\n");

	if (isfirstpost)
	{

	templateBuilder.Append("	isfirstpost = 1;\r\n");

	}	//end if

	templateBuilder.Append("	var allowposttrade = parseInt('1');\r\n");
	templateBuilder.Append("	var allowpostreward = parseInt('1');\r\n");
	templateBuilder.Append("	var allowpostactivity = parseInt('1');\r\n");
	templateBuilder.Append("	var bbinsert = parseInt('1');\r\n");
	templateBuilder.Append("	var allowhtml = parseInt('");
	templateBuilder.Append(htmlon.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("	var forumallowhtml = parseInt('1');\r\n");
	templateBuilder.Append("	var allowsmilies = 1 - parseInt('");
	templateBuilder.Append(smileyoff.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("	var allowbbcode = parseInt('");
	templateBuilder.Append(usergroupinfo.Allowcusbbcode.ToString().Trim());
	templateBuilder.Append("') == 1 && parseInt('");
	templateBuilder.Append(forum.Allowbbcode.ToString().Trim());
	templateBuilder.Append("') == 1;\r\n");
	templateBuilder.Append("	var allowimgcode = parseInt('");
	templateBuilder.Append(forum.Allowimgcode.ToString().Trim());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("	//var wysiwyg = (is_ie || is_moz || (is_opera && opera.version() >= 9)) && parseInt('");
	templateBuilder.Append(config.Defaulteditormode.ToString().Trim());
	templateBuilder.Append("') && allowbbcode == 1 ? 1 : 0;//bbinsert == 1 ? 1 : 0;\r\n");
	templateBuilder.Append("	var wysiwyg = (BROWSER.ie || BROWSER.firefox || (BROWSER.opera >= 9)) && parseInt('");
	templateBuilder.Append(config.Defaulteditormode.ToString().Trim());
	templateBuilder.Append("') && allowbbcode == 1 ? 1 : 0;//bbinsert == 1 ? 1 : 0;\r\n");
	templateBuilder.Append("	var allowswitcheditor = parseInt('");
	templateBuilder.Append(config.Allowswitcheditor.ToString().Trim());
	templateBuilder.Append("') && allowbbcode == 1 ;\r\n");
	templateBuilder.Append("	var custombbcodes = { ");
	templateBuilder.Append(Caches.GetCustomEditButtonList().ToString().Trim());
	templateBuilder.Append(" };\r\n");
	templateBuilder.Append("	var smileyinsert = parseInt('1');\r\n");
	templateBuilder.Append("	var smiliesCount = 32;//显示表情总数\r\n");
	templateBuilder.Append("	var colCount = 8; //每行显示表情个数\r\n");
	templateBuilder.Append("	var title = \"\";				   //标题\r\n");
	templateBuilder.Append("	var showsmiliestitle = 1;        //是否显示标题（0不显示 1显示）\r\n");
	templateBuilder.Append("	var smiliesIsCreate = 0;		   //编辑器是否已被创建(0否，1是）\r\n");
	templateBuilder.Append("	var maxpolloptions = parseInt('");
	templateBuilder.Append(config.Maxpolloptions.ToString().Trim());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("	function alloweditorhtml() {\r\n");
	templateBuilder.Append("		if($('htmlon').checked) {\r\n");
	templateBuilder.Append("			allowhtml = 1;\r\n");
	templateBuilder.Append("			forumallowhtml = 1;\r\n");
	templateBuilder.Append("		} else {\r\n");
	templateBuilder.Append("			allowhtml = 0;\r\n");
	templateBuilder.Append("			forumallowhtml = 0;\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	var simplodemode = parseInt('1');\r\n");
	templateBuilder.Append("		<!--{if $_G['cache']['bbcodes_display'][$_G['groupid']]}-->\r\n");
	templateBuilder.Append("		<!--{loop $_G['cache']['bbcodes_display'][$_G['groupid']] $tag $bbcode}-->\r\n");
	templateBuilder.Append("		<!--{/loop}-->\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if $editor[simplemode] > 0}-->\r\n");
	templateBuilder.Append("		editorsimple();\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_bbar\" class=\"bbar\">\r\n");
	templateBuilder.Append("	<em id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_tip\"></em>\r\n");
	templateBuilder.Append("	<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_svdsecond\"></span>\r\n");
	templateBuilder.Append("	<a href=\"javascript:;\" onclick=\"discuzcode('svd');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_svd\">保存数据</a> |\r\n");
	templateBuilder.Append("	<a href=\"javascript:;\" onclick=\"discuzcode('rst');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_rst\">恢复数据</a> &nbsp;&nbsp;\r\n");
	templateBuilder.Append("	<a href=\"javascript:;\" onclick=\"discuzcode('chck');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_chck\">字数检查</a> |\r\n");
	templateBuilder.Append("	<a href=\"javascript:;\" onclick=\"discuzcode('tpr');return false;\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_tpr\">清空内容</a> &nbsp;&nbsp;\r\n");
	templateBuilder.Append("	<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_resize\"><a href=\"javascript:;\" onclick=\"editorsize('+')\">加大编辑框</a> | <a href=\"javascript:;\" onclick=\"editorsize('-')\">缩小编辑器</a><img src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("editor/images/resize.gif\" onmousedown=\"editorresize(event)\" /></span>\r\n");
	templateBuilder.Append("</div>\r\n");


	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_menus\" class=\"editorrow\" style=\"overflow: hidden; margin-top: -5px; height: 0; border: none; background: transparent;\">\r\n");

	templateBuilder.Append("<div id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_editortoolbar\" class=\"editortoolbar\">\r\n");
	templateBuilder.Append("	<div class=\"p_pop fnm\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontname_menu\" style=\"display: none\">\r\n");
	templateBuilder.Append("	<ul unselectable=\"on\">\r\n");
	string fontoptions = "仿宋_GB2312,黑体,楷体_GB2312,宋体,新宋体,微软雅黑,TrebuchetMS,Tahoma,Arial,Impact,Verdana,TimesNewRoman";
	

	int fontname__loop__id=0;
	foreach(string fontname in fontoptions.Split(comma))
	{
		fontname__loop__id++;

	templateBuilder.Append("	    <li onclick=\"discuzcode('fontname', '");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("')\" style=\"font-family: ");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("\" unselectable=\"on\"><a href=\"javascript:;\" title=\"");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(fontname.ToString());
	templateBuilder.Append("</a></li>\r\n");

	}	//end loop

	templateBuilder.Append("	</ul>\r\n");
	templateBuilder.Append("	</div>\r\n");
	string sizeoptions = "1,2,3,4,5,6,7";
	
	templateBuilder.Append("	<div class=\"p_pop fszm\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_fontsize_menu\" style=\"display: none\">\r\n");
	templateBuilder.Append("	<ul unselectable=\"on\">\r\n");

	int size__loop__id=0;
	foreach(string size in sizeoptions.Split(comma))
	{
		size__loop__id++;

	templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', ");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append(")\" unselectable=\"on\"><a href=\"javascript:;\" title=\"");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append("\"><font size=\"");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append("\" unselectable=\"on\">");
	templateBuilder.Append(size.ToString());
	templateBuilder.Append("</font></a></li>\r\n");

	}	//end loop

	templateBuilder.Append("	</ul>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	if (config.Smileyinsert==1)
	{

	templateBuilder.Append("	<div class=\"p_pof upf\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_sml_menu\" style=\"display: none;width:320px;\">\r\n");

	templateBuilder.Append("<div class=\"smilieslist\">\r\n");
	string defaulttypname = string.Empty;
	
	templateBuilder.Append("	<div id=\"smiliesdiv\">\r\n");
	templateBuilder.Append("		<div class=\"smiliesgroup\" style=\"margin-right: 0pt;\">\r\n");
	templateBuilder.Append("			<ul>\r\n");

	int stype__loop__id=0;
	foreach(DataRow stype in Caches.GetSmilieTypesCache().Rows)
	{
		stype__loop__id++;


	if (stype__loop__id==1)
	{

	 defaulttypname = stype["code"].ToString().Trim();
	

	}	//end if


	if (stype__loop__id==1)
	{

	templateBuilder.Append("				<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles1(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "');\" class=\"current\">" + stype["code"].ToString().Trim() + "</a></li>\r\n");

	}
	else
	{

	templateBuilder.Append("				<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles1(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "');\">" + stype["code"].ToString().Trim() + "</a></li>\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("		 </div>\r\n");
	templateBuilder.Append("		 <div style=\"clear: both;\" id=\"showsmilie\"></div>\r\n");
	templateBuilder.Append("		 <table class=\"smilieslist_table\" id=\"s_preview_table\" style=\"display: none\"><tr><td class=\"smilieslist_preview\" id=\"s_preview\"></td></tr></table>\r\n");
	templateBuilder.Append("		 <div id=\"showsmilie_pagenum\" class=\"smilieslist_page\">&nbsp;</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	function getSmilies(func){\r\n");
	templateBuilder.Append("		if($('showsmilie').innerHTML !='' && $('showsmilie').innerHTML != '正在加载表情...')\r\n");
	templateBuilder.Append("			return;\r\n");
	templateBuilder.Append("		var c = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=smilies\";\r\n");
	templateBuilder.Append("		_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true);\r\n");
	templateBuilder.Append("		setTimeout(\"if($('showsmilie').innerHTML=='')$('showsmilie').innerHTML = '正在加载表情...'\", 2000);\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	function getSmilies_callback(obj) {\r\n");
	templateBuilder.Append("		smilies_HASH = obj; \r\n");
	templateBuilder.Append("		showsmiles1(1, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	//_attachEvent($('");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_popup_smilies'), 'click', function(){\r\n");
	templateBuilder.Append("		getSmilies(getSmilies_callback);\r\n");
	templateBuilder.Append("	//});\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");


	templateBuilder.Append("	</div>\r\n");

	}	//end if

	templateBuilder.Append("<!-- <script type=\"text/javascript\">smilies_show('smiliesdiv', 8, editorid + '_');</");
	templateBuilder.Append("script> -->\r\n");


	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<div class=\"p_pof uploadfile\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_menu\" style=\"display: none\" unselectable=\"on\">\r\n");
	templateBuilder.Append("	<div class=\"p_opt popupfix\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_www\">\r\n");
	templateBuilder.Append("	<span class=\"y\"><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideMenu()\">关闭</a></span>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th width=\"74%\">请输入图片地址<span id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_status\" class=\"xi1\"></span></th>\r\n");
	templateBuilder.Append("				<th width=\"13%\">宽(可选)</th>\r\n");
	templateBuilder.Append("				<th width=\"13%\">高(可选)</th>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<td><input type=\"text\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_param_1\" onchange=\"loadimgsize(this.value)\" style=\"width: 95%;\" value=\"\" class=\"px\" autocomplete=\"off\" /></td>\r\n");
	templateBuilder.Append("				<td><input id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_param_2\" size=\"1\" value=\"\" class=\"px p_fre\" autocomplete=\"off\" /></td>\r\n");
	templateBuilder.Append("				<td><input id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_param_3\" size=\"1\" value=\"\" class=\"px p_fre\" autocomplete=\"off\" /></td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<td colspan=\"3\" class=\"pns mtn\">\r\n");
	templateBuilder.Append("					<button type=\"button\" class=\"pn pnc\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_image_submit\"><strong>提交</strong></button>\r\n");
	templateBuilder.Append("					<button type=\"button\" class=\"pn\" onclick=\"hideMenu();\"><em>取消</em></button>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("		</table>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<input type=\"hidden\" name=\"wysiwyg\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_mode\" value=\"1\" />\r\n");
	templateBuilder.Append("<input type=\"hidden\" id=\"testsubmit\" />\r\n");



	if (pagename=="posttopic.aspx" || (pagename=="editpost.aspx"&&isfirstpost))
	{


	if (enabletag)
	{

	templateBuilder.Append("			<div class=\"pbt cl margint\">\r\n");
	templateBuilder.Append("				<p><strong>标签(Tags):</strong>(用空格隔开多个标签，最多可填写 5 个)</p>\r\n");
	templateBuilder.Append("				<p><input type=\"text\" name=\"tags\" id=\"tags\" class=\"txt\" value=\"");
	templateBuilder.Append(topictags.ToString());
	templateBuilder.Append("\" tabindex=\"1\" /><button name=\"addtags\" type=\"button\" onclick=\"relatekw();return false\">+可用标签</button> <span id=\"tagselect\"></span></p>\r\n");
	templateBuilder.Append("			</div>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		<div id=\"moreinfo\"></div>\r\n");
	templateBuilder.Append("		<div style=\"clear:both;\"></div>\r\n");
	templateBuilder.Append("		<div class=\"pbt cl margint\">\r\n");
	templateBuilder.Append("			<div class=\"custominfoarea\" id=\"custominfoarea\" style=\"display: none;\"></div>\r\n");

	if (postinfo.Layer==0 && forum.Applytopictype==1)
	{

	templateBuilder.Append("			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"");
	templateBuilder.Append(forum.Postbytopictype.ToString().Trim());
	templateBuilder.Append("\" tabindex=\"3\">\r\n");

	}	//end if

	templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("				function RunMutiUpload() {\r\n");
	templateBuilder.Append("				if ($('MultiUploadFile').content != null)\r\n");
	templateBuilder.Append("					$('MultiUploadFile').content.MultiFileUpload.GetAttachmentList();			\r\n");
	templateBuilder.Append("				//switchAttachbutton('attachlist');\r\n");
	templateBuilder.Append("				//updateAttachList();\r\n");
	templateBuilder.Append("				}\r\n");
	templateBuilder.Append("			</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			<button type=\"submit\" id=\"postsubmit\" value=\"true\"\r\n");

	if (pagename=="posttopic.aspx")
	{

	templateBuilder.Append(" name=\"topicsubmit\"\r\n");

	}
	else if (pagename=="postreply.aspx")
	{

	templateBuilder.Append(" name=\"replysubmit\"\r\n");

	}
	else if (pagename=="editpost.aspx")
	{

	templateBuilder.Append(" name=\"editsubmit\"\r\n");

	}	//end if

	templateBuilder.Append(" tabindex=\"1\" class=\"pn\"><span>");
	templateBuilder.Append(actiontitle.ToString());
	templateBuilder.Append("</span></button>\r\n");
	templateBuilder.Append("			<span id=\"more_2\">\r\n");

	if (userinfo.Spaceid>0 && special==""&&action=="newthread"&&config.Enablespace==1)
	{

	templateBuilder.Append("<input type=\"checkbox\" name=\"addtoblog\" /> 添加到个人空间\r\n");

	}	//end if

	templateBuilder.Append("			</span>\r\n");

	if (isseccode)
	{

	templateBuilder.Append("<span style=\"position:relative\">\r\n");

	templateBuilder.Append("<div id=\"vcode_temp\"></div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	var infloat = ");
	templateBuilder.Append(infloat.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("	if (typeof vcodeimgid == 'undefined'){\r\n");
	templateBuilder.Append("		var vcodeimgid = 1;\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	else\r\n");
	templateBuilder.Append("	    vcodeimgid++;\r\n");
	templateBuilder.Append("    $('vcode_temp').parentNode.innerHTML = '<input name=\"vcodetext\" tabindex=\"1002\" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:90px;\" id=\"vcodetext' + vcodeimgid + '\"  onblur=\"if(!seccodefocus) {display(this.id + \\'_menu\\')};\"  onfocus=\"opensecwin('+vcodeimgid+',1)\"   value=\"验证码\" autocomplete=\"off\"/>' +\r\n");
	templateBuilder.Append("	                                       '<div class=\"seccodecontent\"  style=\"display:none;cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n");
	templateBuilder.Append("	function changevcode(form, value){\r\n");
	templateBuilder.Append("		if (!$('vcode')){\r\n");
	templateBuilder.Append("			var vcode = document.createElement('input');\r\n");
	templateBuilder.Append("			vcode.id = 'vcode';\r\n");
	templateBuilder.Append("			vcode.name = 'vcode';\r\n");
	templateBuilder.Append("			vcode.type = 'hidden';\r\n");
	templateBuilder.Append("			vcode.value = value;\r\n");
	templateBuilder.Append("			form.appendChild(vcode);\r\n");
	templateBuilder.Append("		}else{\r\n");
	templateBuilder.Append("			$('vcode').value = value;\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("var secclick = new Array();\r\n");
	templateBuilder.Append("var seccodefocus = 0;\r\n");
	templateBuilder.Append("function opensecwin(id,type) {\r\n");
	templateBuilder.Append("	if($('vcode')){\r\n");
	templateBuilder.Append("	$('vcode').parentNode.removeChild($('vcode'));}\r\n");
	templateBuilder.Append("	if (!secclick['vcodetext' + id]) {\r\n");
	templateBuilder.Append("	    $('vcodetext' + id).value = '';\r\n");
	templateBuilder.Append("	    secclick['vcodetext' + id] = 1;\r\n");
	templateBuilder.Append("	    if(type)\r\n");
	templateBuilder.Append("	        $('vcodetext' + id + '_menu').style.top = parseInt($('vcodetext' + id + '_menu').style.top) - parseInt($('vcodetext' + id + '_menu').style.height) + 'px';\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.position = 'absolute';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.top = (-parseInt($('vcodetext' + id + '_menu').style.height) - 2) + 'px';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.left = '0px';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.display = '';\r\n");
	templateBuilder.Append("	$('vcodetext' + id).focus();\r\n");
	templateBuilder.Append("	$('vcodetext' + id).unselectable = 'off';\r\n");
	templateBuilder.Append("	$('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n");
	templateBuilder.Append("}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");


	templateBuilder.Append("</span>\r\n");

	}	//end if

	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<div class=\"edt_app\">\r\n");

	if (pagename=="posttopic.aspx" || (pagename=="editpost.aspx"&&isfirstpost))
	{


	if (userid!=-1 && usergroupinfo.Allowsetreadperm==1)
	{

	templateBuilder.Append("			<p><strong>阅读权限:</strong></p>\r\n");
	templateBuilder.Append("			<p class=\"mbn\">\r\n");
	templateBuilder.Append("                <em class=\"ftid\">\r\n");
	templateBuilder.Append("                    <select name=\"topicreadperm\" id=\"topicreadperm\" class=\"ps\" style=\"width:90px\">\r\n");
	templateBuilder.Append("                        <option value=\"\">不限</option>\r\n");

	int userGroupInfo__loop__id=0;
	foreach(UserGroupInfo userGroupInfo in userGroupInfoList)
	{
		userGroupInfo__loop__id++;


	if (userGroupInfo.Readaccess!=0)
	{

	templateBuilder.Append("                        <option value=\"");
	templateBuilder.Append(userGroupInfo.Readaccess.ToString().Trim());
	templateBuilder.Append("\" title=\"阅读权限: ");
	templateBuilder.Append(userGroupInfo.Readaccess.ToString().Trim());
	templateBuilder.Append("\"\r\n");

	if (userGroupInfo.Readaccess==topic.Readperm)
	{

	templateBuilder.Append(" selected=\"selected\"\r\n");

	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(userGroupInfo.Grouptitle.ToString().Trim());
	templateBuilder.Append("</option>\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("                        <option value=\"255\">最高权限</option>\r\n");
	templateBuilder.Append("                    </select>\r\n");
	templateBuilder.Append("                </em>\r\n");
	templateBuilder.Append("                <img src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/faq.gif\" alt=\"Tip\" class=\"mtn vm\" style=\"margin: 0;\" onmouseover=\"showTip(this)\" tip=\"阅读权限按由高到低排列，高于或等于选中组的用户才可以阅读。\" />\r\n");
	templateBuilder.Append("            </p>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		<h4 style=\"clear:both;\">发帖选项:</h4>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"htmlon\" id=\"htmlon\"  onclick=\"alloweditorhtml()\" \r\n");

	if (usergroupinfo.Allowhtml!=1)
	{

	templateBuilder.Append("disabled\r\n");

	}	//end if


	if (htmlon==1)
	{

	templateBuilder.Append("checked\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"htmlon\">html 代码</label></p>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" id=\"allowimgcode\" disabled\r\n");

	if (allowimg==1)
	{

	templateBuilder.Append(" checked=\"checked\"\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"allowimgcode\">[img] 代码</label></p>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"parseurloff\" id=\"parseurloff\" \r\n");

	if (parseurloff==1)
	{

	templateBuilder.Append("checked\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"parseurloff\">禁用 网址自动识别</label></p>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"smileyoff\" id=\"smileyoff\" \r\n");

	if (smileyoff==1)
	{

	templateBuilder.Append("checked\r\n");

	}	//end if


	if (forum.Allowsmilies!=1)
	{

	templateBuilder.Append("disabled\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"smileyoff\">禁用 表情</label></p>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"bbcodeoff\" id=\"bbcodeoff\" \r\n");

	if (bbcodeoff==1)
	{

	templateBuilder.Append(" checked\r\n");

	}	//end if


	if (usergroupinfo.Allowcusbbcode!=1)
	{

	templateBuilder.Append(" disabled\r\n");

	}
	else if (forum.Allowbbcode!=1)
	{

	templateBuilder.Append(" disabled\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"bbcodeoff\">禁用 论坛代码</label></p>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" value=\"1\" name=\"usesig\" id=\"usesig\" \r\n");

	if (usesig==1)
	{

	templateBuilder.Append("checked\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"usesig\">使用个人签名</label></p>\r\n");

	if (pagename=="postreply.aspx")
	{

	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" \r\n");

	if (config.Replyemailstatus==1)
	{

	templateBuilder.Append(" checked\r\n");

	}	//end if

	templateBuilder.Append(" /><label for=\"emailnotify\">发送邮件告知楼主</label></p>\r\n");
	templateBuilder.Append("		<p class=\"mbn\"><input type=\"checkbox\" name=\"postreplynotice\" id=\"postreplynotice\" \r\n");

	if (config.Replynotificationstatus==1)
	{

	templateBuilder.Append(" checked \r\n");

	}	//end if

	templateBuilder.Append("/><label for=\"emailnotify\">发送论坛通知给楼主</label></p>\r\n");

	}	//end if


	if (pagename=="posttopic.aspx" || (pagename=="editpost.aspx"&&isfirstpost))
	{


	if (special==""&&Scoresets.GetCreditsTrans()!=0 && usergroupinfo.Maxprice>0)
	{

	templateBuilder.Append("			<p style=\"clear:both;\"><strong>售价</strong>(");
	templateBuilder.Append(userextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append("):</p>\r\n");
	templateBuilder.Append("			<p><input type=\"text\" name=\"topicprice\" value=\"");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("\" class=\"txt\"  size=\"6\"/> ");
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append(" <br/>最高 ");
	templateBuilder.Append(usergroupinfo.Maxprice.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("售价只允许非负整数, 单个主题最大收入 ");
	templateBuilder.Append(Scoresets.GetMaxIncPerTopic().ToString().Trim());
	templateBuilder.Append(userextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("</p>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");



	templateBuilder.Append("			<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_local\" style=\"display: none;\">\r\n");
	templateBuilder.Append("				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n");
	templateBuilder.Append("					<tbody id=\"imgattachbodyhidden\" style=\"display:none\"><tr>\r\n");
	templateBuilder.Append("						<td class=\"atnu\"><span id=\"imglocalno[]\"><img src=\"images/attachicons/common_new.gif\" /></span></td>\r\n");
	templateBuilder.Append("						<td class=\"atna\">\r\n");
	templateBuilder.Append("							<span id=\"imgdeschidden[]\" style=\"display:none\">\r\n");
	templateBuilder.Append("								<span id=\"imglocalfile[]\"></span>\r\n");
	templateBuilder.Append("							</span>\r\n");
	templateBuilder.Append("							<input type=\"hidden\" name=\"imglocalid[]\" />\r\n");
	templateBuilder.Append("						</td>\r\n");
	templateBuilder.Append("						<td class=\"attc delete_msg\"><span id=\"imgcpdel[]\"></span></td>\r\n");
	templateBuilder.Append("					</tr></tbody>\r\n");
	templateBuilder.Append("				</table>\r\n");
	templateBuilder.Append("				<div class=\"p_tbl\"><table cellpadding=\"0\" cellspacing=\"0\" summary=\"post_attachbody\" border=\"0\" width=\"100%\"><tbody id=\"imgattachbody\"></tbody></table></div>\r\n");
	templateBuilder.Append("				<div class=\"upbk\">\r\n");
	templateBuilder.Append("					<div id=\"imgattachbtnhidden\" style=\"display:none\"><span><form name=\"imgattachform\" id=\"imgattachform\" method=\"post\" autocomplete=\"off\" action=\"tools/attachupload.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" target=\"attachframe\" enctype=\"multipart/form-data\"><input type=\"hidden\" name=\"uid\" value=\"$_G['uid']\"><input type=\"hidden\" name=\"hash\" value=\"{echo md5(substr(md5($_G['config']['security']['authkey']), 8).$_G['uid'])}\"><input type=\"file\" name=\"Filedata\" size=\"45\" class=\"filedata\" /></form></span></div>\r\n");
	templateBuilder.Append("					<div id=\"imgattachbtn\"></div>\r\n");
	templateBuilder.Append("					<p id=\"imguploadbtn\">						\r\n");
	templateBuilder.Append("						<button class=\"pn\" type=\"button\" onclick=\"hideMenu();\"><span>取消</span></button>\r\n");
	templateBuilder.Append("						<button class=\"pn pnc\" type=\"button\" onclick=\"uploadAttach(0, 0, 'img')\"><span>上传</span></button>\r\n");
	templateBuilder.Append("					</p>\r\n");
	templateBuilder.Append("					<p id=\"imguploading\" style=\"display: none;\">上传中</p>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("				<div class=\"notice upnf\">\r\n");
	templateBuilder.Append("					文件尺寸: <strong>");
	templateBuilder.Append(FormatBytes(usergroupinfo.Maxattachsize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n");
	templateBuilder.Append("					上传限制: <strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong> 个文件&nbsp;&nbsp;\r\n");
	templateBuilder.Append("					<br />可用扩展名: <strong>");
	templateBuilder.Append(attachextensionsnosize.ToString());
	templateBuilder.Append("</strong>&nbsp;\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</div>		\r\n");
	templateBuilder.Append("		<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_imgattachlist\" style=\"display: none;\">\r\n");
	templateBuilder.Append("			<div class=\"upfilelist\">\r\n");
	templateBuilder.Append("		（有东西）\r\n");
	templateBuilder.Append("				<div id=\"imgattachlist\">\r\n");
	templateBuilder.Append("			（有东西）\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("				<div id=\"unusedimgattachlist\">\r\n");
	templateBuilder.Append("					（有东西）\r\n");
	templateBuilder.Append("				</div>				\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<p class=\"notice\" id=\"imgattach_notice\" style=\"display: none\">点击图片插入到帖子内容中.</p>\r\n");
	templateBuilder.Append("		</div>		\r\n");
	templateBuilder.Append("	<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_albumlist\" style=\"display: none;\">\r\n");
	templateBuilder.Append("		<div class=\"upfilelist\">\r\n");
	templateBuilder.Append("		从我的相册中选择图片\r\n");
	templateBuilder.Append("			<select onchange=\"if(this.value) {ajaxget('forum.php?mod=post&action=albumphoto&aid='+this.value, 'albumphoto');}\">\r\n");
	templateBuilder.Append("				<option value=\"\">选择相册</option>\r\n");
	templateBuilder.Append("				<!--{loop $albumlist $album}-->\r\n");
	templateBuilder.Append("					<option value=\"$album[albumid]\">$album[albumname]</option>\r\n");
	templateBuilder.Append("				<!--{/loop}-->\r\n");
	templateBuilder.Append("			</select>\r\n");
	templateBuilder.Append("			<div id=\"albumphoto\"></div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<p class=\"notice\">点击图片插入到帖子内容，\"attach://\" 开头的附件地址支持任何人下载请谨慎添加</p>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<iframe name=\"attachframe\" id=\"attachframe\" style=\"display: none;\" onload=\"uploadNextAttach();\"></iframe>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\"  reload=\"1\">\r\n");
	templateBuilder.Append("     //获取silverlight插件已经上传的附件列表  //sl上传完返回\r\n");
	templateBuilder.Append("    function getAttachmentList(sender, args) {\r\n");
	templateBuilder.Append("        var attachment = args.AttchmentList;\r\n");
	templateBuilder.Append("        if (isUndefined(attachment) || attachment == '[]') {\r\n");
	templateBuilder.Append("            if (infloat == 1) {\r\n");
	templateBuilder.Append("                pagescrolls('swfreturn'); return false;\r\n");
	templateBuilder.Append("            }\r\n");
	templateBuilder.Append("            else { swfuploadwin(); return; }\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("        var attachmentList = eval(\"(\" + attachment + \")\");\r\n");
	templateBuilder.Append("        switchAttachbutton('attachlist');\r\n");
	templateBuilder.Append("        updateAttachList();\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("    function onLoad(plugin, userContext, sender) {\r\n");
	templateBuilder.Append("        //只读属性,标识 Silverlight 插件是否已经加载。\r\n");
	templateBuilder.Append("        //if (sender.getHost().IsLoaded) {\r\n");
	templateBuilder.Append("        $(\"MultiUploadFile\").content.JavaScriptObject.UploadAttchmentList = getAttachmentList;\r\n");
	templateBuilder.Append("        // }\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<div class=\"p_pof upf\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attach_menu\" style=\"display: none;width:600px;\" unselectable=\"on\">\r\n");
	templateBuilder.Append("		<span class=\"y\"><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideMenu()\">关闭</a></span>\r\n");
	templateBuilder.Append("		<ul class=\"imguptype\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attach_ctrl\">\r\n");
	templateBuilder.Append("			<li><a href=\"javascript:;\" hidefocus=\"true\" class=\"current\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_btn_attachlist\" onclick=\"switchAttachbutton('attachlist');\">附件列表</a></li>\r\n");
	templateBuilder.Append("			<li><a href=\"javascript:;\" hidefocus=\"true\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_btn_upload\" onclick=\"switchAttachbutton('upload');\">普通上传</a></li>\r\n");
	templateBuilder.Append("			<li><a href=\"javascript:;\" hidefocus=\"true\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_btn_swfupload\" onclick=\"switchAttachbutton('swfupload');\">批量上传</a></li>\r\n");
	templateBuilder.Append("		</ul>\r\n");
	templateBuilder.Append("			<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_upload\" style=\"display: none;\">\r\n");
	templateBuilder.Append("				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n");
	templateBuilder.Append("					<tbody id=\"attachbodyhidden\" style=\"display:none\"><tr>\r\n");
	templateBuilder.Append("						<td class=\"atnu\"><span id=\"localno[]\"><img src=\"images/attachicons/common_new.gif\" /></span></td>\r\n");
	templateBuilder.Append("						<td class=\"atna\">\r\n");
	templateBuilder.Append("							<span id=\"deschidden[]\" style=\"display:none\">\r\n");
	templateBuilder.Append("								<span id=\"localfile[]\"></span>\r\n");
	templateBuilder.Append("							</span>\r\n");
	templateBuilder.Append("							<input type=\"hidden\" name=\"localid\" />\r\n");
	templateBuilder.Append("						</td>\r\n");
	templateBuilder.Append("						<td class=\"attc delete_msg\"><span id=\"cpdel[]\"></span></td>\r\n");
	templateBuilder.Append("					</tr></tbody>\r\n");
	templateBuilder.Append("				</table>\r\n");
	templateBuilder.Append("				<div class=\"p_tbl\"><table cellpadding=\"0\" cellspacing=\"0\" summary=\"post_attachbody\" border=\"0\" width=\"100%\"><tbody id=\"attachbody\"></tbody></table></div>\r\n");
	templateBuilder.Append("				<div class=\"upbk\">\r\n");
	templateBuilder.Append("					<div id=\"attachbtnhidden\" style=\"display:none\"><span><form name=\"attachform\" id=\"attachform\" method=\"post\" autocomplete=\"off\" action=\"tools/attachupload.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" target=\"attachframe\" enctype=\"multipart/form-data\"><input type=\"hidden\" name=\"uid\" value=\"$_G['uid']\"><input type=\"hidden\" name=\"hash\" value=\"{echo md5(substr(md5($_G['config']['security']['authkey']), 8).$_G['uid'])}\"><input type=\"file\" name=\"Filedata\" size=\"45\" class=\"fldt\" /></form></span></div>\r\n");
	templateBuilder.Append("					<div id=\"attachbtn\"></div>\r\n");
	templateBuilder.Append("					<p id=\"uploadbtn\">\r\n");
	templateBuilder.Append("						<button type=\"button\" class=\"pn\" onclick=\"hideMenu();\"><span>取消</span></button>\r\n");
	templateBuilder.Append("						<button type=\"button\" class=\"pn pnc\" onclick=\"uploadAttach(0, 0)\"><span>上传</span></button>\r\n");
	templateBuilder.Append("					</p>\r\n");
	templateBuilder.Append("					<p id=\"uploading\" style=\"display: none;\"><img src=\"images/common/uploading.gif\" style=\"vertical-align: middle;\" /> 上传中，请稍候，您可以<a href=\"javascript:;\" onclick=\"hideMenu()\">暂时关闭这个小窗口</a>，上传完成后您会收到通知。</p>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("				<div class=\"notice upnf\">\r\n");
	templateBuilder.Append("					文件尺寸: <strong>");
	templateBuilder.Append(FormatBytes(usergroupinfo.Maxattachsize).ToString().Trim());
	templateBuilder.Append("</strong>&nbsp;\r\n");
	templateBuilder.Append("					上传限制: <strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong> 个文件&nbsp;&nbsp;\r\n");
	templateBuilder.Append("					<br />可用扩展名: <strong>");
	templateBuilder.Append(attachextensionsnosize.ToString());
	templateBuilder.Append("</strong>&nbsp;\r\n");
	templateBuilder.Append("				</div>				\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			<div class=\"p_opt\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_swfupload\" style=\"display: none;\">\r\n");
	templateBuilder.Append("				<div class=\"floatboxswf\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_multiattach\">\r\n");

						string authToken=Discuz.Common.DES.Encode(oluserinfo.Olid.ToString() + "," + oluserinfo.Username.ToString(), oluserinfo.Password.Substring(0, 10)).Replace("+", "[");
						

	if (pagename.IndexOf("goods")<0 && config.Silverlight==1)
	{

	templateBuilder.Append("					<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/uploadfile/silverlight.js\" reload=\"1\"></");
	templateBuilder.Append("script> \r\n");
	templateBuilder.Append("					<div id=\"swfbox\"> \r\n");
	templateBuilder.Append("					<object  id=\"MultiUploadFile\" data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" Width=\"100%\" Height=\"340\">\r\n");
	templateBuilder.Append("					<param name=\"source\" value=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/UploadFile/ClientBin/MultiFileUpload.xap\"/>\r\n");
	templateBuilder.Append("					<param name=\"onError\" value=\"onSilverlightError\" />\r\n");
	templateBuilder.Append("					<param name=\"onLoad\" value=\"onLoad\" />\r\n");
	templateBuilder.Append("					<param name=\"background\" value=\"aliceblue\" />\r\n");
	templateBuilder.Append("					<param name=\"minRuntimeVersion\" value=\"4.0.50401.0\" />\r\n");
	templateBuilder.Append("					<param name=\"autoUpgrade\" value=\"true\" />\r\n");
	templateBuilder.Append("					<param name=\"initParams\" value=\"forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append(",authToken=");
	templateBuilder.Append(authToken.ToString());
	templateBuilder.Append(",max=");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("\" />		  \r\n");
	templateBuilder.Append("					<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0\" style=\"text-decoration:none\" target=\"_blank\">\r\n");
	templateBuilder.Append("					<img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("silverlight/uploadfile/uploadfile.jpg\" alt=\"安装微软Silverlight控件,即刻使用批量上传附件\" style=\"border-style:none\"/>\r\n");
	templateBuilder.Append("					</a>\r\n");
	templateBuilder.Append("					</object></div>\r\n");

	}	//end if

	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("				<div class=\"notice uploadinfo\">\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		<div class=\"p_opt post_tablelist\" unselectable=\"on\" id=\"");
	templateBuilder.Append(editorid.ToString());
	templateBuilder.Append("_attachlist\">\r\n");
	templateBuilder.Append("				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" id=\"attachlist_edittablist\">\r\n");
	templateBuilder.Append("					<tbody>\r\n");
	templateBuilder.Append("					    <tr>\r\n");
	templateBuilder.Append("						<td class=\"atnu\">&nbsp;</td>\r\n");
	templateBuilder.Append("						<td class=\"atna\">文件名&nbsp;(<a class=\"xg2\" href=\"javascript:;\" onclick=\"insertAllAttachTag();return false;\" style=\"margin:0 4px;\">插入全部附件</a>)</td>\r\n");
	templateBuilder.Append("						<td class=\"atds\">描述</td>\r\n");

	if (userid!=-1 && usergroupinfo.Allowsetattachperm==1)
	{

	templateBuilder.Append("<td class=\"attv\">阅读权限</td>\r\n");

	}	//end if


	if (topicattachscorefield>0 && usergroupinfo.Maxprice>0)
	{

	templateBuilder.Append("<td class=\"attp\">");
	templateBuilder.Append(Scoresets.GetTopicAttachCreditsTransName().ToString().Trim());
	templateBuilder.Append("</td>\r\n");

	}	//end if


	if (config.Enablealbum==1 && caninsertalbum)
	{

	templateBuilder.Append("						<td>选择相册</td>\r\n");

	}	//end if

	templateBuilder.Append("						<td class=\"attc delete_msg\"></td>\r\n");
	templateBuilder.Append("					   </tr>\r\n");
	templateBuilder.Append("					</tbody>\r\n");

	if (action=="edit")
	{


	int attachment__loop__id=0;
	foreach(DataRow attachment in attachmentlist.Rows)
	{
		attachment__loop__id++;


	if (Utils.StrToInt(attachment["pid"].ToString().Trim(), 0)==postinfo.Pid)
	{

	string filetypeimage = "";
	
	int isimage = 0;
	
	string inserttype = "";
	

	if (attachment["filetype"].ToString().Trim().IndexOf("image")>-1)
	{

	 filetypeimage = "image.gif";
	
	 inserttype = "insertAttachimgTag";
	
	 isimage = 1;
	

	}
	else
	{

	 inserttype = "insertAttachTag";
	

	if (Utils.GetFileExtName(attachment["attachment"].ToString().Trim())=="rar" || Utils.GetFileExtName(attachment["attachment"].ToString().Trim())=="zip")
	{

	 filetypeimage = "rar.gif";
	

	}
	else
	{

	 filetypeimage = "attachment.gif";
	

	}	//end if


	}	//end if

	templateBuilder.Append("					<tbody id=\"attach_" + attachment["aid"].ToString().Trim() + "\">\r\n");
	templateBuilder.Append("					<tr>\r\n");
	templateBuilder.Append("					<td class=\"atnu\">\r\n");
	templateBuilder.Append("					<img id=\"attach" + attachment["aid"].ToString().Trim() + "_type\" border=\"0\" src=\"images/attachicons/");
	templateBuilder.Append(filetypeimage.ToString());
	templateBuilder.Append("\" class=\"vm\" alt=\"\">\r\n");
	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("					<td class=\"atna\">\r\n");
	templateBuilder.Append("					<span id=\"attach" + attachment["aid"].ToString().Trim() + "\">\r\n");
	templateBuilder.Append("					<a id=\"attachname" + attachment["aid"].ToString().Trim() + "\" onclick=\"");
	templateBuilder.Append(inserttype.ToString());
	templateBuilder.Append("(" + attachment["aid"].ToString().Trim() + ")\" href=\"javascript:;\" isimage=\"");
	templateBuilder.Append(isimage.ToString());
	templateBuilder.Append("\" title=\"" + attachment["attachment"].ToString().Trim() + "\">\r\n");
	templateBuilder.Append(Utils.GetUnicodeSubString(attachment["attachment"].ToString().Trim(),25,"..."));
	templateBuilder.Append("</a> \r\n");
	templateBuilder.Append(" 					<a href=\"javascript:;\" class=\"atturl\" title=\"添加附件地址\" onclick=\"insertText('attach://')\">\r\n");
	templateBuilder.Append("					<img src=\"images/attachicons/attachurl.gif\">\r\n");
	templateBuilder.Append("					</a>\r\n");
	templateBuilder.Append("					</span>\r\n");
	templateBuilder.Append("					<span id=\"attachupdate" + attachment["aid"].ToString().Trim() + "\" style=\"display:none;\">\r\n");
	templateBuilder.Append("					<form enctype=\"multipart/form-data\" target=\"attachframe\" action=\"tools/attachupload.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&aid=" + attachment["aid"].ToString().Trim() + "\" method=\"post\" id=\"attachform_" + attachment["aid"].ToString().Trim() + "\" name=\"attachform_" + attachment["aid"].ToString().Trim() + "\" style=\"float:left;\">\r\n");
	templateBuilder.Append("					<input type=\"file\" name=\"Filedata\" size=\"8\" />\r\n");
	templateBuilder.Append("					<input type=\"hidden\" value=\"" + attachment["aid"].ToString().Trim() + "\" name=\"attachupdatedid\" />\r\n");
	templateBuilder.Append("					<input type=\"submit\" value=\"上传\" />\r\n");
	templateBuilder.Append("					</form>\r\n");
	templateBuilder.Append("					</span>\r\n");
	templateBuilder.Append("					<a id=\"attach" + attachment["aid"].ToString().Trim() + "_opt\" href=\"javascript:;\" class=\"right\" onclick=\"attachupdate('" + attachment["aid"].ToString().Trim() + "', this)\">更新</a>\r\n");
	templateBuilder.Append("					<input type=\"hidden\" value=\"" + attachment["aid"].ToString().Trim() + "\" name=\"attachid\" />\r\n");

	if (isimage==1)
	{

	templateBuilder.Append("					    <img alt=\"\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("attachment.aspx?attachmentid=" + attachment["aid"].ToString().Trim() + "\" id=\"image_" + attachment["aid"].ToString().Trim() + "\" width=\"" + attachment["height"].ToString().Trim() + "\" style=\"position: absolute; top: -10000px;\"/>\r\n");

	}	//end if

	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("					<td class=\"atds\"><input type=\"text\" name=\"attachdesc\" size=\"18\" class=\"txt\" value=\"" + attachment["description"].ToString().Trim() + "\"></td>\r\n");
	templateBuilder.Append("					<td class=\"attv\"><input type=\"text\" size=\"1\" value=\"" + attachment["readperm"].ToString().Trim() + "\" name=\"readperm\"></td>\r\n");
	templateBuilder.Append("					<td class=\"attp\"><input type=\"text\" size=\"1\" value=\"" + attachment["attachprice"].ToString().Trim() + "\" name=\"attachprice\"></td>\r\n");
	templateBuilder.Append("					<td class=\"attc delete_msg\"><a href=\"javascript:;\" class=\"d\" onclick=\"delAttach('" + attachment["aid"].ToString().Trim() + ",");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("',1)\">删除</a></td>\r\n");
	templateBuilder.Append("					</tr></tbody>\r\n");

	}	//end if


	}	//end loop


	}	//end if

	templateBuilder.Append("				</table>\r\n");
	templateBuilder.Append("				<div id=\"attachlist_tablist_current\"></div>\r\n");
	templateBuilder.Append("				<div id=\"attachlist_tablist\"></div>\r\n");
	templateBuilder.Append("				<p class=\"ptm\" id=\"attach_notice\" style=\"display: none\" >点击文件名插入到帖子内容中</p>\r\n");

	if (infloat==0)
	{

	templateBuilder.Append("				<div id=\"uploadlist\" class=\"upfilelist\" style=\"height:auto\">\r\n");

	}
	else
	{

	templateBuilder.Append("				<div id=\"uploadlist\" class=\"upfilelist\">\r\n");

	}	//end if

	templateBuilder.Append("				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n");

	if (pagename.IndexOf("goods")<0 && config.Silverlight==1)
	{

	templateBuilder.Append("					<tbody id=\"attachuploadedhidden\" style=\"display:none\"><tr>\r\n");
	templateBuilder.Append("						<td class=\"attachnum\"><span id=\"sl_localno[]\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("images/attachicons/common_new.gif\" /></span></td>\r\n");
	templateBuilder.Append("						<td class=\"attachctrl\"><span id=\"sl_cpadd[]\"></span></td>\r\n");
	templateBuilder.Append("						<td class=\"attachname\">\r\n");
	templateBuilder.Append("							<span id=\"sl_deschidden[]\" style=\"display:none\">\r\n");
	templateBuilder.Append("								<a href=\"javascript:;\" onclick='parentNode.innerHTML=\"<input type=\\\"text\\\" name=\\\"attachdesc\\\" size=\\\"25\\\" class=\\\"txt\\\" />\"'>描述</a>\r\n");
	templateBuilder.Append("								<span id=\"attachfile[]\"></span>\r\n");
	templateBuilder.Append("								<input type=\"text\" name=\"sl_attachdesc\" style=\"display:none\" />\r\n");
	templateBuilder.Append("							</span>\r\n");
	templateBuilder.Append("						</td>\r\n");

	if (userid!=-1 && usergroupinfo.Allowsetattachperm==1)
	{

	templateBuilder.Append("<td class=\"attachview\"><input type=\"text\" name=\"sl_readperm\" value=\"0\"size=\"1\" class=\"txt\" /></td>\r\n");

	}	//end if


	if (topicattachscorefield>0 && usergroupinfo.Maxprice>0)
	{

	templateBuilder.Append("<td class=\"attachpr\"><input type=\"text\" name=\"sl_attachprice\" value=\"0\" size=\"1\" class=\"txt\" /></td>\r\n");

	}	//end if


	if (config.Enablealbum==1 && caninsertalbum)
	{

	templateBuilder.Append("							<td  style=\"vertical-align:top;\">\r\n");
	templateBuilder.Append("								<select name=\"sl_albums\" style=\"display:none\">\r\n");
	templateBuilder.Append("								<option value=\"0\"></option>\r\n");

	int album__loop__id=0;
	foreach(DataRow album in albumlist.Rows)
	{
		album__loop__id++;

	templateBuilder.Append("								<option value=\"" + album["albumid"].ToString().Trim() + "\">" + album["title"].ToString().Trim() + "</option>\r\n");

	}	//end loop

	templateBuilder.Append("								</select>\r\n");
	templateBuilder.Append("							</td>\r\n");

	}	//end if

	templateBuilder.Append("						<td class=\"attachdel\"><span id=\"sl_cpdel[]\"></span></td>\r\n");
	templateBuilder.Append("					</tr></tbody>\r\n");

	}	//end if

	templateBuilder.Append("					<tbody id=\"attachbodyhidden\" style=\"display:none\"><tr>\r\n");
	templateBuilder.Append("						<td class=\"attachnum\"><span id=\"localno[]\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("images/attachicons/common_new.gif\" /></span></td>\r\n");
	templateBuilder.Append("						<td class=\"attachctrl\"><span id=\"cpadd[]\"></span></td>\r\n");
	templateBuilder.Append("						<td class=\"attachname\">\r\n");
	templateBuilder.Append("							<span id=\"deschidden[]\" style=\"display:none\">\r\n");
	templateBuilder.Append("								<a href=\"javascript:;\" onclick='parentNode.innerHTML=\"<input type=\\\"text\\\" name=\\\"attachdesc\\\" size=\\\"25\\\" class=\\\"txt\\\" />\"'>描述</a>\r\n");
	templateBuilder.Append("								<span id=\"localfile[]\"></span>\r\n");
	templateBuilder.Append("							</span>\r\n");
	templateBuilder.Append("							<input type=\"hidden\" name=\"localid\" />\r\n");
	templateBuilder.Append("						</td>\r\n");

	if (config.Enablealbum==1 && caninsertalbum)
	{

	templateBuilder.Append("							<td  style=\"vertical-align:top;\">\r\n");

	if (albumlist.Rows.Count!=0)
	{

	templateBuilder.Append("								<select name=\"albums\"  style=\"display:none\">\r\n");
	templateBuilder.Append("								<option value=\"0\"></option>\r\n");

	int album__loop__id=0;
	foreach(DataRow album in albumlist.Rows)
	{
		album__loop__id++;

	templateBuilder.Append("								<option value=\"" + album["albumid"].ToString().Trim() + "\">" + album["title"].ToString().Trim() + "</option>\r\n");

	}	//end loop

	templateBuilder.Append("								</select>\r\n");

	}	//end if

	templateBuilder.Append("							</td>\r\n");

	}	//end if

	templateBuilder.Append("						<td class=\"attachdel\"><span id=\"cpdel[]\"></span></td>\r\n");
	templateBuilder.Append("					</tr></tbody>\r\n");
	templateBuilder.Append("				</table>\r\n");

	if (pagename.IndexOf("goods")<0 && config.Silverlight==1)
	{

	templateBuilder.Append("				<div id=\"swfattachlist\">\r\n");
	templateBuilder.Append("					<table cellspacing=\"0\" cellpadding=\"0\" id=\"attachuploadednote\" style=\"display:none;\">\r\n");
	templateBuilder.Append("						<tbody>\r\n");
	templateBuilder.Append("							<tr>\r\n");
	templateBuilder.Append("								<td class=\"attachnum\"></td>\r\n");
	templateBuilder.Append("								<td>您有 <span id=\"attachuploadednotenum\"></span> 个已经上传的附件<span id=\"maxattachnote\" style=\"display: none;\">, 只能使用前<span id=\"num2upload2\"><strong>");
	templateBuilder.Append(config.Maxattachments.ToString().Trim());
	templateBuilder.Append("</strong></span>个</span>  \r\n");
	templateBuilder.Append("								<a onclick=\"addAttachUploaded(attaches);\" href=\"javascript:;\">使用</a>   <a onclick=\"attachlist()\" href=\"javascript:;\">忽略</a>\r\n");
	templateBuilder.Append("								</td>\r\n");
	templateBuilder.Append("							</tr>\r\n");
	templateBuilder.Append("						</tbody>\r\n");
	templateBuilder.Append("					</table>\r\n");
	templateBuilder.Append("				</div>\r\n");

	}	//end if

	templateBuilder.Append("				<table cellpadding=\"0\" cellspacing=\"0\" summary=\"post_attachbody\" border=\"0\" width=\"100%\"><tbody id=\"attachuploaded\"></tbody><tbody id=\"attachbody\"></tbody></table>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("<div id=\"img_hidden\" alt=\"1\" style=\"position:absolute;top:-100000px;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image');width:");
	templateBuilder.Append(thumbwidth.ToString());
	templateBuilder.Append("px;height:");
	templateBuilder.Append(thumbheight.ToString());
	templateBuilder.Append("px\"></div>		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("<Script>\r\n");
	templateBuilder.Append("	var editorform = $('testform');\r\n");
	templateBuilder.Append("	var editorsubmit = $('testsubmit');\r\n");
	templateBuilder.Append("	if(wysiwyg) {\r\n");
	templateBuilder.Append("		newEditor(1, bbcode2html(textobj.value));\r\n");
	templateBuilder.Append("	} else {\r\n");
	templateBuilder.Append("		newEditor(0, textobj.value);\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	if (getQueryString('cedit') == 'yes')\r\n");
	templateBuilder.Append("	{\r\n");
	templateBuilder.Append("		loadData(true);\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script>\r\n");
	templateBuilder.Append("var TABLEBG = '#FFF';//'{  WRAPBG  }';\r\n");
	templateBuilder.Append("var uid = parseInt('");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("var special = parseInt('0');\r\n");
	templateBuilder.Append("var charset = 'utf-8';\r\n");
	templateBuilder.Append("var thumbwidth = parseInt(400);\r\n");
	templateBuilder.Append("var thumbheight = parseInt(300);\r\n");
	templateBuilder.Append("var extensions = '");
	templateBuilder.Append(attachextensions.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("var ATTACHNUM = {'imageused':0,'imageunused':0,'attachused':0,'attachunused':0};\r\n");
	templateBuilder.Append("function switchImagebutton(btn) {\r\n");
	templateBuilder.Append("var btns = ['www', 'albumlist'];\r\n");
	templateBuilder.Append("switchButton(btn, btns);\r\n");
	templateBuilder.Append("$(editorid + '_image_menu').style.height = '';\r\n");
	templateBuilder.Append("}\r\n");
	templateBuilder.Append("function switchAttachbutton(btn) {\r\n");
	templateBuilder.Append("var btns = ['attachlist'];\r\n");
	templateBuilder.Append("btns.push('upload');btns.push('swfupload');switchButton(btn, btns);\r\n");
	templateBuilder.Append("}\r\n");
	templateBuilder.Append("ATTACHNUM['attachused'] = 0;\r\n");
	templateBuilder.Append("ATTACHNUM['attachunused'] = 0;\r\n");

	if (userid!=-1)
	{

	templateBuilder.Append("updateAttachList();\r\n");

	}	//end if

	templateBuilder.Append("switchAttachbutton('upload');\r\n");
	templateBuilder.Append("setCaretAtEnd();\r\n");
	templateBuilder.Append("if(BROWSER.ie >= 5 || BROWSER.firefox >= '2') {\r\n");
	templateBuilder.Append("_attachEvent(window, 'beforeunload', saveData);\r\n");
	templateBuilder.Append("}\r\n");

	if (userid!=-1)
	{

	templateBuilder.Append("getunusedattachlist_dialog();\r\n");

	}	//end if

	templateBuilder.Append("addAttach();\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");


	templateBuilder.Append("		<div style=\"display: none;\">\r\n");
	templateBuilder.Append("			<p class=\"textmsg\" id=\"divshowuploadmsg\" style=\"display:none\"></p>\r\n");
	templateBuilder.Append("			<p class=\"textmsg succ\" id=\"divshowuploadmsgok\" style=\"display:none\"></p>\r\n");
	templateBuilder.Append("			<input type=\"hidden\" name=\"uploadallowmax\" value=\"10\">\r\n");
	templateBuilder.Append("			<input type=\"hidden\" name=\"uploadallowtype\" value=\"jpg,gif\">\r\n");
	templateBuilder.Append("			<input type=\"hidden\" name=\"thumbwidth\" value=\"300\">\r\n");
	templateBuilder.Append("			<input type=\"hidden\" name=\"thumbheight\" value=\"250\">\r\n");
	templateBuilder.Append("			<input type=\"hidden\" name=\"noinsert\" value=\"0\">\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div class=\"margint\" id=\"realmoreinfo\" style=\"display: none;\">\r\n");
	templateBuilder.Append("			<div id=\"newpost\"><input name=\"continuereply\" type=\"checkbox\" \r\n");

	if (continuereply!="")
	{

	templateBuilder.Append("checked\r\n");

	}	//end if

	templateBuilder.Append(" /> 连续回复</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<script type=\"text/javascript\">$(\"moreinfo\").innerHTML = $(\"realmoreinfo\").innerHTML;$(\"moreinfo\").className=\"margint\"</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	</form>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" reload=\"1\" >\r\n");
	templateBuilder.Append("	if (getQueryString('restore') == 1)\r\n");
	templateBuilder.Append("	{\r\n");
	templateBuilder.Append("		loadData(true);\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}
	else
	{


	if (ispost)
	{


	if (needlogin)
	{



	if (infloat!=1)
	{

	templateBuilder.Append("<div class=\"wrap s_clear\" id=\"wrap\">\r\n");
	templateBuilder.Append("<div class=\"main login\">\r\n");
	templateBuilder.Append("	<div class=\"message\">\r\n");
	templateBuilder.Append("		<h1>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" 提示信息</h1>\r\n");
	templateBuilder.Append("		<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n");
	templateBuilder.Append("		<p><b>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</b></p>\r\n");
	templateBuilder.Append("		<p>您还没有登录，请填写下面的登录表单后再尝试访问。</p>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<hr class=\"solidline\"/>\r\n");
	templateBuilder.Append("	<div class=\"nojs\">\r\n");
	templateBuilder.Append("	<div class=\"float\" id=\"floatlayout_login\" style=\"width: 600px; height: 300px;\">\r\n");
	templateBuilder.Append("	<form id=\"formlogin\" name=\"formlogin\" method=\"post\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx\" onsubmit=\"submitLogin(this);\">\r\n");
	templateBuilder.Append("	<input type=\"hidden\" value=\"2592000\" name=\"cookietime\"/>\r\n");
	templateBuilder.Append("		<h1 style=\"margin-bottom:10px;\">会员登录</h1>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" class=\"formtable\">\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"username\">用户名</label></th>\r\n");
	templateBuilder.Append("				<td><input type=\"text\" id=\"username\" name=\"username\" size=\"25\" maxlength=\"40\" tabindex=\"2\" class=\"txt\" />  <a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx\" tabindex=\"-1\" accesskey=\"r\" title=\"立即注册 (ALT + R)\" class=\"lightlink\">立即注册</a>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"password\">密码</label></th>\r\n");
	templateBuilder.Append("				<td><input type=\"password\" name=\"password\" size=\"25\" tabindex=\"3\" class=\"txt\"/> <a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("getpassword.aspx\" tabindex=\"-1\" accesskey=\"g\" title=\"忘记密码 (ALT + G)\" class=\"lightlink\">忘记密码</a>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");

	if (isLoginCode)
	{

	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"vcode\">验证码</label></th>\r\n");
	templateBuilder.Append("                <td>\r\n");
	templateBuilder.Append("				<div style=\"position: relative;margin-bottom:10px;\">\r\n");

	templateBuilder.Append("<div id=\"vcode_temp\"></div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	var infloat = ");
	templateBuilder.Append(infloat.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("	if (typeof vcodeimgid == 'undefined'){\r\n");
	templateBuilder.Append("		var vcodeimgid = 1;\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	else\r\n");
	templateBuilder.Append("	    vcodeimgid++;\r\n");
	templateBuilder.Append("    $('vcode_temp').parentNode.innerHTML = '<input name=\"vcodetext\" tabindex=\"1002\" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:90px;\" id=\"vcodetext' + vcodeimgid + '\"  onblur=\"if(!seccodefocus) {display(this.id + \\'_menu\\')};\"  onfocus=\"opensecwin('+vcodeimgid+',1)\"   value=\"验证码\" autocomplete=\"off\"/>' +\r\n");
	templateBuilder.Append("	                                       '<div class=\"seccodecontent\"  style=\"display:none;cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n");
	templateBuilder.Append("	function changevcode(form, value){\r\n");
	templateBuilder.Append("		if (!$('vcode')){\r\n");
	templateBuilder.Append("			var vcode = document.createElement('input');\r\n");
	templateBuilder.Append("			vcode.id = 'vcode';\r\n");
	templateBuilder.Append("			vcode.name = 'vcode';\r\n");
	templateBuilder.Append("			vcode.type = 'hidden';\r\n");
	templateBuilder.Append("			vcode.value = value;\r\n");
	templateBuilder.Append("			form.appendChild(vcode);\r\n");
	templateBuilder.Append("		}else{\r\n");
	templateBuilder.Append("			$('vcode').value = value;\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("var secclick = new Array();\r\n");
	templateBuilder.Append("var seccodefocus = 0;\r\n");
	templateBuilder.Append("function opensecwin(id,type) {\r\n");
	templateBuilder.Append("	if($('vcode')){\r\n");
	templateBuilder.Append("	$('vcode').parentNode.removeChild($('vcode'));}\r\n");
	templateBuilder.Append("	if (!secclick['vcodetext' + id]) {\r\n");
	templateBuilder.Append("	    $('vcodetext' + id).value = '';\r\n");
	templateBuilder.Append("	    secclick['vcodetext' + id] = 1;\r\n");
	templateBuilder.Append("	    if(type)\r\n");
	templateBuilder.Append("	        $('vcodetext' + id + '_menu').style.top = parseInt($('vcodetext' + id + '_menu').style.top) - parseInt($('vcodetext' + id + '_menu').style.height) + 'px';\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.position = 'absolute';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.top = (-parseInt($('vcodetext' + id + '_menu').style.height) - 2) + 'px';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.left = '0px';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.display = '';\r\n");
	templateBuilder.Append("	$('vcodetext' + id).focus();\r\n");
	templateBuilder.Append("	$('vcodetext' + id).unselectable = 'off';\r\n");
	templateBuilder.Append("	$('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n");
	templateBuilder.Append("}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");


	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");

	}	//end if


	if (config.Secques==1)
	{

	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"questionid\">安全问题</label></th>\r\n");
	templateBuilder.Append("				<td>\r\n");
	templateBuilder.Append("					<select name=\"questionid\" tabindex=\"4\">\r\n");
	templateBuilder.Append("					<option value=\"0\">&nbsp;</option>\r\n");
	templateBuilder.Append("					<option value=\"1\">母亲的名字</option>\r\n");
	templateBuilder.Append("					<option value=\"2\">爷爷的名字</option>\r\n");
	templateBuilder.Append("					<option value=\"3\">父亲出生的城市</option>\r\n");
	templateBuilder.Append("					<option value=\"4\">您其中一位老师的名字</option>\r\n");
	templateBuilder.Append("					<option value=\"5\">您个人计算机的型号</option>\r\n");
	templateBuilder.Append("					<option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
	templateBuilder.Append("					<option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
	templateBuilder.Append("					</select>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"answer\">答案</label></th>\r\n");
	templateBuilder.Append("				<td><input type=\"text\" name=\"answer\" size=\"25\" tabindex=\"5\" class=\"txt\" /></td>\r\n");
	templateBuilder.Append("			</tr>\r\n");

	}	//end if

	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th> </th>\r\n");
	templateBuilder.Append("				<td>\r\n");
	templateBuilder.Append("					<input type=\"submit\" style=\"width:0;filter:alpha(opacity=0);-moz-opacity:0;opacity:0;\"/>\r\n");
	templateBuilder.Append("					<button type=\"submit\" name=\"loginsubmit\" id=\"loginsubmit\" tabindex=\"6\" class=\"pn\"><span>登录</span></button>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");
	templateBuilder.Append("	</form>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	}
	else
	{

	templateBuilder.Append("<div class=\"msgbox\" style=\"background:none;text-align:left;\">\r\n");
	templateBuilder.Append("	<div class=\"msg_inner error_msg\">\r\n");
	templateBuilder.Append("		<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n");
	templateBuilder.Append("		<p><b>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</b></p>\r\n");
	templateBuilder.Append("		<p>您还没有登录，请的登录后再尝试访问。</p>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	setTimeout(\"floatwin('close_newthread');floatwin('close_reply');floatwin('close_edit');floatwin('open_login', '");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("login.aspx', 600, 410)\",1000);\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("<script type=\"text/javascript\">\r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("		document.getElementById(\"username\").focus();\r\n");

	}	//end if

	templateBuilder.Append("		function submitLogin(loginForm)\r\n");
	templateBuilder.Append("		{\r\n");
	templateBuilder.Append("//		    loginForm.action = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?loginsubmit=true&reurl=' + escape(window.location);\r\n");
	templateBuilder.Append("            loginForm.action = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?reurl=' + escape(window.location);\r\n");
	templateBuilder.Append("			loginForm.submit();\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");



	}
	else
	{


	templateBuilder.Append("<div class=\"wrap cl\">\r\n");
	templateBuilder.Append("<div class=\"main\">\r\n");
	templateBuilder.Append("	<div class=\"msgbox\">\r\n");
	templateBuilder.Append("		<h1>出现了");
	templateBuilder.Append(page_err.ToString());
	templateBuilder.Append("个错误</h1>\r\n");
	templateBuilder.Append("		<hr class=\"solidline\"/>\r\n");
	templateBuilder.Append("		<div class=\"msg_inner error_msg\">\r\n");
	templateBuilder.Append("			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");
	templateBuilder.Append("			<p class=\"errorback\">\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("					if(");
	templateBuilder.Append(msgbox_showbacklink.ToString());
	templateBuilder.Append(")\r\n");
	templateBuilder.Append("					{\r\n");
	templateBuilder.Append("						document.write(\"<a href=\\\"");
	templateBuilder.Append(msgbox_backlink.ToString());
	templateBuilder.Append("\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
	templateBuilder.Append("					}\r\n");
	templateBuilder.Append("				</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("				<a href=\"forumindex.aspx\">论坛首页</a>\r\n");

	if (usergroupid==7)
	{

	templateBuilder.Append("				 &nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"login.aspx\">登录</a>&nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

	}	//end if

	templateBuilder.Append("			</p>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</div>\r\n");



	}	//end if


	}
	else
	{


	if (needlogin)
	{



	if (infloat!=1)
	{

	templateBuilder.Append("<div class=\"wrap s_clear\" id=\"wrap\">\r\n");
	templateBuilder.Append("<div class=\"main login\">\r\n");
	templateBuilder.Append("	<div class=\"message\">\r\n");
	templateBuilder.Append("		<h1>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" 提示信息</h1>\r\n");
	templateBuilder.Append("		<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n");
	templateBuilder.Append("		<p><b>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</b></p>\r\n");
	templateBuilder.Append("		<p>您还没有登录，请填写下面的登录表单后再尝试访问。</p>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<hr class=\"solidline\"/>\r\n");
	templateBuilder.Append("	<div class=\"nojs\">\r\n");
	templateBuilder.Append("	<div class=\"float\" id=\"floatlayout_login\" style=\"width: 600px; height: 300px;\">\r\n");
	templateBuilder.Append("	<form id=\"formlogin\" name=\"formlogin\" method=\"post\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx\" onsubmit=\"submitLogin(this);\">\r\n");
	templateBuilder.Append("	<input type=\"hidden\" value=\"2592000\" name=\"cookietime\"/>\r\n");
	templateBuilder.Append("		<h1 style=\"margin-bottom:10px;\">会员登录</h1>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" class=\"formtable\">\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"username\">用户名</label></th>\r\n");
	templateBuilder.Append("				<td><input type=\"text\" id=\"username\" name=\"username\" size=\"25\" maxlength=\"40\" tabindex=\"2\" class=\"txt\" />  <a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx\" tabindex=\"-1\" accesskey=\"r\" title=\"立即注册 (ALT + R)\" class=\"lightlink\">立即注册</a>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"password\">密码</label></th>\r\n");
	templateBuilder.Append("				<td><input type=\"password\" name=\"password\" size=\"25\" tabindex=\"3\" class=\"txt\"/> <a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("getpassword.aspx\" tabindex=\"-1\" accesskey=\"g\" title=\"忘记密码 (ALT + G)\" class=\"lightlink\">忘记密码</a>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");

	if (isLoginCode)
	{

	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"vcode\">验证码</label></th>\r\n");
	templateBuilder.Append("                <td>\r\n");
	templateBuilder.Append("				<div style=\"position: relative;margin-bottom:10px;\">\r\n");

	templateBuilder.Append("<div id=\"vcode_temp\"></div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	var infloat = ");
	templateBuilder.Append(infloat.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("	if (typeof vcodeimgid == 'undefined'){\r\n");
	templateBuilder.Append("		var vcodeimgid = 1;\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	else\r\n");
	templateBuilder.Append("	    vcodeimgid++;\r\n");
	templateBuilder.Append("    $('vcode_temp').parentNode.innerHTML = '<input name=\"vcodetext\" tabindex=\"1002\" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:90px;\" id=\"vcodetext' + vcodeimgid + '\"  onblur=\"if(!seccodefocus) {display(this.id + \\'_menu\\')};\"  onfocus=\"opensecwin('+vcodeimgid+',1)\"   value=\"验证码\" autocomplete=\"off\"/>' +\r\n");
	templateBuilder.Append("	                                       '<div class=\"seccodecontent\"  style=\"display:none;cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n");
	templateBuilder.Append("	function changevcode(form, value){\r\n");
	templateBuilder.Append("		if (!$('vcode')){\r\n");
	templateBuilder.Append("			var vcode = document.createElement('input');\r\n");
	templateBuilder.Append("			vcode.id = 'vcode';\r\n");
	templateBuilder.Append("			vcode.name = 'vcode';\r\n");
	templateBuilder.Append("			vcode.type = 'hidden';\r\n");
	templateBuilder.Append("			vcode.value = value;\r\n");
	templateBuilder.Append("			form.appendChild(vcode);\r\n");
	templateBuilder.Append("		}else{\r\n");
	templateBuilder.Append("			$('vcode').value = value;\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("var secclick = new Array();\r\n");
	templateBuilder.Append("var seccodefocus = 0;\r\n");
	templateBuilder.Append("function opensecwin(id,type) {\r\n");
	templateBuilder.Append("	if($('vcode')){\r\n");
	templateBuilder.Append("	$('vcode').parentNode.removeChild($('vcode'));}\r\n");
	templateBuilder.Append("	if (!secclick['vcodetext' + id]) {\r\n");
	templateBuilder.Append("	    $('vcodetext' + id).value = '';\r\n");
	templateBuilder.Append("	    secclick['vcodetext' + id] = 1;\r\n");
	templateBuilder.Append("	    if(type)\r\n");
	templateBuilder.Append("	        $('vcodetext' + id + '_menu').style.top = parseInt($('vcodetext' + id + '_menu').style.top) - parseInt($('vcodetext' + id + '_menu').style.height) + 'px';\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.position = 'absolute';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.top = (-parseInt($('vcodetext' + id + '_menu').style.height) - 2) + 'px';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.left = '0px';\r\n");
	templateBuilder.Append("	$('vcodetext' + id + '_menu').style.display = '';\r\n");
	templateBuilder.Append("	$('vcodetext' + id).focus();\r\n");
	templateBuilder.Append("	$('vcodetext' + id).unselectable = 'off';\r\n");
	templateBuilder.Append("	$('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n");
	templateBuilder.Append("}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");


	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");

	}	//end if


	if (config.Secques==1)
	{

	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"questionid\">安全问题</label></th>\r\n");
	templateBuilder.Append("				<td>\r\n");
	templateBuilder.Append("					<select name=\"questionid\" tabindex=\"4\">\r\n");
	templateBuilder.Append("					<option value=\"0\">&nbsp;</option>\r\n");
	templateBuilder.Append("					<option value=\"1\">母亲的名字</option>\r\n");
	templateBuilder.Append("					<option value=\"2\">爷爷的名字</option>\r\n");
	templateBuilder.Append("					<option value=\"3\">父亲出生的城市</option>\r\n");
	templateBuilder.Append("					<option value=\"4\">您其中一位老师的名字</option>\r\n");
	templateBuilder.Append("					<option value=\"5\">您个人计算机的型号</option>\r\n");
	templateBuilder.Append("					<option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
	templateBuilder.Append("					<option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
	templateBuilder.Append("					</select>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th><label for=\"answer\">答案</label></th>\r\n");
	templateBuilder.Append("				<td><input type=\"text\" name=\"answer\" size=\"25\" tabindex=\"5\" class=\"txt\" /></td>\r\n");
	templateBuilder.Append("			</tr>\r\n");

	}	//end if

	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<th> </th>\r\n");
	templateBuilder.Append("				<td>\r\n");
	templateBuilder.Append("					<input type=\"submit\" style=\"width:0;filter:alpha(opacity=0);-moz-opacity:0;opacity:0;\"/>\r\n");
	templateBuilder.Append("					<button type=\"submit\" name=\"loginsubmit\" id=\"loginsubmit\" tabindex=\"6\" class=\"pn\"><span>登录</span></button>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");
	templateBuilder.Append("	</form>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	}
	else
	{

	templateBuilder.Append("<div class=\"msgbox\" style=\"background:none;text-align:left;\">\r\n");
	templateBuilder.Append("	<div class=\"msg_inner error_msg\">\r\n");
	templateBuilder.Append("		<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n");
	templateBuilder.Append("		<p><b>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</b></p>\r\n");
	templateBuilder.Append("		<p>您还没有登录，请的登录后再尝试访问。</p>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("	setTimeout(\"floatwin('close_newthread');floatwin('close_reply');floatwin('close_edit');floatwin('open_login', '");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("login.aspx', 600, 410)\",1000);\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("<script type=\"text/javascript\">\r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("		document.getElementById(\"username\").focus();\r\n");

	}	//end if

	templateBuilder.Append("		function submitLogin(loginForm)\r\n");
	templateBuilder.Append("		{\r\n");
	templateBuilder.Append("//		    loginForm.action = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?loginsubmit=true&reurl=' + escape(window.location);\r\n");
	templateBuilder.Append("            loginForm.action = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?reurl=' + escape(window.location);\r\n");
	templateBuilder.Append("			loginForm.submit();\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");



	}
	else
	{


	templateBuilder.Append("<div class=\"wrap cl\">\r\n");
	templateBuilder.Append("<div class=\"main\">\r\n");
	templateBuilder.Append("	<div class=\"msgbox\">\r\n");
	templateBuilder.Append("		<h1>出现了");
	templateBuilder.Append(page_err.ToString());
	templateBuilder.Append("个错误</h1>\r\n");
	templateBuilder.Append("		<hr class=\"solidline\"/>\r\n");
	templateBuilder.Append("		<div class=\"msg_inner error_msg\">\r\n");
	templateBuilder.Append("			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");
	templateBuilder.Append("			<p class=\"errorback\">\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("					if(");
	templateBuilder.Append(msgbox_showbacklink.ToString());
	templateBuilder.Append(")\r\n");
	templateBuilder.Append("					{\r\n");
	templateBuilder.Append("						document.write(\"<a href=\\\"");
	templateBuilder.Append(msgbox_backlink.ToString());
	templateBuilder.Append("\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
	templateBuilder.Append("					}\r\n");
	templateBuilder.Append("				</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("				<a href=\"forumindex.aspx\">论坛首页</a>\r\n");

	if (usergroupid==7)
	{

	templateBuilder.Append("				 &nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"login.aspx\">登录</a>&nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

	}	//end if

	templateBuilder.Append("			</p>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</div>\r\n");


	templateBuilder.Append("			<script type=\"text/javascript\">setcookie(\"dnt_title\", '', 1);</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if


	}	//end if



	if (infloat!=1)
	{


	if (pagename=="website.aspx")
	{

	templateBuilder.Append("       <div id=\"websitebottomad\"></div>\r\n");

	}
	else if (footerad!="")
	{

	templateBuilder.Append("     <div id=\"ad_footerbanner\">");
	templateBuilder.Append(footerad.ToString());
	templateBuilder.Append("</div>   \r\n");

	}	//end if

	templateBuilder.Append("<div id=\"footer\">\r\n");
	templateBuilder.Append("	<div class=\"wrap\"  id=\"wp\">\r\n");
	templateBuilder.Append("		<div id=\"footlinks\">\r\n");
	templateBuilder.Append("			<p><a href=\"");
	templateBuilder.Append(config.Weburl.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(config.Webtitle.ToString().Trim());
	templateBuilder.Append("</a> - ");
	templateBuilder.Append(config.Linktext.ToString().Trim());
	templateBuilder.Append(" - <a target=\"_blank\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("stats.aspx\">统计</a> - \r\n");

	if (config.Sitemapstatus==1)
	{

	templateBuilder.Append("&nbsp;<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("tools/sitemap.aspx\" target=\"_blank\" title=\"百度论坛收录协议\">Sitemap</a>\r\n");

	}	//end if

	templateBuilder.Append("			");
	templateBuilder.Append(config.Statcode.ToString().Trim());
	templateBuilder.Append(config.Icp.ToString().Trim());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("			</p>\r\n");
	templateBuilder.Append("			<div>\r\n");
	templateBuilder.Append("				<a href=\"http://www.comsenz.com/\" target=\"_blank\">Comsenz Technology Ltd</a>\r\n");
	templateBuilder.Append("				- <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("archiver/index.aspx\" target=\"_blank\">简洁版本</a>\r\n");

	if (config.Stylejump==1)
	{


	if (userid!=-1 || config.Guestcachepagetimeout<=0)
	{

	templateBuilder.Append("				- <span id=\"styleswitcher\" class=\"drop\" onmouseover=\"showMenu({'ctrlid':this.id, 'pos':'21'})\" onclick=\"window.location.href='");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtemplate.aspx'\">界面风格</span>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<a title=\"Powered by Discuz!NT\" target=\"_blank\" href=\"http://nt.discuz.net\"><img border=\"0\" alt=\"Discuz!NT\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/discuznt_logo.gif\"/></a>\r\n");
	templateBuilder.Append("		<p id=\"copyright\">\r\n");
	templateBuilder.Append("			Powered by <strong><a href=\"http://nt.discuz.net\" target=\"_blank\" title=\"Discuz!NT 3.5.0 (.NET Framework 2.0/3.x)\">Discuz!NT</a></strong> <em class=\"f_bold\">3.5.0</em>\r\n");

	if (config.Licensed==1)
	{

	templateBuilder.Append("				(<a href=\"\" onclick=\"this.href='http://nt.discuz.net/certificate/?host='+location.href.substring(0, location.href.lastIndexOf('/'))\" target=\"_blank\">Licensed</a>)\r\n");

	}	//end if

	templateBuilder.Append("				");
	templateBuilder.Append(config.Forumcopyright.ToString().Trim());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</p>\r\n");
	templateBuilder.Append("		<p id=\"debuginfo\" class=\"grayfont\">\r\n");

	if (config.Debug!=0)
	{

	templateBuilder.Append("			Processed in ");
	templateBuilder.Append(this.Processtime.ToString().Trim());
	templateBuilder.Append(" second(s)\r\n");

	if (isguestcachepage==1)
	{

	templateBuilder.Append("				(Cached).\r\n");

	}
	else if (querycount>1)
	{

	templateBuilder.Append("				 , ");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append(" queries.\r\n");

	}
	else
	{

	templateBuilder.Append("				 , ");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append(" query.\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		</p>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<a id=\"scrolltop\" href=\"javascript:;\" style=\"display:none;\" class=\"scrolltop\" onclick=\"setScrollToTop(this.id);\">TOP</a>\r\n");
	templateBuilder.Append("<ul id=\"usercenter_menu\" class=\"p_pop\" style=\"display:none;\">\r\n");
	templateBuilder.Append("    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpprofile.aspx?action=avatar\">设置头像</a></li>\r\n");
	templateBuilder.Append("    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpprofile.aspx\">个人资料</a></li>\r\n");
	templateBuilder.Append("    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnewpassword.aspx\">更改密码</a></li>\r\n");
	templateBuilder.Append("    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户组</a></li>\r\n");
	templateBuilder.Append("    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpsubscribe.aspx\">收藏夹</a></li>\r\n");
	templateBuilder.Append("    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpcreditspay.aspx\">积分</a></li>\r\n");
	templateBuilder.Append("</ul>\r\n");

	int prentid__loop__id=0;
	foreach(string prentid in mainnavigationhassub)
	{
		prentid__loop__id++;

	templateBuilder.Append("<ul class=\"p_pop\" id=\"menu_");
	templateBuilder.Append(prentid.ToString());
	templateBuilder.Append("_menu\" style=\"display: none\">\r\n");

	int subnav__loop__id=0;
	foreach(DataRow subnav in subnavigation.Rows)
	{
		subnav__loop__id++;

	bool isoutput = false;
	

	if (subnav["parentid"].ToString().Trim()==prentid)
	{


	if (subnav["level"].ToString().Trim()=="0")
	{

	 isoutput = true;
	

	}
	else
	{


	if (subnav["level"].ToString().Trim()=="1" && userid!=-1)
	{

	 isoutput = true;
	

	}
	else
	{

	bool leveluseradmindi = true;
	
	 leveluseradmindi = (useradminid==3 || useradminid==1 || useradminid==2);
	

	if (subnav["level"].ToString().Trim()=="2" &&  leveluseradmindi)
	{

	 isoutput = true;
	

	}	//end if


	if (subnav["level"].ToString().Trim()=="3" && useradminid==1)
	{

	 isoutput = true;
	

	}	//end if


	}	//end if


	}	//end if


	}	//end if


	if (isoutput)
	{


	if (subnav["id"].ToString().Trim()=="11" || subnav["id"].ToString().Trim()=="12")
	{


	if (config.Statstatus==1)
	{

	templateBuilder.Append("	" + subnav["nav"].ToString().Trim() + "\r\n");
	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="18")
	{


	if (config.Oltimespan>0)
	{

	templateBuilder.Append("    " + subnav["nav"].ToString().Trim() + "\r\n");
	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="24")
	{


	if (config.Enablespace==1)
	{

	templateBuilder.Append("    " + subnav["nav"].ToString().Trim() + "\r\n");
	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="25")
	{


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("    " + subnav["nav"].ToString().Trim() + "\r\n");
	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if


	if (subnav["id"].ToString().Trim()=="26")
	{


	if (config.Enablemall>=1)
	{

	templateBuilder.Append("    " + subnav["nav"].ToString().Trim() + "\r\n");
	continue;


	}
	else
	{

	continue;


	}	//end if


	}	//end if

	templateBuilder.Append("    " + subnav["nav"].ToString().Trim() + "\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("</ul>\r\n");

	}	//end loop


	if (config.Stylejump==1)
	{


	if (userid!=-1 || config.Guestcachepagetimeout<=0)
	{

	templateBuilder.Append("	<ul id=\"styleswitcher_menu\" class=\"popupmenu_popup s_clear\" style=\"display: none;\">\r\n");
	templateBuilder.Append("	");
	templateBuilder.Append(templatelistboxoptions.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("	</ul>\r\n");

	}	//end if


	}	//end if




	templateBuilder.Append("</body>\r\n");
	templateBuilder.Append("</html>\r\n");

	}
	else
	{

	templateBuilder.Append("]]></root>\r\n");

	}	//end if




	Response.Write(templateBuilder.ToString());
}
</script>
