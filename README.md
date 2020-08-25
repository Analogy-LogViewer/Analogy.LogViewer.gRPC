# Analogy gRPC Receiver    <img src="./Assets/Analogy.gRPC2.png" align="right" width="155px" height="155px">

<p align="center">
    
[![Gitter](https://badges.gitter.im/Analogy-LogViewer/community.svg)](https://gitter.im/Analogy-LogViewer/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge) 
[![Build Status](https://dev.azure.com/Analogy-LogViewer/Analogy%20Log%20Viewer/_apis/build/status/Analogy-LogViewer.Analogy.LogViewer.gRPC?branchName=master)](https://dev.azure.com/Analogy-LogViewer/Analogy%20Log%20Viewer/_build/latest?definitionId=34&branchName=master)  ![.NET Core Desktop](https://github.com/Analogy-LogViewer/Analogy.LogViewer.gRPC/workflows/.NET%20Core%20Desktop/badge.svg)
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


With Analogy Log Service you can have multiple executables sending messages to the log service and have Analogy Log Viewer consume those messages:
![Example](./Assets/Analogy.LogService.gif)


To install Analogy Log Server as windows service use the following command line:
> sc create Analogy.LogServer binpath=full path to Analogy.LogServer.exe file
