
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using StatCardApp.Model;
using StatCardApp.BaseClass;

namespace StatCardApp.Function
{
    public static class StatCardFunc
    {

        #region Создание папки со статкарточками
        public static void CreateCardFileSystem()
        {
            string cardsFolder = ".\\StatisticCards";

            DirectoryInfo dir = new DirectoryInfo(cardsFolder);

            if (!dir.Exists)
            {
                dir.Create();
            }
            
        }
        #endregion

        #region Проверка первого запуска программы
        public static bool CheckFirstStart()
        {
            bool result = true;
            FileInfo file = new FileInfo(@".\configuration.conf");
            
            if (!file.Exists)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region создание нового дела
        public static DirectoryInfo CreateDirectoryForCard(CaseInfoModel caseinfo)
        {
            //путь до папки с годом дела
            string path = @".\StatisticCards\" + caseinfo.RegDate.Value.Year;
            DirectoryInfo dir1 = new DirectoryInfo(path);
            //если папки с годом не существует, то создаём новую
            if (!dir1.Exists)
            {
                dir1.Create();
            }
            //формирование номера дела в шестизначном формате

            path += @"\" + ConvertSizeValue(caseinfo.CaseNumber, 6);

            DirectoryInfo dir2 = new DirectoryInfo(path);
            //если папки c номером дела не существует, то создаём новую
            if (!dir2.Exists)
            {
                dir2.Create();
            }
            DirectoryInfo casePath = new DirectoryInfo(path);

            return casePath;
        }

        public static void createNewCase(CaseInfoModel caseinfo)
        {
            string path = CreateDirectoryForCard(caseinfo).FullName;
            //конечный путь до создаваемого файла
            path += @"\caseInfo.conf";

            FileInfo file = new FileInfo(path);

            if (!file.Exists)
            {
                IntoJson<CaseInfoModel>(caseinfo, path);
            }
            
        }
        #endregion

        #region Сохранение конфигурационных данных для программы 
        public static void createNewConfiguration(RegModel regInfo)
        {
            string path = @".\configuration.conf";

            StatCardFunc.IntoJson<RegModel>(regInfo,path);
        }
        #endregion

        public static string GetFileText(string path)
        {
            string textFromFile;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                textFromFile = System.Text.Encoding.Default.GetString(array);
            }
            return textFromFile;
        }

        public static string GetFileTextUTF8(string path)
        {
            string textFromFile;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                textFromFile = System.Text.Encoding.UTF8.GetString(array);
            }
            return textFromFile;
        }

        #region Перевод Json файла в объект класса
        public static object FromJson<T>(string path)
        {
            T obj;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                obj = JsonConvert.DeserializeObject<T>(textFromFile);
            }

            return obj;
        }

        public static StatisticCard FromJson1(string path, Type cardType)
        {
            StatisticCard cardF;

            string textFromFile = StatCardFunc.GetFileText(path);
            var obj = JsonConvert.DeserializeObject(textFromFile, cardType);
            cardF = (StatisticCard)obj;

            return cardF;
        }
        #endregion

        #region Перевод объект класса в файл Json
        public static void IntoJson<T>(T obj, string path)
        {
            string stroka = JsonConvert.SerializeObject(obj);
            //stroka = Regex.Unescape(stroka);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(stroka);
                // запись массива байтов в файл
                fs.Write(array, 0, array.Length);
            }
        }

        public static void IntoJson<T>(T obj, string path, Type type)
        {
            string stroka = JsonConvert.SerializeObject(obj, type, new JsonSerializerSettings());
            //stroka = Regex.Unescape(stroka);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(stroka);
                // запись массива байтов в файл
                fs.Write(array, 0, array.Length);
            }
        }
        #endregion

        public static string ConvertSizeValue(int? value, int valueResLength)
        {
            if (!value.HasValue) return "";

            string zeroRange = "";
            string result = value.ToString();
            int subStrIndex = Convert.ToString(result).Length;

            for (int i = 0; i < valueResLength; i++)
            {
                zeroRange += "0";
            }

            result = zeroRange + value;
            result = result.Substring(subStrIndex);

            return result;
        }

        public static List<T> GetStatCardList<T>(string casePath, CardType cardType)
        {


            string[] files = Directory.GetFiles(casePath);
            List<T> list = new List<T>();
            
            foreach (string file in files)
            {

                if (file.Contains(".conf")) continue;
                StatisticCard Card = (StatisticCard)FromJson<T>(file);
                if (Card.CardType == cardType)
                    list.Add((T)FromJson<T>(file));
            }

            return list;
        }
        
        public static CaseHierarchy GetCaseHierarchy()
        {
            string path = @".\StatisticCards\";

            DirectoryInfo mainDirectory = new DirectoryInfo(path);

            CaseHierarchy caseHierarchy = new CaseHierarchy();

            foreach(DirectoryInfo year in mainDirectory.GetDirectories())
            {
                caseHierarchy.Years.Add(Convert.ToInt32(year.Name));

                List<long> oneYearCases = new List<long>();

                foreach(DirectoryInfo criminalCase in year.GetDirectories())
                {

                    FileInfo[] files = criminalCase.GetFiles();
                    foreach(FileInfo file in files)
                    {
                        if(file.FullName.Contains(".conf"))
                        {
                            CaseInfoModel caseInfo = (CaseInfoModel)FromJson<CaseInfoModel>(file.FullName);
                            
                            oneYearCases.Add(caseInfo.FullCaseNumber);
                        }
                    }

                }

                caseHierarchy.Cases.Add(Convert.ToInt32(year.Name), oneYearCases);
            }

            return caseHierarchy;
        }

        public static bool MainCardPredicate(StatisticCard card)
        {
            return card.ChangeNumber == 0;
        }

        public static bool ChangedCardPredicate(StatisticCard card)
        {
            return card.ChangeNumber > 0;
        }
        public static void CopyList(ref List<object> main, List<object> copy)
        {
            main.Clear();
            main.AddRange(copy);
        }
    }
}



