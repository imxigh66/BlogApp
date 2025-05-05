using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stories.Dto
{
    public class StoryFeedDto
    {
        public List<AuthorStoriesGroupDto> AuthorGroups { get; set; } = new List<AuthorStoriesGroupDto>();
    }
}
