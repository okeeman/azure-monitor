using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDoctor;
using AzureDoctor.Controllers;
// Added to bring model classes into scope.
using AzureDoctor.Models;

namespace AzureDoctor.Tests.Controllers
{
    [TestClass]
    public class AzureDoctorControllerIntegrationTests
    {
        // Database connection context.
        AzureDoctorEntities db = new AzureDoctorEntities();
        // Local entities,
        Subscription s = new Subscription();
        CloudService cls = new CloudService();
        RequestResult r = new RequestResult();
        Certificate c = new Certificate();

        [TestMethod]
        public void TestMakeHealthRequestMethod()
        {
            // Arrange.
            s.AzureSubscriptionID = "15c97ffb-ad63-48be-aca9-31f0a2238418";
            cls.CloudServiceName = "housecondition";
            c.Thumbprint = "E44005204846FFC2430E1FD7311EF21DE0496A36";
                
            // Act.
            r.MakeHealthRequest(s, cls, c);

            // Assert.
            Assert.AreEqual(r.HostedServiceStatus, "Created");
            Assert.AreEqual(r.DeploymentStatus, "Running");
            Assert.AreEqual(r.InstanceStatuses[0], "ReadyRole");
            Assert.AreEqual(r.InstanceCount, "1");
        }

        //[TestMethod]
        //public void TestAzureDoctorIndexController()
        //{
        //    // Arrange
        //    AzureDoctorController controller = new AzureDoctorController();

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Index", result.ViewName);
        //}
    }
}
