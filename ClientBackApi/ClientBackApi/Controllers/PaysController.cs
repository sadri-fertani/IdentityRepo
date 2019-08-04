using ApiApp.Data;
using ApiApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        private readonly IMemoryCache _cache;
        private const string MC_PAYS = "MC_PAYS";

        public PaysController(IReferenceRepository<Pays> repository, IMapper mapper, IMemoryCache cache, ILogger<PaysController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
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
                var results = await _repository.GetAsync(id);

                return _mapper.Map<PaysModel>(results);
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
            if (_cache.TryGetValue(MC_PAYS, out Pays[] results))
            {
                _logger.LogError("List of pays loaded from MemoryCache.");
            }
            else
            {
                results = await _repository.GetAllAsync();

                // Save data in cache.
                _cache.Set(
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
