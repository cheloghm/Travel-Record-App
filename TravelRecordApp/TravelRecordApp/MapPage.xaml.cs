using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        //private bool hasLocationPermission = false;
        ILocationUpdateService loc;
        public MapPage()
        {
            InitializeComponent();
           // GetCurrentLocation();
            // OnAppearing();
            //GetLocation();

        }

        CancellationTokenSource cts;

        

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);
            loc = DependencyService.Get<ILocationUpdateService>();
            loc.LocationChanged += (object sender, ILocationEvenArgs args) =>
            {
                 Position p = new Position(args.Latitude, args.Longitude);
                //MoveMap(p);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromMiles(100));
                locationsMap.MoveToRegion(mapSpan);
                Console.WriteLine($"Latitude: {args.Latitude}, Longitude: {args.Longitude}, Altitude: {location.Altitude}");
                //GetLocation();
            };
            loc.GetUserLocation();
            //GetLocation();
        }

        

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
            //CrossGeolocator.Current.StopListeningAsync();
            loc = null;
        }

        //private async void GetLocation()
        //{
        //    try
        //    {
        //        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
        //        cts = new CancellationTokenSource();
        //        var location = await Geolocation.GetLocationAsync(request, cts.Token);


        //        if (location != null)
        //        {
        //            Position p = new Position(location.Latitude, location.Longitude);
        //            MoveMap(p);
        //            //MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(.444));
        //            //locationsMap.MoveToRegion(mapSpan);
        //            //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        //        }
        //    }
        //    catch (FeatureNotSupportedException fnsEx)
        //    {
        //        // Handle not supported on device exception
        //    }
        //    catch (FeatureNotEnabledException fneEx)
        //    {
        //        // Handle not enabled on device exception
        //    }
        //    catch (PermissionException pEx)
        //    {
        //        // Handle permission exception
        //    }
        //    catch (Exception ex)
        //    {
        //        // Unable to get location
        //    }
        //}

        //private void MoveMap(Position position)
        //{
        //    var center = new Position(position.Latitude, position.Longitude);
        //    var span = new MapSpan(center, 1, 1);
        //    locationsMap.MoveToRegion(span);
        //}


    }
}