using IftarUniversal.Service;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace IftarUniversal.ViewModels
{
    public class PrivacyPolicyPageViewModel : ViewModel
    {
        #region Properties
        private bool _isLocationEnabled;
        public bool IsLocationEnabled
        {
            get
            {
                return _isLocationEnabled;
            }
            set
            {
                _appSettingService.IsLocationEnabled = value; 
                SetProperty(ref _isLocationEnabled, value); 
            }
        } 
        #endregion

        #region Field
        private AppSettingService _appSettingService;
        #endregion

        #region Command
        public DelegateCommand<bool?> IsEnabledChangedCommand { get; private set; }
        public DelegateCommand PageLoadedCommand { get; private set; }
        #endregion
        public PrivacyPolicyPageViewModel(AppSettingService appSettingService)
        {
            this._appSettingService = appSettingService;
            PageLoadedCommand = new DelegateCommand(() =>
            {
                IsLocationEnabled = _appSettingService.IsLocationEnabled; 
            });
             
        }
    }
}
