using System;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;
using Discuz.Data;
using MongoDB;
using MongoDB.GridFS;

namespace Discuz.EntLib.MongoDB.Data
{
    public class AttachFiles : Discuz.Cache.Data.ICacheAttachFiles
    {
        private static string connectString = null;

        private static DBCache mongoCache = EntLibConfigs.GetConfig().Cacheattachfiles;


        static AttachFiles()
        {
            InitialMongoDB();
        }

        static void InitialMongoDB()
        {
            //connectString = string.Format("Username={0};Password={1};Servers={2}:{3};ConnectTimeout={4};ConnectionLifetime={5};MinimumPoolSize={6};MaximumPoolSize={7};Pooled=true",
            //     mongoCache.Username,
            //     mongoCache.Password,
            //     mongoCache.Host,
            //     mongoCache.Port,
            //     mongoCache.TcpClientConnectTimeout,
            //     mongoCache.TcpClientTimeout,
            //     mongoCache.MinConnections,
            //     mongoCache.MaxConnections);
            connectString = string.Format("Username={0};Password={1};Servers={2}:{3};MinimumPoolSize={4};MaximumPoolSize={5};Pooled=true",
                 mongoCache.Username,
                 mongoCache.Password,
                 mongoCache.Host,
                 mongoCache.Port,           
                 mongoCache.MinConnections,
                 mongoCache.MaxConnections);
        }

        private static Mongo mongoDB
        {
            get
            {
                return new Mongo(connectString);
            }
        }

        /// <summary>
        /// 上传文件到mongodb
        /// </summary>
        /// <param name="uploadDir">要上传的路径</param>
        /// <param name="fileName">要上传的文件名</param>
        /// <returns></returns>
        public bool UploadFile(string uploadDir, string fileName)
        {
            //for (int i = 1; i < 50000; i++)
            //{
                try
                {
                    Mongo mongo = mongoDB;
                    mongo.Connect();
                    IMongoDatabase DB = mongo["dnt_mongodb"];

                    using (FileStream fileStream = new FileStream(uploadDir + fileName, FileMode.Open))
                    {
                        int nFileLen = (int)fileStream.Length;

                        byte[] myData = new Byte[nFileLen];
                        fileStream.Read(myData, 0, nFileLen);

                        GridFile fs = new GridFile(DB, "attach_gfstream");
                        using (GridFileStream gfs = fs.Create(fileName))   // fileName + i
                        {
                            gfs.Write(myData, 0, nFileLen);
                        }
                    }
                    mongo.Disconnect();
                }
                catch { }                
            //}
            return true;
        }

        /// <summary>
        /// Response附件
        /// </summary>
        /// <param name="fileName">要获取附件的存储文件名</param>
        /// <param name="originfilename">附件原来名称，即response的名称</param>
        /// <param name="filetype">要获取附件的类型</param>
        /// <returns></returns>
        public bool ResponseFile(string fileName, string originfilename, string filetype)
        {
            Mongo mongo = mongoDB; 
            mongo.Connect();
            IMongoDatabase DB = mongo["dnt_mongodb"];

            try
            {
                GridFile fs = new GridFile(DB, "attach_gfstream");
                using (GridFileStream gfs = fs.OpenRead(fileName))
                {
                    Byte[] buffer = new Byte[gfs.Length];
                    System.Web.HttpContext.Current.Response.ContentType = filetype;
                    if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") > -1)
                        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Utils.UrlEncode(originfilename.Trim()).Replace("+", " "));
                    else
                        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + originfilename.Trim());

                    System.Web.HttpContext.Current.Response.AddHeader("Expires", DateTime.Now.AddDays(20).ToString("r"));
                    System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "public");

                    // 需要读的数据长度
                    long dataToRead = gfs.Length;

                    if (gfs.Length > 10000)
                    {
                        int length;
                        while (dataToRead > 0)
                        {
                            // 检查客户端是否还处于连接状态
                            if (System.Web.HttpContext.Current.Response.IsClientConnected)
                            {
                                length = gfs.Read(buffer, 0, 10000);
                                System.Web.HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                                System.Web.HttpContext.Current.Response.Flush();
                                buffer = new Byte[10000];
                                dataToRead = dataToRead - length;
                            }
                            else
                            {
                                // 如果不再连接则跳出死循环
                                dataToRead = -1;
                            }
                        }
                    }
                    else
                    {
                        gfs.Read(buffer, 0, buffer.Length);
                        System.Web.HttpContext.Current.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        System.Web.HttpContext.Current.Response.Flush();
                    }
                }
                mongo.Disconnect();
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                //System.Web.HttpContext.Current.Response.End();
            }
            catch {}
            return true;
        }
       
    }
}