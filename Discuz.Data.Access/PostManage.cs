﻿#if NET1
#else
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using System.Data.OleDb;
using Discuz.Forum;

namespace Discuz.Data.Access
{
    public partial class DataProvider : IDataProvider
    {
        public string SearchTopicAudit(int fid, string poster, string title, string moderatorname, DateTime postdatetimeStart, DateTime postdatetimeEnd, DateTime deldatetimeStart, DateTime deldatetimeEnd)
        {
            string sqlstring = null;
            sqlstring += " [displayorder]<0";

            if (fid != 0)
            {
                sqlstring += " AND [fid]=" + fid;
            }

            if (poster != "")
            {
                sqlstring += " AND (";
                foreach (string postername in poster.Split(','))
                {
                    if (postername.Trim() != "")
                    {
                        sqlstring += " [poster]='" + postername + "'  OR";
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
                        sqlstring += " [title] LIKE '%" + RegEsc(titlename) + "%' OR";
                    }
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }


            if (moderatorname != "")
            {
                string logtidlist = "";
                //DataTable dt = DbHelper.ExecuteDataset("SELECT [title]	FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE (moderatorname = '" + moderatorname + "') AND (actions = 'DELETE')").Tables[0];
                DataTable dt = DatabaseProvider.GetInstance().GetTitleForModeratormanagelogByModeratorname(moderatorname);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        logtidlist += dr["tid"].ToString() + ",";
                    }
                    sqlstring = sqlstring + " AND tid IN (" + logtidlist.Substring(0, logtidlist.Length - 1) + ") ";
                }
            }

            if (postdatetimeStart.ToString().IndexOf("1900") < 0)
            {
                sqlstring += " AND [postdatetime]>=#" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "#";
            }

            if (postdatetimeEnd.ToString().IndexOf("1900") < 0)
            {
                sqlstring += " AND [postdatetime]<=#" + postdatetimeEnd.ToString("yyyy-MM-dd HH:mm:ss") + "#";
            }

