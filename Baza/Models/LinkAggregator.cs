using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Baza.Models
{
    public class LinkAggregator : DbContext
    {
        public LinkAggregator(DbContextOptions<LinkAggregator> options)
            : base(options)
        { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Links> Links { get; set; }
    }
}
