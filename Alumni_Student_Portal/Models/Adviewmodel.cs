using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alumni_Student_Portal.Models
{
    public class Adviewmodel
    {
        public int Ad_id { get; set; }
        public string Ad_name { get; set; }
        public string Ad_image { get; set; }
        public string Ad_des { get; set; }
        public Nullable<int> pro_fk_Event_Category { get; set; }
        public Nullable<int> pro_fk_Alumni { get; set; }


        public int cat_id { get; set; }
        public string cat_Name { get; set; }
        public string cat_image { get; set; }
    }
}