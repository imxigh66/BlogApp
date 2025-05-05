using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stories.Dto
{
    public class AuthorStoriesGroupDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public List<StoryDto> Stories { get; set; } = new List<StoryDto>();
    }
}
