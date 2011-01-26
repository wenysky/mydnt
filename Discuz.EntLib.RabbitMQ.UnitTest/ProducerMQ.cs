using System;
using System.Collections;
using System.Net;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace Discuz.EntLib.RabbitMQ.UnitTest
{
    public class ProducerMQ
    {
        public static  void InitProducerMQ()
        {
            Uri uri = new Uri("amqp://10.0.4.85:5672/");
            string exchange = "ex1";
            string exchangeType = "direct";
            string routingKey = "m1";
            bool persistMode = true;
            ConnectionFactory cf = new ConnectionFactory();
          
            cf.UserName = "daizhj";
            cf.Password = "617595";
            cf.VirtualHost = "dnt_mq";
            cf.RequestedHeartbeat = 0;
            cf.Endpoint = new AmqpTcpEndpoint(uri);
            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    if (exchangeType != null)
                    {
                        ch.ExchangeDeclare(exchange, exchangeType);//,true,true,false,false, true,null);
                        ch.QueueDeclare("q1", true);//true, true, true, false, false, null);
                        ch.QueueBind("q1", "ex1", "m1", false, null); 
                    }
                    IMapMessageBuilder b = new MapMessageBuilder(ch);
                    IDictionary target = b.Headers;
                    target["header"] = "hello world";
                    IDictionary targetBody = b.Body;
                    targetBody["body"] = "daizhj";
                    if (persistMode)
                    {
                        ((IBasicProperties)b.GetContentHeader()).DeliveryMode = 2;
                    }
                 
                    ch.BasicPublish(exchange, routingKey,
                                               (IBasicProperties)b.GetContentHeader(),
                                               b.GetContentBody());

                }
            }
        }
    }
}
