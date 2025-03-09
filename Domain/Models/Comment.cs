﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string Username { get; set; }  
        public string Content { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }

}
