namespace AccountManagement.Data.DTO
{
    public class BankAccountCreateUpdateDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }

        public decimal Balance { get; set; }

        public int ClientId { get; set; }
    }
}
