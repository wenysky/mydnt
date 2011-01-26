#if NET1

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using SQLDMO;

namespace Discuz.Data.Access
{
	/// <summary>
	/// DataProvider
	/// </summary>
	public class DataProvider: IDataProvider
	{		
        #region ForumManage
        /// <summary>
        /// SQL SERVER SQL���ת��
        /// </summary>
        /// <param name="str">��Ҫת��Ĺؼ��ַ���</param>
        /// <param name="pattern">��Ҫת����ַ�����</param>
        /// <returns>ת�����ַ���</returns>
        private static string RegEsc(string str)
        {
            string[] pattern ={ @"%", @"_", @"'" };
            foreach (string s in pattern)
            {
                //Regex rgx = new Regex(s);
                //keyword = rgx.Replace(keyword, "\\" + s);
                switch (s)
                {
                    case "%":
                        str = str.Replace(s, "[%]");
                        break;
                    case "_":
                        str = str.Replace(s, "[_]");
                        break;
                    case "'":
                        str = str.Replace(s, "['']");
                        break;

                }

            }
            return str;
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="displayorder">��ʾ˳��</param>
        /// <param name="name">����</param>
        /// <param name="url">���ӵ�ַ</param>
        /// <param name="note">��ע</param>
        /// <param name="logo">Logo��ַ</param>
        /// <returns></returns>
        public int AddForumLink(int displayorder, string name, string url, string note, string logo)
        {
            
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar, 100, name),
                                        DbHelper.MakeInParam("@url", (DbType)OleDbType.VarWChar, 100, url),
                                        DbHelper.MakeInParam("@note", (DbType)OleDbType.VarWChar, 200, note),
                                        DbHelper.MakeInParam("@logo", (DbType)OleDbType.VarWChar, 100, logo)
                                    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "forumlinks] ([displayorder], [name],[url],[note],[logo]) VALUES (@displayorder,@name,@url,@note,@logo)";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// ���������������
        /// </summary>
        /// <returns></returns>
        public string GetForumLinks()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forumlinks]";
        }

        /// <summary>
        /// ɾ��ָ����������
        /// </summary>
        /// <param name="forumlinkid"></param>
        /// <returns></returns>
        public int DeleteForumLink(string forumlinkidlist)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forumlinks] WHERE [id] IN (" + forumlinkidlist + ")";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// ����ָ����������
        /// </summary>
        /// <param name="id">��������Id</param>
        /// <param name="displayorder">��ʾ˳��</param>
        /// <param name="name">����</param>
        /// <param name="url">���ӵ�ַ</param>
        /// <param name="note">��ע</param>
        /// <param name="logo">Logo��ַ</param>
        /// <returns></returns>
        public int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar, 100, name),
                                        DbHelper.MakeInParam("@url", (DbType)OleDbType.VarWChar, 100, url),
                                        DbHelper.MakeInParam("@note", (DbType)OleDbType.VarWChar, 200, note),
                                        DbHelper.MakeInParam("@logo", (DbType)OleDbType.VarWChar, 100, logo)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumlinks] SET [displayorder]=@displayorder,[name]=@name,[url]=@url,[note]=@note,[logo]=@logo WHERE [id]=@id";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        /// <summary>
        /// �����ҳ����б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetForumIndexListTable()
        {
            string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [{0}forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// �����ҳ����б�
        /// </summary>
        /// <returns></returns>
        public IDataReader GetForumIndexList()
        {
            string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [{0}forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// ��ü�����̳��ҳ�б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetArchiverForumIndexList()
        {
            string commandText = string.Format("SELECT [{0}forums].[fid], [{0}forums].[name],[{0}forums].[parentidlist], [{0}forums].[status],[{0}forums].[layer], [{0}forumfields].[viewperm] FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid]   ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// ����Ӱ���б�
        /// </summary>
        /// <param name="fid">���id</param>
        /// <returns></returns>
        public IDataReader GetSubForumReader(int fid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer,4, fid)
								   };

            string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [parentid] = @fid AND [status] > 0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, prams);
        }

        /// <summary>
        /// ����Ӱ���б�
        /// </summary>
        /// <param name="fid">���id</param>
        /// <returns></returns>
        public DataTable GetSubForumTable(int fid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer,4, fid)
								   };

            string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [parentid] = @fid AND [status] > 0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, prams).Tables[0];
        }

        /// <summary>
        /// ���ȫ������б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetForumsTable()
        {
            string commandText = string.Format("SELECT [{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// ���õ�ǰ���������(�����Ӱ��)
        /// </summary>
        /// <param name="fid">���id</param>
        /// <returns>������</returns>
        public int SetRealCurrentTopics(int fid)
        {
            string commandText =
                string.Format("UPDATE {0}forums SET [curtopics] = (SELECT COUNT(tid) FROM {0}topics WHERE [displayorder] >= 0 AND [fid]={1}) WHERE [fid]={1}", BaseConfigs.GetTablePrefix, fid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DataTable GetForumListTable()
        {
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [name], [fid] FROM [{0}forums] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix)).Tables[0];

            return dt;
        }

        public string GetTemplates()
        {
            return "SELECT [templateid],[name] FROM [" + BaseConfigs.GetTablePrefix + "templates] ";

        }

        public DataTable GetUserGroupsTitle()
        {
            string sql = "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  ORDER BY [groupid] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserGroupMaxspacephotosize()
        {
            string sql = "SELECT [groupid],[grouptitle],[maxspacephotosize] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  ORDER BY [groupid] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserGroupMaxspaceattachsize()
        {
            string sql = "SELECT [groupid],[grouptitle],[maxspaceattachsize] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  ORDER BY [groupid] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetAttachTypes()
        {
            string sql = "SELECT [id],[extension] FROM [" + BaseConfigs.GetTablePrefix + "attachtypes]  ORDER BY [id] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetForums()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [displayorder] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public string GetForumsTree()
        {
            return "SELECT [fid],[name],[parentid] FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [displayorder] ASC";
        }

        public int GetForumsMaxDisplayOrder()
        {
            return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(displayorder) FROM [" + BaseConfigs.GetTablePrefix + "forums]").Tables[0].Rows[0][0], 0) + 1;
        }

        public DataTable GetForumsMaxDisplayOrder(int parentid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX([displayorder]) FROM [" + BaseConfigs.GetTablePrefix + "forums]  WHERE [parentid]=" + parentid).Tables[0];
        }
        public void UpdateForumsDisplayOrder(int minDisplayOrder)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1  WHERE [displayorder]>" + minDisplayOrder.ToString());
        }

        public void UpdateSubForumCount(int fid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]+1  WHERE [fid]=" + fid.ToString());
        }

        public DataRow GetForum(int fid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + fid.ToString()).Tables[0].Rows[0];
        }

        public DataRowCollection GetModerators(int fid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [inherited]=1 AND [fid]=" + fid + ")").Tables[0].Rows;
        }

        public DataTable GetTopForum(int fid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=0 AND [layer]=0 AND [fid]=" + fid).Tables[0];
        }

        public int UpdateForum(int fid, string name, int subforumcount, int displayorder)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid),
                                        DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar, 50, name),
                                        DbHelper.MakeInParam("@subforumcount", (DbType)OleDbType.Integer, 4, subforumcount),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder) 
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [name]=@name,[subforumcount]=@subforumcount ,[displayorder]=@displayorder WHERE [fid]=@fid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public DataTable GetForumField(int fid, string fieldname)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [" + fieldname + "] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [fid]=" + fid).Tables[0];
        }

        public int UpdateForumField(int fid, string fieldname)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [" + fieldname + "]='' WHERE [fid]=" + fid);
        }

        public int UpdateForumField(int fid, string fieldname, string fieldvalue)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [" + fieldname + "]='" + fieldvalue + "' WHERE [fid]=" + fid);
        }

        public DataRowCollection GetDatechTableIds()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT id FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows;
        }

        public int UpdateMinMaxField(string posttablename, int posttableid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "tablelist] SET [mintid]=" + GetMinPostTableTid(posttablename) + ",[maxtid]=" + GetMaxPostTableTid(posttablename) + "  WHERE [id]=" + posttableid);
        }

        public DataRowCollection GetForumIdList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums]").Tables[0].Rows;
        }

        public int CreateFullTextIndex(string dbname)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("USE " + dbname + ";");
            sb.Append("execute sp_fulltext_database 'enable';");
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }

        public int GetMaxForumId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(fid), 0) FROM " + BaseConfigs.GetTablePrefix + "forums"), 0);
        }

        public DataTable GetForumList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[name] FROM [" + BaseConfigs.GetTablePrefix + "forums]").Tables[0];
        }

        public DataTable GetAllForumList()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [displayorder] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetForumInformation(int fid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer, 4,fid)
			};
            string sql = "SELECT [" + BaseConfigs.GetTablePrefix + "forums].*, [" + BaseConfigs.GetTablePrefix + "forumfields].* FROM [" + BaseConfigs.GetTablePrefix + "forums] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] ON [" + BaseConfigs.GetTablePrefix + "forums].[fid]=[" + BaseConfigs.GetTablePrefix + "forumfields].[fid] WHERE [" + BaseConfigs.GetTablePrefix + "forums].[fid]=@fid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void SaveForumsInfo(ForumInfo foruminfo)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DbParameter[] prams = {
						DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar, 50, foruminfo.Name),
						DbHelper.MakeInParam("@status", (DbType)OleDbType.Integer, 4, foruminfo.Status),
						DbHelper.MakeInParam("@colcount", (DbType)OleDbType.Integer, 4, foruminfo.Colcount),
						DbHelper.MakeInParam("@templateid", (DbType)OleDbType.Integer, 2, foruminfo.Templateid),
						DbHelper.MakeInParam("@allowsmilies", (DbType)OleDbType.Integer, 4, foruminfo.Allowsmilies),
						DbHelper.MakeInParam("@allowrss", (DbType)OleDbType.Integer, 6, foruminfo.Allowrss),
						DbHelper.MakeInParam("@allowhtml", (DbType)OleDbType.Integer, 4, foruminfo.Allowhtml),
						DbHelper.MakeInParam("@allowbbcode", (DbType)OleDbType.Integer, 4, foruminfo.Allowbbcode),
						DbHelper.MakeInParam("@allowimgcode", (DbType)OleDbType.Integer, 4, foruminfo.Allowimgcode),
						DbHelper.MakeInParam("@allowblog", (DbType)OleDbType.Integer, 4, foruminfo.Allowblog),
						DbHelper.MakeInParam("@alloweditrules", (DbType)OleDbType.Integer, 4, foruminfo.Alloweditrules),
						DbHelper.MakeInParam("@allowthumbnail", (DbType)OleDbType.Integer, 4, foruminfo.Allowthumbnail),
                        DbHelper.MakeInParam("@allowtag",(DbType)OleDbType.Integer,4,foruminfo.Allowtag),
                        DbHelper.MakeInParam("@istrade", (DbType)OleDbType.Integer, 4, foruminfo.Istrade),
						DbHelper.MakeInParam("@recyclebin", (DbType)OleDbType.Integer, 4, foruminfo.Recyclebin),
						DbHelper.MakeInParam("@modnewposts", (DbType)OleDbType.Integer, 4, foruminfo.Modnewposts),
						DbHelper.MakeInParam("@jammer", (DbType)OleDbType.Integer, 4, foruminfo.Jammer),
						DbHelper.MakeInParam("@disablewatermark", (DbType)OleDbType.Integer, 4, foruminfo.Disablewatermark),
						DbHelper.MakeInParam("@inheritedmod", (DbType)OleDbType.Integer, 4, foruminfo.Inheritedmod),
						DbHelper.MakeInParam("@autoclose", (DbType)OleDbType.Integer, 2, foruminfo.Autoclose),
						DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, foruminfo.Displayorder),
                        DbHelper.MakeInParam("@allowpostspecial",(DbType)OleDbType.Integer,4,foruminfo.Allowpostspecial),
                        DbHelper.MakeInParam("@allowspecialonly",(DbType)OleDbType.Integer,4,foruminfo.Allowspecialonly),
						DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, foruminfo.Fid)
					};
                    string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [name]=@name, [status]=@status, [colcount]=@colcount, [templateid]=@templateid,"
                        + "[allowsmilies]=@allowsmilies ,[allowrss]=@allowrss, [allowhtml]=@allowhtml, [allowbbcode]=@allowbbcode, [allowimgcode]=@allowimgcode, "
                        + "[allowblog]=@allowblog,[istrade]=@istrade,[alloweditrules]=@alloweditrules ,[allowthumbnail]=@allowthumbnail ,[allowtag]=@allowtag,"
                        + "[recyclebin]=@recyclebin, [modnewposts]=@modnewposts,[jammer]=@jammer,[disablewatermark]=@disablewatermark,[inheritedmod]=@inheritedmod,"
                        + "[autoclose]=@autoclose,[displayorder]=@displayorder,[allowpostspecial]=@allowpostspecial,[allowspecialonly]=@allowspecialonly WHERE [fid]=@fid";
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prams);

                    DbParameter[] prams1 = {
						DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 0, foruminfo.Description),
						DbHelper.MakeInParam("@password", (DbType)OleDbType.VarWChar, 16, foruminfo.Password),
						DbHelper.MakeInParam("@icon", (DbType)OleDbType.VarChar, 255, foruminfo.Icon),
						DbHelper.MakeInParam("@redirect", (DbType)OleDbType.VarChar, 255, foruminfo.Redirect),
						DbHelper.MakeInParam("@attachextensions", (DbType)OleDbType.VarChar, 255, foruminfo.Attachextensions),
						DbHelper.MakeInParam("@rules", (DbType)OleDbType.VarWChar, 0, foruminfo.Rules),
						DbHelper.MakeInParam("@topictypes", (DbType)OleDbType.VarWChar, 0, foruminfo.Topictypes),
						DbHelper.MakeInParam("@viewperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Viewperm),
						DbHelper.MakeInParam("@postperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Postperm),
						DbHelper.MakeInParam("@replyperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Replyperm),
						DbHelper.MakeInParam("@getattachperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Getattachperm),
						DbHelper.MakeInParam("@postattachperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Postattachperm),
                        DbHelper.MakeInParam("@applytopictype", (DbType)OleDbType.TinyInt, 1, foruminfo.Applytopictype),
						DbHelper.MakeInParam("@postbytopictype", (DbType)OleDbType.TinyInt, 1, foruminfo.Postbytopictype),
						DbHelper.MakeInParam("@viewbytopictype", (DbType)OleDbType.TinyInt, 1, foruminfo.Viewbytopictype),
						DbHelper.MakeInParam("@topictypeprefix", (DbType)OleDbType.TinyInt, 1, foruminfo.Topictypeprefix),
						DbHelper.MakeInParam("@permuserlist", (DbType)OleDbType.VarWChar, 0, foruminfo.Permuserlist),
						DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, foruminfo.Fid)
					};
                    sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [description]=@description,[password]=@password,[icon]=@icon,[redirect]=@redirect,"
                         + "[attachextensions]=@attachextensions,[rules]=@rules,[topictypes]=@topictypes,[viewperm]=@viewperm,[postperm]=@postperm,[replyperm]=@replyperm,"
                         + "[getattachperm]=@getattachperm,[postattachperm]=@postattachperm,[applytopictype]=@applytopictype,[postbytopictype]=@postbytopictype,"
                         + "[viewbytopictype]=@viewbytopictype,[topictypeprefix]=@topictypeprefix,[permuserlist]=@permuserlist WHERE [fid]=@fid";
					
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prams1);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            conn.Close();
        }

        public int InsertForumsInf(ForumInfo foruminfo)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@parentid", (DbType)OleDbType.Integer, 2, foruminfo.Parentid),
				DbHelper.MakeInParam("@layer", (DbType)OleDbType.Integer, 4, foruminfo.Layer),
				DbHelper.MakeInParam("@pathlist", (DbType)OleDbType.VarWChar, 3000, foruminfo.Pathlist == null ? " " : foruminfo.Pathlist),
				DbHelper.MakeInParam("@parentidlist", (DbType)OleDbType.VarWChar, 300, foruminfo.Parentidlist== null ? " " : foruminfo.Parentidlist),
				DbHelper.MakeInParam("@subforumcount", (DbType)OleDbType.Integer, 4, foruminfo.Subforumcount),
				DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar, 50, foruminfo.Name),
				DbHelper.MakeInParam("@status", (DbType)OleDbType.Integer, 4, foruminfo.Status),
				DbHelper.MakeInParam("@colcount", (DbType)OleDbType.Integer, 4, foruminfo.Colcount),
				DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, foruminfo.Displayorder),
				DbHelper.MakeInParam("@templateid", (DbType)OleDbType.Integer, 2, foruminfo.Templateid),
				DbHelper.MakeInParam("@allowsmilies", (DbType)OleDbType.Integer, 4, foruminfo.Allowsmilies),
				DbHelper.MakeInParam("@allowrss", (DbType)OleDbType.Integer, 6, foruminfo.Allowrss),
				DbHelper.MakeInParam("@allowhtml", (DbType)OleDbType.Integer, 4, foruminfo.Allowhtml),
				DbHelper.MakeInParam("@allowbbcode", (DbType)OleDbType.Integer, 4, foruminfo.Allowbbcode),
				DbHelper.MakeInParam("@allowimgcode", (DbType)OleDbType.Integer, 4, foruminfo.Allowimgcode),
				DbHelper.MakeInParam("@allowblog", (DbType)OleDbType.Integer, 4, foruminfo.Allowblog),
				DbHelper.MakeInParam("@istrade", (DbType)OleDbType.Integer, 4, foruminfo.Istrade),
				DbHelper.MakeInParam("@alloweditrules", (DbType)OleDbType.Integer, 4, foruminfo.Alloweditrules),
				DbHelper.MakeInParam("@allowthumbnail", (DbType)OleDbType.Integer, 4, foruminfo.Allowthumbnail),
                DbHelper.MakeInParam("@allowtag",(DbType)OleDbType.Integer,4,foruminfo.Allowtag),
				DbHelper.MakeInParam("@recyclebin", (DbType)OleDbType.Integer, 4, foruminfo.Recyclebin),
				DbHelper.MakeInParam("@modnewposts", (DbType)OleDbType.Integer, 4, foruminfo.Modnewposts),
				DbHelper.MakeInParam("@jammer", (DbType)OleDbType.Integer, 4, foruminfo.Jammer),
				DbHelper.MakeInParam("@disablewatermark", (DbType)OleDbType.Integer, 4, foruminfo.Disablewatermark),
				DbHelper.MakeInParam("@inheritedmod", (DbType)OleDbType.Integer, 4, foruminfo.Inheritedmod),
				DbHelper.MakeInParam("@autoclose", (DbType)OleDbType.Integer, 2, foruminfo.Autoclose),                
                DbHelper.MakeInParam("@allowpostspecial",(DbType)OleDbType.Integer,4,foruminfo.Allowpostspecial),
                DbHelper.MakeInParam("@allowspecialonly",(DbType)OleDbType.Integer,4,foruminfo.Allowspecialonly),
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "forums] ([parentid],[layer],[pathlist],[parentidlist],[subforumcount],[name],"
                + "[status],[colcount],[displayorder],[templateid],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],"
                + "[istrade],[alloweditrules],[recyclebin],[modnewposts],[jammer],[disablewatermark],[inheritedmod],[autoclose],[allowthumbnail],"
                + "[allowtag],[allowpostspecial],[allowspecialonly]) VALUES (@parentid,@layer,@pathlist,@parentidlist,@subforumcount,@name,@status, @colcount, @displayorder,"
                + "@templateid,@allowsmilies,@allowrss,@allowhtml,@allowbbcode,@allowimgcode,@allowblog,@istrade,@alloweditrules,@recyclebin,"
                + "@modnewposts,@jammer,@disablewatermark,@inheritedmod,@autoclose,@allowthumbnail,@allowtag,@allowpostspecial,@allowspecialonly)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);

            int fid = GetMaxForumId();

            DbParameter[] prams1 = {
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid),
				DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 0, foruminfo.Description),
				DbHelper.MakeInParam("@password", (DbType)OleDbType.VarChar, 16, foruminfo.Password),
				DbHelper.MakeInParam("@icon", (DbType)OleDbType.VarChar, 255, foruminfo.Icon),
				DbHelper.MakeInParam("@postcredits", (DbType)OleDbType.VarChar, 255, foruminfo.Postcredits),
				DbHelper.MakeInParam("@replycredits", (DbType)OleDbType.VarChar, 255, foruminfo.Replycredits),
				DbHelper.MakeInParam("@redirect", (DbType)OleDbType.VarChar, 255, foruminfo.Redirect),
				DbHelper.MakeInParam("@attachextensions", (DbType)OleDbType.VarChar, 255, foruminfo.Attachextensions),
				DbHelper.MakeInParam("@moderators", (DbType)OleDbType.VarWChar, 0, foruminfo.Moderators),
				DbHelper.MakeInParam("@rules", (DbType)OleDbType.VarWChar, 0, foruminfo.Rules),
				DbHelper.MakeInParam("@topictypes", (DbType)OleDbType.VarWChar, 0, foruminfo.Topictypes),
				DbHelper.MakeInParam("@viewperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Viewperm),
				DbHelper.MakeInParam("@postperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Postperm),
				DbHelper.MakeInParam("@replyperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Replyperm),
				DbHelper.MakeInParam("@getattachperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Getattachperm),
				DbHelper.MakeInParam("@postattachperm", (DbType)OleDbType.VarWChar, 0, foruminfo.Postattachperm)
			};
            sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "forumfields] ([fid],[description],[password],[icon],[postcredits],[replycredits],[redirect],[attachextensions],[moderators],[rules],[topictypes],[viewperm],[postperm],[replyperm],[getattachperm],[postattachperm]) VALUES (@fid,@description,@password,@icon,@postcredits,@replycredits,@redirect,@attachextensions,@moderators,@rules,@topictypes,@viewperm,@postperm,@replyperm,@getattachperm,@postattachperm)";
            DbHelper.ExecuteDataset(CommandType.Text, sql, prams1);
            return fid;
        }

        public void SetForumsPathList(string pathlist, int fid)
        {
            DbParameter[] prams = 
            {
			    DbHelper.MakeInParam("@pathlist", (DbType)OleDbType.VarChar, 3000, pathlist),
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
		    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET pathlist=@pathlist  WHERE [fid]=@fid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void SetForumslayer(int layer, string parentidlist, int fid)
        {
            DbParameter[] prams = 
            {
                DbHelper.MakeInParam("@layer", (DbType)OleDbType.Integer, 2, layer),
                DbHelper.MakeInParam("@parentidlist", (DbType)OleDbType.Char, 300, parentidlist),
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [layer]=@layer WHERE [fid]=@fid", prams);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentidlist]=@parentidlist WHERE [fid]=@fid", prams);
        }

        public int GetForumsParentidByFid(int fid)
        {
            DbParameter[] prams = 
            {
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
		    };
            string sql = "SELECT TOP 1 [parentid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE fid=@fid";
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
        }

        public void MovingForumsPos(string currentfid, string targetfid, bool isaschildnode, string extname)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();

            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    //ȡ�õ�ǰ��̳������Ϣ
                    DataRow dr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + currentfid).Tables[0].Rows[0];

                    //ȡ��Ŀ����̳������Ϣ
                    DataRow targetdr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + targetfid).Tables[0].Rows[0];

                    //��ǰ��̳�����Ӱ��ʱ
                    if (DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 FID FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=" + currentfid).Tables[0].Rows.Count > 0)
                    {
                        #region

                        string sqlstring = "";
                        if (isaschildnode) //��Ϊ��̳�Ӱ�����
                        {
                            //��λ�ڵ�ǰ��̳���(����)��ʾ˳��֮�����̳���ȫ����1(Ϊ�¼������̳�����λ���)
                            sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0}",
                                                      Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1));
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

                            //���µ�ǰ��̳���������Ϣ
                            sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentid]='{1}',[displayorder]='{2}' WHERE [fid]={0}", currentfid, targetdr["fid"].ToString(), Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()) + 1));
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);
                        }
                        else //��Ϊͬ����̳���,��Ŀ����̳���֮ǰ����
                        {
                            //��λ�ڰ�����ǰ��̳�����ʾ˳��֮�����̳���ȫ����1(Ϊ�¼������̳�����λ���)
                            sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0} OR [fid]={1}",
                                                      Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString())),
                                                      targetdr["fid"].ToString());
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

                            //���µ�ǰ��̳���������Ϣ
                            sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentid]='{1}',[displayorder]='{2}'  WHERE [fid]={0}", currentfid, targetdr["parentid"].ToString(), Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim())));
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);
                        }

                        //������������������Ӱ��İ������������
                        if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
                        {
                            if (dr["parentidlist"].ToString().Trim() != "")
                            {
                                DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]-" + dr["topics"].ToString() + ",[posts]=[posts]-" + dr["posts"].ToString() + "  WHERE [fid] IN(" + dr["parentidlist"].ToString().Trim() + ")");
                            }
                            if (targetdr["parentidlist"].ToString().Trim() != "")
                            {
                                DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]+" + dr["topics"].ToString() + ",[posts]=[posts]+" + dr["posts"].ToString() + "  WHERE [fid] IN(" + targetdr["parentidlist"].ToString().Trim() + ")");
                            }
                        }

                        #endregion
                    }
                    else //��ǰ��̳��鲻���Ӱ�
                    {
                        #region

                        //���þɵĸ�һ��������̳��
                        DbHelper.ExecuteDataset(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]-1 WHERE [fid]=" + dr["parentid"].ToString());

                        //��λ�ڵ�ǰ�ڵ���ʾ˳��֮��Ľڵ�ȫ����1 [��ɾ���ڵ��Ч��]
                        if (isaschildnode) //��Ϊ����̳������
                        {
                            //������Ӧ�ı�Ӱ��İ������������
                            if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
                            {
                                DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]-" + dr["topics"].ToString() + ",[posts]=[posts]-" + dr["posts"].ToString() + " WHERE [fid] IN(" + dr["parentidlist"].ToString() + ")");
                                if (targetdr["parentidlist"].ToString().Trim() != "")
                                {
                                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]+" + dr["topics"].ToString() + ",[posts]=[posts]+" + dr["posts"].ToString() + " WHERE [fid] IN(" + targetdr["parentidlist"].ToString() + "," + targetfid + ")");
                                }
                            }

                            //��λ�ڵ�ǰ��̳�����ʾ˳��֮�����̳���ȫ����1(Ϊ�¼������̳�����λ���)
                            string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0}",
                                                             Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1));
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

                            //�����µĸ�һ��������̳��
                            DbHelper.ExecuteDataset(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]+1 WHERE [fid]=" + targetfid);

                            string parentidlist = null;
                            if (targetdr["parentidlist"].ToString().Trim() == "0")
                            {
                                parentidlist = targetfid;
                            }
                            else
                            {
                                parentidlist = targetdr["parentidlist"].ToString().Trim() + "," + targetfid;
                            }

                            //���µ�ǰ��̳���������Ϣ
                            sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentid]='{1}',[layer]='{2}',[pathlist]='{3}', [parentidlist]='{4}',[displayorder]='{5}' WHERE [fid]={0}",
                                                      currentfid,
                                                      targetdr["fid"].ToString(),
                                                      Convert.ToString(Convert.ToInt32(targetdr["layer"].ToString()) + 1),
                                                      targetdr["pathlist"].ToString().Trim() + "<a href=\"showforum-" + currentfid + extname + "\">" + dr["name"].ToString().Trim().Replace("'","''") + "</a>",
                                                      parentidlist,
                                                      Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()) + 1)
                                );
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

                        }
                        else //��Ϊͬ����̳���,��Ŀ����̳���֮ǰ����
                        {
                            //������Ӧ�ı�Ӱ��İ������������
                            if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
                            {
                                DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET [topics]=[topics]-" + dr["topics"].ToString() + ",[posts]=[posts]-" + dr["posts"].ToString() + "  WHERE [fid] IN(" + dr["parentidlist"].ToString() + ")");
                                DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET [topics]=[topics]+" + dr["topics"].ToString() + ",[posts]=[posts]+" + dr["posts"].ToString() + "  WHERE [fid] IN(" + targetdr["parentidlist"].ToString() + ")");
                            }

                            //��λ�ڰ�����ǰ��̳�����ʾ˳��֮�����̳���ȫ����1(Ϊ�¼������̳�����λ���)
                            string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0} OR [fid]={1}",
                                                             Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1),
                                                             targetdr["fid"].ToString());
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

                            //�����µĸ�һ��������̳��
                            DbHelper.ExecuteDataset(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums]  SET [subforumcount]=[subforumcount]+1 WHERE [fid]=" + targetdr["parentid"].ToString());
                            string parentpathlist = "";
                            DataTable dt = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 [pathlist] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + targetdr["parentid"].ToString()).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                parentpathlist = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 [pathlist] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + targetdr["parentid"].ToString()).Tables[0].Rows[0][0].ToString().Trim();
                            }

                            //���µ�ǰ��̳���������Ϣ
                            sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums]  SET [parentid]='{1}',[layer]='{2}',[pathlist]='{3}', [parentidlist]='{4}',[displayorder]='{5}' WHERE [fid]={0}",
                                                      currentfid,
                                                      targetdr["parentid"].ToString(),
                                                      Convert.ToInt32(targetdr["layer"].ToString()),
                                                      parentpathlist + "<a href=\"showforum-" + currentfid + extname + "\">" + dr["name"].ToString().Trim() + "</a>",
                                                      targetdr["parentidlist"].ToString().Trim(),
                                                      Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()))
                                );
                            DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);
                        }

                        #endregion
                    }
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }
        }

        public bool IsExistSubForum(int fid)
        {
            DbParameter[] prams = 
            {
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
		    };
            string sql = "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=@fid";
            if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void DeleteForumsByFid(string postname, string fid)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    //��ȡ����ǰ�ڵ����Ϣ
                    DataRow dr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + fid).Tables[0].Rows[0];

                    //�����ڵ�ǰ�ڵ�����λ��֮��Ľڵ�,����1����
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]-1 WHERE [displayorder]>" + dr["displayorder"].ToString());

                    //�޸ĸ�����е�����̳����
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]-1 WHERE  [fid]=" + dr["parentid"].ToString());

                    //ɾ����ǰ�ڵ�ĸ߼����Բ���
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [fid]=" + fid);

                    //ɾ�����ͶƱ����Ϣ
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid] IN(SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + ")");

                    //ɾ�����Ӹ������е���Ϣ
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid] IN(SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + ") OR [pid] IN(SELECT [pid] FROM [" + postname + "] WHERE [fid]=" + fid + ")");

                    //ɾ���������
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + postname + "] WHERE [fid]=" + fid);

                    //ɾ���������
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid);


                    //ɾ����ǰ�ڵ�
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE  [fid]=" + fid);

                    //ɾ�������б��е������Ϣ
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE  [fid]=" + fid);

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
            DbParameter[] prams = 
            {
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
		    };
            string sql = "SELECT [parentid] From [" + BaseConfigs.GetTablePrefix + "forums] WHERE [inheritedmod]=1 AND [fid]=@fid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    int count = displayorder;


                    //���ݿ��д��ڵ��û�
                    string usernamelist = "";
                    //���������̳�İ�������
                    foreach (string username in moderators.Split(','))
                    {
                        if (username.Trim() != "")
                        {
                            DbParameter[] prams =
								{
									DbHelper.MakeInParam("@username", (DbType)OleDbType.VarChar, 20, username.Trim())
								};
                            //��ȡ����ǰ�ڵ����Ϣ
                            DataTable dt = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]<>7 AND [groupid]<>8 AND [username]=@username", prams).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                DbHelper.ExecuteNonQuery(trans, CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "moderators] ([uid],[fid],[displayorder],[inherited]) VALUES(" + dt.Rows[0][0].ToString() + "," + fid + "," + count.ToString() + "," + inherited.ToString() + ")");
                                usernamelist = usernamelist + username.Trim() + ",";
                                count++;
                            }
                        }
                    }

                    if (usernamelist != "")
                    {
                        DbParameter[] prams1 =
							{
								DbHelper.MakeInParam("@moderators", (DbType)OleDbType.VarChar, 255, usernamelist.Substring(0, usernamelist.Length - 1))

							};
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]=@moderators WHERE [fid] =" + fid, prams1);
                    }
                    else
                    {
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]='' WHERE [fid] =" + fid);
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            conn.Close();
        }

        public DataTable GetFidInForumsByParentid(int parentid)
        {
            DbParameter[] prams =
			{
                DbHelper.MakeInParam("@parentid", (DbType)OleDbType.Integer, 4, parentid)
			};
            string sql = "SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=@parentid ORDER BY [displayorder] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void CombinationForums(string sourcefid, string targetfid, string fidlist)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    //ChildNode = "0";
                    //string fidlist = ("," + FindChildNode(targetfid)).Replace(",0,", "");
                    //�����������������Ϣ
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [fid]=" + targetfid + "  WHERE [fid]=" + sourcefid);
                    //Ҫ����Ŀ����̳��������
                    int totaltopics = Convert.ToInt32(DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT COUNT(tid)  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid] IN(" + fidlist + ")").Tables[0].Rows[0][0].ToString());

                    int totalposts = 0;
                    foreach (DataRow postdr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id] From [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
                    {
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + postdr["id"].ToString() + "] SET [fid]=" + targetfid + "  WHERE [fid]=" + sourcefid);

                        //Ҫ����Ŀ����̳��������
                        totalposts = totalposts + Convert.ToInt32(DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT COUNT(pid)  FROM [" + BaseConfigs.GetTablePrefix + "posts" + postdr["id"].ToString() + "] WHERE [fid] IN(" + fidlist + ")").Tables[0].Rows[0][0].ToString());
                    }

                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=" + totaltopics + " ,[posts]=" + totalposts + " WHERE [fid]=" + targetfid);

                    //��ȡԴ��̳��Ϣ
                    DataRow dr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + sourcefid).Tables[0].Rows[0];

                    //�����ڵ�ǰ�ڵ�����λ��֮��Ľڵ�,����1����
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]-1 WHERE [displayorder]>" + dr["displayorder"].ToString());

                    //�޸ĸ�����е�����̳����
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]-1 WHERE [fid]=" + dr["parentid"].ToString());

                    //ɾ����ǰ�ڵ�ĸ߼����Բ���
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [fid]=" + sourcefid);

                    //ɾ��Դ��̳���
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + sourcefid);
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            conn.Close();
        }

        public void UpdateSubForumCount(int subforumcount, int fid)
        {
            DbParameter[] prams =
			{
                DbHelper.MakeInParam("@subforumcount", (DbType)OleDbType.Integer, 4, subforumcount),
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=@subforumcount WHERE [fid]=@fid";
            DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
        }

        public void UpdateDisplayorderInForumByFid(int displayorder, int fid)
        {
            DbParameter[] prams =
			{
                DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=@displayorder WHERE [fid]=@fid";
            DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
        }

        public DataTable GetMainForum()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [layer]=0 Order By [displayorder] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void SetStatusInForum(int status, int fid)
        {
            DbParameter[] prams =
			{
                DbHelper.MakeInParam("@status", (DbType)OleDbType.Integer, 4, status),
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [status]=@status WHERE [fid]=@fid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public DataTable GetForumByParentid(int parentid)
        {
            DbParameter[] prams =
			{
                DbHelper.MakeInParam("@parentid", (DbType)OleDbType.Integer, 4, parentid)
			};
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=@parentid ORDER BY [DisplayOrder]";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void UpdateStatusByFidlist(string fidlist)
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [status]=0 WHERE [fid] IN(" + fidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void UpdateStatusByFidlistOther(string fidlist)
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [status]=1 WHERE [status]>1 AND [fid] IN(" + fidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public bool BatchSetForumInf(ForumInfo foruminfo, BatchSetParams bsp, string fidlist)
        {
            StringBuilder forums = new StringBuilder();
            StringBuilder forumfields = new StringBuilder();

            forums.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET ");
            if (bsp.SetSetting)
            {
                forums.Append("[Allowsmilies]='" + foruminfo.Allowsmilies + "' ,");
                forums.Append("[Allowrss]='" + foruminfo.Allowrss + "' ,");
                forums.Append("[Allowhtml]='" + foruminfo.Allowhtml + "' ,");
                forums.Append("[Allowbbcode]='" + foruminfo.Allowbbcode + "' ,");
                forums.Append("[Allowimgcode]='" + foruminfo.Allowimgcode + "' ,");
                forums.Append("[Allowblog]='" + foruminfo.Allowblog + "' ,");
                forums.Append("[istrade]='" + foruminfo.Istrade + "' ,");
                forums.Append("[allowpostspecial]='" + foruminfo.Allowpostspecial + "' ,");
                forums.Append("[allowspecialonly]='" + foruminfo.Allowspecialonly + "' ,");
                forums.Append("[Alloweditrules]='" + foruminfo.Alloweditrules + "' ,");
                forums.Append("[allowthumbnail]='" + foruminfo.Allowthumbnail + "' ,");
                forums.Append("[Recyclebin]='" + foruminfo.Recyclebin + "' ,");
                forums.Append("[Modnewposts]='" + foruminfo.Modnewposts + "' ,");
                forums.Append("[Jammer]='" + foruminfo.Jammer + "' ,");
                forums.Append("[Disablewatermark]='" + foruminfo.Disablewatermark + "' ,");
                forums.Append("[Inheritedmod]='" + foruminfo.Inheritedmod + "' ,");
            }
            if (forums.ToString().EndsWith(","))
            {
                forums.Remove(forums.Length - 1, 1);
            }
            forums.Append("WHERE [fid] IN(" + fidlist + ")");


            forumfields.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET ");

            if (bsp.SetPassWord)
            {
                forumfields.Append("[password]='" + foruminfo.Password + "' ,");
            }

            if (bsp.SetAttachExtensions)
            {
                forumfields.Append("[attachextensions]='" + foruminfo.Attachextensions + "' ,");
            }

            if (bsp.SetPostCredits)
            {
                forumfields.Append("[postcredits]='" + foruminfo.Postcredits + "' ,");
            }

            if (bsp.SetReplyCredits)
            {
                forumfields.Append("[replycredits]='" + foruminfo.Replycredits + "' ,");
            }


            if (bsp.SetViewperm)
            {
                forumfields.Append("[Viewperm]='" + foruminfo.Viewperm + "' ,");
            }

            if (bsp.SetPostperm)
            {
                forumfields.Append("[Postperm]='" + foruminfo.Postperm + "' ,");
            }

            if (bsp.SetReplyperm)
            {
                forumfields.Append("[Replyperm]='" + foruminfo.Replyperm + "' ,");
            }

            if (bsp.SetGetattachperm)
            {
                forumfields.Append("[Getattachperm]='" + foruminfo.Getattachperm + "' ,");
            }

            if (bsp.SetPostattachperm)
            {
                forumfields.Append("[Postattachperm]='" + foruminfo.Postattachperm + "' ,");
            }

            if (forumfields.ToString().EndsWith(","))
            {
                forumfields.Remove(forumfields.Length - 1, 1);
            }

            forumfields.Append("WHERE [fid] IN(" + fidlist + ")");


            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    if (forums.ToString().IndexOf("SET WHERE") < 0)
                    {
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, forums.ToString());
                    }

                    if (forumfields.ToString().IndexOf("SET WHERE") < 0)
                    {
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, forumfields.ToString());
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }

        public IDataReader GetTopForumFids(int lastfid, int statcount)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@lastfid", (DbType)OleDbType.Integer, 4, lastfid),
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] > @lastfid", prams);
        }

        public DataSet GetOnlineList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid],(SELECT TOP 1 [grouptitle]  FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "onlinelist].[groupid]) AS GroupName ,[displayorder],[title],[img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] ORDER BY [groupid] ASC");
        }

        public int UpdateOnlineList(int groupid, int displayorder, string img, string title)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@img", (DbType)OleDbType.VarChar, 50, img),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 50, title)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "onlinelist] SET [displayorder]=@displayorder,[title]=@title,[img]=@img  WHERE [groupid]=@groupid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetWords()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "words]";
        }

        public DataTable GetBadWords()
        { 
            string strsql = "SELECT [find],[replacement] FROM ["+ BaseConfigs.GetTablePrefix + "words] ORDER BY [find]";
            return DbHelper.ExecuteDataset(strsql).Tables[0];
        }

        public int DeleteWord(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id);

            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "words] WHERE [id]=@id";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }
        public int GetBadWordId(string find)
        {
            string sql = "select id from " + BaseConfigs.GetTablePrefix + "words where find='" + find + "'";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }
        public void UpdateBadWords(string find, string replacement)
        {
            string sql = "update " + BaseConfigs.GetTablePrefix + "words set replacement='" + replacement + "' where find ='" + find + "'";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }
        public void InsertBadWords(string username, string find,string replacement)
        {
            string sql = "insert " + BaseConfigs.GetTablePrefix + "words(admin,find,replacement) values('" + username + "','" + find + "','" + replacement + "')";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }

        public void DeleteBadWords()
        { 
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "words] ";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }

        public int UpdateWord(int id, string find, string replacement)
        {
            DbParameter[] prams = {
                    DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
					DbHelper.MakeInParam("@find", (DbType)OleDbType.VarChar, 255, find),
					DbHelper.MakeInParam("@replacement", (DbType)OleDbType.VarChar, 255, replacement)
				};

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "words] SET [find]=@find, [replacement]=@replacement WHERE [id]=@id";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public int DeleteWords(string idlist)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "words]  WHERE [ID] IN(" + idlist + ")");
        }

        public bool ExistWord(string find)
        {
            DbParameter parm = DbHelper.MakeInParam("@find", (DbType)OleDbType.VarWChar, 255, find);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1  * FROM [" + BaseConfigs.GetTablePrefix + "words] WHERE [find]=@find", parm).Tables[0].Rows.Count > 0;
        }

        public int AddWord(string username, string find, string replacement)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@username", (DbType)OleDbType.VarWChar, 20, username),
                                        DbHelper.MakeInParam("@find", (DbType)OleDbType.VarWChar, 255, find),
                                        DbHelper.MakeInParam("@replacement", (DbType)OleDbType.VarWChar, 255, replacement)
                                    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "words] ([admin], [find], [replacement]) VALUES (@username,@find,@replacement)";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public bool IsExistTopicType(string typename,int currenttypeid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@typename", (DbType)OleDbType.VarWChar, 30, typename),
                                        DbHelper.MakeInParam("@currenttypeid", (DbType)OleDbType.Integer, 4, currenttypeid)
                                    };
            string sql = "SELECT [typeid] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] WHERE [name]=@typename AND [typeid]<>@currenttypeid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
        }

        public bool IsExistTopicType(string typename)
        {
            DbParameter parms = DbHelper.MakeInParam("@typename", (DbType)OleDbType.VarWChar, 30, typename);
            string sql = "SELECT TOP 1  * FROM [" + BaseConfigs.GetTablePrefix + "topictypes] WHERE [name]=@typename";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
        }

        public string GetTopicTypes()
        {
            return "SELECT [typeid] AS id,[name],[displayorder],[description] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder] ASC";
        }

        public DataTable GetExistTopicTypeOfForum()
        {
            string sql = "SELECT [fid],[topictypes] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [topictypes] NOT LIKE ''";
            return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
        }

        public void UpdateTopicTypeForForum(string topictypes,int fid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@topictypes", (DbType)OleDbType.VarWChar, 0, topictypes),
                                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [topictypes]=@topictypes WHERE [fid]=@fid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateTopicTypes(string name, int displayorder,string description,int typeid)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar,100, name),
				DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer,4,displayorder),
				DbHelper.MakeInParam("@description", (DbType)OleDbType.VarChar,500,description),
				DbHelper.MakeInParam("@typeid", (DbType)OleDbType.Integer,4,typeid)
								   };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topictypes] SET [name]=@name ,[displayorder]=@displayorder, [description]=@description WHERE [typeid]=@typeid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void AddTopicTypes(string typename, int displayorder, string description)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@name",(DbType)OleDbType.VarWChar,100, typename),
				DbHelper.MakeInParam("@displayorder",(DbType)OleDbType.Integer,4,displayorder),
				DbHelper.MakeInParam("@description",(DbType)OleDbType.VarChar,500,description)
								  };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topictypes] ([name],[displayorder],[description]) VALUES(@name,@displayorder,@description)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public int GetMaxTopicTypesId()
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT MAX([typeid]) FROM [" + BaseConfigs.GetTablePrefix + "topictypes]").ToString());
        }

        public void DeleteTopicTypesByTypeidlist(string typeidlist)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topictypes]  WHERE [typeid] IN(" + typeidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }

        public DataTable GetForumNameIncludeTopicType()
        {
            string sql = "SELECT f1.[fid],[name],[topictypes] FROM [" + BaseConfigs.GetTablePrefix + "forums] AS f1 LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] AS f2 ON f1.fid=f2.fid";
            return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
        }

        public DataTable GetForumTopicType()
        {
            string sql = "SELECT [fid],[topictypes] FROM [" + BaseConfigs.GetTablePrefix + "forumfields]";
            return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
        }

        public void ClearTopicTopicType(int typeid)
        {
            DbParameter pram = DbHelper.MakeInParam("@typeid", (DbType)OleDbType.Integer, 4, typeid);
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [typeid]=0 Where [typeid]=@typeid";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql,pram);
        }

        public string GetTopicTypeInfo()
        {
            return "SELECT [typeid] AS id,[name],[description] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder] ASC";
        }

        public string GetTemplateName()
        {
            return "SELECT [templateid],[name] FROM [" + BaseConfigs.GetTablePrefix + "templates]";
        }

        public DataTable GetAttachType()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id],[extension]  FROM [" + BaseConfigs.GetTablePrefix + "attachtypes]  ORDER BY [id] ASC").Tables[0];
        }

        public void UpdatePermUserListByFid(string permuserlist, int fid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@permuserlist", (DbType)OleDbType.VarWChar,0,permuserlist),
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer,4, fid)
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [Permuserlist]=@permuserlist WHERE [fid]=@fid", prams);
        }

        public IDataReader GetTopicsIdentifyItem()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topicidentify]");
        }

        public string ResetTopTopicListSql(int layer, string fid, string parentidlist)
        {

            string filterexpress = "";

            switch (layer)
            {
                case 0:
                    filterexpress = string.Format("[fid]<>{0} AND (',' + TRIM([parentidlist]) + ',' LIKE '%,{1},%')", fid.ToString(), RegEsc(fid.ToString()));
                    break;
                case 1:
                    filterexpress = parentidlist.ToString().Trim();
                    if (filterexpress != string.Empty)
                    {
                        filterexpress =
                            string.Format(
                                "[fid]<>{0} AND ([fid]={1} OR (',' + TRIM([parentidlist]) + ',' LIKE '%,{2},%'))",
                                fid.ToString().Trim(), filterexpress, RegEsc(filterexpress));
                    }
                    else
                    {
                        filterexpress =
                            string.Format(
                                "[fid]<>{0} AND (',' + TRIM([parentidlist]) + ',' LIKE '%,{1},%')",
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
                                "[fid]<>{0} AND ([fid]={1} OR (',' + TRIM([parentidlist]) + ',' LIKE '%,{2},%'))",
                                fid.ToString().Trim(), filterexpress, RegEsc(filterexpress));
                    }
                    else
                    {
                        filterexpress =
                            string.Format(
                                "[fid]<>{0} AND (',' + TRIM([parentidlist]) + ',' LIKE '%,{1},%')",
                                fid.ToString().Trim(), RegEsc(filterexpress));
                    }
                    break;
            }

            return filterexpress;
        }

        public string showforumcondition(int sqlid, int cond)
        {

            string sql = null;
            switch (sqlid)
            {
                case 1:
                    sql = " AND [typeid]=";
                    break;
                case 2:
                    sql = " AND [postdatetime]>=#" + DateTime.Now.AddDays(-1 * cond).ToString("yyyy-MM-dd HH:mm:ss") + "#";
                    break;

                case 3:
                    sql = "tid";
                    break;

            }
            return sql;

        }

        public string DelVisitLogCondition(string deletemod, string visitid, string deletenum, string deletefrom)
        {
            string condition = null;
            switch (deletemod)
            {
                case "chkall":
                    if (visitid != "")
                        condition = " [visitid] IN(" + visitid + ")";
                    break;
                case "deleteNum":
                    if (deletenum != "" && Utils.IsNumeric(deletenum))
                        condition = " [visitid] NOT IN (SELECT TOP " + deletenum + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC)";
                    break;
                case "deleteFrom":
                    if (deletefrom != "")
                        condition = " [postdatetime]<#" + deletefrom + "#";
                    break;
            }
            return condition;
        }


        public string AttachDataBind(string condition, string postname)
        {


            return "SELECT [aid], [attachment], [filename], (SELECT TOP 1 [poster] FROM [" + postname + "] WHERE [" + postname + "].[pid]=[" + BaseConfigs.GetTablePrefix + "attachments].[pid]) AS [poster],(Select TOP 1 [title] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid]=[" + BaseConfigs.GetTablePrefix + "attachments].[tid]) AS [topictitle], [filesize],[downloads]  FROM [" + BaseConfigs.GetTablePrefix + "attachments] " + condition;
        }

        public DataTable GetAttachDataTable(string condition, string postname)
        {
            string sqlstring = "SELECT [aid], [attachment], [filename], (SELECT TOP 1 [poster] FROM [" + postname + "] WHERE [" + postname + "].[pid]=[" + BaseConfigs.GetTablePrefix + "attachments].[pid]) AS [poster],(Select TOP 1 [title] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid]=[" + BaseConfigs.GetTablePrefix + "attachments].[tid]) AS [topictitle], [filesize],[downloads]  FROM [" + BaseConfigs.GetTablePrefix + "attachments] " + condition;
            return DbHelper.ExecuteDataset(sqlstring).Tables[0];
        }


        public bool AuditTopicCount(string condition)
        {

            if (DbHelper.ExecuteDataset("SELECT COUNT(tid) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + condition).Tables[0].Rows[0][0].ToString() == "0")
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


            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + condition;
        }

        public DataTable AuditTopicBind(string condition)
        {
            //DbParameter param =
            //    DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, postdatetime);

            DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + condition).Tables[0];
            return dt;
        }

        public string AuditNewUserClear(string searchuser,string regbefore, string regip)
        {
            string sqlstring = "";
            sqlstring += " [groupid]=8";
            if (searchuser != "")
            {
                sqlstring += " AND [username] LIKE '%" + searchuser + "%'";
            }
            if (regbefore != "")
            {
                sqlstring += " AND [joindate]<=#" + DateTime.Now.AddDays(-Convert.ToDouble(regbefore)).ToString("yyyy-MM-dd HH:mm:ss") + "# ";
                //sqlstring += " AND [joindate]<=@joindate ";
            }

            if (regip != "")
            {
                sqlstring += " AND [regip] LIKE '" + RegEsc(regip) + "%'";
            }

            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + sqlstring;
        }

        public string DelMedalLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
        {
            string condition = "";
            switch (deletemode)
            {
                case "chkall":
                    if (id != "")
                        condition = " [id] IN(" + id + ")";
                    break;
                case "deleteNum":
                    if (deleteNum != "" && Utils.IsNumeric(deleteNum))
                        condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC)";
                    break;
                case "deleteFrom":
                    if (deleteFrom != "")
                        condition = " [postdatetime]<#" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "#";
                    break;
            }
            return condition;

        }

        public DataTable MedalsTable(string medalid)
        {

            DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [medalid]=" + medalid).Tables[0];
            return dt;
        }

        public string DelModeratorManageCondition(string deletemode, string id, string deleteNum, string deleteFrom)
        {
            string condition = "";
            switch (deletemode)
            {
                case "chkall":
                    if (id!= "")
                        condition = " [id] IN(" + id + ")";
                    break;
                case "deleteNum":
                    if (deleteNum != "" && Utils.IsNumeric(deleteNum))
                        condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] ORDER BY [id] DESC)";
                    break;
                case "deleteFrom":
                    if (deleteFrom != "")
                        condition = " [postdatetime]<#" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "#";
                    break;
            }
            return condition;
        }

        public DataTable GroupNameTable(string groupid)
        {
            DataTable dt = DbHelper.ExecuteDataset("SELECT TOP 1 [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid).Tables[0];
            return dt;
        }

        public string PaymentLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
        {
            string condition = "";
            switch (deletemode)
            {
                case "chkall":
                    if (id != "")
                        condition = " [id] IN(" + id + ")";
                    break;
                case "deleteNum":
                    if (deleteNum != "" && Utils.IsNumeric(deleteNum))
                        condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ORDER BY [id] DESC)";
                    break;
                case "deleteFrom":
                    if (deleteFrom != "")
                        condition = " [buydate]<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    break;
            }
            return condition;
        }

        public string PostGridBind(string posttablename, string condition)
        {


            return "SELECT * FROM [" + posttablename + "] WHERE " + condition.ToString();
        }

        public string DelRateScoreLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
        {
            string condition = "";
            switch (deletemode)
            {
                case "chkall":
                    if (id != "")
                        condition = " [id] IN(" + id + ")";
                    break;
                case "deleteNum":
                    if (deleteNum != "" && Utils.IsNumeric(deleteNum))
                        condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ORDER BY [id] DESC)";
                    break;
                case "deleteFrom":
                    if (deleteFrom != "")
                        condition = " [postdatetime]<#" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "#";
                    break;
            }
            return condition;
        }

        public void UpdatePostSP()
        {
            #region ���·ֱ�Ĵ洢����
            foreach (DataRow dr in DatabaseProvider.GetInstance().GetDatechTableIds())
            {
                CreateStoreProc(Convert.ToInt16(dr["id"].ToString()));
            }
            #endregion
        }

        public void CreateStoreProc(int tablelistmaxid)
        {
//            #region �����ֱ�洢����
//            StringBuilder sb = new StringBuilder(@"
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_createpost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_createpost]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_getfirstpostid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_getfirstpostid]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_getpostcount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_getpostcount]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_deletepostbypid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_deletepostbypid]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_getposttree]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_getposttree]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_getsinglepost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_getsinglepost]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_updatepost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_updatepost]
//
//                    ~
//
//                    if exists (select * from sysobjects where id = object_id(N'[dnt_getnewtopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
//                    drop procedure [dnt_getnewtopics]
//
//                    ~
//
//                    CREATE PROCEDURE dnt_createpost
//                    @fid int,
//                    @tid int,
//                    @parentid int,
//                    @layer int,
//                    @poster varchar(20),
//                    @posterid int,
//                    @title nvarchar(60),
//                    @topictitle nvarchar(60),
//                    @postdatetime char(20),
//                    @message ntext,
//                    @ip varchar(15),
//                    @lastedit varchar(50),
//                    @invisible int,
//                    @usesig int,
//                    @htmlon int,
//                    @smileyoff int,
//                    @bbcodeoff int,
//                    @parseurloff int,
//                    @attachment int,
//                    @rate int,
//                    @ratetimes int
//
//                    AS
//
//
//                    DEClARE @postid int
//
//                    DELETE FROM [dnt_postid] WHERE DATEDIFF(n, postdatetime, GETDATE()) >5
//
//                    INSERT INTO [dnt_postid] ([postdatetime]) VALUES(GETDATE())
//
//                    SELECT @postid=SCOPE_IDENTITY()
//
//                    INSERT INTO [dnt_posts1]([pid], [fid], [tid], [parentid], [layer], [poster], [posterid], [title], [postdatetime], [message], [ip], [lastedit], [invisible], [usesig], [htmlon], [smileyoff], [bbcodeoff], [parseurloff], [attachment], [rate], [ratetimes]) VALUES(@postid, @fid, @tid, @parentid, @layer, @poster, @posterid, @title, @postdatetime, @message, @ip, @lastedit, @invisible, @usesig, @htmlon, @smileyoff, @bbcodeoff, @parseurloff, @attachment, @rate, @ratetimes)
//
//                    IF @parentid=0
//                        BEGIN
//                    		
//                            UPDATE [dnt_posts1] SET [parentid]=@postid WHERE [pid]=@postid
//                        END
//
//                    IF @@ERROR=0
//                        BEGIN
//                            IF  @invisible = 0
//                            BEGIN
//                    		
//                                UPDATE [dnt_statistics] SET [totalpost]=[totalpost] + 1
//                    		
//                    		
//                    		
//                                DECLARE @fidlist AS VARCHAR(1000)
//                                DECLARE @strsql AS VARCHAR(4000)
//                    			
//                                SET @fidlist = '';
//                    			
//                                SELECT @fidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @fid
//                                IF RTRIM(@fidlist)<>''
//	                                BEGIN
//		                                SET @fidlist = RTRIM(@fidlist) + ',' + CAST(@fid AS VARCHAR(10))
//	                                END
//                                ELSE
//	                                BEGIN
//		                                SET @fidlist = CAST(@fid AS VARCHAR(10))
//	                                END
//                            
//                    			
//                                UPDATE [dnt_forums] SET 
//						                                [posts]=[posts] + 1, 
//						                                [todayposts]=CASE 
//										                                WHEN DATEDIFF(day, [lastpost], GETDATE())=0 THEN [todayposts]*1 + 1 
//									                                 ELSE 1 
//									                                 END,
//						                                [lasttid]=@tid,	
//                                                        [lasttitle]=@topictitle,
//						                                [lastpost]=@postdatetime,
//						                                [lastposter]=@poster,
//						                                [lastposterid]=@posterid 
//                    							
//				                                WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' + (SELECT @fidlist AS [fid]) + ',') > 0)
//                    			
//                    			
//                                UPDATE [dnt_users] SET
//	                                [lastpost] = @postdatetime,
//	                                [lastpostid] = @postid,
//	                                [lastposttitle] = @title,
//	                                [posts] = [posts] + 1,
//	                                [lastactivity] = GETDATE()
//                                WHERE [uid] = @posterid
//                            
//                            
//                                IF @layer<=0
//	                                BEGIN
//		                                UPDATE [dnt_topics] SET [replies]=0,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid
//	                                END
//                                ELSE
//	                                BEGIN
//		                                UPDATE [dnt_topics] SET [replies]=[replies] + 1,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid
//	                                END
//                            END
//
//                            UPDATE [dnt_topics] SET [lastpostid]=@postid WHERE [tid]=@tid
//                            
//                        IF @posterid <> -1
//                            BEGIN
//                                INSERT [dnt_myposts]([uid], [tid], [pid], [dateline]) VALUES(@posterid, @tid, @postid, @postdatetime)
//                            END
//
//                        END
//                    	
//                    SELECT @postid AS postid
//
//                    ~
//
//
//                    CREATE PROCEDURE dnt_getfirstpostid
//                    @tid int
//                    AS
//                    SELECT TOP 1 [pid] FROM [dnt_posts1] WHERE [tid]=@tid ORDER BY [pid]
//
//                    ~
//
//
//                    CREATE PROCEDURE dnt_getpostcount
//                    @tid int
//                    AS
//                    SELECT COUNT(pid) FROM [dnt_posts1] WHERE [tid]=@tid AND [invisible]=0 AND layer>0
//
//                    ~
//
//
//                    CREATE  PROCEDURE dnt_deletepostbypid
//                        @pid int,
//						@chanageposts AS BIT
//                    AS
//
//                        DECLARE @fid int
//                        DECLARE @tid int
//                        DECLARE @posterid int
//                        DECLARE @lastforumposterid int
//                        DECLARE @layer int
//                        DECLARE @postdatetime smalldatetime
//                        DECLARE @poster varchar(50)
//                        DECLARE @postcount int
//                        DECLARE @title nchar(60)
//                        DECLARE @lasttid int
//                        DECLARE @postid int
//                        DECLARE @todaycount int
//                    	
//                    	
//                        SELECT @fid = [fid],@tid = [tid],@posterid = [posterid],@layer = [layer], @postdatetime = [postdatetime] FROM [dnt_posts1] WHERE pid = @pid
//
//                        DECLARE @fidlist AS VARCHAR(1000)
//                    	
//                        SET @fidlist = '';
//                    	
//                        SELECT @fidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @fid
//                        IF RTRIM(@fidlist)<>''
//                            BEGIN
//                                SET @fidlist = RTRIM(@fidlist) + ',' + CAST(@fid AS VARCHAR(10))
//                            END
//                        ELSE
//                            BEGIN
//                                SET @fidlist = CAST(@fid AS VARCHAR(10))
//                            END
//
//
//                        IF @layer<>0
//
//                            BEGIN
//                    			
//								IF @chanageposts = 1
//									BEGIN
//										UPDATE [dnt_statistics] SET [totalpost]=[totalpost] - 1
//
//										UPDATE [dnt_forums] SET 
//											[posts]=[posts] - 1, 
//											[todayposts]=CASE 
//																WHEN DATEPART(yyyy, @postdatetime)=DATEPART(yyyy,GETDATE()) AND DATEPART(mm, @postdatetime)=DATEPART(mm,GETDATE()) AND DATEPART(dd, @postdatetime)=DATEPART(dd,GETDATE()) THEN [todayposts] - 1
//																ELSE [todayposts]
//														END						
//										WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +
//															(SELECT @fidlist AS [fid]) + ',') > 0)
//                    			
//										UPDATE [dnt_users] SET [posts] = [posts] - 1 WHERE [uid] = @posterid
//
//										UPDATE [dnt_topics] SET [replies]=[replies] - 1 WHERE [tid]=@tid
//									END
//                    			
//                                DELETE FROM [dnt_posts1] WHERE [pid]=@pid
//                    			
//                            END
//                        ELSE
//                            BEGIN
//                    		
//                                SELECT @postcount = COUNT([pid]) FROM [dnt_posts1] WHERE [tid] = @tid
//                                SELECT @todaycount = COUNT([pid]) FROM [dnt_posts1] WHERE [tid] = @tid AND DATEDIFF(d, [postdatetime], GETDATE()) = 0
//                    			
//								IF @chanageposts = 1
//									BEGIN
//										UPDATE [dnt_statistics] SET [totaltopic]=[totaltopic] - 1, [totalpost]=[totalpost] - @postcount
//		                    			
//										UPDATE [dnt_forums] SET [posts]=[posts] - @postcount, [topics]=[topics] - 1,[todayposts]=[todayposts] - @todaycount WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +(SELECT @fidlist AS [fid]) + ',') > 0)
//		                    			
//										UPDATE [dnt_users] SET [posts] = [posts] - @postcount WHERE [uid] = @posterid
//                    			
//									END
//
//                                DELETE FROM [dnt_posts1] WHERE [tid] = @tid
//                    			
//                                DELETE FROM [dnt_topics] WHERE [tid] = @tid
//                    			
//                            END	
//                    		
//
//                        IF @layer<>0
//                            BEGIN
//                                SELECT TOP 1 @pid = [pid], @posterid = [posterid], @postdatetime = [postdatetime], @title = [title], @poster = [poster] FROM [dnt_posts1] WHERE [tid]=@tid ORDER BY [pid] DESC
//                                UPDATE [dnt_topics] SET [lastposter]=@poster,[lastpost]=@postdatetime,[lastpostid]=@pid,[lastposterid]=@posterid WHERE [tid]=@tid
//                            END
//
//
//
//                        SELECT @lasttid = [lasttid] FROM [dnt_forums] WHERE [fid] = @fid
//
//                    	
//                        IF @lasttid = @tid
//                            BEGIN
//
//                    			
//                    			
//
//                                SELECT TOP 1 @pid = [pid], @tid = [tid],@lastforumposterid = [posterid], @title = [title], @postdatetime = [postdatetime], @poster = [poster] FROM [dnt_posts1] WHERE [fid] = @fid ORDER BY [pid] DESC
//                    			
//                            
//                            
//                                UPDATE [dnt_forums] SET 
//                    			
//	                                [lastpost]=@postdatetime,
//	                                [lastposter]=ISNULL(@poster,''),
//	                                [lastposterid]=ISNULL(@lastforumposterid,'0')
//
//                                WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +
//					                                (SELECT @fidlist AS [fid]) + ',') > 0)
//
//
//                    			
//                                SELECT TOP 1 @pid = [pid], @tid = [tid],@posterid = [posterid], @postdatetime = [postdatetime], @title = [title], @poster = [poster] FROM [dnt_posts1] WHERE [posterid]=@posterid ORDER BY [pid] DESC
//                    			
//                                UPDATE [dnt_users] SET
//                    			
//	                                [lastpost] = @postdatetime,
//	                                [lastpostid] = @pid,
//	                                [lastposttitle] = ISNULL(@title,'')
//                    				
//                                WHERE [uid] = @posterid
//                    			
//                            END
//
//
//                    ~
//
//
//                    CREATE PROCEDURE dnt_getposttree
//                    @tid int
//                    AS
//                    SELECT [pid], [layer], [title], [poster], [posterid],[postdatetime],[message] FROM [dnt_posts1] WHERE [tid]=@tid AND [invisible]=0 ORDER BY [parentid];
//
//
//                    ~
//
//                    CREATE PROCEDURE dnt_getsinglepost
//                    @tid int,
//                    @pid int
//                    AS
//                    SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads] FROM [dnt_attachments] WHERE [tid]=@tid
//
//                    SELECT TOP 1 
//	                                [dnt_posts1].[pid], 
//	                                [dnt_posts1].[fid], 
//	                                [dnt_posts1].[title], 
//	                                [dnt_posts1].[layer],
//	                                [dnt_posts1].[message], 
//	                                [dnt_posts1].[ip], 
//	                                [dnt_posts1].[lastedit], 
//	                                [dnt_posts1].[postdatetime], 
//	                                [dnt_posts1].[attachment], 
//	                                [dnt_posts1].[poster], 
//	                                [dnt_posts1].[invisible], 
//	                                [dnt_posts1].[usesig], 
//	                                [dnt_posts1].[htmlon], 
//	                                [dnt_posts1].[smileyoff], 
//	                                [dnt_posts1].[parseurloff], 
//	                                [dnt_posts1].[bbcodeoff], 
//	                                [dnt_posts1].[rate], 
//	                                [dnt_posts1].[ratetimes], 
//	                                [dnt_posts1].[posterid], 
//	                                [dnt_users].[nickname],  
//	                                [dnt_users].[username], 
//	                                [dnt_users].[groupid],
//                                    [dnt_users].[spaceid],
//                                    [dnt_users].[gender],
//									[dnt_users].[bday], 
//	                                [dnt_users].[email], 
//	                                [dnt_users].[showemail], 
//	                                [dnt_users].[digestposts], 
//	                                [dnt_users].[credits], 
//	                                [dnt_users].[extcredits1], 
//	                                [dnt_users].[extcredits2], 
//	                                [dnt_users].[extcredits3], 
//	                                [dnt_users].[extcredits4], 
//	                                [dnt_users].[extcredits5], 
//	                                [dnt_users].[extcredits6], 
//	                                [dnt_users].[extcredits7], 
//	                                [dnt_users].[extcredits8], 
//	                                [dnt_users].[posts], 
//	                                [dnt_users].[joindate], 
//	                                [dnt_users].[onlinestate], 
//	                                [dnt_users].[lastactivity], 
//	                                [dnt_users].[invisible], 
//	                                [dnt_userfields].[avatar], 
//	                                [dnt_userfields].[avatarwidth], 
//	                                [dnt_userfields].[avatarheight], 
//	                                [dnt_userfields].[medals], 
//	                                [dnt_userfields].[sightml] AS signature, 
//	                                [dnt_userfields].[location], 
//	                                [dnt_userfields].[customstatus], 
//	                                [dnt_userfields].[website], 
//	                                [dnt_userfields].[icq], 
//	                                [dnt_userfields].[qq], 
//	                                [dnt_userfields].[msn], 
//	                                [dnt_userfields].[yahoo], 
//	                                [dnt_userfields].[skype] 
//                    FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[pid]=@pid
//
//
//                    ~
//
//                    CREATE PROCEDURE dnt_updatepost
//                        @pid int,
//                        @title nvarchar(160),
//                        @message ntext,
//                        @lastedit nvarchar(50),
//                        @invisible int,
//                        @usesig int,
//                        @htmlon int,
//                        @smileyoff int,
//                        @bbcodeoff int,
//                        @parseurloff int
//                    AS
//                    UPDATE dnt_posts1 SET 
//                        [title]=@title,
//                        [message]=@message,
//                        [lastedit]=@lastedit,
//                        [invisible]=@invisible,
//                        [usesig]=@usesig,
//                        [htmlon]=@htmlon,
//                        [smileyoff]=@smileyoff,
//                        [bbcodeoff]=@bbcodeoff,
//                        [parseurloff]=@parseurloff WHERE [pid]=@pid
//
//
//                    ~
//
//                    CREATE PROCEDURE dnt_getnewtopics 
//                    @fidlist VARCHAR(500)
//                    AS
//                    IF @fidlist<>''
//                    BEGIN
//                    DECLARE @strSQL VARCHAR(5000)
//                    SET @strSQL = 'SELECT TOP 20   [dnt_posts1].[tid], [dnt_posts1].[title], [dnt_posts1].[poster], [dnt_posts1].[postdatetime], [dnt_posts1].[message],[dnt_forums].[name] FROM [dnt_posts1]  LEFT JOIN [dnt_forums] ON [dnt_posts1].[fid]=[dnt_forums].[fid] WHERE  [dnt_forums].[fid] NOT IN ('+@fidlist +')  AND [dnt_posts1].[layer]=0 ORDER BY [dnt_posts1].[pid] DESC' 
//                    END
//                    ELSE
//                    BEGIN
//                    SET @strSQL = 'SELECT TOP 20   [dnt_posts1].[tid], [dnt_posts1].[title], [dnt_posts1].[poster], [dnt_posts1].[postdatetime], [dnt_posts1].[message],[dnt_forums].[name] FROM [dnt_posts1]  LEFT JOIN [dnt_forums] ON [dnt_posts1].[fid]=[dnt_forums].[fid] WHERE [dnt_posts1].[layer]=0 ORDER BY [dnt_posts1].[pid] DESC'
//                    END
//                    EXEC(@strSQL)
//
//               ");

