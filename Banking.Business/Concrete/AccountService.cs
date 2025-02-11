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
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task DeleteAccount(int accountId)
        {
            var account=await _accountRepository.GetById(u=>u.Id == accountId);
            await _accountRepository.Delete(account);
        }

        public async Task<Account> GetAccountByUserId(string userId)
        {
           return await _accountRepository.GetById(u => u.UserId == userId);
        }
        public async Task<Account> GetAccountById(int accountId)
        {
            return await _accountRepository.GetById(u => u.Id == accountId);
        }

        public async Task<List<Account>> GetAccounts(string userId)
        {
           return await _accountRepository.GetAll(a=>a.UserId==userId);
        }

        public async Task UpdateAccount(Account account)
        {
          await _accountRepository.Update(account); 
        }

        public async Task<bool> CheckAccountNumber(string accountNumber)
        {
           var account=await _accountRepository.GetById(n=>n.AccountNumber==accountNumber);
            if (account == null)
                return true;
            return false;

        }

        public async Task AddAccount(Account account)
        {
          await _accountRepository.Add(account);
        }

        public async Task<string> GetUserIdByAccountNumber(string accountNumber)
        {
            var account = await _accountRepository.GetById(n => n.AccountNumber == accountNumber);
            return account.UserId;
        }
         
        public async Task<bool> ChangeBalance(int senderAccountId, int receiverAccountId, double balance)
        {
            var senderAccount = await _accountRepository.GetById(n => n.Id == senderAccountId);
            var receiverAccount = await _accountRepository.GetById(n => n.Id == receiverAccountId);
            if (senderAccount.Balance < balance) return false;
            senderAccount.Balance-=balance;
            receiverAccount.Balance+=balance;
            await _accountRepository.Update(senderAccount);
            await _accountRepository.Update(receiverAccount);
            return true;    
        }

        public async Task<int> GetAccountIdByNumber(string accountNumber)
        {
            var account = await _accountRepository.GetById(n => n.AccountNumber == accountNumber);
            return account.Id;
        }
    }
}
