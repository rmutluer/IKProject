using Human_Resources_Management.ENTITIES;
using Human_Resources_Management.ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.REPOSITORIES.Context
{
    public class HRMDbContext : DbContext
    {
        public HRMDbContext(DbContextOptions<HRMDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=your server;Initial Catalog=databasename ;Persist Security Info=False;User ID=yourid;Password=yourpass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");        }
        public DbSet<Company> Companies{ get; set; }
        public DbSet<Person> Employees { get; set; }
        public DbSet<Package> Packages{ get; set; }
        public DbSet<AdvancePayment> AdvancePayments{ get; set; }
        public DbSet<Expenses> Expenses{ get; set; }
        public DbSet<Vacation> Vacations{ get; set; }

    }
}
