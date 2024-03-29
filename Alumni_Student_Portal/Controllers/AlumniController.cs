﻿using Alumni_Student_Portal.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Alumni_Student_Portal.Controllers
{
    public class AlumniController : Controller
    {
        //
        // GET: /Alumni/
       // Alumni_PortalEntities1 db = new Alumni_PortalEntities1();
        Alumni_PortalEntities1 db = new Alumni_PortalEntities1();
        public ActionResult Home()
        {
            return View();
        }
        
        public ActionResult Index(int? page)
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
            db.SaveChanges(); //adding record in database
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



            if (Session["u_id"] == null)
            {
                return RedirectToAction("Login");
            }

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

        public ActionResult Ads(int? id, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Ad_Post.Where(x => x.pro_fk_Event_Category == id).OrderByDescending(x => x.Ad_id).ToList();
            IPagedList<Ad_Post> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


        }
        public ActionResult ViewAd(int? id)
        {
         

            Adviewmodel ad = new Adviewmodel();
            Ad_Post p = db.Ad_Post.Where(x => x.Ad_id == id).SingleOrDefault();
            ad.Ad_id = p.Ad_id;
            ad.Ad_name = p.Ad_name;
            ad.Ad_image = p.Ad_image;
            ad.Ad_des = p.Ad_des;
            tbl_Event_Category cat = db.tbl_Event_Category.Where(x => x.cat_id == p.pro_fk_Event_Category).SingleOrDefault();
            ad.cat_Name = cat.cat_Name;

          
            
            return View(ad);


            //tbl_user u = db.tbl_user.Where(x => x.u_id == p.pro_fk_user).SingleOrDefault();
            //ad.u_name = u.u_name;
            //ad.cat_image = u.cat_image;
            //ad.u_contact = u.u_contact;
            //ad.pro_fk_user = u.u_id;
           }

        [HttpPost]
        public ActionResult ViewAd(going uvm)
        {
            going u = new going();
            u.go_name = uvm.go_name;
            u.go_email = uvm.go_email;
            u.go_contact = uvm.go_contact;
            u.pro_fk_Alumni = uvm.pro_fk_Alumni;


            db.goings.Add(u);
            db.SaveChanges();
            return RedirectToAction("ViewAd");
         }

        
        public ActionResult SubmitCV()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitCV(CV C, HttpPostedFileBase file)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);//com   
            C.cv1 = (Path.Combine(Server.MapPath("~/uploadcv"), fileName));//com
            string path = uploadfile(file);
            if (path.Equals("-1"))
            {
                ViewBag.error = "file could not be uploaded....";
            }
            else
            {

                CV u = new CV();
                u.Cid = C.Cid;
                u.fullname = C.fullname;
                u.Email = C.Email;
                u.pro_fk_Alumni = u.pro_fk_Alumni;
                u.cv1 = path;

                db.CVs.Add(u);//com
                //db.SaveChanges();//com
                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                C.cv1 = (Path.Combine(Server.MapPath("~/uploadcv"), filename));
                C.cv1 = Path.Combine(Server.MapPath("~/uploadcv"), filename);

                using (Alumni_PortalEntities1 dc = new Alumni_PortalEntities1())
                {
                    C.cv1 = filename;
                    dc.CVs.Add(C);
                    dc.SaveChanges(); //com







                }//

            }
            return View();
        }
        public string uploadfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".pdf") || extension.ToLower().Equals(".docs"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/uploadcv"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/uploadcv" + random + Path.GetFileName(file.FileName);

                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                        ViewBag.Message = "Not work";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only pdf formats are acceptable....'); </script>");
                }



            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }




            return path;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Query()
        {
            return View();
        }

        [HttpPost]
        //   [ValidateAntiForgeryToken]
        public ActionResult Query(Complain C, HttpPostedFileBase file)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);//com   
            C.com = (Path.Combine(Server.MapPath("~/Complain"), fileName));//com
            string path = uploadfile(file);
            if (path.Equals("-1"))
            {
                ViewBag.error = "file could not be uploaded....";
            }
            else
            {

                Complain u = new Complain();
                u.ComplainID = C.ComplainID;
                u.Name = C.Name;
                u.Emain = C.Emain;
                u.pro_fk_Alumni = u.pro_fk_Alumni;
                u.com = path;

                db.Complains.Add(u);//com
                //db.SaveChanges();//com
                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                C.com = (Path.Combine(Server.MapPath("~/Complain"), filename));
                C.com = Path.Combine(Server.MapPath("~/Complain"), filename);

                using (Alumni_PortalEntities1 dc = new Alumni_PortalEntities1())
                {
                    C.com = filename;
                    dc.Complains.Add(C);
                    dc.SaveChanges(); //com







                }//

            }
            return View();
        }


        public string files(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".pdf") || extension.ToLower().Equals(".docs"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Complain"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Complain" + random + Path.GetFileName(file.FileName);

                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }

                else
                {

                    Response.Write("<script>alert('Only pdf formats are acceptable....'); </script>");



                }


            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }



        public ActionResult going()
        {
            return View();
        }

        [HttpPost]
        public ActionResult going(going uvm)
        {

            going u = new going();
            u.go_name = uvm.go_name;
            u.go_email = uvm.go_email;
            u.go_contact = uvm.go_contact;
            u.pro_fk_Alumni = uvm.pro_fk_Alumni;


            db.goings.Add(u);
            db.SaveChanges();
            return RedirectToAction("going");


            //return View();
        }


    }
}