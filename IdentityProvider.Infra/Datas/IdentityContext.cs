using IdentityProvider.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityProvider.Infra.Datas
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext>
                        options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().HasKey(m => m.Id);
            modelBuilder.Entity<User>().HasKey(m => m.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

    

    

    
}
