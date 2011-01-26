using System;
using System.Data.Common;
using System.Data;

using Discuz.Cache;
using Discuz.Config;
using Discuz.Data;

namespace Discuz.EntLib
{
    /// <summary>
    /// 企业级MemCache缓存策略类,只能使用一个web园程序
    /// </summary>
    public class MemCachedStrategy : DefaultCacheStrategy
    {
        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public override void AddObject(string objId, object o)
        {  
            if (!objId.StartsWith("/Forum/ShowTopic/"))
                base.AddObject(objId, o, LocalCacheTime);

            MemCachedManager.CacheClient.Add(objId, o);
        
            RecordLog(objId, "set");
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="o">到期时间,单位:秒</param>
        public override void AddObject(string objId, object o, int expire)
        {
            //凡是以"/Forum/ShowTopic/"为前缀不添加到本地缓存中，现类似键值包括: "/Forum/ShowTopic/Tag/{topicid}/" , "/Forum/ShowTopic/TopList/{fid}"
            if (!objId.StartsWith("/Forum/ShowTopic/"))
                base.AddObject(objId, o, expire);

            //永不过期
            if (expire == 0)
                MemCachedManager.CacheClient.Add(objId, o);
            else
                MemCachedManager.CacheClient.Add(objId, o, DateTime.Now.AddSeconds(expire));

            RecordLog(objId, "set");
        }

  
        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        public override void RemoveObject(string objId)
        {
            //先移除本地cached，然后再移除memcached中的相应数据
            base.RemoveObject(objId);

            MemCachedManager.CacheClient.Delete(objId);

            Discuz.EntLib.SyncCache.SyncRemoteCache(objId);
        }

        /// <summary>
        /// 记录日志方法
        /// </summary>
        /// <param name="objId">缓存键值</param>
        /// <param name="opName">操作名称(set,get)</param>
        private void RecordLog(string objId, string opName)
        {
            //当启用写入数据日志时
            if (MemCachedConfigs.GetConfig().RecordeLog)
            {
                DbParameter[] parms = {
                                        DbHelper.MakeInParam("@cachekey", (DbType)SqlDbType.NVarChar, 200, objId),
                                        DbHelper.MakeInParam("@opname", (DbType)SqlDbType.NVarChar, 10, opName),
                                        DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, Discuz.Common.Utils.GetDateTime())
                                    };
                Discuz.Data.DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO memcachedlogs (cachekey, opname, postdatetime) Values (@cachekey, @opname, @postdatetime)", parms);
            }
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public override object RetrieveObject(string objId)
        {
            object obj = base.RetrieveObject(objId);
       
            if (obj == null)
            {
                obj = MemCachedManager.CacheClient.Get(objId);
                if (obj != null && !objId.StartsWith("/Forum/ShowTopic/"))//对ShowTopic页面缓存数据不放到本地缓存
                {
                    if (objId.StartsWith("/Forum/ShowTopicGuestCachePage/"))//对游客缓存页面ShowTopic数据缓存设置有效时间
                        base.TimeOut = GeneralConfigs.GetConfig().Guestcachepagetimeout * 60;
                    if (objId.StartsWith("/Forum/ShowForumGuestCachePage/"))//对游客缓存页面ShowTopic数据缓存设置有效时间
                        base.TimeOut = MemCachedConfigs.GetConfig().CacheShowForumCacheTime * 60;
                    else
                        base.TimeOut = LocalCacheTime;

                    base.AddObject(objId, obj, TimeOut);
                }
                RecordLog(objId, "get");
            }
            return obj;
        }

        /// <summary>
        /// 到期时间,单位:秒
        /// </summary>
        public override int TimeOut 
        { 
            get 
            {
                return 3600;// MemCachedConfigs.GetConfig().LocalCacheTime; 
            } 
        }

        /// <summary>
        /// 本地缓存到期时间,单位:秒
        /// </summary>
        public int LocalCacheTime
        {
            get
            {
                return MemCachedConfigs.GetConfig().LocalCacheTime;
            }
        }
    }

    #region memcachedlogs数据表结构
    /*
    CREATE TABLE [dbo].[memcachedlogs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cachekey] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_memcachedlogs_cachekey]  DEFAULT (''),
	[opname] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_memcachedlogs_opname]  DEFAULT (''),
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_memcachedlogs_postdatetime]  DEFAULT (getdate())
    ) ON [PRIMARY]
    */
    #endregion

}
