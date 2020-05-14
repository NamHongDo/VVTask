using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
        public DbSet<VTask> VTasks { get; set; }
        public DbSet<KidProfile> Profiles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
/*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VTask>()
                .HasOne(p => p.KidProfile)
                .WithMany(b => b.VTasks);
        }
        */
    }
}
