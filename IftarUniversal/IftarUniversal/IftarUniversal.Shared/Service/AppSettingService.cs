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

        public bool IsLocationSet
        {
            get
            { 
                return localSettings.Values["IsLocationSet"] == null ? false : true;
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
    }
}
