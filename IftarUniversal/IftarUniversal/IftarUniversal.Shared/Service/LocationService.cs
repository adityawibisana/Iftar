using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace IftarUniversal.Service
{
    public class LocationService
    {
        //GeoLocator locator;
        private Geolocator locator;
        public LocationService()
        {
            locator = new Geolocator();  
        }

        public async Task<Geoposition> GetPosition() {
            return await locator.GetGeopositionAsync().AsTask();
            
        }
    }
}
