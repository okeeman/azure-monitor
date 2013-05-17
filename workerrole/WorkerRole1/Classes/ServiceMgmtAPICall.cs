// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This class is used to make the request to the Windows Azure Service Management
// API. It makes requests on a per cloud service basis. It returns an XML file.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring HttpWebRequest into scope.
using System.Net;
// Added to bring XmlTextReader into scope.
using System.Xml;
// Added to bring domain models into scope.
using WorkerRole1.Models;
// Added to bring X509Store & OpenFlags into scope.
using System.Security.Cryptography.X509Certificates;
// Added to bring SecurityException into scope.
using System.Security;
// Added to bring CryptographicException into scope.
using System.Security.Cryptography;
// Added to bring Stream into scope.
using System.IO;
// Added to bring Trace into scope.
using System.Diagnostics;

namespace WorkerRole1.Classes
{
    public class ServiceMgmtAPICall
    {
        public static XmlTextReader MakeHealthRequest(Subscription sub, CloudService cs, Certificate cert)
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
                    Trace.WriteLine("Error: The store is unreadable.");
                    throw;
                }
                else if (e is SecurityException)
                {
                    Trace.WriteLine("Error: You don't have the required permission.");
                    throw;
                }
                else if (e is ArgumentException)
                {
                    Trace.WriteLine("Error: Invalid values in the store.");
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

            try
            {
                // Make the request to the Uri.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                XmlTextReader reader = new XmlTextReader(responseStream);
                return reader;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Web request has thrown an exception " + e.Message, "Information");
                throw;
            }
        }
    }
}
