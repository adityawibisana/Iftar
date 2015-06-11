using IftarUniversal.Service;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

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
        private Geoposition _position;
        private INavigationService _navigationService;
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
        public MainPageViewModel(PrayTime prayTime, LocationService locationService, INavigationService navigationService)
        { 
            this._prayTime = prayTime;
            this._locationService = locationService;
            this._navigationService = navigationService;

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

        public void Update()
        { 
            DateTime now = DateTime.Now;

            //if (_position == null)
            //{
            //    _position = await _locationService.GetPosition();
            //}

            //var x = _prayTime.getPrayerTimes(now.Year, now.Month, now.Day, _position.Coordinate.Latitude, _position.Coordinate.Longitude, TimeZoneInfo.Local.BaseUtcOffset.Hours);
            var x = _prayTime.getPrayerTimes(now.Year, now.Month, now.Day, dLat, dLong, TimeZoneInfo.Local.BaseUtcOffset.Hours);


            DateTime fajrTime = new DateTime(now.Year, now.Month, now.Day, MicroTimeConvert(x[0])[0] , MicroTimeConvert(x[0])[1], 0);
            DateTime maghribTime = new DateTime(now.Year, now.Month, now.Day, MicroTimeConvert(x[5])[0], MicroTimeConvert(x[5])[1], 0);

            long diff = 0;
            if (fajrTime.Ticks > now.Ticks)
            {
                //saur
                Status = "Suhoor Time Remaining";
                diff = fajrTime.Ticks - now.Ticks;

            } 
            else if (maghribTime.Ticks > now.Ticks)
            {
                // puasa
                Status = "Wait";
                diff = maghribTime.Ticks - now.Ticks;
            }
            else
            {
                //sisanya
                Status = "Enjoy the Iftar";
            } 

            TimeSpan ts = TimeSpan.FromTicks(diff);

            Hour = ts.Hours;
            Minute = ts.Minutes;
            Second = ts.Seconds; 
        }

        private int[] MicroTimeConvert(String time)
        {
            String[] t = time.Split(':');
            return new int[] { int.Parse(t[0]), int.Parse(t[1]) };
        }

        

        

        
    }
}
