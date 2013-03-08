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
            ViewBag.Title = "Azure Doctor";

            return View("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page";

            return View("Contact");
        }
    }
}
