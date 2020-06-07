using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapRead.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Infrastructure.Data.Config
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image");

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.FilePath)
                .IsRequired(true)
                .HasMaxLength(100);
        }
    }
}
