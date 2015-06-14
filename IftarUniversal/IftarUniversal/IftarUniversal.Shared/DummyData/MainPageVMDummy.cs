using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace IftarUniversal.DummyData
{
    public class MainPageVMDummy : ViewModel
    {
        private int _hour;
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                SetProperty(ref _hour, value);
            }
        }

        private int _minute;
        public int Minute
        {
            get
            {
                return _minute;
            }
            set
            {
                SetProperty(ref _minute, value);
            }
        }

        private int _second;
        public int Second
        {
            get
            {
                return _second;
            }
            set
            {
                SetProperty(ref _second, value);
            }
        }

        private String _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        public MainPageVMDummy()
        {
            Hour = 99;
            Minute = 59;
            Second = 48;
            Status = "Sahoor Time Remaining";
        }
    }
}
