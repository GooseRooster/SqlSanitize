using SqlSanitize.Shared;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace SqlSanitize.Server.Persistance
{
    public class SqlSanitizeDbContext : DbContext
    {

        ILogger _logger;


        public SqlSanitizeDbContext(ILogger<SqlSanitizeDbContext> logger, DbContextOptions<SqlSanitizeDbContext> options)
        : base(options)
        {
            _logger = logger;
        }



        public DbSet<SensitiveMessage> SensitiveMessages { get; set; }
        public DbSet<SqlSensitive> SqlSensitives { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=SqlSanitize;Trusted_Connection=True;Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SqlSensitive>()
                .HasKey(x => x.Id);
  

            modelBuilder.Entity<SqlSensitive>().Property(x => x.Filter).IsRequired();

            modelBuilder.Entity<SensitiveMessage>().HasKey(x => x.Id);
            modelBuilder.Entity<SensitiveMessage>().Property(x => x.Message).IsRequired();


            string sensitiveText = string.Empty;
            List<string> sensitiveWords = new();
            try
            {
                //load up the list of sensitive phrases for seeding
                sensitiveText = File.ReadAllText("sql_sensitive_list.txt");

                //parse as json string and load into a list of strings
                sensitiveWords = JsonConvert.DeserializeObject<List<string>>(sensitiveText);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fatal error reading data for SqlSensitive table!");
         
            }


            if (string.IsNullOrEmpty(sensitiveText) || sensitiveWords == null || !sensitiveWords.Any())
            {
                _logger.LogError("Could not find data to load for SqlSensitive table!");
                return;

            }

            //create a list of SqlSensitive objects
            List<SqlSensitive> filtersToSave = new();
            foreach (var word in sensitiveWords)
            {
                //if somehow the word or phrase is null or empty, skip it
                if (string.IsNullOrEmpty(word))
                    continue;

                //instantiate a SqlSensitive object with Id
                SqlSensitive toAdd = new();
                toAdd.Filter = word;

                filtersToSave.Add(toAdd);
                

            }

            //populate the SqlSensitive table
            modelBuilder.Entity<SqlSensitive>().HasData(filtersToSave);



        }



    }
}
