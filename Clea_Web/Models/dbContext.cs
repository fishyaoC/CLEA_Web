using Clea_Web.Models;
using Clea_Web.Service;
using Microsoft.EntityFrameworkCore;

namespace Clea_Web.Models
{
    public class dbContext : DbContextCLEA
    {
        private BaseService _service = new BaseService();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_service.ConfigData("ConnectionStrings:DbConnectonString"), sqlServerOptions =>
            {
                sqlServerOptions.CommandTimeout(30);
            });
        }
    }
}
