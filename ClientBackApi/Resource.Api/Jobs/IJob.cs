using System.Threading.Tasks;

namespace Resource.Api.Jobs
{
    public interface IJob
    {
        Task ExecuteAsync();
    }
}
