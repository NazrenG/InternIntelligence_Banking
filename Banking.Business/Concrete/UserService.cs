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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetById(string id)
        {
           return await _userRepository.GetById(u=>u.Id == id);
        }

        public async Task<string> GetLastUserId()
        {
            var list = await _userRepository.GetAll();

            return list.LastOrDefault().Id;
        }
    }
}
