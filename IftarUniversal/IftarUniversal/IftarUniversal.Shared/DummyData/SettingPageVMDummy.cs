using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace IftarUniversal.DummyData
{
    public class SettingPageVMDummy : ViewModel
    {
        #region Properties
        public ObservableCollection<String> MenuList { get; set; }
        #endregion 

        public SettingPageVMDummy()
        { 
            MenuList = new ObservableCollection<String>();
            MenuList.Add("Privacy Policy");
            MenuList.Add("About");  
        }
    }
}
