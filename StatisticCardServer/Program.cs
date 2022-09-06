using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using StatisticCardServer.DataBase;
using Newtonsoft.Json;
using StatCardApp.Client;

namespace StatisticCardServer
{
    class Program
    {
        

        

         // порт для прослушивания подключений
        static void Main(string[] args)
        {
            CriminalCaseDB db = new CriminalCaseDB();
            db.Connect();
            if (db.Connection.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Подключается к бд...");
                db.Connection.Close();
            }
            else
            {
                Console.WriteLine("Не подключается к бд...");
                db.Connection.Dispose();
            }

            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));
            int port = Convert.ToInt32(config.Port);
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(config.IPaddress);
                server = new TcpListener(localAddr, port);

                // запуск слушателя
                server.Start();
                Console.WriteLine("Ожидание подключений... ");

                while (true)
                {
                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    Thread clientThread = new Thread(new ThreadStart(clientObject.DataProccess));
                    clientThread.Start();

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
            Console.ReadLine();
        }
    }
}
