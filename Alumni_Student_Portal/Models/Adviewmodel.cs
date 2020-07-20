using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int go_id { get; set; }

        [Required(ErrorMessage = "This Feild is required")]
        [StringLength(20, MinimumLength = 10)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter upper and lower case alphabet only")]

        public string go_name { get; set; }


        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Required(ErrorMessage = "This Feild is required")]

        public string go_email { get; set; }

        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        public string go_contact { get; set; }
        public Nullable<int> pro_fk_Student { get; set; }
    //    public Nullable<int> pro_fk_Alumni { get; set; }

        public virtual tbl_Alumni tbl_Alumni { get; set; }
        public virtual student student { get; set; }
    }

    
    
}