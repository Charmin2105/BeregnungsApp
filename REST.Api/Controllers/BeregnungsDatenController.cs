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
    [Route("api/beregnungsdaten")]
    public class BeregnungsDatenController : Controller
    {
        //Fields
        private IBeregnungsRepository _beregnungsRepository;
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
        public BeregnungsDatenController(IBeregnungsRepository beregnungsRepository,
            ILogger<BeregnungsDatenController> ilogger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _ilogger = ilogger;
            _beregnungsRepository = beregnungsRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        /// <summary>
        /// Laden aller BeregnungsDaten
        /// </summary>
        /// <param name="resourceParameters">Metadateneinstellungen</param>
        /// <returns>IEnumerable von BeregnungsDaten</returns>
        [HttpGet(Name = "GetBergenungsDatens")]
        public IActionResult GetBeregnungsDatens(BeregnungsDatenResourceParameter resourceParameters)
        {
            //Mapping für OrderBy ist valid
            if (!_propertyMappingService.ValidMappingExistsFor<BeregnungsDatenDto,
                BeregnungsDaten>(resourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<BeregnungsDatenDto>(resourceParameters.Fields))
            {
                return BadRequest();
            }

            var datenFromRepo = _beregnungsRepository.GetBeregnungsDatens(resourceParameters);

            ////erstellen der Links für Seiten
            //var previousPageLink = datenFromRepo.HasPrevious ?
            //    CreateBergenungsDatenResourceUri(resourceParameters,
            //    ResourceUriType.PreviousPage) : null;

            //var nextPageLink = datenFromRepo.HasNext ?
            //    CreateBergenungsDatenResourceUri(resourceParameters,
            //    ResourceUriType.NextPage) : null;

            //Metadaten erstellen
            var paginationMetadata = new
            {
                totalCount = datenFromRepo.TotalCount,
                pageSize = datenFromRepo.PageSize,
                currentPage = datenFromRepo.CurrentPage,
                totalPage = datenFromRepo.TotalPages,
                //previousPageLink = previousPageLink,
                //nextPageLink = nextPageLink
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var daten = Mapper.Map<IEnumerable<BeregnungsDatenDto>>(datenFromRepo);

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
        [HttpGet("{id}", Name = "GetBergenungsDaten")]
        public IActionResult GetBeregnungsDaten(Guid id,[FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<BeregnungsDatenDto>(fields))
            {
                return BadRequest();
            }
            var datenFromRepo = _beregnungsRepository.GetBeregnungsDaten(id);
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
        [HttpPost(Name = "CreateBeregnungsDaten")]
        public IActionResult CreateBeregnungsDaten([FromBody]BeregnungsDatenForCreationDto beregnungsDaten)
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
            if (beregnungsDaten.Betrieb == null)
            {
                ModelState.AddModelError(nameof(BeregnungsDatenForCreationDto), "Betrieb darf nicht leer sein");
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
            _beregnungsRepository.AddBeregnungsDaten(datenEntitiy);

            if (!_beregnungsRepository.Save())
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
        [HttpDelete("{id}", Name ="DeleteBeregnungsDaten")]
        public IActionResult DeleteBeregnungsDaten(Guid id)
        {
            //Exisitert BeregnungsDaten?
            if (!_beregnungsRepository.BeregnungsDatenExists(id))
            {
                return NotFound();
            }

            var beregnungsDatenFromRepo = _beregnungsRepository.GetBeregnungsDaten(id);
            if (beregnungsDatenFromRepo == null)
            {
                return NotFound();
            }

            _beregnungsRepository.DeleteBeregnungsDaten(beregnungsDatenFromRepo);

            if (!_beregnungsRepository.Save())
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
        [HttpPut("{id}",Name = "UpdateBeregnungsDaten")]
        public IActionResult UpdateBeregnungsDaten(Guid id, [FromBody]BeregnungsDatenForUpdateDto beregnungsDaten)
        {
            //Geänderte Daten
            if (beregnungsDaten == null)
            {
                return BadRequest();
            }
            //Existiert Daten?
            if (!_beregnungsRepository.BeregnungsDatenExists(id))
            {
                //Falls nicht wird neu erstellt
                var beregnungsDatenEntity = Mapper.Map<BeregnungsDaten>(beregnungsDaten);
                beregnungsDatenEntity.ID = id;

                _beregnungsRepository.AddBeregnungsDaten(beregnungsDatenEntity);

                if (!_beregnungsRepository.Save())
                {
                    throw new Exception($"Upserting schlug fehl");
                }

                var beregnungsDatenToReturn = Mapper.Map<BeregnungsDatenDto>(beregnungsDatenEntity);

                return CreatedAtRoute("GetBergenungsDaten",
                    new { guid = beregnungsDatenToReturn.ID },
                    beregnungsDatenToReturn);
            }
            var beregnungsDatenFromRepo = _beregnungsRepository.GetBeregnungsDaten(id);

            Mapper.Map(beregnungsDaten, beregnungsDatenFromRepo);

            _beregnungsRepository.UpdateBeregnungsDaten(beregnungsDatenFromRepo);

            if (!_beregnungsRepository.Save())
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
        /// <returns></returns>
        [HttpPatch("{id}",Name = "PartallyUpdateBeregnungsDaten")]
        public IActionResult PartallyUpdateBeregnungsDaten(Guid id,
            [FromBody]JsonPatchDocument<BeregnungsDatenForUpdateDto> patchDoc)
        {
            //Eingabe nicht null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            //Datenexisieren?
            if (!_beregnungsRepository.BeregnungsDatenExists(id))
            {
                return NotFound();
            }

            var beregenungsDatenFromRepo = _beregnungsRepository.GetBeregnungsDaten(id);

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

                _beregnungsRepository.AddBeregnungsDaten(beregnungsDatenToAdd);

                if (!_beregnungsRepository.Save())
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
            _beregnungsRepository.UpdateBeregnungsDaten(beregenungsDatenFromRepo);
            if (!_beregnungsRepository.Save())
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
        public IEnumerable<LinkDto> CreateLinksForBeregnungsDaten(Guid id, string fields)
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
    }
}
