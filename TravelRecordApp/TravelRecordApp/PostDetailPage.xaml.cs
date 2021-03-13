using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostDetailsPage : ContentPage
    {
        Post selectedPost;
        public PostDetailsPage()
        {

        }
        public PostDetailsPage(Post selectedPost)
        {
            InitializeComponent();
            this.selectedPost = selectedPost;
            experienceEntry.Text = selectedPost.Experience;
            venueLabel.Text = selectedPost.VenueName;
            categoryLabel.Text = selectedPost.CategoryName;
            addressLabel.Text = selectedPost.Address;
            coordinatesLabel.Text = $"{selectedPost.Latitude}, {selectedPost.Longitude}";
            distanceLabel.Text = $"{selectedPost.Distance} m";
        }

        private async void updateButton_Clicked(object sender, EventArgs e)
        {
            selectedPost.Experience = experienceEntry.Text;
            #region Code for updating in sqlite local db
            //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //{
            //    conn.CreateTable<Post>();
            //    int rows = conn.Update(selectedPost);
            //    if (rows > 0)
            //    {
            //        DisplayAlert("Success", "Experience Successfully Updated", "Ok");
            //        Navigation.PushAsync(new HomePage());
            //    }
            //    else
            //        DisplayAlert("Failed", "Experience Not Updated!", "Ok");
            //}
            #endregion
            await App.MobileService.GetTable<Post>().UpdateAsync(selectedPost);
            await DisplayAlert("Success", "Experience Successfully Updated", "Ok");
            await Navigation.PushAsync(new HomePage());
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            #region Codefor deleting in sqlite local db
            //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //{
            //    conn.CreateTable<Post>();
            //    int rows = conn.Delete(selectedPost);
            //    if (rows > 0) 
            //    {
            //        DisplayAlert("Success", "Experience Successfully Deleted", "Ok");
            //        Navigation.PushAsync(new HomePage());
            //    }
            //    else
            //        DisplayAlert("Failed", "Experience Not Deleted!", "Ok");
            //}
            #endregion
            await App.MobileService.GetTable<Post>().DeleteAsync(selectedPost);
            await DisplayAlert("Success", "Experience Successfully Deleted", "Ok");
            await Navigation.PushAsync(new HomePage());
        }
    }
}