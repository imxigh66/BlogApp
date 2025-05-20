using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Связь многие-ко-многим со статьями
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
