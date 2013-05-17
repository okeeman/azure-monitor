// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// Standard tests of the Home Controller.
// ----------------------------------------------------------------------------------
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
            Assert.AreEqual("Index", result.ViewName);
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
        public void Instructions()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Instructions() as ViewResult;

            // Assert
            Assert.AreEqual("Instructions", result.ViewName);
        }
    }
}
