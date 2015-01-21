// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// * The Worker Role makes health requests for all cloud services with a status of 
// Active. A subscription may have many cloud services. Each cloud service may have 
// many instances. If any instance is not in ReadyRole, i.e., ready to accept 
// requests, then an error message SMS is sent to the user's phone. 
// * If the Run method throws an exception the exception message is sent to the Azure
// Doctor administrator. 
// * There are a number of Trace.WriteLines included. These prove useful
// during local development & debugging using the Compute UI emulator.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
// Added to bring IAzureDoctorDb and AzureDoctorDb into scope.
using WorkerRole1.Models;
// Added to bring XmlTextReader into scope.
using System.Xml;

namespace WorkerRole1.Classes
{
    public class WorkerRole : RoleEntryPoint
    {
        private IAzureDoctorDb _db;
        PushNotifications pn = new PushNotifications();
        const string ClickatellUsername = "okeeman";
        const string ClickatellPassword = "ECMPIbIELWBCSA";

        // Default constructor - hits real database.
        public WorkerRole()
        {
            _db = new AzureDoctorDb();
        }

        // Fake database - used for unit tests.
        public WorkerRole(IAzureDoctorDb db)
        {
            _db = db;
        }

        public Certificate Certificate
        {
            get
            {
                return _db.Query<Certificate>().First();
            }
        }

        public override void Run()
        {
            Trace.WriteLine("Inside the Run method", "Information");

            while (true)
            {
                try
                {
                    List<CloudService> cloudServiceCollection = new List<CloudService>();
                    foreach (var service in _db.Query<CloudService>())
                    {
                        if (service.Status == "Active")
                        {
                            cloudServiceCollection.Add(service);
                        }
                    }

                    foreach (var service in cloudServiceCollection)
                    {
                        // Find the subscription associated with the CloudService.
                        Subscription sub = _db.Query<Subscription>().FirstOrDefault(s => s.SubscriptionRecordID == service.SubscriptionRecordID);

                        Trace.WriteLine("sub: " + sub.AzureSubscriptionID, "Information");
                        Trace.WriteLine("service: " + service.CloudServiceID, "Information");
                        //Trace.WriteLine("cert: " + cert.Thumbprint, "Information");
                        Trace.WriteLine("cert: " + this.Certificate.Thumbprint, "Information");
                        Trace.WriteLine("service details: " + service.CloudServiceName, "Information");
                        Trace.WriteLine("Before health request", "Information");
                        // Send request to the Windows Azure API.
                        XmlTextReader reader = ServiceMgmtAPICall.MakeHealthRequest(sub, service, this.Certificate);
                        ParseResults pr = new ParseResults();
                        RequestResult rr = pr.ParseXmlText(reader);
                        rr.CloudServiceID = service.CloudServiceID;

                        Trace.WriteLine("Before AllInstancesHealthy check", "Information");
                        // Send SMS if the Cloud Service is unhealthy.
                        if (rr.AllInstancesHealthy != true)
                        {
                            Trace.WriteLine("Not all instances are healthy, SMS push", "Information");
                            Trace.WriteLine("Cloud service " + service.CloudServiceName + " has experienced an error", "Information");
                            string message = String.Format("Your cloud service, {0}, has encountered an error", service.CloudServiceName);
                            pn.PushSMS(sub.PhoneNumber, message, ClickatellUsername, ClickatellPassword);
                        }
                        else
                        {
                            Trace.WriteLine("Cloud service " + service.CloudServiceName + " is healthy", "Information");
                        }

                        Trace.WriteLine("Before save changes", "Information");
                        _db.Add(rr);
                        _db.SaveChanges();
                        Trace.WriteLine("After save changes", "Information");
                      
                        //Get the RequestResultID which was generated in the database as a result of writing the RequestResult entity to it.
                        //This is not known until the RequestResult entity is written to the database. It is a Foreign Key constraint in the InstanceStatu table.
                        int maxID = _db.Query<RequestResult>().Where(r => r.CloudServiceID == service.CloudServiceID).Max(r => r.RequestID);
                        Trace.WriteLine("maxID: " + maxID, "Information");

                        foreach (var status in pr.InstanceStatuses)
                        {
                            Trace.WriteLine("Inside instance statuses foreach in Run method", "Information");
                            InstanceStatu i = new InstanceStatu();
                            i.InstanceStatus = status;
                            i.RequestID = maxID;
                            _db.Add(i);
                            _db.SaveChanges();
                        }
                    }

                    Thread.Sleep(30000);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("The Run method has thrown an exception: " + e.Message + "\nInner exception: " + e.InnerException, "Information");
                    string exceptionMessage = "The Run method has thrown an exception: " + e.Message + "\nInner exception: " + e.InnerException;
                    // Sends SMS to the site admin rather than the users.
                    pn.PushSMS("353872802526", exceptionMessage, ClickatellUsername, ClickatellPassword);
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            return base.OnStart();
        }
    }
}
