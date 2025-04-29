using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IImageManager
    {
        Task<string> StoreImageAsync(byte[] imageData, string filename, string altText);
        Task<bool> DeleteImageAsync(string imageIdentifier);
        string GetImageUrl(string imageIdentifier);
    }
}
