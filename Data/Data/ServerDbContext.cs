﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Data
{
    public class ServerDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TaxPayer> TaxPayers { get; set; }
        public DbSet<Cities> _Cities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"data source=DESKTOP-7R8OIVU\\SQLEXPRESS01;initial catalog=TaxPayers3;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}

