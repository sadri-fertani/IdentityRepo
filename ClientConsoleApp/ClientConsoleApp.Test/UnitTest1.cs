using NUnit.Framework;
using ClientConsoleApp;
using System.Threading.Tasks;
using ClientConsoleApp.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1_EmptyArgument_ThrowException()
        {
            Assert.That(async () => await Program.CallWebApi(string.Empty), Throws.ArgumentException);
        }

        [Test]
        public void Test2_InvalidArgument_ThrowException()
        {
            Assert.That(async () => await Program.CallWebApi("..."), Throws.ArgumentException);
        }

        [Test]
        public async Task Test3_ValidArgument_PaysInfo()
        {
            // Arrange
            var tunisia = new PaysModel()
            {
                Id = 219,
                Code = 788,
                Alpha2 = "TN",
                Alpha3 = "TUN",
                NomFR = "Tunisie",
                NomEN = "Tunisia"
            };

            // Act
            var result = await Program.CallWebApi("pays");
            var resultPays = (result.Result as JObject).ToObject(typeof(PaysModel));

            // Assert
            Assert.That(resultPays, Is.EqualTo(tunisia));
        }

        [Test]
        public async Task Test4_ValidArgument_CampsList()
        {
            // Act
            var result = await Program.CallWebApi("camps");
            var resultCamps = (result.Result as JArray).ToObject(typeof(List<CampModel>));

            // Assert
            Assert.That(resultCamps, Is.Not.Empty);
        }
    }
}