using Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Content
{
    public class ImageContentFactory : IContentFactory
    {
        public Domain.Models.Content.Content CreateContent(string title, Dictionary<string, string> parameters)
        {
            parameters.TryGetValue("url", out var imageUrl);
            parameters.TryGetValue("alt", out var altText);

            return new ImageContent
            {
                Title = title,
                ImageUrl = imageUrl ?? string.Empty,
                AltText = altText ?? title
            };
        }
    }
}
