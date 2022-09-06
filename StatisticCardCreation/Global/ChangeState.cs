using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Global
{
    public static class ChangeState
    {
        private static bool isChanged;

        public static bool IsChanged 
        { 
            get => isChanged;
            set
            {
                isChanged = value;
            } 

        }
    }
}
