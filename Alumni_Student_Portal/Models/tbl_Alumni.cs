//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alumni_Student_Portal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_Alumni
    {
        public tbl_Alumni()
        {
            this.Ad_Post = new HashSet<Ad_Post>();
            this.Complains = new HashSet<Complain>();
            this.CVs = new HashSet<CV>();
            this.goings = new HashSet<going>();
        }
    
        public int Alumni_id { get; set; }
         [Required(ErrorMessage = "This Feild is required")]
         [StringLength(20, MinimumLength = 10)]
         [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$",ErrorMessage="Please Enter upper and lower case alphabet only")]
        public string Alumni_fullname { get; set; }

         [Required(ErrorMessage = "This Feild is required")]
         [StringLength(10, ErrorMessage = "The {0} must be  {2} characters long.", MinimumLength = 6)]
   public string Alumni_Enrolmentnum { get; set; }
        
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
       [Required(ErrorMessage = "This Feild is required")]
        public string Alumni_email { get; set; }


        //[Display(Name = "Mobile Number:")]
        //[Required(ErrorMessage = "Mobile Number is required.")]
        //[RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
         public string Alumni_cellnum { get; set; }
        
        
        [Required(ErrorMessage = "This Feild is required")]
        public string Alumni_department { get; set; }
        
        
        [Required(ErrorMessage = "This Feild is required")]
        public string Alumni_cmsid { get; set; }
    
        public virtual ICollection<Ad_Post> Ad_Post { get; set; }
        public virtual ICollection<Complain> Complains { get; set; }
        public virtual ICollection<CV> CVs { get; set; }
        public virtual ICollection<going> goings { get; set; }
    }
}
