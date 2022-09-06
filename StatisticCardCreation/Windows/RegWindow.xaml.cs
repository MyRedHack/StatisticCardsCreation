using System.Windows;
using StatCardApp.ViewModel;
using StatCardApp.Function;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>

    public partial class RegWindow : Window
    {
        private RegViewModel regViewModel = new RegViewModel();

        public RegWindow()
        {
            InitializeComponent();
            DataContext = regViewModel;
            this.Closed += CloseOwner;
            this.SaveButton.Click += SaveNewPersonInfo;  
        }

        public RegWindow(RegModel regModel)
        {
            InitializeComponent();
            regViewModel.RegInfo = regModel;
            DataContext = regViewModel;
            this.SaveButton.Click += SavePersonInfo;
        }

        #region Обработчик события кнопки сохранения информации о пользователе

        private void SaveNewPersonInfo(object sender, RoutedEventArgs e)
        {
            StatCardFunc.createNewConfiguration(regViewModel.RegInfo);
            this.Owner.Show();
            this.Closed -= CloseOwner;
            this.Close();
        }

        private void SavePersonInfo(object sender, RoutedEventArgs e)
        {
            StatCardFunc.createNewConfiguration(regViewModel.RegInfo);
            this.Close();
        }
        #endregion Обработчик события кнопки сохранения информации о пользователе

        private void CloseOwner(object sender, System.EventArgs e)
        {
            this.Owner.Close();
        }
    }
}