namespace SqlSanitize.Server.Helpers
{
    public static class Sanitizer
    {
        public static async Task<string> SanitizeMessage(ILogger logger, string message)
        {

            try
            {



                return string.Empty;
            }
            catch(Exception ex)
            {
                logger.LogError("Fatal error occurred processing sanitization request! " + ex.Message + "\n" +  ex.StackTrace);
                return string.Empty;
            }



            
        }




    }
}
