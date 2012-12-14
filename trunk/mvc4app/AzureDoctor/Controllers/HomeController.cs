using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureDoctor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "AzureDoctor");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Monitor your Windows Azure Cloud Service";

            return View("About");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page";

            return View("Contact");
        }

        public ActionResult SiteMap()
        {
            ViewBag.Message = "Site map";

            return View("SiteMap");
        }
    }
}
