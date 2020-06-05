using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SnapRead.Infrastructure.Data
{
    public class SnapReadContext : DbContext
    {
        public SnapReadContext(DbContextOptions<SnapReadContext> options) : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
