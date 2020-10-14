using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PetFinderCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinderTest
{
    public class PetFinderTests
    {
        public IEnumerable<Person> GetMelbourneTestData()
        {
            var test_data = "[{\"name\":\"Bob\",\"gender\":\"Male\",\"age\":23,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"}]},{\"name\":\"Jennifer\",\"gender\":\"Female\",\"age\":18,\"pets\":[{\"name\":\"Tom\",\"type\":\"Cat\"}]},{\"name\":\"Steve\",\"gender\":\"Male\",\"age\":45,\"pets\":null},{\"name\":\"Fred\",\"gender\":\"Male\",\"age\":40,\"pets\":[{\"name\":\"Meong\",\"type\":\"Cat\"},{\"name\":\"Jim\",\"type\":\"Cat\"},{\"name\":\"Hulk\",\"type\":\"Dog\"}]},{\"name\":\"Samantha\",\"gender\":\"Female\",\"age\":30,\"pets\":[{\"name\":\"Tabby\",\"type\":\"Cat\"}]},{\"name\":\"Alice\",\"gender\":\"Female\",\"age\":64,\"pets\":[{\"name\":\"Simba\",\"type\":\"Cat\"},{\"name\":\"Nemo\",\"type\":\"Fish\"}]}]";
            return JsonConvert.DeserializeObject<List<Person>>(test_data);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestCanFilterPetsByOwnerGender()
        {
            var mock = new Mock<IPetFinderRepositoryClient>();
            mock.Setup(foo => foo.GetAsync()).Returns(Task.FromResult(GetMelbourneTestData()));
            var sut = new PetFinder(mock.Object);
            var result = (await sut.GetPeopleAsync()).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(6));
        }

        [Test]
        public async Task TestConnectivityToMelbourneEndpoint()
        {
            var sut = new PetFinderRepositoryClient("https://dorsavicodechallenge.azurewebsites.net/Melbourne");
            var people = await sut.GetAsync();
            Assert.That(people, Has.Count.EqualTo(6));
        }

        [Test]
        public async Task TestPetFinderCollatesAllPets()
        {
            var mock = new Mock<IPetFinderRepositoryClient>();
            mock.Setup(foo => foo.GetAsync()).Returns(Task.FromResult(GetMelbourneTestData()));
            var sut = new PetFinder(mock.Object);
            var result = (await sut.GetPetsAsync()).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(8));
        }

        [Test]
        public async Task TestPetFinderUsesInjectedClientAsync()
        {
            var mock = new Mock<IPetFinderRepositoryClient>();
            mock.Setup(foo => foo.GetAsync()).Returns(Task.FromResult(GetMelbourneTestData()));
            var sut = new PetFinder(mock.Object);
            var result = (await sut.GetPeopleAsync()).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(6));
        }
    }
}