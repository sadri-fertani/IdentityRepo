using ApiApp;
using ApiApp.Controllers;
using ApiApp.Data;
using ApiApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class PaysControllerUnitTest
    {
        private Mock<IReferenceRepository<Pays>> MockRepository;
        private IMapper Mapper;
        private Mock<IMemoryCache> MockMemoryCache;
        private Mock<ILogger<PaysController>> MockLogger;
        private Mock<IDistributedCache> MockDistributedCache;

        [OneTimeSetUp]
        public void Setup()
        {
            MockRepository = new Mock<IReferenceRepository<Pays>>();
            MockDistributedCache = new Mock<IDistributedCache>();
            MockMemoryCache = new Mock<IMemoryCache>();
            MockLogger = new Mock<ILogger<PaysController>>();

            // Auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CampProfile());
            });

            Mapper = mockMapper.CreateMapper();
        }

        [Test]
        public async Task Get_WhenCalledWithId_ReturnOkResult_PaysAsync()
        {
            // Arrange
            Pays tunisie = new Pays
            {
                Id = 219,
                Code = 788,
                Alpha2 = "TN",
                Alpha3 = "TUN",
                NomEN = "Tunisia",
                NomFR = "Tunisie"
            };

            MockRepository
                .Setup(r => r.GetAsync(tunisie.Id)).Returns(Task.FromResult(tunisie.Clone() as Pays));

            MockDistributedCache
                .Setup(r => r.GetAsync($"{PaysController.DC_PAYS}-{tunisie.Id.ToString()}", CancellationToken.None)).Returns(Task.FromResult(null as byte[]));

            var controller = new PaysController(MockRepository.Object, Mapper, MockMemoryCache.Object, MockDistributedCache.Object, MockLogger.Object);

            // Act
            var result = await controller.GetPays(tunisie.Id);

            // Assert
            var actionResult = result as ActionResult<PaysModel>;
            Assert.IsNotNull(actionResult);
            Assert.IsAssignableFrom(typeof(ActionResult<PaysModel>), actionResult);

            Assert.That(actionResult.Value, Is.EqualTo(Mapper.Map<PaysModel>(tunisie.Clone())));
        }
    }
}

