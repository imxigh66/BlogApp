using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Content
{
    public interface IContent
    {
        int Id { get; set; }
        string Title { get; set; }
        DateTime CreatedAt { get; set; }
        string Render();
    }
}
