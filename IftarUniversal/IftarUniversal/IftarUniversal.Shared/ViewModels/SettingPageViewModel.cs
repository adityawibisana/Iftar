using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IftarUniversal.ViewModels
{
    public class SettingPageViewModel : ViewModel
    {

        #region Command
        public DelegateCommand BackCommand { get; private set; }
        #endregion

        #region Field 
        private INavigationService _navigationService; 
        #endregion

        public SettingPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            BackCommand = new DelegateCommand(() =>
            {
                _navigationService.GoBack();
            });
        }
    }
}
