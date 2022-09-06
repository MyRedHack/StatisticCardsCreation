using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using StatCardApp.Client;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Windows;
using StatisticCardServer.DataBase;

namespace StatisticProceeding.ViewModel
{
    public class WeeklyCaseTableViewModel : Base
    {
        public WeeklyCaseTableViewModel()
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            CaseDescriptions = SelectCaseListFromServer();
        }

        private CaseDescription caseDescription;
        public CaseDescription CaseDescription
        {
            get => caseDescription;
            set
            {
                caseDescription = value;
                OnPropertyChanged("CaseDescription");
            }
        }

        public List<CaseDescriptionStatus> CaseDescriptions { get; set; } = new List<CaseDescriptionStatus>();

        public void InsertInfoToDataBase()
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            int port = Convert.ToInt32(config.Port);
            string server = config.IPaddress;

            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                string message = JsonConvert.SerializeObject(CaseDescriptions, typeof(List<CaseDescriptionStatus>), new JsonSerializerSettings());
                string messageID = "[SETWEEKLYCASETABLE]";

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

                if (Exception != null)
                {
                    if (Exception.HasError)
                    {
                        MessageBox.Show(Exception.Message);
                    }
                    else
                    {
                        MessageBox.Show("Данные успешно загружены...");
                    }
                }

                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                MessageBox.Show($"Отсутствует соединение");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception: {e}");
            }
        }

        public List<CaseDescriptionStatus> SelectCaseListFromServer()
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            int port = Convert.ToInt32(config.Port);
            string server = config.IPaddress;

            List<CaseDescriptionStatus> list = new List<CaseDescriptionStatus>();
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                string message = "[GETWEEKLYCASETABLE]";

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

                list = (List<CaseDescriptionStatus>)JsonConvert.DeserializeObject(message, typeof(List<CaseDescriptionStatus>), new JsonSerializerSettings());

                //ServerException Exception = (ServerException)JsonConvert.DeserializeObject(message, typeof(ServerException), new JsonSerializerSettings());

                //if (Exception.HasError)
                //{
                //    result = false;
                //    MessageBox.Show(Exception.Message);
                //}
                //else
                //{
                //    MessageBox.Show("Данные успешно загружены...");
                //}
                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                MessageBox.Show($"Отсутствует соединение");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception: {e}");
            }


            return list;
        }
    }
}
