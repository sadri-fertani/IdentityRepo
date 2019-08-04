using ApiApp.Data;
using System;
using System.Threading.Tasks;

namespace ApiApp
{
    public interface IReferenceRepository<T> where T : Reference
    {
        Task<T[]> GetAllAsync();
        Task<T> GetAsync(Int16 id);
    }
}
