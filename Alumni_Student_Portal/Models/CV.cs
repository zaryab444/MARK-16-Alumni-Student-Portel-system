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
    
    public partial class CV
    {
        public int Cid { get; set; }
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid Name")]
        [Required(ErrorMessage = "Please enter  Name.")]
        public string fullname { get; set; }
         [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
         [Required(ErrorMessage = "Please enter  Email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter  CV.")]
        public string cv1 { get; set; }
        public Nullable<int> pro_fk_Student { get; set; }
        public Nullable<int> pro_fk_Alumni { get; set; }
    
        public virtual tbl_Alumni tbl_Alumni { get; set; }
        public virtual student student { get; set; }
    }
}
