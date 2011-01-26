<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.showforum" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2010/12/28 17:12:56.
		本页面代码由Discuz!NT模板引擎生成于 2010/12/28 17:12:56. 
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

	templateBuilder.Append("		<a id=\"forumlist\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\" \r\n");

	if (config.Forumjump==1)
	{

	templateBuilder.Append("onmouseover=\"showMenu(this.id);\" onmouseout=\"showMenu(this.id);\"\r\n");

	}	//end if

	templateBuilder.Append(" class=\"title\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a> &raquo; ");
	templateBuilder.Append(forumnav.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n");

	if (page_err==0)
	{

	templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	var templatepath = \"");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("\";\r\n");
	templateBuilder.Append("    var imagedir = \"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("\";\r\n");
	templateBuilder.Append("	var fid = parseInt(");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("	var postminchars = parseInt(");
	templateBuilder.Append(config.Minpostsize.ToString().Trim());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("	var postmaxchars = parseInt(");
	templateBuilder.Append(config.Maxpostsize.ToString().Trim());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("	var disablepostctrl = parseInt(");
	templateBuilder.Append(disablepostctrl.ToString());
	templateBuilder.Append(");\r\n");
	templateBuilder.Append("	var forumurl = forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_showforum.js\"></");
	templateBuilder.Append("script>\r\n");

	if (page_err==0)
	{

	templateBuilder.Append("<div class=\"wrap cl\">\r\n");


	if (pagewordad.Length>0)
	{

	templateBuilder.Append("<div id=\"ad_text\" class=\"ad_text sclear\">\r\n");
	templateBuilder.Append("	<table cellspacing=\"1\" cellpadding=\"0\" width=\"100%\" summary=\"text ad\">\r\n");
	templateBuilder.Append("	<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	int adindex = 0;
	

	int pageword__loop__id=0;
	foreach(string pageword in pagewordad)
	{
		pageword__loop__id++;


	if (adindex<4)
	{

	templateBuilder.Append("			<td>");
	templateBuilder.Append(pageword.ToString());
	templateBuilder.Append("</td>\r\n");
	 adindex = adindex+1;
	

	}
	else
	{

	templateBuilder.Append("		</tr><tr>\r\n");
	templateBuilder.Append("			<td>");
	templateBuilder.Append(pageword.ToString());
	templateBuilder.Append("</td>\r\n");
	 adindex = 1;
	

	}	//end if


	}	//end loop


	if (pagewordad.Length%4>0)
	{


					for (int j = 0; j < (4 - pagewordad.Length % 4); j++)
					{
				
	templateBuilder.Append("			<td>&nbsp;</td>\r\n");

					}
				

	}	//end if

	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("	</tbody>\r\n");
	templateBuilder.Append("	</table>\r\n");
	templateBuilder.Append("</div>\r\n");

	}	//end if




	if (showforumlogin==1)
	{

	templateBuilder.Append("	<div class=\"main\">\r\n");
	templateBuilder.Append("		<h3>本版块已经被管理员设置了密码</h3>\r\n");
	templateBuilder.Append("		<form id=\"forumlogin\" name=\"forumlogin\" method=\"post\" action=\"\">\r\n");
	templateBuilder.Append("		<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"formtable\">\r\n");
	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<th><label for=\"forumpassword\">请输入密码</label></th>\r\n");
	templateBuilder.Append("			<td><input name=\"forumpassword\" type=\"password\" id=\"forumpassword\" size=\"20\" class=\"txt\"/></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");

	if (isseccode)
	{

	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<th><label for=\"vcode\">输入验证码</label></th>\r\n");
	templateBuilder.Append("			<td>\r\n");
	templateBuilder.Append("				<div style=\"position: relative;\">\r\n");

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
	templateBuilder.Append("		    </td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");

	}	//end if

	templateBuilder.Append("		<tbody>\r\n");
	templateBuilder.Append("		<tr>\r\n");
	templateBuilder.Append("			<th></th>\r\n");
	templateBuilder.Append("			<td><input type=\"submit\"  value=\"确定\"/></td>\r\n");
	templateBuilder.Append("		</tr>\r\n");
	templateBuilder.Append("		</tbody>\r\n");
	templateBuilder.Append("		</table>\r\n");
	templateBuilder.Append("		</form>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	}
	else
	{

	templateBuilder.Append("<div id=\"forumheader\" class=\"main cl\">\r\n");
	templateBuilder.Append("	<span class=\"y o\">\r\n");

	if (forum.Rules!="")
	{

	templateBuilder.Append("        <img id=\"rules_img\"  src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_no.gif\" alt=\"展开/收起\" onclick=\"toggle_collapse('rules');\"/>\r\n");

	}	//end if

	templateBuilder.Append("    </span>\r\n");

	if (page_err==0)
	{

	templateBuilder.Append("	<span class=\"y\">\r\n");

	if (ismoder)
	{

	templateBuilder.Append("<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("modcp.aspx?operation=attention&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"f_bold\">管理面板</a>\r\n");

	}	//end if

	templateBuilder.Append("	</span>\r\n");

	}	//end if

	templateBuilder.Append("	<div class=\"forumaction y\">\r\n");
	templateBuilder.Append("		<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=digest\" class=\"digest\">精华</a>\r\n");

	if (config.Rssstatus!=0&&forum.Allowrss!=0)
	{

	 aspxrewriteurl = this.RssAspxRewrite(forum.Fid);
	
	templateBuilder.Append("		<a class=\"feed\" target=\"_blank\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">RSS</a>	\r\n");

	}	//end if

	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<h1>\r\n");
	 aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid,0);
	templateBuilder.Append(forum.Name.ToString().Trim());
	templateBuilder.Append("</h1>\r\n");
	templateBuilder.Append("	<span class=\"forumstats\">今日: <strong class=\"xi1\">");
	templateBuilder.Append(forum.Todayposts.ToString().Trim());
	templateBuilder.Append("</strong><span class=\"pipe\">|</span>主题: <strong class=\"xi1\">");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append("</strong><span class=\"pipe\">|</span>帖子: <strong class=\"xi1\">");
	templateBuilder.Append(forum.Posts.ToString().Trim());
	templateBuilder.Append("</strong></span>\r\n");

	if (forum.Description!="")
	{

	templateBuilder.Append("	<p>");
	templateBuilder.Append(forum.Description.ToString().Trim());
	templateBuilder.Append("</p>\r\n");

	}	//end if

	templateBuilder.Append("	<p id=\"modedby\">\r\n");

	if (page_err==0)
	{

	templateBuilder.Append("版主: <span class=\"f_c\">\r\n");

	if (forum.Moderators!="")
	{

	templateBuilder.Append("		");
	templateBuilder.Append(forum.Moderators.ToString().Trim());
	templateBuilder.Append("\r\n");

	}
	else
	{

	templateBuilder.Append("		*空缺中*\r\n");

	}	//end if

	templateBuilder.Append("</span>\r\n");

	}	//end if

	templateBuilder.Append("	</p>\r\n");

	if (forum.Rules!="")
	{

	templateBuilder.Append("	<div id=\"rules\">");
	templateBuilder.Append(forum.Rules.ToString().Trim());
	templateBuilder.Append("</div>\r\n");

	}	//end if

	templateBuilder.Append("</div>\r\n");

	if (forum.Subforumcount>0)
	{


	templateBuilder.Append("<div id=\"subforum\" class=\"main cl list\">\r\n");
	templateBuilder.Append("	<div class=\"titlebar xg2\">\r\n");
	templateBuilder.Append("		<span class=\"y\">\r\n");

	if (forum.Moderators!="")
	{

	templateBuilder.Append("			分类版主: ");
	templateBuilder.Append(forum.Moderators.ToString().Trim());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("		<img id=\"category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("_img\"  src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_no.gif\" alt=\"展开/收起\" onclick=\"toggle_collapse('category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("');\" class=\"cursor\"/>\r\n");
	templateBuilder.Append("		</span>\r\n");
	templateBuilder.Append("		<h2>子版块</h2>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<div id=\"category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\"  class=\"fi\" summary=\"category_");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\">\r\n");
	templateBuilder.Append("	<table cellspacing=\"0\" cellpadding=\"0\">\r\n");
	templateBuilder.Append("	<tbody>	\r\n");

	if (forum.Colcount==1)
	{


	int subforum__loop__id=0;
	foreach(IndexPageForumInfo subforum in subforumlist)
	{
		subforum__loop__id++;

	templateBuilder.Append("			<tr>\r\n");
	 aspxrewriteurl = this.ShowForumAspxRewrite(subforum.Fid,0);
	
	templateBuilder.Append("				<th \r\n");

	if (config.Shownewposticon==1)
	{

	templateBuilder.Append("class=\"notopic \r\n");

	if (subforum.Havenew=="new")
	{

	templateBuilder.Append("new\r\n");

	}	//end if

	templateBuilder.Append("\"\r\n");

	}	//end if

	templateBuilder.Append(">\r\n");
	templateBuilder.Append("					<h2>\r\n");

	if (subforum.Icon!="")
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(subforum.Icon.ToString().Trim());
	templateBuilder.Append("\" border=\"0\" align=\"left\" hspace=\"5\" alt=\"");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("\"/>\r\n");

	}	//end if


	if (subforum.Redirect=="")
	{

	 aspxrewriteurl = this.ShowForumAspxRewrite(subforum.Fid,0,subforum.Rewritename);
	
	templateBuilder.Append("							<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">\r\n");

	}
	else
	{

	templateBuilder.Append("							<a href=\"");
	templateBuilder.Append(subforum.Redirect.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">\r\n");

	}	//end if

	templateBuilder.Append("						");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	if (subforum.Todayposts>0)
	{

	templateBuilder.Append("<span class=\"today\">(今日:<strong>");
	templateBuilder.Append(subforum.Todayposts.ToString().Trim());
	templateBuilder.Append("</strong>)</span>\r\n");

	}	//end if

	templateBuilder.Append("					</h2>\r\n");

	if (subforum.Description!="")
	{

	templateBuilder.Append("<p>");
	templateBuilder.Append(subforum.Description.ToString().Trim());
	templateBuilder.Append("</p>\r\n");

	}	//end if


	if (subforum.Moderators!="")
	{

	templateBuilder.Append("<p class=\"moderators\">版主:");
	templateBuilder.Append(subforum.Moderators.ToString().Trim());
	templateBuilder.Append("</p>\r\n");

	}	//end if

	templateBuilder.Append("				</th>\r\n");
	templateBuilder.Append("				<td class=\"nums\"><em>");
	templateBuilder.Append(subforum.Topics.ToString().Trim());
	templateBuilder.Append("</em> / ");
	templateBuilder.Append(subforum.Posts.ToString().Trim());
	templateBuilder.Append("</td>\r\n");
	templateBuilder.Append("				<td class=\"lastpost\">\r\n");

	if (subforum.Status==-1)
	{

	templateBuilder.Append("						私密论坛\r\n");

	}
	else
	{


	if (subforum.Lasttid!=0)
	{

	templateBuilder.Append("						<p>\r\n");
	 aspxrewriteurl = this.ShowTopicAspxRewrite(subforum.Lasttid,0);
	
	templateBuilder.Append("							<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(subforum.Lasttitle.ToString().Trim());
	templateBuilder.Append("</a>\r\n");
	templateBuilder.Append("						</p>\r\n");
	templateBuilder.Append("						<div class=\"topicbackwriter\">by\r\n");

	if (subforum.Lastposter!="")
	{


	if (subforum.Lastposterid==-1)
	{

	templateBuilder.Append("									游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(subforum.Lastposterid);
	
	templateBuilder.Append("									<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(subforum.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if


	}
	else
	{

	templateBuilder.Append("								匿名\r\n");

	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(subforum.Lasttid,0);
	
	templateBuilder.Append("						- 	<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(subforum.Lasttid.ToString().Trim());
	templateBuilder.Append("&page=end#lastpost\" title=\"");
	templateBuilder.Append(subforum.Lasttitle.ToString().Trim());
	templateBuilder.Append("\"><span>\r\n");
	templateBuilder.Append(Convert.ToDateTime(subforum.Lastpost).ToString(" yyyy-MM-dd HH:mm"));
	templateBuilder.Append("</span></a>\r\n");
	templateBuilder.Append("						</div>\r\n");

	}
	else
	{

	templateBuilder.Append("							从未\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("			  </tr>\r\n");

	}	//end loop


	}
	else
	{

	int subforumindex = 0;
	
	double colwidth = 99.6 / forum.Colcount;
	

	int subforum__loop__id=0;
	foreach(IndexPageForumInfo subforum in subforumlist)
	{
		subforum__loop__id++;

	 subforumindex = subforumindex+1;
	

	if (subforumindex==1)
	{

	templateBuilder.Append("			<tr>\r\n");

	}	//end if

	templateBuilder.Append("				<th style=\"width:");
	templateBuilder.Append(colwidth.ToString());
	templateBuilder.Append("%;\" \r\n");

	if (config.Shownewposticon==1)
	{

	templateBuilder.Append("class=\"notopic \r\n");

	if (subforum.Havenew=="new")
	{

	templateBuilder.Append("new\r\n");

	}	//end if

	templateBuilder.Append("\"\r\n");

	}	//end if

	templateBuilder.Append(">\r\n");
	templateBuilder.Append("				<h2>\r\n");

	if (subforum.Icon!="")
	{

	templateBuilder.Append("					<img src=\"");
	templateBuilder.Append(subforum.Icon.ToString().Trim());
	templateBuilder.Append("\" alt=\"");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("\" hspace=\"5\" />\r\n");

	}	//end if


	if (forum.Redirect=="")
	{

	 aspxrewriteurl = this.ShowForumAspxRewrite(subforum.Fid,0,subforum.Rewritename);
	
	templateBuilder.Append("					<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">\r\n");

	}
	else
	{

	templateBuilder.Append("					<a href=\"");
	templateBuilder.Append(subforum.Redirect.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">\r\n");

	}	//end if

	templateBuilder.Append("				");
	templateBuilder.Append(subforum.Name.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	if (subforum.Todayposts>0)
	{

	templateBuilder.Append("				<span class=\"time\">(今日:<strong>");
	templateBuilder.Append(subforum.Todayposts.ToString().Trim());
	templateBuilder.Append("</strong>)</span>\r\n");

	}	//end if

	templateBuilder.Append("				</h2>\r\n");
	templateBuilder.Append("				<p>主题:");
	templateBuilder.Append(subforum.Topics.ToString().Trim());
	templateBuilder.Append(", 帖数:");
	templateBuilder.Append(subforum.Posts.ToString().Trim());
	templateBuilder.Append("</p>\r\n");

	if (subforum.Status==-1)
	{

	templateBuilder.Append("				<p>私密版块</p>\r\n");

	}
	else
	{


	if (subforum.Lasttid!=0)
	{

	templateBuilder.Append("						<p>最后: <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(subforum.Lasttid.ToString().Trim());
	templateBuilder.Append("&page=end#lastpost\" title=\"");
	templateBuilder.Append(subforum.Lasttitle.ToString().Trim());
	templateBuilder.Append("\"><span>\r\n");
	templateBuilder.Append(Convert.ToDateTime(subforum.Lastpost).ToString(" yyyy-MM-dd HH:mm"));
	templateBuilder.Append("</span></a> by \r\n");

	if (subforum.Lastposter!="")
	{


	if (subforum.Lastposterid==-1)
	{

	templateBuilder.Append("									游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(subforum.Lastposterid);
	
	templateBuilder.Append("									<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(subforum.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if


	}
	else
	{

	templateBuilder.Append("								匿名\r\n");

	}	//end if

	templateBuilder.Append("						</p>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("				 </th>\r\n");

	if (subforumindex==forum.Colcount)
	{

	templateBuilder.Append("			</tr>\r\n");
	 subforumindex = 0;
	

	}	//end if


	}	//end loop


	if (subforumindex!=0)
	{

	for (int i = 0; i < forum.Colcount-subforumindex; i++)
	{
		templateBuilder.Append("<td>&nbsp;</td>");
	}

	templateBuilder.Append("			</tr>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("	</tbody>\r\n");
	templateBuilder.Append("	</table>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");



	}	//end if


	if (forum.Layer!=0)
	{

	templateBuilder.Append("<div class=\"pages_btns cl\">\r\n");
	templateBuilder.Append("	<div class=\"pages\">\r\n");
	templateBuilder.Append("		<cite class=\"pageback z\" id=\"visitedforums\"\r\n");

	if (showvisitedforumsmenu)
	{

	templateBuilder.Append(" onmouseover=\"$('visitedforums').id = 'visitedforumstmp';this.id = 'visitedforums';showMenu({'ctrlid':this.id, 'pos':'34'});\"\r\n");

	}	//end if

	templateBuilder.Append("><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\">返回</a></cite>\r\n");

	if (pagecount!=1)
	{

	templateBuilder.Append("			");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n");

	if (pagecount>8)
	{

	templateBuilder.Append("			<kbd><b id=\"pageidtxt1\" title=\"可以输入页码按回车键自动跳转\" onclick=\"$('pageidtxt1').style.display='none';$('pageidinput1').style.display='';$('pageidinput1').focus();\">");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("</b>\r\n");
	templateBuilder.Append("			<input name=\"gopage\" type=\"text\" class=\"txt\" id=\"pageidinput1\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" style=\"display:none;text-align:center;\" onfocus=\"this.value=this.defaultValue;this.size=this.value.length;this.select();\" onblur=\"$('pageidinput1').style.display='none';$('pageidtxt1').style.display='';\" onKeyDown=\"if(event.keyCode==13) { if(parseInt('");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append("')==1) {window.location='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum-");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("-' + (parseInt(this.value) > 0 ? parseInt(this.value) : 1) + '");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("';}else{window.location='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&page=' + (parseInt(this.value) > 0 ? parseInt(this.value) : 1);}}if((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode==8){this.size=this.value.length+1;return true;}else{return false;}\" size=\"");
	templateBuilder.Append(pageid.ToString().Trim().Length.ToString().Trim());
	templateBuilder.Append("\" maxlength=\"9\" />/ ");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("</kbd>\r\n");

	}	//end if

	templateBuilder.Append("			");
	templateBuilder.Append(nextpage.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("	</div>\r\n");

	if (userid<0||canposttopic)
	{

	templateBuilder.Append("	<span onmouseover=\"if($('newspecial_menu')!=null&&$('newspecial_menu').childNodes.length>0)  showMenu(this.id);\" id=\"newspecial\"><a title=\"发新话题\" id=\"newtopic\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\"\r\n");

	if (canposttopic)
	{

	templateBuilder.Append("onclick=\"showWindow('newthread', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("')\"\r\n");

	}
	else
	{

	templateBuilder.Append("onclick=\"showWindow('login', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');\"\r\n");

	}	//end if

	templateBuilder.Append("><img alt=\"发新话题\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/newtopic.png\" style=\"display:inline\"/></a></span>\r\n");

	}	//end if

	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<div id=\"headfilter\" class=\"cl\">\r\n");
	templateBuilder.Append("	<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(ShowForumAspxRewrite(forumid,0).ToString());
	templateBuilder.Append("\">全部</a>\r\n");
	int specialpost = forum.Allowpostspecial&1;
	

	if (specialpost==1)
	{

	templateBuilder.Append("	<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=poll\" \r\n");

	if (filter=="poll")
	{

	templateBuilder.Append("class=\"current\"\r\n");

	}	//end if

	templateBuilder.Append(">投票</a>\r\n");

	}	//end if

	 specialpost = forum.Allowpostspecial&4;
	

	if (specialpost==4)
	{

	templateBuilder.Append("	<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=reward\" id=\"rewardmenu\" class=\"\r\n");

	if (filter=="reward" || filter=="rewarding")
	{

	templateBuilder.Append("current\r\n");

	}
	else
	{


	if (filter=="rewarded")
	{

	templateBuilder.Append("current\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append(" drop\" onMouseOver=\"showMenu(this.id);\">悬赏</a>\r\n");

	}	//end if

	 specialpost = forum.Allowpostspecial&16;
	

	if (specialpost==16)
	{

	templateBuilder.Append("	<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=debate\" \r\n");

	if (filter=="debate")
	{

	templateBuilder.Append("class=\"current\"\r\n");

	}	//end if

	templateBuilder.Append(">辩论</a>\r\n");

	}	//end if


	if (forum.Applytopictype==1 && forum.Viewbytopictype==1)
	{

	templateBuilder.Append("		");
	templateBuilder.Append(topictypeselectlink.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<ul id=\"rewardmenu_menu\" class=\"p_pop\"  style=\"display: none\">\r\n");
	templateBuilder.Append("	<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=rewarding\">进行中的悬赏</a></li>\r\n");
	templateBuilder.Append("	<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&filter=rewarded\">已结束的悬赏</a></li>\r\n");
	templateBuilder.Append("</ul>\r\n");
	templateBuilder.Append("<div class=\"main thread\">\r\n");
	templateBuilder.Append("	<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("topicadmin.aspx?action=moderate&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&infloat=1\">\r\n");
	templateBuilder.Append("		<div class=\"category\">\r\n");
	templateBuilder.Append("		<table summary=\"");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("			<th><span title=\"在新窗口中打开帖子\" id=\"atarget\">新窗</span>筛选:\r\n");

	if (topictypeid==0)
	{

	templateBuilder.Append("					<a id=\"intervalmenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\" class=\"drop xg2\">全部主题</a>\r\n");
	templateBuilder.Append("				    <ul id=\"intervalmenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(0);\">全部主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(1);\">1 天以来主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(2);\">2 天以来主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(7);\">1 周以来主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(30);\">1 个月以来主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(90);\">3 个月以来主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(180);\">6 个月以来主题</a></li>\r\n");
	templateBuilder.Append("				        <li><a href=\"javascript:void(0);\" onclick=\"selectinterval(365);\">1 年以来主题</a></li>\r\n");
	templateBuilder.Append("				    </ul>\r\n");

	}	//end if

	templateBuilder.Append("				<a id=\"ordermenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\"  class=\"drop xg2\">最后回复时间</a>\r\n");
	templateBuilder.Append("				<ul id=\"ordermenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n");
	templateBuilder.Append("				    <li><a href=\"javascript:void(0);\" onclick=\"selectorder(1);\">最后回复时间</a></li>\r\n");
	templateBuilder.Append("				    <li><a href=\"javascript:void(0);\" onclick=\"selectorder(2);\">发布时间</a></li>\r\n");
	templateBuilder.Append("                    <li><a href=\"javascript:void(0);\" onclick=\"selectorder(3);\">查看次数</a></li>\r\n");
	templateBuilder.Append("                    <li><a href=\"javascript:void(0);\" onclick=\"selectorder(4);\">回复次数</a></li>\r\n");
	templateBuilder.Append("				</ul>\r\n");
	templateBuilder.Append("				<span class=\"pipe\">|</span>排序:\r\n");
	templateBuilder.Append("				<a id=\"directmenu\" onclick=\"showMenu(this.id);\" href=\"javascript:;\" class=\"drop xg2\">按降序排列</a>\r\n");
	templateBuilder.Append("				<ul id=\"directmenu_menu\" class=\"p_pop\" style=\"display: none\">\r\n");
	templateBuilder.Append("				    <li><a href=\"javascript:void(0);\" onclick=\"selectdirect(1);\">按降序排列</a></li>\r\n");
	templateBuilder.Append("				    <li><a href=\"javascript:void(0);\" onclick=\"selectdirect(0);\">按升序排列</a></li>\r\n");
	templateBuilder.Append("				</ul>\r\n");
	templateBuilder.Append("			    <script type=\"text/javascript\" reload=1>\r\n");
	templateBuilder.Append("			        var prefix = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?search=1&forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topictypeid.ToString());
	templateBuilder.Append("&filter=");
	templateBuilder.Append(filter.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("			        function loadsearchconditionlink() {\r\n");
	templateBuilder.Append("			            var intervalnumber = ");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("			            switch(intervalnumber){\r\n");
	templateBuilder.Append("			                case 1:$('intervalmenu').innerHTML='1 天以来主题';break;\r\n");
	templateBuilder.Append("			                case 2:$('intervalmenu').innerHTML='2 天以来主题';break;\r\n");
	templateBuilder.Append("			                case 7:$('intervalmenu').innerHTML='1 周以来主题';break;\r\n");
	templateBuilder.Append("			                case 30:$('intervalmenu').innerHTML='1 个月以来主题';break;\r\n");
	templateBuilder.Append("			                case 90:$('intervalmenu').innerHTML='3 个月以来主题';break;\r\n");
	templateBuilder.Append("			                case 180:$('intervalmenu').innerHTML='6 个月以来主题';break;\r\n");
	templateBuilder.Append("			                case 365:$('intervalmenu').innerHTML='1 年以来主题';break;\r\n");
	templateBuilder.Append("			                default:break;\r\n");
	templateBuilder.Append("			            }\r\n");
	templateBuilder.Append("			            var ordernumber = ");
	templateBuilder.Append(order.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("			            if(ordernumber == 2)\r\n");
	templateBuilder.Append("			                $('ordermenu').innerHTML = '发布时间';\r\n");
	templateBuilder.Append("			            var directnumber = ");
	templateBuilder.Append(direct.ToString());
	templateBuilder.Append(";\r\n");
	templateBuilder.Append("			            if(directnumber == 0)\r\n");
	templateBuilder.Append("			                $('directmenu').innerHTML = '按升序排列';\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        loadsearchconditionlink();\r\n");
	templateBuilder.Append("			        function selectinterval(selectvalue) {\r\n");
	templateBuilder.Append("			            window.location.href = prefix + '&order=");
	templateBuilder.Append(order.ToString());
	templateBuilder.Append("&direct=");
	templateBuilder.Append(direct.ToString());
	templateBuilder.Append("&interval=' + selectvalue;\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        function selectorder(selectvalue){\r\n");
	templateBuilder.Append("			            window.location.href = prefix + '&order=' + selectvalue + '&direct=");
	templateBuilder.Append(direct.ToString());
	templateBuilder.Append("&interval=");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			        function selectdirect(selectvalue){\r\n");
	templateBuilder.Append("			            window.location.href = prefix + '&order=");
	templateBuilder.Append(order.ToString());
	templateBuilder.Append("&direct=' + selectvalue + '&interval=");
	templateBuilder.Append(interval.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("			        }\r\n");
	templateBuilder.Append("			    </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			</th>\r\n");
	templateBuilder.Append("			<td class=\"by\">作者</td>\r\n");
	templateBuilder.Append("			<td class=\"num\">回复/查看</td>\r\n");
	templateBuilder.Append("			<td class=\"by\">最后发表</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("		</table>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div class=\"threadlist\">\r\n");
	templateBuilder.Append("		<table summary=\"");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("\" id=\"threadlist\" cellspacing=\"0\" cellpadding=\"0\">\r\n");

	int announcement__loop__id=0;
	foreach(DataRow announcement in announcementlist.Rows)
	{
		announcement__loop__id++;

	templateBuilder.Append("			<tbody>\r\n");
	templateBuilder.Append("			<tr>\r\n");
	templateBuilder.Append("				<td class=\"folder\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/icon_announcement.gif\" alt=\"announcement\" /></td>\r\n");
	templateBuilder.Append("				<td class=\"icon\">&nbsp;</td>\r\n");

	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("				<td class=\"icon\">&nbsp;</td>		\r\n");

	}	//end if

	templateBuilder.Append("				<th class=\"subject f_bold\">\r\n");
	templateBuilder.Append("					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("announcement.aspx#" + announcement["id"].ToString().Trim() + "\">" + announcement["title"].ToString().Trim() + "</a>\r\n");
	templateBuilder.Append("				</th>\r\n");
	templateBuilder.Append("				<td class=\"by\">\r\n");
	templateBuilder.Append("					<cite>\r\n");

	if (Utils.StrToInt(announcement["posterid"].ToString().Trim(), 0)==-1)
	{

	templateBuilder.Append("						游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(announcement["posterid"].ToString().Trim());
	
	templateBuilder.Append("						<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">" + announcement["poster"].ToString().Trim() + "</a>\r\n");

	}	//end if

	templateBuilder.Append("					</cite>\r\n");
	templateBuilder.Append("					<em>" + announcement["starttime"].ToString().Trim() + "</em>\r\n");
	templateBuilder.Append("				</td>\r\n");
	templateBuilder.Append("				<td class=\"num\">&nbsp;</td>\r\n");
	templateBuilder.Append("				<td class=\"by\">&nbsp;</td>\r\n");
	templateBuilder.Append("			</tr>\r\n");
	templateBuilder.Append("			</tbody>\r\n");
break;

	}	//end loop

	string tdivimg = "";
	

	int toptopic__loop__id=0;
	foreach(TopicInfo toptopic in toptopiclist)
	{
		toptopic__loop__id++;

	templateBuilder.Append("			<tbody>\r\n");
	templateBuilder.Append("				<tr>\r\n");
	templateBuilder.Append("					<td class=\"folder\">\r\n");
	 aspxrewriteurl = this.ShowTopicAspxRewrite(toptopic.Tid,0);
	
	templateBuilder.Append("						<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/t_top");
	templateBuilder.Append(toptopic.Displayorder.ToString().Trim());
	templateBuilder.Append(".gif\"/></a>\r\n");
	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("					<td class=\"icon\">\r\n");

	if (toptopic.Special==0)
	{


	if (toptopic.Iconid!=0)
	{

	templateBuilder.Append("									<img src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(toptopic.Iconid.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"listicon\" />\r\n");

	}
	else
	{

	templateBuilder.Append("									&nbsp;\r\n");

	}	//end if


	}	//end if

//特殊帖图标

	if (toptopic.Special==1)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/pollsmall.gif\" alt=\"投票\" />\r\n");

	}	//end if


	if (toptopic.Special==2)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/bonus.gif\" alt=\"悬赏\"/>\r\n");

	}	//end if


	if (toptopic.Special==3)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/rewardsmallend.gif\" alt=\"悬赏已结束\"/>\r\n");

	}	//end if


	if (toptopic.Special==4)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/debatesmall.gif\" alt=\"辩论\"/>\r\n");

	}	//end if

	templateBuilder.Append("					</td>\r\n");

	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("					<td class=\"icon\">						\r\n");

	if (toptopic.Fid==forum.Fid && toptopic.Closed<=1)
	{

	templateBuilder.Append("						<input type=\"checkbox\" name=\"topicid\" topictype=\"displayorder\" value=\"");
	templateBuilder.Append(toptopic.Tid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"modclick(this);\"/>\r\n");

	}
	else
	{

	templateBuilder.Append("						<input type=\"checkbox\" disabled />\r\n");

	}	//end if

	templateBuilder.Append("					</td>\r\n");

	}	//end if

	templateBuilder.Append("					<th class=\"subject hot\">\r\n");

	if (toptopic.Digest>0)
	{

	templateBuilder.Append("							<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/digest");
	templateBuilder.Append(toptopic.Digest.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"digtest\"/></label>\r\n");

	}	//end if


	if (toptopic.Rate>0)
	{

	templateBuilder.Append("							<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/agree.gif\" alt=\"正分\"/></label>\r\n");

	}	//end if


	if (toptopic.Rate<0)
	{

	templateBuilder.Append("							<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/disagree.gif\" alt=\"负分\"/></label>\r\n");

	}	//end if


	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{


	if (forum.Viewbytopictype==1 && toptopic.Topictypename!="")
	{

	templateBuilder.Append("							<em>[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(toptopic.Fid.ToString().Trim());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(toptopic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(toptopic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]</em>\r\n");

	}
	else if (toptopic.Topictypename!="")
	{

	templateBuilder.Append("							<em>[");
	templateBuilder.Append(toptopic.Topictypename.ToString().Trim());
	templateBuilder.Append("]</em>\r\n");

	}	//end if


	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(toptopic.Tid,0);
	

	if (toptopic.Special==3)
	{

	 aspxrewriteurl = this.ShowBonusAspxRewrite(toptopic.Tid,0);
	

	}	//end if


	if (toptopic.Special==4)
	{

	 aspxrewriteurl = this.ShowDebateAspxRewrite(toptopic.Tid);
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(toptopic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("							<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(toptopic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}
	else
	{

	templateBuilder.Append("							<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(toptopic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if


	if (toptopic.Attachment==1)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment.gif\" alt=\"附件\"/>\r\n");

	}
	else if (toptopic.Attachment==2)
	{

	templateBuilder.Append("						   <img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment_image.gif\" alt=\"图片附件\"/>\r\n");

	}	//end if


	if (toptopic.Special==2)
	{

	templateBuilder.Append("							- [悬赏 ");
	templateBuilder.Append(bonusextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(toptopic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(bonusextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n");

	}
	else if (toptopic.Special==3)
	{

	templateBuilder.Append("							- [悬赏已结束]\r\n");

	}
	else if (toptopic.Special==0)
	{


	if (toptopic.Price>0)
	{

	templateBuilder.Append("								- [售价 ");
	templateBuilder.Append(topicextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(toptopic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(topicextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n");

	}	//end if


	}	//end if


	if (toptopic.Readperm>0)
	{

	templateBuilder.Append("							- [阅读权限 <span class=\"bold\">");
	templateBuilder.Append(toptopic.Readperm.ToString().Trim());
	templateBuilder.Append("</span>] \r\n");

	}	//end if


	if (toptopic.Replies/ppp>0)
	{

	templateBuilder.Append("							<span class=\"threadpages\"><script type=\"text/javascript\">getpagenumbers(\"");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("\",");
	templateBuilder.Append(toptopic.Replies.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(ppp.ToString());
	templateBuilder.Append(",0,\"\",");
	templateBuilder.Append(toptopic.Tid.ToString().Trim());
	templateBuilder.Append(",1, \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\", aspxrewrite);</");
	templateBuilder.Append("script></span>				\r\n");

	}	//end if

	templateBuilder.Append("					</th>\r\n");
	templateBuilder.Append("					<td class=\"by\">\r\n");
	templateBuilder.Append("						<cite>\r\n");

	if (toptopic.Posterid==-1)
	{

	templateBuilder.Append("							游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(toptopic.Posterid);
	
	templateBuilder.Append("							<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(toptopic.Poster.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if

	templateBuilder.Append("</cite>\r\n");
	string ttpdtime = ForumUtils.ConvertDateTime(toptopic.Postdatetime);
	
	templateBuilder.Append("						<em>");
	templateBuilder.Append(ttpdtime.ToString());
	templateBuilder.Append("</em>\r\n");
	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("					<td class=\"num\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(toptopic.Replies.ToString().Trim());
	templateBuilder.Append("</a><em>");
	templateBuilder.Append(toptopic.Views.ToString().Trim());
	templateBuilder.Append("</em></td>\r\n");
	templateBuilder.Append("					<td class=\"by\">\r\n");
	templateBuilder.Append("						<cite>\r\n");

	if (toptopic.Lastposterid==-1)
	{

	templateBuilder.Append("								游客\r\n");

	}
	else
	{

	templateBuilder.Append("								<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(UserInfoAspxRewrite(toptopic.Lastposterid).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(toptopic.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if

	templateBuilder.Append("						</cite>\r\n");
	templateBuilder.Append("						<em><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(toptopic.Tid.ToString().Trim());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("&page=end#lastpost\">\r\n");
	string ttlp = ForumUtils.ConvertDateTime(toptopic.Lastpost);
	
	templateBuilder.Append("						");
	templateBuilder.Append(ttlp.ToString());
	templateBuilder.Append("</a></em>\r\n");
	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("				</tr>\r\n");
	templateBuilder.Append("			</tbody>\r\n");

	}	//end loop


	if (toptopiclist.Count>0 && topiclist.Count>0)
	{

	templateBuilder.Append("			<tbody class=\"separation\">\r\n");
	templateBuilder.Append("				<tr>\r\n");
	templateBuilder.Append("					<td class=\"folder\">&nbsp;</td>\r\n");
	templateBuilder.Append("					<td class=\"icon\">&nbsp;</td>\r\n");

	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("					<td class=\"icon\">&nbsp;</td>\r\n");

	}	//end if

	templateBuilder.Append("					<th>版块主题</th>\r\n");
	templateBuilder.Append("					<td class=\"author\">&nbsp;</td>\r\n");
	templateBuilder.Append("					<td class=\"nums\">&nbsp;</td>\r\n");
	templateBuilder.Append("					<td class=\"lastpost\">&nbsp;</td>\r\n");
	templateBuilder.Append("				</tr>\r\n");
	templateBuilder.Append("			</tbody>\r\n");

	}	//end if


	int topic__loop__id=0;
	foreach(TopicInfo topic in topiclist)
	{
		topic__loop__id++;

	templateBuilder.Append("			<tbody>\r\n");
	templateBuilder.Append("				<tr>\r\n");
	templateBuilder.Append("					<td class=\"folder\">\r\n");

	if (topic.Folder!="")
	{

	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	
	templateBuilder.Append("						<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\" target=\"_blank\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/folder_");
	templateBuilder.Append(topic.Folder.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"topicicon\" /></a>\r\n");

	}	//end if

	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("					<td class=\"icon\">\r\n");

	if (topic.Special==0)
	{


	if (topic.Iconid!=0)
	{

	templateBuilder.Append("								<img src=\"");
	templateBuilder.Append(posticondir.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(topic.Iconid.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"listicon\" />\r\n");

	}
	else
	{

	templateBuilder.Append("								&nbsp;\r\n");

	}	//end if


	}	//end if

/*特殊帖图标*/

	if (topic.Special==1)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/pollsmall.gif\" alt=\"投票\" />\r\n");

	}	//end if


	if (topic.Special==2)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/bonus.gif\" alt=\"悬赏\"/>\r\n");

	}	//end if


	if (topic.Special==3)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/rewardsmallend.gif\" alt=\"悬赏已结束\"/>\r\n");

	}	//end if


	if (topic.Special==4)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/debatesmall.gif\" alt=\"辩论\"/>\r\n");

	}	//end if

	templateBuilder.Append("					</td>	\r\n");

	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("					<td class=\"icon\">				\r\n");

	if (topic.Closed<=1)
	{

	templateBuilder.Append("						<input type=\"checkbox\" name=\"topicid\" value=\"");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" onclick=\"modclick(this);\"/>\r\n");

	}
	else
	{

	templateBuilder.Append("						<input type=\"checkbox\" disabled />\r\n");

	}	//end if

	templateBuilder.Append("					</td>\r\n");

	}	//end if

	templateBuilder.Append("					<th class=\"subject\">\r\n");

	if (topic.Digest>0)
	{

	templateBuilder.Append("						<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/digest");
	templateBuilder.Append(topic.Digest.ToString().Trim());
	templateBuilder.Append(".gif\" alt=\"精华\"/></label>\r\n");

	}	//end if


	if (topic.Rate>0)
	{

	templateBuilder.Append("						<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/agree.gif\" alt=\"正分\"/></label>\r\n");

	}	//end if


	if (topic.Rate<0)
	{

	templateBuilder.Append("						<label class=\"y\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/disagree.gif\" alt=\"负分\"/></label>\r\n");

	}	//end if


	if (pageid<3 && forum.Allowthumbnail==1)
	{


	if (topic.Attachment==2)
	{

	templateBuilder.Append("								<span id=\"t_thumbnail_");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\" onmouseover=\"showMenu(this.id, 0, 0, 1, 0)\">\r\n");

	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{

	templateBuilder.Append("									<em>\r\n");

	if (forum.Viewbytopictype==1 && topic.Topictypename!="")
	{

	templateBuilder.Append("									[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]\r\n");

	}
	else if (topic.Topictypename!="")
	{

	templateBuilder.Append("									[");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("]\r\n");

	}	//end if

	templateBuilder.Append("									</em>\r\n");

	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	

	if (topic.Special==3)
	{

	 aspxrewriteurl = this.ShowBonusAspxRewrite(topic.Tid,0);
	

	}	//end if


	if (topic.Special==4)
	{

	 aspxrewriteurl = this.ShowDebateAspxRewrite(topic.Tid);
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("									<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}
	else
	{

	templateBuilder.Append("									<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if


	if (topic.Folder=="new")
	{

	templateBuilder.Append("									<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/posts_new.gif\" />\r\n");

	}	//end if

	templateBuilder.Append("								</span>\r\n");

	}
	else
	{


	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{


	if (forum.Viewbytopictype==1 && topic.Topictypename!="")
	{

	templateBuilder.Append("									<em>[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]</em>\r\n");

	}
	else if (topic.Topictypename!="")
	{

	templateBuilder.Append("									<em>[");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("]</em>\r\n");

	}	//end if


	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	

	if (topic.Special==3)
	{

	 aspxrewriteurl = this.ShowBonusAspxRewrite(topic.Tid,0);
	

	}	//end if


	if (topic.Special==4)
	{

	 aspxrewriteurl = this.ShowDebateAspxRewrite(topic.Tid);
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("									<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}
	else
	{

	templateBuilder.Append("									<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if


	if (topic.Folder=="new")
	{

	templateBuilder.Append("									<span class=\"new\">NEW</span>\r\n");

	}	//end if


	}	//end if


	}
	else
	{


	if (forum.Applytopictype==1 && forum.Topictypeprefix==1)
	{


	if (forum.Viewbytopictype==1 && topic.Topictypename!="")
	{

	templateBuilder.Append("									<em>[<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&typeid=");
	templateBuilder.Append(topic.Typeid.ToString().Trim());
	templateBuilder.Append("\" >");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("</a>]</em>\r\n");

	}
	else if (topic.Topictypename!="")
	{

	templateBuilder.Append("									<em>[");
	templateBuilder.Append(topic.Topictypename.ToString().Trim());
	templateBuilder.Append("]</em>\r\n");

	}	//end if


	}	//end if

	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid,0);
	

	if (topic.Special==3)
	{

	 aspxrewriteurl = this.ShowBonusAspxRewrite(topic.Tid,0);
	

	}	//end if


	if (topic.Special==4)
	{

	 aspxrewriteurl = this.ShowDebateAspxRewrite(topic.Tid);
	

	}	//end if

	int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);
	

	if (ishtmltitle==1)
	{

	templateBuilder.Append("								<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(Topics.GetHtmlTitle(topic.Tid).ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}
	else
	{

	templateBuilder.Append("								<a onclick=\"atarget(this)\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Title.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if


	if (topic.Folder=="new")
	{

	templateBuilder.Append("								<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/posts_new.gif\"/>\r\n");

	}	//end if


	}	//end if


	if (topic.Attachment==1)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment.gif\" alt=\"附件\"/>\r\n");

	}
	else if (topic.Attachment==2)
	{

	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/attachment_image.gif\" alt=\"图片附件\"/>\r\n");

	}	//end if


	if (topic.Special==2)
	{

	templateBuilder.Append("							- [悬赏 ");
	templateBuilder.Append(bonusextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(bonusextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n");

	}
	else if (topic.Special==3)
	{

	templateBuilder.Append("							- [悬赏已结束]\r\n");

	}
	else if (topic.Special==0)
	{


	if (topic.Price>0)
	{

	templateBuilder.Append("								- [售价 ");
	templateBuilder.Append(topicextcreditsinfo.Name.ToString().Trim());
	templateBuilder.Append(" <span class=\"bold\">");
	templateBuilder.Append(topic.Price.ToString().Trim());
	templateBuilder.Append("</span> ");
	templateBuilder.Append(topicextcreditsinfo.Unit.ToString().Trim());
	templateBuilder.Append("] \r\n");

	}	//end if


	}	//end if


	if (topic.Readperm>0)
	{

	templateBuilder.Append("							- [阅读权限 <span class=\"bold\">");
	templateBuilder.Append(topic.Readperm.ToString().Trim());
	templateBuilder.Append("</span>] \r\n");

	}	//end if


	if (topic.Replies/ppp>0)
	{

	templateBuilder.Append("							<span class=\"threadpages\"><script type=\"text/javascript\">getpagenumbers(\"");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("\", ");
	templateBuilder.Append(topic.Replies.ToString().Trim());
	templateBuilder.Append(",");
	templateBuilder.Append(ppp.ToString());
	templateBuilder.Append(",0,\"\",");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append(",1, \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\", aspxrewrite);</");
	templateBuilder.Append("script></span>\r\n");

	}	//end if

	templateBuilder.Append("					</th>\r\n");
	templateBuilder.Append("					<td class=\"by\">\r\n");
	templateBuilder.Append("						<cite>\r\n");

	if (topic.Posterid==-1)
	{

	templateBuilder.Append("								游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);
	
	templateBuilder.Append("								<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\">");
	templateBuilder.Append(topic.Poster.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if

	templateBuilder.Append("						</cite>\r\n");
	string tpdtime = ForumUtils.ConvertDateTime(topic.Postdatetime);
	
	templateBuilder.Append("						<em>");
	templateBuilder.Append(tpdtime.ToString());
	templateBuilder.Append("</em>\r\n");
	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("					<td class=\"num\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" class=\"xg2\">");
	templateBuilder.Append(topic.Replies.ToString().Trim());
	templateBuilder.Append("</a><em>");
	templateBuilder.Append(topic.Views.ToString().Trim());
	templateBuilder.Append("</em></td>\r\n");
	templateBuilder.Append("					<td class=\"by\">\r\n");
	templateBuilder.Append("						<cite>\r\n");

	if (topic.Lastposterid==-1)
	{

	templateBuilder.Append("							游客\r\n");

	}
	else
	{

	templateBuilder.Append("							<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(UserInfoAspxRewrite(topic.Lastposterid).ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(topic.Lastposter.ToString().Trim());
	templateBuilder.Append("</a>\r\n");

	}	//end if

	templateBuilder.Append("						</cite>\r\n");
	templateBuilder.Append("						<em><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showtopic.aspx?topicid=");
	templateBuilder.Append(topic.Tid.ToString().Trim());
	templateBuilder.Append("\r\n");

	if (config.Aspxrewrite!=1)
	{

	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("&page=end#lastpost\">\r\n");
	string tlp = ForumUtils.ConvertDateTime(topic.Lastpost);
	
	templateBuilder.Append("						");
	templateBuilder.Append(tlp.ToString());
	templateBuilder.Append("</a></em>\r\n");
	templateBuilder.Append("					</td>\r\n");
	templateBuilder.Append("				</tr>\r\n");

	if (pageid<3 && forum.Allowthumbnail==1)
	{


	if (topic.Attachment==2)
	{

	string timg = Attachments.GetThumbnailByTid(topic.Tid,160,ThumbnailType.Thumbnail);
	

	if (timg!="")
	{

	 tdivimg = tdivimg+"<div id='t_thumbnail_" + topic.Tid + "_menu' style='display: none;' class='popupmenu_popup'><img src='" + timg + "' /></div>";
	

	}	//end if


	}	//end if


	}	//end if

	templateBuilder.Append("			</tbody>\r\n");

	}	//end loop

	templateBuilder.Append("		</table>\r\n");

	if (topiclist.Count<=0)
	{

	templateBuilder.Append("			<div class=\"zerothreads\">当前板块暂无主题</div>\r\n");

	}	//end if


	if (useradminid>0 && ismoder)
	{

	templateBuilder.Append("            <div id=\"modlayer\" style=\"display: none;\">\r\n");
	templateBuilder.Append("				<input type=\"hidden\" name=\"optgroup\" />\r\n");
	templateBuilder.Append("				<input type=\"hidden\" name=\"operat\" />\r\n");
	templateBuilder.Append("				<input type=\"hidden\" name=\"winheight\" />\r\n");
	templateBuilder.Append("				<a class=\"collapse\" href=\"javascript:;\" onclick=\"$('modlayer').className='collapsed'\">最小化</a>\r\n");
	templateBuilder.Append("				<label><input class=\"checkbox\" type=\"checkbox\" name=\"chkall\" onclick=\"if(!($('modcount').innerHTML = modclickcount = checkall(this.form, 'topicid'))) {$('modlayer').style.display = 'none';}\" /> 全选</label>\r\n");
	templateBuilder.Append("				<h4><span>选中</span><strong onmouseover=\"$('moremodoption').style.display='block'\" onclick=\"$('modlayer').className=''\" id=\"modcount\"></strong><span>篇: </span></h4>\r\n");
	templateBuilder.Append("				<p>\r\n");
	templateBuilder.Append("					<strong><a href=\"javascript:;\" onclick=\"modthreads(3, 'delete');return false;\">删除</a></strong>\r\n");
	templateBuilder.Append("					<span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<strong><a href=\"javascript:;\" onclick=\"modthreads(2, 'move');return false;\">移动</a></strong>\r\n");
	templateBuilder.Append("					<span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<strong><a href=\"javascript:;\" onclick=\"modthreads(2, 'type');return false;\">分类</a></strong>\r\n");
	templateBuilder.Append("					<span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<strong><a href=\"javascript:;\" onclick=\"modthreads(2, 'identify');return false;\">鉴定</a></strong>\r\n");
	templateBuilder.Append("				</p>\r\n");
	templateBuilder.Append("				<div id=\"moremodoption\">\r\n");
	templateBuilder.Append("					<a href=\"javascript:;\" onclick=\"modthreads(1, 'displayorder');return false;\">置顶</a>\r\n");
	templateBuilder.Append("					<a href=\"javascript:;\" onclick=\"modthreads(1, 'highlight');return false;\">高亮</a>\r\n");
	templateBuilder.Append("					<a href=\"javascript:;\" onclick=\"modthreads(1, 'digest');return false;\">精华</a>\r\n");
	templateBuilder.Append("					<span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("					<a href=\"javascript:;\" onclick=\"modthreads(3, 'bump');return false;\">提升下沉</a>\r\n");
	templateBuilder.Append("					<a href=\"javascript:;\" onclick=\"modthreads(4,'close');return false;\">关闭打开</a>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("            </div>\r\n");

	}	//end if

	templateBuilder.Append("		</form>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("<div class=\"pages_btns cl\">\r\n");
	templateBuilder.Append("	<div class=\"pages\">\r\n");
	templateBuilder.Append("		<cite class=\"pageback z\" id=\"visitedforums\"\r\n");

	if (showvisitedforumsmenu)
	{

	templateBuilder.Append(" onmouseover=\"$('visitedforums').id = 'visitedforumstmp';this.id = 'visitedforums';showMenu({'ctrlid':this.id, 'pos':'34'});\"\r\n");

	}	//end if

	templateBuilder.Append("><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\">返回</a></cite>\r\n");

	if (pagecount!=1)
	{

	templateBuilder.Append("			");
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n");

	if (pagecount>8)
	{

	templateBuilder.Append("			<kbd><b id=\"pageidtxt1\" title=\"可以输入页码按回车键自动跳转\" onclick=\"$('pageidtxt1').style.display='none';$('pageidinput1').style.display='';$('pageidinput1').focus();\">");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("</b>\r\n");
	templateBuilder.Append("			<input name=\"gopage\" type=\"text\" class=\"txt\" id=\"pageidinput1\" value=\"");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" style=\"display:none;text-align:center;\" onfocus=\"this.value=this.defaultValue;this.size=this.value.length;this.select();\" onblur=\"$('pageidinput1').style.display='none';$('pageidtxt1').style.display='';\" onKeyDown=\"if(event.keyCode==13) { if(parseInt('");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append("')==1) {window.location='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum-");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("-' + (parseInt(this.value) > 0 ? parseInt(this.value) : 1) + '");
	templateBuilder.Append(config.Extname.ToString().Trim());
	templateBuilder.Append("';}else{window.location='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&page=' + (parseInt(this.value) > 0 ? parseInt(this.value) : 1);}}if((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode==8){this.size=this.value.length+1;return true;}else{return false;}\" size=\"");
	templateBuilder.Append(pageid.ToString().Trim().Length.ToString().Trim());
	templateBuilder.Append("\" maxlength=\"9\" />/ ");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("</kbd>\r\n");

	}	//end if

	templateBuilder.Append("			");
	templateBuilder.Append(nextpage.ToString());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("	</div>\r\n");

	if (userid<0||canposttopic)
	{

	templateBuilder.Append("	<span onmouseover=\"if($('newspecial2_menu')!=null&&$('newspecial_menu').childNodes.length>0)  showMenu(this.id);\" id=\"newspecial2\"><a title=\"发新话题\" id=\"newtopic2\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" onclick=\"showWindow('newthread', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("')\"><img alt=\"发新话题\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/newtopic.png\"  style=\"display:inline\"/></a></span>\r\n");

	}	//end if

	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append(tdivimg.ToString());
	templateBuilder.Append("\r\n");

	if (canquickpost)
	{


	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n");
	string seditorid = "infloatquickposttopic";
	

	if (infloat!=1)
	{

	 seditorid = "quickposttopic";
	

	}	//end if

	templateBuilder.Append("<form method=\"post\" name=\"postform\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\" action=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">\r\n");
	templateBuilder.Append("<div id=\"quickpost\" class=\"quickpost cl \r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("main\r\n");

	}	//end if

	templateBuilder.Append("\">\r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("	<h4 class=\"bm_h\">\r\n");

	}
	else
	{

	templateBuilder.Append("	<h4 class=\"flb\">\r\n");

	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("	<span class=\"y\">\r\n");
	templateBuilder.Append("		<a title=\"关闭\" onclick=\"hideWindow('newthread')\" class=\"flbc\" href=\"javascript:;\">关闭</a>\r\n");
	templateBuilder.Append("	</span>\r\n");

	}	//end if

	templateBuilder.Append("	<em>快速发帖</em>\r\n");

	if (infloat==1 && needaudit)
	{

	templateBuilder.Append("    <span class=\"needverify\">需审核</span>\r\n");

	}	//end if

	templateBuilder.Append("	</h4>\r\n");
	templateBuilder.Append("	<div class=\"bm_inner c cl\">\r\n");

	if (infloat!=1)
	{


	if (quickeditorad!="")
	{

	templateBuilder.Append("		<div class=\"leaderboard\">");
	templateBuilder.Append(quickeditorad.ToString());
	templateBuilder.Append("</div>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		<div class=\"pbt cl\">\r\n");

	if (forum.Applytopictype==1 && topictypeselectoptions!="")
	{

	templateBuilder.Append("			<div class=\"ftid\">\r\n");
	templateBuilder.Append("				<select name=\"typeid\" id=\"typeid\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"1\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"10\"\r\n");

	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(topictypeselectoptions.ToString());
	templateBuilder.Append("</select>\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\">simulateSelect('typeid');</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			</div>\r\n");

	}	//end if

	templateBuilder.Append("			<input type=\"text\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title\" name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" size=\"60\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"2\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"11\"\r\n");

	}	//end if

	templateBuilder.Append(" value=\"\" class=\"txt postpx\"/>\r\n");
	templateBuilder.Append("			<em id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("validatemessage\"></em>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div class=\"pbt cl\">\r\n");
	templateBuilder.Append("			<span style=\"display:none\">\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"htmlon\" id=\"htmlon\" \r\n");

	if (usergroupinfo.Allowhtml!=1)
	{

	templateBuilder.Append(" disabled\r\n");

	}	//end if

	templateBuilder.Append("/>\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"parseurloff\" id=\"parseurloff\" \r\n");

	if (parseurloff==1)
	{

	templateBuilder.Append("checked \r\n");

	}	//end if

	templateBuilder.Append(" />\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"smileyoff\" id=\"smileyoff\" \r\n");

	if (smileyoff==1)
	{

	templateBuilder.Append(" checked disabled \r\n");

	}	//end if

	templateBuilder.Append(" />\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"bbcodeoff\" id=\"bbcodeoff\" \r\n");

	if (bbcodeoff==1)
	{

	templateBuilder.Append(" checked disabled \r\n");

	}	//end if

	templateBuilder.Append(" />\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"usesig\" id=\"usesig\"\r\n");

	if (usesig==1)
	{

	templateBuilder.Append("checked\r\n");

	}	//end if

	templateBuilder.Append("/>\r\n");
	templateBuilder.Append("			</span>\r\n");
	templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("				var bbinsert = parseInt('1');\r\n");
	templateBuilder.Append("				var smiliesCount = 24;\r\n");
	templateBuilder.Append("				var colCount = 8;\r\n");
	templateBuilder.Append("			</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			<div \r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("style=\"margin-right:170px;\" \r\n");

	}
	else
	{

	templateBuilder.Append("style=\"width:600px;\"\r\n");

	}	//end if

	templateBuilder.Append(">\r\n");
	char comma = ',';
	

	templateBuilder.Append("<link href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/seditor.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
	templateBuilder.Append("<div class=\"editor_tb\">\r\n");
	templateBuilder.Append("	<span class=\"y\">\r\n");

	if (topicid>0)
	{

	templateBuilder.Append("		<a onclick=\"switchAdvanceMode(this.href,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form');doane(event);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\">高级模式</a>\r\n");

	}
	else
	{

	templateBuilder.Append("		<a onclick=\"switchAdvanceMode(this.href,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form');doane(event);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\">高级模式</a>\r\n");

	}	//end if


	if (infloat!=1)
	{


	if (userid<0||canposttopic)
	{

	templateBuilder.Append("		<span style=\"display:none;\" onmouseover=\"if($('newspecial_menu').childNodes.length>0)  showMenu(this.id);\" id=\"seditor_newspecial\"><a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\" onclick=\"showWindow('newthread',  '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("');doane(event);\">发新主题</a></span>\r\n");
	templateBuilder.Append("        <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("            setTimeout('if ($(\\'seditor_newspecial_menu\\') != null) $(\\'seditor_newspecial\\').style.display = \\'\\';', 800);\r\n");
	templateBuilder.Append("        </");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("	</span>\r\n");
	templateBuilder.Append("	<div>\r\n");
	templateBuilder.Append("	<!--{if in_array('bold', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"粗体\" class=\"tb_bold\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[b]', '[/b]')\">B</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('color', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"颜色\" class=\"tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor\" onclick=\"showMenu(this.id, true, 0, 2)\">Color</a>\r\n");
	string coloroptions = "Black,Sienna,DarkOliveGreen,DarkGreen,DarkSlateBlue,Navy,Indigo,DarkSlateGray,DarkRed,DarkOrange,Olive,Green,Teal,Blue,SlateGray,DimGray,Red,SandyBrown,YellowGreen,SeaGreen,MediumTurquoise,RoyalBlue,Purple,Gray,Magenta,Orange,Yellow,Lime,Cyan,DeepSkyBlue,DarkOrchid,Silver,Pink,Wheat,LemonChiffon,PaleGreen,PaleTurquoise,LightBlue,Plum,White";
	
	templateBuilder.Append("		<!--{eval $coloroptions = array('Black', 'Sienna', 'DarkOliveGreen', 'DarkGreen', 'DarkSlateBlue', 'Navy', 'Indigo', 'DarkSlateGray', 'DarkRed', 'DarkOrange', 'Olive', 'Green', 'Teal', 'Blue', 'SlateGray', 'DimGray', 'Red', 'SandyBrown', 'YellowGreen','SeaGreen', 'MediumTurquoise','RoyalBlue', 'Purple', 'Gray', 'Magenta', 'Orange', 'Yellow', 'Lime', 'Cyan', 'DeepSkyBlue', 'DarkOrchid', 'Silver', 'Pink', 'Wheat', 'LemonChiffon', 'PaleGreen', 'PaleTurquoise', 'LightBlue', 'Plum', 'White')}-->\r\n");
	templateBuilder.Append("		<div class=\"popupmenu_popup tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor_menu\" style=\"display: none\">\r\n");

	int colorname__loop__id=0;
	foreach(string colorname in coloroptions.Split(comma))
	{
		colorname__loop__id++;

	templateBuilder.Append("				<input type=\"button\" style=\"background-color: ");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[color=");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("]', '[/color]')\" />\r\n");

	if (colorname__loop__id%8==0)
	{

	templateBuilder.Append("<br />\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('img', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"图片\" class=\"tb_img\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("img\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'img')\">Image</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('link', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"链接\" class=\"tb_link\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("url\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'url')\">Link</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('quote', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"引用\" class=\"tb_quote\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[quote]', '[/quote]')\">Quote</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('code', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"代码\" class=\"tb_code\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[code]', '[/code]')\">Code</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('smilies', $seditor[1])}-->\r\n");

	if (config.Smileyinsert==1)
	{

	templateBuilder.Append("		<a href=\"javascript:;\" class=\"tb_smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies\" onclick=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback);showMenu({'ctrlid':this.id, 'evt':'click', 'layer':2})\">Smilies</a>\r\n");

	}	//end if

	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	if (config.Smileyinsert==1)
	{

	templateBuilder.Append("	<div class=\"smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies_menu\" style=\"display:none;width:315px;\">\r\n");
	templateBuilder.Append("		<div class=\"smilieslist\">\r\n");
	string defaulttypname = string.Empty;
	
	templateBuilder.Append("			<div id=\"smiliesdiv\">\r\n");
	templateBuilder.Append("				<div class=\"smiliesgroup\" style=\"margin-right: 0pt;\">\r\n");
	templateBuilder.Append("					<ul>\r\n");

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

	templateBuilder.Append("						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" class=\"current\">" + stype["code"].ToString().Trim() + "</a></li>\r\n");

	}
	else
	{

	templateBuilder.Append("						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">" + stype["code"].ToString().Trim() + "</a></li>\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("					</ul>\r\n");
	templateBuilder.Append("				 </div>\r\n");
	templateBuilder.Append("				 <div style=\"clear: both;\" class=\"float_typeid\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie\"></div>\r\n");
	templateBuilder.Append("				 <table class=\"smilieslist_table\" id=\"s_preview_table\" style=\"display: none\"><tr><td class=\"smilieslist_preview\" id=\"s_preview\"></td></tr></table>\r\n");
	templateBuilder.Append("				 <div id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie_pagenum\" class=\"smilieslist_page\">&nbsp;</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("			//var firstpagesmilies_json ={ {  Caches.GetSmiliesFirstPageCache()  } };\r\n");
	templateBuilder.Append("			//showFirstPageSmilies(firstpagesmilies_json, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("', 16, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(func){\r\n");
	templateBuilder.Append("				if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML !='' && $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML != '正在加载表情...')\r\n");
	templateBuilder.Append("					return;\r\n");
	templateBuilder.Append("				var c = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=smilies\";\r\n");
	templateBuilder.Append("				_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true);\r\n");
	templateBuilder.Append("				setTimeout(\"if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML=='')$('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML = '正在加载表情...'\", 2000);\r\n");
	templateBuilder.Append("			}\r\n");
	templateBuilder.Append("			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback(obj) {\r\n");
	templateBuilder.Append("				smilies_HASH = obj; \r\n");
	templateBuilder.Append("				showsmiles(1, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("			}\r\n");
	templateBuilder.Append("		</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	</div>\r\n");

	}	//end if



	templateBuilder.Append("			<div class=\"postarea cl\">\r\n");
	templateBuilder.Append("				<div class=\"postinner\">\r\n");
	templateBuilder.Append("				<textarea \r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("rows=\"5\"\r\n");

	}
	else
	{

	templateBuilder.Append("rows=\"7\"\r\n");

	}	//end if

	templateBuilder.Append(" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("message\" onKeyDown=\"seditor_ctlent(event, 'fastvalidate($(\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\\'),\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("\\')','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"3\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"12\"\r\n");

	}	//end if

	templateBuilder.Append("  style=\"background-image:url(" + quickbgad[1].ToString().Trim() + ");background-repeat:no-repeat;background-position:50% 50%;\" \r\n");

	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"\r\n");

	}	//end if

	templateBuilder.Append("></textarea>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");

	if (isseccode)
	{

	templateBuilder.Append("		<div class=\"pbt\" style=\"position: relative;\">\r\n");

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


	templateBuilder.Append("		</div>\r\n");

	}	//end if

	templateBuilder.Append("		<div class=\"pbt\">\r\n");
	templateBuilder.Append("			<button type=\"submit\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"topicsubmit\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"4\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"13\"\r\n");

	}	//end if

	templateBuilder.Append(" class=\"pn\"><span>发表帖子</span></button> <span class=\"grayfont\">[Ctrl+Enter快速发布]</span>\r\n");
	templateBuilder.Append("			<a href=\"###\" id=\"adlinkbtn\" style=\"display:none;\" onclick=\"window.open('" + quickbgad[0].ToString().Trim() + "','_blank');\">查看背景广告</a>\r\n");
	templateBuilder.Append("			<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('quickpostmessage').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">隐藏</a>\r\n");
	templateBuilder.Append("			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"");
	templateBuilder.Append(forum.Postbytopictype.ToString().Trim());
	templateBuilder.Append("\" tabindex=\"3\" />\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</form>\r\n");



	}	//end if


	if (config.Whosonlinestatus!=0 && config.Whosonlinestatus!=1)
	{

	templateBuilder.Append("	<div class=\"bm cl\" id=\"online\">\r\n");
	templateBuilder.Append("		<div class=\"bm_h\">		\r\n");
	templateBuilder.Append("			<span class=\"l_action\" style=\"display:none\">\r\n");

	if (DNTRequest.GetString("showonline")=="no")
	{

	templateBuilder.Append("					<a href=\"showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&showonline=yes#online\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_no.gif\" alt=\"收起\" />\r\n");

	}
	else
	{

	templateBuilder.Append("					<a href=\"showforum.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("&showonline=no#online\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/collapsed_yes.gif\" alt=\"展开\" />\r\n");

	}	//end if

	templateBuilder.Append("</a>\r\n");
	templateBuilder.Append("			</span>\r\n");
	templateBuilder.Append("			<h3>\r\n");
	templateBuilder.Append("				<strong>在线用户</strong> - <em id=\"forumtotalonline\">");
	templateBuilder.Append(forumtotalonline.ToString());
	templateBuilder.Append("</em> 人在线<span id=\"invisible\"></span>\r\n");
	templateBuilder.Append("			</h3>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<dl id=\"onlinelist\">\r\n");
	templateBuilder.Append("			<dt style=\"display:none\">");
	templateBuilder.Append(onlineiconlist.ToString());
	templateBuilder.Append("</dt>\r\n");

	if (showforumonline)
	{

	templateBuilder.Append("			<dd>\r\n");
	templateBuilder.Append("			<ul class=\"userlist cl\">\r\n");
	int invisiblecount = 0;
	

	if (forumtotalonline!=0)
	{


	int onlineuser__loop__id=0;
	foreach(OnlineUserInfo onlineuser in onlineuserlist)
	{
		onlineuser__loop__id++;


	if (onlineuser.Invisible==1)
	{

	 invisiblecount = invisiblecount + 1;
	
	templateBuilder.Append("						<li style=\"overflow:hidden;text-align:center;height:70px;width:80px;line-height:60px\">(隐身会员)</li>\r\n");

	}
	else
	{

	templateBuilder.Append("						<li style=\"overflow:hidden;text-align:center;height:70px;width:80px\">\r\n");
	string avatarurl = Avatars.GetAvatarUrl(onlineuser.Userid, AvatarSize.Small);
	
	templateBuilder.Append("								<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" id=\"memberinfo_" + onlineuser__loop__id.ToString() + "\" style=\"border:1px solid #E8E8E8;padding:1px;\" />\r\n");

	if (onlineuser.Userid==-1)
	{

	templateBuilder.Append("								<p>");
	templateBuilder.Append(onlineuser.Username.ToString().Trim());
	templateBuilder.Append("</p>\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(onlineuser.Userid);
	
	templateBuilder.Append("								<p><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(onlineuser.Username.ToString().Trim());
	templateBuilder.Append("</a></p>\r\n");

	}	//end if

	templateBuilder.Append("						</li>\r\n");

	}	//end if


	}	//end loop


	if (invisiblecount>0)
	{

	templateBuilder.Append("					<script type=\"text/javascript\">$('invisible').innerHTML = '(");
	templateBuilder.Append(invisiblecount.ToString());
	templateBuilder.Append("' + \" 隐身)\";</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}
	else
	{

	templateBuilder.Append("                  <script type=\"text/javascript\">$('forumtotalonline').innerHTML = parseInt($('forumtotalonline').innerHTML)+1;</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("					<li style=\"overflow:hidden;text-align:center;height:70px;width:80px\">\r\n");
	string avatarurl = Avatars.GetAvatarUrl(userid, AvatarSize.Small);
	
	templateBuilder.Append("							<img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/noavatar_small.gif';\"  alt=\"头像\" \" style=\"border:1px solid #E8E8E8;padding:1px;\" />\r\n");

	if (userid==-1)
	{

	templateBuilder.Append("							<p>");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</p>\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(userid);
	
	templateBuilder.Append("							<p><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</a></p>\r\n");

	}	//end if

	templateBuilder.Append("					</li>\r\n");

	}	//end if

	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("			</dd>\r\n");

	}	//end if

	templateBuilder.Append("		</dl>\r\n");
	templateBuilder.Append("	</div>\r\n");

	}	//end if

	templateBuilder.Append("</div>\r\n");

	if (userid<0||canposttopic)
	{

	templateBuilder.Append("	<ul class=\"popupmenu_popup newspecialmenu\" id=\"newspecial_menu\" style=\"display: none\">\r\n");

	if (forum.Allowspecialonly<=0)
	{

	templateBuilder.Append("	<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" >发新主题</a></li>\r\n");

	}	//end if

	 specialpost = forum.Allowpostspecial&1;
	

	if (specialpost==1 && usergroupinfo.Allowpostpoll==1)
	{

	templateBuilder.Append("		<li class=\"poll\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&type=poll&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\">发布投票</a></li>\r\n");

	}	//end if

	 specialpost = forum.Allowpostspecial&4;
	

	if (specialpost==4 && usergroupinfo.Allowbonus==1)
	{

	templateBuilder.Append("		<li class=\"reward\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&type=bonus&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\">发布悬赏</a></li>\r\n");

	}	//end if

	 specialpost = forum.Allowpostspecial&16;
	

	if (specialpost==16 && usergroupinfo.Allowdebate==1)
	{

	templateBuilder.Append("		<li class=\"debate\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&type=debate&forumpage=");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("\" >发起辩论</a></li>\r\n");

	}	//end if

	templateBuilder.Append("	</ul>\r\n");
	templateBuilder.Append("	<ul class=\"popupmenu_popup newspecialmenu\" id=\"newspecial2_menu\" style=\"display: none\">\r\n");
	templateBuilder.Append("	</ul>\r\n");
	templateBuilder.Append("    <ul class=\"popupmenu_popup newspecialmenu\" id=\"seditor_newspecial_menu\" style=\"display: none\">\r\n");
	templateBuilder.Append("	</ul>\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("	    $('newspecial2_menu').innerHTML = $('newspecial_menu').innerHTML;\r\n");
	templateBuilder.Append("	    $('seditor_newspecial_menu').innerHTML = $('newspecial_menu').innerHTML;\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("		var maxpage = parseInt('");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("		var pageid = parseInt('");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("		if(maxpage > 1) {\r\n");
	templateBuilder.Append("			document.onkeyup = function(e){\r\n");
	templateBuilder.Append("				e = e ? e : window.event;\r\n");
	templateBuilder.Append("				var tagname = is_ie ? e.srcElement.tagName : e.target.tagName;\r\n");
	templateBuilder.Append("				if(tagname == 'INPUT' || tagname == 'TEXTAREA') return;\r\n");
	templateBuilder.Append("				actualCode = e.keyCode ? e.keyCode : e.charCode;\r\n");
	templateBuilder.Append("				if(pageid < maxpage && actualCode == 39) {\r\n");
	templateBuilder.Append("					window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(Urls.ShowForumAspxRewrite(forumid,pageid+1,forum.Rewritename).ToString().Trim());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("				}\r\n");
	templateBuilder.Append("				if(pageid > 1 && actualCode == 37) {\r\n");
	templateBuilder.Append("					window.location = '");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(Urls.ShowForumAspxRewrite(forumid,pageid-1,forum.Rewritename).ToString().Trim());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("				}\r\n");
	templateBuilder.Append("			}\r\n");
	templateBuilder.Append("		}\r\n");
	templateBuilder.Append("	</");
	templateBuilder.Append("script>\r\n");

	}	//end if

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


	if (config.Forumjump==1)
	{

	templateBuilder.Append("	");
	templateBuilder.Append(navhomemenu.ToString());
	templateBuilder.Append("\r\n");

	}	//end if


	if (showvisitedforumsmenu)
	{

	templateBuilder.Append("<div class=\"p_pop\" id=\"visitedforums_menu\" style=\"display: none\">\r\n");
	templateBuilder.Append("	<h3 class=\"xi1\">浏览过的版块</h3>\r\n");
	templateBuilder.Append("	<ul>\r\n");

	int simpforuminfo__loop__id=0;
	foreach(SimpleForumInfo simpforuminfo in visitedforums)
	{
		simpforuminfo__loop__id++;


	if (simpforuminfo.Fid!=forumid)
	{

	templateBuilder.Append("		<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append(simpforuminfo.Url.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(simpforuminfo.Name.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("	</ul>\r\n");
	templateBuilder.Append("</div>\r\n");

	}	//end if



	if (infloat!=1)
	{


	if (floatad!="")
	{

	templateBuilder.Append("	<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_floatadv.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	");
	templateBuilder.Append(floatad.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\">theFloaters.play();</");
	templateBuilder.Append("script>\r\n");

	}
	else if (doublead!="")
	{

	templateBuilder.Append("	<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_floatadv.js\"></");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	");
	templateBuilder.Append(doublead.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("	<script type=\"text/javascript\">theFloaters.play();</");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if
	templateBuilder.Append(mediaad.ToString());
	templateBuilder.Append("\r\n");

	}
	else
	{


	templateBuilder.Append("<script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/post.js\"></");
	templateBuilder.Append("script>\r\n");
	string seditorid = "infloatquickposttopic";
	

	if (infloat!=1)
	{

	 seditorid = "quickposttopic";
	

	}	//end if

	templateBuilder.Append("<form method=\"post\" name=\"postform\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\" action=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forumid.ToString());
	templateBuilder.Append("\" enctype=\"multipart/form-data\" onsubmit=\"return fastvalidate(this,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">\r\n");
	templateBuilder.Append("<div id=\"quickpost\" class=\"quickpost cl \r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("main\r\n");

	}	//end if

	templateBuilder.Append("\">\r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("	<h4 class=\"bm_h\">\r\n");

	}
	else
	{

	templateBuilder.Append("	<h4 class=\"flb\">\r\n");

	}	//end if


	if (infloat==1)
	{

	templateBuilder.Append("	<span class=\"y\">\r\n");
	templateBuilder.Append("		<a title=\"关闭\" onclick=\"hideWindow('newthread')\" class=\"flbc\" href=\"javascript:;\">关闭</a>\r\n");
	templateBuilder.Append("	</span>\r\n");

	}	//end if

	templateBuilder.Append("	<em>快速发帖</em>\r\n");

	if (infloat==1 && needaudit)
	{

	templateBuilder.Append("    <span class=\"needverify\">需审核</span>\r\n");

	}	//end if

	templateBuilder.Append("	</h4>\r\n");
	templateBuilder.Append("	<div class=\"bm_inner c cl\">\r\n");

	if (infloat!=1)
	{


	if (quickeditorad!="")
	{

	templateBuilder.Append("		<div class=\"leaderboard\">");
	templateBuilder.Append(quickeditorad.ToString());
	templateBuilder.Append("</div>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("		<div class=\"pbt cl\">\r\n");

	if (forum.Applytopictype==1 && topictypeselectoptions!="")
	{

	templateBuilder.Append("			<div class=\"ftid\">\r\n");
	templateBuilder.Append("				<select name=\"typeid\" id=\"typeid\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"1\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"10\"\r\n");

	}	//end if

	templateBuilder.Append(">");
	templateBuilder.Append(topictypeselectoptions.ToString());
	templateBuilder.Append("</select>\r\n");
	templateBuilder.Append("				<script type=\"text/javascript\">simulateSelect('typeid');</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			</div>\r\n");

	}	//end if

	templateBuilder.Append("			<input type=\"text\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("title\" name=\"");
	templateBuilder.Append(config.Antispamposttitle.ToString().Trim());
	templateBuilder.Append("\" size=\"60\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"2\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"11\"\r\n");

	}	//end if

	templateBuilder.Append(" value=\"\" class=\"txt postpx\"/>\r\n");
	templateBuilder.Append("			<em id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("validatemessage\"></em>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div class=\"pbt cl\">\r\n");
	templateBuilder.Append("			<span style=\"display:none\">\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"htmlon\" id=\"htmlon\" \r\n");

	if (usergroupinfo.Allowhtml!=1)
	{

	templateBuilder.Append(" disabled\r\n");

	}	//end if

	templateBuilder.Append("/>\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"parseurloff\" id=\"parseurloff\" \r\n");

	if (parseurloff==1)
	{

	templateBuilder.Append("checked \r\n");

	}	//end if

	templateBuilder.Append(" />\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"smileyoff\" id=\"smileyoff\" \r\n");

	if (smileyoff==1)
	{

	templateBuilder.Append(" checked disabled \r\n");

	}	//end if

	templateBuilder.Append(" />\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"bbcodeoff\" id=\"bbcodeoff\" \r\n");

	if (bbcodeoff==1)
	{

	templateBuilder.Append(" checked disabled \r\n");

	}	//end if

	templateBuilder.Append(" />\r\n");
	templateBuilder.Append("			<input type=\"checkbox\" value=\"1\" name=\"usesig\" id=\"usesig\"\r\n");

	if (usesig==1)
	{

	templateBuilder.Append("checked\r\n");

	}	//end if

	templateBuilder.Append("/>\r\n");
	templateBuilder.Append("			</span>\r\n");
	templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
	templateBuilder.Append("				var bbinsert = parseInt('1');\r\n");
	templateBuilder.Append("				var smiliesCount = 24;\r\n");
	templateBuilder.Append("				var colCount = 8;\r\n");
	templateBuilder.Append("			</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("			<div \r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("style=\"margin-right:170px;\" \r\n");

	}
	else
	{

	templateBuilder.Append("style=\"width:600px;\"\r\n");

	}	//end if

	templateBuilder.Append(">\r\n");
	char comma = ',';
	

	templateBuilder.Append("<link href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/");
	templateBuilder.Append(templatepath.ToString());
	templateBuilder.Append("/seditor.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
	templateBuilder.Append("<div class=\"editor_tb\">\r\n");
	templateBuilder.Append("	<span class=\"y\">\r\n");

	if (topicid>0)
	{

	templateBuilder.Append("		<a onclick=\"switchAdvanceMode(this.href,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form');doane(event);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("postreply.aspx?topicid=");
	templateBuilder.Append(topicid.ToString());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\">高级模式</a>\r\n");

	}
	else
	{

	templateBuilder.Append("		<a onclick=\"switchAdvanceMode(this.href,'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form');doane(event);\" href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\">高级模式</a>\r\n");

	}	//end if


	if (infloat!=1)
	{


	if (userid<0||canposttopic)
	{

	templateBuilder.Append("		<span style=\"display:none;\" onmouseover=\"if($('newspecial_menu').childNodes.length>0)  showMenu(this.id);\" id=\"seditor_newspecial\"><a href=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("posttopic.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("&forumpage=");
	templateBuilder.Append(forumpageid.ToString());
	templateBuilder.Append("\" onclick=\"showWindow('newthread',  '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("showforum.aspx?forumid=");
	templateBuilder.Append(forum.Fid.ToString().Trim());
	templateBuilder.Append("');doane(event);\">发新主题</a></span>\r\n");
	templateBuilder.Append("        <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("            setTimeout('if ($(\\'seditor_newspecial_menu\\') != null) $(\\'seditor_newspecial\\').style.display = \\'\\';', 800);\r\n");
	templateBuilder.Append("        </");
	templateBuilder.Append("script>\r\n");

	}	//end if


	}	//end if

	templateBuilder.Append("	</span>\r\n");
	templateBuilder.Append("	<div>\r\n");
	templateBuilder.Append("	<!--{if in_array('bold', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"粗体\" class=\"tb_bold\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[b]', '[/b]')\">B</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('color', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"颜色\" class=\"tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor\" onclick=\"showMenu(this.id, true, 0, 2)\">Color</a>\r\n");
	string coloroptions = "Black,Sienna,DarkOliveGreen,DarkGreen,DarkSlateBlue,Navy,Indigo,DarkSlateGray,DarkRed,DarkOrange,Olive,Green,Teal,Blue,SlateGray,DimGray,Red,SandyBrown,YellowGreen,SeaGreen,MediumTurquoise,RoyalBlue,Purple,Gray,Magenta,Orange,Yellow,Lime,Cyan,DeepSkyBlue,DarkOrchid,Silver,Pink,Wheat,LemonChiffon,PaleGreen,PaleTurquoise,LightBlue,Plum,White";
	
	templateBuilder.Append("		<!--{eval $coloroptions = array('Black', 'Sienna', 'DarkOliveGreen', 'DarkGreen', 'DarkSlateBlue', 'Navy', 'Indigo', 'DarkSlateGray', 'DarkRed', 'DarkOrange', 'Olive', 'Green', 'Teal', 'Blue', 'SlateGray', 'DimGray', 'Red', 'SandyBrown', 'YellowGreen','SeaGreen', 'MediumTurquoise','RoyalBlue', 'Purple', 'Gray', 'Magenta', 'Orange', 'Yellow', 'Lime', 'Cyan', 'DeepSkyBlue', 'DarkOrchid', 'Silver', 'Pink', 'Wheat', 'LemonChiffon', 'PaleGreen', 'PaleTurquoise', 'LightBlue', 'Plum', 'White')}-->\r\n");
	templateBuilder.Append("		<div class=\"popupmenu_popup tb_color\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("forecolor_menu\" style=\"display: none\">\r\n");

	int colorname__loop__id=0;
	foreach(string colorname in coloroptions.Split(comma))
	{
		colorname__loop__id++;

	templateBuilder.Append("				<input type=\"button\" style=\"background-color: ");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[color=");
	templateBuilder.Append(colorname.ToString());
	templateBuilder.Append("]', '[/color]')\" />\r\n");

	if (colorname__loop__id%8==0)
	{

	templateBuilder.Append("<br />\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('img', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"图片\" class=\"tb_img\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("img\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'img')\">Image</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('link', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"链接\" class=\"tb_link\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("url\" onclick=\"seditor_menu('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', 'url')\">Link</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('quote', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"引用\" class=\"tb_quote\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[quote]', '[/quote]')\">Quote</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('code', $seditor[1])}-->\r\n");
	templateBuilder.Append("		<a href=\"javascript:;\" title=\"代码\" class=\"tb_code\" onclick=\"seditor_insertunit('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("', '[code]', '[/code]')\">Code</a>\r\n");
	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	<!--{if in_array('smilies', $seditor[1])}-->\r\n");

	if (config.Smileyinsert==1)
	{

	templateBuilder.Append("		<a href=\"javascript:;\" class=\"tb_smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies\" onclick=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback);showMenu({'ctrlid':this.id, 'evt':'click', 'layer':2})\">Smilies</a>\r\n");

	}	//end if

	templateBuilder.Append("	<!--{/if}-->\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");

	if (config.Smileyinsert==1)
	{

	templateBuilder.Append("	<div class=\"smilies\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("smilies_menu\" style=\"display:none;width:315px;\">\r\n");
	templateBuilder.Append("		<div class=\"smilieslist\">\r\n");
	string defaulttypname = string.Empty;
	
	templateBuilder.Append("			<div id=\"smiliesdiv\">\r\n");
	templateBuilder.Append("				<div class=\"smiliesgroup\" style=\"margin-right: 0pt;\">\r\n");
	templateBuilder.Append("					<ul>\r\n");

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

	templateBuilder.Append("						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" class=\"current\">" + stype["code"].ToString().Trim() + "</a></li>\r\n");

	}
	else
	{

	templateBuilder.Append("						<li id=\"t_s_" + stype__loop__id.ToString() + "\"><a id=\"s_" + stype__loop__id.ToString() + "\" hidefocus=\"true\" href=\"javascript:;\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\">" + stype["code"].ToString().Trim() + "</a></li>\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("					</ul>\r\n");
	templateBuilder.Append("				 </div>\r\n");
	templateBuilder.Append("				 <div style=\"clear: both;\" class=\"float_typeid\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie\"></div>\r\n");
	templateBuilder.Append("				 <table class=\"smilieslist_table\" id=\"s_preview_table\" style=\"display: none\"><tr><td class=\"smilieslist_preview\" id=\"s_preview\"></td></tr></table>\r\n");
	templateBuilder.Append("				 <div id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie_pagenum\" class=\"smilieslist_page\">&nbsp;</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<script type=\"text/javascript\" reload=\"1\">\r\n");
	templateBuilder.Append("			//var firstpagesmilies_json ={ {  Caches.GetSmiliesFirstPageCache()  } };\r\n");
	templateBuilder.Append("			//showFirstPageSmilies(firstpagesmilies_json, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("', 16, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies(func){\r\n");
	templateBuilder.Append("				if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML !='' && $('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML != '正在加载表情...')\r\n");
	templateBuilder.Append("					return;\r\n");
	templateBuilder.Append("				var c = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=smilies\";\r\n");
	templateBuilder.Append("				_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true);\r\n");
	templateBuilder.Append("				setTimeout(\"if($('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML=='')$('");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("showsmilie').innerHTML = '正在加载表情...'\", 2000);\r\n");
	templateBuilder.Append("			}\r\n");
	templateBuilder.Append("			function ");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("getSmilies_callback(obj) {\r\n");
	templateBuilder.Append("				smilies_HASH = obj; \r\n");
	templateBuilder.Append("				showsmiles(1, '");
	templateBuilder.Append(defaulttypname.ToString());
	templateBuilder.Append("', 1, '");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\r\n");
	templateBuilder.Append("			}\r\n");
	templateBuilder.Append("		</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("	</div>\r\n");

	}	//end if



	templateBuilder.Append("			<div class=\"postarea cl\">\r\n");
	templateBuilder.Append("				<div class=\"postinner\">\r\n");
	templateBuilder.Append("				<textarea \r\n");

	if (infloat!=1)
	{

	templateBuilder.Append("rows=\"5\"\r\n");

	}
	else
	{

	templateBuilder.Append("rows=\"7\"\r\n");

	}	//end if

	templateBuilder.Append(" cols=\"80\" name=\"");
	templateBuilder.Append(config.Antispampostmessage.ToString().Trim());
	templateBuilder.Append("\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("message\" onKeyDown=\"seditor_ctlent(event, 'fastvalidate($(\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("form\\'),\\'");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("\\')','");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("');\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"3\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"12\"\r\n");

	}	//end if

	templateBuilder.Append("  style=\"background-image:url(" + quickbgad[1].ToString().Trim() + ");background-repeat:no-repeat;background-position:50% 50%;\" \r\n");

	if (quickbgad[0].ToString().Trim()!="")
	{

	templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"\r\n");

	}	//end if

	templateBuilder.Append("></textarea>\r\n");
	templateBuilder.Append("				</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("			</div>\r\n");
	templateBuilder.Append("		</div>\r\n");

	if (isseccode)
	{

	templateBuilder.Append("		<div class=\"pbt\" style=\"position: relative;\">\r\n");

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


	templateBuilder.Append("		</div>\r\n");

	}	//end if

	templateBuilder.Append("		<div class=\"pbt\">\r\n");
	templateBuilder.Append("			<button type=\"submit\" id=\"");
	templateBuilder.Append(seditorid.ToString());
	templateBuilder.Append("submit\" name=\"topicsubmit\" \r\n");

	if (infloat==1)
	{

	templateBuilder.Append("tabindex=\"4\"\r\n");

	}
	else
	{

	templateBuilder.Append("tabindex=\"13\"\r\n");

	}	//end if

	templateBuilder.Append(" class=\"pn\"><span>发表帖子</span></button> <span class=\"grayfont\">[Ctrl+Enter快速发布]</span>\r\n");
	templateBuilder.Append("			<a href=\"###\" id=\"adlinkbtn\" style=\"display:none;\" onclick=\"window.open('" + quickbgad[0].ToString().Trim() + "','_blank');\">查看背景广告</a>\r\n");
	templateBuilder.Append("			<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('quickpostmessage').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">隐藏</a>\r\n");
	templateBuilder.Append("			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"");
	templateBuilder.Append(forum.Postbytopictype.ToString().Trim());
	templateBuilder.Append("\" tabindex=\"3\" />\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</form>\r\n");



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
