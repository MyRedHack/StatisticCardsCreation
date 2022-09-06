using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StatCardApp.ViewModel;
using StatCardApp.Model;
using StatCardApp.DerivedControls;
using System.ComponentModel;
using StatCardApp.Global;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для F3Window.xaml
    /// </summary>
    public partial class F3Window : Window
    {
        private ViewModelF3 ViewModel;

        public F3Window(DirectoryInfo casePathInfo)
        {
            InitializeComponent();
            ViewModel = new ViewModelF3(casePathInfo);
            DataContext = ViewModel;
            this.Closing += CheckChanges;
        }

        //Конструктор при открытии существующего документа
        public F3Window(CardF3 F3, FileInfo FilePathInfo)
        {
            InitializeComponent();
            ViewModel = new ViewModelF3(FilePathInfo.Directory, F3);
            ViewModel.ExistingFilePath = FilePathInfo.FullName;

            if (F3.IsExported)
                ViewModel.RegData = F3.RegData;
            DataContext = ViewModel;
            this.Closing += CheckChanges;
        }

        #region Обработчик кнопки для просмотра документа

        private void Button_Click_ViewDocument(object sender, RoutedEventArgs e)
        {
            string casePath;
            if (ViewModel.F3.IsExported)
            {
                casePath = string.Empty;
            }
            else
            {
                casePath = ViewModel.casePathInfo.FullName;
            }

            DocumentViewerF3 doc = new DocumentViewerF3(ViewModel.F3, ViewModel.RegData, ViewModel.casePathInfo.FullName);

            doc.ShowDialog();
        }

        #endregion Обработчик кнопки для просмотра документа

        #region Обработчик события валидации в числовом текстовом поле

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Error.ErrorContent.ToString() == "Не удалось преобразовать значение \"\".")
                return;

            ValidationErrorEventAction a = e.Action;

            if (a == ValidationErrorEventAction.Added)
                ViewModel.ValidErrorCount++;
            else
                ViewModel.ValidErrorCount--;
        }

        #endregion Обработчик события валидации в числовом текстовом поле

        #region Обработчик события валидации в выпадающем списке

        private void CBErrorHandler(object sender, DerivedControlEventArgs e)
        {
            if (e.ErrorAdded == ErrorAdded.Added)
                ViewModel.ValidErrorCount++;
            if (e.ErrorAdded == ErrorAdded.Removed)
                ViewModel.ValidErrorCount--;
        }

        #endregion Обработчик события валидации в выпадающем списке

        #region Ограничение ввода в числовое текстовое поле

        private void IntTextValidate(object sender, KeyEventArgs e)
        {
            Key[] arrayKey = {Key.NumPad0, Key.NumPad1, Key.NumPad2,
                              Key.NumPad3, Key.NumPad4, Key.NumPad5,
                                Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9, Key.D0,
                                Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.Back, Key.Tab };

            if (!arrayKey.Contains(e.Key)) e.Handled = true;
        }

        #endregion Ограничение ввода в числовое текстовое поле

        private void CheckChanges(object sender, CancelEventArgs e)
        {
            if (ChangeState.IsChanged)
            {
                MessageBoxResult result = MessageBox.Show("Изменения не были сохранены. Вы действительно хотите закрыть окно?", "Выход без сохранения", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}