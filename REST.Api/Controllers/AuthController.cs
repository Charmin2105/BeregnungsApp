using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using REST.Api.Models;
using REST.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using REST.Api.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using REST.Api.Helpers;

namespace REST.Api.Controllers
{
    [Route("api/account")]
    public class AuthController : Controller
    {
        private IAccountRepository _accountRepository;
        private ILogger<AuthController> _ilogger;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        private IConfiguration Configuration;

        public AuthController(IAccountRepository accountRepository,
            ILogger<AuthController> ilogger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _ilogger = ilogger;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            Configuration = configuration;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks> 
        /// POST
        /// {    
        ///     "benutzername": "string",
        ///     "passwort": "string"
        /// }
        /// </remarks>
        /// <param name="accountLogin"></param>
        /// <returns>Token</returns>
        /// <response code="200">Returns Token</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("login")]
        public IActionResult CreateToken([FromBody] AccountDto accountLogin)
        {
            //Überprüfung ob der Übergabeparameter leer ist
            if (accountLogin == null)
            {
                return BadRequest();
            }

            //Passwort verschlüsseln
            using (var sha = new SHA512CryptoServiceProvider())
            {
                byte[] hashValue = sha.ComputeHash(Encoding.Default.GetBytes(accountLogin.Passwort));
                accountLogin.Passwort = Convert.ToBase64String(hashValue);
            };

            //Account suchen 

            var accountEntity = Mapper.Map<Account>(accountLogin);

            var account = _accountRepository.GetAccount(accountEntity);

            if (account == null)
            {
                return NotFound();
            }

            //Token generieren 
            var jwt = JwtTokenBuilder(account.IstAdmin);

            return Ok(new { token = jwt });

        }

        /// <summary>
        /// Hinzufügen eines Accounts.
        /// </summary>
        /// <param name="account">Neuer Account.</param>
        /// <returns>CreatedAtRoute </returns>
        /// <remarks>
        /// Beispiel request (erstellen eines neuen Schlags)
        /// POST
        /// {    
        ///     "benutzername": "Admin",
        ///     "eMail": "string",
        ///     "passwort": "nico123",
        ///     "istAdmin": true
        /// }
        /// </remarks>
        /// <response code="201">Returns Account erstellt</response>
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("CreatAccount")]
        public ActionResult<Account> CreatAccount([FromBody]AccountForCreationDto account)
        {
            //Überprüfung ob der Übergabeparameter leer ist
            // <returns>BadRequest </returns>
            if (account == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                //return 422
                return new Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            //Passwort verschlüsseln
            using (var sha = new SHA512CryptoServiceProvider())
            {
                byte[] hashValue = sha.ComputeHash(Encoding.Default.GetBytes(account.Passwort));
                account.Passwort = Convert.ToBase64String(hashValue);
            };

            var accountEntity = Mapper.Map<Account>(account);


            _accountRepository.AddAccount(accountEntity);

            if (!_accountRepository.Save())
            {
                throw new Exception("Fehler beim Speichern eines neuen Accounts");
            }
            var accountToReturn = Mapper.Map<AccountDto>(accountEntity);

            _ilogger.LogInformation($"CreateAccount erfolgreich ID: {accountToReturn.ID} .");

            var links = CreateLinksForAccount(accountToReturn.ID, null);

            var linkedResourceToReturn = accountToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("CreateAccount",
                new { id = linkedResourceToReturn["ID"] }, linkedResourceToReturn);

        }

        /// <summary>
        /// Methode zum erstellen des Tokens
        /// </summary>
        /// <param name="istAdmin"> User ist Admin? </param>
        /// <returns>Token</returns>
        private string JwtTokenBuilder(bool istAdmin)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();

            if (istAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            claims.Add(new Claim(ClaimTypes.Role, "Benutzer"));

            var jwtToken = new JwtSecurityToken(
                issuer: Configuration["JWT:Issuser"],
                audience: Configuration["JWT:Audience"],
                signingCredentials: credentials,
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(Configuration["JWT:Lifetime"]))
                );


            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        /// <summary>
        /// CreateLinksForAccount
        /// </summary>
        /// <param name="account">Übergabe eines SchlagDto</param>
        /// <returns>AccountDto mit Links </returns>
        private IEnumerable<LinkDto> CreateLinksForAccount(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetAccount", new { id = id }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetAccount", new { id = id, fields = fields }),
                    "self",
                    "GET"));
            }

            links.Add(new LinkDto(_urlHelper.Link("DeleteAccount",
               new { id = id }),
               "delete_account",
               "DELETE"));

            links.Add(new LinkDto(_urlHelper.Link("UpdateAccount",
                new { id = id }),
                "update_account",
                "PUT"));

            links.Add(new LinkDto(_urlHelper.Link("PartiallyUpdateAccount",
               new { id = id }),
               "partially_update_account",
               "PATCH"));


            return links;
        }
    }
}