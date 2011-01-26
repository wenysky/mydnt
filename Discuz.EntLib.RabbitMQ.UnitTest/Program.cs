using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;

using RabbitMQ.ServiceModel;
using RabbitMQ.Client;
using Discuz.EntLib.RabbitMQ.Client;
using Discuz.Cache;

namespace Discuz.EntLib.RabbitMQ.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //HttpModuleErrLogClient m_client = new HttpModuleErrLogClient();
        
            //Util.WriteLine(ConsoleColor.Yellow, "  Add Error Log...");
            //m_client.AddLog(new HttpModuleErrLogData(LogLevel.High, "Hello Rabbit"));
            //m_client.AddLog(new HttpModuleErrLogData(LogLevel.Medium, "Hello Rabbit"));
            //m_client.AddLog(new HttpModuleErrLogData(LogLevel.Low, "Hello Rabbit"));
            //m_client.AddLog(new HttpModuleErrLogData(LogLevel.Low, "Last Message"));
            //Util.WriteLine(ConsoleColor.Green, "Add 4 Error Log DONE");

            //Util.WriteLine(ConsoleColor.Yellow, "  Stop RabbitMQ Client Object...");

         //   ProducerMQ.InitProducerMQ();
            CustmerMq.InitCustmerMq();
           // CustmerMq.InitialCustmerMq();
        
            Console.ReadLine();
        }       
    }
}
