using System;
using System.Data;
using System.Xml;
using System.Data.Common;
using System.Collections;

using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.EntLib.ToolKit
{
    public class DbSnapState
    {
        public string ConnectString;
        public bool SnapState;
        public string Message;
    }

    public partial class DbSnapHelper : Discuz.Data.DbHelper
    {
        /// <summary>
        /// 获取快照是否配置正确，以及造成不正确的错误信息
        /// </summary>
        /// <returns></returns>
        public static List<DbSnapState> TestDbSnapState()
        {
            List<DbSnapState> validDbSnap = new List<DbSnapState>();
            foreach (DbSnapInfo dbSnapInfo in DbSnapConfigs.GetEnableSnapList())
            {
                if (dbSnapInfo.DbconnectString == null || dbSnapInfo.DbconnectString.Length == 0)
                    throw new ArgumentNullException("ConnectionString");

                using (DbConnection connection = Factory.CreateConnection())
                {
                    try
                    {
                        connection.ConnectionString = dbSnapInfo.DbconnectString;

                        //运行一条查询语句判断其是否有效
                        ExecuteNonQuery(connection, CommandType.Text, string.Format("SELECT COUNT(fid) FROM [{0}forums]", BaseConfigs.GetTablePrefix), null);

                        validDbSnap.Add(new DbSnapState() { ConnectString = dbSnapInfo.DbconnectString, SnapState = true, Message = "" });
                    }
                    catch (Exception e)
                    {
                        validDbSnap.Add(new DbSnapState() { ConnectString = dbSnapInfo.DbconnectString, SnapState = false, Message = e.Message });
                    }
                }
            }
            return validDbSnap;
        }

        /// <summary>
        /// 移除快照日志表
        /// </summary>
        public static void DropSnapLogTable()
        {
            System.Text.StringBuilder sqlStringBuilder = new System.Text.StringBuilder();
            sqlStringBuilder.Append("if exists (select * from sysobjects where id = object_id(N'[dnt_dbsnaplogs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dnt_dbsnaplogs] ;");
            ExecuteNonQueryInMasterDB(CommandType.Text, sqlStringBuilder.ToString());
        }

        /// <summary>
        /// 创建快照日志列表
        /// </summary>
        public static void CreateSnapLogTable()
        {
            DropSnapLogTable();

            System.Text.StringBuilder sqlStringBuilder = new System.Text.StringBuilder();
            sqlStringBuilder.Append("CREATE TABLE [dbo].[dnt_dbsnaplogs]( ");
            sqlStringBuilder.Append("             [logid] [int] IDENTITY(1,1) NOT NULL, ");
            sqlStringBuilder.Append("             [commandtext] [ntext] NULL CONSTRAINT [DF_dnt_dbsnaplogs_sqlstring]  DEFAULT (''),");
            sqlStringBuilder.Append("             [connectstring] [ntext] NULL CONSTRAINT [DF_dnt_dbsnaplogs_connectstring]  DEFAULT (''),");
            sqlStringBuilder.Append("             [postdatetime] [datetime] NULL CONSTRAINT [DF_dnt_dbsnaplogs_postdatetime]  DEFAULT (getdate())");
            sqlStringBuilder.Append("             ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ");

            ExecuteNonQueryInMasterDB(CommandType.Text, sqlStringBuilder.ToString());
        }



        /// <summary>
        /// 获取快照是否配置正确，以及造成不正确的错误信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDbSnapLogList()
        {
            //运行一条查询语句判断其是否有效
            return ExecuteDatasetInMasterDB(CommandType.Text, "SELECT * FROM [dnt_dbsnaplogs]").Tables[0];
        }
    }

}