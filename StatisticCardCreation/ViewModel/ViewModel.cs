using Microsoft.Win32;
using Newtonsoft.Json;
using StatCardApp.Client;
using StatCardApp.Function;
using StatCardApp.Global;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace StatCardApp.ViewModel
{
    #region БАЗОВЫЙ КЛАСС МОДЕЛИ-ПРЕДСТАВЛЕНИЯ

    abstract public class ViewModel : Base
    {
        public ViewModel(StatisticCard Card)
        {
            #region Инициализация свойств
            this.Card = Card;

            ValidErrorCount = 0;
            if (Card == null)
            {
                RegData = (RegModel)StatCardFunc.FromJson<RegModel>(@".\configuration.conf");
            }
            else
            {
                RegData = this.Card.RegData;
            }
            #endregion Инициализация свойств
        }

        #region Свойства

        //данные списков
        public ListsData data { get; set; } = new ListsData();

        //данные заполнителя
        private RegModel regData = new RegModel();

        public RegModel RegData
        {
            get => regData;
            set
            {
                regData = value;
                OnPropertyChanged("RegData");
            }
        }

        //количество ошибок в полях
        public int ValidErrorCount;

        //информация о пути до расследумого дела
        public DirectoryInfo casePathInfo;

        //Элемент для удаления из любого списка
        private object itemToRemove;

        public object ItemToRemove
        {
            get => itemToRemove;
            set
            {
                itemToRemove = value;
                OnPropertyChanged("ItemToRemove");
            }
        }

        public string ExistingFilePath { get; set; }

        public StatisticCard Card { get; set; }

        #endregion Свойства

        //Функция сохранения карточки
        public void OnSaveCard(StatisticCard card, Type cardType, Boolean sentToServer = false)
        {
            //card.SentToServer = sentToServer;
            //if (StatCardFileTree.CurrentCard != null) StatCardFileTree.CurrentCard.SentToServer = sentToServer;

            //Сохранение в JSON
            if (ExistingFilePath == null)
            {
                SaveNewCard(card);
                MessageBox.Show("Карточка успешно сохранена");
            }
            else
            {
                SaveExistingCard(card);
                MessageBox.Show("Карточка успешно сохранена");
            }
        }

        public void OnSaveImportedCard(StatisticCard card, Type type)
        {
            SavingType savingType = SelectSavingType();  //флаг, отвечающий за выбор в окне сохранения карточки

            if (savingType == SavingType.SavedImport)
            {
                SaveImportedCard(card);
                return;
            }

            if (savingType == SavingType.SavedOriginalSource)
            {
                SaveExistingCard(card);
                return;
            }
        }

        //Функция изменения карточки
        public void OnChangeCard(StatisticCard card, Type cardType)
        {
            string currentDate = DateTime.Today.ToShortDateString();
            string changed = string.Empty;

            card.ChangedFields = new System.Collections.Generic.List<ChangedField>();
            foreach (ChangedField item in StatCardFileTree.ChangedFields)
            {
                card.ChangedFields.Add(item);
            }

            card.CaseInfo = (CaseInfoModel)StatCardFunc.FromJson<CaseInfoModel>(casePathInfo.FullName + "\\caseInfo.conf");
            card.RegData = RegData;
            card.ChangeNumber = DateTime.Now.ToFileTime();
            card.ChangeDate = DateTime.Now;

            card.VisualData = new StatCardVisualData()
            {
                CardType = card.TypeOfCard,
                Date = DateTime.Today,
                SentToServer = false,
                Id = GetCardId(card)
            };

            changed = "(Изменено " + card.ChangeNumber + ")";

            string fileName = $"{casePathInfo.FullName}\\{card.CardType}_{casePathInfo.Name}_{card.VisualData.Id.Replace(":", "_")}_{changed}.json";
            card.VisualData.Path = fileName;

            StatCardFunc.IntoJson(card, fileName, cardType);
            MessageBox.Show("Карточка успешно изменена");

            StatCardFileTree.CurrentCase.CardList.Add(card.VisualData);
        }

        private RelayCommand sendToServer;

        public RelayCommand SendToServer
        {
            get
            {
                return sendToServer ??
                    (sendToServer = new RelayCommand(obj =>
                    {
                        OnSendToServerAsync(Card, Card.TypeOfCard);
                    }));
            }
        }

        //Функция отправления карточки на сервер
        protected async void OnSendToServerAsync(StatisticCard card, Type cardType)
        {
            UploadingCard cardToUpload = new UploadingCard(card, cardType);
            await Task.Run(() => cardToUpload.TransferData());
            card.SentToServer = true;
            OnSaveCard(card, cardType, sentToServer: true);
        }

        private TreeViewFolderItem FindSameItem(ObservableCollection<TreeViewFolderItem> items, string checkingTitle)
        {
            TreeViewFolderItem result = null;
            foreach (TreeViewFolderItem item in items)
            {
                if (item.Title == checkingTitle)
                {
                    result = item;
                    break;
                }
            }

            return result;
        }

        private void WriteImportedCard(StatisticCard card)
        {
            string currentDate = DateTime.Today.ToShortDateString();
            string fileName = $"{casePathInfo.FullName}\\{card.CardType}_{casePathInfo.Name}_{card.VisualData.Id}_{currentDate}.json";
            card.IsExported = false;
            card.VisualData.Id = GetCardId(card);
            card.VisualData.Path = fileName;

            StatCardFunc.IntoJson(card, fileName, card.TypeOfCard);
            ExistingFilePath = fileName;

            TreeViewFolder folder = new TreeViewFolder(casePathInfo.FullName, casePathInfo.Name);
            TreeViewFolder folderParent = new TreeViewFolder(casePathInfo.Parent.FullName, casePathInfo.Parent.Name);

            folder.root.CardList.Clear();
            folder.root.CardList.Add(card.VisualData);

            var yearItem = FindSameItem(StatCardFileTree.MainWindow.StatCardFolder.root.Items, card.VisualData.Date.Year.ToString());

            if (yearItem != null)
            {
                var caseItem = FindSameItem(yearItem.Items, folder.root.Title);

                if (caseItem != null)
                {
                    //folder.root.CardList[0].Path = fileName;
                    //folder.root.CardList[0].CardType = card.TypeOfCard;
                    caseItem.CardList.Add(folder.root.CardList[0]);
                }
                else
                {
                    folder.root.IsCase = true;
                    yearItem.Items.Add(folder.root);
                }
            }
            else
            {
                StatCardFileTree.MainWindow.StatCardFolder.root.Items.Add(folderParent.root);
            }
        }

        private void SaveImportedCard(StatisticCard card)
        {
            casePathInfo = StatCardFunc.CreateDirectoryForCard(card.CaseInfo);
            StatCardFunc.IntoJson(card.CaseInfo, $"{casePathInfo.FullName}/caseInfo.conf");
            ExistingFilePath = null;
            WriteImportedCard(card);
            StatCardFileTree.MainWindow.UpdateCardAvaliability();
        }

        private void SaveNewCard(StatisticCard card)
        {
            string currentDate = DateTime.Now.ToFileTime().ToString();
            string fileName = $"{casePathInfo.FullName}\\{card.CardType}_{casePathInfo.Name}_{currentDate}.json";
            string caseFileText = StatCardFunc.GetFileText(casePathInfo.FullName + @"/caseInfo.conf");
            card.CaseInfo = (CaseInfoModel)JsonConvert.DeserializeObject(caseFileText, typeof(CaseInfoModel));
            card.RegData = RegData;
            card.ChangeNumber = 0;
            card.CardCreationDate = DateTime.Now;

            card.VisualData = new StatCardVisualData()
            {
                CardType = card.TypeOfCard,
                Date = DateTime.Today,
                SentToServer = card.SentToServer,
                Id = GetCardId(card),
                Path = fileName
            };

            ExistingFilePath = fileName;

            StatCardFunc.IntoJson(card, fileName, card.TypeOfCard);
            if (StatCardFileTree.CurrentCase != null)
            {
                StatCardFileTree.CurrentCase.CardList.Add(card.VisualData);
                StatCardFileTree.CurrentCard = StatCardFileTree.CurrentCase.CardList[StatCardFileTree.CurrentCase.CardList.Count - 1];
            }

            StatCardFileTree.CardAvaliability.Reset();
            foreach (StatCardVisualData data in StatCardFileTree.CurrentCase.CardList)
            {
                StatCardFileTree.CardAvaliability.AddAvaliableType(data.CardType);
            }
            StatCardFileTree.MainWindow.UpdateCardAvaliability();
        }

        private void SaveExistingCard(StatisticCard card)
        {
            card.VisualData.Id = GetCardId(card);
            StatCardFileTree.UpdateId(card.VisualData.Path, card.VisualData.Id);
            StatCardFunc.IntoJson(card, ExistingFilePath, card.TypeOfCard);
        }

        private RelayCommand saveCard;

        public RelayCommand SaveCard
        {
            get
            {
                return saveCard ??
                    (saveCard = new RelayCommand(obj =>
                    {
                        var RequiredFields = CheckRequiredFields(Card);
                        if (RequiredFields.HasError)
                        {
                            MessageBox.Show(RequiredFields.Message);
                            return;
                        }

                        //Если все поля валидны и данные совпадают:
                        if (ValidErrorCount == 0)
                        {
                            if (Card.IsExported == false)
                            {
                                OnSaveCard(Card, Card.TypeOfCard);    
                            }
                            else
                            {
                                OnSaveImportedCard(Card, Card.TypeOfCard);
                            }
                            ChangeState.IsChanged = false;
                        }
                        else
                        {
                            MessageBox.Show("Невозможно сохранить статистическую карточку. Исправьте некорректные значения");
                        }
                    },
                    (obj) => Card.Accounting != 9));
            }
        }

        private RelayCommand changeCard;

        public RelayCommand ChangeCard
        {
            get
            {
                return changeCard ?? (changeCard = new RelayCommand(obj =>
                {
                    var RequiredFields = CheckRequiredFields(Card);
                    if (RequiredFields.HasError)
                    {
                        MessageBox.Show(RequiredFields.Message);
                        return;
                    }
                    if (ValidErrorCount == 0)
                    {
                        ChangeState.IsChanged = false;
                        OnChangeCard(Card, Card.TypeOfCard);
                    }
                    else
                    {
                        MessageBox.Show("Невозможно сохранить статистическую карточку. Исправьте некорректные значения");
                    }
                    
                },
                (obj) => Card.Accounting == 9));
            }
        }

        private RelayCommand exportCard;

        public RelayCommand ExportCard
        {
            get
            {
                return exportCard ??
                    (exportCard = new RelayCommand(obj =>
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.Filter = "Json files (*.json)|*.json";
                        var CaseInfo = (CaseInfoModel)StatCardFunc.FromJson<CaseInfoModel>(casePathInfo.FullName + @"\caseInfo.conf");

                        StatisticCard cardToExport = Card;
                        cardToExport.IsExported = true;
                        cardToExport.RegData = this.RegData;
                        cardToExport.CaseInfo = CaseInfo;
                        if (cardToExport.VisualData.CardType == null)
                        {
                            cardToExport.VisualData = new StatCardVisualData()
                            {
                                CardType = cardToExport.TypeOfCard,
                                Date = DateTime.Today,
                                SentToServer = cardToExport.SentToServer,
                                Id = GetCardId(cardToExport),
                                Path = ""
                            };
                        }

                        if (dialog.ShowDialog() == true)
                        {
                            //Сохранение в JSON
                            StatCardFunc.IntoJson(cardToExport, dialog.FileName, Card.TypeOfCard);
                        }
                    }));
            }
        }

        private enum SavingType { SavedImport = 1, SavedOriginalSource = 2, Unsaved = 3 }

        private SavingType SelectSavingType()
        {
            MessageBoxResult res = MessageBox.Show("Сохранить карточку в каталог программы? \n Да - сохранить карточку с импортированием в каталог программы \n Нет - сохранить карточку без импортирования \n" +
                    "Отмена - отменить сохранение", "Сохранение", MessageBoxButton.YesNoCancel);
            switch (res)
            {
                case MessageBoxResult.Yes: return SavingType.SavedImport;
                case MessageBoxResult.No: return SavingType.SavedOriginalSource;
                case MessageBoxResult.Cancel: return SavingType.Unsaved;
                default: return SavingType.Unsaved;
            }
        }

        private string GetCardId(StatisticCard card)
        {
            string result = string.Empty;

            if (card.TypeOfCard == typeof(CardF1))
            {
                result = $"Преступление № {((CardF1)card).CrimeNumber}";
            }

            if (card.TypeOfCard == typeof(CardF1DotOne))
            {
                result = $"Оконченное преступление";
            }

            if (card.TypeOfCard == typeof(CardF1DotTwo))
            {
                result = $"Подозреваемый № {((CardF1DotTwo)card).SuspectNumber} - {((CardF1DotTwo)card).FIO.Surname}";
            }

            if (card.TypeOfCard == typeof(CardF1DotThree))
            {
                result = "Мера пресечения ";
            }

            if (card.TypeOfCard == typeof(CardF2))
            {
                result = $"Лицо по оконченному делу № {((CardF2)card).SuspectNumber} - {((CardF2)card).FIO.Surname}";
            }

            if (card.TypeOfCard == typeof(CardF3))
            {
                result = $"Движение уголовного дела";
            }

            if (card.TypeOfCard == typeof(CardF3DotOne))
            {
                result = $"Талон-уведомление";
            }

            if (card.TypeOfCard == typeof(CardF5))
            {
                result = $"Потерпевший № {((CardF5)card).VictimNumber} - {((CardF5)card).FIO.Surname}";
            }

            if (card.TypeOfCard == typeof(CardF5DotFive))
            {
                result = $"Потерпевший № {((CardF5DotFive)card).VictimNumber} - {((CardF5DotFive)card).OrganizationName}";
            }

            if (card.TypeOfCard == typeof(CardF6))
            {
                result = $"Результат рассмотрения дела судом первой инстанции";
            }

            if (card.ChangeNumber == 0)
            {
                return result;
            }
            else
            {
                return $"{result} (Изменено {card.ChangeDate})";
            }
        }

        private RequiredFieldError CheckRequiredFields(StatisticCard card)
        {
            RequiredFieldError result = new RequiredFieldError();

            if (card.TypeOfCard == typeof(CardF1))
            {
                var F1 = (CardF1)card;
                string message = "Заполните поля: ";
                if (!F1.CrimeNumber.HasValue)
                {
                    result.HasError = true;
                    message += "'Номер преступления'";
                }

                if (!F1.PreInvestigationForm.HasValue)
                {
                    result.HasError = true;
                    message += "'Форма предварительного следствия'";
                }

                if (!F1.StartCaseDate.HasValue)
                {
                    result.HasError = true;
                    message += "'Дата возбуждения уголовного дела'";
                }

                result.Message = message;
            }

            if (card.TypeOfCard == typeof(CardF1DotOne))
            {
                var F11 = (CardF1DotOne)card;
                string message = "Заполните поля: ";
                if (!F11.UPKArticle.HasValue)
                {
                    result.HasError = true;
                    message += "'Решение по делу' ";
                }

                result.Message = message;
            }

            if (card.TypeOfCard == typeof(CardF1DotTwo))
            {
                var F12 = (CardF1DotTwo)card;
                string message = "Заполните поля: ";
                if (!F12.SuspectNumber.HasValue)
                {
                    result.HasError = true;
                    message += "'Порядковый номер подозреваемого' ";
                }

                if (string.IsNullOrWhiteSpace(F12.FIO.Name) || string.IsNullOrWhiteSpace(F12.FIO.Surname) || string.IsNullOrWhiteSpace(F12.FIO.Patronymic))
                {
                    result.HasError = true;
                    message += "'Фамилия' 'Имя' 'Отчество'";
                }

                result.Message = message;
            }

            if (card.TypeOfCard == typeof(CardF5))
            {
                var F5 = (CardF5)card;
                string message = "Заполните поля: ";
                if (!F5.VictimNumber.HasValue)
                {
                    result.HasError = true;
                    message += "'Порядковый номер потерпевшего' ";
                }

                if (string.IsNullOrWhiteSpace(F5.FIO.Name) || string.IsNullOrWhiteSpace(F5.FIO.Surname) || string.IsNullOrWhiteSpace(F5.FIO.Patronymic))
                {
                    result.HasError = true;
                    message += "'Фамилия' 'Имя' 'Отчество'";
                }

                result.Message = message;
            }

            if (card.TypeOfCard == typeof(CardF5DotFive))
            {
                var F55 = (CardF5DotFive)card;
                string message = "Заполните поля: ";
                if (!F55.VictimNumber.HasValue)
                {
                    result.HasError = true;
                    message += "'Порядковый номер потерпевшего' ";
                }

                if (string.IsNullOrWhiteSpace(F55.OrganizationName))
                {
                    result.HasError = true;
                    message += "'Наименование организации'";
                }

                result.Message = message;
            }
            return result;
        }

        private class RequiredFieldError
        {
            public Boolean HasError { get; set; }
            public string Message { get; set; } = string.Empty;
        }
    }

    #endregion БАЗОВЫЙ КЛАСС МОДЕЛИ-ПРЕДСТАВЛЕНИЯ
}