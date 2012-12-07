using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// Added to bring HttpWebRequest into scope.
using System.Net;
// Added to bring SecurityException into scope.
using System.Security;
// Added to bring CryptographicExcdeption into scope.
using System.Security.Cryptography;
// Added to bring X509Store & OpenFlags into scope.
using System.Security.Cryptography.X509Certificates;
// Added to bring XmlTextReader into scope.
using System.Xml;
// Added to bring XDocument & XNamespace into scope.
using System.Xml.Linq;
//using System.Xml.Serialization;
// Added to bring Stream into scope.
using System.IO;
// Added to bring RoleEnvironment into scope.
using Microsoft.WindowsAzure.ServiceRuntime;
// Added to bring UTF8Encoding & Decoder into scope.
using System.Text;

namespace AzureDoctor.Models
{
    public class LogicClass
    {
        public void MakeHealthRequest(CloudService cs, out string serviceName, out List<string> statusList, out int instanceCount)
        {
            string subscriptionId = cs.SubscriptionID; 
            serviceName = cs.ServiceName;
            Uri getDetailsRequest = new Uri("https://management.core.windows.net/" + subscriptionId + "/services/hostedservices/" + serviceName + "?embed-detail=true");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getDetailsRequest);
            request.Headers.Add("x-ms-version", "2012-03-01");
            request.Method = "GET";
            request.ContentType = "application/xml";

            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
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
                }
                else if (e is SecurityException)
                {
                    Console.WriteLine("Error: You don't have the required permission.");
                }
                else if (e is ArgumentException)
                {
                    Console.WriteLine("Error: Invalid values in the store.");
                }
                else
                {
                    throw;
                }
            }

            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, cs.Thumbprint, false);
            if (certCollection.Count == 0)
            {
                throw new Exception("Error: No certificate found containing thumbprint " + cs.Thumbprint);
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

            // If there are multiple instances there will be multiple instance statuses.
            string deploymentSlot = null, instanceCountString = null, instanceSize = null, SDKVersion = null, VIPAddress = null;

            List<string> statuses = new List<string>(), instanceStatuses = new List<string>(), instanceErrorCodes = new List<string>(), 
            instanceFaultDomains = new List<string>(), instanceStateDetails = new List<string>();

            while (reader.Read())
            {
                // Service status(is the service created throught the portal ready to accept a deployment?), Deployment status(app you have uploaded to Azure), VM update status.
                if (reader.Name == "Status")
                {
                    reader.Read();
                    statuses.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "DeploymentSlot")
                {
                    reader.Read();
                    deploymentSlot = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "Configuration")
                {
                    reader.Read();
                    //Decoding the services configuration file and extracting the number of instances from them.
                    UTF8Encoding encoder = new UTF8Encoding();
                    Decoder decoder = encoder.GetDecoder();
                    byte[] b = Convert.FromBase64String(reader.Value);
                    // byte array, int index, int count
                    int charCount = decoder.GetCharCount(b, 0, b.Length);
                    char[] decodedChars = new char[charCount];
                    decoder.GetChars(b, 0, b.Length, decodedChars, 0);
                    String existingConfig = new String(decodedChars);
                    XDocument changeConfigXDoc = XDocument.Parse(existingConfig);
                    XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration");
                    instanceCountString = changeConfigXDoc.Element(ns + "ServiceConfiguration").Element(ns + "Role").Element(ns + "Instances").Attribute("count").Value.ToString();
                    reader.Read();
                }

                if (reader.Name == "InstanceSize")
                {
                    reader.Read();
                    instanceSize = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "InstanceStatus")
                {
                    reader.Read();
                    instanceStatuses.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "InstanceStateDetails")
                {
                    reader.Read();
                    instanceStateDetails.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "InstanceErrorCode")
                {
                    reader.Read();
                    instanceErrorCodes.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "InstanceFaultDomain")
                {
                    reader.Read();
                    instanceFaultDomains.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "SdkVersion")
                {
                    reader.Read();
                    SDKVersion = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "Vip")
                {
                    reader.Read();
                    VIPAddress = reader.Value;
                    reader.Read();
                }
            }

            // there are not always 3 statuses, sometimes only 2 or 1????, do a null check for vmstatus.
            // not getting anything back from azure so collection is null??
            statusList = statuses;

            instanceCount = Int32.Parse(instanceCountString);
        }
    }
}