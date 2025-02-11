namespace Banking.WebApi.Dtos
{
    public class TransactionDto
    {
        public string ReceiverAccountNumber { get; set; }
        public string SenderAccountNumber { get; set; }
    //    public DateTime? Created { get; set; }= DateTime.Now;   

        public double Amount { get; set; }
        //public string? Message { get; set; }
        public string? Status { get; set; } //cash or credit

    }
}
