<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.register" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2010/12/28 17:12:58.
		本页面代码由Discuz!NT模板引擎生成于 2010/12/28 17:12:58. 
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




	if (infloat!=1)
	{

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
	templateBuilder.Append("\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a> &raquo; <strong>用户注册</strong>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	}	//end if


	if (agree=="" && infloat!=1)
	{


	if (page_err==0)
	{


	if (config.Rules==1)
	{

	templateBuilder.Append("        <div class=\"wrap cl\">\r\n");
	templateBuilder.Append("	        <div class=\"blr\">\r\n");
	templateBuilder.Append("		        <h3 class=\"flb\"><em>用户注册协议</em></h3>\r\n");
	templateBuilder.Append("		        <form id=\"form1\" name=\"form1\" method=\"post\" action=\"\">\r\n");
	templateBuilder.Append("		        <div class=\"c cl floatwrap\">\r\n");
	templateBuilder.Append("			        ");
	templateBuilder.Append(config.Rulestxt.ToString().Trim());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		        </div>\r\n");
	templateBuilder.Append("		        <p class=\"fsb pns cl\">\r\n");
	templateBuilder.Append("			        <input name=\"agree\" type=\"hidden\" value=\"true\" />\r\n");
	templateBuilder.Append("			        <button disabled=\"disabled\" type=\"submit\" id=\"btnagree\" class=\"pn pnc\"><span>同意</span></button>\r\n");
	templateBuilder.Append("			        <button name=\"cancel\" id=\"cancel\" type=\"button\" onClick=\"javascript:location.replace('index.aspx')\" class=\"pn\"><span>不同意</span></button>	  \r\n");
	templateBuilder.Append("			        <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("			        var secs = 5;\r\n");
	templateBuilder.Append("			        var wait = secs * 1000;\r\n");
	templateBuilder.Append("			        $(\"btnagree\").innerHTML = \"<span>同 意(\" + secs + \")</span>\";\r\n");
	templateBuilder.Append("			        $(\"btnagree\").disabled = true;\r\n");
	templateBuilder.Append("			        for(i = 1; i <= secs; i++) {\r\n");
	templateBuilder.Append("				        window.setTimeout(\"update(\" + i + \")\", i * 1000);\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        window.setTimeout(\"timer()\", wait);\r\n");
	templateBuilder.Append("			        function update(num, value) {\r\n");
	templateBuilder.Append("				        if(num == (wait/1000)) {\r\n");
	templateBuilder.Append("						        $(\"btnagree\").innerHTML = \"<span>同 意</span>\";\r\n");
	templateBuilder.Append("				        } else {\r\n");
	templateBuilder.Append("						        printnr = (wait / 1000) - num;\r\n");
	templateBuilder.Append("						        $(\"btnagree\").innerHTML = \"<span>同 意(\" + printnr + \")</span>\";\r\n");
	templateBuilder.Append("				        }\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        function timer() {\r\n");
	templateBuilder.Append("				        $(\"btnagree\").disabled = false;\r\n");
	templateBuilder.Append("				        $(\"btnagree\").innerHTML = \"<span>同 意</span>\";\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("		        </p>\r\n");
	templateBuilder.Append("		        </form>\r\n");
	templateBuilder.Append("	        </div>\r\n");
	templateBuilder.Append("        </div>\r\n");

			/*
			<script type="text/javascript">
			location.replace('register.aspx?agree=yes')
			</");
	templateBuilder.Append("script>
			*/
			

	}	//end if


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


	if (createuser=="")
	{


	if (page_err==0)
	{


	if (infloat!=1)
	{

	templateBuilder.Append("	    <div class=\"wrap cl\">\r\n");
	templateBuilder.Append("		    <div class=\"blr\" id=\"floatlayout_register\">\r\n");

	}	//end if


	if (config.Rules==1)
	{

	templateBuilder.Append("		    <div id=\"bbrule\" style=\"display:none\">\r\n");

	if (infloat==1)
	{

	templateBuilder.Append("				    <em>网站服务条款</em><span><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideWindow('register')\" title=\"关闭\">关闭</a></span>\r\n");

	}	//end if

	templateBuilder.Append("			    <div class=\"c cl floatwrap\">\r\n");
	templateBuilder.Append("				    ");
	templateBuilder.Append(config.Rulestxt.ToString().Trim());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("			    </div>\r\n");
	templateBuilder.Append("			    <p class=\"fsb pns cl\">\r\n");
	templateBuilder.Append("				    <button type=\"submit\" id=\"btnagree\" class=\"pn pnc\"  onclick=\"javascript:$('agree').checked=true;$('bbrule').style.display='none';$('bbreg').style.display=''\"><span>同意</span></button>\r\n");
	templateBuilder.Append("				    <button name=\"cancel\" id=\"cancel\"  onClick=\"hideWindow('register')\" class=\"pn\"><span>不同意</span></button>\r\n");
	templateBuilder.Append("			    </p>\r\n");
	templateBuilder.Append("		    </div>\r\n");

	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("	    <div id=\"bbreg\">\r\n");
	templateBuilder.Append("	        <h3 class=\"flb\"><em id=\"returnregmessage\">注册</em><span><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideWindow('register')\" title=\"关闭\">关闭</a></span></h3>\r\n");
	templateBuilder.Append("	        <div id=\"succeedmessage\" class=\"c cl\" style=\"display:none\"></div>\r\n");
	templateBuilder.Append("	            <form id=\"form2\" name=\"form2\" method=\"post\" onsubmit=\"javascript:$('form2').action='");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx?infloat=1&createuser=1&';ajaxpost('form2', 'returnregmessage', 'returnregmessage', 'onerror');return false;\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx?infloat=1&createuser=1&\">\r\n");

	}
	else
	{

	templateBuilder.Append("            <h3 class=\"flb\"><em id=\"returnregmessage\"></em></h3>\r\n");
	templateBuilder.Append("            <div id=\"succeedmessage\" class=\"c cl\" style=\"display:none\"></div>\r\n");
	templateBuilder.Append("            <form id=\"form1\" name=\"form1\" method=\"post\" action=\"?createuser=1\">\r\n");

	}	//end if

	templateBuilder.Append("	        <div class=\"c cl\">\r\n");
	templateBuilder.Append("		        <div style=\"overflow:hidden;overflow-y:auto\" class=\"lgfm\" id=\"reginfo_a\">\r\n");
	templateBuilder.Append("			        <span id=\"activation_hidden\">\r\n");

	if (invitecode!=""||config.Regstatus==3)
	{

	templateBuilder.Append("				        <label><em>邀请码:</em><input name=\"invitecode\" type=\"text\" id=\"invitecode\" size=\"20\" class=\"txt\" maxlength=\"7\"\r\n");

	if (invitecode!="")
	{

	templateBuilder.Append(" value=\"");
	templateBuilder.Append(invitecode.ToString());
	templateBuilder.Append("\" readonly=\"readonly\"\r\n");

	}	//end if

	templateBuilder.Append(" /> *</label>\r\n");

	}	//end if

	templateBuilder.Append("				        <label><em>用户名:</em><input type=\"text\" class=\"txt\" tabindex=\"1\"  value=\"\" maxlength=\"20\" size=\"25\" autocomplete=\"off\" name=\"");
	templateBuilder.Append(config.Antispamregisterusername.ToString().Trim());
	templateBuilder.Append("\" id=\"username\" onkeyup=\"checkusername(this.value);\"/> *</label>\r\n");
	templateBuilder.Append("				        <label><em>密码:</em><input type=\"password\" class=\"txt\" tabindex=\"1\"  id=\"password\" size=\"25\" name=\"password\" onkeyup=\"return checkpasswd(this);\"/> *</label>	\r\n");
	templateBuilder.Append("				        <label id=\"passwdpower\" style=\"display: none;\"><em>密码强度</em><strong id=\"showmsg\"></strong></label>\r\n");
	templateBuilder.Append("				        <label><em>确认密码:</em><input type=\"password\" class=\"txt\" value=\"\" tabindex=\"1\"  id=\"password2\" size=\"25\" name=\"password2\" onkeyup=\"checkdoublepassword(this.form)\"/> *</label>\r\n");
	templateBuilder.Append("				        <label><em>Email:</em><input type=\"text\" class=\"txt\" tabindex=\"1\"  id=\"email\" size=\"25\" autocomplete=\"off\" name=\"");
	templateBuilder.Append(config.Antispamregisteremail.ToString().Trim());
	templateBuilder.Append("\" onkeyup=\"checkemail(this.value)\"/> *</label>\r\n");

	if (config.Realnamesystem==1)
	{

	templateBuilder.Append("				        <label><em>真实姓名:</em><input name=\"realname\" type=\"text\" id=\"realname\" size=\"10\" class=\"txt\" /> *</label>\r\n");
	templateBuilder.Append("				        <label><em>身份证:</em><input name=\"idcard\" type=\"text\" id=\"idcard\" size=\"20\" class=\"txt\" /> *</label>\r\n");
	templateBuilder.Append("				        <label><em>移动电话:</em><input name=\"mobile\" type=\"text\" id=\"mobile\" size=\"20\" class=\"txt\" /> *</label>\r\n");
	templateBuilder.Append("				        <label><em>固定电话:</em><input name=\"phone\" type=\"text\" id=\"phone\" size=\"20\" class=\"txt\" /> *</label>\r\n");

	}	//end if

	templateBuilder.Append("			        </span>\r\n");

	if (isseccode)
	{

	templateBuilder.Append("			        <div class=\"regsec\">\r\n");
	templateBuilder.Append("				        <label style=\"display: inline;\"><em>验证: </em><span style=\"position: relative;\">\r\n");

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


	templateBuilder.Append("	        </span></label>\r\n");
	templateBuilder.Append("			        </div>\r\n");

	}	//end if

	templateBuilder.Append("		        </div>\r\n");
	templateBuilder.Append("		        <div class=\"lgf\">\r\n");
	templateBuilder.Append("			        <h4>已有帐号？\r\n");

	if (infloat==1)
	{

	templateBuilder.Append("				        <a onclick=\"hideWindow('register');showWindow('login', this.href);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx\" class=\"xg2\">现在登录</a>\r\n");

	}
	else
	{

	templateBuilder.Append("				        <a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx\" title=\"现在登录\" class=\"xg2\">现在登录</a>\r\n");

	}	//end if

	templateBuilder.Append("			        </h4>\r\n");
	templateBuilder.Append("		        </div>\r\n");
	templateBuilder.Append("	        </div>\r\n");
	templateBuilder.Append("	        <p class=\"fsb pns cl\">\r\n");
	templateBuilder.Append("		        <span id=\"reginfo_b_btn\">\r\n");
	templateBuilder.Append("		        <button tabindex=\"1\" value=\"true\" name=\"regsubmit\" type=\"submit\" id=\"registerformsubmit\" class=\"pn\" \r\n");

	if (config.Rules==1)
	{

	templateBuilder.Append("onclick=\"return checkagreed();\" \r\n");

	}	//end if

	templateBuilder.Append("><span>创建用户</span></button>\r\n");

	if (config.Rules==1)
	{

	templateBuilder.Append("		        <input type=\"checkbox\" id=\"agree\" value=\"true\" name=\"agree\" class=\"checkbox\" style=\"margin-left:5px;\"/><label for=\"agreebbrule\">同意<a onclick=\"javascript:$('bbrule').style.display='';$('bbreg').style.display='none'\" href=\"javascript:;\">网站服务条款</a></label>\r\n");
	templateBuilder.Append("			        <script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("			            function checkagreed() {\r\n");
	templateBuilder.Append("			             $('returnregmessage').className = '';\r\n");
	templateBuilder.Append("						        if ($('agree').checked == true) {\r\n");
	templateBuilder.Append("							        return true;\r\n");
	templateBuilder.Append("						        }\r\n");
	templateBuilder.Append("						        else {\r\n");
	templateBuilder.Append("						            $('returnregmessage').innerHTML = \"请确认《网络服务条款》\";\r\n");
	templateBuilder.Append("						            $('returnregmessage').className = 'onerror';\r\n");
	templateBuilder.Append("							        return false;\r\n");
	templateBuilder.Append("						        }\r\n");
	templateBuilder.Append("				        }\r\n");
	templateBuilder.Append("			        </");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("		        </span>\r\n");
	templateBuilder.Append("	        </p>\r\n");
	templateBuilder.Append("	    </form>\r\n");
	templateBuilder.Append("        </div>\r\n");
	templateBuilder.Append("        </div>\r\n");
	templateBuilder.Append("        </div>\r\n");
	templateBuilder.Append("        <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	        var PasswordStrength ={\r\n");
	templateBuilder.Append("		        Level : [\"极佳\",\"一般\",\"较弱\",\"太短\"],\r\n");
	templateBuilder.Append("		        LevelValue : [15,10,5,0],//强度值\r\n");
	templateBuilder.Append("		        Factor : [1,2,5],//字符加数,分别为字母，数字，其它\r\n");
	templateBuilder.Append("		        KindFactor : [0,0,10,20],//密码含几种组成的加数 \r\n");
	templateBuilder.Append("		        Regex : [/[a-zA-Z]/g,/\\d/g,/[^a-zA-Z0-9]/g] //字符正则数字正则其它正则\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("	        PasswordStrength.StrengthValue = function(pwd)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        var strengthValue = 0;\r\n");
	templateBuilder.Append("		        var ComposedKind = 0;\r\n");
	templateBuilder.Append("		        for(var i = 0 ; i < this.Regex.length;i++)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        var chars = pwd.match(this.Regex[i]);\r\n");
	templateBuilder.Append("			        if(chars != null)\r\n");
	templateBuilder.Append("			        {\r\n");
	templateBuilder.Append("				        strengthValue += chars.length * this.Factor[i];\r\n");
	templateBuilder.Append("				        ComposedKind ++;\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        strengthValue += this.KindFactor[ComposedKind];\r\n");
	templateBuilder.Append("		        return strengthValue;\r\n");
	templateBuilder.Append("	        } \r\n");
	templateBuilder.Append("	        PasswordStrength.StrengthLevel = function(pwd)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        var value = this.StrengthValue(pwd);\r\n");
	templateBuilder.Append("		        for(var i = 0 ; i < this.LevelValue.length ; i ++)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        if(value >= this.LevelValue[i] )\r\n");
	templateBuilder.Append("				        return this.Level[i];\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	        function checkpasswd(o)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        var pshowmsg = '不得少于6个字符';\r\n");
	templateBuilder.Append("		        if(o.value.length<6)  {\r\n");
	templateBuilder.Append("		            $(\"returnregmessage\").innerHTML = pshowmsg;\r\n");
	templateBuilder.Append("		            $(\"returnregmessage\").className = 'onerror';\r\n");
	templateBuilder.Append("		        } \r\n");
	templateBuilder.Append("		        else\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("		           var showmsg=PasswordStrength.StrengthLevel(o.value);\r\n");
	templateBuilder.Append("		           switch(showmsg) {\r\n");
	templateBuilder.Append("		           case \"太短\": showmsg+=\" <img src='images/level/1.gif' width='88' height='11' />\";break;\r\n");
	templateBuilder.Append("		           case \"较弱\": showmsg+=\" <img src='images/level/2.gif' width='88' height='11' />\";break;\r\n");
	templateBuilder.Append("		           case \"一般\": showmsg+=\" <img src='images/level/3.gif' width='88' height='11' />\";break;\r\n");
	templateBuilder.Append("		           case \"极佳\": showmsg+=\" <img src='images/level/4.gif' width='88' height='11' />\";break;\r\n");
	templateBuilder.Append("		           }\r\n");
	templateBuilder.Append("		           $('passwdpower').style.display='';\r\n");
	templateBuilder.Append("		           $('showmsg').innerHTML = showmsg;\r\n");
	templateBuilder.Append("		           $('returnregmessage').className = '';\r\n");
	templateBuilder.Append("		           $('returnregmessage').innerHTML = '注册';		   \r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("//		        if(pshowmsg!='' &&  pshowmsg!=null && pshowmsg!=undefined)\r\n");
	templateBuilder.Append("//		        {\r\n");
	templateBuilder.Append("//		        $('returnregmessage').innerHTML=pshowmsg;\r\n");
	templateBuilder.Append("//		        $('returnregmessage').className='onerror';\r\n");
	templateBuilder.Append("//		        }\r\n");
	templateBuilder.Append("//		        else\r\n");
	templateBuilder.Append("//		        {\r\n");
	templateBuilder.Append("//		        $('returnregmessage').className='';\r\n");
	templateBuilder.Append("//		        $('returnregmessage').innerHTML='注册';\r\n");
	templateBuilder.Append("//		        }\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	        function checkemail(strMail)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        var str;\r\n");
	templateBuilder.Append("		        if(strMail.length==0) return false; \r\n");
	templateBuilder.Append("		        var objReg = new RegExp(\"[A-Za-z0-9-_]+@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]\") \r\n");
	templateBuilder.Append("		        var IsRightFmt = objReg.test(strMail) \r\n");
	templateBuilder.Append("		        var objRegErrChar = new RegExp(\"[^a-z0-9-._@]\",\"ig\") \r\n");
	templateBuilder.Append("		        var IsRightChar = (strMail.search(objRegErrChar)==-1) \r\n");
	templateBuilder.Append("		        var IsRightLength = strMail.length <= 60 \r\n");
	templateBuilder.Append("		        var IsRightPos = (strMail.indexOf(\"@\",0) != 0 && strMail.indexOf(\".\",0) != 0 && strMail.lastIndexOf(\"@\")+1 != strMail.length && strMail.lastIndexOf(\".\")+1 != strMail.length) \r\n");
	templateBuilder.Append("		        var IsNoDupChar = (strMail.indexOf(\"@\",0) == strMail.lastIndexOf(\"@\")) \r\n");
	templateBuilder.Append("		        if(!(IsRightFmt && IsRightChar && IsRightLength && IsRightPos && IsNoDupChar)) \r\n");
	templateBuilder.Append("		         {\r\n");
	templateBuilder.Append("		         str=\"E-mail 地址无效，请返回重新填写。\";\r\n");
	templateBuilder.Append("		         }\r\n");
	templateBuilder.Append("	            if(str!='' &&  str!=null && str!=undefined)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("		        $('returnregmessage').innerHTML=str;\r\n");
	templateBuilder.Append("		        $('returnregmessage').className='onerror';\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        else\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("		        $('returnregmessage').className='';\r\n");
	templateBuilder.Append("		        $('returnregmessage').innerHTML='注册';\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	        function htmlEncode(source, display, tabs)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        function special(source)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        var result = '';\r\n");
	templateBuilder.Append("			        for (var i = 0; i < source.length; i++)\r\n");
	templateBuilder.Append("			        {\r\n");
	templateBuilder.Append("				        var c = source.charAt(i);\r\n");
	templateBuilder.Append("				        if (c < ' ' || c > '~')\r\n");
	templateBuilder.Append("				        {\r\n");
	templateBuilder.Append("					        c = '&#' + c.charCodeAt() + ';';\r\n");
	templateBuilder.Append("				        }\r\n");
	templateBuilder.Append("				        result += c;\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        return result;\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        function format(source)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        // Use only integer part of tabs, and default to 4\r\n");
	templateBuilder.Append("			        tabs = (tabs >= 0) ? Math.floor(tabs) : 4;\r\n");
	templateBuilder.Append("			        // split along line breaks\r\n");
	templateBuilder.Append("			        var lines = source.split(/\\r\\n|\\r|\\n/);\r\n");
	templateBuilder.Append("			        // expand tabs\r\n");
	templateBuilder.Append("			        for (var i = 0; i < lines.length; i++)\r\n");
	templateBuilder.Append("			        {\r\n");
	templateBuilder.Append("				        var line = lines[i];\r\n");
	templateBuilder.Append("				        var newLine = '';\r\n");
	templateBuilder.Append("				        for (var p = 0; p < line.length; p++)\r\n");
	templateBuilder.Append("				        {\r\n");
	templateBuilder.Append("					        var c = line.charAt(p);\r\n");
	templateBuilder.Append("					        if (c === '\\t')\r\n");
	templateBuilder.Append("					        {\r\n");
	templateBuilder.Append("						        var spaces = tabs - (newLine.length % tabs);\r\n");
	templateBuilder.Append("						        for (var s = 0; s < spaces; s++)\r\n");
	templateBuilder.Append("						        {\r\n");
	templateBuilder.Append("							        newLine += ' ';\r\n");
	templateBuilder.Append("						        }\r\n");
	templateBuilder.Append("					        }\r\n");
	templateBuilder.Append("					        else\r\n");
	templateBuilder.Append("					        {\r\n");
	templateBuilder.Append("						        newLine += c;\r\n");
	templateBuilder.Append("					        }\r\n");
	templateBuilder.Append("				        }\r\n");
	templateBuilder.Append("				        // If a line starts or ends with a space, it evaporates in html\r\n");
	templateBuilder.Append("				        // unless it's an nbsp.\r\n");
	templateBuilder.Append("				        newLine = newLine.replace(/(^ )|( $)/g, '&nbsp;');\r\n");
	templateBuilder.Append("				        lines[i] = newLine;\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        // re-join lines\r\n");
	templateBuilder.Append("			        var result = lines.join('<br />');\r\n");
	templateBuilder.Append("			        // break up contiguous blocks of spaces with non-breaking spaces\r\n");
	templateBuilder.Append("			        result = result.replace(/  /g, ' &nbsp;');\r\n");
	templateBuilder.Append("			        // tada!\r\n");
	templateBuilder.Append("			        return result;\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        var result = source;\r\n");
	templateBuilder.Append("		        // ampersands (&)\r\n");
	templateBuilder.Append("		        result = result.replace(/\\&/g,'&amp;');\r\n");
	templateBuilder.Append("		        // less-thans (<)\r\n");
	templateBuilder.Append("		        result = result.replace(/\\</g,'&lt;');\r\n");
	templateBuilder.Append("		        // greater-thans (>)\r\n");
	templateBuilder.Append("		        result = result.replace(/\\>/g,'&gt;');\r\n");
	templateBuilder.Append("		        if (display)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        // format for display\r\n");
	templateBuilder.Append("			        result = format(result);\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        else\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        // Replace quotes if it isn't for display,\r\n");
	templateBuilder.Append("			        // since it's probably going in an html attribute.\r\n");
	templateBuilder.Append("			        result = result.replace(new RegExp('\"','g'), '&quot;');\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        // special characters\r\n");
	templateBuilder.Append("		        result = special(result);\r\n");
	templateBuilder.Append("		        // tada!\r\n");
	templateBuilder.Append("		        return result;\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	        var profile_username_toolong = '您的用户名超过 20 个字符，请输入一个较短的用户名。';\r\n");
	templateBuilder.Append("	        var profile_username_tooshort = '您输入的用户名小于3个字符, 请输入一个较长的用户名。';\r\n");
	templateBuilder.Append("	        var profile_username_pass = \"<img src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/check_right.gif'/>\";\r\n");
	templateBuilder.Append("	        function checkusername(username)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        var unlen = username.replace(/[^\\x00-\\xff]/g, \"**\").length;\r\n");
	templateBuilder.Append("		        if(unlen < 3 || unlen > 20) {\r\n");
	templateBuilder.Append("			        $(\"returnregmessage\").innerHTML =(unlen < 3 ? profile_username_tooshort : profile_username_toolong);\r\n");
	templateBuilder.Append("			        $('returnregmessage').className='onerror';\r\n");
	templateBuilder.Append("			        return;\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        ajaxRead(\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=checkusername&username=\" + escape(username), \"showcheckresult(obj,'\" + escape(username) + \"');\");\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	        function showcheckresult(obj, username)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("		        var res = obj.getElementsByTagName('result');\r\n");
	templateBuilder.Append("		        var result = \"\";\r\n");
	templateBuilder.Append("		        if (res[0] != null && res[0] != undefined)\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        if (res[0].childNodes.length > 1) {\r\n");
	templateBuilder.Append("				        result = res[0].childNodes[1].nodeValue;\r\n");
	templateBuilder.Append("			        } else {\r\n");
	templateBuilder.Append("				        result = res[0].firstChild.nodeValue;    		\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        if (result == \"1\")\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("		            var tips=\"对不起，您输入的用户名 \\\"\" + htmlEncode(unescape(username), true, 4) + \"\\\" 已经被他人使用或被禁用。\";\r\n");
	templateBuilder.Append("			        $('returnregmessage').innerHTML=tips;\r\n");
	templateBuilder.Append("			        $('returnregmessage').className='onerror';\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("		        else\r\n");
	templateBuilder.Append("		        {\r\n");
	templateBuilder.Append("			        $('returnregmessage').className='';\r\n");
	templateBuilder.Append("			         $('returnregmessage').innerHTML='注册';\r\n");
	templateBuilder.Append("		        }\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	        function checkdoublepassword(theform)\r\n");
	templateBuilder.Append("	        {\r\n");
	templateBuilder.Append("	          var pw1 = theform.password.value;\r\n");
	templateBuilder.Append("	          var pw2 = theform.password2.value;\r\n");
	templateBuilder.Append("	          if(pw1=='' &&  pw2=='')\r\n");
	templateBuilder.Append("	          {\r\n");
	templateBuilder.Append("	          return;\r\n");
	templateBuilder.Append("	          }\r\n");
	templateBuilder.Append("	          var str;\r\n");
	templateBuilder.Append("		         if(pw1!=pw2)\r\n");
	templateBuilder.Append("		         {\r\n");
	templateBuilder.Append("		          str =\"两次输入的密码不一致\";\r\n");
	templateBuilder.Append("		         }\r\n");
	templateBuilder.Append("		          if(str!='' &&  str!=null && str!=undefined)\r\n");
	templateBuilder.Append("		          {\r\n");
	templateBuilder.Append("		          $('returnregmessage').innerHTML=str;\r\n");
	templateBuilder.Append("		          $('returnregmessage').className='onerror';\r\n");
	templateBuilder.Append("		          }\r\n");
	templateBuilder.Append("		          else\r\n");
	templateBuilder.Append("		          {\r\n");
	templateBuilder.Append("		          $('returnregmessage').className='';\r\n");
	templateBuilder.Append("		          $('returnregmessage').innerHTML='注册';\r\n");
	templateBuilder.Append("		          }\r\n");
	templateBuilder.Append("	        }\r\n");
	templateBuilder.Append("	    </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	    <script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n");

	}
	else
	{


	if (infloat==1)
	{

	templateBuilder.Append("        	<h3 class=\"flb\"><em>出现了");
	templateBuilder.Append(page_err.ToString());
	templateBuilder.Append("个错误</em><span><a href=\"javascript:;\" class=\"flbc\" onclick=\"hideWindow('register')\" title=\"关闭\">关闭</a></span></h3>\r\n");
	templateBuilder.Append("            <div class=\"c cl\" id=\"errormsg\">\r\n");
	templateBuilder.Append("		        <div class=\"msg_inner error_msg\">\r\n");
	templateBuilder.Append("		            <p style=\"margin-bottom:16px;line-height:60px;\">");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");
	templateBuilder.Append("		        </div>\r\n");
	templateBuilder.Append("	        </div>\r\n");

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


	}	//end if


	}
	else
	{


	if (createuser!="")
	{


	if (infloat==1)
	{


	if (page_err==0)
	{

	templateBuilder.Append("	            <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	                $('form2').style.display='none';\r\n");
	templateBuilder.Append("	                $('returnregmessage').className='';\r\n");
	templateBuilder.Append("	            </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	            <div class=\"msgbox cl\" id=\"succeemessage\">\r\n");
	templateBuilder.Append("		            <div class=\"msg_inner\">\r\n");
	templateBuilder.Append("		            <p style=\"margin-bottom:16px;\">");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");

	if (msgbox_url!="")
	{

	templateBuilder.Append("		            <p><a href=\"javascript:;\" onclick=\"location.reload()\" class=\"xg2\">如果长时间没有响应请点击此处</a></p>\r\n");
	templateBuilder.Append("		            <script type=\"text/javascript\">setTimeout('location.reload()', 3000);</");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("		            </div>\r\n");
	templateBuilder.Append("	            </div>\r\n");
	templateBuilder.Append("	            <script>\r\n");
	templateBuilder.Append("	                $('succeedmessage').style.display='';\r\n");
	templateBuilder.Append("	                $('succeedmessage').innerHTML=$('succeemessage').innerHTML;\r\n");
	templateBuilder.Append("	                $('returnregmessage').innerHTML='注册';\r\n");
	templateBuilder.Append("	            </");
	templateBuilder.Append("script>	\r\n");

	}
	else
	{

	templateBuilder.Append("	            <p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");

	}	//end if


	}
	else
	{


	if (page_err==0)
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


	}	//end if


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
