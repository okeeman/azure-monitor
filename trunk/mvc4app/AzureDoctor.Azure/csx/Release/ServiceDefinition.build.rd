﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureDoctor.Azure" generation="1" functional="0" release="0" Id="65adc5a0-89a5-43e9-a854-d7f4c7bc2b5f" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureDoctor.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AzureDoctor:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/LB:AzureDoctor:Endpoint1" />
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
        <aCS name="Certificate|AzureDoctor:Azpad" defaultValue="">
          <maps>
            <mapMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/MapCertificate|AzureDoctor:Azpad" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AzureDoctor:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/Endpoint1" />
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
        <map name="MapCertificate|AzureDoctor:Azpad" kind="Identity">
          <certificate>
            <certificateMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/Azpad" />
          </certificate>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AzureDoctor" generation="1" functional="0" release="0" software="F:\Semester 2\Project\AzureDoctor\AzureDoctor.Azure\csx\Release\roles\AzureDoctor" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AzureDoctor&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AzureDoctor&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Azpad" certificateStore="My" certificateLocation="User">
                <certificate>
                  <certificateMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor/Azpad" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Azpad" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctorInstances" />
            <sCSPolicyFaultDomainMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctorFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="AzureDoctorFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AzureDoctorInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="335f647a-e4e0-47b3-8588-865c5488711b" ref="Microsoft.RedDog.Contract\ServiceContract\AzureDoctor.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="7df92acc-4087-4030-a67a-40230772b457" ref="Microsoft.RedDog.Contract\Interface\AzureDoctor:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/AzureDoctor.Azure/AzureDoctor.AzureGroup/AzureDoctor:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>