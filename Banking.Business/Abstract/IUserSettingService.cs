using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Business.Abstract
{
    public interface IUserSettingService
    {
        Task<UserSetting> GetUserSetting(string userId);
        Task Add(UserSetting setting);  
        Task Update(UserSetting setting);   
    }
}
