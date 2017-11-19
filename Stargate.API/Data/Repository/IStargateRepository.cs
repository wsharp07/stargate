using System.Collections.Generic;
using System.Threading.Tasks;
using Stargate.API.Data.Entities;

namespace Stargate.API.Data.Repository
{
    public interface IStargateRepository
    {
        IEnumerable<File> GetFiles();
        File GetFileById(int id);
        File GetFileByFileName(string fileName);
        File GetFileByShortUri(string shortUri);
        Task<int> AddFileAsync(File file);
        void SetShortUri(int id);
    }
}