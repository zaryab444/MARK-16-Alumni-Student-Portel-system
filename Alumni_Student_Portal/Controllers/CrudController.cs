using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Alumni_Student_Portal.Models;

namespace Alumni_Student_Portal.Controllers
{
    public class CrudController : Controller
    {
        private Alumni_PortalEntities db = new Alumni_PortalEntities();

        // GET: /Crud/
        public ActionResult Index()
        {
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
        public ActionResult Create([Bind(Include="Alumni_id,Alumni_fullname,Alumni_Enrolmentnum,Alumni_email,Alumni_cellnum,Alumni_department,Alumni_cmsid")] tbl_Alumni tbl_alumni)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Alumni.Add(tbl_alumni);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include="Alumni_id,Alumni_fullname,Alumni_Enrolmentnum,Alumni_email,Alumni_cellnum,Alumni_department,Alumni_cmsid")] tbl_Alumni tbl_alumni)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_alumni).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_alumni);
        }

        // GET: /Crud/Delete/5
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Alumni tbl_alumni = db.tbl_Alumni.Find(id);
            db.tbl_Alumni.Remove(tbl_alumni);
            db.SaveChanges();
            return RedirectToAction("Index");
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
