﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;
using TravelRecordApp.Model;
using TravelRecordApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        HistoryVM viewModel;
        public HistoryPage()
        {
            InitializeComponent();

            viewModel = new HistoryVM();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            #region Slite local database gets all data regardless of user
            //Using statment helps to close connection to database as soon as the block of code is run
            /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();
                postListView.ItemsSource = posts;
            }*/
            #endregion

            //await viewModel.UpdatePosts();
            var posts = await Post.Read();
            postListView.ItemsSource = posts;

            await AzureAppServiceHelper.SyncAsync();
        }

        private async void postListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedPost = postListView.SelectedItem as Post;
            if (selectedPost != null)
            {
                await AzureAppServiceHelper.SyncAsync();
                await Navigation.PushAsync(new PostDetailsPage(selectedPost));
            }
        }
    }
}