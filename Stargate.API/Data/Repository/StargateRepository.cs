using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stargate.API.Data.Entities;
using Stargate.API.Services;

namespace Stargate.API.Data.Repository
{
    public class StargateRepository : IStargateRepository
    {
        private readonly StargateContext _context;
        public StargateRepository(StargateContext context)
        {
            _context = context;
        }

        public IEnumerable<File> GetFiles()
        {
            return _context.Files;
        }

        public Task<File> GetFileByIdAsync (int id)
        {
            return _context.Files.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<File> GetFileByFileNameAsync (string fileName)
        {
            return _context.Files.SingleOrDefaultAsync(x => x.FileName  == fileName);
        }

        /// <summary>
        /// Add's a file to the database if does not exist. 
        /// If it already exists this will return to you the existing file Id
        /// </summary>
        /// <param name="file">File Entity</param>
        /// <returns></returns>
        public async Task<int> AddFileAsync(File file)
        {
            var existingFile = await GetFileByFileNameAsync(file.FileName);

            if (existingFile != null)
                return existingFile.Id;

            file.CreatedAtUtc = DateTime.UtcNow;

            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
            return file.Id;
        }
    }
}