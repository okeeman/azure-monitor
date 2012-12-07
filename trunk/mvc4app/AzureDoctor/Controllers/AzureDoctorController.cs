using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureDoctor.Models;
// Added to bring HttpWebRequest into scope.
using System.Net;
// Added to bring SecurityException into scope.
using System.Security;
// Added to bring CryptographicExcdeption into scope.
using System.Security.Cryptography;
// Added to bring X509Store & OpenFlags into scope.
using System.Security.Cryptography.X509Certificates;
// Added to bring XmlTextReader into scope.
using System.Xml;
// Added to bring XDocument & XNamespace into scope.
using System.Xml.Linq;
//using System.Xml.Serialization;
// Added to bring Stream into scope.
using System.IO;
// Added to bring RoleEnvironment into scope.
using Microsoft.WindowsAzure.ServiceRuntime;
// Added to bring UTF8Encoding & Decoder into scope.
using System.Text;


namespace AzureDoctor.Controllers
{
    public class AzureDoctorController : Controller
    {
        private AzureDocEntities db = new AzureDocEntities();

        public ActionResult TestService(int id)
        {
            // Resource release after use?
            CloudService cs = db.CloudServices.Single(s => s.UserID == id);
            LogicClass c = new LogicClass();
            string serviceName;
            List<string> statusList;
            int instanceCount;
            c.MakeHealthRequest(cs, out serviceName, out statusList, out instanceCount);

            ViewBag.ServiceName = serviceName;
            ViewBag.StatusList = statusList;
            ViewBag.InstanceCount = instanceCount;
            //ViewBag.DeploymentSlot = deploymentSlot;
            //ViewBag.InstanceSize = instanceSize;
            //ViewBag.InstanceStatuses = instanceStatuses;
            //ViewBag.InstanceStateDetails = instanceStateDetails;
            //ViewBag.InstanceErrorCodes = instanceErrorCodes;
            //ViewBag.InstanceFaultDomains = instanceFaultDomains;
            //ViewBag.SDKVersion = SDKVersion;
            //ViewBag.VIPAddress = VIPAddress;
            ViewBag.Time = DateTime.Now.ToLocalTime();

            if (Request.IsAjaxRequest())
            {
                return PartialView("TestService");
            }
            else
            {
                // Allows for graceful degradation if JavaScript is not present.
                return View("Index");
            }
        }

        //
        // GET: /AzureDoctor/

        public ActionResult Index()
        {
            var cloudservices = db.CloudServices.Include("AzureDoctorUser");
            return View(cloudservices.ToList());
        }

        //
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

        //
        // GET: /AzureDoctor/Create

        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AzureDoctorUsers, "UserID", "FirstName");
            return View();
        }

        //
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

        //
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

        //
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

        //
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

        //
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