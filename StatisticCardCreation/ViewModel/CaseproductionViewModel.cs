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
using StatCardApp.Client;

namespace StatCardApp.ViewModel
{
    public class CaseproductionViewModel : Base
    {

        public ListsData Data { get; set; } = new ListsData();

        private CaseProductionInfo productionInfo = new CaseProductionInfo();
        public CaseProductionInfo ProductionInfo
        {
            get => productionInfo;
            set
            {
                productionInfo = value;
                OnPropertyChanged("ProductionInfo");
            }
        }

        public bool TransferData()
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            int port = Convert.ToInt32(config.Port);
            string server = config.IPaddress;

            bool result = true;

            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                string message = JsonConvert.SerializeObject(ProductionInfo, typeof(CaseProductionInfo), new JsonSerializerSettings());
                string messageID = "[CaseProduction]";

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

    public class CaseProductionInfo : Base
    {
        private long caseNumber;
        public long CaseNumber
        {
            get => caseNumber;
            set
            {
                caseNumber = value;
                OnPropertyChanged("CaseNumber");
            }
        }

        private string position;
        public string Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        private string rank;
        public string Rank
        {
            get => rank;
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        }

        private string fio;
        public string FIO
        {
            get => fio;
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        private DateTime? date;
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
    }
}
