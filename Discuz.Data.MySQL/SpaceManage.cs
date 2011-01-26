#if NET1
#else
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Discuz.Data;
using System.Data;
using System.Data.Common;
using Discuz.Entity;
using MySql.Data.MySqlClient;
using Discuz.Config;
using Discuz.Common;

namespace Discuz.Data.MySql
{
    public partial class DataProvider : IDataProvider 
    {
        #region Space 个人数据操作

        public void AddAlbumCategory(AlbumCategoryInfo aci)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("?title", (DbType)MySqlDbType.String, 50, aci.Title),
                                        DbHelper.MakeInParam("?description", (DbType)MySqlDbType.String, 300, aci.Description),
                                        DbHelper.MakeInParam("?displayorder", (DbType)MySqlDbType.Int32, 4, aci.Displayorder)
                                };

            string sql = string.Format(@"INSERT INTO `{0}albumcategories`(`title`, `description`, `albumcount`, `displayorder`) VALUES(?title, ?description, 0, ?displayorder)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        public void UpdateAlbumCategory(AlbumCategoryInfo aci)
        {
            DbParameter[] parms = {

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
            DbParameter parm = DbHelper.MakeInParam("?albumcateid", (DbType)MySqlDbType.Int32, 4, albumcateid);

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
            DbParameter[] prams = {
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
                DbParameter parm = DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4, postid);
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
                DbParameter parm = DbHelper.MakeInParam("?postid", (DbType)MySqlDbType.Int32, 4, postid);
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
            DbParameter[] prams =
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

            DbParameter[] prams1 =
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
            DbParameter[] prams =
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
            DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
                DbParameter[] prams =
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
              DbParameter[] prams =
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
                DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            //    DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
            //			DbParameter[] prams =
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
                DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
            DbParameter parm = DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid);
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
            DbParameter[] parms = {
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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
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
            //try
            //{
            DbParameter[] prams =
				{
					DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4,photoinfo.Userid),
                    DbHelper.MakeInParam("?username", (DbType)MySqlDbType.VarChar, 20, photoinfo.Username),
					DbHelper.MakeInParam("?title", (DbType)MySqlDbType.VarChar, 20,photoinfo.Title),
					DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4,photoinfo.Albumid),
					DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.VarChar, 255,photoinfo.Filename),
					DbHelper.MakeInParam("?attachment", (DbType)MySqlDbType.VarChar, 255,photoinfo.Attachment),
					DbHelper.MakeInParam("?filesize", (DbType)MySqlDbType.Int32, 4,photoinfo.Filesize),
					DbHelper.MakeInParam("?description", (DbType)MySqlDbType.VarChar, 200,photoinfo.Description),
                    DbHelper.MakeInParam("?isattachment",(DbType)MySqlDbType.Int32,4,photoinfo.IsAttachment),
                    DbHelper.MakeInParam("?commentstatus", (DbType)MySqlDbType.Int32, 1, (byte)photoinfo.Commentstatus),
                    DbHelper.MakeInParam("?tagstatus", (DbType)MySqlDbType.Int32, 1, (byte)photoinfo.Tagstatus)
					//DbHelper.MakeInParam("?creatdatetime", (DbType)MySqlDbType.Datetime, 8,spaceAlbum.Createdatetime)
				};
            string sqlstring = String.Format("INSERT INTO `{0}photos` (`userid`, `username`,`title`, `albumid`, `filename`, `attachment`, `filesize`, `description`,`postdate`,`isattachment`,`commentstatus`, `tagstatus`) VALUES (?userid,?username, ?title, ?albumid, ?filename, ?attachment, ?filesize, ?description,NOW(),?isattachment, ?commentstatus, ?tagstatus)", BaseConfigs.GetTablePrefix);

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
            DbParameter[] parms = {

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
            DbParameter[] prams =
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
            DbParameter[] prams =
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
            DbParameter parm = DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4, photoid);
            string sql = string.Format("UPDATE `{0}photos` SET `views`=`views`+1 WHERE `photoid`=?photoid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        public int GetSpacePhotosCount(int albumid)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
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
            DbParameter[] prams =
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
            DbParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("?moduledefid", (DbType)MySqlDbType.Int32, 4, moduleDefId),
			};

