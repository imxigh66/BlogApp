using Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Content
{
    public class TextContentFactory : IContentFactory
    {
        public Domain.Models.Content.Content  CreateContent(string title, Dictionary<string, string> parameters)
        {
            parameters.TryGetValue("body", out var body);

            return new TextContent
            {
                Title = title,
                Body = body ?? string.Empty
            };
        }
    }
}
