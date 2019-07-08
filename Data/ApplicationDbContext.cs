using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cms.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<cms.Models.Person> Persons { get; set; }
        public DbSet<cms.Models.Country> Countries { get; set; }
        public DbSet<cms.Models.City> Cities { get; set; }
    }
}
