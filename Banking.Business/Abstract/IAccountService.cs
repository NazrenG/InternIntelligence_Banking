using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Business.Abstract
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccounts(string userId);
        Task<Account> GetAccountByUserId(string userId);
        Task<Account> GetAccountById(int accountId);
        Task<int> GetAccountIdByNumber(string accountNumber);
        Task<string> GetUserIdByAccountNumber(string accountNumber);
        //if send money successfully return true,else return false
        Task<bool> ChangeBalance(int senderAccountId, int receiverAccountId, double balance);
        Task DeleteAccount(int accountId);
        Task UpdateAccount(Account account);
        Task AddAccount(Account account);
        Task<bool> CheckAccountNumber(string accountNumber);
    }
}
