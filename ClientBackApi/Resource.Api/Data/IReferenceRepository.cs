using System;
using System.Threading.Tasks;

namespace Resource.Api.Data
{
    public interface IReferenceRepository<T> where T : Reference
    {
        Task<T[]> GetAllAsync();
        Task<T> GetAsync(Int16 id);
    }
}
