using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelRecordApp.Logic;
using TravelRecordApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTravelPage : ContentPage
    {
        ILocationUpdateService loc;
        public NewTravelPage()
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
            Position p;
            loc.LocationChanged += (object sender, ILocationEvenArgs args) =>
            {
                p = new Position(args.Latitude, args.Longitude);
                var venues = VenueLogic.GetVenues(p.Latitude, p.Longitude);
            };
            //var venues = VenueLogic.GetVenues(p.Latitude, p.Longitude);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Post post = new Post()
            {
                Experience = experienceEntry.Text
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Insert(post);
                if (rows > 0)
                    DisplayAlert("Success", "Experience Successfully Inserted", "Ok");
                else
                    DisplayAlert("Failed", "Experience Not Inserted!", "Ok");
            }
            
        }
    }
}