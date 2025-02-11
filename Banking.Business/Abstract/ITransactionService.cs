using Banking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Business.Abstract
{
    public interface ITransactionService
    {
        Task<Transaction> GetById(int id);  
        Task UpdateTransaction(Transaction transaction);
        Task AddTransaction(Transaction transaction);
        Task<List<Transaction>> GetAllTransactions(int accountId);
    }
}
