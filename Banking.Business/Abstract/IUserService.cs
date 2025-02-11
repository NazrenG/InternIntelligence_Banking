using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Business.Abstract
{
    public interface IUserService
    {
        Task<User> GetById(string id);
        Task<string> GetLastUserId();
    }
}
