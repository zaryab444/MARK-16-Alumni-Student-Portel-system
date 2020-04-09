using Alumni_Student_Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace Alumni_Student_Portal.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        Alumni_PortalEntities db = new Alumni_PortalEntities();
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
                return RedirectToAction("Event_Category");

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

	}
}