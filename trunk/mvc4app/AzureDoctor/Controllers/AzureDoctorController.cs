using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureDoctor.Models;
// Added to bring CultureInfo into scope.
using System.Globalization;

namespace AzureDoctor.Controllers
{
    public class AzureDoctorController : Controller
    {
        private AzureDoctorEntities db = new AzureDoctorEntities();

        public ActionResult TestService(int id)
        {
            CloudService cs = db.CloudServices.Single(s => s.UserID == id);
            HealthModel hm = new HealthModel();
            hm.MakeHealthRequest(cs);

            ViewBag.ServiceName = hm.ServiceName;
            ViewBag.StatusList = hm.StatusList;
            ViewBag.InstanceCount = hm.InstanceCount;
            ViewBag.DeploymentSlot = hm.DeploymentSlot;
            ViewBag.InstanceSize = hm.InstanceSize;
            ViewBag.InstanceStatuses = hm.InstanceStatuses;
            ViewBag.VIPAddress = hm.VIPAddress;
            ViewBag.Location = hm.Location;
            ViewBag.AllInstancesHealthy = hm.AllInstancesHealthy;
            // Hard coding date time format for now. Should really have client send its culture info to the server.
            DateTime time = DateTime.Now;             
	        string formatDateTime = "ddd d MMMM yyyy H:mm:ss";
            ViewBag.Time = time.ToString(formatDateTime);

            if (Request.IsAjaxRequest())
            {
                return PartialView("ResultPartial");
            }
            else
            {
                // Allows for graceful degradation if JavaScript is not present.
                return View("Index");
            }
        }

        // GET: /AzureDoctor/
        public ActionResult Index()
        {
            var cloudservices = db.CloudServices.Include("AzureDoctorUser");
            return View(cloudservices.ToList());
        }


        // GET: /AzureDoctor/Details/5
        public ActionResult Details(int id = 0)
        {
            CloudService cloudservice = db.CloudServices.Single(c => c.CloudServiceID == id);
            if (cloudservice == null)
            {
                return HttpNotFound();
            }
            return View(cloudservice);
        }


        // GET: /AzureDoctor/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AzureDoctorUsers, "UserID", "FirstName");
            return View();
        }


        // POST: /AzureDoctor/Create
        [HttpPost]
        public ActionResult Create(CloudService cloudservice)
        {
            if (ModelState.IsValid)
            {
                db.CloudServices.AddObject(cloudservice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AzureDoctorUsers, "UserID", "FirstName", cloudservice.UserID);
            return View(cloudservice);
        }


        // GET: /AzureDoctor/Edit/5
        public ActionResult Edit(int id = 0)
        {
            CloudService cloudservice = db.CloudServices.Single(c => c.CloudServiceID == id);
            if (cloudservice == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AzureDoctorUsers, "UserID", "FirstName", cloudservice.UserID);
            return View(cloudservice);
        }


        // POST: /AzureDoctor/Edit/5
        [HttpPost]
        public ActionResult Edit(CloudService cloudservice)
        {
            if (ModelState.IsValid)
            {
                db.CloudServices.Attach(cloudservice);
                db.ObjectStateManager.ChangeObjectState(cloudservice, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AzureDoctorUsers, "UserID", "FirstName", cloudservice.UserID);
            return View(cloudservice);
        }


        // GET: /AzureDoctor/Delete/5
        public ActionResult Delete(int id = 0)
        {
            CloudService cloudservice = db.CloudServices.Single(c => c.CloudServiceID == id);
            if (cloudservice == null)
            {
                return HttpNotFound();
            }
            return View(cloudservice);
        }


        // POST: /AzureDoctor/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CloudService cloudservice = db.CloudServices.Single(c => c.CloudServiceID == id);
            db.CloudServices.DeleteObject(cloudservice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}