using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SqlSanitize.Server.Persistance;

namespace SqlSanitize.Server.Services
{
    public class Sanitizer : ISanitizer
    {


        public async Task<string> SanitizeMessage(ILogger logger, string message, SqlSanitizeDbContext context)
        {
            try
            {
                logger.LogInformation("Sanitizing message...");

                //One could load this entire table into a MemoryCache instance to reduce round trips to the database when large request traffic is experienced.
                //However, filtering the matches at this level does off load processing work to the database itself, speeding up execution.
                //As SQL keywords are case insensitive, case needs to be evaluated for input, however this has the disadvantage that words used for common communication (like "Specific", or "On") will get flagged, meaning this sanitizer is quite aggressive in practice. 
                //Ideally some kind of rule system could be used to evaluate exceptions such as the above.
                var messageUpper = message.ToUpper();
                var matches = await context.SqlSensitives.Where(x => messageUpper.Contains(x.Filter)).ToListAsync();

                logger.LogInformation(JsonConvert.SerializeObject(matches)); 

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
            }
            catch(Exception ex)
            {
                logger.LogError("Fatal error sanitizing input! " +  ex.Message + "\n" + ex.StackTrace);
                return string.Empty;
            }
        }
    }
}
