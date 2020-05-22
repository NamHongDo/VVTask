using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Kid> Kids { get; set; }
        public DbSet<VTask> VTasks { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Kid>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(k => k.Kids)
                .HasForeignKey(a => a.ApplicationUserId);
            modelBuilder.Entity<VTask>()
                .HasOne(k => k.Kid)
                .WithMany(v => v.VTasks);
            modelBuilder.Entity<Reward>()
               .HasOne(k => k.Kid)
               .WithMany(v => v.Rewards);
        }
    }
}
