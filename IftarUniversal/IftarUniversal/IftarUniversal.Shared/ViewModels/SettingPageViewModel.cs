using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IftarUniversal.ViewModels
{
    public class SettingPageViewModel : ViewModel
    {
        #region Properties
        public ObservableCollection<String> MenuList { get; set; }
        #endregion

        #region Command
        public DelegateCommand BackCommand { get; private set; }

        #endregion

        #region Field 
        private INavigationService _navigationService; 
        #endregion

        public SettingPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService; 

            MenuList = new ObservableCollection<String>();
            MenuList.Add("Privacy Policy");
            MenuList.Add("About");

            BackCommand = new DelegateCommand(() =>
            {
                _navigationService.GoBack();
            });


        }
    }
}
