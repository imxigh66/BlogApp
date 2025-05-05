using DataAccess.Configurations;
using Domain.Models;
using Domain.Models.Content;
using Domain.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BlogDbContext:DbContext
    {
        public BlogDbContext(DbContextOptions opt):base(opt) 
        {
            
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryView> StoryViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
        }


    }
}
