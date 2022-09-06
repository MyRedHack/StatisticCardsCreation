using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StatCardApp.ViewModel;
using StatCardApp.Model;
using StatCardApp.DerivedControls;
using System.Windows.Media.Animation;
using System.ComponentModel;
using StatCardApp.Global;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для F1Window.xaml
    /// </summary>
    public partial class F1Window : Window
    {
        private ViewModelF1 viewModelF1;

        //Конструктор при создании нового документа
        public F1Window(DirectoryInfo casePathInfo)
        {
            InitializeComponent();
            viewModelF1 = new ViewModelF1(casePathInfo);
            viewModelF1.casePathInfo = casePathInfo;
            DataContext = viewModelF1;

            this.Closing += CheckChanges;
        }

        //Конструктор при открытии существующего документа
        public F1Window(CardF1 F1, FileInfo filePathInfo)
        {
            InitializeComponent();
            viewModelF1 = new ViewModelF1(filePathInfo.Directory,  F1);
            viewModelF1.ExistingFilePath = filePathInfo.FullName;

            if (F1.IsExported)
                viewModelF1.RegData = F1.RegData;
            DataContext = viewModelF1;
            this.Closing += CheckChanges;
        }

        #region Обработчик кнопки для просмотра документа

        private void Button_Click_ViewDocument(object sender, RoutedEventArgs e)
        {
            string casePath;
            if (viewModelF1.F1.IsExported)
            {
                casePath = string.Empty;
            }
            else
            {
                casePath = viewModelF1.casePathInfo.FullName;
            }

            DocumentViewerF1 doc = new DocumentViewerF1(viewModelF1.F1, viewModelF1.RegData, casePath);

             doc.ShowDialog();

        }

        #endregion Обработчик кнопки для просмотра документа

        #region Обработчик события валидации числового текстового поля

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Error.ErrorContent.ToString() == "Не удалось преобразовать значение \"\".")
                return;

            ValidationErrorEventAction a = e.Action;

            if (a == ValidationErrorEventAction.Added)
                viewModelF1.ValidErrorCount++;
            else
                viewModelF1.ValidErrorCount--;
        }

        #endregion Обработчик события валидации числового текстового поля

        #region Обработчик события валидации выпадающего списка

        private void CBErrorHandler(object sender, DerivedControlEventArgs e)
        {
            if (e.ErrorAdded == ErrorAdded.Added)
                viewModelF1.ValidErrorCount++;
            if (e.ErrorAdded == ErrorAdded.Removed)
                viewModelF1.ValidErrorCount--;
        }

        #endregion Обработчик события валидации выпадающего списка

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

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            while (tb.ExtentWidth > (tb.ActualWidth - tb.Padding.Left - tb.Padding.Right))
            {
                //MessageBox.Show($"{tb.ExtentWidth} - {(tb.ActualWidth - tb.Padding.Left - tb.Padding.Right)}");
                if (tb.FontSize > 1)
                {
                    tb.FontSize--;
                    tb.UpdateLayout();
                }
                else return;
                //MessageBox.Show($"{tb.ExtentWidth} - {(tb.ActualWidth - tb.Padding.Left - tb.Padding.Right)}");
            }
        }

        private void ExtraInfoList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(viewModelF1.F1.ExtraInfoHasNationalProgramm.ToString());
        }

        private void CheckChanges(object sender, CancelEventArgs e)
        {
            if (ChangeState.IsChanged)
            {
                MessageBoxResult result = MessageBox.Show("Изменения не были сохранены. Вы действительно хотите закрыть окно?", "Выход без сохранения" ,MessageBoxButton.YesNo );
        
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
} 