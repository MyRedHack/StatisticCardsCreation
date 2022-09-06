using System.Windows;
using System.Windows.Controls;
using StatCardApp.ViewModel;
using StatCardApp.Function;
using System.IO;
using System.Collections.ObjectModel;
using System;
using StatCardApp.Global;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для CaseInformation.xaml
    /// </summary>
    public partial class CaseInformation : Window
    {
        private int ValidErrorCount = 0;
        private CaseInfoViewModel caseInfoViewModel = new CaseInfoViewModel();
        MainWindow MainWindow;
        public CaseInformation()
        {
            InitializeComponent();
            DataContext = caseInfoViewModel;
        }

        #region Обработчик события кнопки создания нового дела

        private void Button_Click_CreateCaseFile(object sender, RoutedEventArgs e)
        {
            if (caseInfoViewModel.CheckCaseIsFilled())
            {
                if (ValidErrorCount == 0)
                {
                    string number = string.Empty;
                    RegModel RegData = (RegModel)StatCardFunc.FromJson<RegModel>("configuration.conf");

                    //Формирование полного номера дела
                    number += caseInfoViewModel.CaseInfo.CaseType.Value;
                    number += caseInfoViewModel.CaseInfo.RegDate.Value.Year.ToString().Substring(2, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.MinistryCode, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.DirectionCode, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.ManagementCode, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.DepartmentCode, 2);
                    number += StatCardFunc.ConvertSizeValue(caseInfoViewModel.CaseInfo.CaseNumber, 6);

                    caseInfoViewModel.CaseInfo.FullCaseNumber = Convert.ToInt64(number);

                    if ( AddCaseToDirectoryList() )
                    {
                        StatCardFunc.createNewCase(caseInfoViewModel.CaseInfo);
                    }
                    
                    
                    this.Close();
                }
                else
                    MessageBox.Show("Введены некорректные значения");
            }
            else
            {
                MessageBox.Show("Введены не все значения");
            }
        }

        #endregion Обработчик события кнопки создания нового дела

        #region Событие для проверки валидации числового текстового поля

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                ValidErrorCount++;
            else
                ValidErrorCount--;
        }

        #endregion Событие для проверки валидации числового текстового поля


        /// <summary>
        /// Функция добавляет дело в визуальный список дел в программе
        /// </summary>
        /// <returns>
        /// true если такого дела нет, false если дело уже существует
        /// </returns>
        private Boolean AddCaseToDirectoryList()
        {
            Boolean CaseCanBeAdded = true;

            MainWindow = (MainWindow)Owner;
            DirectoryInfo caseDirectory = StatCardFunc.CreateDirectoryForCard(caseInfoViewModel.CaseInfo);
            TreeViewFolder folder = new TreeViewFolder(caseDirectory.FullName, caseDirectory.Name);
            folder.root.IsCase = true;
            TreeViewFolder folderParent = new TreeViewFolder(caseDirectory.Parent.FullName, caseDirectory.Parent.Name);
  
            var a = MainWindow.StatCardFolder.root.Items;
            var b = folderParent.root.Title;
            var c = folder.root;
            var item = FindSameItem(a, b);

            if (item != null)
            {

                var item1 = FindSameItem(item.Items, folder.root.Title);

                if (item1 == null)
                {
                    item.Items.Add(c);
                    c.Parent = item;
                }
                else
                {
                    //Дело уже существует в списке и его не нужно добавлять
                    CaseCanBeAdded = false;
                    MessageBox.Show("Такое дело уже есть");
                }
            }
            else
            {
                folderParent.root.Items[0].IsCase = true;
                MainWindow.StatCardFolder.root.Items.Add(folderParent.root);
            }

            return CaseCanBeAdded;
        }

        private TreeViewFolderItem FindSameItem(ObservableCollection<TreeViewFolderItem> items, string checkingTitle)
        {
            TreeViewFolderItem result = null;
            foreach(TreeViewFolderItem item in items)
            {
                if (item.Title == checkingTitle)
                {
                    result = item;
                    break;
                }
            }
            
            return result;
        }


    }
}