//            sb.Replace("\"", "'").Replace("dnt_posts1", BaseConfigs.GetTablePrefix + "posts" + tablelistmaxid);
//            sb.Replace("maxtablelistid", tablelistmaxid.ToString());
//            sb.Replace("dnt_createpost", BaseConfigs.GetTablePrefix + "createpost" + tablelistmaxid);
//            sb.Replace("dnt_getfirstpostid", BaseConfigs.GetTablePrefix + "getfirstpost" + tablelistmaxid + "id");
//            sb.Replace("dnt_getpostcount", BaseConfigs.GetTablePrefix + "getpost" + tablelistmaxid + "count");
//            sb.Replace("dnt_deletepostbypid", BaseConfigs.GetTablePrefix + "deletepost" + tablelistmaxid + "bypid");
//            sb.Replace("dnt_getposttree", BaseConfigs.GetTablePrefix + "getpost" + tablelistmaxid + "tree");
//            sb.Replace("dnt_getsinglepost", BaseConfigs.GetTablePrefix + "getsinglepost" + tablelistmaxid);
//            sb.Replace("dnt_updatepost", BaseConfigs.GetTablePrefix + "updatepost" + tablelistmaxid);
//            sb.Replace("dnt_getnewtopics", BaseConfigs.GetTablePrefix + "getnewtopics");
//            sb.Replace("dnt_", BaseConfigs.GetTablePrefix);
            
//            DatabaseProvider.GetInstance().CreatePostProcedure(sb.ToString());

