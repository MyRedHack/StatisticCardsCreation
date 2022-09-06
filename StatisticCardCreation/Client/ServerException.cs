using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticCardServer.DataBase
{
    public class ServerException
    {
        public ServerException() 
        {
            HasError = false;
        }

        public ServerException(string msg)
        {
            Message = msg;
            HasError = false;
        }
        public string Message { get; set; }
        public Boolean HasError { get; set; }
    }
}
