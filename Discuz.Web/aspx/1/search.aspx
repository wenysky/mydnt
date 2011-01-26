<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.search" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2010/12/28 17:12:57.
		本页面代码由Discuz!NT模板引擎生成于 2010/12/28 17:12:57. 
	*/

	base.OnInit(e);

	templateBuilder.Capacity = 220000;
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
	templateBuilder.Append("<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/dnt.css\" type=\"text/css\" media=\"all\" />\r\n");
	templateBuilder.Append("<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/search.css\" type=\"text/css\" media=\"all\" />\r\n");
	templateBuilder.Append("<link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/float.css\" type=\"text/css\" media=\"all\" />\r\n");
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
	templateBuilder.Append("<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_modcp.js\"></");
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
	templateBuilder.Append("</");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append(script.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("</head>\r\n");
	templateBuilder.Append("<body onkeydown=\"if(event.keyCode==27) return false;\">\r\n");
	templateBuilder.Append("<div id=\"append_parent\"></div><div id=\"ajaxwaitid\"></div>\r\n");
	templateBuilder.Append("<div class=\"subhead cl\">\r\n");
	templateBuilder.Append("	<a class=\"xg1 z\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\">返回首页</a>\r\n");

	if (userid!=-1)
	{

	templateBuilder.Append("	<div class=\"sch xg2 y\">\r\n");
	templateBuilder.Append("		<strong><a class=\"vwmy\" href=\"#\">");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</a></strong><span class=\"xg1\">在线</span><span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("		<a title=\"");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("条新短消息\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\" id=\"pm_ntc\">短消息</a><span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("		<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnotice.aspx?filter=all\">通知</a><span class=\"pipe\">|</span>\r\n");
	templateBuilder.Append("		<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户中心</a>\r\n");

	if (useradminid==1)
	{

	templateBuilder.Append("		    <span class=\"pipe\">|</span><a target=\"_blank\" href=\"admin/index.aspx\">系统设置</a>\r\n");

	}	//end if

	templateBuilder.Append("		<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("logout.aspx?userkey=");
	templateBuilder.Append(userkey.ToString());
	templateBuilder.Append("\">退出</a>\r\n");
	templateBuilder.Append("	</div>\r\n");

	}	//end if

	templateBuilder.Append("</div>\r\n");

	if (page_err>0)
	{

	templateBuilder.Append("    <div class=\"wrap cl\">\r\n");
	templateBuilder.Append("        <form action=\"search.aspx\" autocomplete=\"off\" name=\"postpm\" method=\"post\" class=\"searchform\">\r\n");
	templateBuilder.Append("            <table cellspacing=\"0\" cellpadding=\"0\" class=\"mbm\" id=\"tpsch\">\r\n");
	templateBuilder.Append("            <tbody>\r\n");
	templateBuilder.Append("                <tr>\r\n");
	templateBuilder.Append("                <td><h1><a title=\"Discuz!NT|BBS|论坛\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\"><img alt=\"Discuz!NT|BBS|论坛\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/default/images/logo.png\"></a></h1></td>\r\n");
	templateBuilder.Append("                <td>\r\n");
	templateBuilder.Append("                    <ul class=\"tb cl\">\r\n");
	templateBuilder.Append("                        <li id=\"forumli\"><a href=\"search.aspx?keyword=");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("&searchsubmit=1\">论坛</a></li>\r\n");

	if (config.Enablespace==1)
	{

	templateBuilder.Append("                        <li id=\"spaceli\"><a id=\"spacelink\" href=\"search.aspx?type=spacepost&keyword=");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("&searchsubmit=1\">日志</a></li>\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("                        <li id=\"albumli\"><a id=\"albumlink\" href=\"search.aspx?type=album&keyword=");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("&searchsubmit=1\">相册</a></li>\r\n");

	}	//end if

	templateBuilder.Append("                    </ul>\r\n");
	templateBuilder.Append("                    <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                        var type = '");
	templateBuilder.Append(type.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("                        switch (type) {\r\n");
	templateBuilder.Append("                            case 'spacepost': $('spaceli').className = 'a'; break;\r\n");
	templateBuilder.Append("                            case 'album': $('albumli').className = 'a'; break;\r\n");
	templateBuilder.Append("                            default: $('forumli').className = 'a'; break;\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                    </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("                    <input type=\"hidden\" value=\"\" name=\"type\" />\r\n");
	templateBuilder.Append("                    <table cellspacing=\"0\" cellpadding=\"0\" id=\"tps_form\">\r\n");
	templateBuilder.Append("                        <tbody>\r\n");
	templateBuilder.Append("                        <tr>\r\n");
	templateBuilder.Append("                            <td>\r\n");
	templateBuilder.Append("                                <input type=\"text\" tabindex=\"1\" value=\"");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("\" maxlength=\"40\" size=\"45\" class=\"schtxt\" name=\"keyword\" id=\"srchtxt\">\r\n");
	templateBuilder.Append("                            </td>\r\n");
	templateBuilder.Append("                            <td>\r\n");
	templateBuilder.Append("                                <button class=\"schbtn\" id=\"tps_btn\" type=\"submit\"><strong>搜索</strong></button>\r\n");
	templateBuilder.Append("                            </td>\r\n");
	templateBuilder.Append("                            <td style=\"padding-left: 10px; background:#FFF;\">\r\n");
	templateBuilder.Append("                                <label><a href=\"search.aspx?advsearch=1\">高级</a>\r\n");
	templateBuilder.Append("                            </td>\r\n");
	templateBuilder.Append("                        </tr>\r\n");
	templateBuilder.Append("                        </tbody>\r\n");
	templateBuilder.Append("                    </table>\r\n");
	templateBuilder.Append("                </td>\r\n");
	templateBuilder.Append("                </tr>\r\n");
	templateBuilder.Append("            </tbody>\r\n");
	templateBuilder.Append("            </table>\r\n");
	templateBuilder.Append("        </form>\r\n");
	templateBuilder.Append("        <div class=\"tl\">\r\n");
	templateBuilder.Append("            <div class=\"sttl mbn\">\r\n");
	templateBuilder.Append("                <h2>结果: <em>\r\n");

	if (keyword!="")
	{

	templateBuilder.Append("                    对关键词 \"");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("\" \r\n");

	if (poster!="")
	{

	templateBuilder.Append("和\r\n");

	}
	else
	{

	templateBuilder.Append("的\r\n");

	}	//end if


	}	//end if


	if (poster!="")
	{

	templateBuilder.Append("                    对作者 \"");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("\" 的\r\n");

	}	//end if

	templateBuilder.Append("                查询,找到相关主题 ");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append(" 个</em></h2>\r\n");
	templateBuilder.Append("            </div>\r\n");
	templateBuilder.Append("        </div>\r\n");
	templateBuilder.Append("        <p class=\"emp xs2 xg2\">");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n");
	templateBuilder.Append("    </div>\r\n");

	}
	else
	{


	if (searchid==-1)
	{


	if (advsearch==0)
	{

	templateBuilder.Append("            <div class=\"search\">\r\n");
	templateBuilder.Append("                <form action=\"\" name=\"postpm\" autocomplete=\"off\" method=\"post\" class=\"searchform\">\r\n");
	templateBuilder.Append("                    <table cellspacing=\"0\" cellpadding=\"0\">\r\n");
	templateBuilder.Append("                    <tbody>\r\n");
	templateBuilder.Append("                    <tr>\r\n");
	templateBuilder.Append("                        <td class=\"s_logo\"><h1 class=\"mtw ptw\"><a title=\"Discuz!NT|BBS|论坛\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\"><img alt=\"Discuz!NT|BBS|论坛\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/default/images/logo.png\"></a></h1></td>\r\n");
	templateBuilder.Append("                        <td colspan=\"2\"></td>\r\n");
	templateBuilder.Append("                    </tr>\r\n");
	templateBuilder.Append("                    <tr>\r\n");
	templateBuilder.Append("                        <td width=\"400\" class=\"hm xs2\">\r\n");

	if (config.Enablespace==1||config.Enablealbum==1)
	{

	templateBuilder.Append("                            <a id=\"forumlink\" href=\"search.aspx\">论坛</a>\r\n");

	}	//end if


	if (config.Enablespace==1)
	{

	templateBuilder.Append("                            <span class=\"pipe\">|</span><a id=\"spacelink\" href=\"search.aspx?type=spacepost\">空间</a>\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("                            <span class=\"pipe\">|</span><a id=\"albumlink\" href=\"search.aspx?type=album\">相册</a>\r\n");

	}	//end if

	templateBuilder.Append("                        </td>\r\n");
	templateBuilder.Append("                        <td colspan=\"2\"></td>\r\n");
	templateBuilder.Append("                    </tr>\r\n");
	templateBuilder.Append("                    <tr id=\"tps_form\">\r\n");
	templateBuilder.Append("                        <td>\r\n");
	templateBuilder.Append("                            <input type=\"text\" tabindex=\"1\" class=\"schtxt\" value=\"\" maxlength=\"40\" size=\"65\" name=\"keyword\" id=\"srchtxt\">\r\n");
	templateBuilder.Append("                        </td>\r\n");
	templateBuilder.Append("                        <td>\r\n");
	templateBuilder.Append("                            <button class=\"schbtn\" value=\"true\" id=\"tps_btn\" type=\"submit\"><strong>搜索</strong></button>\r\n");
	templateBuilder.Append("                        </td>\r\n");
	templateBuilder.Append("                        <td style=\"padding-left:10px; width: 50px; background:#FFF; text-align: left;\">\r\n");
	templateBuilder.Append("                            <label><a href=\"search.aspx?advsearch=1\">高级</a>\r\n");
	templateBuilder.Append("                        </td>\r\n");
	templateBuilder.Append("                    </tr>\r\n");
	templateBuilder.Append("                    </tbody>\r\n");
	templateBuilder.Append("                    </table>\r\n");
	templateBuilder.Append("                </form>\r\n");

	if (config.Enablespace==1||config.Enablealbum==1)
	{

	templateBuilder.Append("                    <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                        var type = '");
	templateBuilder.Append(type.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("                        switch(type){\r\n");
	templateBuilder.Append("                            case 'spacepost': $('spacelink').style.fontWeight = 700; break;\r\n");
	templateBuilder.Append("                            case 'album': $('albumlink').style.fontWeight = 700; break;\r\n");
	templateBuilder.Append("                            default: $('forumlink').style.fontWeight = 700; break;\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                    </");
	templateBuilder.Append("script>\r\n");

	}	//end if

	templateBuilder.Append("            </div>\r\n");

	}
	else
	{

	templateBuilder.Append("            <div class=\"wrap cl\">\r\n");
	templateBuilder.Append("            <form id=\"postpm\" name=\"postpm\" method=\"post\" onsubmit=\"if(this.chkAuthor.checked) $('type').value='author';return true;\" action=\"\">\r\n");
	templateBuilder.Append("                <div id=\"options_item\">\r\n");
	templateBuilder.Append("                    <div id=\"postoptions\">\r\n");
	templateBuilder.Append("                        <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"tfm\">\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"posttableid\">选择分表</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"posttableid\" id=\"posttableid\">\r\n");

	int table__loop__id=0;
	foreach(DataRow table in tablelist.Rows)
	{
		table__loop__id++;

	templateBuilder.Append("			                                <option value=\"" + table["id"].ToString().Trim() + "\">" + table["description"].ToString().Trim() + "\r\n");

	if (Utils.StrToInt(table__loop__id, 0)==1)
	{

	templateBuilder.Append("(当前使用)\r\n");

	}	//end if

	templateBuilder.Append("</option>\r\n");

	}	//end loop

	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"searchtime\">时间</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"searchtime\" id=\"searchtime\">\r\n");
	templateBuilder.Append("		                                  <option value=\"0\" selected=\"selected\">全部时间</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-1\">1天</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-2\">2天</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-3\">3天</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-7\">1周</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-30\">1个月</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-90\">3个月</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-180\">半年</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-365\">1年</option>\r\n");
	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("		                                <input type=\"radio\" name=\"searchtimetype\"  value=\"1\" />以前\r\n");
	templateBuilder.Append("		                                <input type=\"radio\" name=\"searchtimetype\" value=\"0\" checked />以内\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"resultorder\">结果排序</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"resultorder\" id=\"resultorder\">\r\n");
	templateBuilder.Append("		                                  <option value=\"0\" selected=\"selected\">最后回复时间</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"1\">发表时间</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"2\">回复数量</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"3\">查看次数</option>\r\n");
	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("		                                <input type=\"radio\" name=\"resultordertype\" value=\"1\" />升序\r\n");
	templateBuilder.Append("		                                <input type=\"radio\" name=\"resultordertype\" value=\"0\" checked />降序\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"searchforumid\">搜索范围</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"searchforumid\" size=\"12\" style=\"width:450px\" multiple=\"multiple\" id=\"searchforumid\">\r\n");
	templateBuilder.Append("		                                 <option selected value=\"\">---------- 所有版块(默认) ----------</option>\r\n");
	templateBuilder.Append("			                                <!--模版中所有版块的下拉框中一定要加入value=\"\"否则会提示没有选择版块-->\r\n");
	templateBuilder.Append("			                                ");
	templateBuilder.Append(Caches.GetForumListBoxOptionsCache(true).ToString().Trim());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		                                 </select>\r\n");
	templateBuilder.Append("		                                 <p>(按Ctrl或Shift键可以多选,不选择)</p>\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                        </table>\r\n");
	templateBuilder.Append("                    </div>\r\n");
	templateBuilder.Append("                    <div id=\"spacepostoptions\">\r\n");
	templateBuilder.Append("                        <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"  class=\"tfm\">\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"searchtime\">时间</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"searchtime\" id=\"searchtime\">\r\n");
	templateBuilder.Append("		                                  <option value=\"0\" selected=\"selected\">全部时间</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-1\">1天</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-2\">2天</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-3\">3天</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-7\">1周</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-30\">1个月</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-90\">3个月</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-180\">半年</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-365\">1年</option>\r\n");
	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("		                                  <input type=\"radio\" name=\"searchtimetype\" value=\"1\" />\r\n");
	templateBuilder.Append("		                                以前\r\n");
	templateBuilder.Append("		                                <input name=\"searchtimetype\" type=\"radio\" value=\"0\"/>\r\n");
	templateBuilder.Append("		                                以内\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"resultorder\">结果排序</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"resultorder\" id=\"resultorder\">\r\n");
	templateBuilder.Append("                                          <option value=\"0\" selected=\"selected\">发表时间</option>\r\n");
	templateBuilder.Append("                                          <option value=\"1\">回复数量</option>\r\n");
	templateBuilder.Append("                                          <option value=\"2\">查看次数</option>\r\n");
	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("		                                <input type=\"radio\" name=\"resultordertype\" value=\"1\" />\r\n");
	templateBuilder.Append("		                                升序\r\n");
	templateBuilder.Append("		                                <input name=\"resultordertype\" type=\"radio\" value=\"0\"/>\r\n");
	templateBuilder.Append("		                                降序\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                        </table>\r\n");
	templateBuilder.Append("                    </div>\r\n");
	templateBuilder.Append("                    <div id=\"albumoptions\">\r\n");
	templateBuilder.Append("                        <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"  class=\"tfm\">\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"searchtime\">时间</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"searchtime\" id=\"searchtime\">\r\n");
	templateBuilder.Append("		                                  <option value=\"0\" selected=\"selected\">全部时间</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-1\">1天前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-2\">2天前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-3\">3天前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-7\">1周前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-30\">1个月前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-90\">3个月前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-180\">半年前</option>\r\n");
	templateBuilder.Append("		                                  <option value=\"-365\">1年前</option>\r\n");
	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("		                                  <input type=\"radio\" name=\"searchtimetype\" value=\"1\" />\r\n");
	templateBuilder.Append("		                                之前\r\n");
	templateBuilder.Append("		                                <input name=\"searchtimetype\" type=\"radio\" value=\"0\" />\r\n");
	templateBuilder.Append("		                                之后\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                            <tbody>\r\n");
	templateBuilder.Append("                                <tr>\r\n");
	templateBuilder.Append("	                                <th><label for=\"resultorder\">结果排序</label></th>\r\n");
	templateBuilder.Append("	                                <td>\r\n");
	templateBuilder.Append("		                                <select name=\"resultorder\" id=\"resultorder\">\r\n");
	templateBuilder.Append("		                                  <option value=\"0\" selected=\"selected\">创建时间</option>\r\n");
	templateBuilder.Append("		                                </select>\r\n");
	templateBuilder.Append("		                                <input type=\"radio\" name=\"resultordertype\" value=\"1\" />\r\n");
	templateBuilder.Append("		                                升序\r\n");
	templateBuilder.Append("		                                <input name=\"resultordertype\" type=\"radio\" value=\"0\" />\r\n");
	templateBuilder.Append("		                                降序\r\n");
	templateBuilder.Append("	                                </td>\r\n");
	templateBuilder.Append("                                </tr>\r\n");
	templateBuilder.Append("                            </tbody>\r\n");
	templateBuilder.Append("                        </table>\r\n");
	templateBuilder.Append("                    </div>\r\n");
	templateBuilder.Append("                </div>\r\n");
	templateBuilder.Append("                <table cellspacing=\"0\" cellpadding=\"0\" class=\"mbm\" id=\"tpsch\">\r\n");
	templateBuilder.Append("                    <tbody>\r\n");
	templateBuilder.Append("                        <tr>\r\n");
	templateBuilder.Append("                            <td><h1><a title=\"Discuz!NT|BBS|论坛\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\"><img alt=\"Discuz!NT|BBS|论坛\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/default/images/logo.png\"></a></h1></td>\r\n");
	templateBuilder.Append("                            <td>\r\n");
	templateBuilder.Append("                                <ul class=\"tb cl\">\r\n");
	templateBuilder.Append("                                    <li id=\"forumli\" class=\"a\"><a href=\"search.aspx\">论坛</a></li>\r\n");

	if (config.Enablespace==1)
	{

	templateBuilder.Append("                                    <li id=\"spaceli\"><a href=\"search.aspx?type=spacepost\">日志</a></li>\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("                                    <li id=\"albumli\"><a href=\"search.aspx?type=album\">相册</a></li>\r\n");

	}	//end if

	templateBuilder.Append("                                </ul>\r\n");
	templateBuilder.Append("<!--                                <input type=\"hidden\" value=\"");
	templateBuilder.Append(type.ToString());
	templateBuilder.Append("\" name=\"type\" />\r\n");
	templateBuilder.Append("                                <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                                    var type = '");
	templateBuilder.Append(type.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("                                    switch (type) {\r\n");
	templateBuilder.Append("                                        case 'spacepost': $('spaceli').className = 'a'; break;\r\n");
	templateBuilder.Append("                                        case 'album': $('albumli').className = 'a'; break;\r\n");
	templateBuilder.Append("                                        default: $('forumli').className = 'a'; break;\r\n");
	templateBuilder.Append("                                    }\r\n");
	templateBuilder.Append("                                </");
	templateBuilder.Append("script>-->\r\n");
	templateBuilder.Append("                                <table cellspacing=\"0\" cellpadding=\"0\" id=\"tps_form\">\r\n");
	templateBuilder.Append("	                                <tbody>\r\n");
	templateBuilder.Append("	                                <tr>\r\n");
	templateBuilder.Append("		                                <td>\r\n");
	templateBuilder.Append("			                                <input type=\"text\" tabindex=\"1\" value=\"");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("\" maxlength=\"40\" size=\"45\" class=\"schtxt\" name=\"keyword\" id=\"srchtxt\">\r\n");
	templateBuilder.Append("		                                </td>\r\n");
	templateBuilder.Append("		                                <td>\r\n");
	templateBuilder.Append("			                                <button id=\"tps_btn\" class=\"schbtn\" type=\"submit\"><strong>搜索</strong></button>\r\n");
	templateBuilder.Append("		                                </td>\r\n");
	templateBuilder.Append("		                                <td style=\"padding-left: 10px; background:#FFF;\">\r\n");
	templateBuilder.Append("			                                <label><a href=\"search.aspx\">返回普通搜索</a></label>\r\n");
	templateBuilder.Append("		                                </td>\r\n");
	templateBuilder.Append("	                                </tr>\r\n");
	templateBuilder.Append("	                                </tbody>\r\n");
	templateBuilder.Append("                                </table>\r\n");
	templateBuilder.Append("                            </td>\r\n");
	templateBuilder.Append("                        </tr>\r\n");
	templateBuilder.Append("                    </tbody>\r\n");
	templateBuilder.Append("                </table>\r\n");
	templateBuilder.Append("                <table cellSpacing=\"0\" cellPadding=\"0\" summary=\"搜索选项\"  class=\"tfm\">\r\n");
	templateBuilder.Append("                    <thead>\r\n");
	templateBuilder.Append("                    <tr>\r\n");
	templateBuilder.Append("	                    <th id=\"divsearchoption\">搜索选项</th>\r\n");
	templateBuilder.Append("	                    <td>&nbsp;</td>\r\n");
	templateBuilder.Append("                    </tr>\r\n");
	templateBuilder.Append("                    </thead>\r\n");
	templateBuilder.Append("                    <tbody>\r\n");
	templateBuilder.Append("                        <tr>\r\n");
	templateBuilder.Append("	                        <th><label for=\"poster\">作者</label></th>\r\n");
	templateBuilder.Append("	                        <td>\r\n");
	templateBuilder.Append("	                            <input name=\"poster\" type=\"text\" id=\"poster\" size=\"45\" class=\"txt\" />\r\n");
	templateBuilder.Append("	                            <input type=\"checkbox\" value=\"1\" id=\"chkAuthor\" name=\"chkAuthor\" onclick=\"checkauthoroption(this);\" />搜索该作者所有帖子及相关内容\r\n");
	templateBuilder.Append("	                        </td>\r\n");
	templateBuilder.Append("                        </tr>\r\n");
	templateBuilder.Append("                    </tbody>\r\n");
	templateBuilder.Append("                    <tbody id=\"divsearchtype\">\r\n");
	templateBuilder.Append("                        <tr>\r\n");
	templateBuilder.Append("	                        <th><label for=\"poster\">搜索类型</label></th>\r\n");
	templateBuilder.Append("	                        <td>\r\n");
	templateBuilder.Append("	                            <input type=\"hidden\" name=\"type\" value=\"\" id=\"type\" />\r\n");
	templateBuilder.Append("		                        <input name=\"keywordtype\" type=\"radio\" value=\"0\" checked onclick=\"changeoption('');\" />\r\n");
	templateBuilder.Append("		                        全部主题搜索\r\n");
	templateBuilder.Append("		                        <input name=\"keywordtype\" type=\"radio\" value=\"4\" onclick=\"changeoption('digest');\"/>\r\n");
	templateBuilder.Append("		                        精华主题搜索\r\n");

	if (usergroupinfo.Allowsearch==1)
	{

	templateBuilder.Append("			                        <input type=\"radio\" name=\"keywordtype\" value=\"1\" onclick=\"changeoption('post');\" />\r\n");
	templateBuilder.Append("		                        全文搜索\r\n");

	}	//end if


	if (config.Enablespace==1)
	{

	templateBuilder.Append("		                        <input type=\"radio\" name=\"keywordtype\" value=\"2\" onclick=\"changeoption('spacepost');\" />\r\n");
	templateBuilder.Append("		                        日志搜索\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("		                        <input type=\"radio\" name=\"keywordtype\" value=\"3\" onclick=\"changeoption('album');\"/>\r\n");
	templateBuilder.Append("		                        相册搜索\r\n");

	}	//end if

	templateBuilder.Append("	                        </td>\r\n");
	templateBuilder.Append("                        </tr>\r\n");
	templateBuilder.Append("                    </tbody>\r\n");
	templateBuilder.Append("                </table>\r\n");
	templateBuilder.Append("                <div id=\"options\" style=\"margin-top:-1px;\"></div>\r\n");
	templateBuilder.Append("                <script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_search.js\"></");
	templateBuilder.Append("script>	\r\n");
	templateBuilder.Append("                <table cellSpacing=\"0\" cellPadding=\"0\" summary=\"搜索类型\" class=\"tfm\">\r\n");
	templateBuilder.Append("                    <tbody>\r\n");
	templateBuilder.Append("                    <tr><th></th><td><button name=\"submit\" type=\"submit\" id=\"submit\" class=\"pn\"><span>执行搜索</span></button></td></tr>\r\n");
	templateBuilder.Append("                    </tbody>\r\n");
	templateBuilder.Append("                </table>\r\n");
	templateBuilder.Append("             </form>\r\n");
	templateBuilder.Append("             </div>\r\n");

	}	//end if


	}
	else
	{

	templateBuilder.Append("        <div class=\"wrap cl\">\r\n");
	templateBuilder.Append("            <form action=\"search.aspx\" autocomplete=\"off\" name=\"postpm\" method=\"post\" class=\"searchform\">\r\n");
	templateBuilder.Append("                <table cellspacing=\"0\" cellpadding=\"0\" class=\"mbm\" id=\"tpsch\">\r\n");
	templateBuilder.Append("                <tbody>\r\n");
	templateBuilder.Append("                    <tr>\r\n");
	templateBuilder.Append("                    <td><h1><a title=\"Discuz!NT|BBS|论坛\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\"><img alt=\"Discuz!NT|BBS|论坛\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("templates/default/images/logo.png\"></a></h1></td>\r\n");
	templateBuilder.Append("                    <td>\r\n");
	templateBuilder.Append("                    <ul class=\"tb cl\">\r\n");
	templateBuilder.Append("                        <li id=\"forumli\"><a href=\"search.aspx?keyword=");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("&searchsubmit=1\">论坛</a></li>\r\n");

	if (config.Enablespace==1)
	{

	templateBuilder.Append("                        <li id=\"spaceli\"><a href=\"search.aspx?type=spacepost&keyword=");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("&searchsubmit=1\">日志</a></li>\r\n");

	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("                        <li id=\"albumli\"><a href=\"search.aspx?type=album&keyword=");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("&searchsubmit=1\">相册</a></li>\r\n");

	}	//end if

	templateBuilder.Append("                    </ul>\r\n");
	templateBuilder.Append("                    <input type=\"hidden\" value=\"\" name=\"type\" />\r\n");
	templateBuilder.Append("                    <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                        var type = '");
	templateBuilder.Append(type.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("                        switch (type) {\r\n");
	templateBuilder.Append("                            case 'spacepost': $('spaceli').className = 'a'; break;\r\n");
	templateBuilder.Append("                            case 'album': $('albumli').className = 'a'; break;\r\n");
	templateBuilder.Append("                            default: $('forumli').className = 'a'; break;\r\n");
	templateBuilder.Append("                        }\r\n");
	templateBuilder.Append("                    </");
	templateBuilder.Append("script>\r\n");
	templateBuilder.Append("                        <table cellspacing=\"0\" cellpadding=\"0\" id=\"tps_form\">\r\n");
	templateBuilder.Append("	                        <tbody>\r\n");
	templateBuilder.Append("	                        <tr>\r\n");
	templateBuilder.Append("		                        <td>\r\n");
	templateBuilder.Append("			                        <input type=\"text\" tabindex=\"1\" value=\"");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("\" maxlength=\"40\" size=\"45\" class=\"schtxt\" name=\"keyword\" id=\"srchtxt\">\r\n");
	templateBuilder.Append("		                        </td>\r\n");
	templateBuilder.Append("		                        <td>\r\n");
	templateBuilder.Append("			                        <button class=\"schbtn\" id=\"tps_btn\" type=\"submit\"><strong>搜索</strong></button>\r\n");
	templateBuilder.Append("		                        </td>\r\n");
	templateBuilder.Append("		                        <td style=\"padding-left: 10px; background:#FFF;\">\r\n");
	templateBuilder.Append("			                        <label><a href=\"search.aspx?advsearch=1\">高级</a></label>\r\n");
	templateBuilder.Append("		                        </td>\r\n");
	templateBuilder.Append("	                        </tr>\r\n");
	templateBuilder.Append("	                        </tbody>\r\n");
	templateBuilder.Append("                        </table>\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                    </tr>\r\n");
	templateBuilder.Append("                </tbody>\r\n");
	templateBuilder.Append("                </table>\r\n");
	templateBuilder.Append("            </form>\r\n");
	templateBuilder.Append("            <div class=\"tl\">\r\n");

	if (type=="album")
	{

	templateBuilder.Append("	                <div class=\"sttl mbn\">\r\n");
	templateBuilder.Append("                        <h2>相册搜索结果: <em>共搜索到");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append("个符合条件的相册</em></h2>\r\n");
	templateBuilder.Append("                    </div>\r\n");
	templateBuilder.Append("					<div class=\"slst mtw\">\r\n");
	templateBuilder.Append("						<ul>\r\n");

	int album__loop__id=0;
	foreach(DataRow album in albumlist.Rows)
	{
		album__loop__id++;

	templateBuilder.Append("							<li class=\"pbw\">\r\n");
	templateBuilder.Append("								<h3 class=\"xs3\">\r\n");
	templateBuilder.Append("									 <a href=\"showalbum.aspx?albumid=" + album["albumid"].ToString().Trim() + "\" target=\"_blank\">" + album["title"].ToString().Trim() + "</a>\r\n");
	templateBuilder.Append("								</h3>\r\n");

	if (album["logo"].ToString().Trim()!="")
	{

	templateBuilder.Append("								    <p><a href=\"showalbum.aspx?albumid=" + album["albumid"].ToString().Trim() + "\" target=\"_blank\"><img alt=\"" + album["title"].ToString().Trim() + "\" src=\"" + album["logo"].ToString().Trim() + "\" /></a></p>\r\n");

	}	//end if

	templateBuilder.Append("								<p class=\"xg1\">" + album["imgcount"].ToString().Trim() + "个相片</p>\r\n");
	templateBuilder.Append("								<p>\r\n");
	templateBuilder.Append("								<span> \r\n");
	string cdtime = ForumUtils.ConvertDateTime(album["createdatetime"].ToString().Trim());
	templateBuilder.Append(cdtime.ToString());
	templateBuilder.Append("</span>\r\n");
	templateBuilder.Append("								 - \r\n");
	templateBuilder.Append("								<span>\r\n");

	if (Utils.StrToInt(album["userid"].ToString().Trim(), 0)==-1)
	{

	templateBuilder.Append("										游客\r\n");

	}
	else
	{

	templateBuilder.Append("										<a href=\"showalbumlist.aspx?uid=" + album["userid"].ToString().Trim() + "\" target=\"_parent\">" + album["username"].ToString().Trim() + "</a>\r\n");

	}	//end if

	templateBuilder.Append("								</span>\r\n");
	templateBuilder.Append("								 - \r\n");
	templateBuilder.Append("								<span><a href=\"showalbumlist.aspx?cate=" + album["albumcateid"].ToString().Trim() + "\" target=\"_parent\">" + album["categorytitle"].ToString().Trim() + "</a></span>\r\n");
	templateBuilder.Append("								</p>\r\n");
	templateBuilder.Append("							</li>\r\n");

	}	//end loop

	templateBuilder.Append("						</ul>\r\n");
	templateBuilder.Append("					</div>\r\n");
	templateBuilder.Append("	                <div class=\"pages_btns\">\r\n");
	templateBuilder.Append("		                <div class=\"pages\">\r\n");

	if (pagecount>1)
	{

	templateBuilder.Append("<em>");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("页</em>\r\n");

	}	//end if
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("		                </div>\r\n");
	templateBuilder.Append("	                </div>\r\n");

	}
	else if (type=="spacepost")
	{

	templateBuilder.Append("                    <div class=\"sttl mbn\">\r\n");
	templateBuilder.Append("                        <h2>日志搜索结果: <em>共搜索到");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append("篇符合条件的日志</em></h2>\r\n");
	templateBuilder.Append("                    </div>\r\n");
	templateBuilder.Append("				    <div class=\"slst mtw\">\r\n");
	templateBuilder.Append("					    <ul>\r\n");

	int spacepost__loop__id=0;
	foreach(DataRow spacepost in spacepostlist.Rows)
	{
		spacepost__loop__id++;

	templateBuilder.Append("						    <li class=\"pbw\">\r\n");
	templateBuilder.Append("							    <h3 class=\"xs3\">\r\n");
	string sptitle = LightKeyWord(spacepost["title"].ToString().Trim(),keyword);
	
	templateBuilder.Append("								    <a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/viewspacepost.aspx?postid=" + spacepost["postid"].ToString().Trim() + "\" target=\"_blank\">");
	templateBuilder.Append(sptitle.ToString());
	templateBuilder.Append("</a>\r\n");
	templateBuilder.Append("							    </h3>\r\n");
	templateBuilder.Append("							    <p class=\"xg1\">" + spacepost["commentcount"].ToString().Trim() + " 个评论 - " + spacepost["views"].ToString().Trim() + " 次查看</p>\r\n");
	templateBuilder.Append("							    <p>\r\n");
	templateBuilder.Append("							    <span>\r\n");
	string spdtime = ForumUtils.ConvertDateTime(spacepost["postdatetime"].ToString().Trim());
	templateBuilder.Append(spdtime.ToString());
	templateBuilder.Append("</span>\r\n");
	templateBuilder.Append("							     - \r\n");
	templateBuilder.Append("							    <span>\r\n");

	if (Utils.StrToInt(spacepost["uid"].ToString().Trim(), 0)==-1)
	{

	templateBuilder.Append("									    游客\r\n");

	}
	else
	{

	templateBuilder.Append("									    <a href=\"");
	templateBuilder.Append(spaceurl.ToString());
	templateBuilder.Append("space/?uid=" + spacepost["uid"].ToString().Trim() + "\" target=\"_parent\">" + spacepost["author"].ToString().Trim() + "</a>\r\n");

	}	//end if

	templateBuilder.Append("							    </span>\r\n");
	templateBuilder.Append("							    </p>\r\n");
	templateBuilder.Append("						    </li>\r\n");

	}	//end loop

	templateBuilder.Append("					    </ul>\r\n");
	templateBuilder.Append("				    </div>\r\n");
	templateBuilder.Append("                    <div class=\"pages_btns\">\r\n");
	templateBuilder.Append("                        <div class=\"pages\">\r\n");

	if (pagecount>1)
	{

	templateBuilder.Append("<em>");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("页</em>\r\n");

	}	//end if
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("                        </div>\r\n");
	templateBuilder.Append("                    </div>\r\n");

	}
	else if (type=="author")
	{

	templateBuilder.Append("                    <div id=\"resultid1\" style=\"display:block;\">\r\n");
	templateBuilder.Append("                        <div class=\"sttl mbn\">\r\n");
	templateBuilder.Append("                            <h2>帖子搜索结果: <em>共搜索到");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append("个符合条件的帖子</em></h2>\r\n");
	templateBuilder.Append("                        </div>\r\n");
	templateBuilder.Append("			            <div class=\"slst mtw\">\r\n");
	templateBuilder.Append("				            <ul>\r\n");

	int topic__loop__id=0;
	foreach(DataRow topic in topiclist.Rows)
	{
		topic__loop__id++;

	templateBuilder.Append("					            <li class=\"pbw\">\r\n");
	templateBuilder.Append("						            <h3 class=\"xs3\">\r\n");
	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic["tid"].ToString().Trim(),0);
	
	templateBuilder.Append("							            <a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">" + topic["title"].ToString().Trim() + "</a>\r\n");
	templateBuilder.Append("						            </h3>\r\n");
	templateBuilder.Append("						            <p class=\"xg1\">" + topic["replies"].ToString().Trim() + " 个评论 - " + topic["views"].ToString().Trim() + " 次查看</p>\r\n");
	templateBuilder.Append("						            <p></p>\r\n");
	templateBuilder.Append("						            <p>\r\n");
	templateBuilder.Append("						            <span>\r\n");
	string pdtime = ForumUtils.ConvertDateTime(topic["postdatetime"].ToString().Trim());
	templateBuilder.Append(pdtime.ToString());
	templateBuilder.Append("</span>\r\n");
	templateBuilder.Append("						             - \r\n");
	templateBuilder.Append("						            <span>\r\n");

	if (Utils.StrToInt(topic["posterid"].ToString().Trim(), 0)==-1)
	{

	templateBuilder.Append("							            游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(topic["posterid"].ToString().Trim());
	
	templateBuilder.Append("							            <a id=\"search" + topic["tid"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_parent\" \r\n");

	if (useradminid==1||useradminid==2)
	{

	templateBuilder.Append(" onmouseout=\"showMenu(this.id);\" onmousemove=\"showMenu(this.id);\"\r\n");

	}	//end if

	templateBuilder.Append(">" + topic["poster"].ToString().Trim() + "</a>\r\n");

	}	//end if

	templateBuilder.Append("						            </span>\r\n");
	templateBuilder.Append("						             - \r\n");
	templateBuilder.Append("						            <span> \r\n");
	 aspxrewriteurl = this.ShowForumAspxRewrite(topic["fid"].ToString().Trim(),0);
	
	templateBuilder.Append("							            <a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_parent\">" + topic["forumname"].ToString().Trim() + "</a>\r\n");
	templateBuilder.Append("						            </span>\r\n");
	templateBuilder.Append("						            </p>\r\n");
	templateBuilder.Append("					            </li>\r\n");
	templateBuilder.Append("                                    <ul class=\"p_pop\" id=\"search" + topic["tid"].ToString().Trim() + "_menu\" style=\"display:none\">\r\n");
	templateBuilder.Append("                                        <li><a href=\"useradmin.aspx?action=banuser&uid=" + topic["posterid"].ToString().Trim() + "\" onclick=\"showWindow('mods', this.href);doane(event);\" class=\"forbid_user\">禁言用户</a></li>\r\n");
	templateBuilder.Append("                                        <li><a href=\"###\" onclick=\"if(confirm('您确定要删除吗？')) _auditpost('deletepostsbyuidanddays'," + topic["posterid"].ToString().Trim() + ")\">删除7天内的帖子</a></li>\r\n");
	 aspxrewriteurl = this.UserInfoAspxRewrite(topic["posterid"].ToString().Trim());
	
	templateBuilder.Append("						                <li><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a></li>\r\n");
	templateBuilder.Append("                                        <li><a href=\"search.aspx?posterid={topic[poster]&searchsubmit=1\" class=\"all_topic\" target=\"_blank\">查看所有帖子</a></li>\r\n");
	templateBuilder.Append("                                    </ul>\r\n");

	}	//end loop

	templateBuilder.Append("				            </ul>\r\n");
	templateBuilder.Append("			            </div>\r\n");
	templateBuilder.Append("                        <div class=\"pages_btns\">\r\n");
	templateBuilder.Append("                            <div class=\"pages\">\r\n");

	if (topicpagecount>1)
	{

	templateBuilder.Append("<em>");
	templateBuilder.Append(topicpageid.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(topicpagecount.ToString());
	templateBuilder.Append("页</em>\r\n");

	}	//end if
	templateBuilder.Append(topicpagenumbers.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("                            </div>\r\n");
	templateBuilder.Append("                        </div>\r\n");
	templateBuilder.Append("                    </div>\r\n");

	}
	else
	{

	templateBuilder.Append("                    <div class=\"sttl mbn\">\r\n");
	templateBuilder.Append("                        <h2>结果: <em>\r\n");

	if (keyword!="")
	{

	templateBuilder.Append("                            对关键词 \"");
	templateBuilder.Append(keyword.ToString());
	templateBuilder.Append("\" \r\n");

	if (poster!="")
	{

	templateBuilder.Append("和\r\n");

	}
	else
	{

	templateBuilder.Append("的\r\n");

	}	//end if


	}	//end if


	if (poster!="")
	{

	templateBuilder.Append("                            对作者 \"");
	templateBuilder.Append(poster.ToString());
	templateBuilder.Append("\" 的\r\n");

	}	//end if

	templateBuilder.Append("                        查询,找到相关主题 ");
	templateBuilder.Append(topiccount.ToString());
	templateBuilder.Append(" 个</em></h2>\r\n");
	templateBuilder.Append("                    </div>\r\n");
	templateBuilder.Append("			        <div class=\"slst mtw\">\r\n");
	templateBuilder.Append("				        <ul>\r\n");

	int topic__loop__id=0;
	foreach(DataRow topic in topiclist.Rows)
	{
		topic__loop__id++;

	templateBuilder.Append("					        <li class=\"pbw\">\r\n");
	templateBuilder.Append("						        <h3 class=\"xs3\">\r\n");
	 aspxrewriteurl = this.ShowTopicAspxRewrite(topic["tid"].ToString().Trim(),0);
	
	string tptitle = LightKeyWord(topic["title"].ToString().Trim(),keyword);
	
	templateBuilder.Append("							        <a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(tptitle.ToString());
	templateBuilder.Append("</a>\r\n");
	templateBuilder.Append("						        </h3>\r\n");
	templateBuilder.Append("						        <p class=\"xg1\">" + topic["replies"].ToString().Trim() + " 个评论 - " + topic["views"].ToString().Trim() + " 次查看</p>\r\n");
	templateBuilder.Append("						        <p></p>\r\n");
	templateBuilder.Append("						        <p>\r\n");
	templateBuilder.Append("						        <span> \r\n");
	string pdtime = ForumUtils.ConvertDateTime(topic["postdatetime"].ToString().Trim());
	templateBuilder.Append(pdtime.ToString());
	templateBuilder.Append("</span>\r\n");
	templateBuilder.Append("						         - \r\n");
	templateBuilder.Append("						        <span>\r\n");

	if (Utils.StrToInt(topic["posterid"].ToString().Trim(), 0)==-1)
	{

	templateBuilder.Append("								        游客\r\n");

	}
	else
	{

	 aspxrewriteurl = this.UserInfoAspxRewrite(topic["posterid"].ToString().Trim());
	
	templateBuilder.Append("								        <a id=\"search" + topic["tid"].ToString().Trim() + "\" href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_parent\" \r\n");

	if (useradminid==1||useradminid==2)
	{

	templateBuilder.Append("onmouseout=\"showMenu(this.id);\" onmousemove=\"showMenu(this.id);\"\r\n");

	}	//end if

	templateBuilder.Append(">" + topic["poster"].ToString().Trim() + "</a>\r\n");

	}	//end if

	templateBuilder.Append("						        </span>\r\n");
	templateBuilder.Append("						         - \r\n");
	templateBuilder.Append("						        <span>\r\n");
	 aspxrewriteurl = this.ShowForumAspxRewrite(topic["fid"].ToString().Trim(),0);
	
	templateBuilder.Append("<a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_parent\">" + topic["forumname"].ToString().Trim() + "</a></span>\r\n");
	templateBuilder.Append("						        </p>\r\n");
	templateBuilder.Append("					        </li>\r\n");
	templateBuilder.Append("                                <ul class=\"p_pop\" id=\"search" + topic["tid"].ToString().Trim() + "_menu\" style=\"display:none\">\r\n");
	templateBuilder.Append("                                    <li><a href=\"useradmin.aspx?action=banuser&uid=" + topic["posterid"].ToString().Trim() + "\" onclick=\"showWindow('mods', this.href);doane(event);\" class=\"forbid_user\">禁言用户</a></li>\r\n");
	templateBuilder.Append("                                    <li><a href=\"###\" onclick=\"if(confirm('您确定要删除吗？')) _auditpost('deletepostsbyuidanddays'," + topic["posterid"].ToString().Trim() + ")\">删除7天内的帖子</a></li>\r\n");
	 aspxrewriteurl = this.UserInfoAspxRewrite(topic["posterid"].ToString().Trim());
	
	templateBuilder.Append("						            <li><a href=\"");
	templateBuilder.Append(aspxrewriteurl.ToString());
	templateBuilder.Append("\" target=\"_blank\" class=\"public_info\">查看公共资料</a></li>\r\n");
	templateBuilder.Append("                                    <li><a href=\"search.aspx?posterid=" + topic["poster"].ToString().Trim() + "&searchsubmit=1\" class=\"all_topic\" target=\"_blank\">查看所有帖子</a></li>\r\n");
	templateBuilder.Append("                                </ul>\r\n");

	}	//end loop

	templateBuilder.Append("				        </ul>\r\n");
	templateBuilder.Append("			        </div>\r\n");
	templateBuilder.Append("                    <div class=\"pages_btns\">\r\n");
	templateBuilder.Append("                        <div class=\"pages\">\r\n");

	if (pagecount>1)
	{

	templateBuilder.Append("<em>");
	templateBuilder.Append(pageid.ToString());
	templateBuilder.Append("/");
	templateBuilder.Append(pagecount.ToString());
	templateBuilder.Append("页</em>\r\n");

	}	//end if
	templateBuilder.Append(pagenumbers.ToString());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("                        </div>\r\n");
	templateBuilder.Append("                    </div>\r\n");

	}	//end if

	templateBuilder.Append("            </div>\r\n");

	}	//end if

	templateBuilder.Append("    </div>\r\n");

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
