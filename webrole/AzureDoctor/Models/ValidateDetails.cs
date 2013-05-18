// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This is used to validate the Windows Azure details entered by the user. If a 
// successful request is not made then an error message informs the user to check
// that the details are correct.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// Added to bring HttpWebRequest into scope.
using System.Net;
// Added to bring XmlTextReader into scope.
using System.Xml;
// Added to bring X509Store & OpenFlags into scope.
using System.Security.Cryptography.X509Certificates;
// Added to bring SecurityException into scope.
using System.Security;
// Added to bring CryptographicException into scope.
using System.Security.Cryptography;
// Added to bring Stream into scope.
using System.IO;

namespace AzureDoctor.Models
{
    public class ValidateDetails
    {
        private IAzureDoctorDb _db;

        // Default constructor - hits real database.
        public ValidateDetails()
        {
            _db = new AzureDoctorDb();
        }

        // Fake database - used for unit tests.
        public ValidateDetails(IAzureDoctorDb db)
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

        public bool DetailsAreValid(Subscription sub, CloudService cs)
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
                    throw;
                }
                else if (e is SecurityException)
                {
                    throw;
                }
                else if (e is ArgumentException)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }

            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, this.Certificate.Thumbprint, false);
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
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}