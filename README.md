# SqlSanitize
Sample ASP.Net Hosted Blazor WebAssembly Application for sanitizing client input before storing within SQL Server.


Prerequisites:

MSSQLExpress, .NET 6 SDK

To run, clone the application and launch the ASP.NET hosted Blazor WASM app using the SqlSanitize.Server configuration in either Visual Studio or VSCode.

Notes:

Deployment

Ideally, in a production environment an application such as this would be deployed to a hosting provider such as Azure App Service, allowing for scalable deployment with reliable logging mechanisms. 
In addition, App Service allows overwriting appsettings.json configuration values within memory using secrets from Azure Key Vault (as such, the code in this repository would be modified to load configuration strings from the appsettings.json file). 
This would keep production secrets safe, as the server side appsettings.json file would be written with placeholder values.
CI/CD would be maintained using a provider such as Azure Pipelines to configure deployment of the application, and targeted releases with approvals would authorize deployments to production.
One would also consider locking the server-side endpoints behind a VNET, and enforcing client privileges using login tokens (in a mature application with a user base, that is)



Performance

A couple notes on performance. As stated in the comments of the sanitizer service:

```
//One could load this entire table into a MemoryCache instance to reduce round trips 
//to the database when large request traffic is experienced.
//However, filtering the matches at this level does off load 
//processing work to the database itself, speeding up execution.
//As SQL keywords are case insensitive, case needs to be evaluated for input, 
//however this has the disadvantage that words used for 
//common communication (like "Specific", or "On") will get flagged, 
//meaning this sanitizer is quite aggressive in practice. 
//Ideally some kind of rule system could be used to evaluate exceptions such as the above.
var messageUpper = message.ToUpper();
var matches = await context.SqlSensitives.Where(x => messageUpper.Contains(x.Filter)).ToListAsync();


//replace each match in the message with * characters
foreach(var match in matches)
{
     logger.LogInformation("Replacing match " + match.Id.ToString());
     //number of characters in the match
     var count = match.Filter.Length;
     //construct the string of * characters
     var sanitized = new string('*', count);

     //replace the match
     message = message.Replace(match.Filter, sanitized, StringComparison.InvariantCultureIgnoreCase);
                    
}
                

return message;

```

In addition, the payload returned by GetAllSensitiveMessages could be enhanced with server side filtering and pagination. EF Core makes this practice rather easy and painless to implement, and this would alleviate server load once the table grows in size.

Completing the Solution

There is a lot that could be done to make this solution more robust, and also more secure. For one, while it is fine for this test sample to prepopulate the SqlSensitive table from the provided text file, in a real world solution this data would be stored somewhere else - for example a NoSQL database like 
CosmosDB, with a UI for admin users to append and edit the flags. The application would then load the most up to date flags using a migration script. 

Also, chat systems are reliant on being, well, live. A REST API cannot provide this aspect of a chat solution reliably, so a socket connection using something like SignalR could allow the blazor client to remain connected to the chat at all times. 

As specified in the comments above, the current rules for the sanitizer are very aggressive. For the purposes of this solution it is perfectly acceptable, however in a real world situation it would be ideal to instead have some kind of rule system in place that could prevent aggressive flagging of common phrases
such as "all" or "on". This is necessary due to SQL itself being case insensitive. 

I would also enhance the application (once it had a proper user base) with more information regarding chat history. At the moment it is no more than a mock up demo, however with the power of Blazor and EFCore, if tech such as SignalR is used, would allow for a very robust chat system to be implemented, with history,
filterable criteria, attachments, etc. 




