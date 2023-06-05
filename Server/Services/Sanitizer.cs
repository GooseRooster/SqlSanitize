namespace SqlSanitize.Server.Services
{
    public class Sanitizer : ISanitizer
    {


        public async Task<string> SanitizeMessage(ILogger logger, string message)
        {
            try
            {


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
