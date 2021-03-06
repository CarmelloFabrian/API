using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext>options):base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Account>(a => a.NIK);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Profiling)
                .WithOne(p => p.Account)
                .HasForeignKey<Profiling>(p => p.NIK);

            modelBuilder.Entity<Education>()
                .HasMany(ed => ed.Profilings)
                .WithOne(u => u.Education);

            modelBuilder.Entity<University>()
                .HasMany(u => u.Educations)
                .WithOne(ed => ed.University);

            modelBuilder.Entity<AccountRole>()
               .HasKey(a => new { a.AccountId, a.RoleId });

            modelBuilder.Entity<AccountRole>()
                .HasOne(a => a.Account)
                .WithMany(a => a.AccountRoles)
                .HasForeignKey(ar => ar.AccountId);

            modelBuilder.Entity<AccountRole>()
                .HasOne(r => r.Role)
                .WithMany(r => r.AccountRole)
                .HasForeignKey(ar => ar.RoleId);
        }
    }
}
