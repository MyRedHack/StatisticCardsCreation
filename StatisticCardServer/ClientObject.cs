using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using StatCardApp.Model;
using StatisticCardServer.DataBase;
using System.IO;
using System.Windows;
using StatCardApp.ViewModel;
using Microsoft.Data.SqlClient;

namespace StatisticCardServer
{
    class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient client)
        {
            this.client = client;
        }

        public void DataProccess()
        {
            NetworkStream stream = null;
            
            
            try
            {
                byte[] data = new byte[255];
                stream = client.GetStream();
                
                while(true)
                {
                    string message = String.Empty;
                    int bytes = 0;

                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        message += Encoding.Default.GetString(data, 0, bytes);
                    }
                    while (stream.DataAvailable);

                    string messageID = string.Empty;
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        messageID = message.Substring(0, message.IndexOf(']') + 1);
                        message = message.Replace(messageID, "");
                    }

                    ProccessQuery(messageID, message, stream);                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        private void SendException(ServerException exception, NetworkStream stream)
        {
            string message = JsonConvert.SerializeObject(exception, typeof(ServerException), new JsonSerializerSettings());

            byte[] data = Encoding.Default.GetBytes(message);

            stream.Write(data, 0, data.Length);
        }

        private void ProccessQuery(string queryID, string message, NetworkStream stream)
        {
            if (queryID == "[StatisticCard]")
            {
                ProccessStatisticCard(message, stream);
            }

            if (queryID == "[CaseInfo]")
            {
                ProccessCaseInformation(message, stream);
            }

            if (queryID == "[CaseProduction]")
            {
                ProccessCaseProduction(message, stream);
            }

            if (queryID == "[GETWEEKLYCASETABLE]")
            {
                ProccessWeeklyCaseTable(stream);
            }

            if (queryID == "[SETWEEKLYCASETABLE]")
            {
                ProccessWeeklyCaseTable(message, stream);
            }

            if (queryID == "[EXECUTEREADER]")
            {
                ProccessCustomQuery(message, stream);
            }
        }

        private void ProccessStatisticCard(string message, NetworkStream stream)
        {
            var card = (StatisticCard)JsonConvert.DeserializeObject(message, typeof(StatisticCard));

            if (card != null)
            {
                StatisticCard typedCard = (StatisticCard)JsonConvert.DeserializeObject(message, card.TypeOfCard);
                CriminalCaseDB db = new CriminalCaseDB();
                db.Connect();
                db.InsertCard(typedCard);
                SendException(db.Exception, stream);
            }
        }

        private void ProccessCaseInformation(string message, NetworkStream stream)
        {
            var caseInfo = (CaseInfoModel)JsonConvert.DeserializeObject(message, typeof(CaseInfoModel));
            CriminalCaseDB db = new CriminalCaseDB();
            db.Connect();
            db.InsertCase(caseInfo);
            SendException(db.Exception, stream);
        }

        private void ProccessCaseProduction(string message, NetworkStream stream)
        {
            var productionInfo = (CaseProductionInfo)JsonConvert.DeserializeObject(message, typeof(CaseProductionInfo));
            CriminalCaseDB db = new CriminalCaseDB();
            db.Connect();
            db.InsertCaseProduction(productionInfo);
            SendException(db.Exception, stream);
        }

        private void ProccessWeeklyCaseTable(NetworkStream stream)
        {
            CriminalCaseDB db = new CriminalCaseDB();
            db.Connect();
            List<CaseDescriptionStatus> list = db.GetCasesList();
            SendWeeklkyCaseList(list, stream);
        }

        private void SendWeeklkyCaseList(List<CaseDescriptionStatus> list, NetworkStream stream)
        {
            string message = JsonConvert.SerializeObject(list, typeof(List<CaseDescriptionStatus>), new JsonSerializerSettings());

            byte[] data = Encoding.Default.GetBytes(message);

            stream.Write(data, 0, data.Length);
        }

        private void ProccessWeeklyCaseTable(string message, NetworkStream stream)
        {
            var list = (List<CaseDescriptionStatus>)JsonConvert.DeserializeObject(message, typeof(List<CaseDescriptionStatus>));
            CriminalCaseDB db = new CriminalCaseDB();
            db.Connect();
            try
            {
                db.InsertInfoIntoDataBase(list);
            }
            catch(SqlException e)
            {
                db.Exception.HasError = true;
                db.Exception.Message = e.Message;
            }
            SendException(db.Exception, stream);
        }

        private void ProccessCustomQuery(string message, NetworkStream stream)
        {
            
            CriminalCaseDB db = new CriminalCaseDB();
            db.Connect();

            Dictionary<int, Dictionary<int, Dictionary<string, string>>> dict = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();

            for (int i = 1; i < 100; i++)
            {
                try
                {
                    message = StatCardApp.Function.StatCardFunc.GetFileTextUTF8($"query\\{i}.txt");
                    SqlDataReader Reader = db.ExecuteReader(message);            
                    Dictionary<int, Dictionary<string, string>> queryResultDictionary = QueryResultToDictionary(Reader);
                    Reader.Close();
                    dict.Add(i, queryResultDictionary);
                }
                catch (Exception e) { }
            }

            SendCaseMovementData(dict, stream);
                
            
        }

        private void SendCaseMovementData(Dictionary<int, Dictionary<int, Dictionary<string, string>>> dict, NetworkStream stream)
        {
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("WakeUp");
            string message = JsonConvert.SerializeObject(dict, typeof(Dictionary<int, Dictionary<int, Dictionary<string, string>>>), new JsonSerializerSettings());

            byte[] data = Encoding.Default.GetBytes(message);

            stream.Write(data, 0, data.Length);
        }

        private Dictionary<int, Dictionary<string, string>> QueryResultToDictionary(SqlDataReader reader)
        {
            Dictionary<int, Dictionary<string, string>> queryResult = new Dictionary<int, Dictionary<string, string>>();
            string message = string.Empty;

            if (reader.HasRows)
            {
                int id = 0;
                while (reader.Read())
                {
                    Dictionary<string, string> row = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        try
                        {
                            row.Add(reader.GetName(i), reader.GetDateTime(i).ToShortDateString());
                        }
                        catch(Exception e)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i).ToString());
                        }
                        
                    }

                    queryResult.Add(id, row);
                    id++;
                }
            }

            return queryResult;
        }
    }
}
