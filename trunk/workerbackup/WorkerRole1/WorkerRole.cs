using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
// Note WindowsAzure.Diagnostics - separate from System.Diagnostics.
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
// Added to bring X509Store & OpenFlags into scope.
using System.Security.Cryptography.X509Certificates;
// Added to bring SecurityException into scope.
using System.Security;
// Added to bring CryptographicException into scope.
using System.Security.Cryptography;
// Added to bring Stream into scope.
using System.IO;
// Added to bring XmlTextReader into scope.
using System.Xml;
// Added to bring UTF8Encoding & Decoder into scope.
using System.Text;
// Added to bring XDocument & XNamespace into scope.
using System.Xml.Linq;
// Added to bring Entityobject into scope.
using System.Data.Objects.DataClasses;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Trace.WriteLine("Inside the Run method", "Information");
            PushNotifications pn = new PushNotifications();

            while (true)
            {
                try
                {
                    using (var db = new AzureDoctorEntities())
                    {
                        Certificate cert = db.Certificates.First();

                        List<CloudService> cloudServiceCollection = new List<CloudService>();
                        foreach (var service in db.CloudServices)
                        {
                            if (service.Status == "Active")
                            {
                                cloudServiceCollection.Add(service);
                            }
                        }

                        // DateTime in the database is nullable.
                        DateTime? requestTime = DateTime.Now;

                        foreach (var service in cloudServiceCollection)
                        {
                            // Create a subscription entity associated with the CloudService.
                            Subscription sub = db.Subscriptions.FirstOrDefault(s => s.SubscriptionRecordID == service.SubscriptionRecordID);
                            RequestResult rr = new RequestResult();
                            
                            // Send request to the Azure API.
                            Trace.WriteLine("sub: " + sub.AzureSubscriptionID, "Information");
                            Trace.WriteLine("service: " + service.CloudServiceID, "Information");
                            Trace.WriteLine("cert: " + cert.Thumbprint, "Information");
                            //
                            Trace.WriteLine("service details: " + service.CloudServiceName, "Information");
                            Trace.WriteLine("Before health request", "Information");
                            rr.MakeHealthRequest(sub, service, cert);
                            
                            // Put values into these entity properties. The other properties are populated by the MakeHealthRequest method.
                            Trace.WriteLine("Before assigning CloudServiceID", "Information");
                            rr.CloudServiceID = service.CloudServiceID;
                            Trace.WriteLine("Before assigning DateTime", "Information");
                            rr.DateTime = requestTime;

                            Trace.WriteLine("Before AllInstancesHealthy check", "Information");
                            if (rr.AllInstancesHealthy != true)
                            {
                                Trace.WriteLine("All instance not healthy, SMS push", "Information");
                                Trace.WriteLine("Cloud service " + service.CloudServiceName + " has experienced an error", "Information");
                                string message = String.Format("Your cloud service, {0}, has encountered an error", service.CloudServiceName);
                                pn.PushSMS(sub.PhoneNumber, message);
                            }
                            else
                            {
                                Trace.WriteLine("Cloud service " + service.CloudServiceName + " is healthy", "Information");
                            }

                            Trace.WriteLine("Before save changes", "Information");
                            db.RequestResults.AddObject(rr);
                            db.SaveChanges();
                            Trace.WriteLine("After save changes", "Information");
                      
                            // Get the RequestResultID which was generated in the database as a result of writing the RequestResult entity to it.
                            // This is not known until the RequestResult entity is written to the database. It is a Foreign Key constraint in the InstanceStatus table.
                            int maxID = db.RequestResults.Where(r => r.CloudServiceID == service.CloudServiceID).Max(r => r.RequestID);
                            Trace.WriteLine("maxID: " + maxID, "Information");

                            // Note that InstanceStatuses is not part of the entity. It is a List in local memory created by the MakeHealthRequest method.
                            // A new InstanceStatu entity is created for each member of the List. These entities will become records in the database.
                            foreach (var status in rr.InstanceStatuses)
                            {
                                Trace.WriteLine("Inside instance statuses foreach in Run method", "Information");
                                InstanceStatu i = new InstanceStatu();
                                i.InstanceStatus = status;
                                i.RequestID = maxID;
                                db.InstanceStatus.AddObject(i);
                                db.SaveChanges();
                            }
                        }
                    }

                    Thread.Sleep(30000);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("The Run method has thrown an exception: " + e.Message + "\nInner exception: " + e.InnerException, "Information");
                    string exceptionMessage = "The Run method has thrown an exception: " + e.Message + "\nInner exception: " + e.InnerException;
                    // Sends SMS to the site admin rather than the users.
                    pn.PushSMS("353872802526", exceptionMessage);
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


    public partial class RequestResult : EntityObject
    {
        public List<string> StatusList { get; set; }
        public List<string> InstanceStatuses { get; set; }

        public RequestResult()
        {
            this.StatusList = new List<string>();
            this.InstanceStatuses = new List<string>();
        }

        public void MakeHealthRequest(Subscription sub, CloudService cs, Certificate cert)
        {
            Uri getDetailsRequest = new Uri("https://management.core.windows.net/" + sub.AzureSubscriptionID + "/services/hostedservices/" + cs.CloudServiceName + "?embed-detail=true");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getDetailsRequest);
            request.Headers.Add("x-ms-version", "2012-03-01");
            request.Method = "GET";
            request.ContentType = "application/xml";

            X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                certStore.Open(OpenFlags.ReadOnly);
            }
            catch (Exception e)
            {
                if (e is CryptographicException)
                {
                    // Change these to appropriate exception actions for a Web application/Web service.
                    Console.WriteLine("Error: The store is unreadable.");
                    //certificateExceptionNotification();
                    throw;
                }
                else if (e is SecurityException)
                {
                    Console.WriteLine("Error: You don't have the required permission.");
                    throw;
                }
                else if (e is ArgumentException)
                {
                    Console.WriteLine("Error: Invalid values in the store.");
                    throw;
                }
                else
                {
                    throw;
                }
            }

            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, cert.Thumbprint, false);
            if (certCollection.Count == 0)
            {
                throw new Exception("Error: No certificate found containing thumbprint");
            }
            certStore.Close();

            // There is only 1 cert in this collection, i.e. the one with the specified thumbprint.
            X509Certificate2 certificate = certCollection[0];
            // Add the certificate to the request.
            request.ClientCertificates.Add(certificate);

            // Make the request to the Uri.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            XmlTextReader reader = new XmlTextReader(responseStream);

            while (reader.Read())
            {
                // There are 3 elements in the returned XML document with the name Status. The first is the status of the hosted service created before application deployment.
                // The second is the deployed application status. The third is the VM update status (occurs when changes are made to the VM, e.g. stop VM).
                // Hosted Service status values: Creating, Created, Deleting, Deleted, Changing, ResolvingDns.
                // Deployment status values:  Running, Suspended, RunningTransitioning, SuspendedTransitioning, Starting, Suspending, Deploying, Deleting.
                // VM update status values:  PersistentVMUpdateScheduled, PersistentVMUpdateRunning, PersistentVMUpdateCancelled, PersistentVMUpdateStopped, PersistentVMUpdateCompleted.
                if (reader.Name == "Status")
                {
                    reader.Read();
                    this.StatusList.Add(reader.Value);
                    reader.Read();
                }

                // Deployment Slot may be staging or Production.
                if (reader.Name == "DeploymentSlot")
                {
                    reader.Read();
                    this.DeploymentSlot = reader.Value;
                    reader.Read();
                }

                // Role level; instance size may be Extra Small, Small, Medium, Large or Extra Large.
                if (reader.Name == "InstanceSize")
                {
                    reader.Read();
                    this.InstanceSize = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "Configuration")
                {
                    reader.Read();
                    //Decoding the services configuration file and extracting the number of instances from them.
                    UTF8Encoding encoder = new UTF8Encoding();
                    Decoder decoder = encoder.GetDecoder();
                    byte[] b = Convert.FromBase64String(reader.Value);
                    // GetCharCount parameters: byte array, int index, int count
                    int charCount = decoder.GetCharCount(b, 0, b.Length);
                    char[] decodedChars = new char[charCount];
                    decoder.GetChars(b, 0, b.Length, decodedChars, 0);
                    String existingConfig = new String(decodedChars);
                    XDocument changeConfigXDoc = XDocument.Parse(existingConfig);
                    XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration");
                    string instanceCountString = changeConfigXDoc.Element(ns + "ServiceConfiguration").Element(ns + "Role").Element(ns + "Instances").Attribute("count").Value.ToString();
                    this.InstanceCount = instanceCountString;
                    reader.Read();
                }

                // Instance status values:  RoleStateUnknown, CreatingVM, StartingVM, CreatingRole, StartingRole, ReadyRole, BusyRole,
                // StoppingRole, StoppingVM, DeletingVM, StoppedVM, RestartingRole, CyclingRole, FailedStartingVM, UnresponsiveRole.
                if (reader.Name == "InstanceStatus")
                {
                    reader.Read();
                    this.InstanceStatuses.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "Vip")
                {
                    reader.Read();
                    this.VIPAddress = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "Location")
                {
                    reader.Read();
                    this.Location = reader.Value;
                    reader.Read();
                }
            }

            int serviceStatusListCount = this.StatusList.Count();
            this.HostedServiceStatus = StatusList[0];
            this.DeploymentStatus = StatusList[1];
            // StatusList[2] will only have a value if a VM update has been performed.
            if (serviceStatusListCount > 2)
	        {
		         this.VMStatus = StatusList[2];
	        }

            // Nullable bool. SQL Server bit type can be 0, 1 or null.
            this.AllInstancesHealthy = true;
            foreach (var item in this.InstanceStatuses)
            {
                if (item != "ReadyRole")
                {
                    this.AllInstancesHealthy = false;
                    break;
                }
            }
        }
    }

    public class PushNotifications
    {
        public void PushSMS(string userPhoneNumber, string message)
        {
            Uri getDetailsRequest = new Uri("http://api.clickatell.com/http/sendmsg?user=okeeman&password=ECMPIbIELWBCSA&api_id=3411315&to=" + userPhoneNumber + "&text=" + message);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getDetailsRequest);
            request.Method = "POST";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }
    }
}
