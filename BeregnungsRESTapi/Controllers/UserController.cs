using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeregnungsRESTapi.Controllers;

namespace BeregnungsRESTapi.Controllers
{
    [Route("api/account")]
    public class UserController : Controller
    {
        private IUserRepsository _repsository;

        //Konstruktor
        public UserController(IUserRepsository repsository)
        {
            _repsository = repsository;
        }

        //[HttpGet()]
        //public IActionResult GetAccount()
        //{
           
        //}

    }
}