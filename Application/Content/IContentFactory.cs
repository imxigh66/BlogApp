using Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Content
{
    public interface IContentFactory
    {
        Domain.Models.Content.Content CreateContent(string title, Dictionary<string, string> parameters);
    }
}
