using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureDoctor.Models;
// Added to bring Membership into scope.
using System.Web.Security;
// Added to bring InitializeSimpleMembership into scope.
using AzureDoctor.Filters;

namespace AzureDoctor.Controllers
{
    [HandleError]
    public class AzureDoctorController : Controller
    {
        private AzureDoctorEntities db = new AzureDoctorEntities();

        public ActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult Overview()
        {
            int loggedInUserID = (int)Membership.GetUser().ProviderUserKey;
            SubAndServiceViewModel subcs = new SubAndServiceViewModel();
            subcs.sub = db.Subscriptions.FirstOrDefault(s => s.UserId == loggedInUserID && s.Status == "Active");

            if (subcs.sub != null)
            {
                var getCloudServices = db.CloudServices.Where(cs => cs.SubscriptionRecordID == subcs.sub.SubscriptionRecordID && cs.Status == "Active");

                foreach (var cloudService in getCloudServices)
                {
                    subcs.cloudServices.Add(cloudService);
                }

                //ViewBag.UserHasActiveSubscription = true;
                return View("Overview", subcs);
            }
            else
            {
                //ViewBag.UserHasActiveSubscription = false;
                return View("Overview");
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult GetHealthStatus()
        {
            //RequestResult rr = new RequestResult();
            Certificate cert = db.Certificates.First();

            int loggedInUserID = (int)Membership.GetUser().ProviderUserKey;
            Subscription sub = db.Subscriptions.FirstOrDefault(s => s.UserId == loggedInUserID && s.Status == "Active");

            // Put CloudServices into local memory as a List before making the health requests. 
            List<CloudService> cloudServiceCollection = new List<CloudService>();

            foreach (var service in db.CloudServices.Where(s => s.SubscriptionRecordID == sub.SubscriptionRecordID))
            {
                if (service.Status == "Active")
                {
                    cloudServiceCollection.Add(service);
                }
            }

            DateTime? requestTime = DateTime.Now;
            List<RequestResult> resultCollection = new List<RequestResult>();
            // AllInstancesHealthy is per cloud service. This bool checks all cloud services.
            bool overallAllInstancesHealthy = true;

            foreach (var service in cloudServiceCollection)
            {
                RequestResult rr = new RequestResult();
                rr.MakeHealthRequest(sub, service, cert);
                rr.CloudServiceID = service.CloudServiceID;
                rr.DateTime = requestTime;
                resultCollection.Add(rr);

                if (rr.AllInstancesHealthy == false)
                {
                    overallAllInstancesHealthy = false;
                }
            }

            ViewBag.Subscription = sub;
            ViewBag.CloudServiceCollection = cloudServiceCollection;
            ViewBag.ResultCollection = resultCollection;
            ViewBag.OverallAllInstancesHealthy = overallAllInstancesHealthy;

            if (Request.IsAjaxRequest())
            {
                return PartialView("ResultPartial");
            }
            else
            {
                return View("GetHealthStatus");
            }
        }

        private bool UserHasActiveSubscription(int userId)
        {
            if (db.Subscriptions.Count(s => (s.UserId == userId) && (s.Status == "Active")) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SubscriptionHasCloudServices(Subscription sub)
        {
            if (db.CloudServices.Count(cs => (cs.SubscriptionRecordID == sub.SubscriptionRecordID) && (cs.Status == "Active")) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Authorize]
        public ActionResult AddSubscription()
        {
            int loggedInUserID = (int)Membership.GetUser().ProviderUserKey;
            Subscription sub = new Subscription();

            if (UserHasActiveSubscription(loggedInUserID))
            {
                // UI does not provide this option but it could be accessed by entering the route in the browser address bar.
                ViewBag.ErrorMessage = "You may have only one active Windows Azure subscription per Azure Doctor account. Delete old subscription before adding another.";
                return View("Overview");
            }
            else
            {
                return View("AddSubscription", sub);
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult AddSubscription(Subscription sub)
        {
            int loggedInUserID = (int)Membership.GetUser().ProviderUserKey;
            sub.UserId = loggedInUserID;
            sub.Status = "Active";

            if (ModelState.IsValid)
            {
                db.Subscriptions.AddObject(sub);
                db.SaveChanges();
                return RedirectToAction("Overview");
            }
            else
            {
                return View("AddSubscription", sub);
            }            
        }

        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult EditSubscription(int id)
        {
            Subscription sub = db.Subscriptions.FirstOrDefault(s => s.SubscriptionRecordID == id && s.Status == "Active");

            if (sub == null)
            {
                // UI does not provide this option but it could be accessed by entering the route in the browser address bar.
                ViewBag.ErrorMessage = "You must set up a subscription before attempting to edit it";
                return View("Overview");
            }
            else
            {
                return View("EditSubscription", sub);
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult EditSubscription(Subscription sub)
        {
            if (ModelState.IsValid)
            {
                db.Subscriptions.Attach(sub);
                db.ObjectStateManager.ChangeObjectState(sub, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Overview");
            }
            else
            {
                return View("EditSubscription", sub);
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult DeleteSubscription(int id)
        {
            using (db)
            {
                Subscription sub = db.Subscriptions.FirstOrDefault(s => s.SubscriptionRecordID == id);
               
                if (SubscriptionHasCloudServices(sub))
                {
                    ViewBag.UserHasActiveSubscription = true;
                    ViewBag.ErrorMessage = "You cannot delete a subscription which has cloud services associated with it. Delete the cloud services first.";
                    return View("Overview");
                }
                else
                {
                    return View("DeleteSubscription", sub);
                }
            }
        }
       
        [Authorize]
        [InitializeSimpleMembership]
        [HttpPost, ActionName("DeleteSubscription")]
        public ActionResult DeleteSubscriptionConfirmed(int id)
        {
            using (db)
            {
                Subscription sub = db.Subscriptions.FirstOrDefault(s => s.SubscriptionRecordID == id);
                sub.Status = "Inactive";
                db.SaveChanges();
                return RedirectToAction("Overview");
            }
        }

        [Authorize]
        public ActionResult AddCloudService()
        {
            int loggedInUserID = (int)Membership.GetUser().ProviderUserKey;

            if (!UserHasActiveSubscription(loggedInUserID))
            {
                ViewBag.ErrorMessage = "You must set up a subscription before adding cloud services.";
                return View("Overview");
            }
            else
            {
                CloudService cs = new CloudService();
                return View(cs);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCloudService(CloudService cs)
        {
            int loggedInUserID = (int)Membership.GetUser().ProviderUserKey;
            Subscription sub = db.Subscriptions.FirstOrDefault(s => s.UserId == loggedInUserID && s.Status == "Active");
            cs.SubscriptionRecordID = sub.SubscriptionRecordID;
            cs.Status = "Active";

            if (ModelState.IsValid)
            {
                db.CloudServices.AddObject(cs);
                db.SaveChanges();
                return RedirectToAction("Overview");
            }
            else
            {
                return View(cs);
            }
        }

        [Authorize]
        public ActionResult EditCloudService(int id)
        {
            CloudService cs = db.CloudServices.FirstOrDefault(c => c.CloudServiceID == id);
            return View("EditCloudService", cs);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditCloudService(CloudService cs)
        {
            if (ModelState.IsValid)
            {
                db.CloudServices.Attach(cs);
                db.ObjectStateManager.ChangeObjectState(cs, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Overview");
            }
            else
            {
                return View("EditCloudService", cs);
            }
        }

        [Authorize]
        public ActionResult DeleteCloudService(int id)
        {
            using (db)
            {
                CloudService cs = db.CloudServices.FirstOrDefault(c => c.CloudServiceID == id);
                return View("DeleteCloudService", cs);
            }
        }

        [Authorize]
        [HttpPost, ActionName("DeleteCloudService")]
        public ActionResult DeleteCloudServiceConfirmed(int id)
        {
            using (db)
            {
                CloudService cs = db.CloudServices.FirstOrDefault(c => c.CloudServiceID == id);
                cs.Status = "Inactive";
                db.SaveChanges();
                return RedirectToAction("Overview");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
