using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public class UserInterests
    {
        public List<int> FavoriteAuthors { get; set; } = new List<int>();
        public List<int> FavoriteTags { get; set; } = new List<int>();
    }
}
