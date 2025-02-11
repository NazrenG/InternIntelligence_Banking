using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Entities.Models
{
    public class Account:IEntity
    {
        public int Id { get; set; }
        public string? AccountNumber { get; set; }
        public double Balance { get; set; }
        public double Limit { get; set; }
        public string? UserId { get; set; }

        public virtual User? User { get; set; }


        public virtual List<Transaction>? Receivers { get; set; }
        public virtual List<Transaction>? Senders { get; set; }

    }
}
