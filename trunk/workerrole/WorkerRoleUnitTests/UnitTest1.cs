using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// Added to bring classes under test into scope. Entity Framework reference added also.
using WorkerRole1;

namespace WorkerRoleUnitTests
{
    [TestClass]
    public class AzureDoctorWorkerRoleIntegrationTests
    {
        [TestMethod]
        public void TestMakeHealthRequest()
        {
            // Arrange.
            Subscription sub = new Subscription();
            sub.AzureSubscriptionID = "15c97ffb-ad63-48be-aca9-31f0a2238418";
            CloudService cs = new CloudService();
            cs.CloudServiceName = "housecondition";
            RequestResult rr = new RequestResult();
            Certificate cert = new Certificate();
            cert.Thumbprint = "E44005204846FFC2430E1FD7311EF21DE0496A36";

            // Act.
            rr.MakeHealthRequest(sub, cs, cert);

            // Assert.
            Assert.AreEqual(rr.HostedServiceStatus, "Created");
            Assert.AreEqual(rr.DeploymentStatus, "Running");
            Assert.AreEqual(rr.InstanceStatuses[0], "ReadyRole");
            Assert.AreEqual(rr.InstanceCount, "1");
        }
    }
}
