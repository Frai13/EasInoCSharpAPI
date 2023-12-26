# EasIno CSharp API
[EasIno](https://github.com/Frai13/EasIno) is an Arduino Library that makes easier the communication between Arduino and other devices. The purpose of this API is to provide an easy way to use the EasIno protocol using C# with any of these frameworks:
+ **.NET Framework:** even without receiveing updates, .NET Framework is still being used by many people that are learning C# or are using Windows Forms. This API was created with the objective of give these users the possibility to include EasIno in their applications.
+ **.NET Core:** for latest version of .NET.

## API
Available frameworks:
+ [.NET Framework](EasInoNETFramework/EasInoFWAPI).
+ [.NET Core](EasInoNETCore/EasInoNetCoreAPI).

## Command Line Input
A command line input (CLI) program has been developed to ease the user to interact with EasIno boards without having to create an application.
To see available commands type:
> EasInoCLI.exe --help

Available frameworks:
+ [.NET Framework](EasInoNETFramework/EasInoFWCLI).
+ [.NET Core](EasInoNETCore/EasInoNetCoreCLI).

## Examples
Examples can be found at:
+ [.NET Framework](EasInoNETFramework/EasInoFWExamples).
+ [.NET Core](EasInoNETCore/EasInoNetCoreExamples).

### Send and monitor Form
This example allows the user to communicate with an EasIno board using an UI.

![Configuration](Documentation/SendAndMonitor_configuration.png?raw=true "Configuration")

![Send and receive](Documentation/SendAndMonitor_send_and_receive.png?raw=true "Send and receive")