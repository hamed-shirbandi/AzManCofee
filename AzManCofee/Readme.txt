

use following setting on your app config file 

  <connectionStrings>
    <add name="MyAzManConnectionStringName" connectionString="Data Source=Server;Initial Catalog=NetSqlAzmanStorage;User ID=sa;Password=1234" providerName="System.Data.SqlClient" />
    .
	.
	.

  </connectionStrings>



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

 for more info about AzManCofee visit this page https://github.com/hamed-shirbandi/AzManCofee