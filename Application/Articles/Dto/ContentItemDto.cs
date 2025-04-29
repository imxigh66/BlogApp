using Application.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Dto
{
    public class ContentItemDto
    {
        public ContentType ContentType { get; set; }
        public string Title { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public int Id { get; set; }

       
       

        // Поля для разных типов контента
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public string Body { get; set; }
        public string VideoUrl { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        
    }
}
