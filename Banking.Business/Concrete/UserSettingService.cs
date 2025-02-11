using Banking.Business.Abstract;
using Banking.DataAccess.Abstract;
using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Business.Concrete
{
    public class UserSettingService : IUserSettingService
    {
        private readonly IUserSettingRepository userSettingRepository;

        public UserSettingService(IUserSettingRepository userSettingRepository)
        {
            this.userSettingRepository = userSettingRepository;
        }

        public async Task Add(UserSetting setting)
        {
            await userSettingRepository.Add(setting);
        }

        public async Task<UserSetting> GetUserSetting(string userId)
        {
           return await userSettingRepository.GetById(u=>u.UserId == userId);
        }

        public async Task Update(UserSetting setting)
        {
           await userSettingRepository.Update(setting);
        }
    }
}
