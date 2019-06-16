using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
    [Route("api")]
    public class RootController : Controller
    {
        private IUrlHelper _urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType == "application/vnd.ostfalia.hateoas+json")
            {
                var links = new List<LinkDto>();

                links.Add(
                    new LinkDto(_urlHelper.Link("GetRoot", new { }),
                    "self",
                    "GET"));

                links.Add(
                    new LinkDto(_urlHelper.Link("GetBergenungsDatens", new { }),
                    "beregnungsDaten",
                    "GET"));

                links.Add(
                    new LinkDto(_urlHelper.Link("CreateBeregnungsDaten", new { }),
                    "create_beregnungsDaten",
                    "POST"));

                links.Add(
                    new LinkDto(_urlHelper.Link("GetBetriebe", new { }),
                    "betriebe",
                    "GET"));
                
                links.Add(
                    new LinkDto(_urlHelper.Link("CreateBetrieb", new { }),
                    "create_betriebe",
                    "Post"));

                links.Add(
                    new LinkDto(_urlHelper.Link("GetSchlaege", new { }),
                    "schlaege",
                    "GET"));

                links.Add(
                    new LinkDto(_urlHelper.Link("CreatSchlag", new { }),
                    "create_schlaege",
                    "Post"));


                return Ok(links);
            }

            return NoContent();
        }

    }
}
