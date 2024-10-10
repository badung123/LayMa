using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data
{
    public class LayMaContextFactory : IDesignTimeDbContextFactory<LayMaContext>
    {
        public LayMaContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            var builder = new DbContextOptionsBuilder<LayMaContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new LayMaContext(builder.Options);
        }
    }
}
