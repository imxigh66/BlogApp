using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IStoryMediaService
    {
        Task<string> UploadMediaAsync(byte[] mediaData, string fileName);
        Task<bool> DeleteMediaAsync(string mediaUrl);
    }
}
