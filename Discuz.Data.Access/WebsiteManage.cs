﻿#if NET1
#else
using System;
using System.Data;
using System.Data.Common;
using Discuz.Config;
using System.Data.OleDb;

namespace Discuz.Data.Access
{
    public partial class DataProvider : IDataProvider
    {
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

            int pageTop = (currentPage - 1) * pageSize;
            if (currentPage == 1)
            {
                sql =
                    string.Format(
                        "SELECT TOP {0} t.*,f.[name] FROM ([{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid) LEFT JOIN (SELECT * FROM [{1}forumfields] WHERE [viewperm] IS NULL OR [viewperm]='' OR InStr(','+[viewperm]+',',',7,')<>0) ff ON f.[fid]=ff.[fid] WHERE [closed]<>1 AND [status]=1 AND [password]='' {2} ORDER BY [tid] DESC",
                        pageSize, BaseConfigs.GetTablePrefix, condition);
            }
            else
            {
                sql =
                    string.Format(
                        "SELECT TOP {0} t.*,f.[name] FROM ([{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid) LEFT JOIN (SELECT * FROM [{1}forumfields] WHERE [viewperm] IS NULL OR [viewperm]='' OR InStr(','+[viewperm]+',',',7,')<>0) ff ON f.[fid]=ff.[fid] WHERE [closed]<>1 AND [status]=1 AND [password]='' "
                + "AND [tid]<(SELECT MIN([tid]) FROM (SELECT TOP {2} [tid] FROM ([{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid) LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] WHERE [closed]<>1 AND [status]=1 AND [password]='' {3} ORDER BY [tid] DESC) AS tblTmp){3} ORDER BY [tid] DESC",
                        pageSize, BaseConfigs.GetTablePrefix, pageTop, condition);
            }
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        private static string GetCondition(int forumid, string posterlist, string keylist, string startdate, string enddate)
        {
            string condition = "";
            if (forumid != 0)
            {
                condition += " AND f.[fid]=" + forumid;
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
                condition += " AND [postdatetime]>=#" + startdate + " 00:00:00#";
            }
            if (enddate != "")
            {
                condition += " AND [postdatetime]<=#" + enddate + " 23:59:59#";
            }
            return condition;
        }

        public int GetTopicListCountByCondition(string postname,int forumid, string posterlist, string keylist, string startdate, string enddate)
        {
            //string sql = string.Format("SELECT COUNT(1) FROM [{0}topics] t LEFT JOIN [{0}forums] f ON t.fid=f.fid LEFT JOIN [{0}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]=''", BaseConfigs.GetTablePrefix);
            //string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);
            //DbParameter[] parms = GetParms(startdate, enddate);

            //if (condition != "")
            //    sql += condition;
            //int mintid = DatabaseProvider.GetInstance().GetMinPostTableTid(postname);
            //int maxtid = DatabaseProvider.GetInstance().GetMaxPostTableTid(postname);
            //sql += " AND [tid]>=" + mintid + " AND [tid]<=" + maxtid;
            //return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString());
            string sql = string.Format("SELECT COUNT(1) FROM ([{0}topics] as t LEFT JOIN [{0}forums] as f ON t.fid=f.fid) LEFT JOIN (select * from [{0}forumfields] where [viewperm] IS NULL OR [viewperm]='' OR InStr(','+[viewperm]+',',',7,')<>0) as ff ON f.[fid]=ff.[fid] WHERE [closed]<>1 AND [status]=1 AND [password]=''", BaseConfigs.GetTablePrefix);
            string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);
            if (condition != "")
                sql += condition;
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
        }

        public DataTable GetTopicListByTidlist(string posttableid, string tidlist)
        {
            //string sql =
            //    string.Format(
            //        "SELECT p.*,t.[closed] FROM [{0}posts{1}] p LEFT JOIN [{0}topics] t ON p.[tid]=t.[tid] WHERE [layer]=0 AND [closed]<>1 and p.[tid] IN ({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),p.[tid]),'{2}')",
            //        BaseConfigs.GetTablePrefix, posttableid, tidlist);
            ////string sql = string.Format("SELECT * FROM [{0}topics] WHERE [closed]<>1 AND [tid] IN ({1}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[tid]),'{1}')",
            ////        BaseConfigs.GetTablePrefix,tidlist);
            //return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            string sql = "SELECT p.*,t.[closed] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] p LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t ON p.tid=t.tid WHERE layer=0 "
            + "AND [closed]<>1 and p.[tid] IN (" + tidlist + ") ORDER BY instr('" + tidlist + "',p.[tid])";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void SetBanip(int ip1,int ip2,int ip3,int ip4,string admin,int dateline,int experation)
        { 
            DbParameter[] parameters = {
                                            DbHelper.MakeInParam("@ip1",(DbType)OleDbType.Integer,4,ip1),
                                            DbHelper.MakeInParam("@ip2",(DbType)OleDbType.Integer,4,ip2),
                                            DbHelper.MakeInParam("@ip3",(DbType)OleDbType.Integer,4,ip3),
                                            DbHelper.MakeInParam("@ip4",(DbType)OleDbType.Integer,4,ip4),
                                            DbHelper.MakeInParam("@amdin",(DbType)OleDbType.VarChar,50,admin),
                                            DbHelper.MakeInParam("@dateline",(DbType)OleDbType.Integer,4,dateline),
                                            DbHelper.MakeInParam("@experation",(DbType)OleDbType.Integer,4,experation),
                                       };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "bann](ip1,ip2,ip3,ip4,admin,dateline,experation) VALUES(@ip1,@ip2,@ip3,@ip4,@admin,@dateline,@experation)", parameters);
        }



        #region 前台聚合页相关函数

        public DataTable GetWebSiteAggForumTopicList(string showtype, int topnumber)
        {
            DataTable topicList = new DataTable();
            switch (showtype)
            {
                default://按版块查
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies] FROM [" + BaseConfigs.GetTablePrefix + "forums] f LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t  ON f.[lasttid] = t.[tid] WHERE f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') AND t.[displayorder]>=0").Tables[0]; break;
                    }
                case "1"://按最新主题查
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name] FROM [" + BaseConfigs.GetTablePrefix + "topics] t LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid] = f.[fid] WHERE t.[displayorder]>=0 AND f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') ORDER BY t.[postdatetime] DESC").Tables[0]; break;
                    }
                case "2"://按精华主题查
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name] FROM [" + BaseConfigs.GetTablePrefix + "topics] t LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid] = f.[fid] WHERE t.[digest] >0 AND f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') ORDER BY t.[digest] DESC").Tables[0]; break;
                    }
                case "3"://按版块查
                    {
                        topicList = DbHelper.ExecuteDataset("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies] FROM [" + BaseConfigs.GetTablePrefix + "forums] f LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t  ON f.[lasttid] = t.[tid] WHERE f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') AND t.[displayorder]>=0").Tables[0]; break;
                    }
            }
            return topicList;
        }


        public DataTable GetWebSiteAggHotForumList(int topnumber)
        {
            return DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " [fid], [name], [topics] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [status]=1 AND [layer]> 0 AND [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [password]='') ORDER BY [topics] DESC, [posts] DESC, [todayposts] DESC").Tables[0];
        }
        #endregion
  
    }
}
#endif