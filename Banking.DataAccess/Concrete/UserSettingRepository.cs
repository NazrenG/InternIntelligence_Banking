using Banking.Core.DataAccess.EntityFramework;
using Banking.DataAccess.Abstract;
using Banking.Entities.Data;
using Banking.Entities.Models;

namespace Banking.DataAccess.Concrete
{
    public class UserSettingRepository : EFEntityBaseRepository<BankingDbContext, UserSetting>, IUserSettingRepository
    {
        public UserSettingRepository(BankingDbContext context) : base(context)
        {
        }
    }
}
