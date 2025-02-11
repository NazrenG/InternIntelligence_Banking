using Banking.Core.DataAccess;
using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.DataAccess.Abstract
{
    public interface IUserSettingRepository : IEntityBaseRepository<UserSetting>
    {
    }
}
