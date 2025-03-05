namespace API_Payments.DTO
{
    public class CardBalanceDTO
    {
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal Limit { get; set; }
        public decimal Total { get; set; }
    }
}
