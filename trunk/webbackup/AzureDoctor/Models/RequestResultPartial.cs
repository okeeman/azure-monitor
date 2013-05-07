using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// Added to bring Entityobject into scope.
using System.Data.Objects.DataClasses;
// Added to bring HttpWebRequest into scope.
using System.Net;
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

namespace AzureDoctor.Models
{
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

                // Gets instance count.
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
            this.HostedServiceStatus = this.StatusList[0];
            this.DeploymentStatus = this.StatusList[1];
            // StatusList[2] will only have a value if a VM update has been performed.
            if (serviceStatusListCount > 2)
            {
                this.VMStatus = this.StatusList[2];
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
}