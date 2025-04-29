using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(byte[] imageData, string filename);
        Task<bool> DeleteAsync(string identifier);
        Task<byte[]> GetAsync(string identifier);
        string GetPublicUrl(string identifier);
    }
}