//            #endregion
        }

        public void UpdateMyTopic()
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "mytopics]";//����ҵ������
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            //�ؽ��ҵ������
            sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "mytopics]([uid], [tid], [dateline]) SELECT [posterid],[tid],[postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid]<>-1";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void UpdateMyPost()
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "myposts]";//����ҵ����ӱ�
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            //�ؽ��ҵ����ӱ�
            StringBuilder sqlstring = new StringBuilder();
            sqlstring.Append("DECLARE @tableid int\r\n");
            sqlstring.Append("DECLARE tables_cursor CURSOR FOR SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "tablelist]\r\n");
            sqlstring.Append("OPEN tables_cursor\r\n");
            sqlstring.Append("FETCH NEXT FROM tables_cursor INTO @tableid\r\n");
            sqlstring.Append("WHILE @@FETCH_STATUS = 0\r\n");
            sqlstring.Append("BEGIN\r\n");
            sqlstring.Append("DECLARE @sql varchar(500)\r\n");
            sqlstring.Append("SET @sql ='INSERT INTO [" + BaseConfigs.GetTablePrefix + "myposts]([uid], [tid], [pid], [dateline])\r\n");
            sqlstring.Append("SELECT [posterid],[tid],[pid],[postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "posts' + LTRIM(STR(@tableid)) + '] WHERE [posterid]<>-1'\r\n");
            sqlstring.Append("EXEC(@sql)\r\n");
            sqlstring.Append("FETCH NEXT FROM tables_cursor INTO @tableid\r\n");
            sqlstring.Append("END\r\n");
            sqlstring.Append("CLOSE tables_cursor\r\n");
            sqlstring.Append("DEALLOCATE tables_cursor\r\n");


            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring.ToString());
        }

        public string GetAllIdentifySql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topicidentify]";
        }

        public DataTable GetAllIdentify()
        {
            string sql = GetAllIdentifySql();
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public bool UpdateIdentifyById(int id, string name)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@identifyid", (DbType)OleDbType.Integer,4,id),
				DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar,50, name)
			};
            string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "topicidentify] WHERE [name]=@name AND [identifyid]<>@identifyid";
            if (int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,prams).ToString()) != 0)  //����ͬ�����ƴ��ڣ�����ʧ��
            {
                return false;
            }
            sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topicidentify] SET [name]=@name WHERE [identifyid]=@identifyid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
            return true;
        }

        public bool AddIdentify(string name ,string filename)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar,50, name),
				DbHelper.MakeInParam("@filename", (DbType)OleDbType.VarChar,50,filename),
			};
            string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "topicidentify] WHERE [name]=@name";
            if (int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,prams).ToString()) != 0)  //����ͬ�����ƴ��ڣ�����ʧ��
            {
                return false;
            }
            sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topicidentify] ([name],[filename]) VALUES(@name,@filename)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
            return true;
        }

        public void DeleteIdentify(string idlist)
        {
            string sql = "DELETE [" + BaseConfigs.GetTablePrefix + "topicidentify] WHERE [identifyid] IN (" + idlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// ��ȡ��Ĭ��ģ����
        /// </summary>
        /// <returns></returns>
        public int GetSpecifyForumTemplateCount()
        {
            string sql = "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [templateid] <> 0 AND [templateid]<>" + GeneralConfigs.GetDefaultTemplateID();
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
        }

        public IDataReader GetAttachmentByUid(int uid, string extlist, int pageIndex, int pageSize)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4,uid),
                DbHelper.MakeInParam("@extlist ", (DbType)OleDbType.VarChar,100,extlist),
                DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer,4,pageIndex),
                DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pageSize)
			};

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmyattachmentsbytype", prams);
         
        }

        public int GetUserAttachmentCount(int uid)
        {
            DbParameter[] prams = 
			{
              DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4,uid)
			};
            string sql = string.Format("SELECT COUNT(1) FROM [{0}] WHERE [UID]=@uid", BaseConfigs.GetTablePrefix + "myattachments");
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString());
        }

        public int GetUserAttachmentCount(int uid, string extlist)
        {

            DbParameter[] prams = 
			{
              DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4,uid),
                //DbHelper.MakeInParam("@extlist", (DbType)OleDbType.VarChar,100,extlist)
			};
            //string sql = string.Format("select count(1) from {0} where exists (select * from {1}split(@extlist,',') where charindex(item, attachment)>0) and [UID]=@uid", BaseConfigs.GetTablePrefix + "myattachments",BaseConfigs.GetTablePrefix);
            string sql = string.Format("select count(1) from {0} where [extname] IN ("+extlist+") and [UID]=@uid", BaseConfigs.GetTablePrefix + "myattachments", BaseConfigs.GetTablePrefix);

            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, prams).ToString());


        }


        public IDataReader GetAttachmentByUid(int uid, int pageIndex, int pageSize) 
        
        {

            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4,uid),
                DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer,4,pageIndex),
                DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pageSize)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmyattachments", prams);
        }


        public void DelMyAttachmentByTid(string tidlist)
        {
           DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}myattachments] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
        
        }

        public void DelMyAttachmentByPid(string pidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}myattachments] WHERE [pid] IN ({1})", BaseConfigs.GetTablePrefix, pidlist));

        }

        public void DelMyAttachmentByAid(string aidlist)
        {

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}myattachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidlist));
        }

        public IDataReader GetHotTagsListForForum(int count)
        {
            string sql = string.Format("SELECT TOP {0} * FROM [{1}tags] WHERE [fcount] > 0 AND [orderid] > -1 ORDER BY [orderid], [fcount] DESC", count, BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }
        /// <summary>
        /// ������̳Tag�б�
        /// </summary>
        /// <param name="tagkey">��ѯ�ؼ���</param>
        /// <returns>����Sql���</returns>
        public string GetForumTagsSql(string tagname,int type)
        {
            //type ȫ��0 ����1 ����2

            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tags]";
            if (tagname != "")
            {
                sql += " WHERE [tagname] LIKE '%" + RegEsc(tagname) + "%'";
            }

            if (type == 1)
            {
                if (tagname!="")
                {
                    sql += " AND [orderid] < 0 ";
                }
                else
                {
                    sql += " WHERE [orderid] < 0 ";
                }
                
            }

            if (type == 2)
            {
                if (tagname != "")
                {
                    sql += " AND [orderid] >= 0";
                }
                else
                {
                    sql += " WHERE [orderid] >= 0 ";
                }
            }

            sql += " ORDER BY [fcount] DESC";

            return sql;
        }

        public string GetTopicNumber(string tagname,int from, int end, int type)
        { 
            //type ȫ��0 ����1 ����

            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tags]";

            if (tagname != "")
            {
                sql += " WHERE [tagname] LIKE '%" + RegEsc(tagname) + "%'";
            }


            if (type == 1)
            {
                if (tagname != "")
                {
                    sql += " AND [orderid] < 0 ";
                    sql += " AND [fcount] between " + from + " AND " + end;
                }
                else
                {
                    sql += " WHERE [orderid] < 0 ";
                    sql += " AND [fcount] between " + from + " AND " + end;
                }
            }

            if (type == 2)
            {
                if (tagname != "")
                {
                    sql += " AND [orderid] >= 0 ";
                    sql += " AND [fcount] between " + from + " AND " + end;
                }
                else
                {
                    sql += " WHERE [orderid] >= 0 ";
                    sql += " AND [fcount] between " + from + " AND " + end;
                }
            }

            
            sql += " ORDER BY [fcount] DESC";

            return sql;
        }


        public void UpdateForumTags(int tagid, int orderid, string color)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@orderid", (DbType)OleDbType.Integer,4, orderid),
                DbHelper.MakeInParam("@color", (DbType)OleDbType.Char,6, color),
				DbHelper.MakeInParam("@tagid", (DbType)OleDbType.Integer,4,tagid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "tags] SET [orderid]=@orderid,[color]=@color WHERE [tagid]=@tagid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql,prams);
        }
        /// <summary>
        /// �������п��Ű���б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetOpenForumList()
        {
            string sql = "SELECT [parentid],[fid],[name] FROM (SELECT f.*,ff.[permuserlist],ff.[viewperm] FROM [" + BaseConfigs.GetTablePrefix + "forums] f "
                + "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] ff ON f.[fid]=ff.[fid]) f "
                + "WHERE [status]=1 AND ([permuserlist] IS NULL OR [permuserlist] LIKE '') AND ([viewperm] IS NULL OR [viewperm] LIKE '' "
                + "OR CHARINDEX(',7,',','+CONVERT(VARCHAR(1000),[viewperm])+',')<>0) ORDER BY [displayorder] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// �������п��Ű���б�Sql
        /// </summary>
        /// <returns>���Ű���б�Sql</returns>
        public string GetOpenForumListSql()
        {
            string sql = "SELECT [fid],[name],[parentid] FROM (SELECT f.*,ff.[permuserlist],ff.[viewperm] FROM [" + BaseConfigs.GetTablePrefix + "forums] f "
                + "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] ff ON f.[fid]=ff.[fid]) f "
                + "WHERE [status]=1 AND ([permuserlist] IS NULL OR [permuserlist] LIKE '') AND ([viewperm] IS NULL OR [viewperm] LIKE '' "
                + "OR CHARINDEX(',7,',','+CONVERT(VARCHAR(1000),[viewperm])+',')<>0) ORDER BY [displayorder] ASC";
            return sql;
        }

        /// <summary>
        /// ��ȡ��ȡ��������
        /// </summary>
        /// <param name="inForumList">ָ������б�</param>
        /// <param name="itemCount">��ʾ��������</param>
        /// <param name="givenTids">ָ������</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="tags">�����ǩ</param>
        /// <param name="typesList">��������б�</param>
        /// <param name="digestList">�����б�</param>
        /// <param name="displayorderList">�ö��б�</param>
        /// <param name="orderBy">����ʽ</param>
        /// <returns></returns>
        public DataTable GetExtractTopic(string inForumList, int itemCount, string givenTids, string keyword,
            string tags, string typesList, string digestList, string displayorderList,string orderBy)
        {
            string sql = "SELECT TOP " + itemCount.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [closed]=0";
            if (inForumList != "")
            {
                sql += " AND [fid] IN (" + inForumList + ")";
            }
            if (givenTids != "")
            {
                sql += " AND [tid] IN (" + givenTids + ")";
            }
            if (keyword != "")
            {
                keyword = keyword.ToLower().Replace(" and ", " ");  //��and�滻Ϊ�ո�
                keyword = keyword.Replace(" or ", "|").Replace(" | ","|"); //��or�滻Ϊ|
                keyword = keyword.Replace("*", "");
                keyword = keyword.Replace(" ", "%' AND [title] LIKE '%");
                keyword = keyword.Replace("|", "%' OR [title] LIKE '%");
                keyword = " AND ([title] LIKE '%" + keyword + "%')";
                sql += keyword;
            }
            if (tags != "")
            {
                tags = "'" + tags.Replace(" ", "','") + "'";
                sql += " AND [tid] IN (SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topictags]";
                sql += " WHERE [tagid] IN (SELECT [tagid] FROM [" + BaseConfigs.GetTablePrefix + "tags] WHERE [tagname] IN (" + tags + ")))";
            }
            if (typesList != "")
            {
                sql += " AND [typeid] IN (" + typesList + ")";
            }
            if (digestList != "")
            {
                sql += " AND [digest] IN (" + digestList + ")";
            }
            if (displayorderList != "")
            {
                sql += " AND [displayorder] IN (" + displayorderList + ")";
            }
            sql += " ORDER BY [" + orderBy + "] DESC";
            sql = "SELECT f.[name] AS [forumname],tt.[name] AS [topictype],t.* FROM (" + sql + ") t LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid]=f.[fid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topictypes] tt ON t.[typeid]=tt.[typeid]";
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return dt;
        }

        public IDataReader GetHotTopics(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [views], [tid], [title] FROM [{1}topics] WHERE [displayorder]>=0 ORDER BY [views] DESC", count, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetHotReplyTopics(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [replies], [tid], [title] FROM [{1}topics] WHERE [displayorder]>=0 ORDER BY [replies] DESC", count, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetTopicBonusLogs(int tid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid);

            string sql = string.Format("SELECT [tid],[authorid],[answerid],[answername],[extid],SUM(bonus) AS [bonus] FROM [{0}bonuslog] WHERE [tid]=@tid GROUP BY [answerid],[authorid],[tid],[answername],[extid]", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public IDataReader GetTopicBonusLogsByPost(int tid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid);
            string sql = string.Format("SELECT [pid],[isbest],[bonus],[extid] FROM [{0}bonuslog] WHERE [tid]=@tid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public DataTable GetAllOpenForum()
        {
            string sql = string.Format("SELECT * FROM [{0}forums] f LEFT JOIN [{0}forumfields] ff ON f.[fid] = ff.[fid] WHERE f.[autoclose]=0 AND ff.[password]='' AND ff.[redirect]=''", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void UpdateLastPost(ForumInfo foruminfo, PostInfo postinfo)
        {
            DbParameter[] parms ={
                                     DbHelper.MakeInParam("@lasttid", (DbType)OleDbType.Integer, 4, postinfo.Tid),
                                     DbHelper.MakeInParam("@lasttitle", (DbType)OleDbType.VarWChar, 60, postinfo.Topictitle),
                                     DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.DBTimeStamp, 8, postinfo.Postdatetime),
                                     DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, postinfo.Posterid),
                                     DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 20, postinfo.Poster),
                                     DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, foruminfo.Fid)
                                 };
            string sql = string.Format("UPDATE [{0}forums] SET [lasttid] = @lasttid, [lasttitle] = @lasttitle, [lastpost] = @lastpost, [lastposterid] = @lastposterid, [lastposter] = @lastposter WHERE [fid] = @fid OR [fid] IN ({1})", BaseConfigs.GetTablePrefix, foruminfo.Parentidlist);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        #endregion

		#region GlobalManage
         private string GetSqlstringByPostDatetime(string sqlstring, DateTime postdatetimeStart, DateTime postdatetimeEnd)
        {
            //������Ҫ�ĳɲ������Ժ���Ҫ�ع�����Ҫ���޸ĺ�̨���ݲ�����ʽ
            if (postdatetimeStart.ToString() != "")
            {
                sqlstring += " AND [postdatetime]>=#" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "#";
            }

            if (postdatetimeEnd.ToString() != "")
            {
                sqlstring += " AND [postdatetime]<=#" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "#";
            }
            return sqlstring;
        }


        public DataTable GetAdsTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [advid], [type], [displayorder], [targets], [parameters], [code] FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [available]=1 AND [starttime] <=#" + DateTime.Now.ToShortDateString() + "# AND [endtime] >=#" + DateTime.Now.ToShortDateString() + "# ORDER BY [displayorder] DESC, [advid] DESC").Tables[0];
        }

        /// <summary>
        /// ���ȫ��ָ��ʱ����ڵĹ����б�
        /// </summary>
        /// <param name="starttime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <returns>�����б�</returns>
        public DataTable GetAnnouncementList(string starttime, string endtime)
        {
            DbParameter[] prams = {
										   DbHelper.MakeInParam("@starttime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime)),
										   DbHelper.MakeInParam("@endtime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(endtime))
									   };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [starttime] <=@starttime AND [endtime] >=@starttime ORDER BY [displayorder], [id] DESC", prams).Tables[0];
        }

        /// <summary>
        /// ���ȫ��ָ��ʱ����ڵ�ǰn�������б�
        /// </summary>
        /// <param name="starttime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <param name="maxcount">����¼��,С��0����ȫ��</param>
        /// <returns>�����б�</returns>
        public DataTable GetSimplifiedAnnouncementList(string starttime, string endtime, int maxcount)
        {
            DbParameter[] prams = {
										   DbHelper.MakeInParam("@starttime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime)),
										   DbHelper.MakeInParam("@endtime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(endtime))
									   };
            string topstr = " TOP " + maxcount;
            if (maxcount < 0)
                topstr = "";
            string sqlstr = "SELECT" + topstr + " [id], [title], [poster], [posterid],[starttime] FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [starttime] <=@starttime AND [endtime] >=@starttime ORDER BY [displayorder], [id] DESC";

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, prams).Tables[0];
        }


        public int AddAnnouncement(string poster, int posterid, string title, int displayorder, string starttime, string endtime, string message)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarWChar, 20, poster),
                                        DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer, 4, posterid),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 250, title),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@starttime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(endtime)),
                                        DbHelper.MakeInParam("@message", (DbType)OleDbType.VarWChar, 0, message)
                                    };
            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "announcements] ([poster],[posterid],[title],[displayorder],[starttime],[endtime],[message]) VALUES(@poster, @posterid, @title, @displayorder, @starttime, @endtime, @message)";
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
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] ORDER BY [id] ASC";
        }

        public void DeleteAnnouncements(string idlist)
        {
            string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id] IN(" + idlist + ")";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
        }

        public DataTable GetAnnouncement(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id]=@id", parm).Tables[0];
        }

        public void UpdateAnnouncement(int id, string poster, string title, int displayorder, string starttime, string endtime, string message)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarWChar, 20, poster),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 250, title),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@starttime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(endtime)),
                                        DbHelper.MakeInParam("@message", (DbType)OleDbType.VarWChar, 0, message)
                                    };
            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "announcements] SET [displayorder]=@displayorder,[title]=@title, [poster]=@poster,[starttime]=@starttime,[endtime]=@endtime,[message]=@message WHERE [id]=@id";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public void DeleteAnnouncement(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id);

            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id]=@id", parm);
        }


        /// <summary>
        /// ��ù����ɼ�����б�
        /// </summary>
        /// <returns></returns>
        public IDataReader GetVisibleForumList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [name], [fid], [layer] FROM [{0}forums] WHERE [parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetOnlineGroupIconList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [title], [img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [img]<> '' ORDER BY [displayorder]");
        }

        /// <summary>
        /// ������������б�
        /// </summary>
        /// <returns>���������б�</returns>
        public DataTable GetForumLinkList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, @"SELECT [name],[url],[note],[displayorder]+10000 AS [displayorder],[logo] FROM [" + BaseConfigs.GetTablePrefix + @"forumlinks] WHERE [displayorder] > 0 AND [logo] = '' 
																	UNION SELECT [name],[url],[note],[displayorder],[logo] FROM [" + BaseConfigs.GetTablePrefix + @"forumlinks] WHERE [displayorder] > 0 AND [logo] <> '' ORDER BY [displayorder]").Tables[0];
        }

        /// <summary>
        /// ������ֹ����б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetBanWordList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [find], [replacement] FROM [" + BaseConfigs.GetTablePrefix + "words]").Tables[0];
        }

        /// <summary>
        /// ���ѫ���б�
        /// </summary>
        /// <returns>���ѫ���б�</returns>
        public DataTable GetMedalsList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [medalid], [name], [image],[available]  FROM [" + BaseConfigs.GetTablePrefix + "medals] ORDER BY [medalid]").Tables[0];
        }

        /// <summary>
        /// ���ħ�������б�
        /// </summary>
        /// <returns>ħ�������б�</returns>
        public DataTable GetMagicList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "magic] ORDER BY [id]").Tables[0];
        }

        /// <summary>
        /// ������������б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetTopicTypeList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [typeid],[name] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder]").Tables[0];
        }

        /// <summary>
        /// ��ӻ���ת�ʶһ���¼
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="fromto">����/��</param>
        /// <param name="sendcredits">������������</param>
        /// <param name="receivecredits">�õ���������</param>
        /// <param name="send">������������</param>
        /// <param name="receive">�õ���������</param>
        /// <param name="paydate">ʱ��</param>
        /// <param name="operation">���ֲ���(1=�һ�, 2=ת��)</param>
        /// <returns>ִ��Ӱ�����</returns>
        public int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@fromto",(DbType)OleDbType.Integer,4,fromto),
									   DbHelper.MakeInParam("@sendcredits",(DbType)OleDbType.Integer,4,sendcredits),
									   DbHelper.MakeInParam("@receivecredits",(DbType)OleDbType.Integer,4,receivecredits),
									   DbHelper.MakeInParam("@send",(DbType)OleDbType.Numeric,4,send),
									   DbHelper.MakeInParam("@receive",(DbType)OleDbType.Numeric,4,receive),
									   DbHelper.MakeInParam("@paydate",(DbType)OleDbType.VarChar,0,paydate),
									   DbHelper.MakeInParam("@operation",(DbType)OleDbType.Integer,4,operation)
								   };

            return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "creditslog] ([uid],[fromto],[sendcredits],[receivecredits],[send],[receive],[paydate],[operation]) VALUES(@uid,@fromto,@sendcredits,@receivecredits,@send,@receive,@paydate,@operation)", prams);

        }

        /// <summary>
        /// ����ָ����Χ�Ļ�����־
        /// </summary>
        /// <param name="pagesize">ҳ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="uid">�û�id</param>
        /// <returns>������־</returns>
        public DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                //select c.*,ufrom.username as fromuser ,uto.username as touser from dnt_creditslog c,dnt_users ufrom, dnt_users uto where c.uid=ufrom.uid AND c.fromto=uto.uid
