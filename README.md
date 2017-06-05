# What is AzMan ?
AzMan is a role-based access control (RBAC) framework that provides an administrative tool to manage authorization policy and a runtime that allows applications to perform access checks against that policy. The AzMan administration tool (AzMan.msc) is supplied as a Microsoft Management Console (MMC) snap-in.
Role-based authorization policy specifies access in terms of user roles that reflect an application's authorization requirements. Users are assigned to roles based on their job functions and these roles are granted permissions to perform related tasks.

 What is AzManCofee ?
 --------------------
AzManCofee is the simple library to work with AzMan and have some useful methods to validate user and roles.

# Install via NuGet
To install AzManCofee, run the following command in the Package Manager Console
```code
pm> Install-Package AzManCofee
```
You can also view the [package page](https://www.nuget.org/packages/AzManCofee) on NuGet.


 How to use ?
 -------------
just refrence AzManCofee dll to your project and call methods provided by IAzmanService that implemented in AzManService.
then use following setting in your appSetting on your app config file.

```xml
  <appSettings>
    <add key="AzManConnectionStringName" value="MyAzManConnectionStringName" />
    <add key="AzManAppName" value="MyAppName" />
    <add key="AzManStorageName" value="MyAppStore" />
    <add key="AzManDomainName" value="MyDomain" />
    <add key="AzManBypass" value="true" />
	.
	.
	.

 </appSettings>
```

for example:

```c#
IAzManService azManService= new AzManService();

var result = azManService.UserExists(userName:"adminUser");
```
