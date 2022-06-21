using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LearningManagementSystem.Domain.Contextes
{
     public class SampleContextFactory : IDesignTimeDbContextFactory<AppDbContext>
     {
         public AppDbContext CreateDbContext(string[] args)
         {
             var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
           
             
             ConfigurationBuilder builder = new ConfigurationBuilder();
             var path = Path.GetFullPath("../../LearningManagementSystem/LearningManagementSystem.API/appsettings.json");
             builder.AddJsonFile(path);
             IConfigurationRoot config = builder.Build();
            

             string connectionString = config.GetConnectionString("DefaultConnection");
             optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
             return new AppDbContext(optionsBuilder.Options);
         }
     }
}
