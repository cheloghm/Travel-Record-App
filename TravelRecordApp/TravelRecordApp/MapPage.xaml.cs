using Plugin.Geolocator;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        ILocationUpdateService loc;
        public MapPage()
        {
            InitializeComponent();

        }

        CancellationTokenSource cts;

        

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);
            loc = DependencyService.Get<ILocationUpdateService>();
            loc.LocationChanged += async (object sender, ILocationEvenArgs args) =>
            {
                Position p = new Position(args.Latitude, args.Longitude);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromMeters(1000));
                //MapSpan mapSpan = new MapSpan(p, 2, 2);
                locationsMap.MoveToRegion(mapSpan);
                Console.WriteLine($"Latitude: {args.Latitude}, Longitude: {args.Longitude}, Altitude: {location.Altitude}");

                #region For reading from sql
                //Using statment helps to close connection to database as soon as the block of code is run
                /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Post>();
                    var posts = conn.Table<Post>().ToList();

                    //Method for displaying pins on map
                    DisplayInMap(posts);
                }*/
                #endregion

                var posts = await Post.Read();
                DisplayInMap(posts);

            };
            loc.GetUserLocation();
        }

        private void DisplayInMap(List<Post> posts)
        {
            foreach (var post in posts)
            {
                try
                {
                    var position = new Position(post.Latitude, post.Longitude);
                    var pin = new Pin()
                    {
                        Type = PinType.SavedPin,
                        Position = position,
                        Label = post.VenueName,
                        Address = post.Address
                    };

                    //Placing pin on map
                    locationsMap.Pins.Add(pin);
                }
                catch (NullReferenceException nre)
                {

                }
                catch (Exception ex)
                {

                }
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
            loc = null;
        }
    }
}