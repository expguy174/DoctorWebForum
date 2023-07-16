using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorWebForum.Models
{
    public class homeViewModel
    {
        public int id { get; set; }
        public int users_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public System.DateTime created_at { get; set; }

        public string name { get; set; }
    }
}