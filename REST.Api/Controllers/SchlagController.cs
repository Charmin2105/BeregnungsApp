using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using REST.Api.Entities;
using REST.Api.Models;
using REST.Api.Services;

namespace REST.Api.Controllers
{
    [Route("api/schlaege")]
    public class SchlagController : Controller
    {
        private IBeregnungsRepository _beregnungsRepository;

        public SchlagController(IBeregnungsRepository beregnungsRepository)
        {
            _beregnungsRepository = beregnungsRepository;
        }

        // Laden aller Schläge.
        /// <returns>OK Code </returns>
        [HttpGet()]
        public IActionResult GetSchlaege()
        {
            var schlagfromRepo = _beregnungsRepository.GetSchlaege();
            var schlag = Mapper.Map<IEnumerable<SchlagDto>>(schlagfromRepo);
            return Ok(schlag);
        }

        // Laden eines bestimmten Schlages.
        /// <param name="id">ID des gesuchten Schlages.</param>
        /// <returns>OK Code </returns>
        [HttpGet("{id}", Name = "GetSchlag")]
        public IActionResult GetSchlaege(Guid id)
        {
            
            var schlagfromRepo = _beregnungsRepository.GetSchlaege(id);
            if (schlagfromRepo == null)
            {
                return NotFound();
            }
            var schlag = Mapper.Map<SchlagDto>(schlagfromRepo);
            return Ok(schlag);
        }

        // Hinzufügen eines Schlages.
        /// <param name="schlag">Neuer Schlag.</param>
        /// <returns>CreatedAtRoute </returns>
        [HttpPost]
        public IActionResult CreatSchlag([FromBody]SchlagForCreationDto schlag)
        {
            //Überprüfung ob der Übergabeparameter leer ist
            /// <returns>BadRequest </returns>
            if (schlag == null)
            {
                return BadRequest();
            }

            var schlagEntity = Mapper.Map<Schlag>(schlag);

            _beregnungsRepository.AddSchlag(schlagEntity);

            if (!_beregnungsRepository.Save())
            {
                throw new Exception("Fehler beim Speichern eines neuen Schlages");
            }
            var schlagToReturn = Mapper.Map<SchlagDto>(schlagEntity);

            return CreatedAtRoute("GetSchlag",
                new { id = schlagToReturn.Id },
                schlagToReturn);
        }

        // Hinzufügen eines Schlages.
        /// <param name="id">Id des zu löschenden Schlag.</param>
        /// <returns>NoContent  Code</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteSchlag (Guid id)
        {
            //Existiert der Schlag?
            /// <returns>NotFound </returns>
            if (!_beregnungsRepository.SchlagExists(id))
            {
                return NotFound();
            }

            var schlagFromRepo = _beregnungsRepository.GetSchlaege(id);
            _beregnungsRepository.DeleteSchlag(schlagFromRepo);
            
            if (!_beregnungsRepository.Save())
            {
                throw new Exception($"Löschen des Schlags schlug fehl. ");
            }

            return NoContent();
        }
    }
}