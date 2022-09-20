using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class BankAccount
    {
        [Key] public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [ForeignKey("CurrencyId")]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }



        public decimal Balance { get; set; }

        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; } = null;

    }
}