// AND (c.uid=1 or c.fromto =1)
                sqlstring = string.Format("SELECT TOP {0} [c].*, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [c], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [c].[uid]=[ufrom].[uid] AND [c].[fromto]=[uto].[uid] AND ([c].[uid]={2} OR [c].[fromto] = {2})  ORDER BY [id] DESC", pagesize, BaseConfigs.GetTablePrefix, uid);
            }
            else
            {
                sqlstring = string.Format("SELECT TOP {0} [c].*, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [c], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {2} [id] FROM [{1}creditslog] WHERE [{1}creditslog].[uid]={3}  OR [{1}creditslog].[fromto]={3} ORDER BY [id] DESC) AS tblTmp ) AND [c].[uid]=[ufrom].[uid] AND [c].[fromto]=[uto].[uid] AND ([c].[uid]={3} OR [c].[fromto] = {3}) ORDER BY [c].[id] DESC", pagesize, BaseConfigs.GetTablePrefix, pagetop, uid);
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// ���ָ���û��Ļ��ֽ�����ʷ��¼������
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>��ʷ��¼������</returns>
        public int GetCreditsLogRecordCount(int uid)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}creditslog] WHERE [uid]={1} OR [fromto]={1}", BaseConfigs.GetTablePrefix, uid)), 0);
        }


        public string GetTableStruct()
        {
            #region ���ݱ��ѯ���

            string SqlString = null;
            SqlString = "SELECT ����=case when a.colorder=1 then d.name else '' end,";
            SqlString += "��˵��=case when a.colorder=1 then isnull(f.value,'') else '' end,";
            SqlString += " �ֶ����=a.colorder,";
            SqlString += " �ֶ���=a.name,";
            SqlString += " ��ʶ=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '��'else '' end,";
            SqlString += " ����=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and name in (";
            SqlString += " SELECT name FROM sysindexes WHERE indid in(";
            SqlString += "   SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid";
            SqlString += "  ))) then '��' else '' end,";
            SqlString += " ����=b.name,";
            SqlString += " ռ���ֽ���=a.length,";
            SqlString += " ����=COLUMNPROPERTY(a.id,a.name,'PRECISION'),";
            SqlString += " С��λ��=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),";
            SqlString += " �����=case when a.isnullable=1 then '��'else '' end,";
            SqlString += " Ĭ��ֵ=isnull(e.text,''),";
            SqlString += " �ֶ�˵��=isnull(g.[value],'')";
            SqlString += "FROM syscolumns a";
            SqlString += " left join systypes b on a.xtype=b.xusertype";
            SqlString += " inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'";
            SqlString += " left join syscomments e on a.cdefault=e.id";
            SqlString += " left join sysproperties g on a.id=g.id and a.colid=g.smallid  ";
            SqlString += " left join sysproperties f on d.id=f.id and f.smallid=0";
            //SqlString+="--where d.name='Ҫ��ѯ�ı�'    --���ֻ��ѯָ����,���ϴ�����";
            SqlString += " order by a.id,a.colorder";
            return SqlString;

            #endregion
        }

        public void ShrinkDataBase(string shrinksize, string dbname)
        {
            string SqlString = null;

            SqlString += "SET NOCOUNT ON ";

            SqlString += "DECLARE @LogicalFileName sysname, @MaxMinutes INT, @NewSize INT ";
            SqlString += "USE [" + dbname + "] -- Ҫ���������ݿ��� ";
            SqlString += "SELECT @LogicalFileName = '" + dbname + "_log', -- ��־�ļ��� ";
            SqlString += "@MaxMinutes = 10, -- Limit on time allowed to wrap log. ";
            SqlString += "@NewSize = 1 -- �����趨����־�ļ��Ĵ�С(M) ";
            SqlString += "-- Setup / initialize ";
            SqlString += "DECLARE @OriginalSize int ";
            SqlString += "SELECT @OriginalSize = " + shrinksize;
            SqlString += "FROM sysfiles ";
            SqlString += "WHERE name = @LogicalFileName ";
            SqlString += "SELECT 'Original Size of ' + db_name() + ' LOG is ' + ";
            SqlString += "CONVERT(VARCHAR(30),@OriginalSize) + ' 8K pages or ' + ";
            SqlString += "CONVERT(VARCHAR(30),(@OriginalSize*8/1024)) + 'MB' ";
            SqlString += "FROM sysfiles ";
            SqlString += "WHERE name = @LogicalFileName ";
            SqlString += "CREATE TABLE DummyTrans ";
            SqlString += "(DummyColumn char (8000) not null) ";
            SqlString += "DECLARE @Counter INT, ";
            SqlString += "@StartTime DATETIME, ";
            SqlString += "@TruncLog VARCHAR(255) ";
            SqlString += "SELECT @StartTime = GETDATE(), ";
            SqlString += "@TruncLog = 'BACKUP LOG ' + db_name() + ' WITH TRUNCATE_ONLY' ";
            SqlString += "DBCC SHRINKFILE (@LogicalFileName, @NewSize) ";
            SqlString += "EXEC (@TruncLog) ";
            SqlString += "-- Wrap the log if necessary. ";
            SqlString += "WHILE @MaxMinutes > DATEDIFF (mi, @StartTime, GETDATE()) -- time has not expired ";
            SqlString += "AND @OriginalSize = (SELECT size FROM sysfiles WHERE name = @LogicalFileName) ";
            SqlString += "AND (@OriginalSize * 8 /1024) > @NewSize ";
            SqlString += "BEGIN -- Outer loop. ";
            SqlString += "SELECT @Counter = 0 ";
            SqlString += "WHILE ((@Counter < @OriginalSize / 16) AND (@Counter < 50000)) ";
            SqlString += "BEGIN -- update ";
            SqlString += "INSERT DummyTrans VALUES ('Fill Log') ";
            SqlString += "DELETE DummyTrans ";
            SqlString += "SELECT @Counter = @Counter + 1 ";
            SqlString += "END ";
            SqlString += "EXEC (@TruncLog) ";
            SqlString += "END ";
            SqlString += "SELECT 'Final Size of ' + db_name() + ' LOG is ' + ";
            SqlString += "CONVERT(VARCHAR(30),size) + ' 8K pages or ' + ";
            SqlString += "CONVERT(VARCHAR(30),(size*8/1024)) + 'MB' ";
            SqlString += "FROM sysfiles ";
            SqlString += "WHERE name = @LogicalFileName ";
            SqlString += "DROP TABLE DummyTrans ";
            SqlString += "SET NOCOUNT OFF ";

            DbHelper.ExecuteDataset(CommandType.Text, SqlString);
        }

        public void ClearDBLog(string dbname)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@DBName", (DbType)OleDbType.VarChar, 50, dbname),
			};
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "" + BaseConfigs.GetTablePrefix + "shrinklog", prams);
        }

        public string RunSql(string sql)
        {
            string errorInfo = string.Empty;
            if (sql != "")
            {
                SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
                conn.Open();
                foreach (string sqlStr in Utils.SplitString(sql, "--/* Discuz!NT SQL Separator */--"))
                {
                    using (SqlTransaction trans = conn.BeginTransaction())
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
                            errorInfo += message + "<br />";
                        }
                    }
                }
              conn.Close();
            }
            return errorInfo;
        }

        //�õ����ݿ������
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
        /// ���������ָ�����ӷֱ�idȫ������
        /// </summary>
        /// <param name="DbName">���ݿ�����</param>
        /// <param name="postsid">��ǰ���ӱ��id</param>
        /// <returns></returns>
        public bool CreateORFillIndex(string DbName, string postsid)
        {
            StringBuilder sb = new StringBuilder();

            string currenttablename = BaseConfigs.GetTablePrefix + "posts" + postsid;

            try
            {
                //�����ȫ��������������,���û�о��׳��쳣
                sb.Remove(0, sb.Length);
                DbHelper.ExecuteNonQuery("SELECT TOP 1 [pid] FROM [" + currenttablename + "] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC");

                sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full'; \r\n");
                sb.Append("WHILE fulltextcatalogproperty('pk_" + currenttablename + "_msg','populateStatus')<>0 \r\n");
                sb.Append("BEGIN \r\n");
                sb.Append("WAITFOR DELAY '0:5:30' \r\n");
                sb.Append("END \r\n");
                DbHelper.ExecuteNonQuery(sb.ToString());

                return true;
            }
            catch
            {
                try
                {
                    #region ����ȫ������

                    sb.Remove(0, sb.Length);
                    sb.Append("IF(SELECT DATABASEPROPERTY('[" + DbName + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");

                    try
                    { //�˴�ɾ����ȷ��ȫ������Ŀ¼��ϵͳ���е�����ͬ��
                        sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]', 'drop' ;");
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','drop';");
                        DbHelper.ExecuteNonQuery(sb.ToString());
                    }
                    catch
                    {
                        ;
                    }
                    finally
                    {
                        //ִ��ȫ��������
                        sb.Remove(0, sb.Length);
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','create';");
                        sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]','create','pk_" + currenttablename + "_msg','pk_" + currenttablename + "';");
                        sb.Append("EXECUTE sp_fulltext_column '[" + currenttablename + "]','message','add';");
                        sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]','activate';");
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full';");
                        DbHelper.ExecuteNonQuery(sb.ToString());
                    }
                    return true;

                    #endregion
                }
                catch (SqlException ex)
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
        /// �õ�ָ�����ӷֱ��ȫ����������(���)���
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetSpecialTableFullIndexSQL(string tablename)
        {
            #region ����

            //StringBuilder sb = new StringBuilder();
            //sb.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[" + tablename + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [" + tablename + "];");
            //sb.Append("CREATE TABLE [" + tablename + "] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ," +
            //    "[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ," +
            //    "[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ," +
            //    "[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ," +
            //    "[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ," +
            //    "[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ," +
            //    "[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            //sb.Append(";");
            //sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY]");
            //sb.Append(";");

            //sb.Append("ALTER TABLE [" + tablename + "] ADD ");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR [pid],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR [layer],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR [posterid],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR [message],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR [lastedit],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR [usesig],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR [smileyoff],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR [rate],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR [ratetimes]");

            //sb.Append(";");
            //sb.Append("CREATE  INDEX [parentid] ON [" + tablename + "]([parentid]) ON [PRIMARY]");
            //sb.Append(";");

            //sb.Append("CREATE  UNIQUE  INDEX [showtopic] ON [" + tablename + "]([tid], [invisible], [pid]) ON [PRIMARY]");
            //sb.Append(";");


            //sb.Append("CREATE  INDEX [treelist] ON [" + tablename + "]([tid], [invisible], [parentid]) ON [PRIMARY]");
            //sb.Append(";");

            #endregion

            #region ��ȫ������

            //sb.Append("USE " + GetDbName() + " \r\n");
            //sb.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
            //sb.Append("IF(SELECT DATABASEPROPERTY('[" + GetDbName() + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
            //sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
            //sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_table '[" + tablename + "]', 'drop' ;");
            //sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
            //sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
            //sb.Append("EXECUTE sp_fulltext_column '[" + tablename + "]','message','add';");
            //sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','activate';");
            //sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

            #endregion

            //return sb.ToString();

            return "";
        }


        /// <summary>
        /// ��DataReader�����Զ���༭����ť�б�
        /// </summary>
        /// <returns></returns>
        public IDataReader GetCustomEditButtonList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [available] = 1");
        }

        /// <summary>
        /// ��DataTable�����Զ��尴ť�б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomEditButtonListWithTable()
        {
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [available] = 1");
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
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows;
        }



        public void UpdateAnnouncementPoster(int posterid, string poster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer, 4, posterid),
                                        DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarChar, 20, poster)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "announcements] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
        }

        public bool HasStatisticsByLastUserId(int lastuserid)
        {
            DbParameter parm = DbHelper.MakeInParam("@lastuserid", (DbType)OleDbType.Integer, 4, lastuserid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [lastuserid] FROM  [" + BaseConfigs.GetTablePrefix + "statistics]  WHERE [lastuserid]=@lastuserid",parm).Tables[0].Rows.Count > 0;
        }

        public void UpdateStatisticsLastUserName(int lastuserid, string lastusername)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@lastuserid", (DbType)OleDbType.Integer, 4, lastuserid),
                                        DbHelper.MakeInParam("@lastusername", (DbType)OleDbType.VarChar, 20, lastusername)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [lastusername]=@lastusername WHERE [lastuserid]=@lastuserid", parms);
        }

        public void AddVisitLog(int uid, string username, int groupid, string grouptitle, string ip, string actions, string others)
        {
            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ([uid],[username],[groupid],[grouptitle],[ip],[actions],[others]) VALUES (@uid,@username,@groupid,@grouptitle,@ip,@actions,@others)";

            DbParameter[] prams = {
					DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
					DbHelper.MakeInParam("@username", (DbType)OleDbType.VarChar, 50, username),
					DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid),
					DbHelper.MakeInParam("@grouptitle", (DbType)OleDbType.VarChar, 50, grouptitle),
					DbHelper.MakeInParam("@ip", (DbType)OleDbType.VarChar, 15, ip),
					DbHelper.MakeInParam("@actions", (DbType)OleDbType.VarChar, 100, actions),
					DbHelper.MakeInParam("@others", (DbType)OleDbType.VarChar, 200, others)
				};

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
        }

        public void DeleteVisitLogs()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ");
        }

        public void DeleteVisitLogs(string condition)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition);
        }

        /// <summary>
        /// �õ���ǰָ��ҳ���ĺ�̨������־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <returns></returns>
        public DataTable GetVisitLogList(int pagesize, int currentpage)
        {
            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid]) FROM (SELECT TOP " + pagetop + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC) AS tblTmp )  ORDER BY [visitid] DESC";
                return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            }
        }

        /// <summary>
        /// �õ���ǰָ��������ҳ���ĺ�̨������־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetVisitLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition + " ORDER BY [visitid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid])  FROM (SELECT TOP " + pagetop + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition + " ORDER BY [visitid] DESC) AS tblTmp ) AND " + condition + " ORDER BY [visitid] DESC";
                return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            }
        }

        public int GetVisitLogCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(visitid) FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]").Tables[0].Rows[0][0].ToString());
        }

        public int GetVisitLogCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(visitid) FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        public void UpdateForumAndUserTemplateId(string templateidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [templateid]=0 WHERE [templateid] IN(" + templateidlist + ")");
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [templateid]=0 WHERE [templateid] IN(" + templateidlist + ")");
        }

        public void AddTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar, 50, name),
                                        DbHelper.MakeInParam("@directory", (DbType)OleDbType.VarWChar, 100, directory),
                                        DbHelper.MakeInParam("@copyright", (DbType)OleDbType.VarWChar, 100, copyright),
                                        DbHelper.MakeInParam("@author", (DbType)OleDbType.VarWChar, 100, author),
                                        DbHelper.MakeInParam("@createdate", (DbType)OleDbType.VarWChar, 50, createdate),
                                        DbHelper.MakeInParam("@ver", (DbType)OleDbType.VarWChar, 100, ver),
                                        DbHelper.MakeInParam("@fordntver", (DbType)OleDbType.VarWChar, 100, fordntver)
                                    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "templates] ([name],[directory],[copyright],[author],[createdate],[ver],[fordntver]) VALUES(@name,@directory,@copyright,@author,@createdate,@ver,@fordntver)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// ����µ�ģ����
        /// </summary>
        /// <param name="templateName">ģ������</param>
        /// <param name="directory">ģ���ļ�����Ŀ¼</param>
        /// <param name="copyright">ģ���Ȩ����</param>
        /// <returns>ģ��id</returns>
        public int AddTemplate(string templateName, string directory, string copyright)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@templatename", (DbType)OleDbType.VarChar, 0, templateName),
				DbHelper.MakeInParam("@directory", (DbType)OleDbType.VarChar, 0, directory),
				DbHelper.MakeInParam("@copyright", (DbType)OleDbType.VarChar, 0, copyright),

			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "templates]([templatename],[directory],[copyright]) VALUES(@templatename, @directory, @copyright);SELECT SCOPE_IDENTITY()", prams), -1);
        }

        /// <summary>
        /// ɾ��ָ����ģ����
        /// </summary>
        /// <param name="templateid">ģ��id</param>
        public void DeleteTemplateItem(int templateid)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@templateid", (DbType)OleDbType.Integer, 4, templateid)
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "templates] WHERE [templateid]=@templateid");
        }

        /// <summary>
        /// ɾ��ָ����ģ�����б�,
        /// </summary>
        /// <param name="templateidlist">��ʽΪ�� 1,2,3</param>
        public void DeleteTemplateItem(string templateidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "templates] WHERE [templateid] IN (" + templateidlist + ")");
        }

        /// <summary>
        /// ���������ģ��Ŀ¼�µ�ģ���б�(��:��Ŀ¼����)
        /// </summary>
        /// <param name="templatePath">ģ������·��</param>
        /// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
        /// <returns>ģ���б�</returns>
        public DataTable GetAllTemplateList(string templatePath)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
        }


        public int GetMaxTemplateId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(templateid), 0) FROM " + BaseConfigs.GetTablePrefix + "templates"), 0);
        }

        /// <summary>
        /// �������������־��¼
        /// </summary>
        /// <param name="moderatorname">������</param>
        /// <param name="grouptitle">�������ID</param>
        /// <param name="ip">�ͻ��˵�IP</param>
        /// <param name="fname">��������</param>
        /// <param name="title">���������</param>
        /// <param name="actions">����</param>
        /// <param name="reason">ԭ��</param>
        /// <returns></returns>
        public bool InsertModeratorLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
        {
            try
            {
                DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@moderatoruid", (DbType)OleDbType.Integer, 4, moderatoruid),
                                        DbHelper.MakeInParam("@moderatorname", (DbType)OleDbType.VarWChar, 50, moderatorname),
                                        DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid),
                                        DbHelper.MakeInParam("@grouptitle", (DbType)OleDbType.VarWChar, 50, grouptitle),
                                        DbHelper.MakeInParam("@ip", (DbType)OleDbType.VarChar, 15, ip),
                                        DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(postdatetime)),
                                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid),
                                        DbHelper.MakeInParam("@fname", (DbType)OleDbType.VarWChar, 100, fname),
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 8, tid),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarChar, 200, title),
                                        DbHelper.MakeInParam("@actions", (DbType)OleDbType.VarChar, 50, actions),
                                        DbHelper.MakeInParam("@reason", (DbType)OleDbType.VarWChar, 200, reason)
                                    };

                string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix +
                                   "moderatormanagelog] ([moderatoruid],[moderatorname],[groupid],[grouptitle],[ip],[postdatetime],[fid],[fname],[tid],[title],[actions],[reason]) VALUES (@moderatoruid, @moderatorname, @groupid, @grouptitle,@ip,@postdatetime,@fid,@fname,@tid,@title,@actions,@reason)";
                DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��ָ������ɾ����־
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public bool DeleteModeratorLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE " + condition);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// �õ���ǰָ��������ҳ����ǰ̨������־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetModeratorLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;

            if (condition != "") condition = " WHERE " + condition;

            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  " + condition + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  " + condition + " ORDER BY [id] DESC) AS tblTmp ) " + (condition.Replace("WHERE", "") == "" ? "" : "AND " + condition.Replace("WHERE", "")) + " ORDER BY [id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// �õ�ǰ̨������־��¼��
        /// </summary>
        /// <returns></returns>
        public int GetModeratorLogListCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// �õ�ָ����ѯ�����µ�ǰ̨������־��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public int GetModeratorLogListCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// ɾ����־
        /// </summary>
        /// <returns></returns>
        public bool DeleteMedalLog()
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��ָ������ɾ����־
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public bool DeleteMedalLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// �õ���ǰָ��ҳ����ѫ����־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <returns></returns>
        public DataTable GetMedalLogList(int pagesize, int currentpage)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// �õ���ǰָ��������ҳ����ѫ����־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetMedalLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition + " ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp ) AND " + condition + " ORDER BY [id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// �õ�������־��¼��
        /// </summary>
        /// <returns></returns>
        public int GetMedalLogListCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "medalslog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// �õ�ָ����ѯ�����µ�ѫ����־��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public int GetMedalLogListCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }


        /// <summary>
        /// ����IP��ȡ�����¼��¼
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public DataTable GetErrLoginRecordByIP(string ip)
        {
            DbParameter[] prams = {
										 DbHelper.MakeInParam("@ip",(DbType)OleDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [errcount], [lastupdate] FROM [" + BaseConfigs.GetTablePrefix + "failedlogins] WHERE [ip]=@ip", prams).Tables[0];
        }

        /// <summary>
        /// ����ָ��IP�Ĵ����¼��
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int AddErrLoginCount(string ip)
        {
            DbParameter[] prams = {
										 DbHelper.MakeInParam("@ip",(DbType)OleDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "failedlogins] SET [errcount]=[errcount]+1, [lastupdate]=GETDATE() WHERE [ip]=@ip", prams);
        }

        /// <summary>
        /// ����ָ��IP�Ĵ����¼
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int AddErrLoginRecord(string ip)
        {
            DbParameter[] prams = {
										 DbHelper.MakeInParam("@ip",(DbType)OleDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "failedlogins] ([ip], [errcount], [lastupdate]) VALUES(@ip, 1, GETDATE())", prams);
        }

        /// <summary>
        /// ��ָ��IP�Ĵ����¼��������Ϊ1
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int ResetErrLoginCount(string ip)
        {
            DbParameter[] prams = {
										 DbHelper.MakeInParam("@ip",(DbType)OleDbType.Char,15, ip),
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "failedlogins] SET [errcount]=1, [lastupdate]=GETDATE() WHERE [ip]=@ip", prams);
        }

        /// <summary>
        /// ɾ��ָ��IP���߳���15��ļ�¼
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int DeleteErrLoginRecord(string ip)
        {
            DbParameter[] prams = {
										 DbHelper.MakeInParam("@ip",(DbType)OleDbType.Char,15, ip),
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "failedlogins] WHERE [ip]=@ip OR DATEDIFF(n,[lastupdate], GETDATE()) > 15", prams);
        }

        public int GetPostCount(string posttablename)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([pid]) FROM [" + posttablename + "]").Tables[0].Rows[0][0].ToString());
        }

        public DataTable GetPostTableList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0];
        }

        public int UpdateDetachTable(int fid, string description)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid),
                                        DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 50, description)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "tablelist] SET [description]=@description  Where [id]=@fid";
            //fid, description);
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int StartFullIndex(string dbname)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("USE " + dbname + ";");
            sb.Append("EXECUTE sp_fulltext_database 'enable';");
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }
        public void CreatePostTableAndIndex(string tablename)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, GetSpecialTableFullIndexSQL(tablename, GetDbName()));
        }


        /// <summary>
        /// �õ�ָ�����ӷֱ��ȫ����������(���)���
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetSpecialTableFullIndexSQL(string tablename, string dbname)
        {
            #region ����

            //StringBuilder sb = new StringBuilder();
            //sb.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[" + tablename + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [" + tablename + "];");
            //sb.Append("CREATE TABLE [" + tablename + "] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ," +
            //    "[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ," +
            //    "[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ," +
            //    "[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ," +
            //    "[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ," +
            //    "[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ," +
            //    "[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            //sb.Append(";");
            //sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY]");
            //sb.Append(";");

            //sb.Append("ALTER TABLE [" + tablename + "] ADD ");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR [pid],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR [layer],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR [posterid],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR [message],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR [lastedit],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR [usesig],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR [smileyoff],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR [rate],");
            //sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR [ratetimes]");

            //sb.Append(";");
            //sb.Append("CREATE  INDEX [parentid] ON [" + tablename + "]([parentid]) ON [PRIMARY]");
            //sb.Append(";");

            //sb.Append("CREATE  UNIQUE  INDEX [showtopic] ON [" + tablename + "]([tid], [invisible], [pid]) ON [PRIMARY]");
            //sb.Append(";");


            //sb.Append("CREATE  INDEX [treelist] ON [" + tablename + "]([tid], [invisible], [parentid]) ON [PRIMARY]");
            //sb.Append(";");

            #endregion

            #region ��ȫ������

            //sb.Append("USE " + dbname + " \r\n");
            //sb.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
            //sb.Append("IF(SELECT DATABASEPROPERTY('[" + dbname + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
            //sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
            //sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_table '[" + tablename + "]', 'drop' ;");
            //sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
            //sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
            //sb.Append("EXECUTE sp_fulltext_column '[" + tablename + "]','message','add';");
            //sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','activate';");
            //sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

            #endregion

            //return sb.ToString();

            return "";
        }

        public void AddPostTableToTableList(string description, int mintid, int maxtid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 50, description),
                                        DbHelper.MakeInParam("@mintid", (DbType)OleDbType.Integer, 4, mintid),
                                        DbHelper.MakeInParam("@maxtid", (DbType)OleDbType.Integer, 4, maxtid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "tablelist] ([description],[mintid],[maxtid]) VALUES(@description, @mintid, @maxtid)",parms);
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
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX([tid]) FROM [" + BaseConfigs.GetTablePrefix + "topics]").Tables[0];
        }

        public DataTable GetPostCountFromIndex(string postsid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [rows] FROM [sysindexes] WHERE [name]='PK_" + BaseConfigs.GetTablePrefix + "posts" + postsid + "'").Tables[0];
        }

        public DataTable GetPostCountTable(string postsid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(pid) FROM [" + BaseConfigs.GetTablePrefix + "posts" + postsid + "]").Tables[0];
        }

        public void TestFullTextIndex(int posttableid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "SELECT TOP 1 [pid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC");
        }

        public DataRowCollection GetRateRange(int scoreid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid], [raterange] FROM [" +
                                                     BaseConfigs.GetTablePrefix +
                                                     "usergroups] WHERE [raterange] LIKE '%" + scoreid + ",True,%'").
                            Tables[0].Rows;
        }

        public void UpdateRateRange(string raterange, int groupid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix +
                                                  "usergroups] SET [raterange]='" +
                                                  raterange +
                                                  "' WHERE [groupid]=" + groupid);
        }

        public int GetMaxTableListId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX([id]), 0) FROM " + BaseConfigs.GetTablePrefix + "tablelist"), 0);
        }

        public int GetMaxPostTableTid(string posttablename)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX([tid]), 0) FROM " + posttablename), 0) + 1;
        }

        public int GetMinPostTableTid(string posttablename)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MIN([tid]), 0) FROM " + posttablename), 0) + 1;
        }

        public void AddAdInfo(int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available),
                                        DbHelper.MakeInParam("@type", (DbType)OleDbType.VarWChar, 50, type),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 50, title),
                                        DbHelper.MakeInParam("@targets", (DbType)OleDbType.VarWChar, 255, targets),
                                        DbHelper.MakeInParam("@parameters", (DbType)OleDbType.VarWChar, 0, parameters),
                                        DbHelper.MakeInParam("@code", (DbType)OleDbType.VarWChar, 0, code),
                                        DbHelper.MakeInParam("@starttime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(endtime))                                        
                                    };
            string sql = "INSERT INTO  [" + BaseConfigs.GetTablePrefix + "advertisements] ([available],[type],[displayorder],[title],[targets],[parameters],[code],[starttime],[endtime]) VALUES(@available,@type,@displayorder,@title,@targets,@parameters,@code,@starttime,@endtime)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetAdvertisements()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "advertisements] ORDER BY [advid] ASC";
        }

        public DataRowCollection GetTargetsForumName(string targets)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [name] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] IN(0" + targets + "0)").Tables[0].Rows;
        }

        public int UpdateAdvertisementAvailable(string aidlist, int available)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "advertisements] SET [available]=@available  WHERE [advid] IN(" + aidlist + ")";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int UpdateAdvertisement(int aid, int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@aid", (DbType)OleDbType.Integer, 4, aid),
                                        DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available),
                                        DbHelper.MakeInParam("@type", (DbType)OleDbType.VarWChar, 50, type),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 50, title),
                                        DbHelper.MakeInParam("@targets", (DbType)OleDbType.VarWChar, 255, targets),
                                        DbHelper.MakeInParam("@parameters", (DbType)OleDbType.VarWChar, 0, parameters),
                                        DbHelper.MakeInParam("@code", (DbType)OleDbType.VarWChar, 0, code),
                                        DbHelper.MakeInParam("@starttime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(endtime))
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "advertisements] SET [available]=@available,[type]=@type, [displayorder]=@displayorder,[title]=@title,[targets]=@targets,[parameters]=@parameters,[code]=@code,[starttime]=@starttime,[endtime]=@endtime WHERE [advid]=@aid";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteAdvertisement(string aidlist)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [advid] IN (" + aidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
									   DbHelper.MakeInParam("@authorid",(DbType)OleDbType.Integer,4,posterid),
									   DbHelper.MakeInParam("@buydate",(DbType)OleDbType.DBTimeStamp,4,DateTime.Now),
									   DbHelper.MakeInParam("@amount",(DbType)OleDbType.Integer,4,price),
									   DbHelper.MakeInParam("@netamount",(DbType)OleDbType.Numeric,8,netamount)
								   };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + creditsTrans + "] = [extcredits" + creditsTrans + "] - " + price.ToString() + " WHERE [uid] = @uid", prams);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + creditsTrans + "] = [extcredits" + creditsTrans + "] + @netamount WHERE [uid] = @authorid", prams);
        }

        public int AddPaymentLog(int uid, int tid, int posterid, int price, float netamount)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
									   DbHelper.MakeInParam("@authorid",(DbType)OleDbType.Integer,4,posterid),
									   DbHelper.MakeInParam("@buydate",(DbType)OleDbType.DBTimeStamp,4,DateTime.Now),
									   DbHelper.MakeInParam("@amount",(DbType)OleDbType.Integer,4,price),
									   DbHelper.MakeInParam("@netamount",(DbType)OleDbType.Numeric,8,netamount)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "paymentlog] ([uid],[tid],[authorid],[buydate],[amount],[netamount]) VALUES(@uid,@tid,@authorid,@buydate,@amount,@netamount)", prams);
        }

        /// <summary>
        /// �ж��û��Ƿ��ѹ�������
        /// </summary>
        /// <param name="tid">����id</param>
        /// <param name="uid">�û�id</param>
        /// <returns></returns>
        public bool IsBuyer(int tid, int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [tid] = @tid AND [uid]=@uid", prams), 0) > 0;
        }

        public DataTable GetPayLogInList(int pagesize, int currentpage, int uid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + "[ " + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id]) FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            return dt;
        }

        /// <summary>
        /// ��ȡָ���û���������־��¼��
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns></returns>
        public int GetPaymentLogInRecordCount(int uid)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [authorid]=" + uid).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// ����ָ���û���֧����־��¼��
        /// </summary>
        /// <param name="pagesize">ÿҳ��¼��</param>
        /// <param name="currentpage">��ǰҳ</param>
        /// <param name="uid">�û�id</param>
        /// <returns></returns>
        public DataTable GetPayLogOutList(int pagesize, int currentpage, int uid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            return dt;
        }

        /// <summary>
        /// ����ָ���û�֧����־����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns></returns>
        public int GetPaymentLogOutRecordCount(int uid)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [uid]=" + uid).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// ��ȡָ������Ĺ����¼
        /// </summary>
        /// <param name="pagesize">ÿҳ��¼��</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="tid">����id</param>
        /// <returns></returns>
        public DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "users].[username] AS username FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "users].[username] AS username FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
            return dt;
        }

        /// <summary>
        /// ���⹺���ܴ���
        /// </summary>
        /// <param name="tid">����id</param>
        /// <returns></returns>
        public int GetPaymentLogByTidCount(int tid)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [tid]=" + tid), 0);
        }


        public void AddSmiles(int id, int displayorder, int type, string code, string url)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@type", (DbType)OleDbType.Integer, 4, type),
                                        DbHelper.MakeInParam("@code", (DbType)OleDbType.VarWChar, 30, code),
                                        DbHelper.MakeInParam("@url", (DbType)OleDbType.VarChar, 60, url)
                                    };


            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "smilies] ([id],[displayorder],[type],[code],[url]) Values (@id,@displayorder,@type,@code,@url)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetIcons()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=1";
        }

        public string DeleteSmily(int id)
        {
            return "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [id]=" + id;
        }

        public void DeleteSmilyByType(int type)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=" + type;
            DbHelper.ExecuteNonQuery(CommandType.Text,sql);
        }

        public int UpdateSmilies(int id, int displayorder, int type, string code, string url)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@type", (DbType)OleDbType.Integer, 4, type),
                                        DbHelper.MakeInParam("@code", (DbType)OleDbType.VarWChar, 30, code),
                                        DbHelper.MakeInParam("@url", (DbType)OleDbType.VarChar, 60, url)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "smilies] SET [displayorder]=@displayorder,[type]=@type,[code]=@code,[url]=@url Where [id]=@id";

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int UpdateSmiliesPart(string code, int displayorder, int id)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, displayorder),
                                        DbHelper.MakeInParam("@code", (DbType)OleDbType.VarWChar, 30, code)
                                    };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "smilies] SET [code]=@code,[displayorder]=@displayorder WHERE [id]=@id";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int DeleteSmilies(string idlist)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies]  WHERE [ID] IN(" + idlist + ")");
        }

        public string GetSmilies()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0";
        }

        public int GetMaxSmiliesId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(id), 0) FROM " + BaseConfigs.GetTablePrefix + "smilies"), 0) + 1;
        }

        public DataTable GetSmiliesInfoByType(int type)
        {
            DbParameter parm = DbHelper.MakeInParam("@type", (DbType)OleDbType.Integer, 4, type);
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=@type";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
        }


        /// <summary>
        /// �õ����������
        /// </summary>
        /// <returns>���������</returns>
        public IDataReader GetSmiliesList()
        {
            IDataReader dr = DbHelper.ExecuteReader(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0 ORDER BY [displayorder] DESC,[id] ASC");
            return dr;
        }

        /// <summary>
        /// �õ����������
        /// </summary>
        /// <returns>�������</returns>
        public DataTable GetSmiliesListDataTable()
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] ORDER BY [type],[displayorder],[id]");
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
        /// �õ���������ı��������
        /// </summary>
        /// <returns>�������</returns>
        public DataTable GetSmiliesListWithoutType()
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]<>0 ORDER BY [type],[displayorder],[id]");
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
        /// ��ñ�������б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetSmilieTypes()
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0 ORDER BY [displayorder],[id]");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        public DataRow GetSmilieTypeById(string id)
        {
            DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [id]=" + id);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }
        /// <summary>
        /// ���ͳ����
        /// </summary>
        /// <returns>ͳ����</returns>
        public DataRow GetStatisticsRow()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "statistics]").Tables[0].Rows[0];
        }

        /// <summary>
        /// �������е�ͳ���б��浽���ݿ�
        /// </summary>
        public void SaveStatisticsRow(DataRow dr)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@totaltopic", (DbType)OleDbType.Integer,4,Int32.Parse(dr["totaltopic"].ToString())),
									   DbHelper.MakeInParam("@totalpost",(DbType)OleDbType.Integer,4,Int32.Parse(dr["totalpost"].ToString())),
									   DbHelper.MakeInParam("@totalusers",(DbType)OleDbType.Integer,4,Int32.Parse(dr["totalusers"].ToString())),
									   DbHelper.MakeInParam("@lastusername",(DbType)OleDbType.VarWChar,20,dr["totalusers"].ToString()),
									   DbHelper.MakeInParam("@lastuserid",(DbType)OleDbType.Integer,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									   DbHelper.MakeInParam("@highestonlineusercount",(DbType)OleDbType.Integer,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									   DbHelper.MakeInParam("@highestonlineusertime",(DbType)OleDbType.DBTimeStamp,4,DateTime.Parse(dr["highestonlineusertime"].ToString())),
									   DbHelper.MakeInParam("@highestposts",(DbType)OleDbType.Integer,4,Int32.Parse(dr["highestposts"].ToString())),
									   DbHelper.MakeInParam("@highestpostsdate",(DbType)OleDbType.Char,10,dr["highestpostsdate"].ToString())
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=@totaltopic,[totalpost]=@totalpost, [totalusers]=@totalusers, [lastusername]=@lastusername, [lastuserid]=@lastuserid, [highestonlineusercount]=@highestonlineusercount, [highestonlineusertime]=@highestonlineusertime, [highestposts]=@highestposts, [highestpostsdate]=@highestpostsdate", prams);
        }

        public void UpdateYesterdayPosts(string posttableid)
        {
            int max = Convert.ToInt32(posttableid);
            int min = max > 4 ? (max - 3) : 1;
            

            StringBuilder builder = new StringBuilder();

            for (int i = max; i >= min; i--)
            {
                if (i < max)
                    builder.Append(" UNION ");
                builder.AppendFormat("SELECT COUNT(1) AS [c] FROM [{0}posts{1}] WHERE [postdatetime] < '{2}' AND [postdatetime] > '{3}' AND [invisible]=0", BaseConfigs.GetTablePrefix, i.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            }
            string commandText = string.Format("SELECT SUM([c]) FROM ({0})t", builder.ToString());
            int yesterdayposts = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText), 0);



            int highestposts = Utils.StrToInt(GetStatisticsRow()["highestposts"], 0);
            //int yesterdayposts = Utils.StrToInt(GetStatisticsRow()["yesterdayposts"], 0);
            builder.Remove(0, builder.Length);
            builder.AppendFormat("UPDATE [{0}statistics] SET [yesterdayposts]=", BaseConfigs.GetTablePrefix);
            builder.Append(yesterdayposts.ToString());
            if (yesterdayposts > highestposts)
            {
                builder.Append(", [highestposts]=");
                builder.Append(yesterdayposts.ToString());
                builder.Append(", [highestpostsdate]='");
                builder.Append(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                builder.Append("'");
            }

            DbHelper.ExecuteNonQuery(builder.ToString());

        }

        public IDataReader GetAllForumStatistics()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1) AS [todaypostcount] FROM [{0}forums] WHERE [layer]=1", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetForumStatistics(int fid)
        {
            DbParameter[] prams = { DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer,4, fid) };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1 AND [fid] = @fid) AS [todaypostcount] FROM [{0}forums] WHERE [fid] = @fid AND [layer]=1", BaseConfigs.GetTablePrefix), prams);
        }

        /// <summary>
        /// ����ָ�����Ƶ�ͳ����
        /// </summary>
        /// <param name="param">��Ŀ����</param>
        /// <param name="Value">ָ�����ֵ</param>
        /// <returns>������</returns>
        public int UpdateStatistics(string param, int intValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (!param.Equals(""))
            {
                sb.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET ");
                sb.Append(param);
                sb.Append(" = ");
                sb.Append(intValue);
            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }

        /// <summary>
        /// ����ָ�����Ƶ�ͳ����
        /// </summary>
        /// <param name="param">��Ŀ����</param>
        /// <param name="Value">ָ�����ֵ</param>
        /// <returns>������</returns>
        public int UpdateStatistics(string param, string strValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (!param.Equals(""))
            {
                sb.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET ");
                sb.Append(param);
                sb.Append(" = '");
                sb.Append(strValue);
                sb.Append("'");
            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }

        /// <summary>
        /// ���ǰ̨��Ч��ģ���б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetValidTemplateList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
        }

        /// <summary>
        /// ���ǰ̨��Ч��ģ��ID�б�
        /// </summary>
        /// <returns>ģ��ID�б�</returns>
        public DataTable GetValidTemplateIDList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [templateid] FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
        }

        public DataTable GetPost(string posttablename, int pid)
        {
            DbParameter parm = DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 *  FROM [" + posttablename + "] WHERE [pid]=@pid", parm).Tables[0];
        }

        public DataTable GetMainPostByTid(string posttablename, int tid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + posttablename + "] WHERE [layer]=0  AND [tid]=@tid", parm).Tables[0];
        }

        public DataTable GetAttachmentsByPid(int pid)
        {
            DbParameter parm = DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", parm).Tables[0];
        }

        public DataTable GetAdvertisement(int aid)
        {
            //�˺�������Advs.cs�ļ��нϺ�
            DbParameter parm = DbHelper.MakeInParam("@aid", (DbType)OleDbType.Integer, 4, aid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [advid]=@aid", parm).Tables[0];
        }

        private string GetSearchTopicTitleSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int digest, string keyword)
        {
            keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyword);

            // �滻ת���ַ�
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [displayorder]>=0", BaseConfigs.GetTablePrefix);

            if (posterid > 0)
            {
                strSQL.Append(" AND [posterid]=");
                strSQL.Append(posterid);
            }

            if (digest > 0)
            {
                strSQL.Append(" AND [digest]>0 ");
            }

            if (searchforumid != string.Empty)
            {
                strSQL.Append(" AND [fid] IN (");
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
                    strKeyWord.Append(" OR [title] ");

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

            strSQL.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE [{0}topics].[displayorder]>=0 AND ", BaseConfigs.GetTablePrefix, posttableid, orderfield);

            if (searchforumid != string.Empty)
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
                    strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
                    strKeyWord.Append(RegEsc(keywordlist[i]));
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

            return strSQL.ToString();
        }

        private string GetSearchSpacePostTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
        {
            keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyword);

            // �滻ת���ַ�
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT [postid] FROM [{0}spaceposts] WHERE [{0}spaceposts].[poststatus]=1 ", BaseConfigs.GetTablePrefix);
            
            if (posterid > 0)
            {
                strSQL.Append(" AND [uid]=");
                strSQL.Append(posterid);
            }

            if (searchtime != 0)
            {
                strSQL.AppendFormat(" AND [{0}spaceposts].[postdatetime]", BaseConfigs.GetTablePrefix);
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
                    strKeyWord.Append(" OR [title] ");
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
                    strSQL.Append("[commentcount]");
                    break;
                case 2:
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

            return strSQL.ToString();
        }

        private string GetSearchAlbumTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
        {
            keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyword);

            // �滻ת���ַ�
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT [albumid] FROM [{0}albums] WHERE [{0}albums].[type]=0 ", BaseConfigs.GetTablePrefix);

            if (posterid > 0)
            {
                strSQL.Append(" AND [userid]=");
                strSQL.Append(posterid);
            }

            if (searchtime != 0)
            {
                strSQL.AppendFormat(" AND [{0}albums].[createdatetime]", BaseConfigs.GetTablePrefix);
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
                    strKeyWord.Append(" OR [title] ");
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
                    strSQL.Append("[albumid]");
                    break;
                default:
                    strSQL.Append("[createdatetime]");
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
                string sql = string.Format(@"SELECT DISTINCT [tid], 'forum' AS [datafrom] FROM [{0}posts{1}] WHERE [posterid]={2} AND [tid] NOT IN (SELECT [tid] FROM [{0}topics] WHERE [posterid]={2} AND [displayorder]<0) UNION ALL SELECT [albumid],'album' AS [datafrom] FROM [{0}albums] WHERE   [userid]={2} UNION ALL SELECT [postid],'spacepost' AS [datafrom] FROM [{0}spaceposts] WHERE [uid]={2}", BaseConfigs.GetTablePrefix, posttableid, posterid);
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

            //if (reader != null)
            //{
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
            //}
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
        /// ����ָ��������������
        /// </summary>
        /// <param name="posttableid">���ӱ�id</param>
        /// <param name="userid">�û�id</param>
        /// <param name="usergroupid">�û���id</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="posterid">������id</param>
        /// <param name="type">��������</param>
        /// <param name="searchforumid">�������id</param>
        /// <param name="keywordtype">�ؼ�������</param>
        /// <param name="searchtime">����ʱ��</param>
        /// <param name="searchtimetype">����ʱ������</param>
        /// <param name="resultorder">�������ʽ</param>
        /// <param name="resultordertype">�����������</param>
        /// <returns>����ɹ��򷵻�searchid, ���򷵻�-1</returns>
        public int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, string type, string searchforumid, int keywordtype, int searchtime, int searchtimetype, int resultorder, int resultordertype)
        {

            // ����30���ӵĻ����¼����ɾ��
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
            // �ؼ���������������һ��������Ϊ��
            if (keyword.Equals(""))//����������
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

                //��������ж�
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
                // ����Σ���ַ�
                keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

                StringBuilder strKeyWord = new StringBuilder(keyword);

                // �滻ת���ַ�
                strKeyWord.Replace("'", "''");
                strKeyWord.Replace("%", "[%]");
                strKeyWord.Replace("_", "[_]");
                strKeyWord.Replace("[", "[[]");


                // ��SQL��ѯ����ѭ��ָ��Ϊ"forumid, posterid, ����ʱ�䷶Χ, �ؼ���"
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

            DbParameter[] prams2 = {
										DbHelper.MakeInParam("@searchstring",(DbType)OleDbType.VarChar,255, sql),
										DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,userid),
										DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,4,usergroupid)
									};
            int searchid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format(@"SELECT TOP 1 [searchid] FROM [{0}searchcaches] WHERE [searchstring]=@searchstring AND [groupid]=@groupid", BaseConfigs.GetTablePrefix), prams2), -1);

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

        public string BackUpDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            //SQLServer svr = new SQLServerClass();
            //try
            //{
            //    svr.Connect(ServerName, UserName, Password);
            //    Backup bak = new BackupClass();
            //    bak.Action = 0;
            //    bak.Initialize = true;
            //    bak.Files = backuppath + strFileName + ".config";
            //    bak.Database = strDbName;
            //    bak.SQLBackup(svr);
            //    return string.Empty;
            //}
            //catch(Exception ex)
            //{
            //    string message = ex.Message.Replace("'", " ");
            //    message = message.Replace("\n", " ");
            //    message = message.Replace("\\", "/");
            //    return message;
            //}
            //finally
            //{
            //    svr.DisConnect();
            //}
            return "";
        }

        public string RestoreDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            //#region ���ݿ�Ļָ��Ĵ���

            //SQLServer svr = new SQLServerClass();
            //try
            //{
            //    svr.Connect(ServerName, UserName, Password);
            //    QueryResults qr = svr.EnumProcesses(-1);
            //    int iColPIDNum = -1;
            //    int iColDbName = -1;
            //    for (int i = 1; i <= qr.Columns; i++)
            //    {
            //        string strName = qr.get_ColumnName(i);
            //        if (strName.ToUpper().Trim() == "SPID")
            //        {
            //            iColPIDNum = i;
            //        }
            //        else if (strName.ToUpper().Trim() == "DBNAME")
            //        {
            //            iColDbName = i;
            //        }
            //        if (iColPIDNum != -1 && iColDbName != -1)
            //            break;
            //    }

            //    for (int i = 1; i <= qr.Rows; i++)
            //    {
            //        int lPID = qr.GetColumnLong(i, iColPIDNum);
            //        string strDBName = qr.GetColumnString(i, iColDbName);
            //        if (strDBName.ToUpper() == strDbName.ToUpper())
            //            svr.KillProcess(lPID);
            //    }


            //    Restore res = new RestoreClass();
            //    res.Action = 0;
            //    string path = backuppath + strFileName + ".config";
            //    res.Files = path;

            //    res.Database = strDbName;
            //    res.ReplaceDatabase = true;
            //    res.SQLRestore(svr);

            //    return string.Empty;
            //}
            //catch (Exception err)
            //{
            //    string message = err.Message.Replace("'", " ");
            //    message = message.Replace("\n", " ");
            //    message = message.Replace("\\", "/");
                
            //    return message;
            //}
            //finally
            //{
            //    svr.DisConnect();
            //}

            //#endregion

            return "";
        }

        public string SearchVisitLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
        {
            string sqlstring = null;
            sqlstring += " [visitid]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (others != "")
            {
                sqlstring += " AND [others] LIKE '%" + RegEsc(others) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        
        }


        public string SearchMedalLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string reason)
        {
            string sqlstring = null;
            sqlstring += " [id]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (reason != "")
            {
                sqlstring += " AND [reason] LIKE '%" + RegEsc(reason) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }
            return sqlstring;
        }

        public string SearchModeratorManageLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
        {
            string sqlstring = null;
            sqlstring += " [id]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (others != "")
            {
                sqlstring += " AND [reason] LIKE '%" + RegEsc(others) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [moderatorname] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        }

        public string SearchPaymentLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username)
        {

            string sqlstring = null;
            sqlstring += " [" + BaseConfigs.GetTablePrefix + "paymentlog].[id]>0";

            if (postdatetimeStart.ToString() != "")
            {
                sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[buydate]>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (postdatetimeEnd.ToString() != "")
            {
                sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[buydate]<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            if (Username != "")
            {
                string usernamesearch = " WHERE (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        usernamesearch += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                usernamesearch = usernamesearch.Substring(0, usernamesearch.Length - 3) + ")";

                //�ҳ���ǰ�û���������UID
                DataTable dt = DbHelper.ExecuteDataset("SELECT [uid] From [" + BaseConfigs.GetTablePrefix + "users] " + usernamesearch).Tables[0];
                string uid = "-1";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        uid += "," + dr["uid"].ToString();
                    }
                }
                sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] IN(" + uid + ")";

            }

            return sqlstring;
        }

        public string SearchRateLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
        {
            string sqlstring = null;
            sqlstring += " [id]>0";

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            if (others != "")
            {
                sqlstring += " AND [reason] LIKE '%" + RegEsc(others) + "%'";
            }

            if (Username != "")
            {
                sqlstring += " AND (";
                foreach (string word in Username.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        }

        public string DeletePrivateMessages(bool isnew, string postdatetime, string msgfromlist, bool lowerupper, string subject, string message, bool isupdateusernewpm)
        {
            string sqlstring = null;
            sqlstring += "WHERE [pmid]>0";

            if (isnew)
            {
                sqlstring += " AND [new]=0";
            }

            if (postdatetime != "")
            {
                sqlstring += " AND DATEDIFF(day,postdatetime,getdate())>=" + postdatetime + "";
            }

            if (msgfromlist != "")
            {
                sqlstring += " AND (";
                foreach (string msgfrom in msgfromlist.Split(','))
                {
                    if (msgfrom.Trim() != "")
                    {
                        if (lowerupper)
                        {
                            sqlstring += " [msgfrom]='" + msgfrom + "' OR";
                        }
                        else
                        {
                            sqlstring += " [msgfrom] COLLATE Chinese_PRC_CS_AS_WS ='" + msgfrom + "' OR";

                        }
                    }
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (subject != "")
            {
                sqlstring += " AND (";
                foreach (string sub in subject.Split(','))
                {
                    if (sub.Trim() != "")
                        sqlstring += " [subject] LIKE '%" + RegEsc(sub) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (message != "")
            {
                sqlstring += " AND (";
                foreach (string mess in message.Split(','))
                {
                    if (mess.Trim() != "")
                        sqlstring += " [message] LIKE '%" + RegEsc(mess) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (isupdateusernewpm)
            {
                DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpm]=0 WHERE [uid] IN (SELECT [msgtoid] FROM [" + BaseConfigs.GetTablePrefix + "pms] " + sqlstring + ")");
            }

            DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] " + sqlstring);

            return sqlstring;
        }

        public bool IsExistSmilieCode(string code, int currentid)
        {
            DbParameter[] prams = {
										DbHelper.MakeInParam("@code",(DbType)OleDbType.VarWChar, 30, code),
										DbHelper.MakeInParam("@currentid",(DbType)OleDbType.Integer, 4, currentid)
									};
            string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [code]=@code AND [id]<>@currentid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count != 0;
        }

        public string  GetSmilieByType(int id)
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=" + id;
        }


        public string AddTableData()
        {

            return "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<=3 ORDER BY [groupid]";

        }

        public string Global_UserGrid_GetCondition(string getstring)
        {

            return "[" + BaseConfigs.GetTablePrefix + "users].[username]='" + getstring + "'";

        }

        public int Global_UserGrid_RecordCount()
        {

            return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]").Tables[0].Rows[0][0].ToString());

        }


        public int Global_UserGrid_RecordCount(string condition)
        {

            return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]  WHERE " + condition).Tables[0].Rows[0][0].ToString());

        }

        public string Global_UserGrid_SearchCondition(bool islike, bool ispostdatetime, string username, string nickname, string UserGroup, string email, string credits_start, string credits_end, string lastip, string posts, string digestposts, string uid, string joindateStart, string joindateEnd)
        {

            string searchcondition = " [" + BaseConfigs.GetTablePrefix + "users].[uid]>0 ";
            if (islike)
            {
                if (username != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[username] like'%" + RegEsc(username) + "%'";
                if (nickname != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[nickname] like'%" + RegEsc(nickname) + "%'";
            }
            else
            {
                if (username != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[username] ='" + username + "'";
                if (nickname != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[nickname] ='" + nickname + "'";
            }

            if (UserGroup != "0")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[groupid]=" + UserGroup;
            }

            if (email != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[email] LIKE '%" + RegEsc(email) + "%'";
            }

            if (credits_start != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] >=" + credits_start;
            }

            if (credits_end != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] <=" + credits_end;
            }

            if (lastip != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[lastip] LIKE '%" + RegEsc(lastip) + "%'";
            }

            if (posts != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[posts] >=" + posts;
            }


            if (digestposts != "")
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[digestposts] >=" + digestposts;
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
                    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[uid] IN(" + uid + ")";
                }

            }

            if (ispostdatetime)
            {
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[joindate] >='" + DateTime.Parse(joindateStart).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[joindate] <='" + DateTime.Parse(joindateEnd).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }


            return searchcondition;

        }


        public DataTable Global_UserGrid_Top2(string searchcondition)
        {

            return DbHelper.ExecuteDataset("SELECT TOP 2 [" + BaseConfigs.GetTablePrefix + "users].[uid]  FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + searchcondition).Tables[0];

        }


        public System.Collections.ArrayList CheckDbFree()
        {
          
           
            return null;
        }

        public void DbOptimize(string tablelist)
        {
           
        }

        public void UpdateAdminUsergroup(string targetadminusergroup, string sourceadminusergroup)
        {

            DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=" + targetadminusergroup + " WHERE [groupid]=" + sourceadminusergroup);

        }

        public void UpdateUserCredits(string formula)
        {
            DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [credits]=" + formula);

        }

        public DataTable MailListTable(string usernamelist)
        {

            string strwhere = " WHERE [Email] Is Not null AND (";
            foreach (string username in usernamelist.Split(','))
            {
                if (username.Trim() != "")
                    strwhere += " [username] LIKE '%" + RegEsc(username.Trim()) + "%' OR ";
            }
            strwhere = strwhere.Substring(0, strwhere.Length - 3) + ")";

            DataTable dt = DbHelper.ExecuteDataset("SELECT [username],[Email]  FROM [" + BaseConfigs.GetTablePrefix + "users] " + strwhere).Tables[0];
            return dt;
        }

        public IDataReader GetTagsListByTopic(int topicid)
        {
            string sql = string.Format("SELECT [{0}tags].* FROM [{0}tags], [{0}topictags] WHERE [{0}topictags].[tagid] = [{0}tags].[tagid] AND [{0}topictags].[tid] = @topicid ORDER BY [orderid]", BaseConfigs.GetTablePrefix);

            DbParameter parm = DbHelper.MakeInParam("@topicid", (DbType)OleDbType.Integer, 4, topicid);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public IDataReader GetTagInfo(int tagid)
        {
            string sql = string.Format("SELECT * FROM [{0}tags] WHERE [tagid]=@tagid", BaseConfigs.GetTablePrefix);
            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)OleDbType.Integer, 4, tagid);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public void SetLastExecuteScheduledEventDateTime(string key, string servername, DateTime lastexecuted)
        {
            string sql = string.Format("{0}setlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)OleDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)OleDbType.VarChar, 100, servername),
                DbHelper.MakeInParam("@lastexecuted", (DbType)OleDbType.DBTimeStamp, 8, lastexecuted)
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, parms);
        }

        public DateTime GetLastExecuteScheduledEventDateTime(string key, string servername)
        {
            string sql = string.Format("{0}getlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)OleDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)OleDbType.VarChar, 100, servername),
                DbHelper.MakeOutParam("@lastexecuted", (DbType)OleDbType.DBTimeStamp, 8)
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, parms);

            return Convert.IsDBNull(parms[2].Value) ? DateTime.MinValue : Convert.ToDateTime(parms[2].Value);
        }

        public void UpdateStats(string type, string variable, int count)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@type", (DbType)OleDbType.Char, 10, type),
                DbHelper.MakeInParam("@variable", (DbType)OleDbType.Char, 20, variable),
                DbHelper.MakeInParam("@count", (DbType)OleDbType.Integer, 4, count)
            };
            string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+@count WHERE [type]=@type AND [variable]=@variable", BaseConfigs.GetTablePrefix);
            int lines = DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            if (lines == 0)
            {
                if (count == 0)
                {
                    parms[2].Value = 1;
                }
                commandText = string.Format("INSERT INTO [{0}stats] ([type],[variable],[count]) VALUES(@type, @variable, @count)", BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        public void UpdateStatVars(string type, string variable, string value)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@type", (DbType)OleDbType.Char, 20, type),
                DbHelper.MakeInParam("@variable", (DbType)OleDbType.Char, 20, variable),
                DbHelper.MakeInParam("@value", (DbType)OleDbType.VarWChar, 0, value)
            };
            string commandText = string.Format("UPDATE [{0}statvars] SET [value]=@value WHERE [type]=@type AND [variable]=@variable", BaseConfigs.GetTablePrefix);
            int lines = DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            if (lines == 0)
            {
                commandText = string.Format("INSERT INTO [{0}statvars] ([type],[variable],[value]) VALUES(@type, @variable, @value)", BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        public IDataReader GetAllStats()
        {
            string commandText = string.Format("SELECT [type], [variable], [count] FROM [{0}stats] ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetAllStatVars()
        {
            string commandText = string.Format("SELECT [type], [variable], [value] FROM [{0}statvars] ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetStatsByType(string type)
        {
            DbParameter parm = DbHelper.MakeInParam("@type", (DbType)OleDbType.Char, 10, type);
            string commandText = string.Format("SELECT [type], [variable], [count] FROM [{0}stats] WHERE [type]=@type ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetStatVarsByType(string type)
        {
            DbParameter parm = DbHelper.MakeInParam("@type", (DbType)OleDbType.Char, 10, type);
            string commandText = string.Format("SELECT [type], [variable], [value] FROM [{0}statvars] WHERE [type]=@type ORDER BY [type],[variable]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void DeleteOldDayposts()
        {
            string commandText = string.Format("DELETE FROM {0}statvars WHERE [type]='dayposts' AND [variable]<'{1}'", BaseConfigs.GetTablePrefix, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DateTime GetPostStartTime()
        {
            string commandText = string.Format("SELECT MIN([postdatetime]) FROM [{0}posts1] WHERE [invisible]=0", BaseConfigs.GetTablePrefix);
            Object obj = DbHelper.ExecuteScalar(CommandType.Text, commandText);
            if (obj == null)
                return DateTime.Now;
            return Convert.ToDateTime(obj);
        }

        public int GetForumCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}forums] WHERE [layer]>0 AND [status]>0", BaseConfigs.GetTablePrefix);
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetTodayPostCount(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0", BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetTodayNewMemberCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [joindate]>='{1}'", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetAdminCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [adminid]>0", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetNonPostMemCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [posts]=0", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd"));
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public IDataReader GetBestMember(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT TOP 1 [poster], COUNT(1) AS [posts] FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster] ORDER BY [posts] DESC", BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetMonthPostsStats(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT COUNT(1) AS [count],MONTH([postdatetime]) AS [month],YEAR([postdatetime]) AS [year] FROM [{0}posts{1}] GROUP BY MONTH([postdatetime]),YEAR([postdatetime]) ORDER BY YEAR([postdatetime]),MONTH([postdatetime])", BaseConfigs.GetTablePrefix, posttableid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetDayPostsStats(string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT COUNT(1) AS [count],YEAR([postdatetime]) AS [year],MONTH([postdatetime]) AS [month],DAY([postdatetime]) AS [day] FROM [{0}posts{1}] WHERE [invisible]=0 AND [postdatetime] > '{2}' GROUP BY DAY([postdatetime]), MONTH([postdatetime]),YEAR([postdatetime]) ORDER BY YEAR([postdatetime]),MONTH([postdatetime]),DAY([postdatetime])", BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByTopicCount(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [fid], [name], [topics] FROM [{1}forums] WHERE [status]>0 AND [layer]=0 ORDER BY [topics] DESC", count, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByPostCount(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [fid], [name], [posts] FROM [{1}forums] WHERE [status]>0 AND [layer]=0 ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByMonthPostCount(int count, string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByDayPostCount(int count, string posttableid)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";

            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetUsersRank(int count, string posttableid, string type)
        {
            if (!Utils.IsNumeric(posttableid))
                posttableid = "1";
            string commandText = "";
            switch(type)
            {
                case "posts":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [posts] FROM [{1}users] ORDER BY [posts] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "digestposts":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [digestposts] FROM [{1}users] ORDER BY [digestposts] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "thismonth":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
                    break;
                case "today":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, posttableid, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;

                case "credits":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [credits] FROM [{1}users] ORDER BY [credits] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits1":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits1] FROM [{1}users] ORDER BY [extcredits1] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits2":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits2] FROM [{1}users] ORDER BY [extcredits2] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits3":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits3] FROM [{1}users] ORDER BY [extcredits3] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits4":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits4] FROM [{1}users] ORDER BY [extcredits4] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits5":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits5] FROM [{1}users] ORDER BY [extcredits5] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits6":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits6] FROM [{1}users] ORDER BY [extcredits6] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits7":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits7] FROM [{1}users] ORDER BY [extcredits7] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits8":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits8] FROM [{1}users] ORDER BY [extcredits8] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                //case "oltime":
                //    commandText = string.Format("SELECT TOP {0} [username], [uid], [oltime] FROM [{1}users] ORDER BY [oltime] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                //    break;
                default:
                    return null;

            }
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetSuperModerators()
        {
            string commandText = string.Format("SELECT [fid], [uid] FROM [{0}moderators] WHERE [inherited]=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetModeratorsDetails(string uids, int oltimespan)
        {
            if (!Utils.IsNumericArray(uids.Split(',')))
                return null;
            string uidstr = "m.[uid] IN ({0}) OR ";
            if (uids == string.Empty)
                uidstr = string.Empty;
            string oltimeadd1 = string.Empty;
            string oltimeadd2 = string.Empty;
            if (oltimespan > 0)
            {
                oltimeadd1 = ", o.[thismonth] AS [thismonthol], o.[total] AS [totalol]";
                oltimeadd2 = string.Format(" LEFT JOIN [{0}onlinetime] o ON o.[uid]=m.[uid]", BaseConfigs.GetTablePrefix);
            }

            string commandText = string.Format("SELECT m.[uid], m.[username], m.[adminid], m.[lastactivity], m.[credits], m.[posts]{0} FROM [{1}users] m{2} WHERE {3}m.[adminid] IN (1, 2) ORDER BY m.[adminid]", oltimeadd1, BaseConfigs.GetTablePrefix, oltimeadd2, uidstr);
                //SELECT [uid], [username], [adminid], [lastactivity], [credits], [posts], [oltime] FROM [{0}users] WHERE {1}[adminid] IN (1, 2) ORDER BY [adminid]", BaseConfigs.GetTablePrefix, uidstr);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void UpdateStatCount(string browser, string os, string visitorsadd)
        {
            string month = DateTime.Now.Year + DateTime.Now.Month.ToString("00");
            string dayofweek = ((int)DateTime.Now.DayOfWeek).ToString();
            string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+1 WHERE ([type]='total' AND [variable]='hits') {1} OR ([type]='month' AND [variable]='{2}') OR ([type]='week' AND [variable]='{3}') OR ([type]='hour' AND [variable]='{4}')", BaseConfigs.GetTablePrefix, visitorsadd, month, dayofweek, DateTime.Now.Hour.ToString("00"));
            int affectedrows = DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            int updaterows = visitorsadd.Trim() == string.Empty ? 4 : 7;
            if (updaterows > affectedrows)
            {
            //    commandText = string.Format("INSERT INTO [{0}stats] ([type], [variable], [count]) VALUES ('month', '{1}', 1)", BaseConfigs.GetTablePrefix, DateTime.Now.Month.ToString("00"));
            //    DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            //}
            //else
            //{
                UpdateStats("browser", browser, 0);
                UpdateStats("os", os, 0);
                UpdateStats("total", "members", 0);
                UpdateStats("total", "guests", 0);
                UpdateStats("total", "hits", 0);
                UpdateStats("month", month, 0);
                UpdateStats("week", dayofweek, 0);
                UpdateStats("hour", DateTime.Now.Hour.ToString("00"), 0);
            }
        }

		#endregion

		#region HelpManage

		//ȡ�÷���
        public IDataReader GetHelpList(int id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        
                                    };
            string sql = "SELECT [id],[title],[message],[pid],[orderby] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=@id OR [id]=@id ORDER BY [pid] ASC, [orderby] ASC";

            return DbHelper.ExecuteReader(CommandType.Text, sql,parms);
    
        
        }


        public IDataReader ShowHelp(int id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id),
                                        
                                    };
            string sql = "SELECT [title],[message],[pid],[orderby] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [id]=@id";
            return DbHelper.ExecuteReader(CommandType.Text, sql,parms);

        }


        public IDataReader GetHelpClass()
        {

            string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=0 ORDER BY [orderby] ASC";
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }
        


        public void AddHelp(string title,string message,int pid,int orderby)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.Char, 100, title),
                                        DbHelper.MakeInParam("@message", (DbType)OleDbType.VarWChar,0,message),
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer,4, pid),
                                        DbHelper.MakeInParam("@orderby", (DbType)OleDbType.Integer, 4, orderby)
                                        
                                    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "help]([title],[message],[pid],[orderby]) VALUES(@title,@message,@pid,@orderby)";
        DbHelper.ExecuteNonQuery(CommandType.Text,sql,parms);
        }

        public void DelHelp(string idlist)
        {
            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@idlist", (DbType)OleDbType.Integer, 100, idlist)
                                       
                                        
            //                        };

            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [id] IN ("+idlist+") OR [pid] IN ("+idlist+")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void ModHelp(int id,string title,string message,int pid,int orderby)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.Char, 100, title),
                                        DbHelper.MakeInParam("@message", (DbType)OleDbType.VarWChar,0,message),
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer,4, pid),
                                        DbHelper.MakeInParam("@orderby", (DbType)OleDbType.Integer, 4, orderby),
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id)
                                        
                                    };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "help] SET [title]=@title,[message]=@message,[pid]=@pid,[orderby]=@orderby WHERE [id]=@id";
            DbHelper.ExecuteNonQuery(CommandType.Text,sql,parms);
        
        }

        //public string  admingrid()
        //{
        //    string sql = "select *,case pid when 0 then id else pid end as o from dnt_help order by o";

        //    return sql;
        
        
        //}

        public int HelpCount()
        {
            string sql = "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "help]";
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
        
        }

        public string BindHelpType()

        {
            string sql = "SELECT [id],[title] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=0 ORDER BY [orderby] ASC";
            return sql;
        
        }

        public void UpOrder(string orderby, string id)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@orderby", (DbType)OleDbType.Char, 100, orderby),
                                        DbHelper.MakeInParam("@id", (DbType)OleDbType.VarChar, 100,id),
                                        
                                        
                                    };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "help] SET [ORDERBY]=@orderby  Where id=@id";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

        }

		#endregion

		#region UserManage
        private static int _lastRemoveTimeout;

        public DataTable GetUsers(string idlist)
        {
            if (!Utils.IsNumericArray(idlist.Split(',')))
                return new DataTable();

            string sql = string.Format("SELECT [uid],[username] FROM [{0}users] WHERE [groupid] IN ({1})", BaseConfigs.GetTablePrefix, idlist);
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserGroupInfoByGroupid(int groupid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer, 4,groupid)
			};
            string sql = "SELECT TOP 1 * FROM  [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public DataTable GetAdmingroupByAdmingid(int admingid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@admingid",(DbType)OleDbType.Integer, 4,admingid)
			};
            string sql = "SELECT TOP 1 * FROM  [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@admingid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public DataTable GetMedal()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals]";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public string GetMedalSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals]";
        }

        public DataTable GetExistMedalList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [medalid],[image] FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [image]<>''").Tables[0];
        }

        public void AddMedal(int medalid, string name, int available, string image)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)OleDbType.Integer,2, medalid),
				DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar,50, name),
                DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available),
				DbHelper.MakeInParam("@image",(DbType)OleDbType.VarChar,30,image)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "medals] (medalid,name,available,image) Values (@medalid,@name,@available,@image)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void UpdateMedal(int medalid, string name, string image)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)OleDbType.Integer,2, medalid),
				DbHelper.MakeInParam("@name", (DbType)OleDbType.VarWChar,50, name),
				DbHelper.MakeInParam("@image",(DbType)OleDbType.VarChar,30,image)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medals] SET [name]=@name,[image]=@image  Where [medalid]=@medalid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void SetAvailableForMedal(int available, string medailidlist)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medals] SET [available]=@available WHERE [medalid] IN(" + medailidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void DeleteMedalById(string medailidlist)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [medalid] IN(" + medailidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public int GetMaxMedalId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(medalid), 0) FROM " + BaseConfigs.GetTablePrefix + "medals"), 0) + 1;
        }

        public string GetGroupInfo()
        {
            string sql = "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]";
            return sql;
        }

        /// <summary>
        /// ��õ�ָ����������Ϣ
        /// </summary>
        /// <returns>��������Ϣ</returns>
        public DataTable GetAdminGroupList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "admingroups]").Tables[0];
        }

        /// <summary>
        /// ���ù�������Ϣ
        /// </summary>
        /// <param name="__admingroupsInfo">��������Ϣ</param>
        /// <returns>���ļ�¼��</returns>
        public int SetAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@admingid",(DbType)OleDbType.Integer,2,admingroupsInfo.Admingid),
									   DbHelper.MakeInParam("@alloweditpost",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Alloweditpost),
									   DbHelper.MakeInParam("@alloweditpoll",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Alloweditpoll),
									   DbHelper.MakeInParam("@allowstickthread",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowstickthread),
									   DbHelper.MakeInParam("@allowmodpost",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowmodpost),
									   DbHelper.MakeInParam("@allowdelpost",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowdelpost),
									   DbHelper.MakeInParam("@allowmassprune",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowmassprune),
									   DbHelper.MakeInParam("@allowrefund",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowrefund),
									   DbHelper.MakeInParam("@allowcensorword",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowcensorword),
									   DbHelper.MakeInParam("@allowviewip",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowviewip),
									   DbHelper.MakeInParam("@allowbanip",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowbanip),
									   DbHelper.MakeInParam("@allowedituser",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowedituser),
									   DbHelper.MakeInParam("@allowmoduser",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowmoduser),
									   DbHelper.MakeInParam("@allowbanuser",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowbanuser),
									   DbHelper.MakeInParam("@allowpostannounce",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowpostannounce),
									   DbHelper.MakeInParam("@allowviewlog",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowviewlog),
									   DbHelper.MakeInParam("@disablepostctrl",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Disablepostctrl),
                                       DbHelper.MakeInParam("@allowviewrealname",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowviewrealname)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateadmingroup", prams);
        }

        /// <summary>
        /// ����һ���µĹ�������Ϣ
        /// </summary>
        /// <param name="__admingroupsInfo">Ҫ��ӵĹ�������Ϣ</param>
        /// <returns>���ļ�¼��</returns>
        public int CreateAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@admingid",(DbType)OleDbType.Integer,2,admingroupsInfo.Admingid),
									   DbHelper.MakeInParam("@alloweditpost",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Alloweditpost),
									   DbHelper.MakeInParam("@alloweditpoll",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Alloweditpoll),
									   DbHelper.MakeInParam("@allowstickthread",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowstickthread),
									   DbHelper.MakeInParam("@allowmodpost",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowmodpost),
									   DbHelper.MakeInParam("@allowdelpost",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowdelpost),
									   DbHelper.MakeInParam("@allowmassprune",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowmassprune),
									   DbHelper.MakeInParam("@allowrefund",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowrefund),
									   DbHelper.MakeInParam("@allowcensorword",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowcensorword),
									   DbHelper.MakeInParam("@allowviewip",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowviewip),
									   DbHelper.MakeInParam("@allowbanip",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowbanip),
									   DbHelper.MakeInParam("@allowedituser",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowedituser),
									   DbHelper.MakeInParam("@allowmoduser",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowmoduser),
									   DbHelper.MakeInParam("@allowbanuser",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowbanuser),
									   DbHelper.MakeInParam("@allowpostannounce",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowpostannounce),
									   DbHelper.MakeInParam("@allowviewlog",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowviewlog),
									   DbHelper.MakeInParam("@disablepostctrl",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Disablepostctrl),
                                       DbHelper.MakeInParam("@allowviewrealname",(DbType)OleDbType.TinyInt,1,admingroupsInfo.Allowviewrealname)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createadmingroup", prams);
        }

        /// <summary>
        /// ɾ��ָ���Ĺ�������Ϣ
        /// </summary>
        /// <param name="admingid">������ID</param>
        /// <returns>���ļ�¼��</returns>
        public int DeleteAdminGroupInfo(short admingid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@admingid",(DbType)OleDbType.Integer,2,admingid),
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid] = @admingid", prams);
        }

        public string GetAdminGroupInfoSql()
        {
            return "Select * From [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]>0 AND [radminid]<=3  Order By [groupid]";
        }

        public DataTable GetRaterangeByGroupid(int groupid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer, 4,groupid)
			};
            string sql = "SELECT TOP 1 [raterange] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void UpdateRaterangeByGroupid(string raterange, int groupid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@raterange",(DbType)OleDbType.VarWChar, 500,raterange),
				DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer, 4,groupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [raterange]=@raterange WHERE [groupid]=@groupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public string GetAudituserSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "users] Where [groupid]=8";
        }

        public DataSet GetAudituserUid()
        {
            string sql = "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8";
            return DbHelper.ExecuteDataset(CommandType.Text, sql);
        }

        public void ClearAuthstrByUidlist(string uidlist)
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [authstr]='' WHERE [uid] IN (" + uidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void ClearAllUserAuthstr()
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [authstr]='' WHERE [uid] IN (SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 )";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void DeleteUserByUidlist(string uidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] IN(" + uidlist + ")");
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(" + uidlist + ")");
        }

        public void DeleteAuditUser()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] IN (SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 )");
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 ");
        }

        public DataTable GetAuditUserEmail()
        {
            string sql = "SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }
        public DataTable GetUserEmailByUidlist(string uidlist)
        {
            string sql = "SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(" + uidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public string GetUserGroup()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]";
            return sql;
        }

        public string GetUserGroupTitle()
        {
            return "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]";
        }

        public DataTable GetUserGroupWithOutGuestTitle()
        {
            return DbHelper.ExecuteDataset("SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<>7  ORDER BY [groupid] ASC").Tables[0];
        }

        public string GetAdminUserGroupTitle()
        {
            string sql = "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]> 0 AND [radminid]<=3  ORDER BY [groupid]";
            return sql;
        }

        public void CombinationUsergroupScore(int sourceusergroupid, int targetusergroupid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@sourceusergroupid",(DbType)OleDbType.Integer, 4,sourceusergroupid),
				DbHelper.MakeInParam("@targetusergroupid",(DbType)OleDbType.Integer, 4,targetusergroupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditshigher]=(SELECT [creditshigher] FROM "
                + "[" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@sourceusergroupid) WHERE [groupid]=@targetusergroupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void DeleteUserGroupInfo(int groupid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer, 4,groupid)
			};
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "usergroups] Where [groupid]=@groupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void DeleteAdminGroupInfo(int admingid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@admingid",(DbType)OleDbType.Integer, 4,admingid)
			};
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] Where [admingid]=@admingid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void ChangeUsergroup(int soureceusergroupid, int targetusergroupid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@soureceusergroupid",(DbType)OleDbType.Integer, 4,soureceusergroupid),
                DbHelper.MakeInParam("@targetusergroupid",(DbType)OleDbType.Integer, 4,targetusergroupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@targetusergroupid WHERE [groupid]=@soureceusergroupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }
        public DataTable GetAdmingid(int admingid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@admingid",(DbType)OleDbType.Integer, 4,admingid)
			};
            string sql = "SELECT [admingid]  FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@admingid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void ChangeUserAdminidByGroupid(int adminid, int groupid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@adminid",(DbType)OleDbType.Integer, 4,adminid),
                DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer, 4,groupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=@adminid WHERE [groupid]=@groupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public DataTable GetAvailableMedal()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [available]=1";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public bool IsExistMedalAwardRecord(int medalid, int userid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)OleDbType.Integer,4, medalid),
				DbHelper.MakeInParam("@userid",(DbType)OleDbType.Integer,4,userid)
			};
            string sql = "SELECT TOP 1 ID FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE [medals]=@medalid AND [uid]=@userid";
            if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count != 0)
                return true;
            else
                return false;
        }

        public void AddMedalslog(int adminid, string adminname, string ip, string username, int uid, string actions, int medals, string reason)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@adminid", (DbType)OleDbType.Integer,4, adminid),
				DbHelper.MakeInParam("@adminname",(DbType)OleDbType.VarWChar,50,adminname),
                DbHelper.MakeInParam("@ip", (DbType)OleDbType.VarWChar,15, ip),
				DbHelper.MakeInParam("@username",(DbType)OleDbType.VarWChar,50,username),
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid),
				DbHelper.MakeInParam("@actions",(DbType)OleDbType.VarWChar,100,actions),
                DbHelper.MakeInParam("@medals", (DbType)OleDbType.Integer,4, medals),
				DbHelper.MakeInParam("@reason",(DbType)OleDbType.VarWChar,100,reason)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "medalslog] (adminid,adminname,ip,username,uid,actions,medals,reason) VALUES (@adminid,@adminname,@ip,@username,@uid,@actions,@medals,@reason)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, int medals, int uid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@newactions",(DbType)OleDbType.VarWChar,100,newactions),
                DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp,8,postdatetime),
				DbHelper.MakeInParam("@reason",(DbType)OleDbType.VarWChar,100,reason),
                DbHelper.MakeInParam("@oldactions",(DbType)OleDbType.VarWChar,100,oldactions),
                DbHelper.MakeInParam("@medals", (DbType)OleDbType.Integer,4, medals),
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals]=@medals  AND [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void UpdateMedalslog(string actions, DateTime postdatetime, string reason, int uid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@actions",(DbType)OleDbType.VarWChar,100,actions),
                DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp,8,postdatetime),
				DbHelper.MakeInParam("@reason",(DbType)OleDbType.VarWChar,100,reason),
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid)
			};
            string sql = "Update [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@actions ,[postdatetime]=@postdatetime,[reason]=@reason  WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, string medalidlist, int uid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@newactions",(DbType)OleDbType.VarWChar,100,newactions),
                DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp,8,postdatetime),
				DbHelper.MakeInParam("@reason",(DbType)OleDbType.VarWChar,100,reason),
                DbHelper.MakeInParam("@oldactions",(DbType)OleDbType.VarWChar,100,oldactions),
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid)
			};
            string sql = "Update [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals] NOT IN (" + medalidlist + ") AND [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void SetStopTalkUser(string uidlist)
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=4, [adminid]=0  WHERE [uid] IN (" + uidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void ChangeUserGroupByUid(int groupid, string uidlist)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,4,groupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid  WHERE [uid] IN (" + uidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public DataTable GetTableListInfo()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void DeletePostByPosterid(int tabid, int posterid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer,4, posterid)
			};
            string sql = "DELETE FROM  [" + BaseConfigs.GetTablePrefix + "posts" + tabid + "]   WHERE [posterid]=@posterid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void DeleteTopicByPosterid(int posterid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer,4, posterid)
			};
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid]=@posterid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void ClearPosts(int uid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts]=0 , [posts]=0  WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void UpdateEmailValidateInfo(string authstr, DateTime authtime, int uid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@authstr",(DbType)OleDbType.VarChar,20,authstr),
                DbHelper.MakeInParam("@authtime",(DbType)OleDbType.DBTimeStamp,8,authtime),
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [Authstr]=@authstr,[Authtime]=@authtime ,[Authflag]=1  WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public int GetRadminidByGroupid(int groupid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer,4, groupid)
			};
            string sql = "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
        }

        public string GetTemplateInfo()
        {
            string sql = "SELECT [templateid], [name] FROM [" + BaseConfigs.GetTablePrefix + "templates]";
            return sql;
        }

        public DataTable GetUserEmailByGroupid(string groupidlist)
        {
            string sql = "SELECT [username],[Email]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [Email] Is Not null AND [Email]<>'' AND [groupid] IN(" + groupidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserGroupExceptGroupid(int groupid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer, 4,groupid)
			};
            string sql = "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>@groupid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        /// <summary>
        /// �����ղ���Ϣ
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="tid">����ID</param>
        /// <returns>�����ɹ����� 1 ���򷵻� 0</returns>	
        public int CreateFavorites(int uid, int tid)
        {
            return CreateFavorites(uid, tid, 0);
        }

        /// <summary>
        /// �����ղ���Ϣ
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="tid">����ID</param>
        /// <param name="type">�ղ����ͣ�0=���⣬1=��ᣬ2=������־</param>
        /// <returns>�����ɹ����� 1 ���򷵻� 0</returns>	
        public int CreateFavorites(int uid, int tid, byte type)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
                                       DbHelper.MakeInParam("@type", (DbType)OleDbType.TinyInt, 4, type)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createfavorite", prams);
        }



        /// <summary>
        /// ɾ��ָ���û����ղ���Ϣ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="fitemid">Ҫɾ�����ղ���Ϣid�б�,��Ӣ�Ķ��ŷָ�</param>
        /// <returns>ɾ��������������ʱ���� -1</returns>
        public int DeleteFavorites(int uid, string fidlist, byte type)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid),
                                       DbHelper.MakeInParam("@typeid", (DbType)OleDbType.TinyInt, 1, type)
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid] IN (" + fidlist + ") AND [uid] = @uid AND [typeid]=@typeid", prams);
        }

        /// <summary>
        /// �õ��û��ղ���Ϣ�б�
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="pagesize">��ҳʱÿҳ�ļ�¼��</param>
        /// <param name="pageindex">��ǰҳ��</param>
        /// <returns>�û���Ϣ�б�</returns>
        public DataTable GetFavoritesList(int uid, int pagesize, int pageindex)
        {
            return GetFavoritesList(uid, pagesize, pageindex, 0);
        }

        /// <summary>
        /// �õ��û��ղ���Ϣ�б�
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="pagesize">��ҳʱÿҳ�ļ�¼��</param>
        /// <param name="pageindex">��ǰҳ��</param>
        /// <param name="typeid">�ղ�����id</param>
        /// <returns>�û���Ϣ�б�</returns>
        public DataTable GetFavoritesList(int uid, int pagesize, int pageindex, int typeid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
									   DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex)
								   
								   };

            switch (typeid)
            {
                case 1:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslistbyalbum", prams).Tables[0];

                case 2:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslistbyspacepost", prams).Tables[0];

                case 3:
                    {   //��ȡ�ղص���Ʒ��Ϣ
                        string strSQL = "SELECT [f].[tid], [f].[uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid], [seller], [dateline], [expiration]  FROM [{0}favorites] [f],[{0}goods] [goods] WHERE [f].[tid]=[goods].[goodsid] AND [f].[typeid]=3  AND [f].[uid]= " + uid;
                        
                        if(pageindex == 1)
                        {
                            strSQL = "SELECT TOP " + pagesize + "  [tid], [uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid] AS [posterid], [seller] AS [poster], [dateline] AS [postdatetime], [expiration]  FROM ( " + strSQL + ") f  ORDER BY [tid] DESC";
                        }
                        else
                        {
                            strSQL = "SELECT TOP " + pagesize + "  [tid], [uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid] AS [posterid], [seller] AS [poster], [dateline] AS [postdatetime], [expiration]  FROM ( " + @strSQL + ") f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP " + ((pageindex - 1) * pagesize) + " [tid] FROM (" + strSQL + ") f2  ORDER BY [tid] DESC) AS tblTmp) ORDER BY [tid] DESC";
                        }

                        return DbHelper.ExecuteDataset(CommandType.Text, string.Format(strSQL, BaseConfigs.GetTablePrefix)).Tables[0];
                    }

                //case FavoriteType.ForumTopic:
                default:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslist", prams).Tables[0];

            }
        }

        /// <summary>
        /// �õ��û��ղص�����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�ղ�����</returns>
        public int GetFavoritesCount(int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescount", prams).ToString(), 0);
        }

        public int GetFavoritesCount(int uid, int typeid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
                                       DbHelper.MakeInParam("@typeid",(DbType)OleDbType.TinyInt,1,typeid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescountbytype", prams).ToString(), 0);
        }

        public int CheckFavoritesIsIN(int uid, int tid)
        {
            return CreateFavorites(uid, tid, 0);
        }

        /// <summary>
        /// �ղؼ����Ƿ������ָ��������
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="tid">����id</param>
        /// <returns></returns>
        public int CheckFavoritesIsIN(int uid, int tid, byte type)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
                                        DbHelper.MakeInParam("@type", (DbType)OleDbType.TinyInt, 1, type)
			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([tid]) AS [tidcount] FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid]=@tid AND [uid]=@uid AND [typeid]=@type", prams), 0);
        }


        public void UpdateUserAllInfo(UserInfo userinfo)
        {
            string sqlstring = "Update [" + BaseConfigs.GetTablePrefix + "users] Set username=@username ,nickname=@nickname,secques=@secques,gender=@gender,adminid=@adminid,groupid=@groupid,groupexpiry=@groupexpiry,extgroupids=@extgroupids, regip=@regip," +
                "joindate=@joindate , lastip=@lastip, lastvisit=@lastvisit,  lastactivity=@lastactivity, lastpost=@lastpost, lastposttitle=@lastposttitle,posts=@posts, digestposts=@digestposts,oltime=@oltime,pageviews=@pageviews,credits=@credits," +
                "avatarshowid=@avatarshowid, email=@email,bday=@bday,sigstatus=@sigstatus,tpp=@tpp,ppp=@ppp,templateid=@templateid,pmsound=@pmsound," +
                "showemail=@showemail,newsletter=@newsletter,invisible=@invisible,newpm=@newpm,accessmasks=@accessmasks,extcredits1=@extcredits1,extcredits2=@extcredits2,extcredits3=@extcredits3,extcredits4=@extcredits4,extcredits5=@extcredits5,extcredits6=@extcredits6,extcredits7=@extcredits7,extcredits8=@extcredits8   Where uid=@uid";

            DbParameter[] prams = {
				DbHelper.MakeInParam("@username", (DbType)OleDbType.VarChar, 20, userinfo.Username),
				DbHelper.MakeInParam("@nickname", (DbType)OleDbType.VarChar, 10, userinfo.Nickname),
				DbHelper.MakeInParam("@secques", (DbType)OleDbType.VarChar, 8, userinfo.Secques),
				DbHelper.MakeInParam("@gender", (DbType)OleDbType.Integer, 4, userinfo.Gender),
				DbHelper.MakeInParam("@adminid", (DbType)OleDbType.Integer, 4, userinfo.Uid == 1 ? 1 : userinfo.Adminid),
				DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 2, userinfo.Groupid),
				DbHelper.MakeInParam("@groupexpiry", (DbType)OleDbType.Integer, 4, userinfo.Groupexpiry),
				DbHelper.MakeInParam("@extgroupids", (DbType)OleDbType.VarChar, 60, userinfo.Extgroupids),
				DbHelper.MakeInParam("@regip", (DbType)OleDbType.VarWChar, 15, userinfo.Regip),
				DbHelper.MakeInParam("@joindate", (DbType)OleDbType.DBTimeStamp, 4, userinfo.Joindate),
				DbHelper.MakeInParam("@lastip", (DbType)OleDbType.VarWChar, 15, userinfo.Lastip),
				DbHelper.MakeInParam("@lastvisit", (DbType)OleDbType.DBTimeStamp, 8, userinfo.Lastvisit),
				DbHelper.MakeInParam("@lastactivity", (DbType)OleDbType.DBTimeStamp, 8, userinfo.Lastactivity),
				DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.DBTimeStamp, 8, userinfo.Lastpost),
				DbHelper.MakeInParam("@lastposttitle", (DbType)OleDbType.VarWChar, 80, userinfo.Lastposttitle),
				DbHelper.MakeInParam("@posts", (DbType)OleDbType.Integer, 4, userinfo.Posts),
				DbHelper.MakeInParam("@digestposts", (DbType)OleDbType.Integer, 2, userinfo.Digestposts),
				DbHelper.MakeInParam("@oltime", (DbType)OleDbType.Integer, 4, userinfo.Oltime),
				DbHelper.MakeInParam("@pageviews", (DbType)OleDbType.Integer, 4, userinfo.Pageviews),
				DbHelper.MakeInParam("@credits", (DbType)OleDbType.Numeric, 10, userinfo.Credits),
				DbHelper.MakeInParam("@avatarshowid", (DbType)OleDbType.Integer, 4, userinfo.Avatarshowid),
				DbHelper.MakeInParam("@email", (DbType)OleDbType.VarWChar, 50, userinfo.Email.ToString()),
				DbHelper.MakeInParam("@bday", (DbType)OleDbType.VarWChar, 10, userinfo.Bday.ToString()),
				DbHelper.MakeInParam("@sigstatus", (DbType)OleDbType.Integer, 4, userinfo.Sigstatus.ToString()),
				DbHelper.MakeInParam("@tpp", (DbType)OleDbType.Integer, 4, userinfo.Tpp),
				DbHelper.MakeInParam("@ppp", (DbType)OleDbType.Integer, 4, userinfo.Ppp),
				DbHelper.MakeInParam("@templateid", (DbType)OleDbType.Integer, 4, userinfo.Templateid),
				DbHelper.MakeInParam("@pmsound", (DbType)OleDbType.Integer, 4, userinfo.Pmsound),
				DbHelper.MakeInParam("@showemail", (DbType)OleDbType.Integer, 4, userinfo.Showemail),
				DbHelper.MakeInParam("@newsletter", (DbType)OleDbType.Integer, 4, userinfo.Newsletter),
				DbHelper.MakeInParam("@invisible", (DbType)OleDbType.Integer, 4, userinfo.Invisible),
				DbHelper.MakeInParam("@newpm", (DbType)OleDbType.Integer, 4, userinfo.Newpm),
				DbHelper.MakeInParam("@accessmasks", (DbType)OleDbType.Integer, 4, userinfo.Accessmasks),
				DbHelper.MakeInParam("@extcredits1", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits1),
				DbHelper.MakeInParam("@extcredits2", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits2),
				DbHelper.MakeInParam("@extcredits3", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits3),
				DbHelper.MakeInParam("@extcredits4", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits4),
				DbHelper.MakeInParam("@extcredits5", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits5),
				DbHelper.MakeInParam("@extcredits6", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits6),
				DbHelper.MakeInParam("@extcredits7", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits7),
				DbHelper.MakeInParam("@extcredits8", (DbType)OleDbType.Numeric, 10, userinfo.Extcredits8),
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userinfo.Uid)
			};

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
        }

        public void DeleteModerator(int uid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [uid]=" + uid);
        }

        public void UpdateUserField(UserInfo userinfo, string signature, string authstr, string sightml)
        {
            string sqlstring = "Update [" + BaseConfigs.GetTablePrefix + "userfields] Set website=@website,icq=@icq,qq=@qq,yahoo=@yahoo,msn=@msn,skype=@skype,location=@location,customstatus=@customstatus, avatar=@avatar," +
                         "avatarwidth=@avatarwidth , avatarheight=@avatarheight, medals=@medals,  authstr=@authstr, authtime=@authtime, authflag=@authflag,bio=@bio, signature=@signature,sightml=@sightml,realname=@Realname,idcard=@Idcard,mobile=@Mobile,phone=@Phone Where uid=@uid";

            DbParameter[] prams1 = {
				DbHelper.MakeInParam("@website", (DbType)OleDbType.VarWChar, 80, userinfo.Website),
				DbHelper.MakeInParam("@icq", (DbType)OleDbType.VarChar, 12, userinfo.Icq),
				DbHelper.MakeInParam("@qq", (DbType)OleDbType.VarChar, 12, userinfo.Qq),
				DbHelper.MakeInParam("@yahoo", (DbType)OleDbType.VarChar, 40, userinfo.Yahoo),
				DbHelper.MakeInParam("@msn", (DbType)OleDbType.VarChar, 40, userinfo.Msn),
				DbHelper.MakeInParam("@skype", (DbType)OleDbType.VarChar, 40, userinfo.Skype),
				DbHelper.MakeInParam("@location", (DbType)OleDbType.VarWChar, 50, userinfo.Location),
				DbHelper.MakeInParam("@customstatus", (DbType)OleDbType.VarWChar, 50, userinfo.Customstatus),
				DbHelper.MakeInParam("@avatar", (DbType)OleDbType.VarWChar, 255, userinfo.Avatar),
				DbHelper.MakeInParam("@avatarwidth", (DbType)OleDbType.Integer, 4, userinfo.Avatarwidth),
				DbHelper.MakeInParam("@avatarheight", (DbType)OleDbType.Integer, 4, userinfo.Avatarheight),
				DbHelper.MakeInParam("@medals", (DbType)OleDbType.VarChar, 300, userinfo.Medals),
				DbHelper.MakeInParam("@authstr", (DbType)OleDbType.VarChar, 20, authstr),
				DbHelper.MakeInParam("@authtime", (DbType)OleDbType.DBTimeStamp, 4, userinfo.Authtime),
				DbHelper.MakeInParam("@authflag", (DbType)OleDbType.TinyInt, 1, 1),
				DbHelper.MakeInParam("@bio", (DbType)OleDbType.VarWChar, 500, userinfo.Bio.ToString()),
				DbHelper.MakeInParam("@signature", (DbType)OleDbType.VarWChar, 500, signature),
				DbHelper.MakeInParam("@sightml", (DbType)OleDbType.VarWChar, 1000, sightml),
                 DbHelper.MakeInParam("@Realname", (DbType)OleDbType.VarWChar, 1000, userinfo.Realname),
                DbHelper.MakeInParam("@Idcard", (DbType)OleDbType.VarWChar, 1000, userinfo.Idcard),
                DbHelper.MakeInParam("@Mobile", (DbType)OleDbType.VarWChar, 1000, userinfo.Mobile),
                DbHelper.MakeInParam("@Phone", (DbType)OleDbType.VarWChar, 1000, userinfo.Phone),
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userinfo.Uid)
			};

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
        }



        public void UpdatePMSender(int msgfromid, string msgfrom)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@msgfromid", (DbType)OleDbType.Integer, 4, msgfromid),
                                        DbHelper.MakeInParam("@msgfrom", (DbType)OleDbType.VarChar, 20, msgfrom)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgfrom]=@msgfrom WHERE [msgfromid]=@msgfromid", parms);
        }

        public void UpdatePMReceiver(int msgtoid, string msgto)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@msgtoid", (DbType)OleDbType.Integer, 4, msgtoid),
                                        DbHelper.MakeInParam("@msgto", (DbType)OleDbType.VarChar, 20, msgto)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgto]=@msgto  WHERE [msgtoid]=@msgtoid", parms);
        }



        public DataRowCollection GetModerators(string oldusername)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@oldusername", (DbType)OleDbType.VarChar, 20, RegEsc(oldusername))
			};

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[moderators] FROM  [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [moderators] LIKE '% @oldusername %'", prams).Tables[0].Rows;
        }

        public DataTable GetModeratorsTable(string oldusername)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@oldusername", (DbType)OleDbType.VarChar, 20, RegEsc(oldusername))
			};

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[moderators] FROM  [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [moderators] LIKE '% @oldusername %'", prams).Tables[0];
        }

        public void UpdateModerators(int fid, string moderators)
        {
            DbParameter[] parm = { 
                                        DbHelper.MakeInParam("@moderators", (DbType)OleDbType.VarChar, 20, moderators),
                                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]=@moderators  WHERE [fid]=@fid", parm);
        }

        public void UpdateUserCredits(int userid, float credits, float extcredits1, float extcredits2, float extcredits3, float extcredits4, float extcredits5, float extcredits6, float extcredits7, float extcredits8)
        {
            DbParameter[] prams1 = {
					DbHelper.MakeInParam("@targetuid",(DbType)OleDbType.Integer,4,userid.ToString()),
					DbHelper.MakeInParam("@Credits",(DbType)OleDbType.Numeric,9, credits),
					DbHelper.MakeInParam("@Extcredits1", (DbType)OleDbType.Numeric, 20,extcredits1),
					DbHelper.MakeInParam("@Extcredits2", (DbType)OleDbType.Numeric, 20,extcredits2),
					DbHelper.MakeInParam("@Extcredits3", (DbType)OleDbType.Numeric, 20,extcredits3),
					DbHelper.MakeInParam("@Extcredits4", (DbType)OleDbType.Numeric, 20,extcredits4),
					DbHelper.MakeInParam("@Extcredits5", (DbType)OleDbType.Numeric, 20,extcredits5),
					DbHelper.MakeInParam("@Extcredits6", (DbType)OleDbType.Numeric, 20,extcredits6),
					DbHelper.MakeInParam("@Extcredits7", (DbType)OleDbType.Numeric, 20,extcredits7),
					DbHelper.MakeInParam("@Extcredits8", (DbType)OleDbType.Numeric, 20,extcredits8)
										};

            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET credits=@Credits,extcredits1=@Extcredits1, extcredits2=@Extcredits2, extcredits3=@Extcredits3, extcredits4=@Extcredits4, extcredits5=@Extcredits5, extcredits6=@Extcredits6, extcredits7=@Extcredits7, extcredits8=@Extcredits8 WHERE [uid]=@targetuid";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
        }

        public void UpdateUserCredits(int userid, int extcreditsid, float score)
        {
            DbParameter[] prams1 = {
					DbHelper.MakeInParam("@targetuid",(DbType)OleDbType.Integer,4,userid.ToString()),
					DbHelper.MakeInParam("@Extcredits", (DbType)OleDbType.Numeric, 8, score)
             };

            string sqlstring = string.Format("UPDATE [{0}users] SET extcredits{1}=extcredits{1} + @Extcredits WHERE [uid]=@targetuid", BaseConfigs.GetTablePrefix, extcreditsid);

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
        }

        public void CombinationUser(string posttablename, UserInfo targetuserinfo, UserInfo srcuserinfo)
        {
            DbParameter[] prams = {
					DbHelper.MakeInParam("@target_uid", (DbType)OleDbType.Integer, 4, targetuserinfo.Uid),
					DbHelper.MakeInParam("@target_username", (DbType)OleDbType.VarWChar, 20, targetuserinfo.Username.Trim()),
					DbHelper.MakeInParam("@src_uid", (DbType)OleDbType.Integer, 4, srcuserinfo.Uid)
				};

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", prams);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts]=" + (srcuserinfo.Posts + targetuserinfo.Posts) + "WHERE [uid]=@target_uid", prams);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + posttablename + "] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", prams);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "pms] SET [msgtoid]=@target_uid,[msgto]=@target_username  WHERE [msgtoid]=@src_uid", prams);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "attachments] SET [uid]=@target_uid WHERE [uid]=@src_uid", prams);

        }

        /// <summary>
        /// ͨ���û����õ�UID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetuidByusername(string username)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@username", (DbType)OleDbType.VarWChar, 20, username)
			};

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username", prams).Tables[0];
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
        /// ɾ��ָ���û���������Ϣ
        /// </summary>
        /// <param name="uid">ָ�����û�uid</param>
        /// <param name="delposts">�Ƿ�ɾ������</param>
        /// <param name="delpms">�Ƿ�ɾ������Ϣ</param>
        /// <returns></returns>
        public bool DelUserAllInf(int uid, bool delposts, bool delpms)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "onlinetime] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [uid]=" + uid.ToString());

                    if (delposts)
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "topics] Where [posterid]=" + uid.ToString());

                        //����û�����������
                        foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
                        {
                            if (dr["id"].ToString() != "")
                            {
                                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM  [" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "] WHERE [posterid]=" + uid);
                            }
                        }
                    }
                    else
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [poster]='���û��ѱ�ɾ��'  Where [posterid]=" + uid.ToString());

                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposter]='���û��ѱ�ɾ��'  Where [lastpostid]=" + uid.ToString());

                        //����û�����������
                        foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
                        {
                            if (dr["id"].ToString() != "")
                            {
                                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "] SET  [poster]='���û��ѱ�ɾ��'  WHERE [posterid]=" + uid);
                            }
                        }
                    }

                    if (delpms)
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] Where [msgfromid]=" + uid.ToString());
                    }
                    else
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgfrom]='���û��ѱ�ɾ��'  Where [msgfromid]=" + uid.ToString());
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgto]='���û��ѱ�ɾ��'  Where [msgtoid]=" + uid.ToString());
                    }

                    //ɾ�������������û���Ϣ
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [uid]=" + uid.ToString());

                    //���µ�ǰ��̳������
                    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "Statistics] SET [totalusers]=[totalusers]-1");

                    DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid],[username] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //���µ�ǰ��̳����ע���Ա��Ϣ
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "Statistics] SET [lastuserid]=" + dt.Rows[0][0] + ", [lastusername]='" + dt.Rows[0][1] + "'");
                    }



                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            conn.Close();
            return true;
        }

        public DataTable GetUserGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid", parm).Tables[0];
        }

        public DataTable GetAdminGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@groupid", parm).Tables[0];
        }

        public void AddUserGroup(UserGroupInfo usergroupinfo, int Creditshigher, int Creditslower)
        {
            DbParameter[] prams = 
					{
						DbHelper.MakeInParam("@Radminid",(DbType)OleDbType.Integer,4,usergroupinfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",(DbType)OleDbType.VarWChar,50, Utils.RemoveFontTag(usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",(DbType)OleDbType.Integer,4,Creditshigher),
						DbHelper.MakeInParam("@Creditslower",(DbType)OleDbType.Integer,4,Creditslower),
						DbHelper.MakeInParam("@Stars",(DbType)OleDbType.Integer,4,usergroupinfo.Stars),
						DbHelper.MakeInParam("@Color",(DbType)OleDbType.Char,7,usergroupinfo.Color),
						DbHelper.MakeInParam("@Groupavatar",(DbType)OleDbType.VarWChar,60,usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",(DbType)OleDbType.Integer,4,usergroupinfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",(DbType)OleDbType.Integer,4,usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",(DbType)OleDbType.Integer,4,usergroupinfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",(DbType)OleDbType.Integer,4,usergroupinfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",(DbType)OleDbType.Integer,4,usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",(DbType)OleDbType.Integer,4,usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",(DbType)OleDbType.Integer,4,usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",(DbType)OleDbType.Integer,4,usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",(DbType)OleDbType.Integer,4,usergroupinfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",(DbType)OleDbType.Integer,4,usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",(DbType)OleDbType.Integer,4,usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",(DbType)OleDbType.Integer,4,usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",(DbType)OleDbType.Integer,4,usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",(DbType)OleDbType.Integer,4,usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",(DbType)OleDbType.Integer,4,usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",(DbType)OleDbType.Integer,4,usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("@Allowcusbbcode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",(DbType)OleDbType.Integer,4,usergroupinfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",(DbType)OleDbType.Integer,4,usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",(DbType)OleDbType.Integer,4,usergroupinfo.Allowviewstats),
                        DbHelper.MakeInParam("@Allowtrade",(DbType)OleDbType.Integer,4,usergroupinfo.Allowtrade),
                        DbHelper.MakeInParam("@Allowdiggs",(DbType)OleDbType.Integer,4,usergroupinfo.Allowdiggs),

                        DbHelper.MakeInParam("@Allowdebate",(DbType)OleDbType.Integer,4,usergroupinfo.Allowdebate),
                        DbHelper.MakeInParam("@Allowbonus",(DbType)OleDbType.Integer,4,usergroupinfo.Allowbonus),
                        DbHelper.MakeInParam("@Minbonusprice",(DbType)OleDbType.Integer,4,usergroupinfo.Minbonusprice),
                        DbHelper.MakeInParam("@Maxbonusprice",(DbType)OleDbType.Integer,4,usergroupinfo.Maxbonusprice),

						DbHelper.MakeInParam("@Disableperiodctrl",(DbType)OleDbType.Integer,4,usergroupinfo.Disableperiodctrl),
						DbHelper.MakeInParam("@Reasonpm",(DbType)OleDbType.Integer,4,usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",(DbType)OleDbType.Integer,2,usergroupinfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",(DbType)OleDbType.Integer,2,usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",(DbType)OleDbType.Integer,2,usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",(DbType)OleDbType.Integer,4,usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",(DbType)OleDbType.Integer,4,usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",(DbType)OleDbType.Char,100,usergroupinfo.Attachextensions),
                        DbHelper.MakeInParam("@Maxspaceattachsize",(DbType)OleDbType.Integer,4,usergroupinfo.Maxspaceattachsize),
                        DbHelper.MakeInParam("@Maxspacephotosize",(DbType)OleDbType.Integer,4,usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Raterange",(DbType)OleDbType.Char,100,usergroupinfo.Raterange)
					};

            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "usergroups]  ([radminid],[grouptitle],[creditshigher],[creditslower]," +
                "[stars] ,[color], [groupavatar],[readaccess], [allowvisit],[allowpost],[allowreply]," +
                "[allowpostpoll], [allowdirectpost],[allowgetattach],[allowpostattach],[allowvote],[allowmultigroups]," +
                "[allowsearch],[allowavatar],[allowcstatus],[allowuseblog],[allowinvisible],[allowtransfer]," +
                "[allowsetreadperm],[allowsetattachperm],[allowhidecode],[allowhtml],[allowcusbbcode],[allownickname]," +
                "[allowsigbbcode],[allowsigimgcode],[allowviewpro],[allowviewstats],[allowtrade],[allowdiggs],[disableperiodctrl],[reasonpm]," +
                "[maxprice],[maxpmnum],[maxsigsize],[maxattachsize],[maxsizeperday],[attachextensions],[raterange],[maxspaceattachsize]," +
                "[maxspacephotosize],[allowdebate],[allowbonus],[minbonusprice],[maxbonusprice]) VALUES(" +
                "@Radminid,@Grouptitle,@Creditshigher,@Creditslower,@Stars,@Color,@Groupavatar,@Readaccess,@Allowvisit,@Allowpost,@Allowreply," +
                "@Allowpostpoll,@Allowdirectpost,@Allowgetattach,@Allowpostattach,@Allowvote,@Allowmultigroups,@Allowsearch,@Allowavatar,@Allowcstatus," +
                "@Allowuseblog,@Allowinvisible,@Allowtransfer,@Allowsetreadperm,@Allowsetattachperm,@Allowhidecode,@Allowhtml,@Allowcusbbcode,@Allownickname," +
                "@Allowsigbbcode,@Allowsigimgcode,@Allowviewpro,@Allowviewstats,@Allowtrade,@Allowdiggs,@Disableperiodctrl,@Reasonpm,@Maxprice,@Maxpmnum,@Maxsigsize,@Maxattachsize," +
                "@Maxsizeperday,@Attachextensions,@Raterange,@Maxspaceattachsize,@Maxspacephotosize,@Allowdebate,@Allowbonus,@Minbonusprice,@Maxbonusprice)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
        }

        public void AddOnlineList(string grouptitle)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, GetMaxUserGroupId()),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 50, grouptitle)
                                    };
            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "onlinelist] ([groupid], [title], [img]) VALUES(@groupid,@title, '')";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public DataTable GetMinCreditHigher()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MIN(Creditshigher) FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 ").Tables[0];
        }

        public DataTable GetMaxCreditLower()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(Creditslower) FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 ").Tables[0];
        }

        public DataTable GetUserGroupByCreditshigher(int Creditshigher)
        {
            DbParameter parm = DbHelper.MakeInParam("@Creditshigher", (DbType)OleDbType.Integer, 4, Creditshigher);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0  AND [Creditshigher]<=@Creditshigher AND @Creditshigher<[Creditslower]", parm).Tables[0];
        }

        public void UpdateUserGroupCreditsHigher(int currentGroupID, int Creditslower)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, currentGroupID),
                                        DbHelper.MakeInParam("@creditshigher", (DbType)OleDbType.Integer, 4, Creditslower)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET creditshigher=@creditshigher WHERE [groupid]=@groupid", parms);
        }

        public void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int Creditshigher)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@creditslower", (DbType)OleDbType.Integer, 4, Creditshigher),
                                        DbHelper.MakeInParam("@creditshigher", (DbType)OleDbType.Integer, 4, currentCreditsHigher)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditslower]=@creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditshigher]=@creditshigher", parms);
        }

        public DataTable GetUserGroupByCreditsHigherAndLower(int Creditshigher, int Creditslower)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", (DbType)OleDbType.Integer, 4, Creditshigher),
                                        DbHelper.MakeInParam("@Creditslower", (DbType)OleDbType.Integer, 4, Creditslower)
                                    };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditshigher AND [Creditslower]=@Creditslower", parms).Tables[0];
        }
        public int GetGroupCountByCreditsLower(int Creditshigher)
        {
            DbParameter parm = DbHelper.MakeInParam("@creditslower", (DbType)OleDbType.Integer, 4, Creditshigher);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@creditslower", parm).Tables[0].Rows.Count;
        }

        public void UpdateUserGroupsCreditsLowerByCreditsLower(int Creditslower, int Creditshigher)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", (DbType)OleDbType.Integer, 4, Creditshigher),
                                        DbHelper.MakeInParam("@Creditslower", (DbType)OleDbType.Integer, 4, Creditslower)
                                    };
            DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditslower]=@Creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@Creditshigher", parms);
        }

        public void UpdateUserGroupTitleAndCreditsByGroupid(int groupid, string grouptitle, int creditslower, int creditshigher)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,4,groupid),
                DbHelper.MakeInParam("@grouptitle",(DbType)OleDbType.VarWChar,50,grouptitle),
                DbHelper.MakeInParam("@creditslower",(DbType)OleDbType.Integer,4,creditslower),
                DbHelper.MakeInParam("@creditshigher",(DbType)OleDbType.Integer,4,creditshigher)
            };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [grouptitle]=@grouptitle,[creditshigher]=@creditshigher,[creditslower]=@creditslower WHERE [groupid]=@groupid";
            DbHelper.ExecuteDataset(CommandType.Text, sql, parms);
        }

        public void UpdateUserGroupsCreditsHigherByCreditsHigher(int Creditshigher, int Creditslower)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", (DbType)OleDbType.Integer, 4, Creditshigher),
                                        DbHelper.MakeInParam("@Creditslower", (DbType)OleDbType.Integer, 4, Creditslower)
                                    };

            DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [Creditshigher]=@Creditshigher WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditslower", parms);
        }

        public DataTable GetUserGroupCreditsLowerAndHigher(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  WHERE [groupid]=@groupid", parm).Tables[0];
        }

        public void UpdateUserGroup(UserGroupInfo usergroupinfo, int Creditshigher, int Creditslower)
        {
            DbParameter[] prams = 
					{
						DbHelper.MakeInParam("@Radminid",(DbType)OleDbType.Integer,4,(usergroupinfo.Groupid == 1) ? 1 : usergroupinfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",(DbType)OleDbType.VarWChar,50, Utils.RemoveFontTag(usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",(DbType)OleDbType.Integer,4,Creditshigher),
						DbHelper.MakeInParam("@Creditslower",(DbType)OleDbType.Integer,4,Creditslower),
						DbHelper.MakeInParam("@Stars",(DbType)OleDbType.Integer,4,usergroupinfo.Stars),
						DbHelper.MakeInParam("@Color",(DbType)OleDbType.Char,7,usergroupinfo.Color),
						DbHelper.MakeInParam("@Groupavatar",(DbType)OleDbType.VarWChar,60,usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",(DbType)OleDbType.Integer,4,usergroupinfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",(DbType)OleDbType.Integer,4,usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",(DbType)OleDbType.Integer,4,usergroupinfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",(DbType)OleDbType.Integer,4,usergroupinfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",(DbType)OleDbType.Integer,4,usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",(DbType)OleDbType.Integer,4,usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",(DbType)OleDbType.Integer,4,usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",(DbType)OleDbType.Integer,4,usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",(DbType)OleDbType.Integer,4,usergroupinfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",(DbType)OleDbType.Integer,4,usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",(DbType)OleDbType.Integer,4,usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",(DbType)OleDbType.Integer,4,usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",(DbType)OleDbType.Integer,4,usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",(DbType)OleDbType.Integer,4,usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",(DbType)OleDbType.Integer,4,usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",(DbType)OleDbType.Integer,4,usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("@Allowcusbbcode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",(DbType)OleDbType.Integer,4,usergroupinfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",(DbType)OleDbType.Integer,4,usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",(DbType)OleDbType.Integer,4,usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",(DbType)OleDbType.Integer,4,usergroupinfo.Allowviewstats),
                        DbHelper.MakeInParam("@Allowtrade",(DbType)OleDbType.Integer,4,usergroupinfo.Allowtrade),
                        DbHelper.MakeInParam("@Allowdiggs",(DbType)OleDbType.Integer,4,usergroupinfo.Allowdiggs),
						DbHelper.MakeInParam("@Disableperiodctrl",(DbType)OleDbType.Integer,4,usergroupinfo.Disableperiodctrl),

                        DbHelper.MakeInParam("@Allowdebate",(DbType)OleDbType.Integer,4,usergroupinfo.Allowdebate),
                        DbHelper.MakeInParam("@Allowbonus",(DbType)OleDbType.Integer,4,usergroupinfo.Allowbonus),
                        DbHelper.MakeInParam("@Minbonusprice",(DbType)OleDbType.Integer,4,usergroupinfo.Minbonusprice),
                        DbHelper.MakeInParam("@Maxbonusprice",(DbType)OleDbType.Integer,4,usergroupinfo.Maxbonusprice),

						DbHelper.MakeInParam("@Reasonpm",(DbType)OleDbType.Integer,4,usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",(DbType)OleDbType.Integer,2,usergroupinfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",(DbType)OleDbType.Integer,2,usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",(DbType)OleDbType.Integer,2,usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",(DbType)OleDbType.Integer,4,usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",(DbType)OleDbType.Integer,4,usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",(DbType)OleDbType.Char,100,usergroupinfo.Attachextensions),
                        DbHelper.MakeInParam("@Maxspaceattachsize",(DbType)OleDbType.Integer,4,usergroupinfo.Maxspaceattachsize),
                        DbHelper.MakeInParam("@Maxspacephotosize",(DbType)OleDbType.Integer,4,usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Groupid",(DbType)OleDbType.Integer,4,usergroupinfo.Groupid)

					};

            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups]  SET [radminid]=@Radminid,[grouptitle]=@Grouptitle,[creditshigher]=@Creditshigher," +
                "[creditslower]=@Creditslower,[stars]=@Stars,[color]=@Color,[groupavatar]=@Groupavatar,[readaccess]=@Readaccess, [allowvisit]=@Allowvisit,[allowpost]=@Allowpost," +
                "[allowreply]=@Allowreply,[allowpostpoll]=@Allowpostpoll, [allowdirectpost]=@Allowdirectpost,[allowgetattach]=@Allowgetattach,[allowpostattach]=@Allowpostattach," +
                "[allowvote]=@Allowvote,[allowmultigroups]=@Allowmultigroups,[allowsearch]=@Allowsearch,[allowavatar]=@Allowavatar,[allowcstatus]=@Allowcstatus," +
                "[allowuseblog]=@Allowuseblog,[allowinvisible]=@Allowinvisible,[allowtransfer]=@Allowtransfer,[allowsetreadperm]=@Allowsetreadperm," +
                "[allowsetattachperm]=@Allowsetattachperm,[allowhidecode]=@Allowhidecode,[allowhtml]=@Allowhtml,[allowcusbbcode]=@Allowcusbbcode,[allownickname]=@Allownickname," +
                "[allowsigbbcode]=@Allowsigbbcode,[allowsigimgcode]=@Allowsigimgcode,[allowviewpro]=@Allowviewpro,[allowviewstats]=@Allowviewstats,[allowtrade]=@Allowtrade," +
                "[allowdiggs]=@Allowdiggs,[disableperiodctrl]=@Disableperiodctrl,[allowdebate]=@Allowdebate,[allowbonus]=@Allowbonus,[minbonusprice]=@Minbonusprice,[maxbonusprice]=@Maxbonusprice," +
                "[reasonpm]=@Reasonpm,[maxprice]=@Maxprice,[maxpmnum]=@Maxpmnum,[maxsigsize]=@Maxsigsize,[maxattachsize]=@Maxattachsize," +
                "[maxsizeperday]=@Maxsizeperday,[attachextensions]=@Attachextensions,[maxspaceattachsize]=@Maxspaceattachsize,[maxspacephotosize]=@Maxspacephotosize  WHERE [groupid]=@Groupid";


            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
        }


        public void UpdateOnlineList(UserGroupInfo usergroupinfo)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, usergroupinfo.Groupid),
                                        DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 50, Utils.RemoveFontTag(usergroupinfo.Grouptitle))
                                    };
            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "onlinelist] SET [title]=@title WHERE [groupid]=@groupid";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public bool IsSystemOrTemplateUserGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE ([system]=1 OR [type]=1) AND [groupid]=@groupid", parm).Tables[0].Rows.Count > 0;
        }

        public DataTable GetOthersCommonUserGroup(int exceptgroupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, exceptgroupid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>@groupid", parm).Tables[0];
        }

        public string GetUserGroupRAdminId(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE  [groupid]=@groupid", parm).Tables[0].Rows[0][0].ToString();
        }

        public void UpdateUserGroupLowerAndHigherToLimit(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditshigher]=-9999999 ,creditslower=9999999  WHERE [groupid]=@groupid", parm);
        }

        public void DeleteUserGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid", parm);
        }

        public void DeleteAdminGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@groupid", parm);
        }

        public void DeleteOnlineList(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [groupid]=@groupid", parm);
        }

        public int GetMaxUserGroupId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(groupid), 0) FROM " + BaseConfigs.GetTablePrefix + "usergroups"), 0);
        }



        public bool DeletePaymentLog()
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��ָ������ɾ����־
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public bool DeletePaymentLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public DataTable GetPaymentLogList(int pagesize, int currentpage)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE [id] < (SELECT min([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        public DataTable GetPaymentLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE " + condition + "  Order by [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid  WHERE [id] < (SELECT min([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp ) AND " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// �õ����ֽ�����־��¼��
        /// </summary>
        /// <returns></returns>
        public int GetPaymentLogListCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// �õ�ָ����ѯ�����µĻ��ֽ�����־��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public int GetPaymentLogListCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        public void DeleteModeratorByFid(int fid)
        {
            DbParameter[] prams = { DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid) };
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid]=@fid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }



        public DataTable GetUidModeratorByFid(string fidlist)
        {
            string sql = "SELECT distinct [uid] FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid] IN(" + fidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void AddModerator(int uid, int fid, int displayorder, int inherited)
        {
            DbParameter[] prams = 
            {
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
                DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 2, fid),
                DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 2, displayorder),
                DbHelper.MakeInParam("@inherited", (DbType)OleDbType.Integer, 2, inherited)
		    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "moderators] (uid,fid,displayorder,inherited) VALUES(@uid,@fid,@displayorder,@inherited)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public DataTable GetModeratorInfo(string moderator)
        {
            DbParameter[] prams = 
            {
				DbHelper.MakeInParam("@username", (DbType)OleDbType.VarChar, 20, moderator.Trim())
			};

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid],[groupid]  FROM [" + BaseConfigs.GetTablePrefix + "users] Where [groupid]<>7 AND [groupid]<>8 AND [username]=@username", prams).Tables[0];
        }

        public void SetModerator(string moderator)
        {
            DbParameter[] prams = 
            {
				DbHelper.MakeInParam("@username", (DbType)OleDbType.VarChar, 20, moderator.Trim())
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=3,[groupid]=3 WHERE [username]=@username", prams);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [adminid]=3,[groupid]=3 WHERE [username]=@username", prams);
        }



        public DataTable GetUidAdminIdByUsername(string username)
        {
            DbParameter[] prams =
			{
				DbHelper.MakeInParam("@username", (DbType)OleDbType.VarChar, 20, username)
			};
            string sql = "SELECT TOP 1 [uid],[adminid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username] = @username";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public DataTable GetUidInModeratorsByUid(int currentfid, int uid)
        {
            DbParameter[] prams =
			{
				DbHelper.MakeInParam("@currentfid", (DbType)OleDbType.Integer, 4, currentfid),
                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
			};
            string sql = "SELECT TOP 1 [uid]  FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid]<>@currentfid AND [uid]=@uid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void UpdateUserOnlineInfo(int groupid, int userid)
        {
            DbParameter[] prams =
			{
				DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid),
                DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [groupid]=@groupid WHERE [userid]=@userid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void UpdateUserOtherInfo(int groupid, int userid)
        {
            DbParameter[] prams =
			{
				DbHelper.MakeInParam("@groupid", (DbType)OleDbType.Integer, 4, groupid),
                DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid ,[adminid]=0 WHERE [uid]=@userid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        /// <summary>
        /// �õ���̳�����ע����û�ID���û���
        /// </summary>
        /// <param name="lastuserid">������������ע����û�ID</param>
        /// <param name="lastusername">������������ע����û���</param>
        /// <returns>���ڷ���true,�����ڷ���false</returns>
        public bool GetLastUserInfo(out string lastuserid, out string lastusername)
        {
            lastuserid = "";
            lastusername = "";

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid],[username] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC");
            if (reader.Read())
            {
                lastuserid = reader["uid"].ToString();
                lastusername = reader["username"].ToString().Trim();
                reader.Close();
                return true;
            }
            reader.Close();
            return false;

        }

        public IDataReader GetTopUsers(int statcount, int lastuid)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@lastuid", (DbType)OleDbType.Integer, 4, lastuid),
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] > @lastuid", prams);
        }

        public void ResetUserDigestPosts(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts]=(SELECT COUNT(tid) AS [digestposts] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[posterid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND [digest] > 0) WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid] = @uid", parm);
        }

        public IDataReader GetUsers(int start_uid, int end_uid)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@start_uid", (DbType)OleDbType.Integer, 4, start_uid),
				DbHelper.MakeInParam("@end_uid", (DbType)OleDbType.Integer, 4, end_uid)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] >= @start_uid AND [uid]<=@end_uid", prams);
        }

        public void UpdateUserPostCount(int postcount, int userid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@postcount", (DbType)OleDbType.Integer, 4, postcount),
                                        DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts]=@postcount WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid] = @userid", parms);
        }


        /// <summary>
        /// ������а����б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetModeratorList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}moderators]", BaseConfigs.GetTablePrefix)).Tables[0];
        }


        /// <summary>
        /// ���ȫ�������û���
        /// </summary>
        /// <returns></returns>
        public int GetOnlineAllUserCount()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(olid) FROM [" + BaseConfigs.GetTablePrefix + "online]"), 1);
        }

        /// <summary>
        /// �������߱�
        /// </summary>
        /// <returns></returns>
        public int CreateOnlineTable()
        {
            try
            {
                //StringBuilder sb = new StringBuilder();
                //sb.Append("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE id = object_id(N'[dnt_online]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) DROP TABLE [dnt_online];");
                //sb.Append("CREATE TABLE [dnt_online] ([olid] [int] IDENTITY (1, 1) NOT NULL,[userid] [int] NOT NULL,[ip] [varchar] (15) NOT NULL,[username] [nvarchar] (20) NOT NULL,[nickname] [nvarchar] (20) NOT NULL,[password] [char] (32) NOT NULL,[groupid] [smallint] NOT NULL,[olimg] [varchar] (80) NOT NULL,[adminid] [smallint] NOT NULL,[invisible] [smallint] NOT NULL,[action] [smallint] NOT NULL,[lastactivity] [smallint] NOT NULL,[lastposttime] [datetime] NOT NULL,[lastpostpmtime] [datetime] NOT NULL,[lastsearchtime] [datetime] NOT NULL,[lastupdatetime] [datetime] NOT NULL,[forumid] [int] NOT NULL,[forumname] [nvarchar] (50) NOT NULL,[titleid] [int] NOT NULL,[title] [nvarchar] (80) NOT NULL,[verifycode] [varchar] (10) NOT NULL ) ON [PRIMARY];");
                //sb.Append("ALTER TABLE [dnt_online] WITH NOCHECK ADD CONSTRAINT [PK_dnt_online] PRIMARY KEY CLUSTERED ([olid]) ON [PRIMARY]; ");
                //sb.Append("ALTER TABLE [dnt_online] ADD CONSTRAINT [DF_dnt_online_userid] DEFAULT ((-1)) FOR [userid],CONSTRAINT [DF_dnt_online_ip] DEFAULT ('0.0.0.0') FOR [ip],CONSTRAINT [DF_dnt_online_username] DEFAULT ('') FOR [username],CONSTRAINT [DF_dnt_online_nickname] DEFAULT ('') FOR [nickname],CONSTRAINT [DF_dnt_online_password] DEFAULT ('') FOR [password],CONSTRAINT [DF_dnt_online_groupid] DEFAULT (0) FOR [groupid],CONSTRAINT [DF_dnt_online_olimg] DEFAULT ('') FOR [olimg],CONSTRAINT [DF_dnt_online_adminid] DEFAULT (0) FOR [adminid],CONSTRAINT [DF_dnt_online_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_dnt_online_action] DEFAULT (0) FOR [action],CONSTRAINT [DF_dnt_online_lastactivity] DEFAULT (0) FOR [lastactivity],CONSTRAINT [DF_dnt_online_lastposttime] DEFAULT ('1900-1-1 00:00:00') FOR [lastposttime],CONSTRAINT [DF_dnt_online_lastpostpmtime] DEFAULT ('1900-1-1 00:00:00') FOR [lastpostpmtime],CONSTRAINT [DF_dnt_online_lastsearchtime] DEFAULT ('1900-1-1 00:00:00') FOR [lastsearchtime],CONSTRAINT [DF_dnt_online_lastupdatetime] DEFAULT (getdate()) FOR [lastupdatetime],CONSTRAINT [DF_dnt_online_forumid] DEFAULT (0) FOR [forumid],CONSTRAINT [DF_dnt_online_forumname] DEFAULT ('') FOR [forumname],CONSTRAINT [DF_dnt_online_titleid] DEFAULT (0) FOR [titleid],CONSTRAINT [DF_dnt_online_title] DEFAULT ('') FOR [title],CONSTRAINT [DF_dnt_online_verifycode] DEFAULT ('') FOR [verifycode];");
                //sb.Append("CREATE INDEX [forum] ON [dnt_online]([userid], [forumid], [invisible]) ON [PRIMARY];");
                //sb.Append("CREATE INDEX [invisible] ON [dnt_online]([userid], [invisible]) ON [PRIMARY];");
                //sb.Append("CREATE INDEX [forumid] ON [dnt_online]([forumid]) ON [PRIMARY];");
                //sb.Append("CREATE INDEX [password] ON [dnt_online]([userid], [password]) ON [PRIMARY];");
                //sb.Append("CREATE INDEX [ip] ON [dnt_online]([userid], [ip]) ON [PRIMARY];");
                //sb.AppendFormat("TRUNCATE TABLE [{0}online]", BaseConfigs.GetTablePrefix);

                return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("TRUNCATE TABLE [{0}online]", BaseConfigs.GetTablePrefix));
            }
            catch
            {
                return -1;
            }
        }

        ///// <summary>
        ///// ȡ�����߱����һ����¼��tickcount�ֶ�
        ///// </summary>
        ///// <returns></returns>
        //public DateTime GetLastUpdateTime()
        //{
        //    object val =
        //        DbHelper.ExecuteScalar(CommandType.Text,
        //                               "SELECT TOP 1 [lastupdatetime] FROM [" + BaseConfigs.GetTablePrefix +
        //                               "online] ORDER BY [olid] DESC");
        //    return val == null ? DateTime.Now : Convert.ToDateTime(val);
        //}

        /// <summary>
        /// �������ע���û�������
        /// </summary>
        /// <returns>�û�����</returns>
        public int GetOnlineUserCount()
        {

            return (int)DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(olid) FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]>0").Tables[0].Rows[0][0];

        }

        /// <summary>
        /// ��ð�������û��б�
        /// </summary>
        /// <param name="forumid">���Id</param>
        /// <returns></returns>
        public DataTable GetForumOnlineUserListTable(int forumid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [forumid]={1}", BaseConfigs.GetTablePrefix, forumid)).Tables[0];
        }

        /// <summary>
        /// ���ȫ�������û��б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineUserListTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "online]").Tables[0];
        }

        /// <summary>
        /// ��ð�������û��б�
        /// </summary>
        /// <param name="forumid">���Id</param>
        /// <returns></returns>
        public IDataReader GetForumOnlineUserList(int forumid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [forumid]={1}", BaseConfigs.GetTablePrefix, forumid.ToString()));
        }

        /// <summary>
        /// ���ȫ�������û��б�
        /// </summary>
        /// <returns></returns>
        public IDataReader GetOnlineUserList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "online]");
        }

        /// <summary>
        /// ���������û�ͼ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineGroupIconTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid], [displayorder], [title], [img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [img] <> '' ORDER BY [displayorder]").Tables[0];
        }

        /// <summary>
        /// ����uid���olid
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>olid</returns>
        public int GetOlidByUid(int uid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, uid) };
            return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, string.Format("SELECT olid FROM [{0}online] WHERE [userid]=@userid", BaseConfigs.GetTablePrefix), parms), -1);
        }

        /// <summary>
        /// ���ָ�������û�
        /// </summary>
        /// <param name="olid">����id</param>
        /// <returns>�����û�����ϸ��Ϣ</returns>
        public IDataReader GetOnlineUser(int olid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@olid", (DbType)OleDbType.Integer, 4, olid) };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// ���ָ���û�����ϸ��Ϣ
        /// </summary>
        /// <param name="userid">�����û�ID</param>
        /// <param name="password">�û�����</param>
        /// <returns>�û�����ϸ��Ϣ</returns>
        public DataTable GetOnlineUser(int userid, string password)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid),
                                        DbHelper.MakeInParam("@password", (DbType)OleDbType.Char, 32, password)
                                    };
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}online] WHERE [userid]=@userid AND [password]=@password", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// ���ָ���û�����ϸ��Ϣ
        /// </summary>
        /// <param name="userid">�����û�ID</param>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        public DataTable GetOnlineUserByIP(int userid, string ip)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid),
                                        DbHelper.MakeInParam("@ip", (DbType)OleDbType.VarChar, 15, ip)
                                    };
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}online] WHERE [userid]=@userid AND [ip]=@ip", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// ��������û���֤���Ƿ���Ч
        /// </summary>
        /// <param name="olid">�����û�ID</param>
        /// <param name="verifycode">��֤��</param>
        /// <returns>�����û�ID</returns>
        public bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@olid", (DbType)OleDbType.Integer, 4, olid),
                                        DbHelper.MakeInParam("@verifycode", (DbType)OleDbType.VarChar, 10, verifycode)
                                    };
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 [olid] FROM [{0}online] WHERE [olid]=@olid and [verifycode]=@verifycode", BaseConfigs.GetTablePrefix), parms).Tables[0];
            parms[1].Value = newverifycode;
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [verifycode]=@verifycode WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// �����û�����״̬
        /// </summary>
        /// <param name="uid">�û�Id</param>
        /// <param name="onlinestate">����״̬��������</param>
        /// <returns></returns>
        public int SetUserOnlineState(int uid, int onlinestate)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]={1},[lastactivity]=GETDATE(),[lastvisit]=GETDATE() WHERE [uid]={2}", BaseConfigs.GetTablePrefix, onlinestate, uid));
        }

        /// <summary>
        /// ɾ������������һ�������û���Ϣ
        /// </summary>
        /// <returns>ɾ������</returns>
        public int DeleteRowsByIP(string ip)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,ip)
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN (SELECT [userid] FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]>0 AND [ip]=@ip)", prams);
            if (ip != "0.0.0.0")
            {
                return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]=-1 AND [ip]=@ip", prams);
            }
            return 0;
        }

        /// <summary>
        /// ɾ�����߱���ָ������id����
        /// </summary>
        /// <param name="olid">����id</param>
        /// <returns></returns>
        public int DeleteRows(int olid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [olid]=" + olid.ToString());
        }

        /// <summary>
        /// �����û��ĵ�ǰ�����������Ϣ
        /// </summary>
        /// <param name="olid">�����б�id</param>
        /// <param name="action">����</param>
        /// <param name="inid">����λ�ô���</param>
        public void UpdateAction(int olid, int action, int inid)
        {
            DbParameter[] prams = {
										   //DbHelper.MakeInParam("@tickcount",(DbType)OleDbType.Integer,4,System.Environment.TickCount),
										   DbHelper.MakeInParam("@action",(DbType)OleDbType.Integer,2,action),
                                           DbHelper.MakeInParam("@lastupdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@forumid",(DbType)OleDbType.Integer,4,inid),
										   DbHelper.MakeInParam("@forumname",(DbType)OleDbType.VarWChar,100,""),
										   DbHelper.MakeInParam("@titleid",(DbType)OleDbType.Integer,4,inid),
										   DbHelper.MakeInParam("@title",(DbType)OleDbType.VarWChar,160,""),
										   DbHelper.MakeInParam("@olid",(DbType)OleDbType.Integer,4,olid)

									   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid", prams);
        }

        /// <summary>
        /// �����û��ĵ�ǰ�����������Ϣ
        /// </summary>
        /// <param name="olid">�����б�id</param>
        /// <param name="action">����id</param>
        /// <param name="fid">���id</param>
        /// <param name="forumname">�����</param>
        /// <param name="tid">����id</param>
        /// <param name="topictitle">������</param>
        public void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
        {
            DbParameter[] prams = {
										   //DbHelper.MakeInParam("@tickcount",(DbType)OleDbType.Integer,4,System.Environment.TickCount),
										   DbHelper.MakeInParam("@action",(DbType)OleDbType.Integer,2,action),
                                           DbHelper.MakeInParam("@lastupdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@forumid",(DbType)OleDbType.Integer,4,fid),
										   DbHelper.MakeInParam("@forumname",(DbType)OleDbType.VarWChar,100,forumname),
										   DbHelper.MakeInParam("@titleid",(DbType)OleDbType.Integer,4,tid),
										   DbHelper.MakeInParam("@title",(DbType)OleDbType.VarWChar,160,topictitle),
										   DbHelper.MakeInParam("@olid",(DbType)OleDbType.Integer,4,olid)

									   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid", prams);
        }

        /// <summary>
        /// �����û����ʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        public void UpdateLastTime(int olid)
        {
            DbParameter[] prams = {
										   //DbHelper.MakeInParam("@tickcount",(DbType)OleDbType.Integer,4,System.Environment.TickCount),
                                           DbHelper.MakeInParam("@lastupdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@olid",(DbType)OleDbType.Integer,4,olid)

									   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastupdatetime]=@lastupdatetime WHERE [olid]=@olid", prams);
        }

        /// <summary>
        /// �����û������ʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        public void UpdatePostTime(int olid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastposttime]='{1}' WHERE [olid]={2}", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olid.ToString()));
        }

        /// <summary>
        /// �����û���󷢶���Ϣʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        public void UpdatePostPMTime(int olid)
        {

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastpostpmtime]='{1}' WHERE [olid]={2}", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olid.ToString()));

        }

        /// <summary>
        /// �������߱���ָ���û��Ƿ�����
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="invisible">�Ƿ�����</param>
        public void UpdateInvisible(int olid, int invisible)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [invisible]={1} WHERE [olid]={2}", BaseConfigs.GetTablePrefix, invisible.ToString(), olid.ToString()));
        }

        /// <summary>
        /// �������߱���ָ���û����û�����
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="password">�û�����</param>
        public void UpdatePassword(int olid, string password)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32,password),
									   DbHelper.MakeInParam("@olid",(DbType)OleDbType.Integer,4,olid)

								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [password]=@password WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), prams);
        }

        /// <summary>
        /// �����û�IP��ַ
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="ip">ip��ַ</param>
        public void UpdateIP(int olid, string ip)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,ip),
									   DbHelper.MakeInParam("@olid",(DbType)OleDbType.Integer,4,olid)

								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [ip]=@ip WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), prams);

        }

        /// <summary>
        /// �����û��������ʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        public void UpdateSearchTime(int olid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastsearchtime]={1} WHERE [olid]={2}", BaseConfigs.GetTablePrefix, olid.ToString()));
        }

        /// <summary>
        /// �����û����û���
        /// </summary>
        /// <param name="userid">�û�ID</param>
        /// <param name="groupid">����</param>
        public void UpdateGroupid(int userid, int groupid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [groupid]={1} WHERE [userid]={2}", BaseConfigs.GetTablePrefix, groupid.ToString(), userid.ToString()));
        }


        /// <summary>
        /// ���ָ��ID�Ķ���Ϣ������
        /// </summary>
        /// <param name="pmid">����Ϣpmid</param>
        /// <returns>����Ϣ����</returns>
        public IDataReader GetPrivateMessageInfo(int pmid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@pmid", (DbType)OleDbType.Integer,4, pmid),
			                        };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid]=@pmid", prams);
        }

        /// <summary>
        /// ���ָ���û��Ķ���Ϣ�б�
        /// </summary>
        /// <param name="userid">�û�ID</param>
        /// <param name="folder">����Ϣ����(0:�ռ���,1:������,2:�ݸ���)</param>
        /// <param name="pagesize">ÿҳ��ʾ����Ϣ��</param>
        /// <param name="pageindex">��ǰҪ��ʾ��ҳ��</param>
        /// <param name="inttypewhere">ɸѡ����</param>
        /// <returns>����Ϣ�б�</returns>
        public IDataReader GetPrivateMessageList(int userid, int folder, int pagesize, int pageindex, int inttype)
        {
            string strwhere = "";
            if (inttype == 1)
            {
                strwhere = "[new]=1";
            }
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@userid",(DbType)OleDbType.Integer,4,userid),
									   DbHelper.MakeInParam("@folder",(DbType)OleDbType.Integer,4,folder),
									   DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
									   DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
									   DbHelper.MakeInParam("@strwhere",(DbType)OleDbType.VarChar,500,strwhere)
									   
								   };
            IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpmlist", prams);
            return reader;
        }

        /// <summary>
        /// �õ����û��Ķ���Ϣ����
        /// </summary>
        /// <param name="userid">�û�ID</param>
        /// <param name="folder">�����ļ���(0:�ռ���,1:������,2:�ݸ���)</param>
        /// <param name="state">����Ϣ״̬(0:�Ѷ�����Ϣ��1:δ������Ϣ��-1:ȫ������Ϣ)</param>
        /// <returns>����Ϣ����</returns>
        public int GetPrivateMessageCount(int userid, int folder, int state)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@userid",(DbType)OleDbType.Integer,4,userid),
									   DbHelper.MakeInParam("@folder",(DbType)OleDbType.Integer,4,folder),								   
									   DbHelper.MakeInParam("@state",(DbType)OleDbType.Integer,4,state)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpmcount", prams).ToString(), 0);
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="__privatemessageinfo">����Ϣ����</param>
        /// <param name="savetosentbox">���ö���Ϣ�Ƿ��ڷ����䱣��(0Ϊ������, 1Ϊ����)</param>
        /// <returns>����Ϣ�����ݿ��е�pmid</returns>
        public int CreatePrivateMessage(PrivateMessageInfo privatemessageinfo, int savetosentbox)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@pmid",(DbType)OleDbType.Integer,4,privatemessageinfo.Pmid),
									   DbHelper.MakeInParam("@msgfrom",(DbType)OleDbType.VarWChar,20,privatemessageinfo.Msgfrom),
									   DbHelper.MakeInParam("@msgfromid",(DbType)OleDbType.Integer,4,privatemessageinfo.Msgfromid),
									   DbHelper.MakeInParam("@msgto",(DbType)OleDbType.VarWChar,20,privatemessageinfo.Msgto),
									   DbHelper.MakeInParam("@msgtoid",(DbType)OleDbType.Integer,4,privatemessageinfo.Msgtoid),
									   DbHelper.MakeInParam("@folder",(DbType)OleDbType.Integer,2,privatemessageinfo.Folder),
									   DbHelper.MakeInParam("@new",(DbType)OleDbType.Integer,4,privatemessageinfo.New),
									   DbHelper.MakeInParam("@subject",(DbType)OleDbType.VarWChar,80,privatemessageinfo.Subject),
									   DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp,8,DateTime.Parse(privatemessageinfo.Postdatetime)),
									   DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar,0,privatemessageinfo.Message),
									   DbHelper.MakeInParam("@savetosentbox",(DbType)OleDbType.Integer,4,savetosentbox)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createpm", prams).ToString(), -1);
        }

        /// <summary>
        /// ɾ��ָ���û��Ķ���Ϣ
        /// </summary>
        /// <param name="userid">�û�ID</param>
        /// <param name="pmitemid">Ҫɾ���Ķ���Ϣ�б�(����)</param>
        /// <returns>ɾ����¼��</returns>
        public int DeletePrivateMessages(int userid, string pmidlist)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer,4, userid)
			};

            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid] IN (" + pmidlist + ") AND ([msgtoid] = @userid OR [msgfromid] = @userid)", prams);

        }

        /// <summary>
        /// ����¶���Ϣ��
        /// </summary>
        /// <returns></returns>
        public int GetNewPMCount(int userid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer,4, userid)
			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pmid]) AS [pmcount] FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [new] = 1 AND [folder] = 0 AND [msgtoid] = @userid", prams), 0);
        }

        /// <summary>
        /// ɾ��ָ���û���һ������Ϣ
        /// </summary>
        /// <param name="userid">�û��ɣ�</param>
        /// <param name="pmid">�Уͣɣ�</param>
        /// <returns></returns>
        public int DeletePrivateMessage(int userid, int pmid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer,4, userid),
									   DbHelper.MakeInParam("@pmid", (DbType)OleDbType.Integer,4, pmid)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid]=@pmid AND ([msgtoid] = @userid OR [msgfromid] = @userid)", prams);

        }

        /// <summary>
        /// ���ö���Ϣ״̬
        /// </summary>
        /// <param name="pmid">����ϢID</param>
        /// <param name="state">״ֵ̬</param>
        /// <returns>���¼�¼��</returns>
        public int SetPrivateMessageState(int pmid, byte state)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@pmid", (DbType)OleDbType.Integer,1,pmid),
									   DbHelper.MakeInParam("@state",(DbType)OleDbType.TinyInt,1,state)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [new]=@state WHERE [pmid]=@pmid", prams);

        }

        public int GetRAdminIdByGroup(int groupid)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid).Tables[0].Rows[0][0].ToString());
        }

        public string GetUserGroupsStr()
        {
            return "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]";
        }


        public DataTable GetUserNameListByGroupid(string groupidlist)
        {
            string sql = "SELECT [uid] ,[username]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid] IN(" + groupidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserNameByUid(int uid)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
			};
            string sql = "SELECT TOP 1 [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
        }

        public void ResetPasswordUid(string password, int uid)
        {
            DbParameter[] prams = 
			{
                DbHelper.MakeInParam("@password", (DbType)OleDbType.Char, 32, password),
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [password]=@password WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public void SendPMToUser(string msgfrom, int msgfromid, string msgto, int msgtoid, int folder, string subject, DateTime postdatetime, string message)
        {
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@msgfrom", (DbType)OleDbType.VarWChar,50, msgfrom),
				DbHelper.MakeInParam("@msgfromid", (DbType)OleDbType.Integer, 4, msgfromid),
				DbHelper.MakeInParam("@msgto", (DbType)OleDbType.VarWChar,50, msgto),
				DbHelper.MakeInParam("@msgtoid", (DbType)OleDbType.Integer, 4, msgtoid),
                DbHelper.MakeInParam("@folder", (DbType)OleDbType.Integer, 2, folder),
                DbHelper.MakeInParam("@subject", (DbType)OleDbType.VarWChar,60, subject),
                DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp,8, postdatetime),
				DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar, 0,message)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "pms] (msgfrom,msgfromid,msgto,msgtoid,folder,new,subject,postdatetime,message) " +
                "VALUES (@msgfrom,@msgfromid,@msgto,@msgtoid,@folder,1,@subject,@postdatetime,@message)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
            sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=[newpmcount]+1  WHERE [uid] =@msgtoid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public string GetSystemGroupInfoSql()
        {
            return "Select * From [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<=8 Order By [groupid]";
        }

        public void UpdateUserCredits(int uid, string credits)
        {
            DbParameter[] prams_credits = {
											   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
										   };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [credits] = {1} WHERE [uid]=@uid", BaseConfigs.GetTablePrefix, credits), prams_credits);
        }

        public void UpdateUserGroup(int uid, int groupid)
        {
            DbParameter[] prams_credits = {
											   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
										   };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [groupid] = {1} WHERE [uid]=@uid", BaseConfigs.GetTablePrefix, groupid), prams_credits);

        }

        public bool CheckUserCreditsIsEnough(int uid, float[] values)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)OleDbType.Numeric, 8, values[0]),
									   DbHelper.MakeInParam("@extcredits2", (DbType)OleDbType.Numeric, 8, values[1]),
									   DbHelper.MakeInParam("@extcredits3", (DbType)OleDbType.Numeric, 8, values[2]),
									   DbHelper.MakeInParam("@extcredits4", (DbType)OleDbType.Numeric, 8, values[3]),
									   DbHelper.MakeInParam("@extcredits5", (DbType)OleDbType.Numeric, 8, values[4]),
									   DbHelper.MakeInParam("@extcredits6", (DbType)OleDbType.Numeric, 8, values[5]),
									   DbHelper.MakeInParam("@extcredits7", (DbType)OleDbType.Numeric, 8, values[6]),
									   DbHelper.MakeInParam("@extcredits8", (DbType)OleDbType.Numeric, 8, values[7])
								   };
            string CommandText = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid AND"
                    + "	[extcredits1]>= (case when @extcredits1<0 then abs(@extcredits1) else [extcredits1] end) AND "
                    + "	[extcredits2]>= (case when @extcredits2<0 then abs(@extcredits2) else [extcredits2] end) AND "
                    + "	[extcredits3]>= (case when @extcredits3<0 then abs(@extcredits3) else [extcredits3] end) AND "
                    + "	[extcredits4]>= (case when @extcredits4<0 then abs(@extcredits4) else [extcredits4] end) AND "
                    + "	[extcredits5]>= (case when @extcredits5<0 then abs(@extcredits5) else [extcredits5] end) AND "
                    + "	[extcredits6]>= (case when @extcredits6<0 then abs(@extcredits6) else [extcredits6] end) AND "
                    + "	[extcredits7]>= (case when @extcredits7<0 then abs(@extcredits7) else [extcredits7] end) AND "
                    + "	[extcredits8]>= (case when @extcredits8<0 then abs(@extcredits8) else [extcredits8] end) ";

            if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, prams)) == 0)
            {
                return false;
            }
            return true;
        }

        public void UpdateUserCredits(int uid, float[] values)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)OleDbType.Numeric, 8, values[0]),
									   DbHelper.MakeInParam("@extcredits2", (DbType)OleDbType.Numeric, 8, values[1]),
									   DbHelper.MakeInParam("@extcredits3", (DbType)OleDbType.Numeric, 8, values[2]),
									   DbHelper.MakeInParam("@extcredits4", (DbType)OleDbType.Numeric, 8, values[3]),
									   DbHelper.MakeInParam("@extcredits5", (DbType)OleDbType.Numeric, 8, values[4]),
									   DbHelper.MakeInParam("@extcredits6", (DbType)OleDbType.Numeric, 8, values[5]),
									   DbHelper.MakeInParam("@extcredits7", (DbType)OleDbType.Numeric, 8, values[6]),
									   DbHelper.MakeInParam("@extcredits8", (DbType)OleDbType.Numeric, 8, values[7])
								   };

            string CommandText = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET "
                + "		[extcredits1]=[extcredits1] + @extcredits1, "
                + "		[extcredits2]=[extcredits2] + @extcredits2, "
                + "		[extcredits3]=[extcredits3] + @extcredits3, "
                + "		[extcredits4]=[extcredits4] + @extcredits4, "
                + "		[extcredits5]=[extcredits5] + @extcredits5, "
                + "		[extcredits6]=[extcredits6] + @extcredits6, "
                + "		[extcredits7]=[extcredits7] + @extcredits7, "
                + "		[extcredits8]=[extcredits8] + @extcredits8 "
                + "WHERE [uid]=@uid";

            DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
        }

        public bool CheckUserCreditsIsEnough(int uid, DataRow values, int pos, int mount)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits2", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits3", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits4", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits5", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits6", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits7", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits8", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								   };
            string CommandText = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid AND"
                    + "	[extcredits1]>= (case when @extcredits1 >= 0 then abs(@extcredits1) else 0 end) AND "
                    + "	[extcredits2]>= (case when @extcredits2 >= 0 then abs(@extcredits2) else 0 end) AND "
                    + "	[extcredits3]>= (case when @extcredits3 >= 0 then abs(@extcredits3) else 0 end) AND "
                    + "	[extcredits4]>= (case when @extcredits4 >= 0 then abs(@extcredits4) else 0 end) AND "
                    + "	[extcredits5]>= (case when @extcredits5 >= 0 then abs(@extcredits5) else 0 end) AND "
                    + "	[extcredits6]>= (case when @extcredits6 >= 0 then abs(@extcredits6) else 0 end) AND "
                    + "	[extcredits7]>= (case when @extcredits7 >= 0 then abs(@extcredits7) else 0 end) AND "
                    + "	[extcredits8]>= (case when @extcredits8 >= 0 then abs(@extcredits8) else 0 end) ";

            if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, prams)) == 0)
            {
                return false;
            }
            return true;
        }

        public void UpdateUserCredits(int uid, DataRow values, int pos, int mount)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits2", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits3", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits4", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits5", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits6", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits7", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits8", (DbType)OleDbType.Numeric, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								   };
            if (pos < 0 && mount < 0)
            {
                for (int i = 1; i < prams.Length; i++)
                {
                    prams[i].Value = -Convert.ToInt32(prams[i].Value);
                }
            }

            string CommandText = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET "
                + "	[extcredits1]=[extcredits1] + @extcredits1, "
                + "	[extcredits2]=[extcredits2] + @extcredits2, "
                + "	[extcredits3]=[extcredits3] + @extcredits3, "
                + "	[extcredits4]=[extcredits4] + @extcredits4, "
                + "	[extcredits5]=[extcredits5] + @extcredits5, "
                + "	[extcredits6]=[extcredits6] + @extcredits6, "
                + "	[extcredits7]=[extcredits7] + @extcredits7, "
                + "	[extcredits8]=[extcredits8] + @extcredits8 "
                + "WHERE [uid]=@uid";

            DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
        }


        public DataTable GetUserGroups()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]").Tables[0];
        }

        public DataTable GetUserGroupRateRange(int groupid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [raterange] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid.ToString()).Tables[0];
        }

        public IDataReader GetUserTodayRate(int uid)
        {
            DbParameter[] prams = {
						                DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
								    };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [extcredits], SUM(ABS([score])) AS [todayrate] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE DATEDIFF(d,[postdatetime],getdate()) = 0 AND [uid] = @uid GROUP BY [extcredits]", prams);
        }


        public string GetSpecialGroupInfoSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=-1 AND [groupid]>8 ORDER BY [groupid]";
        }


        ///// <summary>
        ///// ��������ʱ��
        ///// </summary>
        ///// <param name="uid">�û�id</param>
        ///// <returns></returns>
        //public int UpdateOnlineTime(int uid)
        //{
        //    return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [oltime] = [oltime] + DATEDIFF(n,[lastvisit],GETDATE()) WHERE [uid]={1}", BaseConfigs.GetTablePrefix, uid));
        //}

        /// <summary>
        /// ����ָ���û�����Ϣ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�û���Ϣ</returns>
        public IDataReader GetUserInfoToReader(int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid),
			};

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getuserinfo", prams);
        }

        /// <summary>
        /// ��ȡ����û���Ϣ
        /// </summary>
        /// <param name="uid">��id</param>
        /// <returns>�û������Ϣ</returns>
        public IDataReader GetShortUserInfoToReader(int uid)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer,4, uid),
			};

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getshortuserinfo", prams);

        }

        /// <summary>
        /// ����IP�����û�
        /// </summary>
        /// <param name="ip">ip��ַ</param>
        /// <returns>�û���Ϣ</returns>
        public IDataReader GetUserInfoByIP(string ip)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@regip", (DbType)OleDbType.Char,15, ip),
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [" + BaseConfigs.GetTablePrefix + "users].*, [" + BaseConfigs.GetTablePrefix + "userfields].* FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + BaseConfigs.GetTablePrefix + "userfields].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "users].[regip]=@regip ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC", prams);

        }

        public IDataReader GetUserName(int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
			};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid]=@uid", prams);

        }

        public IDataReader GetUserJoinDate(int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
			};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [joindate] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid]=@uid", prams);
        }

        public IDataReader GetUserID(string username)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.VarChar,20,username),
			};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[username]=@username", prams);
        }

        public DataTable GetUserList(int pagesize, int currentpage)
        {
            #region ����û��б�
            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset("SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ON [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users],[" + BaseConfigs.GetTablePrefix + "userfields],[" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND  [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] AND [" + BaseConfigs.GetTablePrefix + "users].[uid] < (SELECT min([uid])  FROM (SELECT TOP " + pagetop + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC) AS tblTmp )  ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC";
                return DbHelper.ExecuteDataset(sqlstring).Tables[0];
            }

            #endregion
        }

        /// <summary>
        /// ����û��б�DataTable
        /// </summary>
        /// <param name="pagesize">ÿҳ��¼��</param>
        /// <param name="pageindex">��ǰҳ��</param>
        /// <returns>�û��б�DataTable</returns>
        public DataTable GetUserList(int pagesize, int pageindex, string orderby, string ordertype)
        {
            string[] arrayorderby = new string[] { "username", "credits", "posts", "admin", "lastactivity", "joindate", "oltime" };
            int i = Array.IndexOf(arrayorderby, orderby);

            switch (i)
            {
                //case 0:
                //    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
                //    break;
                case 0:
                    orderby = string.Format("ORDER BY [{0}users].[username] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 1:
                    orderby = string.Format("ORDER BY [{0}users].[credits] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 2:
                    orderby = string.Format("ORDER BY [{0}users].[posts] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 3:
                    orderby = string.Format("WHERE [{0}users].[adminid] > 0 ORDER BY [{0}users].[adminid] {1}, [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                //case "joindate":
                //    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[joindate] " + ordertype + ",[" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
                //    break;
                case 4:
                    orderby = string.Format("ORDER BY [{0}users].[lastactivity] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 5:
                    orderby = string.Format("ORDER BY [{0}users].[joindate] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 6:
                    orderby = string.Format("ORDER BY [{0}users].[oltime] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                default:
                    orderby = string.Format("ORDER BY [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
            }

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
									   DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
									   DbHelper.MakeInParam("@orderby",(DbType)OleDbType.VarChar,1000,orderby)
								   };
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getuserlist", prams).Tables[0];
        }

        /// <summary>
        /// �ж�ָ���û����Ƿ��Ѵ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>����Ѵ��ڸ��û�id�򷵻�true, ���򷵻�false</returns>
        public bool Exists(int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
			};
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid", prams)) >= 1;
        }

        /// <summary>
        /// �ж�ָ���û����Ƿ��Ѵ���.
        /// </summary>
        /// <param name="username">�û���</param>
        /// <returns>����Ѵ��ڸ��û����򷵻�true, ���򷵻�false</returns>
        public bool Exists(string username)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.Char,20,username),
			};
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [username]=@username", BaseConfigs.GetTablePrefix), prams)) >= 1;
        }

        /// <summary>
        /// �Ƿ���ָ��ip��ַ���û�ע��
        /// </summary>
        /// <param name="ip">ip��ַ</param>
        /// <returns>���ڷ���true,���򷵻�false</returns>
        public bool ExistsByIP(string ip)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@regip",(DbType)OleDbType.Char, 15,ip),
			};
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [regip]=@regip", BaseConfigs.GetTablePrefix), prams)) >= 1;
        }

        /// <summary>
        /// ���Email�Ͱ�ȫ��
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="email">email</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public IDataReader CheckEmailAndSecques(string username, string email, string secques)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.Char,20,username),
									   DbHelper.MakeInParam("@email",(DbType)OleDbType.Char,50, email),
									   DbHelper.MakeInParam("@secques",(DbType)OleDbType.Char,8, secques)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkemailandsecques", prams);
        }

        /// <summary>
        /// �������Ͱ�ȫ��
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public IDataReader CheckPasswordAndSecques(string username, string password, bool originalpassword, string secques)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.Char,20,username),
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32, originalpassword ? Utils.MD5(password) : password),
									   DbHelper.MakeInParam("@secques",(DbType)OleDbType.Char,8, secques)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordandsecques", prams);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public IDataReader CheckPassword(string username, string password, bool originalpassword)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.Char,20, username),
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32, originalpassword ? Utils.MD5(password) : password)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordbyusername", prams);
        }

        /// <summary>
        /// ���DVBBS����ģʽ������
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public IDataReader CheckDvBbsPasswordAndSecques(string username, string password)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.Char,20,username),
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32, Utils.MD5(password).Substring(8, 16))
								   };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid], [password], [secques] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username", prams);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <param name="groupid">�û���id</param>
        /// <param name="adminid">����id</param>
        /// <returns>����û�������ȷ�򷵻�uid, ���򷵻�-1</returns>
        public IDataReader CheckPassword(int uid, string password, bool originalpassword)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32, originalpassword ? Utils.MD5(password) : password)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordbyuid", prams);
        }

        /// <summary>
        /// ����ָ����email�����û��������û�uid
        /// </summary>
        /// <param name="email">email��ַ</param>
        /// <returns>�û�uid</returns>
        public IDataReader FindUserEmail(string email)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@email",(DbType)OleDbType.Char,50, email),
								   };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [email]=@email", prams);
        }

        /// <summary>
        /// �õ���̳���û�����
        /// </summary>
        /// <returns>�û�����</returns>
        public int GetUserCount()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]"), 0);
        }

        /// <summary>
        /// �õ���̳���û�����
        /// </summary>
        /// <returns>�û�����</returns>
        public int GetUserCountByAdmin()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[adminid] > 0"), 0);
        }

        /// <summary>
        /// �������û�.
        /// </summary>
        /// <param name="__userinfo">�û���Ϣ</param>
        /// <returns>�����û�ID, ����Ѵ��ڸ��û����򷵻�-1</returns>
        public int CreateUser(UserInfo userinfo)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.Char,20,userinfo.Username),
									   DbHelper.MakeInParam("@nickname",(DbType)OleDbType.Char,20,userinfo.Nickname),
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32,userinfo.Password),
									   DbHelper.MakeInParam("@secques",(DbType)OleDbType.Char,8,userinfo.Secques),
									   DbHelper.MakeInParam("@gender",(DbType)OleDbType.Integer,4,userinfo.Gender),
									   DbHelper.MakeInParam("@adminid",(DbType)OleDbType.Integer,4,userinfo.Adminid),
									   DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,2,userinfo.Groupid),
									   DbHelper.MakeInParam("@groupexpiry",(DbType)OleDbType.Integer,4,userinfo.Groupexpiry),
									   DbHelper.MakeInParam("@extgroupids",(DbType)OleDbType.Char,60,userinfo.Extgroupids),
									   DbHelper.MakeInParam("@regip",(DbType)OleDbType.VarChar,0,userinfo.Regip),
									   DbHelper.MakeInParam("@joindate",(DbType)OleDbType.VarChar,0,userinfo.Joindate),
									   DbHelper.MakeInParam("@lastip",(DbType)OleDbType.Char,15,userinfo.Lastip),
									   DbHelper.MakeInParam("@lastvisit",(DbType)OleDbType.VarChar,0,userinfo.Lastvisit),
									   DbHelper.MakeInParam("@lastactivity",(DbType)OleDbType.VarChar,0,userinfo.Lastactivity),
									   DbHelper.MakeInParam("@lastpost",(DbType)OleDbType.VarChar,0,userinfo.Lastpost),
									   DbHelper.MakeInParam("@lastpostid",(DbType)OleDbType.Integer,4,userinfo.Lastpostid),
									   DbHelper.MakeInParam("@lastposttitle",(DbType)OleDbType.VarChar,0,userinfo.Lastposttitle),
									   DbHelper.MakeInParam("@posts",(DbType)OleDbType.Integer,4,userinfo.Posts),
									   DbHelper.MakeInParam("@digestposts",(DbType)OleDbType.Integer,2,userinfo.Digestposts),
									   DbHelper.MakeInParam("@oltime",(DbType)OleDbType.Integer,2,userinfo.Oltime),
									   DbHelper.MakeInParam("@pageviews",(DbType)OleDbType.Integer,4,userinfo.Pageviews),
									   DbHelper.MakeInParam("@credits",(DbType)OleDbType.Integer,4,userinfo.Credits),
									   DbHelper.MakeInParam("@extcredits1",(DbType)OleDbType.Numeric,8,userinfo.Extcredits1),
									   DbHelper.MakeInParam("@extcredits2",(DbType)OleDbType.Numeric,8,userinfo.Extcredits2),
									   DbHelper.MakeInParam("@extcredits3",(DbType)OleDbType.Numeric,8,userinfo.Extcredits3),
									   DbHelper.MakeInParam("@extcredits4",(DbType)OleDbType.Numeric,8,userinfo.Extcredits4),
									   DbHelper.MakeInParam("@extcredits5",(DbType)OleDbType.Numeric,8,userinfo.Extcredits5),
									   DbHelper.MakeInParam("@extcredits6",(DbType)OleDbType.Numeric,8,userinfo.Extcredits6),
									   DbHelper.MakeInParam("@extcredits7",(DbType)OleDbType.Numeric,8,userinfo.Extcredits7),
									   DbHelper.MakeInParam("@extcredits8",(DbType)OleDbType.Numeric,8,userinfo.Extcredits8),
									   DbHelper.MakeInParam("@avatarshowid",(DbType)OleDbType.Integer,4,userinfo.Avatarshowid),
									   DbHelper.MakeInParam("@email",(DbType)OleDbType.Char,50,userinfo.Email),
									   DbHelper.MakeInParam("@bday",(DbType)OleDbType.VarChar,0,userinfo.Bday),
									   DbHelper.MakeInParam("@sigstatus",(DbType)OleDbType.Integer,4,userinfo.Sigstatus),
									   DbHelper.MakeInParam("@tpp",(DbType)OleDbType.Integer,4,userinfo.Tpp),
									   DbHelper.MakeInParam("@ppp",(DbType)OleDbType.Integer,4,userinfo.Ppp),
									   DbHelper.MakeInParam("@templateid",(DbType)OleDbType.Integer,2,userinfo.Templateid),
									   DbHelper.MakeInParam("@pmsound",(DbType)OleDbType.Integer,4,userinfo.Pmsound),
									   DbHelper.MakeInParam("@showemail",(DbType)OleDbType.Integer,4,userinfo.Showemail),
									   DbHelper.MakeInParam("@newsletter",(DbType)OleDbType.Integer,4,userinfo.Newsletter),
									   DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,4,userinfo.Invisible),
									   //DbHelper.MakeInParam("@timeoffset",(DbType)OleDbType.Char,4,__userinfo.Timeoffset),
									   DbHelper.MakeInParam("@newpm",(DbType)OleDbType.Integer,4,userinfo.Newpm),
									   DbHelper.MakeInParam("@accessmasks",(DbType)OleDbType.Integer,4,userinfo.Accessmasks),
									   //
									   DbHelper.MakeInParam("@website",(DbType)OleDbType.VarChar,80,userinfo.Website),
									   DbHelper.MakeInParam("@icq",(DbType)OleDbType.VarChar,12,userinfo.Icq),
									   DbHelper.MakeInParam("@qq",(DbType)OleDbType.VarChar,12,userinfo.Qq),
									   DbHelper.MakeInParam("@yahoo",(DbType)OleDbType.VarChar,40,userinfo.Yahoo),
									   DbHelper.MakeInParam("@msn",(DbType)OleDbType.VarChar,40,userinfo.Msn),
									   DbHelper.MakeInParam("@skype",(DbType)OleDbType.VarChar,40,userinfo.Skype),
									   DbHelper.MakeInParam("@location",(DbType)OleDbType.VarChar,30,userinfo.Location),
									   DbHelper.MakeInParam("@customstatus",(DbType)OleDbType.VarChar,30,userinfo.Customstatus),
									   DbHelper.MakeInParam("@avatar",(DbType)OleDbType.VarChar,255,userinfo.Avatar),
									   DbHelper.MakeInParam("@avatarwidth",(DbType)OleDbType.Integer,4,userinfo.Avatarwidth),
									   DbHelper.MakeInParam("@avatarheight",(DbType)OleDbType.Integer,4,userinfo.Avatarheight),
									   DbHelper.MakeInParam("@medals",(DbType)OleDbType.VarChar,40, userinfo.Medals),
									   DbHelper.MakeInParam("@bio",(DbType)OleDbType.VarWChar,500,userinfo.Bio),
									   DbHelper.MakeInParam("@signature",(DbType)OleDbType.VarWChar,500,userinfo.Signature),
									   DbHelper.MakeInParam("@sightml",(DbType)OleDbType.VarWChar,1000,userinfo.Sightml),
									   DbHelper.MakeInParam("@authstr",(DbType)OleDbType.VarChar,20,userinfo.Authstr),
                                       DbHelper.MakeInParam("@realname",(DbType)OleDbType.VarWChar,10,userinfo.Realname),
                                       DbHelper.MakeInParam("@idcard",(DbType)OleDbType.VarChar,20,userinfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",(DbType)OleDbType.VarChar,20,userinfo.Mobile),
                                       DbHelper.MakeInParam("@phone",(DbType)OleDbType.VarChar,20,userinfo.Phone)
								   };

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createuser", prams), -1);
        }

        /// <summary>
        /// ����Ȩ����֤�ַ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="authstr">��֤��</param>
        /// <param name="authflag">��֤��־</param>
        public void UpdateAuthStr(int uid, string authstr, int authflag)
        {

            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@authstr", (DbType)OleDbType.Char, 20, authstr),
									   DbHelper.MakeInParam("@authflag", (DbType)OleDbType.Integer, 4, authflag) 
								   };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserauthstr", prams);
        }

        /// <summary>
        /// ����ָ���û��ĸ�������
        /// </summary>
        /// <param name="__userinfo">�û���Ϣ</param>
        /// <returns>����û���������Ϊfalse, ����Ϊtrue</returns>
        public void UpdateUserProfile(UserInfo userinfo)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userinfo.Uid), 
									   DbHelper.MakeInParam("@nickname",(DbType)OleDbType.Char,20,userinfo.Nickname),
									   DbHelper.MakeInParam("@gender", (DbType)OleDbType.Integer, 4, userinfo.Gender), 
									   DbHelper.MakeInParam("@email", (DbType)OleDbType.Char, 50, userinfo.Email), 
									   DbHelper.MakeInParam("@bday", (DbType)OleDbType.Char, 10, userinfo.Bday), 
									   DbHelper.MakeInParam("@showemail", (DbType)OleDbType.Integer, 4, userinfo.Showemail),
									   DbHelper.MakeInParam("@website", (DbType)OleDbType.VarChar, 80, userinfo.Website), 
									   DbHelper.MakeInParam("@icq", (DbType)OleDbType.VarChar, 12, userinfo.Icq), 
									   DbHelper.MakeInParam("@qq", (DbType)OleDbType.VarChar, 12, userinfo.Qq), 
									   DbHelper.MakeInParam("@yahoo", (DbType)OleDbType.VarChar, 40, userinfo.Yahoo), 
									   DbHelper.MakeInParam("@msn", (DbType)OleDbType.VarChar, 40, userinfo.Msn), 
									   DbHelper.MakeInParam("@skype", (DbType)OleDbType.VarChar, 40, userinfo.Skype), 
									   DbHelper.MakeInParam("@location", (DbType)OleDbType.VarWChar, 30, userinfo.Location), 
									   DbHelper.MakeInParam("@bio", (DbType)OleDbType.VarWChar, 500, userinfo.Bio),
                                       DbHelper.MakeInParam("@realname",(DbType)OleDbType.VarWChar,10,userinfo.Realname),
                                       DbHelper.MakeInParam("@idcard",(DbType)OleDbType.VarChar,20,userinfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",(DbType)OleDbType.VarChar,20,userinfo.Mobile),
                                       DbHelper.MakeInParam("@phone",(DbType)OleDbType.VarChar,20,userinfo.Phone)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserprofile", prams);
        }

        /// <summary>
        /// �����û���̳����
        /// </summary>
        /// <param name="__userinfo">�û���Ϣ</param>
        /// <returns>����û��������򷵻�false, ���򷵻�true</returns>
        public void UpdateUserForumSetting(UserInfo userinfo)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,userinfo.Uid),
									   DbHelper.MakeInParam("@tpp",(DbType)OleDbType.Integer,4,userinfo.Tpp),
									   DbHelper.MakeInParam("@ppp",(DbType)OleDbType.Integer,4,userinfo.Ppp),
									   DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,4,userinfo.Invisible),
									   DbHelper.MakeInParam("@customstatus",(DbType)OleDbType.VarChar,30,userinfo.Customstatus),
                                       DbHelper.MakeInParam("@sigstatus", (DbType)OleDbType.Integer, 4, userinfo.Sigstatus),
                                       DbHelper.MakeInParam("@signature", (DbType)OleDbType.VarWChar, 500, userinfo.Signature),
                                       DbHelper.MakeInParam("@sightml", (DbType)OleDbType.VarWChar, 1000, userinfo.Sightml)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserforumsetting", prams);
        }

        /// <summary>
        /// �޸��û��Զ�������ֶε�ֵ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="extid">��չ�ֶ����(1-8)</param>
        /// <param name="pos">���ӵ���ֵ(�����Ǹ���)</param>
        /// <returns>ִ���Ƿ�ɹ�</returns>
        public void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + extid.ToString() + "]=[extcredits" + extid.ToString() + "] + (" + pos.ToString() + ") WHERE [uid]=" + uid.ToString());
        }

        /// <summary>
        /// ���ָ���û���ָ��������չ�ֶε�ֵ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="extid">��չ�ֶ����(1-8)</param>
        /// <returns>ֵ</returns>
        public float GetUserExtCredits(int uid, int extid)
        {
            return Utils.StrToFloat(DbHelper.ExecuteDataset(CommandType.Text, "SELECT [extcredits" + extid.ToString() + "] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=" + uid.ToString()).Tables[0].Rows[0][0], 0);
        }

        ///// <summary>
        ///// �����û�ǩ��
        ///// </summary>
        ///// <param name="uid">�û�id</param>
        ///// <param name="signature">ǩ��</param>
        ///// <returns>����û��������򷵻�false, ���򷵻�true</returns>
        //public void UpdateUserSignature(int uid, int sigstatus, string signature, string sightml)
        //{
        //    DbParameter[] prams = {
        //                               DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
        //                               DbHelper.MakeInParam("@sigstatus",(DbType)OleDbType.Integer,4,sigstatus),
        //                               DbHelper.MakeInParam("@signature",(DbType)OleDbType.VarWChar,500,signature),
        //                               DbHelper.MakeInParam("@sightml",(DbType)OleDbType.VarWChar,1000,sightml)
        //                           };
        //    DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateusersignature", prams);
        //}

        /// <summary>
        /// �����û�ͷ��
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="avatar">ͷ��</param>
        /// <param name="avatarwidth">ͷ����</param>
        /// <param name="avatarheight">ͷ��߶�</param>
        /// <returns>����û��������򷵻�false, ���򷵻�true</returns>
        public void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid),
									   DbHelper.MakeInParam("@avatar",(DbType)OleDbType.VarChar,255,avatar),
									   DbHelper.MakeInParam("@avatarwidth",(DbType)OleDbType.Integer,4,avatarwidth),
									   DbHelper.MakeInParam("@avatarheight",(DbType)OleDbType.Integer,4,avatarheight),
                                       DbHelper.MakeInParam("@templateid", (DbType)OleDbType.Integer, 4, templateid)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpreference", prams);
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <returns>�ɹ�����true����false</returns>
        public void UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@password", (DbType)OleDbType.Char, 32, originalpassword ? Utils.MD5(password) : password)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpassword", prams);
        }

        /// <summary>
        /// �����û���ȫ����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�ɹ�����true����false</returns>
        public void UpdateUserSecques(int uid, string secques)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@secques", (DbType)OleDbType.Char, 8, secques)
								   };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [secques]=@secques WHERE [uid]=@uid", prams);
        }

        /// <summary>
        /// �����û�����¼ʱ��
        /// </summary>
        /// <param name="uid">�û�id</param>
        public void UpdateUserLastvisit(int uid, string ip)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
									   DbHelper.MakeInParam("@ip", (DbType)OleDbType.Char,15, ip)
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastvisit]=GETDATE(), [lastip]=@ip WHERE [uid] =@uid", prams);
        }

        public void UpdateUserOnlineStateAndLastActivity(string uidlist, int onlinestate, string activitytime)
        {
            DbParameter[] prams = {
                                        DbHelper.MakeInParam("@onlinestate", (DbType)OleDbType.Integer, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid] IN (" + uidlist + ")", prams);
        }

        public void UpdateUserOnlineStateAndLastActivity(int uid, int onlinestate, string activitytime)
        {
            DbParameter[] prams = {
                                        DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
                                        DbHelper.MakeInParam("@onlinestate", (DbType)OleDbType.Integer, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid]=@uid", prams);
        }

        public void UpdateUserOnlineStateAndLastVisit(string uidlist, int onlinestate, string activitytime)
        {
            DbParameter[] prams = {
                                        DbHelper.MakeInParam("@onlinestate", (DbType)OleDbType.Integer, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid] IN (" + uidlist + ")", prams);
        }

        public void UpdateUserOnlineStateAndLastVisit(int uid, int onlinestate, string activitytime)
        {
            DbParameter[] prams = {
                                        DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
                                        DbHelper.MakeInParam("@onlinestate", (DbType)OleDbType.Integer, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid]=@uid", prams);
        }

        /// <summary>
        /// �����û���ǰ������ʱ������ʱ��
        /// </summary>
        /// <param name="uid">�û�uid</param>
        public void UpdateUserLastActivity(int uid, string activitytime)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
									   DbHelper.MakeInParam("@activitytime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(activitytime))
								   };


            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastactivity] = @activitytime  WHERE [uid] = @uid", prams);

        }

        /// <summary>
        /// �����û���Ϣ����δ������Ϣ������
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="pmnum">����Ϣ����</param>
        /// <returns>���¼�¼����</returns>
        public int SetUserNewPMCount(int uid, int pmnum)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@value", (DbType)OleDbType.Integer, 4, pmnum)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=@value WHERE [uid]=@uid", prams);
        }

        /// <summary>
        /// ����ָ���û���ѫ����Ϣ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="medals">ѫ����Ϣ</param>
        public void UpdateMedals(int uid, string medals)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@medals", (DbType)OleDbType.VarChar, 300, medals)
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [medals]=@medals WHERE [uid]=@uid", prams);

        }

        public int DecreaseNewPMCount(int uid, int subval)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
									   DbHelper.MakeInParam("@subval", (DbType)OleDbType.Integer, 4, subval)
			};

            try
            {
                return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=CASE WHEN [newpmcount] >= 0 THEN [newpmcount]-@subval ELSE 0 END WHERE [uid]=@uid", prams);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// �õ��û��¶���Ϣ����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�¶���Ϣ��</returns>
        public int GetUserNewPMCount(int uid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid), 
			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT [newpmcount] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid", prams), 0);
        }

        /// <summary>
        /// �����û�������
        /// </summary>
        /// <param name="useridlist">uid�б�</param>
        /// <returns></returns>
        public int UpdateUserDigest(string useridlist)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts] = (");
            sql.Append("SELECT COUNT([tid]) AS [digest] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[posterid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND [digest]>0");
            sql.Append(") WHERE [uid] IN (");
            sql.Append(useridlist);
            sql.Append(")");

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
        }

        /// <summary>
        /// �����û�SpaceID
        /// </summary>
        /// <param name="spaceid">Ҫ���µ�SpaceId</param>
        /// <param name="userid">Ҫ���µ�UserId</param>
        /// <returns></returns>
        public void UpdateUserSpaceId(int spaceid, int userid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@spaceid",(DbType)OleDbType.Integer,4,spaceid),
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,userid)
								   };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=@spaceid WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
        }

        public DataTable GetUserIdByAuthStr(string authstr)
        {
            DbParameter[] prams = {
										  DbHelper.MakeInParam("@authstr",(DbType)OleDbType.VarChar,20,authstr)
				};

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE DateDiff(d,[authtime],getdate())<=3  AND [authstr]=@authstr", prams).Tables[0];

            return dt;
        }

        /// <summary>
        /// ִ�������û������������ӵĲ�����
        /// </summary>
        /// <param name="onlineuserinfo">�����û���Ϣ����</param>
        /// <returns>��ӳɹ��򷵻ظո���ӵ�olid,ʧ���򷵻�0</returns>
        public int AddOnlineUser(OnlineUserInfo onlineuserinfo, int timeout)
        {

            string strDelTimeOutSql = "";
            // ���timeoutΪ�����������Ҫ��ȷ�����û��Ƿ����ߵ�״̬
            if (timeout > 0)
            {
                if (onlineuserinfo.Userid > 0)
                {
                    strDelTimeOutSql = string.Format("{0}UPDATE [{1}users] SET [onlinestate]=1 WHERE [uid]={2};", strDelTimeOutSql, BaseConfigs.GetTablePrefix, onlineuserinfo.Userid.ToString());
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





            DbParameter[] prams = {
									   DbHelper.MakeInParam("@userid",(DbType)OleDbType.Integer,4,onlineuserinfo.Userid),
									   DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,onlineuserinfo.Ip),
									   DbHelper.MakeInParam("@username",(DbType)OleDbType.VarWChar,40,onlineuserinfo.Username),
									   //DbHelper.MakeInParam("@tickcount",(DbType)OleDbType.Integer,4,System.Environment.TickCount),
									   DbHelper.MakeInParam("@nickname",(DbType)OleDbType.VarWChar,40,onlineuserinfo.Nickname),
									   DbHelper.MakeInParam("@password",(DbType)OleDbType.Char,32,onlineuserinfo.Password),
									   DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,2,onlineuserinfo.Groupid),
									   DbHelper.MakeInParam("@olimg",(DbType)OleDbType.VarChar,80,onlineuserinfo.Olimg),
									   DbHelper.MakeInParam("@adminid",(DbType)OleDbType.Integer,2,onlineuserinfo.Adminid),
									   DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,2,onlineuserinfo.Invisible),
									   DbHelper.MakeInParam("@action",(DbType)OleDbType.Integer,2,onlineuserinfo.Action),
									   DbHelper.MakeInParam("@lastactivity",(DbType)OleDbType.Integer,2,onlineuserinfo.Lastactivity),
									   DbHelper.MakeInParam("@lastposttime",(DbType)OleDbType.DBTimeStamp,8,DateTime.Parse(onlineuserinfo.Lastposttime)),
									   DbHelper.MakeInParam("@lastpostpmtime",(DbType)OleDbType.DBTimeStamp,8,DateTime.Parse(onlineuserinfo.Lastpostpmtime)),
									   DbHelper.MakeInParam("@lastsearchtime",(DbType)OleDbType.DBTimeStamp,8,DateTime.Parse(onlineuserinfo.Lastsearchtime)),
									   DbHelper.MakeInParam("@lastupdatetime",(DbType)OleDbType.DBTimeStamp,8,DateTime.Parse(onlineuserinfo.Lastupdatetime)),
									   DbHelper.MakeInParam("@forumid",(DbType)OleDbType.Integer,4,onlineuserinfo.Forumid),
									   DbHelper.MakeInParam("@forumname",(DbType)OleDbType.VarWChar,50,""),
									   DbHelper.MakeInParam("@titleid",(DbType)OleDbType.Integer,4,onlineuserinfo.Titleid),
									   DbHelper.MakeInParam("@title",(DbType)OleDbType.VarWChar,80,""),
									   DbHelper.MakeInParam("@verifycode",(DbType)OleDbType.VarChar,10,onlineuserinfo.Verifycode)
								   };
            int olid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, strDelTimeOutSql + "INSERT INTO [" + BaseConfigs.GetTablePrefix + "online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title], [verifycode])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode);SELECT SCOPE_IDENTITY()", prams).ToString(), 0);

            //�������û����5����֮�ڲ���������û�
            if (_lastRemoveTimeout == 0 || (System.Environment.TickCount - _lastRemoveTimeout) < 300000)
            {
                DeleteExpiredOnlineUsers(timeout);
                _lastRemoveTimeout = System.Environment.TickCount;
            }
            // ���idֵ̫�����ؽ����߱�
            if (olid > 2147483000)
            {
                CreateOnlineTable();
                DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql + "INSERT INTO [" + BaseConfigs.GetTablePrefix + "online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[titleid],[verifycode])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode);SELECT SCOPE_IDENTITY()", prams);
                return 1;
            }


            return 0;
            //return (int)DbHelper.ExecuteDataset(CommandType.Text, "SELECT [olid] FROM ["+BaseConfigFactory.GetTablePrefix+"online] WHERE [userid]=" + __onlineuserinfo.Userid.ToString()).Tables[0].Rows[0][0];

        }

        private void DeleteExpiredOnlineUsers(int timeout)
        {
            System.Text.StringBuilder timeoutStrBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder memberStrBuilder = new System.Text.StringBuilder();

            string strDelTimeOutSql = "";
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [olid], [userid] FROM [{0}online] WHERE [lastupdatetime]<'{1}'", BaseConfigs.GetTablePrefix, DateTime.Parse(DateTime.Now.AddMinutes(timeout * -1).ToString("yyyy-MM-dd HH:mm:ss"))));
            while (dr.Read())
            {
                timeoutStrBuilder.Append(",");
                timeoutStrBuilder.Append(dr["olid"].ToString());
                if (dr["userid"].ToString() != "-1")
                {
                    memberStrBuilder.Append(",");
                    memberStrBuilder.Append(dr["userid"].ToString());
                }
            }
            dr.Close();

            if (timeoutStrBuilder.Length > 0)
            {
                timeoutStrBuilder.Remove(0, 1);
                strDelTimeOutSql = string.Format("DELETE FROM [{0}online] WHERE [olid] IN ({1});", BaseConfigs.GetTablePrefix, timeoutStrBuilder.ToString());
            }
            if (memberStrBuilder.Length > 0)
            {
                memberStrBuilder.Remove(0, 1);
                strDelTimeOutSql = string.Format("{0}UPDATE [{1}users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN ({2});", strDelTimeOutSql, BaseConfigs.GetTablePrefix, memberStrBuilder.ToString());
            }
            if (strDelTimeOutSql != string.Empty)
                DbHelper.ExecuteNonQuery(strDelTimeOutSql);
        }

        public DataTable GetUserInfo(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userid);
            string sql = "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
        }

        public DataTable GetUserInfo(string username, string password)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@username", (DbType)OleDbType.VarWChar, 20, username),
                                        DbHelper.MakeInParam("@password", (DbType)OleDbType.Char, 32, password)
                                    };
            string sql = "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username AND [password]=@password";

            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public void UpdateUserSpaceId(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid);
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=ABS([spaceid]) WHERE [uid]=@userid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        public int GetUserIdByRewriteName(string rewritename)
        {
            DbParameter parm = DbHelper.MakeInParam("@rewritename", (DbType)OleDbType.Char, 100, rewritename);
            string sql = string.Format("SELECT [userid] FROM [{0}spaceconfigs] WHERE [rewritename]=@rewritename", BaseConfigs.GetTablePrefix);
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), -1);
        }

        public void UpdateUserPMSetting(UserInfo user)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, user.Uid),
                                    DbHelper.MakeInParam("@pmsound", (DbType)OleDbType.Integer, 4, user.Pmsound),
                                    DbHelper.MakeInParam("@newsletter", (DbType)OleDbType.Integer, 4, (int)user.Newsletter)
                                };
            string sql = string.Format(@"UPDATE [{0}users] SET [pmsound]=@pmsound, [newsletter]=@newsletter WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void ClearUserSpace(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid);
            string sql = string.Format("UPDATE [{0}users] SET [spaceid]=0 WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }


        public IDataReader GetUserInfoByName(string username)
        {
            //DbParameter parm =DbHelper.MakeInParam("@username", (DbType)OleDbType.VarWChar, 20, username);
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [uid], [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username] LIKE '%" + RegEsc(username) + "%'");
        }


        public DataTable UserList(int pagesize, int currentpage, string condition)
        {
            #region ����û��б�

            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset("SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid]  LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ON [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] WHERE " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users],[" + BaseConfigs.GetTablePrefix + "userfields],[" + BaseConfigs.GetTablePrefix + "usergroups]  WHERE [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND  [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] AND [" + BaseConfigs.GetTablePrefix + "users].[uid] < (SELECT min([uid])  FROM (SELECT TOP " + pagetop + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + condition + " ORDER BY [uid] DESC) AS tblTmp ) AND " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC";
                return DbHelper.ExecuteDataset(sqlstring).Tables[0];
            }

            #endregion
        }
        public void LessenTotalUsers()
        {

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalusers]=[totalusers]-1");
        }

        public void UpdateOnlineTime(int oltimespan, int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
                                    DbHelper.MakeInParam("@oltimespan", (DbType)OleDbType.Integer, 2, oltimespan),
                                    DbHelper.MakeInParam("@lastupdate", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Now),
                                    DbHelper.MakeInParam("@expectedlastupdate", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Now.AddMinutes(0 - oltimespan))
                                };
            string sql = string.Format("UPDATE [{0}onlinetime] SET [thismonth]=[thismonth]+@oltimespan, [total]=[total]+@oltimespan, [lastupdate]=@lastupdate WHERE [uid]=@uid AND [lastupdate]<=@expectedlastupdate", BaseConfigs.GetTablePrefix);
            if (DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms) < 1)
            {
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("INSERT INTO [{0}onlinetime]([uid], [thismonth], [total], [lastupdate]) VALUES(@uid, @oltimespan, @oltimespan, @lastupdate)", BaseConfigs.GetTablePrefix), parms);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// ����ÿ������ʱ��(����)
        /// </summary>
        public void ResetThismonthOnlineTime()
        {
            DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}onlinetime] SET [thismonth]=0", BaseConfigs.GetTablePrefix));
        }

        public void SynchronizeOltime(int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid),
                                };
            string sql = string.Format("SELECT [total] FROM [{0}onlinetime] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            int total = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);
            
            if (DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [oltime]={1} WHERE [oltime]<{1} AND [uid]=@uid", BaseConfigs.GetTablePrefix, total), parms) < 1)
            {

                try
                {
                    sql = string.Format("UPDATE [{0}onlinetime] SET [total]=(SELECT [oltime] FROM [{0}users] WHERE [uid]=@uid) WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
                    DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
                }
                catch
                {
                }
            }
        }

        public IDataReader GetUserByOnlineTime(string field)
        {
            string commandText = string.Format("SELECT TOP 20 [o].[uid], [u].[username], [o].[{0}] FROM [{1}onlinetime] [o] LEFT JOIN [{1}users] [u] ON [o].[uid]=[u].[uid] ORDER BY [o].[{0}] DESC", field, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }
		#endregion

		#region WebSiteManage
        private DbParameter[] GetParms(string startdate, string enddate)
        {
            DbParameter[] parms = new DbParameter[2];
            if (startdate != "")
            {
                parms[0] = DbHelper.MakeInParam("@startdate", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(startdate));
            }
            if (enddate != "")
            {
                parms[1] = DbHelper.MakeInParam("@enddate", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(enddate).AddDays(1));
            }
            return parms;
        }

        public DataTable GetTopicListByCondition(string postname,int forumid, string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
        {
            string sql = "";
            string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);

            DbParameter[] parms = GetParms(startdate, enddate);

            int pageTop = (currentPage - 1) * pageSize + 1;
            int mintid = DatabaseProvider.GetInstance().GetMinPostTableTid(postname);
            int maxtid = DatabaseProvider.GetInstance().GetMaxPostTableTid(postname);
            //sql = " AND [tid]>=" + mintid + "AND [tid]<=" + maxtid;
            if (currentPage == 1)
            {
                sql =
                    string.Format(
                        "SELECT TOP {0} t.*,f.[name] FROM [{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]='' {2} AND [tid]>=" + mintid + " AND [tid]<=" + maxtid + " ORDER BY [tid] DESC",
                        pageSize, BaseConfigs.GetTablePrefix, condition);
            }
            else
            {
                sql =
                    string.Format(
                        "SELECT TOP {0} t.*,f.[name] FROM [{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]='' "
                + "AND [tid]<(SELECT MIN([tid]) FROM (SELECT TOP {2} [tid] FROM [{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] WHERE [closed]<>1 AND [status]=1 AND [password]='' {3} AND [tid]>=" + mintid + " AND [tid]<=" + maxtid + " ORDER BY [tid] DESC) AS tblTmp){3} AND [tid]>=" + mintid + " AND [tid]<=" + maxtid + " ORDER BY [tid] DESC",
                        pageSize, BaseConfigs.GetTablePrefix, pageTop, condition);
            }
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        private static string GetCondition(int forumid, string posterlist, string keylist, string startdate, string enddate)
        {
            string condition = "";
            if (forumid != 0)
            {
                condition += " AND t.[fid]=" + forumid;
            }
            if (posterlist != "")
            {
                string[] poster = posterlist.Split(',');
                condition += " AND [poster] in (";
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
                    tempkeylist += " [title] LIKE '%" + RegEsc(key) + "%' OR";
                }
                tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
                condition += " AND (" + tempkeylist + ")";
            }
            if (startdate != "")
            {
                //condition += " AND [postdatetime]>='" + startdate + " 00:00:00'";
                condition += " AND [postdatetime]>=@startdate";
            }
            if (enddate != "")
            {
                //condition += " AND [postdatetime]<='" + enddate + " 23:59:59'";
                condition += " AND [postdatetime]<=@enddate";
            }
            return condition;
        }

        public int GetTopicListCountByCondition(string postname,int forumid, string posterlist, string keylist, string startdate, string enddate)
        {
            string sql = string.Format("SELECT COUNT(1) FROM [{0}topics] t LEFT JOIN [{0}forums] f ON t.fid=f.fid LEFT JOIN [{0}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]=''", BaseConfigs.GetTablePrefix);
            string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);
            DbParameter[] parms = GetParms(startdate, enddate);

            if (condition != "")
                sql += condition;
            int mintid = DatabaseProvider.GetInstance().GetMinPostTableTid(postname);
            int maxtid = DatabaseProvider.GetInstance().GetMaxPostTableTid(postname);
            sql += " AND [tid]>=" + mintid + " AND [tid]<=" + maxtid;
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString());
        }

        public DataTable GetTopicListByTidlist(string posttableid, string tidlist)
        {
            string sql =
                string.Format(
                    "SELECT p.*,t.[closed] FROM [{0}posts{1}] p LEFT JOIN [{0}topics] t ON p.[tid]=t.[tid] WHERE [layer]=0 AND [closed]<>1 and p.[tid] IN ({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),p.[tid]),'{2}')",
                    BaseConfigs.GetTablePrefix, posttableid, tidlist);
            //string sql = string.Format("SELECT * FROM [{0}topics] WHERE [closed]<>1 AND [tid] IN ({1}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[tid]),'{1}')",
            //        BaseConfigs.GetTablePrefix,tidlist);
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }





        #region ǰ̨�ۺ�ҳ��غ���

        public DataTable GetWebSiteAggForumTopicList(string showtype, int topnumber)
        {
            DataTable topicList = new DataTable();
            switch (showtype)
            {
                default://������
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies] FROM [" + BaseConfigs.GetTablePrefix + "forums] f LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t  ON f.[lasttid] = t.[tid] WHERE f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') AND t.[displayorder]>=0").Tables[0]; break;
                    }
                case "1"://�����������
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name] FROM [" + BaseConfigs.GetTablePrefix + "topics] t LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid] = f.[fid] WHERE t.[displayorder]>=0 AND f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') ORDER BY t.[postdatetime] DESC").Tables[0]; break;
                    }
                case "2"://�����������
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name] FROM [" + BaseConfigs.GetTablePrefix + "topics] t LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid] = f.[fid] WHERE t.[digest] >0 AND f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') ORDER BY t.[digest] DESC").Tables[0]; break;
                    }
                case "3"://������
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies] FROM [" + BaseConfigs.GetTablePrefix + "forums] f LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t  ON f.[lasttid] = t.[tid] WHERE f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') AND t.[displayorder]>=0").Tables[0]; break;
                    }
            }
            return topicList;
        }


        public DataTable GetWebSiteAggHotForumList(int topnumber)
        {
            return DbHelper.ExecuteDataset("SELECT TOP " + topnumber + "[fid], [name], [topics] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [status]=1 AND [layer]> 0 AND [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [password]='') ORDER BY [topics] DESC, [posts] DESC, [todayposts] DESC").Tables[0];
        }
        #endregion

        //public DataTable GetWebSiteAggForumTopicList(string type, int forumid,int topnumber, string posttablename)
        //{
        //    string orderby = " [t].[tid] ";
        //    if (type.ToLower() == "newtopic")//����
        //    {
        //        orderby = " [t].[tid] ";
        //    }
        //    if (type.ToLower() == "hottopic")//����
        //    {
        //        orderby = " [t].[replies] ";
        //    }
        //    if (type.ToLower() == "views")//�����
        //    {
        //        orderby = " [t].[views] ";
        //    }
        //    if (type.ToLower() == "lastpost")//
        //    {
        //        orderby = " [t].[lastpost] ";
        //    }

        //    string condition = ""; 
        //    if (forumid > 0)
        //    {
        //        condition += " [t].[fid] = " + forumid + " OR (CHARINDEX('," + forumid + ",' , ',' + RTRIM([f].[parentidlist]) + ',') > 0)) ";
        //    }

        //    if (type.ToLower().IndexOf("digest")>=0)//������
        //    {
        //        condition += " [t].[digest] = " + type.Replace("digest", "[digest]").Replace(":"," = ") + " AND ";
        //    }
        //    condition += " [t].[displayorder]>=0 AND [t].[closed]<>1 AND [p].[layer]= 0 ";

        //    return DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " [t].[tid] AS [tid], [t].[title] AS [title], [t].[poster] AS [poster], [t].[posterid] AS [posterid], [t].[postdatetime] AS [postdatetime], [f].[fid], [f].[name], [p].[message] AS [message] FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t] LEFT JOIN [" + posttablename + "] AS [p] ON [t].[tid] = [p].[tid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forums] AS [f] ON [t].[fid] = [f].[fid] WHERE " + condition + " ORDER BY " + orderby + " DESC ").Tables[0];
        //}

        //public DataTable GetWebSiteAggForumHotTopicList()
        //{
        //    string condition = "";
        //    if (forumid >= 0)
        //    {
        //        condition += " [t].[fid] = " + forumid + " AND ";
        //    }
        //    condition += " [displayorder]>=0 AND [closed]<>1 ";
        //    return DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " [t].[tid], [t].[title] FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t] LEFT JOIN [" + posttablename + "] WHERE "+condition+" ORDER BY [t].[replies] DESC").Tables[0];
        //}


        
        ///// <summary>
        ///// ����û��б�DataTable
        ///// </summary>
        ///// <param name="pagesize">ÿҳ��¼��</param>
        ///// <param name="pageindex">��ǰҳ��</param>
        ///// <returns>�û��б�DataTable</returns>
        //public DataTable GetWebSiteAggUserList(string type, int topnumber)
        //{
        //    switch (i)
        //    {
        //        //case 0:
        //        //    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
        //        //    break;
        //        case 0:
        //            orderby = string.Format("ORDER BY [{0}users].[username] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //        case 1:
        //            orderby = string.Format("ORDER BY [{0}users].[credits] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //        case 2:
        //            orderby = string.Format("ORDER BY [{0}users].[posts] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //        case 3:
        //            orderby = string.Format("WHERE [{0}users].[adminid] > 0 ORDER BY [{0}users].[adminid] {1}, [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //        //case "joindate":
        //        //    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[joindate] " + ordertype + ",[" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
        //        //    break;
        //        case 4:
        //            orderby = string.Format("ORDER BY [{0}users].[lastactivity] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //        case 5:
        //            orderby = string.Format("ORDER BY [{0}users].[joindate] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //        default:
        //            orderby = string.Format("ORDER BY [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
        //            break;
        //    }

        //    DbParameter[] prams = {
        //                               DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
        //                               DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
        //                               DbHelper.MakeInParam("@orderby",(DbType)OleDbType.VarChar,1000,orderby)
        //                           };
        //    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getuserlist", prams).Tables[0];
        //}
		#endregion

	}
}
#endif