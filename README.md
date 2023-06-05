# SqlSanitize
Sample ASP.Net Hosted Blazor WebAssembly Application for sanitizing client input before storing within SQL Server.


Prerequisites:
SQLExpress
.NET 6 SDK

To run, clone the application and launch the ASP.NET hosted Blazor WASM app using the SqlSanitize.Server configuration in either Visual Studio or VSCode.

Notes:

Deployment
Ideally, in a production environment am application such as this would be deployed to a hosting provider such as Azure App Service, allowing for scalable deployment with reliable logging mechanisms. 
In addition, App Service allows overwriting appsettings.json configuration values within memory using secrets from Azure Key Vault (as such, the code in this repository would be modified to load configuration strings from the appsettings.json file). 
This would keep production secrets safe, as the server side appsettings.json file would be written with placeholder values.
CI/CD would be maintained using a provider such as Azure Pipelines to configure deployment of the application, and targeted releases with approvals would authorize deployments to production.
One would also consider locking the server-side endpoints behind a VNET, and enforcing client privileges using login tokens (in a mature application with a user base, that is)



Performance
A couple notes on performance. 



