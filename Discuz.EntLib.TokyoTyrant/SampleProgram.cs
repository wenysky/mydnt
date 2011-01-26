/*
 * TokyoTyrant.NET (http://tokyotyrant.codeplex.com)
 * Pure .NET interface to Tokyo Tyrant
 * Copyright (C) 2009 Russell van der Walt (http://blog.ruski.co.za)

 * TokyoTyrant.NET is free software; you can redistribute it and/or modify it under the terms of
 * the Microsoft Public License (Ms-PL). TokyoTyrant.NET is distributed in the hope
 * that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the Microsoft Public
 * License for more details.
 * 
 * Tokyo Cabinet and Tokyo Tyrant is Copyright (C) 2006-2008 Mikio Hirabayashi 
 * http://tokyocabinet.sourceforge.net/
 * 
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Discuz.EntLib.TokyoTyrant;
using Discuz.Config;
using Discuz.Cache.Data;
namespace TokyoTyrantExample
{

    class Program
    {
        private static TcpClientIOPool pool = TcpClientIOPool.GetInstance("dnt_online");

        private static DBCache ttCache = EntLibConfigs.GetConfig().Cacheonlineuser;

        static void InitalTcpClientPool()
        {
            pool = TcpClientIOPool.GetInstance(ttCache.PoolName);
            pool.SetServers(new string[] { ttCache.Host + ":" + ttCache.Port });
            pool.InitConnections = ttCache.IntConnections;
            pool.MinConnections = ttCache.MinConnections;
            pool.MaxConnections = ttCache.MaxConnections;
            pool.MaxIdle = ttCache.MaxIdle;
            pool.MaxBusy = ttCache.MaxBusy;
            pool.MaintenanceSleep = ttCache.MaintenanceSleep;
            pool.TcpClientTimeout = ttCache.TcpClientTimeout;
            pool.TcpClientConnectTimeout = ttCache.TcpClientConnectTimeout;
            pool.Initialize();
        }

        static void TestConn1(int number)
        {
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            //using (var con = new TokyoTyrantClient())
            {
                var con = new TokyoTyrantService();
                //con.Connect("10.0.4.66", 11221);
                for (int i = 0; i < number; i++)
                {
                    var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", 1));
                    foreach (var k in qrecords.Keys)
                    {
                        var column = qrecords[k];
                        Console.WriteLine(k + "\t" + column["username"] + "\t" + column["olid"]);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("first:" + sw.ElapsedMilliseconds);
        }

        static void TestConn2(int number)
        {
            //var con = new TokyoTyrantClient();

            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < number; i++)
            {
                //using (var con = new TokyoTyrantClient())
                {

                    //con.Connect("10.0.4.66", 11221);

                    var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", 1));
                    foreach (var k in qrecords.Keys)
                    {
                        var column = qrecords[k];
                        //Console.WriteLine(k + "\t" + column["username"] + "\t" + column["groupid"]);
                    }
                    //Console.WriteLine("second:" + i);
                }
            }
            sw.Stop();
            Console.WriteLine("second:" + sw.ElapsedMilliseconds);
        }

        static void CreateData(int number)
        {
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 2; i < number; i++)
            {
                IDictionary<string, string> columns = new System.Collections.Generic.Dictionary<string, string>();
                columns.Add("olid", "0");
                columns.Add("userid", "-1");
                columns.Add("ip", "");
                columns.Add("username", "游客");
                columns.Add("nickname", "游客");
                columns.Add("password", "");
                columns.Add("groupid", "7");
                columns.Add("olimg", "");
                columns.Add("adminid", "0");
                columns.Add("invisible", "0");
                columns.Add("action", "0");
                columns.Add("lastactivity", "1900-1-1 00:00:00");
                columns.Add("lastposttime", "1900-1-1 00:00:00");
                columns.Add("lastpostpmtime", "1900-1-1 00:00:00");
                columns.Add("lastsearchtime", "1900-1-1 00:00:00");
                columns.Add("lastupdatetime", "1900-1-1 00:00:00");
                columns.Add("forumid", "0");
                columns.Add("forumname", "");
                columns.Add("titleid", "0");
                columns.Add("title", "");
                columns.Add("verifycode", "");
                columns.Add("newpms", "");
                columns.Add("newnotices", "");
                TokyoTyrantService.PutColumns(pool, i.ToString(), columns, true);
            }
            sw.Stop();
            Console.WriteLine("second:" + sw.ElapsedMilliseconds);
        }

        static void Main(string[] args)
        {
            InitalTcpClientPool();

            CreateData(10000);
            TestConn2(10000);
            //TestConn1(10000);
            //Console.ReadLine();

            //try
            //{

            //Console.Write("Enter the server name (localhost): ");
            //string host = "10.0.4.66";////Console.ReadLine();
            //if (host == "")
            //{
            //    host = "localhost";
            //}
            //int port = 11221;

            //using (var con = new TokyoTyrantClient())
            //{
            //    con.Connect(host, port);

            //IDictionary<string, string> columns = new System.Collections.Generic.Dictionary<string, string>();
            //columns.Add("olid", "0");
            //columns.Add("userid", "-1");
            //columns.Add("ip", "");
            //columns.Add("username", "游客");
            //columns.Add("nickname", "游客");
            //columns.Add("password", "");
            //columns.Add("groupid", "7");
            //columns.Add("olimg", "");
            //columns.Add("adminid", "0");
            //columns.Add("invisible", "0");
            //columns.Add("action", "0");
            //columns.Add("lastactivity", "");
            //columns.Add("lastposttime", "");
            //columns.Add("lastpostpmtime", "");
            //columns.Add("lastsearchtime", "");
            //columns.Add("lastupdatetime", "");
            //columns.Add("forumid", "0");
            //columns.Add("forumname", "");
            //columns.Add("titleid", "0");
            //columns.Add("title", "");
            //columns.Add("verifycode", "");
            //columns.Add("newpms", "");
            //columns.Add("newnotices", "");
            //con.PutColumns("0", columns, true);


            //string key;
            //while ((key = con.IteratorNext()) != null)
            //{
            //    var column = con.GetColumns(new string[] { key })[key];
            //    Console.WriteLine(key + "\t" + column["username"] + "\t" + column["groupid"]);
            //}
            // Console.ReadLine();

            //var con = new TokyoTyrantClient();
            //var qrecords = con.QueryRecords(new TokyoQuery().NumberEquals("olid", 1));

            //foreach (var k in qrecords.Keys)
            //{
            //    var column = qrecords[k];
            //    Console.WriteLine(k + "\t" + column["username"] + "\t" + column["olid"]);
            //}

            //Console.WriteLine("host: " + host);
            //Console.WriteLine("port: " + port.ToString());
            //var status = con.GetDatabaseStatus();
            //foreach (var key in status.Keys) {
            //    Console.WriteLine(key + ": " + status[key]);
            //}
            //Console.WriteLine("");
            //OutputDatabaseSize(con);
            //Console.WriteLine("");
            //Console.WriteLine("WARNING: This will delete all data in the target database.");
            //Console.Write("Are you sure you wish to continue [y/N]?");
            //var chr = Console.ReadLine();
            //if (chr.ToUpper() == "Y") {

            //    Console.WriteLine("\r\nClearing database...");
            //    con.Vanish();
            //    OutputDatabaseSize(con);
            //    Console.WriteLine("");

            //if (status["type"] == "table")
            //{
            //    TableTest(con);
            //}
            //else
            //{
            //    HashTest(con);
            //}
            //    Console.WriteLine("\r\nSynchronizing database...");
            //    con.Synchronize();

            //    Console.WriteLine("\r\nOptimizing database...");
            //    con.Optimize("");
            //    OutputDatabaseSize(con);

            //    Console.WriteLine("Press [ENTER] to terminate...");
            //    Console.ReadLine();
            //}
            //}


            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        public static void OutputDatabaseSize()
        {
            var dbsize = TokyoTyrantService.GetDatabaseSize(pool);
            var reccnt = TokyoTyrantService.GetRecordCount(pool);
            Console.WriteLine("Database has " + reccnt + " records [" + dbsize.ToString() + " bytes]");
        }

        public static void TableTest()
        {

            Console.WriteLine("\r\nRUNNING TABLE TESTS");
            Console.WriteLine("\r\nPopulating database...");
            var key = "";
            IDictionary<string, string> columns;
            var names = new string[] { "Mark", "Tom", "Harry", "Sally", "Sandra", "Paul", "Russell", "David", "Alex", "Michael", "Tina", "Zachary", "Bob", "Elise" };
            for (int i = 0; i < names.Length; i++)
            {
                key = string.Format("ID{0:00000}", i);
                columns = new Dictionary<string, string>();
                columns.Add("name", names[i]);
                columns.Add("age", Convert.ToString(10 + i));
                TokyoTyrantService.PutColumns(pool,key, columns, true);
            }
            OutputDatabaseSize();




            Console.WriteLine("\r\nIterating records...");
            TokyoTyrantService.IteratorInitialize(pool);
            Console.WriteLine("[ID]\t[Name]\t[Age]");
            while ((key = TokyoTyrantService.IteratorNext(pool)) != null)
            {
                var column = TokyoTyrantService.GetColumns(pool, new string[] { key })[key];
                Console.WriteLine(key + "\t" + column["name"] + "\t" + column["age"]);
            }

            Console.WriteLine("\r\nAdding an index...");
            TokyoTyrantService.SetIndex(pool, "name", IndexOption.LEXICAL);

            Console.WriteLine("\r\nQuerying keys...");
            var qkeys = TokyoTyrantService.QueryKeys(pool, new Query().StringStartsWith("name", "Sa"));
            var str = "";
            foreach (var k in qkeys)
            {
                str += k + ", ";
            }
            Console.WriteLine("Keys: " + str);

            Console.WriteLine("\r\nQuerying names...");
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringStartsWith("name", "Sa"));
            Console.WriteLine("[ID]\t[Name]\t[Age]");
            foreach (var k in qrecords.Keys)
            {
                var column = qrecords[k];
                Console.WriteLine(k + "\t" + column["name"] + "\t" + column["age"]);
            }

            Console.WriteLine("\r\nQuerying ages...");
            qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberGreaterThanOrEqual("age", 16).NumberLessThan("age", 20).OrderBy("age", QueryOrder.NUMDESC).LimitTo(3, 0));
            Console.WriteLine("[ID]\t[Name]\t[Age]");
            foreach (var k in qrecords.Keys)
            {
                var column = qrecords[k];
                Console.WriteLine(k + "\t" + column["name"] + "\t" + column["age"]);
            }

            Console.WriteLine("\r\nDeleting with a query...");
            TokyoTyrantService.QueryDelete(pool, new Query().NumberGreaterThan("age", 20));
            OutputDatabaseSize();


            Console.WriteLine("\r\nDeleting first record...");
            TokyoTyrantService.Delete(pool, "ID00000");
            OutputDatabaseSize();

            Console.WriteLine("\r\nDeleting 3 records...");
            TokyoTyrantService.DeleteMultiple(pool, new string[] { "ID00000", "ID00001", "ID00002", "ID00003" });
            OutputDatabaseSize();

            Console.WriteLine("\r\nPrefix searching records...");
            var keys = TokyoTyrantService.GetMatchingKeys(pool, "ID", -1);
            Console.WriteLine("Retrieved " + keys.Length + " keys");
            Console.WriteLine("\r\nRetrieving batch records and checking size...");
            var records = TokyoTyrantService.GetColumns(pool, keys);
            Console.WriteLine("[ID]\t[Name]\t[Age]\t[Size]");
            foreach (var k in records.Keys)
            {
                var column = records[k];
                var size = TokyoTyrantService.GetSize(pool, k);
                Console.WriteLine(k + "\t" + column["name"] + "\t" + column["age"] + "\t" + size + " bytes");
            }





        }

        public static void HashTest()
        {


            Console.WriteLine("\r\nRUNNING HASH TESTS");
            var key = "";

            Console.WriteLine("\r\nPopulating database with 3 items...");

            var names = new string[] { "Mark", "Tom", "Harry" };
            for (int i = 0; i < names.Length; i++)
            {
                key = string.Format("ID{0:00000}", i);
                if (i == 0)
                {
                    TokyoTyrantService.Put(pool, key, names[i], true);
                }
                else
                    TokyoTyrantService.PutFast(pool, key, names[i]);
            }
            OutputDatabaseSize();
            Console.WriteLine("\r\nPopulating database with 11 item in a batch...");
            names = new string[] { "Sally", "Sandra", "Paul", "Russell", "David", "Alex", "Michael", "Tina", "Zachary", "Bob", "Elise" };
            IDictionary<string, string> records = new Dictionary<string, string>();
            for (int i = 0; i < names.Length; i++)
            {
                key = string.Format("ID{0:00000}", i + 3);
                records.Add(key, names[i]);
            }
            TokyoTyrantService.PutMultiple(pool, records);
            OutputDatabaseSize();

            Console.WriteLine("\r\nIterating records...");
            TokyoTyrantService.IteratorInitialize(pool);
            Console.WriteLine("[ID]\t[Name]");
            while ((key = TokyoTyrantService.IteratorNext(pool)) != null)
            {
                var name = TokyoTyrantService.Get(pool, key);
                Console.WriteLine(key + "\t" + name);
            }
            Console.WriteLine("\r\nDeleting first record...");
            TokyoTyrantService.Delete(pool, "ID00000");
            OutputDatabaseSize();

            Console.WriteLine("\r\nDeleting 2 records...");
            TokyoTyrantService.DeleteMultiple(pool, new string[] { "ID00000", "ID00001", "ID00002" });
            OutputDatabaseSize();

            Console.WriteLine("\r\nConcatenating record...");
            TokyoTyrantService.Concatenate(pool, "ID00003", " Jones");
            Console.WriteLine("ID00003: " + TokyoTyrantService.Get(pool, "ID00003"));

            // haven't quite figured this one out yet
            Console.WriteLine("\r\nConcatenating record and shifting left...");
            TokyoTyrantService.ConcatenateShiftLeft(pool, "ID00003", "Kenny", 5);
            Console.WriteLine("ID00003: " + TokyoTyrantService.Get(pool, "ID00003"));


            Console.WriteLine("\r\nPrefix searching records...");
            var keys = TokyoTyrantService.GetMatchingKeys(pool, "ID", -1);
            Console.WriteLine("Retrieved " + keys.Length + " keys");
            Console.WriteLine("\r\nRetrieving batch records and checking size...");
            records = TokyoTyrantService.GetMultiple(pool, keys);
            Console.WriteLine("[ID]\t[Name]\t[Size]");
            foreach (var k in records.Keys)
            {
                var size = TokyoTyrantService.GetSize(pool, k);
                Console.WriteLine(k + "\t" + records[k] + "\t" + size + " bytes");
            }

            key = "NUM1";
            Console.WriteLine("\r\nInserting number...");
            Console.WriteLine(TokyoTyrantService.IncrementInteger(pool, key, 1000000));
            Console.WriteLine(TokyoTyrantService.IncrementInteger(pool, key, 2005000));
            Console.WriteLine(TokyoTyrantService.GetInteger(pool, key));

        }

    }
}
