using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Content
{
    public class VideoContent : Content
    {
        public string VideoUrl { get; set; }
        public int Width { get; set; } = 640;
        public int Height { get; set; } = 360;

        public VideoContent()
        {
            ContentType = "Video";
        }

        public override string Render()
        {
            return $"<iframe width=\"{Width}\" height=\"{Height}\" src=\"{VideoUrl}\" frameborder=\"0\" allowfullscreen></iframe>";
        }
    }
}
