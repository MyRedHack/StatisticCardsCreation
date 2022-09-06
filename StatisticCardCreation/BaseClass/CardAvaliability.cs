using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Model;

namespace StatCardApp
{
    public class CardAvaliability : Base
    {
        public CardAvaliability()
        {
            F1Count = 0;
            F11Count = 0;
        }

        #region Ф-1
        private int F1Count;
        private Boolean _F1IsAvaliable;
        public Boolean F1IsAvaliable
        {
            get => _F1IsAvaliable;
            set
            {
                if (value)
                {
                    F1Count++;
                }
                else
                {
                    F1Count--;
                }

                if (F1Count > 0)
                {
                    _F1IsAvaliable = true;
                }
                else
                {
                    _F1IsAvaliable = false;
                }
            }
        }
        #endregion

        #region Ф-1.1
        private int F11Count;
        private Boolean _F11IsAvaliable;
        public Boolean F11IsAvaliable
        {
            get => _F11IsAvaliable;
            set
            {
                if (value)
                {
                    F11Count++;
                }
                else
                {
                    F11Count--;
                }

                if (F11Count > 0)
                {
                    _F11IsAvaliable = true;
                }
                else
                {
                    _F11IsAvaliable = false;
                }
            }
        }
        #endregion

        #region Ф-1.2
        private int F12Count;
        private Boolean _F12IsAvaliable;
        public Boolean F12IsAvaliable
        {
            get => _F12IsAvaliable;
            set
            {
                if (value)
                {
                    F12Count++;
                }
                else
                {
                    F12Count--;
                }

                if (F12Count > 0)
                {
                    _F12IsAvaliable = true;
                }
                else
                {
                    _F12IsAvaliable = false;
                }
            }
        }
        #endregion

        #region Ф-2
        private int F2Count;
        private Boolean _F2IsAvaliable;
        public Boolean F2IsAvaliable
        {
            get => _F2IsAvaliable;
            set
            {
                if (value)
                {
                    F2Count++;
                }
                else
                {
                    F2Count--;
                }

                if (F2Count > 0)
                {
                    _F2IsAvaliable = true;
                }
                else
                {
                    _F2IsAvaliable = false;
                }
            }
        }
        #endregion



        public void AddAvaliableType(Type type)
        {
            if (type == typeof(CardF1))
            {
                F1IsAvaliable = true;
            }

            if (type == typeof(CardF1DotOne))
            {
                F11IsAvaliable = true;
            }

            if (type == typeof(CardF1DotTwo))
            {
                F12IsAvaliable = true;
            }

            if (type == typeof(CardF2))
            {
                F2IsAvaliable = true;
            }
        }
        public void DeleteAvaliableType(Type type)
        {
            if (type == typeof(CardF1))
            {
                F1IsAvaliable = false;
            }

            if (type == typeof(CardF1DotOne))
            {
                F11IsAvaliable = false;
            }

            if (type == typeof(CardF1DotTwo))
            {
                F12IsAvaliable = false;
            }

            if (type == typeof(CardF2))
            {
                F2IsAvaliable = false;
            }
        }

        public void Reset()
        {
            _F1IsAvaliable = false;
            F1Count = 0;

            _F11IsAvaliable = false;
            F11Count = 0;

            _F12IsAvaliable = false;
            F12Count = 0;

            _F2IsAvaliable = false;
            F2Count = 0;
        }
    }
}
