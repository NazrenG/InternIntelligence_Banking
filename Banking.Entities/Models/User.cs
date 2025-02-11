using Banking.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Entities.Models
{
    public class User:IdentityUser,IEntity
    {
        public virtual List<Account>? Accounts { get; set; }
        public virtual UserSetting? UserSetting { get; set; }
    }
}
