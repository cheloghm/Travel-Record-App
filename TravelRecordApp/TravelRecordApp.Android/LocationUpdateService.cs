using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelRecordApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationUpdateService))]
namespace TravelRecordApp.Droid
{
    public class LocationEventArgs : ILocationEvenArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class LocationUpdateService : Java.Lang.Object, ILocationUpdateService, ILocationListener
    {
        LocationManager locationManager;
        public event EventHandler<ILocationEvenArgs> LocationChanged;

        event EventHandler<ILocationEvenArgs> ILocationUpdateService.LocationChanged
        {
            add
            {
                LocationChanged += value;
            }
            remove
            {
                LocationChanged -= value;
            }
        }

        public void GetUserLocation()
        {
            locationManager = (LocationManager)MainActivity.context.GetSystemService(Context.LocationService);
            // For this example, this method is part of a class that implements ILocationListener, described below
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
        
        }
        ~LocationUpdateService()
        {
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };

                LocationChanged(this, args);
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}