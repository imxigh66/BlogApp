using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string UserIdentifier { get; set; } // Храним ID пользователя или GuestID
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }

}
