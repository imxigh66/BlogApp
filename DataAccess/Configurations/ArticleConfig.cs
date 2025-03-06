using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;
using Domain.Models;

namespace DataAccess.Configurations
{
    internal class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            // Связь "один ко многим" - статья принадлежит одному автору
            builder
                .HasOne(a => a.Author)
                .WithMany(u => u.Articles) // У пользователя может быть много статей
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Опционально: Индексы для быстрого поиска
            builder.HasIndex(a => a.CreatedAt);
        }
    }
}
