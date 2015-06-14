using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace IftarUniversal.ViewModels
{
    public class SettingPageViewModel : ViewModel
    {
        #region Properties
        public ObservableCollection<String> MenuList { get; set; }
        #endregion

        #region Command
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand<ItemClickEventArgs> MenuClickedCommand { get; private set; }

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

            MenuClickedCommand = new DelegateCommand<ItemClickEventArgs>(async (args) =>
            {
                if (args.ClickedItem.ToString() == MenuList[0])
                {
                    navigationService.Navigate("PrivacyPolicy", null);
                }
                else if (args.ClickedItem.ToString() == MenuList[1])
                {
                    MessageDialog dialog = new MessageDialog("Created by : Aditya Wibisana" + Environment.NewLine + "aditya.wibisana@gmail.com");
                    await dialog.ShowAsync();
                }
            }); 
        }
    }
}
