using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapRead.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Infrastructure.Data.Config
{
    public class ImageOcrTextConfig : IEntityTypeConfiguration<ImageOcrText>
    {
        public void Configure(EntityTypeBuilder<ImageOcrText> builder)
        {
            builder.ToTable("ImageOcrText");

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.OcrText)
                .IsRequired(true)
                .HasMaxLength(1000);

            builder.HasOne(a => a.Image)
            .WithOne(b => b.ImageOcrText)
            .HasForeignKey<ImageOcrText>(b => b.ImageId);
        }
    }
}
