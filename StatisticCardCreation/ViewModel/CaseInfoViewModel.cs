using System.Windows;
using System.Windows.Controls;
using StatCardApp.ViewModel;
using StatCardApp.Function;
using System.IO;
using System.Collections.ObjectModel;
using System;
using StatCardApp.Global;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
using StatCardApp.Model;
using StatisticCardServer.DataBase;
using StatCardApp.Client;

namespace StatCardApp.ViewModel
{
    public class CaseInfoViewModel : Base
    {
        public ListsData data { get; set; } = new ListsData();

        private CaseInfoModel caseInfo = new CaseInfoModel();

        public CaseInfoModel CaseInfo
        {
            get { return caseInfo; }
            set
            {
                caseInfo = value;
                OnPropertyChanged("CaseInfo");
                CaseIsFilled = CheckCaseIsFilled();
            }
        }

        private Boolean caseIsFilled;
        public Boolean CaseIsFilled
        {
            get => caseIsFilled;
            set
            {
                caseIsFilled = value;
                OnPropertyChanged("CaseIsFilled");
            }
        }

        public Boolean CheckCaseIsFilled()
        {
            if (CaseInfo.CaseType == null || CaseInfo.CaseNumber == null || !CaseInfo.RegDate.HasValue)
                return false;
            else
                return true;
        }

        public void TransferData()
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            int port = Convert.ToInt32(config.Port);
            string server = config.IPaddress;
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                string message = JsonConvert.SerializeObject(caseInfo, typeof(CaseInfoModel), new JsonSerializerSettings());
                string messageID = "[CaseInfo]";

                message = message.Insert(0, messageID);
                byte[] data = Encoding.Default.GetBytes(message);

                if (client.Connected)
                {
                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);

                    MessageBox.Show(message.ToString());

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
            }
            catch (SocketException e)
            {
                MessageBox.Show($"Отсутствует подключение");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception: {e}");
            }

            //MessageBox.Show("Запрос завершен...");
        }
    }
}