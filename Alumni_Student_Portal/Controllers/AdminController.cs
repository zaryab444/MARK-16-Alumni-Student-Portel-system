using Alumni_Student_Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Data.Entity;
using Alumni_Student_Portal.Models;
using System.Net;
using System.Net.Mail;
using System.IO.Ports;
namespace Alumni_Student_Portal.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        Alumni_PortalEntities1 db = new Alumni_PortalEntities1();


        SerialPort sp = new SerialPort();

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Home(){
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tbl_Admin avm)
        {
            tbl_Admin ad = db.tbl_Admin.Where(x => x.admin_username == avm.admin_username && x.admin_password == avm.admin_password).SingleOrDefault();
            if (ad != null)
            {

                Session["ad_id"] = ad.admin_id.ToString();
                return RedirectToAction("Dashboard");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }
       
        public ActionResult Event_Category()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Event_Category(tbl_Event_Category cvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
               
            }
            else
            {
                tbl_Event_Category cat = new tbl_Event_Category();
                cat.cat_Name = cvm.cat_Name;
                cat.cat_image = path;
                cat.cat_status = 1;
                cat.cat_fk_admin = Convert.ToInt32(Session["ad_id"].ToString());
                db.tbl_Event_Category.Add(cat);
                db.SaveChanges();
                return RedirectToAction("ViewCategory");
            }
                      

            return View();
        }
        public ActionResult ViewCategory(int? page)
        {

            if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }
            // return View();

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_Event_Category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_Event_Category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);



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

                        //    ViewBag.Message = "File uploaded successfully";E:\Alumni_Student_Portal\Alumni_Student_Portal\Content\upload\
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           tbl_Event_Category tbl_cat = db.tbl_Event_Category.Find(id);
            if (tbl_cat == null)
            {
                return HttpNotFound();
            }
            return View(tbl_cat);
        }

        // POST: /Crud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        
        {
            tbl_Event_Category tbl_cat = db.tbl_Event_Category.Find(id);
            db.tbl_Event_Category.Remove(tbl_cat);
            db.SaveChanges();
            return RedirectToAction("ViewCategory");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: /Crud/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Crud/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Alumni_id,Alumni_fullname,Alumni_Enrolmentnum,Alumni_email,Alumni_cellnum,Alumni_department,Alumni_cmsid")] tbl_Alumni tbl_alumni)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Alumni.Add(tbl_alumni);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(tbl_alumni);
        }



        public ActionResult list()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View(db.tbl_Alumni.ToList());
        }

        // GET: /Crud/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Alumni tbl_alumni = db.tbl_Alumni.Find(id);
            if (tbl_alumni == null)
            {
                return HttpNotFound();
            }
            return View(tbl_alumni);
        }

     

        // GET: /Crud/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Alumni tbl_alumni = db.tbl_Alumni.Find(id);
            if (tbl_alumni == null)
            {
                return HttpNotFound();
            }
            return View(tbl_alumni);
        }

        // POST: /Crud/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Alumni_id,Alumni_fullname,Alumni_Enrolmentnum,Alumni_email,Alumni_cellnum,Alumni_department,Alumni_cmsid")] tbl_Alumni tbl_alumni)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_alumni).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("list");
            }
            return View(tbl_alumni);
        }


        public ActionResult Deleted(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Alumni tbl_alumni = db.tbl_Alumni.Find(id);
            if (tbl_alumni == null)
            {
                return HttpNotFound();
            }
            return View(tbl_alumni);
        }

        // POST: /Crud/Delete/5
        [HttpPost, ActionName("Deleted")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletedConfirmed(int id)
        {
            tbl_Alumni tbl_alumni = db.tbl_Alumni.Find(id);
            db.tbl_Alumni.Remove(tbl_alumni);
            db.SaveChanges();
            return RedirectToAction("list");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}



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
            //tbl_user u = db.tbl_user.Where(x => x.u_id == p.pro_fk_user).SingleOrDefault();
            //ad.u_name = u.u_name;
            //ad.cat_image = u.cat_image;
            //ad.u_contact = u.u_contact;
            //ad.pro_fk_user = u.u_id;




            return View(ad);
        }

        public ActionResult Del(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Ad_Post tbl_pro = db.Ad_Post.Find(id);
            if (tbl_pro == null)
            {
                return HttpNotFound();
            }
            return View(tbl_pro);
        }

        [HttpPost, ActionName("Del")]
        [ValidateAntiForgeryToken]
        public ActionResult Delconfirmed(int id)
        {
            Ad_Post tbl_pro = db.Ad_Post.Find(id);
            db.Ad_Post.Remove(tbl_pro);
            db.SaveChanges();
            return RedirectToAction("ViewCategory");
        }

      //Get Email
        public ActionResult Email()
        {

            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Email(Alumni_Student_Portal.Models.gmail model)
        {
            MailMessage mm = new MailMessage("sohail.zaryab61@gmail.com", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            NetworkCredential nc = new NetworkCredential("sohail.zaryab61@gmail.com","zaryabashina");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Send(mm);
            ViewBag.Message = "Mail has been sent sucessfully";
            


            return View();
        }
            }
}