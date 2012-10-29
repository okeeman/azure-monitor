using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Added for HttpWebRequest.
using System.Net;
// Added for SecurityException.
using System.Security;
// Added for CryptographicExcdeption.
using System.Security.Cryptography;
// Added for X509Store & OpenFlags.
using System.Security.Cryptography.X509Certificates;
// Added for XmlTextReader.
using System.Xml;
//using System.Xml.Linq;
//using System.Xml.Serialization;
// Added for Stream.
using System.IO;

namespace AzureDoctor.Controllers
{
    public class AzureDoctorController : Controller
    {
        static int userCount = 1000;

        private AzureDoctorEntities db = new AzureDoctorEntities();

        //
        // GET: /AzureDoctor/

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /AzureDoctor/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Single(u => u.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /AzureDoctor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AzureDoctor/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.ID = ++userCount;
                db.Users.AddObject(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /AzureDoctor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Single(u => u.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /AzureDoctor/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /AzureDoctor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Single(u => u.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /AzureDoctor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Single(u => u.ID == id);
            db.Users.DeleteObject(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TestService(int id)
        {
            User user = db.Users.Single(u => u.ID == id);
            Uri getDetailsRequest = new Uri("https://management.core.windows.net/" + user.SubscriptionID + "/services/hostedservices/" + user.ServiceName + "?embed-detail=true");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getDetailsRequest);
            request.Headers.Add("x-ms-version", "2012-03-01");
            request.Method = "GET";
            request.ContentType = "application/xml";

            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                certStore.Open(OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                if (e is CryptographicException)
                {
                    Console.WriteLine("Error: The store is unreadable.");
                }
                else if (e is SecurityException)
                {
                    Console.WriteLine("Error: You don't have the required permission.");
                }
                else if (e is ArgumentException)
                {
                    Console.WriteLine("Error: Invalid values in the store.");
                }
                else
                {
                    throw;
                }
            }

            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, user.Thumbprint, false);
            if (certCollection.Count == 0)
            {
                throw new Exception("Error: No certificate found containing thumbprint " + user.Thumbprint);
            }
            certStore.Close();

            // There is only 1 cert in this collection, i.e. the one with the specified thumbprint.
            X509Certificate2 certificate = certCollection[0];
            // Add the certificate to the request.
            request.ClientCertificates.Add(certificate);

            // Make the request to the Uri.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            XmlTextReader reader = new XmlTextReader(responseStream);

            string currentServiceStatus = null, instanceStatus = null, deploymentSlot = null, instanceSize = null, instanceErrorCode = null, SDKVersion = null;

            while (reader.Read())
            {
                if (reader.Name == "Status")
                {
                    reader.Read();
                    currentServiceStatus = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "InstanceStatus")
                {
                    reader.Read();
                    instanceStatus = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "DeploymentSlot")
                {
                    reader.Read();
                    deploymentSlot = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "InstanceSize")
                {
                    reader.Read();
                    instanceSize = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "InstanceErrorCode")
                {
                    reader.Read();
                    instanceErrorCode = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "SdkVersion")
                {
                    reader.Read();
                    SDKVersion = reader.Value;
                    reader.Read();
                }
            }

            ViewBag.currentServiceStatus = currentServiceStatus;
            ViewBag.instanceStatus = instanceStatus;
            ViewBag.deploymentSlot = deploymentSlot;
            ViewBag.instanceSize = instanceSize;
            ViewBag.instanceErrorCode = instanceErrorCode;
            ViewBag.SDKVersion = SDKVersion;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}