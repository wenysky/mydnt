using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Discuz.Config;
using Discuz.Entity;

namespace Discuz.EntLib.RabbitMQ.Server.Logs
{
    public class SqlServerDbHelper
    {
        public static string SqlConn = RabbitMQConfigs.GetConfig().SendShortMsg.SqlConn;
       
        /// <summary>
        /// 获取用户表数据
        /// </summary>
        /// <param name="tablePrefix">表前缀</param>
        /// <returns></returns>
        public static DataTable GetUserData(string tablePrefix, string groupId, string uid)
        {
            return ExecuteDataset("Select Top 100 [uid],[username],[email]  FROM [" + tablePrefix + "users] WHERE [groupid]=" + groupId + " AND [uid] > " + uid + " ORDER BY [uid] ASC");
        }

        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="pm"></param>
        public static void CreatePrivateMessage(PrivateMessageInfo pm)
        {
            string commandText = "";
            if (pm.Folder != 0)
                pm.Msgfrom = pm.Msgto;
            else
                commandText = "UPDATE [dnt_users] SET [newpmcount]=ABS(ISNULL([newpmcount],0)*1)+1,[newpm] = 1 WHERE [uid]=" + pm.Msgtoid + ";";

            commandText += "INSERT INTO [dnt_pms] ([msgfrom],[msgfromid],[msgto],[msgtoid],[folder],[new],[subject],[postdatetime],[message]) VALUES (@msgfrom,@msgfromid,@msgto,@msgtoid,@folder,@new,@subject,@postdatetime,@message)";

            DbParameter[] parms = {
									   MakeInParam("@pmid",(DbType)SqlDbType.Int,4,pm.Pmid),
									   MakeInParam("@msgfrom",(DbType)SqlDbType.NVarChar,20,pm.Msgfrom),
									   MakeInParam("@msgfromid",(DbType)SqlDbType.Int,4,pm.Msgfromid),
									   MakeInParam("@msgto",(DbType)SqlDbType.NVarChar,20,pm.Msgto),
									   MakeInParam("@msgtoid",(DbType)SqlDbType.Int,4,pm.Msgtoid),
									   MakeInParam("@folder",(DbType)SqlDbType.SmallInt,2,pm.Folder),
									   MakeInParam("@new",(DbType)SqlDbType.Int,4,pm.New),
									   MakeInParam("@subject",(DbType)SqlDbType.NVarChar,80,pm.Subject),
									   MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(pm.Postdatetime)),
									   MakeInParam("@message",(DbType)SqlDbType.NText,0,pm.Message)								  
								   };
            ExecuteNonQuery(commandText, parms);
        }

        /// <summary>
        /// 获取用户表数据
        /// </summary>
        /// <param name="tablePrefix">表前缀</param>
        /// <returns></returns>
        public static string GetUserGroupName(string groupId)
        {
            return ExecuteDataset("Select Top 1 [grouptitle] FROM [dnt_usergroups] WHERE [groupid]=" + groupId).Rows[0][0].ToString().Trim();
        }

        #region SQLServer Helper

        /// <summary>
        /// 执行SQL语句方法
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        private static DataTable ExecuteDataset(string commandText, params DbParameter[] commandParameters)
        {
            DataSet ds = new DataSet();

            // 返回结果
            using (SqlConnection connection = new SqlConnection(SqlConn))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
         
                // 创建DbDataAdapter和DataSet.
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    da.SelectCommand = command;
                    // 填充DataSet.
                    da.Fill(ds);
                    command.Parameters.Clear();
                }
            }
            return ds.Tables[0];
        }

      
        public static int ExecuteNonQuery(string commandText, params DbParameter[] commandParameters)
        {
            int retval = 0;
            // 返回结果
            using (SqlConnection connection = new SqlConnection(SqlConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(commandText, connection);
                AttachParameters(command, commandParameters); 
                retval = command.ExecuteNonQuery();
                command.Parameters.Clear();
            }     
            return retval;
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

        #region 生成参数

        public static DbParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static DbParameter MakeOutParam(string ParamName, DbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            DbParameter param;

            param = MakeParam(ParamName, DbType, Size);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        public static DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, (SqlDbType)DbType, Size);
            else
                param = new SqlParameter(ParamName, (SqlDbType)DbType);

            return param;
        }

        #endregion 
        #endregion

    }
}
