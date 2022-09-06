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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml;
using System.IO;
using Microsoft.Data.SqlClient;
using StatisticProceeding.ViewModel;
using System.ComponentModel;


namespace StatisticProceeding
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel ViewModel = new MainWindowViewModel();
        int Imola = 0;
        bool flag = true;
        public Str str { get; set; } = new Str();
        public MainWindow()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            DataContext = this;
        }

      

        private void ShowWeeklyCaseTableWindow(object sender, RoutedEventArgs e)
        {
            WeeklyCaseTable window = new WeeklyCaseTable();
            window.Show();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                flag = false;
                int j = 0;
                FormFileAsync();

                await Task.Run(() =>
                {
                    while (Imola == 0)
                    {
                        j++;
                        str.Text = j.ToString();
                        System.Threading.Thread.Sleep(100);
                    }
                    Imola = 0;
                    flag = true;
                });
            }

        }


        private async void FormFileAsync()
        {
            await Task.Run(() => ViewModel.FormCaseMovementFile());
            Imola = 1;
        }

    }

    public class Str : Base
    {
        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

    }
}
