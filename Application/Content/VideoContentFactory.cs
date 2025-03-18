using Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Content
{
    public class VideoContentFactory : IContentFactory
    {
        public Domain.Models.Content.Content CreateContent(string title, Dictionary<string, string> parameters)
        {
            parameters.TryGetValue("url", out var videoUrl);

            int width = 640;
            int height = 360;

            if (parameters.TryGetValue("width", out var widthStr) && int.TryParse(widthStr, out var parsedWidth))
            {
                width = parsedWidth;
            }

            if (parameters.TryGetValue("height", out var heightStr) && int.TryParse(heightStr, out var parsedHeight))
            {
                height = parsedHeight;
            }

            return new VideoContent
            {
                Title = title,
                VideoUrl = videoUrl ?? string.Empty,
                Width = width,
                Height = height
            };
        }
    }
}
