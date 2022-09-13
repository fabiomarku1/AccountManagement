namespace AccountManagement.Data.DTO
{
    public class BankTransactionCreateDto
    {
        public ActionCall Action { get; set; }
        public decimal Amount { get; set; }
        public int BankAccountId { get; set; }
    }
}
