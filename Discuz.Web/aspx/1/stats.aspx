<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.stats" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2010/12/28 17:13:09.
		本页面代码由Discuz!NT模板引擎生成于 2010/12/28 17:13:09. 
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



	templateBuilder.Append("<div class=\"wrap s_clear pageinfo\">\r\n");
	templateBuilder.Append("	<div id=\"nav\">\r\n");
	templateBuilder.Append("	<a href=\"");
	templateBuilder.Append(config.Forumurl.ToString().Trim());
	templateBuilder.Append("\" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a> &raquo; <a href=\"stats.aspx\">统计</a>  &raquo; <strong>\r\n");

	if (type=="")
	{

	templateBuilder.Append("		基本概况\r\n");

	}
	else if (type=="views")
	{

	templateBuilder.Append("		流量统计\r\n");

	}
	else if (type=="client")
	{

	templateBuilder.Append("		客户软件\r\n");

	}
	else if (type=="posts")
	{

	templateBuilder.Append("		发帖量记录\r\n");

	}
	else if (type=="forumsrank")
	{

	templateBuilder.Append("		版块排行\r\n");

	}
	else if (type=="topicsrank")
	{

	templateBuilder.Append("		主题排行\r\n");

	}
	else if (type=="postsrank")
	{

	templateBuilder.Append("		发帖排行\r\n");

	}
	else if (type=="creditsrank")
	{

	templateBuilder.Append("		积分排行\r\n");

	}
	else if (type=="onlinetime")
	{

	templateBuilder.Append("		在线时间\r\n");

	}
	else if (type=="trade")
	{

	templateBuilder.Append("		交易排行\r\n");

	}
	else if (type=="team")
	{

	templateBuilder.Append("		管理团队\r\n");

	}
	else if (type=="modworks")
	{

	templateBuilder.Append("		管理统计\r\n");

	}	//end if

	templateBuilder.Append("	</strong>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	function changeTab(obj)\r\n");
	templateBuilder.Append("	{\r\n");
	templateBuilder.Append("		if (obj.className == 'currenttab')\r\n");
	templateBuilder.Append("		{\r\n");
	templateBuilder.Append("			obj.className = '';\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("		else\r\n");
	templateBuilder.Append("		{\r\n");
	templateBuilder.Append("			obj.className = 'currenttab';\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");

	if (page_err==0)
	{

	templateBuilder.Append("<div class=\"wrap uc cl\">\r\n");
	templateBuilder.Append("	<div class=\"uc_app\">\r\n");
	templateBuilder.Append("		<h2>统计</h2>\r\n");
	templateBuilder.Append("		<ul>\r\n");
	templateBuilder.Append("			<li id=\"tab_main\" class=\"current\"><a onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"stats.aspx\">基本状况</a></li>\r\n");

	if (statstatus)
	{

	templateBuilder.Append("			<li id=\"tab_views\"><a  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=views\">流量统计</a></li>\r\n");
	templateBuilder.Append("			<li id=\"tab_client\"><a onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=client\">客户软件</a></li>\r\n");

	}	//end if

	templateBuilder.Append("			<li id=\"tab_posts\"><a onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=posts\">发帖量记录</a></li>\r\n");
	templateBuilder.Append("			<li id=\"tab_forumsrank\"><a  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=forumsrank\">版块排行</a></li>\r\n");
	templateBuilder.Append("			<li id=\"tab_topicsrank\"><a  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=topicsrank\">主题排行</a></li>\r\n");
	templateBuilder.Append("			<li id=\"tab_postsrank\"><a onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=postsrank\">发帖排行</a></li>\r\n");
	templateBuilder.Append("			<li id=\"tab_creditsrank\"><a  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=creditsrank\">积分排行</a></li>\r\n");
	templateBuilder.Append("			<!--\r\n");
	templateBuilder.Append("			<li><a id=\"tab_trade\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=trade\">交易排行</a></li>\r\n");
	templateBuilder.Append("			-->\r\n");

	if (config.Oltimespan>0)
	{

	templateBuilder.Append("			<li><a id=\"tab_onlinetime\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=onlinetime\">在线时间</a></li>\r\n");

	}	//end if

	templateBuilder.Append("			<!--\r\n");
	templateBuilder.Append("			<li><a id=\"tab_team\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=team\">管理团队</a></li>\r\n");
	templateBuilder.Append("			<li><a id=\"tab_modworks\"  onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" href=\"?type=modworks\">管理统计</a></li>\r\n");
	templateBuilder.Append("			-->\r\n");
	templateBuilder.Append("		</ul>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	try{\r\n");
	templateBuilder.Append("		$(\"tab_main\").className = \"\";\r\n");
	templateBuilder.Append("		$(\"tab_\" + '");
	templateBuilder.Append(type.ToString());
	templateBuilder.Append("').className = \"current\";\r\n");
	templateBuilder.Append("	}catch(e){\r\n");
	templateBuilder.Append("		$(\"tab_main\").className = \"current\";\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	<div class=\"uc_main\">\r\n");
	templateBuilder.Append("	<div class=\"uc_content stats\">\r\n");
	templateBuilder.Append("	<h1>\r\n");

	if (type=="")
	{

	templateBuilder.Append("		基本概况\r\n");

	}
	else if (type=="views")
	{

	templateBuilder.Append("		流量统计\r\n");

	}
	else if (type=="client")
	{

	templateBuilder.Append("		客户软件\r\n");

	}
	else if (type=="posts")
	{

	templateBuilder.Append("		发帖量记录\r\n");

	}
	else if (type=="forumsrank")
	{

	templateBuilder.Append("		版块排行\r\n");

	}
	else if (type=="topicsrank")
	{

	templateBuilder.Append("		主题排行\r\n");

	}
	else if (type=="postsrank")
	{

	templateBuilder.Append("		发帖排行\r\n");

	}
	else if (type=="creditsrank")
	{

	templateBuilder.Append("		积分排行\r\n");

	}
	else if (type=="onlinetime")
	{

	templateBuilder.Append("		在线时间\r\n");

	}
	else if (type=="trade")
	{

	templateBuilder.Append("		交易排行\r\n");

	}
	else if (type=="team")
	{

	templateBuilder.Append("		管理团队\r\n");

	}
	else if (type=="modworks")
	{

	templateBuilder.Append("		管理统计\r\n");

	}	//end if

	templateBuilder.Append("	</h1>\r\n");

	if (type=="")
	{

	templateBuilder.Append("		<h3>基本状况</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\" style=\"margin-bottom:10px;\">\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">注册会员</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(members.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">发帖会员</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(mempost.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">管理成员</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(admins.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">未发帖会员</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(memnonpost.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">新会员</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(lastmember.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">发帖会员占总数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(mempostpercent.ToString());
	templateBuilder.Append("%</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">今日论坛之星</td>\r\n");
	templateBuilder.Append("			<td>\r\n");

	if (bestmem!="")
	{

	templateBuilder.Append("<a href=\"userinfo.aspx?username=");
	templateBuilder.Append(bestmem.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(bestmem.ToString());
	templateBuilder.Append("</a>(");
	templateBuilder.Append(bestmemposts.ToString());
	templateBuilder.Append(")\r\n");

	}	//end if

	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">平均每人发帖数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(mempostavg.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		<tbody  summary=\"论坛统计\">\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">版块数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(forums.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">平均每日新增帖子数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(postsaddavg.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">最热门版块</td>\r\n");
	templateBuilder.Append("			<td><a href=\"");
	templateBuilder.Append(ShowForumAspxRewrite(hotforum.Fid,0).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(hotforum.Name.ToString().Trim());
	templateBuilder.Append("</a></td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">主题数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(topics.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">平均每日注册会员数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(membersaddavg.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">主题数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(hotforum.Topics.ToString().Trim());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">帖子数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(posts.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">最近24小时新增帖子数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(postsaddtoday.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">帖子数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(hotforum.Posts.ToString().Trim());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">平均每个主题被回复次数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(topicreplyavg.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">今日新增会员数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(membersaddtoday.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">论坛活跃指数</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(activeindex.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	if (statstatus)
	{

	templateBuilder.Append("		<h3>流量概况</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">总页面流量</td>\r\n");
	templateBuilder.Append("			<td>" + totalstats["hits"].ToString().Trim() + "</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">访问量最多的月份</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(yearofmaxmonth.ToString());
	templateBuilder.Append(" 年 ");
	templateBuilder.Append(monthofmaxmonth.ToString());
	templateBuilder.Append(" 月</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">共计来访</td>\r\n");
	templateBuilder.Append("			<td>" + totalstats["visitors"].ToString().Trim() + " 人次</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">月份总页面流量</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(maxmonth.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">会员</td>\r\n");
	templateBuilder.Append("			<td>" + totalstats["members"].ToString().Trim() + "</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">时段</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(maxhourfrom.ToString());
	templateBuilder.Append(" - ");
	templateBuilder.Append(maxhourto.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">游客</td>\r\n");
	templateBuilder.Append("			<td>" + totalstats["guests"].ToString().Trim() + "</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">时段总页面流量</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(maxhour.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">平均每人浏览</td>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(pageviewavg.ToString());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("			<td class=\"t_th\">&nbsp;</td>\r\n");
	templateBuilder.Append("			<td>&nbsp;</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if

	templateBuilder.Append("		<h3>月份流量</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");

	if (statstatus)
	{

	templateBuilder.Append("			");
	templateBuilder.Append(monthofstatsbar.ToString());
	templateBuilder.Append("\r\n");

	}
	else
	{

	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td colspan=\"2\" class=\"t_th\">每月新增帖子记录</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("			");
	templateBuilder.Append(monthpostsofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td colspan=\"2\" class=\"t_th\">每日新增帖子记录</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("			");
	templateBuilder.Append(daypostsofstatsbar.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="views")
	{

	templateBuilder.Append("		<h3>流量统计</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td colspan=\"2\">星期流量</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(weekofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td colspan=\"2\">时段流量</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(hourofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="client")
	{

	templateBuilder.Append("		<h3>客户软件</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td colspan=\"2\">操作系统</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(osofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td colspan=\"2\">浏览器</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(browserofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="posts")
	{

	templateBuilder.Append("		<h3>发帖量记录</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td colspan=\"2\"  class=\"colplural\">每月新增帖子记录</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(monthpostsofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td colspan=\"2\"  class=\"colplural\">每日新增帖子记录</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(daypostsofstatsbar.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="forumsrank")
	{

	templateBuilder.Append("		<h3>版块排行</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td width=\"25%\">发帖 排行榜</td>\r\n");
	templateBuilder.Append("			<td width=\"25%\">回复 排行榜</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(topicsforumsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(postsforumsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td width=\"25%\">最近 30 天发帖 排行榜</td>\r\n");
	templateBuilder.Append("			<td width=\"25%\">最近 24 小时发帖 排行榜</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(thismonthforumsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(todayforumsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="topicsrank")
	{

	templateBuilder.Append("		<h3>主题排行</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td width=\"50%\">被浏览最多的主题</td>\r\n");
	templateBuilder.Append("			<td>被回复最多的主题</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(hottopics.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(hotreplytopics.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="postsrank")
	{

	templateBuilder.Append("		<h3>发帖排行</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td width=\"25%\">发帖 排行榜</td>\r\n");
	templateBuilder.Append("			<td width=\"25%\">精华帖 排行榜</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(postsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(digestpostsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr  class=\"colplural\">\r\n");
	templateBuilder.Append("			<td width=\"25%\">最近 30 天发帖 排行榜</td>\r\n");
	templateBuilder.Append("			<td width=\"25%\">最近 24 小时发帖 排行榜</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(thismonthpostsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("			<td class=\"absmiddle\"><ul>");
	templateBuilder.Append(todaypostsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="creditsrank")
	{

	templateBuilder.Append("		<h3>积分排行</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td>积分 排行榜</td>\r\n");

	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[1].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[2].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[3].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[4].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[5].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[6].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[7].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td>" + score[8].ToString().Trim() + " 排行榜</td>\r\n");

	}	//end if

	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td><ul>");
	templateBuilder.Append(creditsrank.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	if (score[1].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank1.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[2].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank2.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[3].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank3.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[4].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank4.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[5].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank5.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[6].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank6.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[7].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank7.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if


	if (score[8].ToString().Trim()!="")
	{

	templateBuilder.Append("				<td><ul>");
	templateBuilder.Append(extcreditsrank8.ToString());
	templateBuilder.Append("</ul></td>\r\n");

	}	//end if

	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (type=="onlinetime")
	{

	templateBuilder.Append("		<h3>主题排行</h3>\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"  class=\"datatable\">\r\n");
	templateBuilder.Append("		<thead>\r\n");
	templateBuilder.Append("		<tr class=\"colplural\">\r\n");
	templateBuilder.Append("			<td width=\"50%\">总在线时间排行(小时)</td>\r\n");
	templateBuilder.Append("			<td>本月在线时间排行(小时)</td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</thead>\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<td><ul>");
	templateBuilder.Append(totalonlinerank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("			<td><ul>");
	templateBuilder.Append(thismonthonlinerank.ToString());
	templateBuilder.Append("</ul></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");

	}	//end if


	if (lastupdate!="" && nextupdate!="")
	{

	templateBuilder.Append("		<div class=\"hintinfo notice\">统计数据已被缓存，上次于 ");
	templateBuilder.Append(lastupdate.ToString());
	templateBuilder.Append(" 被更新，下次将于 ");
	templateBuilder.Append(nextupdate.ToString());
	templateBuilder.Append(" 进行更新</div>\r\n");

	}	//end if

	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

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
