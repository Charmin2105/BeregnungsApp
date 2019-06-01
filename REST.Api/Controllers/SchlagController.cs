using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using REST.Api.Entities;
using REST.Api.Helpers;
using REST.Api.Models;
using REST.Api.Services;

namespace REST.Api.Controllers
{
    [Route("api/schlaege")]
    public class SchlagController : Controller
    {
        //fields
        private IBeregnungsRepository _beregnungsRepository;
        private ILogger<SchlagController> _iLogger;
        private IUrlHelper _urlHelper;


        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="beregnungsRepository"></param>
        /// <param name="ilogger"></param>
        public SchlagController(IBeregnungsRepository beregnungsRepository, ILogger<SchlagController> ilogger, IUrlHelper urlHelper)
        {
            _iLogger = ilogger;
            _beregnungsRepository = beregnungsRepository;
            _urlHelper = urlHelper;
        }
        #endregion
        #region Methods

        /// <summary>
        /// Laden aller Schläge.
        /// </summary>
        /// <param name="pageNumber">Anzahl der Ausgaben.</param>
        /// /// <param name="pageSize">Seitenzahl die Angezeigt werden soll</param>
        /// <returns>OK Code </returns>
        [HttpGet(Name = "GetSchlaege")]
        public IActionResult GetSchlaege(SchlagResourceParameters schlagRessourceParameters)
        {
            var schlagfromRepo = _beregnungsRepository.GetSchlaege(schlagRessourceParameters);
            
            // erstellen der Links
            var previousPageLink = schlagfromRepo.HasPrevious ? 
                CreateSchlagResourceUri(schlagRessourceParameters, 
                ResourceUriType.PreviousPage) : null;
            var nextPageLink = schlagfromRepo.HasNext ? 
                CreateSchlagResourceUri(schlagRessourceParameters, 
                ResourceUriType.NextPage) : null;

            //erstellen der Metadaten
            var paginationMetadata = new
            {
                totalCount = schlagfromRepo.TotalCount,
                pageSize = schlagfromRepo.PageSize,
                currentPage = schlagfromRepo.CurrentPage,
                totalPages = schlagfromRepo.TotalPages,
                previousPage = previousPageLink,
                nextPage = nextPageLink
            };
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));


