<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureDoctor.Azure" generation="1" functional="0" release="0" Id="b980b80a-0167-44e8-a46a-69b2c0408470" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureDoctor.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AzureDoctor:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/LB:AzureDoctor:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="AzureDoctor:Endpoint2" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/LB:AzureDoctor:Endpoint2" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="AzureDoctor:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/MapAzureDoctor:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AzureDoctorInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/MapAzureDoctorInstances" />
          </maps>
        </aCS>
        <aCS name="Certificate|AzureDoctor:AzureCert231212" defaultValue="">
          <maps>
            <mapMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/MapCertificate|AzureDoctor:AzureCert231212" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AzureDoctor:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:AzureDoctor:Endpoint2">
          <toPorts>
            <inPortMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/Endpoint2" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAzureDoctor:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAzureDoctorInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctorInstances" />
          </setting>
        </map>
        <map name="MapCertificate|AzureDoctor:AzureCert231212" kind="Identity">
          <certificate>
            <certificateMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/AzureCert231212" />
          </certificate>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AzureDoctor" generation="1" functional="0" release="0" software="G:\Semester 2\Project\AzureDoctor130513\AzureDoctor.Azure\csx\Debug\roles\AzureDoctor" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Endpoint2" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/AzureCert231212" />
                </certificate>
              </inPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AzureDoctor&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AzureDoctor&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Endpoint2&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0AzureCert231212" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/AzureCert231212" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="AzureCert231212" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctorInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctorUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctorFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="AzureDoctorUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="AzureDoctorFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AzureDoctorInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="5838dc93-2df6-429d-8b4e-28531883e911" ref="Microsoft.RedDog.Contract\ServiceContract\AzureDoctor.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="4f49bd0b-dd9b-451b-a573-40852be397d5" ref="Microsoft.RedDog.Contract\Interface\AzureDoctor:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="171b2433-a473-406a-9e83-ef9b19c83b45" ref="Microsoft.RedDog.Contract\Interface\AzureDoctor:Endpoint2@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor:Endpoint2" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>