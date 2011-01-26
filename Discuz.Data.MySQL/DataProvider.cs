#if NET1
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Discuz.Config;
using Discuz.Entity;
using System.Data;
using Discuz.Data;
using Discuz.Common;
using Discuz.Forum;
using MySql.Data.MySqlClient;

namespace Discuz.Data.MySql
{
   public class DataProvider : IDataProvider
    {

	   #region ForumManage

	   /// <summary>
	   /// MYSQL SQL语句转义
	   /// </summary>
	   /// <param name="str">需要转义的关键字符串</param>
	   /// <param name="pattern">需要转义的字符数组</param>
	   /// <returns>转义后的字符串</returns>
	   private static string RegEsc(string str)
	   {
		   string[] pattern ={ @"%", @"_", @"'" };
		   foreach (string s in pattern)
		   {
			   //Regex rgx = new Regex(s);
			   //keyword = rgx.Replace(keyword, "\\" + s);
			   str = str.Replace(s, "\\" + s);
		   }
		   return str;
	   }
	   /// <summary>
	   /// 添加友情链接
	   /// </summary>
	   /// <param name="displayorder">显示顺序</param>
	   /// <param name="name">名称</param>
	   /// <param name="url">链接地址</param>
	   /// <param name="note">备注</param>
	   /// <param name="logo">Logo地址</param>
	   /// <returns></returns>
	   public int AddForumLink(int displayorder, string name, string url, string note, string logo)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 100, name),
									 DbHelper.MakeInParam("?url", (DbType)MySqlDbType.VarString, 100, url),
									 DbHelper.MakeInParam("?note", (DbType)MySqlDbType.VarString, 200, note),
									 DbHelper.MakeInParam("?logo", (DbType)MySqlDbType.VarString, 100, logo)
								 };
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "forumlinks` (`displayorder`, `name`,`url`,`note`,`logo`) VALUES (?displayorder,?name,?url,?note,?logo)";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   /// <summary>
	   /// 获得所有友情链接
	   /// </summary>
	   /// <returns></returns>
	   public string GetForumLinks()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "forumlinks`";
	   }

	   /// <summary>
	   /// 删除指定友情链接
	   /// </summary>
	   /// <param name="forumlinkid"></param>
	   /// <returns></returns>
	   public int DeleteForumLink(string  forumlinkid)
	   {
           
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "forumlinks` WHERE `id`IN ("+forumlinkid+")";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   /// <summary>
	   /// 更新指定友情链接
	   /// </summary>
	   /// <param name="id">友情链接Id</param>
	   /// <param name="displayorder">显示顺序</param>
	   /// <param name="name">名称</param>
	   /// <param name="url">链接地址</param>
	   /// <param name="note">备注</param>
	   /// <param name="logo">Logo地址</param>
	   /// <returns></returns>
	   public int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 100, name),
									 DbHelper.MakeInParam("?url", (DbType)MySqlDbType.VarString, 100, url),
									 DbHelper.MakeInParam("?note", (DbType)MySqlDbType.VarString, 200, note),
									 DbHelper.MakeInParam("?logo", (DbType)MySqlDbType.VarString, 100, logo),
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forumlinks` SET `displayorder`=?displayorder,`name`=?name,`url`=?url,`note`=?note,`logo`=?logo  Where `id`=?id";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }


	   /// <summary>
	   /// 获得首页版块列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetForumIndexListTable()
	   {
		   // string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, `lastpost`, GETDATE())<600 THEN 'new' ELSE 'old' END AS `havenew`,[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].`fid`=[{0}forumfields].`fid` WHERE [{0}forums].`parentid` NOT IN (SELECT fid FROM [{0}forums] WHERE `status` < 1 AND `layer` = 0) AND [{0}forums].`status` > 0 AND `layer` <= 1 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   //string newtime = Utils.AdDeTime(-600);

		   string commandText = string.Format("SELECT IF(FLOOR(UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(`lastpost`))/60)<600,'new','old') AS `havenew`,`{0}forums`.*,`{0}forums`.`fid` as `fid`,`{0}forumfields`.* FROM `{0}forums` LEFT JOIN `{0}forumfields` ON `{0}forums`.`fid`=`{0}forumfields`.`fid` WHERE `{0}forums`.`parentid` NOT IN (SELECT fid FROM `{0}forums` WHERE `status` < 1 AND `layer` = 0) AND `{0}forums`.`status` > 0 AND `layer` <= 1 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
                     
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   /// <summary>
	   /// 获得首页版块列表
	   /// </summary>
	   /// <returns></returns>
	   public IDataReader GetForumIndexList()
	   {
		   // string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, `lastpost`, GETDATE())<600 THEN 'new' ELSE 'old' END AS `havenew`,[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].`fid`=[{0}forumfields].`fid` WHERE [{0}forums].`parentid` NOT IN (SELECT fid FROM [{0}forums] WHERE `status` < 1 AND `layer` = 0) AND [{0}forums].`status` > 0 AND `layer` <= 1 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   //string newtime = Utils.AdDeTime(-600);
		   string commandText = string.Format("SELECT IF(FLOOR((UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(`lastpost`))/60)<600,'new','old') AS `havenew`,`{0}forums`.*,`{0}forums`.`fid` as `fid`,`{0}forumfields`.* FROM `{0}forums` LEFT JOIN `{0}forumfields` ON `{0}forums`.`fid`=`{0}forumfields`.`fid` WHERE `{0}forums`.`parentid` NOT IN (SELECT `fid` FROM `{0}forums` WHERE `status` < 1 AND `layer` = 0) AND `{0}forums`.`status` > 0 AND `layer` <= 1 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   //string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [{0}forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
		   //string commandText = string.Format("SELECT IF FLOOR((UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(`lastpost`))/60)<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [{0}forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            
		   return DbHelper.ExecuteReader(CommandType.Text, commandText);
	   }

	   /// <summary>
	   /// 获得简介版论坛首页列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetArchiverForumIndexList()
	   {
		   string commandText = string.Format("SELECT `{0}forums`.`fid`, `{0}forums`.`name`, `{0}forums`.`layer`, `{0}forumfields`.`viewperm` FROM `{0}forums` LEFT JOIN `{0}forumfields` ON `{0}forums`.`fid`=`{0}forumfields`.`fid` WHERE `{0}forums`.`status` > 0  ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   /// <summary>
	   /// 获得子版块列表
	   /// </summary>
	   /// <param name="fid">版块id</param>
	   /// <returns></returns>
	   public IDataReader GetSubForumReader(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid)
								 };
		   //string newtime = Utils.AdDeTime(-600);
		   //    string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, `lastpost`, GETDATE())<600 THEN 'new' ELSE 'old' END AS `havenew`,[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].`fid`=[{0}forumfields].`fid` WHERE `parentid` = ?fid AND `status` > 0 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   String commandText = "SELECT IF(FLOOR((UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(`lastpost`))/60)<600 ,'new','old') AS `havenew`,`" + BaseConfigs.GetTablePrefix + "forums`.*,`" + BaseConfigs.GetTablePrefix + "forums`.`fid` As `fid`,`" + BaseConfigs.GetTablePrefix + "forumfields`.* FROM `" + BaseConfigs.GetTablePrefix + "forums` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forumfields` ON `" + BaseConfigs.GetTablePrefix + "forums`.`fid`=`" + BaseConfigs.GetTablePrefix + "forumfields`.`fid` WHERE `parentid` = ?fid AND `status` > 0 ORDER BY `displayorder`";

		   return DbHelper.ExecuteReader(CommandType.Text, commandText, prams);
	   }

	   /// <summary>
	   /// 获得子版块列表
	   /// </summary>
	   /// <param name="fid">版块id</param>
	   /// <returns></returns>
	   public DataTable GetSubForumTable(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid)
								 };

		   //  string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, `lastpost`, GETDATE())<600 THEN 'new' ELSE 'old' END AS `havenew`,[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].`fid`=[{0}forumfields].`fid` WHERE `parentid` = ?fid AND `status` > 0 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   //string newtime = Utils.AdDeTime(-600);
		   String commandText = "SELECT IF(FLOOR((UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(`lastpost`))/60)<600,'new','old') AS `havenew`,`" + BaseConfigs.GetTablePrefix + "forums`.*,`" + BaseConfigs.GetTablePrefix + "forums`.`fid` As `fid`,`" + BaseConfigs.GetTablePrefix + "forumfields`.* FROM `" + BaseConfigs.GetTablePrefix + "forums` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forumfields` ON `" + BaseConfigs.GetTablePrefix + "forums`.`fid`=`" + BaseConfigs.GetTablePrefix + "forumfields`.`fid` WHERE `parentid` = ?fid AND `status` > 0 ORDER BY `displayorder`";
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText, prams).Tables[0];
	   }

	   /// <summary>
	   /// 获得全部版块列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetForumsTable()
	   {
		   string commandText = string.Format("SELECT `{0}forums`.*, `{0}forumfields`.* FROM `{0}forums` LEFT JOIN `{0}forumfields` ON `{0}forums`.`fid`=`{0}forumfields`.`fid`  ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);
		   //return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		   //dt.Columns.Remove("" + BaseConfigs.GetTablePrefix + "forums.fid");
		   //dt.Columns["dnt_forums.fid"].ColumnName = "fid";
		   return dt;
	   }

	   /// <summary>
	   /// 设置当前版块主题数(不含子版块)
	   /// </summary>
	   /// <param name="fid">版块id</param>
	   /// <returns>主题数</returns>
	   public int SetRealCurrentTopics(int fid)
	   {
		   string cc = string.Format("SELECT COUNT(tid) FROM {0}topics WHERE `displayorder` >= 0 AND `fid`={1}", BaseConfigs.GetTablePrefix, fid);
		   string dd = cc;
		   int count = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(tid) FROM {0}topics WHERE `displayorder` >= 0 AND `fid`={1}", BaseConfigs.GetTablePrefix, fid)), 0);
		   string commandText=string.Format("UPDATE {0}forums SET `curtopics` ="+count+"  WHERE `fid`={1}", BaseConfigs.GetTablePrefix, fid);
		   return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
	   }

	   public DataTable GetForumListTable()
	   {
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT `name`, `fid` FROM `{0}forums` WHERE `{0}forums`.`parentid` NOT IN (SELECT fid FROM `{0}forums` WHERE `status` < 1 AND `layer` = 0) AND `status` > 0 AND `displayorder` >=0 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix)).Tables[0];

		   return dt;
	   }

	   public string GetTemplates()
	   {
		   return "Select templateid,name From `" + BaseConfigs.GetTablePrefix + "templates` ";

	   }

	   public DataTable GetUserGroupsTitle()
	   {
		   string sql = "Select groupid,grouptitle  From `" + BaseConfigs.GetTablePrefix + "usergroups`  Order By `groupid` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetAttachTypes()
	   {
		   string sql = "Select id,extension  From `" + BaseConfigs.GetTablePrefix + "attachtypes`  Order By `id` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetForums()
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "forums` ORDER BY `displayorder` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public string GetForumsTree()
	   {
		   return "SELECT `fid`,`name`,`parentid` FROM `" + BaseConfigs.GetTablePrefix + "forums`";
	   }

	   public int GetForumsMaxDisplayOrder()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "Select max(displayorder) From " + BaseConfigs.GetTablePrefix + "forums").Tables[0].Rows[0][0], 0) + 1;
	   }

	   public DataTable GetForumsMaxDisplayOrder(int parentid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "Select Max(`displayorder`) From `" + BaseConfigs.GetTablePrefix + "forums`  Where `parentid`=" + parentid).Tables[0];
	   }
	   public void UpdateForumsDisplayOrder(int minDisplayOrder)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set displayorder=displayorder+1  Where displayorder>" + minDisplayOrder.ToString());
	   }

	   public void UpdateSubForumCount(int fid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set subforumcount=subforumcount+1  Where fid=" + fid.ToString());
	   }

	   public DataRow GetForum(int fid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "Select * From " + BaseConfigs.GetTablePrefix + "forums Where fid=" + fid.ToString() + " LIMIT 1").Tables[0].Rows[0];
	   }

	   public DataRowCollection GetModerators(int fid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `username` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid` IN(SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "moderators` WHERE `inherited`=1 AND `fid`=" + fid + ")").Tables[0].Rows;
	   }

	   public DataTable GetTopForum(int fid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `parentid`=0 AND `layer`=0 AND `fid`=" + fid + "LIMIT 1").Tables[0];
	   }

	   public int UpdateForum(int fid, string name, int subforumcount, int displayorder)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.String, 50, name),
									 DbHelper.MakeInParam("?subforumcount", (DbType)MySqlDbType.Int32,4, subforumcount),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32,4, displayorder),
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `name`=?name,`subforumcount`=?subforumcount,`displayorder`=?displayorder Where `fid`=?fid";
		   //string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `name`='" + name + "',`subforumcount`=" + subforumcount + ",`displayorder`=" + displayorder + " Where `fid`=" + fid;
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public DataTable GetForumField(int fid, string fieldname)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `" + fieldname + "` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` WHERE `fid`=" + fid + " LIMIT 1").Tables[0];
	   }

	   public int UpdateForumField(int fid, string fieldname)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forumfields` SET `" + fieldname + "`='' WHERE `fid`=" + fid);
	   }

	   public int UpdateForumField(int fid, string fieldname, string fieldvalue)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forumfields` SET `" + fieldname + "`='" + fieldvalue + "' WHERE `fid`=" + fid);
	   }

	   public DataRowCollection GetDatechTableIds()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT id FROM `" + BaseConfigs.GetTablePrefix + "tablelist`").Tables[0].Rows;
	   }

	   public int UpdateMinMaxField(string posttablename, int posttableid)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "tablelist` SET `mintid`=" + GetMinPostTableTid(posttablename) + ",`maxtid`=" + GetMaxPostTableTid(posttablename) + "  WHERE `id`=" + posttableid);
	   }

	   public DataRowCollection GetForumIdList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums`").Tables[0].Rows;
	   }

	   public int CreateFullTextIndex(string dbname)
	   {
		   return 0;
	   }

	   public int GetMaxForumId()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT MAX(fid) FROM " + BaseConfigs.GetTablePrefix + "forums"), 0);
	   }

	   public DataTable GetForumList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid`,`name` FROM `" + BaseConfigs.GetTablePrefix + "forums`").Tables[0];
	   }

	   public DataTable GetAllForumList()
	   {
		   string sql = "Select * From `" + BaseConfigs.GetTablePrefix + "forums` Order By `displayorder` Asc";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetForumInformation(int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32, 4,fid)
			};
		   string sql = "SELECT `" + BaseConfigs.GetTablePrefix + "forums`.*, `" + BaseConfigs.GetTablePrefix + "forumfields`.* FROM `" + BaseConfigs.GetTablePrefix + "forums` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forumfields` ON `" + BaseConfigs.GetTablePrefix + "forums`.`fid`=`" + BaseConfigs.GetTablePrefix + "forumfields`.`fid` WHERE `" + BaseConfigs.GetTablePrefix + "forums`.`fid`=?fid";
		   DataTable dt;
		   dt=DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   dt.Columns[0].ColumnName = "fid";

		   return dt;
	   }

	   public void SaveForumsInfo(ForumInfo __foruminfo)
	   {
		   //SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   //conn.Open();
		   //using (SqlTransaction trans = conn.BeginTransaction())
		   //{
		   //  try
		   //{
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, __foruminfo.Name),
									 DbHelper.MakeInParam("?status", (DbType)MySqlDbType.Int32, 4, __foruminfo.Status),
									 DbHelper.MakeInParam("?colcount", (DbType)MySqlDbType.Int16, 4, __foruminfo.Colcount),
									 DbHelper.MakeInParam("?templateid", (DbType)MySqlDbType.Int16, 2, __foruminfo.Templateid),
									 DbHelper.MakeInParam("?allowsmilies", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowsmilies),
									 DbHelper.MakeInParam("?allowrss", (DbType)MySqlDbType.Int32, 6, __foruminfo.Allowrss),
									 DbHelper.MakeInParam("?allowhtml", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowhtml),
									 DbHelper.MakeInParam("?allowbbcode", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowbbcode),
									 DbHelper.MakeInParam("?allowimgcode", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowimgcode),
									 DbHelper.MakeInParam("?allowblog", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowblog),
									 DbHelper.MakeInParam("?allowtrade", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowtrade),
									 DbHelper.MakeInParam("?alloweditrules", (DbType)MySqlDbType.Int32, 4, __foruminfo.Alloweditrules),
									 DbHelper.MakeInParam("?allowthumbnail", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowthumbnail),
									 DbHelper.MakeInParam("?recyclebin", (DbType)MySqlDbType.Int32, 4, __foruminfo.Recyclebin),
									 DbHelper.MakeInParam("?modnewposts", (DbType)MySqlDbType.Int32, 4, __foruminfo.Modnewposts),
									 DbHelper.MakeInParam("?jammer", (DbType)MySqlDbType.Int32, 4, __foruminfo.Jammer),
									 DbHelper.MakeInParam("?disablewatermark", (DbType)MySqlDbType.Int32, 4, __foruminfo.Disablewatermark),
									 DbHelper.MakeInParam("?inheritedmod", (DbType)MySqlDbType.Int32, 4, __foruminfo.Inheritedmod),
									 DbHelper.MakeInParam("?autoclose", (DbType)MySqlDbType.Int16, 2, __foruminfo.Autoclose),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, __foruminfo.Displayorder),
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, __foruminfo.Fid)
								 };
		   string sql = "update `" + BaseConfigs.GetTablePrefix + "forums` set name=?name,  status=?status, colcount=?colcount, templateid=?templateid,allowsmilies=?allowsmilies ,allowrss=?allowrss, allowhtml=?allowhtml, allowbbcode=?allowbbcode, allowimgcode=?allowimgcode, allowblog=?allowblog,allowtrade=?allowtrade,alloweditrules=?alloweditrules ,allowthumbnail=?allowthumbnail ,recyclebin=?recyclebin, modnewposts=?modnewposts,jammer=?jammer,disablewatermark=?disablewatermark,inheritedmod=?inheritedmod,autoclose=?autoclose,displayorder=?displayorder  Where fid=?fid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);

		   IDataParameter[] prams1 = {
									  DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 0, __foruminfo.Description),
									  DbHelper.MakeInParam("?password", (DbType)MySqlDbType.VarString, 16, __foruminfo.Password),
									  DbHelper.MakeInParam("?icon", (DbType)MySqlDbType.VarString, 255, __foruminfo.Icon),
									  DbHelper.MakeInParam("?redirect", (DbType)MySqlDbType.VarString, 255, __foruminfo.Redirect),
									  DbHelper.MakeInParam("?attachextensions", (DbType)MySqlDbType.VarString, 255, __foruminfo.Attachextensions),
									  DbHelper.MakeInParam("?rules", (DbType)MySqlDbType.VarString, 0, __foruminfo.Rules),
									  DbHelper.MakeInParam("?topictypes", (DbType)MySqlDbType.VarString, 0, __foruminfo.Topictypes),
									  DbHelper.MakeInParam("?viewperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Viewperm),
									  DbHelper.MakeInParam("?postperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Postperm),
									  DbHelper.MakeInParam("?replyperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Replyperm),
									  DbHelper.MakeInParam("?getattachperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Getattachperm),
									  DbHelper.MakeInParam("?postattachperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Postattachperm),
									  DbHelper.MakeInParam("?applytopictype", (DbType)MySqlDbType.Int16, 1, __foruminfo.Applytopictype),
									  DbHelper.MakeInParam("?postbytopictype", (DbType)MySqlDbType.Int16, 1, __foruminfo.Postbytopictype),
									  DbHelper.MakeInParam("?viewbytopictype", (DbType)MySqlDbType.Int16, 1, __foruminfo.Viewbytopictype),
									  DbHelper.MakeInParam("?topictypeprefix", (DbType)MySqlDbType.Int16, 1, __foruminfo.Topictypeprefix),
									  DbHelper.MakeInParam("?permuserlist", (DbType)MySqlDbType.VarString, 0, __foruminfo.Permuserlist),
									  DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, __foruminfo.Fid)
								  };
		   sql = "Update `" + BaseConfigs.GetTablePrefix + "forumfields` Set `description`=?description,`password`=?password,`icon`=?icon,`redirect`=?redirect,"
			   + "`attachextensions`=?attachextensions,`rules`=?rules,`topictypes`=?topictypes,`viewperm`=?viewperm,`postperm`=?postperm,`replyperm`=?replyperm,"
			   + "`getattachperm`=?getattachperm,`postattachperm`=?postattachperm,`applytopictype`=?applytopictype,`postbytopictype`=?postbytopictype,"
			   + "`viewbytopictype`=?viewbytopictype,`topictypeprefix`=?topictypeprefix,`permuserlist`=?permuserlist Where `fid`=?fid";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams1);

		   //  trans.Commit();
		   //}
		   //catch (Exception ex)
		   //{
		   //  trans.Rollback();
		   //throw ex;
		   //    }
		   //}
		   //conn.Close();
	   }

	   public int InsertForumsInf(ForumInfo __foruminfo)
	   {
           
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?parentid", (DbType)MySqlDbType.Int16, 2, __foruminfo.Parentid),
									 DbHelper.MakeInParam("?layer", (DbType)MySqlDbType.Int32, 4, __foruminfo.Layer),
									 DbHelper.MakeInParam("?pathlist", (DbType)MySqlDbType.VarString, 3000, __foruminfo.Pathlist == null ? " " : __foruminfo.Pathlist),
									 DbHelper.MakeInParam("?parentidlist", (DbType)MySqlDbType.VarString, 300, __foruminfo.Parentidlist== null ? " " : __foruminfo.Parentidlist),
									 DbHelper.MakeInParam("?subforumcount", (DbType)MySqlDbType.Int32, 4, __foruminfo.Subforumcount),
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, __foruminfo.Name),
									 DbHelper.MakeInParam("?status", (DbType)MySqlDbType.Int32, 4, __foruminfo.Status),
									 DbHelper.MakeInParam("?colcount", (DbType)MySqlDbType.Int16, 4, __foruminfo.Colcount),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, __foruminfo.Displayorder),
									 DbHelper.MakeInParam("?templateid", (DbType)MySqlDbType.Int16, 2, __foruminfo.Templateid),
									 DbHelper.MakeInParam("?allowsmilies", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowsmilies),
									 DbHelper.MakeInParam("?allowrss", (DbType)MySqlDbType.Int32, 6, __foruminfo.Allowrss),
									 DbHelper.MakeInParam("?allowhtml", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowhtml),
									 DbHelper.MakeInParam("?allowbbcode", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowbbcode),
									 DbHelper.MakeInParam("?allowimgcode", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowimgcode),
									 DbHelper.MakeInParam("?allowblog", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowblog),
									 DbHelper.MakeInParam("?allowtrade", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowtrade),
									 DbHelper.MakeInParam("?alloweditrules", (DbType)MySqlDbType.Int32, 4, __foruminfo.Alloweditrules),
									 DbHelper.MakeInParam("?allowthumbnail", (DbType)MySqlDbType.Int32, 4, __foruminfo.Allowthumbnail),
									 DbHelper.MakeInParam("?recyclebin", (DbType)MySqlDbType.Int32, 4, __foruminfo.Recyclebin),
									 DbHelper.MakeInParam("?modnewposts", (DbType)MySqlDbType.Int32, 4, __foruminfo.Modnewposts),
									 DbHelper.MakeInParam("?jammer", (DbType)MySqlDbType.Int32, 4, __foruminfo.Jammer),
									 DbHelper.MakeInParam("?disablewatermark", (DbType)MySqlDbType.Int32, 4, __foruminfo.Disablewatermark),
									 DbHelper.MakeInParam("?inheritedmod", (DbType)MySqlDbType.Int32, 4, __foruminfo.Inheritedmod),
									 DbHelper.MakeInParam("?autoclose", (DbType)MySqlDbType.Int16, 2, __foruminfo.Autoclose)
								 };
		   string sql = "Insert Into `" + BaseConfigs.GetTablePrefix + "forums` (parentid,layer,pathlist,parentidlist,subforumcount,name,status,colcount, displayorder,templateid,allowsmilies,allowrss,allowhtml,allowbbcode,allowimgcode,allowblog,allowtrade,alloweditrules,recyclebin,modnewposts,jammer,disablewatermark,inheritedmod,autoclose,allowthumbnail,lastpost) Values (?parentid,?layer,?pathlist,?parentidlist,?subforumcount,?name,?status, ?colcount, ?displayorder,?templateid,?allowsmilies,?allowrss,?allowhtml,?allowbbcode,?allowimgcode,?allowblog,?allowtrade,?alloweditrules,?recyclebin,?modnewposts,?jammer,?disablewatermark,?inheritedmod,?autoclose,?allowthumbnail,NOW())";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);

		   int fid = GetMaxForumId();

		   IDataParameter[] prams1 = {
									  DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid),
									  DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 0, __foruminfo.Description),
									  DbHelper.MakeInParam("?password", (DbType)MySqlDbType.VarString, 16, __foruminfo.Password),
									  DbHelper.MakeInParam("?icon", (DbType)MySqlDbType.VarString, 255, __foruminfo.Icon),
									  DbHelper.MakeInParam("?postcredits", (DbType)MySqlDbType.VarString, 255, __foruminfo.Postcredits),
									  DbHelper.MakeInParam("?replycredits", (DbType)MySqlDbType.VarString, 255, __foruminfo.Replycredits),
									  DbHelper.MakeInParam("?redirect", (DbType)MySqlDbType.VarString, 255, __foruminfo.Redirect),
									  DbHelper.MakeInParam("?attachextensions", (DbType)MySqlDbType.VarString, 255, __foruminfo.Attachextensions),
									  DbHelper.MakeInParam("?moderators", (DbType)MySqlDbType.VarString, 0, __foruminfo.Moderators),
									  DbHelper.MakeInParam("?rules", (DbType)MySqlDbType.VarString, 0, __foruminfo.Rules),
									  DbHelper.MakeInParam("?topictypes", (DbType)MySqlDbType.VarString, 0, __foruminfo.Topictypes),
									  DbHelper.MakeInParam("?viewperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Viewperm),
									  DbHelper.MakeInParam("?postperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Postperm),
									  DbHelper.MakeInParam("?replyperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Replyperm),
									  DbHelper.MakeInParam("?getattachperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Getattachperm),
									  DbHelper.MakeInParam("?postattachperm", (DbType)MySqlDbType.VarString, 0, __foruminfo.Postattachperm)
								  };
		   sql = "Insert Into `" + BaseConfigs.GetTablePrefix + "forumfields` (fid,description,`password`,icon,postcredits,replycredits,redirect,attachextensions,moderators,rules,topictypes,viewperm,postperm,replyperm,getattachperm,postattachperm) Values (?fid,?description,?password,?icon,?postcredits,?replycredits,?redirect,?attachextensions,?moderators,?rules,?topictypes,?viewperm,?postperm,?replyperm,?getattachperm,?postattachperm)";
		   DbHelper.ExecuteDataset(CommandType.Text, sql, prams1);
		   return fid;
	   }

	   public void SetForumsPathList(string pathlist, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?pathlist", (DbType)MySqlDbType.VarString, 3000, pathlist),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET pathlist=?pathlist  WHERE `fid`=?fid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void SetForumslayer(int layer, string parentidlist, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?layer", (DbType)MySqlDbType.Int16, 2, layer),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid),
				DbHelper.MakeInParam("?parentidlist", (DbType)MySqlDbType.String, 300, parentidlist)
			};
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET layer=?layer WHERE `fid`=?fid", prams);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET parentidlist='0' WHERE `fid`=?fid", prams);
	   }

	   public int GetForumsParentidByFid(int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "SELECT `parentid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE fid=?fid LIMIT 1";
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
	   }

	   public void MovingForumsPos(string currentfid, string targetfid, bool isaschildnode, string extname)
	   {
		   //  MySqlConnection conn = new MySqlConnection(DbHelper.ConnectionString);
		   // conn.Open();

		   //  using (SqlTransaction trans = conn.BeginTransaction())
		   //  {
		   // try
		   //{
		   //取得当前论坛版块的信息
		   DataRow dr = DbHelper.ExecuteDataset(CommandType.Text, "Select *  From " + BaseConfigs.GetTablePrefix + "forums Where fid=" + currentfid.ToString() + " LIMIT 1").Tables[0].Rows[0];

		   //取得目标论坛版块的信息
		   DataRow targetdr = DbHelper.ExecuteDataset(CommandType.Text, "Select *  From " + BaseConfigs.GetTablePrefix + "forums Where fid=" + targetfid.ToString() + " LIMIT 1").Tables[0].Rows[0];

		   //当前论坛版块带子版块时
		   if (DbHelper.ExecuteDataset(CommandType.Text,"SELECT `fid` From " + BaseConfigs.GetTablePrefix + "forums Where parentid=" + currentfid.ToString() + " LIMIT 1").Tables[0].Rows.Count > 0)
		   {
			   #region

			   string sqlstring = "";
			   if (isaschildnode) //作为论坛子版块插入
			   {
				   //让位于当前论坛版块(分类)显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
				   sqlstring = string.Format("Update " + BaseConfigs.GetTablePrefix + "forums Set displayorder=displayorder+1 Where `displayorder`>={0}",
					   Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1));
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);

				   //更新当前论坛版块的相关信息
				   sqlstring = string.Format("Update " + BaseConfigs.GetTablePrefix + "forums Set parentid='{1}',displayorder='{2}'  Where `fid`={0}", currentfid, targetdr["fid"].ToString(), Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()) + 1));
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);
			   }
			   else //作为同级论坛版块,在目标论坛版块之前插入
			   {
				   //让位于包括当前论坛版块显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
				   sqlstring = string.Format("Update `" + BaseConfigs.GetTablePrefix + "forums` Set displayorder=displayorder+1 Where `displayorder`>={0} OR `fid`={1}",
					   Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString())),
					   targetdr["fid"].ToString());
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);

				   //更新当前论坛版块的相关信息
				   sqlstring = string.Format("Update " + BaseConfigs.GetTablePrefix + "forums Set parentid='{1}',displayorder='{2}'  Where `fid`={0}", currentfid, targetdr["parentid"].ToString(), Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim())));
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);
			   }

			   //更新由于上述操作所影响的版块数和帖子数
			   if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
			   {
				   if (dr["parentidlist"].ToString().Trim() != "")
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET `topics`=`topics`-" + dr["topics"].ToString() + ",`posts`=`posts`-" + dr["posts"].ToString() + "  WHERE `fid` IN(" + dr["parentidlist"].ToString().Trim() + ")");
				   }
				   if (targetdr["parentidlist"].ToString().Trim() != "")
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET `topics`=`topics`+" + dr["topics"].ToString() + ",`posts`=`posts`+" + dr["posts"].ToString() + "  WHERE `fid` IN(" + targetdr["parentidlist"].ToString().Trim() + ")");
				   }
			   }

			   #endregion
		   }
		   else //当前论坛版块不带子版
		   {
			   #region

			   //设置旧的父一级的子论坛数
			   DbHelper.ExecuteDataset(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set subforumcount=subforumcount-1 Where `fid`=" + dr["parentid"].ToString());

			   //让位于当前节点显示顺序之后的节点全部减1 [起到删除节点的效果]
			   if (isaschildnode) //作为子论坛版块插入
			   {
				   //更新相应的被影响的版块数和帖子数
				   if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET `topics`=`topics`-" + dr["topics"].ToString() + ",`posts`=`posts`-" + dr["posts"].ToString() + "  WHERE `fid` IN(" + dr["parentidlist"].ToString() + ")");
					   if (targetdr["parentidlist"].ToString().Trim() != "")
					   {
						   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET `topics`=`topics`+" + dr["topics"].ToString() + ",`posts`=`posts`+" + dr["posts"].ToString() + "  WHERE `fid` IN(" + targetdr["parentidlist"].ToString() + "," + targetfid + ")");
					   }
				   }

				   //让位于当前论坛版块显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
				   string sqlstring = string.Format("Update " + BaseConfigs.GetTablePrefix + "forums Set displayorder=displayorder+1 Where `displayorder`>={0}",
					   Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1));
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);

				   //设置新的父一级的子论坛数
				   DbHelper.ExecuteDataset(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set subforumcount=subforumcount+1 Where `fid`=" + targetfid);

				   string parentidlist = null;
				   if (targetdr["parentidlist"].ToString().Trim() == "0")
				   {
					   parentidlist = targetfid;
				   }
				   else
				   {
					   parentidlist = targetdr["parentidlist"].ToString().Trim() + "," + targetfid;
				   }

				   //更新当前论坛版块的相关信息
				   sqlstring = string.Format("Update " + BaseConfigs.GetTablePrefix + "forums Set parentid='{1}',layer='{2}',pathlist='{3}', parentidlist='{4}',displayorder='{5}' Where `fid`={0}",
					   currentfid,
					   targetdr["fid"].ToString(),
					   Convert.ToString(Convert.ToInt32(targetdr["layer"].ToString()) + 1),
					   targetdr["pathlist"].ToString().Trim() + "<a href=\"showforum-" + currentfid + extname + "\">" + dr["name"].ToString().Trim() + "</a>",
					   parentidlist,
					   Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()) + 1)
					   );
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);

			   }
			   else //作为同级论坛版块,在目标论坛版块之前插入
			   {
				   //更新相应的被影响的版块数和帖子数
				   if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET `topics`=`topics`-" + dr["topics"].ToString() + ",`posts`=`posts`-" + dr["posts"].ToString() + "  WHERE `fid` IN(" + dr["parentidlist"].ToString() + ")");
					   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET `topics`=`topics`+" + dr["topics"].ToString() + ",`posts`=`posts`+" + dr["posts"].ToString() + "  WHERE `fid` IN(" + targetdr["parentidlist"].ToString() + ")");
				   }

				   //让位于包括当前论坛版块显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
				   string sqlstring = string.Format("Update `" + BaseConfigs.GetTablePrefix + "forums` Set displayorder=displayorder+1 Where `displayorder`>={0} OR `fid`={1}",
					   Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1),
					   targetdr["fid"].ToString());
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);

				   //设置新的父一级的子论坛数
				   DbHelper.ExecuteDataset(CommandType.Text, "Update `" + BaseConfigs.GetTablePrefix + "forums`  Set subforumcount=subforumcount+1 Where `fid`=" + targetdr["parentid"].ToString());
				   string parentpathlist = "";
				   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `pathlist` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid`=" + targetdr["parentid"].ToString() + " LIMIT 1").Tables[0];
				   if (dt.Rows.Count > 0)
				   {
					   parentpathlist = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `pathlist` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid`=" + targetdr["parentid"].ToString() + " LIMIT 1").Tables[0].Rows[0][0].ToString().Trim();
				   }

				   //更新当前论坛版块的相关信息
				   sqlstring = string.Format("Update `" + BaseConfigs.GetTablePrefix + "forums` SET parentid='{1}',layer='{2}',pathlist='{3}', parentidlist='{4}',displayorder='{5}' Where `fid`={0}",
					   currentfid,
					   targetdr["parentid"].ToString(),
					   Convert.ToInt32(targetdr["layer"].ToString()),
					   parentpathlist + "<a href=\"showforum-" + currentfid + extname + "\">" + RegEsc(dr["name"].ToString().Trim()) + "</a>",
					   targetdr["parentidlist"].ToString().Trim(),
					   Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()))
					   );
				   DbHelper.ExecuteDataset(CommandType.Text, sqlstring);
			   }

			   #endregion
		   }
		   //      trans.Commit();
		   //   }

		   //  catch (Exception ex)
		   //   {
		   //      trans.Rollback();
		   //      throw ex;
		   //   }
		   // conn.Close();
		   // }
	   }

	   public bool IsExistSubForum(int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "Select * From " + BaseConfigs.GetTablePrefix + "forums Where parentid=?fid LIMIT 1";
		   if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count > 0)
			   return true;
		   else
			   return false;
	   }

	   public void DeleteForumsByFid(string postname, string fid)
	   {
		   MySqlConnection conn = new MySqlConnection(DbHelper.ConnectionString);
		   conn.Open();
		   using (MySqlTransaction trans = conn.BeginTransaction())
		   {
			   try
			   {


				   //先取出当前节点的信息
				   DataRow dr = DbHelper.ExecuteDataset(trans,CommandType.Text, "Select * From " + BaseConfigs.GetTablePrefix + "forums Where fid=" + fid.ToString() + "").Tables[0].Rows[0];

				   //调整在当前节点排序位置之后的节点,做减1操作
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set displayorder=displayorder-1 Where  displayorder>" + dr["displayorder"].ToString());

				   //修改父结点中的子论坛个数
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set subforumcount=subforumcount-1 Where  fid=" + dr["parentid"].ToString());

				   //删除当前节点的高级属性部分
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "Delete From " + BaseConfigs.GetTablePrefix + "forumfields Where  fid=" + fid);

				   //删除相关投票的信息
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid` IN(SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=" + fid + ")");

				   //删除帖子附件表中的信息
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `tid` IN(SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=" + fid + ") OR `pid` IN(SELECT `pid` FROM `" + postname + "` WHERE `fid`=" + fid + ")");

				   //删除相关帖子
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM `" + postname + "` WHERE `fid`=" + fid);

				   //删除相关主题
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=" + fid);


				   //删除当前节点
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "forums` Where  `fid`=" + fid);

				   //删除版主列表中的相关信息
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "moderators` Where  `fid`=" + fid);

				   trans.Commit();
			   }
			   catch
			   {
				   trans.Rollback();
			   }

		   }
		   conn.Close();
	   }

	   public DataTable GetParentIdByFid(int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "SELECT `parentid` From `" + BaseConfigs.GetTablePrefix + "forums` WHERE `inheritedmod`=1 AND `fid`=?fid";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
	   {
		   //SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   //conn.Open();
		   //using (SqlTransaction trans = conn.BeginTransaction())
		   //{
		   // try
		   //{
		   int count = displayorder;


		   //数据库中存在的用户
		   string usernamelist = "";
		   //清除已有论坛的版主设置
		   foreach (string username in moderators.Split(','))
		   {
			   if (username.Trim() != "")
			   {
				   IDataParameter[] prams =
								{
									DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarString, 20, username.Trim())
								};
				   //先取出当前节点的信息
				   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "Select `uid` From `" + BaseConfigs.GetTablePrefix + "users` Where `groupid`<>7 AND `groupid`<>8 AND `username`=?username LIMIT 1", prams).Tables[0];
				   if (dt.Rows.Count > 0)
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "moderators` (uid,fid,displayorder,inherited) VALUES(" + dt.Rows[0][0].ToString() + "," + fid + "," + count.ToString() + "," + inherited.ToString() + ")");
					   usernamelist = usernamelist + username.Trim() + ",";
					   count++;
				   }
			   }
		   }

		   if (usernamelist != "")
		   {
			   IDataParameter[] prams1 =
							{
								DbHelper.MakeInParam("?moderators", (DbType)MySqlDbType.VarString, 255, usernamelist.Substring(0, usernamelist.Length - 1))

							};
			   DbHelper.ExecuteNonQuery(CommandType.Text, "Update `" + BaseConfigs.GetTablePrefix + "forumfields` SET `moderators`=?moderators  WHERE `fid` =" + fid, prams1);
		   }
		   else
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "Update `" + BaseConfigs.GetTablePrefix + "forumfields` SET `moderators`='' WHERE `fid` =" + fid);
		   }

		   //  trans.Commit();
		   //}
		   //catch (Exception ex)
		   //{
		   //  trans.Rollback();
		   //    throw ex;
		   //  }
		   //}
		   // conn.Close();
	   }

	   public DataTable GetFidInForumsByParentid(int parentid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?parentid", (DbType)MySqlDbType.Int32, 4, parentid)
			};
		   string sql = "Select fid From `" + BaseConfigs.GetTablePrefix + "forums` Where `parentid`=?parentid ORDER BY `displayorder` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void CombinationForums(string sourcefid, string targetfid, string fidlist)
	   {
		   //SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   //conn.Open();
		   //using (SqlTransaction trans = conn.BeginTransaction())
		   //{
		   //  try
		   //{
		   //ChildNode = "0";
		   //string fidlist = ("," + FindChildNode(targetfid)).Replace(",0,", "");
		   //更新帖子与主题的信息
		   DbHelper.ExecuteNonQuery( CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `fid`=" + targetfid + "  WHERE `fid`=" + sourcefid);
		   //要更新目标论坛的主题数
		   int totaltopics = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(tid)  FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid` IN(" + fidlist + ")").Tables[0].Rows[0][0].ToString());

		   int totalposts = 0;
		   foreach (DataRow postdr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT `id` From `" + BaseConfigs.GetTablePrefix + "tablelist`").Tables[0].Rows)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "posts" + postdr["id"].ToString() + "` SET `fid`=" + targetfid + "  WHERE `fid`=" + sourcefid);

			   //要更新目标论坛的帖子数
			   totalposts = totalposts + Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(pid)  FROM `" + BaseConfigs.GetTablePrefix + "posts" + postdr["id"].ToString() + "` WHERE `fid` IN(" + fidlist + ")").Tables[0].Rows[0][0].ToString());
		   }

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `topics`=" + totaltopics + " ,`posts`=" + totalposts + " WHERE `fid`=" + targetfid);

		   //获取源论坛信息
		   DataRow dr = DbHelper.ExecuteDataset(CommandType.Text, "Select * From " + BaseConfigs.GetTablePrefix + "forums Where fid=" + sourcefid.ToString() + " LIMIT 1").Tables[0].Rows[0];

		   //调整在当前节点排序位置之后的节点,做减1操作
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set displayorder=displayorder-1 Where  displayorder>" + dr["displayorder"].ToString());

		   //修改父结点中的子论坛个数
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forums Set subforumcount=subforumcount-1 Where  fid=" + dr["parentid"].ToString());

		   //删除当前节点的高级属性部分
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From " + BaseConfigs.GetTablePrefix + "forumfields Where  fid=" + sourcefid);

		   //删除源论坛版块
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From " + BaseConfigs.GetTablePrefix + "forums Where  fid=" + sourcefid);
		   //    trans.Commit();

		   //     }
		   //   catch (Exception ex)
		   // {
		   //   trans.Rollback();
		   // throw ex;
		   //}
		   // }
		   //conn.Close();
	   }

	   public void UpdateSubForumCount(int subforumcount, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?subforumcount", (DbType)MySqlDbType.Int32, 4, subforumcount),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `subforumcount`=?subforumcount WHERE `fid`=?fid";
		   DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
	   }

	   public void UpdateDisplayorderInForumByFid(int displayorder, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `displayorder`=?displayorder WHERE `fid`=?fid";
		   DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
	   }

	   public DataTable GetMainForum()
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `layer`=0 Order By `displayorder` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public void SetStatusInForum(int status, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?status", (DbType)MySqlDbType.Int32, 4, status),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `status`=?status WHERE `fid`=?fid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataTable GetForumByParentid(int parentid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?parentid", (DbType)MySqlDbType.Int32, 4, parentid)
			};
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `parentid`=?parentid Order By DisplayOrder";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void UpdateStatusByFidlist(string fidlist)
	   {
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `status`=0 WHERE `fid` IN(" + fidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public void UpdateStatusByFidlistOther(string fidlist)
	   {
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `status`=1 WHERE `status`>1 AND `fid` IN(" + fidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public bool BatchSetForumInf(ForumInfo __foruminfo, BatchSetParams bsp, string fidlist)
	   {
		   StringBuilder forums = new StringBuilder();
		   StringBuilder forumfields = new StringBuilder();

		   forums.Append("UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET ");
		   if (bsp.SetSetting)
		   {
			   forums.Append("`Allowsmilies`='" + __foruminfo.Allowsmilies + "' ,");
			   forums.Append("`Allowrss`='" + __foruminfo.Allowrss + "' ,");
			   forums.Append("`Allowhtml`='" + __foruminfo.Allowhtml + "' ,");
			   forums.Append("`Allowbbcode`='" + __foruminfo.Allowbbcode + "' ,");
			   forums.Append("`Allowimgcode`='" + __foruminfo.Allowimgcode + "' ,");
			   forums.Append("`Allowblog`='" + __foruminfo.Allowblog + "' ,");
			   forums.Append("`Allowtrade`='" + __foruminfo.Allowtrade + "' ,");
			   forums.Append("`Alloweditrules`='" + __foruminfo.Alloweditrules + "' ,");
			   forums.Append("`allowthumbnail`='" + __foruminfo.Allowthumbnail + "' ,");
			   forums.Append("`Recyclebin`='" + __foruminfo.Recyclebin + "' ,");
			   forums.Append("`Modnewposts`='" + __foruminfo.Modnewposts + "' ,");
			   forums.Append("`Jammer`='" + __foruminfo.Jammer + "' ,");
			   forums.Append("`Disablewatermark`='" + __foruminfo.Disablewatermark + "' ,");
			   forums.Append("`Inheritedmod`='" + __foruminfo.Inheritedmod + "' ,");
		   }
		   if (forums.ToString().EndsWith(","))
		   {
			   forums.Remove(forums.Length - 1, 1);
		   }
		   forums.Append("WHERE `fid` IN(" + fidlist + ")");


		   forumfields.Append("UPDATE `" + BaseConfigs.GetTablePrefix + "forumfields` SET ");

		   if (bsp.SetPassWord)
		   {
			   forumfields.Append("`password`='" + __foruminfo.Password + "' ,");
		   }

		   if (bsp.SetAttachExtensions)
		   {
			   forumfields.Append("`attachextensions`='" + __foruminfo.Attachextensions + "' ,");
		   }

		   if (bsp.SetPostCredits)
		   {
			   forumfields.Append("`postcredits`='" + __foruminfo.Postcredits + "' ,");
		   }

		   if (bsp.SetReplyCredits)
		   {
			   forumfields.Append("`replycredits`='" + __foruminfo.Replycredits + "' ,");
		   }


		   if (bsp.SetViewperm)
		   {
			   forumfields.Append("`Viewperm`='" + __foruminfo.Viewperm + "' ,");
		   }

		   if (bsp.SetPostperm)
		   {
			   forumfields.Append("`Postperm`='" + __foruminfo.Postperm + "' ,");
		   }

		   if (bsp.SetReplyperm)
		   {
			   forumfields.Append("`Replyperm`='" + __foruminfo.Replyperm + "' ,");
		   }

		   if (bsp.SetGetattachperm)
		   {
			   forumfields.Append("`Getattachperm`='" + __foruminfo.Getattachperm + "' ,");
		   }

		   if (bsp.SetPostattachperm)
		   {
			   forumfields.Append("`Postattachperm`='" + __foruminfo.Postattachperm + "' ,");
		   }

		   if (forumfields.ToString().EndsWith(","))
		   {
			   forumfields.Remove(forumfields.Length - 1, 1);
		   }

		   forumfields.Append("WHERE `fid` IN(" + fidlist + ")");


		   // SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   //  conn.Open();
		   //using (SqlTransaction trans = conn.BeginTransaction())
		   //  {
		   // try
		   // {
		   if (forums.ToString().IndexOf("SET WHERE") < 0)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, forums.ToString());
		   }

		   if (forumfields.ToString().IndexOf("SET WHERE") < 0)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, forumfields.ToString());
		   }
		   // trans.Commit();
		   // }
		   //   catch
		   //  {
		   //    trans.Rollback();
		   //    return false;
		   //  }
		   // }
		   return true;
	   }

	   public IDataReader GetTopForumFids(int lastfid, int statcount)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?lastfid", (DbType)MySqlDbType.Int32, 4, lastfid),
		   };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid` > ?lastfid LIMIT " + statcount.ToString(), prams);
	   }

	   public DataSet GetOnlineList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid`,(SELECT `grouptitle`  FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `" + BaseConfigs.GetTablePrefix + "usergroups`.`groupid`=`" + BaseConfigs.GetTablePrefix + "onlinelist`.`groupid` LIMIT 1) As GroupName , `displayorder`,`title`,`img` From `" + BaseConfigs.GetTablePrefix + "onlinelist` Order BY `groupid` ASC");
	   }

	   public int UpdateOnlineList(int groupid, int displayorder, string img, string title)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 50, title),
									 DbHelper.MakeInParam("?img", (DbType)MySqlDbType.VarString, 50, img),
									 DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid)

								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "onlinelist` SET `displayorder`=?displayorder,`title`=?title,`img`=?img  Where `groupid`=?groupid";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public string GetWords()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "words`";
	   }

	   public int DeleteWord(int id)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id);

		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "words` WHERE `id`=?id";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public int UpdateWord(int id, string find, string replacement)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?find", (DbType)MySqlDbType.VarString, 255, find),
									 DbHelper.MakeInParam("?replacement", (DbType)MySqlDbType.VarString, 255, replacement),
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id)
								 };

		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "words` SET `find`=?find, `replacement`=?replacement  Where `id`=?id";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public int DeleteWords(string idlist)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "words`  WHERE `ID` IN(" + idlist + ")");
	   }

	   public bool ExistWord(string find)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?find", (DbType)MySqlDbType.VarString, 255, find);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "words` WHERE `find`=?find LIMIT 1", parm).Tables[0].Rows.Count > 0;
	   }

	   public int AddWord(string username, string find, string replacement)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarString, 20, username),
									 DbHelper.MakeInParam("?find", (DbType)MySqlDbType.VarString, 255, find),
									 DbHelper.MakeInParam("?replacement", (DbType)MySqlDbType.VarString, 255, replacement)
								 };
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "words` (`admin`, `find`, `replacement`) VALUES (?username,?find,?replacement)";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public bool IsExistTopicType(string typename,int currenttypeid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?typename", (DbType)MySqlDbType.VarString, 30, typename),
									 DbHelper.MakeInParam("?currenttypeid", (DbType)MySqlDbType.Int32, 4, currenttypeid)
								 };
		   string sql = "SELECT typeid FROM `" + BaseConfigs.GetTablePrefix + "topictypes` WHERE name=?typename AND typeid<>?currenttypeid";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
	   }

	   public bool IsExistTopicType(string typename)
	   {
		   IDataParameter parms = DbHelper.MakeInParam("?typename", (DbType)MySqlDbType.VarString, 30, typename);
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topictypes` WHERE `name`=?typename LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
	   }

	   public string GetTopicTypes()
	   {
		   return "SELECT typeid as id,name,displayorder,description FROM `" + BaseConfigs.GetTablePrefix + "topictypes` ORDER BY `displayorder` ASC";
	   }

	   public DataTable GetExistTopicTypeOfForum()
	   {
		   string sql = "SELECT `fid`,`topictypes` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` WHERE `topictypes` NOT LIKE ''";
		   return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
	   }

	   public void UpdateTopicTypeForForum(string topictypes,int fid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?topictypes", (DbType)MySqlDbType.VarString, 0, topictypes),
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "forumfields` SET `topictypes`=?topictypes WHERE `fid`=?fid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql,parms);
	   }

	   public void UpdateTopicTypes(string name, int displayorder,string description,int typeid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString,100, name),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32,4,displayorder),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString,500,description),
									 DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int32,4,typeid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "topictypes` SET `name`=?name ,`displayorder`=?displayorder, `description`=?description Where `typeid`=?typeid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void AddTopicTypes(string typename, int displayorder, string description)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?name",(DbType)MySqlDbType.VarString,100, typename),
									 DbHelper.MakeInParam("?displayorder",(DbType)MySqlDbType.Int32,4,displayorder),
									 DbHelper.MakeInParam("?description",(DbType)MySqlDbType.VarString,500,description)
								 };
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "topictypes` (`name`,`displayorder`,`description`) VALUES(?name,?displayorder,?description)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void DeleteTopicTypesByTypeidlist(string typeidlist)
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topictypes`  WHERE `typeid` IN(" + typeidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text,sql);
	   }

	   public DataTable GetForumNameIncludeTopicType()
	   {
		   string sql = "SELECT f1.`fid`,`name`,`topictypes` FROM `" + BaseConfigs.GetTablePrefix + "forums` AS f1 LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forumfields` AS f2 ON f1.fid=f2.fid";
		   return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
	   }

	   public DataTable GetForumTopicType()
	   {
		   string sql = "SELECT `fid`,`topictypes` FROM `" + BaseConfigs.GetTablePrefix + "forumfields`";
		   return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
	   }

	   public void ClearTopicTopicType(int typeid)
	   {
		   IDataParameter pram = DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int32, 4, typeid);
		   string sql = "Update " + BaseConfigs.GetTablePrefix + "topics Set `typeid`=0 Where typeid=?typeid";
		   DbHelper.ExecuteNonQuery(CommandType.Text,sql,pram);
	   }

	   public string GetTopicTypeInfo()
	   {
		   return "SELECT typeid as id,name,description FROM `" + BaseConfigs.GetTablePrefix + "topictypes` ORDER BY `displayorder` ASC";
	   }

	   public string GetTemplateName()
	   {
		   return "Select templateid,name From `" + BaseConfigs.GetTablePrefix + "templates`";
	   }

	   public DataTable GetAttachType()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "Select id,extension  From `" + BaseConfigs.GetTablePrefix + "attachtypes`  Order By `id` ASC").Tables[0];
	   }

	   public void UpdatePermUserListByFid(string permuserlist, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?permuserlist", (DbType)MySqlDbType.VarString,0,permuserlist),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid)
			};
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Update " + BaseConfigs.GetTablePrefix + "forumfields Set Permuserlist=?permuserlist Where fid=?fid", prams);
	   }


	   public IDataReader GetTopicsIdentifyItem()
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topicidentify`");
	   }

	   public string ResetTopTopicListSql(int layer, string fid, string parentidlist)
	   {

		   string filterexpress = "";

		   switch (layer)
		   {
			   case 0:
				   filterexpress = string.Format("`fid`<>{0} AND (',' + TRIM(`parentidlist`) + ',' LIKE '%,{1},%')", fid.ToString(), RegEsc(fid.ToString()));
				   break;
			   case 1:
				   filterexpress = parentidlist.ToString().Trim();
				   if (filterexpress != string.Empty)
				   {
					   filterexpress =
						   string.Format(
						   "`fid`<>{0} AND (`fid`={1} OR (',' + TRIM(`parentidlist`) + ',' LIKE '%,{2},%'))",
						   fid.ToString().Trim(), filterexpress, RegEsc(filterexpress));
				   }
				   else
				   {
					   filterexpress =
						   string.Format(
						   "`fid`<>{0} AND (',' + TRIM(`parentidlist`) + ',' LIKE '%,{1},%')",
						   fid.ToString().Trim(), RegEsc(filterexpress));
				   }
				   break;
			   default:
				   filterexpress = parentidlist.ToString().Trim();
				   if (filterexpress != string.Empty)
				   {
					   filterexpress = Utils.CutString(filterexpress, 0, filterexpress.IndexOf(","));
					   filterexpress =
						   string.Format(
						   "`fid`<>{0} AND (`fid`={1} OR (',' + TRIM(`parentidlist`) + ',' LIKE '%,{2},%'))",
						   fid.ToString().Trim(), filterexpress, RegEsc(filterexpress));
				   }
				   else
				   {
					   filterexpress =
						   string.Format(
						   "`fid`<>{0} AND (',' + TRIM(`parentidlist`) + ',' LIKE '%,{1},%')",
						   fid.ToString().Trim(), RegEsc(filterexpress));
				   }
				   break;
		   }

		   return filterexpress;
	   }


	   public string showforumcondition(int sqlid,int cond)
        
	   {
		   string sql = null;
		   switch (sqlid)
		   {
			   case 1:
				   sql=" AND `typeid`=";
				   break;
			   case 2:
				   sql = " AND `postdatetime`>='" + DateTime.Now.AddDays(-1 * cond).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				   break;

			   case 3:
				   sql = " `tid`";
				   break;
                
        
		   }
		   return sql;
        
	   }

	   public string DelVisitLogCondition(string deletemod,string visitid,string deletenum,string deletefrom)
	   {
		   string condition="";
		   switch (deletemod)
		   {
			   case "chkall":
				   if (visitid != "")
					   condition = " `visitid` IN(" + visitid + ")";
				   break;
			   case "deleteNum":
				   if (deletenum != "" && Utils.IsNumeric(deletenum))
					   condition = " `visitid` not in (select `visitid` from `" + BaseConfigs.GetTablePrefix + "adminvisitlog` order by `visitid` desc limit 0,"+deletenum+")";
				   break;
			   case "deleteFrom":
				   if (deletefrom != "")
					   condition = " `postdatetime`<'" + deletefrom + "'";
				   break;
		   }
		   return condition;
	   }


	   public string AttachDataBind(string condition,string postname)
	   {


		   return "Select `aid`, `attachment`, `filename`, (Select `poster` FROM `" + postname + "` WHERE `" + postname + "`.`pid`=`" + BaseConfigs.GetTablePrefix + "attachments`.`pid` LIMIT 1) AS `poster`,(Select `title` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid`=`" + BaseConfigs.GetTablePrefix + "attachments`.`tid` LIMIT 1) AS `topictitle`, `filesize`,`downloads`  From `" + BaseConfigs.GetTablePrefix + "attachments` " + condition;
	   }

	   public DataTable GetAttachDataTable(string condition, string postname)
	   {
		   string sqlstring = "Select `aid`, `attachment`, `filename`, (Select `poster` FROM `" + postname + "` WHERE `" + postname + "`.`pid`=`" + BaseConfigs.GetTablePrefix + "attachments`.`pid` LIMIT 1) AS `poster`,(Select `title` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid`=`" + BaseConfigs.GetTablePrefix + "attachments`.`tid` LIMIT 1) AS `topictitle`, `filesize`,`downloads`  From `" + BaseConfigs.GetTablePrefix + "attachments` " + condition;
		   return DbHelper.ExecuteDataset(sqlstring).Tables[0];
	   }


	   public bool AuditTopicCount(string condition)
	   {

		   if (DbHelper.ExecuteDataset("Select count(tid) From `" + BaseConfigs.GetTablePrefix + "topics` WHERE " +condition).Tables[0].Rows[0][0].ToString() == "0")
		   {
			   return true;
		   }
		   else

		   {
			   return false;
		   }
        
        
	   }

	   public string AuditTopicBindStr(string condition)
	   {


		   return "Select * From `" + BaseConfigs.GetTablePrefix + "topics` WHERE " + condition;
	   }

	   public DataTable AuditTopicBind(string condition)
	   {

		   DataTable dt =DbHelper.ExecuteDataset("Select * From `" + BaseConfigs.GetTablePrefix + "topics` WHERE " + condition).Tables[0];
		   return dt;
	   }

	   public string AuditNewUserClear(string regbefore,string regip)
	   {
		   string sqlstring = "";
		   sqlstring += " `groupid`=8";
		   if (regbefore != "")
		   {
			   sqlstring += " AND `joindate`<='" + DateTime.Now.AddDays(-Convert.ToInt32(regbefore)).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
		   }

		   if (regip != "")
		   {
			   sqlstring += " AND `regip` Like '" + RegEsc(regip) + "%'";
		   }

		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE " + sqlstring;
	   }


	   public string DelMedalLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
	   {
		   string condition = "";
		   switch (deletemode)
		   {
			   case "chkall":
				   if (id != "")
					   condition = " `id` IN(" + id + ")";
				   break;
			   case "deleteNum":
				   if (deleteNum != "" && Utils.IsNumeric(deleteNum))
					   condition = " `id` not in (select `id` from `" + BaseConfigs.GetTablePrefix + "medalslog` order by `id` desc LIMIT 0," + deleteNum + ")";
				   break;
			   case "deleteFrom":
				   if (deleteFrom != "")
					   condition = " `postdatetime`<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				   break;
		   }
		   return condition;
        
	   }

	   public DataTable MedalsTable(string medalid)
	   {

		   DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM `" + BaseConfigs.GetTablePrefix + "medals` WHERE `medalid`=" + medalid).Tables[0];
		   return dt;
	   }

	   public string DelModeratorManageCondition(string deletemode, string id, string deleteNum, string deleteFrom)
	   {
		   string condition = "";
		   switch (deletemode)
		   {
			   case "chkall":
				   if (id != "")
					   condition = " `id` IN(" + id + ")";
				   break;
			   case "deleteNum":
				   if (deleteNum != "" && Utils.IsNumeric(deleteNum))
					   condition = " `id` not in (select `id` from `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` order by `id` desc LIMIT 0," + deleteNum + ")";
				   break;
			   case "deleteFrom":
				   if (deleteFrom != "")
					   condition = " `postdatetime`<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				   break;
		   }
		   return condition;
	   }

	   public DataTable GroupNameTable(string groupid)
	   {
		   DataTable dt = DbHelper.ExecuteDataset("SELECT `grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=" + groupid + " LIMIT 1").Tables[0];
		   return dt;
	   }

	   public string PaymentLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
	   {
		   string condition = "";
		   switch (deletemode)
		   {
			   case "chkall":
				   if (id != "")
					   condition = " `id` IN(" + id + ")";
				   break;
			   case "deleteNum":
				   if (deleteNum != "" && Utils.IsNumeric(deleteNum))
					   condition = " `id` not in (select `id` from `" + BaseConfigs.GetTablePrefix + "paymentlog` order by `id` desc LIMIT 0," + deleteNum + ")";
				   break;
			   case "deleteFrom":
				   if (deleteFrom != "")
					   condition = " `buydate`<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				   break;
		   }
		   return condition;
	   }

	   public string PostGridBind(string posttablename, string condition)
	   {


		   return "Select * From `" + posttablename + "` WHERE " + condition.ToString();
	   }

	   public string DelRateScoreLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
	   {
		   string condition = "";
		   switch (deletemode)
		   {
			   case "chkall":
				   if (id != "")
					   condition = " `id` IN(" + id + ")";
				   break;
			   case "deleteNum":
				   if (deleteNum != "" && Utils.IsNumeric(deleteNum))
					   condition = " `id` not in (select `id` from `" + BaseConfigs.GetTablePrefix + "ratelog` order by `id` desc LIMIT 0," + deleteNum + ")";
				   break;
			   case "deleteFrom":
				   if (deleteFrom != "")
					   condition = " `postdatetime`<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				   break;
		   }
		   return condition;
	   }

	   //MYSQL不支持该方法
	   public void UpdatePostSP()
	   {
	   }

	   //MYSQL不支持该方法
	   public void CreateStoreProc(int tablelistmaxid)
	   {
	   }

	   public void UpdateMyTopic()
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "mytopics`";//清空我的主题表
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		   //重建我的主题表
		   sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "mytopics`(`uid`, `tid`, `dateline`) SELECT `posterid`,`tid`,`postdatetime` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `posterid`<>-1";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public void UpdateMyPost()
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "myposts`";//清空我的帖子表
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);


		   IDataReader DDR = DbHelper.ExecuteReader(CommandType.Text, "SELECT `id` from `" + BaseConfigs.GetTablePrefix + "tablelist`");
		   while (DDR.Read())
		   {
			   string tempsql = "SELECT `posterid`,`tid`,`pid`,`postdatetime` FROM `" + BaseConfigs.GetTablePrefix + "posts" + DDR["id"].ToString().Trim()+ "` WHERE `posterid`<>-1";
			   IDataReader dr=DbHelper.ExecuteReader(CommandType.Text,tempsql);
			   while(dr.Read())
			   {
				   DbHelper.ExecuteNonQuery("INSERT INTO `"+BaseConfigs.GetTablePrefix+"myposts`(`uid`, `tid`, `pid`, `dateline`) values(" + dr["posterid"].ToString() + "," + dr["tid"].ToString() + "," + dr["pid"].ToString() + ",'" + dr["postdatetime"].ToString() + "')");
                
			   }
			   dr.Close();
            
		   }
		   DDR.Close();
            
	   }


	   public string GetAllIdentifySql()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topicidentify`";
	   }

	   public DataTable GetAllIdentify()
	   {
		   string sql = GetAllIdentifySql();
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public bool UpdateIdentifyById(int id, string name)
	   {
		   IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarChar,50, name),
				DbHelper.MakeInParam("?identifyid", (DbType)MySqlDbType.Int32,4,id)
				
			};
		   string sql = "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "topicidentify` WHERE `name`=?name AND `identifyid`<>?identifyid";
		   if (int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString()) != 0)  //有相同的名称存在，更新失败
		   {
			   return false;
		   }
		   sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "topicidentify` SET `name`=?name WHERE `identifyid`=?identifyid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		   return true;
	   }

	   public bool AddIdentify(string name, string filename)
	   {
		   IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarChar,50, name),
				DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.VarChar,50,filename),
		   };
		   string sql = "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "topicidentify` WHERE `name`=?name";
		   if (int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString()) != 0)  //有相同的名称存在，插入失败
		   {
			   return false;
		   }
		   sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "topicidentify` (`name`,`filename`) VALUES(?name,?filename)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		   return true;
	   }

	   public void DeleteIdentify(string idlist)
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topicidentify` WHERE `identifyid` IN (" + idlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public DataTable GetUserGroupMaxspacephotosize()
	   {
		   string sql = "SELECT `groupid`,`grouptitle`,`maxspacephotosize` FROM `" + BaseConfigs.GetTablePrefix + "usergroups`  ORDER BY `groupid` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetUserGroupMaxspaceattachsize()
	   {
		   string sql = "SELECT `groupid`,`grouptitle`,`maxspaceattachsize` FROM `" + BaseConfigs.GetTablePrefix + "usergroups`  ORDER BY `groupid` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public int GetMaxTopicTypesId()
	   {
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT MAX(`typeid`) FROM `" + BaseConfigs.GetTablePrefix + "topictypes`").ToString());
	   }

	   /// <summary>
	   /// 获取非默认模板数
	   /// </summary>
	   /// <returns></returns>
	   public int GetSpecifyForumTemplateCount()
	   {
		   string sql = "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `templateid` <> 0 AND `templateid` <> " + GeneralConfigs.GetDefaultTemplateID();
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
	   }

	   public IDataReader GetAttachmentByUid(int uid, string extlist, int pageIndex, int pageSize)
	   {
		   IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4,uid),
				DbHelper.MakeInParam("?extlist ", (DbType)MySqlDbType.VarChar,100,extlist),
				DbHelper.MakeInParam("?pageindex", (DbType)MySqlDbType.Int32,4,pageIndex),
				DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,pageSize)
			};
		   string sql = "";
          

		   sql = "select *  from `" + BaseConfigs.GetTablePrefix + "myattachments` where  `extname` in (?extlist) and `uid`=?uid order by `aid` desc limit " + (pageIndex - 1) * pageSize + ","+pageSize;
		   IDataReader idr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
		   return idr;
	   }

	   public int GetUserAttachmentCount(int uid)
	   {
		   IDataParameter [] prams = 
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4,uid)
			};
		   string sql = string.Format("SELECT COUNT(1) FROM `{0}` WHERE `UID`=?uid", BaseConfigs.GetTablePrefix + "myattachments");
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString());
           
	   }

	   public int GetUserAttachmentCount(int uid, string extlist)
	   {


		   IDataParameter [] prams = 
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4,uid),
			   //DbHelper.MakeInParam("?extlist", (DbType)SqlDbType.VarChar,100,extlist)
		   };
		   //string sql = string.Format("select count(1) from {0} where exists (select * from {1}split(?extlist,',') where charindex(item, attachment)>0) and `UID`=?uid", BaseConfigs.GetTablePrefix + "myattachments",BaseConfigs.GetTablePrefix);
		   string sql = string.Format("select count(1) from {0} where `extname` IN (" + extlist + ") and `UID`=?uid", BaseConfigs.GetTablePrefix + "myattachments", BaseConfigs.GetTablePrefix);

		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString());

	   }

	   public IDataReader GetAttachmentByUid(int uid, int pageIndex, int pageSize)
	   {
		   IDataParameter [] prams = 
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4,uid),
				DbHelper.MakeInParam("?pageindex", (DbType)MySqlDbType.Int32,4,pageIndex),
				DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,pageSize)
			};

		   string sql = "";
 

		   sql = "select *  from `" + BaseConfigs.GetTablePrefix + "myattachments`  where  `uid`=?uid  order by `aid` desc limit " + (pageIndex - 1) * pageSize+","+pageSize;

		   IDataReader idr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
		   return idr;
	   }

	   public void DelMyAttachmentByTid(string tidlist)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}myattachments` WHERE `tid` IN ({1})", BaseConfigs.GetTablePrefix, tidlist));

	   }

	   public void DelMyAttachmentByPid(string pidlist)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}myattachments` WHERE `pid` IN ({1})", BaseConfigs.GetTablePrefix, pidlist));

	   }

	   public void DelMyAttachmentByAid(string aidlist)
	   {

		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}myattachments` WHERE `aid` IN ({1})", BaseConfigs.GetTablePrefix, aidlist));
	   }

	   #endregion


	   #region GlobalManage

	   private string GetSqlstringByPostDatetime(string sqlstring, DateTime postdatetimeStart, DateTime postdatetimeEnd)
	   {
		   //日期需要改成参数，以后需要重构！需要先修改后台传递参数方式
		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " AND `postdatetime`>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " AND `postdatetime`<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }
		   return sqlstring;
	   }


       
       
	   public DataTable GetAdsTable()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `advid`, `type`, `displayorder`, `targets`, `parameters`, `code` FROM `" + BaseConfigs.GetTablePrefix + "advertisements` WHERE `available`=1 AND `starttime` <='" + DateTime.Now.ToShortDateString().ToString() + "' AND `endtime` >='" + DateTime.Now.ToShortDateString().ToString() + "' ORDER BY `displayorder` DESC, `advid` DESC").Tables[0];
	   }

	   /// <summary>
	   /// 获得全部指定时间段内的公告列表
	   /// </summary>
	   /// <param name="starttime">开始时间</param>
	   /// <param name="endtime">结束时间</param>
	   /// <returns>公告列表</returns>
	   public DataTable GetAnnouncementList(string starttime, string endtime)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?starttime",(DbType)MySqlDbType.VarChar,19,starttime),
									 DbHelper.MakeInParam("?endtime",(DbType)MySqlDbType.VarChar,19,endtime)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "announcements` WHERE `starttime` <=?starttime AND `endtime` >=?starttime ORDER BY `displayorder`, `id` DESC", prams).Tables[0];
	   }

	   /// <summary>
	   /// 获得全部指定时间段内的前n条公告列表
	   /// </summary>
	   /// <param name="starttime">开始时间</param>
	   /// <param name="endtime">结束时间</param>
	   /// <param name="maxcount">最大记录数,小于0返回全部</param>
	   /// <returns>公告列表</returns>
	   public DataTable GetSimplifiedAnnouncementList(string starttime, string endtime, int maxcount)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?starttime",(DbType)MySqlDbType.VarChar,19,starttime),
									 DbHelper.MakeInParam("?endtime",(DbType)MySqlDbType.VarChar,19,endtime)
								 };
		   string topstr = " LIMIT " + maxcount;
		   if (maxcount < 0)
		   {
			   topstr = "";
		   }
		   string sqlstr = "SELECT `id`, `title`, `poster`, `posterid`,`starttime` FROM `" + BaseConfigs.GetTablePrefix + "announcements` WHERE `starttime` <=?starttime AND `endtime` >=?starttime ORDER BY `displayorder`, `id` DESC" + topstr;

		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, prams).Tables[0];
	   }


	   public int AddAnnouncement(string poster, int posterid, string title, int displayorder, string starttime, string endtime, string message)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarChar, 20, poster),
									 DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, posterid),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 250, title),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?starttime", (DbType)MySqlDbType.Datetime, 8, starttime),
									 DbHelper.MakeInParam("?endtime", (DbType)MySqlDbType.Datetime, 8, endtime),
									 DbHelper.MakeInParam("?message", (DbType)MySqlDbType.VarChar, 16, message)
								 };
		   string sqlstring = "INSERT INTO  `" + BaseConfigs.GetTablePrefix + "announcements` (`poster`,`posterid`,`title`,`displayorder`,`starttime`,`endtime`,`message`) VALUES(?poster, ?posterid, ?title, ?displayorder, ?starttime, ?endtime, ?message)";
		   //this.username,
		   //this.userid,
		   //title.Text,
		   //displayorder.Text,
		   //starttime.Text,
		   //endtime.Text,
		   //message.Text);
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
	   }

	   public string GetAnnouncements()
	   {
		   return "Select * From `" + BaseConfigs.GetTablePrefix + "announcements` Order BY `id` ASC";
	   }

	   public void DeleteAnnouncements(string idlist)
	   {
		   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "announcements`  WHERE `ID` IN(" + idlist + ")";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
	   }

	   public DataTable GetAnnouncement(int id)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "announcements` WHERE `id`=?id", parm).Tables[0];
	   }

	   public void UpdateAnnouncement(int id, string poster, string title, int displayorder, string starttime, string endtime, string message)
	   {
		   IDataParameter[] parms = {



									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 250, title),
									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarChar, 20, poster),
									 DbHelper.MakeInParam("?starttime", (DbType)MySqlDbType.Datetime, 8, starttime),
									 DbHelper.MakeInParam("?endtime", (DbType)MySqlDbType.Datetime, 8, endtime),
									 DbHelper.MakeInParam("?message", (DbType)MySqlDbType.VarChar, 16, message),
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id)
								 };
		   string sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "announcements` SET `displayorder`=?displayorder,title=?title, poster=?poster,starttime=?starttime,endtime=?endtime,message=?message WHERE `id`=?id";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
	   }

	   public void DeleteAnnouncement(int id)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "announcements` WHERE `id`=?id", parm);
	   }


	   /// <summary>
	   /// 获得公共可见板块列表
	   /// </summary>
	   /// <returns></returns>
	   public IDataReader GetVisibleForumList()
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT `name`, `fid`, `layer` FROM `{0}forums` WHERE `parentid` NOT IN (SELECT fid FROM `{0}forums` WHERE `status` < 1 AND `layer` = 0) AND `status` > 0 AND `displayorder` >=0 ORDER BY `displayorder`", BaseConfigs.GetTablePrefix));
	   }

	   public IDataReader GetOnlineGroupIconList()
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `title`, `img` FROM `" + BaseConfigs.GetTablePrefix + "onlinelist` WHERE `img`<> '' ORDER BY `displayorder`");
	   }

	   /// <summary>
	   /// 获得友情链接列表
	   /// </summary>
	   /// <returns>友情链接列表</returns>
	   public DataTable GetForumLinkList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, @"SELECT `name`,`url`,`note`,`displayorder`+10000 AS `displayorder`,`logo` FROM `" + BaseConfigs.GetTablePrefix + "forumlinks` WHERE `displayorder` > 0 AND `logo` = '' UNION SELECT `name`,`url`,`note`,`displayorder`,`logo` FROM `" + BaseConfigs.GetTablePrefix + "forumlinks` WHERE `displayorder` > 0 AND `logo` <> '' ORDER BY `displayorder`").Tables[0];
	   }

	   /// <summary>
	   /// 获得脏字过滤列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetBanWordList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `find`, `replacement` FROM `" + BaseConfigs.GetTablePrefix + "words`").Tables[0];
	   }

	   /// <summary>
	   /// 获得勋章列表
	   /// </summary>
	   /// <returns>获得勋章列表</returns>
	   public DataTable GetMedalsList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `medalid`, `name`, `image`,`available`  FROM `" + BaseConfigs.GetTablePrefix + "medals` ORDER BY `medalid`").Tables[0];
	   }

	   /// <summary>
	   /// 获得魔法表情列表
	   /// </summary>
	   /// <returns>魔法表情列表</returns>
	   public DataTable GetMagicList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "magic` ORDER BY `id`").Tables[0];
	   }

	   /// <summary>
	   /// 获得主题类型列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetTopicTypeList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `typeid`,`name` FROM `" + BaseConfigs.GetTablePrefix + "topictypes` ORDER BY `displayorder`").Tables[0];
	   }

	   /// <summary>
	   /// 添加积分转帐兑换记录
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="fromto">来自/到</param>
	   /// <param name="sendcredits">付出积分类型</param>
	   /// <param name="receivecredits">得到积分类型</param>
	   /// <param name="send">付出积分数额</param>
	   /// <param name="receive">得到积分数额</param>
	   /// <param name="paydate">时间</param>
	   /// <param name="operation">积分操作(1=兑换, 2=转帐)</param>
	   /// <returns>执行影响的行</returns>
	   public int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?fromto",(DbType)MySqlDbType.Int32,4,fromto),
									 DbHelper.MakeInParam("?sendcredits",(DbType)MySqlDbType.Int32,4,sendcredits),
									 DbHelper.MakeInParam("?receivecredits",(DbType)MySqlDbType.Int32,4,receivecredits),
									 DbHelper.MakeInParam("?send",(DbType)MySqlDbType.Decimal,4,send),
									 DbHelper.MakeInParam("?receive",(DbType)MySqlDbType.Decimal,4,receive),
									 DbHelper.MakeInParam("?paydate",(DbType)MySqlDbType.VarChar,0,paydate),
									 DbHelper.MakeInParam("?operation",(DbType)MySqlDbType.Int32,4,operation)
								 };

		   return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "creditslog` (`uid`,`fromto`,`sendcredits`,`receivecredits`,`send`,`receive`,`paydate`,`operation`) VALUES(?uid,?fromto,?sendcredits,?receivecredits,?send,?receive,?paydate,?operation)", prams);

	   }

	   /// <summary>
	   /// 返回指定范围的积分日志
	   /// </summary>
	   /// <param name="pagesize">页大小</param>
	   /// <param name="currentpage">当前页数</param>
	   /// <param name="uid">用户id</param>
	   /// <returns>积分日志</returns>
	   public DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
	   {
		   //string strTableCreditslog = string.Concat(BaseConfigs.GetTablePrefix, "creditslog");
		   //string strTableUsers = string.Concat(BaseConfigs.GetTablePrefix, "users");

		   //string sqlstring = "SELECT `" + strTableCreditslog + "`.*, `" + strTableUsers + "`.`username` AS `username`  FROM `" + strTableCreditslog + "`  LEFT JOIN `" + strTableUsers + "` ON `" + strTableUsers + "`.`uid` = `" + strTableCreditslog + "`.`fromto` WHERE `" + strTableCreditslog + "`.`uid`=" + uid + " ORDER BY `" + strTableCreditslog + "`.`id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
		   //return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		   int pagetop = (currentpage - 1) * pagesize;
		   string sqlstring;
		   //if (currentpage == 1)
		   //{
		   //select c.*,ufrom.username as fromuser ,uto.username as touser from dnt_creditslog c,dnt_users ufrom, dnt_users uto where c.uid=ufrom.uid AND c.fromto=uto.uid
		   // AND (c.uid=1 or c.fromto =1)
		   sqlstring = string.Format("SELECT `c`.*, `ufrom`.`username` AS `fromuser`, `uto`.`username` AS `touser` FROM `{0}creditslog` AS `c`, `{0}users` AS `ufrom`, `{0}users` AS `uto` WHERE `c`.`uid`=`ufrom`.`uid` AND `c`.`fromto`=`uto`.`uid` AND (`c`.`uid`={1} OR `c`.`fromto` = {1})  ORDER BY `id` DESC LIMIT "+pagetop+","+pagesize,BaseConfigs.GetTablePrefix, uid);
		   //}
		   //else
		   //{
		   //    sqlstring = string.Format("SELECT `c`.*, `ufrom`.`username` AS `fromuser`, `uto`.`username` AS `touser` FROM `{0}creditslog` AS `c`, `{0}users` AS `ufrom`, `{0}users` AS `uto` WHERE `id` < (SELECT MIN(`id`)  FROM (SELECT `id` FROM `{0}creditslog` WHERE `{0}creditslog`.`uid`={1}  OR `{0}creditslog`.`fromto`={1} ORDER BY `id` DESC) AS tblTmp LIMIT 0,"+pagesize+") AND `c`.`uid`=`ufrom`.`uid` AND `c`.`fromto`=`uto`.`uid` AND (`c`.`uid`={1} OR `c`.`fromto` = {1}) ORDER BY `c`.`id` DESC LIMIT "+pagetop+","+pageto+"",BaseConfigs.GetTablePrefix,uid);
		   //}

		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        
        
        
	   }




	   /// <summary>
	   /// 获得指定用户的积分交易历史记录总条数
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns>历史记录总条数</returns>
	   public int GetCreditsLogRecordCount(int uid)
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "creditslog` WHERE `uid`=" + uid), 0);
	   }


	   //public DataTable GetTableStruct()
	   //{
	   //    //#region 数据表查询语句

	   //    //string SqlString = null;
	   //    //SqlString = "SELECT 表名=case when a.colorder=1 then d.name else '' end,";
	   //    //SqlString += "表说明=case when a.colorder=1 then isnull(f.value,'') else '' end,";
	   //    //SqlString += " 字段序号=a.colorder,";
	   //    //SqlString += " 字段名=a.name,";

	   //    //SqlString += " 标识=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,";
	   //    //SqlString += " 主键=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and name in (";
	   //    //SqlString += " SELECT name FROM sysindexes WHERE indid in(";
	   //    //SqlString += "   SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid";
	   //    //SqlString += "  ))) then '√' else '' end,";
	   //    //SqlString += " 类型=b.name,";
	   //    //SqlString += " 占用字节数=a.length,";
	   //    //SqlString += " 长度=COLUMNPROPERTY(a.id,a.name,'PRECISION'),";
	   //    //SqlString += " 小数位数=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),";
	   //    //SqlString += " 允许空=case when a.isnullable=1 then '√'else '' end,";
	   //    //SqlString += " 默认值=isnull(e.text,''),";
	   //    //SqlString += " 字段说明=isnull(g.`value`,'')";
	   //    //SqlString += "FROM syscolumns a";
	   //    //SqlString += " left join systypes b on a.xtype=b.xusertype";
	   //    //SqlString += " inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'";
	   //    //SqlString += " left join syscomments e on a.cdefault=e.id";
	   //    //SqlString += " left join sysproperties g on a.id=g.id and a.colid=g.smallid  ";
	   //    //SqlString += " left join sysproperties f on d.id=f.id and f.smallid=0";
	   //    ////SqlString+="--where d.name='要查询的表'    --如果只查询指定表,加上此条件";
	   //    //SqlString += " order by a.id,a.colorder";
	   //    //return SqlString;

	   //    //#endregion

	   //    DataTable dt = new DataTable();
	   //    IDataReader ddr = null;
	   //    ddr = DbHelper.ExecuteReader(CommandType.Text, "SHOW TABLES");
	   //    while (ddr.Read())
	   //    {
	   //        DataTable temp = new DataTable();
	   //        //DataTable tablename = new DataTable();
	   //        //tablename.Columns.Add("tablename", typeof(System.String));

	   //        //DataRow dr = dt.NewRow();
	   //        //dr[0] = ddr["Tables_in_dnt_gbk"].ToString();
	   //        ////dr[1] = "";
	   //        ////dr[2] = "";
	   //        ////dr[3] = "";
	   //        ////dr[4] = "";
	   //        ////dr[5] = "";
	   //        //tablename.Rows.Add(dr);
	   //        temp = DbHelper.ExecuteDataset("DESCRIBE " + ddr["Tables_in_dnt_gbk"].ToString()).Tables[0];
	   //        DataRow dataRow = temp.NewRow();
	   //        dataRow[0] = "<b>"+ddr["Tables_in_dnt_gbk"].ToString()+"</b>";
	   //        for (int i = 1; i < temp.Columns.Count; i++)
	   //        {
	   //            dataRow[i] = null;
	   //        }
	   //        temp.Rows.InsertAt(dataRow, 0);

	   //        //dt.Merge(tablename);
	   //        dt.Merge(temp);

	   //    }
            
	   //    return dt;
            
	   //}

	   public void ShrinkDataBase(string shrinksize, string dbname)
	   {
		   //    string SqlString = null;

		   //    SqlString += "SET NOCOUNT ON ";

		   //    SqlString += "DECLARE ?LogicalFileName sysname, ?MaxMinutes INT, ?NewSize INT ";
		   //    SqlString += "USE [" + dbname + "] -- 要操作的数据库名 ";
		   //    SqlString += "SELECT ?LogicalFileName = '" + dbname + "_log', -- 日志文件名 ";
		   //    SqlString += "?MaxMinutes = 10, -- Limit on time allowed to wrap log. ";
		   //    SqlString += "?NewSize = 1 -- 你想设定的日志文件的大小(M) ";
		   //    SqlString += "-- Setup / initialize ";
		   //    SqlString += "DECLARE ?OriginalSize int ";
		   //    SqlString += "SELECT ?OriginalSize = " + shrinksize;
		   //    SqlString += "FROM sysfiles ";
		   //    SqlString += "WHERE name = ?LogicalFileName ";
		   //    SqlString += "SELECT 'Original Size of ' + db_name() + ' LOG is ' + ";
		   //    SqlString += "CONVERT(VARCHAR(30),?OriginalSize) + ' 8K pages or ' + ";
		   //    SqlString += "CONVERT(VARCHAR(30),(?OriginalSize*8/1024)) + 'MB' ";
		   //    SqlString += "FROM sysfiles ";
		   //    SqlString += "WHERE name = ?LogicalFileName ";
		   //    SqlString += "CREATE TABLE DummyTrans ";
		   //    SqlString += "(DummyColumn char (8000) not null) ";
		   //    SqlString += "DECLARE ?Counter INT, ";
		   //    SqlString += "?StartTime DATETIME, ";
		   //    SqlString += "?TruncLog VARCHAR(255) ";
		   //    SqlString += "SELECT ?StartTime = GETDATE(), ";
		   //    SqlString += "?TruncLog = 'BACKUP LOG ' + db_name() + ' WITH TRUNCATE_ONLY' ";
		   //    SqlString += "DBCC SHRINKFILE (?LogicalFileName, ?NewSize) ";
		   //    SqlString += "EXEC (?TruncLog) ";
		   //    SqlString += "-- Wrap the log if necessary. ";
		   //    SqlString += "WHILE ?MaxMinutes > DATEDIFF (mi, ?StartTime, GETDATE()) -- time has not expired ";
		   //    SqlString += "AND ?OriginalSize = (SELECT size FROM sysfiles WHERE name = ?LogicalFileName) ";
		   //    SqlString += "AND (?OriginalSize * 8 /1024) > ?NewSize ";
		   //    SqlString += "BEGIN -- Outer loop. ";
		   //    SqlString += "SELECT ?Counter = 0 ";
		   //    SqlString += "WHILE ((?Counter < ?OriginalSize / 16) AND (?Counter < 50000)) ";
		   //    SqlString += "BEGIN -- update ";
		   //    SqlString += "INSERT DummyTrans VALUES ('Fill Log') ";
		   //    SqlString += "DELETE DummyTrans ";
		   //    SqlString += "SELECT ?Counter = ?Counter + 1 ";
		   //    SqlString += "END ";
		   //    SqlString += "EXEC (?TruncLog) ";
		   //    SqlString += "END ";
		   //    SqlString += "SELECT 'Final Size of ' + db_name() + ' LOG is ' + ";
		   //    SqlString += "CONVERT(VARCHAR(30),size) + ' 8K pages or ' + ";
		   //    SqlString += "CONVERT(VARCHAR(30),(size*8/1024)) + 'MB' ";
		   //    SqlString += "FROM sysfiles ";
		   //    SqlString += "WHERE name = ?LogicalFileName ";
		   //    SqlString += "DROP TABLE DummyTrans ";
		   //    SqlString += "SET NOCOUNT OFF ";

		   //    DbHelper.ExecuteDataset(CommandType.Text, SqlString);
	   }

	   public void ClearDBLog(string dbname)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?DBName", (DbType)MySqlDbType.VarChar, 50, dbname),
		   };
		   DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "" + BaseConfigs.GetTablePrefix + "shrinklog", prams);
	   }

	   public string RunSql(string sql)
	   {
		   string errorInfo = string.Empty;
		   if (sql != "")
		   {
			   MySqlConnection conn = new MySqlConnection(DbHelper.ConnectionString);
			   conn.Open();
			   foreach (string sqlStr in Utils.SplitString(sql, "--/* Discuz!NT SQL Separator */--"))
			   {
				   using (MySqlTransaction trans = conn.BeginTransaction())
				   {
					   try
					   {
						   DbHelper.ExecuteNonQuery(CommandType.Text, sqlStr);
						   trans.Commit();
					   }
					   catch (Exception ex)
					   {
						   trans.Rollback();
						   string message = ex.Message.Replace("'", " ");
						   message = message.Replace("\\", "/");
						   message = message.Replace("\r\n", "\\r\\n");
						   message = message.Replace("\r", "\\r");
						   message = message.Replace("\n", "\\n");
						   errorInfo += message + "<br>";
					   }
				   }
			   }
			   conn.Close();
		   }
		   return errorInfo;
	   }

	   //得到数据库的名称
	   public string GetDbName()
	   {
		   string connectionString = BaseConfigs.GetDBConnectString;
		   foreach (string info in connectionString.Split(';'))
		   {
			   if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
			   {
				   return info.Split('=')[1].Trim();
			   }
		   }
		   return "dnt";
	   }


	   /// <summary>
	   /// 创建并填充指定帖子分表id全文索引
	   /// </summary>
	   /// <param name="DbName">数据库名称</param>
	   /// <param name="postsid">当前帖子表的id</param>
	   /// <returns></returns>
	   public bool CreateORFillIndex(string DbName, string postsid)
	   {
		   StringBuilder sb = new StringBuilder();

		   string currenttablename = BaseConfigs.GetTablePrefix + "posts" + postsid;

		   try
		   {
			   //如果有全文索引则进行填充,如果没有就抛出异常
			   sb.Remove(0, sb.Length);
			   DbHelper.ExecuteNonQuery("SELECT `pid` FROM `" + currenttablename + "` where contains(`message`,'asd') ORDER BY `pid` ASC LIMIT 1");

			   sb.Append("execute sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full'; \r\n");
			   sb.Append("While fulltextcatalogproperty('pk_" + currenttablename + "_msg','populateStatus')<>0 \r\n");
			   sb.Append("begin \r\n");
			   sb.Append("waitfor delay '0:5:30' \r\n");
			   sb.Append("end \r\n");
			   DbHelper.ExecuteNonQuery(sb.ToString());

			   return true;
		   }
		   catch
		   {
			   try
			   {
				   #region 构建全文索引

				   sb.Remove(0, sb.Length);
				   sb.Append("if(select databaseproperty('[" + DbName + "]','isfulltextenabled'))=0  execute sp_fulltext_database 'enable';");

				   try
				   { //此处删除以确保全文索引目录和系统表中的数据同步
					   sb.Append("execute sp_fulltext_table '[" + currenttablename + "]', 'drop' ;");
					   sb.Append("execute sp_fulltext_catalog 'pk_" + currenttablename + "_msg','drop';");
					   DbHelper.ExecuteNonQuery(sb.ToString());
				   }
				   catch
				   {
					   ;
				   }
				   finally
				   {
					   //执行全文填充语句
					   sb.Remove(0, sb.Length);
					   sb.Append("execute sp_fulltext_catalog 'pk_" + currenttablename + "_msg','create';");
					   sb.Append("execute sp_fulltext_table '[" + currenttablename + "]','create','pk_" + currenttablename + "_msg','pk_" + currenttablename + "';");
					   sb.Append("execute sp_fulltext_column '[" + currenttablename + "]','message','add';");
					   sb.Append("execute sp_fulltext_table '[" + currenttablename + "]','activate';");
					   sb.Append("execute sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full';");
					   DbHelper.ExecuteNonQuery(sb.ToString());
				   }
				   return true;

				   #endregion
			   }
			   catch (MySqlException ex)
			   {
				   string message = ex.Message.Replace("'", " ");
				   message = message.Replace("\\", "/");
				   message = message.Replace("\r\n", "\\r\\n");
				   message = message.Replace("\r", "\\r");
				   message = message.Replace("\n", "\\n");
				   return true;
			   }
		   }
	   }


	   /// <summary>
	   /// 得到指定帖子分表的全文索引建立(填充)语句
	   /// </summary>
	   /// <param name="tablename"></param>
	   /// <returns></returns>
	   public string GetSpecialTableFullIndexSQL(string tablename)
	   {
		   #region 建表

		   StringBuilder sb = new StringBuilder();
		   sb.Append("if exists (select * from sysobjects where id = object_id(N'[" + tablename + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  drop table [" + tablename + "];");
		   sb.Append("CREATE TABLE [" + tablename + "] (`pid` `int` NOT NULL ,`fid` `int` NOT NULL ," +
			   "`tid` `int` NOT NULL ,`parentid` `int` NOT NULL ,`layer` `int` NOT NULL ,`poster` `nvarchar` (20) NOT NULL ," +
			   "`posterid` `int` NOT NULL ,`title` `nvarchar` (80) NOT NULL ,`postdatetime` `smalldatetime` NOT NULL ," +
			   "`message` `ntext` NOT NULL ,`ip` `nvarchar` (15) NOT NULL ," +
			   "`lastedit` `nvarchar` (50) NOT NULL ,`invisible` `int` NOT NULL ,`usesig` `int` NOT NULL ,`htmlon` `int` NOT NULL ," +
			   "`smileyoff` `int` NOT NULL ,`parseurloff` `int` NOT NULL ,`bbcodeoff` `int` NOT NULL ,`attachment` `int` NOT NULL ,`rate` `int` NOT NULL ," +
			   "`ratetimes` `int` NOT NULL ) ON `PRIMARY` TEXTIMAGE_ON `PRIMARY`");
		   sb.Append(";");
		   sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED (`pid`)  ON `PRIMARY`");
		   sb.Append(";");

		   sb.Append("ALTER TABLE [" + tablename + "] ADD ");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR `pid`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR `parentid`,CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR `layer`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR `poster`,CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR `posterid`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR `postdatetime`,CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR `message`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR `ip`,CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR `lastedit`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR `invisible`,CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR `usesig`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR `htmlon`,CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR `smileyoff`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR `parseurloff`,CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR `bbcodeoff`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR `attachment`,CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR `rate`,");
		   sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR `ratetimes`");

		   sb.Append(";");
		   sb.Append("CREATE  INDEX `parentid` ON [" + tablename + "](`parentid`) ON `PRIMARY`");
		   sb.Append(";");

		   sb.Append("CREATE  UNIQUE  INDEX `showtopic` ON [" + tablename + "](`tid`, `invisible`, `pid`) ON `PRIMARY`");
		   sb.Append(";");


		   sb.Append("CREATE  INDEX `treelist` ON [" + tablename + "](`tid`, `invisible`, `parentid`) ON `PRIMARY`");
		   sb.Append(";");

		   #endregion

		   #region 建全文索引

		   sb.Append("USE " + GetDbName() + " \r\n");
		   sb.Append("execute sp_fulltext_database 'enable'; \r\n");
		   sb.Append("if(select databaseproperty('[" + GetDbName() + "]','isfulltextenabled'))=0  execute sp_fulltext_database 'enable';");
		   sb.Append("if exists (select * from sysfulltextcatalogs where name ='pk_" + tablename + "_msg')  execute sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
		   sb.Append("if exists (select * from sysfulltextcatalogs where name ='pk_" + tablename + "_msg')  execute sp_fulltext_table '[" + tablename + "]', 'drop' ;");
		   sb.Append("execute sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
		   sb.Append("execute sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
		   sb.Append("execute sp_fulltext_column '[" + tablename + "]','message','add';");
		   sb.Append("execute sp_fulltext_table '[" + tablename + "]','activate';");
		   sb.Append("execute sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

		   #endregion

		   return sb.ToString();
	   }


	   /// <summary>
	   /// 以DataReader返回自定义编辑器按钮列表
	   /// </summary>
	   /// <returns></returns>
	   public IDataReader GetCustomEditButtonList()
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "bbcodes` WHERE `available` = 1");
	   }

	   /// <summary>
	   /// 以DataTable返回自定义按钮列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetCustomEditButtonListWithTable()
	   {
		   DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "bbcodes` WHERE `available` = 1");
		   if (ds != null)
		   {
			   if (ds.Tables.Count > 0)
			   {
				   return ds.Tables[0];
			   }
		   }
		   ds.Dispose();
		   return null;
	   }

	   public DataRowCollection GetTableListIds()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `id` FROM `" + BaseConfigs.GetTablePrefix + "tablelist`").Tables[0].Rows;
	   }



	   public void UpdateAnnouncementPoster(int posterid, string poster)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarChar, 20, poster),
									 DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, posterid)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "announcements` SET `poster`=?poster  WHERE `posterid`=?posterid", parms);
	   }

	   public bool HasStatisticsByLastUserId(int lastuserid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?lastuserid", (DbType)MySqlDbType.Int32, 4, lastuserid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `lastuserid` FROM  `" + BaseConfigs.GetTablePrefix + "statistics`  WHERE `lastuserid`=?lastuserid LIMIT 1",parm).Tables[0].Rows.Count > 0;
	   }

	   public void UpdateStatisticsLastUserName(int lastuserid, string lastusername)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?lastuserid", (DbType)MySqlDbType.Int32, 4, lastuserid),
									 DbHelper.MakeInParam("?lastusername", (DbType)MySqlDbType.VarChar, 20, lastusername)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `lastusername`=?lastusername WHERE `lastuserid`=?lastuserid", parms);
	   }

	   public void AddVisitLog(int uid, string username, int groupid, string grouptitle, string ip, string actions, string others)
	   {
		   string sqlstring = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "adminvisitlog` (`uid`,`username`,`groupid`,`grouptitle`,`ip`,`actions`,`others`,`postdatetime`) VALUES (?uid,?username,?groupid,?grouptitle,?ip,?actions,?others,NOW())";

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 50, username),
									 DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid),
									 DbHelper.MakeInParam("?grouptitle", (DbType)MySqlDbType.VarChar, 50, grouptitle),
									 DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarChar, 15, ip),
									 DbHelper.MakeInParam("?actions", (DbType)MySqlDbType.VarChar, 100, actions),
									 DbHelper.MakeInParam("?others", (DbType)MySqlDbType.VarChar, 200, others)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
	   }

	   public void DeleteVisitLogs()
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog` ");
	   }

	   public void DeleteVisitLogs(string condition)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog` WHERE " + condition);
	   }

	   /// <summary>
	   /// 得到当前指定页数的后台访问日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <returns></returns>
	   public DataTable GetVisitLogList(int pagesize, int currentpage)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog` Order by `visitid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString()).Tables[0];

	   }

	   /// <summary>
	   /// 得到当前指定条件和页数的后台访问日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public DataTable GetVisitLogList(int pagesize, int currentpage, string condition)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog` WHERE " + condition + " ORDER BY `visitid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + ", " + pagesize.ToString()).Tables[0];

	   }

	   public int GetVisitLogCount()
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(visitid) FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog`").Tables[0].Rows[0][0].ToString());
	   }

	   public int GetVisitLogCount(string condition)
	   {

		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(visitid) FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog` WHERE " + condition).Tables[0].Rows[0][0].ToString());
	   }

	   public void UpdateForumAndUserTemplateId(string templateidlist)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `templateid`=1 WHERE `templateid` IN(" + templateidlist + ")");
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `templateid`=1 WHERE `templateid` IN(" + templateidlist + ")");
	   }

	   public void AddTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarChar, 50, name),
									 DbHelper.MakeInParam("?directory", (DbType)MySqlDbType.VarChar, 100, directory),
									 DbHelper.MakeInParam("?copyright", (DbType)MySqlDbType.VarChar, 100, copyright),
									 DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarChar, 100, author),
									 DbHelper.MakeInParam("?createdate", (DbType)MySqlDbType.VarChar, 50, createdate),
									 DbHelper.MakeInParam("?ver", (DbType)MySqlDbType.VarChar, 100, ver),
									 DbHelper.MakeInParam("?fordntver", (DbType)MySqlDbType.VarChar, 100, fordntver)
								 };
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "templates` (`name`,`directory`,`copyright`,`author`,`createdate`,`ver`,`fordntver`) VALUES(?name,?directory,?copyright,?author,?createdate,?ver,?fordntver)";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   /// <summary>
	   /// 添加新的模板项
	   /// </summary>
	   /// <param name="templateName">模板名称</param>
	   /// <param name="directory">模板文件所在目录</param>
	   /// <param name="copyright">模板版权文字</param>
	   /// <returns>模板id</returns>
	   public int AddTemplate(string templateName, string directory, string copyright)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?templatename", (DbType)MySqlDbType.VarChar, 0, templateName),
									 DbHelper.MakeInParam("?directory", (DbType)MySqlDbType.VarChar, 0, directory),
									 DbHelper.MakeInParam("?copyright", (DbType)MySqlDbType.VarChar, 0, copyright),

		   };
		   // return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "templates`(`templatename`,`directory`,`copyright`) VALUES(?templatename, ?directory, ?copyright);SELECT SCOPE_IDENTITY()", prams), -1);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "templates`(`templatename`,`directory`,`copyright`) VALUES(?templatename, ?directory, ?copyright)",prams);
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text,"SELECT `templateid` FROM FROM `" + BaseConfigs.GetTablePrefix + "adminvisitlog` ORDER BY `templateid` DESC LIMIT 1" ).ToString());
           
	   }

	   /// <summary>
	   /// 删除指定的模板项
	   /// </summary>
	   /// <param name="templateid">模板id</param>
	   public void DeleteTemplateItem(int templateid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?templateid", (DbType)MySqlDbType.Int32, 4, templateid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "templates` WHERE `templateid`=?templateid");
	   }

	   /// <summary>
	   /// 删除指定的模板项列表,
	   /// </summary>
	   /// <param name="templateidlist">格式为： 1,2,3</param>
	   public void DeleteTemplateItem(string templateidlist)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "templates` WHERE `templateid` IN (" + templateidlist + ")");
	   }

	   /// <summary>
	   /// 获得所有在模板目录下的模板列表(即:子目录名称)
	   /// </summary>
	   /// <param name="templatePath">模板所在路径</param>
	   /// <example>GetAllTemplateList(Utils.GetMapPath(?"..\..\templates\"))</example>
	   /// <returns>模板列表</returns>
	   public DataTable GetAllTemplateList(string templatePath)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "templates` ORDER BY `templateid`").Tables[0];
	   }


	   public int GetMaxTemplateId()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IF(ISNULL(MAX(templateid)),0,MAX(templateid)) FROM " + BaseConfigs.GetTablePrefix + "templates"), 0) + 1;
	   }

	   /// <summary>
	   /// 插入版主管理日志记录
	   /// </summary>
	   /// <param name="moderatorname">版主名</param>
	   /// <param name="grouptitle">所属组的ID</param>
	   /// <param name="ip">客户端的IP</param>
	   /// <param name="fname">版块的名称</param>
	   /// <param name="title">主题的名称</param>
	   /// <param name="actions">动作</param>
	   /// <param name="reason">原因</param>
	   /// <returns></returns>
	   public bool InsertModeratorLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
	   {
		   try
		   {
			   string sqlstring = string.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` (`moderatoruid`,`moderatorname`,`groupid`,`grouptitle`,`ip`,`postdatetime`,`fid`,`fname`,`tid`,`title`,`actions`,`reason`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
				   moderatoruid,
				   moderatorname,
				   groupid,
				   grouptitle,
				   ip,
				   postdatetime,
				   fid,
				   fname,
				   tid,
				   title,
				   actions,
				   reason
				   );
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 按指定条件删除日志
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public bool DeleteModeratorLog(string condition)
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE " + condition);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }

	   }

	   /// <summary>
	   /// 得到当前指定条件和页数的前台管理日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public DataTable GetModeratorLogList(int pagesize, int currentpage, string condition)
	   {
		   if (condition != "") condition = " WHERE " + condition;

		   string sqlstring = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog`  " + condition + "  Order by `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
	   }

	   /// <summary>
	   /// 得到前台管理日志记录数
	   /// </summary>
	   /// <returns></returns>
	   public int GetModeratorLogListCount()
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog`").Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 得到指定查询条件下的前台管理日志数
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public int GetModeratorLogListCount(string condition)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE " + condition).Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 删除日志
	   /// </summary>
	   /// <returns></returns>
	   public bool DeleteMedalLog()
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "medalslog` ");
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 按指定条件删除日志
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public bool DeleteMedalLog(string condition)
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE  FROM `" + BaseConfigs.GetTablePrefix + "medalslog` WHERE " + condition);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 得到当前指定页数的勋章日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <returns></returns>
	   public DataTable GetMedalLogList(int pagesize, int currentpage)
	   {
		   string sqlstring = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "medalslog` ORDER BY `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
	   }

	   /// <summary>
	   /// 得到当前指定条件和页数的勋章日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public DataTable GetMedalLogList(int pagesize, int currentpage, string condition)
	   {
		   string sqlstring = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "medalslog` WHERE " + condition + "  Order by `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
	   }

	   /// <summary>
	   /// 得到缓存日志记录数
	   /// </summary>
	   /// <returns></returns>
	   public int GetMedalLogListCount()
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "medalslog`").Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 得到指定查询条件下的勋章日志数
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public int GetMedalLogListCount(string condition)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "medalslog` WHERE " + condition).Tables[0].Rows[0][0].ToString());
	   }


	   /// <summary>
	   /// 根据IP获取错误登录记录
	   /// </summary>
	   /// <param name="ip"></param>
	   /// <returns></returns>
	   public DataTable GetErrLoginRecordByIP(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.String,15, ip),
		   };
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `errcount`, `lastupdate` FROM `" + BaseConfigs.GetTablePrefix + "failedlogins` WHERE `ip`=?ip LIMIT 1", prams).Tables[0];
	   }

	   /// <summary>
	   /// 增加指定IP的错误记录数
	   /// </summary>
	   /// <param name="ip"></param>
	   /// <returns></returns>
	   public int AddErrLoginCount(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.String,15, ip),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "failedlogins` SET `errcount`=`errcount`+1, `lastupdate`=NOW() WHERE `ip`=?ip", prams);
	   }

	   /// <summary>
	   /// 增加指定IP的错误记录
	   /// </summary>
	   /// <param name="ip"></param>
	   /// <returns></returns>
	   public int AddErrLoginRecord(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.String,15, ip),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "failedlogins` (`ip`, `errcount`, `lastupdate`) VALUES(?ip, 1, NOW())", prams);
	   }

	   /// <summary>
	   /// 将指定IP的错误登录次数重置为1
	   /// </summary>
	   /// <param name="ip"></param>
	   /// <returns></returns>
	   public int ResetErrLoginCount(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.String,15, ip),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "failedlogins` SET `errcount`=1, `lastupdate`=now() WHERE `ip`=?ip", prams);
	   }

	   /// <summary>
	   /// 删除指定IP或者超过15天的记录
	   /// </summary>
	   /// <param name="ip"></param>
	   /// <returns></returns>
	   public int DeleteErrLoginRecord(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.String,15, ip),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "failedlogins` WHERE `ip`=?ip OR (SUBSTRING_INDEX(SEC_TO_TIME(unix_timestamp(`lastupdate`)-unix_timestamp(NOW())),':',1))*60 > 15", prams);
	   }

	   public int GetPostCount(string posttablename)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(pid) FROM `" + posttablename + "`").Tables[0].Rows[0][0].ToString());
	   }

	   public DataTable GetPostTableList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "tablelist`").Tables[0];
	   }

	   public int UpdateDetachTable(int fid, string description)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarChar, 50, description),
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "tablelist` SET `description`='" + description + "'  Where `id`=" + fid + "";
		   //fid, description);
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public int StartFullIndex(string dbname)
	   {
		   StringBuilder sb = new StringBuilder();
		   sb.Append("USE " + dbname + ";");
		   sb.Append("execute sp_fulltext_database 'enable';");
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
	   }
	   public void CreatePostTableAndIndex(string tablename)
	   {

		   //MySqlConnection Msc = new MySqlConnection(DbHelper.ConnectionString);
		   //Msc.Open();
		   //using (MySqlTransaction trans = Msc.BeginTransaction())
		   //{
		   //    try
		   //    {
		   string sql=GetSpecialTableFullIndexSQL(tablename, "");
		   DbHelper.ExecuteNonQuery(CommandType.Text,sql);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE  INDEX `parentid` ON `" + tablename + "`(`parentid`)");
		   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE  UNIQUE  INDEX `showtopic` ON `" + tablename + "`(`tid`, `invisible`, `pid`)");
		   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE  INDEX `treelist` ON `" + tablename + "`(`tid`, `invisible`, `parentid`)");
		   //DbHelper.ExecuteNonQuery(CommandType.Text, "ALTER TABLE `" + tablename + "` ADD FULLTEXT (`message`)");
		   //        trans.Commit();
		   //    }


		   //    catch
		   //    {
		   //        trans.Rollback();
		   //    }
		   //    finally
		   //    {

		   //        Msc.Close();
		   //    }
		   //}
	   }


	   /// <summary>
	   /// 得到指定帖子分表的全文索引建立(填充)语句
	   /// </summary>
	   /// <param name="tablename"></param>
	   /// <returns></returns>
	   public string GetSpecialTableFullIndexSQL(string tablename, string dbname)
	   {
		   //#region 建表

		   string sb = null;
		   //sb.Append("if exists (select * from sysobjects where id = object_id(N'[" + tablename + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)  drop table [" + tablename + "];");
		   //sb.Append("CREATE TABLE [" + tablename + "] (`pid` `int` NOT NULL ,`fid` `int` NOT NULL ," +
		   //    "`tid` `int` NOT NULL ,`parentid` `int` NOT NULL ,`layer` `int` NOT NULL ,`poster` `nvarchar` (20) NOT NULL ," +
		   //    "`posterid` `int` NOT NULL ,`title` `nvarchar` (80) NOT NULL ,`postdatetime` `smalldatetime` NOT NULL ," +
		   //    "`message` `ntext` NOT NULL ,`ip` `nvarchar` (15) NOT NULL ," +
		   //    "`lastedit` `nvarchar` (50) NOT NULL ,`invisible` `int` NOT NULL ,`usesig` `int` NOT NULL ,`htmlon` `int` NOT NULL ," +
		   //    "`smileyoff` `int` NOT NULL ,`parseurloff` `int` NOT NULL ,`bbcodeoff` `int` NOT NULL ,`attachment` `int` NOT NULL ,`rate` `int` NOT NULL ," +
		   //    "`ratetimes` `int` NOT NULL ) ON `PRIMARY` TEXTIMAGE_ON `PRIMARY`");
		   //sb.Append(";");
		   //sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED (`pid`)  ON `PRIMARY`");
		   //sb.Append(";");

		   //sb.Append("ALTER TABLE [" + tablename + "] ADD ");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR `pid`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR `parentid`,CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR `layer`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR `poster`,CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR `posterid`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR `postdatetime`,CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR `message`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR `ip`,CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR `lastedit`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR `invisible`,CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR `usesig`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR `htmlon`,CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR `smileyoff`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR `parseurloff`,CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR `bbcodeoff`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR `attachment`,CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR `rate`,");
		   //sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR `ratetimes`");

		   //sb.Append(";");
		   //sb.Append("CREATE  INDEX `parentid` ON [" + tablename + "](`parentid`)");
		   //sb.Append(";");

		   //sb.Append("CREATE  UNIQUE  INDEX `showtopic` ON [" + tablename + "](`tid`, `invisible`, `pid`)");
		   //sb.Append(";");


		   //sb.Append("CREATE  INDEX `treelist` ON [" + tablename + "](`tid`, `invisible`, `parentid`)");
		   //sb.Append(";");

		   //#endregion

		   //#region 建全文索引

		   //sb.Append("USE " + dbname + " \r\n");
		   //sb.Append("execute sp_fulltext_database 'enable'; \r\n");
		   //sb.Append("if(select databaseproperty('[" + dbname + "]','isfulltextenabled'))=0  execute sp_fulltext_database 'enable';");
		   //sb.Append("if exists (select * from sysfulltextcatalogs where name ='pk_" + tablename + "_msg')  execute sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
		   //sb.Append("if exists (select * from sysfulltextcatalogs where name ='pk_" + tablename + "_msg')  execute sp_fulltext_table '[" + tablename + "]', 'drop' ;");
		   //sb.Append("execute sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
		   //sb.Append("execute sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
		   //sb.Append("execute sp_fulltext_column '[" + tablename + "]','message','add';");
		   //sb.Append("execute sp_fulltext_table '[" + tablename + "]','activate';");
		   //sb.Append("execute sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

		   //#endregion

		   //return sb.ToString();
		   sb = "CREATE TABLE `" + tablename + "` (" +
			   "`pid` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`fid` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`tid` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`parentid` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`layer` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`poster` VARCHAR( 20 ) NULL ," +
			   "`posterid` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`title` VARCHAR( 60 ) NOT NULL ," +
			   "`postdatetime` DATETIME NOT NULL ," +
			   "`message` VARCHAR( 16 ) NULL ," +
			   "`ip` VARCHAR( 15 ) NULL ," +
			   "`lastedit` VARCHAR( 50 ) NULL ," +
			   "`invisible` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`usesig` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`htmlon` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`smileyoff` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`parseurloff` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`bbcodeoff` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`attachment` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`rate` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "`ratetimes` INT( 4 ) NOT NULL DEFAULT '0'," +
			   "PRIMARY KEY ( `pid` ) " +
			   ") ENGINE = MYISAM CHARACTER SET gbk COLLATE gbk_chinese_ci";

		   //  sb.Append("CREATE  INDEX `parentid` ON `" + tablename + "`(`parentid`)");
		   //sb.Append("CREATE  UNIQUE  INDEX `showtopic` ON `" + tablename + "`(`tid`, `invisible`, `pid`)");
		   // sb.Append("CREATE  INDEX `treelist` ON `" + tablename + "`(`tid`, `invisible`, `parentid`)");
		   return sb;
	   }

	   public void AddPostTableToTableList(string description, int mintid, int maxtid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarChar, 50, description),
									 DbHelper.MakeInParam("?mintid", (DbType)MySqlDbType.Int32, 4, mintid),
									 DbHelper.MakeInParam("?maxtid", (DbType)MySqlDbType.Int32, 4, maxtid)
								 };
		   string sql="INSERT INTO `" + BaseConfigs.GetTablePrefix + "tablelist`(`createdatetime`,`description`,`mintid`,`maxtid`) VALUES(NOW(),'"+description+"', "+mintid+", "+maxtid+")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public void CreatePostProcedure(string sqltemplate)
	   {
		   foreach (string sql in sqltemplate.Split('~'))
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		   }
	   }

	   public DataTable GetMaxTid()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(tid) FROM `" + BaseConfigs.GetTablePrefix + "topics`").Tables[0];
	   }

	   public DataTable GetPostCountFromIndex(string postsid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `rows` FROM `sysindexes` WHERE `name`='PK_" + BaseConfigs.GetTablePrefix + "posts" + postsid + "' LIMIT 1").Tables[0];
	   }

	   public DataTable GetPostCountTable(string postsid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT Count(pid) FROM `" + BaseConfigs.GetTablePrefix + "posts" + postsid + "`").Tables[0];
	   }

	   public void TestFullTextIndex(int posttableid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "SELECT `pid` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` where contains(`message`,'asd') ORDER BY `pid` ASC LIMIT ");
	   }

	   public DataRowCollection GetRateRange(int scoreid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid`, `raterange` FROM `" +
			   BaseConfigs.GetTablePrefix +
			   "usergroups` WHERE `raterange` LIKE '%" + scoreid + ",True,%'").
			   Tables[0].Rows;
	   }

	   public void UpdateRateRange(string raterange, int groupid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix +
			   "usergroups` SET `raterange`='" +
			   raterange +
			   "' WHERE `groupid`=" + groupid);
	   }

	   public int GetMaxTableListId()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IFNULL(MAX(id),0) FROM " + BaseConfigs.GetTablePrefix + "tablelist"), 0);
	   }
                                                                            
	   public int GetMaxPostTableTid(string posttablename)
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IFNULL(MAX(tid),0) FROM " + posttablename), 0) + 1;
	   }

	   public int GetMinPostTableTid(string posttablename)
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IFNULL(MAX(tid),0) FROM " + posttablename), 0) + 1;

	   }

	   public void AddAdInfo(int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.VarChar, 50, type),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 50, title),
									 DbHelper.MakeInParam("?targets", (DbType)MySqlDbType.VarChar, 255, targets),
									 DbHelper.MakeInParam("?parameters", (DbType)MySqlDbType.VarChar, 0, parameters),
									 DbHelper.MakeInParam("?code", (DbType)MySqlDbType.VarChar, 0, code),
									 DbHelper.MakeInParam("?starttime", (DbType)MySqlDbType.Datetime, 8, starttime),
									 DbHelper.MakeInParam("?endtime", (DbType)MySqlDbType.Datetime, 8, endtime)
								 };
		   string sql = "INSERT INTO  `" + BaseConfigs.GetTablePrefix + "advertisements` (`available`,`type`,`displayorder`,`title`,`targets`,`parameters`,`code`,`starttime`,`endtime`) VALUES(?available,?type,?displayorder,?title,?targets,?parameters,?code,?starttime,?endtime)";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public string GetAdvertisements()
	   {
		   return "Select * From `" + BaseConfigs.GetTablePrefix + "advertisements` Order BY `advid` ASC";
	   }

	   public DataRowCollection GetTargetsForumName(string targets)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `name` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid` IN(0" + targets + "0)").Tables[0].Rows;
	   }

	   public int UpdateAdvertisementAvailable(string aidlist, int available)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "advertisements` SET `available`=?available  WHERE `advid` IN(" + aidlist + ")";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public int UpdateAdvertisement(int aid, int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32, 4, aid),
									 DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.VarChar, 50, type),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 50, title),
									 DbHelper.MakeInParam("?targets", (DbType)MySqlDbType.VarChar, 255, targets),
									 DbHelper.MakeInParam("?parameters", (DbType)MySqlDbType.VarChar, 16, parameters),
									 DbHelper.MakeInParam("?code", (DbType)MySqlDbType.VarChar, 16, code),
									 DbHelper.MakeInParam("?starttime", (DbType)MySqlDbType.Datetime, 8, starttime),
									 DbHelper.MakeInParam("?endtime", (DbType)MySqlDbType.Datetime, 8, endtime)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "advertisements` SET `available`=?available,type=?type, displayorder=?displayorder,title=?title,targets=?targets,`parameters`=?parameters,code=?code,starttime=?starttime,endtime=?endtime WHERE `advid`=?aid";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public void DeleteAdvertisement(string aid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32, 4, aid);
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "advertisements` WHERE `advid`=?aid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?authorid",(DbType)MySqlDbType.Int32,4,posterid),
									 DbHelper.MakeInParam("?buydate",(DbType)MySqlDbType.Datetime,4,DateTime.Now),
									 DbHelper.MakeInParam("?amount",(DbType)MySqlDbType.Int32,4,price),
									 DbHelper.MakeInParam("?netamount",(DbType)MySqlDbType.Decimal,8,netamount)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET extcredits" + creditsTrans + " = extcredits" + creditsTrans + " - " + price.ToString() + " WHERE `uid` = ?uid", prams);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET extcredits" + creditsTrans + " = extcredits" + creditsTrans + " + ?netamount WHERE `uid` = ?authorid", prams);
	   }

	   public int AddPaymentLog(int uid, int tid, int posterid, int price, float netamount)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?authorid",(DbType)MySqlDbType.Int32,4,posterid),
									 DbHelper.MakeInParam("?buydate",(DbType)MySqlDbType.Datetime,4,DateTime.Now),
									 DbHelper.MakeInParam("?amount",(DbType)MySqlDbType.Int32,4,price),
									 DbHelper.MakeInParam("?netamount",(DbType)SqlDbType.Float,8,netamount)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "paymentlog` (`uid`,`tid`,`authorid`,`buydate`,`amount`,`netamount`) VALUES(?uid,?tid,?authorid,?buydate,?amount,?netamount)", prams);
	   }

	   /// <summary>
	   /// 判断用户是否已购买主题
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="uid">用户id</param>
	   /// <returns></returns>
	   public bool IsBuyer(int tid, int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT `id` FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` WHERE `tid` = ?tid AND `uid`=?uid", prams), 0) > 0;
	   }

	   public DataTable GetPayLogInList(int pagesize, int currentpage, int uid)
	   {
		   string sqlstring = "SELECT " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName FROM (" + BaseConfigs.GetTablePrefix + "paymentlog LEFT JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid) LEFT JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE `" + BaseConfigs.GetTablePrefix + "paymentlog`.`authorid`=" + uid + "  ORDER BY `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		   return dt;
	   }

	   /// <summary>
	   /// 获取指定用户的收入日志记录数
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns></returns>
	   public int GetPaymentLogInRecordCount(int uid)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` WHERE authorid=" + uid).Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 返回指定用户的支出日志记录数
	   /// </summary>
	   /// <param name="pagesize">每页记录数</param>
	   /// <param name="currentpage">当前页</param>
	   /// <param name="uid">用户id</param>
	   /// <returns></returns>
	   public DataTable GetPayLogOutList(int pagesize, int currentpage, int uid)
	   {
		   string sqlstring = "SELECT " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName FROM (" + BaseConfigs.GetTablePrefix + "paymentlog LEFT JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid) LEFT JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE `" + BaseConfigs.GetTablePrefix + "paymentlog`.`uid`=" + uid + "  ORDER BY `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		   return dt;
	   }

	   /// <summary>
	   /// 返回指定用户支出日志总数
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns></returns>
	   public int GetPaymentLogOutRecordCount(int uid)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` WHERE uid=" + uid).Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 获取指定主题的购买记录
	   /// </summary>
	   /// <param name="pagesize">每页记录数</param>
	   /// <param name="currentpage">当前页数</param>
	   /// <param name="tid">主题id</param>
	   /// <returns></returns>
	   public DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid)
	   {
		   string sqlstring = "SELECT " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "users.username As username FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE `" + BaseConfigs.GetTablePrefix + "paymentlog`.`tid`=" + tid + "  ORDER BY `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		   return dt;
	   }

	   /// <summary>
	   /// 主题购买总次数
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <returns></returns>
	   public int GetPaymentLogByTidCount(int tid)
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` WHERE tid=" + tid), 0);
	   }


	   public void AddSmiles(int id, int displayorder, int type, string code, string url)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 4, type),
									 DbHelper.MakeInParam("?code", (DbType)MySqlDbType.VarChar, 30, code),
									 DbHelper.MakeInParam("?url", (DbType)MySqlDbType.VarChar, 60, url)
								 };


		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "smilies` (id,displayorder,type,code,url) Values (?id,?displayorder,?type,?code,?url)";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public string GetIcons()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE TYPE=1";
	   }

	   public string DeleteSmily(int id)
	   {
		   return "DELETE  FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE `id`=" + id;
	   }

	   public int UpdateSmilies(int id, int displayorder, int type, string code, string url)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 4, type),
									 DbHelper.MakeInParam("?code", (DbType)MySqlDbType.VarChar, 30, code),
									 DbHelper.MakeInParam("?url", (DbType)MySqlDbType.VarChar, 60, url)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "smilies` SET `displayorder`=?displayorder,`type`=?type,`code`=?code,`url`=?url Where `id`=?id";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public int UpdateSmiliesPart(string code, int displayorder, int id)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder),
									 DbHelper.MakeInParam("?code", (DbType)MySqlDbType.VarChar, 30, code)
								 };
		   string sql = "Update `" + BaseConfigs.GetTablePrefix + "smilies` Set code=?code,displayorder=?displayorder Where id=?id";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public int DeleteSmilies(string idlist)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "smilies`  WHERE `ID` IN(" + idlist + ")");
	   }

	   public string GetSmilies()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE `type`=0";
	   }

	   public int GetMaxSmiliesId()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IFNULL(MAX(`id`),0) FROM " + BaseConfigs.GetTablePrefix + "smilies"), 0) + 1;
            
	   }

	   public DataTable GetSmiliesInfoByType(int type)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 4, type);
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE TYPE=?type";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
	   }


	   /// <summary>
	   /// 得到表情符数据
	   /// </summary>
	   /// <returns>表情符数据</returns>
	   public IDataReader GetSmiliesList()
	   {
		   IDataReader dr = DbHelper.ExecuteReader(System.Data.CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE `type`=0 ORDER BY `displayorder` DESC,`id` ASC");
		   return dr;
	   }

	   /// <summary>
	   /// 得到表情符数据
	   /// </summary>
	   /// <returns>表情符表</returns>
	   public DataTable GetSmiliesListDataTable()
	   {
		   DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` ORDER BY `type`,`displayorder`,`id`");
		   if (ds != null)
		   {
			   if (ds.Tables.Count > 0)
			   {
				   return ds.Tables[0];
			   }
		   }
		   return new DataTable();
	   }

	   /// <summary>
	   /// 得到不带分类的表情符数据
	   /// </summary>
	   /// <returns>表情符表</returns>
	   public DataTable GetSmiliesListWithoutType()
	   {
		   DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE `type`<>0 ORDER BY `type`,`displayorder`,`id`");
		   if (ds != null)
		   {
			   if (ds.Tables.Count > 0)
			   {
				   return ds.Tables[0];
			   }
		   }
		   return new DataTable();
	   }

	   /// <summary>
	   /// 获得表情分类列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetSmilieTypes()
	   {
		   DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE `type`=0 ORDER BY `displayorder`,`id`");
		   if (ds != null && ds.Tables.Count > 0)
		   {
			   return ds.Tables[0];
		   }
		   return new DataTable();
	   }

	   public DataRow GetSmilieTypeById(string id)
	   {
		   DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE id=" + id);
		   if (ds != null && ds.Tables[0].Rows.Count == 1)
			   return ds.Tables[0].Rows[0];
		   else
			   return null;
	   }
	   /// <summary>
	   /// 获得统计列
	   /// </summary>
	   /// <returns>统计列</returns>
	   public DataRow GetStatisticsRow()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "statistics` LIMIT 1").Tables[0].Rows[0];
	   }

	   /// <summary>
	   /// 将缓存中的统计列保存到数据库
	   /// </summary>
	   public void SaveStatisticsRow(DataRow dr)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?totaltopic", (DbType)MySqlDbType.Int32,4,Int32.Parse(dr["totaltopic"].ToString())),
									 DbHelper.MakeInParam("?totalpost",(DbType)MySqlDbType.Int32,4,Int32.Parse(dr["totalpost"].ToString())),
									 DbHelper.MakeInParam("?totalusers",(DbType)MySqlDbType.Int32,4,Int32.Parse(dr["totalusers"].ToString())),
									 DbHelper.MakeInParam("?lastusername",(DbType)MySqlDbType.VarChar,20,dr["totalusers"].ToString()),
									 DbHelper.MakeInParam("?lastuserid",(DbType)MySqlDbType.Int32,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									 DbHelper.MakeInParam("?highestonlineusercount",(DbType)MySqlDbType.Int32,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									 DbHelper.MakeInParam("?highestonlineusertime",(DbType)MySqlDbType.Datetime,4,DateTime.Parse(dr["highestonlineusertime"].ToString()))
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totaltopic`=?totaltopic,`totalpost`=?totalpost, `totalusers`=?totalusers, `lastusername`=?lastusername, `lastuserid`=?lastuserid, `highestonlineusercount`=??highestonlineusercount, `highestonlineusertime`=?highestonlineusertime", prams);
	   }

	   public IDataReader GetAllForumStatistics()
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM(`topics`) AS `topiccount`,SUM(`posts`) AS `postcount`,SUM(`todayposts`)-(SELECT SUM(`todayposts`) from `{0}forums` WHERE `lastpost` < CURDATE() AND `layer`=1) AS `todaypostcount` FROM `{0}forums` WHERE `layer`=1", BaseConfigs.GetTablePrefix));
	   }

	   public IDataReader GetForumStatistics(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid)
								 };
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM(`topics`) AS `topiccount`,SUM(`posts`) AS `postcount`,SUM(`todayposts`)-(SELECT SUM(`todayposts`) from `{0}forums` WHERE `lastpost` < CURDATE() AND `layer`=1 AND `fid` = ?fid) AS `todaypostcount` FROM `{0}forums` WHERE `fid` = ?fid AND `layer`=1", BaseConfigs.GetTablePrefix), prams);


	   }

	   /// <summary>
	   /// 更新指定名称的统计项
	   /// </summary>
	   /// <param name="param">项目名称</param>
	   /// <param name="Value">指定项的值</param>
	   /// <returns>更新数</returns>
	   public int UpdateStatistics(string param, int intValue)
	   {
		   System.Text.StringBuilder sb = new System.Text.StringBuilder();
		   if (!param.Equals(""))
		   {
			   sb.Append("UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET ");
			   sb.Append(param);
			   sb.Append(" = ");
			   sb.Append(intValue);
		   }
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
	   }

	   /// <summary>
	   /// 更新指定名称的统计项
	   /// </summary>
	   /// <param name="param">项目名称</param>
	   /// <param name="Value">指定项的值</param>
	   /// <returns>更新数</returns>
	   public int UpdateStatistics(string param, string strValue)
	   {
		   System.Text.StringBuilder sb = new System.Text.StringBuilder();
		   if (!param.Equals(""))
		   {
			   sb.Append("UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET ");
			   sb.Append(param);
			   sb.Append(" = '");
			   sb.Append(strValue);
			   sb.Append("'");
		   }
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
	   }

	   /// <summary>
	   /// 获得前台有效的模板列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetValidTemplateList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "templates` ORDER BY `templateid`").Tables[0];
	   }

	   /// <summary>
	   /// 获得前台有效的模板ID列表
	   /// </summary>
	   /// <returns>模板ID列表</returns>
	   public DataTable GetValidTemplateIDList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `templateid` FROM `" + BaseConfigs.GetTablePrefix + "templates` ORDER BY `templateid`").Tables[0];
	   }

	   public DataTable GetPost(string posttablename, int pid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, pid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT *  FROM `" + posttablename + "` WHERE pid=?pid LIMIT 1", parm).Tables[0];
	   }

	   public DataTable GetMainPostByTid(string posttablename, int tid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + posttablename + "` WHERE `layer`=0  AND `tid`=?tid LIMIT 1", parm).Tables[0];
	   }

	   public DataTable GetAttachmentsByPid(int pid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, pid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `aid`, `tid`, `pid`, `postdatetime`, `readperm`, `filename`, `description`, `filetype`, `filesize`, `attachment`, `downloads` FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `pid`=?pid").Tables[0];
	   }

	   public DataTable GetAdvertisement(int aid)
	   {
		   //此函数放在Advs.cs文件中较好
		   IDataParameter parm = DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32, 4, aid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "advertisements` WHERE `advid`=?aid", parm).Tables[0];
	   }

	   /// <summary>
	   /// 根据指定条件进行搜索
	   /// </summary>
	   /// <param name="posttableid">帖子表id</param>
	   /// <param name="userid">用户id</param>
	   /// <param name="usergroupid">用户组id</param>
	   /// <param name="keyword">关键字</param>
	   /// <param name="posterid">发帖者id</param>
	   /// <param name="type">搜索类型</param>
	   /// <param name="searchforumid">搜索版块id</param>
	   /// <param name="keywordtype">关键字类型</param>
	   /// <param name="searchtime">搜索时间</param>
	   /// <param name="searchtimetype">搜索时间类型</param>
	   /// <param name="resultorder">结果排序方式</param>
	   /// <param name="resultordertype">结果类型类型</param>
	   /// <returns>如果成功则返回searchid, 否则返回-1</returns>
	   private string GetSearchTopicTitleSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int digest, string keyword)
	   {
		   keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

		   StringBuilder strKeyWord = new StringBuilder(keyword);

		   // 替换转义字符
		   strKeyWord.Replace("'", "\\'");
		   strKeyWord.Replace("%", "\\%");
		   strKeyWord.Replace("_", "\\_");
		   strKeyWord.Replace("[", "\\[");

		   StringBuilder strSQL = new StringBuilder();
		   strSQL.AppendFormat("SELECT `tid` FROM `{0}topics` WHERE `displayorder`>=0", BaseConfigs.GetTablePrefix);

		   if (posterid > 0)
		   {
			   strSQL.Append(" AND `posterid`=");
			   strSQL.Append(posterid);
		   }

		   if (digest > 0)
		   {
			   strSQL.Append(" AND `digest`>0 ");
		   }

		   if (searchforumid != string.Empty)
		   {
			   strSQL.Append(" AND `fid` IN (");
			   strSQL.Append(searchforumid);
			   strSQL.Append(")");
		   }

		   string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
		   strKeyWord = new StringBuilder();

		   if (keyword.Length > 0)
		   {
			   strKeyWord.Append(" AND (1=0 ");
			   for (int i = 0; i < keywordlist.Length; i++)
			   {
				   strKeyWord.Append(" OR `title` ");

				   strKeyWord.Append("LIKE '%");
				   strKeyWord.Append(RegEsc(keywordlist[i]));
				   strKeyWord.Append("%' ");
			   }
			   strKeyWord.Append(")");
		   }

		   strSQL.Append(strKeyWord.ToString());

		   strSQL.Append(" ORDER BY ");
		   switch (resultorder)
		   {
			   case 1:
				   strSQL.Append("`tid`");
				   break;
			   case 2:
				   strSQL.Append("`replies`");
				   break;
			   case 3:
				   strSQL.Append("`views`");
				   break;
			   default:
				   strSQL.Append("`postdatetime`");
				   break;
		   }

		   if (resultordertype == 1)
		   {
			   strSQL.Append(" ASC");
		   }
		   else
		   {
			   strSQL.Append(" DESC");
		   }

		   return strSQL.ToString();
	   }

	   private string GetSearchPostContentSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int searchtime, int searchtimetype, int posttableid, StringBuilder strKeyWord)
	   {
		   StringBuilder strSQL = new StringBuilder();

		   string orderfield = "lastpost";
		   switch (resultorder)
		   {
			   case 1:
				   orderfield = "tid";
				   break;
			   case 2:
				   orderfield = "replies";
				   break;
			   case 3:
				   orderfield = "views";
				   break;
			   default:
				   orderfield = "lastpost";
				   break;
		   }

		   strSQL.AppendFormat("SELECT DISTINCT `{0}posts{1}`.`tid`,`{0}topics`.`{2}` FROM `{0}posts{1}` LEFT JOIN `{0}topics` ON `{0}topics`.`tid`=`{0}posts{1}`.`tid` WHERE `{0}topics`.`displayorder`>=0 AND ", BaseConfigs.GetTablePrefix, posttableid, orderfield);

		   if (searchforumid != string.Empty)
		   {
			   strSQL.AppendFormat("`{0}posts{1}`.`fid` IN (", BaseConfigs.GetTablePrefix, posttableid);
			   strSQL.Append(searchforumid);
			   strSQL.Append(") AND ");
		   }

		   if (posterid != -1)
		   {
			   strSQL.AppendFormat("`{0}posts{1}`.`posterid`=", BaseConfigs.GetTablePrefix, posttableid);
			   strSQL.Append(posterid);
			   strSQL.Append(" AND ");
		   }

		   if (searchtime != 0)
		   {
			   strSQL.AppendFormat("`{0}posts{1}`.`postdatetime`", BaseConfigs.GetTablePrefix, posttableid);
			   if (searchtimetype == 1)
			   {
				   strSQL.Append("<'");
			   }
			   else
			   {
				   strSQL.Append(">'");
			   }
			   strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
			   strSQL.Append("'AND ");
		   }

		   string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
		   strKeyWord = new StringBuilder();
		   for (int i = 0; i < keywordlist.Length; i++)
		   {
			   strKeyWord.Append(" OR ");
			   if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
			   {
				   strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, posttableid);
				   strKeyWord.Append(keywordlist[i]);
				   strKeyWord.Append("*\"') ");
			   }
			   else
			   {
				   strKeyWord.AppendFormat("`{0}posts{1}`.`message` LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
				   strKeyWord.Append(RegEsc(keywordlist[i]));
				   strKeyWord.Append("%' ");
			   }
		   }

		   strSQL.Append(strKeyWord.ToString().Substring(3));
		   strSQL.AppendFormat("ORDER BY `{0}topics`.", BaseConfigs.GetTablePrefix);

		   switch (resultorder)
		   {
			   case 1:
				   strSQL.Append("`tid`");
				   break;
			   case 2:
				   strSQL.Append("`replies`");
				   break;
			   case 3:
				   strSQL.Append("`views`");
				   break;
			   default:
				   strSQL.Append("`lastpost`");
				   break;
		   }
		   if (resultordertype == 1)
		   {
			   strSQL.Append(" ASC");
		   }
		   else
		   {
			   strSQL.Append(" DESC");
		   }

		   return strSQL.ToString();
	   }

	   private string GetSearchSpacePostTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
	   {
		   keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

		   StringBuilder strKeyWord = new StringBuilder(keyword);

		   // 替换转义字符
		   strKeyWord.Replace("'", "''");
		   strKeyWord.Replace("%", "[%]");
		   strKeyWord.Replace("_", "[_]");
		   strKeyWord.Replace("[", "[[]");

		   StringBuilder strSQL = new StringBuilder();
		   strSQL.AppendFormat("SELECT `postid` FROM `{0}spaceposts` WHERE `{0}spaceposts`.`poststatus`=1 ", BaseConfigs.GetTablePrefix);

		   if (posterid > 0)
		   {
			   strSQL.Append(" AND `uid`=");
			   strSQL.Append(posterid);
		   }

		   if (searchtime != 0)
		   {
			   strSQL.AppendFormat(" AND `{0}spaceposts`.`postdatetime`", BaseConfigs.GetTablePrefix);
			   if (searchtimetype == 1)
			   {
				   strSQL.Append("<'");
			   }
			   else
			   {
				   strSQL.Append(">'");
			   }
			   strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
			   strSQL.Append("' ");
		   }

		   string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
		   strKeyWord = new StringBuilder();

		   if (keyword.Length > 0)
		   {
			   strKeyWord.Append(" AND (1=0 ");
			   for (int i = 0; i < keywordlist.Length; i++)
			   {
				   strKeyWord.Append(" OR `title` ");
				   strKeyWord.Append("LIKE '%");
				   strKeyWord.Append(RegEsc(keywordlist[i]));
				   strKeyWord.Append("%' ");
			   }
			   strKeyWord.Append(")");
		   }

		   strSQL.Append(strKeyWord.ToString());

		   strSQL.Append(" ORDER BY ");
		   switch (resultorder)
		   {
			   case 1:
				   strSQL.Append("`commentcount`");
				   break;
			   case 2:
				   strSQL.Append("`views`");
				   break;
			   default:
				   strSQL.Append("`postdatetime`");
				   break;
		   }

		   if (resultordertype == 1)
		   {
			   strSQL.Append(" ASC");
		   }
		   else
		   {
			   strSQL.Append(" DESC");
		   }

		   return strSQL.ToString();
	   }

	   private string GetSearchAlbumTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
	   {
		   keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

		   StringBuilder strKeyWord = new StringBuilder(keyword);

		   // 替换转义字符
		   strKeyWord.Replace("'", "''");
		   strKeyWord.Replace("%", "[%]");
		   strKeyWord.Replace("_", "[_]");
		   strKeyWord.Replace("[", "[[]");

		   StringBuilder strSQL = new StringBuilder();
		   strSQL.AppendFormat("SELECT `albumid` FROM `{0}albums` WHERE `{0}albums`.`type`=0 ", BaseConfigs.GetTablePrefix);

		   if (posterid > 0)
		   {
			   strSQL.Append(" AND `userid`=");
			   strSQL.Append(posterid);
		   }

		   if (searchtime != 0)
		   {
			   strSQL.AppendFormat(" AND `{0}albums`.`createdatetime`", BaseConfigs.GetTablePrefix);
			   if (searchtimetype == 1)
			   {
				   strSQL.Append("<'");
			   }
			   else
			   {
				   strSQL.Append(">'");
			   }
			   strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
			   strSQL.Append("' ");
		   }

		   string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
		   strKeyWord = new StringBuilder();

		   if (keyword.Length > 0)
		   {
			   strKeyWord.Append(" AND (1=0 ");
			   for (int i = 0; i < keywordlist.Length; i++)
			   {
				   strKeyWord.Append(" OR `title` ");
				   strKeyWord.Append("LIKE '%");
				   strKeyWord.Append(RegEsc(keywordlist[i]));
				   strKeyWord.Append("%' ");
			   }
			   strKeyWord.Append(")");
		   }

		   strSQL.Append(strKeyWord.ToString());

		   strSQL.Append(" ORDER BY ");
		   switch (resultorder)
		   {
			   case 1:
				   strSQL.Append("`albumid`");
				   break;
			   default:
				   strSQL.Append("`createdatetime`");
				   break;
		   }

		   if (resultordertype == 1)
		   {
			   strSQL.Append(" ASC");
		   }
		   else
		   {
			   strSQL.Append(" DESC");
		   }

		   return strSQL.ToString();
	   }

	   private string GetSearchByPosterSQL(int posterid, int posttableid)
	   {
		   if (posterid > 0)
		   {
			   string sql = string.Format(@"SELECT DISTINCT `tid`, 'forum' AS `datafrom` FROM `{0}posts{1}` WHERE `posterid`={2} AND `tid` NOT IN (SELECT `tid` FROM `{0}topics` WHERE `posterid`={2} AND `displayorder`<0) UNION ALL SELECT `albumid`,'album' AS `datafrom` FROM `{0}albums` WHERE   `userid`={2} UNION ALL SELECT `postid`,'spacepost' AS `datafrom` FROM `{0}spaceposts` WHERE `uid`={2}", BaseConfigs.GetTablePrefix, posttableid, posterid);
			   return sql;
		   }
		   return string.Empty;
	   }

	   private StringBuilder GetSearchByPosterResult(IDataReader reader)
	   {
		   StringBuilder strTids = new StringBuilder("<ForumTopics>");
		   StringBuilder strAlbumids = new StringBuilder("<Albums>");
		   StringBuilder strSpacePostids = new StringBuilder("<SpacePosts>");
		   StringBuilder result = new StringBuilder();
		   if (reader != null)
		   {
			   while (reader.Read())
			   {
				   switch (reader[1].ToString())
				   {
					   case "forum":
						   strTids.AppendFormat("{0},", reader[0].ToString());
						   break;
					   case "album":
						   strAlbumids.AppendFormat("{0},", reader[0].ToString());
						   break;
					   case "spacepost":
						   strSpacePostids.AppendFormat("{0},", reader[0].ToString());
						   break;
				   }
			   }
			   reader.Close();
		   }
            
		   if (strTids.ToString().EndsWith(","))
		   {
			   strTids.Length--;
		   }
		   if (strAlbumids.ToString().EndsWith(","))
		   {
			   strAlbumids.Length--;
		   }
		   if (strSpacePostids.ToString().EndsWith(","))
		   {
			   strSpacePostids.Length--;
		   }
		   strTids.Append("</ForumTopics>");
		   strAlbumids.Append("</Albums>");
		   strSpacePostids.Append("</SpacePosts>");

		   result.Append(strTids.ToString());
		   result.Append(strAlbumids.ToString());
		   result.Append(strSpacePostids.ToString());

		   return result;
	   }
	   /// <summary>
	   /// 根据指定条件进行搜索
	   /// </summary>
	   /// <param name="posttableid">帖子表id</param>
	   /// <param name="userid">用户id</param>
	   /// <param name="usergroupid">用户组id</param>
	   /// <param name="keyword">关键字</param>
	   /// <param name="posterid">发帖者id</param>
	   /// <param name="type">搜索类型</param>
	   /// <param name="searchforumid">搜索版块id</param>
	   /// <param name="keywordtype">关键字类型</param>
	   /// <param name="searchtime">搜索时间</param>
	   /// <param name="searchtimetype">搜索时间类型</param>
	   /// <param name="resultorder">结果排序方式</param>
	   /// <param name="resultordertype">结果类型类型</param>
	   /// <returns>如果成功则返回searchid, 否则返回-1</returns>
	   public int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, string type, string searchforumid, int keywordtype, int searchtime, int searchtimetype, int resultorder, int resultordertype)
	   {

		   // 超过30分钟的缓存纪录将被删除
		   DatabaseProvider.GetInstance().DeleteExpriedSearchCache();
		   string sql = string.Empty;
		   StringBuilder strTids = new StringBuilder();
		   SearchType searchType = SearchType.TopicTitle;

		   switch (keywordtype)
		   {
			   case 0:
				   searchType = SearchType.PostTitle;
				   if (type == "digest")
				   {
					   searchType = SearchType.DigestTopic;
				   }
				   break;
			   case 1:
				   searchType = SearchType.PostContent;
				   break;
			   case 2:
				   searchType = SearchType.SpacePostTitle;
				   break;
			   case 3:
				   searchType = SearchType.AlbumTitle;
				   break;
			   case 8:
				   searchType = SearchType.ByPoster;
				   break;
		   }
		   switch (searchType)
		   {
			   case SearchType.All:
				   break;
			   case SearchType.DigestTopic:
				   sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 1, keyword);
				   break;
			   case SearchType.TopicTitle:
				   sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
				   break;
			   case SearchType.PostTitle:
				   sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
				   break;
			   case SearchType.PostContent:
				   sql = GetSearchPostContentSQL(posterid, searchforumid, resultorder, resultordertype, searchtime, searchtimetype, posttableid, new StringBuilder(keyword));
				   break;
			   case SearchType.SpacePostTitle:
				   sql = GetSearchSpacePostTitleSQL(posterid, resultorder, resultordertype, searchtime, searchtimetype, keyword);
				   break;
			   case SearchType.AlbumTitle:
				   sql = GetSearchAlbumTitleSQL(posterid, resultorder, resultordertype, searchtime, searchtimetype, keyword);
				   break;
			   case SearchType.ByPoster:
				   sql = GetSearchByPosterSQL(posterid, posttableid);
				   break;
			   default:
				   sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
				   break;
		   }

		   #region
		   /*
			// 关键词与作者至少有一个条件不为空
			if (keyword.Equals(""))//按作者搜索
			{

				if (type == "digest")
				{
					strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [digest]>0 AND [posterid]=", BaseConfigs.GetTablePrefix);
					strSQL.Append(posterid);
				}

				else if (type == "post")
				{
					strSQL.AppendFormat("SELECT [pid] FROM [{0}posts{1}] WHERE [posterid]=", BaseConfigs.GetTablePrefix, posttableid);
					strSQL.Append(posterid);
				}
				else
				{
					strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [posterid]=", BaseConfigs.GetTablePrefix);
					strSQL.Append(posterid);
				}

				//所属板块判断
				if (!searchforumid.Equals(""))
				{
					strSQL.Append(" AND [fid] IN (");
					strSQL.Append(searchforumid);
					strSQL.Append(")");
				}


				strSQL.Append(" ORDER BY ");

				switch (resultorder)
				{
					case 1:
						strSQL.Append("[tid]");
						break;
					case 2:
						strSQL.Append("[replies]");
						break;
					case 3:
						strSQL.Append("[views]");
						break;
					default:
						strSQL.Append("[postdatetime]");
						break;
				}
				if (resultordertype == 1)
				{
					strSQL.Append(" ASC");
				}
				else
				{
					strSQL.Append(" DESC");
				}

			}
			else
			{
				// 过滤危险字符
				keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

				StringBuilder strKeyWord = new StringBuilder(keyword);

				// 替换转义字符
				strKeyWord.Replace("'", "''");
				strKeyWord.Replace("%", "[%]");
				strKeyWord.Replace("_", "[_]");
				strKeyWord.Replace("[", "[[]");


				// 将SQL查询条件循序指定为"forumid, posterid, 搜索时间范围, 关键词"
				if (keywordtype == 0)
				{
					strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE", BaseConfigs.GetTablePrefix);
					if (!searchforumid.Equals(""))
					{
						strSQL.Append(" [fid] IN (");
						strSQL.Append(searchforumid);
						strSQL.Append(") AND ");
					}
					if (posterid != -1)
					{
						strSQL.Append("[posterid]=");
						strSQL.Append(posterid);
						strSQL.Append(" AND ");
					}

					if (searchtime != 0)
					{
						strSQL.Append("[postdatetime]");
						if (searchtimetype == 1)
						{
							strSQL.Append("<");
						}
						else
						{
							strSQL.Append(">'");
						}
						strSQL.Append(DateTime.Now.AddDays(searchtime * -1).ToString("yyyy-MM-dd 00:00:00"));
						strSQL.Append("'AND ");
					}

					string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
					strKeyWord = new StringBuilder();
					for (int i = 0; i < keywordlist.Length; i++)
					{
						strKeyWord.Append(" OR [title] ");

						strKeyWord.Append("LIKE '%");
						strKeyWord.Append(keywordlist[i]);
						strKeyWord.Append("%' ");
					}

					strSQL.Append(strKeyWord.ToString().Substring(3));
					strSQL.Append("ORDER BY ");

					switch (resultorder)
					{
						case 1:
							strSQL.Append("[tid]");
							break;
						case 2:
							strSQL.Append("[replies]");
							break;
						case 3:
							strSQL.Append("[views]");
							break;
						default:
							strSQL.Append("[lastpost]");
							break;
					}
					if (resultordertype == 1)
					{
						strSQL.Append(" ASC");
					}
					else
					{
						strSQL.Append(" DESC");
					}
				}
				else
				{
					string orderfield = "lastpost";
					switch (resultorder)
					{
						case 1:
							orderfield = "tid";
							break;
						case 2:
							orderfield = "replies";
							break;
						case 3:
							orderfield = "views";
							break;
						default:
							orderfield = "lastpost";
							break;
					}


					strSQL.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE ", BaseConfigs.GetTablePrefix, posttableid, orderfield);
					if (!searchforumid.Equals(""))
					{
						strSQL.AppendFormat("[{0}posts{1}].[fid] IN (", BaseConfigs.GetTablePrefix, posttableid);
						strSQL.Append(searchforumid);
						strSQL.Append(") AND ");
					}
					if (posterid != -1)
					{
						strSQL.AppendFormat("[{0}posts{1}].[posterid]=", BaseConfigs.GetTablePrefix, posttableid);
						strSQL.Append(posterid);
						strSQL.Append(" AND ");
					}

					if (searchtime != 0)
					{
						strSQL.AppendFormat("[{0}posts{1}].[postdatetime]", BaseConfigs.GetTablePrefix, posttableid);
						if (searchtimetype == 1)
						{
							strSQL.Append("<");
						}
						else
						{
							strSQL.Append(">'");
						}
						strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
						strSQL.Append("'AND ");
					}

					string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
					strKeyWord = new StringBuilder();
					for (int i = 0; i < keywordlist.Length; i++)
					{
						strKeyWord.Append(" OR ");
						if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
						{
							strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, posttableid);
							strKeyWord.Append(keywordlist[i]);
							strKeyWord.Append("*\"') ");
						}
						else
						{
							strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
							strKeyWord.Append(keywordlist[i]);
							strKeyWord.Append("%' ");
						}
					}

					strSQL.Append(strKeyWord.ToString().Substring(3));
					strSQL.AppendFormat("ORDER BY [{0}topics].", BaseConfigs.GetTablePrefix);

					switch (resultorder)
					{
						case 1:
							strSQL.Append("[tid]");
							break;
						case 2:
							strSQL.Append("[replies]");
							break;
						case 3:
							strSQL.Append("[views]");
							break;
						default:
							strSQL.Append("[lastpost]");
							break;
					}
					if (resultordertype == 1)
					{
						strSQL.Append(" ASC");
					}
					else
					{
						strSQL.Append(" DESC");
					}
				}
			}
			*/
		   #endregion

		   if (sql == string.Empty)
		   {
			   return -1;
		   }

		   IDataParameter[] prams2 = {
									  DbHelper.MakeInParam("?searchstring",(DbType)MySqlDbType.VarChar,255, sql),
									  DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,userid),
									  DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32,4,usergroupid)
								  };
		   int searchid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format(@"SELECT `searchid` FROM `{0}searchcaches` WHERE `searchstring`=?searchstring AND `groupid`=?groupid LIMIT 1", BaseConfigs.GetTablePrefix), prams2), -1);

		   if (searchid > -1)
		   {
			   return searchid;
		   }

		   IDataReader reader;
		   try
		   {
			   reader = DbHelper.ExecuteReader(CommandType.Text, sql);
		   }
		   catch
		   {
			   ConfirmFullTextEnable();
			   reader = DbHelper.ExecuteReader(CommandType.Text, sql);
		   }

		   int rowcount = 0;
		   if (reader != null)
		   {
			   switch (searchType)
			   {
				   case SearchType.All:
				   case SearchType.DigestTopic:
				   case SearchType.TopicTitle:
				   case SearchType.PostTitle:
				   case SearchType.PostContent:
					   strTids.Append("<ForumTopics>");
					   break;
				   case SearchType.SpacePostTitle:
					   strTids.Append("<SpacePosts>");
					   break;
				   case SearchType.AlbumTitle:
					   strTids.Append("<Albums>");
					   break;
				   case SearchType.ByPoster:
					   strTids = GetSearchByPosterResult(reader);
					   SearchCacheInfo cacheinfo = new SearchCacheInfo();
					   cacheinfo.Keywords = keyword;
					   cacheinfo.Searchstring = sql;
					   cacheinfo.Postdatetime = Utils.GetDateTime();
					   cacheinfo.Topics = rowcount;
					   cacheinfo.Tids = strTids.ToString();
					   cacheinfo.Uid = userid;
					   cacheinfo.Groupid = usergroupid;
					   cacheinfo.Ip = DNTRequest.GetIP();
					   cacheinfo.Expiration = Utils.GetDateTime();

					   reader.Close();

					   return CreateSearchCache(cacheinfo);

			   }
			   while (reader.Read())
			   {
				   strTids.Append(reader[0].ToString());
				   strTids.Append(",");
				   rowcount++;
			   }
			   if (rowcount > 0)
			   {
				   strTids.Remove(strTids.Length - 1, 1);
				   switch (searchType)
				   {
					   case SearchType.All:
					   case SearchType.DigestTopic:
					   case SearchType.TopicTitle:
					   case SearchType.PostTitle:
					   case SearchType.PostContent:
						   strTids.Append("</ForumTopics>");
						   break;
					   case SearchType.SpacePostTitle:
						   strTids.Append("</SpacePosts>");
						   break;
					   case SearchType.AlbumTitle:
						   strTids.Append("</Albums>");
						   break;

				   }
				   SearchCacheInfo cacheinfo = new SearchCacheInfo();
				   cacheinfo.Keywords = keyword;
				   cacheinfo.Searchstring = sql;
				   cacheinfo.Postdatetime = Utils.GetDateTime();
				   cacheinfo.Topics = rowcount;
				   cacheinfo.Tids = strTids.ToString();
				   cacheinfo.Uid = userid;
				   cacheinfo.Groupid = usergroupid;
				   cacheinfo.Ip = DNTRequest.GetIP();
				   cacheinfo.Expiration = Utils.GetDateTime();

				   reader.Close();

				   return CreateSearchCache(cacheinfo);
			   }
			   reader.Close();
		   }
		   return -1;
	   }     

	   public string RestoreDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
	   { return ""; }

	   public string BackUpDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
	   {
		   //           IDataReader ddr = null;
           
		   //           ddr = DbHelper.ExecuteReader(CommandType.Text,"show tables");
		   //           while(ddr.Read())
		   //           {

		   //string creattable = null;
		   //               IDataReader ddrs = null;
 
		   //             ddrs=DbHelper.ExecuteReader(CommandType.Text,"show create table "+ddr["Tables_in_dnt_gbk"].ToString()+"");
		   //       while(ddrs.Read())
		   //       {
		   //               creattable=ddrs["Create Table"].ToString();
              
		   //       }   
        

		   //           IDataReader inserttext = null;
		   //           inserttext=DbHelper.ExecuteReader(CommandType.Text, "select * from " + ddr["Tables_in_dnt_gbk"].ToString());
		   //               //while(inserttext.FieldCount)
		   //               //{
		   //               //inserttext.GetName()
                
		   //               //}
		   //           string filed = null;
		   //            string lastsql=null;
		   //               for (int i = 0; i < inserttext.FieldCount; i++)
		   //               {

		   //                   filed = filed + inserttext.GetName(i) + ",";

		   //               }
		   //               //filed = filed.Substring(0, filed.Length - 1);

                
              
		   //               while(inserttext.Read())
		   //               {  string insertdata = null;
               
		   //                   for(int i=0;i<inserttext.FieldCount;i++)
		   //               {

		   //                   insertdata = insertdata + inserttext[i].ToString()+",";
		   //                   //filed = filed + inserttext.GetName(i) + ",";
                
		   //               } 
		   //                   insertdata = insertdata.Substring(0, insertdata.Length - 1);
		   //          lastsql=lastsql+"("+insertdata+")"+",";
                
		   //               }

		   //               lastsql = lastsql.Substring(0, lastsql.Length - 1);
		   //               filed = filed.Substring(0, filed.Length - 1);
             

		   //               string backstring = "insert into " + ddr["Tables_in_dnt_gbk"].ToString() + "(" + filed + ") values"+lastsql;

		   //               if (File.Exists("d:\\a.txt"))
		   //               {

		   //                   File.Open("d:\\a.txt");

		   //               }
		   //               else
		   //               { 
                
                
		   //               }

		   //            }



		   return "";
	   }


	   //   public string BackUpDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
	   //{
	   //    SQLServer svr = new SQLServerClass();
	   //    try
	   //    {
	   //        svr.Connect(ServerName, UserName, Password);
	   //        Backup bak = new BackupClass();
	   //        bak.Action = 0;
	   //        bak.Initialize = true;
	   //        bak.Files = backuppath + strFileName + ".config";
	   //        bak.Database = strDbName;
	   //        bak.SQLBackup(svr);
	   //        return string.Empty;
	   //    }
	   //    catch(Exception ex)
	   //    {
	   //        string message = ex.Message.Replace("'", " ");
	   //        message = message.Replace("\n", " ");
	   //        message = message.Replace("\\", "/");
	   //        return message;
	   //    }
	   //    finally
	   //    {
	   //        svr.DisConnect();
	   //    }
	   //}


	   //public string RestoreDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
	   //{
	   //    #region 数据库的恢复的代码

	   //    SQLServer svr = new SQLServerClass();
	   //    try
	   //    {
	   //        svr.Connect(ServerName, UserName, Password);
	   //        QueryResults qr = svr.EnumProcesses(-1);
	   //        int iColPIDNum = -1;
	   //        int iColDbName = -1;
	   //        for (int i = 1; i <= qr.Columns; i++)
	   //        {
	   //            string strName = qr.get_ColumnName(i);
	   //            if (strName.ToUpper().Trim() == "SPID")
	   //            {
	   //                iColPIDNum = i;
	   //            }
	   //            else if (strName.ToUpper().Trim() == "DBNAME")
	   //            {
	   //                iColDbName = i;
	   //            }
	   //            if (iColPIDNum != -1 && iColDbName != -1)
	   //                break;
	   //        }

	   //        for (int i = 1; i <= qr.Rows; i++)
	   //        {
	   //            int lPID = qr.GetColumnLong(i, iColPIDNum);
	   //            string strDBName = qr.GetColumnString(i, iColDbName);
	   //            if (strDBName.ToUpper() == strDbName.ToUpper())
	   //                svr.KillProcess(lPID);
	   //        }


	   //        Restore res = new RestoreClass();
	   //        res.Action = 0;
	   //        string path = backuppath + strFileName + ".config";
	   //        res.Files = path;

	   //        res.Database = strDbName;
	   //        res.ReplaceDatabase = true;
	   //        res.SQLRestore(svr);

	   //        return string.Empty;
	   //    }
	   //    catch (Exception err)
	   //    {
	   //        string message = err.Message.Replace("'", " ");
	   //        message = message.Replace("\n", " ");
	   //        message = message.Replace("\\", "/");

	   //        return message;
	   //    }
	   //    finally
	   //    {
	   //        svr.DisConnect();
	   //    }

	   //    #endregion
	   //}
       

	   public string SearchVisitLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
	   {
		   string sqlstring = null;
		   sqlstring += " `visitid`>0";

		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`>='" + postdatetimeStart + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`<='" + postdatetimeEnd.AddDays(1).ToString() + "'";
		   }

		   if (others != "")
		   {
			   sqlstring += " And `others` LIKE '%" + RegEsc(others) + "%'";
		   }

		   if (Username != "")
		   {
			   sqlstring += " And (";
			   foreach (string word in Username.Split(','))
			   {
				   if (word.Trim() != "")
					   sqlstring += " `username` like '%" + RegEsc(word) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   return sqlstring;

	   }

	   public string SearchMedalLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string reason)
	   {
		   string sqlstring = null;
		   sqlstring += " `id`>0";

		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`>='" + postdatetimeStart.ToString() + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`<='" + postdatetimeEnd.AddDays(1).ToString() + "'";
		   }

		   if (reason != "")
		   {
			   sqlstring += " And `reason` LIKE '%" + RegEsc(reason) + "%'";
		   }

		   if (Username != "")
		   {
			   sqlstring += " And (";
			   foreach (string word in Username.Split(','))
			   {
				   if (word.Trim() != "")
					   sqlstring += " `username` like '%" + RegEsc(word) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }
		   return sqlstring;
	   }

	   public string SearchModeratorManageLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
	   {
		   string sqlstring = null;
		   sqlstring += " `id`>0";

		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`>='" + postdatetimeStart.ToString() + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`<='" + postdatetimeEnd.AddDays(1).ToString() + "'";
		   }

		   if (others != "")
		   {
			   sqlstring += " And `reason` LIKE '%" + RegEsc(others) + "%'";
		   }

		   if (Username != "")
		   {
			   sqlstring += " And (";
			   foreach (string word in Username.Split(','))
			   {
				   if (word.Trim() != "")
					   sqlstring += " `moderatorname` like '%" + RegEsc(word) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   return sqlstring;
	   }

	   public string SearchPaymentLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username)
	   {
		   string sqlstring = null;
		   sqlstring += " `" + BaseConfigs.GetTablePrefix + "paymentlog`.`id`>0";

		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " And `" + BaseConfigs.GetTablePrefix + "paymentlog`.`buydate`>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " And `" + BaseConfigs.GetTablePrefix + "paymentlog`.`buydate`<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }

		   if (Username != "")
		   {
			   string usernamesearch = " WHERE (";
			   foreach (string word in Username.Split(','))
			   {
				   if (word.Trim() != "")
					   usernamesearch += " `username` like '%" + RegEsc(word) + "%' OR ";
			   }
			   usernamesearch = usernamesearch.Substring(0, usernamesearch.Length - 3) + ")";

			   //找出当前用户名所属的UID
			   DataTable dt = DbHelper.ExecuteDataset("SELECT `uid` From `" + BaseConfigs.GetTablePrefix + "users` " + usernamesearch).Tables[0];
			   string uid = "-1";
			   if (dt.Rows.Count > 0)
			   {
				   foreach (DataRow dr in dt.Rows)
				   {
					   uid += "," + dr["uid"].ToString();
				   }
			   }
			   sqlstring += " And `" + BaseConfigs.GetTablePrefix + "paymentlog`.`uid` IN(" + uid + ")";

		   }

		   return sqlstring;
	   }

	   public string SearchRateLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
	   {
		   string sqlstring = null;
		   sqlstring += " `id`>0";

		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`>='" + postdatetimeStart.ToString() + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`<='" + postdatetimeEnd.AddDays(1).ToString() + "'";
		   }

		   if (others != "")
		   {
			   sqlstring += " And `reason` LIKE '%" + RegEsc(others) + "%'";
		   }

		   if (Username != "")
		   {
			   sqlstring += " And (";
			   foreach (string word in Username.Split(','))
			   {
				   if (word.Trim() != "")
					   sqlstring += " `username` like '%" + RegEsc(word) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   return sqlstring;
	   }

	   public string DeletePrivateMessages(bool isnew, string postdatetime, string msgfromlist, bool lowerupper, string subject, string message, bool isupdateusernewpm)
	   {
		   string sqlstring = null;
		   sqlstring += "WHERE `pmid`>0";

		   if (isnew)
		   {
			   sqlstring += " AND `new`=0";
		   }

		   if (postdatetime != "")
		   {
			   sqlstring += " And datediff(postdatetime,NOW())>="+postdatetime;
		   }

		   if (msgfromlist != "")
		   {
			   sqlstring += " And (";
			   foreach (string msgfrom in msgfromlist.Split(','))
			   {
				   if (msgfrom.Trim() != "")
				   {
					   if (lowerupper)
					   {
						   sqlstring += " `msgfrom`='" + msgfrom + "' OR";
					   }
					   else
					   {
						   sqlstring += " `msgfrom` COLLATE Chinese_PRC_CS_AS_WS ='" + msgfrom + "' OR";

					   }
				   }
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   if (subject != "")
		   {
			   sqlstring += " And (";
			   foreach (string sub in subject.Split(','))
			   {
				   if (sub.Trim() != "")
					   sqlstring += " `subject` like '%" + RegEsc(sub) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   if (message != "")
		   {
			   sqlstring += " And (";
			   foreach (string mess in message.Split(','))
			   {
				   if (mess.Trim() != "")
					   sqlstring += " `message` like '%" + RegEsc(mess) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   if (isupdateusernewpm)
		   {
			   DbHelper.ExecuteNonQuery("UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `newpm`=0 WHERE `uid` IN (SELECT `msgtoid` FROM `" + BaseConfigs.GetTablePrefix + "pms` " + sqlstring + ")");
		   }

		   DbHelper.ExecuteNonQuery("DELETE FROM `" + BaseConfigs.GetTablePrefix + "pms` " + sqlstring);

		   return sqlstring;
	   }

	   public bool IsExistSmilieCode(string code, int currentid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?code",(DbType)MySqlDbType.VarChar,30,code),
									 DbHelper.MakeInParam("?currentid",(DbType)MySqlDbType.Int32,4,currentid)
								 };
		   string sql = "SELECT `id` FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE code=?code AND id<>?currentid";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count != 0;
	   }

	   public string GetSmilieByType(int id)
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE TYPE=" + id;
	   }

	   public string AddTableData()
	   {

		   return "SELECT `groupid`, `grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`<=3 ORDER BY `groupid`";
        
	   }

	   public string Global_UserGrid_GetCondition(string getstring)
	   {

		   return "`" + BaseConfigs.GetTablePrefix + "users`.`username`='" + getstring + "'";

	   }

	   public int Global_UserGrid_RecordCount()
	   {

		   return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT  Count(uid) From `" + BaseConfigs.GetTablePrefix + "users`").Tables[0].Rows[0][0].ToString());

	   }


	   public int Global_UserGrid_RecordCount(string condition)
	   {

		   return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT  Count(uid) From `" + BaseConfigs.GetTablePrefix + "users`  WHERE " + condition).Tables[0].Rows[0][0].ToString());

	   }

	   public string Global_UserGrid_SearchCondition(bool islike, bool ispostdatetime, string username, string nickname, string UserGroup, string email, string credits_start, string credits_end, string lastip, string posts, string digestposts, string uid, string joindateStart, string joindateEnd)
	   {

		   string searchcondition = " `" + BaseConfigs.GetTablePrefix + "users`.`uid`>0 ";
		   if (islike)
		   {
			   if (username != "") searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`username` like'%" + RegEsc(username) + "%'";
			   if (nickname != "") searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`nickname` like'%" + RegEsc(nickname) + "%'";
		   }
		   else
		   {
			   if (username != "") searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`username` ='" + username + "'";
			   if (nickname != "") searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`nickname` ='" + nickname + "'";
		   }

		   if (UserGroup!= "0")
		   {
			   searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`groupid`=" + UserGroup;
		   }

		   if (email != "")
		   {
			   searchcondition += " AND `" + BaseConfigs.GetTablePrefix + "users`.`email` LIKE '%" + RegEsc(email) + "%'";
		   }

		   if (credits_start != "")
		   {
			   searchcondition += " AND `" + BaseConfigs.GetTablePrefix + "users`.`credits` >=" + credits_start;
		   }

		   if (credits_end != "")
		   {
			   searchcondition += " AND `" + BaseConfigs.GetTablePrefix + "users`.`credits` <=" + credits_end;
		   }

		   if (lastip != "")
		   {
			   searchcondition += " AND `" + BaseConfigs.GetTablePrefix + "users`.`lastip` LIKE '%" + RegEsc(lastip) + "%'";
		   }

		   if (posts != "")
		   {
			   searchcondition += " AND `" + BaseConfigs.GetTablePrefix + "users`.`posts` >=" + posts;
		   }


		   if (digestposts != "")
		   {
			   searchcondition += " AND `" + BaseConfigs.GetTablePrefix + "users`.`digestposts` >=" + digestposts;
		   }

		   if (uid != "")
		   {
			   uid = uid.Replace(", ", ",");

			   if (uid.IndexOf(",") == 0)
			   {
				   uid = uid.Substring(1, uid.Length - 1);
			   }
			   if (uid.LastIndexOf(",") == (uid.Length - 1))
			   {
				   uid = uid.Substring(0, uid.Length - 1);
			   }

			   if (uid != "")
			   {
				   searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`uid` IN(" + uid + ")";
			   }

		   }

		   if (ispostdatetime)
		   {
			   searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`joindate` >='" + DateTime.Parse(joindateStart).ToString("yyyy-MM-dd HH:mm:ss") + "'";
			   searchcondition += " And `" + BaseConfigs.GetTablePrefix + "users`.`joindate` <='" + DateTime.Parse(joindateStart).ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }


		   return searchcondition;

	   }


	   public DataTable Global_UserGrid_Top2(string searchcondition)

	   {
		   string sql = "SELECT `" + BaseConfigs.GetTablePrefix + "users`.`uid`  FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE " + searchcondition + " LIMIT 0,"+2;
		   return   DbHelper.ExecuteDataset(sql).Tables[0];
        
	   }



	   public System.Collections.ArrayList CheckDbFree()
	   {
		   System.Collections.ArrayList tablelist = new System.Collections.ArrayList();
		   if (DbHelper.Provider.IsDbOptimize() == true)
		   {
			   IDataReader ddr = null;




			   //ddr = DbHelper.ExecuteDataset("SHOW TABLE STATUS").Tables[0];
			   ddr = DbHelper.ExecuteReader(CommandType.Text, "SHOW TABLE STATUS");

			   if (ddr==null)
			   {
				   return null;

			   }


			   //string dbname = string.Format("Tables_in_{0}gbk", BaseConfigs.GetTablePrefix);
			   while (ddr.Read())
			   {

				   if (int.Parse(ddr["Data_free"].ToString()) != 0)
				   {
					   MySqlInfo msi = new MySqlInfo();
					   msi.tablename = ddr["Name"].ToString();
					   msi.tabletype = ddr["Engine"].ToString();
					   msi.rowcount = ddr["Rows"].ToString();
					   msi.tabledata = ddr["Data_length"].ToString();
					   msi.index = ddr["Index_length"].ToString();
					   msi.datafree = ddr["Data_free"].ToString();


					   tablelist.Add(msi);

				   }

			   }
			   ddr.Close();
			   //foreach (string s in tablelist)


		   }
		   return tablelist;
	   }

	   public void DbOptimize(string tablelist)
	   {
		   string[] tables = tablelist.Split(',');
		   for (int i = 0; i < tables.Length; i++)
		   {


			   DbHelper.ExecuteNonQuery("OPTIMIZE TABLE `" + tables[i].ToString() + "`");

		   }
            
           

	   }


	   //public string AnnounceSerachBind(string poster, string title, string postdatetimeStart, string postdatetimeEnd)
	   //{


	   //    StringBuilder builder = new StringBuilder();
	   //    if (!poster.Equals(""))
	   //    {
	   //        builder.Append("`poster` LIKE '%");
	   //        builder.Append(poster);
	   //        builder.Append("%'");
	   //    }

	   //    if (!title.Equals(""))
	   //    {
	   //        if (builder.Length > 0)
	   //        {
	   //            builder.Append(" AND ");
	   //        }
	   //        builder.Append("`title` LIKE '%");
	   //        builder.Append(title);
	   //        builder.Append("%'");
	   //    }

	   //    if (!postdatetimeStart.Equals(""))
	   //    {
	   //        if (builder.Length > 0)
	   //        {
	   //            builder.Append(" AND ");
	   //        }
	   //        builder.Append("`starttime` >= '");
	   //        builder.Append(postdatetimeStart);
	   //        builder.Append("'");
	   //    }

	   //    if (!postdatetimeEnd.Equals(""))
	   //    {
	   //        if (builder.Length > 0)
	   //        {
	   //            builder.Append(" AND ");
	   //        }
	   //        builder.Append("`starttime` <= '");
	   //        builder.Append(postdatetimeEnd);
	   //        builder.Append("'");
	   //    }

	   //    if (builder.Length > 0)
	   //    {
	   //        builder.Insert(0, " WHERE ");
	   //    }


	   //    return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "announcements` " + builder.ToString();
	   //}

	   public void UpdateAdminUsergroup(string targetadminusergroup, string sourceadminusergroup)
	   {

		   DbHelper.ExecuteNonQuery("Update `" + BaseConfigs.GetTablePrefix + "users` SET `groupid`=" + targetadminusergroup + " Where `groupid`=" + sourceadminusergroup);
    
	   }


	   public void UpdateUserCredits(string formula)
	   {
		   DbHelper.ExecuteNonQuery("UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `credits`=" + formula);
        
	   }

	   public DataTable MailListTable(string usernamelist)
	   {

		   string strwhere = " WHERE `Email` Is Not null AND (";
		   foreach (string username in usernamelist.Split(','))
		   {
			   if (username.Trim() != "")
				   strwhere += " `username` like '%" + RegEsc(username.Trim()) + "%' OR ";
		   }
		   strwhere = strwhere.Substring(0, strwhere.Length - 3) + ")";

		   DataTable dt = DbHelper.ExecuteDataset("SELECT `username`,`Email`  From `" + BaseConfigs.GetTablePrefix + "users` " + strwhere).Tables[0];
		   return dt;
	   }


	   public void DeleteSmilyByType(int type)
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "smilies` WHERE `type`=" + type;
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   #endregion


	   #region HelpManage

	   //取得分类
	   public IDataReader GetHelpList(int id)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id),

		   };
		   string sql = "SELECT `id`,`title`,`message`,`pid`,`orderby` FROM `" + BaseConfigs.GetTablePrefix + "help` WHERE `pid`=?id OR `id`=?id ORDER BY `pid` ASC,`orderby` ASC";

		   return DbHelper.ExecuteReader(CommandType.Text, sql, parms);


	   }


	   public IDataReader ShowHelp(int id)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id),

		   };
		   string sql = "SELECT `title`,`message`,`pid`,`orderby` FROM `" + BaseConfigs.GetTablePrefix + "help` WHERE `id`=?id";
		   return DbHelper.ExecuteReader(CommandType.Text, sql, parms);

	   }


	   public IDataReader GetHelpClass()
	   {

		   string sql = "SELECT `id` FROM `" + BaseConfigs.GetTablePrefix + "help` WHERE `pid`=0 ORDER BY `orderby` ASC";
		   return DbHelper.ExecuteReader(CommandType.Text, sql);
	   }



	   public void AddHelp(string title, string message, int pid, int orderby)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.String, 100, title),
									 DbHelper.MakeInParam("?message", (DbType)MySqlDbType.VarChar, 100,message),
									 DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32,4, pid),
									 DbHelper.MakeInParam("?orderby", (DbType)MySqlDbType.Int32, 4, orderby)

								 };
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "help`(`title`,`message`,`pid`,`orderby`) VALUES(?title,?message,?pid,?orderby)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public void DelHelp(string idlist)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?idlist", (DbType)MySqlDbType.String, 100, idlist)


								 };

		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "help` WHERE `id` IN (?idlist) OR `pid` IN (?idlist)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public void ModHelp(int id, string title, string message, int pid, int orderby)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.String, 100, title),
									 DbHelper.MakeInParam("?message", (DbType)MySqlDbType.VarChar, 100,message),
									 DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32,4, pid),
									 DbHelper.MakeInParam("?orderby", (DbType)MySqlDbType.Int32, 4, orderby),
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id)

								 };

		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "help` SET `title`=?title,`message`=?message,`pid`=?pid,`orderby`=?orderby WHERE `id`=?id";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

	   }


	   public int HelpCount()
	   {
		   string sql = "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "help`";
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());

	   }

	   public string BindHelpType()
	   {
		   string sql = "SELECT `id`,`title` FROM `" + BaseConfigs.GetTablePrefix + "help` WHERE `pid`=0 ORDER BY `orderby` ASC";
		   return sql;

	   }

	   public void UpOrder(string orderby, string id)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?orderby", (DbType)MySqlDbType.VarChar, 100, orderby),
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.VarChar, 100,id),
                                        
                                        
		   };

		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "help` SET `ORDERBY`=?orderby  Where id=?id";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

	   }

	   #endregion


	   #region PostManage

	   public string SearchTopicAudit(int fid, string poster, string title, string moderatorname, DateTime postdatetimeStart, DateTime postdatetimeEnd, DateTime deldatetimeStart, DateTime deldatetimeEnd)
	   {
		   string sqlstring = null;
		   sqlstring += " `displayorder`<0";

		   if (fid != 0)
		   {
			   sqlstring += " AND `fid`=" + fid;
		   }

		   if (poster != "")
		   {
			   sqlstring += " AND (";
			   foreach (string postername in poster.Split(','))
			   {
				   if (postername.Trim() != "")
				   {
					   sqlstring += " poster='" + postername + "'  OR";
				   }
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }


		   if (title != "")
		   {
			   sqlstring += " AND (";
			   foreach (string titlename in title.Split(','))
			   {
				   if (titlename.Trim() != "")
				   {
					   sqlstring += " title like '%" + RegEsc(titlename) + "%' OR";
				   }
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }


		   if (moderatorname != "")
		   {
			   string logtidlist = "";
			   //DataTable dt = DbHelper.ExecuteDataset("SELECT `title`	FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE (moderatorname = '" + moderatorname + "') AND (actions = 'DELETE')").Tables[0];
			   DataTable dt = DatabaseProvider.GetInstance().GetTitleForModeratormanagelogByModeratorname(moderatorname);
			   if (dt.Rows.Count > 0)
			   {
				   foreach (DataRow dr in dt.Rows)
				   {
					   logtidlist += dr["title"].ToString() + ",";
				   }
				   sqlstring = sqlstring + " And tid IN (" + logtidlist.Substring(0, logtidlist.Length - 1) + ") ";
			   }
		   }

		   if (postdatetimeStart.ToString().IndexOf("1900") < 0)
		   {
			   sqlstring += " And `postdatetime`>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }

		   if (postdatetimeEnd.ToString().IndexOf("1900") < 0)
		   {
			   sqlstring += " And `postdatetime`<='" + postdatetimeEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }

		   if ((deldatetimeStart.ToString().IndexOf("1900") < 0) && (deldatetimeStart.ToString().IndexOf("1900") < 0))
		   {
			   string logtidlist2 = "";
			   //DataTable dt = DbHelper.ExecuteDataset("SELECT `title`	FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE (postdatetime >= '" + deldatetimeStart.SelectedDate.ToString() + "') AND (postdatetime<='" + deldatetimeEnd.SelectedDate.ToString() + "')AND (actions = 'DELETE')").Tables[0];
			   DataTable dt = DatabaseProvider.GetInstance().GetTitleForModeratormanagelogByPostdatetime(deldatetimeStart, deldatetimeStart);
			   if (dt.Rows.Count > 0)
			   {
				   foreach (DataRow dr in dt.Rows)
				   {
					   logtidlist2 += dr["title"].ToString() + ",";
				   }
				   sqlstring = sqlstring + " And tid IN (" + logtidlist2.Substring(0, logtidlist2.Length - 1) + ") ";
			   }
		   }
		   return sqlstring;
	   }

	   public void AddBBCCode(int available, string tag, string icon, string replacement, string example,
		   string explanation, string param, string nest, string paramsdescript, string paramsdefvalue)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available),
				DbHelper.MakeInParam("?tag", (DbType)MySqlDbType.VarString, 100, tag),
				DbHelper.MakeInParam("?icon", (DbType)MySqlDbType.VarString,50, icon),
				DbHelper.MakeInParam("?replacement", (DbType)MySqlDbType.VarString,0, replacement),
				DbHelper.MakeInParam("?example", (DbType)MySqlDbType.VarString, 255, example),
				DbHelper.MakeInParam("?explanation", (DbType)MySqlDbType.VarString, 0, explanation),
				DbHelper.MakeInParam("?params", (DbType)MySqlDbType.Int32, 4, param),
				DbHelper.MakeInParam("?nest", (DbType)MySqlDbType.Int32, 4, nest),
				DbHelper.MakeInParam("?paramsdescript", (DbType)MySqlDbType.VarString, 0, paramsdescript),
				DbHelper.MakeInParam("?paramsdefvalue", (DbType)MySqlDbType.VarString, 0, paramsdefvalue)
			};
		   string sql = "INSERT INTO  `" + BaseConfigs.GetTablePrefix + "bbcodes` (`available`,`tag`,`icon`,`replacement`,`example`," +
			   "`explanation`,`params`,`nest`,`paramsdescript`,`paramsdefvalue`) VALUES(?available,?tag,?icon,?replacement,?example,?explanation,?params," +
			   "?nest,?paramsdescript,?paramsdefvalue)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }


	   /// <summary>
	   /// 产生附件
	   /// </summary>
	   /// <param name="attachmentinfo">附件描述类实体</param>
	   /// <returns>附件id</returns>
	   public int CreateAttachment(AttachmentInfo attachmentinfo)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,attachmentinfo.Uid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,attachmentinfo.Tid),
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,attachmentinfo.Pid),
									 DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Datetime, 8, DateTime.Parse(attachmentinfo.Postdatetime)),
									 DbHelper.MakeInParam("?readperm",(DbType)MySqlDbType.Int32,4,attachmentinfo.Readperm),
									 DbHelper.MakeInParam("?filename",(DbType)MySqlDbType.VarString,100,attachmentinfo.Filename),
									 DbHelper.MakeInParam("?description",(DbType)MySqlDbType.VarString,100,attachmentinfo.Description),
									 DbHelper.MakeInParam("?filetype",(DbType)MySqlDbType.VarString,50,attachmentinfo.Filetype),
									 DbHelper.MakeInParam("?filesize",(DbType)MySqlDbType.Int32,4,attachmentinfo.Filesize),
									 DbHelper.MakeInParam("?attachment",(DbType)MySqlDbType.VarString,100,attachmentinfo.Attachment),
									 DbHelper.MakeInParam("?downloads",(DbType)MySqlDbType.Int32,4,attachmentinfo.Downloads)
								 };

		   int id = 0,aid=0;
		   String sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "attachments`(`uid`,`tid`, `pid`, `postdatetime`, `readperm`, `filename`, `description`, `filetype`, `filesize`, `attachment`, `downloads`) VALUES(?uid, ?tid, ?pid, ?postdatetime, ?readperm, ?filename, ?description, ?filetype, ?filesize, ?attachment, ?downloads)";
		   DbHelper.ExecuteNonQuery(out id,CommandType.Text, sql, prams);
		   aid = id;
		   IDataParameter[] prams1 ={
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,attachmentinfo.Pid)
								 };

		   String upsql = "UPDATE `" + BaseConfigs.GetTablePrefix + "posts" + Posts.GetPostTableID(attachmentinfo.Tid) + "` SET `attachment`=1 WHERE `pid`=?pid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, upsql, prams1);

		   return aid;
	   }

	   /// <summary>
	   /// 更新主题附件类型
	   /// </summary>
	   /// <param name="tid">主题Id</param>
	   /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
	   /// <returns></returns>
	   public int UpdateTopicAttachmentType(int tid, int attType)
	   {
		   IDataParameter[] parm = {
									DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								};
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}topics` SET `attachment`={1} WHERE `tid`=?tid", BaseConfigs.GetTablePrefix, attType), parm);
	   }

	   /// <summary>
	   /// 更新帖子附件类型
	   /// </summary>
	   /// <param name="pid">帖子Id</param>
	   /// <param name="postTableId">所在帖子表Id</param>
	   /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
	   /// <returns></returns>
	   public int UpdatePostAttachmentType(int pid, string postTableId, int attType)
	   {
		   IDataParameter[] parm = {
									DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,pid)
								};
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}posts{1}` SET `attachment`={2} WHERE `pid`=?pid", BaseConfigs.GetTablePrefix, postTableId, attType), parm);
	   }

	   /// <summary>
	   /// 获取指定附件信息
	   /// </summary>
	   /// <param name="aid">附件Id</param>
	   /// <returns></returns>
	   public IDataReader GetAttachmentInfo(int aid)
	   {
		   IDataParameter[] parm = {
									DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32,4, aid),
		   };

		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM `{0}attachments` WHERE `aid`=?aid LIMIT 1", BaseConfigs.GetTablePrefix), parm);
	   }

	   /// <summary>
	   /// 获得指定帖子的附件个数
	   /// </summary>
	   /// <param name="pid">帖子ID</param>
	   /// <returns>附件个数</returns>
	   public int GetAttachmentCountByPid(int pid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,pid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`aid`) AS `acount` FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `pid`=?pid", prams), 0);
	   }

	   /// <summary>
	   /// 获得指定主题的附件个数
	   /// </summary>
	   /// <param name="tid">主题ID</param>
	   /// <returns>附件个数</returns>
	   public int GetAttachmentCountByTid(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`aid`) AS `acount` FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `tid`=?tid", prams), 0);
	   }

	   /// <summary>
	   /// 获得指定帖子的附件
	   /// </summary>
	   /// <param name="pid">帖子ID</param>
	   /// <returns>帖子信息</returns>
	   public DataTable GetAttachmentListByPid(int pid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,pid)
								 };
		   DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `pid`=?pid", prams);
		   if (ds != null)
		   {
			   if (ds.Tables.Count > 0)
			   {
				   return ds.Tables[0];
			   }
		   }
		   return new DataTable();
	   }

	   /// <summary>
	   /// 获得系统设置的附件类型
	   /// </summary>
	   /// <returns>系统设置的附件类型</returns>
	   public DataTable GetAttachmentType()
	   {
		   DataTable dt = new DataTable();
		   DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT `id`, `extension`, `maxsize` FROM `{0}attachtypes`", BaseConfigs.GetTablePrefix));
		   if (ds != null)
		   {
			   dt = ds.Tables[0];
		   }

		   return dt;
	   }

	   /// <summary>
	   /// 更新附件下载次数
	   /// </summary>
	   /// <param name="aid">附件id</param>
	   public void UpdateAttachmentDownloads(int aid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?aid",(DbType)MySqlDbType.Int32,4,aid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}attachments` SET `downloads`=`downloads`+1 WHERE `aid`=?aid", BaseConfigs.GetTablePrefix), prams);
	   }

	   /// <summary>
	   /// 更新主题是否包含附件
	   /// </summary>
	   /// <param name="tid">主题Id</param>
	   /// <param name="hasAttachment">是否包含附件,0不包含,1包含</param>
	   /// <returns></returns>
	   public int UpdateTopicAttachment(int tid, int hasAttachment)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}topics` SET `attachment`={1} WHERE `tid`=?tid", BaseConfigs.GetTablePrefix, hasAttachment), prams);
	   }

	   /// <summary>
	   /// 获得指定主题的所有附件
	   /// </summary>
	   /// <param name="tid">主题Id</param>
	   /// <returns></returns>
	   public IDataReader GetAttachmentListByTid(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT `aid`,`filename` FROM `{0}attachments` WHERE `tid`=?tid ", BaseConfigs.GetTablePrefix), prams);

	   }

	   /// <summary>
	   /// 获得指定主题的所有附件
	   /// </summary>
	   /// <param name="tidlist">主题Id列表，以英文逗号分割</param>
	   /// <returns></returns>
	   public IDataReader GetAttachmentListByTid(string tidlist)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT `aid`,`filename` FROM `{0}attachments` WHERE `tid` IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
	   }

	   /// <summary>
	   /// 删除指定主题的所有附件
	   /// </summary>
	   /// <param name="tid">版块tid</param>
	   /// <returns>删除个数</returns>
	   public int DeleteAttachmentByTid(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}attachments` WHERE `tid`=?tid ", BaseConfigs.GetTablePrefix), prams);
	   }

	   /// <summary>
	   /// 删除指定主题的所有附件
	   /// </summary>
	   /// <param name="tid">主题Id列表，以英文逗号分割</param>
	   /// <returns>删除个数</returns>
	   public int DeleteAttachmentByTid(string tidlist)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}attachments` WHERE `tid` IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
	   }

	   /// <summary>
	   /// 删除指定附件
	   /// </summary>
	   /// <param name="aid"></param>
	   /// <returns></returns>
	   public int DeleteAttachment(int aid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?aid",(DbType)MySqlDbType.Int32,4,aid)
								 };

		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}attachments` WHERE `aid`=?aid", BaseConfigs.GetTablePrefix), prams);
	   }

	   /// <summary>
	   /// 批量删除附件
	   /// </summary>
	   /// <param name="aidList">附件Id列表，以英文逗号分割</param>
	   /// <returns></returns>
	   public int DeleteAttachment(string aidList)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}attachments` WHERE `aid` IN ({1})", BaseConfigs.GetTablePrefix, aidList));
	   }

	   public int UpdatePostAttachment(int pid, string postTableId, int hasAttachment)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,pid)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}posts{1}` SET `attachment`={2} WHERE `pid`=?pid", BaseConfigs.GetTablePrefix, postTableId, hasAttachment), prams);
	   }

	   /// <summary>
	   /// 根据帖子Id删除附件
	   /// </summary>
	   /// <param name="pid">帖子Id</param>
	   /// <returns></returns>
	   public int DeleteAttachmentByPid(int pid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, pid)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM `{0}attachments` WHERE `pid`=?pid", BaseConfigs.GetTablePrefix), parms);
	   }

	   /// <summary>
	   /// 更新附件信息
	   /// </summary>
	   /// <param name="attachmentInfo">附件对象</param>
	   /// <returns>返回被更新的数量</returns>
	   public int UpdateAttachment(AttachmentInfo attachmentInfo)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Datetime, 8, DateTime.Parse(attachmentInfo.Postdatetime)),
									 DbHelper.MakeInParam("?readperm", (DbType)MySqlDbType.Int32, 4, attachmentInfo.Readperm),
									 DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.VarString, 100, attachmentInfo.Filename),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 100, attachmentInfo.Description),
									 DbHelper.MakeInParam("?filetype", (DbType)MySqlDbType.VarString, 50, attachmentInfo.Filetype),
									 DbHelper.MakeInParam("?filesize", (DbType)MySqlDbType.Int32, 4, attachmentInfo.Filesize),
									 DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.VarString, 100, attachmentInfo.Attachment),
									 DbHelper.MakeInParam("?downloads", (DbType)MySqlDbType.Int32, 4, attachmentInfo.Downloads),
									 DbHelper.MakeInParam("?aid",(DbType)MySqlDbType.Int32, 4,attachmentInfo.Aid)
								 };

		   string sql = string.Format(@"UPDATE `{0}attachments` SET `postdatetime` = ?postdatetime, `readperm` = ?readperm, `filename` = ?filename, `description` = ?description, `filetype` = ?filetype, `filesize` = ?filesize, `attachment` = ?attachment, `downloads` = ?downloads
											WHERE `aid`=?aid", BaseConfigs.GetTablePrefix);

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   /// <summary>
	   /// 更新附件信息
	   /// </summary>
	   /// <param name="aid">附件Id</param>
	   /// <param name="readperm">阅读权限</param>
	   /// <param name="description">描述</param>
	   /// <returns>返回被更新的数量</returns>
	   public int UpdateAttachment(int aid, int readperm, string description)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?readperm", (DbType)MySqlDbType.Int32, 4, readperm),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 100, description),
									 DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32, 4, aid),
		   };

		   string sql = string.Format(@"UPDATE `{0}attachments `SET `readperm` = ?readperm, `description` = ?description WHERE `aid` = ?aid", BaseConfigs.GetTablePrefix);

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public IDataReader GetAttachmentList(string aidList)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT `aid`,`filename`,`tid`,`pid` FROM `{0}attachments` WHERE `aid` IN ({1})", BaseConfigs.GetTablePrefix, aidList));
	   }

	   public IDataReader GetAttachmentListByPid(string pidList)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM `{0}attachments` WHERE `pid` IN ({1})", BaseConfigs.GetTablePrefix, pidList));
	   }

	   /// <summary>
	   /// 获得上传附件文件的大小
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns></returns>
	   public int GetUploadFileSizeByUserId(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT SUM(`filesize`) as `todaysize` FROM `{0}attachments` WHERE `uid`=?uid AND DATEDIFF(`postdatetime`,NOW())=0", BaseConfigs.GetTablePrefix), prams), 0);
	   }

	   /// <summary>
	   /// 取得主题贴的第一个图片附件
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   public DataSet GetFirstImageAttachByTid(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `tid`=?tid AND LEFT(`filetype`, 5)='image' ORDER BY `aid` LIMIT 1", prams);
	   }

	   public DataSet GetAttchType()
	   {
		   string sql = "Select * From `" + BaseConfigs.GetTablePrefix + "attachtypes` Order BY `id` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql);
	   }

	   public void AddAttchType(string extension, string maxsize)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?extension", (DbType)MySqlDbType.VarString,256, extension),
				DbHelper.MakeInParam("?maxsize", (DbType)MySqlDbType.Int32, 4, maxsize)
			};
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "attachtypes` (`extension`, `maxsize`) VALUES (?extension,?maxsize)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateAttchType(string extension, string maxsize, int id)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?extension", (DbType)MySqlDbType.VarString,256, extension),
				DbHelper.MakeInParam("?maxsize", (DbType)MySqlDbType.Int32, 4, maxsize),
				DbHelper.MakeInParam("?id",(DbType)MySqlDbType.Int32, 4,id)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "attachtypes` SET `extension`=?extension ,`maxsize`=?maxsize Where `id`=?id";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void DeleteAttchType(string attchtypeidlist)
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "attachtypes` WHERE `id` IN (" + attchtypeidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public bool IsExistExtensionInAttachtypes(string extensionname)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?extension", (DbType)MySqlDbType.VarString,256, extensionname)
			};
		   string sql = "Select * From `" + BaseConfigs.GetTablePrefix + "attachtypes` WHERE `extension`=?extension LIMIT 1";
		   if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count > 0)
			   return true;
		   else
			   return false;
	   }

	   public DataTable GetTitleForModeratormanagelogByModeratorname(string moderatorname)
	   {
		   string sql = "SELECT `title` FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE (moderatorname = '" + moderatorname + "') AND (actions = 'DELETE')";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetTitleForModeratormanagelogByPostdatetime(DateTime startDateTime, DateTime endDateTime)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?startDateTime", (DbType)MySqlDbType.Datetime, 8, startDateTime),
									 DbHelper.MakeInParam("?endDateTime", (DbType)MySqlDbType.Datetime, 8, endDateTime)
								 };
		   string sql = "SELECT `title` FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE (postdatetime >= ?startDateTime) AND (postdatetime<= ?endDateTime)AND (actions = 'DELETE')";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
	   }

	   public DataTable GetTidForModeratormanagelogByPostdatetime(DateTime postDateTime)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8, postDateTime)
								 };
		   string sql = "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `displayorder`=-1 AND `postdatetime`<=?postdatetime";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
	   }


	   public string GetUnauditNewTopicSQL()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `displayorder`=-2";
	   }

	   public void PassAuditNewTopic(string postTableName, string tidlist)
	   {
		   string sqlstring = "UPDATE  `" + postTableName + "`  SET `invisible`=0 WHERE `layer`=0  AND `tid` IN(" + tidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   sqlstring = "UPDATE  `" + BaseConfigs.GetTablePrefix + "topics`  SET `displayorder`=0 WHERE `tid` IN(" + tidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totaltopic`=`totaltopic` + " + tidlist.Split(',').Length);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totalpost`=`totalpost` + " + tidlist.Split(',').Length);

		   //更新相关的版块统计信息
		   foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` IN(" + tidlist + ") ORDER BY `tid` ASC").Tables[0].Rows)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `topics` = `topics` + 1, `curtopics` = `curtopics` + 1, `posts`=`posts` + 1, `todayposts`=(IF(DATEPART(yyyy, lastpost)=DATEPART(yyyy,NOW()) AND DATEPART(mm, lastpost)=DATEPART(mm,NOW()) AND DATEPART(dd, lastpost)=DATEPART(dd,NOW()),`todayposts`*1 + 1,1)),`lasttid`=" + dr["tid"].ToString() + " ,	`lasttitle`='" + dr["title"].ToString().Replace("'", "''") + "',`lastpost`='" + dr["postdatetime"].ToString() + "',`lastposter`='" + dr["poster"].ToString().Replace("'", "''") + "',`lastposterid`=" + dr["posterid"].ToString() + " WHERE `fid`=" + dr["fid"].ToString());
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `lastpost` = '" + dr["postdatetime"].ToString() + "', `lastpostid` =" + dr["posterid"].ToString() + ", `lastposttitle` ='" + dr["title"].ToString().Replace("'", "''") + "', `posts` = `posts` + 1	WHERE `uid` = " + dr["posterid"].ToString());
		   }
	   }

	   public DataTable GetDetachTableId()
	   {
		   string sql = "SELECT ID FROM `" + BaseConfigs.GetTablePrefix + "tablelist` Order BY ID ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public int GetCurrentPostTableRecordCount(int currentPostTableId)
	   {
		   string sql = "SELECT count(pid) FROM `" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "` WHERE `invisible`=1";
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql));
	   }

	   public string GetUnauditPostSQL(int currentPostTableId)
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "` WHERE `invisible`=1 AND `layer`>0";
	   }

	   public void PassPost(int currentPostTableId, string pidlist)
	   {
		   string sqlstring = "UPDATE  `" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "`  SET `invisible`=0 WHERE `pid` IN(" + pidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totalpost`=`totalpost` + " + pidlist.Split(',').Length);

		   //更新相关的版块统计信息
		   foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "` WHERE `pid` IN(" + pidlist + ") ORDER BY `pid` ASC").Tables[0].Rows)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `posts`=`posts` + 1, `todayposts`=(IF((DATE_FORMAT(lastpost,'%Y')=DATE_FORMAT(now(),'%Y') AND DATE_FORMAT(lastpost,'%m')=DATE_FORMAT(Now(),'%m') AND DATE_FORMAT(lastpost,'%d')=DATE_FORMAT(now(),'%d'),`todayposts`*1 + 1,1)),`lastpost`='" + dr["postdatetime"].ToString() + "',`lastposter`='" + dr["poster"].ToString().Replace("'", "''") + "',`lastposterid`=" + dr["posterid"].ToString() + " WHERE `fid`=" + dr["fid"].ToString());
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `lastpost` = '" + dr["postdatetime"].ToString() + "', `lastpostid` =" + dr["posterid"].ToString() + ", `lastposttitle` ='" + dr["title"].ToString().Replace("'", "''") + "', `posts` = `posts` + 1	WHERE `uid` = " + dr["posterid"].ToString());
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics`  SET `replies`=`replies`+1,`lastposter`='" + dr["poster"].ToString().Replace("'", "''") + "',`lastposterid`=" + dr["posterid"].ToString() + ",`lastpost`='" + dr["postdatetime"].ToString() + "' WHERE `tid`=" + dr["tid"].ToString());
		   }
	   }

	   public DataTable GetPostLayer(int currentPostTableId, int postid)
	   {
		   string sql = "SELECT `layer`,`tid`  FROM `" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "` WHERE `pid`=" + postid.ToString() + " LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public void UpdateBBCCode(int available, string tag, string icon, string replacement, string example,
		   string explanation, string param, string nest, string paramsdescript, string paramsdefvalue, int id)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available),
				DbHelper.MakeInParam("?tag", (DbType)MySqlDbType.VarString, 100, tag),
				DbHelper.MakeInParam("?icon", (DbType)MySqlDbType.VarString,50, icon),
				DbHelper.MakeInParam("?replacement", (DbType)MySqlDbType.VarString,0, replacement),
				DbHelper.MakeInParam("?example", (DbType)MySqlDbType.VarString, 255, example),
				DbHelper.MakeInParam("?explanation", (DbType)MySqlDbType.VarString, 0, explanation),
				DbHelper.MakeInParam("?params", (DbType)MySqlDbType.Int32, 4, param),
				DbHelper.MakeInParam("?nest", (DbType)MySqlDbType.Int32, 4, nest),
				DbHelper.MakeInParam("?paramsdescript", (DbType)MySqlDbType.VarString, 0, paramsdescript),
				DbHelper.MakeInParam("?paramsdefvalue", (DbType)MySqlDbType.VarString, 0, paramsdefvalue),
				DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "bbcodes` SET `available`=?available,tag=?tag, icon=?icon,replacement=?replacement," +
			   "example=?example,explanation=?explanation,params=?params,nest=?nest,paramsdescript=?paramsdescript,paramsdefvalue=?paramsdefvalue " +
			   "WHERE `id`=?id";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataTable GetBBCode()
	   {
		   string sql = "Select * From `" + BaseConfigs.GetTablePrefix + "bbcodes` Order BY `id` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetBBCode(int id)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "bbcodes` WHERE `id`=?id", parm).Tables[0];
	   }

	   public void DeleteBBCode(string idlist)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "bbcodes`  WHERE `id` IN(" + idlist + ")");
	   }

	   public void SetBBCodeAvailableStatus(string idlist, int status)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?status",(DbType)MySqlDbType.Int32,4,status)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "bbcodes` SET `available`=?status  WHERE `id` IN(" + idlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataSet GetBBCCodeById(int id)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, id)
			};
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "bbcodes` WHERE `id`=?id";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
	   }

	   /// <summary>
	   /// 获得帖子列表
	   /// </summary>
	   /// <param name="count">数量</param>
	   /// <param name="views">最小浏览量</param>
	   /// <param name="fid">板块ID</param>
	   /// <param name="timetype">期限类型,一天、一周、一月、不限制</param>
	   /// <param name="ordertype">排序类型,时间倒序、浏览量倒序、最后回复倒序</param>
	   /// <param name="isdigest">是否精华</param>
	   /// <param name="onlyimg">缓存的有效期(单位:分钟)</param>
	   /// <returns></returns>
	   public DataTable GetFocusTopicList(int count, int views, int fid, string starttime, string orderfieldname, string visibleForum, bool isdigest, bool onlyimg)
	   {
		   string digestParam = "";

		   string fidParam = "";

		   if (isdigest)
			   digestParam = " AND `digest` > 0";


		   if (fid > 0)
			   fidParam = " AND `fid`=" + fid;

		   if (count < 0)
			   count = 0;

		   string attParam = "";
		   if (onlyimg)
			   attParam = "AND `attachment`=2";



		   if (visibleForum != string.Empty)
			   visibleForum = " AND `fid` IN (" + visibleForum + ")";

		   string sqlstr = @"SELECT * FROM `{1}topics` WHERE `displayorder` >=0 AND `views` > {2} AND `postdatetime` >'{3}' {4} {5} ORDER BY `{6}` DESC LIMIT {0}";

		   sqlstr = string.Format(sqlstr,
			   count,
			   BaseConfigs.GetTablePrefix,
			   views,
			   starttime,
			   fidParam + digestParam + visibleForum,
			   attParam,
			   orderfieldname
			   );

		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstr).Tables[0];
	   }

	   public void UpdateTopicLastPoster(int lastposterid, string lastposter)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 20, lastposter),
									 DbHelper.MakeInParam("?lastposterid", (DbType)MySqlDbType.Int32, 4, lastposterid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastposter`=?lastposter  WHERE `lastposterid`=?lastposterid", parms);
	   }

	   public void UpdateTopicPoster(int posterid, string poster)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, posterid),
									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarString, 20, poster)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `poster`=?poster WHERE `posterid`=?posterid", parms);
	   }

	   public void UpdatePostPoster(int posterid, string poster, string posttableid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, posterid),
									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarString, 20, poster)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` SET `poster`=?poster WHERE `posterid`=?posterid", parms);
	   }

	   /// <summary>
	   /// 更新主题信息
	   /// </summary>
	   /// <param name="topicinfo"></param>
	   /// <returns></returns>
	   public bool UpdateTopicAllInfo(TopicInfo topicinfo)
	   {
		   string sqlstring = string.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET fid='{1}',iconid='{2}',typeid='{3}',readperm='{4}',price='{5}',poster='{6}'," +
			   "title='{7}',postdatetime='{8}',lastpost='{9}',lastpostid='{10}',lastposter='{11}'," +
			   "views='{12}',replies='{13}',displayorder='{14}',highlight='{15}',digest='{16}',rate='{17}',blog='{18}'," +
			   "poll='{19}',attachment='{20}',moderated='{21}',closed='{22}' WHERE `tid`={0}",
			   topicinfo.Tid.ToString(),
			   topicinfo.Fid.ToString(),
			   topicinfo.Iconid.ToString(),
			   topicinfo.Typeid.ToString(),
			   topicinfo.Readperm.ToString(),
			   topicinfo.Price,
			   topicinfo.Poster,
			   topicinfo.Title,
			   topicinfo.Postdatetime,
			   topicinfo.Lastpost,
			   topicinfo.Lastpostid.ToString(),
			   topicinfo.Lastposter,
			   topicinfo.Views.ToString(),
			   topicinfo.Replies.ToString(),
			   topicinfo.Displayorder.ToString(),
			   topicinfo.Highlight,
			   topicinfo.Digest.ToString(),
			   topicinfo.Rate.ToString(),
			   topicinfo.Hide.ToString(),
			   topicinfo.Poll.ToString(),
			   topicinfo.Attachment.ToString(),
			   topicinfo.Moderated.ToString(),
			   topicinfo.Closed.ToString());

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
		   return true;
	   }

	   /// <summary>
	   /// 根据主题ID删除相应的主题信息
	   /// </summary>
	   /// <param name="tid"></param>
	   /// <returns></returns>
	   public bool DeleteTopicByTid(int tid, string posttablename)
	   {
		   //  SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   // conn.Open();
		   //using (SqlTransaction trans = conn.BeginTransaction())
		   //{
		   // try
		   // {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `tid`=" + tid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `tid`=" + tid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid`=" + tid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + posttablename + "` WHERE `tid`=" + tid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid`=" + tid.ToString());
		   // trans.Commit();
		   //   }
		   //  catch (Exception ex)
		   //   {
		   //       trans.Rollback();
		   //       throw ex;
		   //   }
		   //   }
		   //   conn.Close();
		   return true;

	   }

	   public bool SetTypeid(string topiclist, int value)
	   {
		   //SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   //conn.Open();
		   //using (SqlTransaction trans = conn.BeginTransaction())
		   //{
		   //  try
		   //{
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `typeid`=" + value.ToString() + " WHERE `tid` IN(" + topiclist + ")");
		   //   trans.Commit();
		   //}
		   //catch (Exception ex)
		   //{
		   //  trans.Rollback();
		   //throw ex;
		   //}
		   //}
		   //conn.Close();
		   return true;

	   }

	   public DataSet GetPosts(int pid, int pagesize, int currentpage, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, pid),
									 DbHelper.MakeInParam("?posttablename", (DbType)MySqlDbType.VarString, 30, posttablename)
								 };
		   DataSet ds;
		   String sql = "";
		   //dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `aid`, `tid`, `pid`, `postdatetime`, `readperm`, `filename`, `description`, `filetype`, `filesize`, `attachment`, `downloads` FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `tid`=?pid",prams).Tables[0];
		   sql = "SELECT " +
			   "`" + posttablename + "`.`pid`," +
			   "`" + posttablename + "`.`fid`," +
			   "`" + posttablename + "`.`title`," +
			   "`" + posttablename + "`.`layer`," +
			   "`" + posttablename + "`.`message`," +
			   "`" + posttablename + "`.`ip`," +
			   "`" + posttablename + "`.`lastedit`," +
			   "`" + posttablename + "`.`postdatetime`," +
			   "`" + posttablename + "`.`attachment`," +
			   "`" + posttablename + "`.`poster`," +
			   "`" + posttablename + "`.`posterid`," +
			   "`" + posttablename + "`.`invisible`," +
			   "`" + posttablename + "`.`usesig`," +
			   "`" + posttablename + "`.`htmlon`," +
			   "`" + posttablename + "`.`smileyoff`," +
			   "`" + posttablename + "`.`parseurloff`," +
			   "`" + posttablename + "`.`bbcodeoff`," +
			   "`" + posttablename + "`.`rate`," +
			   "`" + posttablename + "`.`ratetimes`," +

			   "`" + BaseConfigs.GetTablePrefix + "users`.`nickname`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`username`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`groupid`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`spaceid`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`email`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`showemail`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`digestposts`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`credits`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits1`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits2`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits3`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits4`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits5`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits6`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits7`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits8`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`posts`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`joindate`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`onlinestate`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`lastactivity`, " +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`invisible`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`gender`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`bday`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatar`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatarwidth`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatarheight`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`medals`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`sightml` AS signature," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`location`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`customstatus`, " +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`website`, " +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`icq`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`qq`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`msn`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`yahoo`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`skype`" +
			   "FROM `" + posttablename + "` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` ON `" + BaseConfigs.GetTablePrefix + "users`.`uid`=`" + posttablename + "`.`posterid` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "userfields`.`uid`=`" + BaseConfigs.GetTablePrefix + "users`.`uid` WHERE `" + posttablename + "`.`tid`=?pid AND `" + posttablename + "`.`invisible`=0 ORDER BY `" + posttablename + "`.`pid` LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   ds = DbHelper.ExecuteDataset(CommandType.Text,sql, prams);
		   //ds.Tables.Add(dt.Copy());
		   return ds;

	   }


	   public int GetAttachCount(int pid)
	   {
		   IDataParameter[] prams2 = {
									  DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 0, pid)
								  };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`aid`) AS `aidcount` FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `pid` = ?pid", prams2), 0);

	   }

	   public bool SetDisplayorder(string topiclist, int value)
	   {
		   // SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   // conn.Open();
		   // using (SqlTransaction trans = conn.BeginTransaction())
		   //  {
		   //  try
		   //  {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `displayorder`=" + value.ToString() + " WHERE `tid` IN(" + topiclist + ")");
		   //    trans.Commit();
		   //  }
		   //  catch (Exception ex)
		   //   {
		   //   trans.Rollback();
		   //   throw ex;
		   //}
		   //    }
		   // conn.Close();
		   return true;

	   }

	   /// <summary>
	   /// 添加评分记录
	   /// </summary>
	   /// <param name="postidlist">被评分帖子pid</param>
	   /// <param name="userid">评分者uid</param>
	   /// <param name="username">评分者用户名</param>
	   /// <param name="extid">分的积分类型</param>
	   /// <param name="score">积分数值</param>
	   /// <param name="reason">评分理由</param>
	   /// <returns>更新数据行数</returns>
	   public int InsertRateLog(int pid, int userid, string username, int extid, float score, string reason)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, Utils.StrToFloat(pid, 0)),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid),
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.String, 15, username),
									 DbHelper.MakeInParam("?extcredits", (DbType)MySqlDbType.Int16, 1, extid),
									 DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8, DateTime.Now),
									 DbHelper.MakeInParam("?score", (DbType)MySqlDbType.Int16, 6, score),
									 DbHelper.MakeInParam("?reason", (DbType)MySqlDbType.VarString, 20, reason)
								 };

		   string CommandText = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "ratelog` (`pid`,`uid`,`username`,`extcredits`,`postdatetime`,`score`,`reason`) VALUES (?pid,?uid,?username,?extcredits,?postdatetime,?score,?reason)";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
	   }

	   /// <summary>
	   /// 删除日志
	   /// </summary>
	   /// <returns></returns>
	   public bool DeleteRateLog()
	   {
		   try
		   {
			   if (DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "ratelog` ") > 1)
			   {
				   return true;
			   }
		   }
		   catch
		   {
		   }
		   return false;
	   }

	   /// <summary>
	   /// 按指定条件删除日志
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public bool DeleteRateLog(string condition)
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "ratelog` WHERE " + condition);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 得到当前指定页数的评分日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <returns></returns>
	   public DataTable RateLogList(int pagesize, int currentpage, string posttablename)
	   {
		   string sqlstring = "";

		   sqlstring = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "ratelog` Order by `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();


		   sqlstring = "SELECT r.*,p.title As title,p.poster As poster , p.posterid As posterid,  ug.grouptitle As grouptitle FROM (((" + sqlstring + ") as r LEFT JOIN `" + posttablename + "` as p ON r.pid = p.pid) LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` as u ON u.uid = r.uid) LEFT JOIN `" + BaseConfigs.GetTablePrefix + "usergroups` as ug ON ug.groupid = u.groupid";


		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];

	   }

	   /// <summary>
	   /// 得到当前指定条件和页数的评分日志记录(表)
	   /// </summary>
	   /// <param name="pagesize">当前分页的尺寸大小</param>
	   /// <param name="currentpage">当前页码</param>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public DataTable RateLogList(int pagesize, int currentpage, string posttablename, string condition)
	   {
		   string sqlstring = "";

		   sqlstring = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "ratelog` WHERE " + condition + "  Order by `id` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   sqlstring = "SELECT r.*,p.title As title,p.poster As poster , p.posterid As posterid,  ug.grouptitle As grouptitle FROM (((" + sqlstring + ") r LEFT JOIN `" + posttablename + "` p ON r.pid = p.pid) LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` u ON u.uid = r.uid) LEFT JOIN `" + BaseConfigs.GetTablePrefix + "usergroups` ug ON ug.groupid = u.groupid";


		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];

	   }

	   /// <summary>
	   /// 得到评分日志记录数
	   /// </summary>
	   /// <returns></returns>
	   public int GetRateLogCount()
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "ratelog`").Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 得到指定查询条件下的评分日志数
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public int GetRateLogCount(string condition)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "ratelog` WHERE " + condition).Tables[0].Rows[0][0].ToString());
	   }

	   public int GetPostsCount(string posttableid)
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`pid`) AS `portscount` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "`"), 0);
	   }

	   public IDataReader GetMaxAndMinTid(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT MAX(`tid`) AS `maxtid`,MIN(`tid`) AS `mintid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid` IN (SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid`=?fid OR (InStr(concat(',',RTRIM(?fid),','), concat(',',RTRIM(parentidlist),',')) > 0))", prams);

		   // (InStr(',' & RTRIM(?fid) & ',', ',' + RTRIM(parentidlist) + ',') > 0)
	   }

	   public int GetPostCount(int fid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS `postcount` FROM `" +posttablename+"` WHERE `fid` = ?fid", prams), 0);
	   }

	   public int GetPostCount(string posttableid, int tid, int posterid)
	   {
		   string posttablename = string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid);
		   string sqlstr = string.Format("`{0}`.`tid`={1} AND `{0}`.`posterid`={2}", posttablename, tid, posterid);
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,100,sqlstr),
									 DbHelper.MakeInParam("?posttablename", (DbType)MySqlDbType.VarString, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid))
								 };
		   String sql = "SELECT COUNT(pid) FROM `" + string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid) + "` WHERE ?condition AND `layer`>=0";

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
		   // return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getpostcountbycondition", BaseConfigs.GetTablePrefix), prams), 0);
	   }

	   public int GetTodayPostCount(int fid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS `postcount` FROM `" + posttablename + "` WHERE `fid` = ?fid AND DATEDIFF(`postdatetime`, NOW()) = 0 ", prams), 0);
	   }

	   public int GetPostCount(int fid, int posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS `postcount` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE  `fid` IN (SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid`=?fid OR (instr(concat(',',RTRIM(parentidlist),','),concat(',',RTRIM(?fid),',')) > 0))", prams), 0);
	   }

	   public int GetTodayPostCount(int fid, int posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS `postcount` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE  `fid` IN (SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid`=?fid OR (instr(concat(',',RTRIM(parentidlist),','),concat(',',RTRIM(?fid),',')) > 0)) AND DATEDIFF(`postdatetime`, NOW()) = 0 ", prams), 0);
	   }

	   public IDataReader GetMaxAndMinTidByUid(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT MAX(`tid`) AS `maxtid`,MIN(`tid`) AS `mintid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `posterid` = ?uid", prams);
	   }

	   public int GetPostCountByUid(int uid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };

		   return Math.Abs(Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS `postcount` FROM `" + posttablename + "` WHERE `posterid` = ?uid", prams), 0));
	   }

	   public int GetTodayPostCountByUid(int uid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS `postcount` FROM `" + posttablename + "` WHERE `posterid` = ?uid AND DATEDIFF(`postdatetime`, NOW()) = 0 ", prams), 0);
	   }

	   public int GetTopicsCount()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`tid`) AS `topicscount` FROM `" + BaseConfigs.GetTablePrefix + "topics`"), 0);
	   }

	   public void ReSetStatistic(int UserCount, int TopicsCount, int PostCount, string lastuserid, string lastusername)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?totaltopic", (DbType)MySqlDbType.Int32, 4, TopicsCount),
									 DbHelper.MakeInParam("?totalpost", (DbType)MySqlDbType.Int32, 4, PostCount),
									 DbHelper.MakeInParam("?totalusers", (DbType)MySqlDbType.Int32, 4, UserCount),
									 DbHelper.MakeInParam("?lastusername", (DbType)MySqlDbType.VarString, 20, lastusername),
									 DbHelper.MakeInParam("?lastuserid", (DbType)MySqlDbType.Int32, 4, Utils.StrToInt(lastuserid, 0))

								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totaltopic`=?totaltopic,`totalpost`=?totalpost,`totalusers`=?totalusers,`lastusername`=?lastusername,`lastuserid`=?lastuserid", prams);
	   }

	   public IDataReader GetTopicTids(int statcount, int lasttid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?lasttid", (DbType)MySqlDbType.Int32, 4, lasttid),
		   };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` > ?lasttid ORDER BY `tid` LIMIT " + statcount.ToString(), prams);

	   }

	   public IDataReader GetLastPost(int tid, int posttableid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid);
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `pid`, `postdatetime`, `posterid`, `poster` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid` = ?tid ORDER BY `pid` DESC LIMIT 1", parm);
	   }

	   public void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?lastpostid", (DbType)MySqlDbType.Int32, 4, lastpostid),
									 DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.VarString, 20, lastpost),
									 DbHelper.MakeInParam("?lastposterid", (DbType)MySqlDbType.Int32, 4, lastposterid),
									 DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 20, poster),
									 DbHelper.MakeInParam("?postcount", (DbType)MySqlDbType.Int32, 4, postcount),
									 DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpost`=?lastpost, `lastposterid`=?lastposterid, `lastposter`=?lastposter, `replies`=?postcount WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = ?tid", parms);
	   }

	   public void UpdateTopicLastPosterId(int tid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastposterid`=(SELECT  IFNULL(MIN(`lastpostid`),-1)-1 FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = ?tid", parms);
	   }

	   public IDataReader GetTopics(int start_tid, int end_tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?start_tid", (DbType)MySqlDbType.Int32, 4, start_tid),
									 DbHelper.MakeInParam("?end_tid", (DbType)MySqlDbType.Int32, 4, end_tid)
								 };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` >= ?start_tid AND `tid`<=?end_tid  ORDER BY `tid`", prams);
	   }

	   public IDataReader GetForumLastPost(int fid, string posttablename, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost, int lastposterid, string lastposter, int todaypostcount)
	   {
		   IDataParameter[] prams_posts = {
										   DbHelper.MakeInParam("?lastfid", (DbType)MySqlDbType.Int32, 4, fid),
										   DbHelper.MakeInParam("?topiccount", (DbType)MySqlDbType.Int32, 4, topiccount),
										   DbHelper.MakeInParam("?postcount", (DbType)MySqlDbType.Int32, 4, postcount),
										   DbHelper.MakeInParam("?lasttid", (DbType)MySqlDbType.Int32, 4, lasttid),
										   DbHelper.MakeInParam("?lasttitle", (DbType)MySqlDbType.VarString, 80, lasttitle),
										   DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.VarString, 20, lastpost),
										   DbHelper.MakeInParam("?lastposterid", (DbType)MySqlDbType.Int32, 4, lastposterid),
										   DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 20, lastposter),
										   DbHelper.MakeInParam("?todaypostcount", (DbType)MySqlDbType.Int32, 4, todaypostcount)
									   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `tid`, `title`, `postdatetime`, `posterid`, `poster` FROM `" + posttablename + "` WHERE `fid` = ?lastfid ORDER BY `pid` DESC LIMIT 1", prams_posts);
	   }

	   public void UpdateForum(int fid, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost, int lastposterid, string lastposter, int todaypostcount)
	   {
		   IDataParameter[] prams_posts = {
										   DbHelper.MakeInParam("?topiccount", (DbType)MySqlDbType.Int32, 4, topiccount),
										   DbHelper.MakeInParam("?postcount", (DbType)MySqlDbType.Int32, 4, postcount),
										   DbHelper.MakeInParam("?todaypostcount", (DbType)MySqlDbType.Int32, 4, todaypostcount),
										   DbHelper.MakeInParam("?lasttid", (DbType)MySqlDbType.Int32, 4, lasttid),
										   DbHelper.MakeInParam("?lasttitle", (DbType)MySqlDbType.VarChar, 80, lasttitle),
										   DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.VarChar, 20, lastpost),
										   DbHelper.MakeInParam("?lastposterid", (DbType)MySqlDbType.Int32, 4, lastposterid),
										   DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarChar, 20, lastposter),
										   DbHelper.MakeInParam("?lastfid", (DbType)MySqlDbType.Int32, 4, fid)

									   };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `topics` = ?topiccount, `posts`=?postcount, `todayposts` = ?todaypostcount, `lasttid` = ?lasttid, `lasttitle` = ?lasttitle, `lastpost`=?lastpost, `lastposterid` = ?lastposterid, `lastposter`=?lastposter WHERE `" + BaseConfigs.GetTablePrefix + "forums`.`fid` = ?lastfid", prams_posts);
	   }

	   public IDataReader GetForums(int start_fid, int end_fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?start_fid", (DbType)MySqlDbType.Int32, 4, start_fid),
									 DbHelper.MakeInParam("?end_fid", (DbType)MySqlDbType.Int32, 4, end_fid)
								 };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT  `fid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid` >= ?start_fid AND `fid`<=?end_fid", prams);
	   }

	   /// <summary>
	   /// 清除主题里面已经移走的主题
	   /// </summary>
	   public void ReSetClearMove()
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `closed` > 1");
	   }

	   public IDataReader GetLastPostByFid(int fid, string posttablename)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid);
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `tid`, `title`, `postdatetime`, `posterid`, `poster` FROM `" + posttablename + "` WHERE `fid` = ?fid ORDER BY `pid` DESC LIMIT 1", parm);
	   }

	   /// <summary>
	   /// 创建一个投票
	   /// </summary>
	   /// <param name="tid">关联的主题id</param>
	   /// <param name="polltype">投票类型, 0为单选, 1为多选</param>
	   /// <param name="itemcount">投票项总数</param>
	   /// <param name="itemnamelist">投票项目列表</param>
	   /// <param name="itemvaluelist">投票项目结果列表</param>
	   /// <param name="enddatetime">截止日期</param>
	   /// <returns>成功则返回true, 否则返回false</returns>
	   public bool CreatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime, int userid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?polltype",(DbType)MySqlDbType.Int32,4,polltype),
									 DbHelper.MakeInParam("?itemcount",(DbType)MySqlDbType.Int32,4,itemcount),
									 DbHelper.MakeInParam("?itemnamelist",(DbType)MySqlDbType.VarString,0,itemnamelist),
									 DbHelper.MakeInParam("?itemvaluelist",(DbType)MySqlDbType.VarString,0,itemvaluelist),
									 DbHelper.MakeInParam("?usernamelist",(DbType)MySqlDbType.VarString,0,""),
									 DbHelper.MakeInParam("?enddatetime",(DbType)MySqlDbType.Datetime,19,enddatetime),
									 DbHelper.MakeInParam("?userid",(DbType)MySqlDbType.Int32,4,userid)
								 };

		   String sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "polls`(`tid`, `polltype`, `itemcount`, `itemnamelist`, `itemvaluelist`, `usernamelist`, `enddatetime`, `userid`) VALUES(?tid, ?polltype, ?itemcount, ?itemnamelist, ?itemvaluelist, ?usernamelist, ?enddatetime, ?userid)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);

		   string sqlup="UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `poll`=1 WHERE `tid`=?tid";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sqlup, prams) > 0;
	   }

	   public bool UpdatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?polltype",(DbType)MySqlDbType.Int32,4,polltype),
									 DbHelper.MakeInParam("?itemcount",(DbType)MySqlDbType.Int32,4,itemcount),
									 DbHelper.MakeInParam("?itemnamelist",(DbType)MySqlDbType.VarString,0,itemnamelist),
									 DbHelper.MakeInParam("?itemvaluelist",(DbType)MySqlDbType.VarString,0,itemvaluelist),
									 DbHelper.MakeInParam("?enddatetime",(DbType)MySqlDbType.VarString,19,enddatetime),
		   };
		   string sqlUpdatePoll = "UPDATE " + BaseConfigs.GetTablePrefix + "polls SET " +
			   "`polltype`=?polltype," +
			   "`itemcount`=?itemcount," +
			   "`itemnamelist`=?itemnamelist," +
			   "`itemvaluelist`=?itemvaluelist," +
			   "`enddatetime`=?enddatetime " +
			   "WHERE " +
			   "`tid`=?tid";


		   return DbHelper.ExecuteNonQuery(CommandType.Text, sqlUpdatePoll, prams) > 0;
	   }

	   /// <summary>
	   /// 获得投票信息
	   /// </summary>
	   /// <param name="tid"></param>
	   /// <returns></returns>
	   public IDataReader GetPoll(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT `itemvaluelist`, `usernamelist` FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid`=?tid LIMIT 1", prams);
		   return reader;
	   }

	   /// <summary>
	   /// 根据投票信息更新数据库中的记录
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="selitemidlist">选择的投票项id列表</param>
	   /// <param name="username">用户名</param>
	   /// <returns>如果执行成功则返回0, 非法提交返回负值</returns>
	   public int UpdatePoll(int tid, string usernamelist, StringBuilder newselitemidlist)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?itemvaluelist",(DbType)MySqlDbType.VarString,0,newselitemidlist.ToString().Trim()),
									 DbHelper.MakeInParam("?usernamelist",(DbType)MySqlDbType.VarString,0,usernamelist.ToString().Trim()),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   if (DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "polls` SET `itemvaluelist`=?itemvaluelist, `usernamelist`=?usernamelist WHERE `tid`=?tid", prams) > 0)
		   {
			   return 0;
		   }
		   else
		   {
			   return -4;
		   }
	   }

	   /// <summary>
	   /// 获得与指定主题id关联的投票数据
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <returns>投票数据</returns>
	   public DataTable GetPollList(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   string sqlstring = "SELECT `polltype`, `itemcount`, `itemnamelist`, `itemvaluelist`, `enddatetime` FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid`=?tid LIMIT 1";
		   DataTable dtTemp = DbHelper.ExecuteDataset(CommandType.Text, sqlstring, prams).Tables[0];
		   return dtTemp;
	   }

	   /// <summary>
	   /// 获得投票的用户名
	   /// </summary>
	   /// <param name="tid">主题Id</param>
	   /// <returns></returns>
	   public string GetPollUserNameList(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };

		   string strUsernamelist = DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT `usernamelist` FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid`=?tid LIMIT 1", prams);
		   return strUsernamelist;
	   }

	   /// <summary>
	   /// 得到投票帖的投票类型
	   /// </summary>
	   /// <param name="tid">主题ＩＤ</param>
	   /// <returns>投票类型</returns>
	   public int GetPollType(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT `polltype` FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid`=?tid LIMIT 1", prams), 0);
	   }

	   /// <summary>
	   /// 得到投票帖的结束时间
	   /// </summary>
	   /// <param name="tid">主题ＩＤ</param>
	   /// <returns>结束时间</returns>
	   public string GetPollEnddatetime(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   return Utils.GetDate(DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT `enddatetime` FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid`=?tid LIMIT 1", prams), Utils.GetDate());
	   }



	   /// <summary>
	   /// 得到用户帖子分表信息
	   /// </summary>
	   /// <returns>分表记录集</returns>
	   public DataSet GetAllPostTableName()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM `{0}tablelist` ORDER BY `id` DESC", BaseConfigs.GetTablePrefix));
	   }

	   /// <summary>
	   /// 创建帖子
	   /// </summary>
	   /// <param name="postinfo">帖子信息类</param>
	   /// <returns>返回帖子id</returns>
	   public int CreatePost(PostInfo postinfo, string posttableid)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int16,2,postinfo.Fid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,postinfo.Tid),
									 DbHelper.MakeInParam("?parentid",(DbType)MySqlDbType.Int32,4,postinfo.Parentid),
									 DbHelper.MakeInParam("?layer",(DbType)MySqlDbType.Int32,4,postinfo.Layer),
									 DbHelper.MakeInParam("?poster",(DbType)MySqlDbType.VarString,15,postinfo.Poster),
									 DbHelper.MakeInParam("?posterid",(DbType)MySqlDbType.Int32,4,postinfo.Posterid),
									 DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarString,60,postinfo.Title),
									 DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Date,8,DateTime.Parse(postinfo.Postdatetime)),
									 DbHelper.MakeInParam("?message",(DbType)MySqlDbType.LongText,0,postinfo.Message),
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.VarString,15,postinfo.Ip),
									 DbHelper.MakeInParam("?lastedit",(DbType)MySqlDbType.VarString,50,postinfo.Lastedit),
									 DbHelper.MakeInParam("?invisible",(DbType)MySqlDbType.Int32,4,postinfo.Invisible),
									 DbHelper.MakeInParam("?usesig",(DbType)MySqlDbType.Int32,4,postinfo.Usesig),
									 DbHelper.MakeInParam("?htmlon",(DbType)MySqlDbType.Int32,4,postinfo.Htmlon),
									 DbHelper.MakeInParam("?smileyoff",(DbType)MySqlDbType.Int32,4,postinfo.Smileyoff),
									 DbHelper.MakeInParam("?bbcodeoff",(DbType)MySqlDbType.Int32,4,postinfo.Bbcodeoff),
									 DbHelper.MakeInParam("?parseurloff",(DbType)MySqlDbType.Int32,4,postinfo.Parseurloff),
									 DbHelper.MakeInParam("?attachment",(DbType)MySqlDbType.Int32,4,postinfo.Attachment),
									 DbHelper.MakeInParam("?rate",(DbType)MySqlDbType.Int16,2,postinfo.Rate),
									 DbHelper.MakeInParam("?ratetimes",(DbType)MySqlDbType.Int32,4,postinfo.Ratetimes)
								 };
		   #region 存储过程转sql语句 CreatePost
		   string tablename = ""+BaseConfigs.GetTablePrefix+"posts"+posttableid;
		   string strSQL = "";
		   int postid = 0, id = 0;


		   strSQL = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "postid` WHERE FLOOR((UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP('2007-08-08 10:52:01'))/60)>5";
		   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

		   #region 获取插入记录的最大id
		   //OleDbConnection con = new OleDbConnection(BaseConfigs.GetDBConnectString);
		   //OleDbCommand cmd = new OleDbCommand();
		   //cmd.Connection = con ;
		   //con.Open();
		   strSQL = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "postid` (`postdatetime`) VALUES (now())";
		   DbHelper.ExecuteNonQuery(out id, CommandType.Text, strSQL);
		   //cmd.CommandText = strSQL ;
		   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);
		   postid = id;
		   //			strSQL = "SELECT ??identity as `maxidd`";
		   //			cmd.CommandText = strSQL ;
		   //			postid = Convert.ToInt32(cmd.ExecuteScalar());
		   //			con.Close();
		   //			cmd.Dispose();
		   //			con.Dispose();
		   #endregion



		   strSQL = "INSERT INTO `" + tablename + "`(`pid`, `fid`, `tid`, `parentid`, `layer`, `poster`, `posterid`, `title`, `postdatetime`, `message`, `ip`, `lastedit`, `invisible`, `usesig`, `htmlon`, `smileyoff`, `bbcodeoff`, `parseurloff`, `attachment`, `rate`, `ratetimes`) VALUES(" + postid + ", ?fid, ?tid, ?parentid, ?layer, ?poster, ?posterid, ?title, ?postdatetime, ?message, ?ip, ?lastedit, ?invisible, ?usesig, ?htmlon, ?smileyoff, ?bbcodeoff, ?parseurloff, ?attachment, ?rate, ?ratetimes)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);
		   if (postinfo.Parentid == 0)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + tablename + "` SET `parentid`=" + postid + " WHERE `pid`=" + postid);
		   }

		   if (postinfo.Invisible == 0)
		   {
			   //--更新帖子总数
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totalpost`=`totalpost` + 1";
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   IDataParameter[] prams1 = {

										  DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,2,postinfo.Fid)
									  };
			   string fidlist = DbHelper.ExecuteDataset(CommandType.Text, "SELECT IFNULL(`parentidlist`,'') as `fidlist` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid` = ?fid", prams1).Tables[0].Rows[0][0].ToString();
			   if (fidlist != string.Empty)
			   {
				   fidlist = fidlist + "," + postinfo.Fid.ToString();
			   }
			   else
			   {
				   fidlist = postinfo.Fid.ToString();
			   }

			   IDataParameter[] prams2 = {

									
										  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,postinfo.Tid),
										  DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarString,60,postinfo.Title),
										  DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.VarChar,0,postinfo.Postdatetime),
										  DbHelper.MakeInParam("?poster",(DbType)MySqlDbType.VarChar,15,postinfo.Poster),
										  DbHelper.MakeInParam("?posterid",(DbType)MySqlDbType.Int32,4,postinfo.Posterid)
									  };


                
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET " +
				   "`posts`=`posts` + 1," +
				   "`todayposts`=if( " +
				   "(Datediff(`lastpost`,now())=0),(`todayposts`*1 + 1),1)," +

				   "`lasttid`=?tid," +
				   "`lasttitle`=?title," +
				   "`lastpost`=?postdatetime," +
				   "`lastposter`=?poster," +
				   "`lastposterid`=?posterid " +
				   "WHERE (instr(',"+fidlist+",',concat(',',fid,','))> 0)";
			   //"WHERE `fid` in (" + fidlist + ")";
			   //strSQL = "UPDATE `dnt_forums` SET `posts`=`posts`+1,`todayposts`=if( (Datediff(`lastpost`,now())=0),(`todayposts`*1+1),1),`lasttid`=9,`lasttitle`='33333',`lastpost`='2007-08-08 13:35:50',`lastposter`='游客',`lastposterid`=-1 WHERE instr( ',15 ,18,',',& RTRIM(`fid`) & ,') > 0";

			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams2);

			   IDataParameter[] prams3 = {
										  DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.VarChar,0,postinfo.Postdatetime),
										  DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarChar,60,postinfo.Title),
										  DbHelper.MakeInParam("?posterid",(DbType)MySqlDbType.Int32,4,postinfo.Posterid),
			   };
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET " +
				   "`lastpost` = ?postdatetime," +
				   "`lastpostid` = " + postid + "," +
				   "`lastposttitle` = ?title," +
				   "`posts` = `posts` + 1, " +
				   "`lastactivity` = now() " +
				   "WHERE `uid` = ?posterid";
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams3);

			   if (postinfo.Layer <= 0)
			   {
				   IDataParameter[] prams4 = {
											
											
											
											  DbHelper.MakeInParam("?poster",(DbType)MySqlDbType.VarChar,15,postinfo.Poster),
											  DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.VarChar,0,postinfo.Postdatetime),
											
											
											  DbHelper.MakeInParam("?posterid",(DbType)MySqlDbType.Int32,4,postinfo.Posterid),

											  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,postinfo.Tid),
				   };
				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `replies`=0,`lastposter`=?poster,`lastpost`=?postdatetime,`lastposterid`=?posterid WHERE `tid`=?tid";
				   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams4);
			   }
			   else
			   {
				   IDataParameter[] prams5 = {
											
											
											
											  DbHelper.MakeInParam("?poster",(DbType)MySqlDbType.VarChar,15,postinfo.Poster),
											  DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.VarChar,0,postinfo.Postdatetime),
											
											
											  DbHelper.MakeInParam("?posterid",(DbType)MySqlDbType.Int32,4,postinfo.Posterid),

											  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,postinfo.Tid),
				   };
				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `replies`=`replies` + 1,`lastposter`=?poster,`lastpost`=?postdatetime,`lastposterid`=?posterid WHERE `tid`=?tid";
				   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams5);

			   }
			   IDataParameter[] prams6 = {
										
										  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,postinfo.Tid),
			   };
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpostid`=" + postid + " WHERE `tid`=?tid";
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams6);
		   }




		   if (postinfo.Posterid != -1)
		   {
			   IDataParameter[] prams7 = {
										  DbHelper.MakeInParam("?posterid",(DbType)MySqlDbType.Int32,4,postinfo.Posterid),
										  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,postinfo.Tid),
										  DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.VarChar,0,postinfo.Postdatetime),
			   };
			   DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT into `" + BaseConfigs.GetTablePrefix + "myposts`(`uid`, `tid`, `pid`, `dateline`) VALUES(?posterid, ?tid, " + postid + ", ?postdatetime)", prams7);
		   }
		   #endregion
		   return postid;

		   //  return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, Sql,prams));

	   }

	   /// <summary>
	   /// 更新指定帖子信息
	   /// </summary>
	   /// <param name="__postsInfo">帖子信息</param>
	   /// <returns>更新数量</returns>
	   public int UpdatePost(PostInfo __postsInfo, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarString,160,__postsInfo.Title),
									 DbHelper.MakeInParam("?message",(DbType)MySqlDbType.VarString,0,__postsInfo.Message),
									 DbHelper.MakeInParam("?lastedit",(DbType)MySqlDbType.VarString,50,__postsInfo.Lastedit),
									 DbHelper.MakeInParam("?invisible",(DbType)MySqlDbType.Int32,4,__postsInfo.Invisible),
									 DbHelper.MakeInParam("?usesig",(DbType)MySqlDbType.Int32,4,__postsInfo.Usesig),
									 DbHelper.MakeInParam("?htmlon",(DbType)MySqlDbType.Int32,4,__postsInfo.Htmlon),
									 DbHelper.MakeInParam("?smileyoff",(DbType)MySqlDbType.Int32,4,__postsInfo.Smileyoff),
									 DbHelper.MakeInParam("?bbcodeoff",(DbType)MySqlDbType.Int32,4,__postsInfo.Bbcodeoff),
									 DbHelper.MakeInParam("?parseurloff",(DbType)MySqlDbType.Int32,4,__postsInfo.Parseurloff),
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,__postsInfo.Pid)
								 };
		   StringBuilder s = new StringBuilder();
		   s.Append("UPDATE {0}posts{1} SET");
		   s.Append("`title`=?title,");
		   s.Append("`message`=?message,");
		   s.Append("`lastedit`=?lastedit,");
		   s.Append("`invisible`=?invisible,");
		   s.Append("`usesig`=?usesig,");
		   s.Append("`htmlon`=?htmlon,");
		   s.Append("`smileyoff`=?smileyoff,");
		   s.Append("`bbcodeoff`=?bbcodeoff,");
		   s.Append("`parseurloff`=?parseurloff WHERE `pid`=?pid");
		   //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatepost" + posttableid, prams);
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(s.ToString(), BaseConfigs.GetTablePrefix, posttableid), prams);
	   }

	   /// <summary>
	   /// 删除指定ID的帖子
	   /// </summary>
	   /// <param name="pid">帖子ID</param>
	   /// <returns>删除数量</returns>
	   public int DeletePost(string posttableid, int pid)
	   {
		   //IDataParameter[] prams = {
		   //		   DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,pid)
		   //	   };

		   #region 存储过程转sql语句 DeletePost
		   int fid = 0;
		   int tid = 0;
		   int posterid = 0;
		   int lastforumposterid = 0;
		   int layer = 0;
		   DateTime postdatetime;
		   string poster = "";
		   int postcount = 0;
		   string title = "";
		   int lasttid = 0;
		   //int postid=0;
		   int todaycount = 0;

		   string fidlist = "";

		   string strSQL = "";

		   DataTable dt = new DataTable();

		   strSQL = "SELECT `fid`, `tid`, `posterid`,`layer`, `postdatetime` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE pid =" + pid;
		   DataRow dr = DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0];
		   fid = Convert.ToInt32(dr["fid"].ToString());
		   tid = Convert.ToInt32(dr["tid"].ToString());
		   posterid = Convert.ToInt32(dr["posterid"].ToString());
		   layer = Convert.ToInt32(dr["layer"].ToString());
		   postdatetime = Convert.ToDateTime(dr["postdatetime"].ToString());

		   strSQL = "SELECT if((`parentidlist` is null),'',`parentidlist`) as `fidlist` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid` =" + fid;
		   fidlist = DbHelper.ExecuteScalarToStr(CommandType.Text, strSQL);
		   if (fidlist != "")
		   {
			   fidlist = string.Concat(fidlist, ",", fid.ToString());
		   }
		   else
		   {
			   fidlist = fid.ToString();
		   }

		   if (layer != 0)
		   {
			   //--只删除一个帖子
			   //	--更新论坛总的回帖数
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totalpost`=`totalpost` - 1";
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   //	--更新版块内总的回帖数

			   if (Convert.ToDateTime(postdatetime).ToShortDateString() == DateTime.Now.ToShortDateString())
			   {
				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET 	`posts`=`posts` - 1, `todayposts`=`todayposts`-1 WHERE `fid` in (" + fidlist + ")";
			   }
			   else
			   {
				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET 	`posts`=`posts` - 1  WHERE `fid` in (" + fidlist + ")";
			   }
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   //--更新用户总的回帖数
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET  `posts` = `posts`-1 WHERE `uid` =" + posterid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   //--更新主题总的回帖数
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `replies`=`replies` - 1 WHERE `tid`=" + tid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   //--删除帖子
			   strSQL = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `pid`=" + pid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

		   }
		   else
		   {
			   //--删除主题

			   strSQL = "SELECT COUNT(`pid`) FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid` = " + tid;
			   postcount = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0][0].ToString());

			   strSQL = "SELECT COUNT(`pid`) FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid` =" + tid + "  AND DATEDIFF(now(), `postdatetime`) = 0";
			   todaycount = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0][0].ToString());

			   //--更新主题及帖子总数
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totaltopic`=`totaltopic` - 1, `totalpost`=`totalpost` -" + postcount;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   //--更新版块
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `posts`=`posts` -" + postcount + ", `topics`=`topics` - 1,`todayposts`=`todayposts` -" + todaycount + " WHERE `fid` in (" + fidlist + ")";
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   //--更新用户总的回帖数
			   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `posts` = `posts` - " + postcount + " WHERE `uid` = " + posterid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   strSQL = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid` = " + tid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

			   strSQL = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` = " + tid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

		   }

		   if (layer != 0)
		   {
			   strSQL = "SELECT `pid`, `posterid`,`postdatetime`, `title`, `poster` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid`=" + tid + " ORDER BY `pid` DESC LIMIT 1";
			   dt = DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0];
			   if (dt.Rows.Count > 0)
			   {
				   dr = dt.Rows[0];
				   pid = Convert.ToInt32(dr["pid"].ToString());
				   posterid = Convert.ToInt32(dr["posterid"].ToString());
				   postdatetime = Convert.ToDateTime(dr["postdatetime"].ToString());
				   title = dr["title"].ToString();
				   poster = dr["poster"].ToString();

				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastposter`='" + poster + "',`lastpost`='" + postdatetime.ToString() + "',`lastpostid`=" + pid + ",`lastposterid`=" + posterid + " WHERE `tid`=" + tid;
				   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);
			   }

		   }


		   strSQL = "SELECT `lasttid` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid` =" + fid;
		   lasttid = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0][0].ToString());


		   if (lasttid == tid)
		   {

			   strSQL = "SELECT `pid`, `tid`,`posterid`, `title`, `poster`, `postdatetime` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `fid` = " + fid + " ORDER BY `pid` DESC LIMIT 1";
			   dt = DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0];
			   if (dt.Rows.Count > 0)
			   {
				   dr = dt.Rows[0];
				   pid = Convert.ToInt32(dr["pid"].ToString());
				   tid = Convert.ToInt32(dr["tid"].ToString());
				   if (dr["posterid"] == null)
				   {
					   lastforumposterid = 0;
				   }
				   else
				   {
					   lastforumposterid = Convert.ToInt32(dr["posterid"].ToString());
				   }

				   postdatetime = Convert.ToDateTime(dr["postdatetime"].ToString());
				   if (dr["title"] == null)
				   {
					   title = "";
				   }
				   else
				   {
					   title = dr["title"].ToString();
				   }

				   if (dr["poster"] == null)
				   {
					   poster = "";
				   }
				   else
				   {
					   poster = dr["poster"].ToString();
				   }


				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `lasttid`=" + tid + ",`lasttitle`='" + title + "',`lastpost`='" + postdatetime + "',`lastposter`='" + poster + "',`lastposterid`=" + lastforumposterid + " WHERE `fid` in (" + fidlist + ")";
				   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

				   strSQL = "SELECT `pid`, `tid`, `posterid`, `postdatetime`, `title`, `poster` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `posterid`=" + posterid + " ORDER BY `pid` DESC LIMIT 1";
				   dr = DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0];

				   pid = Convert.ToInt32(dr["pid"].ToString());
				   //tid=Convert.ToInt32(dr["tid"].ToString());
				   posterid = Convert.ToInt32(dr["posterid"].ToString());
				   postdatetime = Convert.ToDateTime(dr["postdatetime"].ToString());
				   if (dr["title"] == null)
				   {
					   title = "";
				   }
				   else
				   {
					   title = dr["title"].ToString();
				   }

				   //poster=dr["poster"].ToString();

				   //--更新用户
				   strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `lastpost` = '" + postdatetime + "',`lastpostid` = " + pid + ",`lastposttitle` = '" + title + "'  WHERE `uid` = " + posterid;
				   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);
			   }

		   }





		   #endregion
		   return postcount;

		   // return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletepost{1}bypid", BaseConfigs.GetTablePrefix, posttableid), prams);






	   }

	   /// <summary>
	   /// 获得指定的帖子描述信息
	   /// </summary>
	   /// <param name="pid">帖子id</param>
	   /// <returns>帖子描述信息</returns>
	   public IDataReader GetPostInfo(string posttableid, int pid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32,4, pid),
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM `{0}posts{1}` WHERE `pid`=?pid LIMIT 1", BaseConfigs.GetTablePrefix, posttableid), prams);
	   }

	   /// <summary>
	   /// 获得指定主题的帖子列表
	   /// </summary>
	   /// <param name="topiclist">主题ID列表</param>
	   /// <returns></returns>
	   public DataSet GetPostList(string topiclist, string[] posttableid)
	   {
		   StringBuilder sb = new StringBuilder();
		   for (int i = 0; i < posttableid.Length; i++)
		   {
			   if (sb.Length > 0)
			   {
				   sb.Append(" UNION ALL ");
			   }
			   sb.Append("SELECT * FROM ");
			   sb.Append(BaseConfigs.GetTablePrefix);
			   sb.Append("posts1");
                
			   sb.Append(" WHERE `tid` IN (");
			   sb.Append(topiclist);
			   sb.Append(")");
		   }

		   DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, sb.ToString());
		   return ds;
	   }

	   /// <summary>
	   /// 获取指定条件的帖子DataSet
	   /// </summary>
	   /// <param name="_postpramsinfo">参数列表</param>
	   /// <returns>指定条件的帖子DataSet</returns>
	   public DataTable GetPostListTitle(int Tid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,Tid)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `pid`, `title`, `poster`, `posterid`,`message` FROM `" + posttablename + "` WHERE `tid`=?tid ORDER BY `pid`", prams).Tables[0];

	   }

	   /// <summary>
	   /// 获取指定条件的帖子DataReader
	   /// </summary>
	   /// <param name="_postpramsinfo">参数列表</param>
	   /// <returns>指定条件的帖子DataReader</returns>
	   public IDataReader GetPostListByCondition(PostpramsInfo _postpramsinfo, string posttablename)
	   {
		   IDataReader reader;
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Tid),
									 DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,_postpramsinfo.Pagesize),
									 DbHelper.MakeInParam("?pageindex",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Pageindex),
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,100,_postpramsinfo.Condition),
									 DbHelper.MakeInParam("?posttablename",(DbType)MySqlDbType.VarString,20,posttablename)
								 };

		   IDataParameter[] prams1 = {
									  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Tid)
								  };

		   //string sql1 = "SELECT `aid`, `tid`, `pid`, `postdatetime`, `readperm`, `filename`, `description`, `filetype`, "
		   //    + "`filesize`, `attachment`, `downloads` FROM `dnt_attachments` WHERE `tid`=" + _postpramsinfo.Tid;



		   //pp = DbHelper.ExecuteReader(CommandType.Text, sql1, prams1);

		   string sql = "";
		   sql = "SELECT " +
			   "`" + posttablename + "`.`pid`," +
			   "`" + posttablename + "`.`fid`," +
			   "`" + posttablename + "`.`title`," +
			   "`" + posttablename + "`.`layer`," +
			   "`" + posttablename + "`.`message`," +
			   "`" + posttablename + "`.`ip`," +
			   "`" + posttablename + "`.`lastedit`," +
			   "`" + posttablename + "`.`postdatetime`," +
			   "`" + posttablename + "`.`attachment`," +
			   "`" + posttablename + "`.`poster`," +
			   "`" + posttablename + "`.`posterid`," +
			   "`" + posttablename + "`.`invisible`," +
			   "`" + posttablename + "`.`usesig`," +
			   "`" + posttablename + "`.`htmlon`," +
			   "`" + posttablename + "`.`smileyoff`," +
			   "`" + posttablename + "`.`parseurloff`," +
			   "`" + posttablename + "`.`bbcodeoff`," +
			   "`" + posttablename + "`.`rate`," +
			   "`" + posttablename + "`.`ratetimes`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`spaceid`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`nickname`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`username`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`groupid`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`email`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`showemail`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`digestposts`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`credits`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`gender`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`bday`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits1`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits2`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits3`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits4`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits5`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits6`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits7`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits8`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`posts`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`joindate`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`onlinestate`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`lastactivity`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`invisible` AS `userinvisible`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatar`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatarwidth`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatarheight`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`medals`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`sightml` AS signature," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`location`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`customstatus`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`website`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`icq`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`qq`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`msn`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`yahoo`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`skype` " +
			   "FROM `" + posttablename + "` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` ON `" + BaseConfigs.GetTablePrefix + "users`.`uid`=`" + posttablename + "`.`posterid` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "userfields`.`uid`=`" + BaseConfigs.GetTablePrefix + "users`.`uid` WHERE `" + posttablename + "`.`tid`=" + _postpramsinfo.Tid + " AND `" + posttablename + "`.`invisible`=0 AND " + _postpramsinfo.Condition + " ORDER BY `" + posttablename + "`.`pid` LIMIT " + ((_postpramsinfo.Pageindex - 1) * _postpramsinfo.Pagesize).ToString() + "," + _postpramsinfo.Pagesize.ToString();
 
		   reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
		   return reader;
	   }

	   /// <summary>
	   /// 获取指定条件的帖子DataReader
	   /// </summary>
	   /// <param name="_postpramsinfo">参数列表</param>
	   /// <returns>指定条件的帖子DataReader</returns>
	   public IDataReader GetPostList(PostpramsInfo _postpramsinfo, string posttablename)
	   {
		   string sql;
		   IDataParameter[] prams1 = {
									  DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Tid)
								  };

		   //string sql1 = "SELECT `aid`, `tid`, `pid`, `postdatetime`, `readperm`, `filename`, `description`, `filetype`, "
		   //    + "`filesize`, `attachment`, `downloads` FROM `" + BaseConfigs.GetTablePrefix + "attachments` WHERE `tid`=" + _postpramsinfo.Tid;


		   //pp = DbHelper.ExecuteReader(CommandType.Text, sql1, prams1);

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,_postpramsinfo.Pagesize),
									 DbHelper.MakeInParam("?posttablename",(DbType)MySqlDbType.VarString,20,posttablename),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Tid)
								 };

		   #region 

		   sql = "SELECT " +
			   "`" + posttablename + "`.`pid`,`" + posttablename + "`.`fid`," +
			   "`" + posttablename + "`.`title`,`" + posttablename + "`.`layer`," +
			   "`" + posttablename + "`.`message`,`" + posttablename + "`.`ip`, " +
			   "`" + posttablename + "`.`lastedit`,`" + posttablename + "`.`postdatetime`," +
			   "`" + posttablename + "`.`attachment`,`" + posttablename + "`.`poster`," +
			   "`" + posttablename + "`.`posterid`,`" + posttablename + "`.`invisible`," +
			   "`" + posttablename + "`.`usesig`, " +
			   "`" + posttablename + "`.`htmlon`,`" + posttablename + "`.`smileyoff`," +
			   "`" + posttablename + "`.`parseurloff`,`" + posttablename + "`.`bbcodeoff`," +
			   "`" + posttablename + "`.`rate`,`" + posttablename + "`.`ratetimes`, " +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`nickname`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`spaceid`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`username`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`groupid`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`email`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`showemail`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`gender`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`bday`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`digestposts`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`credits`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits1`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits2`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits3`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits4`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits5`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits6`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits7`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`extcredits8`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`posts`, " +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`joindate`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`onlinestate`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`lastactivity`," +
			   "`" + BaseConfigs.GetTablePrefix + "users`.`invisible` AS `userinvisible`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatar`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatarwidth`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`avatarheight`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`medals`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`sightml` AS signature," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`location`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`customstatus`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`website`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`icq`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`qq`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`msn`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`yahoo`," +
			   "`" + BaseConfigs.GetTablePrefix + "userfields`.`skype` " +
			   "FROM (`" + posttablename + "` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` ON `" + BaseConfigs.GetTablePrefix + "users`.`uid`=`" + posttablename + "`.`posterid`) LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "userfields`.`uid`=`" + BaseConfigs.GetTablePrefix + "users`.`uid` WHERE `" + posttablename + "`.`tid`=" + _postpramsinfo.Tid + " AND `" + posttablename + "`.`invisible`=0 ORDER BY `" + posttablename + "`.`pid` LIMIT " + ((_postpramsinfo.Pageindex - 1) * _postpramsinfo.Pagesize).ToString() + "," + _postpramsinfo.Pagesize.ToString();

		   #endregion

		   return DbHelper.ExecuteReader(CommandType.Text, sql, prams);



	   }


	   /// <summary>
	   /// 返回指定主题的最后回复帖子
	   /// </summary>
	   /// <param name="tid"></param>
	   /// <returns></returns>
	   public DataTable GetLastPostByTid(int tid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
		   };
		   DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM {0} WHERE `tid` = ?tid ORDER BY `pid` DESC LIMIT 1", posttablename), prams);

		   if (ds != null)
		   {
			   if (ds.Tables.Count > 0)
			   {
				   return ds.Tables[0];
			   }
		   }
		   return new DataTable();
	   }

	   /// <summary>
	   /// 获得最后回复的帖子列表
	   /// </summary>
	   /// <param name="_postpramsinfo">参数列表</param>
	   /// <returns>帖子列表</returns>
	   public DataTable GetLastPostList(PostpramsInfo _postpramsinfo, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?postnum",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Pagesize),
									 DbHelper.MakeInParam("?posttablename",(DbType)MySqlDbType.VarString,20,posttablename),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Tid),
		   };
		   string sql = "SELECT  `" + posttablename + "`.`pid`, `" + posttablename + "`.`fid`, `" + posttablename + "`.`layer`, `" + posttablename + "`.`posterid`, `" + posttablename + "`.`title`, `" + posttablename + "`.`message`, `" + posttablename + "`.`postdatetime`, `" + posttablename + "`.`attachment`, `" + posttablename + "`.`poster`, `" + posttablename + "`.`posterid`, `" + posttablename + "`.`invisible`, `" + posttablename + "`.`usesig`, `" + posttablename + "`.`htmlon`, `" + posttablename + "`.`smileyoff`, `" + posttablename + "`.`parseurloff`, `" + posttablename + "`.`bbcodeoff`, `" + posttablename + "`.`rate`, `" + posttablename + "`.`ratetimes`, `" + BaseConfigs.GetTablePrefix + "users`.`username`, `" + BaseConfigs.GetTablePrefix + "users`.`email`, `" + BaseConfigs.GetTablePrefix + "users`.`showemail`, `" + BaseConfigs.GetTablePrefix + "userfields`.`avatar`, `" + BaseConfigs.GetTablePrefix + "userfields`.`avatarwidth`, `" + BaseConfigs.GetTablePrefix + "userfields`.`avatarheight`, `" + BaseConfigs.GetTablePrefix + "userfields`.`sightml` AS signature, `" + BaseConfigs.GetTablePrefix + "userfields`.`location`, `" + BaseConfigs.GetTablePrefix + "userfields`.`customstatus` FROM (`" + posttablename + "` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` ON `" + BaseConfigs.GetTablePrefix + "users`.`uid`=`" + posttablename + "`.`posterid`) LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "userfields`.`uid`=`" + BaseConfigs.GetTablePrefix + "users`.`uid` WHERE `" + posttablename + "`.`tid`=?tid ORDER BY `" + posttablename + "`.`pid` DESC LIMIT " + _postpramsinfo.Pagesize.ToString();

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //DataTable dt = DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getlastpostlist", BaseConfigs.GetTablePrefix), prams).Tables[0];

		   return dt;
	   }

	   /// <summary>
	   /// 获得单个帖子的信息, 包括发帖人的一般资料
	   /// </summary>
	   /// <param name="_postpramsinfo">参数列表</param>
	   /// <returns>帖子的信息</returns>
	   public IDataReader GetSinglePost(out IDataReader _Attachments,PostpramsInfo _postpramsinfo, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Pid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,_postpramsinfo.Tid)
								 };


		   _Attachments = DbHelper.ExecuteReader(CommandType.Text, "SELECT `aid`, `tid`, `pid`, `postdatetime`, `readperm`, `filename`, `description`, `filetype`, `filesize`, `attachment`, `downloads` FROM `"+BaseConfigs.GetTablePrefix+"attachments` WHERE `tid`=?tid", prams);


		   string sql = @"SELECT 

                            `{0}`.`pid`,
                            `{0}`.`fid`,
                            `{0}`.`title`,
                            `{0}`.`layer`,
                            `{0}`.`message`,
                            `{0}`.`ip`,
                            `{0}`.`lastedit`,
                            `{0}`.`postdatetime`,
                            `{0}`.`attachment`,
                            `{0}`.`poster`,
                            `{0}`.`invisible`,
                            `{0}`.`usesig`,
                            `{0}`.`htmlon`,
                            `{0}`.`smileyoff`,
                            `{0}`.`parseurloff`,
                            `{0}`.`bbcodeoff`,
                            `{0}`.`rate`,
                            `{0}`.`ratetimes`,
                            `{0}`.`posterid`,
                            `{1}users`.`nickname`,
                            `{1}users`.`username`,
                            `{1}users`.`groupid`,
                            `{1}users`.`spaceid`,
                            `{1}users`.`email`,
                            `{1}users`.`showemail`,
                            `{1}users`.`digestposts`,
                            `{1}users`.`credits`,
                            `{1}users`.`extcredits1`,
                            `{1}users`.`extcredits2`,
                            `{1}users`.`extcredits3`,
                            `{1}users`.`extcredits4`,
                            `{1}users`.`extcredits5`,
                            `{1}users`.`extcredits6`,
                            `{1}users`.`extcredits7`,
                            `{1}users`.`extcredits8`,
                            `{1}users`.`bday`,
                            `{1}users`.`gender`,
                            `{1}users`.`posts`,
                            `{1}users`.`joindate`,
                            `{1}users`.`onlinestate`,
                            `{1}users`.`lastactivity`,
                            `{1}users`.`invisible`,
                            `{1}userfields`.`avatar`,
                            `{1}userfields`.`avatarwidth`,
                            `{1}userfields`.`avatarheight`,
                            `{1}userfields`.`medals`,
                            `{1}userfields`.`sightml` AS signature,
                            `{1}userfields`.`location`,
                            `{1}userfields`.`customstatus`,
                            `{1}userfields`.`website`,
                            `{1}userfields`.`icq`,
                            `{1}userfields`.`qq`,
                            `{1}userfields`.`msn`,
                            `{1}userfields`.`yahoo`,
                            `{1}userfields`.`skype`
            FROM `{0}` LEFT JOIN `" + BaseConfigs.GetTablePrefix+"users` ON `{1}users`.`uid`=`{0}`.`posterid` LEFT JOIN `{1}userfields` ON `{1}userfields`.`uid`=`{1}users`.`uid` WHERE `{0}`.`pid`=" + _postpramsinfo.Pid.ToString() + " LIMIT 1";



		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text,string.Format(sql,BaseConfigs.GetTablePrefix+"posts"+posttableid,BaseConfigs.GetTablePrefix),prams);

		   return reader;
	   }

	   public DataTable GetPostTree(int tid, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };
		   String sql = "SELECT `pid`, `layer`, `title`, `poster`, `posterid`,`postdatetime`,`message` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid`=?tid AND `invisible`=0 ORDER BY `parentid`";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];

		   // return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getpost{1}tree", BaseConfigs.GetTablePrefix, posttableid), prams).Tables[0];

	   }

	   /// <summary>
	   /// 按条件获取指定tid的帖子总数
	   /// </summary>
	   /// <param name="tid">帖子的tid</param>
	   /// <returns>指定tid的帖子总数</returns>
	   public int GetPostCount(int tid, string condition, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?posttablename", (DbType)MySqlDbType.VarString, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid)),
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,100,condition)
								 };

		   String sql = "SELECT COUNT(pid) FROM `?posttablename` WHERE ?condition AND `layer`>=0";
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
		   //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getpostcountbycondition", BaseConfigs.GetTablePrefix), prams), 0);

	   }

	   /// <summary>
	   /// 获得指定主题的第一个帖子的id
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <returns>帖子id</returns>
	   public int GetFirstPostId(int tid, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };

		   String sql = "";
		   sql = "SELECT `pid` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid`=?tid ORDER BY `pid` LIMIT 1";
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString(), -1);
		   //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getfirstpost{1}id", BaseConfigs.GetTablePrefix, posttableid), prams).ToString(), -1);

	   }

	   /// <summary>
	   /// 判断指定用户是否是指定主题的回复者
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="uid">用户id</param>
	   /// <returns>是否是指定主题的回复者</returns>
	   public bool IsReplier(int tid, int uid, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid)

								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(`pid`) AS `pidcount` FROM `{0}posts{1}` WHERE `tid` = ?tid AND `posterid`=?uid AND ?uid>0", BaseConfigs.GetTablePrefix, posttableid), prams), 0) > 0;
	   }

	   /// <summary>
	   /// 更新帖子的评分值
	   /// </summary>
	   /// <param name="tid">主题ID</param>
	   /// <param name="postidlist">帖子ID列表</param>
	   /// <returns>更新的帖子数量</returns>
	   public int UpdatePostRate(int tid, string postidlist, string posttableid)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}posts{1}` SET `ratetimes` = `ratetimes` + 1 WHERE `pid` IN ({2})", BaseConfigs.GetTablePrefix, posttableid, postidlist));
	   }

	   /// <summary>
	   /// 获取帖子评分列表
	   /// </summary>
	   /// <param name="pid">帖子列表</param>
	   /// <returns></returns>
	   public DataTable GetPostRateList(int pid, int displayRateCount)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pid",(DbType)MySqlDbType.Int32,4,pid)
								 };
		   string commandText = string.Format("SELECT * FROM `{1}ratelog` WHERE `pid`=?pid ORDER BY `id` DESC LIMIT {0}", displayRateCount, BaseConfigs.GetTablePrefix);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText, prams).Tables[0];
	   }

	   /// <summary>
	   /// 获取新主题
	   /// </summary>
	   /// <param name="forumidlist">不允许游客访问的版块ID列表</param>
	   /// <returns></returns>
	   public IDataReader GetNewTopics(string forumidlist)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?fidlist", (DbType)MySqlDbType.VarString, 500, forumidlist);

		   String sql = "";
		   string currenttable = Posts.GetPostTableName();
		   if (forumidlist != null)
		   {
			   sql = "SELECT `" + currenttable + "`.`tid`, `" + currenttable + "`.`title`, `" + currenttable + "`.`poster`, `" + currenttable + "`.`postdatetime`, `" + currenttable + "`.`message`,`" + BaseConfigs.GetTablePrefix + "forums`.`name` FROM `" + currenttable + "`  LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forums` ON `" + currenttable + "`.`fid`=`" + BaseConfigs.GetTablePrefix + "forums`.`fid` WHERE  `" + BaseConfigs.GetTablePrefix + "forums`.`fid` NOT IN (?fidlist)  AND `" + currenttable + "`.`layer`=0 ORDER BY `" + currenttable + "`.`pid` DESC LIMIT 10";
		   }
		   else
		   {
			   sql = "SELECT `" + currenttable + "`.`tid`, `" + currenttable + "`.`title`, `" + currenttable + "`.`poster`, `" + currenttable + "`.`postdatetime`, `" + currenttable + "`.`message`,`" + BaseConfigs.GetTablePrefix + "forums`.`name` FROM `" + currenttable + "`  LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forums` ON `" + currenttable + "`.`fid`=`" + BaseConfigs.GetTablePrefix + "forums`.`fid` WHERE `" + currenttable + "`.`layer`=0 ORDER BY `" + currenttable + "`.`pid` DESC LIMIT 10";
		   }
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, parm);
		   //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getnewtopics", parm);
		   return reader;
	   }

	   /// <summary>
	   /// 获取版块新主题
	   /// </summary>
	   /// <param name="fid">版块Ｉｄ</param>
	   /// <returns></returns>
	   public IDataReader GetForumNewTopics(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,4,fid)

								 };

		   String sql = "SELECT `" + BaseConfigs.GetTablePrefix + "topics`.`tid`,`" + BaseConfigs.GetTablePrefix + "topics`.`title`,`" + BaseConfigs.GetTablePrefix + "topics`.`poster`,`" + BaseConfigs.GetTablePrefix + "topics`.`postdatetime`,`" + BaseConfigs.GetTablePrefix + "posts1`.`message` FROM `" + BaseConfigs.GetTablePrefix + "topics` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "posts1` ON `" + BaseConfigs.GetTablePrefix + "topics`.`tid`=`" + BaseConfigs.GetTablePrefix + "posts1`.`tid`  WHERE `" + BaseConfigs.GetTablePrefix + "posts1`.`layer`=0 AND  `" + BaseConfigs.GetTablePrefix + "topics`.`fid`=?fid ORDER BY `lastpost` DESC LIMIT 10";
		   return DbHelper.ExecuteReader(CommandType.Text, sql, prams);
	   }


	   /// <summary>
	   /// 创建搜索缓存
	   /// </summary>
	   /// <param name="cacheinfo">搜索缓存信息</param>
	   /// <returns>搜索缓存id</returns>
	   public int CreateSearchCache(SearchCacheInfo cacheinfo)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?keywords",(DbType)MySqlDbType.VarChar,255,cacheinfo.Keywords),
									 DbHelper.MakeInParam("?searchstring",(DbType)MySqlDbType.VarChar,255,cacheinfo.Searchstring.Substring(0,255)),
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.VarChar,15,cacheinfo.Ip),
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,cacheinfo.Uid),
									 DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32,4,cacheinfo.Groupid),
									 DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8, DateTime.Parse(cacheinfo.Postdatetime)),
									 DbHelper.MakeInParam("?expiration",(DbType)MySqlDbType.VarString,19,cacheinfo.Expiration),
									 DbHelper.MakeInParam("?topics",(DbType)MySqlDbType.Int32,4,cacheinfo.Topics),
									 DbHelper.MakeInParam("?tids",(DbType)MySqlDbType.String,0,cacheinfo.Tids)
								 };


		   string strSQL = "";
		   int rtnValue = 0;

		   strSQL = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "searchcaches` " +
			   "(`keywords`,`searchstring`,`ip`,`uid`,`groupid`,`postdatetime`,`expiration`,`topics`,`tids`) " +
			   "VALUES " +
			   "(?keywords,?searchstring,?ip,?uid,?groupid,?postdatetime,?expiration,?topics,?tids)";


		   DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);

		   rtnValue = Int32.Parse(DbHelper.ExecuteDataset(CommandType.Text, "SELECT `searchid` FROM `" + BaseConfigs.GetTablePrefix + "searchcaches` ORDER BY `searchid` DESC LIMIT 1").Tables[0].Rows[0][0].ToString());

		   return rtnValue;


	   }



	   /// <summary>
	   /// 删除超过３０分钟的缓存记录
	   /// </summary>
	   public void DeleteExpriedSearchCache()
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?expiration",(DbType)MySqlDbType.Datetime,8,DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss"))
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"DELETE FROM `{0}searchcaches` WHERE `expiration`<?expiration", BaseConfigs.GetTablePrefix), prams);
	   }

	   /// <summary>
	   /// 获得搜索缓存
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetSearchCache(int searchid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?searchid",(DbType)MySqlDbType.Int32,4,searchid),
		   };
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT `tids` FROM `{0}searchcaches` WHERE searchid=?searchid LIMIT 1", BaseConfigs.GetTablePrefix), prams).Tables[0];
	   }

	   /// <summary>
	   /// 获得搜索的精华贴
	   /// </summary>
	   /// <param name="pagesize"></param>
	   /// <param name="strTids"></param>
	   /// <returns></returns>
	   public DataTable GetSearchDigestTopicsList(int pagesize, string strTids)
	   {
		   string commandText = string.Format("SELECT `{0}topics`.`tid`, `{0}topics`.`title`, `{0}topics`.`poster`, `{0}topics`.`posterid`, `{0}topics`.`postdatetime`, `{0}topics`.`replies`, `{0}topics`.`views`, `{0}topics`.`lastpost`,`{0}topics`.`lastposter`, `{0}forums`.`fid`,`{0}forums`.`name` AS `forumname` FROM `{0}topics` LEFT JOIN `{0}forums` ON `{0}forums`.`fid` = `{0}topics`.`fid` WHERE `{0}topics`.tid in({2}) LIMIT {1} ", BaseConfigs.GetTablePrefix, pagesize, strTids);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   /// <summary>
	   /// 获得按帖子搜索的主题列表
	   /// </summary>
	   /// <param name="pagesize"></param>
	   /// <param name="strTids"></param>
	   /// <returns></returns>
	   public DataTable GetSearchPostsTopicsList(int pagesize, string strTids, string postablename)
	   {
		   string commandText = string.Format("SELECT `{2}`.`tid`, `{2}`.`title`, `{2}`.`poster`, `{2}`.`posterid`, `{2}`.`postdatetime`,`{2}`.`lastedit`, `{2}`.`rate`, `{2}`.`ratetimes`, `{0}forums`.`fid`,`{0}forums`.`name` AS `forumname` FROM `{2}` LEFT JOIN `{0}forums` ON `{0}forums`.`fid` = `{2}`.`fid` WHERE `{2}`.pid in({3}) LIMIT {1}", BaseConfigs.GetTablePrefix, pagesize, postablename, strTids);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   /// <summary>
	   /// 按搜索获得主题列表
	   /// </summary>
	   /// <param name="pagesize"></param>
	   /// <param name="strTids"></param>
	   /// <returns></returns>
	   public DataTable GetSearchTopicsList(int pagesize, string strTids)
	   {
		   string commandText = string.Format("SELECT `{0}topics`.`tid`, `{0}topics`.`title`, `{0}topics`.`poster`, `{0}topics`.`posterid`, `{0}topics`.`postdatetime`, `{0}topics`.`replies`, `{0}topics`.`views`, `{0}topics`.`lastpost`,`{0}topics`.`lastposter`, `{0}forums`.`fid`,`{0}forums`.`name` AS `forumname` FROM `{0}topics` LEFT JOIN `{0}forums` ON `{0}forums`.`fid` = `{0}topics`.`fid` WHERE `{0}topics`.tid in({2}) LIMIT {1}", BaseConfigs.GetTablePrefix, pagesize, strTids);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   /// <summary>
	   /// 开启全文索引
	   /// </summary>
	   public void ConfirmFullTextEnable()
	   {
		   //string commandText = "IF(SELECT DATABASEPROPERTY(DB_NAME(), 'IsFullTextEnabled'))=0 EXEC sp_fulltext_DbHelper 'enable'";
		   ////DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
	   }

	   /// <summary>
	   /// 设置主题指定字段的属性值
	   /// </summary>
	   /// <param name="topiclist">要设置的主题列表</param>
	   /// <param name="field">要设置的字段</param>
	   /// <param name="intValue">属性值</param>
	   /// <returns>更新主题个数</returns>
	   public int SetTopicStatus(string topiclist, string field, int intValue)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?field", (DbType)MySqlDbType.Int32, 1, intValue)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}topics` SET `{1}` = ?field WHERE `tid` IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), prams);
	   }

	   /// <summary>
	   /// 设置主题指定字段的属性值
	   /// </summary>
	   /// <param name="topiclist">要设置的主题列表</param>
	   /// <param name="field">要设置的字段</param>
	   /// <param name="intValue">属性值</param>
	   /// <returns>更新主题个数</returns>
	   public int SetTopicStatus(string topiclist, string field, byte intValue)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?field", (DbType)MySqlDbType.Int16, 1, intValue)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}topics` SET `{1}` = ?field WHERE `tid` IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), prams);
	   }

	   /// <summary>
	   /// 设置主题指定字段的属性值(字符型)
	   /// </summary>
	   /// <param name="topiclist">要设置的主题列表</param>
	   /// <param name="field">要设置的字段</param>
	   /// <param name="intValue">属性值</param>
	   /// <returns>更新主题个数</returns>
	   public int SetTopicStatus(string topiclist, string field, string intValue)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?field", (DbType)MySqlDbType.VarString, 500, intValue)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}topics` SET `{1}` = ?field WHERE `tid` IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), prams);
	   }

	   public DataSet GetTopTopicList(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `tid`,`displayorder`,`fid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `displayorder` > 0 ORDER BY `fid`", prams);

	   }


	   public DataTable GetShortForums()
	   {
		   //DataTable topTable = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid`,`parentid`,`parentidlist`, `layer`, '' AS `temptidlist`,'' AS `tid2list`, '' AS `tidlist`,0 AS `tidcount`,0 AS `tid2count`,0 AS `tid3count` FROM `" + BaseConfigs.GetTablePrefix + "forums` ORDER BY `fid` DESC").Tables[0];
		   // SELECT `fid`,`parentid`,`parentidlist`, `layer`, CAST('' AS VARString(1000)) AS `temptidlist`,CAST('' AS VARString(1000)) AS `tid2list`, CAST('' AS VARString(1000)) AS `tidlist`,CAST(0 AS INT) AS `tidcount`,CAST(0 AS INT) AS `tid2count`,CAST(0 AS INT) AS `tid3count` FROM `" + BaseConfigs.GetTablePrefix + "forums` ORDER BY `fid` DESC
		   DataTable topTable = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid`,`parentid`,`parentidlist`, `layer`, CAST('' AS CHAR(1000)) AS `temptidlist`,CAST('' AS CHAR(1000)) AS `tid2list`, CAST('' AS CHAR(1000)) AS `tidlist`,CAST(0 AS SIGNED) AS `tidcount`,CAST(0 AS SIGNED) AS `tid2count`,CAST(0 AS SIGNED) AS `tid3count` FROM `" + BaseConfigs.GetTablePrefix + "forums` ORDER BY `fid` DESC").Tables[0];

		   return topTable;
	   }

	   public IDataReader GetUserListWithTopicList(string topiclist, int losslessdel)
	   {
		   IDataParameter[] prams =
						{
							DbHelper.MakeInParam("?Losslessdel", (DbType)MySqlDbType.Int32, 4, losslessdel)
						};
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `posterid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE DATEDIFF(`postdatetime`, NOW())<?Losslessdel AND `tid` in (" + topiclist + ")", prams);
	   }

	   public IDataReader GetUserListWithTopicList(string topiclist)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `posterid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` in (" + topiclist + ")");
	   }

	   /// <summary>
	   /// 将主题设置关闭/打开
	   /// </summary>
	   /// <param name="topiclist">要设置的主题列表</param>
	   /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
	   /// <returns>更新主题个数</returns>
	   public int SetTopicClose(string topiclist, short intValue)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?field", (DbType)MySqlDbType.UInt16, 2, intValue)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `closed` = ?field WHERE `tid` IN (" + topiclist + ") AND `closed` IN (0,1)", prams);

	   }

	   /// <summary>
	   /// 获得主题指定字段的属性值
	   /// </summary>
	   /// <param name="topiclist">主题列表</param>
	   /// <param name="field">要获得值的字段</param>
	   /// <returns>主题指定字段的状态</returns>
	   public int GetTopicStatus(string topiclist, string field)
	   {
		   return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "SELECT SUM(if((`" + field + "` is null),0,`" + field + "`)) AS `fieldcount` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` IN (" + topiclist + ")").Tables[0].Rows[0][0], 0);

	   }

	   /// <summary>
	   /// 删除指定主题
	   /// </summary>
	   /// <param name="topiclist"></param>
	   /// <param name="reval"></param>
	   /// <param name="posttableid"></param>
	   /// <returns></returns>
	   public int DeleteTopicByTidList(string topiclist, int reval, string posttableid)
	   {


		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tidlist", (DbType)MySqlDbType.VarChar, 200, topiclist),
									 DbHelper.MakeInParam("?posttablename", (DbType)MySqlDbType.VarChar, 200, BaseConfigs.GetTablePrefix + "posts" + posttableid)
								 };




		   int postcount = 0;
		   int topiccount = 0;
		   int todaycount = 0;
		   string sqlstr = "";
		   string fid = "";
		   int tempFid = 0;
		   int tempPosterid = 0;
		   int tempLayer = 0;
		   DateTime temppostdatetime;
		   string tempfidlist;

		   if (topiclist != "")
		   {
			   sqlstr = "SELECT `fid`,`posterid`,`layer`,`postdatetime` FROM `"+BaseConfigs.GetTablePrefix+"posts"+posttableid+"` WHERE `tid` IN (?tidlist)";
			   IDataReader ddr = DbHelper.ExecuteReader(CommandType.Text, sqlstr, prams);
			   while (ddr.Read())
			   {
				   tempFid = Int32.Parse(ddr["fid"].ToString());
				   tempPosterid = Int32.Parse(ddr["posterid"].ToString());
				   tempLayer = Int32.Parse(ddr["layer"].ToString());
				   temppostdatetime = Convert.ToDateTime(ddr["postdatetime"].ToString());
				   postcount = postcount + 1;
				   if (tempLayer == 0)
				   {
					   topiccount = topiccount + 1;
				   }


				   if (Convert.ToDateTime(temppostdatetime).ToShortDateString() == DateTime.Now.ToShortDateString())
				   {

					   todaycount = todaycount + 1;
				   }

				   //if(fid.IndexOf(",",0))
				   if (("," + tempFid.ToString() + ",").IndexOf(fid + ",") == 0)
				   {


					   tempfidlist = DbHelper.ExecuteScalar(CommandType.Text, "select IFNULL(`parentidlist`,'') FROM `"+BaseConfigs.GetTablePrefix+"forums` WHERE `fid` = "+tempFid+"", prams).ToString();
					   //if (tempfidlist == null)
					   //{
					   //    tempfidlist = "";

					   //}

					   if (tempfidlist != "")
					   {
						   fid = fid + "," + tempfidlist + "," + tempFid.ToString();
					   }
					   else
					   {
						   fid = fid + "," + tempFid.ToString();

					   }



				   }
				   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `"+BaseConfigs.GetTablePrefix+"users` SET `posts` = `posts`-1 WHERE `uid` =" + tempPosterid + "");

			   }
		   }










		   if (fid.Length > 0)
		   {
			   fid = fid.Substring(1, (fid.Length) - 1);

               
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totaltopic`=`totaltopic`-" + topiccount + ", `totalpost`=`totalpost` -" + postcount + "");
			   sqlstr = " UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `posts`=`posts` -" + postcount + ",`topics`=`topics` -" + topiccount + ",`todayposts` = `todayposts` - " + todaycount + " WHERE `fid` IN (" + fid + ")";
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstr, prams);

			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `tid` IN (" + topiclist + ")", prams);

			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "polls` WHERE `tid` IN (" + topiclist + ")", prams);
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "posts1` WHERE `tid` IN (" + topiclist + ")", prams);
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "mytopics` WHERE `tid` IN (" + topiclist + ")", prams);
		   }

		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `closed` IN (" + topiclist + ") OR `tid` IN (" + topiclist + ")", prams);





		   return 1;




	   }
	   //    #endregion

	   //    reval = DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);
	   //    return reval;
	   //}

	   public int DeleteClosedTopics(int fid, string topiclist)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=" + fid + " AND `closed` IN (" + topiclist + ")");
	   }

	   public int CopyTopicLink(int oldfid, string topiclist)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int16, 1, oldfid)
								 };

		   ///用户设置转移后保留原连接执行以下三步操作

		   ///1.向表中批量拷贝记录并将closed字段设置为原记录的tid*-1
		   string sql = string.Format("INSERT INTO `{0}topics` (`fid`,`iconid`,`typeid`,`readperm`,`price`,`poster`,`posterid`,`title`,`postdatetime`,`lastpost`,`lastposter`,`lastposterid`,`views`,`replies`,`displayorder`,`highlight`," +
			   "`digest`,`rate`,`poll`,`attachment`,`moderated`,`hide`,`lastpostid`,`magic`,`closed`) SELECT ?fid,`iconid`,`typeid`," +
			   "`readperm`,`price`,`poster`,`posterid`,`title`,`postdatetime`," +
			   "`lastpost`,`lastposter`,`lastposterid`,`views`,`replies`," +
			   "`displayorder`,`highlight`,`digest`,`rate`,`poll`," +
			   "`attachment`,`moderated`,`hide`,`lastpostid`,`magic`," +
			   "`tid` AS `closed` FROM `{0}topics` WHERE `tid` IN ({1})", BaseConfigs.GetTablePrefix, topiclist);

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }


	   public void UpdatePost(string topiclist, int fid, string posttable)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int16, 1, fid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + posttable + "` SET `fid`=?fid WHERE `tid` IN (" + topiclist + ")", prams);
	   }

	   public int UpdateTopic(string topiclist, int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int16, 1, fid)
								 };
		   //更新主题
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `fid`=?fid,`typeid`=0 WHERE `tid` IN (" + topiclist + ")", prams);
	   }


	   public void UpdatePostTid(string postidlist, int tid, string posttableid)
	   {
		   IDataParameter[] prams =
					{
						DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
					};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` SET `tid`=?tid WHERE `pid` IN (" + postidlist + ")", prams);
	   }


	   public void SetPrimaryPost(string subject, int tid, string[] postid, string posttableid)
	   {
		   IDataParameter[] prams1 =
					{
						DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, Utils.StrToInt(postid[0], 0)),
						DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 80, subject)
					};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` SET `title` = ?title, `parentid` = `pid`,`layer` = 0 WHERE `pid` = ?pid", prams1);
	   }

	   public void SetNewTopicProperty(int topicid, int Replies, int lastpostid, int lastposterid, string lastposter, DateTime lastpost)
	   {
		   IDataParameter[] prams2 =
					{
						DbHelper.MakeInParam("?lastpostid", (DbType)MySqlDbType.Int32, 4, lastpostid),
						DbHelper.MakeInParam("?lastposterid", (DbType)MySqlDbType.Int32, 4, lastposterid),
						DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.Datetime, 8, lastpost),
						DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 20, lastposter),
						DbHelper.MakeInParam("?replies", (DbType)MySqlDbType.Int32, 4, Replies),
						DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, topicid)

					};
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpostid`=?lastpostid,`lastposterid` = ?lastposterid, `lastpost` = ?lastpost, `lastposter` = ?lastposter, `replies` = ?replies WHERE `tid` = ?tid", prams2);
	   }

	   public int UpdatePostTidToAnotherTopic(int oldtid, int newtid, string posttableid)
	   {
		   IDataParameter[] prams0 =
				{
					DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, newtid),
					DbHelper.MakeInParam("?oldtid", (DbType)MySqlDbType.Int32, 4, oldtid)
				};

		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` SET `tid` = ?tid, `layer` =if(`layer`=0,1,`layer`)  WHERE `tid` = ?oldtid", prams0);
                                                                                                                                                                 
	   }

	   public int UpdateAttachmentTidToAnotherTopic(int oldtid, int newtid)
	   {
		   IDataParameter[] prams0 =
				{
					DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, newtid),
					DbHelper.MakeInParam("?oldtid", (DbType)MySqlDbType.Int32, 4, oldtid)
				};

		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "attachments` SET `tid`=?tid WHERE `tid`=?oldtid", prams0);
	   }

	   public int DeleteTopic(int tid)
	   {
		   IDataParameter[] prams1 =
				{
					DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
				};
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` = ?tid", prams1);

	   }

	   public int UpdateTopic(int tid, TopicInfo __topicinfo)
	   {
		   IDataParameter[] prams =
						{
							DbHelper.MakeInParam("?lastpostid", (DbType)MySqlDbType.Int32, 4, __topicinfo.Lastpostid),
							DbHelper.MakeInParam("?lastposterid", (DbType)MySqlDbType.Int32, 4, __topicinfo.Lastposterid),
							DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.Datetime, 8, DateTime.Parse(__topicinfo.Lastpost)),
							DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 20, __topicinfo.Lastposter),
							DbHelper.MakeInParam("?replies", (DbType)MySqlDbType.Int32, 4, __topicinfo.Replies),
							DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
						};
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpostid` = ?lastpostid,`lastposterid` = ?lastposterid, `lastpost` = ?lastpost, `lastposter` = ?lastposter, `replies` = `replies` + ?replies WHERE `tid` = ?tid", prams);
	   }

	   public int UpdateTopicReplies(int tid, int topicreplies)
	   {
		   IDataParameter[] prams =
						{
							DbHelper.MakeInParam("?replies", (DbType)MySqlDbType.Int32, 4, topicreplies),
							DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
						};
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `replies` = `replies` + ?replies WHERE `tid` = ?tid", prams);
	   }

	   public int RepairTopics(string topiclist, string posttable)
	   {
		   //string commandtext = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpost` = (SELECT `postdatetime` FROM `" + posttable + "` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = `" + posttable + "`.`tid` LIMIT 1),"
		   //                + "`lastpostid` = (SELECT `pid` FROM `" + posttable + "` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = `" + posttable + "`.`tid` LIMIT 1),"
		   //                + " `lastposter` = (SELECT `poster` FROM `" + posttable + "` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = `" + posttable + "`.`tid LIMIT 1`),"
		   //                + "`lastposterid` = (SELECT `posterid` FROM `" + posttable + "` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = `" + posttable + "`.`tid LIMIT 1`),"
		   //                + " `replies` = (SELECT COUNT(`pid`) FROM `" + posttable + "` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` = `" + posttable + "`.`tid`) - 1 "
		   //                + " WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`tid` IN (" + topiclist + ")";

		   //return DbHelper.ExecuteNonQuery(CommandType.Text, commandtext);

		   string[] topiclistp = topiclist.Split(',');
		   foreach (string s in topiclistp)
		   {
			   int tid = int.Parse(s);
			   string sql = "SELECT `postdatetime`,`pid`,`poster`,`posterid` FROM `" + posttable + "` WHERE `" + posttable + "`.`tid`=" + tid + " LIMIT 1";
			   IDataReader IDR = DbHelper.ExecuteReader(CommandType.Text, sql);
			   string sql1 = "select COUNT(`pid`)-1 FROM `" + posttable + "` WHERE `" + posttable + "`.`tid`=" + tid;
			   int p = int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql1).ToString());
			   if (IDR.Read())
			   {
				   DbHelper.ExecuteNonQuery("UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastpost` ='" + IDR["postdatetime"].ToString() + "' ,`lastpostid` =" + IDR["pid"].ToString() + " , `lastposter` ='" + IDR["poster"].ToString() + "' ,`lastposterid` =" + IDR["posterid"].ToString() + " , `replies` =" + p + "  WHERE `dnt_topics`.`tid`=" + tid);
			   }
			   IDR.Close();

		   }
		   return topiclistp.Length;
	   }

	   public IDataReader GetUserListWithPostList(string postlist, string posttableid)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `posterid` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `pid` in (" + postlist + ")");
	   }

	   public string CheckRateState(int userid, string pid)
	   {
		   IDataParameter[] prams =
					{
						DbHelper.MakeInParam("?pid", (DbType)MySqlDbType.Int32, 4, Utils.StrToFloat(pid, 0)),
						DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid)
					};
		   return DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT `pid` FROM `" + BaseConfigs.GetTablePrefix + "ratelog` WHERE `pid` = ?pid AND `uid` = ?uid", prams);
	   }

	   public IDataReader GetTopicListModeratorLog(int tid)
	   {
		   IDataParameter[] prams =
					{
						DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid),
		   };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `grouptitle`, `moderatorname`,`postdatetime`,`actions` FROM `" + BaseConfigs.GetTablePrefix + "moderatormanagelog` WHERE `tid` = ?tid ORDER BY `id` DESC LIMIT 1", prams);
	   }

	   /// <summary>
	   /// 重设主题类型
	   /// </summary>
	   /// <param name="topictypeid">主题类型</param>
	   /// <param name="topiclist">要设置的主题列表</param>
	   /// <returns></returns>
	   public int ResetTopicTypes(int topictypeid, string topiclist)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int32, 1, topictypeid),
									 DbHelper.MakeInParam("?topiclist", (DbType)MySqlDbType.VarString, 250, topiclist)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `typeid` = ?typeid WHERE `tid` IN (" + topiclist + ")", prams);
	   }

	   /// <summary>
	   /// 按照用户Id获取其回复过的主题总数
	   /// </summary>
	   /// <param name="userId"></param>
	   /// <returns></returns>
	   public int GetTopicsCountbyReplyUserId(int userId)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userId)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(DISTINCT `tid`) FROM `{0}myposts` WHERE `uid` = " + userId + "", BaseConfigs.GetTablePrefix), parms), 0);
	   }


	   public IDataReader GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize)
	   {

          
		   //  string temp = "SELECT DISTINCT `tid` FROM `" + BaseConfigs.GetTablePrefix + "myposts` WHERE `uid`=" + userId + " ORDER BY `tid` DESC LIMIT " + pageSize.ToString() + "";
		   //  IDataReader DDR = DbHelper.ExecuteReader(CommandType.Text, temp);
		   //  //List<string> albmidlist = new List<string>();
		   //  string tidlist = null;
           
		   //  if (DDR.HasRows)
		   //  {
		   //      while (DDR.Read())
		   //      {

		   //          tidlist = tidlist + DDR["tid"].ToString() + ",";

		   //      }
		   //      tidlist = tidlist.Substring(0, tidlist.Length - 1);
		   //  }
		   //  else
		   //  {

		   //      tidlist = "";
		   //  }

		   //  string sql = "SELECT `f`.`name`,`t`.`tid`, `t`.`fid`, `t`.`iconid`, `t`.`typeid`, `t`.`readperm`, `t`.`price`, `t`.`poster`, `t`.`posterid`, `t`.`title`, `t`.`postdatetime`, `t`.`lastpost`, `t`.`lastpostid`, `t`.`lastposter`, `t`.`lastposterid`, `t`.`views`, `t`.`replies`, `t`.`displayorder`, `t`.`highlight`, `t`.`digest`, `t`.`rate`, `t`.`hide`, `t`.`poll`, `t`.`attachment`, `t`.`moderated`, `t`.`closed`, `t`.`magic`" +
		   //"FROM `" + BaseConfigs.GetTablePrefix + "topics` AS `t`,`" + BaseConfigs.GetTablePrefix + "forums` AS `f` WHERE `t`.`fid`=`f`.`fid` AND `tid` IN ("+tidlist+") ORDER BY `tid` DESC LIMIT " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize.ToString();

		   //  IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);
		   string temp =null,sql=null;
		   IDataReader DDR=null;
		   if(pageIndex==1)
		   {
			   temp = "SELECT DISTINCT `tid` FROM `" + BaseConfigs.GetTablePrefix + "myposts` WHERE `uid`=" + userId + " ORDER BY `tid` DESC LIMIT " + pageSize.ToString() + "";
			   DDR = DbHelper.ExecuteReader(CommandType.Text, temp);
			   //List<string> albmidlist = new List<string>();
			   string tidlist = null;
           
			   if (DDR!=null)
			   {
				   while (DDR.Read())
				   {

					   tidlist = tidlist + DDR["tid"].ToString() + ",";

				   }
				   DDR.Close();
				   if (tidlist != null)
				   {
					   tidlist = tidlist.Substring(0, tidlist.Length - 1);
				   }
			   }
			   else
			   {

				   tidlist = "";
			   }

			   sql = @"SELECT `f`.`name`,`t`.`tid`, `t`.`fid`, `t`.`iconid`, `t`.`typeid`, `t`.`readperm`, `t`.`price`, `t`.`poster`, `t`.`posterid`, `t`.`title`, `t`.`postdatetime`, `t`.`lastpost`, `t`.`lastpostid`, `t`.`lastposter`, `t`.`lastposterid`, `t`.`views`, `t`.`replies`, `t`.`displayorder`, `t`.`highlight`, `t`.`digest`, `t`.`rate`, `t`.`hide`, `t`.`poll`, `t`.`attachment`, `t`.`moderated`, `t`.`closed`, `t`.`magic`
					 FROM `"+BaseConfigs.GetTablePrefix+"topics` AS `t`,`"+BaseConfigs.GetTablePrefix+"forums` AS `f` WHERE `t`.`fid`=`f`.`fid` AND `tid` IN ("+tidlist+") ORDER BY `tid` DESC";

		   }

		   else
		   {
			   temp = @"SELECT DISTINCT `tid` FROM `" + BaseConfigs.GetTablePrefix + "myposts` WHERE `uid`=" + userId + " AND `tid` < (SELECT MIN(`tid`) FROM (SELECT DISTINCT `tid` FROM `" + BaseConfigs.GetTablePrefix + "myposts` WHERE `uid`=" + userId + "  ORDER BY `tid` DESC LIMIT 0," + (pageIndex - 1) * pageSize + ") AS `ttt`) ORDER BY `tid` DESC LIMIT 0," + pageSize;
			   DDR = DbHelper.ExecuteReader(CommandType.Text, temp);
			   //List<string> albmidlist = new List<string>();
			   string tidlist = null;

			   if (DDR!=null)
			   {
				   while (DDR.Read())
				   {

					   tidlist = tidlist + DDR["tid"].ToString() + ",";

				   }
				   DDR.Close();
				   if (tidlist != null)
				   {
					   tidlist = tidlist.Substring(0, tidlist.Length - 1);
				   }
			   }
			   else
			   {

				   tidlist = "";
			   }



			   sql = @"SELECT `f`.`name`,`t`.`tid`, `t`.`fid`, `t`.`iconid`, `t`.`typeid`, `t`.`readperm`, `t`.`price`, `t`.`poster`, `t`.`posterid`, `t`.`title`, `t`.`postdatetime`, `t`.`lastpost`, `t`.`lastpostid`, `t`.`lastposter`, `t`.`lastposterid`, `t`.`views`, `t`.`replies`, `t`.`displayorder`, `t`.`highlight`, `t`.`digest`, `t`.`rate`, `t`.`hide`, `t`.`poll`, `t`.`attachment`, `t`.`moderated`, `t`.`closed`, `t`.`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` AS `t`,`" + BaseConfigs.GetTablePrefix + "forums` AS `f` WHERE `t`.`fid`=`f`.`fid`  AND `tid` IN (" + tidlist + ") ORDER BY `tid` DESC";



		   }


		   // IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmyposts", parms);
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);
		   return reader;
	   }
	   /// <summary>
	   /// 按照用户Id获取主题总数
	   /// </summary>
	   /// <param name="userId"></param>
	   /// <returns></returns>
	   public int GetTopicsCountbyUserId(int userId)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userId)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM `{0}mytopics` WHERE `uid` = ?uid", BaseConfigs.GetTablePrefix), parms), 0);
	   }


	   public IDataReader GetTopicsByUserId(int userId, int pageIndex, int pageSize)
	   {
		   //IDataParameter[] parms = {
		   //                            DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userId),
		   //                            DbHelper.MakeInParam("?pageindex", (DbType)MySqlDbType.Int32, 4, pageIndex),
		   //                            DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32, 4, pageSize)
		   //                       };

           

		   //string temp = "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "mytopics` WHERE `uid`=" + userId + " ORDER BY `tid` DESC LIMIT " + pageSize.ToString() + "";

		   //IDataReader DDR = DbHelper.ExecuteReader(CommandType.Text, temp);
		   ////List<string> albmidlist = new List<string>();
		   //string tidlist = null;
		   //if (DDR.HasRows)
		   //{
		   //    while (DDR.Read())
		   //    {

		   //        tidlist = tidlist + DDR["tid"].ToString() + ",";

		   //    }
		   //    tidlist = tidlist.Substring(0, tidlist.Length - 1);
		   //}
		   //else
		   //{

		   //    tidlist = "";
		   //}

		   //        string sql = "SELECT `f`.`name`,`t`.`tid`, `t`.`fid`, `t`.`iconid`, `t`.`typeid`, `t`.`readperm`, `t`.`price`, `t`.`poster`, `t`.`posterid`, `t`.`title`, `t`.`postdatetime`, `t`.`lastpost`, `t`.`lastpostid`, `t`.`lastposter`, `t`.`lastposterid`, `t`.`views`, `t`.`replies`, `t`.`displayorder`, `t`.`highlight`, `t`.`digest`, `t`.`rate`, `t`.`hide`, `t`.`poll`, `t`.`attachment`, `t`.`moderated`, `t`.`closed`, `t`.`magic`" +
		   //                            "FROM `" + BaseConfigs.GetTablePrefix + "topics` AS `t`,`" + BaseConfigs.GetTablePrefix + "forums` AS `f` WHERE `t`.`fid`=`f`.`fid` AND `tid` IN ("+tidlist.ToString()+") ORDER BY `tid` DESC LIMIT " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize.ToString();

		   string temp =null,sql=null;
		   IDataReader DDR=null;

		   if (pageIndex == 1)
		   {
			   temp = "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "mytopics` WHERE `uid`="+userId+" ORDER BY `tid` DESC LIMIT 0," + pageSize;
			   DDR = DbHelper.ExecuteReader(CommandType.Text, temp);
			   //List<string> albmidlist = new List<string>();
			   string tidlist = null;

			   if (DDR!=null)
			   {
				   while (DDR.Read())
				   {

					   tidlist = tidlist + DDR["tid"].ToString() + ",";

				   }
				   DDR.Close();
				   if (tidlist != null)
				   {
					   tidlist = tidlist.Substring(0, tidlist.Length - 1);
				   }
			   }
			   else
			   {

				   tidlist = "";
			   }

			   sql = "SELECT `f`.`name`,`t`.`tid`, `t`.`fid`, `t`.`iconid`, `t`.`typeid`, `t`.`readperm`, `t`.`price`, `t`.`poster`, `t`.`posterid`, `t`.`title`, `t`.`postdatetime`, `t`.`lastpost`, `t`.`lastpostid`, `t`.`lastposter`, `t`.`lastposterid`, `t`.`views`, `t`.`replies`, `t`.`displayorder`, `t`.`highlight`, `t`.`digest`, `t`.`rate`, `t`.`hide`, `t`.`poll`, `t`.`attachment`, `t`.`moderated`, `t`.`closed`, `t`.`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` AS `t`,`" + BaseConfigs.GetTablePrefix + "forums` AS `f` WHERE `t`.`fid`=`f`.`fid` AND `tid` IN (" + tidlist + ") ORDER BY `tid` DESC";
		   }
		   else
		   {





			   temp = "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "mytopics` WHERE `uid`=" + userId + " AND `tid` < (SELECT MIN(`tid`) FROM (SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "mytopics` WHERE `uid`=" + userId + " ORDER BY `tid` DESC LIMIT 0," + (pageIndex - 1) * pageSize + ") AS `ttt`) ORDER BY `tid` DESC LIMIT 0," + pageSize;

			   DDR = DbHelper.ExecuteReader(CommandType.Text, temp);
			   //List<string> albmidlist = new List<string>();
			   string tidlist = null;

			   if (DDR!=null)
			   {
				   while (DDR.Read())
				   {

					   tidlist = tidlist + DDR["tid"].ToString() + ",";

				   }
				   DDR.Close();
				   if (tidlist != null)
				   {
					   tidlist = tidlist.Substring(0, tidlist.Length - 1);
				   }
			   }
			   else
			   {

				   tidlist = "";
			   }
			   sql = "SELECT `f`.`name`,`t`.`tid`, `t`.`fid`, `t`.`iconid`, `t`.`typeid`, `t`.`readperm`, `t`.`price`, `t`.`poster`, `t`.`posterid`, `t`.`title`, `t`.`postdatetime`, `t`.`lastpost`, `t`.`lastpostid`, `t`.`lastposter`, `t`.`lastposterid`, `t`.`views`, `t`.`replies`, `t`.`displayorder`, `t`.`highlight`, `t`.`digest`, `t`.`rate`, `t`.`hide`, `t`.`poll`, `t`.`attachment`, `t`.`moderated`, `t`.`closed`, `t`.`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` AS `t`,`" + BaseConfigs.GetTablePrefix + "forums` AS `f` WHERE `t`.`fid`=`f`.`fid` AND `tid` IN ("+tidlist+") ORDER BY `tid` DESC";


		   }
            


		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);
		   //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmytopics", parms);
		   return reader;
	   }

	   public int CreateTopic(TopicInfo topicinfo)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int16, 2, topicinfo.Fid),
									 DbHelper.MakeInParam("?iconid", (DbType)MySqlDbType.Int16, 2, topicinfo.Iconid),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 60, topicinfo.Title),
									 DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int16, 2, topicinfo.Typeid),
									 DbHelper.MakeInParam("?readperm", (DbType)MySqlDbType.Int32, 4, topicinfo.Readperm),
									 DbHelper.MakeInParam("?price", (DbType)MySqlDbType.Int16, 2, topicinfo.Price),
									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarString,50, topicinfo.Poster),
									 DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, topicinfo.Posterid),
									 DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime,4, DateTime.Parse(topicinfo.Postdatetime)),
									 DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.Datetime, 0, topicinfo.Lastpost),
									 DbHelper.MakeInParam("?lastpostid", (DbType)MySqlDbType.Int32, 4, topicinfo.Lastpostid),
									 DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 15, topicinfo.Lastposter),
									 DbHelper.MakeInParam("?views", (DbType)MySqlDbType.Int32, 4, topicinfo.Views),
									 DbHelper.MakeInParam("?replies", (DbType)MySqlDbType.Int32, 4, topicinfo.Replies),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, topicinfo.Displayorder),
									 DbHelper.MakeInParam("?highlight", (DbType)MySqlDbType.VarString, 500, topicinfo.Highlight),
									 DbHelper.MakeInParam("?digest", (DbType)MySqlDbType.Int32, 4, topicinfo.Digest),
									 DbHelper.MakeInParam("?rate", (DbType)MySqlDbType.Int32, 4, topicinfo.Rate),
									 DbHelper.MakeInParam("?hide", (DbType)MySqlDbType.Int32, 4, topicinfo.Hide),
									 DbHelper.MakeInParam("?poll", (DbType)MySqlDbType.Int32, 4, topicinfo.Poll),
									 DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.Int32, 4, topicinfo.Attachment),
									 DbHelper.MakeInParam("?moderated", (DbType)MySqlDbType.Int32, 4, topicinfo.Moderated),
									 DbHelper.MakeInParam("?closed", (DbType)MySqlDbType.Int32, 4, topicinfo.Closed),
									 DbHelper.MakeInParam("?magic", (DbType)MySqlDbType.Int32, 4, topicinfo.Magic)


								 };

		   //DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM ["+BaseConfigs.GetTablePrefix+"topics] WHERE `tid`>(SELECT ISNULL(max(tid),0)-100 FROM ["+BaseConfigs.GetTablePrefix+"topics]) AND `lastpostid`=0");
		   //DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "topics`(`fid`, `iconid`, `title`, `typeid`, `readperm`, `price`, `poster`, `posterid`, `postdatetime`, `lastpost`, `lastpostid`, `lastposter`, `views`, `replies`, `displayorder`, `highlight`, `digest`, `rate`, `hide`, `poll`, `attachment`, `moderated`, `closed`, `magic`) VALUES(?fid, ?iconid, ?title, ?typeid, ?readperm, ?price, ?poster, ?posterid, ?postdatetime, ?lastpost, ?lastpostid, ?lastposter, ?views, ?replies, ?displayorder, ?highlight, ?digest, ?rate, ?hide, ?poll, ?attachment, ?moderated, ?closed, ?magic)", prams);


		   //return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createtopic", prams).Tables[0].Rows[0][0].ToString(), -1);
		   int topicid,id;

		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid`>(select IFNULL(max(`tid`),0)-100) AND `lastpostid`=0");



		   //string temp = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "topics`(`fid`, `iconid`, `title`, `typeid`, `readperm`, `price`, `poster`, `posterid`, `postdatetime`, `lastpost`, `lastpostid`, `lastposter`, `views`, `replies`, `displayorder`, `highlight`, `digest`, `rate`, `hide`, `poll`, `attachment`, `moderated`, `closed`, `magic`) VALUES(?fid, ?iconid, ?title, ?typeid, ?readperm, ?price, ?poster, ?posterid,'" + DateTime.Parse(topicinfo.Postdatetime) + "', ?lastpost, ?lastpostid, ?lastposter, ?views, ?replies, ?displayorder, ?highlight, ?digest, ?rate, ?hide, ?poll, ?attachment, ?moderated, ?closed, ?magic)";
		   string temp = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "topics`(`fid`, `iconid`, `title`, `typeid`, `readperm`, `price`, `poster`, `posterid`, `postdatetime`, `lastpost`, `lastpostid`, `lastposter`, `views`, `replies`, `displayorder`, `highlight`, `digest`, `rate`, `hide`, `poll`, `attachment`, `moderated`, `closed`, `magic`)" +
			   "VALUES(" + topicinfo.Fid + "," + topicinfo.Iconid + ",'" + topicinfo.Title + "'," + topicinfo.Typeid + ", " + topicinfo.Readperm + "," + topicinfo.Price + ",'" + topicinfo.Poster + "'," + topicinfo.Posterid + ",'" + DateTime.Parse(topicinfo.Postdatetime) + "','" + topicinfo.Lastpost + "'," + topicinfo.Lastpostid + ",'" + topicinfo.Lastposter + "'," + topicinfo.Views + ", " + topicinfo.Replies + "," + topicinfo.Displayorder + ",'" + topicinfo.Highlight + "'," + topicinfo.Digest + "," + topicinfo.Rate + "," + topicinfo.Hide + "," + topicinfo.Poll + "," + topicinfo.Attachment + "," + topicinfo.Moderated + "," + topicinfo.Closed + "," + topicinfo.Magic + ")";

		   DbHelper.ExecuteNonQuery(out id,CommandType.Text,temp,prams);
		   topicid = id;
		   if (topicinfo.Displayorder == 0)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `totaltopic`=`totaltopic` + 1");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forums` SET `topics` = `topics` + 1,`curtopics` = `curtopics` + 1 WHERE `fid` = ?fid", prams);
			   if (topicinfo.Posterid != -1)
			   {

				   DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "mytopics`(`tid`,`uid`,`dateline`) VALUES(" + topicid + ",  ?posterid,  '" + DateTime.Parse(topicinfo.Postdatetime) + "')", prams);
			   }


		   }


		   return Utils.StrToInt(topicid, -1);


	   }

	   /// <summary>
	   /// 增加父版块的主题数
	   /// </summary>
	   /// <param name="fpidlist">父板块id列表</param>
	   /// <param name="topics">主题数</param>
	   /// <param name="posts">贴子数</param>
	   public void AddParentForumTopics(string fpidlist, int topics, int posts)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?topics", (DbType)MySqlDbType.Int32, 4, topics)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}forums` SET `topics` = `topics` + ?topics WHERE `fid` IN ({1})", BaseConfigs.GetTablePrefix, fpidlist), prams);
	   }


	   public IDataReader GetTopicInfo(int tid, int fid, byte mode)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid),
									 DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32,4, tid),
		   };
		   IDataReader reader;
		   switch (mode)
		   {
			   case 1:
				   reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=?fid AND `tid`<?tid AND `displayorder`>=0 ORDER BY `tid` DESC", prams);
				   break;
			   case 2:
				   reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=?fid AND `tid`>?tid AND `displayorder`>=0 ORDER BY `tid` ASC", prams);
				   break;
			   default:
				   IDataParameter[] prams1 = {
											  //   DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid),
											  DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32,4, tid),
				   };
				   reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid`=?tid", prams1);
				   break;
		   }
		   return reader;
	   }


	   public IDataReader GetTopTopics(int fid, int pagesize, int currentpage, string tids)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,4,fid),
									 DbHelper.MakeInParam("?tids",(DbType)MySqlDbType.VarString,500,tids)

								 };
		   string sql = "";
		   sql = "SELECT `tid`,`fid`,`typeid`,`iconid`,`title`,`price`,`hide`,`readperm`, `poll`,`poster`,`posterid`,`replies`,`views`,`postdatetime`,`lastpost`,`lastposter`,`lastpostid`,`lastposterid`,`replies`,`highlight`,`digest`,`displayorder`,`closed`,`attachment`,`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `displayorder`>0 AND `tid` IN (" + tids + ") ORDER BY `lastpost` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);

		   //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettoptopiclist", prams);
		   return reader;
	   }


	   public IDataReader GetTopics(int fid, int pagesize, int pageindex, int startnum, string condition)
	   {
		   IDataParameter[] prams = {
									 //DbHelper.MakeInParam("@fid",(DbType)MySqlDbType.Int32,4,fid),
									 //DbHelper.MakeInParam("@pagesize", (DbType)MySqlDbType.Int32,4,pagesize),
									 //DbHelper.MakeInParam("@pageindex", (DbType)MySqlDbType.Int32,4,pageindex),
									 //DbHelper.MakeInParam("@startnum", (DbType)MySqlDbType.Int32,4,startnum),
									 DbHelper.MakeInParam("@condition", (DbType)MySqlDbType.VarChar,80,condition)									   
								 };

		   string strsql=string.Empty;
		   //if(pageindex==1)
		   //{
		   strsql = "SELECT `tid`,`iconid`,`typeid`,`title`,`price`,`hide`,`readperm`,`poll`,`poster`,`posterid`,`replies`,`views`,`postdatetime`,`lastpost`,`lastposter`,`lastpostid`,`lastposterid`,`replies`,`highlight`,`digest`,`displayorder`,`attachment`,`closed`,`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=" + fid + " AND `displayorder`=0  " + condition + "   ORDER BY `lastpostid` DESC LIMIT "+((pageindex - 1) * pagesize - startnum)+","+ pagesize;
		   //}
		   //else
		   ////            {



		   ////                strsql = @"SELECT `tid`,`iconid`,`typeid`,`title`,`price`,`hide`,`readperm`,
		   ////`poll`,`poster`,`posterid`,`replies`,`views`,`postdatetime`,`lastpost`,`lastposter`,
		   ////`lastpostid`,`lastposterid`,`replies`,`highlight`,`digest`,`displayorder`,`attachment`,`closed`,`magic` FROM 
		   ////`dnt_topics` WHERE `lastpostid` < (SELECT min(`lastpostid`)  FROM (SELECT `lastpostid` FROM `dnt_topics` WHERE `fid`=" + fid + " AND `displayorder`=0 " + condition + "  ORDER BY `lastpostid` DESC LIMIT 0," + ((pageindex - 1) * pagesize - startnum) + ") AS tblTmp ) AND `fid`=" + fid + "  AND `displayorder`=0 " + condition + " ORDER BY `lastpostid` DESC LIMIT 0," + pagesize; 
		   ////                }


		   //String sql = "";
		   //sql = string.Format("SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics" + "` WHERE `fid`={1} AND `displayorder`=0 {2} ORDER BY `lastpostid` DESC LIMIT " + ((currentpage - 1) * pagesize - startnum).ToString() + "," + pagesize.ToString(), pagesize, fid, condition);
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text,strsql,prams);
		   return reader;
	   }


	   public IDataReader GetTopicsByDate(int fid, int pagesize, int currentpage, int startnum, string condition, string orderby, int ascdesc)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,4,fid),
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,80,condition),
									 DbHelper.MakeInParam("?orderby",(DbType)MySqlDbType.VarString,80,orderby),
									 DbHelper.MakeInParam("?ascdesc",(DbType)MySqlDbType.Int32,4,ascdesc)

								 };
		   String sql = "";
		   String sorttype;
		   if (ascdesc == 0)
		   {
			   sorttype = "asc";
		   }
		   else
		   {
			   sorttype = "desc";
		   }

		   sql = "SELECT `tid`,`iconid`,`title`,`price`,`typeid`,`readperm`,`hide`,`poll`,`poster`,`posterid`,`replies`,`views`,`postdatetime`,`lastpost`,`lastposter`,`lastpostid`,`lastposterid`,`replies`,`highlight`,`digest`,`displayorder`,`attachment`,`closed`,`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid`=?fid AND `displayorder`=0 " + condition + " ORDER BY ?orderby " + sorttype + " LIMIT " + ((currentpage - 1) * pagesize - startnum).ToString() + "," + pagesize.ToString();

		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);

		   return reader;
	   }
        

	   public IDataReader GetTopicsByType(int pagesize, int currentpage, int startnum, string condition, int ascdesc)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,pagesize),
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,1000,condition)
								 };
		   String sql = "";
		   sql = "SELECT `tid`,`iconid`,`typeid`,`title`,`price`,`hide`,`readperm`,`poll`,`poster`,`posterid`,`replies`,`views`,`postdatetime`,`lastpost`,`lastposter`,`lastpostid`,`lastposterid`,`replies`,`highlight`,`digest`,`displayorder`,`attachment`,`closed`,`magic` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE  `displayorder`>=0 " + condition + "  ORDER BY `tid` DESC , `lastpostid` DESC  LIMIT " + ((currentpage - 1) * pagesize - startnum).ToString() + "," + pagesize.ToString();
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);

		   return reader;
	   }


	   public IDataReader GetTopicsByTypeDate(int pagesize, int currentpage, int startnum, string condition, string orderby, int ascdesc)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,1000,condition),
									 DbHelper.MakeInParam("?orderby",(DbType)MySqlDbType.VarString,80,orderby),
									 DbHelper.MakeInParam("?ascdesc",(DbType)MySqlDbType.Int32,4,ascdesc)

								 };

		   String sql = "";
		   String sorttype;

		   if (ascdesc == 0)
		   {
			   sorttype = "asc";
		   }
		   else
		   {
			   sorttype = "desc";
		   }


		   sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `displayorder`>=0 "+condition+" ORDER BY ?orderby " + sorttype + " LIMIT " + ((currentpage - 1) * pagesize - startnum).ToString() + "," + pagesize.ToString();
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);

		   return reader;
	   }


	   public DataTable GetTopicList(string topiclist, int displayorder)
	   {
		   string commandText = string.Format("SELECT * FROM `{0}topics` WHERE `displayorder`>{1} AND `tid` IN ({2})", BaseConfigs.GetTablePrefix, displayorder, topiclist);
		   DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, commandText);
		   if (ds != null)
		   {
			   if (ds.Tables.Count > 0)
			   {
				   return ds.Tables[0];
			   }
		   }

		   return null;
	   }

	   /// <summary>
	   /// 列新主题的回复数
	   /// </summary>
	   /// <param name="tid">主题ID</param>
	   public void UpdateTopicReplies(int tid, string posttableid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid)
								 };



		   int count =int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`pid`) FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` WHERE `tid`=?tid AND `invisible`=0", prams).ToString());
		   DbHelper.ExecuteDataset(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `replies`=" + (count - 1) + " WHERE `displayorder`>=0 AND `tid`=?tid", prams);

	   }

	   /// <summary>
	   /// 得到当前版块内正常(未关闭)主题总数
	   /// </summary>
	   /// <param name="fid">版块ID</param>
	   /// <returns>主题总数</returns>
	   public int GetTopicCount(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,4,fid)
								 };
		   string sql = "SELECT `curtopics` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `fid`=" + fid;
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
	   }

	   /// <summary>
	   /// 得到当前版块内(包括子版)正常(未关闭)主题总数
	   /// </summary>
	   /// <param name="fid">版块ID</param>
	   /// <returns>主题总数</returns>
	   public int GetAllTopicCount(int fid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,4,fid)
								 };
		   string sql = "SELECT COUNT(tid) FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE (`fid`=" + fid + "   OR   `fid`  IN (  SELECT fid  FROM `" + BaseConfigs.GetTablePrefix + "forums`  WHERE  INSTR(concat(',',RTRIM(parentidlist),','),'," + fid + ",') > 0))  AND `displayorder`>=0";
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
	   }

	   /// <summary>
	   /// 得到当前版块内正常(未关闭)主题总数
	   /// </summary>
	   /// <param name="fid">版块ID</param>
	   /// <returns>主题总数</returns>
	   public int GetTopicCount(int fid,int state, string condition)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?fid",(DbType)MySqlDbType.Int32,4,fid),
									 DbHelper.MakeInParam("?state",(DbType)MySqlDbType.Int32,4,state),
									 DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,80,condition)
								 };
		   String sql = null;
		   if ((int)(prams[1].Value) == -1)
		   {
			   sql = "SELECT COUNT(tid) FROM `" + BaseConfigs.GetTablePrefix + "topics" + "` WHERE `fid`=?fid AND `displayorder`>-1 AND`closed`<=1 "+condition;
		   }
		   else
		   {

			   sql = "SELECT COUNT(`tid`) FROM `" + BaseConfigs.GetTablePrefix + "topics" + "` WHERE `fid`=?fid AND `displayorder`>-1 AND `closed`=?state AND `closed`<=1 " + condition;
		   }


		   // return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiccountbycondition", prams).Tables[0].Rows[0][0].ToString(), -1);

		   return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows[0][0].ToString(), -1);

	   }

	   /// <summary>
	   /// 得到符合条件的主题总数
	   /// </summary>
	   /// <param name="condition">条件</param>
	   /// <returns>主题总数</returns>
	   public int GetTopicCount(string condition)
	   {
		   //IDataParameter[] prams = {
		   //                           DbHelper.MakeInParam("?condition",(DbType)MySqlDbType.VarString,1000,condition)
		   //                       };

		   //String sql = string.Format("SELECT COUNT(`tid`) FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `closed`<=1 {0}",condition);
		   //return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0].Rows[0][0].ToString(), -1);
		   string sql = "";
		   sql = "SELECT COUNT(`tid`) FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE  `displayorder`>-1 AND `closed`<=1 " + condition + "";

		   return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0].Rows[0][0].ToString(), -1);
	   }

	   /// <summary>
	   /// 更新主题标题
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="topictitle">新标题</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicTitle(int tid, string topictitle)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4, tid),
									 DbHelper.MakeInParam("?topictitle", (DbType)MySqlDbType.VarString, 60, topictitle)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `title` = ?topictitle WHERE `tid` = ?tid", prams);
	   }

	   /// <summary>
	   /// 更新主题图标id
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="iconid">主题图标id</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicIconID(int tid, int iconid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?iconid", (DbType)MySqlDbType.Int16, 2, iconid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4, tid)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `iconid` = ?iconid WHERE `tid` = ?tid", prams);
	   }

	   /// <summary>
	   /// 更新主题价格
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicPrice(int tid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4, tid),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `price` = 0 WHERE `tid` = ?tid", prams);
	   }

	   /// <summary>
	   /// 更新主题价格
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="price">价格</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicPrice(int tid, int price)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?price",(DbType)MySqlDbType.Int32,4, price),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4, tid)

								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `price` = ?price WHERE `tid` = ?tid", prams);
	   }

	   /// <summary>
	   /// 更新主题阅读权限
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="readperm">阅读权限</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicReadperm(int tid, int readperm)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?readperm",(DbType)MySqlDbType.Int32,4, readperm),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4, tid)

								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `readperm` = ?readperm WHERE `tid` = ?tid", prams);
	   }

	   /// <summary>
	   /// 更新主题为已被管理
	   /// </summary>
	   /// <param name="topiclist">主题id列表</param>
	   /// <param name="moderated">管理操作id</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicModerated(string topiclist, int moderated)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?moderated",(DbType)MySqlDbType.Int32,4, moderated),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `moderated` = ?moderated WHERE `tid` IN (" + topiclist + ")", prams);

	   }

	   /// <summary>
	   /// 更新主题
	   /// </summary>
	   /// <param name="topicinfo">主题信息</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopic(TopicInfo topicinfo)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4, topicinfo.Tid),
									 DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int16, 2, topicinfo.Fid),
									 DbHelper.MakeInParam("?iconid", (DbType)MySqlDbType.Int16, 2, topicinfo.Iconid),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 60, topicinfo.Title),
									 DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int16, 2, topicinfo.Typeid),
									 DbHelper.MakeInParam("?readperm", (DbType)MySqlDbType.Int32, 4, topicinfo.Readperm),
									 DbHelper.MakeInParam("?price", (DbType)MySqlDbType.Int16, 2, topicinfo.Price),
									 DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.VarString, 15, topicinfo.Poster),
									 DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, topicinfo.Posterid),
									 DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 4, DateTime.Parse(topicinfo.Postdatetime)),
									 DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.VarString, 0, topicinfo.Lastpost),
									 DbHelper.MakeInParam("?lastposter", (DbType)MySqlDbType.VarString, 15, topicinfo.Lastposter),
									 //DbHelper.MakeInParam("?views", (DbType)MySqlDbType.Int32, 4, topicinfo.Views),
									 DbHelper.MakeInParam("?replies", (DbType)MySqlDbType.Int32, 4, topicinfo.Replies),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, topicinfo.Displayorder),
									 DbHelper.MakeInParam("?highlight", (DbType)MySqlDbType.VarString, 500, topicinfo.Highlight),
									 DbHelper.MakeInParam("?digest", (DbType)MySqlDbType.Int32, 4, topicinfo.Digest),
									 DbHelper.MakeInParam("?rate", (DbType)MySqlDbType.Int32, 4, topicinfo.Rate),
									 DbHelper.MakeInParam("?hide", (DbType)MySqlDbType.Int32, 4, topicinfo.Hide),
									 DbHelper.MakeInParam("?poll", (DbType)MySqlDbType.Int32, 4, topicinfo.Poll),
									 DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.Int32, 4, topicinfo.Attachment),
									 DbHelper.MakeInParam("?moderated", (DbType)MySqlDbType.Int32, 4, topicinfo.Moderated),
									 DbHelper.MakeInParam("?closed", (DbType)MySqlDbType.Int32, 4, topicinfo.Closed),
									 DbHelper.MakeInParam("?magic", (DbType)MySqlDbType.Int32, 4, topicinfo.Magic)
								 };
		   String sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `fid`=?fid,`iconid`=?iconid,`title`=?title,`typeid`=?typeid,`readperm`=?readperm,`price`=?price,`poster`=?poster,`posterid`=?posterid," +
			   "`postdatetime`=?postdatetime,`lastpost`=?lastpost,`lastposter`=?lastposter,`replies`=?replies,`displayorder`=?displayorder,`highlight`=?highlight,`digest`=?digest," +
			   "`rate`=?rate,`hide`=?hide,`poll`=?poll,`attachment`=?attachment,`moderated`=?moderated,`closed`=?closed,`magic`=?magic WHERE `tid`=?tid";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);


		   //  return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatetopic", prams);
	   }

	   /// <summary>
	   /// 判断帖子列表是否都在当前板块
	   /// </summary>
	   /// <param name="topicidlist"></param>
	   /// <param name="fid"></param>
	   /// <returns></returns>
	   public bool InSameForum(string topicidlist, int fid)
	   {
		   string commandText = string.Format("SELECT COUNT(tid) FROM `{0}topics` WHERE `fid`={1} AND `tid` IN ({2})", BaseConfigs.GetTablePrefix, fid, topicidlist);
		   return Utils.SplitString(topicidlist, ",").Length == int.Parse(DbHelper.ExecuteScalar(CommandType.Text, commandText).ToString());
	   }

	   /// <summary>
	   /// 将主题设置为隐藏主题
	   /// </summary>
	   /// <param name="tid"></param>
	   /// <returns></returns>
	   public int UpdateTopicHide(int tid)
	   {
		   string sql = string.Format(@"UPDATE `{0}topics` SET `hide` = 1 WHERE `tid` = {1}", BaseConfigs.GetTablePrefix,tid);

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public DataTable GetTopicList(int forumid, int currentpage, int pagesize)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT `tid`,`title`,`replies` FROM `{1}topics` WHERE `fid`={2} AND `displayorder`>=0 ORDER BY `lastpostid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString(), pagesize.ToString(), BaseConfigs.GetTablePrefix, forumid.ToString())).Tables[0];
	   }

	   public DataTable GetTopicFidByTid(string tidlist)
	   {
		   string sql = "SELECT distinct `fid` From `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` IN(" + tidlist + ")";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetTopicTidByFid(string tidlist, int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32,4, fid)
			};
		   string sql = "SELECT `tid` From `" + BaseConfigs.GetTablePrefix + "topics` WHERE `tid` IN(" + tidlist + ") AND `fid`=?fid";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   /// <summary>
	   /// 更新主题浏览量
	   /// </summary>
	   /// <param name="tid">主题id</param>
	   /// <param name="viewcount">浏览量</param>
	   /// <returns>成功返回1，否则返回0</returns>
	   public int UpdateTopicViewCount(int tid, int viewcount)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?viewcount",(DbType)MySqlDbType.Int32,4,viewcount)
								 };
		   String sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "topics`  SET `views`= `views` + "+viewcount+" WHERE `tid`=?tid";
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		   //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatetopicviewcount",prams);
	   }

	   public string SearchTopics(int forumid, string keyword, string displayorder, string digest, string attachment, string poster, bool lowerupper, string viewsmin, string viewsmax,
		   string repliesmax, string repliesmin, string rate, string lastpost, DateTime postdatetimeStart, DateTime postdatetimeEnd)
	   {
		   string sqlstring = null;
		   sqlstring += " `tid`>0";

		   if (forumid != 0)
		   {
			   sqlstring += " AND `fid`=" + forumid;
		   }

		   if (keyword != "")
		   {
			   sqlstring += " And (";
			   foreach (string word in keyword.Split(','))
			   {
				   if (word.Trim() != "")
					   sqlstring += " `title` like '%" + RegEsc(word) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   switch (displayorder)
		   {
			   case "0":
				   break;
			   case "1":
				   sqlstring += " AND displayorder>0";
				   break;
			   case "2":
				   sqlstring += " AND displayorder<=0";
				   break;
		   }

		   switch (digest)
		   {
			   case "0":
				   break;
			   case "1":
				   sqlstring += " AND digest>=1";
				   break;
			   case "2":
				   sqlstring += " AND digest<1";
				   break;
		   }

		   switch (attachment)
		   {
			   case "0":
				   break;
			   case "1":
				   sqlstring += " AND attachment>0";
				   break;
			   case "2":
				   sqlstring += " AND attachment<=0";
				   break;
		   }

		   if (poster != "")
		   {
			   sqlstring += " AND (";
			   foreach (string postername in poster.Split(','))
			   {
				   if (postername.Trim() != "")
				   {
					   //不区别大小写
					   if (lowerupper)
					   {
						   sqlstring += " poster='" + postername + "' OR";
					   }
					   else
					   {
						   sqlstring += " poster COLLATE Chinese_PRC_CS_AS_WS ='" + postername + "' OR";
					   }
				   }
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   if (viewsmax != "")
		   {
			   sqlstring += " And views>" + viewsmax;
		   }

		   if (viewsmin != "")
		   {
			   sqlstring += " And views<" + viewsmin;
		   }

		   if (repliesmax != "")
		   {
			   sqlstring += " And replies>" + repliesmax;
		   }

		   if (repliesmin != "")
		   {
			   sqlstring += " And replies<" + repliesmin;
		   }

		   if (rate != "")
		   {
			   sqlstring += " And rate>" + rate;
		   }

		   if (lastpost != "")
		   {
			   sqlstring += " And datediff(lastpost,now())>="+lastpost;
		   }

           
		   sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);
		   return sqlstring;
	   }

	   public string SearchAttachment(int forumid, string posttablename, string filesizemin, string filesizemax, string downloadsmin, string downloadsmax, string postdatetime, string filename, string description, string poster)
	   {
		   string sqlstring = null;
		   sqlstring += " WHERE `aid` > 0";


		   if (forumid != 0)
		   {
			   sqlstring += " AND `pid` IN (SELECT PID FROM `" + posttablename + "` WHERE `fid`=" + forumid + ")";
		   }

		   if (filesizemin != "")
		   {
			   sqlstring += " AND `filesize`<" + filesizemin;
		   }

		   if (filesizemax != "")
		   {
			   sqlstring += " AND `filesize`>" + filesizemax;
		   }

		   if (downloadsmin != "")
		   {
			   sqlstring += " AND `downloads`<" + downloadsmin;
		   }

		   if (downloadsmax != "")
		   {
			   sqlstring += " AND `downloads`>" + downloadsmax;
		   }

		   if (postdatetime != "")
		   {
			   sqlstring += " AND datediff(postdatetime,NOW())>="+postdatetime;
		   }

		   if (filename != "")
		   {
			   sqlstring += " AND `filename` like '%" + RegEsc(filename) + "%'";
		   }


		   if (description != "")
		   {
			   sqlstring += " And (";
			   foreach (string word in description.Split(','))
			   {
				   if (word.Trim() != "")
					   sqlstring += " `description` like '%" + RegEsc(word) + "%' OR ";
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   if (poster != "")
		   {
			   sqlstring += " AND `pid` IN (SELECT `pid` FROM `" + posttablename + "` WHERE `poster`='" + poster + "')";
		   }

		   return sqlstring;
	   }

	   public string SearchPost(int forumid, string posttableid, DateTime postdatetimeStart, DateTime postdatetimeEnd, string poster, bool lowerupper, string ip, string message)
	   {
		   string sqlstring = null;
		   sqlstring += " `pid`>0 ";

		   if (forumid != 0)
		   {
			   sqlstring += " AND `fid`=" + forumid;
		   }

		   if (postdatetimeStart.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`>='" + postdatetimeStart.ToString() + "'";
		   }

		   if (postdatetimeEnd.ToString() != "")
		   {
			   sqlstring += " And `postdatetime`<='" + postdatetimeEnd.AddDays(1).ToString() + "'";
		   }

		   if (poster != "")
		   {
			   sqlstring += " AND (";
			   foreach (string postername in poster.Split(','))
			   {
				   if (postername.Trim() != "")
				   {
					   //不区别大小写
					   if (lowerupper)
					   {
						   sqlstring += " poster='" + postername + "' OR";
					   }
					   else
					   {
						   sqlstring += " poster COLLATE Chinese_PRC_CS_AS_WS ='" + postername + "' OR";
					   }
				   }
			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   if (ip != "")
		   {
			   sqlstring += " And `ip` like'%" + RegEsc(ip.Replace(".*", "")) + "%'";
		   }

		   if (message != "")
		   {
			   sqlstring += " AND (";
			   foreach (string messageinf in message.Split(','))
			   {
				   if (messageinf.Trim() != "")
				   {
					   sqlstring += " message like '%" + RegEsc(messageinf) + "%' OR";
				   }

			   }
			   sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
		   }

		   return sqlstring;
	   }


	   public string GetAttchTypeSql()
	   {
		   return "Select * From `" + BaseConfigs.GetTablePrefix + "attachtypes` Order BY `id` ASC";
	   }

	   public void IdentifyTopic(string topiclist, int identify)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?identify", (DbType)MySqlDbType.Int32, 4, identify)
								 };

		   string sql = string.Format("UPDATE `{0}topics` SET `identify`=?identify WHERE `tid` IN ({1})", BaseConfigs.GetTablePrefix, topiclist);
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public void UpdateTopic(int tid, string title, int posterid, string poster)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?tid", (DbType) MySqlDbType.Int32, 4, tid),
									 DbHelper.MakeInParam("?title", (DbType) MySqlDbType.VarString, 60, title),
									 DbHelper.MakeInParam("?posterid", (DbType) MySqlDbType.Int32, 4, posterid),
									 DbHelper.MakeInParam("?poster", (DbType) MySqlDbType.VarString, 20, poster)
								 };

		   string sql = string.Format("UPDATE `{0}topics` SET `title`=?title, `posterid`=?posterid, `poster`=?poster WHERE `tid`=?tid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public string GetTopicCountCondition(out string type, string gettype, int getnewtopic)
	   {

		   string condition = "";
		   type = string.Empty;
		   if (gettype == "digest")
		   {
			   type = "digest";
			   condition += " AND digest>0 ";
		   }

		   if (gettype == "newtopic")
		   {
			   type = "newtopic";
			   condition += " AND postdatetime>='" + DateTime.Now.AddMinutes(-getnewtopic).ToString("yyyy-MM-dd HH:mm:ss") + "'";
		   }
		   return condition;
	   }


	   public IDataReader GetSitemapNewTopics(string forumidlist)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?fidlist", (DbType)MySqlDbType.VarChar, 500, forumidlist);
		   string sql = null;
		   if (forumidlist != "")
		   {
			   sql = "SELECT `tid`, `fid`, `title`, `poster`, `postdatetime`, `lastpost`, `replies`, `views`, `digest` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `fid` NOT IN (?fidlist) ORDER BY `tid` DESC LIMIT 0,20";

		   }
		   else
		   {
			   sql = "SELECT `tid`, `fid`, `title`, `poster`, `postdatetime`, `lastpost`, `replies`, `views`, `digest` FROM `" + BaseConfigs.GetTablePrefix + "topics` ORDER BY `tid` DESC LIMIT 0,20";

		   }
		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, parm);
		   return reader;
	   }

	   public string GetRateLogCountCondition(int userid, string postidlist)
	   {

		   return "`uid`=" + userid + " AND `pid` = " + Utils.StrToInt(postidlist, 0).ToString();
	   }

	   //public void SetNewLastPid(int lastpostid, int tid)
	   //{

	   //    DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "select * from dnt_posts1 where pid not in (" + postidlist + ") and tid=" + topicid + " order by pid desc").Tables[0];
	   //    return dt;
	   //}

	   public DataTable GetOtherPostId(string postidlist, int topicid, int postid)
	   {

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "select * from " + BaseConfigs.GetTablePrefix + "posts" + postid + " where pid not in (" + postidlist + ") and tid=" + topicid + " order by pid desc").Tables[0];
		   return dt;
	   }

	   public int UpdatePostRate(int pid, float rate, string posttableid)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}posts{1}` SET `rate` = `rate` + {2} WHERE `pid` IN ({3})", BaseConfigs.GetTablePrefix, posttableid, rate, pid));
	   }

	   public int CancelPostRate(string postidlist, string posttableid)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}posts{1}` SET `rate` = 0, `ratetimes`=0 WHERE `pid` IN ({2})", BaseConfigs.GetTablePrefix, posttableid, postidlist));
	   }

	   public int GetPostCountByTid(int tid, string posttablename)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?tid", (DbType)MySqlDbType.Int32, 4, tid)
								 };

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`pid`) AS `postcount` FROM `" + posttablename + "` WHERE `tid` = ?tid AND `layer` <> 0", prams), 0);
	   }
	   #endregion 


	   #region SpaceManage
	   #region Space 个人数据操作

	   public void AddAlbumCategory(AlbumCategoryInfo aci)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.String, 50, aci.Title),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.String, 300, aci.Description),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, aci.Displayorder)
								 };

		   string sql = string.Format(@"INSERT INTO `{0}albumcategories`(`title`, `description`, `albumcount`, `displayorder`) VALUES(?title, ?description, 0, ?displayorder)", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }


	   public void UpdateAlbumCategory(AlbumCategoryInfo aci)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.String, 50, aci.Title),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.String, 300, aci.Description),
									 DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, aci.Displayorder),
									 DbHelper.MakeInParam("?albumcateid", (DbType)MySqlDbType.Int32, 4, aci.Albumcateid)
								 };

		   string sql = string.Format(@"UPDATE `{0}albumcategories`
                                         SET `title`=?title, `description`=?description, `displayorder`=?displayorder
                                         WHERE `albumcateid`=?albumcateid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }


	   public void DeleteAlbumCategory(int albumcateid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?albumcateid", (DbType)MySqlDbType.Int32, 4, albumcateid);

		   string sql = string.Format(@"DELETE FROM `{0}albumcategories`
                                         WHERE `albumcateid`=?albumcateid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }


	   public IDataReader GetSpaceConfigDataByUserID(int userid)
	   {
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` WHERE `userid` = " + userid);
		   return __IDataReader;
	   }


	   public IDataReader GetSpaceConfigDataBySpaceID(int spaceid)
	   {
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` WHERE `spaceid` = " + spaceid);
		   return __IDataReader;
	   }


	   /// <summary>
	   /// 保存用户space配置信息
	   /// </summary>
	   /// <param name="__spaceconfiginfo"></param>
	   ///
	   /// <returns></returns>
	   public bool SaveSpaceConfigData(SpaceConfigInfo __spaceconfiginfo)
	   {
		   //try
		   //{
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?spacetitle", (DbType)MySqlDbType.VarString, 100, __spaceconfiginfo.Spacetitle),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200, __spaceconfiginfo.Description),
									 DbHelper.MakeInParam("?blogdispmode", (DbType)MySqlDbType.Int16, 1, __spaceconfiginfo.BlogDispMode),
									 DbHelper.MakeInParam("?bpp", (DbType)MySqlDbType.Int32, 4, __spaceconfiginfo.Bpp),
									 DbHelper.MakeInParam("?commentpref", (DbType)MySqlDbType.Int16, 1, __spaceconfiginfo.Commentpref),
									 DbHelper.MakeInParam("?messagepref", (DbType)MySqlDbType.Int16, 1, __spaceconfiginfo.MessagePref),
									 DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.VarString, 100, __spaceconfiginfo.Rewritename),
									 DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, __spaceconfiginfo.ThemeID),
									 DbHelper.MakeInParam("?themepath", (DbType)MySqlDbType.VarString, 50, __spaceconfiginfo.ThemePath),
									 DbHelper.MakeInParam("?status", (DbType)MySqlDbType.Int32, 4, __spaceconfiginfo.Status),
									 DbHelper.MakeInParam("?updatedatetime", (DbType)MySqlDbType.Datetime, 4, DateTime.Now),

									 //DbHelper.MakeInParam("?defaulttab", (DbType)MySqlDbType.Int32, 4, DateTime.Now),

									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, __spaceconfiginfo.UserID)
								 };
		   string sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `spacetitle` = ?spacetitle ,`description` = ?description,`blogdispmode` = ?blogdispmode,`bpp` = ?bpp, `commentpref` = ?commentpref, `messagepref` = ?messagepref,  `rewritename` = ?rewritename,`themeid`=?themeid,`themepath` = ?themepath, `updatedatetime` = ?updatedatetime WHERE `userid` = ?userid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }


	   /// <summary>
	   /// 建议用户space信息
	   /// </summary>
	   /// <param name="__spaceconfiginfo"></param>
	   ///
	   /// <returns></returns>
	   public int AddSpaceConfigData(SpaceConfigInfo __spaceconfiginfo)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					//DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,__spaceconfiginfo.UserID),
					//DbHelper.MakeInParam("?spacetitle", (DbType)MySqlDbType.VarString, 100,__spaceconfiginfo.Spacetitle),
					//DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200,__spaceconfiginfo.Description),
					//DbHelper.MakeInParam("?blogdispmode", (DbType)MySqlDbType.Int32, 1,__spaceconfiginfo.BlogDispMode),
					//DbHelper.MakeInParam("?bpp", (DbType)MySqlDbType.Int32, 4,__spaceconfiginfo.Bpp),
					//DbHelper.MakeInParam("?commentpref", (DbType)MySqlDbType.Int32, 1,__spaceconfiginfo.CommentPref),
					//DbHelper.MakeInParam("?messagepref", (DbType)MySqlDbType.Int32, 1,__spaceconfiginfo.MessagePref),
					//DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.String, 100,__spaceconfiginfo.RewriteName),
					//DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4,__spaceconfiginfo.ThemeID),
					//DbHelper.MakeInParam("?themepath", (DbType)MySqlDbType.VarString, 50,__spaceconfiginfo.ThemePath),
					//DbHelper.MakeInParam("?postcount", (DbType)MySqlDbType.Int32, 4,__spaceconfiginfo.PostCount),
					//DbHelper.MakeInParam("?commentcount", (DbType)MySqlDbType.Int32, 4,__spaceconfiginfo.CommentCount),
					//DbHelper.MakeInParam("?visitedtimes", (DbType)MySqlDbType.Int32, 4,__spaceconfiginfo.VisitedTimes),
					//DbHelper.MakeInParam("?createdatetime", (DbType)MySqlDbType.Datetime, 4,__spaceconfiginfo.CreateDateTime),
					//DbHelper.MakeInParam("?updatedatetime", (DbType)MySqlDbType.Datetime, 4,__spaceconfiginfo.UpdateDateTime),
					DbHelper.MakeInParam("?status", (DbType)MySqlDbType.Int32, 4, __spaceconfiginfo.Status),
		   };
		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spaceconfigs` (`userid`, `spacetitle`, `description`, `blogdispmode`, `bpp`, `commentpref`, `messagepref`, `rewritename`, `themeid`, `themepath`, `postcount`, `commentcount`, `visitedtimes`, `createdatetime`, `updatedatetime`)" +
			   "VALUES (" + __spaceconfiginfo.UserID + ", '" + __spaceconfiginfo.Spacetitle + "', '" + __spaceconfiginfo.Description + "'," + __spaceconfiginfo.BlogDispMode + ", " + __spaceconfiginfo.Bpp + ", " + __spaceconfiginfo.Commentpref + "," + __spaceconfiginfo.MessagePref + ",'" + __spaceconfiginfo.Rewritename + "', " + __spaceconfiginfo.ThemeID + "," +
			   "'" + __spaceconfiginfo.ThemePath + "'," + __spaceconfiginfo.PostCount + "," + __spaceconfiginfo.CommentCount + "," + __spaceconfiginfo.VisitedTimes + ",'" + __spaceconfiginfo.CreateDateTime + "', '" + __spaceconfiginfo.UpdateDateTime + "')");
		   // string sql = "select  ??Identity";


		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return Int32.Parse(DbHelper.ExecuteDataset(CommandType.Text, "select spaceid from `" + BaseConfigs.GetTablePrefix + "spaceconfigs` order by spaceid desc LIMIT 1").Tables[0].Rows[0][0].ToString());




		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return 0;
		   //}
	   }

	   /// <summary>
	   /// 为当前用户的SPACE访问量加1
	   /// </summary>
	   /// <param name="userid"></param>
	   ///
	   /// <returns></returns>
	   public bool CountUserSpaceVisitedTimesByUserID(int userid)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,userid)
				};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `visitedtimes` = `visitedtimes` + 1 WHERE `userid` = ?userid", prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }


	   /// <summary>
	   /// 更新当前用户的SPACE日志数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <param name="errormsg"></param>
	   /// <returns></returns>
	   public bool CountUserSpacePostCountByUserID(int userid, int postcount)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?postcount", (DbType)MySqlDbType.Int32, 4,postcount),
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,userid)
				};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `postcount` = `postcount` + ?postcount  WHERE `userid` = ?userid", prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }


	   /// <summary>
	   /// 更新当前用户的SPACE评论数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <param name="errormsg"></param>
	   /// <returns></returns>
	   public bool CountUserSpaceCommentCountByUserID(int userid, int commentcount)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?commentcount", (DbType)MySqlDbType.Int32, 4,commentcount),
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,userid)
				};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `commentcount` = `commentcount` + ?commentcount  WHERE `userid` = ?userid", prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }
	   #endregion


	   #region Space 主题数据操作
	   public IDataReader GetSpaceThemeDataByThemeID(int themeid)
	   {
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `themeid` = " + themeid);
		   return __IDataReader;
	   }
	   #endregion


	   #region Space 评论数据操作
	   public bool AddSpaceComment(SpaceCommentInfo __spacecomments)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					//DbHelper.MakeInParam("?commentid", (DbType)MySqlDbType.Int32, 4,__spacecomments.CommentID),
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,__spacecomments.PostID),
					DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarString, 50,__spacecomments.Author),
					DbHelper.MakeInParam("?email", (DbType)MySqlDbType.VarString, 100,__spacecomments.Email),
					DbHelper.MakeInParam("?url", (DbType)MySqlDbType.VarString, 255,__spacecomments.Url),
					DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarString, 100,__spacecomments.Ip),
					DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 4,__spacecomments.PostDateTime),
					DbHelper.MakeInParam("?content", (DbType)MySqlDbType.VarString, 0,__spacecomments.Content),
					DbHelper.MakeInParam("?parentid", (DbType)MySqlDbType.Int32, 4,__spacecomments.ParentID),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spacecomments.Uid),
					DbHelper.MakeInParam("?posttitle", (DbType)MySqlDbType.VarString, 60,__spacecomments.PostTitle)
				};
		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacecomments` ( `postid`, `author`, `email`, `url`, `ip`, `postdatetime`, `content`, `parentid`, `uid`,`posttitle` ) VALUES ( ?postid, ?author, ?email, ?url, ?ip, ?postdatetime, ?content, ?parentid, ?uid, ?posttitle)");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public bool SaveSpaceComment(SpaceCommentInfo __spacecomments)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,__spacecomments.PostID),
					DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarString, 50,__spacecomments.Author),
					DbHelper.MakeInParam("?email", (DbType)MySqlDbType.VarString, 100,__spacecomments.Email),
					DbHelper.MakeInParam("?url", (DbType)MySqlDbType.VarString, 255,__spacecomments.Url),
					DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarString, 100,__spacecomments.Ip),
					DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 4,__spacecomments.PostDateTime),
					DbHelper.MakeInParam("?content", (DbType)MySqlDbType.VarString, 0,__spacecomments.Content),
					DbHelper.MakeInParam("?parentid", (DbType)MySqlDbType.Int32, 4,__spacecomments.ParentID),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spacecomments.Uid),
					DbHelper.MakeInParam("?posttitle", (DbType)MySqlDbType.VarString, 60,__spacecomments.PostTitle),
					DbHelper.MakeInParam("?commentid", (DbType)MySqlDbType.Int32, 4,__spacecomments.CommentID)

				};
		   string sqlstring = String.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "spacecomments`  Set `postid` = ?postid, `author` = ?author, `email` = ?email, `url` = ?url, `ip` = ?ip, `postdatetime` = ?postdatetime, `content` = ?content, `parentid` = ?parentid, `uid` = ?uid, `posttitle`=?posttile  WHERE `commentid` = ?commentid");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   /// <summary>
	   /// 删除评论
	   /// </summary>
	   /// <param name="commentidList">删除评论的commentid列表</param>
	   /// <returns></returns>
	   public bool DeleteSpaceComments(string commentidList, int userid)
	   {
		   if (!Utils.IsNumericArray(commentidList.Split(',')))
			   return false;

		   DataTable dt = DbHelper.ExecuteDataset(string.Format("select DISTINCT `uid` from `{0}spaceposts` where `postid` in (Select `postid` from `{0}spacecomments` where `commentid` in ({1}))", BaseConfigs.GetTablePrefix, commentidList)).Tables[0];



		   if (dt.Rows.Count > 1)
		   {

			   return false;
		   }
		   else
		   {
			   if (int.Parse(dt.Rows[0]["uid"].ToString()) == userid)
			   {
				   DbHelper.ExecuteNonQuery(string.Format("delete * from `{0}spacecomments` where `commentid` in ({1})", BaseConfigs.GetTablePrefix, commentidList));
				   return true;
			   }
			   return false;
		   }
	   }


	   /// <summary>
	   /// 删除评论
	   /// </summary>
	   /// <param name="commentid">删除评论的commentid</param>
	   /// <returns></returns>
	   public bool DeleteSpaceComment(int commentid)
	   {
		   try
		   {
			   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `commentid` = " + commentid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 返回指定页数与条件的评论列表
	   /// </summary>
	   /// <param name="pageSize">每页的记录数</param>
	   /// <param name="currentPage">当前页号</param>
	   /// <param name="userid">用户ID</param>
	   /// <param name="orderbyASC">排序方式，true为升序，false为降序</param>
	   /// <returns></returns>
	   public DataTable GetSpaceCommentList(int pageSize, int currentPage, int userid, bool orderbyASC)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   string ordertype = orderbyASC ? "ASC" : "DESC";
			   int pageTop = (currentPage - 1) * pageSize;

			   string sql = "";

			   //if (currentPage == 1)
			   //{
			   sql = "SELECT * FROM "
				   + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `uid`=?userid ORDER BY `commentid` " + ordertype+" LIMIT "+pageTop+","+pageSize.ToString();
			   return DbHelper.ExecuteDataset(CommandType.Text, sql,parm).Tables[0];
			   //}
			   //else
			   //{
			   //    if (!orderbyASC)
			   //    {
			   //        sql = "SELECT * FROM "
			   //            + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `commentid` < (SELECT min(`commentid`)  FROM "
			   //            + "(SELECT `commentid` FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE "
			   //            + "`uid`=?userid ORDER BY `commentid` " + ordertype + ") AS tblTmp LIMIT 0,"+pageTop+") AND `uid`=?userid ORDER BY `commentid` " + ordertype+"LIMIT 0,"+pageSize.ToString();
			   //    }
			   //    else
			   //    {
			   //        sql = "SELECT * FROM "
			   //            + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `commentid` > (SELECT MAX(`commentid`)  FROM "
			   //            + "(SELECT `commentid` FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE "
			   //            + "`uid`=?userid ORDER BY `commentid` " + ordertype + " LIMIT 0,"+pageTop+") AS tblTmp ) AND `uid`=?userid ORDER BY `commentid` " + ordertype+"LIMIT 0,"+pageSize.ToString();

			   //    }
			   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   //}
		   }
		   catch
		   {
			   return new DataTable();
		   }
	   }

	   public DataTable GetSpaceCommentListByPostid(int pageSize, int currentPage, int postid, bool orderbyASC)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4, postid);
			   string ordertype = orderbyASC ? "ASC" : "DESC";
			   int pageTop = (currentPage - 1) * pageSize;

			   string sql = "";

			   //if (currentPage == 1)
			   //{
			   sql = "SELECT * FROM "
				   + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `postid`=?postid ORDER BY `commentid` " + ordertype+ " LIMIT 0,"+pageSize.ToString();
			   return DbHelper.ExecuteDataset(CommandType.Text, sql,parm).Tables[0];
			   //}
			   //else
			   //{
			   //    if (!orderbyASC)
			   //    {
			   //        sql = "SELECT * FROM "
			   //            + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `commentid` < (SELECT min(`commentid`)  FROM "
			   //            + "(SELECT `commentid` FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE "
			   //            + "`postid`=?postid ORDER BY `commentid` " + ordertype + "+LIMIT 0,"+pageTop+") AS tblTmp ) AND `postid`=?postid ORDER BY `commentid` " + ordertype+"LIMIT 0,"+pageSize.ToString();
			   //    }
			   //    else
			   //    {
			   //        sql = "SELECT * FROM "
			   //            + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `commentid` > (SELECT MAX(`commentid`)  FROM "
			   //            + "(SELECT `commentid` FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE "
			   //            + "`postid`=?postid ORDER BY `commentid` " + ordertype + "LIMIT 0,"+pageTop+") AS tblTmp ) AND `postid`=?postid ORDER BY `commentid` " + ordertype+"LIMIT 0,"+pageSize.ToString();

			   //    }
			   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   //}
		   }
		   catch
		   {
			   return new DataTable();
		   }
	   }


	   /// <summary>
	   /// 返回满足条件的评论数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <returns></returns>
	   public int GetSpaceCommentsCount(int userid)
	   {
		   //try
		   //{
		   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`commentid`) FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `uid`=?userid", parm).ToString());
		   //}
		   //catch
		   //{
		   //    return 0;
		   //}
	   }

	   public int GetSpaceCommentsCountByPostid(int postid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4, postid);
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`commentid`) FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `postid`=?postid", parm).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   /// <summary>
	   /// 返回全部评论数
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetSpaceNewComments(int topcount, int userid)
	   {
		   try
		   {
			   string sql = "SELECT * FROM "
				   + "`" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `postid` IN (SELECT `postid` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid` = " + userid + " AND `commentcount`>0 ORDER BY `postid` DESC LIMIT 0,10) ORDER BY `commentid` DESC LIMIT 0," + topcount.ToString();
			   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

		   }
		   catch
		   {
			   return new DataTable();
		   }
	   }


	   #endregion


	   #region Space 日志数据操作
	   public bool AddSpacePost(SpacePostInfo __spaceposts)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,__spaceposts.PostID),
					DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarString, 20,__spaceposts.Author),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spaceposts.Uid),
					DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8,__spaceposts.PostDateTime),
					DbHelper.MakeInParam("?content", (DbType)MySqlDbType.VarString, 0,__spaceposts.Content),
					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 150,__spaceposts.Title),
					DbHelper.MakeInParam("?category", (DbType)MySqlDbType.VarString, 255,__spaceposts.Category),
					DbHelper.MakeInParam("?poststatus", (DbType)MySqlDbType.Int32, 1,__spaceposts.PostStatus),
					DbHelper.MakeInParam("?commentstatus", (DbType)MySqlDbType.Int32, 1,__spaceposts.CommentStatus),
					DbHelper.MakeInParam("?postupdatetime", (DbType)MySqlDbType.Datetime, 8,__spaceposts.PostUpDateTime),
					DbHelper.MakeInParam("?commentcount", (DbType)MySqlDbType.Int32, 4,__spaceposts.CommentCount)
				};
		   //string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spaceposts`
		   //    (`author`, `uid`, `postdatetime`, `content`, `title`, `category`, `poststatus`, `commentstatus`, `postupdatetime`, `commentcount`)
		   //VALUES ('" + __spaceposts.Author + "'," + __spaceposts.Uid + ",'" + __spaceposts.PostDateTime + "', '" + __spaceposts.Content + "'
		   //    ,'" + __spaceposts.Title + "', '" + __spaceposts.Category + "'," + __spaceposts.PostStatus + ",
		   //    " + __spaceposts.CommentStatus + ", '" + __spaceposts.PostUpDateTime + "'," + __spaceposts.CommentCount + ");");

		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spaceposts` (`author`, `uid`, `postdatetime`, `content`, `title`, `category`, `poststatus`, `commentstatus`, `postupdatetime`, `commentcount`) VALUES (?author,?uid,?postdatetime, ?content,?title, ?category,?poststatus,?commentstatus,?postupdatetime,?commentcount)");
		   IDataReader __IDataReader;


		   // string  sql = "select  ??Identity";

		   lock (this)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);



			   __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "select `postid` from `" + BaseConfigs.GetTablePrefix + "spaceposts` order by postid desc limit 1");
		   }
		   sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `updatedatetime`=?postupdatetime WHERE `userid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   //向关联表中插入相关数据


		   if (__IDataReader != null)
		   {
			   __IDataReader.Read();

			   foreach (string cateogryid in __spaceposts.Category.Split(','))
			   {
				   if (cateogryid != "")
				   {
					   SpacePostCategoryInfo __spacepostCategoryInfo = new SpacePostCategoryInfo();
					   __spacepostCategoryInfo.PostID = Convert.ToInt32(__IDataReader[0].ToString());
					   __spacepostCategoryInfo.CategoryID = Convert.ToInt32(cateogryid);
					   AddSpacePostCategory(__spacepostCategoryInfo);
				   }
			   }

		   }

		   IDataParameter[] prams1 =
				{
					DbHelper.MakeInParam("?spacepostid", (DbType)MySqlDbType.Int32, 4,__IDataReader[0].ToString()),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spaceposts.Uid)
				};
		   //更新与当前日志关联的附件表中的数据
		   DbHelper.ExecuteReader(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceattachments` SET `spacepostid` = ?spacepostid  WHERE `spacepostid` = 0 AND `uid` = ?uid ", prams1);

		   //对当前用户日志加1
		   CountUserSpacePostCountByUserID(__spaceposts.Uid, 1);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public bool SaveSpacePost(SpacePostInfo __spaceposts)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarString, 20, __spaceposts.Author),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, __spaceposts.Uid),
					DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8, __spaceposts.PostDateTime),
					DbHelper.MakeInParam("?content", (DbType)MySqlDbType.VarString, 0, __spaceposts.Content),
					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 150, __spaceposts.Title),
					DbHelper.MakeInParam("?category", (DbType)MySqlDbType.VarString, 255, __spaceposts.Category),
					DbHelper.MakeInParam("?poststatus", (DbType)MySqlDbType.Int16, 1, __spaceposts.PostStatus),
					DbHelper.MakeInParam("?commentstatus", (DbType)MySqlDbType.Int16, 1, __spaceposts.CommentStatus),
					DbHelper.MakeInParam("?postupdatetime", (DbType)MySqlDbType.Datetime, 8, __spaceposts.PostUpDateTime),
					//DbHelper.MakeInParam("?commentcount", (DbType)MySqlDbType.Int32, 4, __spaceposts.CommentCount),
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4, __spaceposts.PostID)
				};
		   string sqlstring = String.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "spaceposts`  SET `author` = ?author, `uid` = ?uid, `postdatetime` = ?postdatetime, `content` = ?content, `title` = ?title, `category` = ?category, `poststatus` = ?poststatus, `commentstatus` = ?commentstatus, `postupdatetime` = ?postupdatetime WHERE `postid` = ?postid");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		   sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `updatedatetime`=?postupdatetime WHERE `userid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		   //先删除指定的日志关联数据再插入新数据
		   DeleteSpacePostCategoryByPostID(__spaceposts.PostID);

		   foreach (string cateogryid in __spaceposts.Category.Split(','))
		   {
			   if (cateogryid != "")
			   {
				   SpacePostCategoryInfo __spacepostCategoryInfo = new SpacePostCategoryInfo();
				   __spacepostCategoryInfo.PostID = __spaceposts.PostID;
				   __spacepostCategoryInfo.CategoryID = Convert.ToInt32(cateogryid);
				   AddSpacePostCategory(__spacepostCategoryInfo);
			   }
		   }


		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public IDataReader GetSpacePost(int postid)
	   {
		   IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE  `postid`=" + postid);
		   return dr;
	   }

	   /// <summary>
	   /// 删除日志
	   /// </summary>
	   /// <param name="postidList">删除日志的postid列表</param>
	   /// <returns></returns>
	   public bool DeleteSpacePosts(string postidList, int userid)
	   {
		   if (!Utils.IsNumericArray(postidList.Split(',')))
		   {
			   return false;
		   }

		   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `postid` IN (" + postidList + ") AND `uid`=" + userid;
		   int deletedCount = DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   if (deletedCount > 0)
		   {
			   sqlstring = string.Format("UPDATE `{0}spaceconfigs` SET `postcount` = `postcount` - {1} WHERE `userid` = {2}", BaseConfigs.GetTablePrefix, deletedCount, userid);
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
		   }
		   return true;
	   }

	   /// <summary>
	   /// 返回指定页数与条件的日志列表
	   /// </summary>
	   /// <param name="pageSize">每页的记录数</param>
	   /// <param name="currentPage">当前页号</param>
	   /// <param name="userid">用户ID</param>
	   /// <returns></returns>
	   public DataTable SpacePostsList(int pageSize, int currentPage, int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   string sql = "SELECT * FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`=?userid ORDER BY `postid` DESC LIMIT "+pageTop+","+pageSize.ToString();
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		   //}
		   //else
		   //{
		   //    string sql = "SELECT * FROM "
		   //        + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `postid` < (SELECT min(`postid`)  FROM "
		   //        + "(SELECT `postid` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE "
		   //        + "`uid`=?userid ORDER BY `postid` DESC LIMIT 0,"+pageTop+") AS tblTmp ) AND `uid`=?userid ORDER BY `postid` DESC LIMIT 0,"+pageSize.ToString();
		   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		   //}
	   }

	   public DataTable SpacePostsList(int pageSize, int currentPage, int userid, int poststatus)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
				DbHelper.MakeInParam("?poststatus", (DbType)MySqlDbType.Int32, 4, poststatus)
			};
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   string sql = "SELECT * FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`=?userid AND `poststatus`=?poststatus ORDER BY `postid` DESC LIMIT "+pageTop+","+pageSize.ToString();
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //}
		   //else
		   //{
		   //    string sql = "SELECT * FROM "
		   //        + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `postid` < (SELECT min(`postid`)  FROM "
		   //        + "(SELECT `postid` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE "
		   //        + "`uid`=?userid AND `poststatus`=?poststatus ORDER BY `postid` DESC LIMIT 0,"+pageTop+") AS tblTmp ) AND `uid`=?userid AND `poststatus`=?poststatus ORDER BY `postid` DESC LIMIT 0,"+pageSize.ToString();
		   //return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //}
	   }

	   public DataTable SpacePostsList(int pageSize, int currentPage, int userid, DateTime postdatetime)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
				DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8, postdatetime)
			};

		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   string sql = "SELECT * FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`=?userid AND `poststatus`=1 AND "
			   + "DATEDIFF(?postdatetime, postdatetime) = 0 ORDER BY `postid` DESC LIMIT " + pageTop + "," + pageSize.ToString();
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //}
		   //else
		   //{
		   //    string sql = "SELECT * FROM "
		   //        + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `postid` < (SELECT min(`postid`)  FROM "
		   //        + "(SELECT `postid` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE "
		   //        + "`uid`=?userid AND `poststatus`=1 AND DATEDIFF(?postdate, postdatetime) = 0 ORDER BY `postid` DESC LIMIT 0,"+pageTop+") AS tblTmp ) "
		   //        + "AND `uid`=?userid AND `poststatus`=1 AND DATEDIFF(?postdate, postdatetime) = 0 ORDER BY `postid` DESC LIMIT 0,"+pageSize.ToString();
		   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //}
	   }

	   /// <summary>
	   /// 返回满足条件的日志数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <returns></returns>
	   public int GetSpacePostsCount(int userid)
	   {
		   try
		   {
			   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
				};
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`postid`) FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE uid=?userid", prams).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   /// <summary>
	   /// 返回满足条件的日志数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <param name="poststatus"></param>
	   /// <returns></returns>
	   public int GetSpacePostsCount(int userid, int poststatus)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
					DbHelper.MakeInParam("?poststatus", (DbType)MySqlDbType.Int32, 4, poststatus)
				};
            
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`postid`) FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`="+userid+" AND `poststatus`="+poststatus+"", prams).ToString());
		   //}
		   //catch
		   //{
		   //    return 0;
		   //}
	   }

	   /// <summary>
	   /// 返回满足条件的日志数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <param name="poststatus"></param>
	   /// <returns></returns>
	   public int GetSpacePostsCount(int userid, int poststatus, string postdatetime)
	   {
		   try
		   {
			   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
					DbHelper.MakeInParam("?poststatus", (DbType)MySqlDbType.Int32, 4, poststatus),
					DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Datetime,8,postdatetime)
				};
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`postid`) FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`=?userid AND `poststatus`=?poststatus AND DATEDIFF(?postdatetime, postdatetime) = 0", prams).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   /// <summary>
	   /// 为当前用户的SPACE日志查看数加1
	   /// </summary>
	   /// <param name="postid"></param>
	   ///
	   /// <returns></returns>
	   public bool CountUserSpacePostByUserID(int postid)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,postid)
				};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceposts` SET `views` = `views` + 1 WHERE `postid` = ?postid", prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }


	   /// <summary>
	   /// 更新当前日志数的评论数
	   /// </summary>
	   /// <param name="postid"></param>
	   /// <param name="errormsg"></param>
	   /// <returns></returns>
	   public bool CountSpaceCommentCountByPostID(int postid, int commentcount)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?commentcount", (DbType)MySqlDbType.Int32, 4,commentcount),
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,postid)
				};

		   if (commentcount >= 0)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceposts` SET `commentcount` = `commentcount` + ?commentcount  WHERE `postid` = ?postid ", prams);
		   }
		   else
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceposts` SET `commentcount` = `commentcount` + ?commentcount  WHERE `postid` = ?postid AND `commentcount`>0", prams);
		   }
		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }


	   /// <summary>
	   /// 更新当前日志数的浏览量
	   /// </summary>
	   /// <param name="postid"></param>
	   /// <param name="errormsg"></param>
	   /// <returns></returns>
	   public bool CountUserSpaceViewsByUserID(int postid, int views)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?views", (DbType)MySqlDbType.Int32, 4,views),
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,postid)
				};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spaceposts` SET `views` = `views` + ?views  WHERE `postid` = ?postid", prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }
	   #endregion


	   #region 日志类型 操作类

	   public IDataReader GetSpaceCategoryByCategoryID(int categoryid)
	   {
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `categoryid` = " + categoryid);
		   return __IDataReader;
	   }


	   public bool AddSpaceCategory(SpaceCategoryInfo __spacecategories)
	   {
		   //try
		   ////{
		   //    IDataParameter[] prams =
		   //    {
		   //        DbHelper.MakeInParam("?categoryid", (DbType)MySqlDbType.Int32, 4,__spacecategories.CategoryID),
		   //        DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 50,__spacecategories.Title),
		   //        DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spacecategories.Uid),
		   //        DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 1000,__spacecategories.Description),
		   //        DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int32, 4,__spacecategories.TypeID),
		   //        DbHelper.MakeInParam("?categorycount", (DbType)MySqlDbType.Int32, 4,__spacecategories.CategoryCount),
		   //        DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4,__spacecategories.Displayorder)
		   //    };
		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacecategories` ( `title`, `uid`, `description`, `typeid`, `categorycount`, `displayorder`) VALUES ('" + __spacecategories.Title + "', " + __spacecategories.Uid + ",'" + __spacecategories.Description + "'," + __spacecategories.TypeID + "," + __spacecategories.CategoryCount + ", " + __spacecategories.Displayorder + ")");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}

	   }

	   public bool SaveSpaceCategory(SpaceCategoryInfo __spacecategories)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 50,__spacecategories.Title),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spacecategories.Uid),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 1000,__spacecategories.Description),
					DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int32, 4,__spacecategories.TypeID),
					DbHelper.MakeInParam("?categorycount", (DbType)MySqlDbType.Int32, 4,__spacecategories.CategoryCount),
					DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4,__spacecategories.Displayorder),
					DbHelper.MakeInParam("?categoryid", (DbType)MySqlDbType.Int32, 4,__spacecategories.CategoryID)
				};
		   string sqlstring = String.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "spacecategories` SET  `title` = ?title, `uid` = ?uid, `description` = ?description, `typeid` = ?typeid, `categorycount` = ?categorycount, `displayorder` = ?displayorder WHERE `categoryid` = ?categoryid ");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}

	   }

	   /// <summary>
	   ///	获取分类列表
	   /// </summary>
	   /// <param name="idList">分类的ID，以","分隔</param>
	   /// <returns>返回分类名称列表</returns>
	   public string GetCategoryNameByIdList(string idList)
	   {
		   if (idList.ToString() != "")
		   {
			   string sql = "SELECT `title` FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `categoryid` IN (" + idList + ")";
			   IDataReader categoryReader = DbHelper.ExecuteReader(CommandType.Text, sql);
			   string categoryNameList = "";
			   while (categoryReader.Read())
			   {
				   categoryNameList += categoryReader["title"].ToString() + ",";
			   }
			   if (categoryNameList == "")
			   {
				   return "";
			   }
			   else
			   {
				   return categoryNameList.Substring(0, categoryNameList.Length - 1);
			   }
		   }
		   else
		   {
			   return "&nbsp;";
		   }
	   }


	   /// <summary>
	   ///	获取分类列表
	   /// </summary>
	   /// <param name="userid">用户的id</param>
	   /// <returns>返回分类名称列表</returns>
	   public IDataReader GetCategoryNameByUserID(int userid)
	   {
		   if (userid > 0)
		   {
			   string sql = "SELECT `categoryid`, `title`  FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `uid` = " + userid;
			   IDataReader categoryReader = DbHelper.ExecuteReader(CommandType.Text, sql);
			   return categoryReader;
		   }
		   else
		   {
			   return null;
		   }
	   }

	   /// <summary>
	   /// 根据用户id获取分类列表
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <returns></returns>
	   public DataTable GetSpaceCategoryListByUserId(int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
		   string sql = "SELECT `categoryid`, `title` FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `uid`=?userid ORDER BY `displayorder`, `categoryid`";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
	   }

	   /// <summary>
	   ///	获取分类列表
	   /// </summary>
	   /// <param name="idList">分类的ID, 以","分隔</param>
	   /// <returns>返回分类名称列表</returns>
	   public IDataReader GetCategoryIDAndName(string idList)
	   {
		   if (idList.Trim() == "")
		   {
			   return null;
		   }

		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT `categoryid`,`title` FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `categoryid` IN (" + idList + ")");
		   return __IDataReader;
	   }

	   /// <summary>
	   /// 删除分类
	   /// </summary>
	   /// <param name="categoryidList">删除分类的categoryid列表</param>
	   /// <returns></returns>
	   public bool DeleteSpaceCategory(string categoryidList, int userid)
	   {
		   try
		   {
			   //清除分类的categoryid列表相关信息
			   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `categoryid` IN (" + categoryidList + ") AND `uid`=" + userid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			   //清除分类的categoryid关联表
			   sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacepostcategories` WHERE `categoryid` IN (" + categoryidList + ")";
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 返回指定页数与条件的分类列表
	   /// </summary>
	   /// <param name="pageSize">每页的记录数</param>
	   /// <param name="currentPage">当前页号</param>
	   /// <param name="userid">用户ID</param>
	   /// <returns></returns>
	   public DataTable GetSpaceCategoryList(int pageSize, int currentPage, int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   int pageTop = (currentPage - 1) * pageSize;
			   //if (currentPage == 1)
			   //{
			   string sql = "SELECT * FROM "
				   + "`" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE uid=?userid ORDER BY `categoryid` DESC LIMIT "+pageTop+","+pageSize.ToString();
			   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   //}
			   //else
			   //{
			   //    string sql = "SELECT * FROM "
			   //        + "`" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `categoryid` < (SELECT min(`categoryid`)  FROM "
			   //        + "(SELECT `categoryid` FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE "
			   //        + "`uid`=?userid ORDER BY `categoryid` DESC) AS tblTmp LIMIT 0,"+pageTop+") AND `uid`=?userid ORDER BY `categoryid` DESC LIMIT 0,"+pageSize.ToString();
			   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   //}
		   }
		   catch
		   {
			   return new DataTable();
		   }
	   }

	   /// <summary>
	   /// 返回满足条件的分类数
	   /// </summary>
	   /// <param name="userid">用户ID</param>
	   /// <returns></returns>
	   public int GetSpaceCategoryCount(int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`categoryid`) FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `uid`=?userid", parm).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }


	   #endregion


	   #region 日志关联类型 操作类
	   public bool AddSpacePostCategory(SpacePostCategoryInfo __spacepostcategories)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,__spacepostcategories.PostID),
					DbHelper.MakeInParam("?categoryid", (DbType)MySqlDbType.Int32, 4,__spacepostcategories.CategoryID),
					DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4,__spacepostcategories.ID)
				};
		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacepostcategories` (`postid`, `categoryid`) VALUES ( ?postid, ?categoryid)");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}

	   }

	   public bool SaveSpacePostCategory(SpacePostCategoryInfo __spacepostcategories)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,__spacepostcategories.PostID),
					DbHelper.MakeInParam("?categoryid", (DbType)MySqlDbType.Int32, 4,__spacepostcategories.CategoryID),
					DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4,__spacepostcategories.ID)
				};
		   string sqlstring = String.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "spacepostcategories` SET `postid` = ?postid, `categoryid` = ?categoryid WHERE  `id` = ?id");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}

	   }

	   public bool DeleteSpacePostCategoryByPostID(int postid)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4,postid)
				};
		   string sqlstring = String.Format("DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacepostcategories` WHERE `postid` = ?postid");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch
		   //{
		   //    return false;
		   //}

	   }

	   #endregion


	   #region 日志附件 操作类
	   public bool AddSpaceAttachment(SpaceAttachmentInfo __spaceattachments)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32, 4,__spaceattachments.AID),
					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spaceattachments.UID),
					DbHelper.MakeInParam("?spacepostid", (DbType)MySqlDbType.Int32, 4,__spaceattachments.SpacePostID),
					DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8,__spaceattachments.PostDateTime),
					DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.VarString, 100,__spaceattachments.FileName),
					DbHelper.MakeInParam("?filetype", (DbType)MySqlDbType.VarString, 50,__spaceattachments.FileType),
					DbHelper.MakeInParam("?filesize", (DbType)SqlDbType.Float, 8,__spaceattachments.FileSize),
					DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.VarString, 100,__spaceattachments.Attachment),
					DbHelper.MakeInParam("?downloads", (DbType)MySqlDbType.Int32, 4,__spaceattachments.Downloads),

		   };
		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spaceattachments` ( `uid`, `spacepostid`, `postdatetime`, `filename`, `filetype`, `filesize`, `attachment`, `downloads`)" +
			   "VALUES ( " + __spaceattachments.UID + ", " + __spaceattachments.SpacePostID + ", '" + __spaceattachments.PostDateTime + "','" + __spaceattachments.FileName + "', '" + __spaceattachments.FileType + "', '" + __spaceattachments.FileSize + "','" + __spaceattachments.Attachment + "', " + __spaceattachments.Downloads + ")");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public bool SaveSpaceAttachment(SpaceAttachmentInfo __spaceattachments)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4,__spaceattachments.UID),
					DbHelper.MakeInParam("?spacepostid", (DbType)MySqlDbType.Int32, 4,__spaceattachments.SpacePostID),
					DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 8,__spaceattachments.PostDateTime),
					DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.VarString, 100,__spaceattachments.FileName),
					DbHelper.MakeInParam("?filetype", (DbType)MySqlDbType.VarString, 50,__spaceattachments.FileType),
					DbHelper.MakeInParam("?filesize", (DbType)SqlDbType.Float, 8,__spaceattachments.FileSize),
					DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.VarString, 100,__spaceattachments.Attachment),
					DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.Int32, 4,__spaceattachments.Downloads),
					DbHelper.MakeInParam("?aid", (DbType)MySqlDbType.Int32, 4,__spaceattachments.AID)
				};
		   string sqlstring = String.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "spaceattachments`  SET `uid` = ?uid, `spacepostid` = ?spacepostid, `postdatetime` = ?postdatetime, `filename` = ?filename, `filetype` = ?filetype, `filesize` = ?filesize, `attachment` = ?attachment, `downloads` = ?downloads  WHERE `aid` = ?aid");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }


	   /// <summary>
	   /// 返回指定页数与条件的分类列表
	   /// </summary>
	   /// <param name="pageSize">每页的记录数</param>
	   /// <param name="currentPage">当前页号</param>
	   /// <param name="userid">用户ID</param>
	   /// <returns></returns>
	   public DataTable GetSpaceAttachmentList(int pageSize, int currentPage, int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   int pageTop = (currentPage - 1) * pageSize;
			   if (currentPage == 1)
			   {
				   string sql = "SELECT * FROM "
					   + "`" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE `uid`=?userid ORDER BY `aid` DESC LIMIT 0,"+pageSize.ToString();
				   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   }
			   else
			   {
				   string sql = "SELECT * FROM "
					   + "`" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE `aid` < (SELECT min(`aid`)  FROM "
					   + "(SELECT `aid` FROM `" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE "
					   + "`uid`=?userid ORDER BY `aid` DESC LIMIT 0,"+pageTop.ToString()+") AS tblTmp ) AND `uid`=?userid ORDER BY `aid` DESC LIMIT 0,"+pageSize.ToString();
				   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   }
		   }
		   catch
		   {
			   return new DataTable();
		   }
	   }

	   /// <summary>
	   /// 返回满足条件的分类数
	   /// </summary>
	   /// <param name="userid">用户ID</param>
	   /// <returns></returns>
	   public int GetSpaceAttachmentCount(int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`aid`) FROM `" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE `uid`=?userid", parm).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }


	   /// <summary>
	   /// 删除指定的附件记录和相关文件
	   /// </summary>
	   /// <param name="aidlist">附件ID串, 格式:1,3,5</param>
	   /// <returns></returns>
	   public bool DeleteSpaceAttachmentByIDList(string aidlist, int userid)
	   {
		   //			IDataParameter[] prams =
		   //			{
		   //						DbHelper.MakeInParam("?aidlist", (DbType)MySqlDbType.VarString, 1000, aidlist)
		   //			};


		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT `filename` FROM `" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE `aid` IN( " + aidlist + " ) AND `uid`=" + userid, null);

		   if (__IDataReader != null)
		   {
			   string path = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/");
			   while (__IDataReader.Read())
			   {
				   try
				   {
					   System.IO.File.Delete(path + __IDataReader[0].ToString());
				   }
				   catch
				   { ;}
			   }
		   }

		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM  `" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE `aid` IN( " + aidlist + " )", null);

		   return true;
	   }

	   #endregion


	   #region 友情链接 操作类

	   /// <summary>
	   /// 返回满足条件的友情链接数
	   /// </summary>
	   /// <param name="userid"></param>
	   /// <returns></returns>
	   public int GetSpaceLinkCount(int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`linkid`) FROM `" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE `userid`=?userid", parm).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   /// <summary>
	   /// 返回指定页数与条件的友情链接列表
	   /// </summary>
	   /// <param name="pageSize">每页的记录数</param>
	   /// <param name="currentPage">当前页号</param>
	   /// <param name="userid">用户ID</param>
	   /// <returns></returns>
	   public DataTable GetSpaceLinkList(int pageSize, int currentPage, int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   int pageTop = (currentPage - 1) * pageSize;
			   if (currentPage == 1)
			   {
				   string sql = "SELECT * FROM "
					   + "`" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE `userid`=?userid ORDER BY `linkid` DESC LIMIT 0,"+pageSize.ToString();
				   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   }
			   else
			   {
				   string sql = "SELECT * FROM "
					   + "`" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE `linkid` < (SELECT min(`linkid`)  FROM "
					   + "(SELECT `linkid` FROM `" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE "
					   + "`userid`=?userid ORDER BY `linkid` DESC LIMIT 0,"+pageTop+") AS tblTmp ) AND `userid`=?userid ORDER BY `linkid` DESC LIMIT 0,"+pageSize.ToString();
				   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
			   }
		   }
		   catch
		   {
			   return new DataTable();
		   }
	   }

	   public IDataReader GetSpaceLinkByLinkID(int linkid)
	   {
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE `linkid` = " + linkid);
		   return __IDataReader;
	   }

	   public bool SaveSpaceLink(SpaceLinkInfo __spacelinks)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?linktitle", (DbType)MySqlDbType.VarString, 50,__spacelinks.LinkTitle),
					DbHelper.MakeInParam("?linkurl", (DbType)MySqlDbType.VarString,255,__spacelinks.LinkUrl),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200,__spacelinks.Description),
					DbHelper.MakeInParam("?linkid", (DbType)MySqlDbType.Int32, 4,__spacelinks.LinkId)
				};
		   string sqlstring = String.Format("UPDATE `" + BaseConfigs.GetTablePrefix + "spacelinks` SET  `linktitle` = ?linktitle, `linkurl` = ?linkurl, `description` = ?description WHERE `linkid` = ?linkid ");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}

	   }

	   public bool AddSpaceLink(SpaceLinkInfo __spacelinks)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,__spacelinks.UserId),
					DbHelper.MakeInParam("?linktitle", (DbType)MySqlDbType.VarString, 50,__spacelinks.LinkTitle),
					DbHelper.MakeInParam("?linkurl", (DbType)MySqlDbType.VarString,255,__spacelinks.LinkUrl),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200,__spacelinks.Description),
					DbHelper.MakeInParam("?linkid", (DbType)MySqlDbType.Int32, 4,__spacelinks.LinkId)
				};
		   string sqlstring = String.Format("INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacelinks` ( `userid`, `linktitle`, `linkurl`, `description`) VALUES (" + __spacelinks.UserId + ", '" + __spacelinks.LinkTitle + "','" + __spacelinks.LinkUrl + "', '" + __spacelinks.Description + "')");

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}

	   }

	   /// <summary>
	   /// 删除友情链接
	   /// </summary>
	   /// <param name="linksList">删除友情链接的linkid列表</param>
	   /// <returns></returns>
	   public bool DeleteSpaceLink(string linksList, int userid)
	   {
		   try
		   {
			   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE `linkid` IN (" + linksList + ") AND userid=" + userid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }
	   #endregion

	   #region	相册 操作类

	   public int GetSpaceAlbumsCount(int userid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`albumid`) FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `userid`=?userid", parm).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   public bool CountAlbumByAlbumID(int albumid)
	   {
		   //try
		   //{
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "albums` SET `views` = `views` + 1 WHERE `albumid` = " + albumid);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public DataTable SpaceAlbumsList(int pageSize, int currentPage, int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   string sql = "SELECT * FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "albums` WHERE `userid`=?userid ORDER BY `albumid` DESC LIMIT "+pageTop+","+pageSize.ToString()+"";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		   //}
		   //else
		   //{
		   //    string sql = "SELECT * FROM "
		   //        + "`" + BaseConfigs.GetTablePrefix + "albums` WHERE `albumid` < (SELECT min(`albumid`)  FROM "
		   //        + "(SELECT  `albumid` FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE "
		   //        + "`userid`=?userid ORDER BY `albumid` DESC LIMT "+pageSize.ToString()+") AS tblTmp ) AND `userid`=?userid ORDER BY `albumid` DESC LIMIT "+pageTop.ToString()+","+pageSize.ToString()+"";
		   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		   //}
	   }

	   public IDataReader SpaceAlbumsList(int userid, int albumcategoryid, int pageSize, int currentPage)
	   {

		   //           string sql = null;
		   //           string resql = null;

		   //           sql = " WHERE `type`=0 AND `imgcount`>0 ";

		   //           if (albumcategoryid != 0)
		   //           {
		   //               sql = sql + " AND `albumcateid`=" + albumcategoryid;
		   //           }

		   //           if (userid > 0)
		   //           {

		   //               sql = sql + " AND `userid`=" + userid;
		   //           }

           
		   //string temp = "SELECT `albumid` FROM `{0}albums` " + sql + " ORDER BY `albumid` DESC LIMIT " + (currentPage - 1) * pageSize + "," + pageSize + "";

		   //               IDataReader DDR = DbHelper.ExecuteReader(CommandType.Text, string.Format(temp, BaseConfigs.GetTablePrefix));
		   //               string albmidlist = null;
		   //               if (DDR != null)
		   //               {
		   //                   while (DDR.Read())
		   //                   {

		   //                       albmidlist = albmidlist + DDR["albumid"].ToString() + ",";

		   //                   }
		   //                   if (albmidlist != null)
		   //                   {
		   //                       albmidlist = albmidlist.Substring(0, albmidlist.Length - 1);

		   //                   }

		   //               }
		   //               else
		   //               {

		   //                   albmidlist = "";
		   //                   return null;
		   //               }

               

		   //               resql = "SELECT * FROM `{0}albums`  WHERE `albumid` IN ("+albmidlist+") ORDER BY `albumid` DESC";

		   string sql = null;
		   string resql = null;

		   sql = " `type`=0 AND `imgcount`>0 ";

		   if (albumcategoryid != 0)
		   {
			   sql = sql + " AND `albumcateid`=" + albumcategoryid;
		   }

		   if (userid > 0)
		   {

			   sql = sql + " AND `userid`=" + userid;
		   }


		   resql = "SELECT * FROM `{0}albums`  WHERE " + sql + " ORDER BY `albumid` DESC LIMIT " + (currentPage - 1) * pageSize + "," + pageSize + "";

		   return DbHelper.ExecuteReader(CommandType.Text, string.Format(resql.ToString(), BaseConfigs.GetTablePrefix));


	   }



	   public int SpaceAlbumsListCount(int userid, int albumcategoryid)
	   {
		   string sql = string.Format("SELECT COUNT(1) FROM `{0}albums` WHERE `type`=0 AND `imgcount`>0 ", BaseConfigs.GetTablePrefix);
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32,  4, userid),
									  
									 DbHelper.MakeInParam("?albumcateid", (DbType)MySqlDbType.Int32, 4, albumcategoryid)
								 };
		   if (userid > 0)
		   {
			   sql += " AND `userid`=?userid";
		   }



		   if (albumcategoryid != 0)
		   {
			   sql += " AND `albumcateid`=?albumcateid";
		   }


		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);
	   }

	   public IDataReader GetSpaceAlbumById(int albumId)
	   {
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `albumid`=" + albumId);
		   return __IDataReader;
	   }

	   public DataTable GetSpaceAlbumByUserId(int userid)
	   {
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `userid`=" + userid).Tables[0];
		   return dt;
	   }

	   public bool AddSpaceAlbum(AlbumInfo spaceAlbum)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,spaceAlbum.Userid),
					DbHelper.MakeInParam("?albumcateid", (DbType)MySqlDbType.Int32, 4,spaceAlbum.Albumcateid),
					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 50,spaceAlbum.Title),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200,spaceAlbum.Description),
					DbHelper.MakeInParam("?password", (DbType)MySqlDbType.VarString, 50,spaceAlbum.Password),
					DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 8,spaceAlbum.Type),
					//DbHelper.MakeInParam("?creatdatetime", (DbType)MySqlDbType.Datetime, 8,spaceAlbum.Createdatetime)
					DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarString, 20, spaceAlbum.Username)
				};
		   string sqlstring = String.Format("INSERT INTO `{0}albums` (`userid`, `albumcateid`, `title`, `description`, `password`, `type`,`createdatetime`,`username`) VALUES ( ?userid, ?albumcateid, ?title, ?description, ?password, ?type,NOW(),?username)", BaseConfigs.GetTablePrefix);

		   //向关联表中插入相关数据
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public bool SaveSpaceAlbum(AlbumInfo spaceAlbum)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?albumcateid", (DbType)MySqlDbType.Int32, 4, spaceAlbum.Albumcateid),
					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 50,spaceAlbum.Title),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200,spaceAlbum.Description),
					DbHelper.MakeInParam("?password", (DbType)MySqlDbType.VarString, 50,spaceAlbum.Password),
					DbHelper.MakeInParam("?imgcount", (DbType)MySqlDbType.Int32, 4,spaceAlbum.Imgcount),
					DbHelper.MakeInParam("?logo", (DbType)MySqlDbType.VarString, 255, spaceAlbum.Logo),
					DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 8,spaceAlbum.Type),
					DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, spaceAlbum.Albumid)
				};
		   string sqlstring = String.Format("UPDATE `{0}albums` SET `albumcateid` = ?albumcateid, `title` = ?title, `description` = ?description, `password` = ?password, `imgcount` = ?imgcount, `logo` = ?logo, `type` = ?type WHERE `albumid` = ?albumid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   public void UpdateAlbumViews(int albumid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
		   string sql = string.Format("UPDATE `{0}albums` SET `views`=`views`+1 WHERE `albumid`=?albumid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public bool DeleteSpaceAlbum(int albumId, int userid)
	   {
		   //try
		   //{
		   //删除照片及文件
		   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `albumid`=" + albumId + " AND `userid`=" + userid;
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }




#if NET1

	   public AlbumCategoryInfoCollection GetAlbumCategory()
	   {
		   string sql = string.Format("SELECT * FROM `{0}albumcategories` ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);

		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);

		   AlbumCategoryInfoCollection acic = new AlbumCategoryInfoCollection();
		   if (reader != null)
		   {
			   while (reader.Read())
			   {
				   AlbumCategoryInfo aci = new AlbumCategoryInfo();
				   aci.Albumcateid = Utils.StrToInt(reader["albumcateid"], 0);
				   aci.Albumcount = Utils.StrToInt(reader["albumcount"], 0);
				   aci.Description = reader["description"].ToString();
				   aci.Displayorder = Utils.StrToInt(reader["displayorder"], 0);
				   aci.Title = reader["title"].ToString();
				   acic.Add(aci);
			   }
			   reader.Close();
		   }
		   return acic;
	   }

#else

        public Discuz.Common.Generic.List<AlbumCategoryInfo> GetAlbumCategory()
        {
            string sql = string.Format("SELECT * FROM `{0}albumcategories` ORDER BY `displayorder`", BaseConfigs.GetTablePrefix);

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);

            Discuz.Common.Generic.List<AlbumCategoryInfo> acic = new Discuz.Common.Generic.List<AlbumCategoryInfo>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    AlbumCategoryInfo aci = new AlbumCategoryInfo();
                    aci.Albumcateid = Utils.StrToInt(reader["albumcateid"], 0);
                    aci.Albumcount = Utils.StrToInt(reader["albumcount"], 0);
                    aci.Description = reader["description"].ToString();
                    aci.Displayorder = Utils.StrToInt(reader["displayorder"], 0);
                    aci.Title = reader["title"].ToString();
                    acic.Add(aci);
                }
                reader.Close();
            }
            return acic;
        }

#endif

	   #endregion

	   #region 照片 操作类
	   public bool AddSpacePhoto(PhotoInfo photoinfo)
	   {
		   //try
		   //{
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,photoinfo.Userid),
					DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, photoinfo.Username),
					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 20,photoinfo.Title),
					DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4,photoinfo.Albumid),
					DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.VarChar, 255,photoinfo.Filename),
					DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.VarChar, 255,photoinfo.Attachment),
					DbHelper.MakeInParam("?filesize", (DbType)MySqlDbType.Int32, 4,photoinfo.Filesize),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarChar, 200,photoinfo.Description)
					//DbHelper.MakeInParam("?creatdatetime", (DbType)MySqlDbType.Datetime, 8,spaceAlbum.Createdatetime)
				};
		   string sqlstring = String.Format("INSERT INTO `{0}photos` (`userid`, `username`,`title`, `albumid`, `filename`, `attachment`, `filesize`, `description`,`postdate`) VALUES (?userid,?username, ?title, ?albumid, ?filename, ?attachment, ?filesize, ?description,NOW())", BaseConfigs.GetTablePrefix);

		   //向关联表中插入相关数据
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

		   return true;
		   //}
		   //catch (Exception ex)
		   //{
		   //    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
		   //    return false;
		   //}
	   }

	   /// <summary>
	   /// 更新图片信息(仅更新 标题、描述、评论设置和标签设置4项)
	   /// </summary>
	   /// <param name="photo"></param>
	   public void UpdatePhotoInfo(PhotoInfo photo)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarString, 20, photo.Title),
									 DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarString, 200, photo.Description),
									 DbHelper.MakeInParam("?commentstatus", (DbType)MySqlDbType.Int16, 1, (byte)photo.Commentstatus),
									 DbHelper.MakeInParam("?tagstatus", (DbType)MySqlDbType.Int16, 1, (byte)photo.Tagstatus),
									 DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4, photo.Photoid)
								 };

		   string sql = string.Format("UPDATE `{0}photos` SET `title`=?title, `description`=?description, `commentstatus`=?commentstatus, `tagstatus`=?tagstatus WHERE `photoid`=?photoid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   /// <summary>
	   /// 通过相册ID得到相册中所有图片的信息
	   /// </summary>
	   /// <param name="albumid">相册ID</param>
	   /// <param name="errormsg"></param>
	   /// <returns></returns>
	   public DataTable GetSpacePhotoByAlbumID(int albumid)
	   {
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4,albumid)
				};
		   string sqlstring = String.Format("SELECT * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `albumid` = ?albumid");

		   //向关联表中插入相关数据
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring, prams).Tables[0];

		   return dt;
	   }

	   /// <summary>
	   /// 获得照片信息
	   /// </summary>
	   /// <param name="photoid">图片Id</param>
	   /// <param name="albumid">相册Id</param>
	   /// <param name="mode">模式,0=当前图片,1上一张,2下一张</param>
	   /// <returns></returns>
	   public IDataReader GetPhotoByID(int photoid, int albumid, byte mode)
	   {
		   IDataParameter[] prams =
				{

					DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid),
					DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4,photoid)
				};
		   string sqlstring;

		   switch (mode)
		   {
			   case 1:
				   sqlstring = String.Format("SELECT * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `albumid` = " + albumid + " AND `photoid`<" + photoid + " ORDER BY `photoid` DESC LIMIT 0,1");
				   break;
			   case 2:
				   sqlstring = String.Format("SELECT * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `albumid` = " + albumid + " AND `photoid`>" + photoid + " ORDER BY `photoid` ASC LIMIT 0,1");
				   break;
			   default:
				   sqlstring = String.Format("SELECT * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `photoid` = " + photoid + "");
				   break;
		   }
		   //向关联表中插入相关数据
		   IDataReader idr = DbHelper.ExecuteReader(CommandType.Text, sqlstring, prams);

		   return idr;
	   }

	   public void UpdatePhotoViews(int photoid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4, photoid);
		   string sql = string.Format("UPDATE `{0}photos` SET `views`=`views`+1 WHERE `photoid`=?photoid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public int GetSpacePhotosCount(int albumid)
	   {
		   try
		   {
			   IDataParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`photoid`) FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `albumid`=?albumid", parm).ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   public DataTable SpacePhotosList(int pageSize, int currentPage, int userid, int albumid)
	   {
		   //try
		   //{
		   //"userid=" + userid + " AND albumid=" + albumid
		   IDataParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,userid),
					DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4,albumid)
				};
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   string sql = "SELECT * FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "photos` WHERE `userid`=?userid AND `albumid`=?albumid ORDER BY `photoid` ASC LIMIT "+pageTop+","+pageSize.ToString();
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //}
		   //else
		   //{
		   //    string sql = "SELECT * FROM "
		   //        + "`" + BaseConfigs.GetTablePrefix + "photos` WHERE `photoid` > (SELECT MAX(`photoid`)  FROM "
		   //        + "(SELECT `photoid` FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE "
		   //        + "`userid`=?userid AND `albumid`=?albumid ORDER BY `photoid` ASC LIMIT 0,"+pageTop+") AS tblTmp ) AND `userid`=?userid "
		   //        + "AND `albumid`=?albumid ORDER BY `photoid` ASC LIMIT "+pageTop.ToString()+","+pageTop.ToString()+"";
		   //    return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		   //}
		   //}
		   //catch
		   //{
		   //    return new DataTable();
		   //}
	   }

	   public bool DeleteSpacePhotoByIDList(string photoidlist, int albumid, int userid)
	   {
		   if (photoidlist == "")
			   return false;
		   IDataReader __IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT `filename` FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `photoid` IN( " + photoidlist + " ) AND `userid`=" + userid, null);

		   if (__IDataReader != null)
		   {
			   while (__IDataReader.Read())
			   {
				   try
				   {
					   string file = Utils.GetMapPath(__IDataReader[0].ToString());
					   System.IO.File.Delete(file);
					   string thumbnailimg = file.Replace(Path.GetExtension(file), "_thumbnail" + Path.GetExtension(file));
					   if (File.Exists(thumbnailimg))
						   File.Delete(thumbnailimg);
				   }
				   catch
				   { ;}
			   }
			   __IDataReader.Close();
		   }

		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM  `" + BaseConfigs.GetTablePrefix + "photos` WHERE `photoid` IN( " + photoidlist + " ) AND `userid`=" + userid.ToString(), null);

		   return true;
	   }

	   public int ChangeAlbum(int targetAlbumId, string photoIdList, int userid)
	   {
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "photos` SET albumid=" + targetAlbumId + " WHERE photoid IN (" + photoIdList + ") AND `userid`=" + userid.ToString();
		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public int GetPhotoSizeByUserid(int userid)
	   {
		   // string sql = "SELECT ISNULL(SUM(filesize)) AS `filesize` FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE userid=" + userid;
		   // string sql = "SELECT IsNull(SUM(filesize)) AS `filesize` FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE userid=" + userid;
		   string sql = "SELECT SUM(`filesize`) FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE userid=" + userid.ToString();

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString(),0);
	   }

	   public int GetSpacePhotoCountByAlbumId(int albumid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
		   string sql = string.Format("SELECT COUNT(1) FROM `{0}photos` WHERE `albumid`=?albumid", BaseConfigs.GetTablePrefix);
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0);
	   }

	   #endregion

	   public string GetThemeDropDownTreeSql()
	   {
		   return "SELECT `themeid`, `name`, `type` AS `parentid` FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` ORDER BY `themeid`";
	   }

	   public string GetTemplateDropDownSql()
	   {
		   return "SELECT `templateid`, `name`  FROM `" + BaseConfigs.GetTablePrefix + "spacetemplates` ORDER BY `templateid`";
	   }

	   public string GetCategoryCheckListSql(int userid)
	   {
		   return "SELECT `categoryid`, `title` FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `uid`=" + userid + " ORDER BY `displayorder`, `categoryid`";
	   }

	   #region 对ThemeInfo的操作
	   public IDataReader GetThemeInfos()
	   {
		   string sql = string.Format(@"SELECT * FROM `{0}spacethemes` ORDER BY `type`", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, null);
	   }

	   public IDataReader GetThemeInfoById(int themeId)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeId)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacethemes` WHERE `themeid` = ?themeid", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }

	   #endregion

	   #region 对TemplateInfo的操作

	   public IDataReader GetTemplateInfos()
	   {
		   string sql = string.Format(@"SELECT * FROM `{0}spacetemplates`", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, null);
	   }
	   public IDataReader GetTemplateInfoById(int templateInfoId)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?templateid", (DbType)MySqlDbType.Int32, 4, templateInfoId)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacetemplates` WHERE `templateid` = ?templateid", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }

	   #endregion


	   #region 对spacemoduledefs表的操作
	   public IDataReader GetModuleDefInfoById(int moduleDefInfoId)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduledefid", (DbType)MySqlDbType.Int32, 4, moduleDefInfoId)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacemoduledefs` WHERE `moduledefid` = ?moduledefid", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }


	   /// <summary>
	   /// 添加ModuleDef信息至数据库
	   /// </summary>
	   /// <param name="moduleDefInfo"></param>
	   /// <returns></returns>
	   public bool AddModuleDef(ModuleDefInfo moduleDefInfo)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?modulename", (DbType)MySqlDbType.VarString, 20, moduleDefInfo.ModuleName),
				DbHelper.MakeInParam("?cachetime", (DbType)MySqlDbType.Int32, 4, moduleDefInfo.CacheTime),
				DbHelper.MakeInParam("?configfile", (DbType)MySqlDbType.VarString, 100, moduleDefInfo.ConfigFile),
				DbHelper.MakeInParam("?controller", (DbType)MySqlDbType.VarString, 255, moduleDefInfo.BussinessController),
		   };

		   string sql = string.Format(@"INSERT INTO `{0}spacemoduledefs`(`modulename`, `cachetime`, `configfile`, `controller`) VALUES(?moduledefid, ?modulename, ?cachetime, ?configfile, ?controller)", BaseConfigs.GetTablePrefix);
		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 修改指定的ModuleDef信息
	   /// </summary>
	   /// <param name="moduleDefInfo"></param>
	   /// <returns></returns>
	   public bool UpdateModuleDef(ModuleDefInfo moduleDefInfo)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?modulename", (DbType)MySqlDbType.VarString, 20, moduleDefInfo.ModuleName),
				DbHelper.MakeInParam("?cachetime", (DbType)MySqlDbType.Int32, 4, moduleDefInfo.CacheTime),
				DbHelper.MakeInParam("?configfile", (DbType)MySqlDbType.VarString, 100, moduleDefInfo.ConfigFile),
				DbHelper.MakeInParam("?controller", (DbType)MySqlDbType.VarString, 255, moduleDefInfo.BussinessController),
				DbHelper.MakeInParam("?moduledefid", (DbType)MySqlDbType.Int32, 4, moduleDefInfo.ModuleDefID)
			};

		   string sql = string.Format(@"UPDATE `{0}spacemoduledefs` SET `modulename`=?modulename, `cachetime`=?cachetime, `configfile`=?configfile, `controller`=?controller WHERE `moduledefid`=?moduledefid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 删除指定的ModuleDef信息
	   /// </summary>
	   /// <param name="moduleDefId"></param>
	   /// <returns></returns>
	   public bool DeleteModuleDef(int moduleDefId)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduledefid", (DbType)MySqlDbType.Int32, 4, moduleDefId),
		   };

		   string sql = string.Format(@"DELETE FROM `{0}_spacemoduledefs` WHERE `moduledefid`=?moduledefid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }


	   public int GetModuleDefIdByUrl(string url)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?configfile", (DbType)MySqlDbType.VarString, 100, url),
		   };

		   string commandText = string.Format(@"SELECT `moduledefid` FROM `{0}spacemoduledefs` WHERE `configfile`=?configfile", BaseConfigs.GetTablePrefix);

		   string str = DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, parms);
		   return str == string.Empty ? 0 : Convert.ToInt32(str);
	   }

	   #endregion

	   #region 对spacemodules表的操作

	   public int GetModulesCountByTabId(int tabId, int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"SELECT COUNT(1) FROM `{0}spacemodules` WHERE `tabid` = ?tabid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   int reval = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);

		   return reval;
	   }

	   /// <summary>
	   /// 根据TabId获得Modules集合
	   /// </summary>
	   /// <param name="tabId"></param>
	   /// <returns></returns>
	   public IDataReader GetModulesByTabId(int tabId, int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacemodules` WHERE `tabid` = ?tabid AND `uid`=?uid ORDER BY `panename`, `displayorder`", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }


	   /// <summary>
	   /// 根据ModuleId获得Module
	   /// </summary>
	   /// <param name="moduleInfoId"></param>
	   /// <returns></returns>
	   public IDataReader GetModuleInfoById(int moduleInfoId, int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleInfoId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacemodules` WHERE `moduleid` = ?moduleid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }


	   /// <summary>
	   /// 添加Module至数据库
	   /// </summary>
	   /// <param name="moduleInfo"></param>
	   /// <returns></returns>
	   public bool AddModule(ModuleInfo moduleInfo)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleInfo.ModuleID),
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, moduleInfo.TabID),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, moduleInfo.Uid),
				DbHelper.MakeInParam("?moduledefid", (DbType)MySqlDbType.Int32, 4, moduleInfo.ModuleDefID),
				DbHelper.MakeInParam("?panename", (DbType)MySqlDbType.VarString, 10, moduleInfo.PaneName),
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, moduleInfo.DisplayOrder),
				DbHelper.MakeInParam("?userpref", (DbType)MySqlDbType.VarString, 4000, moduleInfo.UserPref),
				DbHelper.MakeInParam("?val", (DbType)MySqlDbType.Int16, 1, moduleInfo.Val),
				DbHelper.MakeInParam("?moduleurl", (DbType)MySqlDbType.VarString, 255, moduleInfo.ModuleUrl),
				DbHelper.MakeInParam("?moduletype", (DbType)MySqlDbType.Int16, 2, moduleInfo.ModuleType)
			};

		   string sql = string.Format(@"INSERT INTO `{0}spacemodules`(`moduleid`, `tabid`, `uid`, `moduledefid`, `panename`, `displayorder`, `userpref`, `val`, `moduleurl`, `moduletype`) VALUES(?moduleid, ?tabid, ?uid, ?moduledefid, ?panename, ?displayorder, ?userpref, ?val, ?moduleurl, ?moduletype)", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 更新指定的Module信息
	   /// </summary>
	   /// <param name="moduleInfo"></param>
	   /// <returns></returns>
	   public bool UpdateModule(ModuleInfo moduleInfo)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, moduleInfo.TabID),
				DbHelper.MakeInParam("?moduledefid", (DbType)MySqlDbType.Int32, 4, moduleInfo.ModuleDefID),
				DbHelper.MakeInParam("?panename", (DbType)MySqlDbType.VarString, 10, moduleInfo.PaneName),
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, moduleInfo.DisplayOrder),
				DbHelper.MakeInParam("?userpref", (DbType)MySqlDbType.VarString, 4000, moduleInfo.UserPref),
				DbHelper.MakeInParam("?val", (DbType)MySqlDbType.Int16, 1, moduleInfo.Val),
				DbHelper.MakeInParam("?moduleurl", (DbType)MySqlDbType.VarString, 255, moduleInfo.ModuleUrl),
				DbHelper.MakeInParam("?moduletype", (DbType)MySqlDbType.Int16, 2, moduleInfo.ModuleType),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, moduleInfo.Uid),
				DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleInfo.ModuleID)
			};

		   string sql = string.Format(@"UPDATE `{0}spacemodules` SET `tabid`=?tabid, `moduledefid`=?moduledefid, `panename`=?panename, `displayorder`=?displayorder,`userpref`=?userpref,`val`=?val, moduleurl=?moduleurl, moduletype=?moduletype WHERE `moduleid`=?moduleid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 删除指定的Module信息
	   /// </summary>
	   /// <param name="moduleId"></param>
	   /// <returns></returns>
	   public bool DeleteModule(int moduleId, int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"DELETE FROM `{0}spacemodules` WHERE `moduleid`=?moduleid AND `uid`=?uid", BaseConfigs.GetTablePrefix);
		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 为模块排序
	   /// </summary>
	   /// <param name="mid"></param>
	   /// <param name="panename"></param>
	   /// <param name="displayorder"></param>
	   public void UpdateModuleOrder(int mid, int uid, string panename, int displayorder)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, mid),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
				DbHelper.MakeInParam("?panename", (DbType)MySqlDbType.VarString, 10, panename),
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, displayorder)
			};
		   string commandText = string.Format(@"UPDATE `{0}spacemodules` SET `panename`=?panename, `displayorder`=?displayorder WHERE `moduleid`=?moduleid AND `uid`=?uid", BaseConfigs.GetTablePrefix);
		   RunExecuteSql(commandText, parms);
	   }

	   public void UpdateModuleTab(int moduleid, int uid, int tabid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabid)
			};
		   string commandText = string.Format(@"UPDATE `{0}spacemodules` SET `displayorder`=0, `tabid`=?tabid WHERE `moduleid`=?moduleid AND `uid`=?uid", BaseConfigs.GetTablePrefix);
		   RunExecuteSql(commandText, parms);
	   }

	   public int GetMaxModuleIdByUid(int userid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid)
			};
		   string commandText = string.Format(@"SELECT `moduleid` FROM `{0}spacemodules` WHERE `uid`=?uid ORDER BY `moduleid` DESC LIMIT 0,1", BaseConfigs.GetTablePrefix);
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
	   }

	   #endregion

	   #region 对spacetabs表的操作

	   /// <summary>
	   /// 根据Uid获得Tab集合
	   /// </summary>
	   /// <param name="uid"></param>
	   /// <returns></returns>
	   public IDataReader GetTabInfosByUid(int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacetabs` WHERE `uid`=?uid ORDER BY `tabid` ASC", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }

	   /// <summary>
	   /// 根据TabId获得Tab
	   /// </summary>
	   /// <param name="tabInfoId"></param>
	   /// <returns></returns>
	   public IDataReader GetTabInfoById(int tabInfoId, int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabInfoId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"SELECT * FROM `{0}spacetabs` WHERE `tabid` = ?tabid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunSelectSql(sql, parms);
	   }

	   /// <summary>
	   /// 添加Tab信息至数据库
	   /// </summary>
	   /// <param name="tabInfo"></param>
	   /// <returns></returns>
	   public bool AddTab(TabInfo tabInfo)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabInfo.TabID),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, tabInfo.UserID),
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, tabInfo.DisplayOrder),
				DbHelper.MakeInParam("?tabname", (DbType)MySqlDbType.VarString, 50, tabInfo.TabName),
				DbHelper.MakeInParam("?iconfile", (DbType)MySqlDbType.VarString, 50, tabInfo.IconFile),
				DbHelper.MakeInParam("?template", (DbType)MySqlDbType.VarString, 50, tabInfo.Template)
			};

		   string sql = string.Format(@"INSERT INTO `{0}spacetabs`(`tabid`, `uid`, `displayorder`, `tabname`, `iconfile`, `template`) VALUES(?tabid, ?uid, ?displayorder, ?tabname, ?iconfile, ?template)", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 更新指定Tab信息
	   /// </summary>
	   /// <param name="tabInfo"></param>
	   /// <returns></returns>
	   public bool UpdateTab(TabInfo tabInfo)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, tabInfo.DisplayOrder),
				DbHelper.MakeInParam("?tabname", (DbType)MySqlDbType.VarString, 50, tabInfo.TabName),
				DbHelper.MakeInParam("?iconfile", (DbType)MySqlDbType.VarString, 50, tabInfo.IconFile),
				DbHelper.MakeInParam("?template", (DbType)MySqlDbType.VarString, 50, tabInfo.Template),
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabInfo.TabID),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, tabInfo.UserID),
		   };

		   string sql = string.Format(@"UPDATE `{0}spacetabs` SET `displayorder`=?displayorder, `tabname`=?tabname, `iconfile`=?iconfile, `template` = ?template WHERE `tabid`=?tabid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   /// <summary>
	   /// 删除Tab信息
	   /// </summary>
	   /// <param name="tabId"></param>
	   /// <returns></returns>
	   public bool DeleteTab(int tabId, int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
		   };

		   string sql = string.Format(@"DELETE FROM `{0}spacetabs` WHERE `tabid`=?tabid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   public int GetTabInfoCountByUserId(int userid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid),
		   };
		   string commandText = string.Format(@"SELECT COUNT(1) FROM `{0}spacetabs` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0);
	   }

	   public bool SetTabTemplate(int tabid, int uid, string template)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabid),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
				DbHelper.MakeInParam("?template", (DbType)MySqlDbType.VarString, 50, template)
			};

		   string sql = string.Format(@"UPDATE `{0}spacetabs` SET `template` = ?template WHERE `tabid`=?tabid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }

	   public int GetMaxTabIdByUid(int userid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid)
			};
		   string commandText = string.Format(@"SELECT `tabid` FROM `{0}spacetabs` WHERE `uid`=?uid ORDER BY `tabid` DESC LIMIT 0,1", BaseConfigs.GetTablePrefix);
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
	   }

	   #endregion

	   #region config

	   public void ClearDefaultTab(int userid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
			};
		   string commandText = string.Format("UPDATE `{0}spaceconfigs` SET `defaulttab`=0 WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

	   }
	   public void SetDefaultTab(int userid, int tabid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabid)
			};
		   string commandText = string.Format("UPDATE `{0}spaceconfigs` SET `defaulttab`=?tabid WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
	   }

	   public void SetSpaceTheme(int userid, int themeid, string themepath)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeid),
				DbHelper.MakeInParam("?themepath", (DbType)MySqlDbType.VarString, 50, themepath),
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
			};
		   string commandText = string.Format("UPDATE `{0}spaceconfigs` SET `themeid`=?themeid, `themepath`=?themepath WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
	   }

	   #endregion

	   /// <summary>
	   /// 运行非Select语句
	   /// </summary>
	   /// <param name="sql"></param>
	   /// <param name="parms"></param>
	   /// <returns></returns>
	   private bool RunExecuteSql(string sql, IDataParameter[] parms)
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 运行Select语句
	   /// </summary>
	   /// <param name="sql"></param>
	   /// <param name="parms"></param>
	   /// <returns></returns>
	   private IDataReader RunSelectSql(string sql, IDataParameter[] parms)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
	   }

	   public DataRow GetThemes()
	   {
		   //string sql = "SELECT TOP 1 newid() AS row,`themeid`,`directory` FROM "+BaseConfigs.GetTablePrefix+"spacethemes WHERE type<>0 ORDER BY row";
		   string sql = "SELECT `themeid` AS row,`themeid`,`directory` FROM " + BaseConfigs.GetTablePrefix + "spacethemes WHERE type<>0 ORDER BY 1 LIMIT 0,1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0].Rows[0];
	   }

	   public DataTable GetUnActiveSpaceList()
	   {
		   string sql = "SELECT `uid`,s.`spaceid`,`spacetitle`,`username`,`createdatetime` FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` as s ";
		   sql += "LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` as u ON s.`userid`=u.`uid` ";
		   sql += "WHERE s.`spaceid` IN (SELECT ABS(`spaceid`) AS spaceid  FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `spaceid` < 0) ORDER BY s.`spaceid` DESC";
		   return DbHelper.ExecuteDataset(sql).Tables[0];
	   }

	   public void DeleteSpaces(string uidlist)
	   {
		   DbHelper.ExecuteNonQuery("DELETE  FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` WHERE `userid` IN (" + uidlist + ")");
	   }

	   public void DeleteSpaceThemes(string themeidlist)
	   {
		   DbHelper.ExecuteNonQuery("DELETE  FROM `" + BaseConfigs.GetTablePrefix + "spacethemes`  WHERE `themeid` IN(" + themeidlist + ")");
	   }

	   public void UpdateSpaceThemeInfo(int themeid, string name, string author, string copyright)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name),
									 DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarString, 100, author),
									 DbHelper.MakeInParam("?copyright", (DbType)MySqlDbType.VarString, 100, copyright),
									 DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "spacethemes` SET `name`=?name, `author`=?author, `copyright`=?copyright WHERE themeid=?themeid";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public DataTable GetSpaceThemeDirectory()
	   {
		   return DbHelper.ExecuteDataset("SELECT directory FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `type`<>0").Tables[0];
	   }

	   public bool IsThemeExist(string name)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name);
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE name=?name", parm), 0) > 0;
	   }

	   public bool IsThemeExist(string name, int themeid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name),
									 DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, themeid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `name`=?name AND themeid<>?id", parms), 0) > 0;
	   }

	   //public void AddSpaceTheme(string name)
	   //{
	   //    IDataParameter parm = DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name);
	   //    DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacethemes`(`name`, `type`) VALUES(?name,0)", parm);
	   //}

	   public void AddSpaceTheme(string directory, string name, int type, string author, string createdate, string copyright)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?directory", (DbType)MySqlDbType.VarString, 100, directory),
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 50, type),
									 DbHelper.MakeInParam("?author", (DbType)MySqlDbType.VarString, 100, author),
									 DbHelper.MakeInParam("?createdate", (DbType)MySqlDbType.VarString, 50, createdate),
									 DbHelper.MakeInParam("?copyright", (DbType)MySqlDbType.VarString, 100, copyright)
								 };
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacethemes`(`directory`, `name`, `type`, `author`, `createdate`, `copyright`) VALUES(?directory,?name,?type,?author,?createdate,?copyright)";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public void UpdateThemeName(int themeid, string name)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name),
									 DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spacethemes` SET name=?name WHERE themeid=?themeid", parms);
	   }

	   public void DeleteTheme(int themeid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeid);
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `themeid`=?themeid OR `type`=?themeid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   #region PhotoComment

	   public IDataReader GetPhotoCommentCollection(int photoid)
	   {
		   string commandText = "SELECT * FROM`" + BaseConfigs.GetTablePrefix + "photocomments` WHERE `photoid`=" + photoid + "";
		   return DbHelper.ExecuteReader(CommandType.Text, commandText);
	   }

	   public void CreatePhotoComment(PhotoCommentInfo pcomment)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, pcomment.Userid),
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarString, 20, pcomment.Username),
									 DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4, pcomment.Photoid),
									 DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime, 4, pcomment.Postdatetime),
									 DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarString, 100, pcomment.Ip),
									 DbHelper.MakeInParam("?content", (DbType)MySqlDbType.VarString, 2000, pcomment.Content)
								 };
		   string commandText = string.Format("INSERT INTO `{0}photocomments`(`userid`, `username`, `photoid`, `postdatetime`, `ip`, `content`) VALUES(" + pcomment.Userid + ",'" + pcomment.Username + "'," + pcomment.Photoid + ", '" + pcomment.Postdatetime + "','" + pcomment.Ip + "','" + pcomment.Content + "')", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
	   }

	   public void DeletePhotoComment(int commentid)
	   {
		   string commandText = string.Format("DELETE FROM `{0}photocomments` WHERE `commentid`={1}", BaseConfigs.GetTablePrefix, commentid);
		   DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
	   }

	   #endregion


	   public IDataReader GetFocusPhotoList(int type, int focusphotocount, int vaildDays)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?vaildDays", (DbType)MySqlDbType.Int32, 4, vaildDays);
		   string sql = string.Format("SELECT `p`.* FROM `{0}photos` AS `p`,`{0}albums` AS `a` WHERE DATEDIFF(NOW(),`p`.`postdate`) <" + vaildDays + " AND `a`.`albumid` = `p`.`albumid` AND `a`.`type`=0",
			   BaseConfigs.GetTablePrefix);
		   switch (type)
		   {
			   case 0:
				   sql += " ORDER BY `p`.`views` DESC  LIMIT 0,"+focusphotocount;
				   break;
			   case 1:
				   sql += " ORDER BY `p`.`comments` DESC  LIMIT 0," + focusphotocount;
				   break;
			   case 2:
				   sql += " ODRER BY `p`.`postdate` DESC  LIMIT 0," + focusphotocount;
				   break;
			   default:
				   sql += " ORDER BY `p`.`views` DESC  LIMIT 0," + focusphotocount;
				   break;
		   }
		   return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
	   }


	   public void UpdatePhotoComments(int photoid, int count)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4, photoid),
									 DbHelper.MakeInParam("?count", (DbType)MySqlDbType.Int32, 4, count),
		   };
		   string commandText = string.Format("UPDATE `{0}photos` SET `comments`=`comments`+?count WHERE `photoid`=?photoid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
	   }

	   public DataTable GetPhotosByAlbumid(int albumid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?albumid", (DbType)SqlDbType.Int, 4, albumid);
		   string commandText = string.Format("SELECT `photoid`,`userid`,`username`,`title`, `filename` FROM `{0}photos` WHERE `albumid`=?albumid", BaseConfigs.GetTablePrefix);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
	   }

	   public IDataReader GetPhotoRankList(int type, int photocount)
	   {
		   string sql = string.Format("SELECT `p`.* FROM `{0}photos` as `p`,`{0}albums` as `a` WHERE `a`.`albumid` = `p`.`albumid` AND `a`.`type`=0",
			   BaseConfigs.GetTablePrefix);

      

		   switch (type)
		   {
			   case 0:
				   sql += " ORDER BY `p`.`views` DESC LIMIT 0," + photocount; 
				   break;
			   case 1:
				   sql += " ORDER BY `p`.`comments` DESC LIMIT 0," + photocount; 
				   break;
			   case 2:
				   sql += " ORDER BY `p`.`postdate` DESC LIMIT 0," + photocount; 
				   break;
			   case 3:
				   //                  string  sqltemp = string.Format(@"SELECT * FROM `{0}albums` WHERE albumid IN (SELECT `tid` 
				   //		                                                                FROM `{0}favorites`
				   //		                                                                WHERE  `typeid`=1 AND `tid` in (SELECT `albumid` 
				   //                                                                                                        FROM `{0}albums` 
				   //                                                                                                        WHERE `type]=0) 
				   //		                                                                GROUP BY `tid` 
				   //		                                                                ORDER BY COUNT(`tid`) DESC limit 0,"+photocount+")", BaseConfigs.GetTablePrefix);
				   string sqltemp = "SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `typeid`=1 AND `tid` in (SELECT `albumid` FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `type`=0) GROUP BY `tid` ORDER BY COUNT(`tid`) DESC limit 0," + photocount;
				   IDataReader DDR=DbHelper.ExecuteReader(CommandType.Text, sqltemp);
				   //List<string> albmidlist = new List<string>();
				   string albmidlist=null;
				   if (DDR!=null)
				   {
					   while (DDR.Read())
					   {

						   albmidlist = albmidlist + DDR["tid"].ToString() + ",";

					   }
					   DDR.Close();
					   if (albmidlist != null)
					   { 
						   albmidlist=albmidlist.Substring(0, albmidlist.Length - 1);
                        
					   }
                        
				   }
				   else

				   {

					   albmidlist = "";
				   }
                  
				   sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE albumid IN ('" + albmidlist + "')";

                    
				   break;
			   default:
				   sql += " ORDER BY `p`.`views` DESC LIMIT 0," + photocount;
				   break;
		   }

		   return DbHelper.ExecuteReader(CommandType.Text, sql);
	   }

	   public IDataReader GetRecommendPhotoList(string idlist)
	   {
		   if (!Utils.IsNumericArray(idlist.Split(',')))
		   {
			   return null;
		   }

		   string sql = string.Format("SELECT `p`.* FROM `{0}photos` `p`,`{0}albums` `a` WHERE `p`.`albumid` = `a`.`albumid`  AND `a`.`type`=0 AND `p`.`photoid` IN ({1}) ORDER BY INSTR('{1}',`p`.`photoid`)", BaseConfigs.GetTablePrefix, idlist);

		   return DbHelper.ExecuteReader(CommandType.Text, sql);
	   }

	   public IDataReader GetRecommendAlbumList(string idlist)
	   {
		   if (!Utils.IsNumericArray(idlist.Split(',')))
		   {
			   return null;
		   }

		   string sql = string.Format("SELECT * FROM `{0}albums` WHERE `albumid` IN ({1})  AND `type`=0", BaseConfigs.GetTablePrefix, idlist);

		   return DbHelper.ExecuteReader(CommandType.Text, sql);
	   }



	   public DataTable GetSpaceList(int pagesize, int currentpage, string username, string dateStart, string dateEnd)
	   {
		   int pagetop = (currentpage - 1) * pagesize;
		   string condition = GetSpaceListCondition(username, dateStart, dateEnd);
		   //if (currentpage == 1)
		   //{
		   string sqlstring = "SELECT s.`spaceid`,`userid`,`spacetitle`,`username`,`grouptitle`,`postcount`,`commentcount`,`createdatetime`,`status` ";
		   sqlstring += "FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` as s ";
		   sqlstring += "LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` as u ON s.userid=u.uid  ";
		   sqlstring += "LEFT JOIN `" + BaseConfigs.GetTablePrefix + "usergroups` as g ON u.`groupid`=g.`groupid` ";
		   if (condition != "")
			   sqlstring += "WHERE " + condition + " ";
		   sqlstring += "ORDER BY s.spaceid DESC LIMIT "+pagetop+","+pagesize.ToString();
		   return DbHelper.ExecuteDataset(sqlstring).Tables[0];
		   //}
		   //else
		   //{
		   //    string sqlstring = "SELECT s.`spaceid`,`userid`,`spacetitle`,`username`,`grouptitle`,`postcount`,`commentcount`,`createdatetime`,`status` ";
		   //    sqlstring += "FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` as s ";
		   //    sqlstring += "LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` as u ON s.`userid`=u.`uid` ";
		   //    sqlstring += "LEFT JOIN `" + BaseConfigs.GetTablePrefix + "usergroups` as g ON u.`groupid`=g.`groupid` ";
		   //    sqlstring += "WHERE s.`spaceid`<(SELECT MIN(`spaceid`) FROM (SELECT `spaceid` FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` ";
		   //    sqlstring += "ORDER BY `spaceid` DESC LIMIT 0,"+pagetop+") as tblTmp) ";
		   //    if (condition != "")
		   //        sqlstring += "AND " + condition + " ";
		   //    sqlstring += "ORDER BY s.`spaceid` DESC LIMIT 0,"+pagesize.ToString();

		   //    return DbHelper.ExecuteDataset(sqlstring).Tables[0];
		   //}
	   }

	   public void AdminOpenSpaceStatusBySpaceidlist(string spaceidlist)
	   {
		   DbHelper.ExecuteNonQuery("UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `status`=`status`&~" + (int)SpaceStatusType.AdminClose + "  WHERE `spaceid` IN (" + spaceidlist + ")");
	   }

	   public void AdminCloseSpaceStatusBySpaceidlist(string spaceidlist)
	   {
		   DbHelper.ExecuteNonQuery("UPDATE `" + BaseConfigs.GetTablePrefix + "spaceconfigs` SET `status`=`status`|" + (int)SpaceStatusType.AdminClose + "  WHERE `spaceid` IN (" + spaceidlist + ")");
	   }

	   public int GetSpaceRecordCount(string username, string dateStart, string dateEnd)
	   {
		   string condition = GetSpaceListCondition(username, dateStart, dateEnd);
		   string sqlstring = "SELECT  Count(s.`spaceid`) From `" + BaseConfigs.GetTablePrefix + "spaceconfigs` as s LEFT JOIN `"+BaseConfigs.GetTablePrefix+"users` as u ON s.`userid`=u.`uid` ";
		   if (condition != "")
			   sqlstring += " WHERE " + condition;
		   return Convert.ToInt32(DbHelper.ExecuteDataset(sqlstring).Tables[0].Rows[0][0].ToString());
	   }

	   private string GetSpaceListCondition(string username, string dateStart, string dateEnd)
	   {
		   string condition = " 1=1 ";
		   if (username != "")
			   condition += " And u.`username` like'%" + RegEsc(username) + "%'";

		   if (dateStart != "")
			   condition += " And s.`createdatetime` >='" + dateStart + "'";
		   if (dateEnd != "")
			   condition += " And s.`createdatetime` <='" + dateEnd + "'";
		   return condition;
	   }


	   /// <summary>
	   /// 获取图片集合
	   /// </summary>
	   /// <param name="userid">用户Id,必须指定一个用户,不能为0</param>
	   /// <param name="albumid">相册Id，当为0时表示此用户所有相册</param>
	   /// <param name="count">取出的数量</param>
	   /// <returns></returns>
	   public IDataReader GetPhotoListByUserId(int userid, int albumid, int count)
	   {
		   string sql = string.Format("SELECT `p`.* FROM `{0}photos` `p`,`{0}albums` `a` WHERE `a`.`albumid` = `p`.`albumid` AND `a`.`type`=0 AND `p`.`userid`=?userid", BaseConfigs.GetTablePrefix);

		   if (albumid > 0)
		   {
			   sql += " AND `p`.`albumid`=?albumid";
		   }

		   sql += " ORDER BY `p`.`postdate` DESC LIMIT 0,"+count;

		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
									 DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid)
								 };

		   return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
	   }


	   public void UpdateUserSpaceRewriteName(int userid, string rewritename)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.String, 100, rewritename),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
								 };

		   string sql = string.Format("UPDATE `{0}spaceconfigs` SET `rewritename`=?rewritename WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public bool IsRewritenameExist(string rewriteName)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.String, 100, rewriteName);
		   string sql = string.Format("SELECT COUNT(1) FROM `{0}spaceconfigs` WHERE `rewritename`=?rewritename", BaseConfigs.GetTablePrefix);
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0) > 0;
	   }

	   public string GetUidBySpaceid(string spaceid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?spaceid", (DbType)MySqlDbType.Int32, 4, spaceid);
		   string sql = string.Format("SELECT `userid` FROM `{0}spaceconfigs` WHERE `spaceid`=?spaceid", BaseConfigs.GetTablePrefix);
		   return DbHelper.ExecuteScalar(CommandType.Text, sql, parm).ToString();
	   }


	   public string GetSpaceattachmentsAidListByUid(int uid)
	   {
		   string aidlist = "";
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);
		   string sql = string.Format("SELECT `aid` FROM `{0}spaceattachments` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		   if (dt.Rows.Count == 0)
			   return "";
		   else
		   {
			   foreach (DataRow dr in dt.Rows)
			   {
				   aidlist += dr["aid"].ToString() + ",";
			   }
			   if (aidlist != "")
				   aidlist = aidlist.Substring(0, aidlist.Length - 1);
		   }
		   return aidlist;
	   }


	   public bool DeleteSpaceCategory(int userid)
	   {
		   try
		   {
			   //清除分类的categoryid列表相关信息
			   string sqlstring = "SELECT `categoryid` FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `uid`=" + userid;
			   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			   string categoryidList = "";
			   foreach (DataRow dr in dt.Rows)
			   {
				   categoryidList += dr["categoryid"].ToString();
			   }
			   if (categoryidList != "")
			   {
				   categoryidList = categoryidList.Substring(0, categoryidList.Length - 1);
				   //清除分类的categoryid关联表
				   sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacepostcategories` WHERE `categoryid` IN (" + categoryidList + ")";
				   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			   }

			   sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacecategories` WHERE `uid`=" + userid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }



	   public bool DeleteSpaceComments(int userid)
	   {
		   try
		   {
			   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` WHERE `uid`=" + userid;
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }


	   public bool DeleteSpaceLink(int userid)
	   {
		   try
		   {
			   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacelinks` WHERE userid=" + userid.ToString();
			   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }


	   public bool DeleteModule(int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};

		   string sql = string.Format(@"DELETE FROM `{0}spacemodules` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);
		   return RunExecuteSql(sql, parms);
	   }



	   public bool DeleteSpacePosts(int userid)
	   {
		   string sqlstring = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`=" + userid;
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
		   sqlstring = string.Format("UPDATE `{0}spaceconfigs` SET `postcount` = 0 WHERE `userid` = {1}", BaseConfigs.GetTablePrefix, userid);
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

		   return true;
	   }


	   public bool DeleteTab(int uid)
	   {
		   IDataParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
		   };

		   string sql = string.Format(@"DELETE FROM `{0}spacetabs` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return RunExecuteSql(sql, parms);
	   }


	   public void DeleteSpaceByUid(int uid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);
		   string sql = string.Format("DELETE FROM `{0}spaceconfigs` WHERE `userid`=?uid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public DataTable SpacePhotosList(int albumid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
		   string sql = sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `albumid`=?albumid ORDER BY `photoid` ASC";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
	   }


	   public void UpdateCustomizePanelContent(int moduleid, int userid, string modulecontent)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),                
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
									 DbHelper.MakeInParam("?modulecontent", (DbType)MySqlDbType.VarChar, 0, modulecontent)
								 };

		   string sql = string.Format("UPDATE `{0}spacecustomizepanels` SET `panelcontent`=?modulecontent WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public bool ExistCustomizePanelContent(int moduleid, int userid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
								 };

		   string sql = string.Format("SELECT COUNT(1) FROM `{0}spacecustomizepanels` WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0) > 0;
	   }

	   public void AddCustomizePanelContent(int moduleid, int userid, string modulecontent)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
									 DbHelper.MakeInParam("?modulecontent", (DbType)MySqlDbType.VarChar, 0, modulecontent)
								 };

		   string sql = string.Format("INSERT INTO `{0}spacecustomizepanels`(`moduleid`, `userid`, `panelcontent`) VALUES(?moduleid, ?userid, ?modulecontent)", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public object GetCustomizePanelContent(int moduleid, int userid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
								 };

		   string sql = string.Format("SELECT `panelcontent` FROM `{0}spacecustomizepanels` WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

		   return DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
	   }

	   public void DeleteCustomizePanelContent(int moduleid, int userid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
								 };

		   string sql = string.Format("DELETE FROM `{0}spacecustomizepanels` WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public IDataReader GetModulesByUserId(int uid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);

		   string sql = string.Format("SELECT * FROM `{0}spacemodules` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);

		   return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
	   }


	   public int GetSpaceCustomizePanelCount(int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);

		   string sql =
			   string.Format(
			   "SELECT COUNT(1) FROM `{0}spacemodules` WHERE `uid`=?uid AND `moduleurl`='builtin_customizepanel.xml'",
			   BaseConfigs.GetTablePrefix);

		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0);
	   }

	   public IDataReader GetSpaceCustomizePanelList(int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);

		   string sql =
			   string.Format(
			   "SELECT * FROM `{0}spacemodules` WHERE `uid`=?uid AND `moduleurl`='builtin_customizepanel.xml'",
			   BaseConfigs.GetTablePrefix);

		   return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
	   }

	   public IDataReader GetModuleDefList()
	   {
		   string sql = string.Format("SELECT * FROM `{0}spacemoduledefs`", BaseConfigs.GetTablePrefix);

		   return DbHelper.ExecuteReader(CommandType.Text, sql);
	   }


	   public string GetSapceThemeList(int themeid)
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `type`=" + themeid;
	   }
	   public string DeleteSpaceThemeByThemeid(int themeid)
	   {
		   return "DELETE FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `themeid`=" + themeid;
	   }

	   public void AddModuleDefInfo(ModuleDefInfo mdi)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?modulename", (DbType)MySqlDbType.VarChar, 20, mdi.ModuleName),
									 DbHelper.MakeInParam("?cachetime", (DbType)MySqlDbType.Int32, 4, mdi.CacheTime),
									 DbHelper.MakeInParam("?configfile", (DbType)MySqlDbType.VarChar, 100, mdi.ConfigFile),
									 DbHelper.MakeInParam("?controller", (DbType)MySqlDbType.VarChar, 255, mdi.BussinessController)
								 };

		   string sql = string.Format("INSERT INTO `{0}spacemoduledefs`(`modulename`, `cachetime`, `configfile`, `controller`) VALUES(?modulename, ?cachetime, ?configfile, ?controller)", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

	   }

	   public void DeleteModuleDefByUrl(string url)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?configfile", (DbType)MySqlDbType.VarChar, 100, url)
								 };

		   string sql = string.Format("DELETE FROM `{0}spacemoduledefs` WHERE `configfile` = ?configfile", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }

	   public DataTable GetSearchSpacePostsList(int pagesize, string postids)
	   {
		   string commandText = string.Format("SELECT `{0}spaceposts`.`postid`, `{0}spaceposts`.`title`, `{0}spaceposts`.`author`, `{0}spaceposts`.`uid`, `{0}spaceposts`.`postdatetime`, `{0}spaceposts`.`commentcount`, `{0}spaceposts`.`views` FROM `{0}spaceposts` WHERE `{0}spaceposts`.`postid` IN({2}) ORDER BY INSTR('{2}',`{0}albums`.`albumid`) LIMIT {1}", BaseConfigs.GetTablePrefix, pagesize, postids);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   public DataTable GetSearchAlbumList(int pagesize, string albumids)
	   {
		   string commandText = string.Format("SELECT `{0}albums`.`albumid`, `{0}albums`.`title`, `{0}albums`.`username`, `{0}albums`.`userid`, `{0}albums`.`createdatetime`, `{0}albums`.`imgcount`, `{0}albums`.`views`, `{0}albumcategories`.`albumcateid`,`{0}albumcategories`.`title` AS `categorytitle` FROM `{0}albums` LEFT JOIN `{0}albumcategories` ON `{0}albumcategories`.`albumcateid` = `{0}albums`.`albumcateid` WHERE `{0}albums`.`albumid` IN({2}) ORDER BY INSTR('{2}',`{0}albums`.`albumid`) LIMIT {1}", BaseConfigs.GetTablePrefix, pagesize, albumids);
		   return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
	   }

	   public int GetSpaceAttachmentSizeByUserid(int userid)
	   {
		   string sql = "SELECT IFNULL(SUM(filesize),0) AS `filesize` FROM `" + BaseConfigs.GetTablePrefix + "spaceattachments` WHERE uid=" + userid;
		   //object o = DbHelper.ExecuteScalar(CommandType.Text,sql);
		   return (int)DbHelper.ExecuteScalar(CommandType.Text, sql);
	   }

	   public IDataReader GetFocusPhotoList(int type, int focusphotocount, int validDays, int uid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?validDays", (DbType)MySqlDbType.Int32, 4, validDays),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };
		   string sql = string.Format("SELECT `p`.* FROM `{1}photos` `p`,`{0}albums` `a` WHERE DATEDIFF(NOW(), `postdate`) < ?validDays AND `a`.`albumid` = `p`.`albumid` AND `a`.`type`=0{1}",
			   BaseConfigs.GetTablePrefix, uid > 1 ? " AND `p`.`userid` =?uid" : string.Empty);
		   switch (type)
		   {
			   case 0:
				   sql += " ORDER BY `p`.`views` DESC limit 0," + focusphotocount;
				   break;
			   case 1:
				   sql += " ORDER BY `p`.`comments` DESC limit 0," + focusphotocount;
				   break;
			   case 2:
				   sql += " ORDER BY `p`.`postdate` DESC limit 0," + focusphotocount;
				   break;
			   default:
				   sql += " ORDER BY `p`.`views` DESC limit 0," + focusphotocount;
				   break;
		   }
		   return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
	   }

	   public string GetSpaceThemes()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `type`=0";
	   }
	  
	   #endregion


	   #region UserManage

	   public DataTable GetUsers(string idlist)
	   {
		   if (!Utils.IsNumericArray(idlist.Split(',')))
		   {
			   return new DataTable();
		   }

		   string sql = string.Format("SELECT `uid`,`username` FROM `{0}users` WHERE `groupid` IN ({1})", BaseConfigs.GetTablePrefix, idlist);
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }



	   public DataTable GetUserGroupInfoByGroupid(int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32, 4,groupid)
			};
		   string sql = "SELECT * From  `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=?groupid LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public DataTable GetAdmingroupByAdmingid(int admingid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?admingid",(DbType)MySqlDbType.Int32, 4,admingid)
			};
		   string sql = "SELECT * From  `" + BaseConfigs.GetTablePrefix + "admingroups` WHERE `admingid`=?admingid LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public DataTable GetMedal()
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "medals`";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public void AddMedal(int medalid, string name, int available, string image)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?medalid", (DbType)MySqlDbType.Int16,2, medalid),
				DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarChar,50, name),
				DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available),
				DbHelper.MakeInParam("?image",(DbType)MySqlDbType.VarChar,30,image)
			};
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "medals` (`medalid`,`name`,`available`,`image`) Values (?medalid,?name,?available,?image)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateMedal(int medalid, string name, string image)
	   {
		   IDataParameter[] prams =
			{

				DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarChar,50, name),
				DbHelper.MakeInParam("?image",(DbType)MySqlDbType.VarChar,30,image),
				DbHelper.MakeInParam("?medalid", (DbType)MySqlDbType.Int16,2, medalid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "medals` SET `name`=?name,`image`=?image  Where `medalid`=?medalid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void SetAvailableForMedal(int available, string medailidlist)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?available", (DbType)MySqlDbType.Int32, 4, available)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "medals` SET `available`=?available WHERE `medalid` IN(" + medailidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void DeleteMedalById(string medailidlist)
	   {
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "medals` WHERE `medalid` IN(" + medailidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public int GetMaxMedalId()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IFNULL(MAX(`medalid`),0) FROM `" + BaseConfigs.GetTablePrefix + "medals`"), 0) + 1;
	   }

	   public string GetGroupInfo()
	   {
		   string sql = "SELECT `groupid`, `grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` ORDER BY `groupid`";
		   return sql;
	   }

	   /// <summary>
	   /// 获得到指定管理组信息
	   /// </summary>
	   /// <returns>管理组信息</returns>
	   public DataTable GetAdminGroupList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "admingroups`").Tables[0];
	   }

	   /// <summary>
	   /// 设置管理组信息
	   /// </summary>
	   /// <param name="__admingroupsInfo">管理组信息</param>
	   /// <returns>更改记录数</returns>
	   public int SetAdminGroupInfo(AdminGroupInfo __admingroupsInfo)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?alloweditpost",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Alloweditpost),
									 DbHelper.MakeInParam("?alloweditpoll",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Alloweditpoll),
									 DbHelper.MakeInParam("?allowstickthread",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowstickthread),
									 DbHelper.MakeInParam("?allowmodpost",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowmodpost),
									 DbHelper.MakeInParam("?allowdelpost",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowdelpost),
									 DbHelper.MakeInParam("?allowmassprune",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowmassprune),
									 DbHelper.MakeInParam("?allowrefund",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowrefund),
									 DbHelper.MakeInParam("?allowcensorword",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowcensorword),
									 DbHelper.MakeInParam("?allowviewip",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowviewip),
									 DbHelper.MakeInParam("?allowbanip",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowbanip),
									 DbHelper.MakeInParam("?allowedituser",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowedituser),
									 DbHelper.MakeInParam("?allowmoduser",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowmoduser),
									 DbHelper.MakeInParam("?allowbanuser",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowbanuser),
									 DbHelper.MakeInParam("?allowpostannounce",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowpostannounce),
									 DbHelper.MakeInParam("?allowviewlog",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowviewlog),
									 DbHelper.MakeInParam("?disablepostctrl",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Disablepostctrl),
									 DbHelper.MakeInParam("?allowviewrealname",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowviewrealname),
									 DbHelper.MakeInParam("?admingid",(DbType)MySqlDbType.Int16,2,__admingroupsInfo.Admingid)
								 };


		   string strSQL = "UPDATE `" + BaseConfigs.GetTablePrefix + "admingroups`" +
			   " SET " +
			   "`alloweditpost`=?alloweditpost," +
			   "`alloweditpoll`=?alloweditpoll," +
			   "`allowstickthread`=?allowstickthread," +
			   "`allowmodpost`=?allowmodpost," +
			   "`allowdelpost`=?allowdelpost," +
			   "`allowmassprune`=?allowmassprune," +
			   "`allowrefund`=?allowrefund," +
			   "`allowcensorword`=?allowcensorword," +
			   "`allowviewip`=?allowviewip," +
			   "`allowbanip`=?allowbanip," +
			   "`allowedituser`=?allowedituser," +
			   "`allowmoduser`=?allowmoduser," +
			   "`allowbanuser`=?allowbanuser," +
			   "`allowpostannounce`=?allowpostannounce," +
			   "`allowviewlog`=?allowviewlog," +
			   "`disablepostctrl`=?disablepostctrl, " +
			   "`allowviewrealname`=?allowviewrealname" +

			   " WHERE `admingid`=?admingid";

       
		   return DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);

	   }

	   /// <summary>
	   /// 创建一个新的管理组信息
	   /// </summary>
	   /// <param name="__admingroupsInfo">要添加的管理组信息</param>
	   /// <returns>更改记录数</returns>
	   public int CreateAdminGroupInfo(AdminGroupInfo __admingroupsInfo)
	   {


		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?admingid",(DbType)MySqlDbType.Int16,2,__admingroupsInfo.Admingid),
									 DbHelper.MakeInParam("?alloweditpost",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Alloweditpost),
									 DbHelper.MakeInParam("?alloweditpoll",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Alloweditpoll),
									 DbHelper.MakeInParam("?allowstickthread",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowstickthread),
									 DbHelper.MakeInParam("?allowmodpost",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowmodpost),
									 DbHelper.MakeInParam("?allowdelpost",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowdelpost),
									 DbHelper.MakeInParam("?allowmassprune",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowmassprune),
									 DbHelper.MakeInParam("?allowrefund",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowrefund),
									 DbHelper.MakeInParam("?allowcensorword",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowcensorword),
									 DbHelper.MakeInParam("?allowviewip",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowviewip),
									 DbHelper.MakeInParam("?allowbanip",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowbanip),
									 DbHelper.MakeInParam("?allowedituser",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowedituser),
									 DbHelper.MakeInParam("?allowmoduser",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowmoduser),
									 DbHelper.MakeInParam("?allowbanuser",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowbanuser),
									 DbHelper.MakeInParam("?allowpostannounce",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowpostannounce),
									 DbHelper.MakeInParam("?allowviewlog",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Allowviewlog),
									 DbHelper.MakeInParam("?disablepostctrl",(DbType)MySqlDbType.Int32,4,__admingroupsInfo.Disablepostctrl)
								 };
		   string strSQL = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "admingroups`" +
			   "(`admingid`," +
			   "`alloweditpost`," +
			   "`alloweditpoll`," +
			   "`allowstickthread`," +
			   "`allowmodpost`," +
			   "`allowdelpost`," +
			   "`allowmassprune`," +
			   "`allowrefund`," +
			   "`allowcensorword`," +
			   "`allowviewip`," +
			   "`allowbanip`," +
			   "`allowedituser`," +
			   "`allowmoduser`," +
			   "`allowbanuser`," +
			   "`allowpostannounce`," +
			   "`allowviewlog`," +
			   "`disablepostctrl`) " +
			   "VALUES " +
			   "(?admingid," +
			   "?alloweditpost," +
			   "?alloweditpoll," +
			   "?allowstickthread," +
			   "?allowmodpost," +
			   "?allowdelpost," +
			   "?allowmassprune," +
			   "?allowrefund," +
			   "?allowcensorword," +
			   "?allowviewip," +
			   "?allowbanip," +
			   "?allowedituser," +
			   "?allowmoduser," +
			   "?allowbanuser," +
			   "?allowpostannounce," +
			   "?allowviewlog," +
			   "?disablepostctrl)";



		   return DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);








	   }

	   /// <summary>
	   /// 删除指定的管理组信息
	   /// </summary>
	   /// <param name="admingid">管理组ID</param>
	   /// <returns>更改记录数</returns>
	   public int DeleteAdminGroupInfo(short admingid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?admingid",(DbType)MySqlDbType.Int16,2,admingid),
		   };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "admingroups` WHERE `admingid` = ?admingid", prams);
	   }

	   public string GetAdminGroupInfoSql()
	   {
		   return "Select * From `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`>0 AND `radminid`<=3  Order By `groupid`";
	   }

	   public DataTable GetRaterangeByGroupid(int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32, 4,groupid)
			};
		   string sql = "SELECT `raterange` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=?groupid LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void UpdateRaterangeByGroupid(string raterange, int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?raterange",(DbType)MySqlDbType.VarChar, 500,raterange),
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32, 4,groupid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `raterange`=?raterange  WHERE `groupid`=?groupid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public string GetAudituserSql()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "users` Where `groupid`=8";
	   }

	   public DataSet GetAudituserUid()
	   {
		   string sql = "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `groupid`=8";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql);
	   }

	   public void ClearAuthstrByUidlist(string uidlist)
	   {
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields` SET `authstr`=''  WHERE `uid` IN (" + uidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public void ClearAllUserAuthstr()
	   {
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields` SET `authstr`=''  WHERE `uid` IN (SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `groupid`=8 )";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public void DeleteUserByUidlist(string uidlist)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "userfields` WHERE `uid` IN(" + uidlist + ")");
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid` IN(" + uidlist + ")");
	   }

	   public void DeleteAuditUser()
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "userfields` WHERE `uid` IN (SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `groupid`=8 )");
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `groupid`=8 ");
	   }

	   public DataTable GetAuditUserEmail()
	   {
		   string sql = "SELECT `username`,`password`,`email` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `groupid`=8";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }
	   public DataTable GetUserEmailByUidlist(string uidlist)
	   {
		   string sql = "SELECT `username`,`password`,`email` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid` IN(" + uidlist + ")";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public string GetUserGroup()
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`= 0 And `groupid`>8 ORDER BY `groupid`";
		   return sql;
	   }

	   public string GetUserGroupTitle()
	   {
		   return "SELECT `groupid`,`grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`= 0 And `groupid`>8 ORDER BY `groupid`";
	   }

	   public DataTable GetUserGroupWithOutGuestTitle()
	   {
		   return DbHelper.ExecuteDataset("SELECT `groupid`,`grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`<>7  ORDER BY `groupid` ASC").Tables[0];
	   }

	   public string GetAdminUserGroupTitle()
	   {
		   string sql = "SELECT `groupid`,`grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`> 0 AND `radminid`<=3  ORDER BY `groupid`";
		   return sql;
	   }

	   public void DeleteUserGroupInfo(int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32, 4,groupid)
			};
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "usergroups` Where `groupid`=?groupid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void DeleteAdminGroupInfo(int admingid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?admingid",(DbType)MySqlDbType.Int32, 4,admingid)
			};
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "admingroups` Where `admingid`=?admingid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void ChangeUsergroup(int soureceusergroupid, int targetusergroupid)
	   {
		   IDataParameter[] prams =
			{

				DbHelper.MakeInParam("?targetusergroupid",(DbType)MySqlDbType.Int32, 4,targetusergroupid),
				DbHelper.MakeInParam("?soureceusergroupid",(DbType)MySqlDbType.Int32, 4,soureceusergroupid)
			};
		   string sql = "Update `" + BaseConfigs.GetTablePrefix + "users` SET `groupid`=?targetusergroupid Where `groupid`=?soureceusergroupid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }
	   public DataTable GetAdmingid(int admingid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?admingid",(DbType)MySqlDbType.Int32, 4,admingid)
			};
		   string sql = "SELECT `admingid`  FROM `" + BaseConfigs.GetTablePrefix + "admingroups` WHERE `admingid`=?admingid";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void ChangeUserAdminidByGroupid(int adminid, int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?adminid",(DbType)MySqlDbType.Int32, 4,adminid),
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32, 4,groupid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `adminid`=?adminid WHERE `groupid`=?groupid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataTable GetAvailableMedal()
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "medals` WHERE `available`=1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public bool IsExistMedalAwardRecord(int medalid, int userid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?medalid", (DbType)MySqlDbType.Int32,4, medalid),
				DbHelper.MakeInParam("?userid",(DbType)MySqlDbType.Int32,4,userid)
			};
		   string sql = "SELECT ID FROM `" + BaseConfigs.GetTablePrefix + "medalslog` WHERE `medals`=?medalid AND `uid`=?userid LIMIT 1";
		   if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count != 0)
			   return true;
		   else
			   return false;
	   }

	   public void AddMedalslog(int adminid, string adminname, string ip, string username, int uid, string actions, int medals, string reason)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?adminid", (DbType)MySqlDbType.Int32,4, adminid),
				DbHelper.MakeInParam("?adminname",(DbType)MySqlDbType.VarChar,50,adminname),
				DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarChar,15, ip),
				DbHelper.MakeInParam("?username",(DbType)MySqlDbType.VarChar,50,username),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid),
				DbHelper.MakeInParam("?actions",(DbType)MySqlDbType.VarChar,100,actions),
				DbHelper.MakeInParam("?medals", (DbType)MySqlDbType.Int32,4, medals),
				DbHelper.MakeInParam("?reason",(DbType)MySqlDbType.VarChar,100,reason)
			};
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "medalslog` (`adminid`,`adminname`,`ip`,`username`,`uid`,`actions`,`medals`,`reason`) VALUES (?adminid,?adminname,?ip,?username,?uid,?actions,?medals,?reason)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, int medals, int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?newactions",(DbType)MySqlDbType.VarChar,100,newactions),
				DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Date,8,postdatetime),
				DbHelper.MakeInParam("?reason",(DbType)MySqlDbType.VarChar,100,reason),
				DbHelper.MakeInParam("?oldactions",(DbType)MySqlDbType.String,100,oldactions),
				DbHelper.MakeInParam("?medals", (DbType)MySqlDbType.Int32,4, medals),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid)
			};
		   string sql = "Update `" + BaseConfigs.GetTablePrefix + "medalslog` SET `actions`=?newactions ,`postdatetime`=?postdatetime, reason=?reason  WHERE `actions`=?oldactions AND `medals`=?medals  AND `uid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateMedalslog(string actions, DateTime postdatetime, string reason, int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?actions",(DbType)MySqlDbType.VarChar,100,actions),
				DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Datetime,8,postdatetime),
				DbHelper.MakeInParam("?reason",(DbType)MySqlDbType.VarChar,100,reason),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid)
			};
		   string sql = "Update `" + BaseConfigs.GetTablePrefix + "medalslog` SET `actions`=?actions ,`postdatetime`=?postdatetime,reason=?reason  WHERE `uid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, string medalidlist, int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?newactions",(DbType)MySqlDbType.VarChar,100,newactions),
				DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Datetime,8,postdatetime),
				DbHelper.MakeInParam("?reason",(DbType)MySqlDbType.VarChar,100,reason),
				DbHelper.MakeInParam("?oldactions",(DbType)MySqlDbType.VarChar,100,oldactions),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid)
			};
		   string sql = "Update `" + BaseConfigs.GetTablePrefix + "medalslog` SET `actions`='" + newactions + "' ,`postdatetime`='" + postdatetime + "', reason='" + reason + "'  WHERE `actions`='" + oldactions + "' AND `medals` NOT IN (" + medalidlist + ") AND `uid`=" + uid + "";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void SetStopTalkUser(string uidlist)
	   {
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `groupid`=4, `adminid`=0  WHERE `uid` IN (" + uidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql);
	   }

	   public void ChangeUserGroupByUid(int groupid, string uidlist)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32,4,groupid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `groupid`=?groupid  WHERE `uid` IN (" + uidlist + ")";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataTable GetTableListInfo()
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "tablelist`";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public void DeletePostByPosterid(int tabid, int posterid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32,4, posterid)
			};
		   string sql = "DELETE FROM  `" + BaseConfigs.GetTablePrefix + "posts" + tabid + "`   WHERE `posterid`=?posterid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void DeleteTopicByPosterid(int posterid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32,4, posterid)
			};
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `posterid`=?posterid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void ClearPosts(int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `digestposts`=0 , `posts`=0  WHERE `uid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateEmailValidateInfo(string authstr, DateTime authtime, int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?authstr",(DbType)MySqlDbType.VarChar,20,authstr),
				DbHelper.MakeInParam("?authtime",(DbType)MySqlDbType.Datetime,8,authtime),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields` SET `Authstr`=?authstr,`Authtime`=?authtime ,`Authflag`=1  WHERE `uid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public int GetRadminidByGroupid(int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32,4, groupid)
			};
		   string sql = "SELECT `radminid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=?groupid LIMIT 1";
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
	   }

	   public string GetTemplateInfo()
	   {
		   string sql = "SELECT `templateid`, `name` FROM `" + BaseConfigs.GetTablePrefix + "templates`";
		   return sql;
	   }

	   public DataTable GetUserEmailByGroupid(string groupidlist)
	   {
		   string sql = "SELECT `username`,`Email`  From `" + BaseConfigs.GetTablePrefix + "users` WHERE `Email` Is Not null AND `Email`<>'' AND `groupid` IN(" + groupidlist + ")";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetUserGroupExceptGroupid(int groupid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32, 4,groupid)
			};
		   string sql = "SELECT `groupid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`=0 And `groupid`>8 AND `groupid`<>?groupid";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   /// <summary>
	   /// 创建收藏信息
	   /// </summary>
	   /// <param name="uid">用户ID</param>
	   /// <param name="tid">主题ID</param>
	   /// <returns>创建成功返回 1 否则返回 0</returns>
	   public int CreateFavorites(int uid, int tid)
	   {
		   return CreateFavorites(uid, tid, 0);
	   }

	   /// <summary>
	   /// 创建收藏信息
	   /// </summary>
	   /// <param name="uid">用户ID</param>
	   /// <param name="tid">主题ID</param>
	   /// <param name="type">收藏类型，0=主题，1=相册，2=博客日志</param>
	   /// <returns>创建成功返回 1 否则返回 0</returns>
	   public int CreateFavorites(int uid, int tid, byte type)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.UInt16, 4, type)
								 };


		   string sqlCreateFavorite = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "favorites` (`uid`,`tid`,`typeid`) VALUES(?uid,?tid,?type)";

		   return DbHelper.ExecuteNonQuery(CommandType.Text, sqlCreateFavorite, prams);
	   }



	   /// <summary>
	   /// 删除指定用户的收藏信息
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="fitemid">要删除的收藏信息id列表,以英文逗号分割</param>
	   /// <returns>删除的条数．出错时返回 -1</returns>
	   public int DeleteFavorites(int uid, string fidlist, byte type)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid),
									 DbHelper.MakeInParam("?typeid", (DbType)MySqlDbType.Int32, 1, type)
								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `tid` IN (" + fidlist + ") AND `uid` = ?uid  AND `typeid`=?typeid", prams);
	   }

	   /// <summary>
	   /// 得到用户收藏信息列表
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="pagesize">分页时每页的记录数</param>
	   /// <param name="pageindex">当前页码</param>
	   /// <returns>用户信息列表</returns>
	   public DataTable GetFavoritesList(int uid, int pagesize, int pageindex)
	   {
		   return GetFavoritesList(uid, pagesize, pageindex, 0);
	   }

	   /// <summary>
	   /// 得到用户收藏信息列表
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="pagesize">分页时每页的记录数</param>
	   /// <param name="pageindex">当前页码</param>
	   /// <param name="typeid">收藏类型id</param>
	   /// <returns>用户信息列表</returns>
	   public DataTable GetFavoritesList(int uid, int pagesize, int currentpage, int typeid)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,pagesize),
									 DbHelper.MakeInParam("?pageindex",(DbType)MySqlDbType.Int32,4,currentpage),
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid)
								 };


		   String sql = "";

		   switch (typeid)
		   {
			   case 1:

				   sql = "SELECT `tid`, `uid`, `albumid`, `albumcateid`, `posterid`, `poster`, `title`, `description`, `logo`, `password`, `imgcount`, `views`, `type`, `postdatetime`  FROM (SELECT `f`.`tid`, `f`.`uid`, `albumid`, `albumcateid`, `userid` AS `posterid`, `username` AS `poster`, `title`, `description`, `logo`, `password`, `imgcount`, `views`, `type`, `createdatetime` AS `postdatetime` FROM `" + BaseConfigs.GetTablePrefix + "favorites` `f`,`" + BaseConfigs.GetTablePrefix + "albums` `albums` WHERE `f`.`tid`=`albums`.`albumid` AND `f`.`typeid`=1 AND `f`.`uid`=" + uid + ") f ORDER BY `tid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
				   break;
			   case 2:
				   string tempstring0 = "SELECT `f`.`tid`, `f`.`uid`, `postid`, `author` AS `poster`, `spaceposts`.`uid` AS `posterid`, `postdatetime`, `title`, `category`, `poststatus`, `commentstatus`, `postupdatetime`, `commentcount`, `views` FROM `" + BaseConfigs.GetTablePrefix + "favorites` `f`,`" + BaseConfigs.GetTablePrefix + "spaceposts` `spaceposts` WHERE `f`.`tid`=`spaceposts`.`postid` AND `f`.`typeid`=2 AND `f`.`uid`=" + uid.ToString() + "";
				   sql = "SELECT `tid`, `postid`, `poster`, `posterid`, `uid`, `postdatetime`, `title`, `category`, `poststatus`, `commentstatus`, `postupdatetime`, `commentcount`, `views`  FROM (" + tempstring0 + ") f ORDER BY `tid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
				   break;

			   default:
				   string tempstring1 = "SELECT `f`.`uid`,`f`.`tid`,`topics`.`title`,`topics`.`poster`,`topics`.`postdatetime`,`topics`.`replies`,`topics`.`views`,`topics`.`posterid` FROM `" + BaseConfigs.GetTablePrefix + "favorites` `f`,`" + BaseConfigs.GetTablePrefix + "topics` `topics` WHERE `f`.`tid`=`topics`.`tid` AND `f`.`typeid`=0 AND `f`.`uid`=" + uid + "";
				   sql = "SELECT `uid`,`tid`,`title`,`poster`,`postdatetime`,`replies`,`views`,`posterid`  FROM (" + tempstring1 + ") f ORDER BY `tid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString(); ;
				   break;
		   }

		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   /// <summary>
	   /// 得到用户收藏的总数
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns>收藏总数</returns>
	   public int GetFavoritesCount(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   };



		   string sqlGetFavoritescount = "SELECT COUNT(`uid`) as `c` FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `uid`=?uid";


		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sqlGetFavoritescount, prams).ToString(), 0);


	   }

	   public int GetFavoritesCount(int uid, int typeid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?typeid",(DbType)MySqlDbType.Int32,1,typeid)
								 };
		   String sql = "SELECT COUNT(uid) as c FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `uid`=?uid AND `typeid`=?typeid";
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString(), 0);
		   //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescountbytype", prams).ToString(), 0);
	   }

	   public int CheckFavoritesIsIN(int uid, int tid)
	   {
		   return CreateFavorites(uid, tid, 0);
	   }

	   /// <summary>
	   /// 收藏夹里是否包含了指定的主题
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="tid">主题id</param>
	   /// <returns></returns>
	   public int CheckFavoritesIsIN(int uid, int tid, byte type)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?tid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?type", (DbType)MySqlDbType.Int32, 4, type)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`tid`) AS `tidcount` FROM `" + BaseConfigs.GetTablePrefix + "favorites` WHERE `tid`=?tid AND `uid`=?uid AND `typeid`=?type", prams), 0);
	   }


	   public void UpdateUserAllInfo(UserInfo userinfo)
	   {
		   string sqlstring = "Update `" + BaseConfigs.GetTablePrefix + "users` Set username=?username ,nickname=?nickname,secques=?secques,gender=?gender,adminid=?adminid,groupid=?groupid,groupexpiry=?groupexpiry,extgroupids=?extgroupids, regip=?regip," +
			   "joindate=?joindate , lastip=?lastip, lastvisit=?lastvisit,  lastactivity=?lastactivity, lastpost=?lastpost, lastposttitle=?lastposttitle,posts=?posts, digestposts=?digestposts,oltime=?oltime,pageviews=?pageviews,credits=?credits," +
			   "avatarshowid=?avatarshowid, email=?email,bday=?bday,sigstatus=?sigstatus,tpp=?tpp,ppp=?ppp,templateid=?templateid,pmsound=?pmsound," +
			   "showemail=?showemail,newsletter=?newsletter,invisible=?invisible,newpm=?newpm,accessmasks=?accessmasks,extcredits1=?extcredits1,extcredits2=?extcredits2,extcredits3=?extcredits3,extcredits4=?extcredits4,extcredits5=?extcredits5,extcredits6=?extcredits6,extcredits7=?extcredits7,extcredits8=?extcredits8   Where uid=?uid";

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, userinfo.Username),
									 DbHelper.MakeInParam("?nickname", (DbType)MySqlDbType.VarChar, 10, userinfo.Nickname),
									 DbHelper.MakeInParam("?secques", (DbType)MySqlDbType.VarChar, 8, userinfo.Secques),
									 DbHelper.MakeInParam("?gender", (DbType)MySqlDbType.Int32, 4, userinfo.Gender),
									 DbHelper.MakeInParam("?adminid", (DbType)MySqlDbType.Int32, 4, userinfo.Uid == 1 ? 1 : userinfo.Adminid),
									 DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int16, 2, userinfo.Groupid),
									 DbHelper.MakeInParam("?groupexpiry", (DbType)MySqlDbType.Int32, 4, userinfo.Groupexpiry),
									 DbHelper.MakeInParam("?extgroupids", (DbType)MySqlDbType.VarChar, 60, userinfo.Extgroupids),
									 DbHelper.MakeInParam("?regip", (DbType)MySqlDbType.VarChar, 15, userinfo.Regip),
									 DbHelper.MakeInParam("?joindate", (DbType)MySqlDbType.Datetime, 4, userinfo.Joindate),
									 DbHelper.MakeInParam("?lastip", (DbType)MySqlDbType.VarChar, 15, userinfo.Lastip),
									 DbHelper.MakeInParam("?lastvisit", (DbType)MySqlDbType.Datetime, 8, userinfo.Lastvisit),
									 DbHelper.MakeInParam("?lastactivity", (DbType)MySqlDbType.Datetime, 8, userinfo.Lastactivity),
									 DbHelper.MakeInParam("?lastpost", (DbType)MySqlDbType.Datetime, 8, userinfo.Lastpost),
									 DbHelper.MakeInParam("?lastposttitle", (DbType)MySqlDbType.VarChar, 80, userinfo.Lastposttitle),
									 DbHelper.MakeInParam("?posts", (DbType)MySqlDbType.Int32, 4, userinfo.Posts),
									 DbHelper.MakeInParam("?digestposts", (DbType)MySqlDbType.Int16, 2, userinfo.Digestposts),
									 DbHelper.MakeInParam("?oltime", (DbType)MySqlDbType.Int32, 4, userinfo.Oltime),
									 DbHelper.MakeInParam("?pageviews", (DbType)MySqlDbType.Int32, 4, userinfo.Pageviews),
									 DbHelper.MakeInParam("?credits", (DbType)MySqlDbType.Decimal, 10, userinfo.Credits),
									 DbHelper.MakeInParam("?avatarshowid", (DbType)MySqlDbType.Int32, 4, userinfo.Avatarshowid),
									 DbHelper.MakeInParam("?email", (DbType)MySqlDbType.VarChar, 50, userinfo.Email.ToString()),
									 DbHelper.MakeInParam("?bday", (DbType)MySqlDbType.VarChar, 10, userinfo.Bday.ToString()),
									 DbHelper.MakeInParam("?sigstatus", (DbType)MySqlDbType.Int32, 4, userinfo.Sigstatus.ToString()),
									 DbHelper.MakeInParam("?tpp", (DbType)MySqlDbType.Int32, 4, userinfo.Tpp),
									 DbHelper.MakeInParam("?ppp", (DbType)MySqlDbType.Int32, 4, userinfo.Ppp),
									 DbHelper.MakeInParam("?templateid", (DbType)MySqlDbType.Int32, 4, userinfo.Templateid),
									 DbHelper.MakeInParam("?pmsound", (DbType)MySqlDbType.Int32, 4, userinfo.Pmsound),
									 DbHelper.MakeInParam("?showemail", (DbType)MySqlDbType.Int32, 4, userinfo.Showemail),
									 DbHelper.MakeInParam("?newsletter", (DbType)MySqlDbType.Int32, 4, userinfo.Newsletter),
									 DbHelper.MakeInParam("?invisible", (DbType)MySqlDbType.Int32, 4, userinfo.Invisible),
									 DbHelper.MakeInParam("?newpm", (DbType)MySqlDbType.Int32, 4, userinfo.Newpm),
									 DbHelper.MakeInParam("?accessmasks", (DbType)MySqlDbType.Int32, 4, userinfo.Accessmasks),
									 DbHelper.MakeInParam("?extcredits1", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits1),
									 DbHelper.MakeInParam("?extcredits2", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits2),
									 DbHelper.MakeInParam("?extcredits3", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits3),
									 DbHelper.MakeInParam("?extcredits4", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits4),
									 DbHelper.MakeInParam("?extcredits5", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits5),
									 DbHelper.MakeInParam("?extcredits6", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits6),
									 DbHelper.MakeInParam("?extcredits7", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits7),
									 DbHelper.MakeInParam("?extcredits8", (DbType)MySqlDbType.Decimal, 10, userinfo.Extcredits8),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userinfo.Uid)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
	   }

	   public void DeleteModerator(int uid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "moderators` WHERE `uid`=" + uid);
	   }

	   public void UpdateUserField(UserInfo __userinfo, string signature, string authstr, string sightml)
	   {

		   IDataParameter[] prams1 = {
									  DbHelper.MakeInParam("?website", (DbType)MySqlDbType.VarChar, 80, __userinfo.Website),
									  DbHelper.MakeInParam("?icq", (DbType)MySqlDbType.VarChar, 12, __userinfo.Icq),
									  DbHelper.MakeInParam("?qq", (DbType)MySqlDbType.VarChar, 12, __userinfo.Qq),
									  DbHelper.MakeInParam("?yahoo", (DbType)MySqlDbType.VarChar, 40, __userinfo.Yahoo),
									  DbHelper.MakeInParam("?msn", (DbType)MySqlDbType.VarChar, 40, __userinfo.Msn),
									  DbHelper.MakeInParam("?skype", (DbType)MySqlDbType.VarChar, 40, __userinfo.Skype),
									  DbHelper.MakeInParam("?location", (DbType)MySqlDbType.VarChar, 50, __userinfo.Location),
									  DbHelper.MakeInParam("?customstatus", (DbType)MySqlDbType.VarChar, 50, __userinfo.Customstatus),
									  DbHelper.MakeInParam("?avatar", (DbType)MySqlDbType.VarChar, 255, __userinfo.Avatar),
									  DbHelper.MakeInParam("?avatarwidth", (DbType)MySqlDbType.Int32, 4, __userinfo.Avatarwidth),
									  DbHelper.MakeInParam("?avatarheight", (DbType)MySqlDbType.Int32, 4, __userinfo.Avatarheight),
									  DbHelper.MakeInParam("?medals", (DbType)MySqlDbType.VarChar, 300, __userinfo.Medals),
									  DbHelper.MakeInParam("?authstr", (DbType)MySqlDbType.VarChar, 20, authstr),
									  DbHelper.MakeInParam("?authtime", (DbType)MySqlDbType.Datetime, 4, __userinfo.Authtime),
									  DbHelper.MakeInParam("?authflag", (DbType)MySqlDbType.Int16, 1, 1),
									  DbHelper.MakeInParam("?bio", (DbType)MySqlDbType.VarChar, 500, __userinfo.Bio.ToString()),
									  DbHelper.MakeInParam("?signature", (DbType)MySqlDbType.VarChar, 500, signature),
									  DbHelper.MakeInParam("?sightml", (DbType)MySqlDbType.VarChar, 1000, sightml),
									  DbHelper.MakeInParam("?Realname", (DbType)MySqlDbType.VarChar, 1000, __userinfo.Realname),
									  DbHelper.MakeInParam("?Idcard", (DbType)MySqlDbType.VarChar, 1000, __userinfo.Idcard),
									  DbHelper.MakeInParam("?Mobile", (DbType)MySqlDbType.VarChar, 1000, __userinfo.Mobile),
									  DbHelper.MakeInParam("?Phone", (DbType)MySqlDbType.VarChar, 1000, __userinfo.Phone),
									  DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, __userinfo.Uid)
								  };
		   string sqlstring = "Update `" + BaseConfigs.GetTablePrefix + "userfields` Set website=?website,icq=?icq,qq=?qq,yahoo=?yahoo,msn=?msn,skype=?skype,location=?location,customstatus=?customstatus, avatar=?avatar," +
			   "avatarwidth=?avatarwidth , avatarheight=?avatarheight, medals=?medals,  authstr=?authstr, authtime=?authtime, authflag=?authflag,bio=?bio, signature=?signature,sightml=?sightml,realname=?Realname,idcard=?Idcard,mobile=?Mobile,phone=?Phone   Where uid=?uid";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
	   }



	   public void UpdatePMSender(int msgfromid, string msgfrom)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?msgfrom", (DbType)MySqlDbType.VarChar, 20, msgfrom),
									 DbHelper.MakeInParam("?msgfromid", (DbType)MySqlDbType.Int32, 4, msgfromid)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "pms` SET `msgfrom`=?msgfrom WHERE `msgfromid`=?msgfromid", parms);
	   }

	   public void UpdatePMReceiver(int msgtoid, string msgto)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?msgto", (DbType)MySqlDbType.VarChar, 20, msgto),
									 DbHelper.MakeInParam("?msgtoid", (DbType)MySqlDbType.Int32, 4, msgtoid)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "pms` SET `msgto`=?msgto  WHERE `msgtoid`=?msgtoid", parms);
	   }



	   public DataRowCollection GetModerators(string oldusername)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?oldusername", (DbType)MySqlDbType.VarChar, 20, RegEsc(oldusername))
								 };

		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid`,`moderators` FROM  `" + BaseConfigs.GetTablePrefix + "forumfields` WHERE `moderators` LIKE '% ?oldusername %'", prams).Tables[0].Rows;
	   }

	   public DataTable GetModeratorsTable(string oldusername)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?oldusername", (DbType)MySqlDbType.VarChar, 20, RegEsc(oldusername))
								 };

		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `fid`,`moderators` FROM  `" + BaseConfigs.GetTablePrefix + "forumfields` WHERE `moderators` LIKE '% ?oldusername %'", prams).Tables[0];
	   }

	   public void UpdateModerators(int fid, string moderators)
	   {
		   IDataParameter[] parm = {
									DbHelper.MakeInParam("?moderators", (DbType)MySqlDbType.VarChar, 20, moderators),
									DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
								};

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "forumfields` SET `moderators`=?moderators  WHERE `fid`=?fid", parm);
	   }

	   public void UpdateUserCredits(int userid, float credits, float extcredits1, float extcredits2, float extcredits3, float extcredits4, float extcredits5, float extcredits6, float extcredits7, float extcredits8)
	   {
		   IDataParameter[] prams1 = {

									  DbHelper.MakeInParam("?Credits",(DbType)MySqlDbType.Decimal,9, credits),
									  DbHelper.MakeInParam("?Extcredits1", (DbType)MySqlDbType.Decimal, 20,extcredits1),
									  DbHelper.MakeInParam("?Extcredits2", (DbType)MySqlDbType.Decimal, 20,extcredits2),
									  DbHelper.MakeInParam("?Extcredits3", (DbType)MySqlDbType.Decimal, 20,extcredits3),
									  DbHelper.MakeInParam("?Extcredits4", (DbType)MySqlDbType.Decimal, 20,extcredits4),
									  DbHelper.MakeInParam("?Extcredits5", (DbType)MySqlDbType.Decimal, 20,extcredits5),
									  DbHelper.MakeInParam("?Extcredits6", (DbType)MySqlDbType.Decimal, 20,extcredits6),
									  DbHelper.MakeInParam("?Extcredits7", (DbType)MySqlDbType.Decimal, 20,extcredits7),
									  DbHelper.MakeInParam("?Extcredits8", (DbType)MySqlDbType.Decimal, 20,extcredits8),
									  DbHelper.MakeInParam("?targetuid",(DbType)MySqlDbType.Int32, 4,userid.ToString())
								  };

		   string sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET credits=?Credits,extcredits1=?Extcredits1, extcredits2=?Extcredits2, extcredits3=?Extcredits3, extcredits4=?Extcredits4, extcredits5=?Extcredits5, extcredits6=?Extcredits6, extcredits7=?Extcredits7, extcredits8=?Extcredits8 WHERE `uid`=?targetuid";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
	   }

	   public void CombinationUser(string posttablename, UserInfo __targetuserinfo, UserInfo __srcuserinfo)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?target_uid", (DbType)MySqlDbType.Int32, 4, __targetuserinfo.Uid),
									 DbHelper.MakeInParam("?target_username", (DbType)MySqlDbType.VarChar, 20, __targetuserinfo.Username.Trim()),
									 DbHelper.MakeInParam("?src_uid", (DbType)MySqlDbType.Int32, 4, __srcuserinfo.Uid)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  `" + BaseConfigs.GetTablePrefix + "topics` SET `posterid`=?target_uid,`poster`=?target_username  WHERE `posterid`=?src_uid", prams);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  `" + posttablename + "` SET `posterid`=?target_uid,`poster`=?target_username  WHERE `posterid`=?src_uid", prams);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  `" + BaseConfigs.GetTablePrefix + "pms` SET `msgtoid`=?target_uid,`msgto`=?target_username  WHERE `msgtoid`=?src_uid", prams);
	   }

	   /// <summary>
	   /// 通过用户名得到UID
	   /// </summary>
	   /// <param name="username"></param>
	   /// <returns></returns>
	   public int GetuidByusername(string username)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, username)
								 };

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `username`=?username LIMIT 1", prams).Tables[0];
		   if (dt.Rows.Count > 0)
		   {
			   return Convert.ToInt32(dt.Rows[0][0].ToString());
		   }
		   else
		   {
			   return 0;
		   }
	   }

	   /// <summary>
	   /// 删除指定用户的所有信息,事务处理
	   /// </summary>
	   /// <param name="uid">指定的用户uid</param>
	   /// <param name="delposts">是否删除帖子</param>
	   /// <param name="delpms">是否删除短消息</param>
	   /// <returns></returns>
	   public bool DelUserAllInf(int uid, bool delposts, bool delpms)
	   {
		   //  SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
		   //  conn.Open();
		   //  using (SqlTransaction trans = conn.BeginTransaction())
		   //   {
		   //   try
		   //   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "users` Where `uid`=" + uid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "userfields` Where `uid`=" + uid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "polls` Where `userid`=" + uid.ToString());
		   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "favorites` Where `uid`=" + uid.ToString());

		   if (delposts)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From `" + BaseConfigs.GetTablePrefix + "topics` Where `posterid`=" + uid.ToString());

			   //清除用户所发的帖子
			   foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "tablelist`").Tables[0].Rows)
			   {
				   if (dr["id"].ToString() != "")
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM  `" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "`   WHERE `posterid`=" + uid);
				   }
			   }
		   }
		   else
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `poster`='该用户已被删除'  Where `posterid`=" + uid.ToString());

			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "topics` SET `lastposter`='该用户已被删除'  Where `lastpostid`=" + uid.ToString());

			   //清除用户所发的帖子
			   foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "tablelist`").Tables[0].Rows)
			   {
				   if (dr["id"].ToString() != "")
				   {
					   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  `" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "` SET  `poster`='该用户已被删除'  WHERE `posterid`=" + uid);
				   }
			   }
		   }

		   if (delpms)
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "pms` Where `msgfromid`=" + uid.ToString());
		   }
		   else
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "pms` SET `msgfrom`='该用户已被删除'  Where `msgfromid`=" + uid.ToString());
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "pms` SET `msgto`='该用户已被删除'  Where `msgtoid`=" + uid.ToString());
		   }

		   //删除版主表的相关用户信息
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "moderators` WHERE `uid`=" + uid.ToString());

		   //更新当前论坛总人数
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "Statistics` SET `totalusers`=`totalusers`-1");

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `uid`,`username` FROM `" + BaseConfigs.GetTablePrefix + "users` ORDER BY `uid` DESC LIMIT 1").Tables[0];
		   if (dt.Rows.Count > 0)
		   {
			   //更新当前论坛最新注册会员信息
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics` SET `lastuserid`=" + dt.Rows[0][0] + ", `lastusername`='" + dt.Rows[0][1] + "'");
		   }



		   //trans.Commit();

		   //   }
		   // catch (Exception ex)
		   //{
		   //    trans.Rollback();
		   //    throw ex;
		   //  }
		   // }
		   // conn.Close();
		   return true;
	   }

	   public DataTable GetUserGroup(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);

		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=?groupid", parm).Tables[0];
	   }

	   public DataTable GetAdminGroup(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);

		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "admingroups` WHERE `admingid`=?groupid LIMIT 1", parm).Tables[0];
	   }

	   public void AddUserGroup(UserGroupInfo __usergroupinfo, int Creditshigher, int Creditslower)
	   {
		   IDataParameter[] prams = 
					{
						DbHelper.MakeInParam("?Radminid",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Radminid),
						DbHelper.MakeInParam("?Grouptitle",(DbType)MySqlDbType.VarChar,50, Utils.RemoveFontTag(__usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("?Creditshigher",(DbType)MySqlDbType.Int32,4,Creditshigher),
						DbHelper.MakeInParam("?Creditslower",(DbType)MySqlDbType.Int32,4,Creditslower),
						DbHelper.MakeInParam("?Stars",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Stars),
						DbHelper.MakeInParam("?Color",(DbType)MySqlDbType.VarChar,7,__usergroupinfo.Color),
						DbHelper.MakeInParam("?Groupavatar",(DbType)MySqlDbType.VarChar,60,__usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("?Readaccess",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Readaccess),
						DbHelper.MakeInParam("?Allowvisit",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("?Allowpost",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpost),
						DbHelper.MakeInParam("?Allowreply",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowreply),
						DbHelper.MakeInParam("?Allowpostpoll",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("?Allowdirectpost",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("?Allowgetattach",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("?Allowpostattach",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("?Allowvote",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowvote),
						DbHelper.MakeInParam("?Allowmultigroups",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("?Allowsearch",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("?Allowavatar",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("?Allowcstatus",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("?Allowuseblog",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("?Allowinvisible",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("?Allowtransfer",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("?Allowsetreadperm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("?Allowsetattachperm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("?Allowhidecode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("?Allowhtml",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("?Allowcusbbcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("?Allownickname",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allownickname),
						DbHelper.MakeInParam("?Allowsigbbcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("?Allowsigimgcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("?Allowviewpro",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("?Allowviewstats",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowviewstats),
						DbHelper.MakeInParam("?Disableperiodctrl",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Disableperiodctrl),
						DbHelper.MakeInParam("?Reasonpm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("?Maxprice",(DbType)MySqlDbType.Int32,2,__usergroupinfo.Maxprice),
						DbHelper.MakeInParam("?Maxpmnum",(DbType)MySqlDbType.Int32,2,__usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("?Maxsigsize",(DbType)MySqlDbType.Int32,2,__usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("?Maxattachsize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("?Maxsizeperday",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("?Attachextensions",(DbType)MySqlDbType.VarChar,100,__usergroupinfo.Attachextensions),
						DbHelper.MakeInParam("?Maxspaceattachsize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxspaceattachsize),
						DbHelper.MakeInParam("?Maxspacephotosize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("?Raterange",(DbType)MySqlDbType.VarChar,100,__usergroupinfo.Raterange)
					};

		   string sqlstring = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "usergroups`  (`radminid`,`grouptitle`,`creditshigher`,`creditslower`," +
			   "`stars` ,`color`, `groupavatar`,`readaccess`, `allowvisit`,`allowpost`,`allowreply`," +
			   "`allowpostpoll`, `allowdirectpost`,`allowgetattach`,`allowpostattach`,`allowvote`,`allowmultigroups`," +
			   "`allowsearch`,`allowavatar`,`allowcstatus`,`allowuseblog`,`allowinvisible`,`allowtransfer`," +
			   "`allowsetreadperm`,`allowsetattachperm`,`allowhidecode`,`allowhtml`,`allowcusbbcode`,`allownickname`," +
			   "`allowsigbbcode`,`allowsigimgcode`,`allowviewpro`,`allowviewstats`,`disableperiodctrl`,`reasonpm`," +
			   "`maxprice`,`maxpmnum`,`maxsigsize`,`maxattachsize`,`maxsizeperday`,`attachextensions`,`raterange`,`maxspaceattachsize`,`maxspacephotosize`) VALUES(" +
			   "?Radminid,?Grouptitle,?Creditshigher,?Creditslower,?Stars,?Color,?Groupavatar,?Readaccess,?Allowvisit,?Allowpost,?Allowreply," +
			   "?Allowpostpoll,?Allowdirectpost,?Allowgetattach,?Allowpostattach,?Allowvote,?Allowmultigroups,?Allowsearch,?Allowavatar,?Allowcstatus," +
			   "?Allowuseblog,?Allowinvisible,?Allowtransfer,?Allowsetreadperm,?Allowsetattachperm,?Allowhidecode,?Allowhtml,?Allowcusbbcode,?Allownickname," +
			   "?Allowsigbbcode,?Allowsigimgcode,?Allowviewpro,?Allowviewstats,?Disableperiodctrl,?Reasonpm,?Maxprice,?Maxpmnum,?Maxsigsize,?Maxattachsize," +
			   "?Maxsizeperday,?Attachextensions,?Raterange,?Maxspaceattachsize,?Maxspacephotosize)";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
	   }

	   public void AddOnlineList(string grouptitle)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, GetMaxUserGroupId()),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 50, grouptitle)
								 };
		   string sqlstring = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "onlinelist` (`groupid`, `title`, `img`) VALUES(?groupid,?title,'')";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
	   }

	   public DataTable GetMinCreditHigher()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MIN(Creditshigher) FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`>8 AND `radminid`=0 ").Tables[0];
	   }

	   public DataTable GetMaxCreditLower()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(Creditslower) FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`>8 AND `radminid`=0 ").Tables[0];
	   }

	   public DataTable GetUserGroupByCreditshigher(int Creditshigher)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?Creditshigher", (DbType)MySqlDbType.Int32, 4, Creditshigher);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid`,`creditshigher`,`creditslower` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`>8 AND `radminid`=0  AND `Creditshigher`<=?Creditshigher AND ?Creditshigher<`Creditslower` LIMIT 1", parm).Tables[0];
	   }

	   public void UpdateUserGroupCreditsHigher(int currentGroupID, int Creditslower)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?creditshigher", (DbType)MySqlDbType.Int32, 4, Creditslower),
									 DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, currentGroupID)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET creditshigher=?creditshigher WHERE `groupid`=?groupid", parms);
	   }

	   public void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int Creditshigher)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?creditslower", (DbType)MySqlDbType.Int32, 4, Creditshigher),
									 DbHelper.MakeInParam("?creditshigher", (DbType)MySqlDbType.Int32, 4, currentCreditsHigher)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `creditslower`=?creditslower WHERE `groupid`>8 AND `radminid`=0 AND `creditshigher`=?creditshigher", parms);
	   }

	   public DataTable GetUserGroupByCreditsHigherAndLower(int Creditshigher, int Creditslower)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?Creditshigher", (DbType)MySqlDbType.Int32, 4, Creditshigher),
									 DbHelper.MakeInParam("?Creditslower", (DbType)MySqlDbType.Int32, 4, Creditslower)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`>8 AND `radminid`=0 AND `Creditshigher`=?Creditshigher AND `Creditslower`=?Creditslower", parms).Tables[0];
	   }
	   public int GetGroupCountByCreditsLower(int Creditshigher)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?creditslower", (DbType)MySqlDbType.Int32, 4, Creditshigher);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`>8 AND `radminid`=0 AND `creditslower`=?creditslower", parm).Tables[0].Rows.Count;
	   }

	   public void UpdateUserGroupsCreditsLowerByCreditsLower(int Creditslower, int Creditshigher)
	   {
		   IDataParameter[] parms = {

									 DbHelper.MakeInParam("?Creditslower", (DbType)MySqlDbType.Int32, 4, Creditslower),
									 DbHelper.MakeInParam("?Creditshigher", (DbType)MySqlDbType.Int32, 4, Creditshigher)
								 };
		   DbHelper.ExecuteDataset(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `creditslower`=?Creditslower WHERE `groupid`>8 AND `radminid`=0 AND `creditslower`=?Creditshigher", parms);
	   }

	   public void UpdateUserGroupsCreditsHigherByCreditsHigher(int Creditshigher, int Creditslower)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?Creditshigher", (DbType)MySqlDbType.Int32, 4, Creditshigher),
									 DbHelper.MakeInParam("?Creditslower", (DbType)MySqlDbType.Int32, 4, Creditslower)
								 };

		   DbHelper.ExecuteDataset(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `Creditshigher`=?Creditshigher WHERE `groupid`>8 AND `radminid`=0 AND `Creditshigher`=?Creditslower", parms);
	   }

	   public DataTable GetUserGroupCreditsLowerAndHigher(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);

		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid`,`creditshigher`,`creditslower` FROM `" + BaseConfigs.GetTablePrefix + "usergroups`  WHERE `groupid`=?groupid LIMIT 1", parm).Tables[0];
	   }

	   public void UpdateUserGroup(UserGroupInfo __usergroupinfo, int Creditshigher, int Creditslower)
	   {
		   //IDataParameter[] prams =
		   //        {
		   //            DbHelper.MakeInParam("?Radminid",(DbType)MySqlDbType.Int32,4,(__usergroupinfo.Groupid == 1) ? 1 : __usergroupinfo.Radminid),
		   //            DbHelper.MakeInParam("?Grouptitle",(DbType)MySqlDbType.VarChar, 50, Utils.RemoveFontTag(__usergroupinfo.Grouptitle)),
		   //            DbHelper.MakeInParam("?Creditshigher",(DbType)MySqlDbType.Int32,4,Creditshigher),
		   //            DbHelper.MakeInParam("?Creditslower",(DbType)MySqlDbType.Int32,4,Creditslower),
		   //            DbHelper.MakeInParam("?Stars",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Stars),
		   //            DbHelper.MakeInParam("?Color",(DbType)MySqlDbType.String,7,__usergroupinfo.Color),
		   //            DbHelper.MakeInParam("?Groupavatar",(DbType)MySqlDbType.VarChar,60,__usergroupinfo.Groupavatar),
		   //            DbHelper.MakeInParam("?Readaccess",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Readaccess),
		   //            DbHelper.MakeInParam("?Allowvisit",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowvisit),
		   //            DbHelper.MakeInParam("?Allowpost",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpost),
		   //            DbHelper.MakeInParam("?Allowreply",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowreply),
		   //            DbHelper.MakeInParam("?Allowpostpoll",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpostpoll),
		   //            DbHelper.MakeInParam("?Allowdirectpost",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowdirectpost),
		   //            DbHelper.MakeInParam("?Allowgetattach",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowgetattach),
		   //            DbHelper.MakeInParam("?Allowpostattach",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpostattach),
		   //            DbHelper.MakeInParam("?Allowvote",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowvote),
		   //            DbHelper.MakeInParam("?Allowmultigroups",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowmultigroups),
		   //            DbHelper.MakeInParam("?Allowsearch",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsearch),
		   //            DbHelper.MakeInParam("?Allowavatar",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowavatar),
		   //            DbHelper.MakeInParam("?Allowcstatus",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowcstatus),
		   //            DbHelper.MakeInParam("?Allowuseblog",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowuseblog),
		   //            DbHelper.MakeInParam("?Allowinvisible",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowinvisible),
		   //            DbHelper.MakeInParam("?Allowtransfer",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowtransfer),
		   //            DbHelper.MakeInParam("?Allowsetreadperm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsetreadperm),
		   //            DbHelper.MakeInParam("?Allowsetattachperm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsetattachperm),
		   //            DbHelper.MakeInParam("?Allowhidecode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowhidecode),
		   //            DbHelper.MakeInParam("?Allowhtml",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowhtml),
		   //            DbHelper.MakeInParam("?Allowcusbbcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowcusbbcode),
		   //            DbHelper.MakeInParam("?Allownickname",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allownickname),
		   //            DbHelper.MakeInParam("?Allowsigbbcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsigbbcode),
		   //            DbHelper.MakeInParam("?Allowsigimgcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsigimgcode),
		   //            DbHelper.MakeInParam("?Allowviewpro",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowviewpro),
		   //            DbHelper.MakeInParam("?Allowviewstats",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowviewstats),
		   //            DbHelper.MakeInParam("?Disableperiodctrl",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Disableperiodctrl),
		   //            DbHelper.MakeInParam("?Reasonpm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Reasonpm),
		   //            DbHelper.MakeInParam("?Maxprice",(DbType)MySqlDbType.Int16,2,__usergroupinfo.Maxprice),
		   //            DbHelper.MakeInParam("?Maxpmnum",(DbType)MySqlDbType.Int16,2,__usergroupinfo.Maxpmnum),
		   //            DbHelper.MakeInParam("?Maxsigsize",(DbType)MySqlDbType.Int16,2,__usergroupinfo.Maxsigsize),
		   //            DbHelper.MakeInParam("?Maxattachsize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxattachsize),
		   //            DbHelper.MakeInParam("?Maxsizeperday",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxsizeperday),
		   //            DbHelper.MakeInParam("?Attachextensions",(DbType)MySqlDbType.String,100,__usergroupinfo.Attachextensions),
		   //            DbHelper.MakeInParam("?Groupid",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Groupid)

		   //        };

		   //string sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups`  SET `radminid`=?Radminid,`grouptitle`=?Grouptitle,`creditshigher`=?Creditshigher," +
		   //    "`creditslower`=?Creditslower,`stars`=?Stars,`color`=?Color,`groupavatar`=?Groupavatar,`readaccess`=?Readaccess, `allowvisit`=?Allowvisit,`allowpost`=?Allowpost," +
		   //    "`allowreply`=?Allowreply,`allowpostpoll`=?Allowpostpoll, `allowdirectpost`=?Allowdirectpost,`allowgetattach`=?Allowgetattach,`allowpostattach`=?Allowpostattach," +
		   //    "`allowvote`=?Allowvote,`allowmultigroups`=?Allowmultigroups,`allowsearch`=?Allowsearch,`allowavatar`=?Allowavatar,`allowcstatus`=?Allowcstatus," +
		   //    "`allowuseblog`=?Allowuseblog,`allowinvisible`=?Allowinvisible,`allowtransfer`=?Allowtransfer,`allowsetreadperm`=?Allowsetreadperm," +
		   //    "`allowsetattachperm`=?Allowsetattachperm,`allowhidecode`=?Allowhidecode,`allowhtml`=?Allowhtml,`allowcusbbcode`=?Allowcusbbcode,`allownickname`=?Allownickname," +
		   //    "`allowsigbbcode`=?Allowsigbbcode,`allowsigimgcode`=?Allowsigimgcode,`allowviewpro`=?Allowviewpro,`allowviewstats`=?Allowviewstats," +
		   //    "`disableperiodctrl`=?Disableperiodctrl,`reasonpm`=?Reasonpm,`maxprice`=?Maxprice,`maxpmnum`=?Maxpmnum,`maxsigsize`=?Maxsigsize,`maxattachsize`=?Maxattachsize," +
		   //    "`maxsizeperday`=?Maxsizeperday,`attachextensions`=?Attachextensions  WHERE `groupid`=?Groupid";

		   IDataParameter[] prams = 
					{
						DbHelper.MakeInParam("?Radminid",(DbType)MySqlDbType.Int32,4,(__usergroupinfo.Groupid == 1) ? 1 : __usergroupinfo.Radminid),
						DbHelper.MakeInParam("?Grouptitle",(DbType)MySqlDbType.VarChar,50, Utils.RemoveFontTag(__usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("?Creditshigher",(DbType)MySqlDbType.Int32,4,Creditshigher),
						DbHelper.MakeInParam("?Creditslower",(DbType)MySqlDbType.Int32,4,Creditslower),
						DbHelper.MakeInParam("?Stars",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Stars),
						DbHelper.MakeInParam("?Color",(DbType)MySqlDbType.VarChar,7,__usergroupinfo.Color),
						DbHelper.MakeInParam("?Groupavatar",(DbType)MySqlDbType.VarChar,60,__usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("?Readaccess",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Readaccess),
						DbHelper.MakeInParam("?Allowvisit",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("?Allowpost",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpost),
						DbHelper.MakeInParam("?Allowreply",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowreply),
						DbHelper.MakeInParam("?Allowpostpoll",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("?Allowdirectpost",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("?Allowgetattach",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("?Allowpostattach",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("?Allowvote",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowvote),
						DbHelper.MakeInParam("?Allowmultigroups",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("?Allowsearch",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("?Allowavatar",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("?Allowcstatus",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("?Allowuseblog",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("?Allowinvisible",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("?Allowtransfer",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("?Allowsetreadperm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("?Allowsetattachperm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("?Allowhidecode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("?Allowhtml",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("?Allowcusbbcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("?Allownickname",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allownickname),
						DbHelper.MakeInParam("?Allowsigbbcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("?Allowsigimgcode",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("?Allowviewpro",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("?Allowviewstats",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Allowviewstats),
						DbHelper.MakeInParam("?Disableperiodctrl",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Disableperiodctrl),
						DbHelper.MakeInParam("?Reasonpm",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("?Maxprice",(DbType)MySqlDbType.Int32,2,__usergroupinfo.Maxprice),
						DbHelper.MakeInParam("?Maxpmnum",(DbType)MySqlDbType.Int32,2,__usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("?Maxsigsize",(DbType)MySqlDbType.Int32,2,__usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("?Maxattachsize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("?Maxsizeperday",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("?Attachextensions",(DbType)MySqlDbType.VarChar,100,__usergroupinfo.Attachextensions),
						DbHelper.MakeInParam("?Maxspaceattachsize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxspaceattachsize),
						DbHelper.MakeInParam("?Maxspacephotosize",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("?Groupid",(DbType)MySqlDbType.Int32,4,__usergroupinfo.Groupid)

					};

		   string sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups`  SET `radminid`=?Radminid,`grouptitle`=?Grouptitle,`creditshigher`=?Creditshigher," +
			   "`creditslower`=?Creditslower,`stars`=?Stars,`color`=?Color,`groupavatar`=?Groupavatar,`readaccess`=?Readaccess, `allowvisit`=?Allowvisit,`allowpost`=?Allowpost," +
			   "`allowreply`=?Allowreply,`allowpostpoll`=?Allowpostpoll, `allowdirectpost`=?Allowdirectpost,`allowgetattach`=?Allowgetattach,`allowpostattach`=?Allowpostattach," +
			   "`allowvote`=?Allowvote,`allowmultigroups`=?Allowmultigroups,`allowsearch`=?Allowsearch,`allowavatar`=?Allowavatar,`allowcstatus`=?Allowcstatus," +
			   "`allowuseblog`=?Allowuseblog,`allowinvisible`=?Allowinvisible,`allowtransfer`=?Allowtransfer,`allowsetreadperm`=?Allowsetreadperm," +
			   "`allowsetattachperm`=?Allowsetattachperm,`allowhidecode`=?Allowhidecode,`allowhtml`=?Allowhtml,`allowcusbbcode`=?Allowcusbbcode,`allownickname`=?Allownickname," +
			   "`allowsigbbcode`=?Allowsigbbcode,`allowsigimgcode`=?Allowsigimgcode,`allowviewpro`=?Allowviewpro,`allowviewstats`=?Allowviewstats," +
			   "`disableperiodctrl`=?Disableperiodctrl,`reasonpm`=?Reasonpm,`maxprice`=?Maxprice,`maxpmnum`=?Maxpmnum,`maxsigsize`=?Maxsigsize,`maxattachsize`=?Maxattachsize," +
			   "`maxsizeperday`=?Maxsizeperday,`attachextensions`=?Attachextensions,`maxspaceattachsize`=?Maxspaceattachsize,`maxspacephotosize`=?Maxspacephotosize  WHERE `groupid`=?Groupid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
	   }


	   public void UpdateOnlineList(UserGroupInfo __usergroupinfo)
	   {
		   IDataParameter[] parms = {
									 // DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, __usergroupinfo.Groupid),
									 DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 50, Utils.RemoveFontTag(__usergroupinfo.Grouptitle))
								 };
		   string sqlstring = "UPDATE `" + BaseConfigs.GetTablePrefix + "onlinelist` SET `title`=?title WHERE `groupid`=" + __usergroupinfo.Groupid + "";

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
	   }

	   public bool IsSystemOrTemplateUserGroup(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT *  FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE (`system`=1 OR `type`=1) AND `groupid`=?groupid", parm).Tables[0].Rows.Count > 0;
	   }

	   public DataTable GetOthersCommonUserGroup(int exceptgroupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, exceptgroupid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`=0 And `groupid`>8 AND `groupid`<>?groupid", parm).Tables[0];
	   }

	   public string GetUserGroupRAdminId(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `radminid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE  `groupid`=?groupid", parm).Tables[0].Rows[0][0].ToString();
	   }

	   public void UpdateUserGroupLowerAndHigherToLimit(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `creditshigher`=-9999999 ,creditslower=9999999  WHERE `groupid`=?groupid", parm);
	   }

	   public void DeleteUserGroup(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=?groupid", parm);
	   }

	   public void DeleteAdminGroup(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "admingroups` WHERE `admingid`=?groupid", parm);
	   }

	   public void DeleteOnlineList(int groupid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "onlinelist` WHERE `groupid`=?groupid", parm);
	   }

	   public int GetMaxUserGroupId()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT IFNULL(MAX(groupid),0) FROM " + BaseConfigs.GetTablePrefix + "usergroups"), 0);
	   }



	   public bool DeletePaymentLog()
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` ");
			   return true;
		   }
		   catch
		   {
			   return false;
		   }
	   }

	   /// <summary>
	   /// 按指定条件删除日志
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public bool DeletePaymentLog(string condition)
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` WHERE " + condition);
			   return true;
		   }
		   catch
		   {
			   return false;
		   }

	   }

	   public DataTable GetPaymentLogList(int pagesize, int currentpage)
	   {
		   string sqlstring = "SELECT " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid as fid, " + BaseConfigs.GetTablePrefix + "topics.postdatetime as postdatatime, " + BaseConfigs.GetTablePrefix + "topics.poster as authorname, " + BaseConfigs.GetTablePrefix + "topics.title, " + BaseConfigs.GetTablePrefix + "users.username as username, " + BaseConfigs.GetTablePrefix + "paymentlog.id" + " FROM (" + BaseConfigs.GetTablePrefix + "paymentlog INNER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid) INNER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "paymentlog.uid = " + BaseConfigs.GetTablePrefix + "users.uid LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
	   }

	   public DataTable GetPaymentLogList(int pagesize, int currentpage, string condition)
	   {
		   string sqlstring = "SELECT " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid as fid, " + BaseConfigs.GetTablePrefix + "topics.postdatetime as postdatatime, " + BaseConfigs.GetTablePrefix + "topics.poster as authorname, " + BaseConfigs.GetTablePrefix + "topics.title as title, " + BaseConfigs.GetTablePrefix + "users.username as username " +
			   "FROM (" + BaseConfigs.GetTablePrefix + "paymentlog INNER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid) INNER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "paymentlog.uid = " + BaseConfigs.GetTablePrefix + "users.uid " +
			   "WHERE " + condition + " " +
			   "ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
	   }

	   /// <summary>
	   /// 得到积分交易日志记录数
	   /// </summary>
	   /// <returns></returns>
	   public int GetPaymentLogListCount()
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "paymentlog`").Tables[0].Rows[0][0].ToString());
	   }

	   /// <summary>
	   /// 得到指定查询条件下的积分交易日志数
	   /// </summary>
	   /// <param name="condition">查询条件</param>
	   /// <returns></returns>
	   public int GetPaymentLogListCount(string condition)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM `" + BaseConfigs.GetTablePrefix + "paymentlog` WHERE " + condition).Tables[0].Rows[0][0].ToString());
	   }

	   public void DeleteModeratorByFid(int fid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int32, 4, fid)
			};
		   string sql = "DELETE FROM `" + BaseConfigs.GetTablePrefix + "moderators` WHERE `fid`=?fid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }



	   public DataTable GetUidModeratorByFid(string fidlist)
	   {
		   string sql = "SELECT DISTINCT `uid` FROM `" + BaseConfigs.GetTablePrefix + "moderators` WHERE `fid` IN(" + fidlist + ")";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public void AddModerator(int uid, int fid, int displayorder, int inherited)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
				DbHelper.MakeInParam("?fid", (DbType)MySqlDbType.Int16, 2, fid),
				DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int16, 2, displayorder),
				DbHelper.MakeInParam("?inherited", (DbType)MySqlDbType.Int16, 2, inherited)
			};
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "moderators` (uid,fid,displayorder,inherited) VALUES(?uid,?fid,?displayorder,?inherited)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataTable GetModeratorInfo(string moderator)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, moderator.Trim())
			};

		   return DbHelper.ExecuteDataset(CommandType.Text, "Select `uid`,`groupid`  From `" + BaseConfigs.GetTablePrefix + "users` Where `groupid`<>7 AND `groupid`<>8 AND `username`=?username LIMIT 1", prams).Tables[0];
	   }

	   public void SetModerator(string moderator)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, moderator.Trim())
			};
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `adminid`=3,`groupid`=3  WHERE `username`=?username", prams);
	   }



	   public DataTable GetUidAdminIdByUsername(string username)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, username)
			};
		   string sql = "Select `uid`,`adminid` From `" + BaseConfigs.GetTablePrefix + "users` Where `username` = ?username LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public DataTable GetUidInModeratorsByUid(int currentfid, int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?currentfid", (DbType)MySqlDbType.Int32, 4, currentfid),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};
		   string sql = "Select `uid`  FROM `" + BaseConfigs.GetTablePrefix + "moderators` WHERE `fid`<>?currentfid AND `uid`=?uid LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void UpdateUserOnlineInfo(int groupid, int userid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid),
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "online` SET `groupid`=?groupid  WHERE `userid`=?userid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void UpdateUserOtherInfo(int groupid, int userid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?groupid", (DbType)MySqlDbType.Int32, 4, groupid),
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `groupid`=?groupid ,`adminid`=0  WHERE `uid`=?userid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   /// <summary>
	   /// 得到论坛中最后注册的用户ID和用户名
	   /// </summary>
	   /// <param name="lastuserid">输出参数：最后注册的用户ID</param>
	   /// <param name="lastusername">输出参数：最后注册的用户名</param>
	   /// <returns>存在返回true,不存在返回false</returns>
	   public bool GetLastUserInfo(out string lastuserid, out string lastusername)
	   {
		   lastuserid = "";
		   lastusername = "";

		   IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid`,`username` FROM `" + BaseConfigs.GetTablePrefix + "users` ORDER BY `uid` DESC LIMIT 1");
		   if (reader != null)
		   {
			   if (reader.Read())
			   {
				   lastuserid = reader["uid"].ToString();
				   lastusername = reader["username"].ToString().Trim();
				   reader.Close();
				   return true;
			   }
			   reader.Close();
		   }
		   return false;

	   }

	   public IDataReader GetTopUsers(int statcount, int lastuid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?lastuid", (DbType)MySqlDbType.Int32,4, lastuid),
		   };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid` > ?lastuid LIMIT " + statcount.ToString(), prams);
	   }

	   public void ResetUserDigestPosts(int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);
		   //DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET [digestposts]=(SELECT COUNT(tid) AS [digestposts] FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.[posterid] = `" + BaseConfigs.GetTablePrefix + "users`.[uid] AND [digest] > 0) WHERE `" + BaseConfigs.GetTablePrefix + "users`.[uid] = ?uid", parm);

		   int countdigestpost = Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT COUNT(tid) AS `digestposts` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`posterid` =" + userid + " AND `digest` > 0"), 0);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `digestposts`=" + countdigestpost + " WHERE `" + BaseConfigs.GetTablePrefix + "users`.`uid` = ?uid", parm);



	   }

	   public IDataReader GetUsers(int start_uid, int end_uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?start_uid", (DbType)MySqlDbType.Int32, 4, start_uid),
									 DbHelper.MakeInParam("?end_uid", (DbType)MySqlDbType.Int32, 4, end_uid)
								 };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid` >= ?start_uid AND `uid`<=?end_uid", prams);
	   }

	   public void UpdateUserPostCount(int postcount, int userid)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?postcount", (DbType)MySqlDbType.Int32, 4, postcount),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `posts`=?postcount WHERE `" + BaseConfigs.GetTablePrefix + "users`.`uid` = ?userid", parms);
	   }


	   /// <summary>
	   /// 获得所有版主列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetModeratorList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM `{0}moderators`", BaseConfigs.GetTablePrefix)).Tables[0];
	   }


	   /// <summary>
	   /// 获得全部在线用户数
	   /// </summary>
	   /// <returns></returns>
	   public int GetOnlineAllUserCount()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(olid) FROM `" + BaseConfigs.GetTablePrefix + "online`"), 1);
	   }

	   /// <summary>
	   /// 创建在线表
	   /// </summary>
	   /// <returns></returns>
	   public int CreateOnlineTable()
	   {
		   try
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "DROP TABLE IF EXISTS `" + BaseConfigs.GetTablePrefix + "online`");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE TABLE `" + BaseConfigs.GetTablePrefix + "online` (`olid` int(11) NOT NULL auto_increment,`userid` int(11) NOT NULL default '-1',`ip` varchar(15) NOT NULL default '0.0.0.0', `username` varchar(20) NOT NULL default '',`nickname` varchar(20) NOT NULL default '',`password` varchar(32) NOT NULL default '',`groupid` smallint(6) NOT NULL default '0',`olimg` varchar(80) NOT NULL default '', `adminid` smallint(6) NOT NULL default '0',`invisible` smallint(6) NOT NULL default '0',`action` smallint(6) NOT NULL default '0', `lastactivity` smallint(6) NOT NULL default '0', `lastposttime` datetime NOT NULL default '1900-01-01 00:00:00',`lastpostpmtime` datetime NOT NULL default '1900-01-01 00:00:00', `lastsearchtime` datetime NOT NULL default '1900-01-01 00:00:00', `lastupdatetime` datetime NOT NULL,`forumid` int(11) NOT NULL default '0',`forumname` varchar(50) NOT NULL default '', `titleid` int(11) NOT NULL default '0',`title` varchar(80) NOT NULL default '', `verifycode` varchar(10) NOT NULL default '',PRIMARY KEY(`olid`), KEY `forum` (`userid`,`forumid`,`invisible`),KEY `forumid` (`forumid`), KEY `invisible` (`userid`,`invisible`),KEY `ip` (`userid`,`ip`),KEY `password` (`userid`,`password`) ) ENGINE=MEMORY AUTO_INCREMENT=1 DEFAULT CHARSET=gbk");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "ALTER TABLE `" + BaseConfigs.GetTablePrefix + "online` ADD PRIMARY KEY ( `olid` ) ");
           
			   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE INDEX `forum` ON `" + BaseConfigs.GetTablePrefix + "online`(`userid`, `forumid`, `invisible`);");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE INDEX `invisible` ON `" + BaseConfigs.GetTablePrefix + "online`(`userid`, `invisible`)");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE INDEX `forumid` ON `" + BaseConfigs.GetTablePrefix + "online`(`forumid`)");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE INDEX `password` ON `" + BaseConfigs.GetTablePrefix + "online`(`userid`, `password`)");
			   DbHelper.ExecuteNonQuery(CommandType.Text, "CREATE INDEX `ip` ON `" + BaseConfigs.GetTablePrefix + "online`(`userid`, `ip`)");
                
                
			   return 1;
		   }
		   catch
		   {
			   return -1;
		   }
	   }

	   /// <summary>
	   /// 取得在线表最后一条记录的tickcount字段
	   /// </summary>
	   /// <returns></returns>
	   //public int GetLastTickCount()
	   //{
	   //    return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT `tickcount` FROM `" + BaseConfigs.GetTablePrefix + "online` ORDER BY `olid` DESC LIMIT 1"), System.Environment.TickCount);

            
	   //}

	   /// <summary>
	   /// 获得在线注册用户总数量
	   /// </summary>
	   /// <returns>用户数量</returns>
	   public int GetOnlineUserCount()
	   {

		   return int.Parse(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(olid) FROM `" + BaseConfigs.GetTablePrefix + "online` WHERE `userid`>0").Tables[0].Rows[0][0].ToString());

	   }

	   /// <summary>
	   /// 获得版块在线用户列表
	   /// </summary>
	   /// <param name="forumid">版块Id</param>
	   /// <returns></returns>
	   public DataTable GetForumOnlineUserListTable(int forumid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM `{0}online` WHERE `forumid`={1}", BaseConfigs.GetTablePrefix, forumid)).Tables[0];
	   }

	   /// <summary>
	   /// 获得全部在线用户列表
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetOnlineUserListTable()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "online`").Tables[0];
	   }

	   /// <summary>
	   /// 获得版块在线用户列表
	   /// </summary>
	   /// <param name="forumid">版块Id</param>
	   /// <returns></returns>
	   public IDataReader GetForumOnlineUserList(int forumid)
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM `{0}online` WHERE `forumid`={1}", BaseConfigs.GetTablePrefix, forumid.ToString()));
	   }

	   /// <summary>
	   /// 获得全部在线用户列表
	   /// </summary>
	   /// <returns></returns>
	   public IDataReader GetOnlineUserList()
	   {
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "online`");
	   }

	   /// <summary>
	   /// 返回在线用户图例
	   /// </summary>
	   /// <returns></returns>
	   public DataTable GetOnlineGroupIconTable()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `groupid`, `displayorder`, `title`, `img` FROM `" + BaseConfigs.GetTablePrefix + "onlinelist` WHERE `img` <> '' ORDER BY `displayorder`").Tables[0];
	   }

	   /// <summary>
	   /// 获得指定用户的详细信息
	   /// </summary>
	   /// <param name="userid">在线用户ID</param>
	   /// <param name="password">用户密码</param>
	   /// <returns>用户的详细信息</returns>
	   public DataTable GetOnlineUser(int userid, string password)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
									 DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 32, password)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM `{0}online` WHERE `userid`=?userid AND `password`=?password LIMIT 1", BaseConfigs.GetTablePrefix), parms).Tables[0];
	   }

	   /// <summary>
	   /// 获得指定用户的详细信息
	   /// </summary>
	   /// <param name="userid">在线用户ID</param>
	   /// <param name="ip">IP</param>
	   /// <returns></returns>
	   public DataTable GetOnlineUserByIP(int userid, string ip)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
									 DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarChar, 15, ip)
								 };
		   return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM `{0}online` WHERE `userid`=?userid AND `ip`=?ip LIMIT 1", BaseConfigs.GetTablePrefix), parms).Tables[0];
	   }

	   /// <summary>
	   /// 检查在线用户验证码是否有效
	   /// </summary>
	   /// <param name="olid">在组用户ID</param>
	   /// <param name="verifycode">验证码</param>
	   /// <returns>在组用户ID</returns>
	   public bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?olid", (DbType)MySqlDbType.Int32, 4, olid),
									 DbHelper.MakeInParam("?verifycode", (DbType)MySqlDbType.VarChar, 10, verifycode)
								 };
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT `olid` FROM `{0}online` WHERE `olid`=?olid and `verifycode`=?verifycode LIMIT 1", BaseConfigs.GetTablePrefix), parms).Tables[0];
		   parms[1].Value = newverifycode;
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `verifycode`=?verifycode WHERE `olid`=?olid", BaseConfigs.GetTablePrefix), parms);
		   return dt.Rows.Count > 0;
	   }

	   /// <summary>
	   /// 设置用户在线状态
	   /// </summary>
	   /// <param name="uid">用户Id</param>
	   /// <param name="onlinestate">在线状态，１在线</param>
	   /// <returns></returns>
	   public int SetUserOnlineState(int uid, int onlinestate)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}users` SET `onlinestate`={1},`lastactivity`=now() WHERE `uid`={2}", BaseConfigs.GetTablePrefix, onlinestate, uid));
	   }

	   /// <summary>
	   /// 删除符合条件的一个或多个用户信息
	   /// </summary>
	   /// <returns>删除行数</returns>
	   public int DeleteRowsByIP(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.VarChar,15,ip)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `onlinestate`=0,`lastactivity`=now() WHERE `uid` IN (SELECT `userid` FROM `" + BaseConfigs.GetTablePrefix + "online` WHERE `userid`>0 AND `ip`=?ip)", prams);
		   if (ip != "0.0.0.0")
		   {
			   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "online` WHERE `userid`=-1 AND `ip`=?ip", prams);
		   }
		   return 0;
	   }

	   /// <summary>
	   /// 删除在线表中指定在线id的行
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   /// <returns></returns>
	   public int DeleteRows(int olid)
	   {
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "online` WHERE `olid`=" + olid.ToString());
	   }

	   /// <summary>
	   /// 更新用户的当前动作及相关信息
	   /// </summary>
	   /// <param name="olid">在线列表id</param>
	   /// <param name="action">动作</param>
	   /// <param name="inid">所在位置代码</param>
	   public void UpdateAction(int olid, int action, int inid)
	   {
		   IDataParameter[] prams = {
									 //DbHelper.MakeInParam("?tickcount",(DbType)MySqlDbType.Int32,4,System.Environment.TickCount),
									 DbHelper.MakeInParam("?lastupdatetime",(DbType)MySqlDbType.Datetime,8,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
									 DbHelper.MakeInParam("?action",(DbType)MySqlDbType.Int16,2,action),
									 DbHelper.MakeInParam("?forumid",(DbType)MySqlDbType.Int32,4,inid),
									 DbHelper.MakeInParam("?forumname",(DbType)MySqlDbType.VarChar,100,""),
									 DbHelper.MakeInParam("?titleid",(DbType)MySqlDbType.Int32,4,inid),
									 DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarChar,160,""),
									 DbHelper.MakeInParam("?olid",(DbType)MySqlDbType.Int32,4,olid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "online` SET `lastactivity`=`action`,`action`=?action,`lastupdatetime`=?lastupdatetime,`forumid`=?forumid,`forumname`=?forumname,`titleid`=?titleid,`title`=?title WHERE `olid`=?olid", prams);
	   }

	   /// <summary>
	   /// 更新用户的当前动作及相关信息
	   /// </summary>
	   /// <param name="olid">在线列表id</param>
	   /// <param name="action">动作id</param>
	   /// <param name="fid">版块id</param>
	   /// <param name="forumname">版块名</param>
	   /// <param name="tid">主题id</param>
	   /// <param name="topictitle">主题名</param>
	   public void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
	   {
		   IDataParameter[] prams = {
									 //DbHelper.MakeInParam("?tickcount",(DbType)MySqlDbType.Int32,4,System.Environment.TickCount),
									 DbHelper.MakeInParam("?lastupdatetime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
									 DbHelper.MakeInParam("?action",(DbType)MySqlDbType.Int16,2,action),
									 DbHelper.MakeInParam("?forumid",(DbType)MySqlDbType.Int32,4,fid),
									 DbHelper.MakeInParam("?forumname",(DbType)MySqlDbType.VarChar,100,forumname),
									 DbHelper.MakeInParam("?titleid",(DbType)MySqlDbType.Int32,4,tid),
									 DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarChar,160,topictitle),
									 DbHelper.MakeInParam("?olid",(DbType)MySqlDbType.Int32,4,olid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "online` SET `lastactivity`=`action`,`action`=?action,`lastupdatetime`=?lastupdatetime,`forumid`=?forumid,`forumname`=?forumname,`titleid`=?titleid,`title`=?title WHERE `olid`=?olid", prams);
	   }

	   /// <summary>
	   /// 更新用户最后活动时间
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   public void UpdateLastTime(int olid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?lastupdatetime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
									 //DbHelper.MakeInParam("?tickcount",(DbType)MySqlDbType.Int32,4,System.Environment.TickCount),
									 DbHelper.MakeInParam("?olid",(DbType)MySqlDbType.Int32,4,olid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "online` SET `lastupdatetime`=?lastupdatetime WHERE `olid`=?olid", prams);
	   }

	   /// <summary>
	   /// 更新用户最后发帖时间
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   public void UpdatePostTime(int olid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `lastposttime`=now() WHERE `olid`={1}", BaseConfigs.GetTablePrefix, olid.ToString()));
	   }

	   /// <summary>
	   /// 更新用户最后发短消息时间
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   public void UpdatePostPMTime(int olid)
	   {

		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `lastpostpmtime`=now() WHERE `olid`={1}", BaseConfigs.GetTablePrefix, olid.ToString()));

	   }

	   /// <summary>
	   /// 更新在线表中指定用户是否隐身
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   /// <param name="invisible">是否隐身</param>
	   public void UpdateInvisible(int olid, int invisible)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `invisible`={1} WHERE `olid`={2}", BaseConfigs.GetTablePrefix, invisible.ToString(), olid.ToString()));
	   }

	   /// <summary>
	   /// 更新在线表中指定用户的用户密码
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   /// <param name="password">用户密码</param>
	   public void UpdatePassword(int olid, string password)
	   {

		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?olid",(DbType)MySqlDbType.Int32,4,olid),
									 DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32,password)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `password`=now() WHERE `olid`=?olid", BaseConfigs.GetTablePrefix), prams);
	   }

	   /// <summary>
	   /// 更新用户IP地址
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   /// <param name="ip">ip地址</param>
	   public void UpdateIP(int olid, string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.VarChar,15,ip),
									 DbHelper.MakeInParam("?olid",(DbType)MySqlDbType.Int32,4,olid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `ip`=?ip WHERE `olid`=?olid", BaseConfigs.GetTablePrefix), prams);

	   }

	   /// <summary>
	   /// 更新用户最后搜索时间
	   /// </summary>
	   /// <param name="olid">在线id</param>
	   public void UpdateSearchTime(int olid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `lastsearchtime`=now() WHERE `olid`={1}", BaseConfigs.GetTablePrefix, olid.ToString()));
	   }

	   /// <summary>
	   /// 更新用户的用户组
	   /// </summary>
	   /// <param name="userid">用户ID</param>
	   /// <param name="groupid">组名</param>
	   public void UpdateGroupid(int userid, int groupid)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}online` SET `groupid`={1} WHERE `userid`={2}", BaseConfigs.GetTablePrefix, groupid.ToString(), userid.ToString()));
	   }


	   /// <summary>
	   /// 获得指定ID的短消息的内容
	   /// </summary>
	   /// <param name="pmid">短消息pmid</param>
	   /// <returns>短消息内容</returns>
	   public IDataReader GetPrivateMessageInfo(int pmid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?pmid", (DbType)MySqlDbType.Int32,4, pmid),
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `pmid`=?pmid LIMIT 1", prams);
	   }

	   /// <summary>
	   /// 获得指定用户的短信息列表
	   /// </summary>
	   /// <param name="userid">用户ID</param>
	   /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
	   /// <param name="pagesize">每页显示短信息数</param>
	   /// <param name="pageindex">当前要显示的页数</param>
	   /// <param name="strwhere">筛选条件</param>
	   /// <returns>短信息列表</returns>
	   public IDataReader GetPrivateMessageList(int userid, int folder, int pagesize, int pageindex, int inttype)
	   {


		   #region 存储过程转sql语句 	getpmlist
		   //string sqlGetPMList = string.Empty;
		   //string msgformORtoID = "msgtoid";
		   //if (folder == 1 || folder == 2)
		   //{
		   //    msgformORtoID = "msgfromid";
		   //}
		   //IDataReader reader;

		   //if (strwhere != "")
		   //{
		   //    IDataParameter[] prams = {
		   //                           DbHelper.MakeInParam("?userid",(DbType)MySqlDbType.Int32,4,userid),
		   //                           DbHelper.MakeInParam("?folder",(DbType)MySqlDbType.Int32,4,folder),
		   //                          // DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32,4,pagesize),
		   //                         //  DbHelper.MakeInParam("?pageindex",(DbType)MySqlDbType.Int32,4,pageindex),
		   //                           DbHelper.MakeInParam("?strwhere",(DbType)MySqlDbType.VarChar,500,strwhere)

		   //                       };
		   //    sqlGetPMList = "SELECT *  FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `" + msgformORtoID + "`=?userid AND `folder`=?folder AND ?strwhere ORDER BY `pmid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
		   //    reader = DbHelper.ExecuteReader(CommandType.Text, sqlGetPMList, prams);
		   //}
		   //else
		   //{
		   //    IDataParameter[] prams = {
		   //              // DbHelper.MakeInParam("?pagesize1", (DbType)MySqlDbType.Int32,4,pagesize),
		   //                           DbHelper.MakeInParam("?userid",(DbType)MySqlDbType.Int32,4,userid),
		   //                           DbHelper.MakeInParam("?folder",(DbType)MySqlDbType.Int32,4,folder),

		   //                         //  DbHelper.MakeInParam("?pageindex",(DbType)MySqlDbType.Int32,4,pageindex),
		   //                         //  DbHelper.MakeInParam("?strwhere",(DbType)MySqlDbType.VarChar,500,strwhere)

		   //                       };



		   //    sqlGetPMList = "SELECT *  FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `" + msgformORtoID + "`=?userid AND `folder`=?folder ORDER BY `pmid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();
		   //    reader = DbHelper.ExecuteReader(CommandType.Text, sqlGetPMList, prams);
		   //}



		   //return reader;

		   #endregion

		   string strwhere = "";
		   if (inttype == 1)
		   {
			   strwhere = "`new`=1";
		   }

		   IDataParameter[] prams = {
                         
									 DbHelper.MakeInParam("?strwhere",(DbType)MySqlDbType.VarChar,500,strwhere)

								 };

		   string strsql;
		   string msgformortoid = "msgtoid";
		   if (folder == 1 || folder == 2)
		   {

			   msgformortoid = "msgfromid";
		   }

		   //if (pageindex == 1)
		   //{
		   if (strwhere != "")
		   {
			   strsql = "SELECT `pmid`,`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `" + msgformortoid + "`= " + userid + "  AND `folder`= " + folder + "  AND  " + strwhere + " ORDER BY `pmid` DESC LIMIT " + ((pageindex- 1) * pagesize).ToString()+ "," + pagesize;
		   }
		   else
		   {
			   strsql = "SELECT `pmid`,`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `" + msgformortoid + "`= " + userid + "  AND `folder`= " + folder + "  ORDER BY `pmid` DESC LIMIT " + ((pageindex - 1) * pagesize).ToString() + "," + pagesize;

		   }


		   //}
		   //else
		   //{
		   //    strsql = "SELECT `pmid`,`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `pmid` < (SELECT MIN(`pmid`) FROM (SELECT `pmid` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `" + msgformortoid + "`=" + userid + "  AND `folder`=" + folder + " ORDER BY `pmid` DESC) AS tblTmp LIMIT 0,"+(pageindex - 1) * pagesize+ ") AND `" + msgformortoid + "`=" + userid + "   ORDER BY `pmid` DESC LIMIT 0,"+pagesize;
		   //    if (strwhere != "")
		   //    {
		   //        strsql = "SELECT `pmid`,`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `pmid` < (SELECT MIN(`pmid`) FROM (SELECT `pmid` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `" + msgformortoid + "`=" + userid + "  AND `folder`= " + folder + "  AND  " + strwhere + " ORDER BY `pmid` DESC) AS tblTmp LIMIT 0," + (pageindex - 1) * pagesize + ") AND `" + msgformortoid + "`= " + userid + "  AND `folder`= " + folder + "  AND  " + strwhere + "  ORDER BY `pmid` DESC LIMIT 0," + pagesize;
		   //    }

		   //}


		   return DbHelper.ExecuteReader(CommandType.Text, strsql,prams);
	   }

	   /// <summary>
	   /// 得到当用户的短消息数量
	   /// </summary>
	   /// <param name="userid">用户ID</param>
	   /// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
	   /// <param name="state">短消息状态(0:已读短消息、1:未读短消息、-1:全部短消息)</param>
	   /// <returns>短消息数量</returns>
	   public int GetPrivateMessageCount(int userid, int folder, int state)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?userid",(DbType)MySqlDbType.Int32,4,userid),
									 DbHelper.MakeInParam("?folder",(DbType)MySqlDbType.Int32,4,folder),
									 DbHelper.MakeInParam("?state",(DbType)MySqlDbType.Int32,4,state)
								 };



		   string sqlGetPMCount = string.Empty;
		   if (folder == -1)
		   {
			   sqlGetPMCount = "SELECT COUNT(pmid) AS `pmcount` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE (`msgtoid`=?userid AND `folder`=0) OR (`msgfromid`=?userid AND `folder` = 1)  OR (`msgfromid` = ?userid AND `folder` = 2)";
		   }
		   else
		   {
			   if (folder == 0)
			   {
				   if (state == -1)
				   {
					   sqlGetPMCount = "SELECT COUNT(pmid) AS `pmcount` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `msgtoid`=?userid AND `folder`=?folder";
				   }
				   else
				   {
					   sqlGetPMCount = "SELECT COUNT(pmid) AS `pmcount` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `msgtoid`=?userid AND `folder`=?folder AND `new`=?state";
				   }
			   }
			   else
			   {
				   if (state == -1)
				   {
					   sqlGetPMCount = "SELECT COUNT(pmid) AS `pmcount` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `msgfromid`=?userid AND `folder`=?folder";
				   }
				   else
				   {
					   sqlGetPMCount = "SELECT COUNT(pmid) AS `pmcount` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `msgfromid`=?userid AND `folder`=?folder AND `new`=?state";
				   }
			   }
		   }


		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sqlGetPMCount, prams).ToString(), 0);

	   }

	   /// <summary>
	   /// 创建短消息
	   /// </summary>
	   /// <param name="__privatemessageinfo">短消息内容</param>
	   /// <param name="savetosentbox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
	   /// <returns>短消息在数据库中的pmid</returns>
	   public int CreatePrivateMessage(PrivateMessageInfo __privatemessageinfo, int savetosentbox)
	   {
		   /*
						if (__privatemessageinfo.Folder != 0)
						{
							__privatemessageinfo.Msgfrom = __privatemessageinfo.Msgto;
							//prams[1].Value = prams[3].Value;
						}
						else
						{
							IDataParameter[] prams1 = {
														 DbHelper.MakeInParam("?msgtoid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgtoid),
													  };
							DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET [newpmcount]=iif(([newpmcount] is null),0,[newpmcount])+1,[newpm] = 1 WHERE [uid]=?msgtoid", prams1);
						}
						IDataParameter[] prams2 = {
												   //DbHelper.MakeInParam("?pmid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Pmid),
												   DbHelper.MakeInParam("?msgfrom",(DbType)MySqlDbType.VarChar,20,__privatemessageinfo.Msgfrom),
												   DbHelper.MakeInParam("?msgfromid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgfromid),
												   DbHelper.MakeInParam("?msgto",(DbType)MySqlDbType.VarChar,20,__privatemessageinfo.Msgto),
												   DbHelper.MakeInParam("?msgtoid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgtoid),
												   DbHelper.MakeInParam("?folder",(DbType)MySqlDbType.Int16,2,__privatemessageinfo.Folder),
												   DbHelper.MakeInParam("?new",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.New),
												   DbHelper.MakeInParam("?subject",(DbType)MySqlDbType.VarChar,80,__privatemessageinfo.Subject),
												   DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Date,8,DateTime.Parse(__privatemessageinfo.Postdatetime)),
												   DbHelper.MakeInParam("?message",(DbType)MySqlDbType.VarChar,0,__privatemessageinfo.Message),
												   //DbHelper.MakeInParam("?savetosentbox",(DbType)MySqlDbType.Int32,4,savetosentbox)
											   };
						createpm

						string strSQL = "";
						string pmid = "";
					 //   MySqlConnection conn = new MySqlConnection(BaseConfigs.GetDBConnectString);
					 //   conn.Open();
					  //  using (MySqlTransaction trans = conn.BeginTransaction())
					  //  {
						//    try
						//    {



								int currentpid = 0;
								MySqlConnection con = new MySqlConnection(BaseConfigs.GetDBConnectString);
								strSQL = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "pms` " +
									"([msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message]) " +
									"VALUES " +
									"(?msgfrom,?msgfromid,?msgto,?msgtoid,?folder,?new,?subject,?postdatetime,?message)";
								DbHelper.ExecuteNonQuery(con, CommandType.Text, out currentpid, strSQL, prams2);

								pmid = currentpid.ToString();

								if ((savetosentbox == 1) && (__privatemessageinfo.Folder == 0))
								{
									IDataParameter[] prams3 = {

																  DbHelper.MakeInParam("?msgfrom",(DbType)MySqlDbType.VarChar,20,__privatemessageinfo.Msgfrom),
																  DbHelper.MakeInParam("?msgfromid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgfromid),
																  DbHelper.MakeInParam("?msgto",(DbType)MySqlDbType.VarChar,20,__privatemessageinfo.Msgto),
																  DbHelper.MakeInParam("?msgtoid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgtoid),
																  DbHelper.MakeInParam("?folder",(DbType)MySqlDbType.Int16,2,1),
																  DbHelper.MakeInParam("?new",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.New),
																  DbHelper.MakeInParam("?subject",(DbType)MySqlDbType.VarChar,80,__privatemessageinfo.Subject),
																  DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Date,8,__privatemessageinfo.Postdatetime),
																  DbHelper.MakeInParam("?message",(DbType)MySqlDbType.VarChar,0,__privatemessageinfo.Message),

									};
									DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "pms` " +
										"([msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message]) " +
										"VALUES " +
										"(?msgfrom,?msgfromid,?msgto,?msgtoid,?folder,?new,?subject,?postdatetime,?message)", prams3);
								}
							//    trans.Commit();
						   // }
						   // catch (Exception ex)
						   // {
								// transaction failed
						   //     trans.Rollback();
								// log exception details . . .
						   //     throw ex;
						  //  }
					 //   }
					 //   conn.Close();

		   #endregion
						return Utils.StrToInt(pmid, -1);
			 */



		   if (__privatemessageinfo.Folder != 0)
		   {
			   __privatemessageinfo.Msgfrom = __privatemessageinfo.Msgto;
		   }
		   else
		   {
			   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `newpmcount`= ABS(IF(ISNULL(`newpmcount`),0,newpmcount)*1)+1,`newpm` = 1 WHERE `uid`=" + __privatemessageinfo.Msgtoid + "");
		   }

		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?msgfrom",(DbType)MySqlDbType.VarChar,20,__privatemessageinfo.Msgfrom),
									 DbHelper.MakeInParam("?msgfromid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgfromid),
									 DbHelper.MakeInParam("?msgto",(DbType)MySqlDbType.VarChar,20,__privatemessageinfo.Msgto),
									 DbHelper.MakeInParam("?msgtoid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Msgtoid),
									 DbHelper.MakeInParam("?folder",(DbType)MySqlDbType.Int16,2,__privatemessageinfo.Folder),
									 DbHelper.MakeInParam("?new",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.New),
									 DbHelper.MakeInParam("?subject",(DbType)MySqlDbType.VarString,80,__privatemessageinfo.Subject),
									 DbHelper.MakeInParam("?postdatetime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(__privatemessageinfo.Postdatetime)),
									 DbHelper.MakeInParam("?message",(DbType)MySqlDbType.VarChar,0,__privatemessageinfo.Message),
									 DbHelper.MakeInParam("?savetosentbox",(DbType)SqlDbType.Int,4,savetosentbox),
									 DbHelper.MakeInParam("?pmid",(DbType)MySqlDbType.Int32,4,__privatemessageinfo.Pmid)
								 };

		   string sql1 = "insert into `" + BaseConfigs.GetTablePrefix + "pms`(`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message`) VALUES(?msgfrom,?msgfromid,?msgto,?msgtoid,?folder,?new,?subject,?postdatetime,?message)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql1, prams);

		   int s = Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "select pmid from `" + BaseConfigs.GetTablePrefix + "pms` order by pmid desc LIMIT 1").Tables[0].Rows[0][0].ToString(), -1);






		   if ((savetosentbox == 1) && (__privatemessageinfo.Folder == 0))
		   {


			   DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "pms` " +
				   "(`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message`) " +
				   "VALUES " +
				   "(?msgfrom,?msgfromid,?msgto,?msgtoid,1,?new,?subject,?postdatetime,?message)", prams);
		   }




		   return s;
	   }

	   /// <summary>
	   /// 删除指定用户的短信息
	   /// </summary>
	   /// <param name="userid">用户ID</param>
	   /// <param name="pmitemid">要删除的短信息列表(数组)</param>
	   /// <returns>删除记录数</returns>
	   public int DeletePrivateMessages(int userid, string pmidlist)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32,4, userid)
								 };

		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `pmid` IN (" + pmidlist + ") AND (`msgtoid` = ?userid OR `msgfromid` = ?userid)", prams);

	   }

	   /// <summary>
	   /// 获得新短消息数
	   /// </summary>
	   /// <returns></returns>
	   public int GetNewPMCount(int userid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32,4, userid)
								 };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`pmid`) AS `pmcount` FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `new` = 1 AND `folder` = 0 AND `msgtoid` = ?userid", prams), 0);
	   }

	   /// <summary>
	   /// 删除指定用户的一条短消息
	   /// </summary>
	   /// <param name="userid">用户Ｉｄ</param>
	   /// <param name="pmid">ＰＭＩＤ</param>
	   /// <returns></returns>
	   public int DeletePrivateMessage(int userid, int pmid)
	   {
		   IDataParameter[] prams = {     DbHelper.MakeInParam("?pmid", (DbType)MySqlDbType.Int32,4, pmid),
									 DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32,4, userid)

								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM `" + BaseConfigs.GetTablePrefix + "pms` WHERE `pmid`=?pmid AND (`msgtoid` = ?userid OR `msgfromid` = ?userid)", prams);

	   }

	   /// <summary>
	   /// 设置短信息状态
	   /// </summary>
	   /// <param name="pmid">短信息ID</param>
	   /// <param name="state">状态值</param>
	   /// <returns>更新记录数</returns>
	   public int SetPrivateMessageState(int pmid, byte state)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?state",(DbType)MySqlDbType.Int16,1,state),
									 DbHelper.MakeInParam("?pmid", (DbType)MySqlDbType.Int32,1,pmid)

								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "pms` SET `new`=?state WHERE `pmid`=?pmid", prams);

	   }

	   public int GetRAdminIdByGroup(int groupid)
	   {
		   return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT `radminid` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=" + groupid + " LIMIT 1").Tables[0].Rows[0][0].ToString());
	   }

	   public string GetUserGroupsStr()
	   {
		   return "SELECT `groupid`, `grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` ORDER BY `groupid`";
	   }


	   public DataTable GetUserNameListByGroupid(string groupidlist)
	   {
		   string sql = "SELECT `uid` ,`username`  From `" + BaseConfigs.GetTablePrefix + "users` WHERE `groupid` IN(" + groupidlist + ")";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetUserNameByUid(int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};
		   string sql = "SELECT `username` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
	   }

	   public void ResetPasswordUid(string password, int uid)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 32, password),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `password`=?password WHERE `uid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public void SendPMToUser(string msgfrom, int msgfromid, string msgto, int msgtoid, int folder, string subject, DateTime postdatetime, string message)
	   {
		   IDataParameter[] prams =
			{
				DbHelper.MakeInParam("?msgfrom", (DbType)MySqlDbType.VarChar,50, msgfrom),
				DbHelper.MakeInParam("?msgfromid", (DbType)MySqlDbType.Int32, 4, msgfromid),
				DbHelper.MakeInParam("?msgto", (DbType)MySqlDbType.String,50, msgto),
				DbHelper.MakeInParam("?msgtoid", (DbType)MySqlDbType.Int32, 4, msgtoid),
				DbHelper.MakeInParam("?folder", (DbType)MySqlDbType.Int16, 2, folder),
				DbHelper.MakeInParam("?subject", (DbType)MySqlDbType.String,60, subject),
				DbHelper.MakeInParam("?postdatetime", (DbType)MySqlDbType.Datetime,8, postdatetime),
				DbHelper.MakeInParam("?message",(DbType)MySqlDbType.String, 0,message)
			};
		   string sql = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "pms` (msgfrom,msgfromid,msgto,msgtoid,folder,new,subject,postdatetime,message) " +
			   "VALUES (?msgfrom,?msgfromid,?msgto,?msgtoid,?folder,1,?subject,?postdatetime,?message)";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);


		   IDataParameter[] prams1 =
			{

				DbHelper.MakeInParam("?msgtoid", (DbType)MySqlDbType.Int32, 4, msgtoid)

			};
		   sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `newpmcount`=`newpmcount`+1  WHERE `uid` =?msgtoid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams1);
	   }

	   public string GetSystemGroupInfoSql()
	   {
		   return "Select * From `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`<=8 Order By `groupid`";
	   }

	   public void UpdateUserCredits(int uid, string credits)
	   {
		   IDataParameter[] prams_credits = {
											 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
										 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}users` SET `credits` = {1} WHERE `uid`=?uid", BaseConfigs.GetTablePrefix, credits), prams_credits);
	   }

	   public void UpdateUserGroup(int uid, int groupid)
	   {
		   IDataParameter[] prams_credits = {
											 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
										 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}users` SET `groupid` = {1} WHERE `uid`=?uid", BaseConfigs.GetTablePrefix, groupid), prams_credits);

	   }

	   public bool CheckUserCreditsIsEnough(int uid, float[] values)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
									 DbHelper.MakeInParam("?extcredits1", (DbType)MySqlDbType.Decimal, 8, values[0]),
									 DbHelper.MakeInParam("?extcredits2", (DbType)MySqlDbType.Decimal, 8, values[1]),
									 DbHelper.MakeInParam("?extcredits3", (DbType)MySqlDbType.Decimal, 8, values[2]),
									 DbHelper.MakeInParam("?extcredits4", (DbType)MySqlDbType.Decimal, 8, values[3]),
									 DbHelper.MakeInParam("?extcredits5", (DbType)MySqlDbType.Decimal, 8, values[4]),
									 DbHelper.MakeInParam("?extcredits6", (DbType)MySqlDbType.Decimal, 8, values[5]),
									 DbHelper.MakeInParam("?extcredits7", (DbType)MySqlDbType.Decimal, 8, values[6]),
									 DbHelper.MakeInParam("?extcredits8", (DbType)MySqlDbType.Decimal, 8, values[7])
								 };
		   string CommandText = "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid AND"
			   + "	`extcredits1`>= IF(?extcredits1<0 ,ABS(?extcredits1),`extcredits1`) AND "
			   + "	`extcredits2`>= IF(?extcredits2<0 ,ABS(?extcredits2),`extcredits2`) AND "
			   + "	`extcredits3`>= IF(?extcredits3<0 ,ABS(?extcredits3),`extcredits3`) AND "
			   + "	`extcredits4`>= IF(?extcredits4<0 ,ABS(?extcredits4),`extcredits4`) AND "
			   + "	`extcredits5`>= IF(?extcredits5<0 ,ABS(?extcredits5),`extcredits5`) AND "
			   + "	`extcredits6`>= IF(?extcredits6<0 ,ABS(?extcredits6),`extcredits6`) AND "
			   + "	`extcredits7`>= IF(?extcredits7<0 ,ABS(?extcredits7),`extcredits7`) AND "
			   + "	`extcredits8`>= IF(?extcredits8<0 ,ABS(?extcredits8),`extcredits8`) ";

		   if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, prams)) == 0)
		   {
			   return false;
		   }
		   return true;
	   }

	   public void UpdateUserCredits(int uid, float[] values)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?extcredits1", (DbType)MySqlDbType.Decimal, 8, values[0]),
									 DbHelper.MakeInParam("?extcredits2", (DbType)MySqlDbType.Decimal, 8, values[1]),
									 DbHelper.MakeInParam("?extcredits3", (DbType)MySqlDbType.Decimal, 8, values[2]),
									 DbHelper.MakeInParam("?extcredits4", (DbType)MySqlDbType.Decimal, 8, values[3]),
									 DbHelper.MakeInParam("?extcredits5", (DbType)MySqlDbType.Decimal, 8, values[4]),
									 DbHelper.MakeInParam("?extcredits6", (DbType)MySqlDbType.Decimal, 8, values[5]),
									 DbHelper.MakeInParam("?extcredits7", (DbType)MySqlDbType.Decimal, 8, values[6]),
									 DbHelper.MakeInParam("?extcredits8", (DbType)MySqlDbType.Decimal, 8, values[7]),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };

		   string CommandText = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET "
			   + "		`extcredits1`=`extcredits1` + ?extcredits1, "
			   + "		`extcredits2`=`extcredits2` + ?extcredits2, "
			   + "		`extcredits3`=`extcredits3` + ?extcredits3, "
			   + "		`extcredits4`=`extcredits4` + ?extcredits4, "
			   + "		`extcredits5`=`extcredits5` + ?extcredits5, "
			   + "		`extcredits6`=`extcredits6` + ?extcredits6, "
			   + "		`extcredits7`=`extcredits7` + ?extcredits7, "
			   + "		`extcredits8`=`extcredits8` + ?extcredits8 "
			   + "WHERE `uid`=?uid";

		   DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
	   }

	   public bool CheckUserCreditsIsEnough(int uid, DataRow values, int pos, int mount)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
									 DbHelper.MakeInParam("?extcredits1", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits2", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits3", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits4", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits5", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits6", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits7", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits8", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								 };
		   //string CommandText = "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE [uid]=?uid AND"
		   //        + "	[extcredits1]>= (case when ?extcredits1 >= 0 then abs(?extcredits1) else 0 end) AND "
		   //        + "	[extcredits2]>= (case when ?extcredits2 >= 0 then abs(?extcredits2) else 0 end) AND "
		   //        + "	[extcredits3]>= (case when ?extcredits3 >= 0 then abs(?extcredits3) else 0 end) AND "
		   //        + "	[extcredits4]>= (case when ?extcredits4 >= 0 then abs(?extcredits4) else 0 end) AND "
		   //        + "	[extcredits5]>= (case when ?extcredits5 >= 0 then abs(?extcredits5) else 0 end) AND "
		   //        + "	[extcredits6]>= (case when ?extcredits6 >= 0 then abs(?extcredits6) else 0 end) AND "
		   //        + "	[extcredits7]>= (case when ?extcredits7 >= 0 then abs(?extcredits7) else 0 end) AND "
		   //        + "	[extcredits8]>= (case when ?extcredits8 >= 0 then abs(?extcredits8) else 0 end) ";






		   String CommandText = "SELECT count(1) FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid AND"
			   + "	`extcredits1`>= IF(?extcredits1>=0,abs(?extcredits1),0) AND "
			   + "	`extcredits2`>= IF(?extcredits2>=0,abs(?extcredits2),0) AND "
			   + "	`extcredits3`>= IF(?extcredits3>=0,abs(?extcredits3),0) AND "
			   + "	`extcredits4`>= IF(?extcredits4>=0,abs(?extcredits4),0) AND "
			   + "	`extcredits5`>= IF(?extcredits5>=0,abs(?extcredits5),0) AND "
			   + "	`extcredits6`>= IF(?extcredits6>=0,abs(?extcredits6),0) AND "
			   + "	`extcredits7`>= IF(?extcredits7>=0,abs(?extcredits7),0) AND "
			   + "	`extcredits8`>= IF(?extcredits8>=0,abs(?extcredits8),0)";

		   if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, prams)) == 0)
		   {
			   return false;
		   }
		   return true;
	   }

	   public void UpdateUserCredits(int uid, DataRow values, int pos, int mount)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?extcredits1", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits2", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits3", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits4", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits5", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits6", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits7", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									 DbHelper.MakeInParam("?extcredits8", (DbType)MySqlDbType.Decimal, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };
		   if (pos < 0 && mount < 0)
		   {
			   for (int i = 1; i < prams.Length; i++)
			   {
				   prams[i].Value = -Convert.ToInt32(prams[i].Value);
			   }
		   }

		   string CommandText = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET "
			   + "	`extcredits1`=`extcredits1` + ?extcredits1, "
			   + "	`extcredits2`=`extcredits2` + ?extcredits2, "
			   + "	`extcredits3`=`extcredits3` + ?extcredits3, "
			   + "	`extcredits4`=`extcredits4` + ?extcredits4, "
			   + "	`extcredits5`=`extcredits5` + ?extcredits5, "
			   + "	`extcredits6`=`extcredits6` + ?extcredits6, "
			   + "	`extcredits7`=`extcredits7` + ?extcredits7, "
			   + "	`extcredits8`=`extcredits8` + ?extcredits8 "
			   + "WHERE `uid`=?uid";

		   DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
	   }


	   public DataTable GetUserGroups()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "usergroups` ORDER BY `groupid`").Tables[0];
	   }

	   public DataTable GetUserGroupRateRange(int groupid)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `raterange` FROM `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=" + groupid.ToString() + " LIMIT 1").Tables[0];
	   }

	   public IDataReader GetUserTodayRate(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `extcredits`, SUM(ABS(`score`)) AS `todayrate` FROM `" + BaseConfigs.GetTablePrefix + "ratelog` WHERE DATEDIFF(`postdatetime`,CURDATE()) = 0 AND `uid` = ?uid GROUP BY `extcredits`", prams);
	   }


	   public string GetSpecialGroupInfoSql()
	   {
		   return "Select * From `" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `radminid`=-1 And `groupid`>8 Order By `groupid`";
	   }


	   /// <summary>
	   /// 更新在线时间
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns></returns>
	   public int UpdateOnlineTime(int uid)
            
	   {

		   return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}users` SET `oltime` = `oltime` + FLOOR((UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(`lastvisit`))/60) WHERE `uid`={1}", BaseConfigs.GetTablePrefix, uid));
    
                                                                                                                                      
	   }

	   /// <summary>
	   /// 返回指定用户的信息
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns>用户信息</returns>
	   public IDataReader GetUserInfoToReader(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid),
		   };
		   string sql = "SELECT `" + BaseConfigs.GetTablePrefix + "users`.*, `" + BaseConfigs.GetTablePrefix + "userfields`.website,`" + BaseConfigs.GetTablePrefix + "userfields`.icq,`" + BaseConfigs.GetTablePrefix + "userfields`.qq,`" + BaseConfigs.GetTablePrefix + "userfields`.yahoo,`" + BaseConfigs.GetTablePrefix + "userfields`.msn,`" + BaseConfigs.GetTablePrefix + "userfields`.skype,`" + BaseConfigs.GetTablePrefix + "userfields`.location,`" + BaseConfigs.GetTablePrefix + "userfields`.customstatus,`" + BaseConfigs.GetTablePrefix + "userfields`.avatar,`" + BaseConfigs.GetTablePrefix + "userfields`.avatarwidth,`" + BaseConfigs.GetTablePrefix + "userfields`.avatarheight,`" + BaseConfigs.GetTablePrefix + "userfields`.medals,`" + BaseConfigs.GetTablePrefix + "userfields`.bio,`" + BaseConfigs.GetTablePrefix + "userfields`.signature,`" + BaseConfigs.GetTablePrefix + "userfields`.sightml,`" + BaseConfigs.GetTablePrefix + "userfields`.authstr,`" + BaseConfigs.GetTablePrefix + "userfields`.authtime,`" + BaseConfigs.GetTablePrefix + "userfields`.authflag,`" + BaseConfigs.GetTablePrefix + "userfields`.realname,`" + BaseConfigs.GetTablePrefix + "userfields`.idcard,`" + BaseConfigs.GetTablePrefix + "userfields`.mobile,`" + BaseConfigs.GetTablePrefix + "userfields`.phone  " +
			   "FROM " + BaseConfigs.GetTablePrefix + "users LEFT JOIN " + BaseConfigs.GetTablePrefix + "userfields ON `" + BaseConfigs.GetTablePrefix + "users`.`uid`=`" + BaseConfigs.GetTablePrefix + "userfields`.`uid` WHERE `" + BaseConfigs.GetTablePrefix + "users`.`uid`=?uid LIMIT 1";

		   // return DbHelper.ExecuteReader(CommandType.Text, string.Format(sql,BaseConfigs.GetTablePrefix), prams);
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format(sql, BaseConfigs.GetTablePrefix), prams);



		   // return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getuserinfo", prams);
	   }

	   /// <summary>
	   /// 获取简短用户信息
	   /// </summary>
	   /// <param name="uid">用id</param>
	   /// <returns>用户简短信息</returns>
	   public IDataReader GetShortUserInfoToReader(int uid)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32,4, uid),
		   };

		   //return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getshortuserinfo", prams);
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid", prams);

	   }

	   /// <summary>
	   /// 根据IP查找用户
	   /// </summary>
	   /// <param name="ip">ip地址</param>
	   /// <returns>用户信息</returns>
	   public IDataReader GetUserInfoByIP(string ip)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?regip", (DbType)MySqlDbType.String,15, ip),
		   };

		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `" + BaseConfigs.GetTablePrefix + "users`.*, `" + BaseConfigs.GetTablePrefix + "userfields`.* FROM `" + BaseConfigs.GetTablePrefix + "users` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "users`.`uid`=`" + BaseConfigs.GetTablePrefix + "userfields`.`uid` WHERE `" + BaseConfigs.GetTablePrefix + "users`.`regip`=?regip ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`uid` DESC LIMIT 1", prams);

	   }

	   public IDataReader GetUserName(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `username` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `" + BaseConfigs.GetTablePrefix + "users`.`uid`=?uid LIMIT 1", prams);
	   }

	   public IDataReader GetUserJoinDate(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `joindate` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `" + BaseConfigs.GetTablePrefix + "users`.`uid`=?uid LIMIT 1", prams);
	   }

	   public IDataReader GetUserID(string username)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.VarChar,20,username),
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `" + BaseConfigs.GetTablePrefix + "users`.`username`=?username LIMIT 1", prams);

	   }

	   public DataTable GetUserList(int pagesize, int currentpage)
	   {
		   #region 获得用户列表
		   return DbHelper.ExecuteDataset("SELECT a.`uid`, a.`username`,a.`nickname`, a.`joindate`, a.`credits`, a.`posts`, a.`lastactivity`, a.`email`,a.`lastvisit`,a.`lastvisit`,a.`accessmasks`, a.`location`,`" + BaseConfigs.GetTablePrefix + "usergroups`.`grouptitle` FROM (SELECT `" + BaseConfigs.GetTablePrefix + "users`.*,`" + BaseConfigs.GetTablePrefix + "userfields`.`location` FROM `" + BaseConfigs.GetTablePrefix + "users` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "userfields`.`uid` = `" + BaseConfigs.GetTablePrefix + "users`.`uid`) AS a LEFT JOIN `" + BaseConfigs.GetTablePrefix + "usergroups` ON `" + BaseConfigs.GetTablePrefix + "usergroups`.`groupid`=a.`groupid` ORDER BY a.`uid` DESC LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString()).Tables[0];
		   #endregion
	   }

	   /// <summary>
	   /// 获得用户列表DataTable
	   /// </summary>
	   /// <param name="pagesize">每页记录数</param>
	   /// <param name="pageindex">当前页数</param>
	   /// <returns>用户列表DataTable</returns>
	   public DataTable GetUserList(int pagesize, int currentpage, string orderby,string ordertype)
	   {

		   #region Access,sql语句 getuserlist



		   string[] arrayorderby = new string[] { "username", "credits", "posts", "admin", "lastactivity" };
		   int i=Array.IndexOf(arrayorderby,orderby);


		   switch (i)
		   {
				   //case "uid":
				   //    orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   //    break;
			   case 0:
				   orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`username` " + ordertype + ",`" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   break;
			   case 1:
				   orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`credits` " + ordertype + ",`" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   break;
			   case 2:
				   orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`posts` " + ordertype + ",`" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   break;
			   case 3:
				   orderby = "WHERE `" + BaseConfigs.GetTablePrefix + "users`.`adminid` > 0 ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`adminid`,`" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   break;
				   //case "joindate":
				   //    orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`joindate` " + ordertype + ",`" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   //    break;
			   case 4:
				   orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`lastactivity` " + ordertype + "," + BaseConfigs.GetTablePrefix + "users.uid " + ordertype;
				   break;
			   default:
				   orderby = "ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`uid` " + ordertype;
				   break;
		   }

		   string strSQL = "";
		   string tableUsers = string.Concat(BaseConfigs.GetTablePrefix, "users");
		   string tableUserFields = string.Concat(BaseConfigs.GetTablePrefix, "userfields");

		   strSQL = "SELECT `" + tableUsers + "`.`uid`,`" + tableUsers + "`.`username`,`" + tableUsers + "`.`groupid`,`" + tableUsers + "`.`nickname`, `" + tableUsers + "`.`joindate`, `" + tableUsers + "`.`credits`, `" + tableUsers + "`.`posts`,`" + tableUsers + "`.`lastactivity`, `" + tableUsers + "`.`email`, `" + tableUserFields + "`.`location` " +
			   "FROM `" + tableUsers + "` " +
			   "LEFT JOIN `" + tableUserFields + "` " +
			   "ON `" + tableUserFields + "`.`uid` = `" + tableUsers + "`.`uid` " + orderby + " LIMIT " + ((currentpage - 1) * pagesize).ToString() + "," + pagesize.ToString();

		   #endregion
		   return DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0];



	   }

	   /// <summary>
	   /// 判断指定用户名是否已存在
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns>如果已存在该用户id则返回true, 否则返回false</returns>
	   public bool Exists(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   };
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid", prams)) >= 1;
	   }

	   /// <summary>
	   /// 判断指定用户名是否已存在.
	   /// </summary>
	   /// <param name="username">用户名</param>
	   /// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
	   public bool Exists(string username)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.String,20,username),
		   };
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM `{0}users` WHERE `username`=?username", BaseConfigs.GetTablePrefix), prams)) >= 1;


	   }

	   /// <summary>
	   /// 是否有指定ip地址的用户注册
	   /// </summary>
	   /// <param name="ip">ip地址</param>
	   /// <returns>存在返回true,否则返回false</returns>
	   public bool ExistsByIP(string ip)
	   {
		   //IDataParameter[] prams = {
		   //						   DbHelper.MakeInParam("?regip",(DbType)SqlDbType.String, 15,ip),
		   //};
		   //return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [regip]=?regip", BaseConfigs.GetTablePrefix), prams)) >= 1;\

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?regip",(DbType)MySqlDbType.String, 15,ip),
		   };
		   return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM `{0}users` WHERE `regip`=?regip", BaseConfigs.GetTablePrefix), prams)) >= 1;

	   }

	   /// <summary>
	   /// 检测Email和安全项
	   /// </summary>
	   /// <param name="username">用户名</param>
	   /// <param name="email">email</param>
	   /// <param name="questionid">问题id</param>
	   /// <param name="answer">答案</param>
	   /// <returns>如果正确则返回用户id, 否则返回-1</returns>
	   public IDataReader CheckEmailAndSecques(string username, string email, string secques)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.String,20,username),
									 DbHelper.MakeInParam("?email",(DbType)MySqlDbType.String,50, email),
									 DbHelper.MakeInParam("?secques",(DbType)MySqlDbType.String,8, secques)
								 };
		   String sqlstring = "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `username`=?username AND `email`=?email AND `secques`=?secques LIMIT 1";
		   return DbHelper.ExecuteReader(CommandType.Text, sqlstring, prams);

	   }

	   /// <summary>
	   /// 检测密码和安全项
	   /// </summary>
	   /// <param name="username">用户名</param>
	   /// <param name="password">密码</param>
	   /// <param name="originalpassword">是否非MD5密码</param>
	   /// <param name="questionid">问题id</param>
	   /// <param name="answer">答案</param>
	   /// <returns>如果正确则返回用户id, 否则返回-1</returns>
	   public IDataReader CheckPasswordAndSecques(string username, string password, bool originalpassword, string secques)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.String,20,username),
									 DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32, originalpassword ? Utils.MD5(password) : password),
									 DbHelper.MakeInParam("?secques",(DbType)MySqlDbType.String,8, secques)
								 };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `username`=?username AND `password`=?password AND `secques`=?secques LIMIT 1", prams);
	   }

	   /// <summary>
	   /// 检查密码
	   /// </summary>
	   /// <param name="username">用户名</param>
	   /// <param name="password">密码</param>
	   /// <param name="originalpassword">是否非MD5密码</param>
	   /// <returns>如果正确则返回用户id, 否则返回-1</returns>
	   public IDataReader CheckPassword(string username, string password, bool originalpassword)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.String,20, username),
									 DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32, originalpassword ? Utils.MD5(password) : password)
								 };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `username`=?username AND `password`=?password LIMIT 1", prams);
	   }

	   /// <summary>
	   /// 检测DVBBS兼容模式的密码
	   /// </summary>
	   /// <param name="username">用户名</param>
	   /// <param name="password">密码</param>
	   /// <param name="questionid">问题id</param>
	   /// <param name="answer">答案</param>
	   /// <returns>如果正确则返回用户id, 否则返回-1</returns>
	   public IDataReader CheckDvBbsPasswordAndSecques(string username, string password)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.String,20,username),
			   // DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32, Utils.MD5(password).Substring(8, 16))
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid`, `password`, `secques` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `username`=?username LIMIT 1", prams);
	   }

	   /// <summary>
	   /// 检测密码
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="password">密码</param>
	   /// <param name="originalpassword">是否非MD5密码</param>
	   /// <param name="groupid">用户组id</param>
	   /// <param name="adminid">管理id</param>
	   /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
	   public IDataReader CheckPassword(int uid, string password, bool originalpassword)
	   {

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32, originalpassword ? Utils.MD5(password) : password)
								 };

		   String sql = "SELECT `uid`, `groupid`, `adminid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid AND `password`=?password LIMIT 1";

		   return DbHelper.ExecuteReader(CommandType.Text, sql, prams);
		   // return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordbyuid", prams);
	   }

	   /// <summary>
	   /// 根据指定的email查找用户并返回用户uid
	   /// </summary>
	   /// <param name="email">email地址</param>
	   /// <returns>用户uid</returns>
	   public IDataReader FindUserEmail(string email)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?email",(DbType)MySqlDbType.String,50, email),
		   };
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `email`=?email LIMIT 1", prams);
	   }

	   /// <summary>
	   /// 得到论坛中用户总数
	   /// </summary>
	   /// <returns>用户总数</returns>
	   public int GetUserCount()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM `" + BaseConfigs.GetTablePrefix + "users`"), 0);
	   }

	   /// <summary>
	   /// 得到论坛中用户总数
	   /// </summary>
	   /// <returns>用户总数</returns>
	   public int GetUserCountByAdmin()
	   {
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `" + BaseConfigs.GetTablePrefix + "users`.`adminid` > 0"), 0);
	   }

	   /// <summary>
	   /// 创建新用户.
	   /// </summary>
	   /// <param name="__userinfo">用户信息</param>
	   /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
	   public int CreateUser(UserInfo __userinfo)
	   {
		   if (Exists(__userinfo.Username))
		   {
			   return -1;
		   }

		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.String,20,__userinfo.Username),
									 DbHelper.MakeInParam("?nickname",(DbType)MySqlDbType.String,20,__userinfo.Nickname),
									 DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32,__userinfo.Password),
									 DbHelper.MakeInParam("?secques",(DbType)MySqlDbType.String,8,__userinfo.Secques),
									 DbHelper.MakeInParam("?gender",(DbType)MySqlDbType.Int32,4,__userinfo.Gender),
									 DbHelper.MakeInParam("?adminid",(DbType)MySqlDbType.Int32,4,__userinfo.Adminid),
									 DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int16,2,__userinfo.Groupid),
									 DbHelper.MakeInParam("?groupexpiry",(DbType)MySqlDbType.Int32,4,__userinfo.Groupexpiry),
									 DbHelper.MakeInParam("?extgroupids",(DbType)MySqlDbType.String,60,__userinfo.Extgroupids),
									 DbHelper.MakeInParam("?regip",(DbType)MySqlDbType.VarChar,0,__userinfo.Regip),
									 DbHelper.MakeInParam("?joindate",(DbType)MySqlDbType.VarChar,0,__userinfo.Joindate),
									 DbHelper.MakeInParam("?lastip",(DbType)MySqlDbType.String,15,__userinfo.Lastip),
									 DbHelper.MakeInParam("?lastvisit",(DbType)MySqlDbType.VarChar,0,__userinfo.Lastvisit),
									 DbHelper.MakeInParam("?lastactivity",(DbType)MySqlDbType.VarChar,0,__userinfo.Lastactivity),
									 DbHelper.MakeInParam("?lastpost",(DbType)MySqlDbType.VarChar,0,__userinfo.Lastpost),
									 DbHelper.MakeInParam("?lastpostid",(DbType)MySqlDbType.Int32,4,__userinfo.Lastpostid),
									 DbHelper.MakeInParam("?lastposttitle",(DbType)MySqlDbType.VarChar,0,__userinfo.Lastposttitle),
									 DbHelper.MakeInParam("?posts",(DbType)MySqlDbType.Int32,4,__userinfo.Posts),
									 DbHelper.MakeInParam("?digestposts",(DbType)MySqlDbType.Int16,2,__userinfo.Digestposts),
									 DbHelper.MakeInParam("?oltime",(DbType)MySqlDbType.Int16,2,__userinfo.Oltime),
									 DbHelper.MakeInParam("?pageviews",(DbType)MySqlDbType.Int32,4,__userinfo.Pageviews),
									 DbHelper.MakeInParam("?credits",(DbType)MySqlDbType.Int32,4,__userinfo.Credits),
									 DbHelper.MakeInParam("?extcredits1",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits1),
									 DbHelper.MakeInParam("?extcredits2",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits2),
									 DbHelper.MakeInParam("?extcredits3",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits3),
									 DbHelper.MakeInParam("?extcredits4",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits4),
									 DbHelper.MakeInParam("?extcredits5",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits5),
									 DbHelper.MakeInParam("?extcredits6",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits6),
									 DbHelper.MakeInParam("?extcredits7",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits7),
									 DbHelper.MakeInParam("?extcredits8",(DbType)MySqlDbType.Double,8,__userinfo.Extcredits8),
									 DbHelper.MakeInParam("?avatarshowid",(DbType)MySqlDbType.Int32,4,__userinfo.Avatarshowid),
									 DbHelper.MakeInParam("?email",(DbType)MySqlDbType.String,50,__userinfo.Email),
									 DbHelper.MakeInParam("?bday",(DbType)MySqlDbType.VarChar,0,__userinfo.Bday),
									 DbHelper.MakeInParam("?sigstatus",(DbType)MySqlDbType.Int32,4,__userinfo.Sigstatus),
									 DbHelper.MakeInParam("?tpp",(DbType)MySqlDbType.Int32,4,__userinfo.Tpp),
									 DbHelper.MakeInParam("?ppp",(DbType)MySqlDbType.Int32,4,__userinfo.Ppp),
									 DbHelper.MakeInParam("?templateid",(DbType)MySqlDbType.Int16,2,__userinfo.Templateid),
									 DbHelper.MakeInParam("?pmsound",(DbType)MySqlDbType.Int32,4,__userinfo.Pmsound),
									 DbHelper.MakeInParam("?showemail",(DbType)MySqlDbType.Int32,4,__userinfo.Showemail),
									 DbHelper.MakeInParam("?newsletter",(DbType)MySqlDbType.Int32,4,__userinfo.Newsletter),
									 DbHelper.MakeInParam("?invisible",(DbType)MySqlDbType.Int32,4,__userinfo.Invisible),
									 DbHelper.MakeInParam("?newpm",(DbType)MySqlDbType.Int32,4,__userinfo.Newpm),
									 DbHelper.MakeInParam("?accessmasks",(DbType)MySqlDbType.Int32,4,__userinfo.Accessmasks)
								 };
		   IDataParameter[] prams2 = {
									  //
									  DbHelper.MakeInParam("?website",(DbType)MySqlDbType.VarChar,80,__userinfo.Website),
									  DbHelper.MakeInParam("?icq",(DbType)MySqlDbType.VarChar,12,__userinfo.Icq),
									  DbHelper.MakeInParam("?qq",(DbType)MySqlDbType.VarChar,12,__userinfo.Qq),
									  DbHelper.MakeInParam("?yahoo",(DbType)MySqlDbType.VarChar,40,__userinfo.Yahoo),
									  DbHelper.MakeInParam("?msn",(DbType)MySqlDbType.VarChar,40,__userinfo.Msn),
									  DbHelper.MakeInParam("?skype",(DbType)MySqlDbType.VarChar,40,__userinfo.Skype),
									  DbHelper.MakeInParam("?location",(DbType)MySqlDbType.VarChar,30,__userinfo.Location),
									  DbHelper.MakeInParam("?customstatus",(DbType)MySqlDbType.VarChar,30,__userinfo.Customstatus),
									  DbHelper.MakeInParam("?avatar",(DbType)MySqlDbType.VarChar,255,__userinfo.Avatar),
									  DbHelper.MakeInParam("?avatarwidth",(DbType)MySqlDbType.Int32,4,(__userinfo.Avatarwidth == 0)? 60 : __userinfo.Avatarwidth),
									  DbHelper.MakeInParam("?avatarheight",(DbType)MySqlDbType.Int32,4,(__userinfo.Avatarheight == 0)? 60 : __userinfo.Avatarheight),
									  DbHelper.MakeInParam("?medals",(DbType)MySqlDbType.VarChar,40, __userinfo.Medals),
									  DbHelper.MakeInParam("?bio",(DbType)MySqlDbType.VarChar,0,__userinfo.Bio),
									  DbHelper.MakeInParam("?signature",(DbType)MySqlDbType.VarChar,0,__userinfo.Signature),
									  DbHelper.MakeInParam("?sightml",(DbType)MySqlDbType.VarChar,0,__userinfo.Sightml),
									  DbHelper.MakeInParam("?authstr",(DbType)MySqlDbType.VarChar,20,__userinfo.Authstr),
									  //DbHelper.MakeInParam("?authtime",(DbType)MySqlDbType.VarChar,0,__userinfo.Authtime),
									  DbHelper.MakeInParam("?realname",(DbType)MySqlDbType.VarChar,10,__userinfo.Realname),
									  DbHelper.MakeInParam("?idcard",(DbType)MySqlDbType.VarChar,20,__userinfo.Idcard),
									  DbHelper.MakeInParam("?mobile",(DbType)MySqlDbType.VarChar,20,__userinfo.Mobile),
									  DbHelper.MakeInParam("?phone",(DbType)MySqlDbType.VarChar,20,__userinfo.Phone)
								  };

		   string sqlstring = string.Empty;

		   int uid,id;

		   MySqlConnection conn = new MySqlConnection(DbHelper.ConnectionString);
		   conn.Open();
		   using (MySqlTransaction trans = conn.BeginTransaction())
		   {
			   try
			   {

				   DbHelper.ExecuteNonQuery(out id,trans, CommandType.Text,"INSERT INTO `" + BaseConfigs.GetTablePrefix + "users" + "`(`username`,`nickname`, `password`, `secques`, `gender`, `adminid`, `groupid`, `groupexpiry`, `extgroupids`, `regip`, `joindate`, `lastip`, `lastvisit`, `lastactivity`, `lastpost`, `lastpostid`, `lastposttitle`, `posts`, `digestposts`, `oltime`, `pageviews`, `credits`, `extcredits1`, `extcredits2`, `extcredits3`, `extcredits4`, `extcredits5`, `extcredits6`, `extcredits7`, `extcredits8`, `avatarshowid`, `email`, `bday`, `sigstatus`, `tpp`, `ppp`, `templateid`, `pmsound`, `showemail`, `newsletter`, `invisible`, `newpm`, `accessmasks`) " +
					   "VALUES(?username,?nickname, ?password, ?secques, ?gender, ?adminid, ?groupid, ?groupexpiry, ?extgroupids, ?regip, ?joindate, ?lastip, ?lastvisit, ?lastactivity, ?lastpost, ?lastpostid, ?lastposttitle, ?posts, ?digestposts, ?oltime, ?pageviews, ?credits, ?extcredits1, ?extcredits2, ?extcredits3, ?extcredits4, ?extcredits5, ?extcredits6, ?extcredits7, ?extcredits8, ?avatarshowid, ?email, ?bday, ?sigstatus, ?tpp, ?ppp, ?templateid, ?pmsound, ?showemail, ?newsletter, ?invisible, ?newpm, ?accessmasks);SELECT @@session.identity", prams);
				   uid = id;
				   //uid = (int)DbHelper.ExecuteScalar(trans, CommandType.Text, "select uid from `" + BaseConfigs.GetTablePrefix + "users` order by uid desc LIMIT 1");
				   //DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics" + "` SET `totalusers`=`totalusers` + 1,`lastusername`='" + __userinfo.Username + "',`lastuserid`=" + uid);


				   sqlstring = "INSERT INTO `" + BaseConfigs.GetTablePrefix + "userfields` (`uid`,`website`,`icq`,`qq`,`yahoo`,`msn`,`skype`,`location`,`customstatus`,`avatar`,`avatarwidth`,`avatarheight`,`medals`,`bio`,`signature`,`sightml`,`authstr`,`authtime`,`realname`,`idcard`,`mobile`,`phone`) VALUES (" + uid +
					   ",?website,?icq,?qq,?yahoo,?msn,?skype,?location,?customstatus,?avatar,?avatarwidth,?avatarheight,?medals,?bio,?signature,?sightml,?authstr,NOW(),?realname,?idcard,?mobile,?phone)";
                   


				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, sqlstring, prams2);

				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "statistics" + "` SET `totalusers`=`totalusers` + 1,`lastusername`=?username,`lastuserid`=" + uid, prams);
				   trans.Commit();
			   }
			   catch (Exception ex)
			   {
				   trans.Rollback();
				   throw ex;
			   }

			   finally
			   {
				   conn.Close();

			   }


		   }



		   return Utils.StrToInt(uid, -1);



	   }

	   /// <summary>
	   /// 更新权限验证字符串
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="authstr">验证串</param>
	   /// <param name="authflag">验证标志</param>
	   public void UpdateAuthStr(int uid, string authstr, int authflag)
	   {

		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?authstr", (DbType)MySqlDbType.String, 20, authstr),
									 DbHelper.MakeInParam("?authflag", (DbType)MySqlDbType.Int32, 4, authflag),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };


		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields" + "` SET `authstr`=?authstr, `authtime` = now(), `authflag`=?authflag WHERE `uid`=?uid";


		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);


	   }

	   /// <summary>
	   /// 更新指定用户的个人资料
	   /// </summary>
	   /// <param name="__userinfo">用户信息</param>
	   /// <returns>如果用户不存在则为false, 否则为true</returns>
	   public void UpdateUserProfile(UserInfo __userinfo)
	   {



		   IDataParameter[] prams1 = {
									  DbHelper.MakeInParam("?nickname",(DbType)MySqlDbType.String,20,__userinfo.Nickname),
									  DbHelper.MakeInParam("?gender", (DbType)MySqlDbType.Int32, 4, __userinfo.Gender),
									  DbHelper.MakeInParam("?email", (DbType)MySqlDbType.String, 50, __userinfo.Email),
									  DbHelper.MakeInParam("?bday", (DbType)MySqlDbType.String, 10, __userinfo.Bday),
									  DbHelper.MakeInParam("?showemail", (DbType)MySqlDbType.Int32, 4, __userinfo.Showemail),
									  DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, __userinfo.Uid)

								  };
		   IDataParameter[] prams2 = {
									  DbHelper.MakeInParam("?website", (DbType)MySqlDbType.VarChar, 80, __userinfo.Website),
									  DbHelper.MakeInParam("?icq",(DbType) MySqlDbType.VarChar, 12, __userinfo.Icq),
									  DbHelper.MakeInParam("?qq",(DbType)MySqlDbType.VarChar, 12, __userinfo.Qq),
									  DbHelper.MakeInParam("?yahoo", (DbType)MySqlDbType.VarChar, 40, __userinfo.Yahoo),
									  DbHelper.MakeInParam("?msn", (DbType)MySqlDbType.VarChar, 40, __userinfo.Msn),
									  DbHelper.MakeInParam("?skype", (DbType)MySqlDbType.VarChar, 40, __userinfo.Skype),
									  DbHelper.MakeInParam("?location", (DbType)MySqlDbType.VarChar, 30, __userinfo.Location),
									  DbHelper.MakeInParam("?bio", (DbType)MySqlDbType.VarChar, 0, __userinfo.Bio),
									  //  DbHelper.MakeInParam("?signature", (DbType)MySqlDbType.VarChar, 0, __userinfo.Signature),
									  DbHelper.MakeInParam("?realname",(DbType)MySqlDbType.VarChar,10,__userinfo.Realname),

									  DbHelper.MakeInParam("?idcard",(DbType)MySqlDbType.VarChar,20,__userinfo.Idcard),
									  DbHelper.MakeInParam("?mobile",(DbType)MySqlDbType.VarChar,20,__userinfo.Mobile),
									  DbHelper.MakeInParam("?phone",(DbType)MySqlDbType.VarChar,20,__userinfo.Phone),
									  DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, __userinfo.Uid)
								  };


		   MySqlConnection conn = new MySqlConnection(BaseConfigs.GetDBConnectString);
		   conn.Open();
		   using (MySqlTransaction trans = conn.BeginTransaction())
		   {
			   try
			   {
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users" + "` SET  `nickname`=?nickname, `gender`=?gender , `email`=?email , `bday`=?bday, `showemail`=?showemail WHERE `uid`=?uid", prams1);
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields" + "` SET  `website`=?website , `icq`=?icq , `qq`=?qq , `yahoo`=?yahoo , `msn`=?msn , `skype`=?skype , `location`=?location , `bio`=?bio,`idcard`=?idcard,`mobile`=?mobile,`phone`=?phone,`realname`=?realname WHERE  `uid`=?uid", prams2);
				   trans.Commit();
			   }
			   catch (Exception ex)
			   {
				   trans.Rollback();
				   throw ex;
			   }
			   finally
			   {
				   conn.Close(); 
			   }
		   }
            


	   }

	   /// <summary>
	   /// 更新用户论坛设置
	   /// </summary>
	   /// <param name="__userinfo">用户信息</param>
	   /// <returns>如果用户不存在则返回false, 否则返回true</returns>
	   public void UpdateUserForumSetting(UserInfo __userinfo)
	   {
		   IDataParameter[] prams1 = {

									  DbHelper.MakeInParam("?tpp",(DbType)MySqlDbType.Int32,4,__userinfo.Tpp),
									  DbHelper.MakeInParam("?ppp",(DbType)MySqlDbType.Int32,4,__userinfo.Ppp),
									  //DbHelper.MakeInParam("?templateid",(DbType)MySqlDbType.Int16,2,__userinfo.Templateid),
									  //DbHelper.MakeInParam("?pmsound",(DbType)MySqlDbType.Int32,4,__userinfo.Pmsound),
									  //DbHelper.MakeInParam("?newsletter",(DbType)MySqlDbType.Int32,4,__userinfo.Newsletter),
									  DbHelper.MakeInParam("?invisible",(DbType)MySqlDbType.Int32,4,__userinfo.Invisible),
									  DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,__userinfo.Uid)
								  };
		   IDataParameter[] prams2 = {

										  
									  DbHelper.MakeInParam("?signature", (DbType)MySqlDbType.VarChar, 500, __userinfo.Signature),
                
									  DbHelper.MakeInParam("?sightml", (DbType)MySqlDbType.VarChar, 1000, __userinfo.Sightml),
									  DbHelper.MakeInParam("?customstatus",(DbType)MySqlDbType.VarChar,30,__userinfo.Customstatus),
									  DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,__userinfo.Uid)
								  };

		   //  MySqlConnection conn = new MySqlConnection(BaseConfigs.GetDBConnectString);
		   // conn.Open();
		   //using (MySqlTransaction trans = conn.BeginTransaction())
		   //{
		   //  try
		   //{
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users" + "` SET  `tpp`=?tpp, `ppp`=?ppp,`invisible`=?invisible WHERE `uid`=?uid", prams1);
		   //DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users" + "` SET  `tpp`=?tpp, `ppp`=?ppp, `templateid`=?templateid, `pmsound`=?pmsound, `newsletter`=?newsletter, `invisible`=?invisible WHERE `uid`=?uid", prams1);
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields" + "` SET  `customstatus`=?customstatus WHERE  `uid`=?uid", prams2);
		   //  trans.Commit();
		   //}
		   //catch (Exception ex)
		   //{
		   //  trans.Rollback();
		   //throw ex;
		   //}
		   //}
		   //conn.Close();
	   }

	   /// <summary>
	   /// 修改用户自定义积分字段的值
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="extid">扩展字段序号(1-8)</param>
	   /// <param name="pos">增加的数值(可以是负数)</param>
	   /// <returns>执行是否成功</returns>
	   public void UpdateUserExtCredits(int uid, int extid, float pos)
	   {
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `extcredits" + extid.ToString() + "`=`extcredits" + extid.ToString() + "` + (" + pos.ToString() + ") WHERE `uid`=" + uid.ToString());
	   }

	   /// <summary>
	   /// 获得指定用户的指定积分扩展字段的值
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="extid">扩展字段序号(1-8)</param>
	   /// <returns>值</returns>
	   public float GetUserExtCredits(int uid, int extid)
	   {
		   return Utils.StrToFloat(DbHelper.ExecuteDataset(CommandType.Text, "SELECT `extcredits" + extid.ToString() + "` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=" + uid.ToString()).Tables[0].Rows[0][0], 0);
	   }

	   /// <summary>
	   /// 更新用户签名
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="signature">签名</param>
	   /// <returns>如果用户不存在则返回false, 否则返回true</returns>
	   public void UpdateUserSignature(int uid, int sigstatus, string signature, string sightml)
	   {
		   //IDataParameter[] prams1 = {
		   //                           DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   //                           DbHelper.MakeInParam("?sigstatus",(DbType)MySqlDbType.Int32,4,sigstatus),

		   //                       };

		   //IDataParameter[] prams2 = {
		   //                           DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   //                           DbHelper.MakeInParam("?signature",(DbType)MySqlDbType.VarChar,500,signature),
		   //                           DbHelper.MakeInParam("?sightml",(DbType)MySqlDbType.VarChar,1000,sightml)
		   //                       };




		   // DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateusersignature", prams);

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `sigstatus`=" + sigstatus + " WHERE `uid`=" + uid + "");
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields` SET  `signature`='" + signature + "',`sightml`='" + sightml + "' WHERE `uid`=" + uid + "");




	   }

	   /// <summary>
	   /// 更新用户头像
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="avatar">头像</param>
	   /// <param name="avatarwidth">头像宽度</param>
	   /// <param name="avatarheight">头像高度</param>
	   /// <returns>如果用户不存在则返回false, 否则返回true</returns>
	   public void UpdateUserAvatar(int uid, string avatar, int avatarwidth, int avatarheight)
	   {
		   //IDataParameter[] prams = {
		   //                           DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
		   //                           DbHelper.MakeInParam("?avatar",(DbType)MySqlDbType.VarChar,255,avatar),
		   //                           DbHelper.MakeInParam("?avatarwidth",(DbType)MySqlDbType.Int32,4,avatarwidth),
		   //                           DbHelper.MakeInParam("?avatarheight",(DbType)MySqlDbType.Int32,4,avatarheight)
		   //                       };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields" + "` SET  `avatar`='" + avatar + "', `avatarwidth`=" + avatarwidth + ", `avatarheight`=" + avatarheight + " WHERE  `uid`=" + uid + "");


	   }

	   /// <summary>
	   /// 更新用户密码
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="password">密码</param>
	   /// <param name="originalpassword">是否非MD5密码</param>
	   /// <returns>成功返回true否则false</returns>
	   public void UpdateUserPassword(int uid, string password, bool originalpassword)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 100, originalpassword ? Utils.MD5(password) : password),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "users  SET  `password`=?password  WHERE  `uid`=?uid", prams);

	   }

	   /// <summary>
	   /// 更新用户安全问题
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="questionid">问题id</param>
	   /// <param name="answer">答案</param>
	   /// <returns>成功返回true否则false</returns>
	   public void UpdateUserSecques(int uid, string secques)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?secques", (DbType)MySqlDbType.String, 8, secques),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };

		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `secques`=?secques WHERE `uid`=?uid", prams);
	   }

	   /// <summary>
	   /// 更新用户最后登录时间
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   public void UpdateUserLastvisit(int uid, string ip)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.VarChar,15, ip),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `lastvisit`=NOW(), `lastip`=?ip WHERE `uid` =?uid", prams);
	   }

	   public void UpdateUserOnlineStateAndLastActivity(string uidlist, int onlinestate, string activitytime)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?onlinestate", (DbType)MySqlDbType.Int32, 4, onlinestate),
									 DbHelper.MakeInParam("?activitytime", (DbType)MySqlDbType.VarChar, 25, activitytime)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `onlinestate`=?onlinestate,`lastactivity` = ?activitytime WHERE `uid` IN (" + uidlist + ")", prams);
	   }

	   public void UpdateUserOnlineStateAndLastActivity(int uid, int onlinestate, string activitytime)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?onlinestate", (DbType)MySqlDbType.Int32, 4, onlinestate),
									 DbHelper.MakeInParam("?activitytime", (DbType)MySqlDbType.VarChar, 25, activitytime),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `onlinestate`=?onlinestate,`lastactivity` = ?activitytime WHERE `uid`=?uid", prams);
	   }

	   public void UpdateUserOnlineStateAndLastVisit(string uidlist, int onlinestate, string activitytime)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?onlinestate", (DbType)MySqlDbType.Int32, 4, onlinestate),
									 DbHelper.MakeInParam("?activitytime", (DbType)MySqlDbType.VarChar, 25, activitytime)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `onlinestate`=?onlinestate,`lastvisit` = ?activitytime WHERE `uid` IN (" + uidlist + ")", prams);
	   }

	   public void UpdateUserOnlineStateAndLastVisit(int uid, int onlinestate, string activitytime)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?onlinestate", (DbType)MySqlDbType.Int32, 4, onlinestate),

									 DbHelper.MakeInParam("?activitytime", (DbType)MySqlDbType.VarChar, 25, activitytime),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `onlinestate`=?onlinestate,`lastvisit` = ?activitytime WHERE `uid`=?uid", prams);
	   }

	   /// <summary>
	   /// 更新用户当前的在线时间和最后活动时间
	   /// </summary>
	   /// <param name="uid">用户uid</param>
	   public void UpdateUserOnlineTime(int uid, string activitytime)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?activitytime", (DbType)MySqlDbType.VarChar, 25, activitytime),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };


		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `lastactivity` = ?activitytime  WHERE `uid` = ?uid", prams);

	   }

	   /// <summary>
	   /// 设置用户信息表中未读短消息的数量
	   /// </summary>
	   /// <param name="uid">用户ID</param>
	   /// <param name="pmnum">短消息数量</param>
	   /// <returns>更新记录个数</returns>
	   public int SetUserNewPMCount(int uid, int pmnum)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?value", (DbType)MySqlDbType.Int32, 4, pmnum),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)

								 };
		   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `newpmcount`=?value WHERE `uid`=?uid", prams);
	   }

	   /// <summary>
	   /// 更新指定用户的勋章信息
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <param name="medals">勋章信息</param>
	   public void UpdateMedals(int uid, string medals)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?medals", (DbType)MySqlDbType.VarChar, 300, medals),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)

								 };
		   DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields` SET `medals`=?medals WHERE `uid`=?uid", prams);

	   }

	   public int DecreaseNewPMCount(int uid, int subval)
	   {
		   IDataParameter[] prams = {

									 DbHelper.MakeInParam("?subval", (DbType)MySqlDbType.Int32, 4, subval),
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid)
								 };

		   try
		   {
			   //    return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET [newpmcount]=CASE WHEN [newpmcount] >= 0 THEN [newpmcount]-?subval ELSE 0 END WHERE [uid]=?uid", prams);
			   //  return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET [newpmcount]=IIF([newpmcount] >= 0,[newpmcount]-?subval,0) WHERE [uid]=?uid", prams);
			   return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `newpmcount`=IF(`newpmcount` >= 0,`newpmcount`-?subval,0) WHERE `uid`=?uid", prams);
		   }
		   catch
		   {
			   return -1;
		   }
	   }

	   /// <summary>
	   /// 得到用户新短消息数量
	   /// </summary>
	   /// <param name="uid">用户id</param>
	   /// <returns>新短消息数</returns>
	   public int GetUserNewPMCount(int uid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
		   };
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT `newpmcount` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `uid`=?uid", prams), 0);
	   }

	   /// <summary>
	   /// 更新用户精华数
	   /// </summary>
	   /// <param name="useridlist">uid列表</param>
	   /// <returns></returns>
	   public int UpdateUserDigest(string useridlist)
	   {
		   //StringBuilder sql = new StringBuilder();
		   //sql.Append("UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET [digestposts] = (");
		   //sql.Append("SELECT COUNT([tid]) AS [digest] FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.[posterid] = `" + BaseConfigs.GetTablePrefix + "users`.[uid] AND [digest]>0");
		   //sql.Append(") WHERE [uid] IN (");
		   //sql.Append(useridlist);
		   //sql.Append(")");

		   string count = "SELECT COUNT(`tid`) AS `digest` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `" + BaseConfigs.GetTablePrefix + "topics`.`posterid` in (" + useridlist + ") AND `digest`>0";
		   int i = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, count));
		   string sql = "update `" + BaseConfigs.GetTablePrefix + "users` SET `digestposts` = " + i + " where `uid` in (" + useridlist + ")";


		   return DbHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
	   }

	   /// <summary>
	   /// 更新用户SpaceID
	   /// </summary>
	   /// <param name="spaceid">要更新的SpaceId</param>
	   /// <param name="userid">要更新的UserId</param>
	   /// <returns></returns>
	   public void UpdateUserSpaceId(int spaceid, int userid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?spaceid",(DbType)MySqlDbType.Int32,4,spaceid),
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,userid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `spaceid`=?spaceid WHERE `uid`=?uid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }

	   public DataTable GetUserIdByAuthStr(string authstr)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?authstr",(DbType)MySqlDbType.VarChar,20,authstr)
								 };

		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "userfields` WHERE DateDiff(`authtime`,CURDATE())<=3  AND `authstr`=?authstr", prams).Tables[0];

		   return dt;
	   }

	   /// <summary>
	   /// 执行在线用户向表及缓存中添加的操作。
	   /// </summary>
	   /// <param name="__onlineuserinfo">在组用户信息内容</param>
	   /// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
	   public int AddOnlineUser(OnlineUserInfo __onlineuserinfo, int timeout)
	   {
		   string strDelTimeOutSql = "";
		   // 如果timeout为负数则代表不需要精确更新用户是否在线的状态
		   if (timeout > 0)
		   {
			   if (__onlineuserinfo.Userid > 0)
			   {
				   strDelTimeOutSql = string.Format("{0}UPDATE `{1}users` SET `onlinestate`=1 WHERE `uid`={2};", strDelTimeOutSql, BaseConfigs.GetTablePrefix, __onlineuserinfo.Userid.ToString()); 
				   DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql);
			   }
		   }
		   else
		   {
			   timeout = timeout * -1;
		   }

		   if (timeout > 9999)
		   {
			   timeout = 9999;
		   }

		   System.Text.StringBuilder sb = new System.Text.StringBuilder();
		   System.Text.StringBuilder sb2 = new System.Text.StringBuilder();

		   //IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT `userid` FROM `{0}online` WHERE `tickcount`<{1}", BaseConfigs.GetTablePrefix, Utils.SafeInt32(System.Environment.TickCount - timeout * 60000).ToString()));
		   IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT `userid` FROM `{0}online` WHERE `lastupdatetime`<'{1}'", BaseConfigs.GetTablePrefix, DateTime.Now.AddMinutes(timeout * -1).ToString("yyyy-MM-dd HH:mm:ss")));
		   try
		   {
			   while (dr.Read())
			   {
				   sb.Append(",");
				   sb.Append(dr[0].ToString());
				   if (dr[0].ToString() != "-1")
				   {
					   sb2.Append(",");
					   sb2.Append(dr[0].ToString());
				   }
			   }
		   }
		   finally
		   {
			   dr.Close();
		   }

		   if (sb.Length > 0)
		   {
			   sb.Remove(0, 1);
			   strDelTimeOutSql = string.Format("{0}DELETE FROM `{1}online` WHERE `userid` IN ({2});", strDelTimeOutSql, BaseConfigs.GetTablePrefix, sb.ToString());
			   DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql);
		   }
		   if (sb2.Length > 0)
		   {
			   sb2.Remove(0, 1);
			   strDelTimeOutSql = string.Format("{0}UPDATE `{1}users` SET `onlinestate`=0,`lastactivity`=NOW() WHERE `uid` IN ({2});", strDelTimeOutSql, BaseConfigs.GetTablePrefix, sb2.ToString());
			   DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql);
		   }




		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?userid",(DbType)MySqlDbType.Int32,4,__onlineuserinfo.Userid),
									 DbHelper.MakeInParam("?ip",(DbType)MySqlDbType.VarChar,15,__onlineuserinfo.Ip),
									 DbHelper.MakeInParam("?username",(DbType)MySqlDbType.VarChar,40,__onlineuserinfo.Username),
									 //DbHelper.MakeInParam("?tickcount",(DbType)MySqlDbType.Int32,4,System.Environment.TickCount),
									 DbHelper.MakeInParam("?nickname",(DbType)MySqlDbType.VarChar,40,__onlineuserinfo.Nickname),
									 DbHelper.MakeInParam("?password",(DbType)MySqlDbType.String,32,__onlineuserinfo.Password),
									 DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int16,2,__onlineuserinfo.Groupid),
									 DbHelper.MakeInParam("?olimg",(DbType)MySqlDbType.VarChar,80,__onlineuserinfo.Olimg),
									 DbHelper.MakeInParam("?adminid",(DbType)MySqlDbType.Int16,2,__onlineuserinfo.Adminid),
									 DbHelper.MakeInParam("?invisible",(DbType)MySqlDbType.Int16,2,__onlineuserinfo.Invisible),
									 DbHelper.MakeInParam("?action",(DbType)MySqlDbType.Int16,2,__onlineuserinfo.Action),
									 DbHelper.MakeInParam("?lastactivity",(DbType)MySqlDbType.Int16,2,__onlineuserinfo.Lastactivity),
									 DbHelper.MakeInParam("?lastposttime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(__onlineuserinfo.Lastposttime)),
									 DbHelper.MakeInParam("?lastpostpmtime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(__onlineuserinfo.Lastpostpmtime)),
									 DbHelper.MakeInParam("?lastsearchtime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(__onlineuserinfo.Lastsearchtime)),
									 DbHelper.MakeInParam("?lastupdatetime",(DbType)MySqlDbType.Datetime,8,DateTime.Parse(__onlineuserinfo.Lastupdatetime)),
									 DbHelper.MakeInParam("?forumid",(DbType)MySqlDbType.Int32,4,__onlineuserinfo.Forumid),
									 DbHelper.MakeInParam("?forumname",(DbType)MySqlDbType.VarChar,50,""),
									 DbHelper.MakeInParam("?titleid",(DbType)MySqlDbType.Int32,4,__onlineuserinfo.Titleid),
									 DbHelper.MakeInParam("?title",(DbType)MySqlDbType.VarChar,80,""),
									 DbHelper.MakeInParam("?verifycode",(DbType)MySqlDbType.VarChar,10,__onlineuserinfo.Verifycode)
								 };
		   //MySqlConnection cn = new MySqlConnection(BaseConfigs.GetDBConnectString);
            
		   // DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql + "INSERT INTO `" + BaseConfigs.GetTablePrefix + "online`(`userid`,`ip`,`username`,`tickcount`,`nickname`,`password`,`groupid`,`olimg`,`adminid`,`invisible`,`action`,`lastactivity`,`lastposttime`,`lastpostpmtime`,`lastsearchtime`,`lastupdatetime`,`forumid`,`forumname`,`titleid`,`title`, `verifycode`)VALUES(?userid,?ip,?username,?tickcount,?nickname,?password,?groupid,?olimg,?adminid,?invisible,?action,?lastactivity,?lastposttime,?lastpostpmtime,?lastsearchtime,?lastupdatetime,?forumid,?forumname,?titleid,?title,?verifycode)", prams);
		   int olid,id;
		   DbHelper.ExecuteNonQuery(out id,CommandType.Text,"INSERT INTO `" + BaseConfigs.GetTablePrefix + "online`(`userid`,`ip`,`username`,`nickname`,`password`,`groupid`,`olimg`,`adminid`,`invisible`,`action`,`lastactivity`,`lastposttime`,`lastpostpmtime`,`lastsearchtime`,`lastupdatetime`,`forumid`,`forumname`,`titleid`,`title`, `verifycode`) VALUES(?userid,?ip,?username,?nickname,?password,?groupid,?olimg,?adminid,?invisible,?action,?lastactivity,?lastposttime,?lastpostpmtime,?lastsearchtime,?lastupdatetime,?forumid,?forumname,?titleid,?title,?verifycode)", prams);
		   olid = id;
		   //int olid = Int32.Parse(DbHelper.ExecuteDataset(CommandType.Text, "select  `olid` from `" + BaseConfigs.GetTablePrefix + "online` order by `olid` desc LIMIT 1").Tables[0].Rows[0][0].ToString());

		   // 如果id值太大则重建在线表
		   if (olid > 2147483000)
		   {
			   CreateOnlineTable();
			   DbHelper.ExecuteNonQuery(CommandType.Text,"INSERT INTO `" + BaseConfigs.GetTablePrefix + "online`(`userid`,`ip`,`username`,`nickname`,`password`,`groupid`,`olimg`,`adminid`,`invisible`,`action`,`lastactivity`,`lastposttime`,`lastpostpmtime`,`lastsearchtime`,`lastupdatetime`,`forumid`,`titleid`,`verifycode`) VALUES(?userid,?ip,?username,?nickname,?password,?groupid,?olimg,?adminid,?invisible,?action,?lastactivity,?lastposttime,?lastpostpmtime,?lastsearchtime,?lastupdatetime,?forumid,?forumname,?titleid,?title,?verifycode)", prams);
			   return 1;
		   }


		   return 0;
	   }

	   public DataTable GetUserInfo(int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);
		   string sql = "select * from `" + BaseConfigs.GetTablePrefix + "users`  where `uid`=?uid LIMIT 1";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
	   }

	   public DataTable GetUserInfo(string username, string password)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?username", (DbType)MySqlDbType.String, 20, username),
									 DbHelper.MakeInParam("?password", (DbType)MySqlDbType.VarChar, 32, password)
								 };
		   string sql = "select * from `" + BaseConfigs.GetTablePrefix + "users`  where `username`=?username And `password`=?password LIMIT 1";

		   return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
	   }

	   public void UpdateUserSpaceId(int userid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `spaceid`=ABS(`spaceid`) WHERE `uid`=?userid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public int GetUserIdByRewriteName(string rewritename)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.String, 100, rewritename);
		   string sql = string.Format("SELECT `userid` FROM `{0}spaceconfigs` WHERE `rewritename`=?rewritename", BaseConfigs.GetTablePrefix);
		   return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), -1);
	   }


	   public void UpdateUserPMSetting(UserInfo user)
	   {
		   IDataParameter[] parms = {
									 DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, user.Uid),
									 DbHelper.MakeInParam("?pmsound", (DbType)MySqlDbType.Int32, 4, user.Pmsound),
									 DbHelper.MakeInParam("?newsletter", (DbType)MySqlDbType.Int32, 4, (int)user.Newsletter)
								 };
		   string sql = string.Format(@"UPDATE `{0}users` SET `pmsound`=?pmsound, `newsletter`=?newsletter WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
	   }
	   public void ClearUserSpace(int uid)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);
		   string sql = string.Format("UPDATE `{0}users` SET `spaceid`=0 WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
	   }

	   public string GetMedalSql()
	   {
		   return "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "medals`";
	   }

	   public IDataReader GetUserInfoByName(string username)
	   {
		   //IDataParameter parm =DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, username);
		   return DbHelper.ExecuteReader(CommandType.Text, "SELECT `uid`, `username` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE `username` like '%" + RegEsc(username) + "%'");
	   }


	   public void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
	   {
		   IDataParameter[] prams = {
									 DbHelper.MakeInParam("?uid",(DbType)MySqlDbType.Int32,4,uid),
									 DbHelper.MakeInParam("?avatar",(DbType)MySqlDbType.VarChar,255,avatar),
									 DbHelper.MakeInParam("?avatarwidth",(DbType)MySqlDbType.Int32,4,avatarwidth),
									 DbHelper.MakeInParam("?avatarheight",(DbType)MySqlDbType.Int32,4,avatarheight),
									 DbHelper.MakeInParam("?templateid", (DbType)MySqlDbType.Int32, 4, templateid)
								 };
		   MySqlConnection Msc = new MySqlConnection(DbHelper.ConnectionString);
		   Msc.Open();
		   using (MySqlTransaction trans = Msc.BeginTransaction())
		   {
			   try
			   {
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "userfields` SET `avatar`=?avatar, `avatarwidth`=?avatarwidth, `avatarheight`=?avatarheight WHERE `uid`=?uid", prams);
				   DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "users` SET `templateid`=?templateid WHERE `uid`=?uid", prams);
				   trans.Commit();
			   }


			   catch
			   {
				   trans.Rollback();
			   }
			   finally
			   {

				   Msc.Close();
			   }

		   }

		   //DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpreference", prams);
	   }




	   public DataTable UserList(int pagesize, int currentpage, string condition)
	   {
		   #region 获得用户列表

		   int pagetop = (currentpage - 1) * pagesize;

		   //if (currentpage == 1)
		   //{
		   return DbHelper.ExecuteDataset("SELECT `" + BaseConfigs.GetTablePrefix + "users`.`uid`, `" + BaseConfigs.GetTablePrefix + "users`.`username`,`" + BaseConfigs.GetTablePrefix + "users`.`nickname`, `" + BaseConfigs.GetTablePrefix + "users`.`joindate`, `" + BaseConfigs.GetTablePrefix + "users`.`credits`, `" + BaseConfigs.GetTablePrefix + "users`.`posts`, `" + BaseConfigs.GetTablePrefix + "users`.`lastactivity`, `" + BaseConfigs.GetTablePrefix + "users`.`email`,`" + BaseConfigs.GetTablePrefix + "users`.`lastvisit`,`" + BaseConfigs.GetTablePrefix + "users`.`lastvisit`,`" + BaseConfigs.GetTablePrefix + "users`.`accessmasks`, `" + BaseConfigs.GetTablePrefix + "userfields`.`location`,`" + BaseConfigs.GetTablePrefix + "usergroups`.`grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "users` LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` ON `" + BaseConfigs.GetTablePrefix + "userfields`.`uid` = `" + BaseConfigs.GetTablePrefix + "users`.`uid`  LEFT JOIN `" + BaseConfigs.GetTablePrefix + "usergroups` ON `" + BaseConfigs.GetTablePrefix + "usergroups`.`groupid`=`" + BaseConfigs.GetTablePrefix + "users`.`groupid` WHERE " + condition + " ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`uid` DESC LIMIT "+pagetop+","+pagesize.ToString()+"").Tables[0];
		   //}
		   //else
		   //{
		   //    string sqlstring = "SELECT `" + BaseConfigs.GetTablePrefix + "users`.`uid`, `" + BaseConfigs.GetTablePrefix + "users`.`username`,`" + BaseConfigs.GetTablePrefix + "users`.`nickname`, `" + BaseConfigs.GetTablePrefix + "users`.`joindate`, `" + BaseConfigs.GetTablePrefix + "users`.`credits`, `" + BaseConfigs.GetTablePrefix + "users`.`posts`, `" + BaseConfigs.GetTablePrefix + "users`.`lastactivity`, `" + BaseConfigs.GetTablePrefix + "users`.`email`,`" + BaseConfigs.GetTablePrefix + "users`.`lastvisit`,`" + BaseConfigs.GetTablePrefix + "users`.`lastvisit`,`" + BaseConfigs.GetTablePrefix + "users`.`accessmasks`, `" + BaseConfigs.GetTablePrefix + "userfields`.`location`,`" + BaseConfigs.GetTablePrefix + "usergroups`.`grouptitle` FROM `" + BaseConfigs.GetTablePrefix + "users`,`" + BaseConfigs.GetTablePrefix + "userfields`,`" + BaseConfigs.GetTablePrefix + "usergroups`  WHERE `" + BaseConfigs.GetTablePrefix + "userfields`.`uid` = `" + BaseConfigs.GetTablePrefix + "users`.`uid` AND  `" + BaseConfigs.GetTablePrefix + "usergroups`.`groupid`=`" + BaseConfigs.GetTablePrefix + "users`.`groupid` AND `" + BaseConfigs.GetTablePrefix + "users`.`uid` < (SELECT min(`uid`)  FROM (SELECT `uid` FROM `" + BaseConfigs.GetTablePrefix + "users` WHERE " + condition + " ORDER BY `uid` DESC LIMIT 0,"+pagetop+") AS tblTmp ) AND " + condition + " ORDER BY `" + BaseConfigs.GetTablePrefix + "users`.`uid` DESC LIMIT 0,"+pagesize.ToString();
		   //    return DbHelper.ExecuteDataset(sqlstring).Tables[0];
		   //}

		   #endregion
	   }

	   public DataTable GetExistMedalList()
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `medalid`,`image` FROM `" + BaseConfigs.GetTablePrefix + "medals` WHERE `image`<>''").Tables[0];
	   }

	   public void UpdateUserGroupTitleAndCreditsByGroupid(int groupid, string grouptitle, int creditslower, int creditshigher)
	   {
		   IDataParameter[] parms = {
               
									 DbHelper.MakeInParam("?grouptitle",(DbType)MySqlDbType.VarChar,50,grouptitle),
									 DbHelper.MakeInParam("?creditshigher",(DbType)MySqlDbType.Int32,4,creditshigher),
									 DbHelper.MakeInParam("?creditslower",(DbType)MySqlDbType.Int32,4,creditslower), 
									 DbHelper.MakeInParam("?groupid",(DbType)MySqlDbType.Int32,4,groupid)
								 };
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `grouptitle`=?grouptitle,`creditshigher`=?creditshigher,`creditslower`=?creditslower WHERE `groupid`=?groupid";
		   DbHelper.ExecuteDataset(CommandType.Text, sql, parms);
	   }

	   public void UpdateUserCredits(int userid, int extcreditsid, float score)
	   {
		   IDataParameter[] prams1 = {
					
									  DbHelper.MakeInParam("?Extcredits", (DbType)MySqlDbType.Decimal, 20, score),
									  DbHelper.MakeInParam("?targetuid",(DbType)MySqlDbType.Int32,4,userid.ToString())
								  };

		   string sqlstring = string.Format("UPDATE `{0}users` SET extcredits{1}=extcredits{1} + ?Extcredits WHERE `uid`=?targetuid", BaseConfigs.GetTablePrefix, extcreditsid);

		   DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
	   }


	   public void CombinationUsergroupScore(int sourceusergroupid, int targetusergroupid)
	   {
		   IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("?sourceusergroupid",(DbType)MySqlDbType.Int32, 4,sourceusergroupid),
				DbHelper.MakeInParam("?targetusergroupid",(DbType)MySqlDbType.Int32, 4,targetusergroupid)
			};
		   string sql = "UPDATE `" + BaseConfigs.GetTablePrefix + "usergroups` SET `creditshigher`=(SELECT `creditshigher` FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "usergroups` WHERE `groupid`=?sourceusergroupid) WHERE `groupid`=?targetusergroupid";
		   DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
	   }
	   public IDataReader GetOnlineUser(int olid)
	   {
		   IDataParameter[] parms = { DbHelper.MakeInParam("@olid", (DbType)MySqlDbType.Int32, 4, olid) };
		   return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
	   }

	   public int GetOlidByUid(int uid)
	   {
		   IDataParameter[] parms = { DbHelper.MakeInParam("@userid", (DbType)MySqlDbType.Int32, 4, uid) };
		   return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, string.Format("SELECT olid FROM [{0}online] WHERE [userid]=@userid", BaseConfigs.GetTablePrefix), parms), -1);
	   }

	   #endregion


	   #region WebsiteManage
	   private IDataParameter[] GetParms(string startdate, string enddate)
	   {
		   IDataParameter[] parms = new IDataParameter[2];
		   if (startdate != "")
		   {
			   parms[0] = DbHelper.MakeInParam("?startdate", (DbType)MySqlDbType.Datetime, 8, DateTime.Parse(startdate));
		   }
		   if (enddate != "")
		   {
			   parms[1] = DbHelper.MakeInParam("?enddate", (DbType)MySqlDbType.Datetime, 8, DateTime.Parse(enddate).AddDays(1));
		   }
		   return parms;
	   }


	   public DataTable GetTopicListByCondition(int forumid, string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
	   {
		   string sql = "";
		   string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   sql = "SELECT t.*,f.`name` FROM `" + BaseConfigs.GetTablePrefix + "topics` t LEFT JOIN " + BaseConfigs.GetTablePrefix + "forums f ON t.fid=f.fid LEFT JOIN `" + BaseConfigs.GetTablePrefix + "forumfields` as ff ON f.`fid`=ff.`fid` AND (ff.`viewperm` IS NULL OR ff.`viewperm`='' OR InStr(','+ff.`viewperm`+',',',7,')<>0) "
			   + "WHERE `closed`<>1 " + condition + " ORDER BY `tid` DESC LIMIT "+pageTop+","+pageSize.ToString();

           
                        
		   //}
		   //else
		   //{
		   //    sql = "SELECT t.*,f.`name` FROM `" + BaseConfigs.GetTablePrefix + "topics` t LEFT JOIN " + BaseConfigs.GetTablePrefix + "forums f ON t.fid=f.fid "
		   //        + "WHERE `closed`<>1 AND `tid`<(SELECT MIN(`tid`) FROM (SELECT `tid` FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE "
		   //        + "`closed`<>1 " + condition + " ORDER BY `tid` DESC LIMIT 0,"+pageTop.ToString()+") AS tblTmp)" + condition + " ORDER BY `tid` DESC LIMIT 0,"+pageSize.ToString();
		   //}
		   return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
	   }

	   private static string GetCondition(int forumid, string posterlist, string keylist, string startdate, string enddate)
	   {
		   string condition = "";
		   if (forumid != 0)
		   {
			   condition += " AND f.`fid`=" + forumid;
		   }
		   if (posterlist != "")
		   {
			   string[] poster = posterlist.Split(',');
			   condition += " AND `poster` in (";
			   string tempposerlist = "";
			   foreach (string p in poster)
			   {
				   tempposerlist += "'" + p + "',";
			   }
			   if (tempposerlist != "")
				   tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);
			   condition += tempposerlist + ")";
		   }
		   if (keylist != "")
		   {
			   string tempkeylist = "";
			   foreach (string key in keylist.Split(','))
			   {
				   tempkeylist += " `title` LIKE '%" + RegEsc(key) + "%' OR";
			   }
			   tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
			   condition += " AND (" + tempkeylist + ")";
		   }
		   //if (startdate != "")
		   //{
		   //    condition += " AND `postdatetime`>='" + startdate + " 00:00:00'";
		   //}
		   //if (enddate != "")
		   //{
		   //    condition += " AND `postdatetime`<='" + enddate + " 23:59:59'";
		   //}
		   if (startdate != "")
		   {
			   //condition += " AND [postdatetime]>='" + startdate + " 00:00:00'";
			   condition += " AND `postdatetime`>=?startdate";
		   }
		   if (enddate != "")
		   {
			   //condition += " AND [postdatetime]<='" + enddate + " 23:59:59'";
			   condition += " AND `postdatetime`<=?enddate";
		   }
		   return condition;
	   }

	   public int GetTopicListCountByCondition(int forumid, string posterlist, string keylist, string startdate, string enddate)
	   {
		   string sql = string.Format("SELECT COUNT(1) FROM `{0}topics` t LEFT JOIN `{0}forums` f ON t.fid=f.fid LEFT JOIN `{0}forumfields` ff ON f.`fid`=ff.`fid`   AND (ff.`viewperm` IS NULL OR ff.`viewperm`='' OR InStr(','+ff.`viewperm`+',',',7,')<>0) WHERE `closed`<>1 AND `status`=1 AND `password`=''", BaseConfigs.GetTablePrefix);
		   string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   if (condition != "")
			   sql += condition;
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,parms).ToString());
	   }

	   public DataTable GetTopicLitByTidlist(string tidlist)
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE (`tid` IN (" + tidlist + ")) ORDER BY INSTR('" + tidlist + "',`tid`)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }


	   public DataTable GetTopicListByTidlist(string posttableid, string tidlist)
	   {
		   string sql = "SELECT p.*,t.`closed` FROM `" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "` p LEFT JOIN `" + BaseConfigs.GetTablePrefix + "topics` t ON p.tid=t.tid WHERE layer=0 "
			   + "AND `closed`<>1 and p.`tid` IN (" + tidlist + ") ORDER BY instr('" + tidlist + "',p.`tid`)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }






	   public int GetAlbumListCountByCondition(string username, string title, string description, string startdate, string enddate, bool isshowall)
	   {
		   string sql = "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "albums` t";

		   if (isshowall)
		   {
			   sql += " WHERE 1=1";
		   }
		   else
		   {
			   sql += " WHERE `type` = 0 AND  `imgcount` > 0";
		   }


		   string condition = GetAlbumListCondition(username, title, description, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   if (condition != "")
			   sql += condition;
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,parms).ToString());
	   }


	   public DataTable GetAlbumLitByAlbumidList(string albumlist)
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `type` = 0 "
			   + "AND `albumid` IN (" + albumlist + ") ORDER BY INSTR('" + albumlist + "',`albumid`)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public int GetSpaceCountByCondition(string posterlist, string keylist, string startdate, string enddate)
	   {
		   string sql = "SELECT COUNT(*) FROM (SELECT s.*,u.`username` FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` s LEFT JOIN `"+BaseConfigs.GetTablePrefix+"users` u ON s.`userid`=u.`uid`) AS tblTmp WHERE `status`=0";
		   string condition = GetSpaceCondition(posterlist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   if (condition != "")
			   sql += condition;
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,parms).ToString());
	   }

	   public DataTable GetSpaceByCondition(string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
	   {
		   string sql = "";
		   string condition = GetSpaceCondition(posterlist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{

		   //sql = "SELECT TOP " + pageSize.ToString() + " s.*,u.`username`,f.`avatar`,(SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE userid=s.userid) albumcount "
		   //    + "FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` s LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` f ON `userid`=f.`uid` "
		   //    + "LEFT JOIN ["+BaseConfigs.GetTablePrefix+"users] u ON u.`uid`=`userid` WHERE `status`=0" + condition + " ORDER BY s.`spaceid` DESC";

		   sql = "SELECT s.*, u.username,f.avatar, " +
			   " (SELECT COUNT(*)" +
			   " FROM `" + BaseConfigs.GetTablePrefix + "albums` " +
			   " WHERE userid = s.userid) AS albumcount " +
			   " FROM (" + BaseConfigs.GetTablePrefix + "spaceconfigs as s LEFT JOIN " +
			   "" + BaseConfigs.GetTablePrefix + "userfields as f ON s.userid = f.uid) LEFT JOIN " +
			   "" + BaseConfigs.GetTablePrefix + "users as u ON u.uid = s.userid " +
			   "  WHERE (s.status = 0)" + condition + 
			   "  ORDER BY s.spaceid DESC LIMIT " + pageTop + "," + pageSize;


		   //}
		   //else
		   //{
		   //    sql = "SELECT s.*,u.`username`,f.`avatar`,(SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE `userid`=s.`userid`) albumcount "
		   //        + "FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` as s LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` as f ON `userid`=f.`uid` "
		   //        + "LEFT JOIN `" + BaseConfigs.GetTablePrefix + "users` as u ON u.`uid`=`userid` WHERE `status`=0 AND s.`spaceid`<(SELECT MIN(`spaceid`) FROM (SELECT " 
		   //        + " `spaceid` FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` WHERE `status`=0 " + condition + " ORDER BY `spaceid` DESC LIMIT 0,"+pageTop+") AS tblTmp)" + condition + " ORDER BY s.`spaceid` DESC LIMIT 0,"+pageSize.ToString();
		   //}
		   return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
	   }


	   public DataTable GetSpaceLitByTidlist(string spaceidlist)
	   {
		   string sql = "SELECT s.*,f.`avatar`,(SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE userid=s.userid) albumcount "
			   + "FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` s LEFT JOIN `" + BaseConfigs.GetTablePrefix + "userfields` f ON `userid`=f.`uid` "
			   + "WHERE (`spaceid` IN (" + spaceidlist + ")) ORDER BY instr('" + spaceidlist + "',`spaceid`)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }


	   public DataTable GetWebSiteAggForumTopicList(string showtype, int topnumber)
	   {
		   DataTable topicList = new DataTable();
		   switch (showtype)
		   {
			   default://按版块查
			   {
				   topicList = DbHelper.ExecuteDataset("SELECT f.`fid`, f.`name`, f.`lasttid` AS `tid`, f.`lasttitle` AS `title` , f.`lastposterid` AS `posterid`, f.`lastposter` AS `poster`, f.`lastpost` AS `postdatetime`, t.`views`, t.`replies` FROM `" + BaseConfigs.GetTablePrefix + "forums` f LEFT JOIN `" + BaseConfigs.GetTablePrefix + "topics` t  ON f.`lasttid` = t.`tid` WHERE f.`status`=1  AND `layer`> 0 AND f.`fid` IN (SELECT ff.`fid` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` ff WHERE ff.`password` ='') AND t.`displayorder`>=0").Tables[0]; break;
			   }
			   case "1"://按最新主题查
			   {
				   topicList = DbHelper.ExecuteDataset("SELECT t.`tid`, t.`title`, t.`postdatetime`, t.`poster`, t.`posterid`, t.`fid`, t.`views`, t.`replies`, f.`name` FROM `" + BaseConfigs.GetTablePrefix + "topics` t LEFT OUTER JOIN `" + BaseConfigs.GetTablePrefix + "forums` f ON t.`fid` = f.`fid` WHERE t.`displayorder`>=0 AND f.`status`=1  AND `layer`> 0 AND f.`fid` IN (SELECT ff.`fid` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` ff WHERE ff.`password` ='') ORDER BY t.`postdatetime` DESC LIMIT 0," + topnumber).Tables[0]; break;
			   }
			   case "2"://按精华主题查
			   {
				   topicList = DbHelper.ExecuteDataset("SELECT t.`tid`, t.`title`, t.`postdatetime`, t.`poster`, t.`posterid`, t.`fid`, t.`views`, t.`replies`, f.`name` FROM `" + BaseConfigs.GetTablePrefix + "topics` t LEFT OUTER JOIN `" + BaseConfigs.GetTablePrefix + "forums` f ON t.`fid` = f.`fid` WHERE t.`digest` >0 AND f.`status`=1  AND `layer`> 0 AND f.`fid` IN (SELECT ff.`fid` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` ff WHERE ff.`password` ='') ORDER BY t.`digest` DESC LIMIT 0," + topnumber).Tables[0]; break;
			   }
			   case "3"://按版块查
			   {
				   topicList = DbHelper.ExecuteDataset("SELECT f.`fid`, f.`name`, f.`lasttid` AS `tid`, f.`lasttitle` AS `title` , f.`lastposterid` AS `posterid`, f.`lastposter` AS `poster`, f.`lastpost` AS `postdatetime`, t.`views`, t.`replies` FROM `" + BaseConfigs.GetTablePrefix + "forums` f LEFT JOIN `" + BaseConfigs.GetTablePrefix + "topics` t  ON f.`lasttid` = t.`tid` WHERE f.`status`=1  AND `layer`> 0 AND f.`fid` IN (SELECT ff.`fid` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` ff WHERE ff.`password` ='') AND t.`displayorder`>=0").Tables[0]; break;
			   }
		   }
		   return topicList;
	   }

	   public DataTable GetWebSiteAggForumHotTopicList()
	   {
		   return DbHelper.ExecuteDataset("SELECT `tid`, `title` FROM `" + BaseConfigs.GetTablePrefix + "topics`  WHERE `displayorder`>=0 AND `closed`<>1 ORDER BY `replies` DESC LIMIT 0,10").Tables[0];
        
	   }

	   public DataTable GetWebSiteAggSpacePostList(int topnumber)
	   {
		   return DbHelper.ExecuteDataset(" SELECT postid, author, uid, postdatetime, title, commentcount, views FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE poststatus = 1 LIMIT 0," + topnumber + "").Tables[0];
	   }

	   public DataTable GetWebSiteAggRecentUpdateSpaceList(int topnumber)
	   {
		   return DbHelper.ExecuteDataset(" SELECT spaceid, userid, spacetitle, postcount, commentcount, visitedtimes FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` WHERE status = 0 AND postcount>0 ORDER BY updatedatetime DESC LIMIT 0," + topnumber + "").Tables[0];
	   }

	   public DataTable GetWebSiteAggTopSpaceList(string orderby,int topnumber)
	   {
		   return DbHelper.ExecuteDataset(" SELECT s.*,u.avatar FROM `" + BaseConfigs.GetTablePrefix + "spaceconfigs` as s Left Join `" + BaseConfigs.GetTablePrefix + "userfields` as u ON s.userid = u.uid  WHERE s.status = 0 ORDER BY s.`" + orderby + "` DESC LIMIT 0," + topnumber).Tables[0];
	   }

	   public DataTable GetWebSiteAggSpacePostsList(int pageSize, int currentPage)
	   {
		   DataTable dt = new DataTable();

		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{
		   string sql = "SELECT * FROM "
			   + "`" + BaseConfigs.GetTablePrefix + "spaceposts` ORDER BY `postid` DESC LIMIT "+pageTop+","+pageSize.ToString();
		   dt = DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		   //}
		   //else
		   //{
		   //    string sql = "SELECT * FROM "
		   //        + "`" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `postid` < (SELECT min(`postid`)  FROM "
		   //        + "(SELECT `postid` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` "
		   //        + "ORDER BY `postid` DESC LIMIT 0,"+pageTop+") AS tblTmp ) ORDER BY `postid` DESC LIMIT 0,"+pageSize.ToString();
		   //    dt = DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		   //}
		   return dt;
	   }


	   public int GetWebSiteAggSpacePostsCount()
	   {
		   try
		   {
			   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(`postid`) FROM `" + BaseConfigs.GetTablePrefix + "spaceposts`").ToString());
		   }
		   catch
		   {
			   return 0;
		   }
	   }

	   public DataTable GetAlbumListByCondition(string username, string title, string description, string startdate, string enddate, int pageSize, int currentPage, bool isshowall)
	   {
		   string sql = "";
		   string condition = GetAlbumListCondition(username, title, description, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   int pageTop = (currentPage - 1) * pageSize;


		   string strisshowall = "";

		   if (isshowall)
		   {
			   strisshowall = " 1=1";
		   }
		   else
		   {
			   strisshowall = " `type` = 0 AND  `imgcount` > 0 ";
		   }
		   //if (currentPage == 1)
		   //{
		   sql = "SELECT *  FROM `" + BaseConfigs.GetTablePrefix + "albums`  WHERE  "+strisshowall+" " + condition + " ORDER BY `albumid` DESC LIMIT "+pageTop+","+pageSize.ToString();
		   //}
		   //else
		   //{
		   //    sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "albums`  WHERE `type` = 0 AND `albumid`<(SELECT MIN(`albumid`) FROM (SELECT `albumid` FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE "
		   //        + "`type` = 0 " + condition + " ORDER BY `albumid` DESC LIMIT 0,"+pageTop+") AS tblTmp)" + condition + " ORDER BY `albumid` DESC LIMIT 0," + pageSize.ToString();
		   //}
		   return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
	   }

	   public IDataReader GetAlbumListByCondition(int type, int focusphotocount, int vaildDays)
	   {
		   IDataParameter parm = DbHelper.MakeInParam("?vailddays", (DbType)MySqlDbType.Int32, 4, vaildDays);
		   string sql = "SELECT * FROM `"+BaseConfigs.GetTablePrefix+"albums` WHERE DATEDIFF(NOW(), `createdatetime`) < " + vaildDays + " AND `imgcount`>0 AND `type`=0"; 

		   switch (type)
		   {
			   case 0:
				   sql += " ORDER BY `createdatetime` DESC  LIMIT 0,"+focusphotocount;
				   break;
			   default:
				   sql += " ORDER BY `createdatetime` DESC  LIMIT 0,"+focusphotocount;
				   break;
		   }
           

		   return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
	   }


	   public int GetPhotoCountByCondition(string photousernamelist, string keylist, string startdate, string enddate)
	   {
		   // string sql = "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE 1=1";
		   string sql = "SELECT COUNT(1) FROM `" + BaseConfigs.GetTablePrefix + "photos` as p LEFT JOIN `" + BaseConfigs.GetTablePrefix + "albums` as a ON p.`albumid`=a.`albumid` WHERE a.`type`=0";

		   string condition = GetPhotoCondition(photousernamelist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   if (condition != "")
			   sql += condition;
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,parms).ToString());
	   }


	   public DataTable GetPhotoByCondition(string photousernamelist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
	   {
		   string sql = "";
		   string condition = GetPhotoCondition(photousernamelist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{

		   //sql = "SELECT p.* FROM `" + BaseConfigs.GetTablePrefix + "photos` as p WHERE 1=1" + condition + " ORDER BY `photoid` DESC LIMIT "+pageTop+","+ pageSize.ToString();

		   sql = " SELECT  p.* FROM `" + BaseConfigs.GetTablePrefix + "photos` AS p LEFT JOIN `" + BaseConfigs.GetTablePrefix + "albums` AS a ON p.`albumid`=a.`albumid` WHERE a.`type`=0" + condition + " ORDER BY p.`photoid` DESC LIMIT "+pageTop+","+pageSize;
		   //}
		   //else
		   //{
		   //    sql = "SELECT  * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `photoid`<(SELECT MIN(`photoid`) FROM (SELECT "
		   //        + " `photoid` FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE 1=1 " + condition + " ORDER BY `photoid` DESC LIMIT 0,"+pageTop+") AS tblTmp)" + condition + " ORDER BY `photoid` DESC LIMIT 0,"+pageSize.ToString();
		   //}
		   return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
	   }

	   public DataTable GetPhotoByIdOrderList(string idlist)
	   {
		   if (!Common.Utils.IsNumericArray(idlist.Split(',')))
		   {
			   return new DataTable();
		   }
		   string sql = "SELECT `photoid`,`title` FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE (photoid IN (" + idlist + ")) "
			   + "ORDER BY instr('" + idlist + "',photoid)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetAlbumByIdOrderList(string idlist)
	   {
		   if (!Common.Utils.IsNumericArray(idlist.Split(',')))
		   {
			   return new DataTable();
		   }
		   string sql = "SELECT `albumid`,`title` FROM `" + BaseConfigs.GetTablePrefix + "albums` WHERE (albumid IN (" + idlist + ")) "
			   + "ORDER BY instr('" + idlist + "',albumid)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public DataTable GetWebSiteAggHotForumList()
	   {
		   //return DbHelper.ExecuteDataset("SELECT `fid`, `name` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE status=1 ORDER BY topics DESC, posts DESC, todayposts DESC").Tables[0];
		   return DbHelper.ExecuteDataset("SELECT `fid`, `name` FROM `" + BaseConfigs.GetTablePrefix + "forums` WHERE `status`=1 AND `layer`> 0 AND `fid` IN (SELECT `fid` FROM `" + BaseConfigs.GetTablePrefix + "forumfields` WHERE `password`='') ORDER BY `topics` DESC, `posts` DESC, `todayposts` DESC").Tables[0];

	   }

	   public DataTable GetWebSiteAggForumNewTopicList()
	   {
		   return DbHelper.ExecuteDataset("SELECT tid, title FROM `" + BaseConfigs.GetTablePrefix + "topics` WHERE `displayorder`>=0 AND `closed`<>1 ORDER BY `tid` DESC LIMIT 0,10").Tables[0];
	   }

	   private static string GetAlbumListCondition(string usernamelist, string titlelist, string descriptionlist, string startdate, string enddate)
	   {
            
		   string condition = "";
		   if (usernamelist != "")
		   {
			   string[] username = usernamelist.Split(',');
			   condition += " AND `username` in (";
			   string tempusernamelist = "";
			   foreach (string p in username)
			   {
				   tempusernamelist += "'" + p + "',";
			   }
			   if (tempusernamelist != "")
				   tempusernamelist = tempusernamelist.Substring(0, tempusernamelist.Length - 1);
			   condition += tempusernamelist + ")";
		   }
		   if (titlelist != "")
		   {
			   string[] title = titlelist.Split(',');
			   condition += " AND `title` in (";
			   string temptitlelist = "";
			   foreach (string p in title)
			   {
				   temptitlelist += "'" + p + "',";
			   }
			   if (temptitlelist != "")
				   temptitlelist = temptitlelist.Substring(0, temptitlelist.Length - 1);
			   condition += temptitlelist + ")";
		   }
		   if (descriptionlist != "")
		   {
			   string tempdescriptionlist = "";
			   foreach (string description in descriptionlist.Split(','))
			   {
				   tempdescriptionlist += " `description` LIKE '%" + RegEsc(description) + "%' OR";
			   }
			   tempdescriptionlist = tempdescriptionlist.Substring(0, tempdescriptionlist.Length - 2);
			   condition += " AND (" + tempdescriptionlist + ")";
		   }
		   if (startdate != "")
		   {
			   condition += " AND `createdatetime`>=?startdate";
		   }
		   if (enddate != "")
		   {
			   condition += " AND `createdatetime`<=?enddate";
		   }
		   return condition;
	   }

	   private string GetPhotoCondition(string photousernamelist, string keylist, string startdate, string enddate)
	   {
		   string condition = "";
		   if (photousernamelist != "")
		   {
			   string[] poster = photousernamelist.Split(',');
			   condition += " AND p.`username` in (";
			   string tempposerlist = "";
			   foreach (string p in poster)
			   {
				   tempposerlist += "'" + p + "',";
			   }
			   if (tempposerlist != "")
				   tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);
			   condition += tempposerlist + ")";
		   }
		   if (keylist != "")
		   {
			   string tempkeylist = "";
			   foreach (string key in keylist.Split(','))
			   {
				   tempkeylist += " p.`title` LIKE '%" + RegEsc(key) + "%' OR";
			   }
			   tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
			   condition += " AND (" + tempkeylist + ")";
		   }
		   if (startdate != "")
		   {
			   condition += " AND p.`postdate`>=?startdate";
		   }
		   if (enddate != "")
		   {
			   condition += " AND p.`postdate`<=?enddate";
		   }
		   return condition;
	   }


	   //private static string GetAlbumListCondition(string usernamelist, string titlelist, string descriptionlist, string startdate, string enddate)
	   //{
	   //    string condition = "";
	   //    if (usernamelist != "")
	   //    {
	   //        string[] username = usernamelist.Split(',');
	   //        condition += " AND `username` in (";
	   //        string tempusernamelist = "";
	   //        foreach (string p in username)
	   //        {
	   //            tempusernamelist += "'" + p + "',";
	   //        }
	   //        if (tempusernamelist != "")
	   //            tempusernamelist = tempusernamelist.Substring(0, tempusernamelist.Length - 1);
	   //        condition += tempusernamelist + ")";
	   //    }
	   //    if (titlelist != "")
	   //    {
	   //        string[] title = titlelist.Split(',');
	   //        condition += " AND `title` in (";
	   //        string temptitlelist = "";
	   //        foreach (string p in title)
	   //        {
	   //            temptitlelist += "'" + p + "',";
	   //        }
	   //        if (temptitlelist != "")
	   //            temptitlelist = temptitlelist.Substring(0, temptitlelist.Length - 1);
	   //        condition += temptitlelist + ")";
	   //    }
	   //    if (descriptionlist != "")
	   //    {
	   //        string tempdescriptionlist = "";
	   //        foreach (string description in descriptionlist.Split(','))
	   //        {
	   //            tempdescriptionlist += " `description` LIKE '%" + description + "%' OR";
	   //        }
	   //        tempdescriptionlist = tempdescriptionlist.Substring(0, tempdescriptionlist.Length - 2);
	   //        condition += " AND (" + tempdescriptionlist + ")";
	   //    }
	   //    if (startdate != "")
	   //    {
	   //        condition += " AND `createdatetime`>='" + startdate + " 00:00:00'";
	   //    }
	   //    if (enddate != "")
	   //    {
	   //        condition += " AND `createdatetime`<='" + enddate + " 23:59:59'";
	   //    }
	   //    return condition;
	   //}

	   private string GetSpaceCondition(string posterlist, string keylist, string startdate, string enddate)
	   {
		   string condition = "";
		   if (posterlist != "")
		   {
			   string[] poster = posterlist.Split(',');
			   condition += " AND `username` in (";
			   string tempposerlist = "";
			   foreach (string p in poster)
			   {
				   tempposerlist += "'" + p + "',";
			   }
			   if (tempposerlist != "")
				   tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);
			   condition += tempposerlist + ")";
		   }
		   if (keylist != "")
		   {
			   string tempkeylist = "";
			   foreach (string key in keylist.Split(','))
			   {
				   tempkeylist += " `spacetitle` LIKE '%" + RegEsc(key) + "%' OR";
			   }
			   tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
			   condition += " AND (" + tempkeylist + ")";
		   }
		   if (startdate != "")
		   {
			   condition += " AND `createdatetime`>=?startdate";
		   }
		   if (enddate != "")
		   {
			   condition += " AND `createdatetime`<=?enddate";
		   }
		   return condition;
	   }


	   public int GetSpacePostCountByCondition(string posterlist, string keylist, string startdate, string enddate)
	   {
		   string sql = "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE 1=1";
		   string condition = GetSpacePostCondition(posterlist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   if (condition != "")
			   sql += condition;
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,parms).ToString());
	   }

	   private string GetSpacePostCondition(string posterlist, string keylist, string startdate, string enddate)
	   {
		   string condition = "";
		   if (posterlist != "")
		   {
			   string[] poster = posterlist.Split(',');
			   condition += " AND `author` in (";
			   string tempposerlist = "";
			   foreach (string p in poster)
			   {
				   tempposerlist += "'" + p + "',";
			   }
			   if (tempposerlist != "")
				   tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);
			   condition += tempposerlist + ")";
		   }
		   if (keylist != "")
		   {
			   string tempkeylist = "";
			   foreach (string key in keylist.Split(','))
			   {
				   tempkeylist += " `title` LIKE '%" + RegEsc(key) + "%' OR";
			   }
			   tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
			   condition += " AND (" + tempkeylist + ")";
		   }
		   if (startdate != "")
		   {
			   condition += " AND `postdatetime`>=?startdate";
		   }
		   if (enddate != "")
		   {
			   condition += " AND `postdatetime`<=?enddate";
		   }
		   return condition;
	   }

	   public DataTable GetSpacePostByCondition(string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
	   {
		   string sql = "";
		   string condition = GetSpacePostCondition(posterlist, keylist, startdate, enddate);
		   IDataParameter[] parms = GetParms(startdate, enddate);
		   int pageTop = (currentPage - 1) * pageSize;
		   //if (currentPage == 1)
		   //{

		   sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE 1=1" + condition + " ORDER BY `postid` DESC LIMIT "+pageTop
			   +","+pageSize.ToString();
		   //}
		   //else
		   //{
		   //    sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `postid`<(SELECT MIN(`postid`) FROM (SELECT `postid` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE 1=1 " + condition + " ORDER BY `postid` DESC LIMIT 0,"+pageTop+") AS tblTmp)" + condition + " ORDER BY `postid` DESC LIMIT 0,"+pageSize.ToString();
		   //}
		   return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
	   }

	   public DataTable GetSpacepostLitByTidlist(string postidlist)
	   {
		   string sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "spaceposts`"
			   + "WHERE (`postid` IN (" + postidlist + ")) ORDER BY INSTR('" + postidlist + "',`postid`)";
		   return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
	   }

	   public string[] GetSpaceLastPostInfo(int userid)
	   {
		   IDataParameter pram = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);
		   string sql = "SELECT `postid`,`title` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `uid`=?uid ORDER BY `postdatetime` DESC LIMIT 0,1";
		   DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql, pram).Tables[0];
		   string[] result = new string[2];
		   if (dt != null && dt.Rows.Count != 0)
		   {
			   result[0] = dt.Rows[0]["postid"].ToString();
			   result[1] = dt.Rows[0]["title"].ToString().Trim();
		   }
		   else
		   {
			   result[0] = "0";
			   result[1] = "";
		   }
		   return result;
	   }

	   public DataTable GetWebSiteAggTopSpacePostList(string orderby, int topnumber)
	   {
		   return DbHelper.ExecuteDataset(" SELECT `postid`,`title`,`author`,`uid`,`postdatetime`,`commentcount`,`views` FROM `" + BaseConfigs.GetTablePrefix + "spaceposts` WHERE `poststatus` = 1 ORDER BY `" + orderby + "` DESC LIMIT 0," + topnumber).Tables[0];
	   }

	   public DataTable GetWebSiteAggSpaceTopComments(int topnumber)
	   {
		   return DbHelper.ExecuteDataset(CommandType.Text, "SELECT `postid`,`content`,`posttitle`,`author`,`uid` FROM `" + BaseConfigs.GetTablePrefix + "spacecomments` ORDER BY `commentid` DESC LIMIT 0,"+topnumber).Tables[0];
	   }

	   public int GetUidByAlbumid(int albumid)
	   {
		   IDataParameter pram = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
		   string sql = "select `userid` from `" + BaseConfigs.GetTablePrefix + "albums` where `albumid`=?albumid";
		   return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, pram).ToString());
	   }
	  
	   #endregion
    }
}

#endif