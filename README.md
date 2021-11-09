# Analogy gRPC Receiver    <img src="./Assets/Analogy.gRPC2.png" align="right" width="155px" height="155px">

<p align="center">
    
![.NET Core Desktop](https://github.com/Analogy-LogViewer/Analogy.LogViewer.gRPC/workflows/.NET%20Core%20Desktop/badge.svg)
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=Analogy-LogViewer/Analogy.LogViewer.gRPC)](https://dependabot.com)
<a href="https://github.com/Analogy-LogViewer/Analogy.LogViewer.gRPC/issues">
    <img src="https://img.shields.io/github/issues/Analogy-LogViewer/Analogy.LogViewer.gRPC"  alt="Issues" />
</a>
![GitHub closed issues](https://img.shields.io/github/issues-closed-raw/Analogy-LogViewer/Analogy.LogViewer.gRPC)
<a href="https://github.com/Analogy-LogViewer/Analogy.LogViewer.gRPC/blob/master/LICENSE.md">
    <img src="https://img.shields.io/github/license/Analogy-LogViewer/Analogy.LogViewer.gRPC"  alt="License" />
</a>
</p>

gRPC data provider for Analogy Log Viewer

The following modules exists:
| Nuget   |      Version      |  Description |
|----------|:-------------:|------|
| [Analogy.LogViewer.gRPC](https://www.nuget.org/packages/Analogy.LogViewer.gRPC/) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogViewer.gRPC)](https://www.nuget.org/packages/Analogy.LogViewer.gRPC/) | Primary Analogy Log Viewer grRPC providers |
| [Analogy.LogServer](https://www.nuget.org/packages/Analogy.LogServer/) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogServer)](https://www.nuget.org/packages/Analogy.LogServer/) | A windows Service for receiving logs |
| [Analogy.LogServer.Clients](https://www.nuget.org/packages/Analogy.LogServer.Clients/) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogServer.Clients)](https://www.nuget.org/packages/Analogy.LogServer.Clients) | gRPC client to pull back messages from Analogy Service |
| [Analogy.AspNetCore.LogProvider](https://www.nuget.org/packages/Analogy.AspNetCore.LogProvider/) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.AspNetCore.LogProvider)](https://www.nuget.org/packages/Analogy.AspNetCore.LogProvider) | AspNetCore Logger provider |
| [NLog Target](https://github.com/Analogy-LogViewer/Analogy.LogViewer.NLog.Targets) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogViewer.NLog.Targets)](https://www.nuget.org/packages/Analogy.LogViewer.NLog.Targets) | NLog target to stream logs to Analogy |
| [Serilog Sink](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog.Sinks) |   [![Nuget](https://img.shields.io/nuget/v/Analogy.LogViewer.Serilog.Sinks)](https://www.nuget.org/packages/Analogy.LogViewer.Serilog.Sinks) | Serilog Sink for sending logs to Analogy |

With Analogy Log Service you can have multiple executables sending messages to the log service and have Analogy Log Viewer consume those messages:
![Example](./Assets/Analogy.LogService.gif)


To install Analogy Log Server as windows service use the following command line:
> sc create Analogy.LogServer binpath=full path to Analogy.LogServer.exe file


## Usage

Once you have setup Analogy Log Server you can start sending messages to it:


- NLog Target: 
Add nuget [Analogy.LogViewer.NLog.Targets](https://www.nuget.org/packages/Analogy.LogViewer.NLog.Targets/)
 In your nlog.config add:
 ```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="warn">

  <extensions>
    <add assembly="NLog.Web.AspNetCore"/>
    <add assembly="Analogy.LogViewer.NLog.Targets"/>
  </extensions>
 
  <targets>
    <!-- write logs to file  -->
    <target xsi:type="File" name="allfile"
        fileName="c:\KALPA\logs\${processname:fullName=false}-KX.nlog"
        layout="${longdate}|${uppercase:${level}}|${logger}|${message}${exception:format=tostring}|${processname:fullName=false}|${processid}"
        keepFileOpen="false"
        archiveFileName="c:\KALPA\logs\${processname:fullName=false}-KX-${shortdate}.{##}.nlog"
        archiveNumbering="Sequence"
        archiveEvery="Day"
        maxArchiveFiles="100"
        archiveAboveSize="30000000">
    </target>
    <!-- write logs to Analogy Log Server  -->
    <target xsi:type="NlogAnalogyGRPCTarget" name="NLogToAnalogyGRPCTarget"
            layout="${longdate}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|${processname:fullName=false}|${processid}">
      <contextproperty name="MachineName" layout="${machinename}" />
      <contextproperty name="ThreadId" layout="${threadid}"/>
      <contextproperty name="ProcessId" layout="${processid}" />
      <contextproperty name="callsite" layout="   ${callsite:className=true:fileName=true:includeSourcePath=true:methodName=true}" />
      <contextproperty name="ProcessName" layout="${processname:fullName=false}" />
    </target>
  </targets>

  <rules>
    <logger name="*" minlevel="Trace" writeTo="allfile" />
    <logger name="*" minlevel="Trace" writeTo="NLogToAnalogyGRPCTarget" />
  </rules>
</nlog>
```
 
-  Serilog Sink: 
Add nuget [Analogy.LogViewer.Serilog.Sinks](https://www.nuget.org/packages/Analogy.LogViewer.Serilog.Sinks//)


- Analogy.AspNetCore.LogProvider:
Add Nuget package [Analogy.AspNetCore.LogProvider](https://www.nuget.org/packages/Analogy.AspNetCore.LogProvider/) and then add to the Configure method (in  Startup.cs) the following: 

```csharp

 public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
 {
     loggerFactory.AddAnalogyLogger(new AnalogyLoggerConfiguration
     {
         LogLevel = LogLevel.Trace,
         EventId = 0,
         AnalogyServerUrl = "http://localhost:6000"
      });
     }

```

- Python logging: go to [Python Logging](https://github.com/Analogy-LogViewer/Analogy-Python-Logging/) for more information.

