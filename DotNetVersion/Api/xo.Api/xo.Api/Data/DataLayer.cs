using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using xo.Api.Entities;

namespace xo.Api.Data
{
    public class DataLayer : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set;}

        IConfiguration _Configuration;
        public DataLayer(IConfiguration configuration)
        {
            this._Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_Configuration.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
