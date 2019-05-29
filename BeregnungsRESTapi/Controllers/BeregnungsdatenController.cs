using AutoMapper;
using BeregnungsRESTapi.Models;
using BeregnungsRESTapi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeregnungsRESTapi.Controllers
{
    [Route("api/bergenungsdaten")]
    public class BeregnungsdatenController : Controller
    {
        private IBeregnungsdatenRepository _repository;

        public BeregnungsdatenController(IBeregnungsdatenRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IActionResult GetBeregnungsdatens()
        {
            var entitiesFromRepo = _repository.GetBeregnungs();

            return Ok(entitiesFromRepo);
        }

        [HttpGet("{id}", Name = "GetBeregnungsdaten")]
        public IActionResult GetBeregnung(Guid id)
        {
            var beregungFromRepo = _repository.GetBeregnung(id);

            if (beregungFromRepo == null)
            {
                return BadRequest();
            }
            var beregnungsOutput = Mapper.Map<BergnungsDto>(beregungFromRepo);
            return Ok(beregnungsOutput);
        }
    }
}
