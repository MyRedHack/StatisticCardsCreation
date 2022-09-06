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
using Microsoft.Data.SqlClient;
using StatCardApp.Function;
using OfficeOpenXml;
using System.IO;

namespace StatisticProceeding.ViewModel
{
    public class MainWindowViewModel
    {
        ExcelPackage package;
        public int FormCaseMovementFile()
        {
            var CaseMovementData = GetCaseMovementData();

            FileInfo file = new FileInfo("casemovementtemplate.xlsx");

            package = new ExcelPackage(file);

            CreateTestExcel(CaseMovementData[3]);

            return 1;
        }

        public Dictionary<int, Dictionary<int, Dictionary<string, string>>> GetCaseMovementData()
        {
            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));

            int port = Convert.ToInt32(config.Port);
            string server = config.IPaddress;
            Dictionary<int, Dictionary<int, Dictionary<string, string>>> list = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                string message = "[EXECUTEREADER]";

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

                list = (Dictionary<int, Dictionary<int, Dictionary<string, string>>>)JsonConvert.DeserializeObject(message, typeof(Dictionary<int, Dictionary<int, Dictionary<string, string>>>), new JsonSerializerSettings());

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

        public void CreateTestExcel(Dictionary<int, Dictionary<string, string>> data)
        {
            int rowOffset = 3;
            int columnOffset = 1;

            var ws = package.Workbook.Worksheets[3];

            for(int i = 1 + rowOffset; i <= data.Count + rowOffset; i++)
            {
                var Keys = data[i - (rowOffset + 1)].Keys;
                var KeysList = Keys.ToList();

                ws.Row(i).Style.WrapText = true;
                ws.Row(i).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Row(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(i).CustomHeight = true;
                ws.Row(i).Height = 100;

                for(int j = 1 + columnOffset; j <= KeysList.Count + columnOffset; j++)
                {
                    ws.Cells[i, j].Value = data[i - (rowOffset + 1)][KeysList[j - (columnOffset + 1)]];
                }
            }

            package.SaveAs(new FileInfo("ДвижениеУд.xlsx"));
        }
    }
}
