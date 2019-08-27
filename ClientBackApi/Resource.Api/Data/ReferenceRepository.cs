using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Resource.Api.Data
{
    public class ReferenceRepository<T> : IReferenceRepository<T> where T : Reference
    {
        private readonly CampContext _context;
        private readonly ILogger<ReferenceRepository<T>> _logger;
        private readonly DbSet<T> _table = null;

        public ReferenceRepository(CampContext context, ILogger<ReferenceRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
            _table = _context.Set<T>();
        }

        public async Task<T[]> GetAllAsync()
        {
            _logger.LogInformation($"Getting all {typeof(T).Name}");

            return await _table.ToArrayAsync<T>();
        }

        public async Task<T> GetAsync(Int16 id)
        {
            _logger.LogInformation($"Getting only one {typeof(T).Name} with id:{id}");

            return await _table.Where(t => t.Id == id).FirstOrDefaultAsync<T>();
        }
    }
}
