using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using toolfox.Models;
using System.Data.Common;
using System.Data.Sql;

namespace RestApis.Data;

// Generic repository pattern -- single set of methods is created to handle CRUD. Laravel's eloquent uses this.
public class AppDbContext: DbContext
{
  protected readonly IConfiguration Configuration;

  public AppDbContext(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
  {
    // For SQLite
    options.UseSqlite(Configuration.GetConnectionString("ToolfoxDataContext"));

    // For SQL Servers
    // options.UseSqlServer(Configuration.GetConnectionString("ToolfoxDataContext"));
  }

  // Place must be a table that exists in the DB
  public DbSet<PlaceModel> Place {get; set;}
}