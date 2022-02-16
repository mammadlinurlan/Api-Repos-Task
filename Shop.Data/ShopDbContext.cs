﻿using Microsoft.EntityFrameworkCore;
using Shop.Core.Entities;
using Shop.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
           

            base.OnModelCreating(modelBuilder);
        }
    }
}
