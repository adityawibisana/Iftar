using IftarUniversal.Service;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

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
        #endregion

        #region Fields

        private IHelloService _helloService;
        private PrayTime _prayTime;
        private LocationService _locationService;
        private Geoposition _position;

        #endregion


        public MainPageViewModel(IHelloService helloService, PrayTime prayTime, LocationService locationService)
        {
            this._helloService = helloService;
            this._prayTime = prayTime;
            this._locationService = locationService;

            
        }

        public async void Update()
        {
            DateTime dt = DateTime.Now;

            if (_position == null)
            {
                _position = await _locationService.GetPosition();
            }

            var x = _prayTime.getPrayerTimes(dt.Year, dt.Month, dt.Day, _position.Coordinate.Latitude, _position.Coordinate.Longitude, TimeZoneInfo.Local.BaseUtcOffset.Hours);

            int[] hourMin = MicroTimeConvert(x[5]);

            // wait for iftar
            if (dt.Hour <= hourMin[0] && dt.Minute < hourMin[1])
            {

            }
            else
            {

            }

            //DateTime magribTime = new DateTime(dt.Year, dt.Month, dt.Day, hourMin[0], hourMin[1], 0);
           
        }

        private int[] MicroTimeConvert(String time)
        {
            String[] t = time.Split(':');
            return new int[] { int.Parse(t[0]), int.Parse(t[1]) };
        }

        

        

        
    }
}
