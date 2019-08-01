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
    [Route("api/betriebe/{BetriebID}/schlaege")]
    public class SchlagController : Controller
    {
        //fields
        private IBetriebRepository _betriebRepository;
        private ILogger<SchlagController> _iLogger;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;


        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="beregnungsRepository"></param>
        /// <param name="ilogger"></param>
        public SchlagController(IBetriebRepository betriebRepository,
            ILogger<SchlagController> ilogger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _iLogger = ilogger;
            _betriebRepository = betriebRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }
        #endregion
        #region Methods

        /// <summary>
        /// Laden aller Schläge.
        /// </summary>
        /// <param name="pageNumber">Anzahl der Ausgaben.</param>
        /// /// <param name="pageSize">Seitenzahl die Angezeigt werden soll</param>
        /// <returns>OK Code </returns>
        /// <response code="200">Returns alle Schläge</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(Name = "GetSchlaege")]
        [HttpHead]
        public ActionResult<Schlag> GetSchlaege(Guid betriebID, SchlagResourceParameter resourceParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            if (!_typeHelperService.TypeHasProperties<SchlagDto>(resourceParameters.Fields))
            {
                return BadRequest();
            }
            //Aus DB laden
            var schlagfromRepo = _betriebRepository.GetSchlaege(betriebID,resourceParameters);
            var schlag = Mapper.Map<IEnumerable<SchlagDto>>(schlagfromRepo);

            //Falls der Header "application/vnd.ostfalia.hateoas+json" werden keine Links im Header mit angegeben. 
            if (mediaType == "application/vnd.ostfalia.hateoas+json")
            {

                //erstellen der Metadaten
                var paginationMetadata = new
                {
                    totalCount = schlagfromRepo.TotalCount,
                    pageSize = schlagfromRepo.PageSize,
                    currentPage = schlagfromRepo.CurrentPage,
                    totalPages = schlagfromRepo.TotalPages,
                };
                Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                //Links für alle Daten
                var links = CreateLinksForschlaege(resourceParameters,
                    schlagfromRepo.HasNext,
                    schlagfromRepo.HasPrevious);

                //DataShape
                var shapedSchlaege = schlag.ShapeData(resourceParameters.Fields);

                //Links für jede einzelnen Datensatz
                var shapedSchlaegeWithLinks = shapedSchlaege.Select(s =>
                {
                    var schlagAsDictionary = s as IDictionary<string, object>;
                    var schlagLinks = CreateLinksForSchlag((Guid)schlagAsDictionary["ID"], resourceParameters.Fields);
                    schlagAsDictionary.Add("links", schlagLinks);
                    return schlagAsDictionary;

                });
                var linkedCollectionResource = new
                {
                    value = shapedSchlaegeWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                // erstellen der Links
                var previousPageLink = schlagfromRepo.HasPrevious ?
                    CreateSchlagResourceUri(resourceParameters,
                    ResourceUriType.PreviousPage) : null;
                var nextPageLink = schlagfromRepo.HasNext ?
                    CreateSchlagResourceUri(resourceParameters,
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
                return Ok(schlag.ShapeData(resourceParameters.Fields));
            }

        }

        /// <summary>
        /// CreateSchlagResourceUri
        /// </summary>
        /// <param name="schlagRessourceParameters">Übergabe der RessourceParameter</param>
        /// <param name="type">Übergabe der ResourceUriType </param>
        /// <returns>Link</returns>
        private string CreateSchlagResourceUri( SchlagResourceParameter resourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetSchlaege", new
                    {
                        fields = resourceParameters.Fields,
                        pageNumber = resourceParameters.PageNumber - 1,
                        pageSize = resourceParameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetSchlaege", new
                    {
                        fields = resourceParameters.Fields,
                        pageNumber = resourceParameters.PageNumber + 1,
                        pageSize = resourceParameters.PageSize
                    });
                default:
                    return _urlHelper.Link("GetSchlaege", new
                    {
                        fields = resourceParameters.Fields,
                        pageNumber = resourceParameters.PageNumber,
                        pageSize = resourceParameters.PageSize
                    });
            }
        }

        /// <summary>
        /// Laden eines bestimmten Schlages.
        /// </summary>
        /// <param name="id">ID des gesuchten Schlages.</param>
        /// <returns>OK Code </returns>
        /// <response code="200">Returns einen bestimmten Schlag</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}", Name = "GetSchlag")]
        public ActionResult<Schlag> GetSchlag(Guid betriebID, Guid id, [FromQuery] string fields)
        {
            var schlagfromRepo = _betriebRepository.GetSchlag(betriebID, id);
            if (schlagfromRepo == null)
            {
                return NotFound();
            }
            var schlag = Mapper.Map<SchlagDto>(schlagfromRepo);

            var links = CreateLinksForSchlag(id, fields);

            var linkedResourceToReturn = schlag.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Hinzufügen eines Schlages.
        /// </summary>
        /// <param name="schlag">Neuer Schlag.</param>
        /// <returns>CreatedAtRoute </returns>
        /// <remarks>
        /// Beispiel request (erstellen eines neuen Schlags)
        /// POST /schlaege/id
        /// {       
        ///    "name": "Schlag 23",
        /// }
        /// </remarks>
        /// <response code="201">Returns Schlag erstellt</response>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost(Name = "CreatSchlag")]
        public ActionResult<Schlag> CreatSchlag(Guid betriebID, [FromBody]SchlagForCreationDto schlag)
        {
            //Überprüfung ob der Übergabeparameter leer ist
            // <returns>BadRequest </returns>
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

            _betriebRepository.AddSchlag(betriebID,schlagEntity);

            if (!_betriebRepository.Save())
            {
                throw new Exception("Fehler beim Speichern eines neuen Schlages");
            }
            var schlagToReturn = Mapper.Map<SchlagDto>(schlagEntity);

            _iLogger.LogInformation($"CreatSchlag erfolgreich ID: {schlagToReturn.ID} .");

            var links = CreateLinksForSchlag(schlagToReturn.ID, null);

            var linkedResourceToReturn = schlagToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetSchlag",
                new { id = linkedResourceToReturn["ID"] }, linkedResourceToReturn);

        }

        ///<summary>
        /// Löschen eines Schlages.
        /// </summary>
        /// <param name="id">Id des zu löschenden Schlag.</param>
        /// <returns>NoContent  Code</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteSchlag")]
        public ActionResult<Schlag> DeleteSchlag(Guid betriebID, Guid id)
        {
            //Existiert der Schlag?
            if (!_betriebRepository.SchlagExists(betriebID,id))
            {
                return NotFound();
            }

            var schlagFromRepo = _betriebRepository.GetSchlag(betriebID, id);
            if (schlagFromRepo == null)
            {
                return NotFound();
            }
            _betriebRepository.DeleteSchlag(schlagFromRepo);

            if (!_betriebRepository.Save())
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
        /// <remarks>
        /// Beispiel request (ändert den Namen des Schlags)
        /// PUT /schlaege/id
        /// {
        ///    "id": "96337487-31ef-4497-9246-8cc120dc80de",
        ///    "name": "Schlag 88",
        ///}
        /// </remarks>
        /// <response code="201">Returns Schlag erstellt</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "UpdateSchlag")]
        public ActionResult<Schlag> UpdateSchlag(Guid betriebID, Guid id, [FromBody]SchlagForUpdateDto schlag)
        {
            //geänderte Daten
            // <returns>BadRequest </returns>
            if (schlag == null)
            {
                return BadRequest();
            }

            //Existiert der Schlag?
            // <returns>NotFound </returns>
            if (!_betriebRepository.SchlagExists(betriebID,id))
            {
                var schlagEntity = Mapper.Map<Schlag>(schlag);
                schlagEntity.ID = id;

                _betriebRepository.AddSchlag(betriebID,schlagEntity);

                if (!_betriebRepository.Save())
                {
                    throw new Exception($"Upserting schlug fehl.");
                }

                var schlagToReturn = Mapper.Map<SchlagDto>(schlagEntity);

                var links = CreateLinksForSchlag(schlagToReturn.ID, null);

                var linkedResourceToReturn = schlag.ShapeData(null)
                    as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return Ok(("GetSchlag",
                    new { id = linkedResourceToReturn["ID"] }, linkedResourceToReturn));
            }
            var schlagFromRepo = _betriebRepository.GetSchlag(betriebID,id);
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

            _betriebRepository.UpdateSchlag(schlagFromRepo);

            if (!_betriebRepository.Save())
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
        /// <remarks>
        /// Beispiel request (ändert den Namen des Schlags)
        /// PATCH /schlaege/id
        /// [
        ///	    {
        ///		"op" : "replace",
        ///		"path":"/name",
        ///		"value" : "13"
        ///	    }
        ///]
        /// </remarks>
        /// <response code="201">Returns Schlag erstellt</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{id}", Name = "PartiallyUpdateSchlag")]
        public ActionResult<Schlag> PartiallyUpdateSchlag(Guid betriebID, Guid id,
            [FromBody]JsonPatchDocument<SchlagForUpdateDto> patchDoc)
        {
            //Eingabe ist nicht null
            if (patchDoc == null)
            {
                return BadRequest();
            }
            //Existiert der Schlag?
            // <returns>NotFound </returns>
            if (!_betriebRepository.SchlagExists(betriebID,id))
            {
                return NotFound();
            }

            var schlagFromRepo = _betriebRepository.GetSchlag(betriebID,id);

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

                _betriebRepository.AddSchlag(betriebID,schlagToAdd);

                if (!_betriebRepository.Save())
                {
                    throw new Exception($"Upserting Schlag mit der ID: {id} schlug fehl");
                }
                var schlagToReturn = Mapper.Map<SchlagDto>(schlagToAdd);

                return CreatedAtRoute("GetSchlag",
                        new { id = schlagToReturn.ID },
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

            _betriebRepository.UpdateSchlag(schlagFromRepo);
            if (!_betriebRepository.Save())
            {
                throw new Exception($"Patch Schlag mit der ID: {id} schlug fehl");
            }

            _iLogger.LogInformation($"Patch Schlag mit ID: {schlagFromRepo.ID} erfolgreich.");

            return NoContent();


        }

        /// <summary>
        /// CreateLinksForSchlag
        /// </summary>
        /// <param name="schlag">Übergabe eines SchlagDto</param>
        /// <returns>SchlagDto mit Links </returns>
        private IEnumerable<LinkDto> CreateLinksForSchlag(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetSchlag", new { id = id }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetSchlag", new { id = id, fields = fields }),
                    "self",
                    "GET"));
            }

            links.Add(new LinkDto(_urlHelper.Link("GetSchlag",
                new { id = id }),
                "self",
                "GET"));

            links.Add(new LinkDto(_urlHelper.Link("DeleteSchlag",
               new { id = id }),
               "delete_schlag",
               "DELETE"));

            links.Add(new LinkDto(_urlHelper.Link("UpdateSchlag",
                new { id = id }),
                "update_schlag",
                "PUT"));

            links.Add(new LinkDto(_urlHelper.Link("PartiallyUpdateSchlag",
               new { id = id }),
               "partially_update_schlag",
               "PATCH"));


            return links;
        }

        /// <summary>
        /// CreateLinksForschlaege
        /// </summary>
        /// <param name="resourceParameter">Resource Parameter</param>
        /// <param name="hasNext">Ob es eine nächste Seite gibt</param>
        /// <param name="hasPrevious">Ob es eine vorherige Seite gibt</param>
        /// <returns>IEnumerable<LinkDto></returns>
        private IEnumerable<LinkDto> CreateLinksForschlaege(SchlagResourceParameter resourceParameter, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateSchlagResourceUri(resourceParameter,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateSchlagResourceUri(resourceParameter,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateSchlagResourceUri(resourceParameter,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }
            return links;
        }

        /// <summary>
        /// GetSchlagOptions
        /// </summary>
        /// <returns>OK</returns>
        [HttpOptions]
        public IActionResult GetSchlagOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH,PUT,DELETE");
            return Ok();
        }

        #endregion
    }

}