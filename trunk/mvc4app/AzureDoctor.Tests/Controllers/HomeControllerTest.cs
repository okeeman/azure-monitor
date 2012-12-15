using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDoctor;
using AzureDoctor.Controllers;

namespace AzureDoctor.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestHomeIndexRedirect()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as RedirectToRouteResult;

            // Assert
            // Redirects to AzureDoctorController, Index Action.
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("About", result.ViewName);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.AreEqual("Contact", result.ViewName);
        }

        [TestMethod]
        public void SiteMap()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.SiteMap() as ViewResult;

            // Assert
            Assert.AreEqual("SiteMap", result.ViewName);
        }
    }
}
