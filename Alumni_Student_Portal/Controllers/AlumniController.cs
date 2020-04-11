using Alumni_Student_Portal.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult Index(int ? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_Event_Category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_Event_Category> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
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
        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }

    
    [HttpGet]
    public ActionResult CreateAd()
        {
            List<tbl_Event_Category> li = db.tbl_Event_Category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            return View();
        }

[HttpPost]
    public ActionResult CreateAd(Ad_Post pvm, HttpPostedFileBase imgfile)
    {
        List<tbl_Event_Category> li = db.tbl_Event_Category.ToList();
        ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");


        string path = uploadimgfile(imgfile);
        if (path.Equals("-1"))
        {
            ViewBag.error = "Image could not be uploaded....";
        }
        else
        {
            Ad_Post p = new Ad_Post();
            p.Ad_name = pvm.Ad_name;
            p.Ad_des = pvm.Ad_des;
            p.Ad_image = path;
            p.pro_fk_Event_Category = pvm.pro_fk_Event_Category;
            // p.pro_fk_user = Convert.ToInt32(Session["u_id"].ToString());  // isko null krna ha
            db.Ad_Post.Add(p);
            db.SaveChanges();
            Response.Redirect("Index");

        }

        return View();
    }
    
    }
}