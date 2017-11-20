using System.Collections.Generic;
using System.Threading.Tasks;
using Stargate.API.Data.Entities;

namespace Stargate.API.Data.Repository
{
    public interface IStargateRepository
    {
        IEnumerable<File> GetFiles();
        Task<File> GetFileByIdAsync (int id);
        Task<File> GetFileByFileNameAsync (string fileName);
        Task<File> GetFileByShortUriAsync (string shortUri);
        Task<int> AddFileAsync(File file);
    }
}