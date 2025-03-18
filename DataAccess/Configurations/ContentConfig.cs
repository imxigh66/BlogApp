using Domain.Models.Content;
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
    public class ContentConfig : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.ToTable("Contents");

            builder.HasDiscriminator(c => c.ContentType)
                .HasValue<TextContent>("Text")
                .HasValue<ImageContent>("Image")
                .HasValue<VideoContent>("Video");

            builder.HasKey(c => c.Id);

            // Конфигурация для подклассов
            builder.HasOne<Article>().WithMany(a => a.ContentItems).HasForeignKey("ArticleId");
        }
    }
}
