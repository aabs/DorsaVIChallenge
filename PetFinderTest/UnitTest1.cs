using FluentAssertions;
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
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel")).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(6));
        }

        [Test]
        public async Task TestCanGetByAgePeople()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel", age: 18)).ToList();

            // confirm that the pet finder used the client injected.
            result.Should().HaveCount(1);
            result.ElementAt(0).Name.Should().Be("Jennifer");
        }

        [Test]
        public async Task TestCanGetOnlyFemalePeople()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel", gender: Gender.Female)).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task TestCanGetOnlyMalePeople()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel", gender: Gender.Female)).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task TestCanRetrievePeopleByNameAsync()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel", name: "Bob")).Single();

            // confirm that the pet finder used the client injected.
            result.Should().NotBeNull();
            result.Name.Should().Be("Bob");
            result.Age.Should().Be(23);
        }

        [Test]
        public async Task TestConnectivityToMelbourneEndpoint()
        {
            var ep = BuildEndpointProvider();
            var sut = new PetFinderRepositoryClient(ep);
            var people = await sut.GetAsync("mel");
            Assert.That(people, Has.Count.EqualTo(6));
        }

        [Test]
        public async Task TestGetNoIndeterminatePeople()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel", gender: Gender.DeclinedToSay)).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task TestPetFinderCollatesAllPets()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPetsAsync("mel")).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(8));
        }

        [Test]
        public async Task TestPetFinderUsesInjectedClientAsync()
        {
            PetFinder sut = CreateMockPetFinder();
            var result = (await sut.GetPeopleAsync("mel")).ToList();

            // confirm that the pet finder used the client injected.
            Assert.That(result, Has.Count.EqualTo(6));
        }

        private EndpointProvider BuildEndpointProvider()
        {
            var ep = new EndpointProvider();
            ep.AddEndpoint("mel", "https://dorsavicodechallenge.azurewebsites.net/Melbourne");
            ep.AddEndpoint("syd", "https://dorsavicodechallenge.azurewebsites.net/Sydney");
            return ep;
        }

        private PetFinder CreateMockPetFinder()
        {
            var mock = new Mock<IPetFinderRepositoryClient>();
            mock.Setup(foo => foo.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(GetMelbourneTestData()));
            var sut = new PetFinder(mock.Object);
            return sut;
        }
    }
}