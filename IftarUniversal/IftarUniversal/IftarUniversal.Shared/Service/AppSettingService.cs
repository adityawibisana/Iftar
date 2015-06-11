using System;
using System.Collections.Generic;
using System.Text;

namespace IftarUniversal.Service
{
    public class AppSettingService
    {
        Windows.Storage.ApplicationDataContainer localSettings;

        public AppSettingService()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        public Double UserLatitude
        {
            get
            {
                return (double) localSettings.Values["latitude"];
            }
            set
            {
                localSettings.Values["latitude"] = value;
            }
        }

        public Double UserLongitude
        {
            get
            {
                return (double)localSettings.Values["longitude"];
            }
            set
            {
                localSettings.Values["longitude"] = value;
            }
        }
    }
}
