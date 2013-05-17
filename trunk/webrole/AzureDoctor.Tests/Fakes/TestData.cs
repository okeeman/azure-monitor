// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This is the test data which is inserted into the fake database for unit tests.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring domain models into scope.
using AzureDoctor.Models;

namespace AzureDoctor.Tests
{
    class TestData
    {
        public static IQueryable<Subscription> Subscriptions
        {
            get
            {
                var subscriptions = new List<Subscription>();
                var subscription = new Subscription() { SubscriptionRecordID = 7777, SubscriptionName = "FakeSubName", AzureSubscriptionID = "FakeAzureSubID", UserId = 8888, PhoneNumber = "3531234567", Status = "Active" };
                subscriptions.Add(subscription);

                return subscriptions.AsQueryable();
            }
        }

        public static IQueryable<CloudService> CloudServices
        {
            get
            {
                var cloudServices = new List<CloudService>();
                var cloudService1 = new CloudService() { CloudServiceID = 9111, SubscriptionRecordID = 7777, CloudServiceName = "FakeCS1", Status = "Active" };
                cloudServices.Add(cloudService1);
                var cloudService2 = new CloudService() { CloudServiceID = 9222, SubscriptionRecordID = 7777, CloudServiceName = "FakeCS2", Status = "Active" };
                cloudServices.Add(cloudService2);
                var cloudService3 = new CloudService() { CloudServiceID = 9333, SubscriptionRecordID = 7777, CloudServiceName = "FakeCS3", Status = "Active" };
                cloudServices.Add(cloudService3);

                return cloudServices.AsQueryable();
            }
        }

        public static IQueryable<RequestResult> RequestResults
        {
            get
            {
                var requestResults = new List<RequestResult>();
                var requestResult1 = new RequestResult() { RequestID = 1, CloudServiceID = 9111, AllInstancesHealthy = true, DeploymentSlot = "Production", InstanceSize = "Small", InstanceCount = "1", VIPAddress = "111.222.333.444", Location = "West Europe", HostedServiceStatus = "Created", DeploymentStatus = "Running", VMStatus = "PersistentVMUpdateCompleted", DateTime = DateTime.Now.AddMinutes(-10) };
                requestResults.Add(requestResult1);
                var requestResult2 = new RequestResult() { RequestID = 2, CloudServiceID = 9222, AllInstancesHealthy = true, DeploymentSlot = "Production", InstanceSize = "Medium", InstanceCount = "2", VIPAddress = "222.333.444.555", Location = "West Europe", HostedServiceStatus = "Created", DeploymentStatus = "Running", VMStatus = null, DateTime = DateTime.Now.AddMinutes(-10) };
                requestResults.Add(requestResult2);
                var requestResult3 = new RequestResult() { RequestID = 3, CloudServiceID = 9333, AllInstancesHealthy = true, DeploymentSlot = "Production", InstanceSize = "Large", InstanceCount = "3", VIPAddress = "333.444.555.666", Location = "West Europe", HostedServiceStatus = "Created", DeploymentStatus = "Running", VMStatus = "PersistentVMUpdateCompleted", DateTime = DateTime.Now.AddMinutes(-10) };
                requestResults.Add(requestResult3);
                var requestResult4 = new RequestResult() { RequestID = 4, CloudServiceID = 9111, AllInstancesHealthy = true, DeploymentSlot = "Production", InstanceSize = "Small", InstanceCount = "1", VIPAddress = "111.222.333.444", Location = "West Europe", HostedServiceStatus = "Created", DeploymentStatus = "Running", VMStatus = "PersistentVMUpdateCompleted", DateTime = DateTime.Now.AddMinutes(-9) };
                requestResults.Add(requestResult4);
                var requestResult5 = new RequestResult() { RequestID = 5, CloudServiceID = 9222, AllInstancesHealthy = true, DeploymentSlot = "Production", InstanceSize = "Medium", InstanceCount = "2", VIPAddress = "222.333.444.555", Location = "West Europe", HostedServiceStatus = "Created", DeploymentStatus = "Running", VMStatus = null, DateTime = DateTime.Now.AddMinutes(-9) };
                requestResults.Add(requestResult5);
                var requestResult6 = new RequestResult() { RequestID = 6, CloudServiceID = 9333, AllInstancesHealthy = true, DeploymentSlot = "Production", InstanceSize = "Large", InstanceCount = "3", VIPAddress = "333.444.555.666", Location = "West Europe", HostedServiceStatus = "Created", DeploymentStatus = "Running", VMStatus = "PersistentVMUpdateCompleted", DateTime = DateTime.Now.AddMinutes(-9) };
                requestResults.Add(requestResult6);

                return requestResults.AsQueryable();
            }
        }

        public static IQueryable<InstanceStatu> InstanceStatuses
        {
            get
            {
                var instanceStatuses = new List<InstanceStatu>();
                var instanceStatus1 = new InstanceStatu() { InstanceStatusID = 1, RequestID = 1, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus1);
                var instanceStatus2 = new InstanceStatu() { InstanceStatusID = 2, RequestID = 2, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus2);
                var instanceStatus3 = new InstanceStatu() { InstanceStatusID = 3, RequestID = 2, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus3);
                var instanceStatus4 = new InstanceStatu() { InstanceStatusID = 4, RequestID = 3, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus4);
                var instanceStatus5 = new InstanceStatu() { InstanceStatusID = 5, RequestID = 3, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus5);
                var instanceStatus6 = new InstanceStatu() { InstanceStatusID = 6, RequestID = 3, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus6);
                var instanceStatus7 = new InstanceStatu() { InstanceStatusID = 7, RequestID = 4, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus7);
                var instanceStatus8 = new InstanceStatu() { InstanceStatusID = 8, RequestID = 5, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus8);
                var instanceStatus9 = new InstanceStatu() { InstanceStatusID = 9, RequestID = 5, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus9);
                var instanceStatus10 = new InstanceStatu() { InstanceStatusID = 10, RequestID = 6, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus10);
                var instanceStatus11 = new InstanceStatu() { InstanceStatusID = 11, RequestID = 6, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus11);
                var instanceStatus12 = new InstanceStatu() { InstanceStatusID = 12, RequestID = 6, InstanceStatus = "ReadyRole" };
                instanceStatuses.Add(instanceStatus12);

                return instanceStatuses.AsQueryable();
            }
        }

        public static IQueryable<Certificate> Certificates
        {
            get
            {
                var certificates = new List<Certificate>();
                var certificate = new Certificate() { CertRecordID = 1, CertificateName = "FakeCert", Thumbprint = "FakeThumbbprint", ExpiryDate = DateTime.Now.AddYears(1) };
                certificates.Add(certificate);

                return certificates.AsQueryable();
            }
        }
    }
}
