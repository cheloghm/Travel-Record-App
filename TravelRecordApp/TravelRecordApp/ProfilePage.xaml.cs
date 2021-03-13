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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

           // using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //{
                var postTable = await App.MobileService.GetTable<Post>().Where(p => p.UserId == App.user.Id).ToListAsync();

            var categories = (from p in postTable
                                  orderby p.CategoryId
                                  select p.CategoryName).Distinct().ToList();

                Dictionary<string, int> categoriesCount = new Dictionary<string, int>();
                
                foreach (var category in categories)
                {
                    var count = (from post in postTable
                                 where post.CategoryName == category
                                 select post).ToList().Count;

                    //var count2 = postTable.Where(p => p.CategoryName == category).ToList().Count; //Can also be used to get the categories above

                    categoriesCount.Add(category, count);
                }

                categoryListView.ItemsSource = categoriesCount;

                postCountLabel.Text = postTable.Count.ToString();
            //}
        }
    }
}