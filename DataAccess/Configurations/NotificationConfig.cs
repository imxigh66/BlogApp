using Domain.Models.Notifications;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasDiscriminator(n => n.NotificationType)
                .HasValue<ArticlePublishedNotification>("ArticlePublished")
                .HasValue<NewCommentNotification>("NewComment");

            // Другие конфигурации...
        }
    }
}
