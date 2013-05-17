// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// Dependency injection is used to instantiate a controller which targets a fake
// database. This allows the controller actions to be tested independently of the
// data source. The test initialiser inserts fake data into the fake database. A fake
// controller context is used to mock the Ajax request made in one of the actions.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDoctor;
using AzureDoctor.Controllers;
// Added to bring domain models into scope.
using AzureDoctor.Models;

namespace AzureDoctor.Tests.Controllers
{
    [TestClass]
    public class AzureDoctorControllerUnitTests
    {
        FakeAzureDoctorDb db = new FakeAzureDoctorDb();

        [TestInitialize]
        public void Add_Fake_Data_To_Fake_Db()
        {
            db.AddSet(TestData.Subscriptions);
            db.AddSet(TestData.CloudServices);
            db.AddSet(TestData.RequestResults);
            db.AddSet(TestData.InstanceStatuses);
            db.AddSet(TestData.Certificates);
        }

        [TestMethod]
        public void Overview_Returns_Sub_And_Service_View_Model()
        {
            // Arrange
            AzureDoctorController controller = new AzureDoctorController(db);

            // Act
            ViewResult result = controller.Overview() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(SubAndServiceViewModel));
        }

        [TestMethod]
        public void Overview_Returns_Fake_Sub_Details()
        {
            // Arrange
            AzureDoctorController controller = new AzureDoctorController(db);

            // Act
            ViewResult result = controller.Overview() as ViewResult;
            SubAndServiceViewModel ssvm = (SubAndServiceViewModel)result.Model;

            // Assert
            Assert.AreEqual(7777, ssvm.Sub.SubscriptionRecordID);
            Assert.AreEqual("FakeSubName", ssvm.Sub.SubscriptionName);
            Assert.AreEqual("FakeAzureSubID", ssvm.Sub.AzureSubscriptionID);
            Assert.AreEqual(8888, ssvm.Sub.UserId);
            Assert.AreEqual("3531234567", ssvm.Sub.PhoneNumber);
            Assert.AreEqual("Active", ssvm.Sub.Status);
        }

        [TestMethod]
        public void GetHealthStatus_Returns_Correct_View_Name()
        {
            // Arrange
            AzureDoctorController controller = new AzureDoctorController(db);
            // This will fake the Ajax request.
            controller.ControllerContext = new FakeControllerContext();

            // Act
            ViewResult result = controller.GetHealthStatus() as ViewResult;

            // Assert
            Assert.AreEqual("GetHealthStatus", result.ViewName);
        }

        [TestMethod]
        public void GetHealthStatus_Sets_OverallAllInstancesHealthy_To_True()
        {
            // Arrange
            AzureDoctorController controller = new AzureDoctorController(db);
            controller.ControllerContext = new FakeControllerContext();

            // Act
            ViewResult result = controller.GetHealthStatus() as ViewResult;

            // Assert
            Assert.AreEqual("GetHealthStatus", result.ViewName);
            Assert.AreEqual(true, result.ViewBag.OverallAllInstancesHealthy);
        }
    }
}
