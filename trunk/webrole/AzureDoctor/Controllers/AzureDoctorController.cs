using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureDoctor.Models;

namespace AzureDoctor.Controllers
{
    [HandleError]
    public class AzureDoctorController : Controller
    {
        private AzureDoctorEntities db = new AzureDoctorEntities();

        // ASP.NET does not differentiate between Actions with the same signature but it does differentiate between Actions with different attributes.
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        } 

        // Cloud service ID will be hard-coded for now. There will be a connection between users and subscriptions in the next iteration.
        // To allow for having multiple subscriptions could pass in a collection of subscription ID's.
        // The parameter is hard-coded in the Ajax.Actionlink for this iteration.
        [Authorize]
        [HttpPost]
        public ActionResult GetHealthStatus(int id)
        {
            RequestResult rr = new RequestResult();
            Certificate cert = db.Certificates.First();
            Subscription sub = db.Subscriptions.Single(s => s.SubscriptionRecordID == id);
            // Put CloudServices into local memory as a List before making the health requests. 
            List<CloudService> cloudServiceCollection = new List<CloudService>();
            foreach (var service in db.CloudServices.Where(s => s.SubscriptionRecordID == sub.SubscriptionRecordID))
            {
                cloudServiceCollection.Add(service);
            }

            DateTime? requestTime = DateTime.Now;
            List<RequestResult> resultCollection = new List<RequestResult>();
            foreach (var service in cloudServiceCollection)
            {
                rr.MakeHealthRequest(sub, service, cert);
                rr.CloudServiceID = service.CloudServiceID;
                rr.DateTime = requestTime;
                resultCollection.Add(rr);
            }

            ViewBag.Subscription = sub;
            ViewBag.CloudServiceCollection = cloudServiceCollection;
            ViewBag.RequestResult = rr;

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
    }
}
