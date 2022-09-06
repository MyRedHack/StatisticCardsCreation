using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StatisticProceeding.ViewModel;
using OfficeOpenXml;
using System.IO;

namespace StatisticProceeding
{
    /// <summary>
    /// Логика взаимодействия для WeeklyCaseTable.xaml
    /// </summary>
    public partial class WeeklyCaseTable : Window
    {
        WeeklyCaseTableViewModel ViewModel = new WeeklyCaseTableViewModel();
        public WeeklyCaseTable()
        {
            InitializeComponent();
            DataContext = ViewModel;

        }

        private void FormExcelFile(FileInfo file, List<CaseDescriptionStatus> data)
        {
            //DeleteIfExist(file);

            var package = new ExcelPackage(file);

            //var ws = package.Workbook.Worksheets.Add(Name:"Список личного состава дежурки ВСО");
            var ws = package.Workbook.Worksheets[0];

            try
            {
                ws.Tables.Delete("Table1");
            }
            catch (Exception e) { }

            ws.Column(1).Width = 4.14;
            ws.Column(2).Width = 23.29;
            ws.Column(3).Width = 29.57;
            ws.Column(4).Width = 21.71;
            ws.Column(5).Width = 121.86;
            ws.Column(6).Width = 22.57;
            ws.Column(7).Width = 13.43;
            ws.Column(8).Width = 13;
            ws.Column(9).Width = 89;

            for (int i = 4; i < data.Count+4; i++)
            {
                ws.Row(i).Style.WrapText = true;
                ws.Row(i).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Row(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(i).CustomHeight = true;
                ws.Row(i).Height = 100;

                ws.Cells[i, 1].Value = i-3;
                ws.Cells[i, 2].Value = data[i-4].CaseNumber;
                ws.Cells[i, 3].Value = data[i-4].CriminalProceedings;
                ws.Cells[i, 4].Value = data[i-4].Suspects;
                ws.Cells[i, 5].Value = data[i-4].CrimeInfo;
                ws.Cells[i, 6].Value = data[i-4].CriminalProductions;
                ws.Cells[i, 7].Value = data[i-4].InvestigationTerm;
                ws.Cells[i, 8].Value = data[i-4].GuardTerm;
                ws.Cells[i, 9].Value = data[i-4].InvestigationActions;

                ws.Cells[i, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ws.Cells[i, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);


            }
           


            for (int i = 4; i < data.Count + 4; i++)
            {
                ws.Row(i).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                switch (data[i - 4].Status)
                {
                    case 0: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow); break;
                    case 2: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue); break;
                    case 3: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red); break;
                    case 5: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray); break;
                    case 7: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGreen); break;
                    case 8: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Purple); break;
                    default: ws.Row(i).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White); break;
                }

            }

            package.SaveAs(new FileInfo("result.xlsx"));


        }

        private void DeleteIfExist(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public List<CaseDescription> DowncastCasesList(List<CaseDescriptionStatus> CasesListStatus)
        {
            List<CaseDescription> CasesList = new List<CaseDescription>();

            foreach(CaseDescriptionStatus item in CasesListStatus)
            {
                CasesList.Add(item);
            }
                
            return CasesList;
        }



        private void FormWeeklyCaseTable(object sender, RoutedEventArgs e)
        {
            try
            {
                FileInfo file = new FileInfo(fileName: @"file.xlsx");
                FormExcelFile(file, ViewModel.CaseDescriptions);
                MessageBox.Show("Файл сформирован");
            }
            catch (Exception ee)
            {
                MessageBox.Show($"Обшибка - {ee.Message}");
            }

            ViewModel.InsertInfoToDataBase();
        }
    }
}
