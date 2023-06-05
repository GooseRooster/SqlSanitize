using SqlSanitize.Server.Persistance;

namespace SqlSanitize.Server.Services
{
    public interface ISanitizer
    {
       Task<string> SanitizeMessage(ILogger logger, string message, SqlSanitizeDbContext context);



    }
}
