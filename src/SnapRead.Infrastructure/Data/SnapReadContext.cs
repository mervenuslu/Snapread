using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnapRead.Core.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SnapRead.Infrastructure.Data
{
    public class SnapReadContext : IdentityDbContext<ApplicationUser>
    {
        public SnapReadContext(DbContextOptions<SnapReadContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<ImageOcrText> ImageOcrTexts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
