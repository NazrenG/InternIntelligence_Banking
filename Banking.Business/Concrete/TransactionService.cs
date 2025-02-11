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

        public async Task<List<Transaction>> GetAllTransactions(int accountId)
        {
            return await _transactionRepository.GetAll(t => t.SenderAccountId == accountId || t.ReceiverAccountId == accountId);
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
