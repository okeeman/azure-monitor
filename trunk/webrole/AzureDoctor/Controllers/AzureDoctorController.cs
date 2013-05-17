// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// * The web role allows setting up and editing of an Azure Doctor account. It polls
// the database every 30 seconds using Ajax. The latest results for the logged in
// user are displayed. 
// * There are checks in the CRUD to ensure that a user does not enter invalid 
// details. On creation of a subscription or cloud service in Azure Doctor its status
// is set to 'Active'. When a user 'deletes' a subscription or cloud service its 
// status is set to 'Inactive' and the Worker Role will stop making health requests 
// for it. This allows the health history to be maintained in the database and for
// ad hoc SQL queries to be executed against this data.
// * The userKey field is used to isolate the Membership dependency when testing as 
// this depenency is difficult to mock. In production the user ID will be determined 
// using the Membership class but when testing this class will be bypassed and the 
// ID assigned in the non-default constructor will be used.
// * When the user is logged in the application requires a HTTPS connection. These
// connections use a self-signed certificate. This will cause browsers to warn users
// as the certificate is not signed by a root authority. The connection is actually
// secure though.
// ----------------------------------------------------------------------------------
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
        private IAzureDoctorDb _db;
        private int? userKey;

        // Default constructor - hits real database.
        public AzureDoctorController()
        {
            _db = new AzureDoctorDb();
            this.userKey = null;
        }

        // Fake database - used for unit tests.
        public AzureDoctorController(IAzureDoctorDb db)
        {
            _db = db;
            this.userKey = 8888;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        private int getUserId(int? id)
        {
            if (id == null)
            {
                return (int)Membership.GetUser().ProviderUserKey;
            }
            else
            {
                return (int)id;
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        [RequireHttps]
        public ActionResult Overview()
        {
            int userID = this.getUserId(this.userKey);
            SubAndServiceViewModel subcs = new SubAndServiceViewModel();
            subcs.Sub = _db.Query<Subscription>().FirstOrDefault(s => s.UserId == userID && s.Status == "Active");

            if (subcs.Sub != null)
            {
                var getCloudServices = _db.Query<CloudService>().Where(cs => cs.SubscriptionRecordID == subcs.Sub.SubscriptionRecordID && cs.Status == "Active");

                foreach (var cloudService in getCloudServices)
                {
                    subcs.CloudServices.Add(cloudService);
                }

                return View("Overview", subcs);
            }
            else
            {
                return View("Overview");
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        [RequireHttps]
        public ActionResult GetHealthStatus()
        {
            int userID = this.getUserId(this.userKey);
            Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.UserId == userID && s.Status == "Active");
            bool overallAllInstancesHealthy = true;
            List<ResultsViewModel> rvmCollection = new List<ResultsViewModel>();

            if (!UserHasActiveSubscription(userID) || !SubscriptionHasCloudServices(sub))
            {
                ViewBag.ErrorMessage = "You must set up a subscription and cloud service(s) in your Azure Doctor account before checking their health";
                return View("Overview");
            }
            else
            {
                foreach (var service in _db.Query<CloudService>().Where(s => s.SubscriptionRecordID == sub.SubscriptionRecordID))
                {
                    ResultsViewModel rvm = new ResultsViewModel();

                    if (service.Status == "Active")
                    {
                        var latestResultID = _db.Query<RequestResult>().Where(r => r.CloudServiceID == service.CloudServiceID).Max(r => r.RequestID);
                        var latestResult = _db.Query<RequestResult>().FirstOrDefault(r => r.RequestID == latestResultID);

                        rvm.CloudServiceName = service.CloudServiceName;
                        DateTime time = DateTime.Now.ToUniversalTime().AddHours(1);
                        string formatDateTime = "ddd d MMMM yyyy H:mm:ss";
                        rvm.DateTime = time.ToString(formatDateTime);
                        rvm.Location = latestResult.Location;
                        rvm.InstanceCount = latestResult.InstanceCount;
                        rvm.InstanceSize = latestResult.InstanceSize;
                        rvm.DeploymentSlot = latestResult.DeploymentSlot;
                        rvm.HostedServiceStatus = latestResult.HostedServiceStatus;
                        rvm.VIPAddress = latestResult.VIPAddress;

                        if (latestResult.AllInstancesHealthy != true)
                        {
                            overallAllInstancesHealthy = false;
                        }

                        var instanceStatuses = _db.Query<InstanceStatu>().Where(i => i.RequestID == latestResult.RequestID);

                        foreach (var status in instanceStatuses)
                        {
                            rvm.InstanceStatuses.Add(status.InstanceStatus);
                        }

                        rvmCollection.Add(rvm);
                    }
                }

                ViewBag.ResultsViewModelCollection = rvmCollection;
                ViewBag.OverallAllInstancesHealthy = overallAllInstancesHealthy;
            }

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
            if (_db.Query<Subscription>().Count(s => (s.UserId == userId) && (s.Status == "Active")) > 0)
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
            if (_db.Query<CloudService>().Count(cs => (cs.SubscriptionRecordID == sub.SubscriptionRecordID) && (cs.Status == "Active")) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Authorize]
        [RequireHttps]
        public ActionResult AddSubscription()
        {
            int userID = this.getUserId(this.userKey);
            Subscription sub = new Subscription();

            if (UserHasActiveSubscription(userID))
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
        [RequireHttps]
        public ActionResult AddSubscription(Subscription sub)
        {
            int userID = this.getUserId(this.userKey);
            sub.UserId = userID;
            sub.Status = "Active";

            if (ModelState.IsValid)
            {
                _db.Add(sub);
                _db.SaveChanges();
                return RedirectToAction("Overview");
            }
            else
            {
                return View("AddSubscription", sub);
            }            
        }

        [Authorize]
        [InitializeSimpleMembership]
        [RequireHttps]
        public ActionResult EditSubscription(int id)
        {
            Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.SubscriptionRecordID == id && s.Status == "Active");

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
        [RequireHttps]
        public ActionResult EditSubscription(Subscription sub)
        {
            if (ModelState.IsValid)
            {
                _db.Update(sub);
                _db.SaveChanges();
                return RedirectToAction("Overview");
            }
            else
            {
                return View("EditSubscription", sub);
            }
        }

        [Authorize]
        [InitializeSimpleMembership]
        [RequireHttps]
        public ActionResult DeleteSubscription(int id)
        {
            using (_db)
            {
                Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.SubscriptionRecordID == id);
               
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
        [RequireHttps]
        public ActionResult DeleteSubscriptionConfirmed(int id)
        {
            using (_db)
            {
                Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.SubscriptionRecordID == id);
                sub.Status = "Inactive";
                _db.SaveChanges();
                return RedirectToAction("Overview");
            }
        }

        [Authorize]
        [RequireHttps]
        public ActionResult AddCloudService()
        {
            int userID = this.getUserId(this.userKey);

            if (!UserHasActiveSubscription(userID))
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
        [RequireHttps]
        public ActionResult AddCloudService(CloudService cs)
        {
            int userID = this.getUserId(this.userKey);
            Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.UserId == userID && s.Status == "Active");
            cs.SubscriptionRecordID = sub.SubscriptionRecordID;
            cs.Status = "Active";

            // Check that the details entered are valid. This is to prevent the Worker Role from making requests with invalid details which will return HTTP 403 - Forbidden.
            ValidateDetails v = new ValidateDetails();
            if (!v.DetailsAreValid(sub, cs))
            {
                ViewBag.ErrorMessage = "Windows Azure has not returned a health result. Check that your subscription and cloud service details are correct.";
                return View("Overview");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _db.Add(cs);
                    _db.SaveChanges();
                    return RedirectToAction("Overview");
                }
                else
                {
                    return View(cs);
                }
            }
        }

        [Authorize]
        [RequireHttps]
        public ActionResult EditCloudService(int id)
        {
            CloudService cs = _db.Query<CloudService>().FirstOrDefault(c => c.CloudServiceID == id);
            return View("EditCloudService", cs);
        }

        [Authorize]
        [HttpPost]
        [RequireHttps]
        public ActionResult EditCloudService(CloudService cs)
        {
            // Check that the edited details entered are valid. This is to prevent the Worker Role from making requests with invalid details which will return HTTP 403 - Forbidden.
            int userID = this.getUserId(this.userKey);
            Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.UserId == userID && s.Status == "Active");
            ValidateDetails v = new ValidateDetails();
            if (!v.DetailsAreValid(sub, cs))
            {
                ViewBag.ErrorMessage = "Windows Azure has not returned a health result. Check that your subscription and cloud service details are correct.";
                return View("Overview");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _db.Update(cs);
                    _db.SaveChanges();
                    return RedirectToAction("Overview");
                }
                else
                {
                    return View("EditCloudService", cs);
                }
            }
        }

        [Authorize]
        [RequireHttps]
        public ActionResult DeleteCloudService(int id)
        {
            using (_db)
            {
                CloudService cs = _db.Query<CloudService>().FirstOrDefault(c => c.CloudServiceID == id);
                return View("DeleteCloudService", cs);
            }
        }

        [Authorize]
        [HttpPost, ActionName("DeleteCloudService")]
        [RequireHttps]
        public ActionResult DeleteCloudServiceConfirmed(int id)
        {
            using (_db)
            {
                CloudService cs = _db.Query<CloudService>().FirstOrDefault(c => c.CloudServiceID == id);
                cs.Status = "Inactive";
                _db.SaveChanges();
                return RedirectToAction("Overview");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
