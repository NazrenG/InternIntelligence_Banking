using Banking.Entities.Models;

namespace Banking.Business.Abstract
{
    public interface ITransactionService
    {
        Task<Transaction> GetById(int id);  
        Task UpdateTransaction(Transaction transaction);
        Task AddTransaction(Transaction transaction);
        Task<List<string>> GetAllTransactions(List<int> accountId);
    }
}
