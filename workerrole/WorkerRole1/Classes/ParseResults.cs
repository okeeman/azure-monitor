// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This class parse the XML file returned by Windows Azure and creates a 
// RequestResult entity and a list of instance statuses which are used to populate 
// the RequestResult and InstanceStatu tables in the database.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring domain models into scope.
using WorkerRole1.Models;
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
// Added to bring XDocument & XNamespace into scope.
using System.Xml.Linq;

namespace WorkerRole1.Classes
{
    public class ParseResults
    {
        public List<string> InstanceStatuses = new List<string>();

        public RequestResult ParseXmlText(XmlTextReader reader)
        {
            RequestResult rr = new RequestResult();
            DateTime? requestTime = DateTime.Now;
            rr.DateTime = requestTime;
            int statusCount = 0;

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
                    statusCount++;
                    if (statusCount == 1)
                    {
                        rr.HostedServiceStatus = reader.Value;
                    }
                    else if (statusCount == 2)
                    {
                        rr.DeploymentStatus = reader.Value;
                    }
                    else if (statusCount == 3)
                    {
                        rr.VMStatus = reader.Value;
                    }
                    reader.Read();
                }

                // Deployment Slot may be Staging or Production.
                if (reader.Name == "DeploymentSlot")
                {
                    reader.Read();
                    rr.DeploymentSlot = reader.Value;
                    reader.Read();
                }

                // Role level; instance size may be Extra Small, Small, Medium, Large or Extra Large.
                if (reader.Name == "InstanceSize")
                {
                    reader.Read();
                    rr.InstanceSize = reader.Value;
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
                    rr.InstanceCount = instanceCountString;
                    reader.Read();
                }

                // Instance status values:  RoleStateUnknown, CreatingVM, StartingVM, CreatingRole, StartingRole, ReadyRole, BusyRole,
                // StoppingRole, StoppingVM, DeletingVM, StoppedVM, RestartingRole, CyclingRole, FailedStartingVM, UnresponsiveRole.
                if (reader.Name == "InstanceStatus")
                {
                    reader.Read();
                    InstanceStatuses.Add(reader.Value);
                    reader.Read();
                }

                if (reader.Name == "Vip")
                {
                    reader.Read();
                    rr.VIPAddress = reader.Value;
                    reader.Read();
                }

                if (reader.Name == "Location")
                {
                    reader.Read();
                    rr.Location = reader.Value;
                    reader.Read();
                }
            }

            rr.AllInstancesHealthy = true;
            foreach (var item in InstanceStatuses)
            {
                if (item != "ReadyRole")
                {
                    rr.AllInstancesHealthy = false;
                    break;
                }
            }

            return rr;
        }
    }
}
