using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IArticleExporter
    {
        byte[] Export(Domain.Models.Article article);
        string GetContentType();
        string GetFileExtension();
    }
}
