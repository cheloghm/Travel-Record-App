﻿using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelRecordApp.Model
{
    public class Post : INotifyPropertyChanged
    {
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; } // Int  type for sqlite
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string experience;

        public string Experience
        {
            get { return experience; }
            set
            {
                experience = value;
                OnPropertyChanged("Experience");
            }
        }

        private string venueName;

        public string VenueName
        {
            get { return venueName; }
            set
            {
                venueName = value;
                OnPropertyChanged("VenueName");
            }
        }

        private string categoryId;

        public string CategoryId
        {
            get { return categoryId; }
            set
            {
                categoryId = value;
                OnPropertyChanged("CategoryId");
            }
        }

        private string categoryName;

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set
            {

                address = value;
                OnPropertyChanged("Address");
            }
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        private int distance;

        public int Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                OnPropertyChanged("Distance");
            }
        }

        private string userId;

        public string UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private Venue venue;

        [JsonIgnore]
        public Venue Venue
        {
            get { return venue; }
            set 
            {
                venue = value;

                if (venue.categories != null)
                {
                    var firstCategory = venue.categories.FirstOrDefault();

                    if (firstCategory != null)
                    {
                        CategoryId = firstCategory.id;
                        CategoryName = firstCategory.name;
                    }
                }

                if (venue.location != null)
                {
                    Address = venue.location.address;
                    Distance = venue.location.distance;
                    Latitude = venue.location.lat;
                    Longitude = venue.location.lng;
                }
                
                VenueName = venue.name;
                UserId = App.user.Id;
                OnPropertyChanged("Venue");
            }
        }

        private DateTimeOffset createdate;

        public DateTimeOffset CreateDate
        {
            get { return createdate; }
            set 
            { 
                createdate = value;
                OnPropertyChanged("createdate");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public static async void Insert(Post post)
        {
            await App.postsTable.InsertAsync(post);
            await App.MobileService.SyncContext.PushAsync();
        }

        public static async Task<List<Post>> Read()
        {
            var posts = await App.postsTable.Where(p => p.UserId == App.user.Id).ToListAsync();
            await App.MobileService.SyncContext.PushAsync();
            return posts;
        }

        public static Dictionary<string, int> PostCategories(List<Post> posts)
        {
            var categories = (from p in posts
                              orderby p.CategoryId
                              select p.CategoryName).Distinct().ToList();

            Dictionary<string, int> categoriesCount = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                var count = (from post in posts
                             where post.CategoryName == category
                             select post).ToList().Count;

                //var count2 = postTable.Where(p => p.CategoryName == category).ToList().Count; //Can also be used to get the categories above

                categoriesCount.Add(category, count);
            }
            return categoriesCount;
        }

        public static async Task<bool> Update(Post post)
        {
            await App.postsTable.UpdateAsync(post);
            await App.MobileService.SyncContext.PushAsync();
            return true;
        }

        public static async Task<bool> Delete(Post post)
        {
            await App.postsTable.DeleteAsync(post);
            await App.MobileService.SyncContext.PushAsync();
            return true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
