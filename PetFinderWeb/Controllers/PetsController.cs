﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetFinderCore;

namespace PetFinderWeb
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PetFinderCore.Pet, Models.Pet>();
            CreateMap<PetFinderCore.Person, Models.Person>();
        }
    }

    public class PetsController : Controller
    {
        private readonly ILogger<PetsController> _logger;
        private readonly IEndpointProvider endpointProvider;
        private readonly IMapper mapper;
        private readonly IPetFinder petFinder;

        public PetsController(ILogger<PetsController> logger, IMapper mapper, IPetFinder petFinder, IEndpointProvider endpointProvider)
        {
            _logger = logger;
            this.mapper = mapper;
            this.petFinder = petFinder;
            this.endpointProvider = endpointProvider;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        public async Task<IActionResult> SearchAsync(string name, string location, string petKind)
        {
            var endpointsToQuery = new List<string>();

            if (!string.IsNullOrWhiteSpace(location) && location != "ignore")
            { // if location provided, use it
                endpointsToQuery.Add(location);
            }
            else
            { // otherwise search on every endpoint known
                endpointsToQuery.AddRange(endpointProvider.Endpoints.Keys);
            }

            List<Models.Person> results = new List<Models.Person>();

            foreach (var city in endpointsToQuery)
            {
                PetKind? kind = null;
                if (!string.IsNullOrWhiteSpace(petKind) && petKind != "ignore")
                {
                    kind = (PetKind?)Enum.Parse(typeof(PetKind), petKind);
                }
                results.AddRange((await petFinder.GetPeopleAsync(city, name: name, kind: kind)).Select(p => mapper.Map<Models.Person>(p)));
            }

            return View(InjectHumanReadableCityNames(results));
        }

        internal IEnumerable<Models.Person> InjectHumanReadableCityNames(List<Models.Person> people)
        {
            foreach (var person in people)
            {
                var city = person.Location;
                switch (city)
                {
                    case "mel":
                        city = "Melbourne";
                        break;

                    case "syd":
                        city = "Sydney";
                        break;

                    default:
                        break;
                }
                person.Location = city;
                yield return person;
            }
        }
    }
}