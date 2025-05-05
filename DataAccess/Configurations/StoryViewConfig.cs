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
    public class StoryViewConfig : IEntityTypeConfiguration<StoryView>
    {
        public void Configure(EntityTypeBuilder<StoryView> builder)
        {
            builder.HasKey(v => v.Id);

            builder.HasOne(v => v.Story)
                .WithMany(s => s.Views)
                .HasForeignKey(v => v.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Viewer)
                .WithMany()
                .HasForeignKey(v => v.ViewerId)
                .OnDelete(DeleteBehavior.Restrict); // Избегаем циклических каскадных удалений

            builder.HasIndex(v => new { v.StoryId, v.ViewerId }).IsUnique();
        }
    }
}
