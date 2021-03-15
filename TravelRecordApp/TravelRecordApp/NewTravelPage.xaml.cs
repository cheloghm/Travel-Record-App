using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using TravelRecordApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTravelPage : ContentPage
    {
        NewTravelVM viewModel;
        public NewTravelPage()
        {
            InitializeComponent();
            viewModel = new NewTravelVM();
            BindingContext = viewModel;
        }

        CancellationTokenSource cts;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);


            if (location != null)
            {
                var venues = await Venue.GetVenues(location.Latitude, location.Longitude);
                venueListView.ItemsSource = venues;
            }
        }
    }
}