            if ((deldatetimeStart.ToString().IndexOf("1900") < 0) && (deldatetimeStart.ToString().IndexOf("1900") < 0))
            {
                string logtidlist2 = "";
                //DataTable dt = DbHelper.ExecuteDataset("SELECT [title]	FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE (postdatetime >= '" + deldatetimeStart.SelectedDate.ToString() + "') AND (postdatetime<='" + deldatetimeEnd.SelectedDate.ToString() + "')AND (actions = 'DELETE')").Tables[0];
                DataTable dt = DatabaseProvider.GetInstance().GetTitleForModeratormanagelogByPostdatetime(deldatetimeStart, deldatetimeStart);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        logtidlist2 += dr["title"].ToString() + ",";
                    }
                    sqlstring = sqlstring + " AND tid IN (" + logtidlist2.Substring(0, logtidlist2.Length - 1) + ") ";
                }
            }
            return sqlstring;
        }

        public void AddBBCCode(int available, string tag, string icon, string replacement, string example,
    string explanation, string param, string nest, string paramsdescript, string paramsdefvalue)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available),
				DbHelper.MakeInParam("@tag", (DbType)OleDbType.VarChar, 100, tag),
				DbHelper.MakeInParam("@icon", (DbType)OleDbType.VarChar,50, icon),
				DbHelper.MakeInParam("@replacement", (DbType)OleDbType.VarWChar,0, replacement),
				DbHelper.MakeInParam("@example", (DbType)OleDbType.VarWChar, 255, example),
				DbHelper.MakeInParam("@explanation", (DbType)OleDbType.VarWChar, 0, explanation),
				DbHelper.MakeInParam("@params", (DbType)OleDbType.Integer, 4, param),
				DbHelper.MakeInParam("@nest", (DbType)OleDbType.Integer, 4, nest),
				DbHelper.MakeInParam("@paramsdescript", (DbType)OleDbType.VarWChar, 0, paramsdescript),
				DbHelper.MakeInParam("@paramsdefvalue", (DbType)OleDbType.VarWChar, 0, paramsdefvalue)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "bbcodes] ([available],[tag],[icon],[replacement],[example]," +
                "[explanation],[params],[nest],[paramsdescript],[paramsdefvalue]) VALUES(@available,@tag,@icon,@replacement,@example,@explanation,@params," +
                "@nest,@paramsdescript,@paramsdefvalue)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类实体</param>
        /// <returns>附件id</returns>
        public int CreateAttachment(AttachmentInfo attachmentinfo)
        {
            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,attachmentinfo.Uid),
            //                            DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,attachmentinfo.Tid),
            //                            DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,attachmentinfo.Pid),
            //                            DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(attachmentinfo.Postdatetime)),
            //                            DbHelper.MakeInParam("@readperm",(DbType)OleDbType.Integer,4,attachmentinfo.Readperm),
            //                            DbHelper.MakeInParam("@filename",(DbType)OleDbType.VarChar,100,attachmentinfo.Filename),
            //                            DbHelper.MakeInParam("@description",(DbType)OleDbType.VarChar,100,attachmentinfo.Description),
            //                            DbHelper.MakeInParam("@filetype",(DbType)OleDbType.VarChar,50,attachmentinfo.Filetype),
            //                            DbHelper.MakeInParam("@filesize",(DbType)OleDbType.Integer,4,attachmentinfo.Filesize),
            //                            DbHelper.MakeInParam("@attachment",(DbType)OleDbType.VarChar,100,attachmentinfo.Attachment),
            //                            DbHelper.MakeInParam("@downloads",(DbType)OleDbType.Integer,4,attachmentinfo.Downloads),
            //                            DbHelper.MakeInParam("@extname",(DbType)OleDbType.VarWChar,50,Utils.GetFileExtName(attachmentinfo.Attachment)),
            //                            DbHelper.MakeInParam("@attachprice",(DbType)OleDbType.Integer,4,attachmentinfo.Attachprice)
            //                       };
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createattachment", parms), -1);


            DbParameter[] prams = {
                                        DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,attachmentinfo.Uid),
                                        DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,attachmentinfo.Tid),
                                        DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,attachmentinfo.Pid),
                                        DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(attachmentinfo.Postdatetime)),
                                        DbHelper.MakeInParam("@readperm",(DbType)OleDbType.Integer,4,attachmentinfo.Readperm),
                                        DbHelper.MakeInParam("@filename",(DbType)OleDbType.VarChar,100,attachmentinfo.Filename),
                                        DbHelper.MakeInParam("@description",(DbType)OleDbType.VarChar,100,attachmentinfo.Description),
                                        DbHelper.MakeInParam("@filetype",(DbType)OleDbType.VarChar,50,attachmentinfo.Filetype),
                                        DbHelper.MakeInParam("@filesize",(DbType)OleDbType.Integer,4,attachmentinfo.Filesize),
                                        DbHelper.MakeInParam("@attachment",(DbType)OleDbType.VarChar,100,attachmentinfo.Attachment),
                                        DbHelper.MakeInParam("@downloads",(DbType)OleDbType.Integer,4,attachmentinfo.Downloads)
							       };

            DbParameter[] prams1 ={
            DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,attachmentinfo.Pid)
            };

            DbParameter[] prams2 = {
                                        DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,attachmentinfo.Uid),
                                        DbHelper.MakeInParam("@attachment",(DbType)OleDbType.VarChar,100,attachmentinfo.Attachment),
                                        DbHelper.MakeInParam("@description",(DbType)OleDbType.VarChar,100,attachmentinfo.Description),
                                        DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(attachmentinfo.Postdatetime)),
                                        DbHelper.MakeInParam("@downloads",(DbType)OleDbType.Integer,4,attachmentinfo.Downloads),
                                        DbHelper.MakeInParam("@filename",(DbType)OleDbType.VarChar,100,attachmentinfo.Filename),
                                        DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,attachmentinfo.Pid),
                                        DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,attachmentinfo.Tid),
                                        DbHelper.MakeInParam("@extname",(DbType)OleDbType.VarWChar,50,Utils.GetFileExtName(attachmentinfo.Attachment))
                                       
							       };
            int id = 0;
            int aid = 0;

            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "attachments]([uid],[tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize],  [attachment], [downloads]) VALUES(@uid, @tid, @pid, @postdatetime, @readperm, @filename, @description, @filetype, @filesize,  @attachment, @downloads)";

            DbHelper.ExecuteNonQuery(out id, CommandType.Text, sql, prams);
            aid = id;

            String upsql = "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + Posts.GetPostTableID(attachmentinfo.Tid) + "] SET [attachment]=1 WHERE [pid]=@pid";
            DbHelper.ExecuteNonQuery(CommandType.Text, upsql, prams1);

            string sql2 = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "myattachments]([aid],[uid],[attachment],[description],[postdatetime],[downloads],[filename],[pid],[tid],[extname]) VALUES(" + aid + ",@uid,@attachment,@description,@postdatetime,@downloads,@filename,@pid,@tid,@extname)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql2, prams2);
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
            DbParameter[] parm = {
										DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								    };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [attachment]={1} WHERE [tid]=@tid", BaseConfigs.GetTablePrefix, attType), parm);
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
            DbParameter[] parm = {
										DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid)
								    };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [attachment]={2} WHERE [pid]=@pid", BaseConfigs.GetTablePrefix, postTableId, attType), parm);
        }

        /// <summary>
        /// 获取指定附件信息
        /// </summary>
        /// <param name="aid">附件Id</param>
        /// <returns></returns>
        public IDataReader GetAttachmentInfo(int aid)
        {
            DbParameter[] parm = {
									   DbHelper.MakeInParam("@aid", (DbType)OleDbType.Integer,4, aid),
			};

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}attachments] WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 获得指定帖子的附件个数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>附件个数</returns>
        public int GetAttachmentCountByPid(int pid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) AS [acount] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", parms), 0);
        }

        /// <summary>
        /// 获得指定主题的附件个数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>附件个数</returns>
        public int GetAttachmentCountByTid(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) AS [acount] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=@tid", parms), 0);
        }

        /// <summary>
        /// 获得指定帖子的附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>帖子信息</returns>
        public DataTable GetAttachmentListByPid(int pid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid)
								   };
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", parms);
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
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [id], [extension], [maxsize] FROM [{0}attachtypes]", BaseConfigs.GetTablePrefix));
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
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@aid",(DbType)OleDbType.Integer,4,aid)
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}attachments] SET [downloads]=[downloads]+1 WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新主题是否包含附件
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="hasAttachment">是否包含附件,0不包含,1包含</param>
        /// <returns></returns>
        public int UpdateTopicAttachment(int tid, int hasAttachment)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [attachment]={1} WHERE [tid]=@tid", BaseConfigs.GetTablePrefix, hasAttachment), parms);
        }

        /// <summary>
        /// 获得指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public IDataReader GetAttachmentListByTid(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename] FROM [{0}attachments] WHERE [tid]=@tid ", BaseConfigs.GetTablePrefix), parms);

        }

        /// <summary>
        /// 获得指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">主题Id列表，以英文逗号分割</param>
        /// <returns></returns>
        public IDataReader GetAttachmentListByTid(string tidlist)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename] FROM [{0}attachments] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">版块tid</param>
        /// <returns>删除个数</returns>
        public int DeleteAttachmentByTid(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE * FROM [{0}attachments] WHERE [tid]=@tid ", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题Id列表，以英文逗号分割</param>
        /// <returns>删除个数</returns>
        public int DeleteAttachmentByTid(string tidlist)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE * FROM [{0}attachments] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
        }

        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public int DeleteAttachment(int aid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@aid",(DbType)OleDbType.Integer,4,aid)
								   };

            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE * FROM [{0}attachments] WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id列表，以英文逗号分割</param>
        /// <returns></returns>
        public int DeleteAttachment(string aidList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE * FROM [{0}attachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidList));
        }

        public int UpdatePostAttachment(int pid, string postTableId, int hasAttachment)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [attachment]={2} WHERE [pid]=@pid", BaseConfigs.GetTablePrefix, postTableId, hasAttachment), parms);
        }

        /// <summary>
        /// 根据帖子Id删除附件
        /// </summary>
        /// <param name="pid">帖子Id</param>
        /// <returns></returns>
        public int DeleteAttachmentByPid(int pid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid)
                                    };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE * FROM [{0}attachments] WHERE [pid]=@pid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        public int UpdateAttachment(AttachmentInfo attachmentInfo)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(attachmentInfo.Postdatetime)),
										DbHelper.MakeInParam("@readperm", (DbType)OleDbType.Integer, 4, attachmentInfo.Readperm),
										DbHelper.MakeInParam("@filename", (DbType)OleDbType.VarWChar, 100, attachmentInfo.Filename),
										DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 100, attachmentInfo.Description),
										DbHelper.MakeInParam("@filetype", (DbType)OleDbType.VarWChar, 50, attachmentInfo.Filetype),
										DbHelper.MakeInParam("@filesize", (DbType)OleDbType.Integer, 4, attachmentInfo.Filesize),
										DbHelper.MakeInParam("@attachment", (DbType)OleDbType.VarWChar, 100, attachmentInfo.Attachment),
										DbHelper.MakeInParam("@downloads", (DbType)OleDbType.Integer, 4, attachmentInfo.Downloads),
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, attachmentInfo.Tid),
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, attachmentInfo.Pid),
                                        DbHelper.MakeInParam("@attachprice",(DbType)OleDbType.Integer,4,attachmentInfo.Attachprice),
                                        DbHelper.MakeInParam("@aid",(DbType)OleDbType.Integer, 4,attachmentInfo.Aid)
								   };

            string sql = string.Format(@"UPDATE [{0}attachments] SET [postdatetime] = @postdatetime, [readperm] = @readperm, [filename] = @filename, [description] = @description, [filetype] = @filetype, [filesize] = @filesize, [attachment] = @attachment, [downloads] = @downloads, [tid]=@tid, [pid]=@pid, [attachprice]=@attachprice  
											WHERE [aid]=@aid", BaseConfigs.GetTablePrefix);

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
            DbParameter[] parms = {
										DbHelper.MakeInParam("@readperm", (DbType)OleDbType.Integer, 4, readperm),
										DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 100, description),
                                        DbHelper.MakeInParam("@aid", (DbType)OleDbType.Integer, 4, aid)
								   };

            string sql = string.Format(@"UPDATE [{0}attachments] SET [readperm] = @readperm, [description] = @description WHERE [aid] = @aid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="aid">附件Id</param>
        /// <param name="readperm">阅读权限</param>
        /// <param name="description">描述</param>
        /// <returns>返回被更新的数量</returns>
        public int UpdateAttachment(int aid, int readperm, string description, int attachprice)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@readperm", (DbType)OleDbType.Integer, 4, readperm),
										DbHelper.MakeInParam("@description", (DbType)OleDbType.VarWChar, 100, description),
                                        DbHelper.MakeInParam("@attachprice", (DbType)OleDbType.Integer, 4, attachprice),
                                        DbHelper.MakeInParam("@aid", (DbType)OleDbType.Integer, 4, aid)
								   };

            string sql = string.Format(@"UPDATE [{0}attachments] SET [readperm] = @readperm, [description] = @description, [attachprice] = @attachprice WHERE [aid] = @aid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        public IDataReader GetAttachmentList(string aidList)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename],[tid],[pid] FROM [{0}attachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidList));
        }

        public IDataReader GetAttachmentListByPid(string pidList)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}attachments] WHERE [pid] IN ({1})", BaseConfigs.GetTablePrefix, pidList));
        }

        /// <summary>
        /// 获得上传附件文件的大小
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetUploadFileSizeByUserId(int uid)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid)
            //                       };
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT SUM([filesize]) AS [todaysize] FROM [{0}attachments] WHERE [uid]=@uid AND DATEDIFF(d,[postdatetime],GETDATE())=0", BaseConfigs.GetTablePrefix), parms), 0);
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT SUM([filesize]) as [todaysize] FROM [{0}attachments] WHERE [uid]=@uid AND DATEDIFF(\"d\",[postdatetime],NOW())=0", BaseConfigs.GetTablePrefix), prams), 0);
        }

        /// <summary>
        /// 取得主题贴的第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        public IDataReader GetFirstImageAttachByTid(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
								   };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=@tid AND LEFT([filetype], 5)='image' ORDER BY [aid]", parms);
        }

        public DataSet GetAttchType()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] ORDER BY [id] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql);
        }

        public string GetAttchTypeSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] ORDER BY [id] ASC";
        }

        public void AddAttchType(string extension, string maxsize)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@extension", (DbType)OleDbType.VarChar,256, extension),
				DbHelper.MakeInParam("@maxsize", (DbType)OleDbType.Integer, 4, maxsize)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "attachtypes] ([extension], [maxsize]) VALUES (@extension,@maxsize)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }



        public void UpdateAttchType(string extension, string maxsize, int id)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@extension", (DbType)OleDbType.VarChar,256, extension),
				DbHelper.MakeInParam("@maxsize", (DbType)OleDbType.Integer, 4, maxsize),
				DbHelper.MakeInParam("@id",(DbType)OleDbType.Integer, 4,id)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "attachtypes] SET [extension]=@extension ,[maxsize]=@maxsize WHERE [id]=@id";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteAttchType(string attchtypeidlist)
        {
            string sql = "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] WHERE [id] IN (" + attchtypeidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public bool IsExistExtensionInAttachtypes(string extensionname)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@extension", (DbType)OleDbType.VarChar,256, extensionname)
            };
            string sql = "SELECT TOP 1  * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] WHERE [extension]=@extension";
            if (DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        public DataTable GetTitleForModeratormanagelogByModeratorname(string moderatorname)
        {
            string sql = "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE ([moderatorname] = '" + moderatorname + "') AND ([actions] = 'DELETE')";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetTitleForModeratormanagelogByPostdatetime(DateTime startDateTime, DateTime endDateTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@startDateTime", (DbType)OleDbType.DBTimeStamp, 8, startDateTime),
                                        DbHelper.MakeInParam("@endDateTime", (DbType)OleDbType.DBTimeStamp, 8, endDateTime)
            };
            string sql = "SELECT [title] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE (postdatetime >= @startDateTime) AND ([postdatetime]<= @endDateTime) AND ([actions] = 'DELETE')";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public DataTable GetTidForModeratormanagelogByPostdatetime(DateTime postDateTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, postDateTime)
            };
            string sql = "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=-1 AND [postdatetime]<=@postdatetime";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }


        public string GetUnauditNewTopicSQL()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=-2 OR [displayorder]=-3 ORDER BY [tid] DESC";
        }

        public IDataReader GetUnauditNewTopic(string fidlist, int tpp, int pageid, int filter)
        {
            //DbHelper.ExecuteReader(CommandType.Text,"SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=-2 and fid= "+fid+" ORDER BY [tid] DESC");

            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@filter", (DbType)OleDbType.Integer, 4, filter),
                                        DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 255, fidlist)
                                  };

            int pagetop = (pageid - 1) * tpp;
            string sqlstring;


            if (pageid == 1)
            {

                sqlstring = "SELECT TOP " + tpp.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=@filter";
                if (fidlist != "0")
                {
                    sqlstring += " AND [fid] IN (@fidlist)";
                }
                sqlstring += " ORDER BY [tid] DESC";
            }
            else
            {

                sqlstring = "SELECT TOP " + tpp.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "topics]  WHERE [tid] < (SELECT MIN([tid])  FROM (SELECT TOP " + pagetop + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics]   WHERE [displayorder]=@filter";
                if (fidlist != "0")
                {
                    sqlstring += " AND [fid] IN (@fidlist)";
                }
                sqlstring += " ORDER BY [tid] DESC) AS T)  ORDER BY [tid] DESC";
            }

            return DbHelper.ExecuteReader(CommandType.Text, sqlstring, parms);
        }

        public int GetUnauditNewTopicCount(string fidlist, int filter)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@filter", (DbType)OleDbType.Integer, 4, filter),
                                        DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 255, fidlist) 
                                  };

            string sql = "";
            sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=@filter";
            if (fidlist != "0")
            {

                sql += " AND fid IN (@fidlist)";
            }
            return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, sql, parms), 0);

        }


        public IDataReader GetUnauditNewPost(string fidlist, int ppp, int pageid, int tableid, int filter)
        {
            //DbHelper.ExecuteReader(CommandType.Text,"SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=-2 and fid= "+fid+" ORDER BY [tid] DESC");

            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@filter", (DbType)OleDbType.Integer, 4, filter),
                                        DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 255, fidlist)
                                  };


            int pagetop = (pageid - 1) * ppp;
            string sqlstring;


            if (pageid == 1)
            {
                sqlstring = "SELECT TOP " + ppp.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "] WHERE [invisible]=@filter AND [layer]>0";
                if (fidlist != "0")
                {
                    sqlstring += " AND [fid] IN (@fidlist)";
                }
                sqlstring += " ORDER BY [pid] DESC";

            }
            else
            {
                sqlstring = "SELECT TOP " + ppp.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "]  WHERE [pid] < (SELECT MIN([pid])  FROM (SELECT TOP " + pagetop + " [pid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "]  where [invisible]=@filter AND [layer]>0";
                if (fidlist != "0")
                {
                    sqlstring += " AND [fid] IN (@fidlist)";
                }
                sqlstring += " ORDER BY [pid] DESC) AS T)  ORDER BY [pid] DESC";
            }

            return DbHelper.ExecuteReader(CommandType.Text, sqlstring, parms);
        }

        public int GetUnauditNewPostCount(string fidlist, int tableid, int filter)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@filter", (DbType)OleDbType.Integer, 4, filter),
                                        DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 255, fidlist)
                                  };
            string sql = "";
            sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "] WHERE [invisible]=@filter AND [layer]>0";

            if (fidlist != "0")
            {
                sql += " AND fid IN (@fidlist)";
            }
            return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, sql, parms), 0);
        }

        public void PassAuditNewTopic(string tableid, string ignore, string validate, string delete, string fidlist)
        {
            //if (!Utils.IsNumericArray(tidlist.Split(',')))
            //{
            //    return;
            //}

            //postTableName = Utils.ChkSQL(postTableName);
            //if (validate != "")
            //{
            //    string sqlstring = "UPDATE  [" + postTableName + "]  SET [invisible]=0 WHERE [layer]=0  AND [tid] IN(" + tidlist + ")";
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            //    sqlstring = "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics]  SET [displayorder]=0 WHERE [tid] IN(" + tidlist + ")";
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            //    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic] + " + tidlist.Split(',').Length);

            //    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + " + tidlist.Split(',').Length);

            //    //更新相关的版块统计信息
            //    foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") ORDER BY [tid] ASC").Tables[0].Rows)
            //    {
            //        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm,[lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lasttid]=" + dr["tid"].ToString() + " ,	[lasttitle]='" + dr["title"].ToString().Replace("'", "''") + "',[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
            //        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
            //    }
            //}


            //if (delete != "")
            //{
            //    string sqlstring = "DELETE  [" + postTableName + "]  WHERE [tid] IN(" + tidlist + ")";
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            //    sqlstring = "DELETE  [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ")";
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            //    foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") ORDER BY [tid] ASC").Tables[0].Rows)
            //    {
            //        DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + dr["fid"].ToString());
            //        DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] = " + dr["posterid"].ToString());
            //    }
            //}

            //    string sqlstring = "UPDATE  [" + postTableName + "]  SET [invisible]=0 WHERE [layer]=0  AND [tid] IN(" + tidlist + ")";
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            //    sqlstring = "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics]  SET [displayorder]=0 WHERE [tid] IN(" + tidlist + ")";
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            //    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic] + " + tidlist.Split(',').Length);

            //    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + " + tidlist.Split(',').Length);

            //    //更新相关的版块统计信息
            //    foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") ORDER BY [tid] ASC").Tables[0].Rows)
            //    {
            //        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm,[lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lasttid]=" + dr["tid"].ToString() + " ,	[lasttitle]='" + dr["title"].ToString().Replace("'", "''") + "',[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
            //        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
            //    }

            if (!validate.Equals(""))
            {
                DbHelper.ExecuteNonQuery("UPDATE  [" + BaseConfigs.GetTablePrefix + "topics]  SET [displayorder]=0 WHERE ([tid] IN(" + validate + ")) AND ([fid] IN (" + fidlist + "))");
                DbHelper.ExecuteNonQuery("UPDATE  [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "]  SET [invisible]=0 WHERE [layer]=0  AND ([tid] IN(" + validate + ")) AND ([fid] IN (" + fidlist + "))");
                foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + validate + ") ORDER BY [tid] ASC").Tables[0].Rows)
                {
                    //DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm,[lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lasttid]=" + dr["tid"].ToString() + " ,	[lasttitle]='" + dr["title"].ToString().Replace("'", "''") + "',[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=(IIF(DATEPART(\"yyyy\", lastpost)=DATEPART(\"yyyy\",NOW()) AND DATEPART(\"m\", lastpost)=DATEPART(\"m\",NOW()) AND DATEPART(\"d\", lastpost)=DATEPART(\"d\",NOW()),[todayposts]*1 + 1,1)),[lasttid]=" + dr["tid"].ToString() + " ,	[lasttitle]='" + dr["title"].ToString().Replace("'", "''") + "',[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
                }
            }

            if (!delete.Equals(""))
            {
                DbHelper.ExecuteNonQuery("DELETE  * FROM  [" + BaseConfigs.GetTablePrefix + "topics]  WHERE ([tid] IN(" + delete + "))  AND ([fid] IN (" + fidlist + "))");
                DbHelper.ExecuteNonQuery("DELETE *  FROM  [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "]  WHERE [layer]=0  AND ([tid] IN(" + delete + "))  AND ([fid] IN (" + fidlist + "))");
            }

            if (!ignore.Equals(""))
            {
                DbHelper.ExecuteNonQuery("UPDATE  [" + BaseConfigs.GetTablePrefix + "topics]  SET [displayorder]=-3 WHERE ([tid] IN(" + ignore + "))  AND ([fid] IN (" + fidlist + "))");
            }

        }


        public void PassAuditNewTopic(string postTableName, string tidlist)
        {
            if (!Utils.IsNumericArray(tidlist.Split(',')))
            {
                return;
            }

            postTableName = Utils.ChkSQL(postTableName);

            string sqlstring = "UPDATE  [" + postTableName + "]  SET [invisible]=0 WHERE [layer]=0  AND [tid] IN(" + tidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            sqlstring = "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics]  SET [displayorder]=0 WHERE [tid] IN(" + tidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic] + " + tidlist.Split(',').Length);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + " + tidlist.Split(',').Length);

            //更新相关的版块统计信息
            foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") ORDER BY [tid] ASC").Tables[0].Rows)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=(IIF(DATEPART(\"yyyy\", lastpost)=DATEPART(\"yyyy\",NOW()) AND DATEPART(\"m\", lastpost)=DATEPART(\"m\",NOW()) AND DATEPART(\"d\", lastpost)=DATEPART(\"d\",NOW()),[todayposts]*1 + 1,1)),[lasttid]=" + dr["tid"].ToString() + " ,	[lasttitle]='" + dr["title"].ToString().Replace("'", "''") + "',[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
            }
        }

        //public DataTable GetDetachTableId()
        //{
        //    string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "tablelist] ORDER BY ID ASC";
        //    return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        //}

        public int GetCurrentPostTableRecordCount(int currentPostTableId)
        {
            string sql = "SELECT count(pid) FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [invisible]=1 OR [invisible]=-3";
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql));
        }

        public string GetUnauditPostSQL(int currentPostTableId)
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE ([invisible]=1 OR [invisible]=-3) AND [layer]>0 ORDER BY [pid] DESC";
        }

        public DataTable GetUnauditPostSQL(int currentPostTableId, string fidlist)
        {
            return DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [invisible]=1 AND [layer]>0  AND [fid]=" + fidlist + "").Tables[0];
        }

        public void PassPost(int tableid, string validate, string delete, string ignore, string fidlist)
        {
            DbParameter[] parms = 
			{
                //DbHelper.MakeInParam("@tableid", (DbType)OleDbType.Integer, 4, tableid),
				DbHelper.MakeInParam("@validate", (DbType)OleDbType.VarChar, 100, validate),
                DbHelper.MakeInParam("@ignore", (DbType)OleDbType.VarChar, 100, ignore),
                DbHelper.MakeInParam("@delete", (DbType)OleDbType.VarChar,100, delete)
            };
            if (!validate.Equals(""))
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "] SET [invisible]=0 WHERE ([pid] IN(" + validate + "))  AND ([fid] IN (" + fidlist + "))", parms);
            }
            if (!delete.Equals(""))
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "] WHERE ([pid] IN(" + delete + "))  AND ([fid] IN (" + fidlist + "))", parms);
            }

            if (!ignore.Equals(""))
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + tableid + "] SET [invisible]=-3 WHERE ([pid] IN(" + ignore + "))  AND ([fid] IN (" + fidlist + "))", parms);

            }

        }

        public void PassPost(int currentPostTableId, string pidlist)
        {
            if (!Utils.IsNumericArray(pidlist.Split(',')))
            {
                return;
            }
            string sqlstring = "UPDATE  [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "]  SET [invisible]=0 WHERE [pid] IN(" + pidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + " + pidlist.Split(',').Length);

            //更新相关的版块统计信息
            foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [pid] IN(" + pidlist + ") ORDER BY [pid] ASC").Tables[0].Rows)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [posts]=[posts] + 1, [todayposts]=(IIF(DATEPART(\"yyyy\", lastpost)=DATEPART(\"yyyy\",NOW()) AND DATEPART(\"m\", lastpost)=DATEPART(\"m\",NOW()) AND DATEPART(\"d\", lastpost)=DATEPART(\"d\",NOW()),[todayposts]*1 + 1,1)),[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics]  SET [replies]=[replies]+1,[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + ",[lastpost]='" + dr["postdatetime"].ToString() + "' WHERE [tid]=" + dr["tid"].ToString());
            }
        }

        public DataTable GetPostLayer(int currentPostTableId, int postid)
        {
            string sql = "SELECT TOP 1 [layer],[tid]  FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [pid]=" + postid;
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void UpdateBBCCode(int available, string tag, string icon, string replacement, string example,
    string explanation, string param, string nest, string paramsdescript, string paramsdefvalue, int id)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@available", (DbType)OleDbType.Integer, 4, available),
				DbHelper.MakeInParam("@tag", (DbType)OleDbType.VarChar, 100, tag),
				DbHelper.MakeInParam("@icon", (DbType)OleDbType.VarChar,50, icon),
				DbHelper.MakeInParam("@replacement", (DbType)OleDbType.VarWChar,0, replacement),
				DbHelper.MakeInParam("@example", (DbType)OleDbType.VarWChar, 255, example),
				DbHelper.MakeInParam("@explanation", (DbType)OleDbType.VarWChar, 0, explanation),
				DbHelper.MakeInParam("@params", (DbType)OleDbType.Integer, 4, param),
				DbHelper.MakeInParam("@nest", (DbType)OleDbType.Integer, 4, nest),
				DbHelper.MakeInParam("@paramsdescript", (DbType)OleDbType.VarWChar, 0, paramsdescript),
				DbHelper.MakeInParam("@paramsdefvalue", (DbType)OleDbType.VarWChar, 0, paramsdefvalue),
				DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "bbcodes] SET [available]=@available,tag=@tag, icon=@icon,replacement=@replacement," +
                "example=@example,explanation=@explanation,params=@params,nest=@nest,paramsdescript=@paramsdescript,paramsdefvalue=@paramsdefvalue " +
                "WHERE [id]=@id";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public DataTable GetBBCode()
        {
            string sql = "Select * From [" + BaseConfigs.GetTablePrefix + "bbcodes] Order BY [id] ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetBBCode(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=@id", parm).Tables[0];
        }

        public void DeleteBBCode(string idlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes]  WHERE [id] IN(" + idlist + ")");
        }

        public void SetBBCodeAvailableStatus(string idlist, int status)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@status",(DbType)OleDbType.Integer,4,status)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "bbcodes] SET [available]=@status  WHERE [id] IN(" + idlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public DataSet GetBBCCodeById(int id)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@id", (DbType)OleDbType.Integer, 4, id)
			};
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=@id";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 获取关注主题列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="starttime">起始时间</param>
        /// <param name="orderfieldname">排序字段</param>
        /// <param name="visibleForum">板块范围(逗号分隔)</param>
        /// <param name="isdigest">是否精华</param>
        /// <param name="onlyimg">是否仅取带有图片附件的帖子</param>
        /// <returns></returns>
        public DataTable GetFocusTopicList(int count, int views, int fid, string starttime, string orderfieldname, string visibleForum, bool isdigest, bool onlyimg)
        {
            //DbParameter param = DbHelper.MakeInParam("@starttime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(starttime));

            //string digestParam = "";
            //if (isdigest)
            //{
            //    digestParam = " AND [t].[digest] > 0";
            //}

            //string fidParam = "";
            //if (fid > 0)
            //{
            //    fidParam = " AND ([t].[fid] = " + fid + " OR CHARINDEX('," + fid + ",' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 ) "; //" AND [fid]=" + fid;
            //}

            //if (count < 0)
            //{
            //    count = 0;
            //}

            //string attParam = "";
            //if (onlyimg)
            //{
            //    attParam = " AND [t].[attachment]=2";
            //}

            //if (visibleForum != string.Empty)
            //{
            //    visibleForum = " AND [t].[fid] IN (" + visibleForum + ")";
            //}

            //string sqlstr = @"SELECT TOP {0} [t].*, [f].[name] FROM [{1}topics] AS [t] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forums] AS [f] ON [t].[fid] = [f].[fid] WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0 AND [t].[views] > {2} AND [t].[postdatetime] > @starttime{3}{4} ORDER BY [t].[{5}] DESC";

            //sqlstr = string.Format(sqlstr,
            //    count,
            //    BaseConfigs.GetTablePrefix,
            //    views,
            //    fidParam + digestParam + visibleForum,
            //    attParam,
            //    orderfieldname
            //    );

            //return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, param).Tables[0];

            string digestParam = "";
            //if (isdigest)
            //    digestParam = " AND [digest] > 0";

            string fidParam = "";
            //if (fid > 0)
            //    fidParam = " AND [fid]=" + fid;

            //if (count < 0)
            //    count = 0;

            //string attParam = "";
            //if (onlyimg)
            //    attParam = "AND [attachment]=2";

            //if (visibleForum != string.Empty)
            //    visibleForum = " AND [fid] IN (" + visibleForum + ")";

            //string sqlstr = "SELECT TOP {0} * FROM [{1}topics] WHERE [displayorder] >=0 AND [views] > {2} AND [postdatetime] > '{3}'{4}{5} ORDER BY [{6}] DESC";

            //sqlstr = string.Format(sqlstr,
            //    count,
            //    BaseConfigs.GetTablePrefix,
            //    views,
            //    starttime,
            //    fidParam + digestParam + visibleForum,
            //    attParam,
            //    orderfieldname
            //    );
            if (isdigest)
                digestParam = " AND [digest] > 0";


            if (fid > 0)
                fidParam = " AND [fid]=" + fid;

            if (count < 0)
                count = 0;

            string attParam = "";
            if (onlyimg)
                attParam = "AND [attachment]=2";



            if (visibleForum != string.Empty)
                visibleForum = " AND [fid] IN (" + visibleForum + ")";

            string sqlstr = @"SELECT TOP {0} * FROM [{1}topics] WHERE [displayorder] >=0 AND [views] > {2} AND [postdatetime] > #{3}# {4} {5} ORDER BY [{6}] DESC";

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
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarChar, 20, lastposter),
                                        DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, lastposterid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposter]=@lastposter  WHERE [lastposterid]=@lastposterid", parms);
        }

        public void UpdateTopicPoster(int posterid, string poster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarChar, 20, poster),
                                        DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer, 4, posterid)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
        }

        public void UpdatePostPoster(int posterid, string poster, string posttableid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer, 4, posterid),
                                        DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarChar, 20, poster)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
        }

        /// <summary>
        /// 更新主题信息
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        public bool UpdateTopicAllInfo(TopicInfo topicinfo)
        {
            string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET fid='{1}',iconid='{2}',typeid='{3}',readperm='{4}',price='{5}',poster='{6}'," +
                "title='{7}',postdatetime='{8}',lastpost='{9}',lastpostid='{10}',lastposter='{11}'," +
                "views='{12}',replies='{13}',displayorder='{14}',highlight='{15}',digest='{16}',rate='{17}',blog='{18}'," +
                //"poll='{19}',attachment='{20}',moderated='{21}',closed='{22}' WHERE [tid]={0}",
                "attachment='{19}',moderated='{20}',closed='{21}' WHERE [tid]={0}",
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
                //topicinfo.Poll.ToString(),
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

            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=" + tid.ToString());
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid]=" + tid.ToString());
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=" + tid.ToString());
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + posttablename + "] WHERE [tid]=" + tid.ToString());
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid]=" + tid.ToString());
            return true;
        }

        public bool SetTypeid(string topiclist, int value)
        {

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [typeid]=" + value.ToString() + " WHERE [tid] IN(" + topiclist + ")");
            return true;
        }

        public DataSet GetPosts(int tid, int pagesize, int pageindex, string posttablename)
        {
            //DbParameter[] parms = {
            //    DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
            //    DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pagesize),
            //    DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageindex),
            //    DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 30, posttablename)
            //};
            //DataSet ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpostlist", parms);
            //return ds;
            DbParameter[] prams = {
				DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, tid),
                //DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pagesize),
                //DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageindex),
                //DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 30, posttablename)
			};
            DataSet ds;
            String sql = "";
            int pagetop = (pageindex - 1) * pagesize;
            //dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=@tid").Tables[0];
            if (pageindex == 1)
            {
                sql = "SELECT TOP " + pagesize + " " +
                                     " [" + posttablename + "].[pid]," +
                                     "[" + posttablename + "].[fid]," +
                                     "[" + posttablename + "].[title]," +
                                     "[" + posttablename + "].[layer]," +
                                     "[" + posttablename + "].[message]," +
                                     "[" + posttablename + "].[ip]," +
                                     "[" + posttablename + "].[lastedit]," +
                                     "[" + posttablename + "].[postdatetime]," +
                                     "[" + posttablename + "].[attachment]," +
                                     "[" + posttablename + "].[poster]," +
                                     "[" + posttablename + "].[posterid]," +
                                     "[" + posttablename + "].[invisible] as [invisible]," +
                                     "[" + posttablename + "].[usesig]," +
                                     "[" + posttablename + "].[htmlon]," +
                                     "[" + posttablename + "].[smileyoff]," +
                                     "[" + posttablename + "].[parseurloff]," +
                                     "[" + posttablename + "].[bbcodeoff]," +
                                     "[" + posttablename + "].[rate]," +
                                     "[" + posttablename + "].[ratetimes]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[nickname]," +
                                      "[" + BaseConfigs.GetTablePrefix + "users].[gender]," +
                                       "[" + BaseConfigs.GetTablePrefix + "users].[bday]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[username]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[groupid]," +
                                      "[" + BaseConfigs.GetTablePrefix + "users].[spaceid]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[email]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[showemail]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[digestposts]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[credits]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits1]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits2]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits3]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits4]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits5]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits6]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits7]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[extcredits8]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[posts]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[joindate]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[onlinestate]," +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[lastactivity], " +
                                     "[" + BaseConfigs.GetTablePrefix + "users].[invisible]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[avatar]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[medals]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS signature," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[location]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[customstatus], " +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[website], " +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[icq]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[qq]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[msn]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[yahoo]," +
                                     "[" + BaseConfigs.GetTablePrefix + "userfields].[skype]" +
      "FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=@tid AND [" + posttablename + "].[invisible]=0 ORDER BY [" + posttablename + "].[pid]";
            }
            else
            {
                sql = "SELECT TOP " + pagesize + " " +
                                        " [" + posttablename + "].[pid]," +
                                        "[" + posttablename + "].[fid]," +
                                        "[" + posttablename + "].[title]," +
                                        "[" + posttablename + "].[layer]," +
                                        "[" + posttablename + "].[message]," +
                                        "[" + posttablename + "].[ip]," +
                                        "[" + posttablename + "].[lastedit]," +
                                        "[" + posttablename + "].[postdatetime]," +
                                        "[" + posttablename + "].[attachment]," +
                                        "[" + posttablename + "].[poster]," +
                                        "[" + posttablename + "].[posterid]," +
                                        "[" + posttablename + "].[invisible] as [invisible]," +
                                        "[" + posttablename + "].[usesig]," +
                                        "[" + posttablename + "].[htmlon]," +
                                        "[" + posttablename + "].[smileyoff]," +
                                        "[" + posttablename + "].[parseurloff]," +
                                        "[" + posttablename + "].[bbcodeoff]," +
                                        "[" + posttablename + "].[rate]," +
                                        "[" + posttablename + "].[ratetimes]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[nickname]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[gender]," +
                                       "[" + BaseConfigs.GetTablePrefix + "users].[bday]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[username]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[groupid]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[spaceid]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[email]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[showemail]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[digestposts]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[credits]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits1]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits2]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits3]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits4]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits5]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits6]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits7]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[extcredits8]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[posts]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[joindate]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[onlinestate]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[lastactivity]," +
                                        "[" + BaseConfigs.GetTablePrefix + "users].[invisible] AS [userinvisible]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[avatar]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[medals]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS [signature]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[location]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[customstatus]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[website]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[icq]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[qq]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[msn]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[yahoo]," +
                                        "[" + BaseConfigs.GetTablePrefix + "userfields].[skype] " +
        "FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]= @pid  AND [pid] > (SELECT MAX([pid]) FROM (SELECT TOP " + pagetop + " [" + posttablename + "].[pid] FROM [" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=@pid AND [" + posttablename + "].[invisible]=0 ORDER BY [" + posttablename + "].[pid]) AS tblTmp) ORDER BY [" + posttablename + "].[pid]";

            }

            ds = DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
            //ds.Tables.Add(dt.Copy());
            return ds;
        }


        public int GetAttachCount(int pid)
        {
            DbParameter[] prams2 = {
						DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 0, pid)
					};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) AS [aidcount] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid] = @pid", prams2), 0);

        }

        public bool SetDisplayorder(string topiclist, int value)
        {
            //SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            //conn.Open();
            //using (SqlTransaction trans = conn.BeginTransaction())
            //{
            //    try
            //    {
            DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [displayorder]=" + value.ToString() + " WHERE [tid] IN(" + topiclist + ")");
            //        trans.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        throw ex;
            //    }
            //}
            //conn.Close();
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
            DbParameter[] parms = {
				DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid),
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userid),
				DbHelper.MakeInParam("@username", (DbType)OleDbType.VarWChar, 20, username),
				DbHelper.MakeInParam("@extcredits", (DbType)OleDbType.Integer, 1, extid),
				//DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Now),
				DbHelper.MakeInParam("@score", (DbType)OleDbType.Integer, 2, score),
				DbHelper.MakeInParam("@reason", (DbType)OleDbType.VarWChar, 50, reason)
			};

            string CommandText = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "ratelog] ([pid],[uid],[username],[extcredits],[score],[reason]) VALUES (@pid,@uid,@username,@extcredits,@score,@reason)";

            return DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, parms);
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public bool DeleteRateLog()
        {
            try
            {
                if (DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ") > 1)
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
        /// 删除日志
        /// </summary>
        /// <param name="pid"></param>
        public void DeleteRateLog(int pid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid)
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE [pid]=@pid", parms);

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
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition);
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
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring = "";

            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC";
            }

            sqlstring = "SELECT r.*,p.[title] AS title,p.[poster] AS poster , p.[posterid] AS posterid,  ug.[grouptitle] AS grouptitle FROM ((((" + sqlstring + ") r LEFT JOIN [" + posttablename + "] p ON r.[pid] = p.[pid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON u.[uid] = r.[uid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ug ON ug.[groupid] = u.[groupid])";


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
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring = "";

            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition + "  ORDER BY [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp )  AND " + condition + " ORDER BY [id] DESC";
            }

            sqlstring = "SELECT r.*,p.[title] AS title,p.[poster] AS poster , p.[posterid] AS posterid,  ug.[grouptitle] AS grouptitle FROM (" + sqlstring + ") r LEFT JOIN [" + posttablename + "] p ON r.[pid] = p.[pid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON u.[uid] = r.[uid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ug ON ug.[groupid] = u.[groupid]";


            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];

        }

        /// <summary>
        /// 得到评分日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetRateLogCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "ratelog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 得到指定查询条件下的评分日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetRateLogCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        public int GetPostsCount(string posttableid)
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [portscount] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "]"), 0);
        }

        public IDataReader GetMaxAndMinTid(int fid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT MAX([tid]) AS [maxtid],MIN([tid]) AS [mintid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=@fid OR (CHARINDEX(',' + RTRIM(@fid) + ',', ',' + RTRIM(parentidlist) + ',') > 0))", parms);
        }

        public int GetPostCount(int fid, string posttablename)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [fid] = @fid", parms), 0);
        }

        public int GetPostCountByTid(int tid, string posttablename)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
			};

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [tid] = @tid AND [layer] <> 0", parms), 0);
        }

        public int GetPostCount(string posttableid, int tid, int posterid)
        {
            string posttablename = string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid);
            string sqlstr = string.Format("[{0}].[tid]={1} AND [{0}].[posterid]={2}", posttablename, tid, posterid);
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarWChar,100,sqlstr),
									   DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid))
								   };
            String sql = "SELECT COUNT(pid) FROM [" + string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid) + "] WHERE " + sqlstr + " AND [layer]>=0";

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
        }

        public int GetTodayPostCount(int fid, string posttablename)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [fid] = @fid AND DATEDIFF(\"d\", [postdatetime], NOW()) = 0 ", parms), 0);
        }

        public int GetPostCount(int fid, int posttableid)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS [postcount] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE  [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=@fid OR (INSTR(',' + RTRIM(parentidlist) + ',',',' + RTRIM(@fid) + ',') > 0))", prams), 0);
        }

        public int GetTodayPostCount(int fid, int posttableid)
        {
            DbParameter[] prams = {
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
			};

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pid) AS [postcount] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE  [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=@fid OR (INSTR(',' + RTRIM(parentidlist) + ',',',' + RTRIM(@fid) + ',') > 0)) AND DATEDIFF(\"d\", [postdatetime], NOW()) = 0 ", prams), 0);
        }

        public IDataReader GetMaxAndMinTidByUid(int uid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT MAX([tid]) AS [maxtid],MIN([tid]) AS [mintid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid] = @uid", parms);
        }

        public int GetPostCountByUid(int uid, string posttablename)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
			};

            return Math.Abs(Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [posterid] = @uid", parms), 0));
        }

        public int GetTodayPostCountByUid(int uid, string posttablename)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, uid)
			};

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [posterid] = @uid AND DATEDIFF(\"d\", [postdatetime], NOW()) = 0 ", parms), 0);
        }

        public int GetTopicsCount()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([tid]) AS [topicscount] FROM [" + BaseConfigs.GetTablePrefix + "topics]"), 0);
        }

        public void ReSetStatistic(int UserCount, int TopicsCount, int PostCount, string lastuserid, string lastusername)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@totaltopic", (DbType)OleDbType.Integer, 4, TopicsCount),
				DbHelper.MakeInParam("@totalpost", (DbType)OleDbType.Integer, 4, PostCount),
				DbHelper.MakeInParam("@totalusers", (DbType)OleDbType.Integer, 4, UserCount),
				DbHelper.MakeInParam("@lastusername", (DbType)OleDbType.VarChar, 20, lastusername),
				DbHelper.MakeInParam("@lastuserid", (DbType)OleDbType.Integer, 4, Utils.StrToInt(lastuserid, 0))

			};

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=@totaltopic,[totalpost]=@totalpost,[totalusers]=@totalusers,[lastusername]=@lastusername,[lastuserid]=@lastuserid", parms);
        }

        public IDataReader GetTopicTids(int statcount, int lasttid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@lasttid", (DbType)OleDbType.Integer, 4, lasttid),
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] > @lasttid ORDER BY [tid]", parms);

        }

        public IDataReader GetLastPost(int tid, int posttableid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid);
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [pid], [postdatetime], [posterid], [poster] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] = @tid ORDER BY [pid] DESC", parm);
        }

        public void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@postcount", (DbType)OleDbType.Integer, 4, postcount),
                                        DbHelper.MakeInParam("@lastpostid", (DbType)OleDbType.Integer, 4, lastpostid),
                                        DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.VarChar, 20, lastpost),
                                        DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, lastposterid),
                                        DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarChar, 20, poster),
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpost]=@lastpost, [lastposterid]=@lastposterid, [lastposter]=@lastposter, [replies]=@postcount WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = @tid", parms);
        }

        public void UpdateTopicLastPosterId(int tid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposterid]=(SELECT  (iif(isnull(MIN(lastpostid)),-1,MIN(lastpostid)))-1 FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = @tid", parms);
        }

        public IDataReader GetTopics(int start_tid, int end_tid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@start_tid", (DbType)OleDbType.Integer, 4, start_tid),
				DbHelper.MakeInParam("@end_tid", (DbType)OleDbType.Integer, 4, end_tid)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] >= @start_tid AND [tid]<=@end_tid  ORDER BY [tid]", parms);
        }

        public IDataReader GetForumLastPost(int fid, string posttablename, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost, int lastposterid, string lastposter, int todaypostcount)
        {
            //DbParameter[] prams_posts = {
            //        DbHelper.MakeInParam("@lastfid", (DbType)OleDbType.Integer, 4, fid),
            //        DbHelper.MakeInParam("@topiccount", (DbType)OleDbType.Integer, 4, topiccount),
            //        DbHelper.MakeInParam("@postcount", (DbType)OleDbType.Integer, 4, postcount),
            //        DbHelper.MakeInParam("@lasttid", (DbType)OleDbType.Integer, 4, lasttid),
            //        DbHelper.MakeInParam("@lasttitle", (DbType)OleDbType.VarWChar, 80, lasttitle),
            //        DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.VarChar, 20, lastpost),
            //        DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, lastposterid),
            //        DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 20, lastposter),
            //        DbHelper.MakeInParam("@todaypostcount", (DbType)OleDbType.Integer, 4, todaypostcount)
            //                                };
            //return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster] FROM [" + posttablename + "] WHERE [fid] = @lastfid OR [fid] IN (SELECT fid  FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE CHARINDEX(',' + RTRIM(@lastfid) + ',', ',' + RTRIM(parentidlist) + ',') > 0) ORDER BY [pid] DESC", prams_posts);
            DbParameter[] prams_posts = {
					DbHelper.MakeInParam("@lastfid", (DbType)OleDbType.Integer, 4, fid),
					DbHelper.MakeInParam("@topiccount", (DbType)OleDbType.Integer, 4, topiccount),
					DbHelper.MakeInParam("@postcount", (DbType)OleDbType.Integer, 4, postcount),
					DbHelper.MakeInParam("@lasttid", (DbType)OleDbType.Integer, 4, lasttid),
					DbHelper.MakeInParam("@lasttitle", (DbType)OleDbType.VarWChar, 80, lasttitle),
					DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.VarChar, 20, lastpost),
					DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, lastposterid),
					DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 20, lastposter),
					DbHelper.MakeInParam("@todaypostcount", (DbType)OleDbType.Integer, 4, todaypostcount)
                                            };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster] FROM [" + posttablename + "] WHERE [fid] = @lastfid ORDER BY [pid] DESC", prams_posts);
        }

        public void UpdateForum(int fid, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost, int lastposterid, string lastposter, int todaypostcount)
        {
            DbParameter[] prams_posts = {
					DbHelper.MakeInParam("@topiccount", (DbType)OleDbType.Integer, 4, topiccount),
					DbHelper.MakeInParam("@postcount", (DbType)OleDbType.Integer, 4, postcount),
					DbHelper.MakeInParam("@lasttid", (DbType)OleDbType.Integer, 4, lasttid),
					DbHelper.MakeInParam("@lasttitle", (DbType)OleDbType.VarWChar, 80, lasttitle),
					DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.VarChar, 20, lastpost),
					DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, lastposterid),
					DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 20, lastposter),
					DbHelper.MakeInParam("@todaypostcount", (DbType)OleDbType.Integer, 4, todaypostcount),
                    DbHelper.MakeInParam("@lastfid", (DbType)OleDbType.Integer, 4, fid)
                                            };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = @topiccount, [posts]=@postcount, [todayposts] = @todaypostcount, [lasttid] = @lasttid, [lasttitle] = @lasttitle, [lastpost]=@lastpost, [lastposterid] = @lastposterid, [lastposter]=@lastposter WHERE [" + BaseConfigs.GetTablePrefix + "forums].[fid] = @lastfid", prams_posts);
        }

        public IDataReader GetForums(int start_fid, int end_fid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@start_fid", (DbType)OleDbType.Integer, 4, start_fid),
				DbHelper.MakeInParam("@end_fid", (DbType)OleDbType.Integer, 4, end_fid)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT  [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] >= @start_fid AND [fid]<=@end_fid", parms);
        }

        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        public void ReSetClearMove()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [closed] > 1");
        }

        public IDataReader GetLastPostByFid(int fid, string posttablename)
        {
            DbParameter parm = DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid);
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster] FROM [" + posttablename + "] WHERE [fid] = @fid ORDER BY [pid] DESC", parm);
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
        ////public bool CreatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime, int userid)
        ////{
        ////    DbParameter[] parms = {
        ////                               DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
        ////                               DbHelper.MakeInParam("@polltype",(DbType)OleDbType.Integer,4,polltype),
        ////                               DbHelper.MakeInParam("@itemcount",(DbType)OleDbType.Integer,4,itemcount),
        ////                               DbHelper.MakeInParam("@itemnamelist",(DbType)OleDbType.VarWChar,0,itemnamelist),
        ////                               DbHelper.MakeInParam("@itemvaluelist",(DbType)SqlDbType.Text,0,itemvaluelist),
        ////                               DbHelper.MakeInParam("@usernamelist",(DbType)OleDbType.VarWChar,0,""),
        ////                               DbHelper.MakeInParam("@enddatetime",(DbType)OleDbType.VarChar,19,enddatetime),
        ////                               DbHelper.MakeInParam("@userid",(DbType)OleDbType.Integer,4,userid)
        ////                           };
        ////    return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createpoll", parms) > 0;
        ////}
        public int CreatePoll(PollInfo pollinfo)
        {
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,pollinfo.Tid),
                                       DbHelper.MakeInParam("@displayorder",(DbType)OleDbType.Integer,4,pollinfo.Displayorder),
                                       DbHelper.MakeInParam("@multiple",(DbType)OleDbType.Integer,4,pollinfo.Multiple),
                                       DbHelper.MakeInParam("@visible",(DbType)OleDbType.Integer,4,pollinfo.Visible),
                                       DbHelper.MakeInParam("@maxchoices",(DbType)OleDbType.Integer,4,pollinfo.Maxchoices),
                                       DbHelper.MakeInParam("@expiration",(DbType)OleDbType.DBTimeStamp,8,pollinfo.Expiration),
                                       DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,pollinfo.Uid),
                                       DbHelper.MakeInParam("@voternames",(DbType)OleDbType.VarWChar,0,pollinfo.Voternames)
                                  };
            //string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "polls] ( [tid] ,[displayorder] ,[multiple] ,[visible] ,[maxchoices] ,[expiration] ,[uid] ,[voternames] ) VALUES (@tid, @displayorder, @multiple, @visible, @maxchoices, @expiration, @uid, @voternames);SELECT SCOPE_IDENTITY()  AS 'pollid'";
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), -1);
            int pollid;
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "polls] ( [tid] ,[displayorder] ,[multiple] ,[visible] ,[maxchoices] ,[expiration] ,[uid] ,[voternames] ) VALUES (@tid, @displayorder, @multiple, @visible, @maxchoices, @expiration, @uid, @voternames);";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
            string sql1 = "select max(pollid) from [" + BaseConfigs.GetTablePrefix + "polls]";
            pollid = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql1));
            return pollid;
        }

        public int CreatePollOption(PollOptionInfo polloptioninfo)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,polloptioninfo.Tid),
            //                           DbHelper.MakeInParam("@pollid",(DbType)OleDbType.Integer,4,polloptioninfo.Pollid),
            //                           DbHelper.MakeInParam("@votes",(DbType)OleDbType.Integer,4,polloptioninfo.Votes),
            //                           DbHelper.MakeInParam("@displayorder",(DbType)OleDbType.Integer,4,polloptioninfo.Displayorder),
            //                           DbHelper.MakeInParam("@polloption",(DbType)OleDbType.VarChar,80,polloptioninfo.Polloption),
            //                           DbHelper.MakeInParam("@voternames",(DbType)OleDbType.VarWChar,0,polloptioninfo.Voternames)
            //                       };
            //string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "polloptions] ([tid] ,[pollid] ,[votes] ,[displayorder] ,[polloption] ,[voternames] ) VALUES (@tid, @pollid, @votes, @displayorder, @polloption, @voternames);SELECT SCOPE_IDENTITY()  AS 'polloptionid'";
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), -1);
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,polloptioninfo.Tid),
									   DbHelper.MakeInParam("@pollid",(DbType)OleDbType.Integer,4,polloptioninfo.Pollid),
									   DbHelper.MakeInParam("@votes",(DbType)OleDbType.Integer,4,polloptioninfo.Votes),
									   DbHelper.MakeInParam("@displayorder",(DbType)OleDbType.Integer,4,polloptioninfo.Displayorder),
									   DbHelper.MakeInParam("@polloption",(DbType)OleDbType.VarChar,80,polloptioninfo.Polloption),
									   DbHelper.MakeInParam("@voternames",(DbType)OleDbType.VarWChar,0,polloptioninfo.Voternames)
								   };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "polloptions] ([tid] ,[pollid] ,[votes] ,[displayorder] ,[polloption] ,[voternames] ) VALUES (@tid, @pollid, @votes, @displayorder, @polloption, @voternames);";
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), -1);
        }



        //public bool UpdatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime)
        //{
        //    DbParameter[] parms = {
        //                               DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
        //                               DbHelper.MakeInParam("@polltype",(DbType)OleDbType.Integer,4,polltype),
        //                               DbHelper.MakeInParam("@itemcount",(DbType)OleDbType.Integer,4,itemcount),
        //                               DbHelper.MakeInParam("@itemnamelist",(DbType)SqlDbType.Text,0,itemnamelist),
        //                               DbHelper.MakeInParam("@itemvaluelist",(DbType)SqlDbType.Text,0,itemvaluelist),
        //                               DbHelper.MakeInParam("@enddatetime",(DbType)OleDbType.VarChar,19,enddatetime),
        //                           };
        //    return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatepoll", parms) > 0;
        //}

        /// <summary>
        /// 获得投票信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        //public IDataReader GetPoll(int tid)
        //{
        //    DbParameter[] parms = {
        //                               DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
        //                           };
        //    IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [itemvaluelist], [usernamelist] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", parms);
        //    return reader;
        //}
        public IDataReader GetPollAndOptions(int tid)
        {
            string sql = string.Format("SELECT [poll].[multiple], [poll].[maxchoices], [poll].[expiration], [poll].[pollid], [options].[polloptionid], [options].[votes], [options].[polloption], [options].[voternames] FROM [{0}polls] AS [poll] LEFT JOIN [{0}polloptions] AS [options] ON [poll].[tid] = [options].[tid] WHERE [tid]={1}",
                 BaseConfigs.GetTablePrefix,
                 tid);
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }

        /// <summary>
        /// 根据投票信息更新数据库中的记录
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="selitemidlist">选择的投票项id列表</param>
        /// <param name="username">用户名</param>
        /// <returns>如果执行成功则返回0, 非法提交返回负值</returns>
        //public int UpdatePoll(int tid, string usernamelist, StringBuilder newselitemidlist)
        //{
        //    DbParameter[] parms = {
        //                                DbHelper.MakeInParam("@itemvaluelist",(DbType)SqlDbType.Text,0,newselitemidlist.ToString().Trim()),
        //                                DbHelper.MakeInParam("@usernamelist",(DbType)SqlDbType.Text,0,usernamelist.ToString().Trim()),
        //                                DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
        //                            };
        //    if (DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "polls] SET [itemvaluelist]=@itemvaluelist, [usernamelist]=@usernamelist WHERE [tid]=@tid", parms) > 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return -4;
        //    }
        //}
        public bool UpdatePoll(PollInfo pollinfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,pollinfo.Tid),
									   DbHelper.MakeInParam("@displayorder",(DbType)OleDbType.Integer,4,pollinfo.Displayorder),
									   DbHelper.MakeInParam("@multiple",(DbType)OleDbType.Integer,4,pollinfo.Multiple),
									   DbHelper.MakeInParam("@visible",(DbType)OleDbType.Integer,4,pollinfo.Visible),
									   DbHelper.MakeInParam("@maxchoices",(DbType)OleDbType.Integer,4,pollinfo.Maxchoices),
									   DbHelper.MakeInParam("@expiration",(DbType)OleDbType.DBTimeStamp,8,pollinfo.Expiration),
                                       DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,pollinfo.Uid),
                                       DbHelper.MakeInParam("@voternames",(DbType)OleDbType.VarWChar,0,pollinfo.Voternames),
                                       DbHelper.MakeInParam("@pollid",(DbType)OleDbType.Integer,4,pollinfo.Pollid),
                                  };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "polls] set [tid] = @tid, [displayorder] = @displayorder, [multiple] = @multiple, [visible] = @visible, [maxchoices] = @maxchoices, [expiration] = @expiration, [uid] = @uid, [voternames] = @voternames WHERE [pollid] = @pollid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms) > 0;
        }

        public bool UpdatePollOption(PollOptionInfo polloptioninfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,polloptioninfo.Tid),
									   DbHelper.MakeInParam("@pollid",(DbType)OleDbType.Integer,4,polloptioninfo.Pollid),
									   DbHelper.MakeInParam("@votes",(DbType)OleDbType.Integer,4,polloptioninfo.Votes),
									   DbHelper.MakeInParam("@displayorder",(DbType)OleDbType.Integer,4,polloptioninfo.Displayorder),
									   DbHelper.MakeInParam("@polloption",(DbType)OleDbType.VarChar,80,polloptioninfo.Polloption),
									   DbHelper.MakeInParam("@voternames",(DbType)OleDbType.VarWChar,0,polloptioninfo.Voternames),
                                       DbHelper.MakeInParam("@polloptionid",(DbType)OleDbType.Integer,4,polloptioninfo.Polloptionid)
								   };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "polloptions] set [tid] = @tid, [pollid] = @pollid, [votes] = @votes, [displayorder] = @displayorder, [polloption] = @polloption, [voternames] = @voternames WHERE [polloptionid] = @polloptionid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms) > 0;
        }

        public bool DeletePollOption(PollOptionInfo polloptioninfo)
        {
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@polloptionid",(DbType)OleDbType.Integer,4,polloptioninfo.Polloptionid)
								   };
            string sql = "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "polloptions] WHERE [polloptionid] = @polloptionid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms) > 0;
        }

        /// <summary>
        /// 获得与指定主题id关联的投票数据
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>投票数据</returns>
        //public DataTable GetPollList(int tid)
        //{
        //    DbParameter[] parms = {
        //                                DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
        //                            };
        //    DataTable dtTemp = DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpoll", parms).Tables[0];
        //    return dtTemp;
        //}
        public IDataReader GetPollList(int tid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}polls] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid));
        }

        public IDataReader GetPollOptionList(int tid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}polloptions] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid));
        }


        /// <summary>
        /// 获得投票的用户名
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public string GetPollUserNameList(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer, 4, tid)
								   };

            string strUsernamelist = DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT TOP 1 [voternames] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", parms);
            return strUsernamelist;
        }

        /// <summary>
        /// 得到投票帖的投票类型
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>投票类型</returns>
        public int GetPollType(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT TOP 1 [multiple] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", parms), 0);
        }

        /// <summary>
        /// 得到投票帖的结束时间
        /// </summary>
        /// <param name="tid">主题ＩＤ</param>
        /// <returns>结束时间</returns>
        public string GetPollEnddatetime(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            return Utils.GetDate(DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT TOP 1 [expiration] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", parms), Utils.GetDate());
        }



        /// <summary>
        /// 得到用户帖子分表信息
        /// </summary>
        /// <returns>分表记录集</returns>
        public DataSet GetAllPostTableName()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}tablelist] ORDER BY [id] DESC", BaseConfigs.GetTablePrefix));
        }

        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postinfo">帖子信息类</param>
        /// <returns>返回帖子id</returns>
        public int CreatePost(PostInfo postinfo, string posttableid)
        {
            //DbParameter[] parms = {

            //                           DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,2,postinfo.Fid),
            //                           DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
            //                           DbHelper.MakeInParam("@parentid",(DbType)OleDbType.Integer,4,postinfo.Parentid),
            //                           DbHelper.MakeInParam("@layer",(DbType)OleDbType.Integer,4,postinfo.Layer),
            //                           DbHelper.MakeInParam("@poster",(DbType)OleDbType.VarChar,15,postinfo.Poster),
            //                           DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid),
            //                           DbHelper.MakeInParam("@title",(DbType)OleDbType.VarWChar,60,postinfo.Title),
            //                           DbHelper.MakeInParam("@topictitle",(DbType)OleDbType.VarWChar,60,postinfo.Topictitle),
            //                           DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.DBTimeStamp,4, DateTime.Parse(postinfo.Postdatetime)),
            //                           DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar,0,postinfo.Message),
            //                           DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,postinfo.Ip),
            //                           DbHelper.MakeInParam("@lastedit",(DbType)OleDbType.VarWChar,50,postinfo.Lastedit),
            //                           DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,4,postinfo.Invisible),
            //                           DbHelper.MakeInParam("@usesig",(DbType)OleDbType.Integer,4,postinfo.Usesig),
            //                           DbHelper.MakeInParam("@htmlon",(DbType)OleDbType.Integer,4,postinfo.Htmlon),
            //                           DbHelper.MakeInParam("@smileyoff",(DbType)OleDbType.Integer,4,postinfo.Smileyoff),
            //                           DbHelper.MakeInParam("@bbcodeoff",(DbType)OleDbType.Integer,4,postinfo.Bbcodeoff),
            //                           DbHelper.MakeInParam("@parseurloff",(DbType)OleDbType.Integer,4,postinfo.Parseurloff),
            //                           DbHelper.MakeInParam("@attachment",(DbType)OleDbType.Integer,4,postinfo.Attachment),
            //                           DbHelper.MakeInParam("@rate",(DbType)OleDbType.Integer,2,postinfo.Rate),
            //                           DbHelper.MakeInParam("@ratetimes",(DbType)OleDbType.Integer,4,postinfo.Ratetimes)
            //                       };
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createpost{1}", BaseConfigs.GetTablePrefix, posttableid), parms).ToString(), -1);
            DbParameter[] prams = {

									   DbHelper.MakeInParam("@fid",(DbType)OleDbType.SmallInt,2,postinfo.Fid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
									   DbHelper.MakeInParam("@parentid",(DbType)OleDbType.Integer,4,postinfo.Parentid),
									   DbHelper.MakeInParam("@layer",(DbType)OleDbType.Integer,4,postinfo.Layer),
									   DbHelper.MakeInParam("@poster",(DbType)OleDbType.VarChar,15,postinfo.Poster),
									   DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid),
									   DbHelper.MakeInParam("@title",(DbType)OleDbType.VarChar,60,postinfo.Title),
									   DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.Date,8,DateTime.Parse(postinfo.Postdatetime)),
									   DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar,0,postinfo.Message),
									   DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,postinfo.Ip),
									   DbHelper.MakeInParam("@lastedit",(DbType)OleDbType.VarChar,50,postinfo.Lastedit),
									   DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,4,postinfo.Invisible),
									   DbHelper.MakeInParam("@usesig",(DbType)OleDbType.Integer,4,postinfo.Usesig),
									   DbHelper.MakeInParam("@htmlon",(DbType)OleDbType.Integer,4,postinfo.Htmlon),
									   DbHelper.MakeInParam("@smileyoff",(DbType)OleDbType.Integer,4,postinfo.Smileyoff),
									   DbHelper.MakeInParam("@bbcodeoff",(DbType)OleDbType.Integer,4,postinfo.Bbcodeoff),
									   DbHelper.MakeInParam("@parseurloff",(DbType)OleDbType.Integer,4,postinfo.Parseurloff),
									   DbHelper.MakeInParam("@attachment",(DbType)OleDbType.Integer,4,postinfo.Attachment),
									   DbHelper.MakeInParam("@rate",(DbType)OleDbType.SmallInt,2,postinfo.Rate),
									   DbHelper.MakeInParam("@ratetimes",(DbType)OleDbType.Integer,4,postinfo.Ratetimes)
								   };
            #region 存储过程转sql语句 CreatePost
            string tablename = BaseConfigs.GetTablePrefix + "posts" + posttableid;
            string strSQL = "";
            int postid = 0;


            strSQL = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "postid] WHERE DATEDIFF(\"n\",[postdatetime],now()) >5";
            DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

            #region 获取插入记录的最大id
            strSQL = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "postid] ([postdatetime]) VALUES (now())";
            DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);
            postid = Int32.Parse(DbHelper.ExecuteDataset(CommandType.Text, "select top 1 pid from [" + BaseConfigs.GetTablePrefix + "postid] order by pid desc").Tables[0].Rows[0][0].ToString());
            #endregion



            strSQL = "INSERT INTO [" + tablename + "]([pid], [fid], [tid], [parentid], [layer], [poster], [posterid], [title], [postdatetime], [message], [ip], [lastedit], [invisible], [usesig], [htmlon], [smileyoff], [bbcodeoff], [parseurloff], [attachment], [rate], [ratetimes]) VALUES(" + postid + ", @fid, @tid, @parentid, @layer, @poster, @posterid, @title, @postdatetime, @message, @ip, @lastedit, @invisible, @usesig, @htmlon, @smileyoff, @bbcodeoff, @parseurloff, @attachment, @rate, @ratetimes)";
            DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);
            if (postinfo.Parentid == 0)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + tablename + "] SET [parentid]=" + postid + " WHERE [pid]=" + postid);
            }

            if (postinfo.Invisible == 0)
            {
                //--更新帖子总数
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + 1";
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                DbParameter[] prams1 = {

												DbHelper.MakeInParam("@fid",(DbType)OleDbType.SmallInt,2,postinfo.Fid)
											};
                string fidlist = DbHelper.ExecuteDataset(CommandType.Text, "SELECT iif(isnull([parentidlist]),'',[parentidlist]) as [fidlist] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] = @fid", prams1).Tables[0].Rows[0][0].ToString();
                if (fidlist != string.Empty)
                {
                    fidlist = fidlist + "," + postinfo.Fid.ToString();
                }
                else
                {
                    fidlist = postinfo.Fid.ToString();
                }

                DbParameter[] prams2 = {									
											DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
											DbHelper.MakeInParam("@title",(DbType)OleDbType.VarChar,60,postinfo.Title),
											DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.VarChar,0,postinfo.Postdatetime),
											DbHelper.MakeInParam("@poster",(DbType)OleDbType.VarChar,15,postinfo.Poster),
											DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid)
										};
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET " +
                    "[posts]=[posts] + 1," +
                    "[todayposts]=iif( " +
                    "(Datediff(\"d\",[lastpost],now())=0),([todayposts]*1 + 1),1)," +

                    "[lasttid]=@tid," +
                    "[lasttitle]=@title," +
                    "[lastpost]=@postdatetime," +
                    "[lastposter]=@poster," +
                    "[lastposterid]=@posterid " +
                    "WHERE (instr( '," + fidlist + ",',',' & RTRIM([fid]) & ',') > 0)";


                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams2);

                DbParameter[] prams3 = {
											DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.VarChar,0,postinfo.Postdatetime),
											DbHelper.MakeInParam("@title",(DbType)OleDbType.VarChar,60,postinfo.Title),
											DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid),
				                        };
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET " +
                    "[lastpost] = @postdatetime," +
                    "[lastpostid] = " + postid + "," +
                    "[lastposttitle] = @title," +
                    "[posts] = [posts] + 1, " +
                    "[lastactivity] = now() " +
                    "WHERE [uid] = @posterid";
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams3);

                if (postinfo.Layer <= 0)
                {
                    DbParameter[] prams4 = {
												DbHelper.MakeInParam("@poster",(DbType)OleDbType.VarChar,15,postinfo.Poster),
												DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.VarChar,0,postinfo.Postdatetime),
												DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid),
												DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
					                        };
                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies]=0,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid";
                    DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams4);
                }
                else
                {
                    DbParameter[] prams5 = {
												DbHelper.MakeInParam("@poster",(DbType)OleDbType.VarChar,15,postinfo.Poster),
												DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.VarChar,0,postinfo.Postdatetime),
												DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid),
												DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
						                    };
                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies]=[replies] + 1,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid";
                    DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams5);

                }
            }

            DbParameter[] prams6 = {
                                        DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
					                };
            strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpostid]=" + postid + " WHERE [tid]=@tid";
            DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams6);
            if (postinfo.Posterid != -1)
            {
                DbParameter[] prams7 = {
			                                DbHelper.MakeInParam("@posterid",(DbType)OleDbType.Integer,4,postinfo.Posterid),
					                        DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postinfo.Tid),
                                            DbHelper.MakeInParam("@postdatetime",(DbType)OleDbType.VarChar,0,postinfo.Postdatetime),
                                        };
                DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT into [" + BaseConfigs.GetTablePrefix + "myposts]([uid], [tid], [pid], [dateline]) VALUES(@posterid, @tid, " + postid + ", @postdatetime)", prams7);
            }

            #endregion
            return postid;
        }

        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="__postsInfo">帖子信息</param>
        /// <returns>更新数量</returns>
        public int UpdatePost(PostInfo __postsInfo, string posttableid)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,__postsInfo.Pid),
            //                           DbHelper.MakeInParam("@title",(DbType)OleDbType.VarWChar,160,__postsInfo.Title),
            //                           DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar,0,__postsInfo.Message),
            //                           DbHelper.MakeInParam("@lastedit",(DbType)OleDbType.VarChar,50,__postsInfo.Lastedit),
            //                           DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,4,__postsInfo.Invisible),
            //                           DbHelper.MakeInParam("@usesig",(DbType)OleDbType.Integer,4,__postsInfo.Usesig),
            //                           DbHelper.MakeInParam("@htmlon",(DbType)OleDbType.Integer,4,__postsInfo.Htmlon),
            //                           DbHelper.MakeInParam("@smileyoff",(DbType)OleDbType.Integer,4,__postsInfo.Smileyoff),
            //                           DbHelper.MakeInParam("@bbcodeoff",(DbType)OleDbType.Integer,4,__postsInfo.Bbcodeoff),
            //                           DbHelper.MakeInParam("@parseurloff",(DbType)OleDbType.Integer,4,__postsInfo.Parseurloff),
            //                       };
            //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatepost" + posttableid, parms);
            DbParameter[] prams = {
	                                   DbHelper.MakeInParam("@title",(DbType)OleDbType.VarWChar,160,__postsInfo.Title),
									   DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar,0,__postsInfo.Message),
									   DbHelper.MakeInParam("@lastedit",(DbType)OleDbType.VarChar,50,__postsInfo.Lastedit),
									   DbHelper.MakeInParam("@invisible",(DbType)OleDbType.Integer,4,__postsInfo.Invisible),
									   DbHelper.MakeInParam("@usesig",(DbType)OleDbType.Integer,4,__postsInfo.Usesig),
									   DbHelper.MakeInParam("@htmlon",(DbType)OleDbType.Integer,4,__postsInfo.Htmlon),
									   DbHelper.MakeInParam("@smileyoff",(DbType)OleDbType.Integer,4,__postsInfo.Smileyoff),
									   DbHelper.MakeInParam("@bbcodeoff",(DbType)OleDbType.Integer,4,__postsInfo.Bbcodeoff),
									   DbHelper.MakeInParam("@parseurloff",(DbType)OleDbType.Integer,4,__postsInfo.Parseurloff), 
                                       DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,__postsInfo.Pid)
								   };
            StringBuilder s = new StringBuilder();
            s.Append("UPDATE {0}posts{1} SET");
            s.Append("[title]=@title,");
            s.Append("[message]=@message,");
            s.Append("[lastedit]=@lastedit,");
            s.Append("[invisible]=@invisible,");
            s.Append("[usesig]=@usesig,");
            s.Append("[htmlon]=@htmlon,");
            s.Append("[smileyoff]=@smileyoff,");
            s.Append("[bbcodeoff]=@bbcodeoff,");
            s.Append("[parseurloff]=@parseurloff WHERE [pid]=@pid");
            //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatepost" + posttableid, prams);
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(s.ToString(), BaseConfigs.GetTablePrefix, posttableid), prams);
        }

        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="posttableid">当前分表ID</param>
        /// <param name="pid">帖子ID</param>
        /// <returns>删除数量</returns>
        public int DeletePost(string posttableid, int pid, bool chanageposts)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid),
            //                           DbHelper.MakeInParam("@chanageposts",(DbType)OleDbType.Integer,1,chanageposts)
            //                       };
            //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletepost{1}bypid", BaseConfigs.GetTablePrefix, posttableid), parms);
            //DbParameter[] prams = {
            //		   DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid)
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

            strSQL = "SELECT [fid], [tid], [posterid],[layer], [postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE pid =" + pid;
            DataRow dr = DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0];
            fid = Convert.ToInt32(dr["fid"].ToString());
            tid = Convert.ToInt32(dr["tid"].ToString());
            posterid = Convert.ToInt32(dr["posterid"].ToString());
            layer = Convert.ToInt32(dr["layer"].ToString());
            postdatetime = Convert.ToDateTime(dr["postdatetime"].ToString());

            strSQL = "SELECT iif(([parentidlist] is null),'',[parentidlist]) as [fidlist] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] =" + fid;
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
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] - 1";
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                //	--更新版块内总的回帖数

                if (Convert.ToDateTime(postdatetime).ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET 	[posts]=[posts] - 1, [todayposts]=[todayposts]-1 WHERE [fid] in (" + fidlist + ")";
                }
                else
                {
                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET 	[posts]=[posts] - 1  WHERE [fid] in (" + fidlist + ")";
                }
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                //--更新用户总的回帖数
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET  [posts] = [posts]-1 WHERE [uid] =" + posterid;
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                //--更新主题总的回帖数
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies]=[replies] - 1 WHERE [tid]=" + tid;
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                //--删除帖子
                strSQL = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [pid]=" + pid;
                postcount = DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

            }
            else
            {
                //--删除主题

                strSQL = "SELECT COUNT([pid]) FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] = " + tid;
                postcount = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0][0].ToString());

                strSQL = "SELECT COUNT([pid]) FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] =" + tid + "  AND DATEDIFF(\"d\", [postdatetime], now()) = 0";
                todaycount = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0][0].ToString());

                //--更新主题及帖子总数
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic] - 1, [totalpost]=[totalpost] -" + postcount;
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                //--更新版块
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [posts]=[posts] -" + postcount + ", [topics]=[topics] - 1,[todayposts]=[todayposts] -" + todaycount + " WHERE [fid] in (" + fidlist + ")";
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                //--更新用户总的回帖数
                strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts] = [posts] - " + postcount + " WHERE [uid] = " + posterid;
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                strSQL = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] = " + tid;
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                strSQL = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] = " + tid;
                DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

            }

            if (layer != 0)
            {
                strSQL = "SELECT TOP 1 [pid], [posterid],[postdatetime], [title], [poster] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid]=" + tid + " ORDER BY [pid] DESC";
                dt = DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    pid = Convert.ToInt32(dr["pid"].ToString());
                    posterid = Convert.ToInt32(dr["posterid"].ToString());
                    postdatetime = Convert.ToDateTime(dr["postdatetime"].ToString());
                    title = dr["title"].ToString();
                    poster = dr["poster"].ToString();

                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposter]='" + poster + "',[lastpost]='" + postdatetime.ToString() + "',[lastpostid]=" + pid + ",[lastposterid]=" + posterid + " WHERE [tid]=" + tid;
                    DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);
                }

            }


            strSQL = "SELECT [lasttid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] =" + fid;
            lasttid = Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, strSQL).Tables[0].Rows[0][0].ToString());


            if (lasttid == tid)
            {

                strSQL = "SELECT TOP 1 [pid], [tid],[posterid], [title], [poster], [postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [fid] = " + fid + " ORDER BY [pid] DESC";
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


                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [lasttid]=" + tid + ",[lasttitle]='" + title + "',[lastpost]='" + postdatetime + "',[lastposter]='" + poster + "',[lastposterid]=" + lastforumposterid + " WHERE [fid] in (" + fidlist + ")";
                    DbHelper.ExecuteNonQuery(CommandType.Text, strSQL);

                    strSQL = "SELECT TOP 1 [pid], [tid], [posterid], [postdatetime], [title], [poster] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [posterid]=" + posterid + " ORDER BY [pid] DESC";
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
                    strSQL = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + postdatetime + "',[lastpostid] = " + pid + ",[lastposttitle] = '" + title + "'  WHERE [uid] = " + posterid;
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
        /// <param name="posttableid">当前分表ID</param>
        /// <param name="pid">帖子id</param>
        /// <returns>帖子描述信息</returns>
        public IDataReader GetPostInfo(string posttableid, int pid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer,4, pid),
			};
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}posts{1}] WHERE [pid]=@pid", BaseConfigs.GetTablePrefix, posttableid), parms);
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
                sb.Append("SELECT * FROM [");
                sb.Append(BaseConfigs.GetTablePrefix);
                sb.Append("posts");
                sb.Append(posttableid[i]);
                sb.Append("] WHERE [tid] IN (");
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
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,Tid)	
								   };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [pid], [title], [poster], [posterid],[message] FROM [" + posttablename + "] WHERE [tid]=@tid ORDER BY [pid]", parms).Tables[0];

        }

        /// <summary>
        /// 获取指定条件的帖子DataReader
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataReader</returns>
        public IDataReader GetPostListByCondition(PostpramsInfo _postpramsinfo, string posttablename)
        {
            //DbParameter[] parms = {
            //                               DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,_postpramsinfo.Tid),
            //                               DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,_postpramsinfo.Pagesize),
            //                               DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,_postpramsinfo.Pageindex),
            //                               DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarWChar,100,_postpramsinfo.Condition),
            //                               DbHelper.MakeInParam("@posttablename",(DbType)OleDbType.VarChar,20,posttablename)
            //                           };
            //return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpostlistbycondition", parms);
            IDataReader reader;
            DbParameter[] prams = {
										   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,_postpramsinfo.Tid),
                                           //DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,_postpramsinfo.Pagesize),
                                           //DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,_postpramsinfo.Pageindex),
										   DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarWChar,100,_postpramsinfo.Condition),
										   DbHelper.MakeInParam("@posttablename",(DbType)OleDbType.VarChar,20,posttablename)
									   };

            DbParameter[] prams1 = {
										   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,_postpramsinfo.Tid)
									   };

            //string sql1 = "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], "
            //    + "[filesize], [attachment], [downloads] FROM [dnt_attachments] WHERE [tid]=" + _postpramsinfo.Tid;



            //pp = DbHelper.ExecuteReader(CommandType.Text, sql1, prams1);

            string sql = "";
            int pagetop = (_postpramsinfo.Pageindex - 1) * _postpramsinfo.Pagesize;
            if (_postpramsinfo.Pageindex == 1)
            {
                sql = "SELECT TOP " + _postpramsinfo.Pagesize + " " +
                                   "[" + posttablename + "].[pid]," +
                                   "[" + posttablename + "].[fid]," +
                                   "[" + posttablename + "].[title]," +
                                   "[" + posttablename + "].[layer]," +
                                   "[" + posttablename + "].[message]," +
                                   "[" + posttablename + "].[ip]," +
                                   "[" + posttablename + "].[lastedit]," +
                                   "[" + posttablename + "].[postdatetime]," +
                                   "[" + posttablename + "].[attachment]," +
                                   "[" + posttablename + "].[poster]," +
                                   "[" + posttablename + "].[posterid]," +
                                   "[" + posttablename + "].[invisible]," +
                                   "[" + posttablename + "].[usesig]," +
                                   "[" + posttablename + "].[htmlon]," +
                                   "[" + posttablename + "].[smileyoff]," +
                                   "[" + posttablename + "].[parseurloff]," +
                                   "[" + posttablename + "].[bbcodeoff]," +
                                   "[" + posttablename + "].[rate]," +
                                   "[" + posttablename + "].[ratetimes]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[nickname]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[username]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[groupid]," +
                                    "[" + BaseConfigs.GetTablePrefix + "users].[spaceid]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[email]," +
                                    "[" + BaseConfigs.GetTablePrefix + "users].[gender]," +
                                       "[" + BaseConfigs.GetTablePrefix + "users].[bday]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[showemail]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[digestposts]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[credits]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits1]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits2]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits3]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits4]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits5]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits6]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits7]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[extcredits8]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[posts]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[joindate]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[onlinestate]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[lastactivity]," +
                                   "[" + BaseConfigs.GetTablePrefix + "users].[invisible] AS [userinvisible]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[avatar]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[medals]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS signature," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[location]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[customstatus]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[website]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[icq]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[qq]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[msn]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[yahoo]," +
                                   "[" + BaseConfigs.GetTablePrefix + "userfields].[skype] " +
    "FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=" + _postpramsinfo.Tid + " AND [" + posttablename + "].[invisible]=0 AND " + _postpramsinfo.Condition + " ORDER BY [" + posttablename + "].[pid]";
            }
            else
            {

                sql = "SELECT TOP " + _postpramsinfo.Pagesize + " " +
                                         "[" + posttablename + "].[pid]," +
                                         "[" + posttablename + "].[fid]," +
                                         "[" + posttablename + "].[title]," +
                                         "[" + posttablename + "].[layer]," +
                                         "[" + posttablename + "].[message]," +
                                         "[" + posttablename + "].[ip]," +
                                         "[" + posttablename + "].[lastedit]," +
                                         "[" + posttablename + "].[postdatetime]," +
                                         "[" + posttablename + "].[attachment]," +
                                         "[" + posttablename + "].[poster]," +
                                         "[" + posttablename + "].[posterid]," +
                                         "[" + posttablename + "].[invisible]," +
                                         "[" + posttablename + "].[usesig]," +
                                         "[" + posttablename + "].[htmlon]," +
                                         "[" + posttablename + "].[smileyoff]," +
                                         "[" + posttablename + "].[parseurloff]," +
                                         "[" + posttablename + "].[bbcodeoff]," +
                                         "[" + posttablename + "].[rate]," +
                                         "[" + posttablename + "].[ratetimes]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[nickname]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[username]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[groupid]," +
                                          "[" + BaseConfigs.GetTablePrefix + "users].[spaceid]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[email]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[showemail]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[digestposts]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[credits]," +
                                            "[" + BaseConfigs.GetTablePrefix + "users].[gender]," +
                                                  "[" + BaseConfigs.GetTablePrefix + "users].[bday]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits1]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits2]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits3]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits4]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits5]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits6]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits7]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[extcredits8]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[posts]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[joindate]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[onlinestate]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[lastactivity]," +
                                         "[" + BaseConfigs.GetTablePrefix + "users].[invisible] AS [userinvisible]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[avatar]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[medals]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS signature," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[location]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[customstatus]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[website]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[icq]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[qq]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[msn]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[yahoo]," +
                                         "[" + BaseConfigs.GetTablePrefix + "userfields].[skype] " +
         "FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=" + _postpramsinfo.Tid + " AND [" + posttablename + "].[invisible]=0 AND " + _postpramsinfo.Condition + "  AND [pid] > (SELECT MAX([pid]) FROM (SELECT TOP  " + _postpramsinfo.Pagesize + " [" + posttablename + "].[pid] FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=" + _postpramsinfo.Tid + "  AND " + _postpramsinfo.Condition + "  ORDER BY [" + posttablename + "].[pid]) AS tblTmp) ORDER BY [" + posttablename + "].[pid]";


            }

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
            //DbParameter[] parms = {
            //                               DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,_postpramsinfo.Tid),
            //                               DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,_postpramsinfo.Pagesize),
            //                               DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,_postpramsinfo.Pageindex),
            //                               DbHelper.MakeInParam("@posttablename",(DbType)OleDbType.VarChar,20,posttablename)
            //                           };
            //return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getpostlist", BaseConfigs.GetTablePrefix), parms);
            string sql;
            DbParameter[] prams1 = {
										   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,_postpramsinfo.Tid)
									   };

            //string sql1 = "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], "
            //    + "[filesize], [attachment], [downloads] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=" + _postpramsinfo.Tid;


            //pp = DbHelper.ExecuteReader(CommandType.Text, sql1, prams1);

            DbParameter[] prams = {           
                                    DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,_postpramsinfo.Pagesize),
                                    DbHelper.MakeInParam("@posttablename",(DbType)OleDbType.VarChar,20,posttablename),  
									DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,_postpramsinfo.Pageindex), 
                                    DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,_postpramsinfo.Tid)
								    };

            #region 挺长的一段话
            if (_postpramsinfo.Pageindex == 1)
            {

                sql = "SELECT TOP  " + _postpramsinfo.Pagesize + " " +
                                              "[" + posttablename + "].[pid],[" + posttablename + "].[fid]," +
                                              "[" + posttablename + "].[title],[" + posttablename + "].[layer]," +
                                              "[" + posttablename + "].[message],[" + posttablename + "].[ip], " +
                                              "[" + posttablename + "].[lastedit],[" + posttablename + "].[postdatetime]," +
                                              "[" + posttablename + "].[attachment],[" + posttablename + "].[poster]," +
                                              "[" + posttablename + "].[posterid],[" + posttablename + "].[invisible]," +
                                              "[" + posttablename + "].[usesig], " +
                                              "[" + posttablename + "].[htmlon],[" + posttablename + "].[smileyoff]," +
                                              "[" + posttablename + "].[parseurloff],[" + posttablename + "].[bbcodeoff]," +
                                              "[" + posttablename + "].[rate],[" + posttablename + "].[ratetimes], " +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[nickname]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[gender]," +
                                       "[" + BaseConfigs.GetTablePrefix + "users].[bday]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[username]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[groupid]," +
                                               "[" + BaseConfigs.GetTablePrefix + "users].[spaceid]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[email]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[showemail]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[digestposts]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[credits]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits1]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits2]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits3]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits4]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits5]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits6]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits7]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits8]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[posts], " +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[joindate]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[onlinestate]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[lastactivity]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[invisible] AS [userinvisible]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[avatar]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[medals]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS signature," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[location]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[customstatus]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[website]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[icq]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[qq]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[msn]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[yahoo]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[skype] " +
               "FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=" + _postpramsinfo.Tid + " AND [" + posttablename + "].[invisible]=0 ORDER BY [" + posttablename + "].[pid]";
            }
            else
            {

                sql = "SELECT TOP " + _postpramsinfo.Pagesize + " " +
                                              "[" + posttablename + "].[pid]," +
                                              "[" + posttablename + "].[fid],[" + posttablename + "].[title],[" + posttablename + "].[layer],[" + posttablename + "].[message]," +
                                              "[" + posttablename + "].[ip],[" + posttablename + "].[lastedit], [" + posttablename + "].[postdatetime],[" + posttablename + "].[attachment]," +
                                              "[" + posttablename + "].[poster],[" + posttablename + "].[posterid],[" + posttablename + "].[invisible], [" + posttablename + "].[usesig]," +
                                              "[" + posttablename + "].[htmlon],[" + posttablename + "].[smileyoff], [" + posttablename + "].[parseurloff]," +
                                              "[" + posttablename + "].[bbcodeoff], [" + posttablename + "].[rate], [" + posttablename + "].[ratetimes]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[gender]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[bday]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[nickname],[" + BaseConfigs.GetTablePrefix + "users].[username], [" + BaseConfigs.GetTablePrefix + "users].[groupid], [" + BaseConfigs.GetTablePrefix + "users].[email], [" + BaseConfigs.GetTablePrefix + "users].[spaceid]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[showemail], [" + BaseConfigs.GetTablePrefix + "users].[digestposts], [" + BaseConfigs.GetTablePrefix + "users].[credits]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits1], [" + BaseConfigs.GetTablePrefix + "users].[extcredits2], [" + BaseConfigs.GetTablePrefix + "users].[extcredits3]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits4], [" + BaseConfigs.GetTablePrefix + "users].[extcredits5], [" + BaseConfigs.GetTablePrefix + "users].[extcredits6]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[extcredits7],[" + BaseConfigs.GetTablePrefix + "users].[extcredits8], [" + BaseConfigs.GetTablePrefix + "users].[posts],[" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[onlinestate]," +
                                              "[" + BaseConfigs.GetTablePrefix + "users].[lastactivity],  [" + BaseConfigs.GetTablePrefix + "users].[invisible] AS [userinvisible], [" + BaseConfigs.GetTablePrefix + "userfields].[avatar]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth], [" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight], [" + BaseConfigs.GetTablePrefix + "userfields].[medals],[" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS [signature]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[location], [" + BaseConfigs.GetTablePrefix + "userfields].[customstatus], [" + BaseConfigs.GetTablePrefix + "userfields].[website], [" + BaseConfigs.GetTablePrefix + "userfields].[icq]," +
                                              "[" + BaseConfigs.GetTablePrefix + "userfields].[qq], [" + BaseConfigs.GetTablePrefix + "userfields].[msn], [" + BaseConfigs.GetTablePrefix + "userfields].[yahoo], [" + BaseConfigs.GetTablePrefix + "userfields].[skype] " +
              "FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=" + _postpramsinfo.Tid + " AND [pid] > (SELECT MAX([pid]) FROM (SELECT TOP " + (_postpramsinfo.Pageindex - 1) * _postpramsinfo.Pagesize + "  [" + posttablename + "].[pid] FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=" + _postpramsinfo.Tid + " AND [" + posttablename + "].[invisible]=0 ORDER BY [" + posttablename + "].[pid]) AS tblTmp) ORDER BY [" + posttablename + "].[pid]";

            }
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
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
								   };
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM {0} WHERE [tid] = @tid ORDER BY [pid] DESC", posttablename), parms);

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
        public DataTable GetLastPostList(PostpramsInfo postpramsinfo, string posttablename)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postpramsinfo.Tid),
            //                           DbHelper.MakeInParam("@postnum",(DbType)OleDbType.Integer,4,postpramsinfo.Pagesize),
            //                           DbHelper.MakeInParam("@posttablename",(DbType)OleDbType.VarChar,20,posttablename)
            //                       };
            //DataTable dt = DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getlastpostlist", BaseConfigs.GetTablePrefix), parms).Tables[0];

            //return dt;
            DbParameter[] prams = {
                                       DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postpramsinfo.Tid)
								   };
            string sql = "SELECT TOP " + postpramsinfo.Pagesize + " [" + posttablename + "].[pid], [" + posttablename + "].[fid], [" + posttablename + "].[layer], [" + posttablename + "].[posterid], [" + posttablename + "].[title], [" + posttablename + "].[message], [" + posttablename + "].[postdatetime], [" + posttablename + "].[attachment], [" + posttablename + "].[poster], [" + posttablename + "].[posterid], [" + posttablename + "].[invisible], [" + posttablename + "].[usesig], [" + posttablename + "].[htmlon], [" + posttablename + "].[smileyoff], [" + posttablename + "].[parseurloff], [" + posttablename + "].[bbcodeoff], [" + posttablename + "].[rate], [" + posttablename + "].[ratetimes], [" + BaseConfigs.GetTablePrefix + "users].[username], [" + BaseConfigs.GetTablePrefix + "users].[email], [" + BaseConfigs.GetTablePrefix + "users].[showemail], [" + BaseConfigs.GetTablePrefix + "userfields].[avatar], [" + BaseConfigs.GetTablePrefix + "userfields].[avatarwidth], [" + BaseConfigs.GetTablePrefix + "userfields].[avatarheight], [" + BaseConfigs.GetTablePrefix + "userfields].[sightml] AS signature, [" + BaseConfigs.GetTablePrefix + "userfields].[location], [" + BaseConfigs.GetTablePrefix + "userfields].[customstatus] FROM ([" + posttablename + "] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + posttablename + "].[posterid]) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid] WHERE [" + posttablename + "].[tid]=@tid ORDER BY [" + posttablename + "].[pid] DESC";

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
            return dt;
        }

        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        public IDataReader GetSinglePost(out IDataReader attachments, PostpramsInfo postpramsinfo, string posttableid)
        {
            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,postpramsinfo.Pid),
            //                            DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postpramsinfo.Tid)
            //                        };
            //attachments = null;
            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getsinglepost{1}", BaseConfigs.GetTablePrefix, posttableid), parms);

            //return reader;
            DbParameter[] prams = {
										DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,postpramsinfo.Pid),
										DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,postpramsinfo.Tid)
									};


            attachments = DbHelper.ExecuteReader(CommandType.Text, "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads],[attachprice],[uid] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=@tid", prams);


            string sql = @"SELECT TOP 1 
                            [{0}posts{1}].[pid], 
                            [{0}posts{1}].[fid], 
                            [{0}posts{1}].[layer],
                            [{0}posts{1}].[message], 
