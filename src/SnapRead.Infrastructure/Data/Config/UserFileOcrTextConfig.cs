using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapRead.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Infrastructure.Data.Config
{
    public class UserFileOcrTextConfig : IEntityTypeConfiguration<UserFileOcrText>
    {
        public void Configure(EntityTypeBuilder<UserFileOcrText> builder)
        {
            builder.ToTable("UserFileOcrText");

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.OcrText)
                .IsRequired(true)
                .HasMaxLength(1000);

            builder.HasOne(a => a.UserFile)
        .WithOne(b => b.UserFileOcrText)
        .HasForeignKey<UserFileOcrText>(b => b.UserFileId);
        }
    }
}
