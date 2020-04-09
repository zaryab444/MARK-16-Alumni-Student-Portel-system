using Alumni_Student_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alumni_Student_Portal.Controllers
{
    public class AlumniController : Controller
    {
        //
        // GET: /Alumni/
        Alumni_PortalEntities db = new Alumni_PortalEntities();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(tbl_Alumni uvm)
        {   
                tbl_Alumni u = new tbl_Alumni();
                u.Alumni_fullname = uvm.Alumni_fullname;
                u.Alumni_Enrolmentnum = uvm.Alumni_Enrolmentnum;
                u.Alumni_email = uvm.Alumni_email;
                u.Alumni_cellnum = uvm.Alumni_cellnum;
                u.Alumni_department = uvm.Alumni_department;
                u.Alumni_cmsid = uvm.Alumni_cmsid;
           
                db.tbl_Alumni.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");

        

         //   return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(tbl_Alumni avm)
        {
            tbl_Alumni ad = db.tbl_Alumni.Where(x => x.Alumni_email == avm.Alumni_email && x.Alumni_Enrolmentnum == avm.Alumni_Enrolmentnum).SingleOrDefault();
            if (ad != null)
            {

                Session["u_id"] = ad.Alumni_id.ToString();
                return RedirectToAction("CreateAd");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }

            return View();
        }
    
    
    public ActionResult CreateAd()
        {
            return View();
        }    
    
    }
}