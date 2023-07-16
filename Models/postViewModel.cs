using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorWebForum.Models
{
    public class postViewModel
    {
        public int id { get; set; }
        public int users_id { get; set; }
        public int location_id { get; set; }
        public int professional_id { get; set; }
        public int experience_id { get; set; }
        public string location_name { get; set; }
        public string professional_name { get; set; }
        public string experience_name { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public int reply_count { get; set; }
        public System.DateTime created_at { get; set; }  
    }
}