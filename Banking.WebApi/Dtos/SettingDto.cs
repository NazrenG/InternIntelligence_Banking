namespace Banking.WebApi.Dtos
{
    public class SettingDto
    {
        public bool TwoFactorEnabled { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public bool AppNotificationsEnabled { get; set; }

        //if balance low when transfer time
        public bool LowBalanceAlertEnabled { get; set; }
    }
}
