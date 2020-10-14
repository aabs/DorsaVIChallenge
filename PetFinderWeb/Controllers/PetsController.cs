using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetFinderWeb
{
    public class PetsController : Controller
    {
        private readonly ILogger<PetsController> _logger;

        public PetsController(ILogger<PetsController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var pf = new PetFinderCore.PetFinder(new PetFinderCore.PetFinderRepositoryClient("https://dorsavicodechallenge.azurewebsites.net/Melbourne"));
            var people = (await pf.GetPeopleAsync()).Select(p => new Models.Person { Age = p.Age, Gender = p.Gender, Name = p.Name });
            return View(people);
        }
    }
}