using IftarUniversal.Service;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace IftarUniversal.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Properties 
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
        #endregion

        #region Fields

        private PrayTime _prayTime;
        private LocationService _locationService; 
        private INavigationService _navigationService;
        private ResourceLoader _langLoader;
        private AppSettingService _appSettingService;
        private BackgroundTaskService _backgroundTaskService;
        Timer _timer;

        #endregion

        #region Commands
        public DelegateCommand PageLoadedCommand { get; private set; }
        public DelegateCommand MenuCommand { get; private set; }
        #endregion

        #region dummy
        double dLat = -8.636867;
        double dLong = 115.26345;
        #endregion
        public MainPageViewModel(PrayTime prayTime, LocationService locationService, INavigationService navigationService, ResourceLoader langLoader, 
            AppSettingService settingService, BackgroundTaskService backgroundTaskService)
        { 
            this._prayTime = prayTime;
            this._locationService = locationService;
            this._navigationService = navigationService;
            this._langLoader = langLoader;
            this._appSettingService = settingService;
            this._backgroundTaskService = backgroundTaskService;

            backgroundTaskService.Create(); 

            Status = "Calculating ...";

            if (PageLoadedCommand == null)
            {
                PageLoadedCommand = new DelegateCommand(() =>
                {
                    Update();
                    StartTimer();
                });
            }

            MenuCommand = new DelegateCommand(() =>
            {
                _timer.Dispose();
                _navigationService.Navigate("Setting", null);
            });
        } 

        private void StartTimer()
        { 
           _timer = new Timer(new TimerCallback( async (state) =>
           {
               await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
               {
                   Second--;
                   if (Second<0)
                   {
                       Second = 59;
                       Minute--;
                       if (Minute<0)
                       {
                           Hour--;
                           if (Hour < 0)
                           { 
                               Hour = 0;
                               Minute = 0;
                               Second = 0;

                               _timer.Dispose();
                               Update();
                           }
                           else
                           {
                               Minute = 59;
                           }
                       }

                   }
               });
           }), new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)); 
        }

        public async void Update()
        {
            DateTime now = DateTime.Now;

            bool isFirstTimeScheduler = false;
            if (!_appSettingService.IsLocationSet)
            {
                Geoposition position = await _locationService.GetPosition();

                if (position == null)
                {
                    MessageDialog dialog = new MessageDialog("We can not get your location. Application will now exit");
                    await dialog.ShowAsync();

                    App.Current.Exit();
                } 

                _appSettingService.UserLatitude = position.Coordinate.Point.Position.Latitude;
                _appSettingService.UserLongitude = position.Coordinate.Point.Position.Longitude;

                _appSettingService.IsLocationSet = true;
                isFirstTimeScheduler = true;

                //if (!_appSettingService.IsLocationEnabled)
                //{
                //    MessageDialog dialog = new MessageDialog("Location must be enabled before you can continue");
                //    await dialog.ShowAsync();
                //    _navigationService.Navigate("PrivacyPolicyPage", null);
                //}
            } 

            var x = _prayTime.getPrayerTimes(now.Year, now.Month, now.Day, 
                _appSettingService.UserLatitude, _appSettingService.UserLongitude, 
                TimeZoneInfo.Local.BaseUtcOffset.Hours); 


            DateTime fajrTime = new DateTime(now.Year, now.Month, now.Day, MicroTimeConvert(x[0])[0], MicroTimeConvert(x[0])[1], 0);
            DateTime maghribTime = new DateTime(now.Year, now.Month, now.Day, MicroTimeConvert(x[5])[0], MicroTimeConvert(x[5])[1], 0);

            long diff = 0;
            if (fajrTime.Ticks > now.Ticks)
            {
                //saur
                Status = _langLoader.GetString("Suhoor Time Remaining");
                diff = fajrTime.Ticks - now.Ticks;

            }
            else if (maghribTime.Ticks > now.Ticks)
            {
                // puasa
                Status = _langLoader.GetString("Wait");
                diff = maghribTime.Ticks - now.Ticks;
            }
            else
            {
                //sisanya
                Status = _langLoader.GetString("Enjoy the Iftar");
            }

            TimeSpan ts = TimeSpan.FromTicks(diff);

            Hour = ts.Hours;
            Minute = ts.Minutes;
            Second = ts.Seconds; 
            
            if (isFirstTimeScheduler)
            { 
                ToastNotificationService toast = new ToastNotificationService();
                toast.CreateToast(fajrTime.AddMinutes(-20.0), "20 minutes before Fajr");
                toast.CreateToast(maghribTime, "Iftar Time"); 
            } 
        }

        private int[] MicroTimeConvert(String time)
        {
            String[] t = time.Split(':');
            return new int[] { int.Parse(t[0]), int.Parse(t[1]) };
        }

        

        

        
    }
}
