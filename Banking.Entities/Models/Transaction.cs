using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Entities.Models
{
    public class Transaction:IEntity
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }=DateTime.Now;    

        public double? Amount { get; set; }
        public string? Message { get; set; }
        public string? Status {  get; set; } //cash or credit
        public int? ReceiverAccountId { get; set; }    
        public int? SenderAccountId { get; set; }    
        public virtual Account? ReceiverAccount { get; set; }
        public virtual Account? SenderAccount { get; set; }
      
    }
}
