// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// The Windows Azure Service Management API returns an XML file which has the
// cloud service status in it. These tests use a fake results XML file to test the
// Azure Doctor program logic. Some of the functionality from earlier iterations was 
// refactored into separate classes in order to make it possible to test the program
// logic without having to mahe HTTP requests or be connected to a database.
// ----------------------------------------------------------------------------------
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// Added to bring FakeAzureDoctorDb and TestData into scope.
using WorkerRoleUnitTests.Fakes;
// Added to bring FakeAzureDoctorDb and TestData into scope.
using WorkerRole1.Classes;
// Added to bring domain models into scope.
using WorkerRole1.Models;
// Added to bring IQueryable<> into scope.
using System.Linq;
// Added to bring XmlTextReader into scope.
using System.Xml;
// Added to bring WorkerRole into scope.
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WorkerRoleUnitTests.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        FakeAzureDoctorDb db = new FakeAzureDoctorDb();

        [TestInitialize]
        public void FakeDataToFakeDb()
        {
            db.AddSet(TestData.Subscriptions);
            db.AddSet(TestData.CloudServices);
            db.AddSet(TestData.RequestResults);
            db.AddSet(TestData.InstanceStatuses);
            db.AddSet(TestData.Certificates);
        }

        [TestMethod]
        public void ParseResults_Returns_AllInstancesHealthy_True()
        {
            // Arrange
            XmlTextReader reader = new XmlTextReader("../../Fakes/FakeResultsHealthy.xml");

            // Act
            ParseResults pr = new ParseResults();
            RequestResult rr = pr.ParseXmlText(reader);

            // Assert
            Assert.AreEqual(true, rr.AllInstancesHealthy);
        }

        [TestMethod]
        public void ParseResults_Returns_AllInstancesHealthy_False()
        {
            // Arrange
            XmlTextReader reader = new XmlTextReader("../../Fakes/FakeResultsUnhealthy.xml");

            // Act
            ParseResults pr = new ParseResults();
            RequestResult rr = pr.ParseXmlText(reader);

            // Assert
            Assert.AreEqual(false, rr.AllInstancesHealthy);
        }
    }
}
