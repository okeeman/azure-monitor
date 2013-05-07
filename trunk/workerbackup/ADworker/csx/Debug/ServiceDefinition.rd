<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ADworker" generation="1" functional="0" release="0" Id="ca23642e-2651-4613-874d-d441d629a471" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ADworkerGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="Certificate|WorkerRole1:AzureCert231212" defaultValue="">
          <maps>
            <mapMoniker name="/ADworker/ADworkerGroup/MapCertificate|WorkerRole1:AzureCert231212" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ADworker/ADworkerGroup/MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ADworker/ADworkerGroup/MapWorkerRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapCertificate|WorkerRole1:AzureCert231212" kind="Identity">
          <certificate>
            <certificateMoniker name="/ADworker/ADworkerGroup/WorkerRole1/AzureCert231212" />
          </certificate>
        </map>
        <map name="MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ADworker/ADworkerGroup/WorkerRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ADworker/ADworkerGroup/WorkerRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WorkerRole1" generation="1" functional="0" release="0" software="G:\Semester 2\Project\ADworker140413\ADworker\csx\Debug\roles\WorkerRole1" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0AzureCert231212" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/ADworker/ADworkerGroup/WorkerRole1/AzureCert231212" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="AzureCert231212" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ADworker/ADworkerGroup/WorkerRole1Instances" />
            <sCSPolicyUpdateDomainMoniker name="/ADworker/ADworkerGroup/WorkerRole1UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ADworker/ADworkerGroup/WorkerRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WorkerRole1UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WorkerRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WorkerRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>