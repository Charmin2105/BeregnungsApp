using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using REST.Api.Entities;
using REST.Api.Helpers;
using REST.Api.Models;
using REST.Api.Services;

namespace REST.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/betriebe/{BetriebID}/mitarbeiters")]
    public class MitarbeiterController : Controller
    {
        //Fields
        private IBetriebRepository _betriebRepository;
        private ILogger<MitarbeiterController> _ilogger;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="mitarbeiterRepository">mitarbeiterRepsoitory</param>
        /// <param name="ilogger">ilogger</param>
        /// <param name="urlHelper">urlHelper</param>
        /// <param name="propertyMappingService">propertyMappingService</param>
        /// <param name="typeHelperService">typeHelperService</param>
        public MitarbeiterController(IBetriebRepository mitarbeiterRepository,
            ILogger<MitarbeiterController> ilogger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _ilogger = ilogger;
            _betriebRepository = mitarbeiterRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        /// <summary>
        /// Alle Mitarbeiter eines Betriebes 
        /// </summary>
        /// <param name="betriebID">ID des Betriebs</param>
        /// <returns>OK Code</returns>
        /// <response code="200">Returns alle Mitarbeiter eines Betriebes</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(Name = "GetMitarbeiters")]
        [HttpHead]
        public ActionResult<Mitarbeiter> GetMitarbeiters(Guid betriebID)
        {
            if (!_betriebRepository.BetriebExists(betriebID))
            {
                return NotFound();
            }

            var mitarbeiterFromRepo = _betriebRepository.GetMitarbeiters(betriebID);

            var mitarbeiterVonBetrieb = Mapper.Map<IEnumerable<MitarbeiterDto>>(mitarbeiterFromRepo);
            mitarbeiterVonBetrieb = mitarbeiterVonBetrieb.Select(mitarbeiter =>
            {
                mitarbeiter = CreateLinksForMitarbeiter(mitarbeiter);
                return mitarbeiter;
            });

            var wrapper = new LinkedCollectionResourceWrapperDto<MitarbeiterDto>(mitarbeiterVonBetrieb);

            return Ok(CreateLinksForMitarbeiter(wrapper));
        }

        /// <summary>
        /// Einen bestimmten Mitarbeiter laden
        /// </summary>
        /// <param name="betriebID">Id des Betriebs</param>
        /// <param name="id">ID des Mitarbeiters</param>
        /// <returns>OK Code</returns>
        /// <response code="200">Returns einen bestimmten Mitarbeiter eines Betriebes</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}", Name = "GetMitarbeiter")]
        public ActionResult<Mitarbeiter> GetMitarbeiter(Guid betriebID, Guid id)
        {
            if (!_betriebRepository.BetriebExists(betriebID))
            {
                return NotFound();
            }

            var mitarbeiterFromRepo = _betriebRepository.GetMitarbeiter(betriebID, id);
            if (mitarbeiterFromRepo == null)
            {
                return NotFound();
            }
            var mitarbeiter = Mapper.Map<MitarbeiterDto>(mitarbeiterFromRepo);
            return Ok(CreateLinksForMitarbeiter(mitarbeiter));
        }

        /// <summary>
        /// Create Mitarbeiter
        /// </summary>
        /// <param name="betriebID">ID des Betriebs</param>
        /// <param name="mitarbeiter">Mitarbeiter Body</param>
        /// <returns>CreateAtRoute</returns>
        /// <remarks>
        /// Beispiel request (Erstellen eines neuen Mitarbeiters)
        /// POST api/betriebe/{BetriebID}/mitarbeiters
        /// {	       
        ///     "vorname": "Nico",
        ///     "nachname": "Herrmann",
        ///     "gebDatum": "1993-05-21T00:00:00+00:00",
        ///	}
        /// </remarks>
        /// <response code="201">Returns Mitarbeiter für einen Betrieb  erstellt</response>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Administrator")]
        [HttpPost(Name = "CreateMitarbeiter")]
        public ActionResult<Mitarbeiter> CreateMitarbeiter(Guid betriebID, [FromBody] MitarbeiterForCreationDto mitarbeiter)
        {
            //Body leer?
            if (mitarbeiter == null)
            {
                return BadRequest();
            }

            //Validierung
            if (mitarbeiter.Vorname == mitarbeiter.Nachname)
            {
                ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
                   "Nachname darf nicht gleich mit Vorname sein.");
            }
            if (mitarbeiter.Geburtstag >= DateTime.Today)
            {
                ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
                     "Geburtsdatum darf nicht in der Zukunft liegen");
            }

            if (!ModelState.IsValid)
            {
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            // Exisitert der Betrieb?
            if (!_betriebRepository.BetriebExists(betriebID))
            {
                return NotFound();
            }

            //Mapper
            var mitarbeiterEntity = Mapper.Map<Mitarbeiter>(mitarbeiter);

            _betriebRepository.AddMitarbeiter(betriebID, mitarbeiterEntity);

            if (!_betriebRepository.Save())
            {
                throw new Exception($"CreateMitarbeiter für den Betrieb mit ID {betriebID} schlug fehll.");
            }

            var mitarbeiterToReturn = Mapper.Map<MitarbeiterDto>(mitarbeiterEntity);

            return CreatedAtRoute("CreateMitarbeiter",
                    new { id = mitarbeiterToReturn.ID },
                    CreateLinksForMitarbeiter(mitarbeiterToReturn));
        }

        /// <summary>
        /// Löschen eines Mitarbeiters von einem Betrieb
        /// </summary>
        /// <param name="betriebID">ID des Betriebs</param>
        /// <param name="id">ID des Mitarbeiter</param>
        /// <returns>No Content</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}", Name = "DeleteMitarbeiter")]
        public ActionResult<Mitarbeiter> DeleteMitarbeiter(Guid betriebID, Guid id)
        {
            //Existiert der Betrieb?
            if (!_betriebRepository.BetriebExists(betriebID))
            {
                return NotFound();
            }

            var mitarbeiterFromRepo = _betriebRepository.GetMitarbeiter(betriebID, id);
            if (mitarbeiterFromRepo == null)
            {
                return NotFound();
            }
            _betriebRepository.DeleteMitarbeiter(mitarbeiterFromRepo);

            if (!_betriebRepository.Save())
            {
                throw new Exception($"Löschen des Mitarbeiter: {mitarbeiterFromRepo.Nachname} nicht möglich");
            }

            _ilogger.LogInformation(100, $"Mitarbeiter:  {mitarbeiterFromRepo.Nachname} wurde gelöscht");

            return NoContent();
        }

        /// <summary>
        /// Update eines Mitarbeiters
        /// </summary>
        /// <param name="betriebID">ID des Betriebs</param>
        /// <param name="id">ID des Mitarbeiter</param>
        /// <param name="mitarbeiter">Mitarbeiter Body</param>
        /// <returns>No Content</returns>
        /// remarks>
        /// Beispiel request (Ändern eines  Mitarbeiters)
        /// PUT api/betriebe/{BetriebID}/mitarbeiters
        /// {	
        ///     "id": "414f445d-c0f4-4419-849d-9a22231d524b",
        ///     "vorname": "Nico",
        ///     "nachname": "Herrmann",
        ///     "gebDatum": "1993-05-21T00:00:00+00:00",
        ///	}
        /// </remarks>
        /// <response code="201">Returns Mitarbeiter für einen Betrieb  erstellt</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}", Name = "UpdateMitarbeiter")]
        public ActionResult<Mitarbeiter> UpdateMitarbeiter(Guid betriebID, Guid id,
            [FromBody] MitarbeiterForUpdateDto mitarbeiter)
        {
            //Body null?
            if (mitarbeiter == null)
            {
                return BadRequest();
            }

            //Validierung
            if (mitarbeiter.Vorname == mitarbeiter.Nachname)
            {
                ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
                   "Nachname darf nicht gleich mit Vorname sein.");
            }
            if (mitarbeiter.Geburtstag >= DateTime.Today)
            {
                ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
                     "Geburtsdatum darf nicht in der Zukunft liegen");
            }

            if (!ModelState.IsValid)
            {
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            // Existiert Betrieb?
            if (!_betriebRepository.BetriebExists(betriebID))
            {
                return NotFound();
            }

            // Mitarbeiter aus DB laden
            // Falls nicht vorhanden wird erstellt
            var mitarbeiterFromRepo = _betriebRepository.GetMitarbeiter(betriebID, id);
            if (mitarbeiterFromRepo == null)
            {
                var mitarbeiterToAdd = Mapper.Map<Mitarbeiter>(mitarbeiter);
                mitarbeiterToAdd.ID = id;

                _betriebRepository.AddMitarbeiter(betriebID, mitarbeiterToAdd);

                if (!_betriebRepository.Save())
                {
                    throw new Exception($"Upserting Mitarbeiter: {mitarbeiterToAdd.Nachname} vom Betrieb mit der ID:{betriebID} schlug fehl beim Speichern");
                }

                var mitarbeiterToReturn = Mapper.Map<MitarbeiterDto>(mitarbeiterToAdd);
                return CreatedAtRoute("GetMitarbeiter",
                    new { betriebID = betriebID, mitarbeiterID = mitarbeiterToReturn.ID },
                    mitarbeiterToReturn);
            }
            Mapper.Map(mitarbeiter, mitarbeiterFromRepo);

            if (!_betriebRepository.Save())
            {
                throw new Exception($"Upserting Mitarbeiter: {mitarbeiter.Nachname} vom Betrieb mit der ID:{betriebID} schlug fehl beim Speichern");
            }
            return NoContent();
        }

        /// <summary>
        /// Teilupdate eines Mitarbeiters
        /// </summary>
        /// <param name="betriebID">ID des Betriebs</param>
        /// <param name="id">ID des Mitarbeiters</param>
        /// <param name="patchDoc">PatchDoc</param>
        /// <returns>NoContent</returns>
        ///         /// remarks>
        /// Beispiel request (Ändern eines  Mitarbeiters)
        /// PATTCH api/betriebe/{BetriebID}/mitarbeiters
        /// [
        ///     {
        ///		    "op" : "replace",
        ///         "path":"/Nachname",
        ///         "value" : "Müller"
        ///
        ///    }
        ///]
        /// </remarks>
        /// <response code="201">Returns Mitarbeiter für einen Betrieb  erstellt</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id}", Name = "PartiallyUpdateMitarbeiter")]
        public ActionResult<Mitarbeiter> PartiallyUpdateMitarbeiter(Guid betriebID, Guid id,
            [FromBody] JsonPatchDocument<MitarbeiterForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_betriebRepository.BetriebExists(betriebID))
            {
                return NotFound();
            }

            // Mitarbeiter aus DB laden
            var mitarbeiterFromRepo = _betriebRepository.GetMitarbeiter(betriebID, id);
            if (mitarbeiterFromRepo == null)
            {
                var mitarbeiterDto = new MitarbeiterForUpdateDto();
                patchDoc.ApplyTo(mitarbeiterDto, ModelState);

                //Validierung
                if (mitarbeiterDto.Vorname == mitarbeiterDto.Nachname)
                {
                    ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
                       "Nachname darf nicht gleich mit Vorname sein.");
                }
                if (mitarbeiterDto.Geburtstag >= DateTime.Today)
                {
                    ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
                         "Geburtsdatum darf nicht in der Zukunft liegen");
                }

                TryValidateModel(mitarbeiterDto);

                if (!ModelState.IsValid)
                {
                    return new Helpers.UnprocessableEntityObjectResult(ModelState);
                }
                var mitarbeiterToAdd = Mapper.Map<Mitarbeiter>(mitarbeiterDto);
                mitarbeiterToAdd.ID = id;

                _betriebRepository.AddMitarbeiter(betriebID, mitarbeiterToAdd);

                if (!_betriebRepository.Save())
                {
                    throw new Exception($"Upserting Mitarbeiter: {mitarbeiterToAdd.Nachname} vom Betrieb mit der ID:{betriebID} schlug fehl beim Speichern");
                }

                var mitarbeiterToReturn = Mapper.Map<MitarbeiterDto>(mitarbeiterToAdd);
                return CreatedAtRoute("GetMitarbeiter",
                    new { betriebID = betriebID, mitarbeiterID = mitarbeiterToReturn.ID },
                    mitarbeiterToReturn);
            }

            var mitarbeiterToPatch = Mapper.Map<MitarbeiterForUpdateDto>(mitarbeiterFromRepo);

            patchDoc.ApplyTo(mitarbeiterToPatch, ModelState);

            //Validierung
            //if (mitarbeiterToPatch.Vorname == mitarbeiterToPatch.Nachname)
            //{
            //    ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
            //       "Nachname darf nicht gleich mit Vorname sein.");
            //}
            //if (mitarbeiterToPatch.GebDatum >= DateTime.Today)
            //{
            //    ModelState.AddModelError(nameof(MitarbeiterForCreationDto),
            //         "Geburtsdatum darf nicht in der Zukunft liegen");
            //}

            TryValidateModel(mitarbeiterToPatch);

            if (!ModelState.IsValid)
            {
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(mitarbeiterToPatch, mitarbeiterFromRepo);

            _betriebRepository.UpdateMitarbeiter(mitarbeiterFromRepo);

            if (!_betriebRepository.Save())
            {
                throw new Exception($"Patching Mitarbeiter: {mitarbeiterToPatch.Nachname} vom Betrieb mit der ID:{betriebID} schlug fehl beim Speichern");
            }
            return NoContent();

        }

        /// <summary>
        /// Links für Mitarbeiter erstellen
        /// </summary>
        /// <param name="mitarbeiter">Jeweiligen Mitarbeiter</param>
        /// <returns> Mitarbeiter</returns>
        private MitarbeiterDto CreateLinksForMitarbeiter(MitarbeiterDto mitarbeiter)
        {
            mitarbeiter.Links.Add(new LinkDto(_urlHelper.Link("GetMitarbeiter",
                new { id = mitarbeiter.ID }),
                "self",
                "GET"));

            mitarbeiter.Links.Add(
                new LinkDto(_urlHelper.Link("DeleteMitarbeiter",
                new { id = mitarbeiter.ID }),
                "delete_mitarbeiter",
                "DELETE"));

            mitarbeiter.Links.Add(
                new LinkDto(_urlHelper.Link("UpdateMitarbeiter",
                new { id = mitarbeiter.ID }),
                "update_mitarbeiter",
                "PUT"));

            mitarbeiter.Links.Add(
                new LinkDto(_urlHelper.Link("PartiallyUpdateMitarbeiter",
                new { id = mitarbeiter.ID }),
                "partially_update_mitarbeiter",
                "PATCH"));

            return mitarbeiter;
        }

        /// <summary>
        /// WrapperLinks für Mitarbeiter
        /// </summary>
        /// <param name="mitarbeiterWrapper"></param>
        /// <returns>LinkedCollectionResourceWrapperDto</returns>
        private LinkedCollectionResourceWrapperDto<MitarbeiterDto> CreateLinksForMitarbeiter(
            LinkedCollectionResourceWrapperDto<MitarbeiterDto> mitarbeiterWrapper)
        {
            // link to self
            mitarbeiterWrapper.Links.Add(
                new LinkDto(_urlHelper.Link("GetMitarbeiters", new { }),
                "self",
                "GET"));

            return mitarbeiterWrapper;
        }

        /// <summary>
        /// GetMitarbeiterOptions
        /// </summary>
        /// <returns>OK</returns>
        [HttpOptions]
        public IActionResult GetMitarbeiterOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH,PUT,DELETE");
            return Ok();
        }
    }
}