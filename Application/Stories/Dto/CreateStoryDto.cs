using Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stories.Dto
{
    public class CreateStoryDto
    {
        public string Content { get; set; }
        public IFormFile Media { get; set; }
        public StoryType Type { get; set; }
    }
}
