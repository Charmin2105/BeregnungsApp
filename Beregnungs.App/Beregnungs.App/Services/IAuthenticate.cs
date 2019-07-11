using Beregnungs.App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Beregnungs.App.Services
{
    public interface IAuthenticate
    {
         Task<bool> Login(Account account);
    }
}
