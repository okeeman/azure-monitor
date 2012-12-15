using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDoctor;
using AzureDoctor.Controllers;
//
using AzureDoctor.Models;

namespace AzureDoctor.Tests.Controllers
{
    [TestClass]
    public class AzureDoctorControllerIntegrationTests
    {
        
        [TestMethod]
        public void TestMakeHealthRequest()
        {
            // Arrange.
            CloudService cs = new CloudService();
            cs.ServiceName = "housecondition";
            cs.SubscriptionID = "7f5f45c1-da00-4435-9694-364ba894dbde";
            cs.Thumbprint = "52D117A8589A69D81290B4FB2A11E5B26324FD88";
            cs.UserID = 1;
            cs.CloudServiceID = 1;

            HealthModel hm = new HealthModel();
                
            // Act.
            hm.MakeHealthRequest(cs);

            // Assert.
            Assert.AreEqual(hm.StatusList[0], "Created");
            Assert.AreEqual(hm.StatusList[1], "Running");
            Assert.AreEqual(hm.InstanceStatuses[0], "ReadyRole");
            Assert.AreEqual(hm.InstanceStatuses[1], "ReadyRole");
            Assert.AreEqual(hm.InstanceCount, 2);
        }
    }
}
