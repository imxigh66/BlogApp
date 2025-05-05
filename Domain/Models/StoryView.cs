using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StoryView
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public Story Story { get; set; }
        public int ViewerId { get; set; }
        public User Viewer { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
