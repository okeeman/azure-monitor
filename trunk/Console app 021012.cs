// ----------------------------------------------------------------------------------
// Windows Azure health monitor project.
// ----------------------------------------------------------------------------------
// A console application to access the status of a Cloud Service running in Windows 
// Azure. A REST call is made by supplying the name of the service, the thumbprint
// for the management certificate and the subscription ID. Various properties may 
// be accessed via the Service Management API, some of which are shown below.
// ----------------------------------------------------------------------------------

using System;
using System.Net;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


public class Test
{
    public static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;

        string serviceName = "myServiceName", thumbprint = "myThumbprint", subscriptionID = "mySubscriptionID";
        Uri getDetailsRequest = new Uri("https://management.core.windows.net/" + subscriptionID + "/services/hostedservices/" + serviceName + "?embed-detail=true");
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
                Console.WriteLine("Error: The store is unreadable.");
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

        // Print out details of the certificates.
        int count = certStore.Certificates.Count;
        Console.WriteLine("Number of certificates in Current User, My: {0}\n", count);
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("Certificate {0} is : \n{1}\n", (i + 1), certStore.Certificates[i]);
        }

        // The cert with the specified thumbprint is put into the collection named certCollection.
        X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
        if (certCollection.Count == 0)
        {
            throw new Exception("Error: No certificate found containing thumbprint " + thumbprint);
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

        string currentServiceStatus, instanceStatus, deploymentSlot, instanceSize, instanceErrorCode, SDKVersion;

        while (reader.Read())
        {
            if (reader.Name == "Status")
            {
                reader.Read();
                currentServiceStatus = reader.Value;
                Console.WriteLine("Current service status: " + currentServiceStatus);
                reader.Read();
            }

            if (reader.Name == "InstanceStatus")
            {
                reader.Read();
                instanceStatus = reader.Value;
                Console.WriteLine("Instance status: " + instanceStatus);
                reader.Read();
            }

            if (reader.Name == "DeploymentSlot")
            {
                reader.Read();
                deploymentSlot = reader.Value;
                Console.WriteLine("Deployment slot: " + deploymentSlot);
                reader.Read();
            }

            if (reader.Name == "InstanceSize")
            {
                reader.Read();
                instanceSize = reader.Value;
                Console.WriteLine("Instance size: " + instanceSize);
                reader.Read();
            }

            if (reader.Name == "InstanceErrorCode")
            {
                reader.Read();
                instanceErrorCode = reader.Value;
                Console.WriteLine("Instance error code: " + instanceErrorCode);
                reader.Read();
            }

            if (reader.Name == "SdkVersion")
            {
                reader.Read();
                SDKVersion = reader.Value;
                Console.WriteLine("SDK version: " + SDKVersion);
                reader.Read();
            }
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}