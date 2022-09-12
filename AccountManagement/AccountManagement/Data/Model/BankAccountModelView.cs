using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Data.Model
{
    public class BankAccountModelView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }

        public decimal Balance { get; set; }

        public int ClientId { get; set; }

        public bool IsActive { get; set; }
    }
}
