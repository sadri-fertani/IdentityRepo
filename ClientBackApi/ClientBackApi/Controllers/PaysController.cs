using ApiApp.Data;
using ApiApp.Models;
using AutoMapper;
using ClientBackApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PaysController : ControllerBase
    {
        private readonly IReferenceRepository<Pays> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaysController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private const string MC_PAYS = "MC_PAYS";

        public PaysController(
            IReferenceRepository<Pays> repository, 
            IMapper mapper, 
            IMemoryCache memoryCache, 
            IDistributedCache distributedCache,
            ILogger<PaysController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<ActionResult<PaysModel[]>> GetAllPays()
        {
            try
            {
                Pays[] results = await LoadPays();

                return _mapper.Map<PaysModel[]>(results);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaysModel>> GetPays(Int16 id)
        {
            try
            {
                Pays result = (await _distributedCache.GetAsync($"{MC_PAYS}-{id.ToString()}")).ToObject<Pays>();

                // Look for cache key.
                if (result == null)
                {
                    result = await _repository.GetAsync(id);
                    _logger.LogError($"Pays {result.NomEN} loaded from Database.");

                    // Save data in cache.
                    _distributedCache.Set(
                        $"{MC_PAYS}-{id.ToString()}",
                        result.ToByteArray<Pays>(),
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                            SlidingExpiration = TimeSpan.FromMinutes(10.0)
                        }
                    );

                    _logger.LogError($"Pays {result.NomEN} saved into DistributedCache.");
                }
                else
                {
                    _logger.LogError($"Pays {result.NomEN} loaded from DistributedCache.");
                }

                return _mapper.Map<PaysModel>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        /// <summary>
        /// Load list of Pays
        /// </summary>
        /// <returns>list of Pays</returns>
        private async Task<Pays[]> LoadPays()
        {
            // Look for cache key.
            if (_memoryCache.TryGetValue(MC_PAYS, out Pays[] results))
            {
                _logger.LogError("List of pays loaded from MemoryCache.");
            }
            else
            {
                results = await _repository.GetAllAsync();
                _logger.LogError("List of pays loaded from Database.");

                // Save data in cache.
                _memoryCache.Set(
                    MC_PAYS,
                    results,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                        Priority = CacheItemPriority.Normal
                    }
                );

                _logger.LogError("List of pays saved into MemoryCache.");
            }

            return results;
        }
    }
}
