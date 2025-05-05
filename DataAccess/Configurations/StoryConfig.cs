using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class StoryConfig : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Content)
                .IsRequired();
            builder.Property(s => s.MediaUrl)
        .IsRequired(false);
            builder.Property(s => s.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(s => s.Author)
                .WithMany()
                .HasForeignKey(s => s.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.ExpiresAt);
        }
    }
}
