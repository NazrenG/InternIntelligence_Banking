using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Entities.Models
{
    public class UserSetting:IEntity
    {
        public int Id { get; set; } 
        public bool TwoFactorEnabled { get; set; }  
        public bool EmailNotificationsEnabled { get; set; } 
        public bool AppNotificationsEnabled { get; set; }

        //if balance low when transfer time
        public bool LowBalanceAlertEnabled { get; set; } 
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
