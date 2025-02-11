using Banking.Core.DataAccess.EntityFramework;
using Banking.DataAccess.Abstract;
using Banking.Entities.Data;
using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.DataAccess.Concrete
{
    public class AccountRepository : EFEntityBaseRepository<BankingDbContext, Account>, IAccountRepository
    {
        public AccountRepository(BankingDbContext context) : base(context)
        {
        }
    }
}
