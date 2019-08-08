using System.Threading.Tasks;

namespace ClientBackApi.Jobs
{
    public interface IJob
    {
        Task ExecuteAsync();
    }
}
