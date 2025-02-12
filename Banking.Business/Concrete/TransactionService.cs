using Banking.Business.Abstract;
using Banking.DataAccess.Abstract;
using Banking.Entities.Models;

namespace Banking.Business.Concrete
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await _transactionRepository.Add(transaction);  
        }

        public async Task<List<string>> GetAllTransactions(List<int> accountIds)
        {
             var list = await _transactionRepository.GetAll(t =>
                accountIds.Contains(t.SenderAccountId) ||
                accountIds.Contains(t.ReceiverAccountId)
            );

             return list.Select(t => t.Message).ToList();
        }


        public async Task<Transaction> GetById(int id)
        {
          return  await _transactionRepository.GetById(t => t.Id == id);
        }

        public async Task UpdateTransaction(Transaction transaction)
        { 
            await _transactionRepository.Update(transaction);
        }
    }
}
