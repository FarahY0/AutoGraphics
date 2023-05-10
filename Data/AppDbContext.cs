using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoGraphic.Models.ViewModels;

namespace AutoGraphic.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {
      



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
       

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ApplicationUser>()
        //        .HasOne<Department>(u => u.Department)
        //        .WithOne(d => d.ApplicationUsers)
        //        .HasForeignKey<Department>(d => d.ApplicationUserId)
        //        .IsRequired(false);
        //} 

    }

}

