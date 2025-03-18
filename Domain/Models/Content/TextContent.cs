using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Content
{
    

    public class TextContent : Content
    {
        public string Body { get; set; }

        public TextContent()
        {
            ContentType = "Text";
        }

        public override string Render()
        {
            return Body;
        }
    }
}
