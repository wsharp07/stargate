using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stargate.API.Data.Entities;
using Stargate.API.Services;

namespace Stargate.API.Data.Repository
{
    public class StargateRepository : IStargateRepository
    {
        private readonly IUriShortener _shortener;
        private readonly StargateContext _context;
        public StargateRepository(StargateContext context, IUriShortener shortener)
        {
            _context = context;
            _shortener = shortener;
        }

        public IEnumerable<File> GetFiles()
        {
            return _context.Files;
        }

        public File GetFileById(int id)
        {
            return _context.Files.SingleOrDefault(x => x.Id == id);
        }

        public File GetFileByFileName(string fileName)
        {
            return _context.Files.SingleOrDefault(x => x.FileName  == fileName);
        }

        public File GetFileByShortUri(string shortUri)
        {
            return _context.Files.SingleOrDefault(x => x.ShortUri == shortUri);
        }

        public async Task<int> AddFileAsync(File file)
        {
            file.CreatedAtUtc = DateTime.UtcNow;

            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
            return file.Id;
        }

        public void SetShortUri(int id)
        {
            var file = GetFileById(id);
            file.ShortUri = _shortener.GetShortUri(id);
            _context.SaveChanges();
        }
    }
}