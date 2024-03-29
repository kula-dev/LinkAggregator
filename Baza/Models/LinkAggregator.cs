﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baza.Models;

namespace Baza.Models
{
    public class LinkAggregator : DbContext
    {
        public LinkAggregator(DbContextOptions<LinkAggregator> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<Baza.Models.Likes> Likes { get; set; }
    }
}
