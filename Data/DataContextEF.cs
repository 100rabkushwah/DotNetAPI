using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DotnetAPI.Data;
public class DataContextEF : DbContext
{
    private readonly IConfiguration _config;
    public DataContextEF(IConfiguration config)
    {
        _config = config;
    }

    //here User is our Class of Model and Users is table in in our database
    public virtual DbSet<User> Users {get; set;}
    public virtual DbSet<UserSalary> UserSalary {get; set;}
    public virtual DbSet<UserJobInfo> UserJobInfo {get; set;}

    //this is where we are goimg to get our connectionstring
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            //dotnet add package Microsoft.EntityFrameWorkCore.SqlServer if UseSqlServer give error
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                            optionsBuilder=>optionsBuilder.EnableRetryOnFailure());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.HasDefaultSchema("TutorialAppSchema");

        //Here we are telling to out entityframe work thar User is Users table in our data base
        modelBuilder.Entity<User>()
            .ToTable("Users","TutorialAppSchema")
            .HasKey(u => u.UserId);
        
         modelBuilder.Entity<UserSalary>()
             .HasKey(u => u.UserId);

         modelBuilder.Entity<UserJobInfo>()
            .HasKey(u => u.UserId);
    }
}