using Domain.Models;
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
    }
}
