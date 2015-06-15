using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace IftarUniversal.Service
{
    public class AppSettingService
    {
        private Windows.Storage.ApplicationDataContainer localSettings;

        public AppSettingService()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        public bool IsFirstTimeScheduler
        {
            get
            {
                return localSettings.Values["IsFirstTimeScheduler"] == null ? true : (bool)localSettings.Values["IsFirstTimeScheduler"];
            }
            set
            {
                localSettings.Values["IsFirstTimeScheduler"] = false;
            }
        }

        public bool IsLocationSet
        {
            get
            {    
                return localSettings.Values["IsLocationSet"] == null ? false : (bool) localSettings.Values["IsLocationSet"];
            }
            set
            {
                localSettings.Values["IsLocationSet"] = value;
            }
        }

        public double UserLatitude
        {
            get
            { 
                return (double) localSettings.Values["UserLatitude"];
            }
            set
            {
                localSettings.Values["UserLatitude"] = value;
            }
        }

        public double UserLongitude
        {
            get
            {
                return (double)localSettings.Values["UserLongitude"];
            }
            set
            {
                localSettings.Values["UserLongitude"] = value;
            }
        }

        public bool IsLocationEnabled
        {
            get
            {
                return localSettings.Values["IsLocationEnabled"] == null ? true : (bool) localSettings.Values["IsLocationEnabled"];
            }
            set
            {
                localSettings.Values["IsLocationEnabled"] = value;
            }
        }
    }
}
