// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 1.1.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (C) 2007-2010 LShift Ltd., Cohesive Financial
//   Technologies LLC., and Rabbit Technologies Ltd.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//
// The MPL v1.1:
//
//---------------------------------------------------------------------------
//   The contents of this file are subject to the Mozilla Public License
//   Version 1.1 (the "License"); you may not use this file except in
//   compliance with the License. You may obtain a copy of the License at
//   http://www.rabbitmq.com/mpl.html
//
//   Software distributed under the License is distributed on an "AS IS"
//   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//   License for the specific language governing rights and limitations
//   under the License.
using System;
using System.IO;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;

namespace Discuz.EntLib.RabbitMQ.UnitTest
{
    public class CustmerMq
    {
        public static int InitCustmerMq()
        {
            string exchange = "ex1";
            string exchangeType = "direct";
            string routingKey = "m1";

            string serverAddress = "10.0.4.85:5672";
            ConnectionFactory cf = new ConnectionFactory();
            cf.Address = serverAddress;
            cf.UserName = "daizhj";
            cf.Password = "617595";
            cf.VirtualHost = "dnt_mq";
            cf.RequestedHeartbeat = 0;
            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    //普通使用方式BasicGet
                    ////noAck = true，不需要回复，接收到消息后，queue上的消息就会清除
                    ////noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，直到调用channel.basicAck(deliveryTag, false); queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息
                    //BasicGetResult res = ch.BasicGet("q1", false);
                    //if (res != null)
                    //{
                    //    bool t = res.Redelivered;
                    //    t = true;
                    //    Console.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(res.Body));
                    //    //ch.BasicAck(res.DeliveryTag, false);
                    //}
                    //else
                    //{
                    //    Console.WriteLine("No message！");
                    //}                 

                    //第二种取法QueueingBasicConsumer基于订阅模式
                    //QueueingBasicConsumer consumer = new QueueingBasicConsumer(ch);
                    //ch.BasicConsume("q1", false, null, consumer);
                    //while (true)
                    //{
                    //    try
                    //    {
                    //        BasicDeliverEventArgs e = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    //        IBasicProperties props = e.BasicProperties;
                    //        byte[] body = e.Body;
                    //        Console.WriteLine(System.Text.Encoding.UTF8.GetString(body));
                    //        //ch.BasicAck(e.DeliveryTag, true);
                    //        ProcessRemainMessage();                          
                    //    }
                    //    catch (EndOfStreamException ex) //
                    //    {
                    //        //  The consumer was removed, either through channel or connection closure, or through the action of IModel.BasicCancel(). 
                    //        Console.WriteLine(ex.ToString());
                    //        break;
                    //    }
                    //}

                    //第三种使用方式，貌似不好用               
                    //Subscription sub;
                    //if (exchange == "")
                    //    sub = new Subscription(ch, routingKey);
                    //else
                    //    sub = new Subscription(ch,"q1", false, exchange, exchangeType, routingKey);

                    //Console.WriteLine("Consumer tag: " + sub.ConsumerTag);
                    //foreach (BasicDeliverEventArgs e in sub)
                    //{
                    //    sub.Ack(e);
                    //    //e.Redelivered = true; //这块要测试
                    //    ProcessSingleDelivery(e);
                    //    if (Encoding.UTF8.GetString(e.Body) == "quit")
                    //    {
                    //        Console.WriteLine("Quitting!");
                    //        break;
                    //    }
                    //}                  
                }
            }
            return 0;
        }

        public static void ProcessSingleDelivery(BasicDeliverEventArgs e)
        {
            Console.WriteLine("Delivery =========================================");
            DebugUtil.DumpProperties(e, Console.Out, 0);
            Console.WriteLine("----------------------------------------");

            if (e.BasicProperties.ContentType == MapMessageReader.MimeType)
            {
                IMapMessageReader r = new MapMessageReader(e.BasicProperties, e.Body);
                DebugUtil.DumpProperties(r.Body, Console.Out, 0);
            }
            else if (e.BasicProperties.ContentType == StreamMessageReader.MimeType)
            {
                IStreamMessageReader r = new StreamMessageReader(e.BasicProperties, e.Body);
                while (true)
                {
                    try
                    {
                        object v = r.ReadObject();//r.Headers;
                        Console.WriteLine("(" + v.GetType() + ") " + v);
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }
            }
            else
            {
                // No special content-type. Already covered by the DumpProperties above.
            }

            Console.WriteLine("==================================================");
        }
    }
}
