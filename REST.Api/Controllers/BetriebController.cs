using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/betriebe")]
    public class BetriebController : Controller
    {
        //Fields
        private IBetriebRepository _betriebsRepository;
        private ILogger<BetriebController> _ilogger;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="betriebRepsoitory">betriebRepsoitory</param>
        /// <param name="ilogger">ilogger</param>
        /// <param name="urlHelper">urlHelper</param>
        /// <param name="propertyMappingService">propertyMappingService</param>
        /// <param name="typeHelperService">typeHelperService</param>
        public BetriebController(IBetriebRepository betriebRepsoitory,
            ILogger<BetriebController> ilogger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _ilogger = ilogger;
            _betriebsRepository = betriebRepsoitory;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        /// <summary>
        /// Laden aller Betriebe
        /// </summary>
        /// <param name="resourceParameters">resourceParameters</param>
        /// <returns>IEnumerable von Betrieben</returns>
        [HttpGet(Name = "GetBetriebe")]
        public IActionResult GetBetriebe(BetriebResourceParameter resourceParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            if (!_typeHelperService.TypeHasProperties<BetriebDto>(resourceParameters.Fields))
            {
                return BadRequest();
            }

            //Aus DB laden
            var betriebFromRepo = _betriebsRepository.GetBetriebe(resourceParameters);
            var betrieb = Mapper.Map<IEnumerable<BetriebDto>>(betriebFromRepo);

            //Falls der Header "application/vnd.ostfalia.hateoas+json" werden keine Links im Header mit angegeben. 
            if (mediaType == "application/vnd.ostfalia.hateoas+json")
            {
                //Metadaten erstellen
                var paginationMetadata = new
                {
                    totalCount = betriebFromRepo.TotalCount,
                    pageSize = betriebFromRepo.PageSize,
                    currentPage = betriebFromRepo.CurrentPage,
                    totalPage = betriebFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                //Links für alle Daten
                var links = CreateLinksForBetriebe(resourceParameters,
                    betriebFromRepo.HasNext,
                    betriebFromRepo.HasPrevious);

                //DataShape
                var shapedBetrieb = betrieb.ShapeData(resourceParameters.Fields);

                //Links für jeden einzelnen Datensatz
                var shapedBetriebWithLinks = shapedBetrieb.Select(d =>
                {
                    var betriebAsDictionary = d as IDictionary<string, object>;
                    var betriebLinks = CreateLinksForBetrieb((Guid)betriebAsDictionary["ID"], resourceParameters.Fields);
                    betriebAsDictionary.Add("links", betriebLinks);
                    return betriebAsDictionary;

                });
                var linkedCollectionResource = new
                {
                    value = shapedBetriebWithLinks,
                    links = links
                };


                return Ok(linkedCollectionResource);
            }
            else
            {
                // erstellen der Links
                var previousPageLink = betriebFromRepo.HasPrevious ?
                    CreateBetriebResourceUri(resourceParameters,
                    ResourceUriType.PreviousPage) : null;
                var nextPageLink = betriebFromRepo.HasNext ?
                    CreateBetriebResourceUri(resourceParameters,
                    ResourceUriType.NextPage) : null;

                //erstellen der Metadaten
                var paginationMetadata = new
                {
                    totalCount = betriebFromRepo.TotalCount,
                    pageSize = betriebFromRepo.PageSize,
                    currentPage = betriebFromRepo.CurrentPage,
                    totalPages = betriebFromRepo.TotalPages,
                    previousPage = previousPageLink,
                    nextPage = nextPageLink
                };
                Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
                return Ok(betrieb.ShapeData(resourceParameters.Fields));
            }


        }

        /// <summary>
        /// Laden eines bestimmten Betriebs
        /// </summary>
        /// <param name="id">Id des gesuchten Betriebs</param>
        /// <param name="fields">fields parameter</param>
        /// <returns>Ok Status Code</returns>
        [HttpGet("{id}", Name = "GetBetrieb")]
        public IActionResult GetBetrieb(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<BetriebDto>(fields))
            {
                return BadRequest();
            }
            var BetriebFromRepo = _betriebsRepository.GetBetrieb(id);
            if (BetriebFromRepo == null)
            {
                return NotFound();
            }
            var betrieb = Mapper.Map<BetriebDto>(BetriebFromRepo);

            var links = CreateLinksForBetrieb(id, fields);

            var linkedResourceToReturn = betrieb.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Hinzufügen eines neuen Betriebs
        /// </summary>
        /// <param name="betrieb"> Betrieb body</param>
        /// <returns>CreateAtRoute</returns>
        [HttpPost(Name = "CreateBetrieb")]
        public IActionResult CreateBetrieb([FromBody]BetriebForCreationDto betrieb)
        {
            //Überprüfung ob der Übergabeparameter leer ist
            if (betrieb == null)
            {
                return BadRequest();
            }

            //Validierung
            if (betrieb.Name == string.Empty)
            {

                ModelState.AddModelError(nameof(BetriebForCreationDto), "Bitte einen Namen für den Betrieb eingeben.");
            }


            if (!ModelState.IsValid)
            {
                //return 422
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            //Mapper
            var betriebEntitiy = Mapper.Map<Betrieb>(betrieb);
            _betriebsRepository.AddBetrieb(betriebEntitiy);

            if (!_betriebsRepository.Save())
            {
                throw new Exception("Fehler beim speichern eines neuen Betriebs");
            }

            var betriebToReturn = Mapper.Map<BetriebDto>(betriebEntitiy);

            var links = CreateLinksForBetrieb(betriebToReturn.ID, null);

            var linkedResourceToReturn = betriebToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            _ilogger.LogInformation($"CreateBetrieb erfolgreich ID{betriebToReturn.ID}.");

            return CreatedAtRoute("GetBetrieb",
                new { id = linkedResourceToReturn["ID"] }, linkedResourceToReturn);
        }

        /// <summary>
        /// Betrieb löschen
        /// </summary>
        /// <param name="id">ID des zu löschenden Betriebs</param>
        /// <returns>No Content Code</returns>
        [HttpDelete("{id}", Name = "DeleteBetrieb")]
        public IActionResult DeleteBetrieb(Guid id)
        {
            //Exisitert Betrieb?
            if (!_betriebsRepository.BetriebExists(id))
            {
                return NotFound();
            }

            var betriebFromRepo = _betriebsRepository.GetBetrieb(id);
            if (betriebFromRepo == null)
            {
                return NotFound();
            }

            _betriebsRepository.DeleteBetrieb(betriebFromRepo);

            if (!_betriebsRepository.Save())
            {
                throw new Exception($"Löschen der Betrieb mit der ID: {id} schlug fehl");
            }
            _ilogger.LogInformation($"Betrieb {betriebFromRepo} erfolgreich gelöscht");

            return NoContent();

        }

        /// <summary>
        /// Update eines Betriebs
        /// </summary>
        /// <param name="id">ID des zu updatenden Betriebs</param>
        /// <param name="betrieb">Update body</param>
        /// <returns> No Content Code</returns>
        [HttpPut("{id}", Name = "UpdateBetrieb")]
        public IActionResult UpdateBetrieb(Guid id, [FromBody]BetriebForUpdateDto betrieb)
        {
            //Geänderte Daten
            if (betrieb == null)
            {
                return BadRequest();
            }
            //Existiert Daten?
            if (!_betriebsRepository.BetriebExists(id))
            {
                //Falls nicht wird neu erstellt
                var betriebEntity = Mapper.Map<Betrieb>(betrieb);
                betriebEntity.ID = id;

                _betriebsRepository.AddBetrieb(betriebEntity);

                if (!_betriebsRepository.Save())
                {
                    throw new Exception($"Upserting schlug fehl");
                }

                var betriebToReturn = Mapper.Map<BetriebDto>(betriebEntity);

                return CreatedAtRoute("GetBetrieb",
                    new { guid = betriebToReturn.ID },
                    betriebToReturn);
            }
            var betriebFromRepo = _betriebsRepository.GetBetrieb(id);

            Mapper.Map(betrieb, betriebFromRepo);

            _betriebsRepository.UpdateBetrieb(betriebFromRepo);

            if (!_betriebsRepository.Save())
            {
                throw new Exception($"Fehler beim Speichern ");
            }

            _ilogger.LogInformation($"Update Betrieb mit ID: {betriebFromRepo.ID} erfolgreich.");

            return NoContent();

        }

        [HttpPatch("{id}", Name = "PartallyUpdateBetrieb")]
        public IActionResult PartallyUpdateBetrieb(Guid id,
            [FromBody]JsonPatchDocument<BetriebForUpdateDto> patchDoc)
        {
            //Eingabe nicht null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            //Datenexisieren?
            if (!_betriebsRepository.BetriebExists(id))
            {
                return NotFound();
            }

            var betriebFromRepo = _betriebsRepository.GetBetrieb(id);

            if (betriebFromRepo == null)
            {
                var betriebDto = new BetriebForUpdateDto();
                patchDoc.ApplyTo(betriebDto);

                TryValidateModel(betriebDto);

                if (!ModelState.IsValid)
                {
                    return new Helpers.UnprocessableEntityObjectResult(ModelState);
                }

                var betriebToAdd = Mapper.Map<Betrieb>(betriebDto);
                betriebToAdd.ID = id;

                _betriebsRepository.AddBetrieb(betriebToAdd);

                if (!_betriebsRepository.Save())
                {
                    throw new Exception($"Fehler beim Speichern ");
                }

                var betriebToReturn = Mapper.Map<BetriebDto>(betriebToAdd);

                return CreatedAtRoute("GetBetrieb",
                    new { guid = betriebToReturn.ID },
                    betriebToReturn);
            }

            var betriebToPatch = Mapper.Map<BetriebForUpdateDto>(betriebFromRepo);

            patchDoc.ApplyTo(betriebToPatch, ModelState);

            TryValidateModel(betriebToPatch);

            if (!ModelState.IsValid)
            {
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(betriebToPatch, betriebFromRepo);
            _betriebsRepository.UpdateBetrieb(betriebFromRepo);
            if (!_betriebsRepository.Save())
            {
                throw new Exception($"Fehler beim Speichern ");
            }
            _ilogger.LogInformation($"Patch Betrieb mit ID: {betriebFromRepo.ID} erfolgreich.");

            return NoContent();
        }

        /// <summary>
        /// CreateLinksForBetrieb
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="fields">Fields</param>
        /// <returns>IEnumerable<LinkDto> links</returns>
        private IEnumerable<LinkDto> CreateLinksForBetrieb(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetBetriebe", new { id = id }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetBetriebe", new { id = id, fields = fields }),
                    "self",
                    "GET"));
            }
            links.Add(
                    new LinkDto(_urlHelper.Link("GetBetriebe", new { id = id }),
                    "delete_betrieb",
                    "DELETE"));
            links.Add(
                    new LinkDto(_urlHelper.Link("GetBetriebe", new { id = id }),
                    "update_betrieb",
                    "PUT"));
            links.Add(
                    new LinkDto(_urlHelper.Link("GetBetriebe", new { id = id }),
                    "partally_update_betrieb",
                    "PATCH"));

            return links;
        }

        /// <summary>
        /// CreateLinksForBetrieb
        /// </summary>
        /// <param name="ResourceParameter">ResourceParameter</param>
        /// <param name="hasNext">Hat nächste Seite Bool</param>
        /// <param name="hasPrevious">Hat vorherige Seite Bool</param>
        /// <returns>IEnumerable<LinkDto> links</returns>
        private IEnumerable<LinkDto> CreateLinksForBetriebe(BetriebResourceParameter resourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateBetriebResourceUri(resourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateBetriebResourceUri(resourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateBetriebResourceUri(resourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }
            return links;
        }

        /// <summary>
        /// CreateBetriebResourceUri
        /// </summary>
        /// <param name="ressourceParameters">Übergabe der RessourceParameter</param>
        /// <param name="type">Übergabe der ResourceUriType </param>
        /// <returns>string</returns>
        private string CreateBetriebResourceUri(BetriebResourceParameter resourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetBetriebe",
                        new
                        {
                            fields = resourceParameters.Fields,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetBetriebe",
                        new
                        {
                            fields = resourceParameters.Fields,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetBetriebe",
                        new
                        {
                            fields = resourceParameters.Fields,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }
    }
}
