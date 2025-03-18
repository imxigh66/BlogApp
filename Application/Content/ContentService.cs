using Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Content
{
    public class ContentService
    {
        public Domain.Models.Content.Content CreateContent(ContentType type, string title, Dictionary<string, string> parameters)
        {
            var factory = ContentFactoryProducer.GetFactory(type);
            return factory.CreateContent(title, parameters);
        }
    }
}
