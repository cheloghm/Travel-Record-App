using CoreLocation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelRecordApp.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationUpdateService))]
namespace TravelRecordApp.iOS
{
    public class LocationEventArgs : ILocationEvenArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    class LocationUpdateService : ILocationUpdateService
    {

        CLLocationManager LocationManager;

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
            LocationManager = new CLLocationManager()
            {
                DesiredAccuracy = CLLocation.AccuracyBest,
                DistanceFilter = CLLocationDistance.FilterNone
            };

            LocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
            {
                var locations = e.Locations;
                LocationEventArgs args = new LocationEventArgs
                {
                    Latitude = locations[locations.Length - 1].Coordinate.Latitude,
                    Longitude = locations[locations.Length - 1].Coordinate.Longitude
                };

                LocationChanged(this, args);

            };

            LocationManager.AuthorizationChanged += (object sender, CLAuthorizationChangedEventArgs e) =>
            {
                if (e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                {
                    LocationManager.StartUpdatingLocation();
                }
            };

            LocationManager.RequestWhenInUseAuthorization();

        }

        ~LocationUpdateService()
        {
            LocationManager.StopUpdatingLocation();
        }
    }
}