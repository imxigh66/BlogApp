using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Content
{
    public class ImageContent : Content
    {
        public string ImageUrl { get; set; }
        public string AltText { get; set; }

        public ImageContent()
        {
            ContentType = "Image";
        }

        public override string Render()
        {
            return $"<img src=\"{ImageUrl}\" alt=\"{AltText}\" />";
        }
    }
}
