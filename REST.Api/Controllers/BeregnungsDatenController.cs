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
    [Route("api/betriebe/{BetriebID}/beregnungsdaten")]
    public class BeregnungsDatenController : Controller
    {
        //Fields
        private IBetriebRepository _betriebsRepository;
        private ILogger<BeregnungsDatenController> _ilogger;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="beregnungsRepository">Repository</param>
        /// <param name="ilogger">Logger</param>
        /// <param name="urlHelper">UrlHelper</param>
        /// <param name="propertyMappingService"></param>
        /// <param name="typeHelperService"></param>
        public BeregnungsDatenController(IBetriebRepository beregnungsRepository,
            ILogger<BeregnungsDatenController> ilogger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _ilogger = ilogger;
            _betriebsRepository = beregnungsRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        /// <summary>
        /// Laden aller BeregnungsDaten
        /// </summary>
        /// <param name="resourceParameters">Metadateneinstellungen</param>
        /// <returns>IEnumerable von BeregnungsDaten</returns>
        /// <response code="200">Returns alle Beregnungs Daten</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(Name = "GetBergenungsDatens")]
        [HttpHead]
        public ActionResult<BeregnungsDaten> GetBeregnungsDatens(Guid betriebID, 
            BeregnungsDatenResourceParameter resourceParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            //Mapping für OrderBy ist valid
            if (!_propertyMappingService.ValidMappingExistsFor<BeregnungsDatenDto,
                BeregnungsDaten>(resourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<BeregnungsDatenMitBetriebDto>(resourceParameters.Fields))
            {
                return BadRequest();
            }

            //Aus DB laden
            var datenFromRepo = _betriebsRepository.GetBeregnungsDatens(betriebID, resourceParameters);
            var daten = Mapper.Map<IEnumerable<BeregnungsDatenMitBetriebDto>>(datenFromRepo);


            //Falls der Header "application/vnd.ostfalia.hateoas+json" werden keine Links im Header mit angegeben. 
            if (mediaType == "application/vnd.ostfalia.hateoas+json")
            {
                //Metadaten erstellen
                var paginationMetadata = new
                {
                    totalCount = datenFromRepo.TotalCount,
                    pageSize = datenFromRepo.PageSize,
                    currentPage = datenFromRepo.CurrentPage,
                    totalPage = datenFromRepo.TotalPages,
                };
                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                //Links für alle Daten
                var links = CreateLinksForBeregnungsDatens(resourceParameters,
                    datenFromRepo.HasNext,
                    datenFromRepo.HasPrevious);

                //DataShape
                var shapedDatens = daten.ShapeData(resourceParameters.Fields);

                //Links für jede einzelnen Datensatz
                var shapedDatensWithLinks = shapedDatens.Select(d =>
                {
                    var datenAsDictionary = d as IDictionary<string, object>;
                    var datenLinks = CreateLinksForBeregnungsDaten((Guid)datenAsDictionary["ID"], resourceParameters.Fields);
                    datenAsDictionary.Add("links", datenLinks);
                    return datenAsDictionary;

                });
                var linkedCollectionResource = new
                {
                    value = shapedDatensWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                //erstellen der Links für Seiten
                var previousPageLink = datenFromRepo.HasPrevious ?
                    CreateBergenungsDatenResourceUri(resourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = datenFromRepo.HasNext ?
                    CreateBergenungsDatenResourceUri(resourceParameters,
                    ResourceUriType.NextPage) : null;

                {
                    //Metadaten erstellen
                    var paginationMetadata = new
                    {
                        totalCount = datenFromRepo.TotalCount,
                        pageSize = datenFromRepo.PageSize,
                        currentPage = datenFromRepo.CurrentPage,
                        totalPage = datenFromRepo.TotalPages,
                        previousPageLink = previousPageLink,
                        nextPageLink = nextPageLink
                    };
                    Response.Headers.Add("X-Pagination",
                        Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                    return Ok(daten.ShapeData(resourceParameters.Fields));
                }
            }
        }

        /// <summary>
        /// CreateBergenungsDatenResourceUri
        /// </summary>
        /// <param name="ressourceParameters">Übergabe der RessourceParameter</param>
        /// <param name="type">Übergabe der ResourceUriType </param>
        /// <returns>string</returns>
        private string CreateBergenungsDatenResourceUri(BeregnungsDatenResourceParameter resourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetBergenungsDatens",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            istAbgeschloss = resourceParameters.IstAbgeschlossen,
                            schlagid = resourceParameters.SchlagId,
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetBergenungsDatens",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            istAbgeschloss = resourceParameters.IstAbgeschlossen,
                            schlagid = resourceParameters.SchlagId,
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetBergenungsDatens",
                        new
                        {
                            fields = resourceParameters.Fields,
                            orderBy = resourceParameters.OrderBy,
                            istAbgeschloss = resourceParameters.IstAbgeschlossen,
                            schlagid = resourceParameters.SchlagId,
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize
                        });
            }
        }

        /// <summary>
        /// Laden einer bestimmten BeregnungsDaten.
        /// </summary>
        /// <param name="id">ID des gesuchten BeregnungsDaten.</param>
        /// <returns>OK Code </returns>
        /// <response code="200">Returns einer bestimmten Beregnungs Daten</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}", Name = "GetBergenungsDaten")]
        public ActionResult<BeregnungsDaten> GetBeregnungsDaten(Guid betriebID, 
            Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<BeregnungsDatenDto>(fields))
            {
                return BadRequest();
            }
            var datenFromRepo = _betriebsRepository.GetBeregnungsDaten(betriebID, id);
            if (datenFromRepo == null)
            {
                return NotFound();
            }
            var daten = Mapper.Map<BeregnungsDatenDto>(datenFromRepo);

            var links = CreateLinksForBeregnungsDaten(id, fields);

            var linkedResourceToReturn = daten.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Hinzufügen einer neuen BeregnungsDaten
        /// </summary>
        /// <param name="beregnungsDaten"> Beregnungsdaten Body</param>
        /// <returns>CreateAtRoute</returns>
        ///  <remarks>
        /// Beispiel request (Erstellt eine neuen Beregnungsdatensatz)
        /// POST /beregnungsdaten/id
        /// {
        ///    "startDatum": "2019-05-21T00:00:00+02:00",
        ///    "startUhrzeit": "2019-06-04T00:00:00",
        ///    "endDatum": "2019-05-23T00:00:00+02:00",
        ///    "betrieb": "99fcb56d-8eb4-4cac-a208-10d179f35a97",
        ///    "schlagID": "99fcb56d-8eb4-4cac-a208-10d179f35a97",
        ///    "duese": "Düsenmaster 3000",
        ///    "wasseruhrAnfang": 2000,
        ///    "wasseruhrEnde": 3000,
        ///    "vorkomnisse": "Alle Tod",
        ///    "istAbgeschlossen": true,
        /// }        
        /// </remarks>
        /// <response code="201">Returns Beregnungs Daten erstellt</response>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost(Name = "CreateBeregnungsDaten")]
        public ActionResult<BeregnungsDaten> CreateBeregnungsDaten(Guid betriebID, [FromBody]BeregnungsDatenForCreationDto beregnungsDaten)
        {
            //Überprüfung ob der Übergabeparameter leer ist
            if (beregnungsDaten == null)
            {
                return BadRequest();
            }

            //Validierung
            if (beregnungsDaten.WasseruhrAnfang == beregnungsDaten.WasseruhrEnde)
            {

                ModelState.AddModelError(nameof(BeregnungsDatenForCreationDto), "Wasseruhrständen dürfen nicht gleich sein");
            }
            if (beregnungsDaten.IstAbgeschlossen && beregnungsDaten.WasseruhrAnfang >= beregnungsDaten.WasseruhrEnde)
            {
                ModelState.AddModelError(nameof(BeregnungsDatenForCreationDto), "Wasseruhrstand am Anfang darf nicht größer sein als am Ende");
            }
            if (beregnungsDaten.Duese == string.Empty)
            {
                ModelState.AddModelError(nameof(BeregnungsDatenForCreationDto), "Bitte eine Düse angeben");
            }

            if (!ModelState.IsValid)
            {
                //return 422
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            var datenEntitiy = Mapper.Map<BeregnungsDaten>(beregnungsDaten);
            _betriebsRepository.AddBeregnungsDaten(betriebID, datenEntitiy);

            if (!_betriebsRepository.Save())
            {
                throw new Exception("Fehler beim speichern einer neuen Beregnungsdaten");
            }

            var datenToReturn = Mapper.Map<BeregnungsDatenDto>(datenEntitiy);

            var links = CreateLinksForBeregnungsDaten(datenToReturn.ID, null);

            var linkedResourceToReturn = datenToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            _ilogger.LogInformation($"CreateBeregnungsDaten erfolgreich ID{datenToReturn.ID}.");

            return CreatedAtRoute("GetBergenungsDaten",
                new { id = linkedResourceToReturn["ID"] }, linkedResourceToReturn);

        }

        /// <summary>
        /// Löschen einer BeregnungsDaten
        /// </summary>
        /// <param name="id">ID des zu löschen Datensatz</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteBeregnungsDaten")]
        public ActionResult<BeregnungsDaten> DeleteBeregnungsDaten(Guid betriebID, Guid id)
        {
            //Exisitert BeregnungsDaten?
            if (!_betriebsRepository.BeregnungsDatenExists(id))
            {
                return NotFound();
            }

            var beregnungsDatenFromRepo = _betriebsRepository.GetBeregnungsDaten(betriebID, id);
            if (beregnungsDatenFromRepo == null)
            {
                return NotFound();
            }

            _betriebsRepository.DeleteBeregnungsDaten(beregnungsDatenFromRepo);

            if (!_betriebsRepository.Save())
            {
                throw new Exception($"Löschen der BeregnungsDaten mit der ID: {id} schlug fehl");
            }
            _ilogger.LogInformation($"BergenungsDaten {beregnungsDatenFromRepo} erfolgreich gelöscht");

            return NoContent();

        }

        /// <summary>
        /// Update von BeregnungsDaten
        /// </summary>
        /// <param name="id">ID des zu Updatenden Datensatz</param>
        /// <param name="beregnungsDaten">Neue Daten</param>
        /// <returns>NoContent</returns>
        ///  <remarks>
        /// Beispiel request (Ändern eines Beregnungsdatensatz)
        /// PUT /beregnungsdaten/id
        /// {
        ///    "id": "63c2d821-cfe7-4d75-9cfe-33d3df9d224a",
        ///    "startDatum": "2019-05-21T00:00:00+02:00",
        ///    "startUhrzeit": "2019-06-04T00:00:00",
        ///    "endDatum": "2019-05-23T00:00:00+02:00",
        ///    "betrieb": "99fcb56d-8eb4-4cac-a208-10d179f35a97",
        ///    "schlagID": "99fcb56d-8eb4-4cac-a208-10d179f35a97",
        ///    "duese": "Düsenmaster 3000",
        ///    "wasseruhrAnfang": 2000,
        ///    "wasseruhrEnde": 3000,
        ///    "vorkomnisse": "Alle Tod",
        ///    "istAbgeschlossen": true,
        /// }        
        /// </remarks>
        /// <response code="201">Returns Beregnungs Daten erstellt</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "UpdateBeregnungsDaten")]
        public ActionResult<BeregnungsDaten> UpdateBeregnungsDaten(Guid betriebID, Guid id, [FromBody]BeregnungsDatenForUpdateDto beregnungsDaten)
        {
            //Geänderte Daten
            if (beregnungsDaten == null)
            {
                return BadRequest();
            }
            //Existiert Daten?
            if (!_betriebsRepository.BeregnungsDatenExists(id))
            {
                //Falls nicht wird neu erstellt
                var beregnungsDatenEntity = Mapper.Map<BeregnungsDaten>(beregnungsDaten);
                beregnungsDatenEntity.ID = id;

                _betriebsRepository.AddBeregnungsDaten(betriebID, beregnungsDatenEntity);

                if (!_betriebsRepository.Save())
                {
                    throw new Exception($"Upserting schlug fehl");
                }

                var beregnungsDatenToReturn = Mapper.Map<BeregnungsDatenDto>(beregnungsDatenEntity);

                return CreatedAtRoute("GetBergenungsDaten",
                    new { guid = beregnungsDatenToReturn.ID },
                    beregnungsDatenToReturn);
            }
            var beregnungsDatenFromRepo = _betriebsRepository.GetBeregnungsDaten(betriebID, id);

            Mapper.Map(beregnungsDaten, beregnungsDatenFromRepo);

            _betriebsRepository.UpdateBeregnungsDaten(beregnungsDatenFromRepo);

            if (!_betriebsRepository.Save())
            {
                throw new Exception($"Fehler beim Speichern ");
            }

            _ilogger.LogInformation($"Update BeregnungsDaten mit ID: {beregnungsDatenFromRepo.ID} erfolgreich.");

            return NoContent();

        }

        /// <summary>
        /// Patch Update von BeregnungsDaten
        /// </summary>
        /// <param name="id">Id des zu Updatenen</param>
        /// <param name="patchDoc">Update Body</param>
        /// <returns>NoContent</returns>
        /// <remarks>
        /// Beispiel request (Ändern eines Beregnungsdatensatz)
        /// PATCH /beregnungsdaten/id
        /// [
        /// 	{
        /// 		"op" : "replace",
        /// 		"path":"/duese",
        /// 		"value" : "Düsenmaster 5000"
        /// 	}
        /// ]
        /// </remarks>
        /// <response code="201">Returns Beregnungs Daten erstellt</response>
        #region HTTP
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{id}", Name = "PartallyUpdateBeregnungsDaten")]
        #endregion
        public ActionResult<BeregnungsDaten> PartallyUpdateBeregnungsDaten(Guid betriebID, Guid id,
                [FromBody]JsonPatchDocument<BeregnungsDatenForUpdateDto> patchDoc)
        {
            //Eingabe nicht null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            //Datenexisieren?
            if (!_betriebsRepository.BeregnungsDatenExists(id))
            {
                return NotFound();
            }

            var beregenungsDatenFromRepo = _betriebsRepository.GetBeregnungsDaten(betriebID, id);

            if (beregenungsDatenFromRepo == null)
            {
                var beregnungsDatenDto = new BeregnungsDatenForUpdateDto();
                patchDoc.ApplyTo(beregnungsDatenDto);

                TryValidateModel(beregnungsDatenDto);

                if (!ModelState.IsValid)
                {
                    return new Helpers.UnprocessableEntityObjectResult(ModelState);
                }

                var beregnungsDatenToAdd = Mapper.Map<BeregnungsDaten>(beregnungsDatenDto);
                beregnungsDatenToAdd.ID = id;

                _betriebsRepository.AddBeregnungsDaten(betriebID, beregnungsDatenToAdd);

                if (!_betriebsRepository.Save())
                {
                    throw new Exception($"Fehler beim Speichern ");
                }

                var beregnungsDatenToReturn = Mapper.Map<BeregnungsDatenDto>(beregnungsDatenToAdd);

                return CreatedAtRoute("GetBergenungsDaten",
                    new { guid = beregnungsDatenToReturn.ID },
                    beregnungsDatenToReturn);
            }

            var beregnunsDatenToPatch = Mapper.Map<BeregnungsDatenForUpdateDto>(beregenungsDatenFromRepo);

            patchDoc.ApplyTo(beregnunsDatenToPatch, ModelState);

            TryValidateModel(beregnunsDatenToPatch);

            if (!ModelState.IsValid)
            {
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(beregnunsDatenToPatch, beregenungsDatenFromRepo);
            _betriebsRepository.UpdateBeregnungsDaten(beregenungsDatenFromRepo);
            if (!_betriebsRepository.Save())
            {
                throw new Exception($"Fehler beim Speichern ");
            }
            _ilogger.LogInformation($"Patch BeregnungsDaten mit ID: {beregenungsDatenFromRepo.ID} erfolgreich.");

            return NoContent();
        }

        /// <summary>
        /// CreateLinksForBeregnungsDaten
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="fields">Fields</param>
        /// <returns>IEnumerable<LinkDto> links</returns>
        private IEnumerable<LinkDto> CreateLinksForBeregnungsDaten(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetBergenungsDaten", new { id = id }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetBergenungsDaten", new { id = id, fields = fields }),
                    "self",
                    "GET"));
            }
            links.Add(
                    new LinkDto(_urlHelper.Link("DeleteBeregnungsDaten", new { id = id }),
                    "delete_beregungsdaten",
                    "DELETE"));
            links.Add(
                    new LinkDto(_urlHelper.Link("UpdateBeregnungsDaten", new { id = id }),
                    "update_beregnungsdaten",
                    "PUT"));
            links.Add(
                    new LinkDto(_urlHelper.Link("PartallyUpdateBeregnungsDaten", new { id = id }),
                    "partally_update_beregnungsdaten",
                    "PATCH"));

            return links;
        }

        /// <summary>
        /// CreateLinksForBeregnungsDatens
        /// </summary>
        /// <param name="beregnungsDatenResourceParameter">BeregnungsDatenResourceParameter</param>
        /// <param name="hasNext">Hat nächste Seite Bool</param>
        /// <param name="hasPrevious">Hat vorherige Seite Bool</param>
        /// <returns>Links</returns>
        private IEnumerable<LinkDto> CreateLinksForBeregnungsDatens(BeregnungsDatenResourceParameter resourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateBergenungsDatenResourceUri(resourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateBergenungsDatenResourceUri(resourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateBergenungsDatenResourceUri(resourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }
            return links;
        }

        /// <summary>
        /// GetBeregnungsDatenOptions
        /// </summary>
        /// <returns>OK</returns>
        [HttpOptions]
        public IActionResult GetBeregnungsDatenOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH,PUT,DELETE");
            return Ok();
        }
    }
}
