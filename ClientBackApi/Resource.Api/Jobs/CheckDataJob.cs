using Resource.Api.Data;
using Resource.Api.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Resource.Api.Services;
using System.Threading.Tasks;

namespace Resource.Api.Jobs
{
    public class CheckDataJob : IJob
    {
        private readonly IReferenceRepository<Pays> _referenceRepository;
        private readonly ILogger<CheckDataJob> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public CheckDataJob(IReferenceRepository<Pays> referenceRepository, ILogger<CheckDataJob> logger, IEmailSender emailSender, IConfiguration configuration)
        {
            _referenceRepository = referenceRepository;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [LogEverything]
        public async Task ExecuteAsync()
        {
            _logger.LogInformation($"Job started");

            var tunisie = await _referenceRepository.GetAsync(219);

            // TODO ...

            _logger.LogInformation($"Sending eMail...");
            await _emailSender.SendEmail(
                _configuration["Jobs:CheckData:DestinationEmail"],
                _configuration["Jobs:CheckData:DestinationUserName"],
                _configuration["Jobs:CheckData:SubjectEmail"], 
                "Message test...");

            _logger.LogInformation($"Job ended");
        }
    }
}
