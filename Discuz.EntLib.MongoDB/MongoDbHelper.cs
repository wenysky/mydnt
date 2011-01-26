using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using MongoDB;

namespace Discuz.EntLib.MongoDB
{
    public class MongoDbHelper
    {
        public static void Insert(Mongo monoDB, string tableName, Document doc)
        {
            if (monoDB.TryConnect())
            {
                IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                table.Insert(doc);
                monoDB.Disconnect();
            }
        }
      

        public static void Delete(Mongo monoDB, string tableName, Document selector)
        {
            if (monoDB.TryConnect())
            {
                IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                table.Remove(selector);
                monoDB.Disconnect();
            }
        }

        public static void Update(Mongo monoDB, string tableName, Document doc)
        {
            monoDB.Connect();
            IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
            table.Save(doc);
            monoDB.Disconnect();
        }

        public static void Update(Mongo monoDB, string tableName, Document doc, Document selector)
        {
            if (monoDB.TryConnect())
            {
                IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                table.Update(doc, selector);
                monoDB.Disconnect();
            }
        }

        public static List<Document> Find(Mongo monoDB, string tableName, Document spec)
        {
            List<Document> docList = null;
            if (monoDB.TryConnect())
            {
                IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                docList = table.Find(spec).Documents.ToList();
                monoDB.Disconnect();
            }
            return docList;
        }

        public static List<Document> Find(Mongo monoDB, string tableName, Document spec, int limit, int skip)
        {
             List<Document> docList = null;
             if (monoDB.TryConnect())
             {
                 IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                 docList = table.Find(spec).Skip(skip).Limit(limit).Documents.ToList();
                 monoDB.Disconnect();
             }
            return docList;
        }

        public static List<Document> Find(Mongo monoDB, string tableName, Document spec, string orderField, IndexOrder orderType, int limit, int skip)
        {
             List<Document> docList = null;
             if (monoDB.TryConnect())
             {
                 IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                 docList = table.Find(spec).Sort(orderField, orderType).Skip(skip).Limit(limit).Documents.ToList();
                 monoDB.Disconnect();
             }
            return docList;
        }

        public static Document FindOne(Mongo monoDB, string tableName, Document spec)
        {
            Document doc = null;
            if (monoDB.TryConnect())
            {
                IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                doc = table.FindOne(spec);
                monoDB.Disconnect();
            }
            return doc;
        }

        public static ICursor FindAll(Mongo monoDB, string tableName)
        {
            ICursor cursor = null;
            if (monoDB.TryConnect())
            {
                IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                cursor = table.FindAll();
                monoDB.Disconnect();
            }
            return cursor;
        }

        public static long Count(Mongo monoDB, string tableName, Document spec)
        {
             long count = 0;
             if (monoDB.TryConnect())
             {
                 IMongoCollection table = monoDB.GetDatabase("dnt_mongodb")[tableName];
                 count = spec == null ? table.Count() : table.Count(spec);
                 monoDB.Disconnect();
             }
            return count;
        }

        public static void CleanTable(Mongo monoDB, string tableName)
        {
            if (monoDB.TryConnect())
            {
                IMongoDatabase dnt_mongodb = monoDB.GetDatabase("dnt_mongodb");
                dnt_mongodb["$cmd"].FindOne(new Document() { { "drop", tableName } });
                monoDB.Disconnect();
            }
        }
    }
}
