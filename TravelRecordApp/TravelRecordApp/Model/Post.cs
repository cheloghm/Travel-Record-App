using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TravelRecordApp.Model
{
    public class Post
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Experience { get; set; }
    }
}
