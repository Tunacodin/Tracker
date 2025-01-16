using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Context;

namespace Persistence
{
    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TrackerDbContext>
    //{
    //    public TrackerDbContext CreateDbContext(string[] args)
    //    {
    //        DbContextOptionsBuilder<TrackerDbContext> dbContextOptionsBuilder = new();
    //        dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
    //        return new(dbContextOptionsBuilder.Options);
    //    }
    //}
    //static class Configuration
    //{
    //    static public string ConnectionString
    //    {
    //        get
    //        {
    //            ConfigurationManager configurationManager = new();
    //            try
    //            {
    //                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../TrackerAPI"));
    //                configurationManager.AddJsonFile("appsettings.json");
    //            }
    //            catch
    //            {
    //                configurationManager.AddJsonFile("appsettings.Production.json");
    //            }

    //            return configurationManager.GetConnectionString("TrackerDb");
    //        }
    //    }
    //}
}