[{0}posts{1}].[title], 
                            [{0}posts{1}].[ip], 
                            [{0}posts{1}].[lastedit], 
                            [{0}posts{1}].[postdatetime], 
                             [{0}posts{1}].[attachment], 
                            [{0}posts{1}].[poster], 
                             [{0}posts{1}].[invisible], 
                          [{0}posts{1}].[usesig], 
                            [{0}posts{1}].[htmlon], 
                            [{0}posts{1}].[smileyoff], 
                            [{0}posts{1}].[parseurloff], 
                            [{0}posts{1}].[bbcodeoff], 
                            [{0}posts{1}].[rate], 
                            [{0}posts{1}].[ratetimes], 
                            [{0}posts{1}].[posterid], 
                            [{0}users].[nickname],  
                            [{0}users].[username], 
                            [{0}users].[groupid], 
                             [{0}users].[spaceid], 
                            [{0}users].[email], 
                            [{0}users].[showemail], 
                            [{0}users].[digestposts], 
                            [{0}users].[credits], 
                            [{0}users].[extcredits1], 
                            [{0}users].[extcredits2], 
                            [{0}users].[extcredits3], 
                            [{0}users].[extcredits4], 
                            [{0}users].[extcredits5], 
                            [{0}users].[extcredits6], 
                            [{0}users].[extcredits7], 
                            [{0}users].[extcredits8], 
                            [{0}users].[posts],
 [{0}users].[gender], 
                            [{0}users].[bday],
                            [{0}users].[joindate], 
                            [{0}users].[onlinestate], 
                            [{0}users].[lastactivity], 
                            [{0}users].[invisible] as [invisible], 
                            [{0}userfields].[avatar], 
                            [{0}userfields].[avatarwidth], 
                            [{0}userfields].[avatarheight], 
                            [{0}userfields].[medals], 
                            [{0}userfields].[sightml] AS signature, 
                            [{0}userfields].[location], 
                            [{0}userfields].[customstatus], 
                            [{0}userfields].[website], 
                            [{0}userfields].[icq], 
                            [{0}userfields].[qq], 
                            [{0}userfields].[msn], 
                            [{0}userfields].[yahoo], 
                            [{0}userfields].[skype] 
            FROM ([" + BaseConfigs.GetTablePrefix + "posts{1}] LEFT JOIN [{0}users] ON [{0}users].[uid]=[{0}posts{1}].[posterid]) LEFT JOIN [{0}userfields] ON [{0}userfields].[uid]=[{0}users].[uid] WHERE [{0}posts{1}].[pid]=" + postpramsinfo.Pid + "";



            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, string.Format(sql, BaseConfigs.GetTablePrefix, posttableid));

            return reader;
        }

        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        public IDataReader GetSinglePost(int tid, int posttableid)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
									};
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}POSTS{1} WHERE [TID]={2} AND [LAYER]=0", BaseConfigs.GetTablePrefix, posttableid, tid), parms);

            return reader;
        }

        public DataTable GetPostTree(int tid, string posttableid)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
            //                       };
            //return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getpost{1}tree", BaseConfigs.GetTablePrefix, posttableid), parms).Tables[0];
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            String sql = "SELECT [pid], [layer], [title], [poster], [posterid],[postdatetime],[message] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid]=@tid AND [invisible]=0 ORDER BY [parentid]";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];

        }

        /// <summary>
        /// 按条件获取指定tid的帖子总数
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <returns>指定tid的帖子总数</returns>
        public int GetPostCount(int tid, string condition, string posttableid)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarWChar,100,condition),
            //                           DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid))
            //                       };
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getpostcountbycondition", BaseConfigs.GetTablePrefix), parms), 0);
            DbParameter[] prams = {
                                       DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid)),
                                       DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarWChar,100,condition)
								   };

            String sql = "SELECT COUNT(pid) FROM [@posttablename] WHERE @condition AND [layer]>=0";
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
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
            //                       };
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getfirstpost{1}id", BaseConfigs.GetTablePrefix, posttableid), parms), -1);
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)
								   };
            string sql = "SELECT TOP 1 [pid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid]=" + tid + " ORDER BY [pid]";

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), -1);

            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getfirstpost{1}id", BaseConfigs.GetTablePrefix, posttableid), prams), -1);

        }

        /// <summary>
        /// 判断指定用户是否是指定主题的回复者
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>是否是指定主题的回复者</returns>
        public bool IsReplier(int tid, int uid, string posttableid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid),
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,uid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT([pid]) AS [pidcount] FROM [{0}posts{1}] WHERE [tid] = @tid AND [posterid]=@uid AND @uid>0", BaseConfigs.GetTablePrefix, posttableid), parms), 0) > 0;
        }

        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postidlist">帖子ID列表</param>
        /// <returns>更新的帖子数量</returns>
        public int UpdatePostRateTimes(int ratetimes, string postidlist, string posttableid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [ratetimes] = [ratetimes] + {3} WHERE [pid] IN ({2})", BaseConfigs.GetTablePrefix, posttableid, postidlist, ratetimes));
        }

        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="postidlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        public int UpdatePostRate(int pid, float rate, string posttableid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [rate] = [rate] + {2} WHERE [pid] IN ({3})", BaseConfigs.GetTablePrefix, posttableid, rate, pid));
        }

        /// <summary>
        /// 撤消帖子的评分值
        /// </summary>
        /// <param name="postidlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        public int CancelPostRate(string postidlist, string posttableid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [rate] = 0, [ratetimes]=0 WHERE [pid] IN ({2})", BaseConfigs.GetTablePrefix, posttableid, postidlist));
        }

        /// <summary>
        /// 获取帖子评分列表
        /// </summary>
        /// <param name="pid">帖子列表</param>
        /// <returns></returns>
        public DataTable GetPostRateList(int pid, int displayRateCount)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid",(DbType)OleDbType.Integer,4,pid)
								   };
            string commandText = string.Format("SELECT TOP {0} * FROM [{1}ratelog] WHERE [pid]=@pid ORDER BY [id] DESC", displayRateCount, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 获取新主题
        /// </summary>
        /// <param name="forumidlist">不允许游客访问的版块id列表</param>
        /// <returns></returns>
        public IDataReader GetNewTopics(string forumidlist)
        {
            //DbParameter parm = DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 500, forumidlist);

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getnewtopics", parm);
            //return reader;
            DbParameter parm = DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 500, forumidlist);

            String sql = "";
            string currenttable = Posts.GetPostTableName();
            if (forumidlist != "")
            {
                sql = "SELECT TOP 20 [" + currenttable + "].[tid], [" + currenttable + "].[title], [" + currenttable + "].[poster], [" + currenttable + "].[postdatetime], [" + currenttable + "].[message],[" + BaseConfigs.GetTablePrefix + "forums].[name] FROM [" + currenttable + "]  LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forums] ON [" + currenttable + "].[fid]=[" + BaseConfigs.GetTablePrefix + "forums].[fid] WHERE  [" + BaseConfigs.GetTablePrefix + "forums].[fid] NOT IN (@fidlist)  AND [" + currenttable + "].[layer]=0 ORDER BY [" + currenttable + "].[pid] DESC";
            }
            else
            {
                sql = "SELECT TOP 20   [" + currenttable + "].[tid], [" + currenttable + "].[title], [" + currenttable + "].[poster], [" + currenttable + "].[postdatetime], [" + currenttable + "].[message],[" + BaseConfigs.GetTablePrefix + "forums].[name] FROM [" + currenttable + "]  LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forums] ON [" + currenttable + "].[fid]=[" + BaseConfigs.GetTablePrefix + "forums].[fid] WHERE [" + currenttable + "].[layer]=0 ORDER BY [" + currenttable + "].[pid] DESC";
            }
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, parm);
            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getnewtopics", parm);
            return reader;
        }

        public IDataReader GetSitemapNewTopics(string forumidlist)
        {
            //DbParameter parm = DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 500, forumidlist);

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getsitemapnewtopics", parm);
            //return reader;
            DbParameter parm = DbHelper.MakeInParam("@fidlist", (DbType)OleDbType.VarChar, 500, forumidlist);
            string sql = null;
            if (forumidlist != "")
            {
                sql = "SELECT TOP 20 [tid], [fid], [title], [poster], [postdatetime], [lastpost], [replies], [views], [digest] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid] NOT IN (@fidlist) ORDER BY [tid] DESC";

            }
            else
            {
                sql = "SELECT TOP 20 [tid], [fid], [title], [poster], [postdatetime], [lastpost], [replies], [views], [digest] FROM [" + BaseConfigs.GetTablePrefix + "topics] ORDER BY [tid] DESC";

            }
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, parm);
            return reader;
        }

        /// <summary>
        /// 获取版块新主题
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns></returns>
        public IDataReader GetForumNewTopics(int fid)
        {
            //DbParameter[] parms = {
            //                               DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid)

            //                           };
            //return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getforumnewtopics", parms);
            DbParameter[] prams = {
										   DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid)
									   
									   };

            String sql = "";
            sql = "SELECT TOP 20 [" + BaseConfigs.GetTablePrefix + "topics].[tid],[" + BaseConfigs.GetTablePrefix + "topics].[title],[" + BaseConfigs.GetTablePrefix + "topics].[poster],[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime],[" + BaseConfigs.GetTablePrefix + "posts1].[message] FROM [" + BaseConfigs.GetTablePrefix + "topics] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "posts1] ON [" + BaseConfigs.GetTablePrefix + "topics].[tid]=[" + BaseConfigs.GetTablePrefix + "posts1].[tid]  WHERE [" + BaseConfigs.GetTablePrefix + "posts1].[layer]=0 AND  [" + BaseConfigs.GetTablePrefix + "topics].[fid]=@fid ORDER BY [lastpost] DESC";

            //return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getforumnewtopics", prams);
            return DbHelper.ExecuteReader(CommandType.Text, sql, prams);
        }


        /// <summary>
        /// 创建搜索缓存
        /// </summary>
        /// <param name="cacheinfo">搜索缓存信息</param>
        /// <returns>搜索缓存id</returns>
        public int CreateSearchCache(SearchCacheInfo cacheinfo)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@keywords",(DbType)OleDbType.VarWChar,255,cacheinfo.Keywords),
            //                           DbHelper.MakeInParam("@searchstring",(DbType)OleDbType.VarWChar,255,cacheinfo.Searchstring),
            //                           DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,cacheinfo.Ip),
            //                           DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,cacheinfo.Uid),
            //                           DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,4,cacheinfo.Groupid),
            //                           DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(cacheinfo.Postdatetime)),
            //                           DbHelper.MakeInParam("@expiration",(DbType)OleDbType.VarChar,19,cacheinfo.Expiration),
            //                           DbHelper.MakeInParam("@topics",(DbType)OleDbType.Integer,4,cacheinfo.Topics),
            //                           DbHelper.MakeInParam("@tids",(DbType)SqlDbType.Text,0,cacheinfo.Tids)
            //                       };
            //return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createsearchcache", BaseConfigs.GetTablePrefix), parms).ToString(), -1);
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@keywords",(DbType)OleDbType.VarWChar,255,cacheinfo.Keywords),
									   DbHelper.MakeInParam("@searchstring",(DbType)OleDbType.VarWChar,255,cacheinfo.Searchstring),
									   DbHelper.MakeInParam("@ip",(DbType)OleDbType.VarChar,15,cacheinfo.Ip),
									   DbHelper.MakeInParam("@uid",(DbType)OleDbType.Integer,4,cacheinfo.Uid),
									   DbHelper.MakeInParam("@groupid",(DbType)OleDbType.Integer,4,cacheinfo.Groupid),
									   DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(cacheinfo.Postdatetime)),
									   DbHelper.MakeInParam("@expiration",(DbType)OleDbType.VarChar,19,cacheinfo.Expiration),
									   DbHelper.MakeInParam("@topics",(DbType)OleDbType.Integer,4,cacheinfo.Topics),
									   DbHelper.MakeInParam("@tids",(DbType)OleDbType.Char,0,cacheinfo.Tids)
								   };


            string strSQL = "";
            int rtnValue = 0;


            //OleDbConnection con = new OleDbConnection(BaseConfigFactory.GetDBConnectString);
            strSQL = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "searchcaches] " +
                "([keywords],[searchstring],[ip],[uid],[groupid],[postdatetime],[expiration],[topics],[tids]) " +
                "VALUES " +
                "(@keywords,@searchstring,@ip,@uid,@groupid,@postdatetime,@expiration,@topics,@tids)";

            //sql = "select  @@Identity";
            // System.Object lockThis = new System.Object();

            DbHelper.ExecuteNonQuery(CommandType.Text, strSQL, prams);

            rtnValue = Int32.Parse(DbHelper.ExecuteDataset(CommandType.Text, "select top 1 searchid from [" + BaseConfigs.GetTablePrefix + "searchcaches] order by searchid desc").Tables[0].Rows[0][0].ToString());

            //  DbHelper.ExecuteNonQuery(con, CommandType.Text, out rtnValue, strSQL, prams);
            return rtnValue;
        }

        /// <summary>
        /// 删除超过３０分钟的缓存记录
        /// </summary>
        public void DeleteExpriedSearchCache()
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@expiration",(DbType)OleDbType.DBTimeStamp,8,DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss"))
								   };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"DELETE FROM [{0}searchcaches] WHERE [expiration]<@expiration", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获得搜索缓存
        /// </summary>
        /// <returns></returns>
        public DataTable GetSearchCache(int searchid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@searchid",(DbType)OleDbType.Integer,4,searchid)
			};
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 [tids] FROM [{0}searchcaches] WHERE [searchid]=@searchid", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 获得搜索的精华贴
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <returns></returns>
        public DataTable GetSearchDigestTopicsList(int pagesize, string strTids)
        {
            string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter], [{0}topics].[lastposterid], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, strTids);
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
            string commandText = string.Format("SELECT TOP {1} [{2}].[tid], [{2}].[title], [{2}].[poster], [{2}].[posterid],[lastposterid], [{2}].[postdatetime],[{2}].[lastedit], [{2}].[rate], [{2}].[ratetimes], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{2}] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{2}].[fid] WHERE [{2}].[pid] IN({3}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{2}].[tid]),'{3}')", BaseConfigs.GetTablePrefix, pagesize, postablename, strTids);
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
            string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].tid in({2})", BaseConfigs.GetTablePrefix, pagesize, strTids); 
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 开启全文索引
        /// </summary>
        public void ConfirmFullTextEnable()
        {
            //string commandText = "IF(SELECT DATABASEPROPERTY(DB_NAME(), 'IsFullTextEnabled'))=0 EXEC sp_fulltext_database 'enable'";
            //DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
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
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)OleDbType.Integer, 1, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), parms);
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
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)OleDbType.Integer, 1, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), parms);
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
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)OleDbType.VarChar, 500, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), parms);
        }

        public DataSet GetTopTopicList(int fid)
        {
            DbParameter[] parms = {
					DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 4, fid)
				};
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [tid],[displayorder],[fid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder] > 0 ORDER BY [fid]", parms);

        }


        public DataTable GetShortForums()
        {
            DataTable topTable = DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[parentid],[parentidlist], [layer], '' AS [temptidlist],'' AS [tid2list], '' AS [tidlist],0 AS [tidcount],0 AS [tid2count],0 AS [tid3count] FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [fid] DESC").Tables[0];
            // SELECT [fid],[parentid],[parentidlist], [layer], CAST('' AS VARCHAR(1000)) AS [temptidlist],CAST('' AS VARCHAR(1000)) AS [tid2list], CAST('' AS VARCHAR(1000)) AS [tidlist],CAST(0 AS INT) AS [tidcount],CAST(0 AS INT) AS [tid2count],CAST(0 AS INT) AS [tid3count] FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [fid] DESC
            return topTable;
        }

        public IDataReader GetUserListWithTopicList(string topiclist, int losslessdel)
        {
            DbParameter[] prams =
						{
							DbHelper.MakeInParam("@Losslessdel", (DbType)OleDbType.Integer, 4, losslessdel)
						};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [posterid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE DATEDIFF(\"d\", [postdatetime], NOW())<@Losslessdel AND [tid] in (" + topiclist + ")", prams);
        }

        public IDataReader GetUserListWithTopicList(string topiclist)
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [posterid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN (" + topiclist + ")");
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicClose(string topiclist, short intValue)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)OleDbType.Integer, 1, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [closed] = @field WHERE [tid] IN (" + topiclist + ") AND [closed] IN (0,1)", parms);

        }

        /// <summary>
        /// 获得主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="field">要获得值的字段</param>
        /// <returns>主题指定字段的状态</returns>
        public int GetTopicStatus(string topiclist, string field)
        {
            return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "SELECT SUM(iif(([" + field + "] is null),0,[" + field + "])) AS [fieldcount] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN (" + topiclist + ")").Tables[0].Rows[0][0], 0);
        }

        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
        /// <param name="posttableid">所以分表的ID</param>
        /// <param name="chanageposts">删除帖时是否要减版块帖数</param>
        /// <returns></returns>
        public int DeleteTopicByTidList(string topiclist, string posttableid, bool chanageposts)
        {
            //DbParameter[] parms = {
            //        DbHelper.MakeInParam("@tidlist", (DbType)OleDbType.VarChar, 2000, topiclist),
            //        DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 2000, BaseConfigs.GetTablePrefix + "posts" + posttableid),
            //        DbHelper.MakeInParam("@chanageposts",(DbType)OleDbType.Integer,1,chanageposts)
            //    };

            //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "deletetopicbytidlist", parms);
            DbParameter[] prams = {
					DbHelper.MakeInParam("@tidlist", (DbType)OleDbType.VarChar, 2000, topiclist),
					DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 2000, BaseConfigs.GetTablePrefix + "posts" + posttableid),
                    DbHelper.MakeInParam("@chanageposts",(DbType)OleDbType.Boolean,1,chanageposts)
				};

            //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "deletetopicbytidlist", prams);

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
                sqlstr = "SELECT [fid],[posterid],[layer],[postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] IN (" + topiclist + ")";
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

                    if (("," + tempFid.ToString() + ",").IndexOf(fid + ",") == 0)
                    {
                        tempfidlist = (string)DbHelper.ExecuteScalar(CommandType.Text, "select iif(ISNULL([parentidlist]),'',[parentidlist]) FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] = " + tempFid);
                        if (tempfidlist == null)
                        {
                            tempfidlist = "";

                        }

                        if (tempfidlist != "")
                        {
                            fid = fid + "," + tempfidlist + "," + tempFid.ToString();
                        }
                        else
                        {
                            fid = fid + "," + tempFid.ToString();

                        }
                    }
                    if (@chanageposts)
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts] = [posts] - 1 WHERE [uid] =" + tempPosterid + "");
                    }

                }

                ddr.Close();
            }

            if (fid.Length > 0)
            {
                fid = fid.Substring(1, (fid.Length) - 1);

                if (@chanageposts)
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic]-" + topiccount + ", [totalpost]=[totalpost] -" + postcount + "");
                    sqlstr = " UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [posts]=[posts] -" + postcount + ",[topics]=[topics] -" + topiccount + ",[todayposts] = [todayposts] - " + todaycount + " WHERE [fid] IN (" + fid + ")";
                    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstr);
                }

                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid] IN (" + topiclist + ")");

                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid] IN (" + topiclist + ")");
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] IN (" + topiclist + ")");
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "mytopics] WHERE [tid] IN (" + topiclist + ")");
            }

            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [closed] IN (" + topiclist + ") OR [tid] IN (" + topiclist + ")");

            return 1;
        }

        public int DeleteClosedTopics(int fid, string topiclist)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + " AND [closed] IN (" + topiclist + ")");
        }

        public int CopyTopicLink(int oldfid, string topiclist)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 2, oldfid)
								   };

            ///用户设置转移后保留原连接执行以下三步操作

            ///1.向表中批量拷贝记录并将closed字段设置为原记录的tid*-1
            string sql = string.Format(@"INSERT INTO [{0}topics] (
					[fid],
					[iconid],
					[typeid],
					[readperm],
					[price],
					[poster],
					[posterid],
					[title],
					[postdatetime],
					[lastpost],
					[lastposter],
					[lastposterid],
					[views],
					[replies],
					[displayorder],
					[highlight],
					[digest],
					[rate],					
					[attachment],
					[moderated],
					[hide],
					[lastpostid],
					[magic],
					[closed]
					) SELECT @fid,
					[iconid],
					[typeid],
					[readperm],
					[price],
					[poster],
					[posterid],
					[title],
					[postdatetime],
					[lastpost],
					[lastposter],
					[lastposterid],
					[views],
					[replies],
					[displayorder],
					[highlight],
					[digest],
					[rate],
					[attachment],
					[moderated],
					[hide],
					[lastpostid],
					[magic],
					[tid] AS [closed] FROM [{0}topics] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, topiclist);

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        public void UpdatePost(string topiclist, int fid, string posttable)
        {
            DbParameter[] parms = {
				                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 1, fid)
			                        };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + posttable + "] SET [fid]=@fid WHERE [tid] IN (" + topiclist + ")", parms);
        }
        /// <summary>
        /// 更新主题所属版块,会将主题分类至为0
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public int UpdateTopic(string topiclist, int fid)
        {
            DbParameter[] parms = {
				                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 1, fid)
			                        };
            //更新主题
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [fid]=@fid, [typeid]=0 WHERE [tid] IN (" + topiclist + ")", parms);
        }


        public void UpdatePostTid(string postidlist, int tid, string posttableid)
        {
            DbParameter[] parms =
					{
						DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
					};

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [tid]=@tid WHERE [pid] IN (" + postidlist + ")", parms);
        }


        public void SetPrimaryPost(string subject, int tid, string[] postid, string posttableid)
        {
            DbParameter[] prams1 =
					{
						DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, Utils.StrToInt(postid[0], 0)),
						DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 80, subject)
					};

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [title] = @title, [parentid] = [pid],[layer] = 0 WHERE [pid] = @pid", prams1);
        }

        public void SetNewTopicProperty(int topicid, int Replies, int lastpostid, int lastposterid, string lastposter, DateTime lastpost)
        {
            DbParameter[] prams2 =
					{
						DbHelper.MakeInParam("@lastpostid", (DbType)OleDbType.Integer, 4, lastpostid),
						DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, lastposterid),
						DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.DBTimeStamp, 8, lastpost),
						DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 20, lastposter),
						DbHelper.MakeInParam("@replies", (DbType)OleDbType.Integer, 4, Replies),
                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, topicid)

					};
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpostid]=@lastpostid,[lastposterid] = @lastposterid, [lastpost] = @lastpost, [lastposter] = @lastposter, [replies] = @replies WHERE [tid] = @tid", prams2);
        }

        public int UpdatePostTidToAnotherTopic(int oldtid, int newtid, string posttableid)
        {
            DbParameter[] prams0 =
				{
					DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, newtid),
					DbHelper.MakeInParam("@oldtid", (DbType)OleDbType.Integer, 4, oldtid)
				};

            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [tid] = @tid, [layer] = CASE WHEN [layer] = 0 THEN 1 ELSE [layer] END WHERE [tid] = @oldtid", prams0);
        }

        public int UpdateAttachmentTidToAnotherTopic(int oldtid, int newtid)
        {
            DbParameter[] prams0 =
				{
					DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, newtid),
					DbHelper.MakeInParam("@oldtid", (DbType)OleDbType.Integer, 4, oldtid)
				};

            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "attachments] SET [tid]=@tid WHERE [tid]=@oldtid", prams0);
        }

        public int DeleteTopic(int tid)
        {
            DbParameter[] prams1 =
				{
					DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
				};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] = @tid", prams1);

        }

        public int UpdateTopic(int tid, TopicInfo __topicinfo)
        {
            DbParameter[] parms =
						{
							DbHelper.MakeInParam("@lastpostid", (DbType)OleDbType.Integer, 4, __topicinfo.Lastpostid),
							DbHelper.MakeInParam("@lastposterid", (DbType)OleDbType.Integer, 4, __topicinfo.Lastposterid),
							DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Parse(__topicinfo.Lastpost)),
							DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 20, __topicinfo.Lastposter),
							DbHelper.MakeInParam("@replies", (DbType)OleDbType.Integer, 4, __topicinfo.Replies),
                            DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
						};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpostid] = @lastpostid,[lastposterid] = @lastposterid, [lastpost] = @lastpost, [lastposter] = @lastposter, [replies] = [replies] + @replies WHERE [tid] = @tid", parms);
        }

        public int UpdateTopicReplies(int tid, int topicreplies)
        {
            DbParameter[] parms =
						{
							DbHelper.MakeInParam("@replies", (DbType)OleDbType.Integer, 4, topicreplies),
							DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
						};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies] = [replies] + @replies WHERE [tid] = @tid", parms);
        }

        public int RepairTopics(string topiclist, string posttable)
        {

            string commandtext = "";
            //string sql = "SELECT TOP 1 [" + posttable + "].[postdatetime],[" + posttable + "].[pid],[" + posttable + "].[poster],[" + posttable + "].[posterid]  FROM [" + posttable + "]  left join [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid] ORDER BY [" + posttable + "].[PID] DESC";
            string sql = "SELECT TOP 1 [" + posttable + "].[postdatetime],[" + posttable + "].[pid],[" + posttable + "].[poster],[" + posttable + "].[posterid]  FROM [" + posttable + "] WHERE  [" + posttable + "].[tid]=" + topiclist + "  ORDER BY [" + posttable + "].[PID] DESC";
            IDataReader IR = DbHelper.ExecuteReader(CommandType.Text, sql);

            if (IR.Read())
            {

                commandtext = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpost] ='" + IR["postdatetime"].ToString() + "' ,"
                            + "[lastpostid] =" + int.Parse(IR["pid"].ToString()) + ","
                            + " [lastposter] = '" + IR["poster"].ToString() + "',"
                            + "[lastposterid] = " + int.Parse(IR["posterid"].ToString()) + ","
                            + " [replies] = (SELECT COUNT([pid]) FROM [" + posttable + "] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid]) - 1 "
                            + " WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] IN (" + topiclist + ")";
            }
            IR.Close();
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandtext);
        }

        public IDataReader GetUserListWithPostList(string postlist, string posttableid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [posterid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [pid] in (" + postlist + ")");
        }

        public string CheckRateState(int userid, string pid)
        {
            DbParameter[] parms =
					{
						DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, Utils.StrToFloat(pid, 0)),
						DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userid)
					};
            return DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT [pid] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE [pid] = @pid AND [uid] = @uid", parms);
        }

        public IDataReader GetTopicListModeratorLog(int tid)
        {
            DbParameter[] parms =
					{
						DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
					};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [grouptitle], [moderatorname],[postdatetime],[actions] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE [tid] = @tid ORDER BY [id] DESC", parms);
        }

        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        public int ResetTopicTypes(int topictypeid, string topiclist)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@typeid", (DbType)OleDbType.Integer, 1, topictypeid),
									   DbHelper.MakeInParam("@topiclist", (DbType)OleDbType.VarWChar, 250, topiclist)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [typeid] = @typeid WHERE [tid] IN (" + topiclist + ")", parms);
        }

        /// <summary>
        /// 按照用户Id获取其回复过的主题总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTopicsCountbyReplyUserId(int userId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userId)
								   };

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1)  From (SELECT DISTINCT [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid] = @uid)", BaseConfigs.GetTablePrefix), parms), 0);
        }


        public IDataReader GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userId),
            //                           DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageIndex),
            //                           DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pageSize)
            //                       };
            String sql = "";
            if (pageIndex == 1)
            {
                //    sql = "SELECT [f].[name],[t].[tid], [t].[fid], [t].[iconid], [t].[typeid], [t].[readperm], [t].[price], [t].[poster], [t].[posterid], [t].[title], [t].[postdatetime], [t].[lastpost], [t].[lastpostid], [t].[lastposter], [t].[lastposterid], [t].[views], [t].[replies], [t].[displayorder], [t].[highlight], [t].[digest], [t].[rate], [t].[hide], [t].[poll], [t].[attachment], [t].[moderated], [t].[closed], [t].[magic]" +
                //"FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t],[" + BaseConfigs.GetTablePrefix + "forums] AS [f] WHERE [t].[fid]=[f].[fid] AND [tid] IN (SELECT  TOP " + pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid]=" + userId + " ORDER BY [tid] DESC) ORDER BY [tid] DESC";
                sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN (SELECT DISTINCT TOP " + pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid]=" + userId + " ORDER BY [tid] DESC) ORDER BY [tid] DESC";

            }
            else
            {
                //sql = " SELECT [f].[name],[t].[tid], [t].[fid], [t].[iconid], [t].[typeid], [t].[readperm], [t].[price], [t].[poster], [t].[posterid], [t].[title], [t].[postdatetime], [t].[lastpost], [t].[lastpostid], [t].[lastposter], [t].[lastposterid], [t].[views], [t].[replies], [t].[displayorder], [t].[highlight], [t].[digest], [t].[rate], [t].[hide], [t].[poll], [t].[attachment], [t].[moderated], [t].[closed], [t].[magic]" +
                // "FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t],[" + BaseConfigs.GetTablePrefix + "forums] AS [f] WHERE [t].[fid]=[f].[fid] AND [tid] IN (SELECT  TOP " + pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid]=" + userId + "" +
                // "AND [tid] < (SELECT MIN([tid])FROM (SELECT  TOP " + (pageIndex - 1) * pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid]=" + userId + " ORDER BY [tid] DESC) AS [ttt]) ORDER BY [tid] DESC) ORDER BY [tid] DESC";
                sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN (SELECT DISTINCT TOP " + pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid]=" + userId + " AND [tid] < (SELECT MIN([tid]) FROM (SELECT DISTINCT TOP " + (pageIndex - 1) * pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "myposts] WHERE [uid]=" + userId + " ORDER BY [tid] DESC) AS [ttt])ORDER BY [tid] DESC) ORDER BY [tid] DESC";
            }


            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);

            // IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmyposts", parms);
            return reader;
        }
        /// <summary>
        /// 按照用户Id获取主题总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTopicsCountbyUserId(int userId)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userId)
								   };

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}mytopics] WHERE [uid] = @uid", BaseConfigs.GetTablePrefix), parms), 0);
        }


        public IDataReader GetTopicsByUserId(int userId, int pageIndex, int pageSize)
        {
            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userId),
            //                            DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageIndex),
            //                            DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pageSize)
            //                       };

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmytopics", parms);
            //return reader;

            if (pageSize == 0)
            {


                return null;
            }




            DbParameter[] parms = {
										DbHelper.MakeInParam("@uid", (DbType)OleDbType.Integer, 4, userId),
										DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageIndex),
										DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pageSize)
								   };

            String sql = "";
            if (pageIndex == 1)
            {
                sql = "SELECT [f].[name],[t].[tid], [t].[fid], [t].[iconid], [t].[typeid], [t].[readperm], [t].[price], [t].[poster], [t].[posterid], [t].[title], [t].[postdatetime], [t].[lastpost], [t].[lastpostid], [t].[lastposter], [t].[lastposterid], [t].[views], [t].[replies], [t].[displayorder], [t].[highlight], [t].[digest], [t].[rate], [t].[hide], [t].[poll], [t].[attachment], [t].[moderated], [t].[closed], [t].[magic],[t].[Special]" +
                     "FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t],[" + BaseConfigs.GetTablePrefix + "forums] AS [f] WHERE [t].[fid]=[f].[fid] AND [tid] IN (SELECT TOP " + pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "mytopics] WHERE [uid]=" + userId + " ORDER BY [tid] DESC) ORDER BY [tid] DESC";
            }
            else
            {
                sql = "SELECT [f].[name],[t].[tid], [t].[fid], [t].[iconid], [t].[typeid], [t].[readperm], [t].[price], [t].[poster], [t].[posterid], [t].[title], [t].[postdatetime], [t].[lastpost], [t].[lastpostid], [t].[lastposter], [t].[lastposterid], [t].[views], [t].[replies], [t].[displayorder], [t].[highlight], [t].[digest], [t].[rate], [t].[hide], [t].[poll], [t].[attachment], [t].[moderated], [t].[closed], [t].[magic],[t].[Special]" +
                  "FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t],[" + BaseConfigs.GetTablePrefix + "forums] AS [f] WHERE [t].[fid]=[f].[fid] AND [tid] IN (SELECT TOP " + pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "mytopics] WHERE [uid]=" + userId + "" +
                  "AND [tid] < (SELECT MIN([tid]) FROM (SELECT TOP " + (pageIndex - 1) * pageSize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "mytopics] WHERE [uid]=" + userId + " ORDER BY [tid] DESC) AS [ttt]) ORDER BY [tid] DESC) ORDER BY [tid] DESC";

            }


            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, parms);
            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmytopics", parms);
            return reader;
        }

        public int CreateTopic(TopicInfo topicinfo)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 2, topicinfo.Fid), 
										DbHelper.MakeInParam("@iconid", (DbType)OleDbType.Integer, 2, topicinfo.Iconid), 
										DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 60, topicinfo.Title), 
										DbHelper.MakeInParam("@typeid", (DbType)OleDbType.Integer, 2, topicinfo.Typeid), 
										DbHelper.MakeInParam("@readperm", (DbType)OleDbType.Integer, 4, topicinfo.Readperm), 
										DbHelper.MakeInParam("@price", (DbType)OleDbType.Integer, 2, topicinfo.Price), 
										DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarWChar, 15, topicinfo.Poster), 
										DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer, 4, topicinfo.Posterid), 
										DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp,4, DateTime.Parse(topicinfo.Postdatetime)), 
										DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.VarChar, 0, topicinfo.Lastpost), 
										DbHelper.MakeInParam("@lastpostid", (DbType)OleDbType.Integer, 4, topicinfo.Lastpostid),
										DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 15, topicinfo.Lastposter), 
										DbHelper.MakeInParam("@views", (DbType)OleDbType.Integer, 4, topicinfo.Views), 
										DbHelper.MakeInParam("@replies", (DbType)OleDbType.Integer, 4, topicinfo.Replies), 
										DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, topicinfo.Displayorder), 
										DbHelper.MakeInParam("@highlight", (DbType)OleDbType.VarChar, 500, topicinfo.Highlight), 
										DbHelper.MakeInParam("@digest", (DbType)OleDbType.Integer, 4, topicinfo.Digest), 
										DbHelper.MakeInParam("@rate", (DbType)OleDbType.Integer, 4, topicinfo.Rate), 
										DbHelper.MakeInParam("@hide", (DbType)OleDbType.Integer, 4, topicinfo.Hide), 
										//DbHelper.MakeInParam("@poll", (DbType)OleDbType.Integer, 4, topicinfo.Poll), 
										DbHelper.MakeInParam("@attachment", (DbType)OleDbType.Integer, 4, topicinfo.Attachment), 
										DbHelper.MakeInParam("@moderated", (DbType)OleDbType.Integer, 4, topicinfo.Moderated), 
										DbHelper.MakeInParam("@closed", (DbType)OleDbType.Integer, 4, topicinfo.Closed),
										DbHelper.MakeInParam("@magic", (DbType)OleDbType.Integer, 4, topicinfo.Magic),
                                        DbHelper.MakeInParam("@special", (DbType)OleDbType.Integer, 1, topicinfo.Special),
                                        DbHelper.MakeInParam("@attention", (DbType)OleDbType.Integer, 4, topicinfo.Attention)
								   };
            //return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createtopic", parms).Tables[0].Rows[0][0].ToString(), -1);
            int topicid = 0;

            string sql = "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid]>" +
                         "(SELECT (iif(ISNULL(max(tid)),0,max(tid)))-100 FROM [" + BaseConfigs.GetTablePrefix + "topics]) AND [lastpostid]=0";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            sql = "INSERT INTO ["+BaseConfigs.GetTablePrefix+"topics]([fid], [iconid], [title], [typeid], [readperm], [price], [poster], [posterid], [postdatetime], [lastpost], [lastpostid], [lastposter], [views], [replies], [displayorder], [highlight], [digest], [rate], [hide], [attachment], [moderated], [closed], [magic], [special],[attention]) VALUES(@fid, @iconid, @title, @typeid, @readperm, @price, @poster, @posterid, @postdatetime, @lastpost, @lastpostid, @lastposter, @views, @replies, @displayorder, @highlight, @digest, @rate, @hide, @attachment, @moderated, @closed, @magic, @special,@attention)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
            topicid = Int32.Parse(DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] ORDER BY [tid] DESC").Tables[0].Rows[0][0].ToString());
            if (topicinfo.Displayorder == 0)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic] + 1");

                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1,[curtopics] = [curtopics] + 1 WHERE [fid] = " + topicinfo.Fid);
                if (topicinfo.Posterid != -1)
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "mytopics]([tid],[uid],[dateline]) VALUES(" + topicid + ", " + topicinfo.Posterid + ",  #" + DateTime.Parse(topicinfo.Postdatetime) + "#)");
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
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@topics", (DbType)OleDbType.Integer, 4, topics)
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}forums] SET [topics] = [topics] + @topics WHERE [fid] IN ({1})", BaseConfigs.GetTablePrefix, fpidlist), parms);
        }


        public IDataReader GetTopicInfo(int tid, int fid, byte mode)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer,4, fid),
									   DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer,4, tid),
			};
            IDataReader reader;
            switch (mode)
            {
                case 1:
                    reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=@fid AND [tid]<@tid AND [displayorder]>=0 ORDER BY [tid] DESC", parms);
                    break;
                case 2:
                    reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=@fid AND [tid]>@tid AND [displayorder]>=0 ORDER BY [tid] ASC", parms);
                    break;
                default:
                    DbParameter[] parms1 = {
									   DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer,4, tid),
			        };
                    reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid]=@tid", parms1);
                    break;
            }
            return reader;
        }


        public IDataReader GetTopTopics(int fid, int pagesize, int pageindex, string tids)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid),
            //                           DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
            //                           DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
            //                           DbHelper.MakeInParam("@tids",(DbType)OleDbType.VarChar,500,tids)

            //                       };
            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettoptopiclist", parms);
            //return reader;
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid),
									  // DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
									   DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
									   DbHelper.MakeInParam("@tids",(DbType)OleDbType.VarChar,500,tids)
									   
								   };
            String sql = "";
            if (pageindex == 1)
            {
                sql = "SELECT TOP " + pagesize + " [rate], [tid],[fid],[typeid],[iconid],[title],[price],[hide],[readperm], [special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[special] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]>0 AND [tid] IN (" + tids + ") ORDER BY [lastpost] DESC";
            }
            else
            {
                //              sql = "SELECT TOP " + pagesize + " [tid], [fid],[typeid], [iconid], [title], [price], [hide], [readperm], [poll], [poster], [posterid], [replies], [views],[postdatetime], [lastpost], [lastposter], [lastpostid], [lastposterid], [replies] AS Expr1, [highlight], [digest], [displayorder], [closed], [attachment] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE (tid not in (SELECT TOP (" + pagesize + "-1)*" + pagesize + " [tid]" +
                //"FROM ["+BaseConfigs.GetTablePrefix+"topics] WHERE [displayorder] > 0 AND [tid] IN (@tids) ORDER BY [lastpost] DESC)) AND (displayorder > 0) AND (tid IN (@tids)) ORDER BY lastpost DESC";

                sql = "SELECT TOP " + pagesize + " [rate], [tid],[fid],[typeid],[iconid],[title],[price],[hide],[readperm], [special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[special]  FROM [" + BaseConfigs.GetTablePrefix + "topics" + "] WHERE ([tid] NOT IN (SELECT TOP " + (pageindex - 1) * pagesize + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics" + "] WHERE [displayorder] > 0" + " AND [tid] IN ( " + tids + " ) ORDER BY [lastpost] DESC)) AND ([displayorder] > 0)" + " AND ([tid] IN ( " + tids + " )) ORDER BY [lastpost] DESC";
            }
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettoptopiclist", prams);
            return reader;
        }


        public IDataReader GetTopics(int fid, int pagesize, int pageindex, int startnum, string condition)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid),
            //                           DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
            //                           DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer,4,pageindex),
            //                           DbHelper.MakeInParam("@startnum", (DbType)OleDbType.Integer,4,startnum),
            //                           DbHelper.MakeInParam("@condition", (DbType)OleDbType.VarChar,80,condition)									   
            //                       };
            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclist", parms);

            //return reader;
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid),
									  // DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
									   DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer,4,pageindex),
									   DbHelper.MakeInParam("@startnum", (DbType)OleDbType.Integer,4,startnum),
									  // DbHelper.MakeInParam("@condition", (DbType)OleDbType.VarChar,80,condition)									   
								   };
            String sql = "";
            //StringBuilder sql = new StringBuilder();
            //if(pageindex==1)
            //{

            //    sql.Append("SELECT TOP "+pagesize+" [tid],[iconid],[typeid],[title],[price],[hide],[readperm],[poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=@fid AND [displayorder]=0 "+condition+" ORDER BY [lastpostid] DESC");

            //}
            //else

            //{

            //sql.Append("SELECT TOP "+pagesize+" [tid],[iconid],[typeid],[title],[price],[hide],[readperm],");
            //sql.Append("[poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],");
            //sql.Append("[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic] FROM");
            //sql.Append("[{0}topics] WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP (" + pagesize + "-1)*" + pagesize + "-@startnum [lastpostid] FROM [{1}topics] WHERE [fid]=@fid AND [displayorder]=0 "+condition+" ORDER BY [lastpostid] DESC) AS tblTmp )");
            //sql.Append("AND [fid]=@fid AND [displayorder]=0 "+condition+" ORDER BY [lastpostid] DESC");

            //}
            if (pageindex == 1)
            {
                sql = string.Format("SELECT TOP {0} [tid],[iconid],[typeid],[title],[price],[hide],[readperm], [poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics" + "] WHERE [fid]={1} AND [displayorder]=0 {2} ORDER BY [lastpostid] DESC", pagesize, fid, condition);
            }
            else
            {
                sql = string.Format("SELECT TOP {0} [tid],[iconid],[typeid],[title],[price],[hide],[readperm], [poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics" + "] WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP " + ((pageindex - 1) * pagesize - startnum) + " [lastpostid] FROM [" + BaseConfigs.GetTablePrefix + "topics" + "] WHERE [fid]={1} AND [displayorder]=0 {2} ORDER BY [lastpostid] DESC) AS tblTmp ) AND [fid]={3} AND [displayorder]=0 {4} ORDER BY [lastpostid] DESC", pagesize, fid, condition, fid, condition);
            }
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, String.Format(sql.ToString(), BaseConfigs.GetTablePrefix, BaseConfigs.GetTablePrefix), prams);
            // IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, sql, prams);

            return reader;
        }


        public IDataReader GetTopicsByDate(int fid, int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid),
            //                           DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
            //                           DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
            //                           DbHelper.MakeInParam("@startnum",(DbType)OleDbType.Integer,4,startnum),
            //                           DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarChar,80,condition),
            //                           DbHelper.MakeInParam("@orderby",(DbType)OleDbType.VarChar,80,orderby),
            //                           DbHelper.MakeInParam("@ascdesc",(DbType)OleDbType.Integer,4,ascdesc)

            //                       };

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclistbydate", parms);

            //return reader;
            DbParameter[] prams = {
                                       //DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid),
                                       //DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
                                       //DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
                                       //DbHelper.MakeInParam("@startnum",(DbType)OleDbType.Integer,4,startnum),
                                       //DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarChar,80,condition),
									   DbHelper.MakeInParam("@orderby",(DbType)OleDbType.VarChar,80,orderby),
                                       //DbHelper.MakeInParam("@ascdesc",(DbType)OleDbType.Integer,4,ascdesc)
				                    
								   };
            String sql = "";
            String sorttype;
            int s = ((pageindex - 1) * pagesize) - startnum;
            if (ascdesc == 0)
            {
                sorttype = "asc";
            }
            else
            {
                sorttype = "desc";
            }


            if (pageindex == 1)
            {
                sql = "SELECT TOP " + pagesize + " [tid],[iconid],[title],[price],[typeid],[readperm],[hide],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + " AND [displayorder]=0 " + condition + " ORDER BY " + orderby + "" + sorttype;
            }
            else
            {

                if (sorttype == "desc")
                {
                    sql = "SELECT TOP " + pagesize + " [tid],[iconid],[title],[price],[typeid],[readperm],[hide],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + orderby + " < (SELECT min(" + orderby + ") FROM (SELECT TOP " + s + " " + orderby + "  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + " AND [displayorder]=0 " + condition + " ORDER BY " + orderby + " " + sorttype + ") AS tblTmp ) AND [fid]=" + fid + " AND [displayorder]=0 " + condition + " ORDER BY " + orderby + " " + @sorttype;
                }
                else
                {
                    sql = "SELECT TOP " + pagesize + " [tid],[iconid],[title],[price],[typeid],[readperm],[hide],[special],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[rate] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + orderby + " < (SELECT MAX(" + orderby + ") FROM (SELECT TOP " + s + " " + orderby + "  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + " AND [displayorder]=0 " + condition + " ORDER BY " + orderby + " " + sorttype + ") AS tblTmp ) AND [fid]=" + fid + " AND [displayorder]=0 " + condition + " ORDER BY " + orderby + " " + @sorttype;
                }




            }



            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, prams);

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclistbydate", prams);

            return reader;
        }


        public IDataReader GetTopicsByType(int pagesize, int pageindex, int startnum, string condition, int ascdesc)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
									   DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarChar,1000,condition)
								   };
            if (pagesize == 0)
            {

                return null;
            }

            string sorttype = null, strsql = null;
            if (ascdesc == 0)
            {
                sorttype = "ASC";
            }
            else
            {
                sorttype = "DESC";
            }
            if (pageindex == 1)
            {
                strsql = "SELECT TOP " + pagesize + " [tid],[iconid],[typeid],[title],[price],[hide],[readperm],[poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE  [displayorder]>=0 " + condition + " ORDER BY [tid] " + sorttype + ",[lastpostid] " + sorttype + "";
            }
            else
            {

                int p = (pageindex - 1) * pagesize - startnum;
                if (sorttype == "DESC")
                {
                    strsql = "SELECT TOP " + pagesize + " [tid],[iconid],[typeid],[title],[price],[hide],[readperm],[poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP " + p + " [lastpostid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE  [displayorder]>=0 " + condition + " ORDER BY [tid] " + sorttype + " , [lastpostid] " + sorttype + ") AS tblTmp ) AND  [displayorder]>=0 " + condition + " ORDER BY [tid] " + sorttype + " ,  [lastpostid] " + sorttype;
                }
                else
                {

                    strsql = "SELECT TOP " + pagesize + " [tid],[iconid],[typeid],[title],[price],[hide],[readperm],[poll],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[attachment],[closed],[magic],[special],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [lastpostid] > (SELECT MAX([lastpostid])  FROM (SELECT TOP " + p + " [lastpostid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE  [displayorder]>=0 " + condition + " ORDER BY [tid] " + sorttype + " , [lastpostid] ' + @sorttype + ') AS tblTmp ) AND  [displayorder]>=0 " + condition + " ORDER BY [tid] " + sorttype + " ,  [lastpostid] " + sorttype;

                }




            }

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, strsql, prams);

            return reader;
        }


        public IDataReader GetTopicsByTypeDate(int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
        {
            //DbParameter[] parms = {
            //                           DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer,4,pagesize),
            //                           DbHelper.MakeInParam("@pageindex",(DbType)OleDbType.Integer,4,pageindex),
            //                           DbHelper.MakeInParam("@startnum",(DbType)OleDbType.Integer,4,startnum),
            //                           DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarChar,1000,condition),
            //                           DbHelper.MakeInParam("@orderby",(DbType)OleDbType.VarChar,80,orderby),
            //                           DbHelper.MakeInParam("@ascdesc",(DbType)OleDbType.Integer,4,ascdesc)

            //                       };

            //IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclistbytypedate", parms);

            //return reader;
            DbParameter[] prams = {

									   DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarChar,1000,condition),
									  
				                    
								   };

            if (pagesize == 0)
            {
                return null;


            }

            string sorttype = "", strsql = "";
            int s = ((pageindex - 1) * pagesize) - startnum;
            if (ascdesc == 0)
            {
                sorttype = "asc";
            }
            else
            { sorttype = "desc"; }

            if (pageindex == 1)
            {
                strsql = "SELECT TOP " + pagesize + " [tid],[iconid],[typeid],[title],[poll],[price],[hide],[readperm],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]>=0 " + condition + "  ORDER BY  " + orderby + " " + sorttype;
            }
            else
            {
                if (sorttype == "desc")
                {

                    strsql = "SELECT TOP " + pagesize + " [tid],[iconid],[typeid],[title],[poll],[price],[hide],[readperm],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + orderby + "] < (SELECT min([" + orderby + "]) FROM (SELECT TOP " + s + " [" + orderby + "]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE  [displayorder]>=0 " + condition + " ORDER BY  " + orderby + " " + sorttype + ") AS tblTmp ) AND [displayorder]>=0  " + condition + " ORDER BY  " + orderby + " " + sorttype;

                }
                else
                {
                    strsql = "SELECT TOP " + pagesize + " [tid],[iconid],[typeid],[title],[poll],[price],[hide],[readperm],[poster],[posterid],[replies],[views],[postdatetime],[lastpost],[lastposter],[lastpostid],[lastposterid],[replies],[highlight],[digest],[displayorder],[closed],[attachment],[magic],[rate]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + orderby + "] > (SELECT max([" + orderby + "]) FROM (SELECT TOP " + s + " [" + orderby + "]  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE  [displayorder]>=0 " + condition + " ORDER BY  " + orderby + " " + sorttype + ") AS tblTmp ) AND [displayorder]>=0  " + condition + " ORDER BY  " + orderby + " " + sorttype;

                }
            }


            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, strsql, prams);

            return reader;
        }


        public DataTable GetTopicList(string topiclist, int displayorder)
        {
            string commandText = string.Format("SELECT * FROM [{0}topics] WHERE [displayorder]>{1} AND [tid] IN ({2})", BaseConfigs.GetTablePrefix, displayorder, topiclist);
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
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)								   
								   };
            int count = int.Parse(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid]=@tid AND [invisible]=0", prams).ToString());
            DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies]=" + (count - 1) + " WHERE [displayorder]>=0 AND [tid]=@tid", prams);

        }

        /// <summary>
        /// 得到当前版块内正常(未关闭)主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public int GetTopicCount(int fid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid)
								   };
            string sql = "SELECT [curtopics] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + fid;
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
        }

        /// <summary>
        /// 得到当前版块内(包括子版)正常(未关闭)主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public int GetAllTopicCount(int fid)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid)
								   };
            string sql = "SELECT COUNT(tid) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE ([fid]=" + @fid + "   OR   [fid]  IN (  SELECT fid  FROM [" + BaseConfigs.GetTablePrefix + "forums]  WHERE  InStr(',' + RTRIM(@fid) + ',', ',' + RTRIM(parentidlist) + ',') > 0))  AND [displayorder]>=0";
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
        }

        /// <summary>
        /// 得到当前版块内主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public int GetTopicCount(int fid, int state, string condition)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@fid",(DbType)OleDbType.Integer,4,fid)
								   };
            string sql = "SELECT COUNT(tid) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE ([fid]=" + fid + "   OR   [fid]  IN (  SELECT fid  FROM [" + BaseConfigs.GetTablePrefix + "forums]  WHERE  INSTR(',' & RTRIM(parentidlist) & ',','," + fid + ",') > 0))  AND [displayorder]>=0";
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, prams), 0);
        }

        /// <summary>
        /// 得到符合条件的主题总数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>主题总数</returns>
        public int GetTopicCount(string condition)
        {
            DbParameter[] prams = {
									   DbHelper.MakeInParam("@condition",(DbType)OleDbType.VarChar,1000,condition)
								   };
            string sql = "SELECT COUNT(tid) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]>-1 AND [closed]<=1 " + @condition;
            return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows[0][0].ToString(), -1);
        }

        /// <summary>
        /// 更新主题标题
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">新标题</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicTitle(int tid, string topictitle)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@topictitle", (DbType)OleDbType.VarWChar, 60, topictitle),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, tid)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [title] = @topictitle WHERE [tid] = @tid", parms);
        }


        public int UpdateTopicTitleAndPoster(int tid, string topictitle, int posterid, string poster)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@topictitle", (DbType)OleDbType.VarWChar, 60, topictitle),
                                        DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer,4, posterid),
                                         DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarWChar, 20, poster),
                                         DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, tid)

								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [title] = @topictitle,[posterid] = @posterid,[poster] = @poster WHERE [tid] = @tid", parms);
        }
        /// <summary>
        /// 更新主题图标id
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="iconid">主题图标id</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicIconID(int tid, int iconid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@iconid", (DbType)OleDbType.Integer, 2, iconid),
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, tid)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [iconid] = @iconid WHERE [tid] = @tid", parms);
        }

        /// <summary>
        /// 更新主题价格
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicPrice(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, tid),
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [price] = 0 WHERE [tid] = @tid", parms);
        }

        /// <summary>
        /// 更新主题价格
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="price">价格</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicPrice(int tid, int price)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@price",(DbType)OleDbType.Integer,4, price),
                                        DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, tid)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [price] = @price WHERE [tid] = @tid", parms);
        }

        /// <summary>
        /// 更新主题阅读权限
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="readperm">阅读权限</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicReadperm(int tid, int readperm)
        {
            DbParameter[] parms = {
									  DbHelper.MakeInParam("@readperm",(DbType)OleDbType.Integer,4, readperm),
									  DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, tid), 
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [readperm] = @readperm WHERE [tid] = @tid", parms);
        }

        /// <summary>
        /// 更新主题为已被管理
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <param name="moderated">管理操作id</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicModerated(string topiclist, int moderated)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@moderated",(DbType)OleDbType.Integer,4, moderated),
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [moderated] = @moderated WHERE [tid] IN (" + topiclist + ")", parms);

        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicinfo">主题信息</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopic(TopicInfo topicinfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4, topicinfo.Tid),
									   DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer, 2, topicinfo.Fid), 
									   DbHelper.MakeInParam("@iconid", (DbType)OleDbType.Integer, 2, topicinfo.Iconid), 
									   DbHelper.MakeInParam("@title", (DbType)OleDbType.VarWChar, 60, topicinfo.Title), 
									   DbHelper.MakeInParam("@typeid", (DbType)OleDbType.Integer, 2, topicinfo.Typeid), 
									   DbHelper.MakeInParam("@readperm", (DbType)OleDbType.Integer, 4, topicinfo.Readperm), 
									   DbHelper.MakeInParam("@price", (DbType)OleDbType.Integer, 2, topicinfo.Price), 
									   DbHelper.MakeInParam("@poster", (DbType)OleDbType.VarWChar, 15, topicinfo.Poster), 
									   DbHelper.MakeInParam("@posterid", (DbType)OleDbType.Integer, 4, topicinfo.Posterid), 
									   DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 4, DateTime.Parse(topicinfo.Postdatetime)), 
									   DbHelper.MakeInParam("@lastpost", (DbType)OleDbType.VarChar, 0, topicinfo.Lastpost), 
									   DbHelper.MakeInParam("@lastposter", (DbType)OleDbType.VarWChar, 15, topicinfo.Lastposter), 
									   //DbHelper.MakeInParam("@views", (DbType)OleDbType.Integer, 4, topicinfo.Views), 
									   DbHelper.MakeInParam("@replies", (DbType)OleDbType.Integer, 4, topicinfo.Replies), 
									   DbHelper.MakeInParam("@displayorder", (DbType)OleDbType.Integer, 4, topicinfo.Displayorder), 
									   DbHelper.MakeInParam("@highlight", (DbType)OleDbType.VarChar, 500, topicinfo.Highlight), 
									   DbHelper.MakeInParam("@digest", (DbType)OleDbType.Integer, 4, topicinfo.Digest), 
									   DbHelper.MakeInParam("@rate", (DbType)OleDbType.Integer, 4, topicinfo.Rate), 
									   DbHelper.MakeInParam("@hide", (DbType)OleDbType.Integer, 4, topicinfo.Hide), 
									   DbHelper.MakeInParam("@special", (DbType)OleDbType.Integer, 4, topicinfo.Special), 
									   DbHelper.MakeInParam("@attachment", (DbType)OleDbType.Integer, 4, topicinfo.Attachment), 
									   DbHelper.MakeInParam("@moderated", (DbType)OleDbType.Integer, 4, topicinfo.Moderated), 
									   DbHelper.MakeInParam("@closed", (DbType)OleDbType.Integer, 4, topicinfo.Closed),
									   DbHelper.MakeInParam("@magic", (DbType)OleDbType.Integer, 4, topicinfo.Magic),
                                       //DbHelper.MakeInParam("@attention", (DbType)OleDbType.Integer, 4, topicinfo.Attention),
								   };
            string sql = @"UPDATE " + BaseConfigs.GetTablePrefix + @"topics SET
	                    [fid]=@fid,
	                    [iconid]=@iconid,
	                    [title]=@title,
	                    [typeid]=@typeid,
	                    [readperm]=@readperm,
	                    [price]=@price,
	                    [poster]=@poster,
	                    [posterid]=@posterid,
	                    [postdatetime]=@postdatetime,
	                    [lastpost]=@lastpost,
	                    [lastposter]=@lastposter,
	                    [replies]=@replies,
	                    [displayorder]=@displayorder,
	                    [highlight]=@highlight,
	                    [digest]=@digest,
	                    [rate]=@rate,
	                    [hide]=@hide,
                        [special]=@special,
	                    [attachment]=@attachment,
	                    [moderated]=@moderated,
	                    [closed]=@closed,
	                    [magic]=@magic
                        WHERE [tid]=@tid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 判断帖子列表是否都在当前板块
        /// </summary>
        /// <param name="topicidlist"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public bool InSameForum(string topicidlist, int fid)
        {
            string commandText = string.Format("SELECT COUNT([tid]) FROM [{0}topics] WHERE [fid]={1} AND [tid] IN ({2})", BaseConfigs.GetTablePrefix, fid, topicidlist);
            return Utils.SplitString(topicidlist, ",").Length == (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        /// <summary>
        /// 将主题设置为隐藏主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int UpdateTopicHide(int tid)
        {
            string sql = string.Format(@"UPDATE [{0}topics] SET [hide] = 1 WHERE [tid] = {1}", BaseConfigs.GetTablePrefix, tid);

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public DataTable GetTopicList(int forumid, int pageid, int tpp)
        {
            DataTable dt;
            if (pageid == 1)
            {
                dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP {0} [tid],[title],[replies] FROM [{1}topics] WHERE [fid]={2} AND [displayorder]>=0 ORDER BY [lastpostid] DESC", tpp.ToString(), BaseConfigs.GetTablePrefix, forumid.ToString())).Tables[0];
            }
            else
            {
                dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP {0} [tid],[title],[replies] FROM [{1}topics] WHERE [lastpostid] < (SELECT MIN([lastpostid])  FROM (SELECT TOP {2} [lastpostid] FROM [{1}topics] WHERE [fid]={3} AND [displayorder]>=0 ORDER BY [lastpostid] DESC) AS tblTmp ) AND [fid]={3} AND [displayorder]>=0 ORDER BY [lastpostid] DESC", tpp.ToString(), BaseConfigs.GetTablePrefix, ((pageid - 1) * tpp).ToString(), forumid.ToString())).Tables[0];
            }
            return dt;
        }

        public DataTable GetTopicFidByTid(string tidlist)
        {
            string sql = "SELECT DISTINCT [fid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetTopicTidByFid(string tidlist, int fid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@fid", (DbType)OleDbType.Integer,4, fid)
            };
            string sql = "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") AND [fid]=@fid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        /// <summary>
        /// 更新主题浏览量
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="viewcount">浏览量</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicViewCount(int tid, int viewcount)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@viewcount",(DbType)OleDbType.Integer,4,viewcount),	
										DbHelper.MakeInParam("@tid",(DbType)OleDbType.Integer,4,tid)			   
									};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics]  SET [views]= [views] + @viewcount WHERE [tid]=@tid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql,parms);
        }

        public string SearchTopics(int forumid, string keyword, string displayorder, string digest, string attachment, string poster, bool lowerupper, string viewsmin, string viewsmax,
                                        string repliesmax, string repliesmin, string rate, string lastpost, DateTime postdatetimeStart, DateTime postdatetimeEnd)
        {
            string sqlstring = null;
            sqlstring += " [tid]>0";

            if (forumid != 0)
            {
                sqlstring += " AND [fid]=" + forumid;
            }

            if (keyword != "")
            {
                sqlstring += " AND (";
                foreach (string word in keyword.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [title] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            switch (displayorder)
            {
                case "0":
                    break;
                case "1":
                    sqlstring += " AND [displayorder]>0";
                    break;
                case "2":
                    sqlstring += " AND [displayorder]<=0";
                    break;
            }

            switch (digest)
            {
                case "0":
                    break;
                case "1":
                    sqlstring += " AND [digest]>=1";
                    break;
                case "2":
                    sqlstring += " AND [digest]<1";
                    break;
            }

            switch (attachment)
            {
                case "0":
                    break;
                case "1":
                    sqlstring += " AND [attachment]>0";
                    break;
                case "2":
                    sqlstring += " AND [attachment]<=0";
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
                            sqlstring += " [poster]='" + postername + "' OR";
                        }
                        else
                        {
                            sqlstring += " [poster] COLLATE Chinese_PRC_CS_AS_WS ='" + postername + "' OR";
                        }
                    }
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (viewsmax != "")
            {
                sqlstring += " AND [views]>" + viewsmax;
            }

            if (viewsmin != "")
            {
                sqlstring += " AND [views]<" + viewsmin;
            }

            if (repliesmax != "")
            {
                sqlstring += " AND [replies]>" + repliesmax;
            }

            if (repliesmin != "")
            {
                sqlstring += " AND [replies]<" + repliesmin;
            }

            if (rate != "")
            {
                sqlstring += " AND [rate]>" + rate;
            }

            if (lastpost != "")
            {
                sqlstring += " AND DATEDIFF(\"d\",[lastpost],NOW())>=" + lastpost + "";
            }

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

            return sqlstring;
        }

        public string SearchAttachment(int forumid, string posttablename, string filesizemin, string filesizemax, string downloadsmin, string downloadsmax, string postdatetime, string filename, string description, string poster)
        {
            string sqlstring = null;
            sqlstring += " WHERE [aid] > 0";


            if (forumid != 0)
            {
                sqlstring += " AND [pid] IN (SELECT PID FROM [" + posttablename + "] WHERE [fid]=" + forumid + ")";
            }

            if (filesizemin != "")
            {
                sqlstring += " AND [filesize]<" + filesizemin;
            }

            if (filesizemax != "")
            {
                sqlstring += " AND [filesize]>" + filesizemax;
            }

            if (downloadsmin != "")
            {
                sqlstring += " AND [downloads]<" + downloadsmin;
            }

            if (downloadsmax != "")
            {
                sqlstring += " AND [downloads]>" + downloadsmax;
            }

            if (postdatetime != "")
            {
                sqlstring += " AND datediff(\"d\",postdatetime,NOW())>=" + postdatetime;
            }

            if (filename != "")
            {
                sqlstring += " AND [filename] like '%" + RegEsc(filename) + "%'";
            }


            if (description != "")
            {
                sqlstring += " AND (";
                foreach (string word in description.Split(','))
                {
                    if (word.Trim() != "")
                        sqlstring += " [description] LIKE '%" + RegEsc(word) + "%' OR ";
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (poster != "")
            {
                sqlstring += " AND [pid] IN (SELECT [pid] FROM [" + posttablename + "] WHERE [poster]='" + poster + "')";
            }

            return sqlstring;
        }

        public string SearchPost(int forumid, string posttableid, DateTime postdatetimeStart, DateTime postdatetimeEnd, string poster, bool lowerupper, string ip, string message)
        {
            string sqlstring = null;
            sqlstring += " [pid]>0 ";

            if (forumid != 0)
            {
                sqlstring += " AND [fid]=" + forumid;
            }

            sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

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
                            sqlstring += " [poster]='" + postername + "' OR";
                        }
                        else
                        {
                            sqlstring += " [poster] COLLATE Chinese_PRC_CS_AS_WS ='" + postername + "' OR";
                        }
                    }
                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            if (ip != "")
            {
                sqlstring += " AND [ip] LIKE'%" + RegEsc(ip.Replace(".*", "")) + "%'";
            }

            if (message != "")
            {
                sqlstring += " AND (";
                foreach (string messageinf in message.Split(','))
                {
                    if (messageinf.Trim() != "")
                    {
                        sqlstring += " [message] LIKE '%" + RegEsc(messageinf) + "%' OR";
                    }

                }
                sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
            }

            return sqlstring;
        }

        public void IdentifyTopic(string topiclist, int identify)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@identify", (DbType)OleDbType.Integer, 4, identify)
            };

            string sql = string.Format("UPDATE [{0}topics] SET [identify]=@identify WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, topiclist);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateTopic(int tid, string title, int posterid, string poster)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@tid", (DbType) OleDbType.Integer, 4, tid),
                                      DbHelper.MakeInParam("@title", (DbType) OleDbType.VarWChar, 60, title),
                                      DbHelper.MakeInParam("@posterid", (DbType) OleDbType.Integer, 4, posterid),
                                      DbHelper.MakeInParam("@poster", (DbType) OleDbType.VarWChar, 20, poster)
                                  };

            string sql = string.Format("UPDATE [{0}topics] SET [title]=@title, [posterid]=@posterid, [poster]=@poster WHERE [tid]=@tid", BaseConfigs.GetTablePrefix);
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
                condition += " AND postdatetime>=#" + DateTime.Now.AddMinutes(-getnewtopic).ToString("yyyy-MM-dd HH:mm:ss") + "#";
            }
            return condition;
        }


        public string GetRateLogCountCondition(int userid, string postidlist)
        {

            return "[uid]=" + userid + " AND [pid] = " + Utils.StrToInt(postidlist, 0).ToString();
        }

        public DataTable GetOtherPostId(string postidlist, int topicid, int postid)
        {

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "select * from " + BaseConfigs.GetTablePrefix + "posts" + postid + " where pid not in (" + postidlist + ") and tid=" + topicid + " order by pid desc").Tables[0];
            return dt;
        }

        public void CreateTopicTags(string tags, int topicid, int userid, string curdatetime)
        {
            //DbParameter[] parms = {
            //    DbHelper.MakeInParam("@tags", (DbType)OleDbType.VarWChar, 55, tags),
            //    DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, topicid),
            //    DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid),
            //    DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, curdatetime)                
            //};

            //DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createtopictags", BaseConfigs.GetTablePrefix), parms);
            DbParameter[] parms = {
                DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid),
                DbHelper.MakeInParam("@postdatetime", (DbType)OleDbType.DBTimeStamp, 8, curdatetime)                
            };

            DbParameter[] parms1 = {
                DbHelper.MakeInParam("@tags", (DbType)OleDbType.VarWChar, 55, tags)          
            };


            DbParameter[] parms2 = {
                DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, topicid)            
            };

            //dnt_split(@tags, ' ');
            if (tags != "")
            {
                foreach (string str in tags.Split(' '))
                {
                    #region 注释掉
                    //string sql = "INSERT INTO [dnt_tags]([tagname], [userid], [postdatetime], [orderid], [color], [count], [fcount], [pcount], [scount], [vcount]) SELECT [item], " + userid + ", '" + curdatetime + "', 0, '', 0, 0, 0, 0, 0 FROM dnt_splitstringstable AS [newtags] WHERE not exists (SELECT [tagname] FROM [dnt_tags] WHERE [newtags].[item] = [tagname])";
                    //DbHelper.ExecuteNonQuery(CommandType.Text, sql);

                    //string sql1 = "UPDATE [dnt_tags] SET [fcount]=[fcount]+1,[count]=[count]+1 WHERE EXISTS (SELECT [item] FROM dnt_splitstringstable AS [newtags] WHERE [newtags].[item] = [tagname])";
                    //DbHelper.ExecuteNonQuery(CommandType.Text, sql1, parms1);


                    //string sql2 = "INSERT INTO [dnt_topictags] (tagid, tid) SELECT tagid, " + topicid + " FROM [dnt_tags] WHERE EXISTS (SELECT * FROM dnt_splitstringstable WHERE [item] = [dnt_tags].[tagname])";
                    //DbHelper.ExecuteNonQuery(CommandType.Text, sql2);

                    #endregion

                    string str_word = "SELECT tagid,tagname FROM [" + BaseConfigs.GetTablePrefix + "tags] where tagname='" + str + "'";

                    DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, str_word).Tables[0];

                    if (dt.Rows.Count == 0)
                    {
                        string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "tags]([tagname], [userid], [postdatetime], [orderid], [color], [count], [fcount], [pcount], [scount], [vcount]) VALUES('" + str + "', " + userid + ", #" + curdatetime + "#, 0, '', 0, 0, 0, 0, 0)";
                        DbHelper.ExecuteNonQuery(CommandType.Text, sql);

                        string sql1 = "SELECT [tagid] FROM  [" + BaseConfigs.GetTablePrefix + "tags] WHERE [tagname]='" + str + "'";
                        int cur_tagid = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql1));

                        string sql3 = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topictags] (tagid, tid) VALUES(" + cur_tagid + "," + topicid + ")";
                        DbHelper.ExecuteNonQuery(CommandType.Text, sql3);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[0]["tagname"].ToString() != "")
                            {
                                string sql1 = "UPDATE [" + BaseConfigs.GetTablePrefix + "tags] SET [fcount]=[fcount]+1,[count]=[count]+1 WHERE tagname='" + str + "'";
                                DbHelper.ExecuteNonQuery(CommandType.Text, sql1, parms1);

                                string sql2 = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topictags] (tagid, tid) VALUES(" + dt.Rows[0]["tagid"] + "," + topicid + ")";
                                DbHelper.ExecuteNonQuery(CommandType.Text, sql2);
                            }
                        }
                    }
                }
            }
        }

        public IDataReader GetTopicListByTag(int tagid, int pageindex, int pagesize)
        {
            //string sql = string.Format("{0}gettopiclistbytag", BaseConfigs.GetTablePrefix);
            string strSQL = "";
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tagid", (DbType)OleDbType.Integer, 4, tagid),
                DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageindex),
                DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pagesize)
            };


            if (pageindex == 1)
            {
                strSQL = "SELECT TOP " + pagesize + " [t].[tid], [t].[title],[t].[poster],[t].[posterid],[t].[fid],[t].[postdatetime],[t].[replies],[t].[views],[t].[lastposter],[t].[lastposterid],[t].[lastpost] FROM [" + BaseConfigs.GetTablePrefix + "topictags] AS [tt], [" + BaseConfigs.GetTablePrefix + "topics] AS [t] WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = " + tagid + " ORDER BY [t].[lastpostid] DESC";
            }
            else
            {
                strSQL = "SELECT TOP " + pagesize + " [t].[tid], [t].[title],[t].[poster],[t].[posterid],[t].[fid],[t].[postdatetime],[t].[replies],[t].[views],[t].[lastposter],[t].[lastposterid],[t].[lastpost] FROM [" + BaseConfigs.GetTablePrefix + "topictags] AS [tt], [" + BaseConfigs.GetTablePrefix + "topics] AS [t] WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = " + tagid + " AND [t].[lastpostid] < (SELECT MIN([lastpostid]) FROM (SELECT TOP " + (pageindex - 1) * pagesize + " [lastpostid] FROM [" + BaseConfigs.GetTablePrefix + "topictags] AS [tt], [" + BaseConfigs.GetTablePrefix + "topics] AS [t] WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = " + tagid + " ORDER BY [t].[lastpostid] DESC) as tblTmp) ORDER BY [t].[lastpostid] DESC";
            }

            return DbHelper.ExecuteReader(CommandType.Text, strSQL, parms);
        }

        public IDataReader GetRelatedTopics(int topicid, int count)
        {
            string sql = string.Format("SELECT TOP {0} * FROM [{1}topictagcaches] WHERE [tid]=@tid ORDER BY [linktid] DESC", count, BaseConfigs.GetTablePrefix);

            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, topicid);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        public int GetTopicsCountByTag(int tagid)
        {
            string sql = string.Format(@"SELECT COUNT(1) FROM [{0}topictags] AS [tt], [{0}topics] AS [t] WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = @tagid", BaseConfigs.GetTablePrefix);

            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)OleDbType.Integer, 4, tagid);

            return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0].Rows[0][0], 0);
        }

        public void NeatenRelateTopics()
        {
            //string sql = string.Format(@"{0}neatenrelatetopic", BaseConfigs.GetTablePrefix);
            string sql = "SELECT DISTINCT [tagid] FROM [" + BaseConfigs.GetTablePrefix + "topictags]";
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sql1 = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topictagcaches] SELECT [t1].[tid],[t2].[tid],[t2].[title] FROM (SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topictags] WHERE [tagid] = " + dt.Rows[0]["tagid"] + ") AS [t1],(SELECT [t].[tid],[t].[title] FROM [" + BaseConfigs.GetTablePrefix + "topics] AS [t],[" + BaseConfigs.GetTablePrefix + "topictags] AS [tt] WHERE [tt].[tagid] =" + dt.Rows[0]["tagid"] + " AND [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0) AS [t2] WHERE [t1].[tid] <> [t2].[tid] AND NOT EXISTS (SELECT 1 FROM [" + BaseConfigs.GetTablePrefix + "topictagcaches] WHERE [tid]=[t1].[tid] AND [linktid]=[t2].[tid])";
                DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        public void DeleteTopicTags(int topicid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, topicid);
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "tags] SET [count]=[count]-1,[fcount]=[fcount]-1 WHERE EXISTS (SELECT [tagid] FROM [" + BaseConfigs.GetTablePrefix + "topictags] WHERE [tid] = @tid AND [tagid] = [" + BaseConfigs.GetTablePrefix + "tags].[tagid])";

            string sql1 = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topictags] WHERE [tid] = @tid";

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql1, parm);
        }

        public void DeleteRelatedTopics(int topicid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, topicid);
            string sql = string.Format("DELETE * FROM [{0}topictagcaches] WHERE [tid] = @tid OR [linktid] = @tid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        public void AddBonusLog(int tid, int authorid, int winerid, string winnerName, int postid, int bonus, int extid, int isbest)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
                                    DbHelper.MakeInParam("@authorid", (DbType)OleDbType.Integer, 4, authorid),
                                    DbHelper.MakeInParam("@winerid", (DbType)OleDbType.Integer, 4, winerid),
                                    DbHelper.MakeInParam("@winnername", (DbType)OleDbType.VarWChar, 20, winnerName),
                                    DbHelper.MakeInParam("@postid", (DbType)OleDbType.Integer, 4, postid),
                                    DbHelper.MakeInParam("@dateline", (DbType)OleDbType.DBTimeStamp, 4, DateTime.Now),
                                    DbHelper.MakeInParam("@bonus", (DbType)OleDbType.Integer, 4, bonus),
                                    DbHelper.MakeInParam("@extid", (DbType)OleDbType.Integer, 1, extid),
                                    DbHelper.MakeInParam("@isbest", (DbType)OleDbType.Integer, 4, isbest)
                                  };

            string sql = string.Format("INSERT INTO [{0}bonuslog] VALUES(@tid, @authorid, @winerid, @winnername, @postid, @dateline, @bonus, @extid, @isbest)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateMagicValue(int tid, int magic)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@magic", (DbType)OleDbType.Integer, 4, magic),
                                    DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
                                  };

            string sql = string.Format("UPDATE [{0}topics] SET [magic]=@magic WHERE [tid]=@tid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public IDataReader GetPostDebate(int tid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
                                    
                                  };
            string sql = string.Format("SELECT  [pid],[opinion] FROM [{0}postdebatefields]", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);

        }
        public void AddDebateTopic(DebateInfo debatetopic)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, debatetopic.Tid),
                                      DbHelper.MakeInParam("@positiveopinion", (DbType)OleDbType.VarWChar, 200, debatetopic.Positiveopinion),
                                      DbHelper.MakeInParam("@negativeopinion", (DbType)OleDbType.VarWChar, 200, debatetopic.Negativeopinion),
                                      //DbHelper.MakeInParam("@positivecolor", (DbType)SqlDbType.Char, 6, debatetopic.Positivecolor),
                                      //DbHelper.MakeInParam("@negativecolor", (DbType)SqlDbType.Char, 6, debatetopic.Negativecolor),
                                      //DbHelper.MakeInParam("@positivebordercolor", (DbType)SqlDbType.Char, 6, debatetopic.Positivebordercolor),
                                      //DbHelper.MakeInParam("@negativebordercolor", (DbType)SqlDbType.Char, 6, debatetopic.Negativebordercolor),
                                      DbHelper.MakeInParam("@terminaltime", (DbType)OleDbType.DBTimeStamp, 8, debatetopic.Terminaltime)                                
                                  };

            string sql = string.Format(@"INSERT INTO [{0}debates]([tid], [positiveopinion], [negativeopinion], [terminaltime]) VALUES(@tid, @positiveopinion, @negativeopinion, @terminaltime)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateTopicSpecial(int tid, byte special)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
                                        DbHelper.MakeInParam("@special", (DbType)OleDbType.Integer, 1, special)
                                  };

            string sql = string.Format(@"UPDATE [{0}topics] SET [special] = @special WHERE [tid] = @tid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public IDataReader GetDebateTopic(int tid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
                                        
                                  };
            string sql = string.Format("SELECT  * FROM [{0}debates] WHERE [tid]=@tid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);

        }

        public IDataReader GetHotDebatesList(string hotfield, int defhotcount, int getcount)
        {

            string sql = string.Format("SELECT TOP {0} t.*,d.* FROM [{1}topics] AS t LEFT JOIN  [{1}debates] AS d ON t.[tid]=d.[tid] WHERE t.[{2}]>=[{3}] AND t.[special]=4", getcount, BaseConfigs.GetTablePrefix, hotfield, defhotcount);
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }

        public void CreateDebatePostExpand(DebatePostExpandInfo dpei)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, dpei.Tid),
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, dpei.Pid),
                                        DbHelper.MakeInParam("@opinion", (DbType)OleDbType.Integer, 4, dpei.Opinion), 
                                        DbHelper.MakeInParam("@diggs", (DbType)OleDbType.Integer, 4, dpei.Diggs)
                                  };

            //string sql = string.Format("{0}createdebatepostexpand", BaseConfigs.GetTablePrefix);

            string sql2 = "";
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "postdebatefields] VALUES(@tid, @pid, @opinion, @diggs)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

            if (dpei.Opinion == 1)
            {
                sql2 = "UPDATE [" + BaseConfigs.GetTablePrefix + "debates] SET [positivediggs] = [positivediggs] + 1 WHERE [tid] =" + dpei.Tid;
            }
            else if (dpei.Opinion == 2)
            {
                sql2 = "UPDATE [" + BaseConfigs.GetTablePrefix + "debates] SET [negativediggs] = [negativediggs] + 1 WHERE [tid] =" + dpei.Tid;
            }

            DbHelper.ExecuteNonQuery(CommandType.Text, sql2);
        }

        public IDataReader GetRecommendDebates(string tidlist)
        {

            string sql = string.Format("SELECT  t.[tid],t.[title],t.[lastpost],t.[lastposter],d.* FROM [{0}topics] AS t LEFT JOIN [{0}debates] AS d ON t.[tid]=d.[tid] WHERE t.tid IN ({1}) AND t.[special]=4", BaseConfigs.GetTablePrefix, tidlist);
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }

        public void AddCommentDabetas(int tid, int tableid, string commentmsg)
        {
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid)
                                  };
            string selectsql = string.Format("SELECT [message] FROM [{0}posts{1}] WHERE [tid]=@tid AND [layer]=0", BaseConfigs.GetTablePrefix, tableid);
            string message = DbHelper.ExecuteScalarToStr(CommandType.Text, selectsql, parms);

            DbParameter[] parms2 = {
                                       DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
									   DbHelper.MakeInParam("@message",(DbType)OleDbType.VarWChar, 0, message + commentmsg)
                                        
                                  };
            string updatesql = string.Format("UPDATE [{0}posts{1}] SET [message]=@message WHERE [tid]=@tid AND [layer]=0", BaseConfigs.GetTablePrefix, tableid);
            DbHelper.ExecuteNonQuery(CommandType.Text, updatesql, parms2);

        }
        public void AddDebateDigg(int tid, int pid, int field, string ip, UserInfo userinfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid),
                                        DbHelper.MakeInParam("@debates", (DbType)OleDbType.Integer,4, field),
                                        //DbHelper.MakeInParam("@field", (DbType)OleDbType.VarChar,44, Enum.GetName(typeof(CountenanceType),field)),
                                        DbHelper.MakeInParam("@diggerip",(DbType)OleDbType.VarChar,15,ip),
                	                    DbHelper.MakeInParam("@diggdatetime", (DbType)OleDbType.DBTimeStamp, 8, DateTime.Now),
                                        DbHelper.MakeInParam("@diggerid", (DbType)OleDbType.Integer, 4,userinfo.Uid),
                                        DbHelper.MakeInParam("@digger", (DbType)OleDbType.VarWChar, 20,userinfo.Username)
                                        
                                  };
            string str1 = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "debatediggs]([tid],[pid],[digger],[diggerid],[diggerip],[diggdatetime]) VALUES(@tid,@pid,@digger,@diggerid,@diggerip,@diggdatetime)";
            DbHelper.ExecuteNonQuery(CommandType.Text, str1, parms);
            string str2 = "UPDATE  [" + BaseConfigs.GetTablePrefix + "postdebatefields] SET [opinion]=@debates,[diggs]=[diggs]+1 WHERE [tid]=@tid AND [pid]=@pid";
            DbHelper.ExecuteNonQuery(CommandType.Text, str2, parms);
            string str3 = string.Format("UPDATE  [" + BaseConfigs.GetTablePrefix + "debates] SET {0}={0}+1 WHERE [tid]=@tid", Enum.GetName(typeof(DebateType), field));
            DbHelper.ExecuteNonQuery(CommandType.Text, str3, parms);

        }

        public bool AllowDiggs(int pid, int userid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@pid", (DbType)OleDbType.Integer, 4, pid),
                                        DbHelper.MakeInParam("@userid", (DbType)OleDbType.Integer, 4, userid),
                                        
                                  };
            string str1 = "SELECT  COUNT(0) FROM [" + BaseConfigs.GetTablePrefix + "debatediggs] WHERE [pid]=@pid AND [diggerid]=@userid";
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, str1, parms), 0) < 1;
        }

        public IDataReader GetDebatePostList(int tid, int opinion, int pageSize, int pageIndex, string postTableName, PostOrderType postOrderType)
        {
            //string orderField = "diggs";
            //string orderByASC = " ASC";

            //if (postOrderType.Orderfield == PostOrderType.OrderFiled.PostDateTime)
            //    orderField = "postdatetime";

            //if (postOrderType.Orderdirection == OrderDirection.DESC)
            //    orderByASC = " DESC";

            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
            //                            DbHelper.MakeInParam("@opinion", (DbType)OleDbType.Integer, 4, opinion),
            //                            DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pageSize),
            //                            DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageIndex),
            //                            DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 20, postTableName),
            //                            DbHelper.MakeInParam("@orderby", (DbType)OleDbType.VarChar, 20, orderField),
            //                            DbHelper.MakeInParam("@ascdesc", (DbType)OleDbType.VarChar, 5, orderByASC)
            //                      };

            //string sql = string.Format("{0}getdebatepostlist", BaseConfigs.GetTablePrefix);

            //return DbHelper.ExecuteReader(CommandType.StoredProcedure, sql, parms);
            string orderField = "diggs";
            string orderByASC = " ASC";

            if (postOrderType.Orderfield == PostOrderType.OrderFiled.PostDateTime)
                orderField = "postdatetime";

            if (postOrderType.Orderdirection == OrderDirection.DESC)
                orderByASC = " DESC";

            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
                                        DbHelper.MakeInParam("@opinion", (DbType)OleDbType.Integer, 4, opinion),
                                        DbHelper.MakeInParam("@pagesize", (DbType)OleDbType.Integer, 4, pageSize),
                                        DbHelper.MakeInParam("@pageindex", (DbType)OleDbType.Integer, 4, pageIndex),
                                        DbHelper.MakeInParam("@posttablename", (DbType)OleDbType.VarChar, 20, postTableName)
                                  };
            string sql = "";
            int pagetop = (pageIndex - 1) * pageSize;

            if (pageIndex == 1)
            {
                sql = "SELECT " + postTableName + ".pid," + postTableName + ".fid," + postTableName + ".tid," + postTableName + ".parentid," + postTableName + ".layer," + postTableName + ".poster," + postTableName + ".posterid," + postTableName + ".title," + postTableName + ".postdatetime," + postTableName + ".message," + postTableName + ".ip," + postTableName + ".lastedit," + postTableName + ".invisible," + postTableName + ".usesig," + postTableName + ".htmlon," + postTableName + ".smileyoff," + postTableName + ".parseurloff," + postTableName + ".bbcodeoff," + postTableName + ".attachment," + postTableName + ".rate," + postTableName + ".ratetimes," + BaseConfigs.GetTablePrefix + "users.uid," + BaseConfigs.GetTablePrefix + "users.username," + BaseConfigs.GetTablePrefix + "users.nickname," + BaseConfigs.GetTablePrefix + "users.password," + BaseConfigs.GetTablePrefix + "users.secques," + BaseConfigs.GetTablePrefix + "users.spaceid," + BaseConfigs.GetTablePrefix + "users.gender," + BaseConfigs.GetTablePrefix + "users.adminid," + BaseConfigs.GetTablePrefix + "users.groupid," + BaseConfigs.GetTablePrefix + "users.groupexpiry," + BaseConfigs.GetTablePrefix + "users.extgroupids," + BaseConfigs.GetTablePrefix + "users.regip," + BaseConfigs.GetTablePrefix + "users.joindate," + BaseConfigs.GetTablePrefix + "users.ip," + BaseConfigs.GetTablePrefix + "users.lastvisit," + BaseConfigs.GetTablePrefix + "users.lastactivity," + BaseConfigs.GetTablePrefix + "users.lastpost," + BaseConfigs.GetTablePrefix + "users.lastpostid," + BaseConfigs.GetTablePrefix + "users.lastposttitle," + BaseConfigs.GetTablePrefix + "users.posts," + BaseConfigs.GetTablePrefix + "users.digestposts," + BaseConfigs.GetTablePrefix + "users.oltime," + BaseConfigs.GetTablePrefix + "users.pageviews," + BaseConfigs.GetTablePrefix + "users.credits," + BaseConfigs.GetTablePrefix + "users.extcredits1," + BaseConfigs.GetTablePrefix + "users.extcredits2," + BaseConfigs.GetTablePrefix + "users.extcredits3," + BaseConfigs.GetTablePrefix + "users.extcredits4," + BaseConfigs.GetTablePrefix + "users.extcredits5," + BaseConfigs.GetTablePrefix + "users.extcredits6," + BaseConfigs.GetTablePrefix + "users.extcredits7," + BaseConfigs.GetTablePrefix + "users.extcredits8," + BaseConfigs.GetTablePrefix + "users.avatarshowid," + BaseConfigs.GetTablePrefix + "users.email," + BaseConfigs.GetTablePrefix + "users.bday," + BaseConfigs.GetTablePrefix + "users.stgstatus," + BaseConfigs.GetTablePrefix + "users.tpp," + BaseConfigs.GetTablePrefix + "users.ppp," + BaseConfigs.GetTablePrefix + "users.templateid," + BaseConfigs.GetTablePrefix + "users.pmsound," + BaseConfigs.GetTablePrefix + "users.showemail," + BaseConfigs.GetTablePrefix + "users.newpm," + BaseConfigs.GetTablePrefix + "users.newpmcount," + BaseConfigs.GetTablePrefix + "users.accessmasks," + BaseConfigs.GetTablePrefix + "users.onlinestate," + BaseConfigs.GetTablePrefix + "users.newsletter," + BaseConfigs.GetTablePrefix + "userfields.uid," + BaseConfigs.GetTablePrefix + "userfields.website," + BaseConfigs.GetTablePrefix + "userfields.icq," + BaseConfigs.GetTablePrefix + "userfields.qq," + BaseConfigs.GetTablePrefix + "userfields.yahoo," + BaseConfigs.GetTablePrefix + "userfields.msn," + BaseConfigs.GetTablePrefix + "userfields.skype," + BaseConfigs.GetTablePrefix + "userfields.location," + BaseConfigs.GetTablePrefix + "userfields.customstatus," + BaseConfigs.GetTablePrefix + "userfields.avatar," + BaseConfigs.GetTablePrefix + "userfields.avatarwidth," + BaseConfigs.GetTablePrefix + "userfields.avatarheight," + BaseConfigs.GetTablePrefix + "userfields.medals," + BaseConfigs.GetTablePrefix + "userfields.bio," + BaseConfigs.GetTablePrefix + "userfields.signature," + BaseConfigs.GetTablePrefix + "userfields.sightml," + BaseConfigs.GetTablePrefix + "userfields.authstr," + BaseConfigs.GetTablePrefix + "userfields.authtime," + BaseConfigs.GetTablePrefix + "userfields.authflag," + BaseConfigs.GetTablePrefix + "userfields.realname," + BaseConfigs.GetTablePrefix + "userfields.idcard," + BaseConfigs.GetTablePrefix + "userfields.mobile," + BaseConfigs.GetTablePrefix + "userfields.phone FROM(( " + postTableName + " LEFT JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + postTableName + ".posterid) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid]) WHERE " + postTableName + ".invisible=0 AND " + postTableName + ".pid IN (SELECT TOP " + pageSize + " pid FROM " + BaseConfigs.GetTablePrefix + "postdebatefields WHERE opinion=" + opinion + " AND tid=" + tid + ")";
            }
            else
            {
                sql = "SELECT " + postTableName + ".pid," + postTableName + ".fid," + postTableName + ".tid," + postTableName + ".parentid," + postTableName + ".layer," + postTableName + ".poster," + postTableName + ".posterid," + postTableName + ".title," + postTableName + ".postdatetime," + postTableName + ".message," + postTableName + ".ip," + postTableName + ".lastedit," + postTableName + ".invisible," + postTableName + ".usesig," + postTableName + ".htmlon," + postTableName + ".smileyoff," + postTableName + ".parseurloff," + postTableName + ".bbcodeoff," + postTableName + ".attachment," + postTableName + ".rate," + postTableName + ".ratetimes," + BaseConfigs.GetTablePrefix + "users.uid," + BaseConfigs.GetTablePrefix + "users.username," + BaseConfigs.GetTablePrefix + "users.nickname," + BaseConfigs.GetTablePrefix + "users.password," + BaseConfigs.GetTablePrefix + "users.secques," + BaseConfigs.GetTablePrefix + "users.spaceid," + BaseConfigs.GetTablePrefix + "users.gender," + BaseConfigs.GetTablePrefix + "users.adminid," + BaseConfigs.GetTablePrefix + "users.groupid," + BaseConfigs.GetTablePrefix + "users.groupexpiry," + BaseConfigs.GetTablePrefix + "users.extgroupids," + BaseConfigs.GetTablePrefix + "users.regip," + BaseConfigs.GetTablePrefix + "users.joindate," + BaseConfigs.GetTablePrefix + "users.ip," + BaseConfigs.GetTablePrefix + "users.lastvisit," + BaseConfigs.GetTablePrefix + "users.lastactivity," + BaseConfigs.GetTablePrefix + "users.lastpost," + BaseConfigs.GetTablePrefix + "users.lastpostid," + BaseConfigs.GetTablePrefix + "users.lastposttitle," + BaseConfigs.GetTablePrefix + "users.posts," + BaseConfigs.GetTablePrefix + "users.digestposts," + BaseConfigs.GetTablePrefix + "users.oltime," + BaseConfigs.GetTablePrefix + "users.pageviews," + BaseConfigs.GetTablePrefix + "users.credits," + BaseConfigs.GetTablePrefix + "users.extcredits1," + BaseConfigs.GetTablePrefix + "users.extcredits2," + BaseConfigs.GetTablePrefix + "users.extcredits3," + BaseConfigs.GetTablePrefix + "users.extcredits4," + BaseConfigs.GetTablePrefix + "users.extcredits5," + BaseConfigs.GetTablePrefix + "users.extcredits6," + BaseConfigs.GetTablePrefix + "users.extcredits7," + BaseConfigs.GetTablePrefix + "users.extcredits8," + BaseConfigs.GetTablePrefix + "users.avatarshowid," + BaseConfigs.GetTablePrefix + "users.email," + BaseConfigs.GetTablePrefix + "users.bday," + BaseConfigs.GetTablePrefix + "users.stgstatus," + BaseConfigs.GetTablePrefix + "users.tpp," + BaseConfigs.GetTablePrefix + "users.ppp," + BaseConfigs.GetTablePrefix + "users.templateid," + BaseConfigs.GetTablePrefix + "users.pmsound," + BaseConfigs.GetTablePrefix + "users.showemail," + BaseConfigs.GetTablePrefix + "users.newpm," + BaseConfigs.GetTablePrefix + "users.newpmcount," + BaseConfigs.GetTablePrefix + "users.accessmasks," + BaseConfigs.GetTablePrefix + "users.onlinestate," + BaseConfigs.GetTablePrefix + "users.newsletter," + BaseConfigs.GetTablePrefix + "userfields.uid," + BaseConfigs.GetTablePrefix + "userfields.website," + BaseConfigs.GetTablePrefix + "userfields.icq," + BaseConfigs.GetTablePrefix + "userfields.qq," + BaseConfigs.GetTablePrefix + "userfields.yahoo," + BaseConfigs.GetTablePrefix + "userfields.msn," + BaseConfigs.GetTablePrefix + "userfields.skype," + BaseConfigs.GetTablePrefix + "userfields.location," + BaseConfigs.GetTablePrefix + "userfields.customstatus," + BaseConfigs.GetTablePrefix + "userfields.avatar," + BaseConfigs.GetTablePrefix + "userfields.avatarwidth," + BaseConfigs.GetTablePrefix + "userfields.avatarheight," + BaseConfigs.GetTablePrefix + "userfields.medals," + BaseConfigs.GetTablePrefix + "userfields.bio," + BaseConfigs.GetTablePrefix + "userfields.signature," + BaseConfigs.GetTablePrefix + "userfields.sightml," + BaseConfigs.GetTablePrefix + "userfields.authstr," + BaseConfigs.GetTablePrefix + "userfields.authtime," + BaseConfigs.GetTablePrefix + "userfields.authflag," + BaseConfigs.GetTablePrefix + "userfields.realname," + BaseConfigs.GetTablePrefix + "userfields.idcard," + BaseConfigs.GetTablePrefix + "userfields.mobile," + BaseConfigs.GetTablePrefix + "userfields.phone FROM(( " + postTableName + " LEFT JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + postTableName + ".posterid) LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid]=[" + BaseConfigs.GetTablePrefix + "users].[uid]) WHERE " + postTableName + ".invisible=0 AND " + postTableName + ".pid IN (SELECT TOP " + pageSize + " pid FROM " + BaseConfigs.GetTablePrefix + "postdebatefields WHERE opinion=" + opinion + " AND tid=" + tid + " AND pid > (SELECT MAX(pid) FROM (SELECT TOP " + pagetop + " pid FROM " + BaseConfigs.GetTablePrefix + "postdebatefields WHERE opinion=" + opinion + " AND tid=" + tid + " ORDER BY pid) AS tblTmp) ORDER BY pid)";
            }
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        public DataTable GetLastPostList(int fid, int count, string posttablename, string visibleForum)
        {
            //string fidParam = "";
            //if (fid > 0)
            //{
            //    fidParam = " AND ([p].[fid] = " + fid + " OR CHARINDEX('," + fid + ",' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 ) "; //" AND [fid]=" + fid;
            //}

            //if (visibleForum != string.Empty)
            //{
            //    visibleForum = " AND [p].[fid] IN (" + visibleForum + ")";
            //}
            //string sql = string.Format("SELECT TOP {0} [p].[pid], [p].[fid], [p].[tid], [p].[layer], [p].[posterid], [p].[title], [p].[postdatetime], [p].[attachment], [p].[poster], [p].[posterid]  FROM [{1}] AS [p] LEFT JOIN [{2}forums] AS [f] ON [p].[fid] = [f].[fid] LEFT JOIN [{2}topics] AS [t] ON [p].[tid]=[t].[tid] WHERE [p].[layer]>0 AND [t].[closed]<>1 AND  [t].[displayorder] >=0 AND [p].[invisible]<>1 {3} {4} ORDER BY [p].[pid] DESC", count, posttablename, BaseConfigs.GetTablePrefix, fidParam, visibleForum);
            //return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            string fidParam = "";
            if (fid > 0)
            {
                fidParam = " AND ([p].[fid] = " + fid + " OR InStr('," + fid + ",' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 ) "; //" AND [fid]=" + fid;
            }

            if (visibleForum != string.Empty)
            {
                visibleForum = " AND [p].[fid] IN (" + visibleForum + ")";
            }
            string sql = string.Format("SELECT TOP {0} [p].[pid], [p].[fid], [p].[tid], [p].[layer], [p].[posterid], [p].[title], [p].[postdatetime], [p].[attachment], [p].[poster], [p].[posterid]  FROM [{1}] AS [p] LEFT JOIN [{2}forums] AS [f] ON [p].[fid] = [f].[fid] LEFT JOIN [{2}topics] AS [t] ON [p].[tid]=[t].[tid] WHERE [p].[layer]>0 AND [t].[closed]<>1 AND  [t].[displayorder] >=0 AND [p].[invisible]<>1 {3} {4} ORDER BY [p].[pid] DESC", count, posttablename, BaseConfigs.GetTablePrefix, fidParam, visibleForum);
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public IDataReader GetUesrDiggs(int tid, int uid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
                                        DbHelper.MakeInParam("@diggerid", (DbType)OleDbType.Integer, 4, uid)

                                  };
            string sql = "SELECT [pid] FROM [" + BaseConfigs.GetTablePrefix + "debatediggs] WHERE [tid]=@tid AND [diggerid]=@diggerid";
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);

        }

        public int ReviseDebateTopicDiggs(int tid, int debateOpinion)
        {
            //DbParameter[] parms = {
            //                            DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
            //                            DbHelper.MakeInParam("@opinion", (DbType)OleDbType.Integer, 4, debateOpinion),
            //                            DbHelper.MakeOutParam("@count", (DbType)OleDbType.Integer, 4)
            //                      };


            //string sql = string.Format(@"{0}revisedebatetopicdiggs", BaseConfigs.GetTablePrefix);

            //DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, parms);

            //return Convert.ToInt32(parms[2].Value);
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)OleDbType.Integer, 4, tid),
                                        DbHelper.MakeInParam("@opinion", (DbType)OleDbType.Integer, 4, debateOpinion)
                                  };


            //string sql = string.Format(@"{0}revisedebatetopicdiggs", BaseConfigs.GetTablePrefix);
            string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "postdebatefields] WHERE [tid] = @tid AND [opinion] = @opinion";
            int icount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql));

            string sql1 = "";
            if (debateOpinion == 1)
            {
                sql1 = "UPDATE [" + BaseConfigs.GetTablePrefix + "debates] SET [positivediggs]= " + icount + " WHERE [tid] =" + tid;
            }
            else
            {
                sql1 = "UPDATE [" + BaseConfigs.GetTablePrefix + "debates] SET [negativediggs]= " + icount + " WHERE [tid] =" + @tid;
            }
            DbHelper.ExecuteNonQuery(CommandType.Text, sql1);

            return icount;
        }

        public IDataReader GetDebatePostDiggs(string pidlist)
        {
            if (!Utils.IsNumericList(pidlist))
                return null;

            string sql = string.Format("SELECT [pid],[diggs] FROM [{0}postdebatefields] WHERE [pid] IN ({1})", BaseConfigs.GetTablePrefix, pidlist);

            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }

        public int GetLastPostTid(ForumInfo foruminfo, string visibleForum)
        {
            string fidParam = "";
            if (foruminfo.Fid > 0)
            {
                fidParam = "AND ([t].[fid] = " + foruminfo.Fid + " OR InStr('," + foruminfo.Fid + ",' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 )  ";
            }


            if (!Utils.StrIsNullOrEmpty(visibleForum))
            {
                fidParam += " AND [t].[fid] IN (" + visibleForum + ")";
            }

            string sql = string.Format("SELECT TOP 1 [tid] FROM [{0}topics] AS t LEFT JOIN [{0}forums] AS f  ON [t].[fid] = [f].[fid] WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0  {1}  ORDER BY [t].[lastpost] DESC",
                                       BaseConfigs.GetTablePrefix,
                                       fidParam);

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql), -1);
        }

        public void SetPostsBanned(int tableid, string postlistid, int invisible)
        {
            string sql = String.Format("UPDATE [{0}] SET [invisible]={1} WHERE [PID] IN ({2})", BaseConfigs.GetTablePrefix + "posts" + tableid, invisible, postlistid);
            DbHelper.ExecuteNonQuery(sql);
        }

        public void SetTopicsBump(string tidlist, int lastpostid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@lastpostid", (DbType)OleDbType.Integer, 4, lastpostid)
                                  };
            string sql = string.Format("UPDATE {0} SET [lastpostid]={1} WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix + "topics", lastpostid != 0 ? "@lastpostid" : "0 - [lastpostid]", tidlist);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int GetPostId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "postid] ([postdatetime]) VALUES(NOW());SELECT SCOPE_IDENTITY()"), -1);
        }

        public DataTable GetPostList(string postlist, int tableid)
        {

            string commandText = string.Format("SELECT * FROM [{0}posts{1}] WHERE [pid] IN ({2})", BaseConfigs.GetTablePrefix, tableid, postlist);
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

        public string GetTopicFilterCondition(string filter)
        {
            switch (filter.ToLower().Trim())
            {
                case "poll": return " AND [special] = 1 ";
                case "reward": return " AND ([special] = 2 OR [special] = 3) ";
                case "rewarded": return " AND [special] = 3 ";
                case "rewarding": return " AND [special] = 2 ";
                case "debate": return " AND [special] = 4 ";
                case "digest": return " AND [digest] > 0 ";
                default: return "";
            }
        }


        public int GetDebatesPostCount(int tid, int debateOpinion)
        {
            string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "postdebatefields] WHERE [tid] = " + tid + " AND [opinion] =" + debateOpinion;
            return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, sql), 0);

        }



        public void DeleteDebatePost(int tid, string opinion, int pid)
        {
            int postdebatefieldscount = DbHelper.ExecuteNonQuery("DELETE * FROM " + BaseConfigs.GetTablePrefix + "postdebatefields WHERE [pid]=" + pid);
            int debatediggscount = DbHelper.ExecuteNonQuery("DELETE * FROM " + BaseConfigs.GetTablePrefix + "debatediggs WHERE [pid]=" + pid);

            // Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT COUNT(1) FROM " + BaseConfigs.GetTablePrefix + "postdebatefields WHERE [PID]=" + pid), -1);
            //Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT COUNT(1) FROM " + BaseConfigs.GetTablePrefix + "debatediggs WHERE [PID]=" + pid), -1);
            DbHelper.ExecuteNonQuery(string.Format("UPDATE " + BaseConfigs.GetTablePrefix + "DEBATES SET {0}={0}-{1} WHERE [TID]={2}", opinion, postdebatefieldscount + debatediggscount, tid));
        }

        private string GetAttentionSql(string keyword)
        {

            string sqlstring = " And ([title] LIKE '%" + RegEsc(keyword) + "%' or  [poster] LIKE '%" + RegEsc(keyword) + "%' OR [lastpost] LIKE '%" + RegEsc(keyword) + "%' OR [postdatetime] LIKE '%" + RegEsc(keyword) + "%')";
            return sqlstring;
        }


        public IDataReader GetAttentionTopics(string fidlist, int tpp, int pageid, string keyword)
        {

            string condition = string.Empty;
            if (keyword != "")
            {

                condition += GetAttentionSql(keyword);
            }

            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tpp", (DbType)OleDbType.Integer, 4, tpp),
                                        DbHelper.MakeInParam("@fid", (DbType)OleDbType.VarChar, 255, fidlist)
                                        
                                  };

            //return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getattentiontopics", parms);
            string sqlstr = string.Empty;
            int pagetop = (pageid - 1) * tpp;
            if (pageid == 1)
            {
                sqlstr = "SELECT TOP  "+ tpp +" * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]>=0  AND [attention]=1";
                if (fidlist != "0")
                {
                    sqlstr += "  AND [fid] IN ("+ fidlist +")";
                }

                if (condition != "")
                {
                    sqlstr += condition;
                }

            }
            else
            {
                sqlstr = "'SELECT TOP  " + tpp + "  * FROM [" + BaseConfigs.GetTablePrefix + "topics]  WHERE [tid] < (SELECT MIN([tid])  FROM (SELECT TOP " + pagetop + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics]   WHERE [displayorder]>=0  AND [attention]=1";
                if (fidlist != "0")
                {
                    sqlstr += "  AND [fid] IN (" + fidlist + ")";
                }
                if (condition != "")
                {
                    sqlstr += condition;
                }

                sqlstr += "   ORDER BY [tid] DESC)  AS T) ";
                if (fidlist != "0")
                {
                    sqlstr += "  AND [fid] IN (" + fidlist + ")";
                }
                if (condition != "")
                {
                    sqlstr += condition;
                }

                sqlstr += "   AND [displayorder]>=0  AND [attention]=1   ORDER BY [tid] DESC";
            }

           return DbHelper.ExecuteReader(CommandType.Text, sqlstr, parms);


        }

        public int GetAttentionTopicCount(string fidlist, string keyword)
        {
            string condition = string.Empty;
            if (keyword != "")
            {

                condition += GetAttentionSql(keyword);
            }
            string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]>=0 AND [attention]=1";
            if (fidlist != "0")
            {
                sql += " AND [fid] IN (" + fidlist + ")";

            }

            return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, sql + condition), 0);

        }

        public void UpdateAttentionTopics(string topiclist, int attention)
        {
            DbParameter[] parms = {
                                       
                                        DbHelper.MakeInParam("@attention", (DbType)OleDbType.Integer, 4, attention)
                                  };
            string sql = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [attention]=@attention WHERE [tid] IN ({0})", topiclist);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateAttentionTopics(int attention, string fidlist, string datetime)
        {
            DbParameter[] parms = {
                                       
                                        DbHelper.MakeInParam("@attention", (DbType)OleDbType.Integer, 4, attention),
                                        DbHelper.MakeInParam("@datetime", (DbType)OleDbType.DBTimeStamp, 8, datetime)
                                  };
            string sqlstr = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [attention]=@attention WHERE";
            if (fidlist != "0")
            {
                sqlstr += " ([fid] IN ({0})) AND";

            }
            sqlstr += "  [postdatetime]<@datetime";
            string sql = string.Format(sqlstr, fidlist);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

        }


        public IDataReader GetNoUsedAttachmentListByTid(int userid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@userid", (DbType)OleDbType.VarWChar, 255, userid)
                                  };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [aid], [attachment] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [uid]= @userid AND [tid]=0 AND [pid]=0", parms);
        }


        public int DeleteNoUsedForumAttachment()
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]= 0 AND [pid]=0 AND DATEDIFF(\"n\", postdatetime, NOW()) > 30 ");
        }

        public IDataReader GetNoUsedForumAttachment()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]= 0 AND [pid]=0 AND DATEDIFF(\"n\", postdatetime, NOW()) > 30 ");
        }

        public IDataReader GetAttachPaymentLogByAid(int aid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@aid", (DbType)OleDbType.Integer, 4, aid)
                                  };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachpaymentlog] WHERE [aid]= @aid", parms);
        }

        public IDataReader GetAttachPaymentLogByUid(string attachidlist, int uid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid] FROM [" + BaseConfigs.GetTablePrefix + "attachpaymentlog] WHERE [aid] IN ({0})  AND [uid] = {1}", attachidlist, uid));
        }
        

    }
}
#endif
