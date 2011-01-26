using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.EntLib.SphinxClient
{  

    public class SphinxSqlService : Discuz.Config.SphinxConfig.ISqlService
    {
        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="talbeName">当前分表名称</param>
        /// <param name="tid">主题ID</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="fid">所属版块ID</param>
        /// <param name="posterid">发帖人ID</param>
        /// <param name="postdatetime">发帖日期</param>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public int CreatePost(string tableName, int pid, int tid, int fid, int posterid, string postdatetime, string title, string message)
        {
            DbParameter[] prams = {									
                                       MakeParam("?pid", (DbType)MySqlDbType.Int32, 4, ParameterDirection.Input, pid),
                                       MakeParam("?tid", (DbType)MySqlDbType.Int32, 4, ParameterDirection.Input, tid),
                                       MakeParam("?fid", (DbType)MySqlDbType.Int16, 2, ParameterDirection.Input, fid),
                                       MakeParam("?posterid", (DbType)MySqlDbType.Int32, 4, ParameterDirection.Input, posterid),
                                       MakeParam("?postdatetime", (DbType)MySqlDbType.VarString, 20, ParameterDirection.Input, postdatetime),
                                       MakeParam("?title", (DbType)MySqlDbType.VarString, 60, ParameterDirection.Input, title),
                                       MakeParam("?message", (DbType)MySqlDbType.Text, 0, ParameterDirection.Input, message)
								   };

            string commandText = "SET NAMES utf8;INSERT INTO  `" + tableName + "` (`pid`,`tid`,`fid`,`posterid`,`postdatetime`,`title`,`message`) VALUES(?pid,?tid,?fid,?posterid,?postdatetime,?title,?message)";

            try
            {
                return ExecuteNonQuery(commandText, prams);
            }
            catch 
            {
                try
                {
                    CreatePostTable(tableName);

                    return ExecuteNonQuery(commandText, prams);
                }
                catch 
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 创建Sphinx的数据表（目前只支持mysql数据库类型）
        /// </summary>
        /// <param name="tableName">当前分表名称</param>
        /// <returns></returns>
        public bool CreatePostTable(string tableName)
        {
            EntLibConfigInfo entLibConfigInfo = EntLibConfigs.GetConfig();
            //数据库MY.ini文件中也要指定或在安装实例是即指定为utf-8
            //SET NAMES utf8;CREATE DATABASE IF NOT EXISTS test;
            string commandText = string.Format("SET NAMES utf8;" +
                                    "CREATE TABLE IF NOT EXISTS `{0}` (" +
                                    "`pid` INT(11) NOT NULL ," +
                                    "`tid` INT(11) NOT NULL ," +
                                    "`fid` INT(11) NOT NULL ," +
                                    "`posterid` INT(11) NOT NULL ," +
                                    "`postdatetime` DATETIME NOT NULL," +
                                    "`title` varchar(60) NOT NULL," +
                                    "`message` TEXT NOT NULL," +
                                    "PRIMARY KEY  (`pid`)" +
                                    ") DEFAULT CHARSET=utf8;",
                                    tableName);

            ExecuteNonQuery(commandText, null);
            return true;
        }


        /// <summary>
        /// 更新Sphinx表帖子
        /// </summary>
        /// <param name="tableName">当前分表名称</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="tid">主题ID</param>     
        /// <param name="fid">所属版块ID</param>
        /// <param name="posterid">发帖人</param>
        /// <param name="postdatetime">发帖日期</param>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public int UpdatePost(string tableName, int pid, int tid, int fid, int posterid, string postdatetime, string title, string message)
        {
            DbParameter[] prams = {									
                                       MakeParam("?pid", (DbType)MySqlDbType.Int32, 4, ParameterDirection.Input, pid),
                                       MakeParam("?tid", (DbType)MySqlDbType.Int32, 4, ParameterDirection.Input, tid),
                                       MakeParam("?fid", (DbType)MySqlDbType.Int16, 2, ParameterDirection.Input, fid),
                                       MakeParam("?posterid", (DbType)MySqlDbType.Int32, 4, ParameterDirection.Input, posterid),
                                       MakeParam("?postdatetime", (DbType)MySqlDbType.VarString, 20, ParameterDirection.Input, postdatetime),
                                       MakeParam("?title", (DbType)MySqlDbType.VarString, 60, ParameterDirection.Input, title),
                                       MakeParam("?message", (DbType)MySqlDbType.Text, 0, ParameterDirection.Input, message)
								   };

            string commandText = "SET NAMES utf8;UPDATE `" + tableName + "` SET  `tid` = ?tid, `fid` = ?fid, `posterid`= ?posterid,`postdatetime` = ?postdatetime,`title` = ?title, `message` = ?message WHERE `pid` = ?pid";

            try
            {
                int result = ExecuteNonQuery(commandText, prams);
                if (result == 0)//如果为0表示当前表中没有该记录，则插入该记录
                {
                    result = CreatePost(tableName, pid, tid, fid, posterid, postdatetime, title, message);
                }
                return result;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取要搜索的主题ID(Tid)信息
        /// </summary>
        /// <param name="posterId">发帖者id</param>
        /// <param name="searchForumId">搜索版块id</param>
        /// <param name="resultOrder">结果排序方式</param>
        /// <param name="resultOrderType">结果类型类型</param>
        /// <param name="searchTime">搜索时间</param>
        /// <param name="searchTimeType">搜索时间类型</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <param name="strKeyWord">关键字</param>
        /// <returns></returns>
        public string GetSearchPostContentSQL(int posterId, string searchForumId, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, int postTableId, StringBuilder strKeyWord)
        {
            if (string.IsNullOrEmpty(strKeyWord.ToString()))
                return "";

            SphinxClient cli = new SphinxClient(sphinxConfig.SphinxServiceHost, sphinxConfig.SphinxServicePort);
            cli.SetLimits(0, 300000);//30万条数据（单个帖子分表一般不应超过30万条记录）
            cli.SetClientTimeOut(EntLibConfigs.GetConfig().Sphinxconfig.SphClientTimeOut);

            //Dictionary<string, int> indexWeight = new Dictionary<string, int>();               
            //indexWeight.Add("message",5);
            //indexWeight.Add("title",1);
            //cli.SetIndexWeights(indexWeight);

            if (searchForumId != "")
                cli.SetFilter("fid", TypeConverter.StrToInt(searchForumId), false);

            if (posterId != -1)
                cli.SetFilter("posterid", posterId, false);               
 
            if (searchTime != 0)
            {
                if (searchTimeType == 1) //"<" 多少天之前 即： searchTimeType == 1 
                    cli.SetFilterRange("postdatetime", DateTimeHelper.ConvertToUnixTimestamp(DateTime.Now.AddYears(-10)), DateTimeHelper.ConvertToUnixTimestamp(DateTime.Now.AddDays(searchTime)), false);
                else  //">" 多少天之内 
                    cli.SetFilterRange("postdatetime", DateTimeHelper.ConvertToUnixTimestamp(DateTime.Now.AddDays(searchTime)), DateTimeHelper.ConvertToUnixTimestamp(DateTime.Now), false);          
            }

            if(resultOrderType == 1) //" ASC"
                cli.SetGroupBy("tid", GroupBy.SPH_GROUPBY_ATTR, "tid ASC");
            else // DESC           
                cli.SetGroupBy("tid", GroupBy.SPH_GROUPBY_ATTR);
          
            cli.SetGroupDistinct("tid");
            
            string orderBy = "";
            switch (resultOrder)
            {
                case 1://tid
                    orderBy = "tid";
                    break;
                case 2://replies
                    orderBy = "tid";
                    break;
                case 3://views
                    orderBy = "tid";
                    break;
                default:
                    orderBy = "postdatetime";
                    break;
            }
            cli.SetSortMode(SortMode.SPH_SORT_EXTENDED, orderBy + (resultOrderType == 1 ? " ASC" : " DESC"));
            
            foreach (string keyword in Utils.SplitString(strKeyWord.ToString(), " "))
            {
                if (string.IsNullOrEmpty(EntLibConfigs.GetConfig().Sphinxconfig.IndexSectionName))
                {
                    cli.AddQuery(keyword, BaseConfigs.GetTablePrefix + "posts" + postTableId);//搜索主索引
                    cli.AddQuery(keyword, BaseConfigs.GetTablePrefix + "posts" + postTableId + "_stem");//搜索增量索引
                }
                else //当有值时，比如"dnt_posts1"
                {
                    cli.AddQuery(keyword, EntLibConfigs.GetConfig().Sphinxconfig.IndexSectionName + postTableId);//搜索主索引
                    cli.AddQuery(keyword, EntLibConfigs.GetConfig().Sphinxconfig.IndexSectionName + postTableId + "_stem");//搜索增量索引
                }
            }
          
            cli.SetWeights(new int[] {1,100});
      
            SphinxResult[] results = cli.RunQueries();
            string postid = "";
            foreach (SphinxResult sr in results)
            {
                foreach(SphinxMatch sphinxMatch in sr.matches)
                {
                    if (postid.Length < 280000)//防止SQL语句过长而无法被执行，这里指定最长为28万
                    {
                        postid += sphinxMatch.docId + ",";
                    }
                }
            }        

            if (string.IsNullOrEmpty(postid)) //当没有搜索到任何数据时
                return "";
   
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT [{0}posts{1}].[tid] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE [{0}posts{1}].[pid] IN ({2}) AND [{0}topics].[displayorder]>=0 ",
                                   BaseConfigs.GetTablePrefix,
                                   postTableId,
                                   postid.TrimEnd(','));
            return sqlBuilder.ToString();
        }

        private SphinxConfig sphinxConfig = EntLibConfigs.GetConfig().Sphinxconfig;

        #region MYSQL Helper

        private DbParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        private DbParameter MakeOutParam(string ParamName, DbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        private DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            DbParameter param;

            param = MakeParam(ParamName, DbType, Size);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        private DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            MySqlParameter param;

            if (Size > 0)
                param = new MySqlParameter(ParamName, (MySqlDbType)DbType, Size);
            else
                param = new MySqlParameter(ParamName, (MySqlDbType)DbType);

            return (DbParameter)param;
        }


        /// <summary>
        /// 执行MYSQL语句方法
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(string commandText, params DbParameter[] commandParameters)
        {
            // 返回结果
            int result;

            MySqlConnection connection = new MySqlConnection();
            {
                connection.ConnectionString = EntLibConfigs.GetConfig().Sphinxconfig.MySqlConn;
                connection.Open();

                MySqlCommand command = new MySqlCommand(commandText, connection);

                // 分配命令参数
                if (commandParameters != null)
                    AttachParameters(command, commandParameters);
                result = command.ExecuteNonQuery();//System.Data.CommandBehavior.CloseConnection);

                // 清除参数,以便再次使用.
                command.Parameters.Clear();
            }
            return result;
        }

        /// <summary>
        /// 将DbParameter参数数组(参数值)分配给DbCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="commandParameters">DbParameters数组</param>
        private static void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        #endregion
    }
}
