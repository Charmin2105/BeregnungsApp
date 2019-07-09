using System;
using System.Collections.Generic;
using System.Linq;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    public class AccountRepository : IAccountRepository
    {

        private BeregnungsContext _context;

        public AccountRepository(BeregnungsContext context)
        {
            _context = context;
        }

        public PagedList<Account> Account(AccountResourceParameter resourceParameter)
        {
            var collectionBeforPaging = _context.Accounts.OrderBy(a => a.EMail);
            return PagedList<Account>.Create(collectionBeforPaging, resourceParameter.PageNumber, resourceParameter.PageSize);
        }

        public bool AccountExists(Guid guid)
        {
            return _context.Accounts.Any(a => a.ID == guid);
        }

        public void AddAccount(Account account)
        {
            account.ID = Guid.NewGuid();
            _context.Accounts.Add(account);
        }

        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
        }

        public Account GetAccount(Account account)
        {
            return _context.Accounts.Where(a => a.Benutzername == account.Benutzername && a.Passwort == account.Passwort).FirstOrDefault();
        }

        public IEnumerable<Account> GetAccounts(IEnumerable<Guid> guids)
        {
            return _context.Accounts.Where(a => guids.Contains(a.ID)).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAccount(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
