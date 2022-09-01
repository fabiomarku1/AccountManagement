using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data
{
    public class BankTransaction
    {
        [Key] public int Id { get; set; }

        [ForeignKey("BankAccountId")]
        public BankAccount RequestingBankAccount { get; set; }
        public int BankAccountId { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }= DateTime.Now;
        public DateTime? DateModified { get; set; } = null;
        public ActionCall Action { get; set; }
        
    }
    public enum ActionCall
    {
        Depositim = 1,
        Terheqje = 2,

    }


}
