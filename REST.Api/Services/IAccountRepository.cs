using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;

namespace REST.Api.Services
{
    public interface IAccountRepository
    {
        PagedList<Account> Account(AccountResourceParameter resourceParameter);
        Account GetAccount(Account account);
        IEnumerable<Account> GetAccounts(IEnumerable<Guid> guids);
        void AddAccount(Account account);
        void DeleteAccount(Account account);
        void UpdateAccount(Account account);
        bool AccountExists(Guid guid);
        bool Save();
    }
}
