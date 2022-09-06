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
    /// Логика взаимодействия для F11Window.xaml
    /// </summary>
    public partial class F11Window : Window
    {
        #region Основное определение

        private ViewModelF1DotOne ViewModelF1DotOne;

        //Конструктор при создании нового документа
        public F11Window(DirectoryInfo casePathInfo)
        {
            InitializeComponent();
            ViewModelF1DotOne = new ViewModelF1DotOne(casePathInfo);
            ViewModelF1DotOne.casePathInfo = casePathInfo;
            DataContext = ViewModelF1DotOne;
            this.Closing += CheckChanges;
        }

        //Конструктор при открытии существующего документа
        public F11Window(CardF1DotOne F11, FileInfo FilePathInfo)
        {
            InitializeComponent();
            ViewModelF1DotOne = new ViewModelF1DotOne(FilePathInfo.Directory, F11);
            ViewModelF1DotOne.ExistingFilePath = FilePathInfo.FullName;

            if (F11.IsExported)
                ViewModelF1DotOne.RegData = F11.RegData;
            DataContext = ViewModelF1DotOne;
            this.Closing += CheckChanges;
        }

        #endregion Основное определение

        #region Обработчик кнопки для просмотра документа

        private void Button_Click_ViewDocument(object sender, RoutedEventArgs e)
        {
            string casePath;
            if (ViewModelF1DotOne.F11.IsExported)
            {
                casePath = string.Empty;
            }
            else
            {
                casePath = ViewModelF1DotOne.casePathInfo.FullName;
            }

            DocumentViewerF11 doc = new DocumentViewerF11(ViewModelF1DotOne.F11, ViewModelF1DotOne.RegData, ViewModelF1DotOne.casePathInfo.FullName);
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
                ViewModelF1DotOne.ValidErrorCount++;
            else
                ViewModelF1DotOne.ValidErrorCount--;
        }

        #endregion Обработчик события валидации в числовом текстовом поле

        #region Обработчик события валидации в выпадающем списке

        private void CBErrorHandler(object sender, DerivedControlEventArgs e)
        {
            if (e.ErrorAdded == ErrorAdded.Added)
                ViewModelF1DotOne.ValidErrorCount++;
            if (e.ErrorAdded == ErrorAdded.Removed)
                ViewModelF1DotOne.ValidErrorCount--;
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