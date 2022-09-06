using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StatCardApp.Model.CardFields;
using StatCardApp;
using StatCardApp.Model;

namespace StatCardApp.Global
{
    public class StatCardFileTree
    {

        public static TreeViewFolderItem CurrentCase { get; set; }
        public static StatCardVisualData CurrentCard { get; set; }

        public static CardAvaliability CardAvaliability { get; set; }

        public static MainWindow MainWindow { get; set; }

        public static void UpdateId(string Path, string Id)
        {
            foreach(StatCardVisualData VisualData in MainWindow.MWViewModel.StatCardList.CardList)
            {
                if (VisualData.Path == Path)
                {
                    VisualData.Id = Id;
                }
            }
        }

        public static List<ChangedField> ChangedFields { get; set; }
        public static void AddChangedField(ChangedField field)
        {
            bool flag = true;
            ChangedField itemToRemove = new ChangedField();

            if (ChangedFields == null) ChangedFields = new List<ChangedField>();

            try
            {
                foreach (ChangedField item in ChangedFields)
                {
                    if (item.Field == field.Field)
                    {
                        flag = false;
                        itemToRemove = item;
                        break;
                    }

                }
            }
            catch { }

            if (flag) ChangedFields.Add(field);
            else
            {
                ChangedFields.Remove(itemToRemove);
                ChangedFields.Add(field);
            }

        }
    }
}