            var schlag = Mapper.Map<IEnumerable<SchlagDto>>(schlagfromRepo);
            return Ok(schlag);
        }
        /// <summary>
        /// CreateSchlagResourceUri
        /// </summary>
        /// <param name="schlagRessourceParameters">Übergabe der RessourceParameter</param>
        /// <param name="type">Übergabe der ResourceUriType </param>
        /// <returns>Link</returns>
        private string CreateSchlagResourceUri(SchlagResourceParameters schlagRessourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetSchlaege", new
                    {
                        pageNumber = schlagRessourceParameters.PageNumber - 1,
                        pageSize = schlagRessourceParameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetSchlaege", new
                    {
                        pageNumber = schlagRessourceParameters.PageNumber + 1,
                        pageSize = schlagRessourceParameters.PageSize
                    });
                default:
                    return _urlHelper.Link("GetSchlaege", new
                    {
                        pageNumber = schlagRessourceParameters.PageNumber,
                        pageSize = schlagRessourceParameters.PageSize
                    });
            }
        }

        /// <summary>
        /// Laden eines bestimmten Schlages.
        /// </summary>
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

        /// <summary>
        /// Hinzufügen eines Schlages.
        /// </summary>
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

            //Für evtl.spätere weitere Eigenschaft die Validiert werden müssen
            //if (schlag.Name == schlag.irgendwas)
            //{
            //    ModelState.AddModelError(nameof(SchlagForCreationDto), "Name und irgendwas darf nicht gleich sein");
            //}

            if (!ModelState.IsValid)
            {
                //return 422
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }



            var schlagEntity = Mapper.Map<Schlag>(schlag);

            _beregnungsRepository.AddSchlag(schlagEntity);

            if (!_beregnungsRepository.Save())
            {
                throw new Exception("Fehler beim Speichern eines neuen Schlages");
            }
            var schlagToReturn = Mapper.Map<SchlagDto>(schlagEntity);

            _iLogger.LogInformation($"CreatSchlag erfolgreich ID: {schlagToReturn.Id} .");

            return CreatedAtRoute("GetSchlag",
                new { id = schlagToReturn.Id },
                schlagToReturn);

        }

        ///<summary>
        /// Löschen eines Schlages.
        /// </summary>
        /// <param name="id">Id des zu löschenden Schlag.</param>
        /// <returns>NoContent  Code</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteSchlag(Guid id)
        {
            //Existiert der Schlag?
            /// <returns>NotFound </returns>
            if (!_beregnungsRepository.SchlagExists(id))
            {
                return NotFound();
            }

            var schlagFromRepo = _beregnungsRepository.GetSchlaege(id);
            if (schlagFromRepo == null)
            {
                return NotFound();
            }
            _beregnungsRepository.DeleteSchlag(schlagFromRepo);

            if (!_beregnungsRepository.Save())
            {
                throw new Exception($"Löschen des Schlags schlug fehl. ");
            }
            _iLogger.LogInformation($"Schlag {schlagFromRepo.Name} erfolgreich gelöscht.");

            return NoContent();
        }

        ///<summary>
        /// Update eines Schlages.
        /// </summary>
        /// <param name="id">Id des zu updatenden Schlag.</param>
        /// <param name="schlag">Schlag Entity</param>
        /// <returns>NoContent  Code</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateSchlag(Guid id, [FromBody]SchlagForUpdateDto schlag)
        {
            //geänderte Daten
            /// <returns>BadRequest </returns>
            if (schlag == null)
            {
                return BadRequest();
            }

            //Existiert der Schlag?
            /// <returns>NotFound </returns>
            if (!_beregnungsRepository.SchlagExists(id))
            {
                var schlagEntity = Mapper.Map<Schlag>(schlag);
                schlagEntity.ID = id;

                _beregnungsRepository.AddSchlag(schlagEntity);

                if (!_beregnungsRepository.Save())
                {
                    throw new Exception($"Upserting schlug fehl.");
                }

                var schlagToReturn = Mapper.Map<SchlagDto>(schlagEntity);

                return CreatedAtRoute("GetSchlag",
                    new { id = schlagToReturn.Id },
                    schlagToReturn);
            }
            var schlagFromRepo = _beregnungsRepository.GetSchlaege(id);
            //if (schlagFromRepo == null)
            //{
            //    var schlagEntity = Mapper.Map<Schlag>(schlag);
            //    schlagEntity.ID = id;

            //    _beregnungsRepository.AddSchlag(schlagEntity);

            //    if (!_beregnungsRepository.Save())
            //    {
            //        throw new Exception($"Upserting schlug fehl.");
            //    }

            //    var schlagToReturn = Mapper.Map<SchlagDto>(schlagEntity);

            //    return CreatedAtRoute("GetSchlag",
            //        new { id = schlagToReturn.Id },
            //        schlagToReturn);
            //}

            Mapper.Map(schlag, schlagFromRepo);

            _beregnungsRepository.UpdateSchlag(schlagFromRepo);

            if (!_beregnungsRepository.Save())
            {
                throw new Exception($"Speichern des Schlags schlug fehl. ");
            }

            _iLogger.LogInformation($"Update Schlag mit ID: {schlagFromRepo.ID} erfolgreich.");

            return NoContent();
        }

        ///<summary>
        /// Teilupdate eines Schlages.
        /// </summary>
        /// <param name="id">Id des zu updatenden Schlag.</param>
        /// <param name="patchDoc">Schlag Entity</param>
        /// <returns>NoContent  Code</returns>
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateSchlag(Guid id,
            [FromBody]JsonPatchDocument<SchlagForUpdateDto> patchDoc)
        {
            //Eingabe ist nicht null
            if (patchDoc == null)
            {
                return BadRequest();
            }
            //Existiert der Schlag?
            /// <returns>NotFound </returns>
            if (!_beregnungsRepository.SchlagExists(id))
            {
                return NotFound();
            }

            var schlagFromRepo = _beregnungsRepository.GetSchlaege(id);

            if (schlagFromRepo == null)
            {
                var schlagDto = new SchlagForUpdateDto();
                patchDoc.ApplyTo(schlagDto);

                TryValidateModel(schlagDto);

                if (!ModelState.IsValid)
                {
                    return new Helpers.UnprocessableEntityObjectResult(ModelState);
                }

                var schlagToAdd = Mapper.Map<Schlag>(schlagDto);
                schlagToAdd.ID = id;

                _beregnungsRepository.AddSchlag(schlagToAdd);

                if (!_beregnungsRepository.Save())
                {
                    throw new Exception($"Upserting Schlag mit der ID: {id} schlug fehl");
                }
                var schlagToReturn = Mapper.Map<SchlagDto>(schlagToAdd);

                return CreatedAtRoute("GetSchlag",
                        new { id = schlagToReturn.Id },
                        schlagToReturn);
            }

            var schlagToPatch = Mapper.Map<SchlagForUpdateDto>(schlagFromRepo);

            patchDoc.ApplyTo(schlagToPatch, ModelState);

            TryValidateModel(schlagToPatch);

            if (!ModelState.IsValid)
            {
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }


            Mapper.Map(schlagToPatch, schlagFromRepo);

            _beregnungsRepository.UpdateSchlag(schlagFromRepo);
            if (!_beregnungsRepository.Save())
            {
                throw new Exception($"Patch Schlag mit der ID: {id} schlug fehl");
            }

            _iLogger.LogInformation($"Patch Schlag mit ID: {schlagFromRepo.ID} erfolgreich.");

            return NoContent();


        }
        #endregion
    }

}