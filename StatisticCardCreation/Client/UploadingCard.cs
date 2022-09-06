using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows;
using StatCardApp;
using Newtonsoft.Json;
using StatCardApp.Model;
using StatisticCardServer.DataBase;
using System.IO;

namespace StatCardApp.Client
{
    public class UploadingCard
    {
        private int port = 8888;
        private string server = "127.0.0.1";
        StatisticCard card;
        Type cardType;
        public UploadingCard(StatisticCard card, Type cardType)
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            port = Convert.ToInt32(config.Port);
            server = config.IPaddress;

            this.card = card;
            this.cardType = cardType;
        }
        public bool TransferData()
        {
            bool result = true;
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                string message = JsonConvert.SerializeObject(card, cardType, new JsonSerializerSettings());

                string messageID = "[StatisticCard]";
                message = message.Insert(0, messageID);
                
                byte[] data = Encoding.Default.GetBytes(message);
                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);


                message = string.Empty;

                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    message += Encoding.Default.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);

                ServerException Exception = (ServerException)JsonConvert.DeserializeObject(message, typeof(ServerException), new JsonSerializerSettings());

                if (Exception.HasError)
                {
                    result = false;
                    MessageBox.Show(Exception.Message);
                }
                else
                {
                    MessageBox.Show("Данные успешно загружены...");
                }
                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                result = false;
                MessageBox.Show($"Отсутствует соединение");
            }
            catch (Exception e)
            {
                result = false;
                MessageBox.Show($"Exception: {e}");
            }


            return result;
        }
    }
}
