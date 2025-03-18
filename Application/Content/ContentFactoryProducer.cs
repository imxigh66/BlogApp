using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Content
{
    public enum ContentType
    {
        Text,
        Image,
        Video
    }

    public class ContentFactoryProducer
    {
        public static IContentFactory GetFactory(ContentType type)
        {
            return type switch
            {
                ContentType.Text => new TextContentFactory(),
                ContentType.Image => new ImageContentFactory(),
                ContentType.Video => new VideoContentFactory(),
                _ => throw new ArgumentException("Неизвестный тип контента", nameof(type))
            };
        }
    }
}
