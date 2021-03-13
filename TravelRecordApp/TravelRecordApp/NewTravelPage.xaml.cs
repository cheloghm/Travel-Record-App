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
        Position p;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);


            if (location != null)
            {
                var venues = await VenueLogic.GetVenues(location.Latitude, location.Longitude);
                venueListView.ItemsSource = venues;
            }

        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedVenue = venueListView.SelectedItem as Venue;
                var firstCategory = selectedVenue.categories.FirstOrDefault();
                Post post = new Post()
                {
                    Experience = experienceEntry.Text,
                    CategoryId = firstCategory.id,
                    CategoryName = firstCategory.name,
                    Address = selectedVenue.location.address,
                    Distance = selectedVenue.location.distance,
                    Latitude = selectedVenue.location.lat,
                    Longitude = selectedVenue.location.lng,
                    VenueName = selectedVenue.name,
                    UserId = App.user.Id
                };

                #region Inserting to local Sqlite Db
                //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                //{
                //    conn.CreateTable<Post>();
                //    int rows = conn.Insert(post);
                //    if (rows > 0)
                //    { 
                //        DisplayAlert("Success", "Experience Successfully Inserted", "Ok");
                //        Navigation.PushAsync(new HomePage());
                //    }
                //    else
                //        DisplayAlert("Failed", "Experience Not Inserted!", "Ok");
                //}
                #endregion
                await App.MobileService.GetTable<Post>().InsertAsync(post);
                await DisplayAlert("Success", "Experience Successfully Inserted", "Ok");
            }
            catch (NullReferenceException nre)
            {
                await DisplayAlert("Failed", "Experience Not Inserted!", "Ok");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                await DisplayAlert("Failed", "Experience Not Inserted!", "Ok");
            }
            
        }
    }
}