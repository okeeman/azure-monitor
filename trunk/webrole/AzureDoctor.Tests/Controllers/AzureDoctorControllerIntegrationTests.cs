//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using AzureDoctor;
//using AzureDoctor.Controllers;
////
//using AzureDoctor.Models;
//// Added to bring Fakes into scope.
//using Microsoft.QualityTools.Testing.Fakes;
//using AzureDoctor.Models.Fakes;
//using AzureDoctor.Controllers.Fakes;
//using System.Data.Entity.Fakes;
//using System.Fakes;
//using System.Data.Entity;

//namespace AzureDoctor.Tests.Controllers
//{
//    [TestClass]
//    public class AzureDoctorControllerIntegrationTests
//    {
//        [TestMethod]
//        public void TestMakeHealthRequest()
//        {



//            // Arrange.
//            Subscription sub = new Subscription();
//            CloudService cs = new CloudService();
//            RequestResult rr = new RequestResult();
//            Certificate cert = new Certificate();
//            cs.CloudServiceName = "housecondition";
//            cert.Thumbprint = "E44005204846FFC2430E1FD7311EF21DE0496A36";
//            cs.CloudServiceID = 1;
                
//            // Act.
//            rr.MakeHealthRequest(sub, cs, cert);

//            // Assert.
//            Assert.AreEqual(rr.HostedServiceStatus, "Created");
//            Assert.AreEqual(rr.DeploymentStatus, "Running");
//            Assert.AreEqual(rr.InstanceStatuses[0], "ReadyRole");
//            Assert.AreEqual(rr.InstanceCount, 1);
//        }

        
//        [TestMethod]
//        public void TestOfTestService()
//        {
//            using (ShimsContext.Create())
//            {
//                // Arrange.
//                // Set up a fake health model and set its properties.
//                ShimHealthModel hm = new ShimHealthModel();
//                List<string> statusList = new List<string>();
//                statusList.Add("Created");
//                statusList.Add("Running");
//                hm.StatusListGet = () => statusList;
//                hm.DeploymentSlotGet = () => "Production";
//                hm.InstanceSizeGet = () => "Small";
//                hm.InstanceCountGet = () => 2;
//                List<string> instanceStatuses = new List<string>();
//                instanceStatuses.Add("ReadyRole");
//                instanceStatuses.Add("ReadyRole");
//                hm.InstanceStatusesGet = () => instanceStatuses;
//                hm.VIPAddressGet = () => "111.22.33.4444";
//                hm.LocationGet = () => "Fake Location";
//                hm.AllInstancesHealthyGet = () => true;

//                // Set up a fake date time with a constant value.
//                ShimDateTime.NowGet = () => { return new DateTime(2012, 1, 1); };

//                // Set up a fake cloud service model and set its properties.
//                ShimCloudService cs = new ShimCloudService();
//                cs.CloudServiceIDGet = () => 1;
//                cs.UserIDGet = () => 1;
//                cs.SubscriptionIDGet = () => "Fake Subscription ID";
//                cs.ThumbprintGet = () => "Fake Thumbprint";
//                cs.ServiceNameGet = () => "Fake Service Name";

//                // Set connection to the fake database.
//                ShimAzureDoctorEntities db = new ShimAzureDoctorEntities();

//                //
//                ShimAzureDoctorDomainService ds = new ShimAzureDoctorDomainService();

//                ShimDbContext dbc = new ShimDbContext();
//                //ShimDbExtensions dbe = new ShimDbExtensions();
//                ShimDatabase d = new ShimDatabase();
                
//                ShimDbModelBuilder dbmb = new ShimDbModelBuilder();
//                //cs = new DbSet<ShimCloudService>.Create<ShimCloudService>();
//                //DbSet<ShimHealthModel> hmSet = new DbSet<ShimHealthModel>();
//                //ShimDbSet cloudservicedbs = new ShimDbSet();
//                //ShimCreateDatabaseIfNotExists<ShimCloudService> csdatabase = new 
               
//                // Provide a fake user ID.
//                int fakeUserID = 1;

//                // Act.
//                AzureDoctorController controller = new AzureDoctorController();
//                ViewResult result = controller.TestService(fakeUserID) as ViewResult;

//                // Assert.
//                Assert.IsNotNull(result);
//            }
//    }
//}
