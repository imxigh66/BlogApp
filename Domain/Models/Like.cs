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
        public string ClientIpAddress { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }

}