            string sql = string.Format(@"DELETE FROM `{0}_spacemoduledefs` WHERE `moduledefid`=?moduledefid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(sql, parms);
        }


        public int GetModuleDefIdByUrl(string url)
        {
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabId),
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
			};

            string sql = string.Format(@"DELETE FROM `{0}spacetabs` WHERE `tabid`=?tabid AND `uid`=?uid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(sql, parms);
        }

        public int GetTabInfoCountByUserId(int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid),
			};
            string commandText = string.Format(@"SELECT COUNT(1) FROM `{0}spacetabs` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0);
        }

        public bool SetTabTemplate(int tabid, int uid, string template)
        {
            DbParameter[] parms =
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
            DbParameter[] parms =
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
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
			};
            string commandText = string.Format("UPDATE `{0}spaceconfigs` SET `defaulttab`=0 WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

        }
        public void SetDefaultTab(int userid, int tabid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
				DbHelper.MakeInParam("?tabid", (DbType)MySqlDbType.Int32, 4, tabid)
			};
            string commandText = string.Format("UPDATE `{0}spaceconfigs` SET `defaulttab`=?tabid WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void SetSpaceTheme(int userid, int themeid, string themepath)
        {
            DbParameter[] parms =
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
        private bool RunExecuteSql(string sql, DbParameter[] parms)
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
        private IDataReader RunSelectSql(string sql, DbParameter[] parms)
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
            DbParameter[] parms = {
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
            DbParameter parm = DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name);
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE name=?name", parm), 0) > 0;
        }

        public bool IsThemeExist(string name, int themeid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name),
                                    DbHelper.MakeInParam("?id", (DbType)MySqlDbType.Int32, 4, themeid)
                                };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(*) FROM `" + BaseConfigs.GetTablePrefix + "spacethemes` WHERE `name`=?name AND themeid<>?id", parms), 0) > 0;
        }

        //public void AddSpaceTheme(string name)
        //{
        //    DbParameter parm = DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name);
        //    DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO `" + BaseConfigs.GetTablePrefix + "spacethemes`(`name`, `type`) VALUES(?name,0)", parm);
        //}

        public void AddSpaceTheme(string directory, string name, int type, string author, string createdate, string copyright)
        {
            DbParameter[] parms = {
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
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("?name", (DbType)MySqlDbType.VarString, 50, name),
                                        DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeid)

                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE `" + BaseConfigs.GetTablePrefix + "spacethemes` SET name=?name WHERE themeid=?themeid", parms);
        }

        public void DeleteTheme(int themeid)
        {
            DbParameter parm = DbHelper.MakeInParam("?themeid", (DbType)MySqlDbType.Int32, 4, themeid);
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
            DbParameter[] parms = {
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
            DbParameter parm = DbHelper.MakeInParam("?vaildDays", (DbType)MySqlDbType.Int32, 4, vaildDays);
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
            DbParameter[] parms = {
                DbHelper.MakeInParam("?photoid", (DbType)MySqlDbType.Int32, 4, photoid),
                DbHelper.MakeInParam("?count", (DbType)MySqlDbType.Int32, 4, count),
            };
            string commandText = string.Format("UPDATE `{0}photos` SET `comments`=`comments`+?count WHERE `photoid`=?photoid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetPhotosByAlbumid(int albumid)
        {
            DbParameter parm = DbHelper.MakeInParam("?albumid", (DbType)SqlDbType.Int, 4, albumid);
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

            DbParameter[] parms = {
                                        DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
                                        DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid)
                                    };

            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }


        public void UpdateUserSpaceRewriteName(int userid, string rewritename)
        {
            DbParameter[] parms = {
            DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.String, 100, rewritename),
            DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
            };

            string sql = string.Format("UPDATE `{0}spaceconfigs` SET `rewritename`=?rewritename WHERE `userid`=?userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public bool IsRewritenameExist(string rewriteName)
        {
            DbParameter parm = DbHelper.MakeInParam("?rewritename", (DbType)MySqlDbType.String, 100, rewriteName);
            string sql = string.Format("SELECT COUNT(1) FROM `{0}spaceconfigs` WHERE `rewritename`=?rewritename", BaseConfigs.GetTablePrefix);
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0) > 0;
        }

        public string GetUidBySpaceid(string spaceid)
        {
            DbParameter parm = DbHelper.MakeInParam("?spaceid", (DbType)MySqlDbType.Int32, 4, spaceid);
            string sql = string.Format("SELECT `userid` FROM `{0}spaceconfigs` WHERE `spaceid`=?spaceid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteScalar(CommandType.Text, sql, parm).ToString();
        }


        public string GetSpaceattachmentsAidListByUid(int uid)
        {
            string aidlist = "";
            DbParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);
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
            DbParameter[] parms =
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
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid),
			};

            string sql = string.Format(@"DELETE FROM `{0}spacetabs` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(sql, parms);
        }


        public void DeleteSpaceByUid(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);
            string sql = string.Format("DELETE FROM `{0}spaceconfigs` WHERE `userid`=?uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        public DataTable SpacePhotosList(int albumid)
        {
            DbParameter parm = DbHelper.MakeInParam("?albumid", (DbType)MySqlDbType.Int32, 4, albumid);
            string sql = sql = "SELECT * FROM `" + BaseConfigs.GetTablePrefix + "photos` WHERE `albumid`=?albumid ORDER BY `photoid` ASC";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
        }


        public void UpdateCustomizePanelContent(int moduleid, int userid, string modulecontent)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),                
                DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
                DbHelper.MakeInParam("?modulecontent", (DbType)MySqlDbType.VarChar, 0, modulecontent)
            };

            string sql = string.Format("UPDATE `{0}spacecustomizepanels` SET `panelcontent`=?modulecontent WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public bool ExistCustomizePanelContent(int moduleid, int userid)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
                DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
            };

            string sql = string.Format("SELECT COUNT(1) FROM `{0}spacecustomizepanels` WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0) > 0;
        }

        public void AddCustomizePanelContent(int moduleid, int userid, string modulecontent)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
                DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid),
                DbHelper.MakeInParam("?modulecontent", (DbType)MySqlDbType.VarChar, 0, modulecontent)
            };

            string sql = string.Format("INSERT INTO `{0}spacecustomizepanels`(`moduleid`, `userid`, `panelcontent`) VALUES(?moduleid, ?userid, ?modulecontent)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public object GetCustomizePanelContent(int moduleid, int userid)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
                DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
            };

            string sql = string.Format("SELECT `panelcontent` FROM `{0}spacecustomizepanels` WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
        }

        public void DeleteCustomizePanelContent(int moduleid, int userid)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("?moduleid", (DbType)MySqlDbType.Int32, 4, moduleid),
                DbHelper.MakeInParam("?userid", (DbType)MySqlDbType.Int32, 4, userid)
            };

            string sql = string.Format("DELETE FROM `{0}spacecustomizepanels` WHERE `moduleid`=?moduleid AND `userid`=?userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public IDataReader GetModulesByUserId(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, uid);

            string sql = string.Format("SELECT * FROM `{0}spacemodules` WHERE `uid`=?uid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }


        public int GetSpaceCustomizePanelCount(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);

            string sql =
                string.Format(
                    "SELECT COUNT(1) FROM `{0}spacemodules` WHERE `uid`=?uid AND `moduleurl`='builtin_customizepanel.xml'",
                    BaseConfigs.GetTablePrefix);

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0);
        }

        public IDataReader GetSpaceCustomizePanelList(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int32, 4, userid);

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
            DbParameter[] parms = {
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
            DbParameter[] parms = {
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
            DbParameter[] parms = {
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
    }
}
#endif