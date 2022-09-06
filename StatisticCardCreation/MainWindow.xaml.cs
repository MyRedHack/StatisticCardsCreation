using StatCardApp.Client;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using StatCardApp.ViewModel;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.DerivedControls;
using StatCardApp.Global;
using StatCardApp.Model.CardFields;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace StatCardApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        
        string casePath = string.Empty;
        public TreeViewFolder StatCardFolder { get; set; } 
        public TreeViewFolderItem CurrentCase;
        public TreeViewFolderItem CurrentCard;
        public MainWindowViewModel MWViewModel { get; set; } = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.8);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.8);
            treeView1.Width = this.Width * 0.25;
            CasePanel.Width = this.Width * 0.75;
            treeView1.Height = CasePanel.MaxHeight = this.Height * 0.8;
            StatCardList.Height = CasePanel.MaxHeight * 0.7;

            DirectoryInfo StatCardFilesRoot = new DirectoryInfo("StatisticCards\\");
            if (!StatCardFilesRoot.Exists)
            {
                StatCardFunc.CreateCardFileSystem();
            }

            StatCardFileTree.CardAvaliability = MWViewModel.CardAvaliability;
            StatCardFileTree.MainWindow = this;

            DataContext = MWViewModel;
            CardImportMenuItem.DataContext = this;
            
            StatCardFolder = new TreeViewFolder("StatisticCards\\", "Статистические карты");


            treeView1.Items.Add(StatCardFolder.root);

        }

        private void CheckFirstStart(object sender, EventArgs e)
        {
            //Проверка первого запуска программы
            if (!StatCardFunc.CheckFirstStart())
            {
                RegWindow regWindow = new RegWindow();
                this.Hide();
                regWindow.Owner = this;
                regWindow.Show();
            }
        }

        #region Обработчик кнопки создания нового дела
        private void Button_Click_CreateCase(object sender, RoutedEventArgs e)
        {
            CaseInformation caseInfo = new CaseInformation();
            caseInfo.Owner = this;
            caseInfo.Show();
            
        }
        #endregion

        #region Обработчик кнопки отправки дела на сервер
        private void Button_Click_SendCaseToServer(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            //Конвертация файла .json в объект информации о деле
            casePath = (string)button.Tag;
            CaseInfoViewModel vm = new CaseInfoViewModel();
            vm.CaseInfo = (CaseInfoModel)StatCardFunc.FromJson<CaseInfoModel>(casePath + "\\caseInfo.conf");
            vm.TransferData();
        }
        #endregion 

        #region Обработчик кнопки создания карточки Ф-1
        private void Button_Click_CreateF1Card(object sender, RoutedEventArgs e)
        { 
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F1Window f1Window = new F1Window(casePathInfo);
            f1Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f1Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-1.1
        private void Button_Click_CreateF11Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F11Window f11Window = new F11Window(casePathInfo);
            f11Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f11Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-1.2
        private void Button_Click_CreateF12Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F12Window f12Window = new F12Window(casePathInfo);
            f12Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f12Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-1.3
        private void Button_Click_CreateF13Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F13Window f13Window = new F13Window(casePathInfo);
            f13Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f13Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-2
        private void Button_Click_CreateF2Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F2Window f2Window = new F2Window(casePathInfo);
            f2Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f2Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-3
        private void Button_Click_CreateF3Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F3Window f3Window = new F3Window(casePathInfo);
            f3Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f3Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-3.1
        private void Button_Click_CreateF31Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F31Window f31Window = new F31Window(casePathInfo);
            f31Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f31Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-5
        private void Button_Click_CreateF5Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F5Window f5Window = new F5Window(casePathInfo);
            f5Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f5Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-5.5
        private void Button_Click_CreateF55Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F55Window f55Window = new F55Window(casePathInfo);
            f55Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f55Window.ShowDialog();
        }
        #endregion

        #region Обработчик кнопки создания карточки Ф-6
        private void Button_Click_CreateF6Card(object sender, RoutedEventArgs e)
        {
            DirectoryInfo casePathInfo = new DirectoryInfo(casePath);
            F6Window f6Window = new F6Window(casePathInfo);
            f6Window.Owner = this;
            StatCardFileTree.MainWindow = this;
            f6Window.ShowDialog();
        }
        #endregion


        #region Обработчик события кнопки выбора дела
        private void Button_Click_SelectCase(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            //Конвертация файла .json в объект информации о деле
            MWViewModel.CaseInfoVM.CaseInfo = (CaseInfoModel)StatCardFunc.FromJson<CaseInfoModel>((string)button.Tag + "\\caseInfo.conf");
            casePath = (string)button.Tag;

            MWViewModel.StatCardList = (TreeViewFolderItem)button.DataContext;
            
            StatCardFileTree.CurrentCase = MWViewModel.StatCardList;

            MWViewModel.CardAvaliability.Reset();
            foreach(StatCardVisualData data in MWViewModel.StatCardList.CardList)
            {
                MWViewModel.CardAvaliability.AddAvaliableType(data.CardType);
            }
            CaseDescription.IsEnabled = true;
            CaseProductionButton.IsEnabled = true;
            UpdateCardAvaliability();
        }
        #endregion

        #region Обработчик события кнопки удаления дела
        private void Button_Click_DeleteCase(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            //Конвертация файла .json в объект информации о деле
            casePath = ((TreeViewFolderItem)button.DataContext).Path;
            var caseToDelete = (TreeViewFolderItem)button.DataContext;
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить дело?", "Удаление дела", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(casePath, true);
                caseToDelete.Parent.Items.Remove(caseToDelete);
            }

            if (caseToDelete.Parent.Items.Count == 0)
            {
                Directory.Delete(caseToDelete.Parent.Path, true);
                StatCardFolder.root.Items.Remove(caseToDelete.Parent);
            }

            if (MWViewModel.StatCardList.CardList != null)
            {
                MWViewModel.StatCardList.CardList.Clear();
            }
        }
        #endregion

        #region Обработчик открытия окна настроек пользователя
        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            RegModel regModel = new RegModel();
            regModel = (RegModel)StatCardFunc.FromJson<RegModel>( @".\configuration.conf");
            RegWindow userSettings = new RegWindow(regModel);
            userSettings.Show();
        }
        #endregion


        #region Команда импортирования карточки
        private RelayCommand importCard;
        public RelayCommand ImportCard
        {
            get
            {
                return importCard ?? (
                    importCard = new RelayCommand(obj =>
                    {

                        OpenFileDialog dialog = new OpenFileDialog();
                        dialog.Filter = "Json files (*.json)|*.json";
                        if (dialog.ShowDialog() == true)
                        {
                            //Импорт карточки
                            try
                            {
                                LoadCardFromFile(dialog.FileName);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Не удаётся открыть файл");
                            }
                        }
                    }));
            }
        }
        #endregion

        #region Выгрузка карточки из файла
        public void LoadCardFromFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            StatisticCard F = StatCardFunc.FromJson1(path, typeof(StatisticCard));
            var Card = StatCardFunc.FromJson1(path, F.TypeOfCard);
            ((StatisticCard)Card).CallWindow(fileInfo);
        }
        #endregion

        private void EnterCaseDataFieldLostFocus(object sender, RoutedEventArgs e)
        {
            StatCardFunc.IntoJson(MWViewModel.CaseInfoVM.CaseInfo, $"{casePath}/caseInfo.conf");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (StatCardVisualData)button.DataContext;

            string path = item.Path;
            //Если путь содержит расширение .json
            if (path.Contains(".json"))
            {
                StatCardFileTree.CurrentCard = item;

                LoadCardFromFile(path);
            }
        }

        private void DeleteCard(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            //Конвертация файла .json в объект информации о деле
            string cardPath = ((StatCardVisualData)button.DataContext).Path;
            var cardToDelete = (StatCardVisualData)button.DataContext;
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить дело?", "Удаление дела", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                File.Delete(cardPath);
                StatCardFileTree.CurrentCase.CardList.Remove(cardToDelete); 
            }

            MWViewModel.CardAvaliability.DeleteAvaliableType(cardToDelete.CardType);
            UpdateCardAvaliability();
        }

        public void UpdateCardAvaliability()
        {
            newF11Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable && !StatCardFileTree.CardAvaliability.F11IsAvaliable;
            newF12Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable;
            newF13Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable && StatCardFileTree.CardAvaliability.F12IsAvaliable;
            newF2Button.IsEnabled = StatCardFileTree.CardAvaliability.F12IsAvaliable;
            newF3Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable;
            newF31Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable;
            newF5Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable;
            newF55Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable;
            newF6Button.IsEnabled = StatCardFileTree.CardAvaliability.F1IsAvaliable && StatCardFileTree.CardAvaliability.F2IsAvaliable;
        }

        async private void SendToServer(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (StatCardVisualData)button.DataContext;
            var card = StatCardFunc.FromJson1(item.Path, item.CardType);

            bool sentToServer = await OnSendToServerAsync(card, card.TypeOfCard);
            if (sentToServer)
            {
                item.SentToServer = true;
                card.SentToServer = sentToServer;
                StatCardFunc.IntoJson(card, item.Path, item.CardType);
            }
        }

        //Функция отправления карточки на сервер
        protected async Task<bool> OnSendToServerAsync(StatisticCard card, Type cardType)
        {
            UploadingCard cardToUpload = new UploadingCard(card, cardType);
            return await Task.Run(() => cardToUpload.TransferData());
        }

        private void ShowCaseProducationWindow(object sender, RoutedEventArgs e)
        {
            CaseProductionWindow window = new CaseProductionWindow(MWViewModel.CaseInfoVM.CaseInfo);
            window.Show();
        }
    }
}
